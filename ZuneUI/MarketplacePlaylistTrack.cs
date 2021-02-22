// Decompiled with JetBrains decompiler
// Type: ZuneUI.MarketplacePlaylistTrack
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public class MarketplacePlaylistTrack : MarketplaceTrackActionCommand
    {
        public MarketplacePlaylistTrack() => Download.Instance.DownloadAllPendingEvent += new EventHandler(this.OnDownloadAllPendingEvent);

        protected override void OnDispose(bool disposing)
        {
            base.OnDispose(disposing);
            if (!disposing)
                return;
            Download.Instance.DownloadAllPendingEvent -= new EventHandler(this.OnDownloadAllPendingEvent);
        }

        private void OnDownloadAllPendingEvent(object sender, object args)
        {
            if (this.Downloading || this.DownloadingHidden && !this.ShowHiddenProgress)
                return;
            this.UpdateState();
        }
    }
}
