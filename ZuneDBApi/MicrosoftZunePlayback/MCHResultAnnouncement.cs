using System.Runtime.InteropServices;

namespace MicrosoftZunePlayback;

// Native struct pointed to by MCPlayerStatus.FirstDenial and passed directly to
// IMCPlayerEvents.OnAlertOccurred. Layout recovered from the original decompiled
// PlayerInterop.MarshalAlert/HResultDenialsEqual's raw offset reads:
// id:wchar_t*@0, hResult:int@12, playbackId:int@16, sourceFile:wchar_t*@24,
// sourceLine:uint@32. The 4-byte gap between Id (offset 0-8) and HResult (offset 12) is
// never read by any managed call site anywhere in the original — its content is
// unknown, and no field is declared for it here rather than inventing one.
[StructLayout(LayoutKind.Explicit)]
internal readonly struct MCHResultAnnouncement
{
    [FieldOffset(0)]
    public readonly nint Id;

    [FieldOffset(12)]
    public readonly int HResult;

    [FieldOffset(16)]
    public readonly int PlaybackID;

    [FieldOffset(24)]
    public readonly nint SourceFile;

    [FieldOffset(32)]
    public readonly uint SourceLine;
}
