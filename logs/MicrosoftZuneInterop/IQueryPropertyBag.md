# IQueryPropertyBag — decompilation log

Append-only. Do not edit previous entries.

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
