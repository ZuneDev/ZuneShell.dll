# SafeBitmap — decompilation log

Append-only. Do not edit previous entries.

---

## 2026-07-20 — Decompilation

**Source:** ILSpy `find_type_hierarchy` + `decompile_type` on
`MicrosoftZuneLibrary.SafeBitmap` in `ZuneShell/lib/ZuneDBApi.dll`
(mixed-mode C++/CLI assembly), query "full verbatim source, base class,
constructors, all members".

Original decompiles to:

```csharp
public class SafeBitmap : SafeHandleZeroOrMinusOneIsInvalid
{
    public unsafe SafeBitmap(HBITMAP__* hBitmap) : base(ownsHandle: true)
    {
        handle = (IntPtr)hBitmap;
    }

    protected unsafe override bool ReleaseHandle()
    {
        return global::<Module>.DeleteObject((void*)handle) != 0;
    }
}
```

**Translation decisions:**

- `HBITMAP__*` is not a real .NET type — it's ILSpy's rendering of the
  forward-declared `struct HBITMAP__ *` pattern Win32 headers use to make
  `HBITMAP` a distinct pointer type from plain `void*`. There is no managed
  equivalent; the constructor parameter was changed to plain `IntPtr hBitmap`,
  which is what every other Win32 handle wrapper in this codebase style uses
  and is exactly what the base `SafeHandle.handle` field stores anyway.
- `global::<Module>.DeleteObject` is an unresolved native import (a
  module-level P/Invoke that C++/CLI compiles directly into `<Module>` rather
  than a named class). This is GDI32's `DeleteObject` — reconstructed as an
  explicit `[DllImport("gdi32.dll")] static extern bool DeleteObject(IntPtr hObject)`,
  which is the standard managed signature for this well-documented Win32 API
  (see MSDN `DeleteObject function (wingdi.h)` — a source outside this
  project's own uncertainty, per CLAUDE.md's *Logging* guidance not to
  compound guesses).
- Dropped the `unsafe`/pointer-cast plumbing entirely since `IntPtr` already
  carries the handle without needing raw pointer arithmetic — `SafeHandle`
  subclasses conventionally work through `handle`/`IntPtr`, not `void*`.
- Left the `[DllImport]` ungated by `#if PLATFORM`: per CLAUDE.md ("Working in
  this project"), Windows-only APIs required for backward-compat parity (like
  `Microsoft.Win32.RegistryHive` in the `Configuration` classes) are expected
  and not gated — the P/Invoke declaration itself compiles fine on all
  platforms; only a runtime call on non-Windows would fail (`DllNotFoundException`),
  same as the rest of this stage-2 code today.
- Dropped the two `[SecurityPermission]` attributes from the class — these
  are CAS (Code Access Security) attributes that .NET (Core) 8 no longer
  enforces or even fully supports; keeping them would be dead decoration with
  no runtime effect.

No unknowns; `DeleteObject`'s signature is a standard, unambiguous Win32 API.
