using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Microsoft.Zune.Service;

// Native COM interface IMusicAlbumCollection — vtable layout (x64, IUnknown = slots 0–2).
// Fully recovered from AlbumOfferCollection.Init/GetCollection/!AlbumOfferCollection's
// decompiled bodies in ZuneShell/lib/ZuneDBApi.dll (raw `*(vtable + N)` call sites) —
// see logs/Microsoft.Zune/Service/OfferCollection.md. No slot beyond 4 is ever called;
// this is the interface's complete public surface.
//   [3]  GetCount() -> int
//   [4]  GetItem(int index, out MusicAlbumMetadata, out IContextData*) -> HRESULT
//
// No GUID recoverable — instances are only ever received as a factory out-param
// (from the native Service backend, not yet reverse engineered — see
// logs/Microsoft.Zune/Service/Service.md), never QueryInterface'd for by name.
[GeneratedComInterface]
[Guid("4a7c2e1f-8b3d-4f6a-9c1e-2d8b5a3f7c1e")]
internal partial interface IMusicAlbumCollection
{
    [PreserveSig]
    int GetCount();

    [PreserveSig]
    int GetItem(int index, out MusicAlbumMetadata metadata, out IContextData? context);
}
