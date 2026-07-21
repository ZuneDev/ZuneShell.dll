using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace MicrosoftZunePlayback;

// Native COM interface IMCTransport — vtable layout (x64, IUnknown = slots 0–2), fully
// recovered from PlayerInterop's original decompiled call sites (Play/Pause/Stop/Seek*/
// Rate setter/ResetOnStop setter/PositionEventInterval setter/ProgressivePlaybackRelease
// and ReopenFile). GUID recovered from ZuneDBApi.dll's
// <Module>._GUID_2f33a725_95cb_4080_adef_93a067a707ba static field (used as the
// QueryInterface argument in PlayerInterop's original Initialize()).
[GeneratedComInterface]
[Guid("2f33a725-95cb-4080-adef-93a067a707ba")]
internal partial interface IMCTransport
{
    [PreserveSig]
    int Initialize(IMCTransportEvents? pEvents);

    [PreserveSig]
    int Pause();

    [PreserveSig]
    int Play();

    [PreserveSig]
    int Stop();

    [PreserveSig]
    int SeekToRelativePosition(long offsetIn100nsUnits);

    [PreserveSig]
    int SeekToAbsolutePosition(long offsetIn100nsUnits);

    [PreserveSig]
    int SeekToRelativeFrame(long offsetInFrames);

    [PreserveSig]
    int SetRate(float rate);

    [PreserveSig]
    int SetResetOnStop([MarshalAs(UnmanagedType.U1)] bool resetOnStop);

    [PreserveSig]
    int SetPositionEventInterval(uint milliseconds);

    [PreserveSig]
    int ReleaseFile();

    [PreserveSig]
    int ReopenFile();
}
