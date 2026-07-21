using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Threading;

namespace MicrosoftZunePlayback;

// See logs/MicrosoftZunePlayback/PlayerInterop.md for the full decompilation/recovery
// notes. Summary: the original wraps six native COM interfaces (IMCPlayer, IMCTransport,
// IMCPlayerSetUri, IMCDynamicImage, IMCVolumeControl, IZuneSpectrumMgr — all now fully
// reconstructed as `[GeneratedComInterface]`s in this folder, with real GUIDs recovered
// from ZuneDBApi.dll's <Module> static fields for everything except IMCPlayer itself)
// plus a native CPlayerInteropEventSink callback object (now CPlayerInteropEventSink.cs,
// a managed `[GeneratedComClass]` implementing the corresponding IMCPlayerEvents/
// IMCTransportEvents/IMCPlayerSetUriEvents interfaces). Every method below now performs
// the real COM calls the original did.
//
// The one thing that could NOT be recovered is how to obtain a live IMCPlayer instance
// in the first place: the original calls two internal native entry points
// (WmpCoreInitialize() and CWmpPlayer_GetInstance(IMCPlayer**)) that are native code
// statically linked into the original ZuneShell/lib/ZuneDBApi.dll — not P/Invokable
// exports, and this project doesn't ship (or reimplement) that native playback engine.
// See the TODO in Initialize() below.
public class PlayerInterop : IDisposable
{
    private static readonly StrategyBasedComWrappers s_comWrappers = new();

    private static PlayerInterop? _singletonInstance;

    public static PlayerInterop Instance => _singletonInstance ??= new PlayerInterop();

    private IMCPlayer? _player;
    private IMCTransport? _transport;
    private IMCPlayerSetUri? _setUri;
    private IMCDynamicImage? _dynamicImage;
    private IMCVolumeControl? _volumeControl;
    private IZuneSpectrumMgr? _spectrumMgr;
    private CPlayerInteropEventSink? _eventSink;

    private MCPlayerState _state = MCPlayerState.Uninitialized;
    private MCTransportState _transportState = MCTransportState.Invalid;
    private long _duration;
    private bool _isReady;
    private Announcement? _firstDenial;
    private string? _currentUri;
    private int _currentUriID;
    private Dictionary<string, object?> _properties = new();
    private float _rate = 1f;
    private bool _endOfMedia;
    private long _position;
    private long _minSeekPosition;
    private long _maxSeekPosition;
    private bool _resetOnStop;
    private TimeSpan _positionEventInterval = TimeSpan.FromMilliseconds(100.0);
    private int _nDynamicImage;
    private int _volume;
    private bool _mute;
    private bool _canChangeVideoRate;
    private bool _canSeek = true;
    private nint _windowHandle;
    private VideoWindow? _windowHost;
    private volatile bool _fShuttingDown;

    private ManualResetEventSlim? _gotStateCloseEvent;
    private ManualResetEventSlim? _gotStateUninitializeEvent;
    private ManualResetEventSlim? _progressivePlaybackReleaseEvent;

    public int CurrentUriID => _currentUriID;

    public string? CurrentUri => _currentUri;

    public TimeSpan PositionEventInterval
    {
        get => _positionEventInterval;
        set
        {
            if (_transport is not null)
            {
                _positionEventInterval = value;
                _transport.SetPositionEventInterval((uint)value.TotalMilliseconds);
            }
        }
    }

    public bool ResetOnStop
    {
        get => _resetOnStop;
        set
        {
            if (_transport is not null)
            {
                _resetOnStop = value;
                _transport.SetResetOnStop(value);
            }
        }
    }

    public long MaxSeekPosition => _maxSeekPosition;

    public long MinSeekPosition => _minSeekPosition;

    public long Position => _position;

    public bool EndOfMedia => _endOfMedia;

    // Native application of a new rate happens asynchronously — this mirrors the
    // original, which doesn't update `_rate` here; OnTransportStatusChanged does once
    // the native transport reports the change actually took effect.
    public float Rate
    {
        get => _rate;
        set => _transport?.SetRate(value);
    }

    public MCTransportState TransportState => _transportState;

    public bool ShowGDIVideo
    {
        set => _dynamicImage?.SetShowGDIVideo(value);
    }

    public VideoWindow? VideoPosition
    {
        set
        {
            _windowHost = value;
            if (value is not null)
                _dynamicImage?.SetVideoPosition(new RECT(value.Left, value.Top, value.Right, value.Bottom));
        }
    }

