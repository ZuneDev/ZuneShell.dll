using System.Runtime.InteropServices;

namespace MicrosoftZunePlayback;

// Native struct passed to IMCPlayerEvents.OnStatusChanged. Layout recovered from the
// original decompiled PlayerInterop.OnStatusChanged's raw offset reads:
// state:int@0, duration:long@8 (4 bytes of padding before it for 8-byte alignment),
// isReady:bool@16, firstDenial:MCHResultAnnouncement*@24 (7 bytes of padding before it).
[StructLayout(LayoutKind.Explicit)]
internal readonly struct MCPlayerStatus
{
    [FieldOffset(0)]
    public readonly int State;

    [FieldOffset(8)]
    public readonly long Duration;

    [FieldOffset(16)]
    private readonly byte _isReady;

    [FieldOffset(24)]
    public readonly nint FirstDenial;

    public bool IsReady => _isReady != 0;
}
