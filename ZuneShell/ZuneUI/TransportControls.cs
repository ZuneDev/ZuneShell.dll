// Decompiled with JetBrains decompiler
// Type: ZuneUI.TransportControls
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.PerfTrace;
using Microsoft.Zune.QuickMix;
using Microsoft.Zune.Service;
using Microsoft.Zune.Util;
using MicrosoftZuneLibrary;
using MicrosoftZunePlayback;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UIXControls;

namespace ZuneUI
{
    public class TransportControls : SingletonModelItem<TransportControls>
    {
        internal const long c_TicksPerSecond = 10000000;
        private const long _rewindDelay = 50000000;
        private const float c_overscanFactor = 0.1f;
        private const int c_maxConsecutiveErrors = 5;
        private const int c_ratingUnrated = -1;
        private const string c_knownInvalidUri = ".:* INVALID URI *:.";
        private PlayerInterop _playbackWrapper;
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
        private MCPlayerState _lastKnownPlayerState;
        private MCTransportState _lastKnownTransportState;
        private long _lastKnownPosition;
        private Notification _nowPlayingNotification;
        private float _currentTrackDuration;
        private float _currentTrackPosition;
        private float _downloadProgress;
        private PlaybackTrack _lastKnownPreparedTrack;
        private PlaybackTrack _lastKnownPlaybackTrack;
        private List<PlaybackTrack> _tracksSubmittedToPlayer = new List<PlaybackTrack>(2);
        private Dictionary<PlaybackTrack, int> _errors = new Dictionary<PlaybackTrack, int>();
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
            this._playbackWrapper = PlayerInterop.Instance;
            this._taskbarPlayer = TaskbarPlayer.Instance;
            this._videoStream = new VideoStream();
            if (!this.CanRender3DVideo)
                this._videoStream.DisplayDetailsChanged += OnVideoDetailsChanged;
            this._shuffling = new BooleanChoice(this);
            this._shuffling.Value = ClientConfiguration.Playback.ModeShuffle;
            this._shuffling.ChosenChanged += OnShufflingChanged;
            this.UpdateShufflingDescription();
            this._repeating = new BooleanChoice(this);
            this._repeating.Value = ClientConfiguration.Playback.ModeLoop;
            this._repeating.ChosenChanged += OnRepeatingChanged;
            this.UpdateRepeatingDescription();
            this._muted = new BooleanChoice(this);
            this._muted.Value = ClientConfiguration.Playback.Mute;
            this._muted.ChosenChanged += OnMutingChanged;
            this.UpdateMutingDescription();
            this._showTotalTime = new BooleanChoice(this);
            this._showTotalTime.Value = ClientConfiguration.Playback.ShowTotalTime;
            this._showTotalTime.ChosenChanged += OnShowTotalTimeChanged;
            this._showNowPlayingList = new BooleanChoice(this);
            this._showNowPlayingList.Value = ClientConfiguration.Playback.ShowNowPlayingList;
            this._showNowPlayingList.ChosenChanged += OnShowNowPlayingListChanged;
            this.UpdateShowNowPlayingListDescription();
            this._fastforwarding = new BooleanChoice(this);
            this._fastforwarding.ChosenChanged += OnFastforwardingChanged;
            this._rewinding = new BooleanChoice(this);
            this._rewinding.ChosenChanged += OnRewindingChanged;
            float num = ClientConfiguration.Playback.Volume;
            if (num < 0.0 || num > 100.0)
                num = 50f;
            this._volume = new RangedValue(this);
            this._volume.MinValue = 0.0f;
            this._volume.MaxValue = 100f;
            this._volume.Value = num;
            this._volume.PropertyChanged += OnVolumeControlChanged;
            this._play = new Command(this, Shell.LoadString(StringId.IDS_PLAY), OnPlayClicked);
            this._play.Available = false;
            this._pause = new Command(this, Shell.LoadString(StringId.IDS_PAUSE), OnPauseClicked);
            this._pause.Available = false;
            this._back = new Command(this, Shell.LoadString(StringId.IDS_PREVIOUS), OnBackClicked);
            this._back.Available = false;
            this._forward = new Command(this, Shell.LoadString(StringId.IDS_NEXT), OnForwardClicked);
            this._forward.Available = false;
            this._stop = new Command(this, Shell.LoadString(StringId.IDS_STOP), OnStopClicked);
            this._stop.Available = false;
            this._fastforwardhotkey = new Command(this, new EventHandler(this.OnFastforwardHotkeyPressed));
            this._rewindhotkey = new Command(this, new EventHandler(this.OnRewindHotkeyPressed));
            this._playbackWrapper.StatusChanged += OnPlaybackStatusChanged;
            this._playbackWrapper.TransportStatusChanged += OnTransportStatusChanged;
            this._playbackWrapper.TransportPositionChanged += OnTransportPositionChanged;
            this._playbackWrapper.UriSet += OnUriSet;
            this._playbackWrapper.AlertSent += OnAlertSent;
            this._playbackWrapper.PlayerPropertyChanged += OnPlayerPropertyChanged;
            this._playbackWrapper.PlayerBandwithUpdate += OnBandwidthCapacityUpdate;
            this._lastKnownPlayerState = this._playbackWrapper.State;
            this._lastKnownTransportState = this._playbackWrapper.TransportState;
            this._lastKnownPosition = this._playbackWrapper.Position;
            this._shellInstance = (Shell)ZuneShell.DefaultInstance;
            this._shellInstance.PropertyChanged += OnShellPropertyChanged;
            this._timerDelayedConfigPersist = new Microsoft.Iris.Timer();
            this._timerDelayedConfigPersist.Interval = 500;
            this._timerDelayedConfigPersist.AutoRepeat = false;
            this._timerDelayedConfigPersist.Tick += OnDelayedConfigPersistTimerTick;
            this._playerState = PlayerState.Stopped;
            this._spectrumConfigList = new List<SpectrumOutputConfig>();
            this._isSpectrumAvailable = false;
            this.IsSeekEnabled = this._playbackWrapper.CanSeek;
            Download.Instance.DownloadProgressEvent += OnDownloadProgressed;
            this._lastKnownCurrentTrackRating = -1;
            this._currentTrackRatingChangedEventHandler = OnCurrentTrackRatingChanged;
            this._isStreamingTimeoutTimer = new Microsoft.Iris.Timer(this);
            this._isStreamingTimeoutTimer.Interval = 30000;
            this._isStreamingTimeoutTimer.AutoRepeat = false;
            this._isStreamingTimeoutTimer.Tick += OnIsStreamingTimeout;
            SignIn.Instance.SignInStatusUpdatedEvent += OnSignInEvent;
        }

        protected override void OnDispose(bool fDisposing)
        {
            base.OnDispose(fDisposing);
            if (!fDisposing)
                return;
            this.DisconnectAllSpectrumAnimationSources();
            this._isSpectrumAvailable = false;
            Microsoft.Zune.Util.Notification.ResetNowPlaying();
            if (this.WillSaveCurrentPlaylistOnShutdown())
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
            if (this._lastKnownPlaybackTrack != null)
                this._lastKnownPlaybackTrack.OnEndPlayback(false);
            this._playbackWrapper.StatusChanged -= OnPlaybackStatusChanged;
            this._playbackWrapper.TransportStatusChanged -= OnTransportStatusChanged;
            this._playbackWrapper.TransportPositionChanged -= OnTransportPositionChanged;
            this._playbackWrapper.UriSet -= OnUriSet;
            this._playbackWrapper.AlertSent -= OnAlertSent;
            this._playbackWrapper.PlayerPropertyChanged -= OnPlayerPropertyChanged;
            if (this._videoStream != null && !this.CanRender3DVideo)
                this._videoStream.DisplayDetailsChanged -= OnVideoDetailsChanged;
            PlayerInterop.Instance.Dispose();
            if (this._videoStream != null)
            {
                this._videoStream.Dispose();
                this._videoStream = null;
            }
            if (this._timerDelayedConfigPersist != null)
            {
                this._timerDelayedConfigPersist.Tick -= OnDelayedConfigPersistTimerTick;
                this._timerDelayedConfigPersist.Dispose();
                this._timerDelayedConfigPersist = null;
            }
            this._shellInstance.PropertyChanged -= OnShellPropertyChanged;
            Download.Instance.DownloadProgressEvent -= OnDownloadProgressed;
            if (this._currentTrack != null)
                this._currentTrack.RatingChanged.Invoked -= this._currentTrackRatingChangedEventHandler;
            SignIn.Instance.SignInStatusUpdatedEvent -= OnSignInEvent;
        }

        private void OnSignInEvent(object sender, EventArgs args)
        {
            if (this._playlistCurrent != null)
                this._playlistCurrent.UpdateTracks();
            if (this._playlistPending == null)
                return;
            this._playlistPending.UpdateTracks();
        }

        private void PersistSettings()
        {
            if (this._timerDelayedConfigPersist == null)
                return;
            this._timerDelayedConfigPersist.Stop();
            this._timerDelayedConfigPersist.Start();
        }