    public IntPtr WindowHandle
    {
        get => _windowHandle;
        set => _windowHandle = value;
    }

    public bool CanSeek => _canSeek;

    public bool CanChangeVideoRate => _canChangeVideoRate;

    public bool Mute
    {
        get => _mute;
        set
        {
            _mute = value;
            _volumeControl?.SetMute(value);
        }
    }

    public int Volume
    {
        get => _volume;
        set
        {
            _volume = value;
            if (!_mute)
                _volumeControl?.SetVolume(value);
        }
    }

    // Only applied the next time Initialize() runs — matches the original, which never
    // forwards this setter to the native IMCDynamicImage after the first Initialize().
    public int DynamicImage
    {
        get => _nDynamicImage;
        set => _nDynamicImage = value;
    }

    public Announcement? FirstDenial => _firstDenial;

    public bool IsReady => _isReady;

    public long Duration => _duration;

    public MCPlayerState State => _state;

    public event EventHandler? UriSet;
    public event EventHandler? TransportPositionChanged;
    public event EventHandler? TransportStatusChanged;
    public event AnnouncementHandler? AlertSent;
    public event EventHandler? StatusChanged;
    public event PlayerBandwithUpdateEventHandler? PlayerBandwithUpdate;
    public event PlayerPropertyChangedEventHandler? PlayerPropertyChanged;

    private PlayerInterop()
    {
    }

    public void Initialize()
    {
        _gotStateCloseEvent = new ManualResetEventSlim(false);
        _gotStateUninitializeEvent = new ManualResetEventSlim(false);

        // TODO: the original calls two native entry points here — WmpCoreInitialize()
        // and CWmpPlayer_GetInstance(IMCPlayer** ppPlayer) — that are internal, native
        // code statically linked into the original ZuneShell/lib/ZuneDBApi.dll. Neither
        // is a P/Invokable export, and neither has been reimplemented: there is no
        // playback engine backing this project yet. See
        // logs/MicrosoftZunePlayback/PlayerInterop.md. Once a real factory exists,
        // obtain its IMCPlayer pointer here — everything below is real COM interop
        // against the reconstructed interfaces and needs no further changes.
        nint playerPtr = 0;
        if (playerPtr == 0)
        {
            // No native engine available: reach a "soft" ready state so callers that
            // don't depend on actual playback keep working — every other method below
            // already no-ops when its sub-interface is null, exactly like the original
            // did when a given QueryInterface failed.
            _state = MCPlayerState.Open;
            _isReady = true;
            _properties = new Dictionary<string, object?>();
            return;
        }

        _player = (IMCPlayer)s_comWrappers.GetOrCreateObjectForComInstance(playerPtr, CreateObjectFlags.None);
        _transport = (IMCTransport)_player;
        _setUri = (IMCPlayerSetUri)_player;
        _dynamicImage = (IMCDynamicImage)_player;
        _volumeControl = (IMCVolumeControl)_player;
        _spectrumMgr = (IZuneSpectrumMgr)_player;

        int hr = _dynamicImage.Initialize(_nDynamicImage);
        if (hr < 0)
            throw new COMException("PlayerInterop failed to initialize IMCDynamicImage", hr);

        _eventSink = new CPlayerInteropEventSink(this);

        hr = _player.Initialize(_windowHandle, 0, (IMCPlayerEvents)_eventSink);
        if (hr < 0)
            throw new COMException("PlayerInterop failed to initialize IMCPlayer", hr);

        hr = _transport.Initialize((IMCTransportEvents)_eventSink);
        if (hr < 0)
            throw new COMException("PlayerInterop failed to initialize IMCTransport", hr);

        hr = _setUri.Initialize((IMCPlayerSetUriEvents)_eventSink);
        if (hr < 0)
            throw new COMException("PlayerInterop failed to initialize IMCPlayerSetUri", hr);

        const int E_NOTIMPL = unchecked((int)0x80004001);
        hr = _transport.SetPositionEventInterval((uint)_positionEventInterval.TotalMilliseconds);
        if (hr < 0 && hr != E_NOTIMPL)
            throw new COMException("Attempt to set initial position event firing interval failed.", hr);

        _properties = new Dictionary<string, object?>();
    }

