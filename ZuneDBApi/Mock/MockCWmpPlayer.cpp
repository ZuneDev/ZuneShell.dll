#include "pch.h"
#include "MockCWmpPlayer.h"

HRESULT __stdcall MockCWmpPlayer::QueryInterface(REFIID riid, void** ppvObject)
{
    if (riid == IID_IUnknown
        /*|| riid == IID_IMCPlayer*/
        || riid == IID_IMCPlayerSetUri
        || riid == IID_IMCDynamicImage
        || riid == IID_IMCTransport
        || riid == IID_IMCVolumeControl
        || riid == IID_IZuneSpectrumMgr) {
        *ppvObject = static_cast<IMCPlayer*>(this);
        AddRef();
        return S_OK;
    }
    *ppvObject = nullptr;
    return E_NOINTERFACE;
}

ULONG __stdcall MockCWmpPlayer::AddRef(void)
{
    return InterlockedIncrement(&m_refCount);
}

ULONG __stdcall MockCWmpPlayer::Release(void)
{
    ULONG count = InterlockedDecrement(&m_refCount);
    if (count == 0) {
        delete this;
    }
    return count;
}

int MockCWmpPlayer::Initialize(int n)
{
    return 0;
}

int MockCWmpPlayer::Initialize(HWND* pHWND, unsigned int p1, IMCPlayerEvents* pEvents)
{
    return 0;
}

int MockCWmpPlayer::SetTransportEvents(IMCTransportEvents* events)
{
    return 0;
}

int MockCWmpPlayer::SetPositionEventInterval(unsigned int intervalMilliseconds)
{
    return 0;
}

int MockCWmpPlayer::SetPlayerSetUriEvents(IMCPlayerSetUriEvents* events)
{
    return 0;
}
