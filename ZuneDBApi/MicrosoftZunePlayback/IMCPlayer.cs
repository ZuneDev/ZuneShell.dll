using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace MicrosoftZunePlayback;

// Native COM interface IMCPlayer — vtable layout (x64, IUnknown = slots 0–2):
//   [3]  Initialize(HWND hwnd, uint flags, IMCPlayerEvents* pEvents) -> HRESULT
//   [4]  Uninitialize() -> HRESULT
//   [5]  Close() -> HRESULT
//   [6]  unknown — never observed at any managed call site
//   [7]  unknown — never observed at any managed call site
//   [8]  unknown — called once, with just `this`, at the very end of the managed
//        Uninitialize() sequence (after the native state machine has already reported
//        MCPlayerState.Uninitialized via the event sink), immediately before every
//        sub-interface pointer is released. Purpose unconfirmed.
//
// Sub-interfaces (IMCTransport, IMCPlayerSetUri, IMCDynamicImage, IMCVolumeControl,
// IZuneSpectrumMgr) are obtained via QueryInterface on this object using their own
// GUIDs (recovered from ZuneDBApi.dll's <Module> static GUID fields — see each
// interface's own file). With `[GeneratedComInterface]`, casting a ComWrappers-wrapped
// IMCPlayer instance to any of those interface types performs that QueryInterface
// automatically.
//
// TODO: no GUID for IMCPlayer itself was recovered — the original never QueryInterface's
// for it; it's returned already-typed from the native CWmpPlayer_GetInstance factory.
// The GUID below is a placeholder. See logs/MicrosoftZunePlayback/PlayerInterop.md.
[GeneratedComInterface]
[Guid("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d")]
internal partial interface IMCPlayer
{
    [PreserveSig]
    int Initialize(nint hwnd, uint flags, IMCPlayerEvents? pEvents);

    [PreserveSig]
    int Uninitialize();

    [PreserveSig]
    int Close();

    void _Reserved6();
    void _Reserved7();

    [PreserveSig]
    int _Reserved8();
}
