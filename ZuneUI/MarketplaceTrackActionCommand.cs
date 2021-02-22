// Decompiled with JetBrains decompiler
// Type: ZuneUI.MarketplaceTrackActionCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;
using System;
using ZuneXml;

namespace ZuneUI
{
    public class MarketplaceTrackActionCommand : MarketplaceActionCommand
    {
        public MarketplaceTrackActionCommand() => SignIn.Instance.SignInStatusUpdatedEvent += new EventHandler(this.OnSignInEvent);

        internal Track TrackModel => this.Model as Track;

        public override void UpdateState()
        {
            base.UpdateState();
            if (this.TrackModel == null || this.CanFindInCollection || this.CanFindInHiddenCollection)
                return;
            if (!this.Downloading)
            {
                if (this.TrackModel.CanPurchaseFree)
                {
                    this.Description = Shell.LoadString(StringId.IDS_FREE);
                    this.Available = true;
                }
                else if (this.TrackModel.CanDownload)
                {
                    this.Description = Shell.LoadString(StringId.IDS_DOWNLOAD);
                    this.Available = !this.DownloadingHidden;
                }
                else if (this.TrackModel.CanPurchaseSubscriptionFree)
                {
                    this.Description = Shell.LoadString(StringId.IDS_PURCHASE_BUTTON);
                    this.Available = true;
                }
                else if (this.TrackModel.CanPurchaseAlbumOnly)
                {
                    this.Description = Shell.LoadString(StringId.IDS_ALBUM_ONLY);
                    this.Available = false;
                }
                else if (this.TrackModel.CanPurchase)
                {
                    this.Description = string.Format(Shell.LoadString(StringId.IDS_BUY), (object)this.TrackModel.PointsPrice);
                    this.Available = true;
                    this.HasPoints = this.TrackModel.HasPoints;
                }
                else if (this.TrackModel.CanSubscriptionPlay)
                {
                    this.Description = Shell.LoadString(StringId.IDS_PLAY_SONG);
                    this.Available = true;
                }
                else if (this.TrackModel.CanPreview)
                {
                    this.Description = Shell.LoadString(StringId.IDS_PREVIEW_SONG);
                    this.Available = true;
                }
                else if (this.CanFindInZuneDotNet && !FeatureEnablement.IsFeatureEnabled(Features.eMusic))
                {
                    this.Description = Shell.LoadString(StringId.IDS_MORE_INFO);
                    this.Available = true;
                }
            }
            if (this.TrackModel.CanPurchase || !((HRESULT)Download.Instance.GetErrorCode(this.Id) == HRESULT._NS_E_MEDIA_NOT_PURCHASED))
                return;
            Download.Instance.SetErrorCode(this.Id, HRESULT._S_OK.Int);
        }

        public override void FindInCollection()
        {
            if (!this.CanFindInCollection)
                return;
            MusicLibraryPage.FindInCollection(-1, -1, this.CollectionId);
        }

        protected override Microsoft.Zune.Service.EContentType ContentType => Microsoft.Zune.Service.EContentType.MusicTrack;

        protected override void OnDispose(bool fDisposing)
        {
            base.OnDispose(fDisposing);
            if (!fDisposing)
                return;
            SignIn.Instance.SignInStatusUpdatedEvent -= new EventHandler(this.OnSignInEvent);
        }

        private void OnSignInEvent(object sender, EventArgs args) => this.UpdateState();
    }
}
