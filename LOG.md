# ZuneDBApi — Research Log

This file is **append-only**. Do not edit or remove previous entries.

---

## 2026-07-12 — MicrosoftZuneInterop decompilation

### Q1: Real GUID for `IQueryPropertyBag`

**File:** `ZuneDBApi/MicrosoftZuneInterop/IQueryPropertyBag.cs`

The `[GeneratedComInterface]` attribute requires a GUID. The native binary does not expose this GUID through managed metadata (ILSpy returns no result when searching for `IQueryPropertyBag` as a type). A placeholder GUID `3D8A1F2B-6C4E-4A5D-9B7F-2E0C1A8D3F4E` is currently used.

The real GUID (if one exists) would need to be recovered from the native binary using Ghidra — look for a `__declspec(uuid(...))` annotation on the `IQueryPropertyBag` class, or a `DEFINE_GUID`/`IID_IQueryPropertyBag` symbol. It is also possible the interface has no GUID and QI is never used by the native callers (the user noted it is not CoCreateInstance-registered), in which case the placeholder is acceptable long-term.

**Assumption made in code:** placeholder GUID used. Logged per unknowns procedure.

---

---

## 2026-07-12 — Q1 follow-up: GUID search in managed metadata

**Task:** Research whether the real GUID for `IQueryPropertyBag` is encoded anywhere accessible to ILSpy (checked `<Module>` and root-namespace types).

**Findings:**

1. The global-namespace struct `IQueryPropertyBag` (separate from `MicrosoftZuneInterop.IQueryPropertyBag`) has no `[Guid]` attribute — only `[NativeCppClass]`, `[DebugInfoInPDB]`, `[MiscellaneousBits(65)]`.
2. `<Module>` could not be decompiled by ILSpy (tool error); its fields are not inspectable through the managed surface.
3. Full decompilation of `MicrosoftZuneInterop.QueryPropertyBag` shows no IID reference anywhere in managed code. `GetIQueryPropertyBag()` simply returns the raw `m_pPropertyBag` pointer with no `QueryInterface` call.
4. The destructor (`!QueryPropertyBag`) calls offset 16 on the vtable, confirming IUnknown layout (slot 2 = Release). This does not reveal the IID.

**Conclusion:** The GUID is not present in managed metadata. If one exists, it lives only in native code (`__declspec(uuid(...))` or `DEFINE_GUID`). Ghidra remains the only path to recovery. Placeholder GUID in `IQueryPropertyBag.cs` stands.

---

### Q2: Unknown vtable slots in `IQueryPropertyBag`

**File:** `ZuneDBApi/MicrosoftZuneInterop/IQueryPropertyBag.cs`

The vtable offsets observed in `QueryPropertyBag`'s decompiled code (ILSpy) are:
- Slot 5 (offset 40): `SetString(prop, wchar_t*) -> HRESULT`
- Slot 7 (offset 56): `SetInt(prop, int) -> HRESULT`
- Slot 13 (offset 104): `IsSet(prop, int*) -> HRESULT`

Slots 3, 4, 6, and 8–12 are not observed being called in any managed code that ILSpy can see. Their signatures (return type, parameters) are unknown.

Placeholder `void _ReservedN()` methods are declared in the interface to preserve vtable ordering. These must not be called. If any of these slots are not HRESULT-returning void methods, the placeholders will be wrong and must be corrected before the CCW/RCW is used by native code.

**Needs:** Ghidra analysis of the native `IQueryPropertyBag` vtable definition.

---

### Q3: `kPropIdMap` — 37-entry property name table

**File:** `ZuneDBApi/MicrosoftZuneInterop/QueryPropertyBag.cs`, `MapNameToProp`

`QueryPropertyBag.MapNameToProp` walks a static array `MicrosoftZuneInterop.?A0x52c37a46.kPropIdMap` of 37 `PropIdMapEntry` structs (each 16 bytes: `wchar_t* name` at [0..7], `EQueryPropertyBagProp id` at [8..11], 4 bytes padding at [12..15]). The property name strings and their corresponding `EQueryPropertyBagProp` values are embedded in native data and are not visible through ILSpy.

Without this table, `MapNameToProp` (and by extension `SetValue` and `IsSet`) cannot be implemented.

**Needs:** Ghidra analysis of `kPropIdMap` in the native segment of `ZuneDBApi.dll`.

---

### Q4: `EQueryPropertyBagProp` enum values

**File:** `ZuneDBApi/GlobalEnums.cs` (where it will be added)

`EQueryPropertyBagProp` appears in the assembly listing and in the signatures of `IQueryPropertyBag` methods, but lives in the global (unnamed) namespace like other C++/CLI-compiled native enums. ILSpy queries for it with a namespace prefix fail. It has not yet been queried or added to `GlobalEnums.cs`.

The 37 entries in `kPropIdMap` (Q3 above) presumably map to values of this enum, so resolving Q3 and Q4 together would be efficient.

**Needs:** ILSpy query for `EQueryPropertyBagProp` (no namespace prefix), then Ghidra cross-reference against `kPropIdMap`.

---

### Q5: `ZuneLibraryExports.CreatePropertyBag` — P/Invoke signature

**File:** `ZuneDBApi/MicrosoftZuneInterop/QueryPropertyBag.cs`, constructor

The original constructor calls `global::<Module>.ZuneLibraryExports.CreatePropertyBag(&pPropertyBag)`. In a managed reimplementation this would be a P/Invoke into the native export. The DLL name, exact export symbol, and calling convention are unknown.

**Assumption made in code:** constructor body is empty with a `// TODO` comment.

---

### Q6: Calling convention for `IQueryPropertyBag` vtable methods

**File:** `ZuneDBApi/MicrosoftZuneInterop/QueryPropertyBag.cs` (previous `IntPtr`-based draft)

The ILSpy decompilation shows `delegate* unmanaged[Cdecl, Cdecl]` for the vtable dispatch (the double `Cdecl` appears to be an ILSpy artifact of C++/CLI compilation). Standard COM uses `__stdcall` on x86 and the platform ABI on x64 (where Cdecl and Stdcall converge). The current implementation uses `[GeneratedComInterface]` which handles this, but if raw vtable dispatch is ever needed (e.g. for the unknown slots), the calling convention should be verified against the native binary.
