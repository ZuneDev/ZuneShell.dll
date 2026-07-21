# QueryPropertyBag — decompilation log

Append-only. Do not edit previous entries.

---

## 2026-07-20 — StrategyBasedComWrappers.Instance does not exist

**Task:** build error `CS0117: 'StrategyBasedComWrappers' does not contain a
definition for 'Instance'` on the two call sites in this file (constructor
TODO comment and `GetIQueryPropertyBag`).

**Verification:** fetched the official API reference
(`learn.microsoft.com/.../system.runtime.interopservices.marshalling.strategybasedcomwrappers`)
rather than guessing at the correct member name. Confirmed `StrategyBasedComWrappers`
has no static `Instance`/`Default`/singleton member of any kind — only a public
parameterless constructor plus instance methods/properties inherited from
`ComWrappers`. This differs from the assumption baked into the code as written
(that it exposed a singleton like many other `ComWrappers`-derived helper types
do by convention).

**Fix:** added a `private static readonly StrategyBasedComWrappers s_comWrappers = new();`
field and replaced both `StrategyBasedComWrappers.Instance` references with it.
`ComWrappers`-derived types are meant to be instantiated once and reused for
the process lifetime (per the same doc page's remarks on `ComWrappers`), so a
single cached static instance is the correct usage pattern here, not a
per-call `new()`.

## 2026-07-20 — EQueryPropertyBagProp placeholder declaration

**Task:** `dotnet build` failed because `EQueryPropertyBagProp` (referenced by
`IQueryPropertyBag.cs` and this file) did not exist as a type anywhere in the
project — only documented as an unknown in the 2026-07-12 entries below.

**Verification via ILSpy:** re-checked via `mcp__ilspy__get_type_members` on
`EQueryPropertyBagProp` in `ZuneShell/lib/ZuneDBApi.dll` — confirms the
2026-07-12 finding still holds: the type exists in the assembly (`Kind: Enum`,
`Namespace: ` empty) but exposes only the implicit `value__` backing field, no
named members. This is consistent with it being a native-only C++ enum from
an anonymous namespace that ILSpy cannot recover members for.

**Action:** declared `public enum EQueryPropertyBagProp { }` (no members) in
`MicrosoftZuneInterop/EQueryPropertyBagProp.cs`, in the global namespace to
match the original metadata's empty namespace. An empty C# enum still allows
arbitrary values via cast — e.g. `(EQueryPropertyBagProp)(-1)` in
`QueryPropertyBag.MapNameToProp`'s sentinel check — so this is sufficient to
unblock compilation without guessing at names/values that remain genuinely
unrecoverable without Ghidra (per *Dealing with unknowns and uncertainty*,
step 4: this is the assumption with fewest predicates, documented via
`// TODO` in the new file rather than guessed).

**Also observed:** after this type existed, the previously-reported
`SYSLIB1051` diagnostic on `IQueryPropertyBag.SetString`/`SetInt`/`IsSet`
(prop parameter) disappeared. That diagnostic was a downstream symptom of the
missing type (the source generator couldn't reason about a parameter type
that didn't resolve), not a real marshalling limitation of `EQueryPropertyBagProp`
itself — a plain `int`-backed enum is blittable and needs no special handling
for `[GeneratedComInterface]`.

## 2026-07-12 — Decompilation

**Source:** ILSpy decompilation of `MicrosoftZuneInterop.QueryPropertyBag` in
`ZuneShell/lib/ZuneDBApi.dll` (mixed-mode C++/CLI assembly).

`QueryPropertyBag` is a managed wrapper around a native `IQueryPropertyBag*` COM
pointer created by the native export `ZuneLibraryExports.CreatePropertyBag`. The
class is not itself a COM object — it simply holds the pointer and delegates to it.

### Constructor

Calls `global::<Module>.ZuneLibraryExports.CreatePropertyBag(&pPropertyBag)`.
This is a native export. The DLL name, exact symbol, and calling convention are
unknown and cannot be determined from the managed metadata alone.

**Unknown (Q5):** P/Invoke signature for `CreatePropertyBag`. Needs native binary
inspection or documentation. Constructor is stubbed with a `// TODO` comment.

### SetValue / IsSet

Dispatch to `IQueryPropertyBag` vtable slots 5, 7, 13 (see
`../MicrosoftZuneInterop/IQueryPropertyBag.md`). Logic is fully implementable once
`MapNameToProp` is implemented.

### MapNameToProp

Walks a static 37-entry native array `MicrosoftZuneInterop.?A0x52c37a46.kPropIdMap`
of `PropIdMapEntry` structs (each 16 bytes: `wchar_t*` name at [0..7],
`EQueryPropertyBagProp` id at [8..11], 4 bytes padding at [12..15]) using
`_wcsicmp` for case-insensitive comparison.

The name strings and enum values are in native data — not visible through ILSpy.
Without them, `MapNameToProp` (and by extension `SetValue` and `IsSet`) cannot be
implemented.

**Unknown (Q3):** The 37 name strings and their `EQueryPropertyBagProp` values.
Needs Ghidra analysis of the native data segment.

**Unknown (Q4):** `EQueryPropertyBagProp` enum definition. Lives in the global
(unnamed) namespace; ILSpy queries with a namespace prefix fail. Resolving Q3 and
Q4 together in Ghidra would be efficient since the map table embeds the enum values
directly.

### GetIQueryPropertyBag

Returns `m_pPropertyBag` directly. No `QueryInterface`, no IID involved.

### PackIDList — implemented 2026-07-12

**Source:** Full ILSpy decompilation of `PackIDList` in `ZuneDBApi.dll`.

The original allocates a 16-byte `IDList` struct on the native heap and fills it:

```csharp
// Original (ILSpy output, simplified):
IDList* ptr = (IDList*)global::<Module>.@new(16uL);
*(int*)ptr = count;                                          // Count at offset 0
*(long*)((ulong)(nint)ptr + 8uL) = 0L;                     // zero Ids ptr at offset 8
*(long*)((ulong)(nint)ptr + 8uL) = (nint)global::<Module>.new[](count * 4uL); // heap int[]
for (int i = 0; i < count; i++)
    *(int*)(i * 4 + *(long*)((ulong)(nint)ptr + 8uL)) = (int)multiIds[i];
```

**Translation decisions:**

- `global::<Module>.@new(16)` → `Marshal.AllocHGlobal(16)`.
  `operator new` and `Marshal.AllocHGlobal` both allocate unmanaged heap memory.
  The key difference: `operator new` can return null on failure; `Marshal.AllocHGlobal`
  throws `OutOfMemoryException` instead. The original null-check / try-catch is
  therefore not needed — if allocation fails the exception propagates naturally.

- `global::<Module>.new[](count * 4)` → `Marshal.AllocHGlobal(count * sizeof(int))`.
  Same allocation family; same reasoning. `sizeof(int)` is 4 on all platforms.

- The original overflow guard `(count > 4611686018427387903L ? ulong.MaxValue : count * 4)`
  is omitted. `IList.Count` returns a 32-bit `int` (max ~2 billion). Multiplying by
  4 yields at most ~8 GB, which fits in a 64-bit `nint` and would result in an
  `OutOfMemoryException` from the allocator long before arithmetic overflow.

- The Ids pointer slot at offset 8 is zeroed before the second `AllocHGlobal` call,
  matching the original's initialization order. This ensures the struct is never in
  a half-initialized state if the second allocation throws.

- Pointer arithmetic is done with `unsafe` pointer casts, directly mirroring the
  original byte-level layout.

**Reference:** The `IDList` struct layout (16 bytes: `int Count` + 4 pad + `int* Ids`)
is confirmed by the ILSpy output for this method and consistent with the comment
at the top of `QueryPropertyBag.cs`.

---

## 2026-07-12 — IMultiSortAttributes vtable layout

**Source:** ILSpy decompilation of `PackMultiSortAttributes` in `ZuneDBApi.dll`.

The original `PackMultiSortAttributes` calls two vtable slots on the native
`IMultiSortAttributes*` returned by `CreateMultiSortAttributes`:

```csharp
// ptr  ← vtable slot at byte offset 32
ptr  = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*>)(*(ulong*)(*(long*)intPtr  + 32)))((nint)intPtr);
// ptr2 ← vtable slot at byte offset 40
ptr2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*>)(*(ulong*)(*(long*)intPtr2 + 40)))((nint)intPtr2);
```

In the fill loop:
- `ptr4` starts at `ptr2` and advances by 4; `*ptr4 = (int)eQuerySortType` → `ptr2`
  receives `EQuerySortType` values → offset 40 (slot 5) = **GetSortOrders()**
- `ptr5 = ptr - ptr2`; `*(int*)((byte*)ptr5 + ptr4) = num5` simplifies to writing
  the schema index (`CSchemaMap.GetIndex` result) into `ptr[i]` → offset 32 (slot 4)
  = **GetSortAttributes()**

Assuming IUnknown at slots 0–2 (offsets 0, 8, 16), slot 3 (offset 24) is not called
in this method and remains unknown. The original comment had these two methods
transposed; the code comment and this log both reflect the corrected order.

| Slot | Byte offset | Signature |
|------|------------|-----------|
| 0–2  | 0–16       | IUnknown (assumed) |
| 3    | 24         | unknown |
| 4    | 32         | `GetSortAttributes() -> int*` (schema index array) |
| 5    | 40         | `GetSortOrders() -> int*` (EQuerySortType array) |

### PackMultiSortAttributes — still a stub

Requires two native P/Invokes that cannot yet be resolved from managed metadata:
- `ZuneLibraryExports.CreateMultiSortAttributes(int count, IMultiSortAttributes** out)`
- `CSchemaMap.GetIndex(wchar_t* name) -> int`

Both the DLL names and calling conventions are unknown. Stubbed with a `// TODO`
comment listing the full algorithm derived from the decompilation.
