using System.Runtime.InteropServices;

namespace MicrosoftZunePlayback;

// Native payload of an OnPropertyChanged("volumeinfo", ...) callback. Layout recovered
// from the original decompiled PlayerInterop.OnPropertyChanged's raw offset reads:
// volume:int@0, mute:int@4 (a 4-byte BOOL, not a 1-byte bool, per the original's
// `*(int*)(pData + 4) != 0` check).
[StructLayout(LayoutKind.Explicit)]
internal readonly struct VolumeInfoData
{
    [FieldOffset(0)]
    public readonly int Volume;

    [FieldOffset(4)]
    private readonly int _mute;

    public bool Mute => _mute != 0;
}
