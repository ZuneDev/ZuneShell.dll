#pragma once

#include "..\IMCPlayer.h"
#include "..\IMCTransport.h"
#include "..\IMCVolumeControl.h"
#include "..\IMCDynamicImage.h"
#include "..\IZuneSpectrumMgr.h"

class MockCWmpPlayer : public IMCPlayer, IMCPlayerSetUri, IMCDynamicImage,
    IMCTransport, IMCVolumeControl, IZuneSpectrumMgr
{
private:
    LONG m_refCount;

public:
    MockCWmpPlayer() : m_refCount(1) {}

    // Inherited via IUnknown
    HRESULT __stdcall QueryInterface(REFIID riid, void** ppvObject) override;
    ULONG __stdcall AddRef(void) override;
    ULONG __stdcall Release(void) override;

    // Inherited via IMCDynamicImage
    HRESULT Initialize(int) override;

    // Inherited via IMCPlayer
    HRESULT Initialize(HWND hWnd, unsigned int p1, IMCPlayerEvents* pEvents) override;

    // Inherited via IMCTransport
    HRESULT SetTransportEvents(IMCTransportEvents* events) override;
    HRESULT SetPositionEventInterval(unsigned int intervalMilliseconds) override;

    // Inherited via IMCPlayerSetUri
    HRESULT SetPlayerSetUriEvents(IMCPlayerSetUriEvents* events) override;
};

