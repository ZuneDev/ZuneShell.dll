#include "pch.h"
#include "PlayerInterop.h"
#include ".\Mock\MockCWmpPlayer.h"

#include "zunecore.h"

#define WIN32_LEAN_AND_MEAN
#include "windows.h"
#include "synchapi.h"

using namespace System::Runtime::InteropServices;

MicrosoftZunePlayback::PlayerInterop::PlayerInterop()
{
    _windowHandle = IntPtr::Zero;
    //_windowHost = nullptr;
    _gotStateCloseEvent = nullptr;
    _gotStateUninitializeEvent = nullptr;
    _fShuttingDown = false;
}

MicrosoftZunePlayback::PlayerInterop::~PlayerInterop()
{
    //_firstDenial = nullptr;
    Uninitialize();
}

void MicrosoftZunePlayback::PlayerInterop::Initialize()
{
    Initialize_ZuneCore();

    IMCPlayer* ptr = nullptr;
    IMCTransport* ptr2 = nullptr;
    IMCPlayerSetUri* ptr3 = nullptr;
    IMCDynamicImage* ptr4 = nullptr;
    IMCVolumeControl* ptr5 = nullptr;
    IZuneSpectrumMgr* ptr6 = nullptr;
    int num = 0;

    _gotStateCloseEvent = CreateEventW(NULL, 0, 0, NULL);
    if (_gotStateCloseEvent == NULL)
    {
        throw gcnew COMException("PlayerInterop failed to CreateEvent for close", 0);
    }

    _gotStateUninitializeEvent = CreateEventW(NULL, 0, 0, NULL);
    if (_gotStateUninitializeEvent == NULL)
    {
        throw gcnew COMException("PlayerInterop failed to CreateEvent for uninit", 0);
    }

    //num = WmpCoreInitialize();
    if (num < 0)
    {
        throw gcnew COMException("Playback core initialization failed", num);
    }

    ptr = new MockCWmpPlayer;
    /*num = CWmpPlayer_GetInstance(&ptr);*/
    if (num < 0 || ptr == nullptr)
    {
        throw gcnew COMException("PlayerInterop failed to get IMCPlayer instance", num);
    }
    _uPlayer = ptr;

    num = _uPlayer->QueryInterface(IID_IMCTransport, (void**)&ptr2);
    if (num < 0 || ptr2 == nullptr)
    {
        throw gcnew COMException("PlayerInterop failed to get IMCTransport instance", num);
    }
    _uTransport = ptr2;

    num = _uPlayer->QueryInterface(IID_IMCPlayerSetUri, (void**)&ptr3);
    if (num < 0 || ptr3 == nullptr)
    {
        throw gcnew COMException("PlayerInterop failed to get IMCPlayerSetUri instance", num);
    }
    _uSetUri = ptr3;

    num = _uPlayer->QueryInterface(IID_IMCDynamicImage, (void**)&ptr4);
    if (num < 0 || ptr4 == nullptr)
    {
        throw gcnew COMException("PlayerInterop failed to get IMCDynamicImage instance", num);
    }
    _uDynamicImage = ptr4;

    num = _uDynamicImage->Initialize(_nDynamicImage);
    if (num < 0)
    {
        throw gcnew COMException("PlayerInterop failed to initialize IMCDynamicImage", num);
    }

    num = _uPlayer->QueryInterface(IID_IMCVolumeControl, (void**)&ptr5);
    if (num < 0 || ptr5 == nullptr)
    {
        throw gcnew COMException("PlayerInterop failed to get IMCVolumeControl instance", num);
    }
    _uVolumeControl = ptr5;

    num = _uPlayer->QueryInterface(IID_IZuneSpectrumMgr, (void**)&ptr6);
    if (num < 0 || ptr6 == nullptr)
    {
        throw gcnew COMException("PlayerInterop failed to get IZuneSpectrumMgr instance", num);
    }
    _uZuneSpectrumMgr = ptr6;

    _uEventSink = new CPlayerInteropEventSink();

    IMCPlayerEvents* ptr8 = _uEventSink->playerEvents;
    num = _uPlayer->Initialize((HWND*)_windowHandle.ToPointer(), 0, ptr8);
    if (num < 0)
    {
        throw gcnew COMException("PlayerInterop failed to initialize IMCPlayer", num);
    }

    IMCTransportEvents* ptr9 = _uEventSink->transportEvents;
    num = _uTransport->SetTransportEvents(ptr9);
    if (num < 0)
    {
        throw gcnew COMException("PlayerInterop failed to initialize IMCTransport", num);
    }

    IMCPlayerSetUriEvents* ptr10 = _uEventSink->playerSetUriEvents;
    num = _uSetUri->SetPlayerSetUriEvents(ptr10);
    if (num < 0)
    {
        throw gcnew COMException("PlayerInterop failed to initialize IMCPlayerSetUri", num);
    }

    num = _uTransport->SetPositionEventInterval((unsigned int)_positionEventInterval.TotalMilliseconds);
    if (num < 0)
    {
        throw gcnew COMException("Attempt to set initial position event firing interval failed.", num);
    }

    _properties = gcnew Dictionary<String^, Object^>();
}

void MicrosoftZunePlayback::PlayerInterop::Uninitialize()
{
    bool flag = false;

    if (_uPlayer != NULL)
    {
        _fShuttingDown = true;
    }
}