        private void OnDelayedConfigPersistTimerTick(object sender, EventArgs args)
        {
            ClientConfiguration.Playback.ModeShuffle = this._shuffling.Value;
            ClientConfiguration.Playback.ModeLoop = this._repeating.Value;
            ClientConfiguration.Playback.Mute = this._muted.Value;
            ClientConfiguration.Playback.Volume = (int)this._volume.Value;
            ClientConfiguration.Playback.ShowTotalTime = this._showTotalTime.Value;
            ClientConfiguration.Playback.ShowNowPlayingList = this._showNowPlayingList.Value;
        }

        private void OnCurrentTrackRatingChanged(object sender, EventArgs args)
        {
            int num = -1;
            if (this.CurrentTrack != null && this.CurrentTrack.CanRate)
                num = this.CurrentTrackRating;
            if (num == this._lastKnownCurrentTrackRating)
                return;
            this.FirePropertyChanged("CurrentTrackRating");
            this._lastKnownCurrentTrackRating = num;
        }

        public void TrackRatingUpdatedExternally(int mediaID, int newRating)
        {
            if (this.CurrentPlaylist == null)
                return;
            foreach (PlaybackTrack playbackTrack in CurrentPlaylist)
            {
                if (playbackTrack is LibraryPlaybackTrack libraryPlaybackTrack && libraryPlaybackTrack.MediaId == mediaID)
                    libraryPlaybackTrack.RatingUpdatedExternally(newRating);
            }
        }

        private void OnIsStreamingTimeout(object sender, EventArgs args) => this.SupressDownloads = false;

