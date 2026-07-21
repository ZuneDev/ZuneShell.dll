# MicrosoftZuneLibrary namespace — decompilation log

Append-only. Do not edit previous entries.

---

## 2026-07-21 — Bulk reconstruction of the MicrosoftZuneLibrary namespace

**Trigger:** continuation of "decompile the missing types in ZuneShell.csproj" after
ZuneDBApi and ZuneImpl were brought to a clean build in an earlier pass (see
`logs/EQueryType.md`, `logs/Microsoft.Zune/Service/Service.md`, and
`logs/MicrosoftZunePlayback/PlayerInterop.md` for that earlier pass). This entry covers
the second, much larger pass: the entire `MicrosoftZuneLibrary` namespace, which
`ZuneShell.csproj` depends on for device sync, firmware update, wireless provisioning,
CD burning, and the local media library database — none of which had been touched yet.

**Source:** `mcp__ilspy__get_type_members`/`decompile_type` on every type ILSpy reported
missing, one subsystem at a time: `Device`/`DeviceList`/`DeviceAssetSet`/`SyncRules`/
`SyncRulesView`/`GasGauge` (device sync), `FirmwareUpdater`/`FirmwareRestorer`/
`FirmwareRestorePoint`/`UpdatePackageCollection`/`UpdateStep`/`FirmwareUpdatePackage`
(firmware), `WlanProfile`/`WlanProfileList`/`WlanAuthCipherPair`/
`WlanAuthCipherPairList` (wireless), `ZuneLibraryCDDevice`/`ZuneLibraryCDDeviceList`/
`ZuneLibraryCDRecorder` (CD burning), `ZuneLibrary`/`ZuneQueryList`/`SchemaMap`
(the library database core), `AlbumMetadata`/`TrackMetadata`/`FileEntry`/
`IDatabaseMedia` (media metadata), `LibraryVirtualList`/`LibraryDataProviderListItem`
(virtualized list rows), `HMESettings` (Windows Media Player home sharing),
`AppInitializationSequencer`/`InteropNotifications`/`MetadataMgrNotifications`/
`LaunchFromShellHelper` (app startup/shell plumbing), `BandwidthTestInterop`/
`BandwidthTestErrorArgs` (MBR bandwidth probe, in `MicrosoftZunePlayback` not
`MicrosoftZuneLibrary`), plus ~10 supporting global-namespace enums
(`EEndpointCapability`, `ESyncRelationship`, `ETranscodeOptimization`,
`ESyncOperationStatus`, `EDeviceSyncRuleType`, `ESyncMode`, `EQuerySortType`,
`ESubscriptionSource` already existed) and ~50 one-line delegate types.

**Same convention as the `Service.cs`/`PlayerInterop` passes**: every type's *public*
signature was reconstructed in full from ILSpy, but method bodies are "pure no-op
stub"s (defaults / `unchecked((int)0x80004005)` for HRESULT-shaped returns) wherever the
original body drives a native COM pointer this project doesn't have (no
`IEndpointHost`, `IFirmwareUpdater`, `IWlanProvider`, `IWMPCDDevice`,
`IDatabaseQueryResults`, etc. have been reverse engineered — that's genuinely a Ghidra
job, not an ILSpy one, same reasoning as `logs/Microsoft.Zune/Service/Service.md`).

**Exceptions — real logic, not stubs:**
- `AlbumMetadata`/`TrackMetadata`: the originals wrap `IAlbumInfo*`/`ITrackInfo*` with
  every property forwarding to a vtable call, but no code anywhere in this solution
  constructs one from a real native pointer (only `AlbumMetadata.GetTrack`, itself
  unreachable, would have). Reimplemented as plain mutable POCOs with ordinary backing
  fields — behaviorally identical for every reachable caller, and `AlbumMetadata`'s
  pure `ReleaseYear` 2-digit-year normalization (00-29 → 2000s, 30-99 → 1900s) was kept
  verbatim since it's self-contained, non-native logic.
- `ZuneLibrary.CompareWithoutArticles`/`DoesFileExist`: implemented for real
  (`string.Compare`/`File.Exists`) rather than stubbed, since these have no native
  dependency at all — the only reason they're not literally identical to the original
  is that the original's article-stripping logic isn't visible to ILSpy either
  (native), so `CompareWithoutArticles` is an approximation (plain culture-aware
  compare, no article-stripping) rather than a guess at the stripping rule.
- `HttpWebRequest`/`HttpWebResponse` (from the earlier pass, `Microsoft.Zune.Service`):
  backed by a real `System.Net.Http.HttpClient` — see
  `logs/Microsoft.Zune/Service/HttpWebRequest.md` for that entry specifically.

