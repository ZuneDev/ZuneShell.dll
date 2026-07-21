using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace MicrosoftZunePlayback;

// Native COM interface IMCPlayerSetUri — vtable layout (x64, IUnknown = slots 0–2),
// fully recovered from PlayerInterop's original decompiled SetUri/SetNextUri/CancelNext.
// GUID recovered from ZuneDBApi.dll's
// <Module>._GUID_58864c93_45f9_4c6d_aa3f_80f6caa08281 static field.
[GeneratedComInterface]
[Guid("58864c93-45f9-4c6d-aa3f-80f6caa08281")]
internal partial interface IMCPlayerSetUri
{
    [PreserveSig]
    int Initialize(IMCPlayerSetUriEvents? pEvents);

    [PreserveSig]
    int SetUri([MarshalAs(UnmanagedType.LPWStr)] string uri, long startPositionIn100nsUnits, uint uriId);

    [PreserveSig]
    int SetNextUri([MarshalAs(UnmanagedType.LPWStr)] string uri, long startPositionIn100nsUnits, uint uriId);

    [PreserveSig]
    int CancelNext();
}
