# Windowing — decompilation log

Append-only. Do not edit previous entries.

---

## 2026-07-26 — ForceSetForegroundWindow investigated (ILSpy + Ghidra)

**Question investigated:** does `Windowing.ForceSetForegroundWindow` queue any
work on the Iris dispatcher?

**Source 1 — ILSpy** `decompile_method` on
`Microsoft.Zune.Util.Windowing.ForceSetForegroundWindow` in
`libs/ref/ZuneDBApi.dll` (mixed-mode C++/CLI assembly). Full decompiled body:

```csharp
public unsafe static void ForceSetForegroundWindow(IntPtr hwnd)
{
    tagINPUT tagINPUT;
    Unsafe.SkipInit(out tagINPUT);
    Unsafe.InitBlock(ref Unsafe.AddByteOffset(ref tagINPUT, 8), 0, 32);
    *(int*)(&tagINPUT) = 1;                                    // INPUT.type = INPUT_KEYBOARD
    Unsafe.As<tagINPUT, short>(ref Unsafe.AddByteOffset(ref tagINPUT, 8)) = 0; // wVk = 0
    global::<Module>.SendInput(1u, &tagINPUT, 40);
    global::<Module>.SetForegroundWindow((HWND__*)hwnd.ToPointer());
}
```

`Microsoft.Zune.Util.Windowing` (`get_type_members`) has exactly one member —
this method. No fields, no other methods, nothing dispatcher-related on the
type itself.

`SendInput`/`SetForegroundWindow` appear as `global::<Module>` calls, i.e.
plain `user32.dll` P/Invokes emitted by the C++/CLI compiler, not
Zune-specific native functions — `search_members_by_name` for `SendInput`
found zero managed members (confirms it's a module-level native import, not
something reimplementable/inspectable further).

**Source 2 — Ghidra**, `ZuneDBApi.dll` (x64, already in project at
`/ZuneDBApi.dll`, backed by
`/home/yoshiask/repos/ZuneDev/windows/shared/zune-x64/Zune/ZuneDBApi.dll`).
`search_functions` found `ForceSetForegroundWindow @ 180108b00`.
`decompile_function` and `disassemble_function` both come back empty /
`"WARNING: Control flow encountered bad instruction data" ... /* .NET CLR
Managed Code */ halt_baddata()` — i.e. this function has **no native x86
body at all**; it's pure MSIL in this mixed-mode assembly (matches ILSpy:
the whole implementation is IL-level calls into the CLR's P/Invoke thunks
for `user32.dll`, there's no hand-written native codegen to recover).
`get_function_xrefs` found **no internal callers** in `ZuneDBApi.dll` — it's
a leaf utility exposed to, and only invoked from, other assemblies.

**Behavior:** this is the standard Win32 "foreground lock" bypass trick —
synthesize a throwaway keyboard `INPUT` via `SendInput` (which makes the
calling thread believe it just received input, satisfying
`SetForegroundWindow`'s foreground-lock-timeout heuristic) and then call
`SetForegroundWindow(hwnd)` immediately after. That's the entire method:
two Win32 calls, no allocations, no other side effects.

**Caller context** (`ZuneShell/Microsoft/Zune/Shell/StandAlone.cs:157-160`,
reimplementation side): the *caller* schedules the call through Iris —
`Application.DeferredInvoke(() => Windowing.ForceSetForegroundWindow(...),
DeferredInvokePriority.Low)` — but that's the dispatcher queuing this method
as a leaf action, not this method queuing anything itself.

**Conclusion:** `ForceSetForegroundWindow` does **not** queue, post, or
otherwise interact with the Iris dispatcher in any way. It is a pure,
synchronous Win32 interop call (`SendInput` + `SetForegroundWindow`) with no
managed-side state, no Iris types referenced, and no re-entrancy into any
dispatch queue. Any dispatcher involvement seen in the codebase is the
*caller's* choice (wrapping the call in `Application.DeferredInvoke`), not
something the method does internally.

No assumptions were required — both ILSpy (full IL body) and Ghidra (confirms
no additional native code path exists beyond the IL) agree.
