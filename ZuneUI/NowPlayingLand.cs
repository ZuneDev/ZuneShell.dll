// Decompiled with JetBrains decompiler
// Type: ZuneUI.NowPlayingLand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;

namespace ZuneUI
{
    public class NowPlayingLand : PlaybackPage
    {
        public NowPlayingLand()
        {
            this.BackgroundUI = NowPlayingLand.LandBackgroundUI;
            this.UIPath = "NowPlaying";
        }

        protected override void OnNavigatedAwayWorker(IPage destination)
        {
            this.ShouldHandleEscape = false;
            base.OnNavigatedAwayWorker(destination);
        }

        public static void NavigateToLand()
        {
            PlaybackTrack currentTrack = SingletonModelItem<TransportControls>.Instance.CurrentTrack;
            bool exitOnPlaybackStopped = false;
            if (currentTrack != null && currentTrack.IsVideo)
                exitOnPlaybackStopped = true;
            NowPlayingLand.NavigateToLand(false, exitOnPlaybackStopped);
        }

        public static void NavigateToLand(bool showMix, bool exitOnPlaybackStopped)
        {
            if (ZuneShell.DefaultInstance.CurrentPage is NowPlayingLand)
                return;
            NowPlayingLand nowPlayingLand = new NowPlayingLand();
            nowPlayingLand.ShowMixOnEntry = showMix;
            nowPlayingLand.ExitOnPlaybackStopped = exitOnPlaybackStopped;
            ZuneShell.DefaultInstance.NavigateToPage((ZunePage)nowPlayingLand);
            SQMLog.Log(SQMDataId.NowPlayingClicks, 1);
        }

        private static string LandBackgroundUI => "res://ZuneShellResources!NowPlayingLand.uix#NowPlayingBackground";
    }
}
