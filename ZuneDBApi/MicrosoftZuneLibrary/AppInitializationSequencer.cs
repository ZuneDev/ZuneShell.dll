using ZuneUI;

namespace MicrosoftZuneLibrary;

// Original marshals a native "core phase 2 ready" callback onto the app thread. Not
// reverse engineered — see logs/MicrosoftZuneLibrary/ZuneLibrary.md. UIReady() and
// CorePhase2Ready() are never called by anything reachable without the native core
// this sequences against, so the callback the constructor stores is simply never
// invoked.
public class AppInitializationSequencer
{
    private CorePhase2ReadyCallback m_GcCorePhase2ReadyCallback;

    public AppInitializationSequencer(CorePhase2ReadyCallback corePhase2ReadyCallback)
    {
        m_GcCorePhase2ReadyCallback = corePhase2ReadyCallback;
    }

    public bool UIReady() => false;

    public void CorePhase2Ready(HRESULT hr)
    {
        m_GcCorePhase2ReadyCallback?.Invoke(hr.hr, hr.IsSuccess);
    }
}
