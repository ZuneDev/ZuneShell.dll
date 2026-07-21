# IQueryPropertyBag — decompilation log

Append-only. Do not edit previous entries.

---

## 2026-07-21 — Ghidra chase for the remaining `_Reserved` slots: dead end (documented, not guessed)

**Task:** attempt to fill in `IQueryPropertyBag`'s unknown vtable slots (3, 4, 6,
8–12) now that a Ghidra MCP connection was available, continuing where the
2026-07-12 entries below left off.

**Method and findings:**

1. `ZuneDBApi.dll` (`/home/yoshiask/repos/ZuneDev/windows/shared/zune-x64/Zune/ZuneDBApi.dll`)
   was already open in Ghidra. `search_functions` found
   `ZuneLibraryExports.CreatePropertyBag @ 180111ad8`, but decompiling it showed only
   a 6-byte `JMP qword ptr [0x180001498]` — an IAT thunk. `list_imports` confirmed
   `CreatePropertyBag`'s `original_imported_name` is the mangled
   `?CreatePropertyBag@ZuneLibraryExports@@YAJPEAPEAUIQueryPropertyBag@@@Z`, i.e. it's
   a genuine **external import**, not code inside `ZuneDBApi.dll` itself. Same is true
   of `CreateMultiSortAttributes`, `CWmpPlayer_GetInstance`, and `WmpCoreInitialize`.

2. Imported and auto-analyzed `ZuneNativeLib.dll` (10 MB, 23,128 functions after
   analysis) from the same shared install directory, since it's the most likely home
   for these exports. `CreatePropertyBag` and `CreateMultiSortAttributes` **are**
   defined there (at `180309118`/`18030baa0`) — a genuine positive finding, logged
   below. `CWmpPlayer_GetInstance` is *not* in `ZuneNativeLib.dll`; its actual host
   DLL is still unidentified (see `logs/MicrosoftZunePlayback/PlayerInterop.md` for
   the `IMCPlayer` side of this same gap).

3. Decompiling `CreatePropertyBag` in `ZuneNativeLib.dll` revealed a generic
   GUID-keyed **singleton/factory activation framework** (a hand-rolled
   `ISingletonManager`-style locator, matching the `g_pSingletonManager` global
   imported by `ZuneDBApi.dll`): a static table at `0x180948110` maps a 16-byte GUID
   (via an index-lookup function) to a 0x30-byte record holding a factory function
   pointer and a lazily-created singleton instance pointer. `CreatePropertyBag` and
   `QueryDatabase`/`GetFieldValues`/`SetFieldValues` all resolve the *same* GUID to
   the *same* singleton — an internal database/library manager object, not
   `IQueryPropertyBag` itself — and dispatch to different vtable slots on it
   (`+0x40`, `+0x48`, `+0xb0`, `+0xc8` respectively). `IQueryPropertyBag` itself is
   constructed *inside* that manager's `CreatePropertyBag` vtable method, whose
   concrete implementation class could not be identified: `list_classes` on
   `ZuneNativeLib.dll` returns only DLL-import namespaces (no C++ class/RTTI
   symbols recovered — this binary appears to have no recoverable RTTI, unlike
   `ZuneShell/lib/ZuneDBApi.dll`'s managed metadata side), and `search_strings` for
   `"PropertyBag"` in `ZuneNativeLib.dll` finds only the four mangled *export* name
   strings, no RTTI type-descriptor string for any concrete implementing class.

**Conclusion (negative but verified, not a guess):** the concrete native class
implementing `IQueryPropertyBag`'s vtable — and therefore slots 3, 4, 6, 8–12 — is
not reachable through this factory chain without either (a) hooking/dynamic
tracing of a running Zune process (out of scope for static Ghidra analysis) or (b)
manually walking the singleton table's opaque factory-function pointers with no
symbol or RTTI signal to confirm correctness at any step, which would cross from
"reverse engineering" into "guessing a memory layout that looks plausible" — exactly
what CLAUDE.md's *Dealing with unknowns and uncertainty* section forbids. Per that
section's step 4, no further attempt was made; the `_ReservedN()` placeholders in
`IQueryPropertyBag.cs` are left as-is. Effort was redirected to a different family of
previously-skipped COM interfaces (the six offer-collection interfaces) that turned
out to be fully recoverable from ILSpy alone — see
`logs/Microsoft.Zune/Service/OfferCollection.md`.

**Positive, actionable finding:** `QueryPropertyBag`'s constructor TODO (get an
`IQueryPropertyBag*` from `ZuneLibraryExports.CreatePropertyBag`) is a real,
now-located P/Invoke target: `ZuneNativeLib.dll`, mangled export
`?CreatePropertyBag@ZuneLibraryExports@@YAJPEAPEAUIQueryPropertyBag@@@Z`, signature
`int CreatePropertyBag(IQueryPropertyBag** ppPropertyBag)` (`__cdecl`, matching the
x64 Microsoft ABI regardless per the 2026-07-12 calling-convention entry below). Not
yet wired up — `ZuneNativeLib.dll` isn't currently a build-time dependency of this
project and doing so raises platform/deployment questions (Windows-only native DLL,
not addressed by this entry) outside the scope of this session's task.

