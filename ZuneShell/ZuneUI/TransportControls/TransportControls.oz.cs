using Microsoft.Iris;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Util;
using MicrosoftZunePlayback;
using StrixMusic.Sdk.MediaPlayback;
using System;

namespace ZuneUI
{
    partial class TransportControls
    {
        private IPlaybackHandlerService _playbackHandler;

        private void OnInit()
        {
            _playbackHandler = ZuneApplication.PlaybackHandler;
            _playbackHandler.CurrentItemChanged += PlaybackHandler_CurrentItemChanged;
        }

        private void OnUninit()
        {
            _playbackHandler.CurrentItemChanged -= PlaybackHandler_CurrentItemChanged;
        }

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

        private async void PlaybackHandler_CurrentItemChanged(object sender, PlaybackItem e)
        {
            // NOTE: This currently does not work. Perhaps something to do with
            // the lack of a valid media ID?

            string title = $"[Strix] {e.Track.Name}";
            int num = e.Track.TrackNumber ?? 0;
            string album = e.Track.Album?.Name ?? string.Empty;

            string artist = string.Empty;
            await foreach (var artistItem in e.Track.GetArtistItemsAsync(1, 0))
                artist = artistItem.Name;

            Application.DeferredInvoke(new DeferredInvokeHandler(delegate
            {
                Microsoft.Zune.Util.Notification.ResetNowPlaying();
                Microsoft.Zune.Util.Notification.BroadcastNowPlaying(EMediaTypes.eMediaTypeAudio, album, artist, title, num, Guid.NewGuid());
            }), DeferredInvokePriority.Normal);
        }
    }
}
