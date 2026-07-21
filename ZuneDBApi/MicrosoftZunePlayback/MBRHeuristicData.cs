using System.Runtime.InteropServices;

namespace MicrosoftZunePlayback;

// Native payload of an OnPropertyChanged("mbrheuristicsdata", ...) callback — the
// original's decompiled body names the native type `_MBRHEURISTICDATA` (a global,
// native-only struct also visible elsewhere in ZuneDBApi.dll's metadata) and copies 48
// bytes of it via `Unsafe.CopyBlock`. Field layout recovered from how those 48 bytes are
// unpacked into BandwidthUpdateArgs's constructor arguments, in order:
// buffering:long@0, quality:float@8, id:int@12, latestBandwidth:int@16,
// recentAverageBandwidth:int@20, totalAverageBandwidth:int@24, bitrate:int@28,
// droppedFrames:int@32, testLength:int@36, testPosition:int@40, currentState:int@44.
[StructLayout(LayoutKind.Explicit, Size = 48)]
internal readonly struct MBRHeuristicData
{
    [FieldOffset(0)]
    public readonly long Buffering;

    [FieldOffset(8)]
    public readonly float Quality;

    [FieldOffset(12)]
    public readonly int Id;

    [FieldOffset(16)]
    public readonly int LatestBandwidth;

    [FieldOffset(20)]
    public readonly int RecentAverageBandwidth;

    [FieldOffset(24)]
    public readonly int TotalAverageBandwidth;

    [FieldOffset(28)]
    public readonly int Bitrate;

    [FieldOffset(32)]
    public readonly int DroppedFrames;

    [FieldOffset(36)]
    public readonly int TestLength;

    [FieldOffset(40)]
    public readonly int TestPosition;

    [FieldOffset(44)]
    public readonly MBRHeuristicState CurrentState;
}
