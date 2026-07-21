using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace MicrosoftZunePlayback;

// Native COM event-sink interface IMCPlayerEvents. Unlike IMCPlayer/IMCTransport/etc,
// this interface is implemented BY our managed code (CPlayerInteropEventSink) and
// exposed to native code as a CCW, rather than consumed from a native pointer — so its
// GUID was never seen at a QueryInterface call site to recover. Placeholder GUID;
// see logs/MicrosoftZunePlayback/PlayerInterop.md.
//
// pStatus/pAlert/pData are kept as raw `nint` at the COM boundary (the safest,
// certainly-blittable parameter type for a `[GeneratedComInterface]` method) and cast to
// MCPlayerStatus*/MCHResultAnnouncement*/a payload-specific struct* immediately in
// CPlayerInteropEventSink — see those struct files for the recovered layouts.
[GeneratedComInterface]
[Guid("b2c3d4e5-f6a7-4b5c-9d0e-1f2a3b4c5d6e")]
internal partial interface IMCPlayerEvents
{
    [PreserveSig]
    int OnStatusChanged(nint pStatus);

    [PreserveSig]
    int OnAlertOccurred(nint pAlert);

    [PreserveSig]
    int OnPropertyChanged([MarshalAs(UnmanagedType.LPWStr)] string wzKey, nint pData, uint dataLength);
}
