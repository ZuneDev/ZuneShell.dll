#if OPENZUNE

using CommunityToolkit.Diagnostics;
using NAudio.Wave;
using StrixMusic.Sdk.MediaPlayback;
using System;
using System.Threading;
using System.Threading.Tasks;

using StrixPlaybackState = StrixMusic.Sdk.MediaPlayback.PlaybackState;

namespace Microsoft.Zune.Playback
{
    public class MediaFoundationAudioService : IAudioPlayerService
    {
        readonly WaveOutEvent m_audio = new();
        private PlaybackItem m_currentSource;

        public static MediaFoundationAudioService Instance => new();

        private MediaFoundationAudioService() { }

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

        public StrixPlaybackState PlaybackState => m_audio.PlaybackState switch
        {
            NAudio.Wave.PlaybackState.Playing => StrixPlaybackState.Playing,

            NAudio.Wave.PlaybackState.Paused or
            NAudio.Wave.PlaybackState.Stopped => StrixPlaybackState.Paused,

            _ => StrixPlaybackState.None
        };

        public double Volume
        {
            get => m_audio.Volume;
            private set
            {
                m_audio.Volume = (float)value;
                VolumeChanged?.Invoke(this, value);
            }
        }

        public double PlaybackSpeed { get; private set; }

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
            m_audio.Pause();
            return Task.CompletedTask;
        }

        public async Task Play(PlaybackItem sourceConfig, CancellationToken cancellationToken = default)
        {
            WaveStream reader = null;
            var stream = sourceConfig.MediaConfig.FileStreamSource;
            var uri = sourceConfig.MediaConfig.MediaSourceUri;

            if (stream != null)
            {
                reader = new StreamMediaFoundationReader(stream);
            }
            else if (uri.IsFile)
            {
                reader = new MediaFoundationReader(uri.ToString());
            }
            else if (uri.Scheme.StartsWith("http"))
            {
                // Use this URI for testing: https://www.newgrounds.com/audio/download/641172

                System.Net.Http.HttpClient client = new();

                var httpStream = await client.GetStreamAsync(uri
#if NET5_0_OR_GREATER
                    , cancellationToken);
#else
                    );
#endif
                cancellationToken.ThrowIfCancellationRequested();

                // NAudio requires some fancy stuff to stream from the web. This is
                // a relatively simple workaround that downloads the entire file
                // into memory before playing.
                stream = new System.IO.MemoryStream();
                await httpStream.CopyToAsync(stream
#if NET5_0_OR_GREATER
                    , cancellationToken);
#else
                    );
#endif
                cancellationToken.ThrowIfCancellationRequested();

                reader = new StreamMediaFoundationReader(stream);
            }

            m_audio.Init(reader);
            m_audio.Play();
        }

        public Task Preload(PlaybackItem sourceConfig, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task ResumeAsync(CancellationToken cancellationToken = default)
        {
            m_audio.Play();
            return Task.CompletedTask;
        }

        public Task SeekAsync(TimeSpan position, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}

#endif
