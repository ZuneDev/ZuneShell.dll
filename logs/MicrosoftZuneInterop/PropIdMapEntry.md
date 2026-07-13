# PropIdMapEntry — decompilation log

Append-only. Do not edit previous entries.

---

## 2026-07-12 — Decompilation

**Source:** ILSpy decompilation of `MicrosoftZuneInterop.PropIdMapEntry` in
`ZuneShell/lib/ZuneDBApi.dll` (mixed-mode C++/CLI assembly).

ILSpy reports the type as:

```csharp
[StructLayout(LayoutKind.Sequential, Size = 16)]
[NativeCppClass]
internal static struct PropIdMapEntry
{
    private long <alignment member>;
}
```

`[NativeCppClass]` means the type is a native C++ struct projected into the managed
surface for interop purposes only — it has no managed fields, only a size guarantee.
The single `long <alignment member>` field is an ILSpy artifact used to fill the
declared size; it carries no semantic meaning.

### Layout

The 16-byte layout is derived from the decompilation of `QueryPropertyBag.MapNameToProp`
(see `QueryPropertyBag.md`), which walks the array as:

```
[0..7]   wchar_t*             name   (pointer to null-terminated wide string)
[8..11]  EQueryPropertyBagProp id
[12..15] (padding)
```

This matches the C++/CLI compiler's natural layout for a struct containing a 64-bit
pointer followed by a 32-bit enum on x64, with 4 bytes of trailing padding to reach
the next 8-byte alignment boundary.

### Implementation decision

The struct is internal and not part of the public API surface. It exists purely so
the 16-byte element size of `kPropIdMap` is encoded in one place and not scattered
as magic numbers. Since ILSpy cannot see the field names or types (they are in native
data), the managed declaration retains only the `Size = 16` guarantee:

```csharp
[StructLayout(LayoutKind.Sequential, Size = 16)]
internal struct PropIdMapEntry
{
    private long _alignment;
}
```

No further implementation is possible until `kPropIdMap` is recovered from the native
binary (see `QueryPropertyBag.md` — unknown Q3).
