using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Microsoft.Zune.Service;

// Native COM interface IVideoCollection — vtable layout (x64, IUnknown = slots 0–2).
// Fully recovered from VideoOfferCollection.Init/GetCollection/!VideoOfferCollection's
// decompiled bodies in ZuneShell/lib/ZuneDBApi.dll — see
// logs/Microsoft.Zune/Service/OfferCollection.md. (Init's *internal* control flow
// beyond the initial GetItem call is corrupted in ILSpy's output, but the interface
// shape itself — GetCount/GetItem — is identical to the other three collections and
// unambiguous.)
//   [3]  GetCount() -> int
//   [4]  GetItem(int index, out VideoMetadata, out IContextData*) -> HRESULT
[GeneratedComInterface]
[Guid("1f8c3a6e-4d2b-4a9f-8c1e-5d3a7b2f9c6e")]
internal partial interface IVideoCollection
{
    [PreserveSig]
    int GetCount();

    [PreserveSig]
    int GetItem(int index, out VideoMetadata metadata, out IContextData? context);
}