        private void DeserializeNowPlayingList(object arg)
        {
            string path = arg as string;
            object args = null;
            if (path != null)
            {
                if (File.Exists(path))
                {
                    try
                    {
                        using (Stream serializationStream = File.OpenRead(path))
                            args = new BinaryFormatter().Deserialize(serializationStream);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            if (args == null)
                return;
            Application.DeferredInvoke(new DeferredInvokeHandler(this.DeserializationComplete), args);
        }

        private void DeserializationComplete(object arg)
        {
            NowPlayingList nowPlayingList = arg as NowPlayingList;
            if (this._playlistCurrent != null || nowPlayingList == null)
                return;
            this._playlistCurrent = nowPlayingList;
            this.ShowNotification();
            this.UpdatePropertiesAndCommands();
            if (!this._resumeLastNowPlayingRequested)
                return;
            this.Play.Invoke();
        }

        public bool IsInitialized
        {
            get => this._isInitialized;
            private set
            {
                if (this._isInitialized == value)
                    return;
                this._isInitialized = value;
                this.FirePropertyChanged(nameof(IsInitialized));
            }
        }

        public bool HasPlayed
        {
            get => this._hasPlayed;
            private set
            {
                if (this._hasPlayed == value)
                    return;
                this._hasPlayed = value;
                this.FirePropertyChanged(nameof(HasPlayed));
            }
        }

        public event EventHandler PlaybackStopped;

        public BooleanChoice Shuffling => this._shuffling;

        public BooleanChoice Repeating => this._repeating;

        public BooleanChoice Muted => this._muted;

        public BooleanChoice ShowTotalTime => this._showTotalTime;

        public BooleanChoice ShowNowPlayingList => this._showNowPlayingList;

        public BooleanChoice Fastforwarding => this._fastforwarding;

        public BooleanChoice Rewinding => this._rewinding;

        public RangedValue Volume => this._volume;

        public Command Play => this._play;

        public Command Pause => this._pause;

        public Command Stop => this._stop;

        public Command Back => this._back;

        public Command Forward => this._forward;

        public bool Opening
        {
            get => this._opening;
            private set
            {
                if (this._opening == value)
                    return;
                this._opening = value;
                this.FirePropertyChanged(nameof(Opening));
            }
        }

        public bool Buffering
        {
            get => this._buffering;
            private set
            {
                if (this._buffering == value)
                    return;
                this._buffering = value;
                this.FirePropertyChanged(nameof(Buffering));
            }
        }

        public bool IsSeekEnabled
        {
            get => this._seekEnabled;
            private set
            {
                if (this._seekEnabled == value)
                    return;
                this._seekEnabled = value;
                this.FirePropertyChanged(nameof(IsSeekEnabled));
            }
        }

        public bool IsStreamingVideo
        {
            get => this._isStreamingVideo;
            private set
            {
                if (this._isStreamingVideo == value)
                    return;
                this._isStreamingVideo = value;
                this.FirePropertyChanged(nameof(IsStreamingVideo));
            }
        }

        public bool SupressDownloads
        {
            get => this._supressDownloads;
            private set
            {
                if (this._supressDownloads == value)
                    return;
                this._supressDownloads = value;
                this.FirePropertyChanged(nameof(SupressDownloads));
            }
        }

        public int ZoomScaleFactor
        {
            get => this._zoomScaleFactor;
            set
            {
                this._zoomScaleFactor = value;
                this.FirePropertyChanged(nameof(ZoomScaleFactor));
            }
        }

        public bool Playing => this._isPlaying;

        public bool PlayingVideo
        {
            get => this._playingVideo;
            private set
            {
                if (this._playingVideo == value)
                    return;
                this._playingVideo = value;
                this.FirePropertyChanged(nameof(PlayingVideo));
            }
        }

        public Command FastforwardHotkey => this._fastforwardhotkey;

        public Command RewindHotkey => this._rewindhotkey;

        public int CurrentTrackIndex => this._currentTrackIndex;

        public bool WillSaveCurrentPlaylistOnShutdown() => this._playlistCurrent != null && this._playlistCurrent.CurrentTrack != null && !this._playlistCurrent.CurrentTrack.IsVideo && this._playlistCurrent.QuickMixSession == null;

        public void StartPlayingAt(PlaybackTrack track)
        {
            if (this._playlistCurrent == null || this._playlistCurrent.TrackList == null)
                return;
            int newCurrentIndex = this._playlistCurrent.TrackList.IndexOf(track);
            if (newCurrentIndex <= -1)
                return;
            this.StartPlayingAt(newCurrentIndex);
        }

        public void StartPlayingAt(int newCurrentIndex)
        {
            if (this._playlistCurrent == null)
                return;
            this._playlistCurrent.MoveToTrackIndex(newCurrentIndex);
            if (this._playerState == PlayerState.Playing)
            {
                this.SetUriOnPlayer();
            }
            else
            {
                this._playlistPending = this._playlistCurrent;
                if (this._playerState == PlayerState.Paused)
                    this._playbackWrapper.Stop();
                else
                    this.PlayPendingList();
            }
        }

        public void CloseCurrentSession() => this.Stop.Invoke();

        public void SeekToPosition(float value)
        {
            long offsetIn100nsUnits = (long)(value * 10000000.0);
            if (this._playbackWrapper != null)
                this._playbackWrapper.SeekToAbsolutePosition(offsetIn100nsUnits);
            this._rewinding.Value = false;
            this._fastforwarding.Value = false;
        }

        public float CurrentTrackDuration
        {
            get => this._currentTrackDuration;
            private set
            {
                if (_currentTrackDuration == (double)value)
                    return;
                this._currentTrackDuration = value;
                this.FirePropertyChanged(nameof(CurrentTrackDuration));
            }
        }

        public float CurrentTrackPosition
        {
            get => this._currentTrackPosition;
            private set
            {
                if (_currentTrackPosition == (double)value)
                    return;
                this._currentTrackPosition = value;
                this.FirePropertyChanged(nameof(CurrentTrackPosition));
            }
        }

        public float CurrentTrackDownloadProgress
        {
            get => this._downloadProgress;
            private set
            {
                if (_downloadProgress == (double)value)
                    return;
                this._downloadProgress = value;
                this.FirePropertyChanged(nameof(CurrentTrackDownloadProgress));
            }
        }

        public PlaybackTrack CurrentTrack => this._currentTrack;

        public int CurrentTrackRating => this.CurrentTrack == null || !this.CurrentTrack.CanRate ? 0 : this.CurrentTrack.UserRating;

        public bool ShowErrors
        {
            get => this._showErrors;
            set
            {
                if (this._showErrors == value)
                    return;
                this._showErrors = value;
                this.FirePropertyChanged(nameof(ShowErrors));
            }
        }

        public void ClearAllErrors()
        {
            if (this._errors.Count <= 0)
                return;
            this._errors.Clear();
            this.FirePropertyChanged("ErrorCount");
        }

        public bool IsCurrentTrack(Guid zuneMediaId)
        {
            PlaybackTrack currentTrack = this.CurrentTrack;
            return currentTrack != null && !GuidHelper.IsEmpty(currentTrack.ZuneMediaId) && currentTrack.ZuneMediaId == zuneMediaId;
        }

        public int GetErrorCode(Guid zuneMediaId)
        {
            if (this._errors.Count > 0)
            {
                foreach (KeyValuePair<PlaybackTrack, int> error in this._errors)
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
            if (!(this.CurrentTrack is LibraryPlaybackTrack currentTrack))
                return this.IsCurrentTrack(zuneMediaId);
            return currentTrack.MediaId == id && currentTrack.MediaType == type;
        }

        public int GetLibraryErrorCode(int id, MediaType type)
        {
            if (this._errors.Count > 0)
            {
                foreach (KeyValuePair<PlaybackTrack, int> error in this._errors)
                {
                    if (error.Key is LibraryPlaybackTrack key && key.MediaId == id && key.MediaType == type)
                        return error.Value;
                }
            }
            return 0;
        }

        public int GetLibraryErrorCode(PlaybackTrack track)
        {
            int num;
            return this._errors.TryGetValue(track, out num) ? num : 0;
        }

        internal void ClearError(PlaybackTrack track)
        {
            if (!this._errors.Remove(track))
                return;
            this.FirePropertyChanged("ErrorCount");
        }

        public int ErrorCount => this._errors.Count;

        public VideoStream VideoStream => this._videoStream;

        public bool CanRender3DVideo => Application.RenderingType != RenderingType.GDI;

        public bool HasPlaylist => this._hasPlaylist;

        public bool PlaylistSupportsShuffle => this._playlistSupportsShuffle;

        public ArrayListDataSet CurrentPlaylist => this._currentPlaylist;

        public string QuickMixTitle
        {
            get
            {
                string str = string.Empty;
                if (this._playlistCurrent != null)
                    str = this._playlistCurrent.QuickMixTitle;
                return str;
            }
        }

        public EQuickMixType QuickMixType
        {
            get
            {
                EQuickMixType equickMixType = EQuickMixType.eQuickMixTypeInvalid;
                if (this._playlistCurrent != null)
                    equickMixType = this._playlistCurrent.QuickMixType;
                return equickMixType;
            }
        }

        public bool DontPlayMarketplaceTracks
        {
            get => this._dontPlayMarketplaceTracks;
            set
            {
                if (value == this._dontPlayMarketplaceTracks)
                    return;
                this._dontPlayMarketplaceTracks = value;
                if (this._playlistCurrent != null)
                    this._playlistCurrent.DontPlayMarketplaceTracks = this._dontPlayMarketplaceTracks;
                if (this._playlistPending == null)
                    return;
                this._playlistPending.DontPlayMarketplaceTracks = this._dontPlayMarketplaceTracks;
            }
        }

        public QuickMixSession QuickMixSession
        {
            get
            {
                QuickMixSession quickMixSession = null;
                if (this._playlistCurrent != null)
                    quickMixSession = this._playlistCurrent.QuickMixSession;
                return quickMixSession;
            }
        }

        public bool IsPlaybackContextCompatible => this._isContextCompatible;

        public JumpListPin RequestedJumpListPin
        {
            get => !this.IsInitialized ? null : this._requestedJumpListPin;
            set
            {
                if (this._requestedJumpListPin == value)
                    return;
                this._requestedJumpListPin = value;
                if (!this.IsInitialized)
                    return;
                this.FirePropertyChanged(nameof(RequestedJumpListPin));
            }
        }

        public bool ShuffleAllRequested
        {
            get => this.IsInitialized && this._shuffleAllRequested;
            set
            {
                if (this._shuffleAllRequested == value)
                    return;
                this._shuffleAllRequested = value;
                if (!this.IsInitialized)
                    return;
                this.FirePropertyChanged(nameof(ShuffleAllRequested));
            }
        }

        public int BandwidthCapacity
        {
            get => this._bandwidthCapacity;
            private set
            {
                this._bandwidthCapacity = value;
                this.FirePropertyChanged(nameof(BandwidthCapacity));
            }
        }

        public BandwidthUpdateArgs BandwidthUpdateInfo
        {
            get => this._bandwidthUpdateInfo;
            private set
            {
                this._bandwidthUpdateInfo = value;
                this.FirePropertyChanged(nameof(BandwidthUpdateInfo));
            }
        }

        private void OnBandwidthCapacityUpdate(object sender, BandwidthUpdateArgs args) => Application.DeferredInvoke(new DeferredInvokeHandler(this.OnBandwidthCapacityUpdateOnApp), args);

        private void OnBandwidthCapacityUpdateOnApp(object obj)
        {
            if (obj == null)
                return;
            BandwidthUpdateArgs bandwidthUpdateArgs = (BandwidthUpdateArgs)obj;
            if (bandwidthUpdateArgs == null || bandwidthUpdateArgs.currentState != MBRHeuristicState.Playback)
                return;
            this.BandwidthCapacity = bandwidthUpdateArgs.RecentAverageBandwidth;
            this.BandwidthUpdateInfo = bandwidthUpdateArgs;
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
               this.Stop.Invoke();
               MessageBox.Show(title, description, null);
           }, DeferredInvokePriority.Low);
        }

        private void ReportStreamingAction(PlayerState previousPlayerState)
        {
            if (this.CurrentTrack != null && this.CurrentTrack.IsVideo && this.CurrentTrack.IsStreaming)
            {
                Guid zuneMediaId = this.CurrentTrack.ZuneMediaId;
                if (zuneMediaId != this._streamingReportMediaId && this._streamingReportIsOpen)
                {
                    this._streamingReportIsOpen = false;
                    previousPlayerState = PlayerState.Stopped;
                    Service.Instance.ReportStreamingAction(EStreamingActionType.Stop, this._streamingReportMediaInstanceId, new AsyncCompleteHandler(this.OnStreamingRestrictionResponse));
                }
                if (previousPlayerState == PlayerState.Stopped && this._playerState == PlayerState.Playing)
                {
                    this._streamingReportMediaInstanceId = this.CurrentTrack.ZuneMediaInstanceId;
                    if (!(this._streamingReportMediaInstanceId != Guid.Empty))
                        return;
                    this._streamingReportIsOpen = true;
                    this._streamingReportMediaId = zuneMediaId;
                    Service.Instance.ReportStreamingAction(EStreamingActionType.Start, this._streamingReportMediaInstanceId, new AsyncCompleteHandler(this.OnStreamingRestrictionResponse));
                }
                else if (previousPlayerState == PlayerState.Paused && this._playerState == PlayerState.Playing)
                    Service.Instance.ReportStreamingAction(EStreamingActionType.Resume, this._streamingReportMediaInstanceId, new AsyncCompleteHandler(this.OnStreamingRestrictionResponse));
                else if (this._playerState == PlayerState.Paused)
                {
                    Service.Instance.ReportStreamingAction(EStreamingActionType.Pause, this._streamingReportMediaInstanceId, new AsyncCompleteHandler(this.OnStreamingRestrictionResponse));
                }
                else
                {
                    if (this._playerState != PlayerState.Stopped)
                        return;
                    this._streamingReportIsOpen = false;
                    Service.Instance.ReportStreamingAction(EStreamingActionType.Stop, this._streamingReportMediaInstanceId, new AsyncCompleteHandler(this.OnStreamingRestrictionResponse));
                }
            }
            else
            {
                if (!this._streamingReportIsOpen)
                    return;
                this._streamingReportIsOpen = false;
                Service.Instance.ReportStreamingAction(EStreamingActionType.Stop, this._streamingReportMediaInstanceId, new AsyncCompleteHandler(this.OnStreamingRestrictionResponse));
            }
        }

        private void SetPlayerState(PlayerState stateNew)
        {
            PlayerState playerState = this._playerState;
            if (stateNew != this._playerState)
            {
                this._playerState = stateNew;
                if (this._playerState == PlayerState.Stopped)
                {
                    this._rewinding.Value = false;
                    this._fastforwarding.Value = false;
                    ++this._lastKnownSetUriCallID;
                    this.FirePropertyChanged("PlaybackStopped");
                    if (this.PlaybackStopped != null)
                        this.PlaybackStopped(this, null);
                }

#if WINDOWS
                if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Media.SystemMediaTransportControls"))
                {
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
            this.UpdatePropertiesAndCommands();
            this.ReportStreamingAction(playerState);
        }

        public void PlayItem(
          object item,
          PlayNavigationOptions playNavigationOptions,
          PlaybackContext playbackContext)
        {
            ArrayListDataSet arrayListDataSet = new ArrayListDataSet();
            arrayListDataSet.Add(item);
            this.PlayItemsWorker(arrayListDataSet, -1, true, playNavigationOptions, playbackContext, null);
        }

        public void PlayItem(object item) => this.PlayItem(item, PlayNavigationOptions.NavigateVideosToNowPlaying);

        public void PlayItem(object item, PlayNavigationOptions playNavigationOptions)
        {
            ArrayListDataSet arrayListDataSet = new ArrayListDataSet();
            arrayListDataSet.Add(item);
            this.PlayItemsWorker(arrayListDataSet, -1, true, playNavigationOptions, null);
        }

        public void PlayItem(object item, PlaybackContext playbackContext)
        {
            ArrayListDataSet arrayListDataSet = new ArrayListDataSet();
            arrayListDataSet.Add(item);
            this.PlayItemsWorker(arrayListDataSet, -1, true, PlayNavigationOptions.NavigateVideosToNowPlaying, playbackContext, null);
        }

        public void PlayItems(IList items) => this.PlayItemsWorker(items, -1, true, PlayNavigationOptions.NavigateVideosToNowPlaying, null);

        public void PlayItems(IList items, PlayNavigationOptions playNavigationOptions) => this.PlayItemsWorker(items, -1, true, playNavigationOptions, null);

        public void PlayItems(
          IList items,
          PlayNavigationOptions playNavigationOptions,
          ContainerPlayMarker containerPlayMarker)
        {
            this.PlayItemsWorker(items, -1, true, playNavigationOptions, containerPlayMarker);
        }

        public void PlayItems(
          IList items,
          PlayNavigationOptions playNavigationOptions,
          PlaybackContext playbackContext)
        {
            this.PlayItemsWorker(items, -1, true, playNavigationOptions, playbackContext, null);
        }

        public void PlayItems(IList items, PlaybackContext playbackContext) => this.PlayItemsWorker(items, -1, true, PlayNavigationOptions.NavigateVideosToNowPlaying, playbackContext, null);

        public void PlayItems(IList items, int startIndex) => this.PlayItemsWorker(items, startIndex, true, PlayNavigationOptions.NavigateVideosToNowPlaying, null);

        public void PlayItems(
          IList items,
          int startIndex,
          PlayNavigationOptions playNavigationOptions,
          ContainerPlayMarker containerPlayMarker)
        {
            this.PlayItemsWorker(items, startIndex, true, PlayNavigationOptions.NavigateVideosToNowPlaying, containerPlayMarker);
        }

        public void AddToNowPlaying(IList items)
        {
            int count = this.PlayItemsWorker(items, -1, false, PlayNavigationOptions.NavigateVideosToNowPlaying, null);
            if (count <= 0)
                return;
            PlaylistManager.Instance.NotifyItemsAdded(-1, count);
        }

        private int PlayItemsWorker(
          IList items,
          int startIndex,
          bool clearQueue,
          PlayNavigationOptions playNavigationOptions,
          ContainerPlayMarker containerPlayMarker)
        {
            return this.PlayItemsWorker(items, startIndex, clearQueue, playNavigationOptions, this._shellInstance.CurrentPage.PlaybackContext, containerPlayMarker);
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
            bool flag = this._playlistCurrent != null;
            int num;
            if (clearQueue || !flag)
            {
                if (clearQueue || this._playlistPending == null)
                {
                    if (this._playlistPending != null)
                        this._playlistPending.Dispose();
                    this._playlistPending = new NowPlayingList(items, startIndex, playbackContext, playNavigationOptions, this._shuffling.Value, containerPlayMarker, this._dontPlayMarketplaceTracks);
                    num = this._playlistPending.Count;
                }
                else
                    num = this._playlistPending.AddItems(items);
                if (playbackContext == PlaybackContext.QuickMix)
                {
                    if (this._lastKnownTransportState == MCTransportState.Playing || this._lastKnownTransportState == MCTransportState.Paused)
                        this._playbackWrapper.Stop();
                    else
                        this._playlistPending.PlayWhenReady = true;
                }
                else if (this._playlistPending.Count == 0)
                {
                    this._playlistPending.Dispose();
                    this._playlistPending = null;
                    this.Stop.Invoke();
                }
                else if (flag && (this._lastKnownTransportState == MCTransportState.Playing || this._lastKnownTransportState == MCTransportState.Paused))
                    this._playbackWrapper.Stop();
                else
                    this.PlayPendingList();
            }
            else
            {
                num = this._playlistCurrent.AddItems(items);
                if (this._playlistPending != null)
                    this._playlistPending.Dispose();
                this._playlistPending = null;
                this.UpdateNextTrack();
            }
            return num;
        }

        internal void PlayPendingList()
        {
            if (this._playlistPending == null)
                return;
            if (this._playlistCurrent != null && this._playlistCurrent != this._playlistPending)
                this._playlistCurrent.Dispose();
            this._playlistCurrent = this._playlistPending;
            this._playlistPending = null;
            this._playlistCurrent.SetShuffling(this._shuffling.Value);
            this._playlistCurrent.SetRepeating(this._repeating.Value);
            this._consecutiveErrors = 0;
            if (this._errors.Count > 0)
            {
                this.FirePropertyChanged("ErrorCount");
                this._errors.Clear();
            }
            this.SetPlayerState(PlayerState.Playing);
            this.SetUriOnPlayer();
            bool flag1 = false;
            if (this._playlistCurrent != null)
            {
                PlaybackTrack currentTrack = this._playlistCurrent.CurrentTrack;
                PlayNavigationOptions navigationOptions = this._playlistCurrent.PlayNavigationOptions;
                bool flag2 = false;
                if (currentTrack != null && currentTrack.IsVideo)
                    flag1 = true;
                switch (navigationOptions)
                {
                    case PlayNavigationOptions.None:
                        if (flag2)
                        {
                            NowPlayingLand.NavigateToLand(navigationOptions == PlayNavigationOptions.NavigateToNowPlayingWithMix, true);
                            break;
                        }
                        break;
                    case PlayNavigationOptions.NavigateVideosToNowPlaying:
                        if (flag1)
                        {
                            flag2 = true;
                            goto case PlayNavigationOptions.None;
                        }
                        else
                            goto case PlayNavigationOptions.None;
                    default:
                        flag2 = true;
                        this._playlistCurrent.PlayNavigationOptions = PlayNavigationOptions.NavigateVideosToNowPlaying;
                        goto case PlayNavigationOptions.None;
                }
            }
            this.PlayingVideo = flag1;
        }

        public void RemoveFromNowPlaying(IList indices)
        {
            if (this._playlistCurrent == null)
                return;
            bool flag = this._playlistCurrent.Remove(indices);
            if (this._playlistCurrent.Count == 0)
                this.Stop.Invoke();
            else if (flag)
                this.SetUriOnPlayer();
            else
                this.UpdateNextTrack();
        }

        public void ReorderNowPlaying(IList indices, int targetIndex)
        {
            if (this._playlistCurrent == null)
                return;
            this._playlistCurrent.Reorder(indices, targetIndex);
            this.UpdateNextTrack();
        }

        public IList GetNextTracks(int count)
        {
            IList list = null;
            if (this._playlistCurrent != null)
                list = this._playlistCurrent.GetNextTracks(count);
            return list;
        }

        public IList CreateAlbumListForBackground(IList allAlbums, int totalDesired)
        {
            List<object> objectList1 = new List<object>(totalDesired);
            Dictionary<int, object> dictionary = new Dictionary<int, object>();
            if (this._playlistCurrent != null)
            {
                int num = Math.Min(this._playlistCurrent.Count, totalDesired);
                for (int itemIndex = 0; itemIndex < num; ++itemIndex)
                {
                    if (this._playlistCurrent.TrackList[itemIndex] is LibraryPlaybackTrack track && track.MediaType == MediaType.Track)
                        dictionary[track.AlbumLibraryId] = null;
                }
            }
            List<object> objectList2 = new List<object>(totalDesired);
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
                this.DisableSlowDataThumbnailExtraction(album);
            int num1 = 0;
            while (objectList1.Count < totalDesired)
                objectList1.Add(objectList1[num1++]);
            Random random = new Random();
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
            if (!(album is LibraryDataProviderItemBase providerItemBase))
                return;
            providerItemBase.SetSlowDataThumbnailExtraction(false);
        }

        public void Phase2Init()
        {
            if (!this.CanRender3DVideo)
                this._playbackWrapper.WindowHandle = Application.Window.Handle;
            else
                this._playbackWrapper.DynamicImage = this._videoStream.StreamID;
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.AsyncPhase2Init), null);
        }

        private void AsyncPhase2Init(object arg)
        {
            this._playbackWrapper.Initialize();
            Application.DeferredInvoke(new DeferredInvokeHandler(this.CompletePhase2Init), null);
        }

        private void CompletePhase2Init(object obj)
        {
            this._playbackWrapper.Volume = (int)this._volume.Value;
            this._playbackWrapper.Mute = this._muted.Value;
            this._isSpectrumAvailable = true;
            this.ConnectAllSpectrumAnimationSources();
            this._taskbarPlayer.Initialize(Application.Window.Handle, new TaskbarPlayerCommandHandler(this.OnTaskbarPlayerCommand));
            this.IsInitialized = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.DeserializeNowPlayingList), _savedNowPlayingFilename);
            if (this.RequestedJumpListPin != null)
                this.FirePropertyChanged("RequestedJumpListPin");
            if (!this.ShuffleAllRequested)
                return;
            this.FirePropertyChanged("ShuffleAllRequested");
        }

