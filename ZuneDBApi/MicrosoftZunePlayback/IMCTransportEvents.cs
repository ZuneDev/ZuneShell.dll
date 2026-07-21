using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace MicrosoftZunePlayback;

// Native COM event-sink interface IMCTransportEvents — implemented BY our managed code
// (CPlayerInteropEventSink), same situation as IMCPlayerEvents (see that file's header
// comment). Placeholder GUID; see logs/MicrosoftZunePlayback/PlayerInterop.md.
//
// pTransportStatus is kept as raw `nint` at the COM boundary (the safest, certainly-
// blittable parameter type for a `[GeneratedComInterface]` method) and cast to
// MCTransportStatus* immediately in CPlayerInteropEventSink — see MCTransportStatus.cs
// for the recovered layout.
[GeneratedComInterface]
[Guid("c3d4e5f6-a7b8-4c5d-9e0f-1a2b3c4d5e6f")]
internal partial interface IMCTransportEvents
{
    [PreserveSig]
    int OnTransportStatusChanged(nint pTransportStatus);

    [PreserveSig]
    int OnTransportPositionChanged(long position, long minSeekPosition, long maxSeekPosition);
}