    public void Uninitialize()
    {
        bool reachedUninitialized = true;
        if (_player is not null)
        {
            _fShuttingDown = true;
            _player.Close();
            _gotStateCloseEvent!.Wait(5000);
            _player.Uninitialize();
            reachedUninitialized = _gotStateUninitializeEvent!.Wait(5000) && _state == MCPlayerState.Uninitialized;
            Microsoft.Zune.Util.ShipAssert.AssertId(reachedUninitialized, 80000u, 0u);
            _ = _player._Reserved8();
        }

        _gotStateCloseEvent?.Dispose();
        _gotStateCloseEvent = null;
        _gotStateUninitializeEvent?.Dispose();
        _gotStateUninitializeEvent = null;

        if (reachedUninitialized)
        {
            _eventSink = null;
            _player = null;
            _transport = null;
            _setUri = null;
            _dynamicImage = null;
            _volumeControl = null;
            _spectrumMgr = null;
            // TODO: WmpCoreDeinitialize() — see Initialize()'s TODO.
        }
    }

    public void Close()
    {
        _player?.Close();
    }

    public void Play()
    {
        if (_volumeControl is not null && _transport is not null)
        {
            if (_mute)
                _volumeControl.SetMute(true);
            else
                _volumeControl.SetVolume(_volume);
            _transport.Play();
        }
    }

    public void Pause()
    {
        _transport?.Pause();
    }

    public void Stop()
    {
        _transport?.Stop();
    }

    public void SeekToRelativePosition(long offsetIn100nsUnits)
    {
        _transport?.SeekToRelativePosition(offsetIn100nsUnits);
    }

    public void SeekToAbsolutePosition(long offsetIn100nsUnits)
    {
        _transport?.SeekToAbsolutePosition(offsetIn100nsUnits);
    }

    public void SeekToRelativeFrame(long offsetInFrames)
    {
        _transport?.SeekToRelativeFrame(offsetInFrames);
    }

    public void SetUri(string uri, long startPositionIn100nsUnits, int uriID)
    {
        if (uri is null)
            return;
        _setUri?.SetUri(uri, startPositionIn100nsUnits, (uint)uriID);
    }

    public void SetNextUri(string uri, long startPositionIn100nsUnits, int uriID)
    {
        if (uri is null)
            return;
        _setUri?.SetNextUri(uri, startPositionIn100nsUnits, (uint)uriID);
    }

    public void CancelNext()
    {
        _setUri?.CancelNext();
    }

    public void ConnectAnimationsToSpectrumAnalyzer(uint uixAnimationsId, uint bandCount, bool outputFrequencyData, bool outputWaveformData, bool enableStereoOutput)
    {
        _spectrumMgr?.ConnectAnimationsToSpectrumAnalyzer(uixAnimationsId, bandCount, outputFrequencyData, outputWaveformData, enableStereoOutput);
    }

    public void DisconnectAnimationsFromSpectrumAnalyzer(uint uixAnimationsId)
    {
        _spectrumMgr?.DisconnectAnimationsFromSpectrumAnalyzer(uixAnimationsId);
    }

    // NOTE: nothing observed in the original decompiled PlayerInterop ever signals the
    // event this waits on — it must happen inside native code this project can't see
    // (there's no visible SetEvent call anywhere in the decompiled class). Preserved
    // faithfully anyway: with no live _transport (see Initialize()'s TODO) this method
    // is unreachable in practice today, so the unresolved wait never actually executes.
    public void ProgressivePlaybackReleaseFile()
    {
        if (_transport is null)
            return;
        if (_progressivePlaybackReleaseEvent is null)
        {
            _progressivePlaybackReleaseEvent = new ManualResetEventSlim(false);
            _transport.ReleaseFile();
            bool signaled = _progressivePlaybackReleaseEvent.Wait(30000);
            Microsoft.Zune.Util.ShipAssert.AssertId(signaled, 80001u, 0u);
        }
    }

    public void ProgressivePlaybackReopenFile()
    {
        if (_transport is null || _progressivePlaybackReleaseEvent is null)
            return;
        _transport.ReopenFile();
        _progressivePlaybackReleaseEvent.Dispose();
        _progressivePlaybackReleaseEvent = null;
    }