        private void OnTaskbarPlayerCommand(ETaskbarPlayerCommand command, int value)
        {
            switch (command)
            {
                case ETaskbarPlayerCommand.PC_Connect:
                    this.UpdateTaskbarPlayer();
                    break;
                case ETaskbarPlayerCommand.PC_Play:
                    this.Play.Invoke();
                    break;
                case ETaskbarPlayerCommand.PC_Pause:
                    this.Pause.Invoke();
                    break;
                case ETaskbarPlayerCommand.PC_Forward:
                    this.Forward.Invoke();
                    break;
                case ETaskbarPlayerCommand.PC_Back:
                    this.Back.Invoke();
                    break;
                case ETaskbarPlayerCommand.PC_Rate:
                    if (this.CurrentTrack == null || !this.CurrentTrack.CanRate)
                        break;
                    switch ((ETaskbarPlayerState)value)
                    {
                        case ETaskbarPlayerState.PS_RatingNotRated:
                            this.CurrentTrack.UserRating = 0;
                            break;
                        case ETaskbarPlayerState.PS_RatingLoveIt:
                            this.CurrentTrack.UserRating = 8;
                            break;
                        case ETaskbarPlayerState.PS_RatingHateIt:
                            this.CurrentTrack.UserRating = 2;
                            break;
                    }
                    this.UpdateTaskbarPlayer();
                    break;
            }
        }

