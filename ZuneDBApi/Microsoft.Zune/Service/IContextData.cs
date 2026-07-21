using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Microsoft.Zune.Service;

// Native COM interface IContextData — vtable layout (x64, IUnknown = slots 0–2):
//   [3]  GetContextString(out BSTR) -> HRESULT
//
// Fully recovered: OfferCollection.GetRecommendationContext's original body (ILSpy
// decompilation of MicrosoftZuneInterop... no — Microsoft.Zune.Service.OfferCollection
// in ZuneShell/lib/ZuneDBApi.dll) calls only this one slot, at vtable+24. No GUID is
// recoverable from managed metadata (same situation as IQueryPropertyBag — objects of
// this type are only ever received as `out` params from IMusicAlbumCollection::GetItem
// and friends, never QueryInterface'd for by IID).
[GeneratedComInterface]
[Guid("6b2b5a3d-1e0a-4f7a-9c3e-7a2b5d1f4e8a")]
internal partial interface IContextData
{
    [PreserveSig]
    int GetContextString([MarshalAs(UnmanagedType.BStr)] out string value);
}
