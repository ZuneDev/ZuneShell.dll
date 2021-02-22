// Decompiled with JetBrains decompiler
// Type: ZuneUI.MarketplaceReportFavouriteArtistsCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;
using System;
using System.Collections;

namespace ZuneUI
{
    public class MarketplaceReportFavouriteArtistsCommand : Command
    {
        private HRESULT m_lastError;
        private Guid m_userId;
        private IList m_artists;

        public MarketplaceReportFavouriteArtistsCommand()
        {
        }

        public MarketplaceReportFavouriteArtistsCommand(IModelItem owner)
          : base((IModelItemOwner)owner)
        {
        }

        public event EventHandler Completed;

        public HRESULT LastError
        {
            get => this.m_lastError;
            private set
            {
                if (!(this.m_lastError != value))
                    return;
                this.m_lastError = value;
                this.FirePropertyChanged(nameof(LastError));
            }
        }

        public Guid UserId
        {
            get => this.m_userId;
            set
            {
                if (!(this.m_userId != value))
                    return;
                this.m_userId = value;
                this.FirePropertyChanged(nameof(UserId));
            }
        }

        public IList Artists
        {
            get => this.m_artists;
            set
            {
                if (this.m_artists == value)
                    return;
                this.m_artists = value;
                this.FirePropertyChanged(nameof(Artists));
            }
        }

        protected override void OnInvoked() => Microsoft.Zune.Service.Service.Instance.ReportFavouriteArtists(this.m_userId, this.m_artists, new AsyncCompleteHandler(this.OnCompleteHandler));

        private void OnCompleteHandler(HRESULT hr) => Application.DeferredInvoke(new DeferredInvokeHandler(this.OnCompleteHandler), (object)hr);

        private void OnCompleteHandler(object arg)
        {
            this.LastError = (HRESULT)arg;
            if (this.Completed != null)
                this.Completed((object)this, (EventArgs)null);
            this.FirePropertyChanged("Completed");
            if (!this.LastError.IsError)
                return;
            ErrorDialogInfo.Show(this.LastError.Int, Shell.LoadString(StringId.IDS_ArtistChooserUploadFailedDialogTitle));
        }
    }
}