    internal unsafe void OnStatusChanged(MCPlayerStatus* pStatus)
    {
        bool changed = false;

        if (_state != (MCPlayerState)pStatus->State)
        {
            _state = (MCPlayerState)pStatus->State;
            changed = true;
        }

        if (_duration != pStatus->Duration)
        {
            _duration = pStatus->Duration;
            changed = true;
        }

        if (_isReady != pStatus->IsReady)
        {
            _isReady = pStatus->IsReady;
            changed = true;
        }

        var firstDenial = (MCHResultAnnouncement*)pStatus->FirstDenial;
        if (firstDenial is null)
        {
            if (_firstDenial is not null)
            {
                _firstDenial = null;
                changed = true;
            }
        }
        else if (!HResultDenialsEqual(firstDenial, _firstDenial))
        {
            _firstDenial = MarshalAlert(firstDenial);
            AlertSent?.Invoke(_firstDenial);
        }

        if (!_fShuttingDown && changed)
            StatusChanged?.Invoke(this, EventArgs.Empty);

        if (_fShuttingDown)
        {
            if (_state == MCPlayerState.Closed)
                _gotStateCloseEvent?.Set();
            else if (_state == MCPlayerState.Uninitialized)
                _gotStateUninitializeEvent?.Set();
        }
    }

    internal unsafe void OnPropertyChanged(string wzKey, byte* pData, uint dataLength)
    {
        object? value = null;
        if (pData is not null)
        {
            switch (wzKey)
            {
                case "presentationinfo":
                    var presentationInfo = (PresentationInfoData*)pData;
                    value = new PresentationInfo((int)presentationInfo->AspectRatioX, (int)presentationInfo->AspectRatioY, presentationInfo->NativeX, presentationInfo->NativeY, presentationInfo->NeedOverscan);
                    break;
                case "volumeinfo":
                    var volumeInfo = (VolumeInfoData*)pData;
                    _volume = volumeInfo->Volume;
                    _mute = volumeInfo->Mute;
                    break;
                case "canchangevideorate":
                    bool canChangeVideoRate = *pData != 0;
                    _canChangeVideoRate = canChangeVideoRate;
                    value = canChangeVideoRate;
                    break;
                case "mbrheuristicsdata":
                    var heuristicData = (MBRHeuristicData*)pData;
                    PlayerBandwithUpdate?.Invoke(this, new BandwidthUpdateArgs(heuristicData->Buffering, heuristicData->Quality, heuristicData->Id, heuristicData->LatestBandwidth, heuristicData->RecentAverageBandwidth, heuristicData->TotalAverageBandwidth, heuristicData->Bitrate, heuristicData->DroppedFrames, heuristicData->TestLength, heuristicData->TestPosition, heuristicData->CurrentState));
                    break;
            }
        }
        _properties[wzKey] = value;
        PlayerPropertyChanged?.Invoke(this, new PlayerPropertyChangedEventArgs(wzKey, value));
    }

    internal unsafe void OnAlertOccurred(MCHResultAnnouncement* pAlert)
    {
        AlertSent?.Invoke(MarshalAlert(pAlert));
    }

    internal unsafe void OnTransportStatusChanged(MCTransportStatus* pTransportStatus)
    {
        _transportState = pTransportStatus->State;
        _rate = pTransportStatus->Rate;
        _endOfMedia = pTransportStatus->EndOfMedia;
        _canSeek = pTransportStatus->CanSeek;
        TransportStatusChanged?.Invoke(this, EventArgs.Empty);
    }

    internal void OnTransportPositionChanged(long position, long minSeekPosition, long maxSeekPosition)
    {
        _position = position;
        _minSeekPosition = minSeekPosition;
        _maxSeekPosition = maxSeekPosition;
        TransportPositionChanged?.Invoke(this, EventArgs.Empty);
    }

    internal void OnUriSet(string wzUri, uint dwParam)
    {
        _currentUriID = (int)dwParam;
        _currentUri = wzUri;
        UriSet?.Invoke(this, EventArgs.Empty);
    }

    private static unsafe Announcement MarshalAlert(MCHResultAnnouncement* hrAlert)
    {
        return new Announcement
        {
            Id = Marshal.PtrToStringUni(hrAlert->Id),
            HResult = hrAlert->HResult,
            PlaybackID = hrAlert->PlaybackID,
            SourceFile = Marshal.PtrToStringUni(hrAlert->SourceFile),
            SourceLine = hrAlert->SourceLine,
        };
    }

    private static unsafe bool HResultDenialsEqual(MCHResultAnnouncement* a, Announcement? b)
    {
        if (a is null)
            return b is null;
        if (b is null)
            return false;
        return a->HResult == b.HResult
            && a->SourceLine == b.SourceLine
            && Marshal.PtrToStringUni(a->Id) == b.Id
            && Marshal.PtrToStringUni(a->SourceFile) == b.SourceFile;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _firstDenial = null;
            Uninitialize();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~PlayerInterop()
    {
        Dispose(false);
    }
}
