# MicrosoftZunePlayback.PlayerInterop — decompilation log

Append-only. Do not edit previous entries.

---

## 2026-07-21 — Structs instead of raw offset arithmetic

**Trigger:** user feedback on the entry below — the `OnStatusChanged`/
`OnTransportStatusChanged`/`MarshalAlert`/`HResultDenialsEqual`/`OnPropertyChanged`
methods read the native buffers via raw `byte* p = (byte*)ptr; *(int*)(p + N)` casts.
Asked to replace that with proper struct definitions instead.

**Change:** added `MCPlayerStatus.cs`, `MCTransportStatus.cs`, and
`MCHResultAnnouncement.cs` (the three structs `IMCPlayerEvents`/`IMCTransportEvents`
pass pointers to), plus `PresentationInfoData.cs`, `VolumeInfoData.cs`, and
`MBRHeuristicData.cs` (the three tagged-union payload shapes `OnPropertyChanged`'s
`pData` can point at, keyed by the `wzKey` string — `"presentationinfo"`,
`"volumeinfo"`, `"mbrheuristicsdata"`). All six use `[StructLayout(LayoutKind.Explicit)]`
with `[FieldOffset]` on exactly the offsets each field was recovered at in the entry
below — explicit layout doesn't require covering every byte, so gaps with no evidence
(like `MCHResultAnnouncement`'s unexplained 4 bytes between `Id` and `HResult`) are
simply left undeclared rather than padded with an invented field. `PlayerInterop`'s
`OnXxx`/`MarshalAlert`/`HResultDenialsEqual` methods now dereference typed pointer
fields (`pStatus->Duration`, `hrAlert->HResult`, etc.) instead of manual byte-offset
casts.

**COM boundary stays `nint`:** `IMCPlayerEvents`/`IMCTransportEvents`'s methods still
take plain `nint` for `pStatus`/`pAlert`/`pData`/`pTransportStatus` — the safest,
definitely-blittable parameter type for a `[GeneratedComInterface]` method, since raw
pointer-to-struct parameters on source-generated COM interfaces weren't verified to be
supported. The cast to the typed pointer (`(MCPlayerStatus*)pStatus`, etc.) happens
immediately in `CPlayerInteropEventSink`, one line inside the COM boundary, so none of
the *logic* ever touches an untyped offset again.

---

## 2026-07-21 — Full COM interface reconstruction

**Trigger:** user request to complete the PlayerInterop implementation including its
COM interfaces, superseding the stub described in the entry below.

**Source:** `mcp__ilspy__decompile_type`/`decompile_method` on the *entire* original
`MicrosoftZunePlayback.PlayerInterop` class body (constructor, `Initialize`,
`Uninitialize`, `Close`, `Play`, `Pause`, `Stop`, all three `SeekTo*` methods, `SetUri`,
`SetNextUri`, `CancelNext`, `ConnectAnimationsToSpectrumAnalyzer`,
`DisconnectAnimationsFromSpectrumAnalyzer`, `ProgressivePlaybackReleaseFile`,
`ProgressivePlaybackReopenFile`, every property getter/setter, `OnStatusChanged`,
`OnPropertyChanged`, `OnTransportStatusChanged`, `OnTransportPositionChanged`,
`OnUriSet`, `OnAlertOccurred`, `MarshalAlert`, `HResultDenialsEqual`, `Dispose`, and the
finalizer) in `ZuneShell/lib/ZuneDBApi.dll`. Property setters in particular only
decompiled successfully via a single whole-class `decompile_type` call — per-method
`decompile_method` calls errored out for every property accessor tried (`set_Volume`,
`set_Mute`, `set_Rate`, `set_WindowHandle`, etc.) for unknown reasons; the whole-class
call worked and returned all of them at once.

**Six native COM interfaces, fully reconstructed as `[GeneratedComInterface]` partials**
(`IMCPlayer.cs`, `IMCTransport.cs`, `IMCPlayerSetUri.cs`, `IMCDynamicImage.cs`,
`IMCVolumeControl.cs`, `IZuneSpectrumMgr.cs`), with vtable slots read directly off the
decompiled `(*(ulong*)(*(long*)ptr + N))` call sites (N/8 = slot index, same recovery
method as `logs/MicrosoftZuneInterop/IQueryPropertyBag.md`):

- **IMCTransport, IMCPlayerSetUri, IMCDynamicImage, IMCVolumeControl, IZuneSpectrumMgr**
  are *fully* recovered with no gaps — every vtable slot from 3 up to the highest
  observed is backed by a real call site (e.g. `IMCTransport` slots 3–14: `Initialize`,
  `Pause`, `Play`, `Stop`, `SeekToRelativePosition`, `SeekToAbsolutePosition`,
  `SeekToRelativeFrame`, `SetRate`, `SetResetOnStop`, `SetPositionEventInterval`,
  `ReleaseFile`, `ReopenFile` — recovered from `Play`/`Pause`/`Stop`/the three `SeekTo*`
  methods/the `Rate` and `ResetOnStop` setters/`Initialize`/`ProgressivePlaybackRelease`
  and `ReopenFile`).
- **IMCPlayer** has slots 3 (`Initialize`), 4 (`Uninitialize`), 5 (`Close`) confirmed,
  but slots 6–7 are never referenced anywhere and slot 8 is called once (with just
  `this`, discarding the result) at the very end of the original `Uninitialize()`
  sequence, after the native state machine already reported `MCPlayerState.Uninitialized`
  via the event sink, right before every sub-interface pointer is released — its purpose
  is unconfirmed and it's declared as `_Reserved8()` accordingly.

**Real GUIDs recovered for 5 of 6 interfaces** — unlike `IQueryPropertyBag` (which had no
GUID anywhere in managed metadata), the original `Initialize()` obtains `IMCTransport`,
`IMCPlayerSetUri`, `IMCDynamicImage`, `IMCVolumeControl`, and `IZuneSpectrumMgr` via
`IMCPlayer::QueryInterface` (vtable slot 0) using literal GUIDs ILSpy exposes as
`<Module>._GUID_<hex>` static field names (e.g.
`_GUID_2f33a725_95cb_4080_adef_93a067a707ba` → `2f33a725-95cb-4080-adef-93a067a707ba`
for `IMCTransport`). These are the *real* native IIDs, used verbatim in each interface's
`[Guid(...)]` attribute. `IMCPlayer` itself is never QueryInterface'd for (it's returned
already-typed from the native factory), so it keeps a placeholder GUID.

