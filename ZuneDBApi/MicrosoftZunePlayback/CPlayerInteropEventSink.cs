using System.Runtime.InteropServices.Marshalling;

namespace MicrosoftZunePlayback;

// Original is a native C++ class implementing IMCPlayerEvents/IMCTransportEvents/
// IMCPlayerSetUriEvents via multiple inheritance (each base's vtable lives at a
// different byte offset within the object — offset 0/8/16 respectively, per the pointer
// adjustments `uEventSink`, `uEventSink + 8`, `uEventSink + 16` seen at the three
// Initialize() call sites in PlayerInterop's original decompiled body) plus a
// gcroot<PlayerInterop^> back-reference used to forward native callbacks into managed
// code, and a fourth field (offset 40) used only by ProgressivePlaybackReleaseFile/
// ReopenFile to stash a manual-reset event handle.
//
// None of that C++ ABI layout needs to be replicated here: `[GeneratedComClass]`'s
// ComWrappers-based CCW answers QueryInterface for every interface this class
// implements on its own, and the "extra" event handle field is just a plain field on
// PlayerInterop now (see its ProgressivePlaybackReleaseFile/ReopenFile). This class is
// purely a forwarding shim from the three native-callable interfaces to PlayerInterop's
// internal On* handlers.
[GeneratedComClass]
internal sealed unsafe partial class CPlayerInteropEventSink(PlayerInterop owner) : IMCPlayerEvents, IMCTransportEvents, IMCPlayerSetUriEvents
{
    public int OnStatusChanged(nint pStatus)
    {
        owner.OnStatusChanged((MCPlayerStatus*)pStatus);
        return 0;
    }

    public int OnAlertOccurred(nint pAlert)
    {
        owner.OnAlertOccurred((MCHResultAnnouncement*)pAlert);
        return 0;
    }

    public int OnPropertyChanged(string wzKey, nint pData, uint dataLength)
    {
        owner.OnPropertyChanged(wzKey, (byte*)pData, dataLength);
        return 0;
    }

    public int OnTransportStatusChanged(nint pTransportStatus)
    {
        owner.OnTransportStatusChanged((MCTransportStatus*)pTransportStatus);
        return 0;
    }

    public int OnTransportPositionChanged(long position, long minSeekPosition, long maxSeekPosition)
    {
        owner.OnTransportPositionChanged(position, minSeekPosition, maxSeekPosition);
        return 0;
    }

    public int OnUriSet(string wzUri, uint dwParam)
    {
        owner.OnUriSet(wzUri, dwParam);
        return 0;
    }
}
