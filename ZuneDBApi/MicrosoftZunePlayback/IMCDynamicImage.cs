using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace MicrosoftZunePlayback;

// Native COM interface IMCDynamicImage — vtable layout (x64, IUnknown = slots 0–2),
// fully recovered from PlayerInterop's original decompiled Initialize/VideoPosition
// setter/ShowGDIVideo setter. GUID recovered from ZuneDBApi.dll's
// <Module>._GUID_102e281e_28ad_4688_aaff_f560f8053d90 static field.
[GeneratedComInterface]
[Guid("102e281e-28ad-4688-aaff-f560f8053d90")]
internal partial interface IMCDynamicImage
{
    [PreserveSig]
    int Initialize(int dynamicImage);

    [PreserveSig]
    int SetVideoPosition(RECT position);

    [PreserveSig]
    int SetShowGDIVideo([MarshalAs(UnmanagedType.U1)] bool show);
}
