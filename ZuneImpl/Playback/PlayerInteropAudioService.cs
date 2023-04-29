#if OPENZUNE

using MicrosoftZunePlayback;
using StrixMusic.Sdk.MediaPlayback;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Zune.Playback
{
    public class PlayerInteropAudioService : IAudioPlayerService
    {
        private readonly PlayerInterop _playbackWrapper = PlayerInterop.Instance;

        public PlaybackItem CurrentSource { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public TimeSpan Position => new(_playbackWrapper.Position);

        public PlaybackState PlaybackState => throw new NotImplementedException();

        public double Volume => _playbackWrapper.Volume / 100d;

        public double PlaybackSpeed => 1.0d;

        public event EventHandler<PlaybackItem> CurrentSourceChanged;
        public event EventHandler<float[]> QuantumProcessed;
        public event EventHandler<TimeSpan> PositionChanged;
        public event EventHandler<PlaybackState> PlaybackStateChanged;
        public event EventHandler<double> VolumeChanged;
        public event EventHandler<double> PlaybackSpeedChanged;

        public Task ChangePlaybackSpeedAsync(double speed, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task ChangeVolumeAsync(double volume, CancellationToken cancellationToken = default)
        {
            _playbackWrapper.Volume = (int)(volume * 100);
            return Task.CompletedTask;
        }

        public Task PauseAsync(CancellationToken cancellationToken = default)
        {
            _playbackWrapper.Pause();
            return Task.CompletedTask;
        }

        public Task Play(PlaybackItem sourceConfig, CancellationToken cancellationToken = default)
        {
            _playbackWrapper.Play();
            return Task.CompletedTask;
        }

        public Task Preload(PlaybackItem sourceConfig, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task ResumeAsync(CancellationToken cancellationToken = default)
        {
            _playbackWrapper.Play();
            return Task.CompletedTask;
        }

        public Task SeekAsync(TimeSpan position, CancellationToken cancellationToken = default)
        {
            if (_playbackWrapper.CanSeek)
                _playbackWrapper.SeekToAbsolutePosition(position.Ticks);
            return Task.CompletedTask;
        }
    }
}

#endif
