# MicrosoftZuneLibrary.FriendsOfArtistQuery — decompilation log

Append-only. Do not edit previous entries.

---

## 2026-07-26 — Stage 1 + 2 for FriendsOfArtistQuery, AsyncQueryParams, SetResultsParams

**Trigger:** explicit request to complete stages 1 and 2 for
`MicrosoftZuneLibrary.FriendsOfArtistQuery`. The user pointed out the type lives in
`ZuneDBApi.dll` (the original mixed-mode C++/CLI assembly at
`/home/yoshiask/repos/ZuneDev/windows/shared/zune-x64/Zune/ZuneDBApi.dll`), not in the
pure-managed `ZuneShell.dll` this session initially checked (which only carries the
`Microsoft.Zune.Shell`/`ZuneUI`/`ZuneXml` layer — the `MicrosoftZuneLibrary` namespace is
merged into `ZuneDBApi.dll`, confirmed via `ilspy_list_assembly_types` returning 0 hits
for that namespace in `ZuneShell.dll` and 160 hits in `ZuneDBApi.dll`).

**Source:** `mcp__ilspy__decompile_type` on `MicrosoftZuneLibrary.FriendsOfArtistQuery`,
`MicrosoftZuneLibrary.AsyncQueryParams`, and `MicrosoftZuneLibrary.SetResultsParams` (all
three are top-level types in that namespace, not nested — confirmed via
`list_assembly_types`).

**`AsyncQueryParams`/`SetResultsParams`**: plain immutable data-carrier classes with no
native dependency at all (just `Guid`/`bool`/`int`/`IList`/`Microsoft.Iris.DeferredInvokeHandler`
fields exposed as read-only properties). Transcribed as real logic — auto-properties set
from the constructor, matching CLAUDE.md's "prefer auto-properties" guidance rather than
the original's explicit backing-field get-only pattern (behaviorally identical).

**`FriendsOfArtistQuery`**: extends `Microsoft.Iris.ModelItem` (already present in this
repo via the `UIX.csproj` reference — real, not reimplemented this session) and is a
lazy, non-thread-safe singleton (`Instance` property), matching the original exactly
(no double-checked locking in the original either).

Real logic kept for:
- `Query(Guid, bool, int[, bool])`: the cache-hit short-circuit
  (`artistId == m_artistId && (!getFriends || m_getFriends) && userId == m_userId`),
  field assignment, `Guid.Empty` fast path, and `ThreadPool.QueueUserWorkItem` dispatch —
  none of this touches native code.
- `SetResults`/`DeferredSetResults`: pure change-tracking + `FirePropertyChanged` calls
  (`FirePropertyChanged` is `protected` on the real `ModelItem` base, so no reimplementation
  needed).
- The `Application.DeferredInvoke(DeferredInvokeHandler, object)` completion hop: this is
  real `Microsoft.Iris` framework machinery already implemented in `UIX.csproj`
  (`libs/ZuneUIXTools/libs/MicrosoftIris/UIX/Microsoft/Iris/Application.cs:324`), not a
  native call — confirmed by reading it before assuming it needed stubbing.

Stubbed (native-dependent):
- `AsyncQuery`'s body originally calls `global::<Module>.ZuneLibraryExports.UserCardsForMedia`
  and `.GetFieldValues` — native module-level exports of `ZuneDBApi.dll`, resolved through
  the same opaque native singleton/factory table documented as a dead end (no recoverable
  RTTI or symbol for the concrete implementation, would require dynamic tracing of a running
  process) in `logs/MicrosoftZuneInterop/IQueryPropertyBag.md`'s 2026-07-21 entry for
  `ZuneLibraryExports.CreatePropertyBag`/`CreateMultiSortAttributes`. Per CLAUDE.md's
  *Dealing with unknowns* step 4, this was not re-attempted from scratch — same native
  export family, same already-documented dead end. ILSpy's decompilation of `AsyncQuery`
  also emits unreadable `$ArrayType$...` struct spam for the native `DBPropertyRequestStruct`
  stack-allocated array, consistent with CLAUDE.md's warning that native/mixed-mode
  decompiler output is low quality and shouldn't be transcribed verbatim.
- Kept the surrounding framework intact (still runs on the thread pool, still completes via
  `Application.DeferredInvoke` so `QueryComplete` flips to `true` and `HasFriends`/`Friends`
  etc. fire their property-changed notifications) — every query now completes with "no
  friends found," the same treatment as `MicrosoftZuneLibrary.LibraryDataProviderItemBase`'s
  `GetFieldValue`/`SetFieldValue` stubs and `Microsoft.Zune.Util.DRMCanDoQuery`'s "every
  query answers no" convention.

**Dispose/finalizer note:** ILSpy's decompilation of the original showed invalid C# —
`private void ~FriendsOfArtistQuery() {}` as an ordinary method with an empty body, and
`Dispose(bool P_0)` calling `Finalize()` directly in the non-disposing branch. Per
CLAUDE.md's *COM objects* rule (rewrite invalid C++-style destructor syntax as a proper
C# `~Type()` finalizer), this was rewritten as the standard `Dispose(bool)`/`~Type()`
pattern with identical externally-observable behavior (disposing calls `base.Dispose()`;
the finalizer path is a no-op, matching the original's empty destructor body) rather than
transcribing the decompiler artifact verbatim.

**Result:** `ZuneDBApi.csproj` builds with 0 errors (1351 pre-existing warnings, none new).
