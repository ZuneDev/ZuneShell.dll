# Microsoft.Zune.Service offer collections — decompilation log

Append-only. Do not edit previous entries.

---

## 2026-07-21 — Six collection COM interfaces + IMediaRights/IPriceInfo/IContextData reverse engineered

**Trigger:** user request to continue reverse engineering COM interfaces skipped in
previous iterations. Started by chasing the two remaining `_ReservedN` gaps in
`IQueryPropertyBag` (slots 3, 4, 6, 8–12) via Ghidra — see the negative result logged
in `logs/MicrosoftZuneInterop/IQueryPropertyBag.md` under this same date. That dead
end prompted a pivot to a *different* family of skipped interfaces flagged in this
file's 2026-07-21 entry above: the six native COM interfaces backing
`AlbumOfferCollection`, `AppOfferCollection`, `TrackOfferCollection`,
`VideoOfferCollection`, `BillingOfferCollection`, and `CreditCardCollection`, which
were previously simplified to drop their `Init`/native-pointer logic entirely. Unlike
`IService` (see `logs/Microsoft.Zune/Service/Service.md`), these six interfaces'
vtable layouts turned out to be *fully recoverable from ILSpy alone* — no Ghidra
needed — because ILSpy's raw pointer-arithmetic dump of each `*OfferCollection.Init`
method (mixed-mode C++/CLI decompiled by `mcp__ilspy__decompile_type` against
`ZuneShell/lib/ZuneDBApi.dll`) preserves every `*(vtable + N)` call site verbatim,
exactly the same recovery method already validated for `IQueryPropertyBag` and the
`PlayerInterop` sub-interfaces.

**Interfaces recovered, each with GetCount()/GetItem(...) at slots 3/4 (IUnknown
assumed at 0–2), no slot beyond 4 ever called:**
- `IMusicAlbumCollection` (`AlbumOfferCollection.Init`)
- `IAppCollection` (`AppOfferCollection.Init`)
- `IMusicTrackCollection` (`TrackOfferCollection.Init`)
- `IVideoCollection` (`VideoOfferCollection.Init` — see caveat below)
- `IBillingOfferCollection` (`BillingOfferCollection.Init`) — flat scalar/BSTR
  `GetItem` (no metadata struct), 8 out-params
- `ICreditCardCollection` (`CreditCardCollection.Init`) — flat scalar/BSTR
  `GetItem`, 15 out-params

**A single shared interface, `IMediaRights`,** turned out to be embedded as a native
pointer field inside all four media metadata structs (`MusicAlbumMetadata` @88,
`AppMetadata` @136, `MusicTrackMetadata` @152, `VideoMetadata` @192 — different
offsets in each parent struct, same interface). Cross-referencing all four
`Init` bodies against each other was essential: no single offer type calls every
slot, but every slot two or more offer types *do* call agrees exactly (e.g. slot 9
"HasRights(EMediaRights, EMediaFormat)" is called identically by Album, Track, and
Video). Recovered slots: 8 (`CanConvertFormat`, Video only), 9 (`HasRights`), 13
(`GetPriceInfo`, simple 3-arg), 16 (`GetUpgradePriceInfo`, Video only, richest
overload with 3 GUIDs), 17 (`GetPriceInfo2`, App/Video, 2-GUID+BSTR overload), 18
(`IsInCollection`), 21 (`GetPreviouslyPurchased2`, App/Video, rich overload with
rights+format), 22 (`GetPreviouslyPurchased`, Album/Track, simple 2-bool overload),
26 (`IsSubscriptionFree`, Track only), 28 (`GetSeasonPurchaseInfo`, Video only, takes
the also-recovered `ESeasonPurchaseFlags`). All method *names* are inferred from
behavior, not recovered — `IMediaRights` is never part of any public surface (only
consumed internally by `OfferCollection` subclasses), so no original identifiers
exist to match. Gaps (slots 3–7, 10–12, 14–15, 19–20, 23–25, 27) are declared as
`_ReservedN()` placeholders per the `IQueryPropertyBag` convention.

**`IPriceInfo`** (a further nested interface, returned by `IMediaRights`'s
GetPriceInfo* methods) was recovered independently from `PriceInfo.Init(IPriceInfo*)`
in `ZuneShell/lib/ZuneDBApi.dll` — slots 3–6 (`GetPointsPrice`, `GetCurrencyPrice`,
`HasPoints`, `HasCurrency`) and 8–9 (`GetDisplayPrice`, `GetCurrencyCode` as BSTR
out-params); slot 7 is never called and left as `_Reserved7()`.

