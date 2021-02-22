// Decompiled with JetBrains decompiler
// Type: ZuneUI.MarketplaceActionCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Util;
using System;
using System.Collections;

namespace ZuneUI
{
    public abstract class MarketplaceActionCommand : ProgressCommand
    {
        private bool m_allowPlay = true;
        private bool m_allowDownload = true;
        private bool m_hasPoints;
        private bool m_downloading;
        private bool m_downloadingHidden;
        private bool m_showHiddenProgress;
        private int m_collectionId = -1;
        private DataProviderObject m_model;
        private Guid m_id;
        private StringId m_addToCollectionStringId = StringId.IDS_ADD_TO_COLLECTION;

        public MarketplaceActionCommand()
        {
            this.UniqueId = Guid.NewGuid();
            Download.Instance.DownloadEvent += new DownloadEventHandler(this.OnDownloadEvent);
            Download.Instance.DownloadProgressEvent += new DownloadEventProgressHandler(this.OnDownloadProgressEvent);
        }

        public virtual bool CanFindInCollection => this.m_collectionId >= 0;

        public virtual bool CanFindInCollectionShortcut => this.m_collectionId >= 0 && ZuneApplication.Service.GetMediaStatus(this.Id, this.ContentType) == EMediaStatus.StatusInCollectionShortcut;

        public virtual bool CanFindInCollectionOwned => this.m_collectionId >= 0 && ZuneApplication.Service.GetMediaStatus(this.Id, this.ContentType) == EMediaStatus.StatusInCollectionOwned;

        public virtual bool CanFindInHiddenCollection => ZuneApplication.Service.InHiddenCollection(this.Id, this.ContentType);

        public virtual bool ShowHiddenProgress
        {
            get => this.m_showHiddenProgress;
            set
            {
                if (value == this.m_showHiddenProgress)
                    return;
                this.m_showHiddenProgress = value;
                this.FirePropertyChanged(nameof(ShowHiddenProgress));
            }
        }

        public bool AllowPlay
        {
            get => this.m_allowPlay;
            set
            {
                if (value == this.m_allowPlay)
                    return;
                this.m_allowPlay = value;
                this.FirePropertyChanged(nameof(AllowPlay));
            }
        }

        public bool AllowDownload
        {
            get => this.m_allowDownload;
            set
            {
                if (this.m_allowDownload == value)
                    return;
                this.m_allowDownload = value;
                this.FirePropertyChanged(nameof(AllowDownload));
                this.UpdateState();
            }
        }

        public abstract void FindInCollection();

        public bool CanFindInZuneDotNet => this.Model.GetProperty("AlbumId") is Guid property && property != Guid.Empty;

        public bool Downloading
        {
            get => this.m_downloading;
            protected set
            {
                if (value == this.m_downloading)
                    return;
                this.m_downloading = value;
                this.FirePropertyChanged(nameof(Downloading));
            }
        }

        public bool DownloadingHidden
        {
            get => this.m_downloadingHidden;
            protected set
            {
                if (value == this.m_downloadingHidden)
                    return;
                this.m_downloadingHidden = value;
                this.FirePropertyChanged(nameof(DownloadingHidden));
            }
        }

        public void FindInZuneDotNet()
        {
            object property = this.Model.GetProperty("AlbumId");
            if (!(property is Guid guid) || !(guid != Guid.Empty))
                return;
            ZuneDotNet.ViewAlbum((Guid)property);
        }

        public void FindInDownloads()
        {
            if (!this.Downloading)
                return;
            ZuneShell.DefaultInstance.Execute("Marketplace\\Downloads\\Home", (IDictionary)null);
        }

