using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace MicrosoftZunePlayback;

// Native COM event-sink interface IMCPlayerSetUriEvents — implemented BY our managed
// code (CPlayerInteropEventSink), same situation as IMCPlayerEvents (see that file's
// header comment). Placeholder GUID; see logs/MicrosoftZunePlayback/PlayerInterop.md.
[GeneratedComInterface]
[Guid("d4e5f6a7-b8c9-4d5e-9f0a-1b2c3d4e5f6a")]
internal partial interface IMCPlayerSetUriEvents
{
    [PreserveSig]
    int OnUriSet([MarshalAs(UnmanagedType.LPWStr)] string wzUri, uint dwParam);
}
