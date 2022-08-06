#if OPENZUNE

using LibVLCSharp.Shared;
using NAudio.Wave;
using StrixMusic.Sdk.MediaPlayback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public static VlcAudioService Instance => new();

        private VlcAudioService()
        {
            Core.Initialize();

            bool debug =
#if DEBUG
                true;
#else
                false;
#endif

            m_vlc = new(enableDebugLogs: debug);
            m_player = new(m_vlc);
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

        public TimeSpan Position { get; private set; }

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
            return Task.CompletedTask;
        }

        public async Task Play(PlaybackItem sourceConfig, CancellationToken cancellationToken = default)
        {
            if (CurrentSource != sourceConfig)
                await Preload(sourceConfig, cancellationToken);

            m_player.Play(m_media);
        }

        public async Task Preload(PlaybackItem sourceConfig, CancellationToken cancellationToken = default)
        {
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

            await m_media.Parse(MediaParseOptions.ParseNetwork | MediaParseOptions.ParseLocal, cancellationToken: cancellationToken);
        }

        public Task ResumeAsync(CancellationToken cancellationToken = default)
        {
            m_player.Play();
            return Task.CompletedTask;
        }

        public Task SeekAsync(TimeSpan position, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}

#endif