        private void UpdateTaskbarPlayer()
        {
            ETaskbarPlayerState etaskbarPlayerState1 = (ETaskbarPlayerState)(0 | (this._playerState == PlayerState.Playing ? 2 : 0) | (this._playerState == PlayerState.Paused ? 4 : 0) | (this._playerState == PlayerState.Stopped ? 1 : 0));
            ETaskbarPlayerState etaskbarPlayerState2;
            if (this.CurrentTrack != null && this.CurrentTrack.CanRate)
            {
                RatingConstants userRating = (RatingConstants)this.CurrentTrack.UserRating;
                etaskbarPlayerState2 = userRating != RatingConstants.Unrated ? (userRating > RatingConstants.MaxHateIt ? etaskbarPlayerState1 | ETaskbarPlayerState.PS_RatingLoveIt : etaskbarPlayerState1 | ETaskbarPlayerState.PS_RatingHateIt) : etaskbarPlayerState1 | ETaskbarPlayerState.PS_RatingNotRated;
            }
            else
                etaskbarPlayerState2 = etaskbarPlayerState1 | ETaskbarPlayerState.PS_RatingNotRated;
            ETaskbarPlayerState state = etaskbarPlayerState2 | (this.Play.Available ? ETaskbarPlayerState.PS_CanPlay : 0) | (this.Pause.Available ? ETaskbarPlayerState.PS_CanPause : 0) | (this.Forward.Available ? ETaskbarPlayerState.PS_CanForward : 0) | (this.Back.Available ? ETaskbarPlayerState.PS_CanBack : 0);
            if (this.CurrentTrack != null && this.CurrentTrack.CanRate)
                state |= ETaskbarPlayerState.PS_CanRate;
            this._taskbarPlayer.UpdateToolbar(state);
        }

        public int CreateSpectrumAnimationSource(
          int numBands,
          bool outputFrequencyData,
          bool outputWaveformData,
          bool enableStereoOutput)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            for (int index = 0; index < numBands; ++index)
            {
                if (outputFrequencyData)
                {
                    if (!enableStereoOutput)
                    {
                        dictionary[string.Format("Frequency{0}", index)] = index;
                    }
                    else
                    {
                        dictionary[string.Format("FrequencyL{0}", index)] = index;
                        dictionary[string.Format("FrequencyR{0}", index)] = index + 1024;
                    }
                }
                if (outputWaveformData)
                {
                    if (!enableStereoOutput)
                    {
                        dictionary[string.Format("Waveform{0}", index)] = index + 2048;
                    }
                    else
                    {
                        dictionary[string.Format("WaveformL{0}", index)] = index + 2048;
                        dictionary[string.Format("WaveformR{0}", index)] = index + 3072;
                    }
                }
            }
            int externalAnimationInput = Application.CreateExternalAnimationInput(dictionary);
            SpectrumOutputConfig spectrumOutputConfig = new SpectrumOutputConfig();
            spectrumOutputConfig.SourceId = (uint)externalAnimationInput;
            spectrumOutputConfig.NumBands = (uint)numBands;
            spectrumOutputConfig.Frequency = outputFrequencyData;
            spectrumOutputConfig.Waveform = outputWaveformData;
            spectrumOutputConfig.Stereo = enableStereoOutput;
            spectrumOutputConfig.IsConnected = this._isSpectrumAvailable;
            this._spectrumConfigList.Add(spectrumOutputConfig);
            if (this._isSpectrumAvailable)
                this._playbackWrapper.ConnectAnimationsToSpectrumAnalyzer(spectrumOutputConfig.SourceId, spectrumOutputConfig.NumBands, spectrumOutputConfig.Frequency, spectrumOutputConfig.Waveform, spectrumOutputConfig.Stereo);
            return externalAnimationInput;
        }

        public void DisposeSpectrumAnimationSource(int inputSourceId)
        {
            if (inputSourceId <= 0)
                return;
            for (int index = 0; index < this._spectrumConfigList.Count; ++index)
            {
                SpectrumOutputConfig spectrumConfig = this._spectrumConfigList[index];
                if (spectrumConfig.SourceId == inputSourceId)
                {
                    if (spectrumConfig.IsConnected)
                    {
                        this._playbackWrapper.DisconnectAnimationsFromSpectrumAnalyzer(spectrumConfig.SourceId);
                        spectrumConfig.IsConnected = false;
                    }
                    Application.DisposeExternalAnimationInput(inputSourceId);
                    this._spectrumConfigList.RemoveAt(index);
                    break;
                }
            }
        }

        public void ConnectAllSpectrumAnimationSources()
        {
            for (int index = 0; index < this._spectrumConfigList.Count; ++index)
            {
                SpectrumOutputConfig spectrumConfig = this._spectrumConfigList[index];
                if (!spectrumConfig.IsConnected)
                {
                    this._playbackWrapper.ConnectAnimationsToSpectrumAnalyzer(spectrumConfig.SourceId, spectrumConfig.NumBands, spectrumConfig.Frequency, spectrumConfig.Waveform, spectrumConfig.Stereo);
                    spectrumConfig.IsConnected = true;
                    this._spectrumConfigList[index] = spectrumConfig;
                }
            }
        }

        public void DisconnectAllSpectrumAnimationSources()
        {
            if (!this._isSpectrumAvailable)
                return;
            for (int index = 0; index < this._spectrumConfigList.Count; ++index)
            {
                SpectrumOutputConfig spectrumConfig = this._spectrumConfigList[index];
                if (spectrumConfig.IsConnected)
                {
                    this._playbackWrapper.DisconnectAnimationsFromSpectrumAnalyzer(spectrumConfig.SourceId);
                    spectrumConfig.IsConnected = false;
                    this._spectrumConfigList[index] = spectrumConfig;
                }
            }
        }

        public void ResumeLastNowPlayingHandler()
        {
            if (!this.IsInitialized)
            {
                this._resumeLastNowPlayingRequested = true;
            }
            else
            {
                if (this._isPlaying)
                    return;
                this.Play.Invoke();
            }
        }

        private void OnShellPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (!(args.PropertyName == "CurrentPage"))
                return;
            this._pagePlaybackContext = this._shellInstance.CurrentPage.PlaybackContext;
            this.UpdatePropertiesAndCommands();
        }

        private void OnDownloadProgressed(Guid zuneMediaId, float percent)
        {
            if (this.CurrentTrack == null || !(this.CurrentTrack.ZuneMediaId == zuneMediaId))
                return;
            this.CurrentTrackDownloadProgress = percent;
        }

        private void OnShufflingChanged(object sender, EventArgs e)
        {
            if (this._playlistCurrent != null)
            {
                this._playlistCurrent.SetShuffling(this._shuffling.Value);
                this.UpdateNextTrack();
            }
            this.UpdateShufflingDescription();
            this.PersistSettings();
            SQMLog.Log(SQMDataId.ShuffleClicks, 1);
        }

        private void UpdateShufflingDescription() => this._shuffling.Description = Shell.LoadString(!this._shuffling.Value ? StringId.IDS_SHUFFLE_ON : StringId.IDS_SHUFFLE_OFF);

        private void OnRepeatingChanged(object sender, EventArgs e)
        {
            if (this._playlistCurrent != null)
            {
                this._playlistCurrent.SetRepeating(this._repeating.Value);
                this.UpdateNextTrack();
            }
            SQMLog.Log(SQMDataId.RepeatClicks, 1);
            this.UpdateRepeatingDescription();
            this.PersistSettings();
        }

        private void UpdateRepeatingDescription() => this._repeating.Description = Shell.LoadString(!this._repeating.Value ? StringId.IDS_REPEAT_ON : StringId.IDS_REPEAT_OFF);

        private void OnMutingChanged(object sender, EventArgs e)
        {
            this.UpdateMutingDescription();
            this._playbackWrapper.Mute = this._muted.Value;
            SQMLog.Log(SQMDataId.VolumeMuteClicks, 1);
            this.PersistSettings();
        }

        private void UpdateMutingDescription() => this._muted.Description = Shell.LoadString(!this._muted.Value ? StringId.IDS_MUTE : StringId.IDS_UNMUTE);

        private void OnShowTotalTimeChanged(object sender, EventArgs e) => this.PersistSettings();

        private void OnShowNowPlayingListChanged(object sender, EventArgs e)
        {
            this.UpdateShowNowPlayingListDescription();
            this.PersistSettings();
        }

        private void UpdateShowNowPlayingListDescription() => this._showNowPlayingList.Description = Shell.LoadString(!this._showNowPlayingList.Value ? StringId.IDS_NOWPLAYINGLIST_ON : StringId.IDS_NOWPLAYINGLIST_OFF);

        private void OnPlayingChanged(object sender, EventArgs e)
        {
        }

        private
