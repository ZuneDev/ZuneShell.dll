// Decompiled with JetBrains decompiler
// Type: ZuneUI.MarketplaceVideoActionCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using ZuneXml;

namespace ZuneUI
{
    public class MarketplaceVideoActionCommand : MarketplaceActionCommand
    {
        private bool _enableForAlbumOnly;

        public override void FindInCollection()
        {
            if (!this.CanFindInCollection)
                return;
            VideoLibraryPage.FindInCollection(this.CollectionId);
        }

        internal Video VideoModel => (Video)this.Model;

        public bool EnableForAlbumOnly
        {
            get => this._enableForAlbumOnly;
            set
            {
                if (this._enableForAlbumOnly == value)
                    return;
                this._enableForAlbumOnly = value;
                this.FirePropertyChanged(nameof(EnableForAlbumOnly));
            }
        }

        public override void UpdateState()
        {
            base.UpdateState();
            if (this.CanFindInCollection || this.Downloading)
                return;
            if (this.VideoModel.CanPurchaseAlbumOnly)
            {
                this.Description = Shell.LoadString(StringId.IDS_ALBUM_ONLY);
                this.Available = this._enableForAlbumOnly;
            }
            else if (this.VideoModel.CanRent)
            {
                this.Description = Shell.LoadString(StringId.IDS_RENT_BUTTON);
                this.Available = true;
            }
            else if (this.VideoModel.CanPurchase)
            {
                if (this.VideoModel.PointsPrice > 0)
                    this.Description = Shell.LoadString(StringId.IDS_PURCHASE_BUTTON);
                else
                    this.Description = Shell.LoadString(StringId.IDS_FREE);
                this.Available = true;
            }
            else
            {
                this.Description = Shell.LoadString(StringId.IDS_NOT_AVAILABLE);
                this.Available = false;
            }
        }

        protected override EContentType ContentType => EContentType.Video;
    }
}
