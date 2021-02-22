// Decompiled with JetBrains decompiler
// Type: ZuneUI.ChannelTrackActionCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using System;

namespace ZuneUI
{
    public class ChannelTrackActionCommand : MarketplaceActionCommand
    {
        private bool _lastSignedInUserHadActiveSubscription;

        public ChannelTrackActionCommand()
        {
            this.ShowHiddenProgress = true;
            SignIn.Instance.SignInStatusUpdatedEvent += new EventHandler(this.OnSignInEvent);
        }

        protected override EContentType ContentType => EContentType.MusicTrack;

        public override void UpdateState()
        {
            this.Progress = -1f;
            this.Downloading = false;
            this._lastSignedInUserHadActiveSubscription = SignIn.Instance.LastSignedInUserHadActiveSubscription;
            bool fIsDownloadPending = false;
            bool fIsHidden = false;
            this.CollectionId = (int)this.Model.GetProperty("MediaId");
            if (this.CanAddToCollection)
            {
                this.Description = ZuneUI.Shell.LoadString(StringId.IDS_ADD_TO_COLLECTION);
                this.Available = true;
            }
            else if (this.CanFindInCollection)
            {
                this.Description = ZuneUI.Shell.LoadString(StringId.IDS_INCOLLECTION);
                this.Available = true;
            }
            else if (ZuneApplication.Service.IsDownloading(this.Id, EContentType.MusicTrack, out fIsDownloadPending, out fIsHidden))
            {
                this.Description = ZuneUI.Shell.LoadString(StringId.IDS_PENDING);
                this.Downloading = true;
                this.Available = true;
            }
            else if (this.CanDownload)
            {
                this.Description = ZuneUI.Shell.LoadString(StringId.IDS_DOWNLOAD);
                this.Available = true;
            }
            else if (this.CanPurchase)
            {
                this.Description = ZuneUI.Shell.LoadString(StringId.IDS_PURCHASE_BUTTON);
                this.Available = true;
            }
            else
                this.Available = false;
        }

        public override bool CanFindInCollection => PlaylistManager.IsInVisibleCollection(this.CollectionId, MediaType.Track);

        public override void FindInCollection() => MusicLibraryPage.FindInCollection(-1, -1, this.CollectionId);

        public bool CanAddToCollection => this._lastSignedInUserHadActiveSubscription && ZuneApplication.Service.InHiddenCollection(this.Id, EContentType.MusicTrack);

        public bool CanDownload => this._lastSignedInUserHadActiveSubscription && !this.CanFindInCollection && (!this.CanAddToCollection && !this.Downloading) && this.Id != Guid.Empty && Download.Instance.GetErrorCode(this.Id) != HRESULT._ZUNE_E_NO_SUBSCRIPTION_DOWNLOAD_RIGHTS.Int;

        public bool CanPurchase => this.Id != Guid.Empty;

        protected override string ZuneMediaIdPropertyName => "ZuneMediaId";

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
