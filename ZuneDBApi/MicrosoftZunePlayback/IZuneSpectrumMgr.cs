using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace MicrosoftZunePlayback;

// Native COM interface IZuneSpectrumMgr — vtable layout (x64, IUnknown = slots 0–2),
// fully recovered from PlayerInterop's original decompiled
// Connect/DisconnectAnimationsToSpectrumAnalyzer. GUID recovered from ZuneDBApi.dll's
// <Module>._GUID_aff1732d_13f3_45e5_a52f_a854729e8730 static field.
[GeneratedComInterface]
[Guid("aff1732d-13f3-45e5-a52f-a854729e8730")]
internal partial interface IZuneSpectrumMgr
{
    [PreserveSig]
    int ConnectAnimationsToSpectrumAnalyzer(uint uixAnimationsId, uint bandCount, [MarshalAs(UnmanagedType.U1)] bool outputFrequencyData, [MarshalAs(UnmanagedType.U1)] bool outputWaveformData, [MarshalAs(UnmanagedType.U1)] bool enableStereoOutput);

    [PreserveSig]
    int DisconnectAnimationsFromSpectrumAnalyzer(uint uixAnimationsId);
}
