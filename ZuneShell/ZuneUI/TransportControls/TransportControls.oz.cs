using Microsoft.Zune.Util;
using MicrosoftZunePlayback;
using StrixMusic.Sdk.MediaPlayback;
using System;

namespace ZuneUI
{
    partial class TransportControls
    {
        private readonly IPlaybackHandlerService _playbackHandler = Microsoft.Zune.Shell.ZuneApplication.PlaybackHandler;

        private async void OnPlayClicked(object sender, EventArgs e)
        {
            if (!_play.Available)
                return;
            SQMLog.Log(SQMDataId.PlayClicks, 1);

            if (Playing || _playlistCurrent == null)
                return;
            if (_lastKnownPlayerState != MCPlayerState.Closed)
            {
                SetPlayerState(PlayerState.Playing);

                if (_playbackHandler != null)
                {
                    await _playbackHandler.ResumeAsync();
                }

                if (!PlayingVideo || _playlistCurrent.PlayNavigationOptions != PlayNavigationOptions.NavigateVideosToNowPlaying)
                    return;
                NowPlayingLand.NavigateToLand();
            }
            else
            {
                if (_playlistPending != null)
                    _playlistPending.Dispose();
                _playlistPending = _playlistCurrent;
                PlayPendingList();
            }
        }

        private async void OnPauseClicked(object sender, EventArgs e)
        {
            if (!_pause.Available)
                return;
            SQMLog.Log(SQMDataId.PauseClicks, 1);

            if (!Playing)
                return;

            SetPlayerState(PlayerState.Paused);

            if (_playbackHandler != null)
            {
                await _playbackHandler.PauseAsync();
            }
        }

        private async void OnStopClicked(object sender, EventArgs e)
        {
            if (!_stop.Available)
                return;
            SQMLog.Log(SQMDataId.StopClicks, 1);
            if (_playlistPending != null)
                _playlistPending.Dispose();
            _playlistPending = null;
            if (_playlistCurrent != null)
                _playlistCurrent.Dispose();
            _playlistCurrent = null;
            if (_playerState != PlayerState.Stopped)
            {
                SetPlayerState(PlayerState.Stopped);

                if (_playbackHandler != null)
                {
                    await _playbackHandler.PauseAsync();
                    await _playbackHandler.SeekAsync(TimeSpan.Zero);
                }
            }
            else
                UpdatePropertiesAndCommands();
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            if (!_back.Available || _playlistCurrent == null)
                return;
            SQMLog.Log(SQMDataId.SkipBackwardClicks, 1);

            if (_lastKnownPosition > 50000000L || !_playlistCurrent.CanRetreat)
            {
                await _playbackHandler.SeekAsync(TimeSpan.Zero);
            }
            else
            {
                if (_playbackHandler != null)
                {
                    await _playbackHandler.PreviousAsync();
                }
                SetUriOnPlayer();
            }
        }

        private async void OnForwardClicked(object sender, EventArgs e)
        {
            if (!_forward.Available || _playlistCurrent == null)
                return;
            SQMLog.Log(SQMDataId.SkipForwardClicks, 1);

            if (_currentTrack != null && _currentTrack.IsVideo)
                return;
            if (_lastKnownPlaybackTrack != null)
                _lastKnownPlaybackTrack.OnSkip();
            if (_playlistCurrent.CanAdvance)
            {
                if (_playbackHandler != null && _playbackHandler.NextItems.Count > 0)
                {
                    await _playbackHandler.NextAsync();
                }

                SetUriOnPlayer();
            }
            else
            {
                SetPlayerState(PlayerState.Stopped);

                if (_playbackHandler != null)
                {
                    await _playbackHandler.PauseAsync();
                    await _playbackHandler.SeekAsync(TimeSpan.Zero);
                }
            }
        }
    }
}