#if OPENZUNE
            async
#endif
            void OnPlayClicked(object sender, EventArgs e)
        {
            if (!this._play.Available)
                return;
            SQMLog.Log(SQMDataId.PlayClicks, 1);

            if (this.Playing || this._playlistCurrent == null)
                return;
            if (this._lastKnownPlayerState != MCPlayerState.Closed)
            {
                this.SetPlayerState(PlayerState.Playing);

#if OPENZUNE
                var playbackHandler = Microsoft.Zune.Shell.ZuneApplication.PlaybackHandler;
                if (playbackHandler != null)
                {
                    await playbackHandler.ResumeAsync();
                }
#else
                this._playbackWrapper.Play();
#endif

                if (!this.PlayingVideo || this._playlistCurrent.PlayNavigationOptions != PlayNavigationOptions.NavigateVideosToNowPlaying)
                    return;
                NowPlayingLand.NavigateToLand();
            }
            else
            {
                if (this._playlistPending != null)
                    this._playlistPending.Dispose();
                this._playlistPending = this._playlistCurrent;
                this.PlayPendingList();
            }
        }

        private
#if OPENZUNE
            async
#endif
            void OnPauseClicked(object sender, EventArgs e)
        {
            if (!this._pause.Available)
                return;
            SQMLog.Log(SQMDataId.PauseClicks, 1);

            if (!this.Playing)
                return;

            this.SetPlayerState(PlayerState.Paused);

#if OPENZUNE
            var playbackHandler = Microsoft.Zune.Shell.ZuneApplication.PlaybackHandler;
            if (playbackHandler != null)
            {
                await playbackHandler.PauseAsync();
            }
#else
            this._playbackWrapper.Pause();
#endif
        }

        private
#if OPENZUNE
            async
#endif
            void OnStopClicked(object sender, EventArgs e)
        {
            if (!this._stop.Available)
                return;
            SQMLog.Log(SQMDataId.StopClicks, 1);
            if (this._playlistPending != null)
                this._playlistPending.Dispose();
            this._playlistPending = null;
            if (this._playlistCurrent != null)
                this._playlistCurrent.Dispose();
            this._playlistCurrent = null;
            if (this._playerState != PlayerState.Stopped)
            {
                this.SetPlayerState(PlayerState.Stopped);
#if OPENZUNE
                var playbackHandler = Microsoft.Zune.Shell.ZuneApplication.PlaybackHandler;
                if (playbackHandler != null)
                {
                    await playbackHandler.PauseAsync();
                    await playbackHandler.SeekAsync(TimeSpan.Zero);
                }
#else
                this._playbackWrapper.Stop();
#endif
            }
            else
                this.UpdatePropertiesAndCommands();
        }

        private
#if OPENZUNE
            async
#endif
            void OnBackClicked(object sender, EventArgs e)
        {
            if (!this._back.Available || this._playlistCurrent == null)
                return;
            SQMLog.Log(SQMDataId.SkipBackwardClicks, 1);

            if (this._lastKnownPosition > 50000000L || !this._playlistCurrent.CanRetreat)
            {
                this._playbackWrapper.SeekToAbsolutePosition(0L);
            }
            else
            {
#if OPENZUNE
                var playbackHandler = Microsoft.Zune.Shell.ZuneApplication.PlaybackHandler;
                if (playbackHandler != null)
                {
                    await playbackHandler.PreviousAsync();
                }
#else
                this._playlistCurrent.Retreat();
#endif
                this.SetUriOnPlayer();
            }
        }

        private
#if OPENZUNE
            async
