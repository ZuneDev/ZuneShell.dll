#if OPENZUNE

using LibVLCSharp.Shared;
using StrixMusic.Sdk.MediaPlayback;
using System;
using System.Threading;
using System.Threading.Tasks;

using StrixPlaybackState = StrixMusic.Sdk.MediaPlayback.PlaybackState;

namespace Microsoft.Zune.Playback
{
    public class VlcAudioService : IAudioPlayerService
    {
        readonly LibVLC m_vlc;
        readonly MediaPlayer m_player;
        private Media m_media;
        private PlaybackItem m_currentSource;
        private TimeSpan m_position;

        public static VlcAudioService Instance => new();

        private VlcAudioService()
        {
            Core.Initialize(@"C:\Program Files\VideoLAN\VLC");

            bool debug =
#if DEBUG
                true;
#else
                false;
#endif

            m_vlc = new(enableDebugLogs: debug);
            m_player = new(m_vlc);

            m_player.TimeChanged += (sender, e) => Position = TimeSpan.FromMilliseconds(e.Time);
        }

        public PlaybackItem CurrentSource
        {
            get => m_currentSource;
            set
            {
                m_currentSource = value;
                CurrentSourceChanged?.Invoke(this, value);
            }
        }

        public TimeSpan Position
        {
            get => m_position;
            set
            {
                m_position = value;
                PositionChanged?.Invoke(this, value);
            }
        }

        public StrixPlaybackState PlaybackState => m_player.State switch
        {
            VLCState.Playing => StrixPlaybackState.Playing,

            VLCState.Paused or
            VLCState.Stopped => StrixPlaybackState.Paused,

            VLCState.Opening or
            VLCState.Buffering => StrixPlaybackState.Loading,

            VLCState.Error => StrixPlaybackState.Failed,

            _ => StrixPlaybackState.None
        };

        public double Volume
        {
            get => m_player.Volume / 100d;
            private set
            {
                m_player.Volume = (int)(value * 100);
                VolumeChanged?.Invoke(this, value);
            }
        }

        public double PlaybackSpeed
        {
            get => m_player.Rate;
            private set
            {
                m_player.SetRate((float)value);
                VolumeChanged?.Invoke(this, value);
            }
        }

        public event EventHandler<PlaybackItem> CurrentSourceChanged;
        public event EventHandler<float[]> QuantumProcessed;
        public event EventHandler<TimeSpan> PositionChanged;
        public event EventHandler<StrixPlaybackState> PlaybackStateChanged;
        public event EventHandler<double> VolumeChanged;
        public event EventHandler<double> PlaybackSpeedChanged;

        public Task ChangePlaybackSpeedAsync(double speed, CancellationToken cancellationToken = default)
        {
            PlaybackSpeed = speed;
            return Task.CompletedTask;
        }

        public Task ChangeVolumeAsync(double volume, CancellationToken cancellationToken = default)
        {
            Volume = (float)volume;
            return Task.CompletedTask;
        }

        public Task PauseAsync(CancellationToken cancellationToken = default)
        {
            m_player.Pause();

            if (m_player.Media != null)
                PlaybackStateChanged?.Invoke(this, PlaybackState);

            return Task.CompletedTask;
        }

        public async Task Play(PlaybackItem sourceConfig, CancellationToken cancellationToken = default)
        {
            if (CurrentSource != sourceConfig)
                await Preload(sourceConfig, cancellationToken);

            if (m_player.Play())
                PlaybackStateChanged?.Invoke(this, PlaybackState);
        }

        public async Task Preload(PlaybackItem sourceConfig, CancellationToken cancellationToken = default)
        {
            PlaybackStateChanged?.Invoke(this, StrixPlaybackState.Loading);

            CurrentSource = sourceConfig;
            var uri = sourceConfig.MediaConfig.MediaSourceUri;
            var stream = sourceConfig.MediaConfig.FileStreamSource;
            
            if (uri != null)
            {
                m_media = new(m_vlc, uri);
            }
            else if (stream != null)
            {
                m_media = new(m_vlc, new StreamMediaInput(stream));
            }

            var parseStatus = await m_media.Parse(MediaParseOptions.ParseNetwork | MediaParseOptions.ParseLocal, cancellationToken: cancellationToken);
            if (parseStatus == MediaParsedStatus.Failed || parseStatus == MediaParsedStatus.Timeout)
                PlaybackStateChanged?.Invoke(this, StrixPlaybackState.Failed);
            else if (parseStatus == MediaParsedStatus.Done || parseStatus == MediaParsedStatus.Skipped)
                PlaybackStateChanged?.Invoke(this, PlaybackState);

            m_player.Media = m_media;
        }

        public Task ResumeAsync(CancellationToken cancellationToken = default)
        {
            m_player.Play();
            return Task.CompletedTask;
        }

        public Task SeekAsync(TimeSpan position, CancellationToken cancellationToken = default)
        {
            if (m_player.IsSeekable)
                m_player.SeekTo(position);

            return Task.CompletedTask;
        }
    }
}

#endif
