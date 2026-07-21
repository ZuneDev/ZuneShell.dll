using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace MicrosoftZunePlayback;

// Native COM interface IMCVolumeControl — vtable layout (x64, IUnknown = slots 0–2),
// fully recovered from PlayerInterop's original decompiled Play()/Volume setter/Mute
// setter. GUID recovered from ZuneDBApi.dll's
// <Module>._GUID_f6ba930c_78c3_488c_924d_2d3fc1e8fb70 static field.
[GeneratedComInterface]
[Guid("f6ba930c-78c3-488c-924d-2d3fc1e8fb70")]
internal partial interface IMCVolumeControl
{
    [PreserveSig]
    int SetVolume(int volume);

    [PreserveSig]
    int SetMute([MarshalAs(UnmanagedType.U1)] bool mute);
}
