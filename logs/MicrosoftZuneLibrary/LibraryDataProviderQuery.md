# MicrosoftZuneLibrary.LibraryDataProviderQuery — decompilation log

Append-only. Do not edit previous entries.

---

## 2026-07-27 — Stage 1 + 2 for LibraryDataProviderQuery, LibraryDataProviderQueryResult, DeferredSetResultArgs

**Trigger:** explicit request to complete stages 1 and 2 for
`MicrosoftZuneLibrary.LibraryDataProviderQuery`, following on from
`LibraryDataProvider.Register()` being wired up to call `Application.RegisterDataProvider`
in an earlier session (see `10fa402 Fill in more gaps in ZuneDBApi`), which left
`LibraryDataProvider.ConstructQuery` still returning `null` — meaning no "Library"-provider
query could ever actually be constructed until this type existed.

**Source:** `mcp__ilspy__decompile_type`/`get_type_members` on
`MicrosoftZuneLibrary.LibraryDataProviderQuery`, `LibraryDataProviderQueryResult`, and
`DeferredSetResultArgs` in the real `ZuneDBApi.dll`
(`/home/yoshiask/repos/ZuneDev/windows/shared/zune-x64/Zune/ZuneDBApi.dll`), plus
`ZuneLibrary.QueryDatabase` (5-arg overload) for cross-reference, since it calls the same
native export.

**`LibraryDataProviderQueryResult`/`DeferredSetResultArgs`**: plain, non-native
`DataProviderObject` subclass and data-carrier respectively — transcribed verbatim. (A
byte-identical copy of `LibraryDataProviderQueryResult`, renamed to
`StrixLibraryDataProviderQueryResult` and gated behind `#if OPENZUNE`, already existed in
`ZuneImpl/Library/` from an earlier Strix-integration pass — cross-checked against a fresh
ILSpy decompile of the real original and confirmed to match exactly, including the
`UnderlyingCollectionTypeCookie`/`UnderlyingCollectionTypeName` foreach in the
constructor.)

**`LibraryDataProviderQuery.BeginExecuteWorker`**: the original decompiles to raw
`IQueryPropertyBag*` vtable-offset dispatch (`*(long*)iQueryPropertyBag + N`) because the
native interface has no visible symbol names — per CLAUDE.md's *COM objects* rule, this
was translated onto `MicrosoftZuneInterop.QueryPropertyBag`'s managed wrapper instead of
reproducing raw pointer arithmetic. Mapped every vtable offset seen in the decompile to an
`EQueryPropertyBagProp` slot by dividing by 8 (offset 24→slot 3, 32→slot 4, 40→slot 5,
48→slot 6, 56→slot 7) and cross-checked each against the existing
`EQueryPropertyBagProp.cs` enum values already used at each call site — all 30+ call sites
agreed with the existing enum with zero mismatches, confirming the offsets. Slots 3/4/6
were previously undeclared reserved slots on `IQueryPropertyBag`; added
`SetIDList`/`SetMultiSortAttributes`/`SetInt64` there and corresponding
`QueryPropertyBag.SetIDList`/`SetMultiSortAttributes` methods (mirroring the existing
`SetValue`/`IsSet` style), plus a `ulong` case in `SetValue`'s switch for `SetInt64`
(needed for `DrmStateMask`).

**Found and fixed a latent bug in `QueryPropertyBag` while wiring this up:** `SetValue`/
`IsSet` used `_bag!.Method(...)` (null-forgiving) even though `_bag` is *always* null today
(`CreatePropertyBag` is a documented TODO, never called) — meaning any real caller would
have hit a `NullReferenceException` the first time either was invoked. Since property-bag
population and the `EQueryType` derivation switch that follows it are pure C#/Iris business
logic with no native dependency of their own (only the final `QueryDatabase` call is
native), added a `Dictionary<EQueryPropertyBagProp, object> _localValues` that `SetValue`/
`SetIDList` always populate and that `IsSet` falls back to when `_bag` is null. This makes
`LibraryDataProviderQuery`'s `EQueryType` derivation (which depends on `IsSet("Keywords")`,
`IsSet("ArtistIds")`, etc. reflecting what was actually set earlier in the same method)
behave correctly today, and remains forward-compatible: once `CreatePropertyBag` is wired
up, the same code paths start forwarding to the real native bag as well.

**Native tail stubbed:** the actual `global::<Module>.ZuneLibraryExports.QueryDatabase`
call (a raw native module-level export of the mixed-mode original, not a real accessible
managed type — confirmed it's the same export `ZuneLibrary.QueryDatabase`'s 5-arg overload
already calls internally) is not reverse engineered — no native database engine exists in
this codebase, see `logs/MicrosoftZuneLibrary/ZuneLibrary.md`. Replaced with the same
"always succeeds with an empty result set" convention already used by
`ZuneLibrary.QueryDatabase`'s existing stub (`new ZuneQueryList()`), dropping the
now-meaningless `COMException`/error-string plumbing around it (there is no HRESULT to
report). `GetSortAttributes`/`SetMultiSortAttributes` are wired the same way as the
original but are provably dead in practice: `LibraryDataProvider.GetSortAttributes` is
itself a stub that always returns `false` (see the entry below in `ZuneLibrary.md`), so
`QueryPropertyBag.PackMultiSortAttributes`'s `NotImplementedException` (needs
`CSchemaMap.GetIndex` + a `CreateMultiSortAttributes` factory, neither reverse engineered)
is never actually reached.

**`LibraryVirtualList`/`ZuneQueryList` construction matches the already-simplified
signatures** established in the earlier `LibraryVirtualList`/`LibraryDataProviderItemBase`
base-class-recovery pass (see `ZuneLibrary.md`, 2026-07-21): `LibraryVirtualList`'s
constructor has no `owner` parameter (dropped there since `ZuneQueryList` is always empty,
so `OnRequestItem` can never fire), and `ZuneQueryList` has no native-pointer constructor
overload — used the existing 3-arg `LibraryVirtualList(ZuneQueryList, bool, bool)` and
parameterless `ZuneQueryList()` accordingly, rather than reintroducing the original's
4-arg/native-pointer signatures.

**Wired up:** `LibraryDataProvider.ConstructQuery` now returns
`new LibraryDataProviderQuery(queryTypeCookie)` instead of `null`.

**Result:** `ZuneDBApi.csproj`, `ZuneImpl.csproj`, and `ZuneShell.csproj` all still build
with 0 errors, no new warnings.
