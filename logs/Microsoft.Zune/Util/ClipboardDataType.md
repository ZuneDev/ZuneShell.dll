# ClipboardDataType — decompilation log

Append-only. Do not edit previous entries.

---

## 2026-07-20 — Full decompilation

**Source:** ILSpy `decompile_type` on `Microsoft.Zune.Util.ClipboardDataType` in
`ZuneShell/lib/ZuneDBApi.dll` (mixed-mode C++/CLI assembly), query "full source
verbatim including all enum member integer values and any [Flags] attribute".

This enum is a genuine managed C# enum (namespace `Microsoft.Zune.Util`, matching
CLAUDE.md's folder mapping exactly), so the decompilation is fully accurate:

```csharp
public enum ClipboardDataType
{
    Text = 1,
    Image = 2,
    UnicodeText = 13,
    FileDropList = 15,
}
```

Values were copied verbatim; no assumptions required. `Microsoft.Zune.Util.Clipboard`
(same folder) already referenced this type as a stub dependency before this file
existed — this decompilation unblocks that class's build.
