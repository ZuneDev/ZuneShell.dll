# EQueryType, EListType, EQueryTypeView, EPinType — decompilation log

Append-only. Do not edit previous entries.

---

## 2026-07-21 — Full decompilation, global namespace

**Source:** ILSpy `decompile_type` on each of `EQueryType`, `EListType`,
`EQueryTypeView`, and `EPinType` in `ZuneShell/lib/ZuneDBApi.dll`. All four returned
fully reflectable enum bodies with explicit integer values, copied verbatim — same
situation as `EMediaTypes` (see `logs/EMediaTypes.md`), unlike `DRMQueryState`/
`EMediaRights`/`EMediaFormat` (see `logs/DRMQueryState.md`).

**Namespace:** all four report an empty namespace via ILSpy, confirming global
C++/CLI declaration in the original assembly, same as `EMediaTypes`. Each was placed
at the `ZuneDBApi` project root (`EQueryType.cs`, `EListType.cs`, `EQueryTypeView.cs`,
`EPinType.cs`) with no `namespace` declaration, for the same reason `EMediaTypes.cs`
is there.

**Trigger:** needed by `ZuneImpl/Library/{ZuneQueryList,StrixLibraryDataProviderQuery}.cs`
after `ZuneImpl.csproj`/`ZuneShell.csproj` stopped referencing the prebuilt
`ZuneShell/lib/ZuneDBApi.dll` binary directly (see
`logs/Microsoft.Zune/Service/Service.md` for the fuller trigger description).
