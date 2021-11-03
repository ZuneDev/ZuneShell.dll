// Decompiled with JetBrains decompiler
// Type: ZuneUI.LibraryVideoActionCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Util;

namespace ZuneUI
{
    public class LibraryVideoActionCommand : MarketplaceActionCommand
    {
        public override bool CanFindInCollection => false;

        protected override string ZuneMediaIdPropertyName => "ZuneMediaId";

        public override void FindInCollection()
        {
        }

        public override void UpdateState()
        {
            this.Progress = -1f;
            this.CollectionId = -1;
            this.HasPoints = false;
            this.Downloading = false;
            bool fIsDownloadPending = false;
            bool fIsHidden = false;
            if (this.AllowDownload)
            {
                if (ZuneApplication.Service.IsDownloading(this.Id, this.ContentType, out fIsDownloadPending, out fIsHidden))
                {
                    this.Downloading = true;
                    if (fIsDownloadPending)
                    {
                        this.Description = Shell.LoadString(StringId.IDS_PENDING);
                    }
                    else
                    {
                        this.Description = string.Empty;
                        DownloadTask task = DownloadManager.Instance.GetTask(this.Id.ToString());
                        if (task == null)
                            return;
                        this.UpdateProgress(this.Id, task.GetProgress());
                    }
                }
                else
                {
                    this.Description = Shell.LoadString(StringId.IDS_DOWNLOAD);
                    this.Downloading = false;
                }
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