**Simplifications beyond the original (base classes dropped):**
`LibraryVirtualList`/`LibraryDataProviderListItem` were written as standalone classes
rather than reconstructing their real base classes (`VirtualDatabaseList` extending
`Microsoft.Iris.VirtualList`, and `LibraryDataProviderItemBase` extending
`Microsoft.Iris.DataProviderObject`, respectively). **This was later found to be
wrong** — see the entry below.

**`FileEntry`'s constructor** originally takes a native `ushort* path`; since nothing
outside `MicrosoftZuneLibrary` constructs a `FileEntry` (only `LaunchFromShellHelper`'s
unreachable `FileFound` callback would), the constructor was changed to take `string`
and made `internal`, matching the pattern used elsewhere for internal-only natively-fed
constructors.

**Needs:** Ghidra analysis of every native interface named above to replace stub
bodies with real COM calls, following the `QueryPropertyBag` pattern (see
`logs/MicrosoftZuneInterop/IQueryPropertyBag.md`).

---

## 2026-07-21 — Fixed: LibraryVirtualList/LibraryDataProviderListItem base classes reconstructed

**Follow-up to the entry below.** Reconstructed the real base-class chain instead of
the standalone simplification: `VirtualDatabaseList` (extends `Microsoft.Iris.VirtualList`,
implements `IQueryListEvents`) and `LibraryDataProviderItemBase` (extends
`Microsoft.Iris.DataProviderObject`, implements `IDatabaseMedia`), plus their small
supporting types `ListNotifyData`, `AsyncGetThumbnailState`, `BulkItemAction`,
`LibraryDataProvider`, `StaticLibraryDataProvider`.

**`VirtualDatabaseList`** transcribed close to verbatim — its original body is pure
C#/Iris framework code (deferred list-change notification batching) with no native
pointer involved; only the ETW `PERFTRACE_COLLECTIONEVENT` calls were dropped
(diagnostic-only, same treatment as WPP tracing elsewhere in this codebase).

**`LibraryDataProviderItemBase.GetProperty`/`SetProperty`/`GetMediaIdAndType`**
likewise transcribed close to verbatim — real `DataProviderMapping`-based property
dispatch logic with no native dependency of its own, only WPP tracing dropped. Its
thumbnail-extraction machinery (`SetNewThumbnail`, `BeginGetThumbnail`,
`GetFieldValue`/`SetFieldValue`) does eventually reach the native library database, so
those stay stubbed.

**`LibraryDataProvider.NameToMediaType`** transcribed verbatim (pure switch over Iris
markup type-schema names, no native dependency). `LibraryDataProvider.ActOnItems`/
`GetSortAttributes` and `StaticLibraryDataProvider.Register` remain stubs — they call
into the native database/sync-rules engine.

**`LibraryVirtualList`/`LibraryDataProviderListItem` simplified beyond the original
in one respect that stands**: the original constructor wiring (auto-refresh
advise/unadvise against a live `ZuneQueryList`, seeding `Count` from a real query,
`OnRequestItem` constructing real `LibraryDataProviderListItem`/`UnavailableLibraryListItem`
rows via an internal `LibraryDataProviderQuery` owner) was dropped, because
`ZuneQueryList` is permanently empty in this codebase (no native database — see the
entry above) — `OnRequestItem` can structurally never be called with `Count` stuck at
0, so reconstructing `LibraryDataProviderQuery`/`UnavailableLibraryListItem` to satisfy
it would add a large amount of code that's provably dead until the native database
exists. `LibraryDataProviderListItem`'s constructor accordingly dropped its
`LibraryDataProviderQuery owner` parameter (replaced with the plain `DataProviderQuery`
its base class actually needs) since nothing else calls it.

**Result:** `ZuneDBApi.csproj`/`ZuneImpl.csproj` still build clean after this fix.
`ZuneShell.csproj`'s error count dropped from 380 to 336 — the ~20 additional
newly-surfaced names below (`FeatureEnablement`, `MessagingService`, etc.) are the
next item, tracked in the entry below this one.

---

## 2026-07-21 — Build regression: LibraryVirtualList/LibraryDataProviderListItem needed their real base classes

**Finding:** after the bulk pass above got `ZuneDBApi.csproj` and `ZuneImpl.csproj` back
to a clean build, rebuilding `ZuneShell.csproj` dropped its error count from 437 to 8,
then — once those last 8 names resolved — jumped to **380 distinct errors across 74
files**. This is not new breakage; it's the *next* layer of missing surface that
Roslyn couldn't report yet, because a file with an unresolved type error skips deeper
semantic checks (member lookups, argument matching) on expressions that mention that
type. Concretely: `ListReleaseBehaviorManager.cs` calls
`libraryVirtualList.VisualReleaseBehavior`, and `MergeHelper.cs`/`LibraryAlbumInfo.cs`
call `libraryDataProviderListItem.GetProperty(...)`/`.SetProperty(...)` —
members that only exist on the real `VirtualDatabaseList`/`LibraryDataProviderItemBase`
base classes this pass had dropped as "unreachable." They were not unreachable; the
first error-triage pass just couldn't see that far into those files yet.

