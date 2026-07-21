# Microsoft.Zune.Service.Service — decompilation log

Append-only. Do not edit previous entries.

---

## 2026-07-21 — Full stub reconstruction

**Trigger:** `ZuneImpl/ZuneImpl.csproj` and `ZuneShell/ZuneShell.csproj` were updated
(outside this log) to drop their prebuilt-binary reference to the original
`ZuneShell/lib/ZuneDBApi.dll` and rely solely on the `ZuneDBApi` project reference.
This surfaced ~136 missing-type build errors in `ZuneImpl`, most of them rooted in
`Microsoft.Zune.Service.Service`, which backs `ZuneImpl/Service/CommunityService.cs`
and `ZuneImpl/Service/ProxyService.cs` via `Service.Instance.<Method>(...)` calls.

**Source:** `mcp__ilspy__get_type_members` on `Microsoft.Zune.Service.Service` in
`ZuneShell/lib/ZuneDBApi.dll`. The class wraps a single native COM pointer
(`private IService* m_pService`) behind ~90 public methods plus a `static Service
Instance` singleton and two `static` helpers (`GetEndPointUri`,
`ContentTypeToListType`).

**Decision:** Implemented every member with the exact public signature (matching
`ZuneImpl/Service/IService.cs`, which was already written against the original API
and confirms parameter directions — `out`/`ref`/plain — for every method), but each
body is a "pure no-op stub" per CLAUDE.md's stage 2 convention: returns a default
value or `HRESULT._E_FAIL`/`._S_OK` rather than talking to a real service backend.

**Why not real logic:** The native `IService` vtable has not been reverse engineered.
Unlike `IQueryPropertyBag` (see `logs/MicrosoftZuneInterop/IQueryPropertyBag.md`),
where a handful of vtable slots could be read directly off decompiled call sites,
`Service`'s ~90 methods each dispatch through `m_pService`'s vtable with dozens of
distinct native struct/interface parameters (`MusicAlbumMetadata`, `AppMetadata`,
`IPriceInfo`, `IContextData`, `IMusicAlbumCollection`, etc. — see
`logs/Microsoft.Zune/Service/OfferCollection.md`) that ILSpy cannot see at all, since
they're pure native types never exposed to managed code. Reconstructing this
correctly requires Ghidra analysis of the native commerce/account/DRM service layer.

**Needs:** Ghidra analysis of the native `IService` vtable and its parameter structs,
then replacing each stub body with a real `[GeneratedComInterface]`-backed call,
following the `QueryPropertyBag` pattern (see
`logs/MicrosoftZuneInterop/IQueryPropertyBag.md` and
`logs/MicrosoftZuneInterop/QueryPropertyBag.md`).

**Also needed for this file to compile:** `EMediaRights` and `EMediaFormat` (used as
parameters on one `GetContentUri` overload) are native `[NativeCppClass]` enums with
no reflectable members in the managed metadata (same situation as `DRMQueryState` —
see `logs/DRMQueryState.md`). No code anywhere in the solution references a named
member of either enum (only the types themselves), so both were declared as
single-member placeholder enums (`Invalid = -1`) at the project root, with a TODO
citing the handful of raw integer literals (0–13) observed at unrelated native call
sites in `AlbumOfferCollection`/`AppOfferCollection`/`TrackOfferCollection`/
`VideoOfferCollection`'s original decompiled bodies. Recovering real member names
also requires Ghidra.
