using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Microsoft.Zune.Service;

// Native COM interface IMediaRights — vtable layout (x64, IUnknown = slots 0–2).
// Embedded as a native pointer field inside MusicAlbumMetadata, AppMetadata,
// MusicTrackMetadata, and VideoMetadata (a different byte offset in each — see those
// structs). Recovered by cross-referencing the four *OfferCollection.Init methods in
// ZuneShell/lib/ZuneDBApi.dll (see logs/Microsoft.Zune/Service/OfferCollection.md):
// each offer type only calls the subset of slots it needs, but the slot numbers agree
// everywhere they overlap (e.g. slot 9 "HasRights" is called with the identical
// (EMediaRights, EMediaFormat) signature by Album, Track, and Video).
//
// Method names below are inferred from behavior at the call site, not recovered from
// any symbol — IMediaRights is never part of a public API surface (only consumed
// internally by OfferCollection subclasses), so no original names exist to match.
//   [3]–[7]   unknown — never observed at any managed call site
//   [8]  CanConvertFormat(EMediaRights, EMediaFormat from, EMediaFormat to) -> BOOL  (Video only)
//   [9]  HasRights(EMediaRights, EMediaFormat) -> BOOL
//   [10]–[12] unknown
//   [13] GetPriceInfo(EMediaRights, EMediaFormat, out IPriceInfo) -> HRESULT
//   [14]–[15] unknown
//   [16] GetUpgradePriceInfo(EMediaRights, EMediaFormat from, EMediaFormat to, Guid*, Guid*, Guid*, out IPriceInfo, out BSTR) -> HRESULT  (Video only)
//   [17] GetPriceInfo2(EMediaRights, EMediaFormat, Guid*, Guid*, out IPriceInfo, out BSTR) -> HRESULT  (App, Video)
//   [18] IsInCollection() -> BOOL
//   [19]–[20] unknown
//   [21] GetPreviouslyPurchased2(EMediaRights, EMediaFormat, BOOL, BOOL) -> BOOL  (App, Video — richer overload)
//   [22] GetPreviouslyPurchased(BOOL, BOOL) -> BOOL  (Album, Track — simpler overload)
//   [23]–[25] unknown
//   [26] IsSubscriptionFree() -> BOOL  (Track only)
//   [27] unknown
//   [28] GetSeasonPurchaseInfo(ESeasonPurchaseFlags) -> BOOL  (Video only)
//
// No GUID recoverable; never QueryInterface'd for by name in any managed call site.
[GeneratedComInterface]
[Guid("2c9d4f7a-3b6e-4a1d-9f8c-5e2a7b4d1c9f")]
internal partial interface IMediaRights
{
    void _Reserved3();
    void _Reserved4();
    void _Reserved5();
    void _Reserved6();
    void _Reserved7();

    [PreserveSig]
    int CanConvertFormat(EMediaRights rights, EMediaFormat fromFormat, EMediaFormat toFormat);

    [PreserveSig]
    int HasRights(EMediaRights rights, EMediaFormat format);

    void _Reserved10();
    void _Reserved11();
    void _Reserved12();

    [PreserveSig]
    int GetPriceInfo(EMediaRights rights, EMediaFormat format, out IPriceInfo? priceInfo);

    void _Reserved14();
    void _Reserved15();

    [PreserveSig]
    unsafe int GetUpgradePriceInfo(EMediaRights rights, EMediaFormat fromFormat, EMediaFormat toFormat, Guid* guid1, Guid* guid2, Guid* guid3, out IPriceInfo? priceInfo, [MarshalAs(UnmanagedType.BStr)] out string expirationDate);

    [PreserveSig]
    unsafe int GetPriceInfo2(EMediaRights rights, EMediaFormat format, Guid* guid1, Guid* guid2, out IPriceInfo? priceInfo, [MarshalAs(UnmanagedType.BStr)] out string expirationDate);

    [PreserveSig]
    int IsInCollection();

    void _Reserved19();
    void _Reserved20();

    [PreserveSig]
    int GetPreviouslyPurchased2(EMediaRights rights, EMediaFormat format, int flag1, int flag2);

    [PreserveSig]
    int GetPreviouslyPurchased(int flag1, int flag2);

    void _Reserved23();
    void _Reserved24();
    void _Reserved25();

    [PreserveSig]
    int IsSubscriptionFree();

    void _Reserved27();

    [PreserveSig]
    int GetSeasonPurchaseInfo(ESeasonPurchaseFlags flags);
}