**Also newly visible in this same layer** (never previously reported, for the same
reason): `FeatureEnablement`, `MessagingService`, `PhotoManager`,
`StaticLibraryDataProvider`, `Win7ShellManager`, `WorkerQueue`, `SQMData`,
`EWin7LibraryKind`, `UsageDataService`, `FamilySettingsManager`, `FeaturesChangedApi`,
`ContentRefreshTask`, `UriResourceTracker`, `TelemetryAPI`, `UpdateManager`,
`RadioStationManager`, `PinManager`, `WMISEndpointIds`, plus ~15 more delegate/args
types (`CheckForUpdatesArgs`, `SyncEventArgs`, `FirmwareUpdateBeginArgs`, etc.) and the
already-known `System.Drawing`/native-pointer gaps (`Point`, `Size`,
`VirtualDatabaseList`, `HttpWebException`, `JumpListSession`).

**Status:** not yet fixed at the time this entry was written. This confirms
`ZuneShell.csproj`'s true dependency surface is substantially larger than what a single
missing-type triage pass can reveal — each wave of fixes uncovers the next. See the
two follow-up entries above for how the `VirtualDatabaseList`/`LibraryDataProviderItemBase`
half and the ~20-name second half were each resolved.

---

## 2026-07-21 — Fixed: the ~20-name second wave (FeatureEnablement, MessagingService, etc.)

**Follow-up to the entry above.** Added ~30 more types spread across `Microsoft.Zune.Util`
(`FeatureEnablement`, `FeaturesChangedApi`/`FeaturesChangedHandler`, `Win7ShellManager`
+ its 3 delegates, `PhotoManager`, `FamilySettingsManager`, `RadioStationManager`
+`RadioStationProgressHandler`, `PinManager`, `SQMData`, `TelemetryAPI`, `UpdateManager`
+`UpdateProgressHandler`, `ContentRefreshTask`, `JumpListSession`), `Microsoft.Zune.Messaging`
(`MessagingService`, `CommentCallback`), `Microsoft.Zune.Service` (`UriResourceTracker`,
`WMISEndpointIds`, `HttpWebException`), and `MicrosoftZuneLibrary` (`WorkerQueue`,
`CallbackOnUIThread`, `CompletionAction`, `SyncEventArgs`/`DeferrableSyncEventArgs`/
`DeleteFromLibraryEventArgs`, `CheckForUpdatesArgs`, `FirmwareProcessCompleteArgs`,
`FirmwareUpdateBeginArgs`/`FirmwareUpdateProgressArgs`, `FirmwareRestorePointCollection`),
plus the global-namespace `EWin7LibraryKind` enum. Same stub convention as every other
entry in this file.

**Found and fixed a earlier mistake in passing:** `Microsoft.Zune.Util.JumpListEntry`
(written in the very first ZuneShell pass, before any real caller of it had been
checked) had invented properties (`Title`/`Path`/`Arguments`/`IconPath`) that don't
match the original at all. The real original (confirmed via `get_type_members`) has
`IconIndex`/`CommandLineArguments`/`Name` — which is what `JumpListManager.cs` actually
calls. Corrected in place. This is the second time an unverified guess at a "simple
POCO" property shape turned out wrong once real call sites were checked (the first
being the `VirtualDatabaseList`/`LibraryDataProviderItemBase` base-class drop above) —
reinforcing that even small POCO-looking types need their real call sites checked
before finalizing a shape, not just their constructor arguments.

**`SQMData.s_rgSQMDataPoints` left as an empty array** rather than transcribing the
original's ~230-entry static table (one `SQMDataPoint` per `SQMDataId`, each needing a
hand-authored `SQMAction`/`argCount` not visible via ILSpy's member listing, only via
decompiling the static initializer, which risks size explosion for a purely-diagnostic
data structure). `SQMLog.FindDataPoint`'s linear scan simply falls through to
`s_sqmDataPointInvalid` for every id with this — the same externally-observable
behavior as "SQM logging is disabled," consistent with every other telemetry call in
this codebase being a no-op.

**Result:** `ZuneDBApi.csproj`/`ZuneImpl.csproj` still build clean.
`ZuneShell.csproj`'s error count dropped from 336 to 88.

---

## 2026-07-21 — ZuneShell.csproj reaches a clean build (0 errors)

