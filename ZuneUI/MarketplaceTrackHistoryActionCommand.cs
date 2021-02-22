// Decompiled with JetBrains decompiler
// Type: ZuneUI.MarketplaceTrackHistoryActionCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using ZuneXml;

namespace ZuneUI
{
    public class MarketplaceTrackHistoryActionCommand : MarketplaceTrackActionCommand
    {
        internal TrackHistory TrackHistoryModel => (TrackHistory)this.Model;

        public override void UpdateState()
        {
            base.UpdateState();
            if (!this.CanFindInCollection && !this.Downloading)
            {
                if (!this.TrackHistoryModel.CanDownload)
                {
                    this.Description = Shell.LoadString(StringId.IDS_PURCHASE_BUTTON);
                    Download.Instance.SetHistoryErrorCode(this.Id, HRESULT._NS_E_MEDIA_DOWNLOAD_MAXIMUM_EXCEEDED.Int);
                }
                else
                    this.Description = Shell.LoadString(StringId.IDS_DOWNLOAD);
                this.Available = true;
            }
            else
                this.Available = false;
        }

        protected override EContentType ContentType => EContentType.MusicTrack;
    }
}
