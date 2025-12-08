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
    int Initialize(int) override;

    // Inherited via IMCPlayer
    int Initialize(HWND*, unsigned int, IMCPlayerEvents*) override;

    // Inherited via IMCTransport
    int SetTransportEvents(IMCTransportEvents* events) override;
    int SetPositionEventInterval(unsigned int intervalMilliseconds) override;

    // Inherited via IMCPlayerSetUri
    int SetPlayerSetUriEvents(IMCPlayerSetUriEvents* events) override;
};