**Follow-up to the entry above.** The remaining 88 errors were all real signature/shape
mismatches against actual `ZuneShell.csproj` call sites (not missing types) — fixed
each by checking the real decompiled ZuneShell source and correcting the ZuneDBApi
side to match, rather than touching the checked-in decompiled ZuneShell files:

- **`eErrorCondition` double-declaration bug, found and fixed:** `Microsoft.Zune/ErrorMapperApi/ErrorMapperApi.cs`
  (written in an earlier session, before this one) declared its own
  `Microsoft.Zune.ErrorMapperApi.eErrorCondition` enum. ILSpy's assembly-wide type
  listing confirms only *one* `eErrorCondition` exists in the original — in the global
  namespace (recovered this session as `eErrorCondition.cs`, matching the
  `EMediaTypes.cs` global-namespace convention). The namespaced duplicate was deleted
  and `ErrorMapperApi.cs` updated to use the real global enum. This is the second
  confirmed case this session of a memory/earlier-session assumption not matching real
  ILSpy data once cross-checked (the first being `JumpListEntry`'s properties, see the
  entry above) — a reminder that everything from earlier sessions is worth
  double-checking against ILSpy directly when a real call site disagrees with it,
  not just newly-written code.
- **`out` vs `ref` parameter-passing convention, ~50 occurrences:** the original
  ZuneShell.dll (decompiled and checked in as ZuneShell's own source, not touched this
  session) calls `Device`/`SyncRules`/`HMESettings`/`DeviceList` methods with `ref`, not
  `out`, for what ILSpy's `get_type_members` reports as `Int32&`/`Boolean&`/etc.
  parameters. Both are valid translations of a bare by-ref IL parameter (the difference
  is only a `[Out]` attribute, which ILSpy's member-listing view doesn't surface) — and
  since the actual calling convention is only observable at the real call site, not at
  the callee's metadata, `Device.cs`/`DeviceList.cs`/`SyncRules.cs`/`HMESettings.cs`'s
  by-ref parameters were bulk-converted from `out` to `ref` to match every real
  ZuneShell.csproj caller. `FirmwareOperationBase.CheckPowerRequirements` was the one
  exception confirmed to actually use `out` at its call sites, and was left as `out`.
- **`FirmwareOperationBase` recovered as a real shared base class** of
  `FirmwareUpdater`/`FirmwareRestorer` (`EnterContinuousPowerMode`/
  `LeaveContinuousPowerMode`/`IsValid`/`CheckPowerRequirements` all live there in the
  original, confirmed via `mcp__ilspy__search_members_by_name` after `UIFirmwareUpdater.cs`/
  `UIFirmwareRestorer.cs` called them directly on both types).
- **`UpdatePackageCollection`'s original has two *named* indexed properties**
  (`Item[int]` get-only and a separate `Selected[int]` get/set) — legal in the IL the
  original C++/CLI or VB.NET compiler emitted, but C# has no syntax for a non-default
  named indexer. The real decompiled `UIFirmwareUpdater.cs` calls them as plain methods
  (`collection.get_Item(index)`, `collection.set_Selected(index, value)`), which is
  exactly how C# requires calling a named indexer's accessors — so `UpdatePackageCollection`
  declares ordinary `get_Item`/`set_Selected` methods instead of a C# `this[int]`
  indexer, sidestepping the CS0571 "cannot explicitly call accessor" error a compiler-
  synthesized indexer would produce.
- **Constructor accessibility fixes:** `Microsoft.Zune.Service.Address`/`CreditCard`/
  `AccountSettings` (all written in an earlier session) had `internal` parameterless
  constructors, but real `ZuneShell.csproj` call sites (`new Address()`, `new CreditCard()`,
  `new AccountSettings()`) construct them directly from a different assembly — changed
  to `public` to match actual reachable usage.
- **`AutoPlaylistBuilder`'s constructors were missing entirely** (an earlier pass in
  this session added the type but never checked `mcp__ilspy__decompile_method` for its
  `.ctor` overloads) — added all three original overloads
  (`AutoPlaylistBuilder(int playlistId)`, `AutoPlaylistBuilder(EMediaTypes type)`,
  `AutoPlaylistBuilder()`).
- **`JumpListSession.GetDisallowedDestinations`'s generic type was wrong**
  (`List<string>` guessed, should be `List<JumpListEntry>` per `JumpListManager.cs`'s
  actual call site, which calls `.CommandLineArguments` on each list element).

**Final result: `ZuneDBApi.csproj`, `ZuneImpl.csproj`, and `ZuneShell.csproj` all build
with 0 errors.** Remaining warnings are pre-existing `CA1416` Windows-Registry-API
platform-compatibility notices (expected per CLAUDE.md — the original Zune client was
Windows-only) and nullability warnings, none new from this session's work.
