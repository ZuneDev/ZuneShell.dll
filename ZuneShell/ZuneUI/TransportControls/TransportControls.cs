// Decompiled with JetBrains decompiler
// Type: ZuneUI.TransportControls
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.PerfTrace;
using Microsoft.Zune.Playback;
using Microsoft.Zune.QuickMix;
using Microsoft.Zune.Service;
using Microsoft.Zune.Util;
using MicrosoftZuneLibrary;
using MicrosoftZunePlayback;
using StrixMusic.Sdk.MediaPlayback;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UIXControls;
using ZuneUI.Strix;

namespace ZuneUI
{
    public partial class TransportControls : SingletonModelItem<TransportControls>
    {
        internal const long c_TicksPerSecond = 10000000;
        readonly Stack<IDisposable> _toDisposeOnEnd = new(2);
        private const long _rewindDelay = 50000000;
        private const float c_overscanFactor = 0.1f;
        private const int c_maxConsecutiveErrors = 5;
        private const int c_ratingUnrated = -1;
        private const string c_knownInvalidUri = ".:* INVALID URI *:.";
        private IPlaybackHandlerService _player;
        private BooleanChoice _shuffling;
        private BooleanChoice _repeating;
        private BooleanChoice _muted;
        private BooleanChoice _showTotalTime;
        private BooleanChoice _showNowPlayingList;
        private BooleanChoice _fastforwarding;
        private BooleanChoice _rewinding;
        private RangedValue _volume;
        private Command _play;
        private Command _pause;
        private Command _back;
        private Command _forward;
        private Command _stop;
        private bool _playingVideo;
        private bool _opening;
        private bool _buffering;
        private bool _seekEnabled;
        private int _zoomScaleFactor;
        private Command _fastforwardhotkey;
        private Command _rewindhotkey;
        private NowPlayingList _playlistPending;
        private NowPlayingList _playlistCurrent;
        private Microsoft.Iris.Timer _timerDelayedConfigPersist;
        private VideoStream _videoStream;
        private PlaybackState _lastKnownPlayerState;
        private PlaybackState _lastKnownTransportState;
        private long _lastKnownPosition;
        private Notification _nowPlayingNotification;
        private float _currentTrackDuration;
        private float _currentTrackPosition;
        private float _downloadProgress;
        private PlaybackTrack _lastKnownPreparedTrack;
        private PlaybackTrack _lastKnownPlaybackTrack;
        private List<PlaybackTrack> _tracksSubmittedToPlayer = new(2);
        private Dictionary<PlaybackTrack, int> _errors = new();
        private int _consecutiveErrors;
        private bool _showErrors = true;
        private int _lastKnownSetUriCallID;
        private static string _savedNowPlayingFilename = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Shell.LoadString(StringId.IDS_APPDATAFOLDERNAME).TrimStart('\\')), "NowPlaying.dat");
        private Shell _shellInstance;
        private PlayerState _playerState;
        private bool _streamingReportIsOpen;
        private Guid _streamingReportMediaId;
        private Guid _streamingReportMediaInstanceId;
        private TaskbarPlayer _taskbarPlayer;
        private bool _isInitialized;
        private bool _resumeLastNowPlayingRequested;
        private bool _shuffleAllRequested;
        private JumpListPin _requestedJumpListPin;
        private PlaybackContext _pagePlaybackContext;
        private PlaybackTrack _currentTrack;
        private bool _isPlaying;
        private bool _hasPlaylist;
        private bool _playlistSupportsShuffle = true;
        private bool _hasPlayed;
        private int _currentTrackIndex = -1;
        private ArrayListDataSet _currentPlaylist;
        private bool _isContextCompatible;
        private bool _isStreamingVideo;
        private bool _supressDownloads;
        private DateTime _currentPlayStartTime = DateTime.MinValue;
        private List<SpectrumOutputConfig> _spectrumConfigList;
        private bool _isSpectrumAvailable;
        private int _lastKnownCurrentTrackRating;
        private EventHandler _currentTrackRatingChangedEventHandler;
        private Microsoft.Iris.Timer _isStreamingTimeoutTimer;
        private bool _dontPlayMarketplaceTracks;
        private int _bandwidthCapacity;
        private BandwidthUpdateArgs _bandwidthUpdateInfo;

        public TransportControls()
        {
            _player = Microsoft.Zune.Shell.ZuneApplication.PlaybackHandler;
            _taskbarPlayer = TaskbarPlayer.Instance;
            _videoStream = new VideoStream();
            if (!CanRender3DVideo)
                _videoStream.DisplayDetailsChanged += OnVideoDetailsChanged;
            _shuffling = new BooleanChoice(this);
            _shuffling.Value = ClientConfiguration.Playback.ModeShuffle;
            _shuffling.ChosenChanged += OnShufflingChanged;
            UpdateShufflingDescription();
            _repeating = new BooleanChoice(this);
            _repeating.Value = ClientConfiguration.Playback.ModeLoop;
            _repeating.ChosenChanged += OnRepeatingChanged;
            UpdateRepeatingDescription();
            _muted = new BooleanChoice(this);
            _muted.Value = ClientConfiguration.Playback.Mute;
            _muted.ChosenChanged += OnMutingChanged;
            UpdateMutingDescription();
            _showTotalTime = new BooleanChoice(this);
            _showTotalTime.Value = ClientConfiguration.Playback.ShowTotalTime;
            _showTotalTime.ChosenChanged += OnShowTotalTimeChanged;
            _showNowPlayingList = new BooleanChoice(this);
            _showNowPlayingList.Value = ClientConfiguration.Playback.ShowNowPlayingList;
            _showNowPlayingList.ChosenChanged += OnShowNowPlayingListChanged;
            UpdateShowNowPlayingListDescription();
            _fastforwarding = new BooleanChoice(this);
            _fastforwarding.ChosenChanged += OnFastforwardingChanged;
            _rewinding = new BooleanChoice(this);
            _rewinding.ChosenChanged += OnRewindingChanged;
            float num = ClientConfiguration.Playback.Volume;
            if (num < 0.0 || num > 100.0)
                num = 50f;
            _volume = new RangedValue(this);
            _volume.MinValue = 0.0f;
            _volume.MaxValue = 100f;
            _volume.Value = num;
            _volume.PropertyChanged += OnVolumeControlChanged;
            _play = new Command(this, Shell.LoadString(StringId.IDS_PLAY), OnPlayClicked);
            _play.Available = false;
            _pause = new Command(this, Shell.LoadString(StringId.IDS_PAUSE), OnPauseClicked);
            _pause.Available = false;
            _back = new Command(this, Shell.LoadString(StringId.IDS_PREVIOUS), OnBackClicked);
            _back.Available = false;
            _forward = new Command(this, Shell.LoadString(StringId.IDS_NEXT), OnForwardClicked);
            _forward.Available = false;
            _stop = new Command(this, Shell.LoadString(StringId.IDS_STOP), OnStopClicked);
            _stop.Available = false;
            _fastforwardhotkey = new Command(this, new EventHandler(OnFastforwardHotkeyPressed));
            _rewindhotkey = new Command(this, new EventHandler(OnRewindHotkeyPressed));
            _player.CurrentItemChanged += Player_CurrentItemChanged;
            _player.PlaybackStateChanged += OnPlaybackStatusChanged;
            _player.PositionChanged += OnTransportPositionChanged;
            // TODO: _player.VolumeChanged += OnVolumeControlChanged;
            _lastKnownPlayerState = _player.PlaybackState;
            _lastKnownTransportState = _player.PlaybackState;
            _lastKnownPosition = _player.Position.Ticks;
            _shellInstance = (Shell)ZuneShell.DefaultInstance;
            _shellInstance.PropertyChanged += OnShellPropertyChanged;
            _timerDelayedConfigPersist = new Microsoft.Iris.Timer();
            _timerDelayedConfigPersist.Interval = 500;
            _timerDelayedConfigPersist.AutoRepeat = false;
            _timerDelayedConfigPersist.Tick += OnDelayedConfigPersistTimerTick;
            _playerState = PlayerState.Stopped;
            _spectrumConfigList = new List<SpectrumOutputConfig>();
            _isSpectrumAvailable = false;
            // TODO: Handle audio services that can't seek
            IsSeekEnabled = true;// _playbackWrapper.CanSeek;
            Download.Instance.DownloadProgressEvent += OnDownloadProgressed;
            _lastKnownCurrentTrackRating = -1;
            _currentTrackRatingChangedEventHandler = OnCurrentTrackRatingChanged;
            _isStreamingTimeoutTimer = new Microsoft.Iris.Timer(this);
            _isStreamingTimeoutTimer.Interval = 30000;
            _isStreamingTimeoutTimer.AutoRepeat = false;
            _isStreamingTimeoutTimer.Tick += OnIsStreamingTimeout;
            SignIn.Instance.SignInStatusUpdatedEvent += OnSignInEvent;
        }

        private bool TryGetPlayerInterop(out PlayerInterop playerInterop)
        {
            if (_player is PlayerInteropAudioService pias)
            {
                playerInterop = pias.PlaybackWrapper;
                return true;
            }

            playerInterop = null;
            return false;
        }

        protected override void OnDispose(bool fDisposing)
        {
            base.OnDispose(fDisposing);
            if (!fDisposing)
                return;
            DisconnectAllSpectrumAnimationSources();
            _isSpectrumAvailable = false;
            Microsoft.Zune.Util.Notification.ResetNowPlaying();
            if (WillSaveCurrentPlaylistOnShutdown())
            {
                try
                {
                    using (Stream serializationStream = File.Create(_savedNowPlayingFilename))
                        new BinaryFormatter().Serialize(serializationStream, _playlistCurrent);
                }
                catch (Exception ex)
                {
                }
            }
            else if (File.Exists(_savedNowPlayingFilename))
            {
                try
                {
                    File.Delete(_savedNowPlayingFilename);
                }
                catch (Exception ex)
                {
                }
            }
            _lastKnownPlaybackTrack?.OnEndPlayback(false);
            _player.CurrentItemChanged -= Player_CurrentItemChanged;
            _player.PlaybackStateChanged -= OnPlaybackStatusChanged;
            _player.PositionChanged -= OnTransportPositionChanged;
            // TODO: _player.VolumeChanged -= OnVolumeControlChanged;
            if (_videoStream != null && !CanRender3DVideo)
                _videoStream.DisplayDetailsChanged -= OnVideoDetailsChanged;
            PlayerInterop.Instance.Dispose();
            if (_videoStream != null)
            {
                _videoStream.Dispose();
                _videoStream = null;
            }
            if (_timerDelayedConfigPersist != null)
            {
                _timerDelayedConfigPersist.Tick -= OnDelayedConfigPersistTimerTick;
                _timerDelayedConfigPersist.Dispose();
                _timerDelayedConfigPersist = null;
            }
            _shellInstance.PropertyChanged -= OnShellPropertyChanged;
            Download.Instance.DownloadProgressEvent -= OnDownloadProgressed;
            if (_currentTrack != null)
                _currentTrack.RatingChanged.Invoked -= _currentTrackRatingChangedEventHandler;
            SignIn.Instance.SignInStatusUpdatedEvent -= OnSignInEvent;
        }

        private void OnSignInEvent(object sender, EventArgs args)
        {
            _playlistCurrent?.UpdateTracks();
            _playlistPending?.UpdateTracks();
        }

        private void PersistSettings()
        {
            if (_timerDelayedConfigPersist == null)
                return;

            _timerDelayedConfigPersist.Stop();
            _timerDelayedConfigPersist.Start();
        }

        private void OnDelayedConfigPersistTimerTick(object sender, EventArgs args)
        {
            ClientConfiguration.Playback.ModeShuffle = _shuffling.Value;
            ClientConfiguration.Playback.ModeLoop = _repeating.Value;
            ClientConfiguration.Playback.Mute = _muted.Value;
            ClientConfiguration.Playback.Volume = (int)_volume.Value;
            ClientConfiguration.Playback.ShowTotalTime = _showTotalTime.Value;
            ClientConfiguration.Playback.ShowNowPlayingList = _showNowPlayingList.Value;
        }

        private void OnCurrentTrackRatingChanged(object sender, EventArgs args)
        {
            int num = -1;
            if (CurrentTrack != null && CurrentTrack.CanRate)
                num = CurrentTrackRating;
            if (num == _lastKnownCurrentTrackRating)
                return;
            FirePropertyChanged("CurrentTrackRating");
            _lastKnownCurrentTrackRating = num;
        }

        public void TrackRatingUpdatedExternally(int mediaID, int newRating)
        {
            if (CurrentPlaylist == null)
                return;

            foreach (PlaybackTrack playbackTrack in CurrentPlaylist)
            {
                if (playbackTrack is LibraryPlaybackTrack libraryPlaybackTrack && libraryPlaybackTrack.MediaId == mediaID)
                    libraryPlaybackTrack.RatingUpdatedExternally(newRating);
            }
        }

        private void OnIsStreamingTimeout(object sender, EventArgs args) => SupressDownloads = false;

        private void DeserializeNowPlayingList(object arg)
        {
            object args = null;
            if (arg is string path)
            {
                if (File.Exists(path))
                {
                    try
                    {
                        using (Stream serializationStream = File.OpenRead(path))
                            args = new BinaryFormatter().Deserialize(serializationStream);
                    }
                    catch
                    {
                    }
                }
            }

            if (args != null)
                Application.DeferredInvoke(new DeferredInvokeHandler(DeserializationComplete), args);
        }

        private void DeserializationComplete(object arg)
        {
            if (_playlistCurrent != null || arg is not NowPlayingList nowPlayingList)
                return;
            _playlistCurrent = nowPlayingList;
            ShowNotification();
            UpdatePropertiesAndCommands();
            if (_resumeLastNowPlayingRequested)
                Play.Invoke();
        }

        public bool IsInitialized
        {
            get => _isInitialized;
            private set
            {
                if (_isInitialized == value)
                    return;
                _isInitialized = value;
                FirePropertyChanged(nameof(IsInitialized));
            }
        }

        public bool HasPlayed
        {
            get => _hasPlayed;
            private set
            {
                if (_hasPlayed == value)
                    return;
                _hasPlayed = value;
                FirePropertyChanged(nameof(HasPlayed));
            }
        }

        public event EventHandler PlaybackStopped;

        public BooleanChoice Shuffling => _shuffling;

        public BooleanChoice Repeating => _repeating;

        public BooleanChoice Muted => _muted;

        public BooleanChoice ShowTotalTime => _showTotalTime;

        public BooleanChoice ShowNowPlayingList => _showNowPlayingList;

        public BooleanChoice Fastforwarding => _fastforwarding;

        public BooleanChoice Rewinding => _rewinding;

        public RangedValue Volume => _volume;

        public Command Play => _play;

        public Command Pause => _pause;

        public Command Stop => _stop;

        public Command Back => _back;

        public Command Forward => _forward;

        public bool Opening
        {
            get => _opening;
            private set
            {
                if (_opening == value)
                    return;
                _opening = value;
                FirePropertyChanged(nameof(Opening));
            }
        }

        public bool Buffering
        {
            get => _buffering;
            private set
            {
                if (_buffering == value)
                    return;
                _buffering = value;
                FirePropertyChanged(nameof(Buffering));
            }
        }

        public bool IsSeekEnabled
        {
            get => _seekEnabled;
            private set
            {
                if (_seekEnabled == value)
                    return;
                _seekEnabled = value;
                FirePropertyChanged(nameof(IsSeekEnabled));
            }
        }

        public bool IsStreamingVideo
        {
            get => _isStreamingVideo;
            private set
            {
                if (_isStreamingVideo == value)
                    return;
                _isStreamingVideo = value;
                FirePropertyChanged(nameof(IsStreamingVideo));
            }
        }

        public bool SupressDownloads
        {
            get => _supressDownloads;
            private set
            {
                if (_supressDownloads == value)
                    return;
                _supressDownloads = value;
                FirePropertyChanged(nameof(SupressDownloads));
            }
        }

        public int ZoomScaleFactor
        {
            get => _zoomScaleFactor;
            set
            {
                _zoomScaleFactor = value;
                FirePropertyChanged(nameof(ZoomScaleFactor));
            }
        }

        public bool Playing => _isPlaying;

        public bool PlayingVideo
        {
            get => _playingVideo;
            private set
            {
                if (_playingVideo == value)
                    return;
                _playingVideo = value;
                FirePropertyChanged(nameof(PlayingVideo));
            }
        }

        public Command FastforwardHotkey => _fastforwardhotkey;

        public Command RewindHotkey => _rewindhotkey;

        public int CurrentTrackIndex => _currentTrackIndex;

        public bool WillSaveCurrentPlaylistOnShutdown() => _playlistCurrent != null && _playlistCurrent.CurrentTrack != null && !_playlistCurrent.CurrentTrack.IsVideo && _playlistCurrent.QuickMixSession == null;

        public void StartPlayingAt(PlaybackTrack track)
        {
            if (_playlistCurrent == null || _playlistCurrent.TrackList == null)
                return;
            int newCurrentIndex = _playlistCurrent.TrackList.IndexOf(track);
            if (newCurrentIndex <= -1)
                return;
            StartPlayingAt(newCurrentIndex);
        }

        public async void StartPlayingAt(int newCurrentIndex)
        {
            if (_playlistCurrent == null)
                return;

            _playlistCurrent.MoveToTrackIndex(newCurrentIndex);
            if (_playerState == PlayerState.Playing)
            {
                SetUriOnPlayer();
            }
            else
            {
                _playlistPending = _playlistCurrent;
                if (_playerState == PlayerState.Paused)
                    await _player.PauseAsync();
                else
                    PlayPendingList();
            }
        }

        public void CloseCurrentSession() => Stop.Invoke();

        public void SeekToPosition(float value)
        {
            var position = TimeSpan.FromTicks((long)(value * 10000000.0));
            AsyncHelper.Run(_player?.SeekAsync(position));
            _rewinding.Value = false;
            _fastforwarding.Value = false;
        }

        public float CurrentTrackDuration
        {
            get => _currentTrackDuration;
            private set
            {
                if (_currentTrackDuration == (double)value)
                    return;
                _currentTrackDuration = value;
                FirePropertyChanged(nameof(CurrentTrackDuration));
            }
        }

        public float CurrentTrackPosition
        {
            get => _currentTrackPosition;
            private set
            {
                if (_currentTrackPosition == (double)value)
                    return;
                _currentTrackPosition = value;
                FirePropertyChanged(nameof(CurrentTrackPosition));
            }
        }

        public float CurrentTrackDownloadProgress
        {
            get => _downloadProgress;
            private set
            {
                if (_downloadProgress == (double)value)
                    return;
                _downloadProgress = value;
                FirePropertyChanged(nameof(CurrentTrackDownloadProgress));
            }
        }

        public PlaybackTrack CurrentTrack => _currentTrack;

        public int CurrentTrackRating => CurrentTrack == null || !CurrentTrack.CanRate ? 0 : CurrentTrack.UserRating;

        public bool ShowErrors
        {
            get => _showErrors;
            set
            {
                if (_showErrors == value)
                    return;
                _showErrors = value;
                FirePropertyChanged(nameof(ShowErrors));
            }
        }

        public void ClearAllErrors()
        {
            if (_errors.Count <= 0)
                return;
            _errors.Clear();
            FirePropertyChanged("ErrorCount");
        }

        public bool IsCurrentTrack(Guid zuneMediaId)
        {
            PlaybackTrack currentTrack = CurrentTrack;
            return currentTrack != null && !GuidHelper.IsEmpty(currentTrack.ZuneMediaId) && currentTrack.ZuneMediaId == zuneMediaId;
        }

        public int GetErrorCode(Guid zuneMediaId)
        {
            if (_errors.Count > 0)
            {
                foreach (KeyValuePair<PlaybackTrack, int> error in _errors)
                {
                    PlaybackTrack key = error.Key;
                    if (key != null && key.ZuneMediaId == zuneMediaId)
                        return error.Value;
                }
            }
            return 0;
        }

        public bool IsCurrentTrack(int id, MediaType type, Guid zuneMediaId)
        {
            if (CurrentTrack is not LibraryPlaybackTrack currentTrack)
                return IsCurrentTrack(zuneMediaId);
            return currentTrack.MediaId == id && currentTrack.MediaType == type;
        }

        public int GetLibraryErrorCode(int id, MediaType type)
        {
            if (_errors.Count > 0)
            {
                foreach (KeyValuePair<PlaybackTrack, int> error in _errors)
                {
                    if (error.Key is LibraryPlaybackTrack key && key.MediaId == id && key.MediaType == type)
                        return error.Value;
                }
            }
            return 0;
        }

        public int GetLibraryErrorCode(PlaybackTrack track) => _errors.TryGetValue(track, out int num) ? num : 0;

        internal void ClearError(PlaybackTrack track)
        {
            if (_errors.Remove(track))
                FirePropertyChanged("ErrorCount");
        }

        public int ErrorCount => _errors.Count;

        public VideoStream VideoStream => _videoStream;

        public bool CanRender3DVideo => Application.RenderingType != RenderingType.GDI;

        public bool HasPlaylist => _hasPlaylist;

        public bool PlaylistSupportsShuffle => _playlistSupportsShuffle;

        public ArrayListDataSet CurrentPlaylist => _currentPlaylist;

        public string QuickMixTitle
        {
            get
            {
                string str = string.Empty;
                if (_playlistCurrent != null)
                    str = _playlistCurrent.QuickMixTitle;
                return str;
            }
        }

        public EQuickMixType QuickMixType
        {
            get
            {
                EQuickMixType equickMixType = EQuickMixType.eQuickMixTypeInvalid;
                if (_playlistCurrent != null)
                    equickMixType = _playlistCurrent.QuickMixType;
                return equickMixType;
            }
        }

        public bool DontPlayMarketplaceTracks
        {
            get => _dontPlayMarketplaceTracks;
            set
            {
                if (value == _dontPlayMarketplaceTracks)
                    return;
                _dontPlayMarketplaceTracks = value;
                if (_playlistCurrent != null)
                    _playlistCurrent.DontPlayMarketplaceTracks = _dontPlayMarketplaceTracks;
                if (_playlistPending == null)
                    return;
                _playlistPending.DontPlayMarketplaceTracks = _dontPlayMarketplaceTracks;
            }
        }

        public QuickMixSession QuickMixSession
        {
            get
            {
                QuickMixSession quickMixSession = null;
                if (_playlistCurrent != null)
                    quickMixSession = _playlistCurrent.QuickMixSession;
                return quickMixSession;
            }
        }

        public bool IsPlaybackContextCompatible => _isContextCompatible;

        public JumpListPin RequestedJumpListPin
        {
            get => !IsInitialized ? null : _requestedJumpListPin;
            set
            {
                if (_requestedJumpListPin == value)
                    return;
                _requestedJumpListPin = value;
                if (!IsInitialized)
                    return;
                FirePropertyChanged(nameof(RequestedJumpListPin));
            }
        }

        public bool ShuffleAllRequested
        {
            get => IsInitialized && _shuffleAllRequested;
            set
            {
                if (_shuffleAllRequested == value)
                    return;
                _shuffleAllRequested = value;
                if (!IsInitialized)
                    return;
                FirePropertyChanged(nameof(ShuffleAllRequested));
            }
        }

        public int BandwidthCapacity
        {
            get => _bandwidthCapacity;
            private set
            {
                _bandwidthCapacity = value;
                FirePropertyChanged(nameof(BandwidthCapacity));
            }
        }

        public BandwidthUpdateArgs BandwidthUpdateInfo
        {
            get => _bandwidthUpdateInfo;
            private set
            {
                _bandwidthUpdateInfo = value;
                FirePropertyChanged(nameof(BandwidthUpdateInfo));
            }
        }

        private void OnBandwidthCapacityUpdate(object sender, BandwidthUpdateArgs args) => Application.DeferredInvoke(new DeferredInvokeHandler(OnBandwidthCapacityUpdateOnApp), args);

        private void OnBandwidthCapacityUpdateOnApp(object obj)
        {
            if (obj == null)
                return;
            BandwidthUpdateArgs bandwidthUpdateArgs = (BandwidthUpdateArgs)obj;
            if (bandwidthUpdateArgs == null || bandwidthUpdateArgs.currentState != MBRHeuristicState.Playback)
                return;
            BandwidthCapacity = bandwidthUpdateArgs.RecentAverageBandwidth;
            BandwidthUpdateInfo = bandwidthUpdateArgs;
        }

        private void OnStreamingRestrictionResponse(HRESULT hr)
        {
            if (!hr.IsError)
                return;
            string title = Shell.LoadString(StringId.IDS_PLAYBACK_CANNOT_PLAY);
            string description;
            if (hr == HRESULT._ZEST_E_MAX_CONCURRENTSTREAMING_EXCEEDED || hr == HRESULT._ZEST_E_MULTITUNER_CONCURRENTSTREAMING_DETECTED || hr == HRESULT._ZEST_E_MEDIAINSTANCE_STREAMING_OCCUPIED)
                description = Microsoft.Zune.ErrorMapperApi.ErrorMapperApi.GetMappedErrorDescriptionAndUrl(hr.Int).Description;
            else
                description = Shell.LoadString(StringId.IDS_PLAYBACK_UNKNOWN_CONCURRENT_STREAMING_RESTRICTION);
            Application.DeferredInvoke(delegate
            {
                Stop.Invoke();
                MessageBox.Show(title, description, null);
            }, DeferredInvokePriority.Low);
        }

        private void ReportStreamingAction(PlayerState previousPlayerState)
        {
            if (CurrentTrack != null && CurrentTrack.IsVideo && CurrentTrack.IsStreaming)
            {
                Guid zuneMediaId = CurrentTrack.ZuneMediaId;
                if (zuneMediaId != _streamingReportMediaId && _streamingReportIsOpen)
                {
                    _streamingReportIsOpen = false;
                    previousPlayerState = PlayerState.Stopped;
                    Service.Instance.ReportStreamingAction(EStreamingActionType.Stop, _streamingReportMediaInstanceId, new AsyncCompleteHandler(OnStreamingRestrictionResponse));
                }
                if (previousPlayerState == PlayerState.Stopped && _playerState == PlayerState.Playing)
                {
                    _streamingReportMediaInstanceId = CurrentTrack.ZuneMediaInstanceId;
                    if (!(_streamingReportMediaInstanceId != Guid.Empty))
                        return;
                    _streamingReportIsOpen = true;
                    _streamingReportMediaId = zuneMediaId;
                    Service.Instance.ReportStreamingAction(EStreamingActionType.Start, _streamingReportMediaInstanceId, new AsyncCompleteHandler(OnStreamingRestrictionResponse));
                }
                else if (previousPlayerState == PlayerState.Paused && _playerState == PlayerState.Playing)
                    Service.Instance.ReportStreamingAction(EStreamingActionType.Resume, _streamingReportMediaInstanceId, new AsyncCompleteHandler(OnStreamingRestrictionResponse));
                else if (_playerState == PlayerState.Paused)
                {
                    Service.Instance.ReportStreamingAction(EStreamingActionType.Pause, _streamingReportMediaInstanceId, new AsyncCompleteHandler(OnStreamingRestrictionResponse));
                }
                else
                {
                    if (_playerState != PlayerState.Stopped)
                        return;
                    _streamingReportIsOpen = false;
                    Service.Instance.ReportStreamingAction(EStreamingActionType.Stop, _streamingReportMediaInstanceId, new AsyncCompleteHandler(OnStreamingRestrictionResponse));
                }
            }
            else
            {
                if (!_streamingReportIsOpen)
                    return;
                _streamingReportIsOpen = false;
                Service.Instance.ReportStreamingAction(EStreamingActionType.Stop, _streamingReportMediaInstanceId, new AsyncCompleteHandler(OnStreamingRestrictionResponse));
            }
        }

        private void SetPlayerState(PlayerState stateNew)
        {
            PlayerState playerState = _playerState;
            if (stateNew != _playerState)
            {
                _playerState = stateNew;
                if (_playerState == PlayerState.Stopped)
                {
                    _rewinding.Value = false;
                    _fastforwarding.Value = false;
                    ++_lastKnownSetUriCallID;
                    FirePropertyChanged("PlaybackStopped");
                    PlaybackStopped?.Invoke(this, null);
                }

                UpdateSmtcState(stateNew);
            }
            UpdatePropertiesAndCommands();
            ReportStreamingAction(playerState);
        }

        public void PlayItem(
          object item,
          PlayNavigationOptions playNavigationOptions,
          PlaybackContext playbackContext)
        {
            ArrayListDataSet arrayListDataSet = new()
            {
                item
            };
            PlayItemsWorker(arrayListDataSet, -1, true, playNavigationOptions, playbackContext, null);
        }

        public void PlayItem(object item) => PlayItem(item, PlayNavigationOptions.NavigateVideosToNowPlaying);

        public void PlayItem(object item, PlayNavigationOptions playNavigationOptions)
        {
            ArrayListDataSet arrayListDataSet = new()
            {
                item
            };
            PlayItemsWorker(arrayListDataSet, -1, true, playNavigationOptions, null);
        }

        public void PlayItem(object item, PlaybackContext playbackContext)
        {
            ArrayListDataSet arrayListDataSet = new()
            {
                item
            };
            PlayItemsWorker(arrayListDataSet, -1, true, PlayNavigationOptions.NavigateVideosToNowPlaying, playbackContext, null);
        }

        public void PlayItems(IList items) => PlayItemsWorker(items, -1, true, PlayNavigationOptions.NavigateVideosToNowPlaying, null);

        public void PlayItems(IList items, PlayNavigationOptions playNavigationOptions) => PlayItemsWorker(items, -1, true, playNavigationOptions, null);

        public void PlayItems(
          IList items,
          PlayNavigationOptions playNavigationOptions,
          ContainerPlayMarker containerPlayMarker)
        {
            PlayItemsWorker(items, -1, true, playNavigationOptions, containerPlayMarker);
        }

        public void PlayItems(
          IList items,
          PlayNavigationOptions playNavigationOptions,
          PlaybackContext playbackContext)
        {
            PlayItemsWorker(items, -1, true, playNavigationOptions, playbackContext, null);
        }

        public void PlayItems(IList items, PlaybackContext playbackContext) => PlayItemsWorker(items, -1, true, PlayNavigationOptions.NavigateVideosToNowPlaying, playbackContext, null);

        public void PlayItems(IList items, int startIndex) => PlayItemsWorker(items, startIndex, true, PlayNavigationOptions.NavigateVideosToNowPlaying, null);

        public void PlayItems(
          IList items,
          int startIndex,
          PlayNavigationOptions playNavigationOptions,
          ContainerPlayMarker containerPlayMarker)
        {
            PlayItemsWorker(items, startIndex, true, PlayNavigationOptions.NavigateVideosToNowPlaying, containerPlayMarker);
        }

        public void AddToNowPlaying(IList items)
        {
            int count = PlayItemsWorker(items, -1, false, PlayNavigationOptions.NavigateVideosToNowPlaying, null);
            if (count > 0)
                PlaylistManager.Instance.NotifyItemsAdded(-1, count);
        }

        private int PlayItemsWorker(
          IList items,
          int startIndex,
          bool clearQueue,
          PlayNavigationOptions playNavigationOptions,
          ContainerPlayMarker containerPlayMarker)
        {
            return PlayItemsWorker(items, startIndex, clearQueue, playNavigationOptions, _shellInstance.CurrentPage.PlaybackContext, containerPlayMarker);
        }

        private int PlayItemsWorker(
          IList items,
          int startIndex,
          bool clearQueue,
          PlayNavigationOptions playNavigationOptions,
          PlaybackContext playbackContext,
          ContainerPlayMarker containerPlayMarker)
        {
            PerfTrace.TraceUICollectionEvent(UICollectionEvent.PlayRequestIssued, "");
            bool hasPlaylist = _playlistCurrent != null;
            int num;
            if (clearQueue || !hasPlaylist)
            {
                if (clearQueue || _playlistPending == null)
                {
                    _playlistPending?.Dispose();
                    _playlistPending = new NowPlayingList(items, startIndex, playbackContext, playNavigationOptions, _shuffling.Value, containerPlayMarker, _dontPlayMarketplaceTracks);
                    num = _playlistPending.Count;
                }
                else
                    num = _playlistPending.AddItems(items);
                if (playbackContext == PlaybackContext.QuickMix)
                {
                    if (_lastKnownTransportState == PlaybackState.Playing || _lastKnownTransportState == PlaybackState.Paused)
                        AsyncHelper.Run(_player.PauseAsync());
                    else
                        _playlistPending.PlayWhenReady = true;
                }
                else if (_playlistPending.Count == 0)
                {
                    _playlistPending.Dispose();
                    _playlistPending = null;
                    Stop.Invoke();
                }
                else if (hasPlaylist && (_lastKnownTransportState == PlaybackState.Playing || _lastKnownTransportState == PlaybackState.Paused))
                    AsyncHelper.Run(_player.PauseAsync());
                else
                    PlayPendingList();
            }
            else
            {
                num = _playlistCurrent.AddItems(items);
                _playlistPending?.Dispose();
                _playlistPending = null;
                UpdateNextTrack();
            }
            return num;
        }

        internal void PlayPendingList()
        {
            if (_playlistPending == null)
                return;
            if (_playlistCurrent != null && _playlistCurrent != _playlistPending)
                _playlistCurrent.Dispose();
            _playlistCurrent = _playlistPending;
            _playlistPending = null;
            _playlistCurrent.SetShuffling(_shuffling.Value);
            _playlistCurrent.SetRepeating(_repeating.Value);
            _consecutiveErrors = 0;
            if (_errors.Count > 0)
            {
                FirePropertyChanged("ErrorCount");
                _errors.Clear();
            }
            SetPlayerState(PlayerState.Playing);
            SetUriOnPlayer();
            bool isCurrentVideo = false;
            if (_playlistCurrent != null)
            {
                PlaybackTrack currentTrack = _playlistCurrent.CurrentTrack;
                PlayNavigationOptions navigationOptions = _playlistCurrent.PlayNavigationOptions;
                bool navigate = false;
                if (currentTrack != null && currentTrack.IsVideo)
                    isCurrentVideo = true;
                switch (navigationOptions)
                {
                    case PlayNavigationOptions.None:
                        if (navigate)
                            NowPlayingLand.NavigateToLand(navigationOptions == PlayNavigationOptions.NavigateToNowPlayingWithMix, true);
                        break;

                    case PlayNavigationOptions.NavigateVideosToNowPlaying:
                        if (isCurrentVideo)
                            navigate = true;
                        goto case PlayNavigationOptions.None;

                    default:
                        navigate = true;
                        _playlistCurrent.PlayNavigationOptions = PlayNavigationOptions.NavigateVideosToNowPlaying;
                        goto case PlayNavigationOptions.None;
                }
            }
            PlayingVideo = isCurrentVideo;
        }

        public void RemoveFromNowPlaying(IList indices)
        {
            if (_playlistCurrent == null)
                return;

            bool removed = _playlistCurrent.Remove(indices);
            if (_playlistCurrent.Count == 0)
                Stop.Invoke();
            else if (removed)
                SetUriOnPlayer();
            else
                UpdateNextTrack();
        }

        public void ReorderNowPlaying(IList indices, int targetIndex)
        {
            if (_playlistCurrent == null)
                return;
            _playlistCurrent.Reorder(indices, targetIndex);
            UpdateNextTrack();
        }

        public IList GetNextTracks(int count) => _playlistCurrent?.GetNextTracks(count);

        public IList CreateAlbumListForBackground(IList allAlbums, int totalDesired)
        {
            List<object> objectList1 = new(totalDesired);
            Dictionary<int, object> dictionary = new();
            if (_playlistCurrent != null)
            {
                int num = Math.Min(_playlistCurrent.Count, totalDesired);
                for (int itemIndex = 0; itemIndex < num; ++itemIndex)
                {
                    if (_playlistCurrent.TrackList[itemIndex] is LibraryPlaybackTrack track && track.MediaType == MediaType.Track)
                        dictionary[track.AlbumLibraryId] = null;
                }
            }
            List<object> objectList2 = new(totalDesired);
            for (int index = 0; index < allAlbums.Count; ++index)
            {
                DataProviderObject allAlbum = (DataProviderObject)allAlbums[index];
                int property = (int)allAlbum.GetProperty("LibraryId");
                if ((bool)allAlbum.GetProperty("HasAlbumArt"))
                {
                    if (dictionary.ContainsKey(property))
                        objectList1.Add(allAlbum);
                    else if (objectList2.Count < totalDesired)
                        objectList2.Add(allAlbum);
                }
                if (objectList1.Count >= totalDesired)
                    break;
            }
            for (int index = 0; index < objectList2.Count && objectList1.Count < totalDesired; ++index)
                objectList1.Add(objectList2[index]);
            if (objectList1.Count == 0)
                return null;
            foreach (object album in objectList1)
                DisableSlowDataThumbnailExtraction(album);
            int num1 = 0;
            while (objectList1.Count < totalDesired)
                objectList1.Add(objectList1[num1++]);
            Random random = new();
            for (int index1 = objectList1.Count - 1; index1 > 0; --index1)
            {
                int index2 = random.Next(index1 + 1);
                object obj = objectList1[index2];
                objectList1[index2] = objectList1[index1];
                objectList1[index1] = obj;
            }
            return new ListDataSet(objectList1);
        }

        public void DisableSlowDataThumbnailExtraction(object album)
        {
            if (album is LibraryDataProviderItemBase providerItemBase)
                providerItemBase.SetSlowDataThumbnailExtraction(false);
        }

        public void Phase2Init()
        {
            if (TryGetPlayerInterop(out var playbackWrapper))
            {
                if (!CanRender3DVideo)
                    playbackWrapper.WindowHandle = Application.Window.Handle;
                else
                    playbackWrapper.DynamicImage = _videoStream.StreamID;
            }
            ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncPhase2Init), null);
        }

        private void AsyncPhase2Init(object arg) => Application.DeferredInvoke(new DeferredInvokeHandler(CompletePhase2Init), arg);

        private void CompletePhase2Init(object obj)
        {
            AsyncHelper.Run(_player.ChangeVolumeAsync(_volume.Value / 100d));
            
            _isSpectrumAvailable = true;
            ConnectAllSpectrumAnimationSources();

            _taskbarPlayer.Initialize(Application.Window.Handle, new TaskbarPlayerCommandHandler(OnTaskbarPlayerCommand));

            IsInitialized = true;

            ThreadPool.QueueUserWorkItem(new WaitCallback(DeserializeNowPlayingList), _savedNowPlayingFilename);
            if (RequestedJumpListPin != null)
                FirePropertyChanged("RequestedJumpListPin");
            if (ShuffleAllRequested)
                FirePropertyChanged("ShuffleAllRequested");
        }

        private void OnTaskbarPlayerCommand(ETaskbarPlayerCommand command, int value)
        {
            switch (command)
            {
                case ETaskbarPlayerCommand.PC_Connect:
                    UpdateTaskbarPlayer();
                    break;
                case ETaskbarPlayerCommand.PC_Play:
                    Play.Invoke();
                    break;
                case ETaskbarPlayerCommand.PC_Pause:
                    Pause.Invoke();
                    break;
                case ETaskbarPlayerCommand.PC_Forward:
                    Forward.Invoke();
                    break;
                case ETaskbarPlayerCommand.PC_Back:
                    Back.Invoke();
                    break;
                case ETaskbarPlayerCommand.PC_Rate:
                    if (CurrentTrack == null || !CurrentTrack.CanRate)
                        break;
                    switch ((ETaskbarPlayerState)value)
                    {
                        case ETaskbarPlayerState.PS_RatingNotRated:
                            CurrentTrack.UserRating = 0;
                            break;
                        case ETaskbarPlayerState.PS_RatingLoveIt:
                            CurrentTrack.UserRating = 8;
                            break;
                        case ETaskbarPlayerState.PS_RatingHateIt:
                            CurrentTrack.UserRating = 2;
                            break;
                    }
                    UpdateTaskbarPlayer();
                    break;
            }
        }

        private void UpdateTaskbarPlayer()
        {
            ETaskbarPlayerState etaskbarPlayerState1 = (ETaskbarPlayerState)(0 | (_playerState == PlayerState.Playing ? 2 : 0) | (_playerState == PlayerState.Paused ? 4 : 0) | (_playerState == PlayerState.Stopped ? 1 : 0));
            ETaskbarPlayerState etaskbarPlayerState2;
            if (CurrentTrack != null && CurrentTrack.CanRate)
            {
                RatingConstants userRating = (RatingConstants)CurrentTrack.UserRating;
                etaskbarPlayerState2 = userRating != RatingConstants.Unrated ? (userRating > RatingConstants.MaxHateIt ? etaskbarPlayerState1 | ETaskbarPlayerState.PS_RatingLoveIt : etaskbarPlayerState1 | ETaskbarPlayerState.PS_RatingHateIt) : etaskbarPlayerState1 | ETaskbarPlayerState.PS_RatingNotRated;
            }
            else
                etaskbarPlayerState2 = etaskbarPlayerState1 | ETaskbarPlayerState.PS_RatingNotRated;
            ETaskbarPlayerState state = etaskbarPlayerState2 | (Play.Available ? ETaskbarPlayerState.PS_CanPlay : 0) | (Pause.Available ? ETaskbarPlayerState.PS_CanPause : 0) | (Forward.Available ? ETaskbarPlayerState.PS_CanForward : 0) | (Back.Available ? ETaskbarPlayerState.PS_CanBack : 0);
            if (CurrentTrack != null && CurrentTrack.CanRate)
                state |= ETaskbarPlayerState.PS_CanRate;
            _taskbarPlayer.UpdateToolbar(state);
        }

        public int CreateSpectrumAnimationSource(
          int numBands,
          bool outputFrequencyData,
          bool outputWaveformData,
          bool enableStereoOutput)
        {
            Dictionary<string, int> dictionary = new();
            for (int index = 0; index < numBands; ++index)
            {
                if (outputFrequencyData)
                {
                    if (!enableStereoOutput)
                    {
                        dictionary[$"Frequency{index}"] = index;
                    }
                    else
                    {
                        dictionary[$"FrequencyL{index}"] = index;
                        dictionary[$"FrequencyR{index}"] = index + 1024;
                    }
                }
                if (outputWaveformData)
                {
                    if (!enableStereoOutput)
                    {
                        dictionary[$"Waveform{index}"] = index + 2048;
                    }
                    else
                    {
                        dictionary[$"WaveformL{index}"] = index + 2048;
                        dictionary[$"WaveformR{index}"] = index + 3072;
                    }
                }
            }
            int externalAnimationInput = Application.CreateExternalAnimationInput(dictionary);
            SpectrumOutputConfig spectrumOutputConfig = new()
            {
                SourceId = (uint)externalAnimationInput,
                NumBands = (uint)numBands,
                Frequency = outputFrequencyData,
                Waveform = outputWaveformData,
                Stereo = enableStereoOutput,
                IsConnected = _isSpectrumAvailable
            };
            _spectrumConfigList.Add(spectrumOutputConfig);

            if (_isSpectrumAvailable && TryGetPlayerInterop(out var playbackWrapper))
                playbackWrapper.ConnectAnimationsToSpectrumAnalyzer(spectrumOutputConfig.SourceId, spectrumOutputConfig.NumBands, spectrumOutputConfig.Frequency, spectrumOutputConfig.Waveform, spectrumOutputConfig.Stereo);
            
            return externalAnimationInput;
        }

        public void DisposeSpectrumAnimationSource(int inputSourceId)
        {
            if (inputSourceId <= 0)
                return;
            for (int index = 0; index < _spectrumConfigList.Count; ++index)
            {
                SpectrumOutputConfig spectrumConfig = _spectrumConfigList[index];
                if (spectrumConfig.SourceId == inputSourceId)
                {
                    if (spectrumConfig.IsConnected && TryGetPlayerInterop(out var playbackWrapper))
                    {
                        playbackWrapper.DisconnectAnimationsFromSpectrumAnalyzer(spectrumConfig.SourceId);
                        spectrumConfig.IsConnected = false;
                    }

                    Application.DisposeExternalAnimationInput(inputSourceId);
                    _spectrumConfigList.RemoveAt(index);
                    break;
                }
            }
        }

        public void ConnectAllSpectrumAnimationSources()
        {
            for (int index = 0; index < _spectrumConfigList.Count; ++index)
            {
                SpectrumOutputConfig spectrumConfig = _spectrumConfigList[index];
                if (!spectrumConfig.IsConnected && TryGetPlayerInterop(out var playbackWrapper))
                {
                    playbackWrapper.ConnectAnimationsToSpectrumAnalyzer(spectrumConfig.SourceId, spectrumConfig.NumBands, spectrumConfig.Frequency, spectrumConfig.Waveform, spectrumConfig.Stereo);
                    spectrumConfig.IsConnected = true;
                    _spectrumConfigList[index] = spectrumConfig;
                }
            }
        }

        public void DisconnectAllSpectrumAnimationSources()
        {
            if (!_isSpectrumAvailable)
                return;
            for (int index = 0; index < _spectrumConfigList.Count; ++index)
            {
                SpectrumOutputConfig spectrumConfig = _spectrumConfigList[index];
                if (spectrumConfig.IsConnected && TryGetPlayerInterop(out var playbackWrapper))
                {
                    playbackWrapper.DisconnectAnimationsFromSpectrumAnalyzer(spectrumConfig.SourceId);
                    spectrumConfig.IsConnected = false;
                    _spectrumConfigList[index] = spectrumConfig;
                }
            }
        }

        public void ResumeLastNowPlayingHandler()
        {
            if (!IsInitialized)
            {
                _resumeLastNowPlayingRequested = true;
            }
            else if(!_isPlaying)
            {
                Play.Invoke();
            }
        }

        private void OnShellPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "CurrentPage")
                return;
            _pagePlaybackContext = _shellInstance.CurrentPage.PlaybackContext;
            UpdatePropertiesAndCommands();
        }

        private void OnDownloadProgressed(Guid zuneMediaId, float percent)
        {
            if (CurrentTrack == null || !(CurrentTrack.ZuneMediaId == zuneMediaId))
                return;
            CurrentTrackDownloadProgress = percent;
        }

        private void OnShufflingChanged(object sender, EventArgs e)
        {
            if (_playlistCurrent != null)
            {
                _playlistCurrent.SetShuffling(_shuffling.Value);
                UpdateNextTrack();
            }
            UpdateShufflingDescription();
            PersistSettings();
            SQMLog.Log(SQMDataId.ShuffleClicks, 1);
        }

        private void UpdateShufflingDescription() => _shuffling.Description = Shell.LoadString(!_shuffling.Value ? StringId.IDS_SHUFFLE_ON : StringId.IDS_SHUFFLE_OFF);

        private void OnRepeatingChanged(object sender, EventArgs e)
        {
            if (_playlistCurrent != null)
            {
                _playlistCurrent.SetRepeating(_repeating.Value);
                UpdateNextTrack();
            }
            SQMLog.Log(SQMDataId.RepeatClicks, 1);
            UpdateRepeatingDescription();
            PersistSettings();
        }

        private void UpdateRepeatingDescription() => _repeating.Description = Shell.LoadString(!_repeating.Value ? StringId.IDS_REPEAT_ON : StringId.IDS_REPEAT_OFF);

        private void OnMutingChanged(object sender, EventArgs e)
        {
            UpdateMutingDescription();

            double volume = _muted.Value ? 0.0 : 0.5;
            AsyncHelper.Run(_player.ChangeVolumeAsync(volume));

            SQMLog.Log(SQMDataId.VolumeMuteClicks, 1);
            PersistSettings();
        }

        private void UpdateMutingDescription() => _muted.Description = Shell.LoadString(!_muted.Value ? StringId.IDS_MUTE : StringId.IDS_UNMUTE);

        private void OnShowTotalTimeChanged(object sender, EventArgs e) => PersistSettings();

        private void OnShowNowPlayingListChanged(object sender, EventArgs e)
        {
            UpdateShowNowPlayingListDescription();
            PersistSettings();
        }

        private void UpdateShowNowPlayingListDescription() => _showNowPlayingList.Description = Shell.LoadString(!_showNowPlayingList.Value ? StringId.IDS_NOWPLAYINGLIST_ON : StringId.IDS_NOWPLAYINGLIST_OFF);

        private void OnPlayingChanged(object sender, EventArgs e)
        {
        }

        private void OnFastforwardingChanged(object sender, EventArgs e)
        {
            if (_fastforwarding.Value && (_currentTrack == null || !_currentTrack.IsVideo
                || (TryGetPlayerInterop(out var playbackWrapper) && playbackWrapper.CanChangeVideoRate)))
            {
                _rewinding.Value = false;
                AsyncHelper.Run(_player.ChangePlaybackSpeedAsync(5.0));
            }
            else
                AsyncHelper.Run(_player.ChangePlaybackSpeedAsync(1.0));
        }

        private void OnRewindingChanged(object sender, EventArgs e)
        {
            if (_fastforwarding.Value && (_currentTrack == null || !_currentTrack.IsVideo
                || (TryGetPlayerInterop(out var playbackWrapper) && playbackWrapper.CanChangeVideoRate)))
            {
                _rewinding.Value = false;
                AsyncHelper.Run(_player.ChangePlaybackSpeedAsync(-5.0));
            }
            else
                AsyncHelper.Run(_player.ChangePlaybackSpeedAsync(1.0));
        }

        private void OnFastforwardHotkeyPressed(object sender, EventArgs e) => _fastforwarding.Value = !_fastforwarding.Value;

        private void OnRewindHotkeyPressed(object sender, EventArgs e) => _rewinding.Value = !_rewinding.Value;

        private void OnPlaybackStatusChanged(object sender, PlaybackState e) => Application.DeferredInvoke(new DeferredInvokeHandler(DeferredPlaybackStatusChanged), new object[2]
        {
           _player.PlaybackState,
           false    // EndOfMedia
        });

        private void OnTransportStatusChanged(object sender, EventArgs e) => Application.DeferredInvoke(new DeferredInvokeHandler(DeferredTransportStatusChanged), new object[3]
        {
           _player.PlaybackState,
           false, // _playbackWrapper.EndOfMedia,
           true // _playbackWrapper.CanSeek
        });

        private void OnTransportPositionChanged(object sender, TimeSpan e) => Application.DeferredInvoke(new DeferredInvokeHandler(DeferredTransportPositionChanged), e);

        private void OnUriSet(object sender, EventArgs e)
        {
            if (_player.CurrentItem?.MediaConfig is ZuneMediaSourceConfig zuneSrcCfg)
            {
                var args = new object[2]
                {
                   zuneSrcCfg.MediaSourceUri,
                   0, // TODO: playbackWrapper.CurrentUriID
                };
                Application.DeferredInvoke(new DeferredInvokeHandler(DeferredUriSet), args);
            }
        }

        private void OnAlertSent(Announcement alert) => Application.DeferredInvoke(new DeferredInvokeHandler(DeferredAlertHandler), alert);

        private void OnPlayerPropertyChanged(object sender, PlayerPropertyChangedEventArgs e)
        {
            if (e.Key == "presentationinfo")
                Application.DeferredInvoke(new DeferredInvokeHandler(DeferredPresentationInfoChangedHandler), e.Value);
            else if (e.Key == "volumeinfo")
                Application.DeferredInvoke(new DeferredInvokeHandler(DeferredVolumeInfoChangedHandler), e.Value);
            else if (e.Key == "canchangevideorate")
                Application.DeferredInvoke(new DeferredInvokeHandler(DeferredCanChangeVideoRateHandler), e.Value);
        }

        private void OnVolumeControlChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Value")
                return;

            _muted.Value = false;
            _player.ChangeVolumeAsync(_volume.Value / 100f);
            SQMLog.Log(SQMDataId.VolumeAdjustmentClicks, 1);
            PersistSettings();
        }

        private void OnVideoDetailsChanged(object sender, EventArgs args)
        {
            if (TryGetPlayerInterop(out var playbackWrapper))
            {
                playbackWrapper.VideoPosition = new VideoWindow(_videoStream.DisplayPosition.X, _videoStream.DisplayPosition.Y, _videoStream.DisplayPosition.X + _videoStream.DisplaySize.Width, _videoStream.DisplayPosition.Y + _videoStream.DisplaySize.Height);
                playbackWrapper.ShowGDIVideo = _videoStream.DisplayVisibility;
            }
        }

        private void DeferredPlaybackStatusChanged(object obj)
        {
            if (IsDisposed)
                return;

            object[] objArray = (object[])obj;
            var playerState = (PlaybackState)objArray[0];
            bool endOfMedia = (bool)objArray[1];
            bool startedOpening = false;
            switch (playerState)
            {
                case PlaybackState.None:
                    if (Playing)
                    {
                        SetUriOnPlayer();
                        break;
                    }
                    break;

                case PlaybackState.Loading:
                    _rewinding.Value = false;
                    _fastforwarding.Value = false;
                    startedOpening = true;
                    break;

                case PlaybackState.Loaded:
                    if (Playing && _lastKnownPlaybackTrack != null)
                    {
                        _lastKnownPlaybackTrack.OnEndPlayback(endOfMedia);
                        _lastKnownPlaybackTrack = _lastKnownPreparedTrack;
                        _lastKnownPreparedTrack = null;
                        _lastKnownPlaybackTrack?.OnBeginPlayback(null);
                    }

                    // TODO: Get duration of track
                    CurrentTrackDuration = 0f; // _playbackWrapper.Duration >= 0L ? _playbackWrapper.Duration / 1E+07f : 0.0f;
                    break;
            }

            _lastKnownPlayerState = playerState;
            if (_opening == startedOpening)
                return;
            Opening = startedOpening;
        }

        private void ShowNotification()
        {
            if (_nowPlayingNotification == null)
            {
                _nowPlayingNotification = new NowPlayingNotification();
                NotificationArea.Instance.Add(_nowPlayingNotification);
            }
            else
                NotificationArea.Instance.ForceToFront(_nowPlayingNotification);
        }

        private void HideNotification()
        {
            if (_nowPlayingNotification == null)
                return;
            NotificationArea.Instance.Remove(_nowPlayingNotification);
            _nowPlayingNotification = null;
        }

        public void ShowPreparingNotification()
        {
            HideNotification();
            HidePreparingNotification(false);
            NotificationArea.Instance.Add(new PreparingPlayNotification());
        }

        public void HidePreparingNotification() => HidePreparingNotification(true);

        private void HidePreparingNotification(bool restoreNowPlayingNotification)
        {
            NotificationArea.Instance.RemoveAll(NotificationTask.PreparingPlay, NotificationState.Normal);
            if (!restoreNowPlayingNotification || _playlistCurrent == null)
                return;
            ShowNotification();
        }

        private void DeferredTransportStatusChanged(object obj)
        {
            if (IsDisposed)
                return;
            object[] objArray = (object[])obj;
            var playerState = (PlaybackState)objArray[0];
            bool endOfMedia = (bool)objArray[1];
            IsSeekEnabled = (bool)objArray[2];
            if (playerState == PlaybackState.Loading)
            {
                Buffering = true;
                UpdatePropertiesAndCommands();
            }
            else
            {
                if (Buffering)
                {
                    Buffering = false;
                    UpdatePropertiesAndCommands();
                }
                switch (playerState)
                {
                    case PlaybackState.Paused:
                        bool endOfQueue = _playlistCurrent != null && _playlistCurrent.Count == 1 && _repeating.Value && endOfMedia;
                        if (_lastKnownPlaybackTrack != null)
                        {
                            _lastKnownPlaybackTrack.OnEndPlayback(endOfMedia);
                            if (!endOfQueue)
                                _lastKnownPlaybackTrack = null;
                        }
                        if (_playlistPending != null)
                        {
                            if (_playlistPending.TrackList != null && _playlistPending.TrackList.Count > 0)
                            {
                                PlayPendingList();
                                break;
                            }
                            SetPlayerState(PlayerState.Stopped);
                            _playlistPending.PlayWhenReady = true;
                            break;
                        }
                        if (_lastKnownPreparedTrack != null)
                        {
                            _lastKnownPlaybackTrack = _lastKnownPreparedTrack;
                            _lastKnownPlaybackTrack.OnBeginPlayback(null);
                            if (Playing)
                                AsyncHelper.Run(_player.PauseAsync());
                            _lastKnownPreparedTrack = null;
                            break;
                        }
                        if (endOfQueue && _lastKnownPlaybackTrack != null)
                        {
                            _lastKnownPlaybackTrack.OnBeginPlayback(null);
                            AsyncHelper.Run(_player.SeekAsync(TimeSpan.Zero));
                            AsyncHelper.Run(_player.PauseAsync());
                            break;
                        }
                        _playlistCurrent?.ResetForReplay();
                        SetPlayerState(PlayerState.Stopped);
                        // TODO: _playbackWrapper.Close();
                        break;

                    case PlaybackState.Playing:
                        PerfTrace.TraceUICollectionEvent(UICollectionEvent.PlayRequestComplete, "");
                        UseSmtc();
                        break;
                }
            }
            _lastKnownTransportState = playerState;
        }

        bool isInit = false;
        private void UseSmtc()
        {
            // Use SMTC when available
#if WINDOWS8
            if (Microsoft.WinRT.ApiInformation.IsTypePresent("Windows.Media.SystemMediaTransportControls")
                && !OSVersion.IsLessThanWin10())
            {
                // On Windows 8.1 / 10 / 11
                var _systemMediaTransportControls = Windows.Media.SystemMediaTransportControlsInterop.GetForWindow(Application.Window.Handle);
                _systemMediaTransportControls.IsPlayEnabled = true;
                _systemMediaTransportControls.IsPauseEnabled = true;
                _systemMediaTransportControls.IsStopEnabled = true;
                _systemMediaTransportControls.IsNextEnabled = true;
                _systemMediaTransportControls.IsPreviousEnabled = true;

                if (!isInit)
                {
                    isInit = true;
                    _systemMediaTransportControls.ButtonPressed += (e, args) =>
                    {
                        Action<object, EventArgs> method = args.Button switch
                        {
                            Windows.Media.SystemMediaTransportControlsButton.Play => OnPlayClicked,
                            Windows.Media.SystemMediaTransportControlsButton.Pause => OnPauseClicked,
                            Windows.Media.SystemMediaTransportControlsButton.Stop => OnStopClicked,
                            Windows.Media.SystemMediaTransportControlsButton.Next => OnForwardClicked,
                            Windows.Media.SystemMediaTransportControlsButton.Previous => OnBackClicked,

                            _ => throw new NotImplementedException()
                        };

                        Application.DeferredInvoke(_ => method(null, null), null);
                    };
                }

                // Get the updater
                Windows.Media.SystemMediaTransportControlsDisplayUpdater updater = _systemMediaTransportControls.DisplayUpdater;

                // Music metadata
                if (_lastKnownPlaybackTrack != null)
                {
                    updater.Type = _lastKnownPlaybackTrack.MediaType == MediaType.Video
                        ? Windows.Media.MediaPlaybackType.Video
                        : Windows.Media.MediaPlaybackType.Music;
                    updater.MusicProperties.Title = _lastKnownPlaybackTrack.Title;

                    if (_lastKnownPlaybackTrack is LibraryPlaybackTrack track)
                    {
                        updater.MusicProperties.Artist = track.DisplayArtist;
                    }
                }

                updater.Update();
            }
#endif
        }

        private void UpdateSmtcState(PlayerState stateNew)
        {
#if WINDOWS8
                if (Microsoft.WinRT.ApiInformation.IsTypePresent("Windows.Media.SystemMediaTransportControls")
                    && !OSVersion.IsLessThanWin10())
                {
                    // Windows 8.1 doesn't seem to support non-Metro apps using this API.

                    // On Windows 8.1 / 10 / 11
                    var _systemMediaTransportControls = Windows.Media.SystemMediaTransportControlsInterop.GetForWindow(Application.Window.Handle);
                    _systemMediaTransportControls.PlaybackStatus = stateNew switch
                    {
                        PlayerState.Stopped => Windows.Media.MediaPlaybackStatus.Stopped,
                        PlayerState.Playing => Windows.Media.MediaPlaybackStatus.Playing,
                        PlayerState.Paused => Windows.Media.MediaPlaybackStatus.Paused,
                        _ => throw new NotImplementedException(),
                    };
                }
#endif
        }

        private void DeferredTransportPositionChanged(object obj)
        {
            if (IsDisposed || obj is not TimeSpan position)
                return;

            _lastKnownPosition = position.Ticks;
            CurrentTrackPosition = _lastKnownPosition / 1E+07f;

            if (_lastKnownPlaybackTrack == null)
                return;

            if (_lastKnownPosition > 0L)
                _consecutiveErrors = 0;

            _lastKnownPlaybackTrack.OnPositionChanged(_lastKnownPosition);
        }

        private void DeferredUriSet(object obj)
        {
            if (IsDisposed)
                return;
            object[] objArray = (object[])obj;
            string str = (string)objArray[0];
            int num1 = (int)objArray[1];
            _lastKnownPreparedTrack = null;
            int num2;
            for (num2 = 0; num2 < _tracksSubmittedToPlayer.Count; ++num2)
            {
                PlaybackTrack playbackTrack = _tracksSubmittedToPlayer[num2];
                if (playbackTrack.PlaybackID == num1)
                {
                    _lastKnownPreparedTrack = playbackTrack;
                    ++num2;
                    break;
                }
            }
            _tracksSubmittedToPlayer.RemoveRange(0, num2);
            if (_lastKnownPreparedTrack == null || _tracksSubmittedToPlayer.Count != 0)
                return;
            _playlistCurrent?.SyncCurrentTrackTo(_lastKnownPreparedTrack);
            UpdateNextTrack();
        }

        private void DeferredAlertHandler(object obj)
        {
            if (IsDisposed || obj is not Announcement announcement)
                return;
            PlaybackTrack playbackTrack = null;
            if (CurrentTrack != null && CurrentTrack.PlaybackID == announcement.PlaybackID)
            {
                playbackTrack = CurrentTrack;
            }
            else
            {
                for (int index = _tracksSubmittedToPlayer.Count - 1; index >= 0; --index)
                {
                    if (_tracksSubmittedToPlayer[index].PlaybackID == announcement.PlaybackID)
                    {
                        playbackTrack = _tracksSubmittedToPlayer[index];
                        break;
                    }
                }
            }
            if (playbackTrack != null)
            {
                ++_consecutiveErrors;
                _errors[playbackTrack] = announcement.HResult;
                _playlistCurrent?.SyncCurrentTrackTo(playbackTrack);
                FirePropertyChanged("ErrorCount");
            }
            bool flag = announcement.HResult == HRESULT._NS_E_CD_BUSY;
            if (_consecutiveErrors < 5 && !flag && Playing && (_playlistCurrent != null && _playlistCurrent.CanAdvance))
            {
                _playlistCurrent.Advance();
                // TODO: _playbackWrapper.Close();
            }
            else
            {
                SetPlayerState(PlayerState.Stopped);
                //_playbackWrapper.Close();
                if (_lastKnownPlaybackTrack != null)
                {
                    _lastKnownPlaybackTrack.OnEndPlayback(false);
                    _lastKnownPlaybackTrack = null;
                }
                if (flag || !ShowErrors)
                    return;
                ErrorDialogInfo.Show(announcement.HResult, Shell.LoadString(StringId.IDS_PLAYBACK_ERROR));
            }
        }

        private void DeferredPresentationInfoChangedHandler(object obj)
        {
            if (IsDisposed || _videoStream == null)
                return;
            PresentationInfo presentationInfo = (PresentationInfo)obj;
            _videoStream.ContentWidth = presentationInfo.ContentWidth;
            _videoStream.ContentHeight = presentationInfo.ContentHeight;
            _videoStream.ContentAspectWidth = presentationInfo.ContentAspectWidth;
            _videoStream.ContentAspectHeight = presentationInfo.ContentAspectHeight;
            _videoStream.ContentOverscanPercent = presentationInfo.NeedOverscan ? 0.1f : 0.0f;
            FirePropertyChanged("VideoStream");
        }

        private void DeferredVolumeInfoChangedHandler(object obj)
        {
            _volume.Value = (float)(_player.Volume * 100);
            FirePropertyChanged("Volume");
            _muted.Value = _player.Volume == 0;
            FirePropertyChanged("Mute");
        }

        private void DeferredCanChangeVideoRateHandler(object obj)
        {
            if (_currentTrack == null || !_currentTrack.IsVideo)
                return;

            _forward.Available = !TryGetPlayerInterop(out var playbackWrapper) && playbackWrapper.CanChangeVideoRate;
        }

        private void SetUriOnPlayer()
        {
            PlaybackTrack track = null;
            if (_playlistCurrent != null)
                track = _playlistCurrent.CurrentTrack;
            PlaybackTrack nextTrack = null;
            if (_playlistCurrent != null && _playlistCurrent.Count > 1)
                nextTrack = _playlistCurrent.NextTrack;
            if (track != null)
            {
                SetUrisOnPlayerAsync(track, nextTrack);
            }
            else
            {
                SetPlayerState(PlayerState.Stopped);
                //_playbackWrapper.Close();
                UpdatePropertiesAndCommands();
            }
        }

        private void SetNextUriOnPlayer()
        {
            PlaybackTrack nextTrack = null;
            if (_playlistCurrent != null)
                nextTrack = _playlistCurrent.NextTrack;
            if (nextTrack != null && _playlistCurrent.Count > 1)
                SetUrisOnPlayerAsync(null, nextTrack);
            //else
            //    _playbackWrapper.CancelNext();
        }

        private void SetUrisOnPlayerAsync(PlaybackTrack track, PlaybackTrack nextTrack)
        {
            int myID = ++_lastKnownSetUriCallID;

            _DEBUG_Trace("Queueing track {0}", track);

            ThreadPool.QueueUserWorkItem(args =>
            {
                while (_toDisposeOnEnd.TryPop(out var disposable))
                    disposable.Dispose();

                var dataRoot = Microsoft.Zune.Shell.ZuneApplication.DataRoot;
                var ph = Microsoft.Zune.Shell.ZuneApplication.PlaybackHandler;
                var device = ph.ActiveDevice;
                ph.ClearNext();
                ph.ClearPrevious();

                if (track != null)
                {
                    try
                    {
                        var stTrack = ((StrixPlaybackTrack)track).Track;

                        Application.DeferredInvoke(delegate
                        {
                            if (IsDisposed || myID != _lastKnownSetUriCallID)
                                return;

                            var source = ((StrixMusic.Sdk.AdapterModels.IMerged<StrixMusic.Sdk.CoreModels.ICoreTrack>)stTrack)
                                .Sources[0];
                            var mediaConfig = AsyncHelper.Run(() => source.SourceCore.GetMediaSourceAsync(source));

                            // This should set URIs, not necessarily begin playback
                            PlaybackItem playbackItem = new()
                            {
                                Track = stTrack,
                                MediaConfig = mediaConfig,
                            };
                            ph.InsertNext(0, playbackItem);
                            ph.PlayFromNext(0);
                            //AsyncHelper.Run(dataRoot.Library.PlayTrackCollectionAsync(stTrack));

                            if (mediaConfig.FileStreamSource != null)
                                _toDisposeOnEnd.Push(mediaConfig.FileStreamSource);

                            ReportStreamingAction(PlayerState.Stopped);
                            _tracksSubmittedToPlayer.Remove(track);
                            _tracksSubmittedToPlayer.Add(track);
                            if (nextTrack != null)
                                return;

                            // TODO: _playbackWrapper.CancelNext();
                            //ph.ClearNext();
                            UpdatePropertiesAndCommands();
                        }, null);
                    }
                    catch (Exception ex)
                    {
                        OnAlertSent(new Announcement()
                        {
                            HResult = ex.HResult,
                            PlaybackID = track.PlaybackID
                        });
                    }
                }

                if (nextTrack == null)
                    return;

                try
                {
                    var stTrack = ((StrixPlaybackTrack)nextTrack).Track;

                    Application.DeferredInvoke(delegate
                    {
                        if (IsDisposed || myID != _lastKnownSetUriCallID)
                            return;

                        //var source = ((StrixMusic.Sdk.AdapterModels.IMerged<StrixMusic.Sdk.CoreModels.ICoreTrack>)stTrack)
                        //        .Sources[0];
                        //var mediaConfig = AsyncHelper.Run(() => source.SourceCore.GetMediaSourceAsync(source));

                        //PlaybackItem playbackItem = new()
                        //{
                        //    Track = stTrack,
                        //    MediaConfig = mediaConfig,
                        //};

                        //ph.InsertNext(1, playbackItem);
                        //AsyncHelper.Run(dataRoot.Library.PlayTrackCollectionAsync(stTrack));

                        _tracksSubmittedToPlayer.Remove(nextTrack);
                        _tracksSubmittedToPlayer.Add(nextTrack);
                        UpdatePropertiesAndCommands();
                    }, null);
                }
                catch
                {
                }

                AsyncHelper.Run(ph.PlayFromNext(0));
            }, null);
        }

        private void UpdateNextTrack()
        {
            SetNextUriOnPlayer();
            UpdatePropertiesAndCommands();
        }

        private bool AreContextsCompatible(PlaybackContext contextCurrent, PlaybackContext contextNext) => contextCurrent == PlaybackContext.None || contextNext == contextCurrent;

        public bool IsCurrentPlaylistContextCompatible(PlaybackContext context)
        {
            return _playlistCurrent != null && AreContextsCompatible(context, _playlistCurrent.PlaybackContext);
        }

        private void UpdatePropertiesAndCommands()
        {
            bool isPlaying = _playerState == PlayerState.Playing;
            ArrayListDataSet arrayListDataSet;
            bool isPlaylist;
            bool isContextCompatible;
            PlaybackTrack playbackTrack;
            int currentTrackIndex;
            bool supportsShuffle;
            if (_playlistCurrent != null)
            {
                arrayListDataSet = _playlistCurrent.TrackList;
                isPlaylist = true;
                isContextCompatible = IsCurrentPlaylistContextCompatible(_pagePlaybackContext);
                playbackTrack = _playlistCurrent.CurrentTrack;
                currentTrackIndex = _playlistCurrent.ListIndexOfCurrentTrack;
                supportsShuffle = _playlistCurrent.QuickMixSession == null;
            }
            else
            {
                isPlaylist = false;
                arrayListDataSet = null;
                isContextCompatible = true;
                playbackTrack = null;
                currentTrackIndex = -1;
                PlayingVideo = false;
                supportsShuffle = true;
            }

            if (arrayListDataSet != _currentPlaylist)
            {
                _currentPlaylist = arrayListDataSet;
                FirePropertyChanged("CurrentPlaylist");
            }
            if (isPlaylist != _hasPlaylist)
            {
                _hasPlaylist = isPlaylist;
                FirePropertyChanged("HasPlaylist");
            }
            if (supportsShuffle != _playlistSupportsShuffle)
            {
                _playlistSupportsShuffle = supportsShuffle;
                FirePropertyChanged("PlaylistSupportsShuffle");
            }
            if (isPlaying != _isPlaying)
            {
                _isPlaying = isPlaying;
                FirePropertyChanged("Playing");
                if (_isPlaying)
                {
                    HasPlayed = true;
                    _currentPlayStartTime = DateTime.Now;
                }
                else if (_currentPlayStartTime != DateTime.MinValue)
                {
                    Telemetry.Instance.ReportPlaybackTime((int)DateTime.Now.Subtract(_currentPlayStartTime).TotalSeconds);
                    _currentPlayStartTime = DateTime.MinValue;
                }
            }
            if (currentTrackIndex != _currentTrackIndex)
            {
                if (_currentPlayStartTime != DateTime.MinValue)
                {
                    Telemetry.Instance.ReportPlaybackTime((int)DateTime.Now.Subtract(_currentPlayStartTime).TotalSeconds);
                    _currentPlayStartTime = DateTime.Now;
                }
                _currentTrackIndex = currentTrackIndex;
                FirePropertyChanged("CurrentTrackIndex");
            }
            if (!ReferenceEquals(playbackTrack, _currentTrack))
            {
                if (_currentTrack != null)
                    _currentTrack.RatingChanged.Invoked -= _currentTrackRatingChangedEventHandler;
                _currentTrack = playbackTrack;
                if (_currentTrack != null)
                    _currentTrack.RatingChanged.Invoked += _currentTrackRatingChangedEventHandler;
                FirePropertyChanged("CurrentTrack");
                OnCurrentTrackRatingChanged(this, null);
                CurrentTrackDownloadProgress = 0.0f;
                if (_currentTrack != null)
                    ShowNotification();
                else
                    HideNotification();
                ZoomScaleFactor = 0;
            }
            if (isContextCompatible != _isContextCompatible)
            {
                _isContextCompatible = isContextCompatible;
                FirePropertyChanged("IsPlaybackContextCompatible");
            }
            UpdateAvailabilityOfCommands();
            if (Playing && CurrentTrack != null && (CurrentTrack.IsVideo && CurrentTrack.IsStreaming))
            {
                _isStreamingTimeoutTimer.Enabled = false;
                IsStreamingVideo = true;
                SupressDownloads = true;
            }
            else
            {
                if (SupressDownloads)
                    _isStreamingTimeoutTimer.Enabled = true;
                IsStreamingVideo = false;
            }
        }

        private void UpdateAvailabilityOfCommands()
        {
            bool playing = Playing;
            bool hasPlaylist = _playlistCurrent != null;
            _play.Available = !playing && hasPlaylist && !Buffering;
            _pause.Available = playing && !Buffering;
            _stop.Available = hasPlaylist;
            if (_playerState == PlayerState.Stopped || Buffering)
            {
                _forward.Available = false;
                _back.Available = false;
            }
            else if (_playerState == PlayerState.Playing)
            {
                if (_currentTrack == null)
                {
                    _forward.Available = false;
                    _back.Available = false;
                }
                else
                {
                    _forward.Available = !_currentTrack.IsVideo || (TryGetPlayerInterop(out var playbackWrapper) && playbackWrapper.CanChangeVideoRate);
                    _back.Available = true;
                }
            }
            else
            {
                _forward.Available = _currentTrack == null ? _playlistCurrent != null : _playlistCurrent != null && !_currentTrack.IsVideo;
                _back.Available = _playlistCurrent != null && _playlistCurrent.CanRetreat;
            }
            UpdateTaskbarPlayer();
        }

        public static string FormatDuration(float seconds, bool prefixWithNegative) => Shell.TimeSpanToString(new TimeSpan(0, 0, (int)seconds), prefixWithNegative);

        public static string FormatDuration(float seconds) => FormatDuration(seconds, false);

        [Conditional("DEBUG_TRANSPORT")]
        private static void _DEBUG_Trace(string message, params object[] args)
        {
            Debug.WriteLine(message, args);

            var stackTrace = new StackTrace(1);
            Debug.WriteLine(stackTrace);
        }

        [Conditional("DEBUG_TRANSPORT_PROPERTIES")]
        private static void _DEBUG_TracePropChange(string name, object arg)
        {
            Debug.WriteLine($"Accessed property '{name}' with argument '{arg}'");
        }

        private struct SpectrumOutputConfig
        {
            public uint SourceId;
            public uint NumBands;
            public bool Frequency;
            public bool Waveform;
            public bool Stereo;
            public bool IsConnected;
        }

        private enum PlayerState
        {
            Stopped,
            Playing,
            Paused,
        }
    }
}