**`IContextData`** (returned by the four media `GetItem` methods' `IContextData**`
out-param) was recovered from `OfferCollection.GetRecommendationContext`'s *original*
3-arg body (`ZuneShell/lib/ZuneDBApi.dll`) — a single slot 3, `GetContextString(out
BSTR)`. This let `GetRecommendationContext` be restored to its original 3-arg shape
(`Guid, IDictionary, IContextData?`) instead of the 2-arg simplification from the
2026-07-21 entry above; accessibility had to be tightened from `protected` to
`private protected` since `IContextData` is `internal` and C# won't allow a less
accessible parameter type on a more accessible member (CS0051).

**Metadata struct sizes are verified, not guessed:** `mcp__ilspy__decompile_type`
against the bare struct names (`MusicAlbumMetadata`, `AppMetadata`,
`MusicTrackMetadata`, `VideoMetadata`, `VideoOfferParams`) shows each as an opaque
native value type, but still carrying its real
`[StructLayout(LayoutKind.Sequential, Size = N)]` from the original metadata — 112,
144, 160, 200, and 12 bytes respectively. Every struct below is declared
`[StructLayout(LayoutKind.Explicit, Size = <verified N>)]` with `[FieldOffset]` only
on bytes actually read in some `Init` body; this is safe specifically *because* the
overall size is verified (not inferred from the highest observed offset), so native
`GetItem` writing the full struct can never overrun our declared layout even though
several fields remain unnamed. Same precedent as `MCHResultAnnouncement` in
`logs/MicrosoftZunePlayback/PlayerInterop.md`.

**BSTR ownership:** all metadata-struct string fields and all flat
`IBillingOfferCollection`/`ICreditCardCollection` `GetItem` string out-params are
BSTRs the caller owns once the call returns (the original metadata structs have a
native `{ctor}`/`{dtor}` pair that frees them automatically on C++ scope exit, which
doesn't exist for us — so each `*OfferCollection.Init` here calls
`Marshal.PtrToStringUni` then `Marshal.FreeBSTR` explicitly, matching the
already-established explicit-free idiom `BillingOfferCollection`/
`CreditCardCollection`'s *original* decompiled bodies already used for their own
flat BSTR out-params).

**`ECreditCardType`'s values are verified, not guessed**, unlike every other
placeholder native enum in this project: `CreditCardCollection.Init` does a bare
`(CreditCardType)creditCardType` reinterpret cast with no translation table, so its
members must be numerically identical to the already-recovered public
`Microsoft.Zune.Service.CreditCardType` enum. Only the native member *names* are a
TODO. `EBillingPaymentType` and `ESeasonPurchaseFlags` got the normal placeholder
treatment (values/names both TODO, `ESeasonPurchaseFlags` at least has 1/2 inferred
from a binary branch — see that file's header comment).

**Caveat — `VideoOfferCollection.Init` is only partially reconstructed.** ILSpy's own
decompiler flags this method's control flow as corrupted (`Incompatible stack
types: I vs Ref`), and several `bool` locals are read before any visible assignment
in the raw dump. The original walks a hardcoded 13-entry `VideoOfferParams` table
(recovered verbatim: `{Rights, FormatA, FormatB}` triples covering rental/HD/season-
pass combinations — values `{3,3,4}, {3,2,4}, {3,4,-1}, {7,2,-1}, {7,3,-1}, {8,2,-1},
{8,3,-1}, {9,3,-1}, {9,2,-1}, {12,2,4}, {12,3,4}, {13,2,-1}, {13,3,-1}`) to emit
*multiple* `VideoOffer` entries per video with different `isRental`/`isHD`/
`isSeasonPurchase` flags. Per CLAUDE.md's decompilation rules ("should not be used
verbatim" for low-quality output), that tier-selection algorithm was **not**
reimplemented — guessing it from a self-reported-corrupted decompile would risk
silently-wrong purchase-tier logic, worse than an honest gap. What *is* real: the
`IVideoCollection` interface, `VideoMetadata`'s field layout, and every
`IMediaRights`/`IPriceInfo` call this file makes are all independently corroborated
by the other three (uncorrupted) offer collections. The reimplementation emits one
`VideoOffer` per item using rights tier 12 ("owned", by analogy with Album/Track's
tier-4/3 "owned" probes) and hardcodes `isHD`/`isRental`/`isSeasonPurchase`/
`previouslyPurchased` to `false`. TODO: recover the real per-tier enumeration via
Ghidra once the corrupted control flow can be cross-checked against the native
binary.

**Verified with `dotnet build ZuneDBApi/ZuneDBApi.csproj`:** 0 errors. New files
introduce the expected `CS86xx` nullable-reference warnings on `string`-typed
BSTR/null-return paths, consistent with the same warning pattern already present
throughout this project's pre-nullable-adoption classes (e.g. `Offer.cs`,
`Address.cs`) — not a new class of issue.

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
