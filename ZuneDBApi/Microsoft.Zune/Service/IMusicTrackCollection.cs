using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Microsoft.Zune.Service;

// Native COM interface IMusicTrackCollection — vtable layout (x64, IUnknown = slots 0–2).
// Fully recovered from TrackOfferCollection.Init/GetCollection/!TrackOfferCollection's
// decompiled bodies in ZuneShell/lib/ZuneDBApi.dll — see
// logs/Microsoft.Zune/Service/OfferCollection.md.
//   [3]  GetCount() -> int
//   [4]  GetItem(int index, out MusicTrackMetadata, out IContextData*) -> HRESULT
[GeneratedComInterface]
[Guid("9e4b7d2a-3f1c-4e9b-8a6d-3f2c9b4e7d1a")]
internal partial interface IMusicTrackCollection
{
    [PreserveSig]
    int GetCount();

    [PreserveSig]
    int GetItem(int index, out MusicTrackMetadata metadata, out IContextData? context);
}
