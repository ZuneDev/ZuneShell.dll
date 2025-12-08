#pragma once
#include "IMCPlayer.h"
#include "IMCTransport.h"
#include "CPlayerInteropEventSink.h"

using namespace System;
using namespace System::Collections::Generic;

namespace MicrosoftZunePlayback
{
    public ref class PlayerInterop : IDisposable
    {
    private:
        IMCPlayer* _uPlayer = 0;
        IMCTransport* _uTransport = 0;
        IMCPlayerSetUri* _uSetUri = 0;
        IMCDynamicImage* _uDynamicImage = 0;
        IMCVolumeControl* _uVolumeControl;
        IZuneSpectrumMgr* _uZuneSpectrumMgr = 0;
        CPlayerInteropEventSink* _uEventSink;
        //MCPlayerState _state = MCPlayerState.Uninitialized;
        long _duration = 0L;
        bool _isReady = false;
        //Announcement _firstDenial;
        String^ _currentUri;
        int _currentUriID;
        Dictionary<String^, Object^>^ _properties;
        //MCTransportState _transportState = MCTransportState.Invalid;
        float _rate = 1.0;
        bool _endOfMedia = false;
        long _position = 0L;
        long _minSeekPosition = 0L;
        long _maxSeekPosition = 0L;
        bool _resetOnStop = false;
        TimeSpan _positionEventInterval = TimeSpan::FromMilliseconds(100.0);
        int _nDynamicImage = 0;
        int _volume;
        bool _mute;
        bool _canChangeVideoRate = false;
        bool _canSeek = true;
        IntPtr _windowHandle;
        //VideoWindow _windowHost;
    public:
        static PlayerInterop^ _singletonInstance = nullptr;
    private:
        void* _gotStateCloseEvent;
        void* _gotStateUninitializeEvent;
        volatile bool _fShuttingDown;

    public:
        //event PlayerPropertyChangedEventHandler^ PlayerPropertyChanged;
        //event PlayerBandwithUpdateEventHandler^ PlayerBandwithUpdate;
        event EventHandler^ StatusChanged;
        //event AnnouncementHandler^ AlertSent;
        event EventHandler^ TransportStatusChanged;
        event EventHandler^ TransportPositionChanged;
        event EventHandler^ UriSet;

        static property PlayerInterop^ Instance
        {
            PlayerInterop^ get()
            {
                if (!_singletonInstance)
                {
                    _singletonInstance = gcnew PlayerInterop();
                }

                return _singletonInstance;
            }
        }

        property int CurrentUriID
        {
            int get() { return _currentUriID; }
        }

        property String^ CurrentUri
        {
            String^ get() { return _currentUri; }
        }

        property TimeSpan PositionEventInterval
        {
            TimeSpan get() { return _positionEventInterval; }
            void set(TimeSpan value)
            {
                if (_uTransport != 0)
                {
                    _positionEventInterval = value;
                    long num = *(long*)_uTransport + 96;

                    typedef int (*IMCTransport_SetPositionEventInterval)(IMCTransport* x, unsigned int y);
                    ((IMCTransport_SetPositionEventInterval)(*(unsigned long*)num))(_uTransport, (unsigned int)value.TotalMilliseconds);
                }
            }
        }

    private:
        PlayerInterop();
        ~PlayerInterop();

    public:
        void Initialize();
        void Uninitialize();
    };
}

