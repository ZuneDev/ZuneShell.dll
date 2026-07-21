# EMediaTypes — decompilation log

Append-only. Do not edit previous entries.

---

## 2026-07-20 — Full decompilation

**Source:** ILSpy `decompile_type` on `EMediaTypes` in `ZuneShell/lib/ZuneDBApi.dll`
(mixed-mode C++/CLI assembly), query "full source verbatim including all enum
member integer values and any [Flags] attribute". `get_type_members` was used
first, which listed all 63 named members (but not their values); `decompile_type`
then returned the full enum body with explicit integer values for each member.

Unlike `EQueryPropertyBagProp` (see `MicrosoftZuneInterop/EQueryPropertyBagProp.md`),
this enum's members and values are fully present in the managed metadata and were
copied verbatim — no assumptions required.

**Namespace:** `get_type_members` reports `Namespace: ` (empty) for this type,
confirming it is declared in the global C++/CLI namespace in the original
assembly, unlike most other reimplemented types which map to
`Microsoft.Zune.*` / `MicrosoftZuneInterop` / `MicrosoftZuneLibrary` per
CLAUDE.md's folder mapping. Since CLAUDE.md's namespace-to-folder table has no
entry for the global namespace, and this type is consumed by both
`Microsoft.Zune.Util` (`Notification.cs`) and `Microsoft.Zune.Configuration`
(`FileAssociationInfo.cs`) without a namespace qualifier — confirming it truly
needs to stay global for those call sites to keep compiling as originally
written — the file was placed at the ZuneDBApi project root (`EMediaTypes.cs`)
with no `namespace` declaration, matching the original global-namespace
placement exactly, rather than being folded into any one namespace's folder.
