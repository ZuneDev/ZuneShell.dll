using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Microsoft.Zune.Service;

// Native COM interface IAppCollection — vtable layout (x64, IUnknown = slots 0–2).
// Fully recovered from AppOfferCollection.Init/GetCollection/!AppOfferCollection's
// decompiled bodies in ZuneShell/lib/ZuneDBApi.dll — see
// logs/Microsoft.Zune/Service/OfferCollection.md. Same shape as
// IMusicAlbumCollection/IMusicTrackCollection/IVideoCollection (all four collection
// interfaces share this GetCount/GetItem pattern); AppOfferCollection.Init happens to
// always pass null for the IContextData** out param, but the vtable slot and
// signature are identical.
//   [3]  GetCount() -> int
//   [4]  GetItem(int index, out AppMetadata, out IContextData*) -> HRESULT
[GeneratedComInterface]
[Guid("7d2f9a4c-1e6b-4c8a-9f3d-6b1a4e7c2f9d")]
internal partial interface IAppCollection
{
    [PreserveSig]
    int GetCount();

    [PreserveSig]
    int GetItem(int index, out AppMetadata metadata, out IContextData? context);
}
