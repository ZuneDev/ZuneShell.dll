using System.Runtime.InteropServices;

namespace MicrosoftZunePlayback;

// Native payload of an OnPropertyChanged("presentationinfo", ...) callback. Layout
// recovered from the original decompiled PlayerInterop.OnPropertyChanged's raw offset
// reads: aspectRatioX:float@0, aspectRatioY:float@4, nativeX:int@8, nativeY:int@12,
// needOverscan:bool@16.
[StructLayout(LayoutKind.Explicit)]
internal readonly struct PresentationInfoData
{
    [FieldOffset(0)]
    public readonly float AspectRatioX;

    [FieldOffset(4)]
    public readonly float AspectRatioY;

    [FieldOffset(8)]
    public readonly int NativeX;

    [FieldOffset(12)]
    public readonly int NativeY;

    [FieldOffset(16)]
    private readonly byte _needOverscan;

    public bool NeedOverscan => _needOverscan != 0;
}
