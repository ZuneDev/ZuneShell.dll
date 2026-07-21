using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Microsoft.Zune.Service;

// Native COM interface IBillingOfferCollection — vtable layout (x64, IUnknown = slots 0–2).
// Fully recovered from BillingOfferCollection.Init/!BillingOfferCollection's decompiled
// bodies in ZuneShell/lib/ZuneDBApi.dll — see logs/Microsoft.Zune/Service/OfferCollection.md.
// Unlike the four media collections, GetItem here returns flat scalar/BSTR fields
// directly instead of a native metadata struct — matching BillingOffer's flat public
// shape (Id/OfferType/OfferName/DisplayPrice/Points/Price/Taxes/Trial).
//   [3]  GetCount() -> int
//   [4]  GetItem(int index, out ulong id, out EBillingOfferType, out BSTR name,
//                out BSTR displayPrice, out uint points, out float price,
//                out int taxesIncluded, out int isTrial) -> HRESULT
[GeneratedComInterface]
[Guid("5c8a3f1e-2b7d-4a9c-9e1f-3b6d8a2c5f7e")]
internal partial interface IBillingOfferCollection
{
    [PreserveSig]
    int GetCount();

    [PreserveSig]
    int GetItem(
        int index,
        out ulong id,
        out EBillingOfferType offerType,
        [MarshalAs(UnmanagedType.BStr)] out string offerName,
        [MarshalAs(UnmanagedType.BStr)] out string displayPrice,
        out uint points,
        out float price,
        out int taxesIncluded,
        out int isTrial);
}