        public virtual void UpdateState()
        {
            this.Progress = -1f;
            this.CollectionId = -1;
            this.HasPoints = false;
            this.Downloading = false;
            bool fIsDownloadPending = false;
            bool fIsHidden = false;
            if (this.AllowDownload)
            {
                int dbMediaId;
                if (ZuneApplication.Service.InVisibleCollection(this.Id, this.ContentType, out dbMediaId))
                {
                    this.Description = ZuneUI.Shell.LoadString(StringId.IDS_INCOLLECTION);
                    this.Available = true;
                    this.CollectionId = dbMediaId;
                }
                else if (ZuneApplication.Service.InHiddenCollection(this.Id, this.ContentType))
                {
                    this.Description = ZuneUI.Shell.LoadString(this.AddToCollectionStringId);
                    this.Available = true;
                }
                else if (ZuneApplication.Service.IsDownloading(this.Id, this.ContentType, out fIsDownloadPending, out fIsHidden) && (!fIsHidden || this.ShowHiddenProgress))
                {
                    this.Downloading = true;
                    this.Available = true;
                    if (fIsDownloadPending)
                    {
                        this.Description = ZuneUI.Shell.LoadString(StringId.IDS_PENDING);
                    }
                    else
                    {
                        this.Description = string.Empty;
                        DownloadTask task = DownloadManager.Instance.GetTask(this.Id.ToString());
                        if (task != null)
                            this.OnDownloadProgressEvent(this.Id, task.GetProgress());
                    }
                }
                else
                {
                    this.Description = ZuneUI.Shell.LoadString(StringId.IDS_NOT_AVAILABLE);
                    this.Available = false;
                }
            }
            else
            {
                this.Description = ZuneUI.Shell.LoadString(StringId.IDS_NOT_AVAILABLE);
                this.Available = false;
            }
            this.DownloadingHidden = fIsHidden;
        }

        private void OnDownloadProgressEvent(Guid trackId, float percent)
        {
            if (!(trackId == this.m_id) || this.DownloadingHidden && !this.ShowHiddenProgress)
                return;
            this.UpdateProgress(trackId, percent);
            this.HasPoints = false;
            if (this.SecondsToProgressivePlayback == 0 && this.AllowPlay)
                this.Description = ZuneUI.Shell.LoadString(StringId.IDS_PLAY_SONG);
            else
                this.Description = string.Format(ZuneUI.Shell.LoadString(StringId.IDS_DOWNLOAD_PROGRESS), (object)(int)percent);
            this.Downloading = true;
            this.Available = true;
        }

        private void OnDownloadEvent(Guid trackId, HRESULT hr)
        {
            if (!(trackId == this.m_id))
                return;
            if (hr == HRESULT._E_PENDING && (!this.DownloadingHidden || this.ShowHiddenProgress))
            {
                this.HasPoints = false;
                this.Description = ZuneUI.Shell.LoadString(StringId.IDS_PENDING);
                this.Downloading = true;
                this.Available = true;
            }
            else
                this.UpdateState();
        }

        public DataProviderObject Model
        {
            get => this.m_model;
            set
            {
                if (this.m_model == value)
                    return;
                this.m_model = value;
                this.m_id = value != null ? (Guid)this.m_model.GetProperty(this.ZuneMediaIdPropertyName) : Guid.Empty;
                this.UpdateProgress(Guid.Empty, -1f);
                this.UpdateState();
                this.FirePropertyChanged(nameof(Model));
            }
        }

        protected virtual string ZuneMediaIdPropertyName => "Id";

        public Guid Id
        {
            get => this.m_id;
            set
            {
                if (!(this.m_id != value))
                    return;
                this.m_id = value;
                this.FirePropertyChanged(nameof(Id));
            }
        }

        public bool HasPoints
        {
            get => this.m_hasPoints;
            set
            {
                if (this.m_hasPoints == value)
                    return;
                this.m_hasPoints = value;
                this.FirePropertyChanged(nameof(HasPoints));
            }
        }

        protected abstract EContentType ContentType { get; }

        protected int CollectionId
        {
            get => this.m_collectionId;
            set
            {
                if (this.m_collectionId == value)
                    return;
                this.m_collectionId = value;
                this.FirePropertyChanged(nameof(CollectionId));
                this.FirePropertyChanged("CanFindInCollection");
                this.FirePropertyChanged("CanFindInCollectionOwned");
                this.FirePropertyChanged("CanFindInHiddenCollection");
            }
        }

        protected override void OnDispose(bool fDisposing)
        {
            base.OnDispose(fDisposing);
            if (!fDisposing)
                return;
            Download.Instance.DownloadEvent -= new DownloadEventHandler(this.OnDownloadEvent);
            Download.Instance.DownloadProgressEvent -= new DownloadEventProgressHandler(this.OnDownloadProgressEvent);
        }

        public StringId AddToCollectionStringId
        {
            get => this.m_addToCollectionStringId;
            set
            {
                if (this.m_addToCollectionStringId == value)
                    return;
                this.m_addToCollectionStringId = value;
                this.FirePropertyChanged(nameof(AddToCollectionStringId));
            }
        }
    }
}