---

## 2026-07-12 — Interface reconstruction

**Source:** ILSpy decompilation of `ZuneShell/lib/ZuneDBApi.dll` (mixed-mode C++/CLI assembly).

ILSpy cannot see the native side of a C++/CLI assembly, so `IQueryPropertyBag` is only
visible indirectly — through the managed `QueryPropertyBag` class that wraps it. The
vtable offsets were read from the raw `delegate* unmanaged[Cdecl, Cdecl]` call sites
that ILSpy emits for native virtual dispatch.

### Vtable layout (x64, pointer size = 8 bytes)

Offsets confirmed by reading the decompiled call sites in `QueryPropertyBag`:

| Slot | Byte offset | Signature | Source |
|------|------------|-----------|--------|
| 0 | 0  | `QueryInterface` (IUnknown) | assumed; standard COM |
| 1 | 8  | `AddRef` (IUnknown) | assumed; standard COM |
| 2 | 16 | `Release` (IUnknown) | confirmed — `!QueryPropertyBag()` destructor calls `*(vtable + 16)` |
| 3 | 24 | unknown | not observed in any managed call site |
| 4 | 32 | unknown | not observed in any managed call site |
| 5 | 40 | `SetString(EQueryPropertyBagProp prop, wchar_t* value) -> HRESULT` | `SetValue` with `string` branch: `*(vtable + 40)` |
| 6 | 48 | unknown | not observed in any managed call site |
| 7 | 56 | `SetInt(EQueryPropertyBagProp prop, int value) -> HRESULT` | `SetValue` with `int`/`bool` branch: `*(vtable + 56)` |
| 8–12 | 64–96 | unknown | not observed in any managed call site |
| 13 | 104 | `IsSet(EQueryPropertyBagProp prop, int* out) -> HRESULT` | `IsSet`: `*(vtable + 104)` |

Unknown slots (3, 4, 6, 8–12) are declared as `void _ReservedN()` placeholders in
`IQueryPropertyBag.cs` to preserve vtable ordering without blocking compilation.
These placeholders must not be called, and their signatures must be verified against
the native binary before the CCW/RCW is used in production.

**Needs:** Ghidra analysis of the native `IQueryPropertyBag` vtable definition to
fill in slots 3, 4, 6, and 8–12.

---

## 2026-07-12 — GUID: initial search

**Source:** ILSpy search for `IQueryPropertyBag` as a type in `ZuneDBApi.dll`.

The `[GeneratedComInterface]` attribute requires a GUID. ILSpy finds no managed type
named `IQueryPropertyBag` (only the global-namespace native struct and the
`CComPtrNtv<IQueryPropertyBag>` wrapper). A placeholder GUID
`3D8A1F2B-6C4E-4A5D-9B7F-2E0C1A8D3F4E` was used.

---

## 2026-07-12 — GUID: follow-up search in managed metadata

**Task:** Check whether the real GUID is encoded anywhere accessible to ILSpy.
Specifically examined: the global-namespace `IQueryPropertyBag` struct, `<Module>`,
and all call sites in `QueryPropertyBag`.

**Findings:**

1. The global-namespace struct `IQueryPropertyBag` carries only `[NativeCppClass]`,
   `[DebugInfoInPDB]`, and `[MiscellaneousBits(65)]` — no `[Guid]` attribute.
2. `<Module>` could not be decompiled by ILSpy (tool returned an error); its fields
   are not inspectable through the managed surface.
3. Full decompilation of `MicrosoftZuneInterop.QueryPropertyBag` shows no IID
   reference in managed code. `GetIQueryPropertyBag()` returns the raw pointer
   directly; there is no `QueryInterface` call visible.
4. The destructor (`!QueryPropertyBag`) calls `*(vtable + 16)` with just `this`,
   confirming `Release` is at slot 2 — consistent with IUnknown, but this reveals
   no IID.

**Conclusion:** The GUID is not present in the managed metadata of `ZuneDBApi.dll`.
If one exists it is in the native segment only (`__declspec(uuid(...))` or
`DEFINE_GUID`). Recovery requires Ghidra. Placeholder GUID stands.

It is also possible the interface has no GUID at all: the user confirmed that
`IQueryPropertyBag` objects are not `CoCreateInstance`-registered, and no
`QueryInterface` call was observed, so a GUID may simply never be needed at runtime.

---

## 2026-07-12 — Calling convention

The ILSpy decompilation emits `delegate* unmanaged[Cdecl, Cdecl]` for the vtable
dispatch. The duplicate `Cdecl` is a known ILSpy artifact of C++/CLI compilation and
does not mean the calling convention is Cdecl. On x64 Windows, Stdcall and Cdecl
are identical (both use the Microsoft x64 ABI), so the distinction does not affect
correctness there.

The current implementation uses `[GeneratedComInterface]`, which selects the correct
calling convention automatically. If raw vtable dispatch is ever needed (e.g. to call
the placeholder-reserved slots), the convention should be verified against the native
binary before assuming Stdcall.
