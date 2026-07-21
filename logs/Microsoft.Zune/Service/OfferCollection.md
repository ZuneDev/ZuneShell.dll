# Microsoft.Zune.Service offer collections — decompilation log

Append-only. Do not edit previous entries.

---

## 2026-07-21 — Collection classes simplified to public-surface-only

**Source:** ILSpy `decompile_type` on `Microsoft.Zune.Service.{AlbumOfferCollection,
AppOfferCollection, TrackOfferCollection, VideoOfferCollection, BillingOfferCollection,
CreditCardCollection}`, plus `OfferCollection` (their shared abstract base) and
`Offer`/`AlbumOffer`/`AppOffer`/`TrackOffer`/`VideoOffer`/`PriceInfo`, in
`ZuneShell/lib/ZuneDBApi.dll`.

**Findings:** Every collection class's original decompiled body used C++-style
`~Foo()`/`!Foo()` destructor syntax (invalid C#, per CLAUDE.md's COM objects section)
around an internal native pointer (e.g. `IMusicAlbumCollection*`,
`IAppCollection*`, `IVideoCollection*`, `IBillingOfferCollection*`,
`ICreditCardCollection*`) and an internal `Init(...)` method that walked the native
collection via raw vtable calls, unmarshaling nested native structs
(`MusicAlbumMetadata`, `AppMetadata`, `MusicTrackMetadata`, `VideoMetadata`, each with
their *own* nested vtable-dispatched sub-objects at fixed byte offsets). None of these
native interfaces or structs have any presence in `ZuneDBApi.dll`'s managed metadata —
ILSpy can only show the raw pointer arithmetic at the call sites, not the interfaces
themselves.

**Checked:** grepped `ZuneImpl` for any call to `.Init(`, `.GetCollection()`, or a
public constructor of these six collection types — none exist. All six also have no
public constructor in the original (only an `internal` no-arg constructor); external
code (`ZuneImpl`) only ever receives instances via `Get*CompleteCallback` delegates
and reads their public `Items` property.

**Decision:** Since `Init`/`GetCollection`/the native pointer field are all `internal`
(not part of the "original API surface" CLAUDE.md requires matching — only public/
protected members are), they were dropped rather than faithfully transcribed. Each
collection class now exposes just its original public surface (`IList Items { get; }`,
`IDisposable`), backed by a plain `ArrayList`/ `IList` field. `OfferCollection`'s
protected `GetRecommendationContext` helper was kept but simplified to drop its
`IContextData*` parameter (only the `IDictionary` cache-lookup path is reimplemented).

**Why not real logic:** correctly reconstructing `Init` would require the full native
vtable layout of six distinct COM interfaces plus four native metadata structs with
byte-exact field offsets — none of which are visible to ILSpy. Fabricating plausible-
looking offsets without Ghidra verification would produce code that *compiles* but
silently corrupts memory or reads garbage at runtime, which is worse than an honest
stub. This follows the same "don't guess past what's observed" principle documented
in `logs/MicrosoftZuneInterop/IQueryPropertyBag.md`.

**Needs:** Ghidra analysis of `IMusicAlbumCollection`, `IAppCollection`,
`IMusicTrackCollection`, `IVideoCollection`, `IBillingOfferCollection`,
`ICreditCardCollection`, and the `MusicAlbumMetadata`/`AppMetadata`/
`MusicTrackMetadata`/`VideoMetadata` structs, then reimplementing `Init` as a real
`[GeneratedComInterface]`-backed unmarshal, following the `QueryPropertyBag` pattern.

`TokenDetails`'s original `internal unsafe TokenDetails(ITokenDetails* pTokenDetails)`
constructor was handled the same way — `ITokenDetails` is equally unrecovered, and
`ZuneImpl` never constructs a `TokenDetails` itself (only ever receives one via `out`
params), so it was replaced with a parameterless `internal` constructor.

All public getter-only classes in this family (`Offer` and its four subclasses,
`PriceInfo`, `BillingOffer`, `DRMInfo`, `CountryBaseDetails`, `CountryFieldValidator`,
`RatingSystemBase`, `RatingValue`) had no native pointers at all and were transcribed
verbatim (field-for-field, property-for-property) with no simplification needed.
