using System.Runtime.InteropServices;

namespace MicrosoftZunePlayback;

// Native struct passed to IMCTransportEvents.OnTransportStatusChanged. Layout recovered
// from the original decompiled PlayerInterop.OnTransportStatusChanged's raw offset
// reads: state:int@0, rate:float@4, endOfMedia:bool@8, canSeek:bool@9.
[StructLayout(LayoutKind.Explicit)]
internal readonly struct MCTransportStatus
{
    [FieldOffset(0)]
    public readonly MCTransportState State;

    [FieldOffset(4)]
    public readonly float Rate;

    [FieldOffset(8)]
    private readonly byte _endOfMedia;

    [FieldOffset(9)]
    private readonly byte _canSeek;

    public bool EndOfMedia => _endOfMedia != 0;

    public bool CanSeek => _canSeek != 0;
}