**Casting instead of manual QueryInterface:** rather than declaring an explicit
`QueryInterface(Guid*, void**)` method and calling it manually (as the original C++ did,
and as slot 0 is shown doing), `PlayerInterop.Initialize()` obtains the sub-interfaces by
casting the `[GeneratedComInterface]`-wrapped `IMCPlayer` RCW directly —
`(IMCTransport)_player` etc. — relying on .NET's source-generated COM support for
interface-to-interface casts on a `ComWrappers`-based RCW, which performs a real
`QueryInterface` call under the hood. This is the same idiom the existing
`MicrosoftZuneInterop.QueryPropertyBag`/`IQueryPropertyBag` pair already established.

**Event sink (native→managed direction):** the original native `CPlayerInteropEventSink`
implements `IMCPlayerEvents`/`IMCTransportEvents`/`IMCPlayerSetUriEvents` via C++
multiple inheritance (base vtables at object offsets 0/8/16, confirmed by the pointer
adjustments `uEventSink`, `uEventSink + 8`, `uEventSink + 16` at the three `Initialize()`
call sites) plus a `gcroot<PlayerInterop^>` back-reference and a fourth field (offset 40)
used only to stash a manual-reset event handle for
`ProgressivePlaybackReleaseFile`/`ReopenFile`. None of that C++ ABI layout was
replicated: the reimplemented `CPlayerInteropEventSink` is a plain `[GeneratedComClass]`
implementing all three event interfaces directly — `ComWrappers` answers `QueryInterface`
for each on its own — and the offset-40 event handle is just an ordinary
`ManualResetEventSlim` field on `PlayerInterop` now
(`_progressivePlaybackReleaseEvent`). The three event interfaces themselves
(`IMCPlayerEvents.cs`, `IMCTransportEvents.cs`, `IMCPlayerSetUriEvents.cs`) use
placeholder GUIDs — they're implemented by our own code and never QueryInterface'd for
by name in the decompiled managed call sites, so no real GUID was recoverable.

**Structs skipped in favor of raw offset reads:** `MCPlayerStatus`, `MCTransportStatus`,
and `MCHResultAnnouncement` (the native structs `OnStatusChanged`/
`OnTransportStatusChanged`/`MarshalAlert`/`HResultDenialsEqual` read from) were *not*
turned into typed C# structs. `MCHResultAnnouncement` in particular has an unexplained
4-byte gap between its `Id` pointer (offset 0) and `HResult` (offset 12) that no managed
call site ever reads — naming a field there would be inventing data. Since none of these
three are part of any public API surface (they only ever appear as raw pointers passed
between the native side and our own internal `OnXxx` handlers), the original raw
`byte*`-plus-offset reads were kept almost verbatim (translated from C++/CLI unsafe IL
to plain C# unsafe pointer arithmetic) rather than wrapping them in a struct whose layout
can't be fully verified. `RECT` (used by `IMCDynamicImage.SetVideoPosition`, originally
`tagRECT`) is the one exception — it's a completely standard, stable, publicly documented
Win32 struct, safe to declare confidently.

