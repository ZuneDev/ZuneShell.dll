using Microsoft.Zune.Util;
using MicrosoftZunePlayback;
using System;

namespace ZuneUI
{
    partial class TransportControls
    {
        private void OnPlayClicked(object sender, EventArgs e)
        {
            if (!this._play.Available)
                return;
            SQMLog.Log(SQMDataId.PlayClicks, 1);

            if (this.Playing || this._playlistCurrent == null)
                return;
            if (this._lastKnownPlayerState != MCPlayerState.Closed)
            {
                this.SetPlayerState(PlayerState.Playing);
                this._playbackWrapper.Play();

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

        private void OnPauseClicked(object sender, EventArgs e)
        {
            if (!this._pause.Available)
                return;
            SQMLog.Log(SQMDataId.PauseClicks, 1);

            if (!this.Playing)
                return;

            this.SetPlayerState(PlayerState.Paused);
            this._playbackWrapper.Pause();
        }

        private void OnStopClicked(object sender, EventArgs e)
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
                this._playbackWrapper.Stop();
            }
            else
                this.UpdatePropertiesAndCommands();
        }

        private void OnBackClicked(object sender, EventArgs e)
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
                this._playlistCurrent.Retreat();
                this.SetUriOnPlayer();
            }
        }

        private void OnForwardClicked(object sender, EventArgs e)
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
                this._playlistCurrent.Advance();
                this.SetUriOnPlayer();
            }
            else
            {
                this.SetPlayerState(PlayerState.Stopped);
                this._playbackWrapper.Stop();
            }
        }
    }
}