#endif
            void OnForwardClicked(object sender, EventArgs e)
        {
            if (!this._forward.Available || this._playlistCurrent == null)
                return;
            SQMLog.Log(SQMDataId.SkipForwardClicks, 1);

            if (this._currentTrack != null && this._currentTrack.IsVideo)
                return;
            if (this._lastKnownPlaybackTrack != null)
                this._lastKnownPlaybackTrack.OnSkip();
            if (this._playlistCurrent.CanAdvance)
            {
#if OPENZUNE
                var playbackHandler = Microsoft.Zune.Shell.ZuneApplication.PlaybackHandler;
                if (playbackHandler != null && playbackHandler.NextItems.Count > 0)
                {
                    await playbackHandler.NextAsync();
                }
#else
                this._playlistCurrent.Advance();
#endif
                this.SetUriOnPlayer();
            }
            else
            {
                this.SetPlayerState(PlayerState.Stopped);
#if OPENZUNE
                var playbackHandler = Microsoft.Zune.Shell.ZuneApplication.PlaybackHandler;
                if (playbackHandler != null)
                {
                    await playbackHandler.PauseAsync();
                    await playbackHandler.SeekAsync(TimeSpan.Zero);
                }
#else
                this._playbackWrapper.Stop();
#endif
            }
        }

        private void OnFastforwardingChanged(object sender, EventArgs e)
        {
            if ((this._currentTrack == null || !this._currentTrack.IsVideo || this._playbackWrapper.CanChangeVideoRate) && this._fastforwarding.Value)
            {
                this._rewinding.Value = false;
                this._playbackWrapper.Rate = 5f;
            }
            else
                this._playbackWrapper.Rate = 1f;
        }

        private void OnRewindingChanged(object sender, EventArgs e)
        {
            if ((this._currentTrack == null || !this._currentTrack.IsVideo || this._playbackWrapper.CanChangeVideoRate) && this._rewinding.Value)
            {
                this._fastforwarding.Value = false;
                this._playbackWrapper.Rate = -5f;
            }
            else
                this._playbackWrapper.Rate = 1f;
        }

        private void OnFastforwardHotkeyPressed(object sender, EventArgs e) => this._fastforwarding.Value = !this._fastforwarding.Value;

        private void OnRewindHotkeyPressed(object sender, EventArgs e) => this._rewinding.Value = !this._rewinding.Value;

        private void OnPlaybackStatusChanged(object sender, EventArgs e) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredPlaybackStatusChanged), new object[2]
        {
       _playbackWrapper.State,
       _playbackWrapper.EndOfMedia
        });

        private void OnTransportStatusChanged(object sender, EventArgs e) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredTransportStatusChanged), new object[3]
        {
       _playbackWrapper.TransportState,
       _playbackWrapper.EndOfMedia,
       _playbackWrapper.CanSeek
        });

        private void OnTransportPositionChanged(object sender, EventArgs e) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredTransportPositionChanged), _playbackWrapper.Position);

        private void OnUriSet(object sender, EventArgs e) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredUriSet), new object[2]
        {
       _playbackWrapper.CurrentUri,
       _playbackWrapper.CurrentUriID
        });

        private void OnAlertSent(Announcement alert) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredAlertHandler), alert);

        private void OnPlayerPropertyChanged(object sender, PlayerPropertyChangedEventArgs e)
        {
            if (e.Key == "presentationinfo")
                Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredPresentationInfoChangedHandler), e.Value);
            else if (e.Key == "volumeinfo")
            {
                Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredVolumeInfoChangedHandler), e.Value);
            }
            else
            {
                if (!(e.Key == "canchangevideorate"))
                    return;
                Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredCanChangeVideoRateHandler), e.Value);
            }
        }

        private void OnVolumeControlChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(e.PropertyName == "Value"))
                return;
            this._muted.Value = false;
            this._playbackWrapper.Mute = false;
            this._playbackWrapper.Volume = (int)this._volume.Value;
            SQMLog.Log(SQMDataId.VolumeAdjustmentClicks, 1);
            this.PersistSettings();
        }

        private void OnVideoDetailsChanged(object sender, EventArgs args)
        {
            this._playbackWrapper.VideoPosition = new VideoWindow(this._videoStream.DisplayPosition.X, this._videoStream.DisplayPosition.Y, this._videoStream.DisplayPosition.X + this._videoStream.DisplaySize.Width, this._videoStream.DisplayPosition.Y + this._videoStream.DisplaySize.Height);
            this._playbackWrapper.ShowGDIVideo = this._videoStream.DisplayVisibility;
        }

        private void DeferredPlaybackStatusChanged(object obj)
        {
            if (this.IsDisposed)
                return;
            object[] objArray = (object[])obj;
            MCPlayerState mcPlayerState = (MCPlayerState)objArray[0];
            bool endOfMedia = (bool)objArray[1];
            bool flag = false;
            switch (mcPlayerState)
            {
                case MCPlayerState.Closed:
                    if (this.Playing)
                    {
                        this.SetUriOnPlayer();
                        break;
                    }
                    break;
                case MCPlayerState.Open:
                    this._rewinding.Value = false;
                    this._fastforwarding.Value = false;
                    flag = true;
                    break;
                case MCPlayerState.Built:
                    if (this.Playing && this._lastKnownPlaybackTrack != null)
                    {
                        this._lastKnownPlaybackTrack.OnEndPlayback(endOfMedia);
                        this._lastKnownPlaybackTrack = this._lastKnownPreparedTrack;
                        this._lastKnownPreparedTrack = null;
                        if (this._lastKnownPlaybackTrack != null)
                            this._lastKnownPlaybackTrack.OnBeginPlayback(this._playbackWrapper);
                    }
                    this.CurrentTrackDuration = this._playbackWrapper.Duration >= 0L ? _playbackWrapper.Duration / 1E+07f : 0.0f;
                    break;
            }
            this._lastKnownPlayerState = mcPlayerState;
            if (this._opening == flag)
                return;
            this.Opening = flag;
        }

        private void ShowNotification()
        {
            if (this._nowPlayingNotification == null)
            {
                this._nowPlayingNotification = new NowPlayingNotification();
                NotificationArea.Instance.Add(this._nowPlayingNotification);
            }
            else
                NotificationArea.Instance.ForceToFront(this._nowPlayingNotification);
        }

        private void HideNotification()
        {
            if (this._nowPlayingNotification == null)
                return;
            NotificationArea.Instance.Remove(this._nowPlayingNotification);
            this._nowPlayingNotification = null;
        }

        public void ShowPreparingNotification()
        {
            this.HideNotification();
            this.HidePreparingNotification(false);
            NotificationArea.Instance.Add(new PreparingPlayNotification());
        }

        public void HidePreparingNotification() => this.HidePreparingNotification(true);

        private void HidePreparingNotification(bool restoreNowPlayingNotification)
        {
            NotificationArea.Instance.RemoveAll(NotificationTask.PreparingPlay, NotificationState.Normal);
            if (!restoreNowPlayingNotification || this._playlistCurrent == null)
                return;
            this.ShowNotification();
        }

        private void DeferredTransportStatusChanged(object obj)
        {
            if (this.IsDisposed)
                return;
            object[] objArray = (object[])obj;
            MCTransportState mcTransportState = (MCTransportState)objArray[0];
            bool endOfMedia = (bool)objArray[1];
            this.IsSeekEnabled = (bool)objArray[2];
            if (mcTransportState == MCTransportState.Buffering)
            {
                this.Buffering = true;
                this.UpdatePropertiesAndCommands();
            }
            else
            {
                if (this.Buffering)
                {
                    this.Buffering = false;
                    this.UpdatePropertiesAndCommands();
                }
                switch (mcTransportState)
                {
                    case MCTransportState.Stopped:
                        bool flag = this._playlistCurrent != null && this._playlistCurrent.Count == 1 && this._repeating.Value && endOfMedia;
                        if (this._lastKnownPlaybackTrack != null)
                        {
                            this._lastKnownPlaybackTrack.OnEndPlayback(endOfMedia);
                            if (!flag)
                                this._lastKnownPlaybackTrack = null;
                        }
                        if (this._playlistPending != null)
                        {
                            if (this._playlistPending.TrackList != null && this._playlistPending.TrackList.Count > 0)
                            {
                                this.PlayPendingList();
                                break;
                            }
                            this.SetPlayerState(PlayerState.Stopped);
                            this._playlistPending.PlayWhenReady = true;
                            break;
                        }
                        if (this._lastKnownPreparedTrack != null)
                        {
                            this._lastKnownPlaybackTrack = this._lastKnownPreparedTrack;
                            this._lastKnownPlaybackTrack.OnBeginPlayback(this._playbackWrapper);
                            if (this.Playing)
                                this._playbackWrapper.Play();
                            this._lastKnownPreparedTrack = null;
                            break;
                        }
                        if (flag && this._lastKnownPlaybackTrack != null)
                        {
                            this._lastKnownPlaybackTrack.OnBeginPlayback(this._playbackWrapper);
                            this._playbackWrapper.SeekToAbsolutePosition(0L);
                            this._playbackWrapper.Play();
                            break;
                        }
                        if (this._playlistCurrent != null)
                            this._playlistCurrent.ResetForReplay();
                        this.SetPlayerState(PlayerState.Stopped);
                        this._playbackWrapper.Close();
                        break;
                    case MCTransportState.Playing:
                        PerfTrace.TraceUICollectionEvent(UICollectionEvent.PlayRequestComplete, "");

                        // Use SMTC when available
#if WINDOWS
                        if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Media.SystemMediaTransportControls"))
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
                            updater.Type = _lastKnownPlaybackTrack.MediaType == MediaType.Video
                                ? Windows.Media.MediaPlaybackType.Video
                                : Windows.Media.MediaPlaybackType.Music;
                            updater.MusicProperties.Title = _lastKnownPlaybackTrack.Title;
                            if (_lastKnownPlaybackTrack is LibraryPlaybackTrack track)
                            {
                                updater.MusicProperties.Artist = track.DisplayArtist;
                            }

                            updater.Update();
                        }
#endif
                        break;
                }
            }
            this._lastKnownTransportState = mcTransportState;
        }
        bool isInit = false;

        private void DeferredTransportPositionChanged(object obj)
        {
            if (this.IsDisposed)
                return;
            this._lastKnownPosition = (long)obj;
            this.CurrentTrackPosition = _lastKnownPosition / 1E+07f;
            if (this._lastKnownPlaybackTrack == null)
                return;
            if (this._lastKnownPosition > 0L)
                this._consecutiveErrors = 0;
            this._lastKnownPlaybackTrack.OnPositionChanged(this._lastKnownPosition);
        }

        private void DeferredUriSet(object obj)
        {
            if (this.IsDisposed)
                return;
            object[] objArray = (object[])obj;
            string str = (string)objArray[0];
            int num1 = (int)objArray[1];
            this._lastKnownPreparedTrack = null;
            int num2;
            for (num2 = 0; num2 < this._tracksSubmittedToPlayer.Count; ++num2)
            {
                PlaybackTrack playbackTrack = this._tracksSubmittedToPlayer[num2];
                if (playbackTrack.PlaybackID == num1)
                {
                    this._lastKnownPreparedTrack = playbackTrack;
                    ++num2;
                    break;
                }
            }
            this._tracksSubmittedToPlayer.RemoveRange(0, num2);
            if (this._lastKnownPreparedTrack == null || this._tracksSubmittedToPlayer.Count != 0)
                return;
            if (this._playlistCurrent != null)
                this._playlistCurrent.SyncCurrentTrackTo(this._lastKnownPreparedTrack);
            this.UpdateNextTrack();
        }

        private void DeferredAlertHandler(object obj)
        {
            if (this.IsDisposed || !(obj is Announcement announcement))
                return;
            PlaybackTrack playbackTrack = null;
            if (this.CurrentTrack != null && this.CurrentTrack.PlaybackID == announcement.PlaybackID)
            {
                playbackTrack = this.CurrentTrack;
            }
            else
            {
                for (int index = this._tracksSubmittedToPlayer.Count - 1; index >= 0; --index)
                {
                    if (this._tracksSubmittedToPlayer[index].PlaybackID == announcement.PlaybackID)
                    {
                        playbackTrack = this._tracksSubmittedToPlayer[index];
                        break;
                    }
                }
            }
            if (playbackTrack != null)
            {
                ++this._consecutiveErrors;
                this._errors[playbackTrack] = announcement.HResult;
                if (this._playlistCurrent != null)
                    this._playlistCurrent.SyncCurrentTrackTo(playbackTrack);
                this.FirePropertyChanged("ErrorCount");
            }
            bool flag = announcement.HResult == HRESULT._NS_E_CD_BUSY;
            if (this._consecutiveErrors < 5 && !flag && this.Playing && (this._playlistCurrent != null && this._playlistCurrent.CanAdvance))
            {
                this._playlistCurrent.Advance();
                this._playbackWrapper.Close();
            }
            else
            {
                this.SetPlayerState(PlayerState.Stopped);
                this._playbackWrapper.Close();
                if (this._lastKnownPlaybackTrack != null)
                {
                    this._lastKnownPlaybackTrack.OnEndPlayback(false);
                    this._lastKnownPlaybackTrack = null;
                }
                if (flag || !this.ShowErrors)
                    return;
                ErrorDialogInfo.Show(announcement.HResult, Shell.LoadString(StringId.IDS_PLAYBACK_ERROR));
            }
        }

        private void DeferredPresentationInfoChangedHandler(object obj)
        {
            if (this.IsDisposed || this._videoStream == null)
                return;
            PresentationInfo presentationInfo = (PresentationInfo)obj;
            this._videoStream.ContentWidth = presentationInfo.ContentWidth;
            this._videoStream.ContentHeight = presentationInfo.ContentHeight;
            this._videoStream.ContentAspectWidth = presentationInfo.ContentAspectWidth;
            this._videoStream.ContentAspectHeight = presentationInfo.ContentAspectHeight;
            this._videoStream.ContentOverscanPercent = presentationInfo.NeedOverscan ? 0.1f : 0.0f;
            this.FirePropertyChanged("VideoStream");
        }

        private void DeferredVolumeInfoChangedHandler(object obj)
        {
            this._volume.Value = _playbackWrapper.Volume;
            this.FirePropertyChanged("Volume");
            this._muted.Value = this._playbackWrapper.Mute;
            this.FirePropertyChanged("Mute");
        }

        private void DeferredCanChangeVideoRateHandler(object obj)
        {
            if (this._currentTrack == null || !this._currentTrack.IsVideo)
                return;
            this._forward.Available = this._playbackWrapper.CanChangeVideoRate;
        }

        private void SetUriOnPlayer()
        {
            PlaybackTrack track = null;
            if (this._playlistCurrent != null)
                track = this._playlistCurrent.CurrentTrack;
            PlaybackTrack nextTrack = null;
            if (this._playlistCurrent != null && this._playlistCurrent.Count > 1)
                nextTrack = this._playlistCurrent.NextTrack;
            if (track != null)
            {
                this.SetUrisOnPlayerAsync(track, nextTrack);
            }
            else
            {
                this.SetPlayerState(PlayerState.Stopped);
                this._playbackWrapper.Close();
                this.UpdatePropertiesAndCommands();
            }
        }

        private void SetNextUriOnPlayer()
        {
            PlaybackTrack nextTrack = null;
            if (this._playlistCurrent != null)
                nextTrack = this._playlistCurrent.NextTrack;
            if (nextTrack != null && this._playlistCurrent.Count > 1)
                this.SetUrisOnPlayerAsync(null, nextTrack);
            else
                this._playbackWrapper.CancelNext();
        }

        private void SetUrisOnPlayerAsync(PlaybackTrack track, PlaybackTrack nextTrack)
        {
            int myID = ++this._lastKnownSetUriCallID;
            ThreadPool.QueueUserWorkItem(args =>
            {
                if (track != null)
                {
                    string trackUri;
                    HRESULT uri = track.GetURI(out trackUri);
                    if (string.IsNullOrEmpty(trackUri))
                        trackUri = ".:* INVALID URI *:.";
                    if (uri.IsSuccess)
                        Application.DeferredInvoke(delegate
                        {
                            if (this.IsDisposed || myID != this._lastKnownSetUriCallID)
                                return;
                            this._playbackWrapper.SetUri(trackUri, 0L, track.PlaybackID);
                            this.ReportStreamingAction(PlayerState.Stopped);
                            this._tracksSubmittedToPlayer.Remove(track);
                            this._tracksSubmittedToPlayer.Add(track);
                            if (nextTrack != null)
                                return;
                            this._playbackWrapper.CancelNext();
                            this.UpdatePropertiesAndCommands();
                        }, null);
                    else
                        this.OnAlertSent(new Announcement()
                        {
                            HResult = uri.Int,
                            PlaybackID = track.PlaybackID
                        });
                }
                if (nextTrack == null)
                    return;
                string nextTrackUri;
                HRESULT uri1 = nextTrack.GetURI(out nextTrackUri);
                if (string.IsNullOrEmpty(nextTrackUri))
                    nextTrackUri = ".:* INVALID URI *:.";
                if (!uri1.IsSuccess)
                    return;
                Application.DeferredInvoke(delegate
                {
                    if (this.IsDisposed || myID != this._lastKnownSetUriCallID)
                        return;
                    this._playbackWrapper.SetNextUri(nextTrackUri, 0L, nextTrack.PlaybackID);
                    this._tracksSubmittedToPlayer.Remove(nextTrack);
                    this._tracksSubmittedToPlayer.Add(nextTrack);
                    this.UpdatePropertiesAndCommands();
                }, null);
            }, null);
        }

        private void UpdateNextTrack()
        {
            this.SetNextUriOnPlayer();
            this.UpdatePropertiesAndCommands();
        }

        private bool AreContextsCompatible(PlaybackContext contextCurrent, PlaybackContext contextNext) => contextCurrent == PlaybackContext.None || contextNext == contextCurrent;

        public bool IsCurrentPlaylistContextCompatible(PlaybackContext context)
        {
            bool flag = true;
            if (this._playlistCurrent != null)
                flag = this.AreContextsCompatible(context, this._playlistCurrent.PlaybackContext);
            return flag;
        }

        private void UpdatePropertiesAndCommands()
        {
            bool isPlaying = this._playerState == PlayerState.Playing;
            ArrayListDataSet arrayListDataSet;
            bool isPlaylist;
            bool flag3;
            PlaybackTrack playbackTrack;
            int num;
            bool supportsShuffle;
            if (this._playlistCurrent != null)
            {
                arrayListDataSet = this._playlistCurrent.TrackList;
                isPlaylist = true;
                flag3 = this.IsCurrentPlaylistContextCompatible(this._pagePlaybackContext);
                playbackTrack = this._playlistCurrent.CurrentTrack;
                num = this._playlistCurrent.ListIndexOfCurrentTrack;
                supportsShuffle = this._playlistCurrent.QuickMixSession == null;
            }
            else
            {
                isPlaylist = false;
                arrayListDataSet = null;
                flag3 = true;
                playbackTrack = null;
                num = -1;
                this.PlayingVideo = false;
                supportsShuffle = true;
            }
            if (arrayListDataSet != this._currentPlaylist)
            {
                this._currentPlaylist = arrayListDataSet;
                this.FirePropertyChanged("CurrentPlaylist");
            }
            if (isPlaylist != this._hasPlaylist)
            {
                this._hasPlaylist = isPlaylist;
                this.FirePropertyChanged("HasPlaylist");
            }
            if (supportsShuffle != this._playlistSupportsShuffle)
            {
                this._playlistSupportsShuffle = supportsShuffle;
                this.FirePropertyChanged("PlaylistSupportsShuffle");
            }
            if (isPlaying != this._isPlaying)
            {
                this._isPlaying = isPlaying;
                this.FirePropertyChanged("Playing");
                if (this._isPlaying)
                {
                    this.HasPlayed = true;
                    this._currentPlayStartTime = DateTime.Now;
                }
                else if (this._currentPlayStartTime != DateTime.MinValue)
                {
                    Telemetry.Instance.ReportPlaybackTime((int)DateTime.Now.Subtract(this._currentPlayStartTime).TotalSeconds);
                    this._currentPlayStartTime = DateTime.MinValue;
                }
            }
            if (num != this._currentTrackIndex)
            {
                if (this._currentPlayStartTime != DateTime.MinValue)
                {
                    Telemetry.Instance.ReportPlaybackTime((int)DateTime.Now.Subtract(this._currentPlayStartTime).TotalSeconds);
                    this._currentPlayStartTime = DateTime.Now;
                }
                this._currentTrackIndex = num;
                this.FirePropertyChanged("CurrentTrackIndex");
            }
            if (!ReferenceEquals(playbackTrack, _currentTrack))
            {
                if (this._currentTrack != null)
                    this._currentTrack.RatingChanged.Invoked -= this._currentTrackRatingChangedEventHandler;
                this._currentTrack = playbackTrack;
                if (this._currentTrack != null)
                    this._currentTrack.RatingChanged.Invoked += this._currentTrackRatingChangedEventHandler;
                this.FirePropertyChanged("CurrentTrack");
                this.OnCurrentTrackRatingChanged(this, null);
                this.CurrentTrackDownloadProgress = 0.0f;
                if (this._currentTrack != null)
                    this.ShowNotification();
                else
                    this.HideNotification();
                this.ZoomScaleFactor = 0;
            }
            if (flag3 != this._isContextCompatible)
            {
                this._isContextCompatible = flag3;
                this.FirePropertyChanged("IsPlaybackContextCompatible");
            }
            this.UpdateAvailabilityOfCommands();
            if (this.Playing && this.CurrentTrack != null && (this.CurrentTrack.IsVideo && this.CurrentTrack.IsStreaming))
            {
                this._isStreamingTimeoutTimer.Enabled = false;
                this.IsStreamingVideo = true;
                this.SupressDownloads = true;
            }
            else
            {
                if (this.SupressDownloads)
                    this._isStreamingTimeoutTimer.Enabled = true;
                this.IsStreamingVideo = false;
            }
        }

        private void UpdateAvailabilityOfCommands()
        {
            bool playing = this.Playing;
            bool flag = this._playlistCurrent != null;
            this._play.Available = !playing && flag && !this.Buffering;
            this._pause.Available = playing && !this.Buffering;
            this._stop.Available = flag;
            if (this._playerState == PlayerState.Stopped || this.Buffering)
            {
                this._forward.Available = false;
                this._back.Available = false;
            }
            else if (this._playerState == PlayerState.Playing)
            {
                if (this._currentTrack == null)
                {
                    this._forward.Available = false;
                    this._back.Available = false;
                }
                else
                {
                    this._forward.Available = !this._currentTrack.IsVideo || this._playbackWrapper.CanChangeVideoRate;
                    this._back.Available = true;
                }
            }
            else
            {
                this._forward.Available = this._currentTrack == null ? this._playlistCurrent != null : this._playlistCurrent != null && !this._currentTrack.IsVideo;
                this._back.Available = this._playlistCurrent != null && this._playlistCurrent.CanRetreat;
            }
            this.UpdateTaskbarPlayer();
        }

        public static string FormatDuration(float seconds, bool prefixWithNegative) => Shell.TimeSpanToString(new TimeSpan(0, 0, (int)seconds), prefixWithNegative);

        public static string FormatDuration(float seconds) => FormatDuration(seconds, false);

        [Conditional("DEBUG_TRANSPORT")]
        private static void _DEBUG_Trace(string message, params object[] args)
        {
        }

        [Conditional("DEBUG_TRANSPORT_PROPERTIES")]
        private static void _DEBUG_TracePropChange(string name, object arg)
        {
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