**`HResultDenialsEqual`'s original body** was an unreadable ~150-line ILSpy dump of
manually inlined `wcscmp`-style loops (an artifact of decompiling raw pointer/`fixed`
IL). It was replaced with a direct equivalent using `Marshal.PtrToStringUni` for the two
string comparisons, keeping the exact same four-way comparison (`HResult`, `SourceLine`,
`Id`, `SourceFile`) with none of the intermediate character-walking logic.

**What still cannot be completed:** obtaining an actual `IMCPlayer` instance in the first
place. The original `Initialize()` calls `WmpCoreInitialize()` and
`CWmpPlayer_GetInstance(IMCPlayer**)` — both resolved by ILSpy as `<Module>`-scoped
calls, meaning they're native machine code statically linked directly into the original
`ZuneShell/lib/ZuneDBApi.dll`, not P/Invokable exports of some other module. This project
doesn't ship or reimplement that native playback engine at all, so there is nothing to
call here — `Initialize()` has a `nint playerPtr = 0;` TODO placeholder marking exactly
where a real factory call needs to go once one exists; every line after it is real,
finished COM interop against the interfaces above and needs no further changes when that
day comes. Also unrecovered: what signals the `ManualResetEventSlim` that
`ProgressivePlaybackReleaseFile` waits on — no `SetEvent`-equivalent call for it appears
anywhere in the decompiled `PlayerInterop`, so it must happen inside native code this
project can't see. Both gaps are called out in-line in `PlayerInterop.cs`.

---

## 2026-07-21 — Full public surface, stub playback engine (superseded above)

**Trigger:** `ZuneImpl/Playback/PlayerInteropAudioService.cs` is compiled whenever
`OPENZUNE` is defined (it is — see `Directory.Build.props` at the repo root and in
`libs/ZuneUIXTools/libs/MicrosoftIris`), and needs `MicrosoftZunePlayback.PlayerInterop`
plus several supporting types.

**Source:** `mcp__ilspy__get_type_members` on `MicrosoftZunePlayback.PlayerInterop`,
and `decompile_type` on `MCTransportState`, `MCPlayerState`, `MBRHeuristicState`,
`VideoWindow`, `Announcement`, `AnnouncementHandler`, `PlayerPropertyChangedEventArgs`,
`PlayerPropertyChangedEventHandler`, `PlayerBandwithUpdateEventHandler`, and
`BandwidthUpdateArgs` — all in `ZuneShell/lib/ZuneDBApi.dll`. All ten enums/POCOs
had fully reflectable members and were transcribed verbatim.

**Namespace/folder:** `MicrosoftZunePlayback` has no entry in CLAUDE.md's
namespace-to-folder table. Followed the existing pattern for `MicrosoftZuneInterop`
and `MicrosoftZuneLibrary` (top-level folder matching the namespace name exactly) and
created `MicrosoftZunePlayback/` at the project root.

**`PlayerInterop` itself:** wraps six native COM pointers (`IMCPlayer`, `IMCTransport`,
`IMCPlayerSetUri`, `IMCDynamicImage`, `IMCVolumeControl`, `IZuneSpectrumMgr`) plus a
`CPlayerInteropEventSink` callback sink — none reverse engineered (native playback
engine, requires Ghidra). The full public surface (every method/property/event from
`get_type_members`) was reconstructed so `PlayerInteropAudioService` keeps compiling
against the exact original API, but method bodies are stubs: they update the backing
fields and raise the corresponding event synchronously (e.g. `SetUri` stores the URI
and immediately raises `UriSet`; `Play`/`Pause`/`Stop` update `TransportState` and raise
`TransportStatusChanged`) rather than driving a real native player. This was a
deliberate choice beyond a pure no-op: `PlayerInteropAudioService.Play` subscribes to
`UriSet` and waits for it before calling `Play()` — a silent no-op stub that never
raises the event would make that call path hang forever. Raising the event keeps the
caller's state machine unstuck while being explicit (via the class-level comment) that
no audio actually plays yet.

**Needs:** Ghidra analysis of the six native playback interfaces, then replacing the
field-update stubs with real COM calls, following the `QueryPropertyBag` pattern (see
`logs/MicrosoftZuneInterop/IQueryPropertyBag.md`).
