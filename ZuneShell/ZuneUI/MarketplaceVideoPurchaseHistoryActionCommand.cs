// Decompiled with JetBrains decompiler
// Type: ZuneUI.MarketplaceVideoPurchaseHistoryActionCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class MarketplaceVideoPurchaseHistoryActionCommand : MarketplaceVideoHistoryActionCommand
    {
        public override void UpdateState()
        {
            base.UpdateState();
            if (!this.Downloading)
            {
                if (this.CanFindInCollectionShortcut)
                {
                    this.Description = Shell.LoadString(StringId.IDS_DOWNLOAD);
                    this.Available = true;
                }
                else if (!this.CanFindInCollection)
                {
                    if (!this.VideoHistoryModel.CanDownload && !this.VideoHistoryModel.CanPurchase)
                    {
                        this.Description = Shell.LoadString(StringId.IDS_NOT_AVAILABLE);
                        this.Available = false;
                        Download.Instance.SetHistoryErrorCode(this.Id, HRESULT._ZUNE_E_CONTENT_NOT_SUPPORTED_ON_TUNER.Int);
                    }
                    else if (!this.VideoHistoryModel.CanDownload)
                    {
                        this.Description = Shell.LoadString(StringId.IDS_PURCHASE_BUTTON);
                        this.Available = true;
                        Download.Instance.SetHistoryErrorCode(this.Id, HRESULT._NS_E_MEDIA_DOWNLOAD_MAXIMUM_EXCEEDED.Int);
                    }
                    else
                    {
                        this.Description = Shell.LoadString(StringId.IDS_DOWNLOAD);
                        this.Available = true;
                    }
                }
                else
                    this.Available = false;
            }
            else
                this.Available = false;
        }
    }
}
