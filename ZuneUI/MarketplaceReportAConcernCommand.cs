// Decompiled with JetBrains decompiler
// Type: ZuneUI.MarketplaceReportAConcernCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Service;
using Microsoft.Zune.Util;
using System;

namespace ZuneUI
{
    public class MarketplaceReportAConcernCommand : Command
    {
        private HRESULT m_lastError;
        private EConcernType m_concernType;
        private EContentType m_contentType;
        private Guid m_mediaId;
        private string m_message;

        public MarketplaceReportAConcernCommand()
        {
        }

        public MarketplaceReportAConcernCommand(IModelItem owner)
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

        public EConcernType ConcernType
        {
            get => this.m_concernType;
            set
            {
                if (this.m_concernType == value)
                    return;
                this.m_concernType = value;
                this.FirePropertyChanged(nameof(ConcernType));
            }
        }

        public EContentType ContentType
        {
            get => this.m_contentType;
            set
            {
                if (this.m_contentType == value)
                    return;
                this.m_contentType = value;
                this.FirePropertyChanged(nameof(ContentType));
            }
        }

        public Guid MediaId
        {
            get => this.m_mediaId;
            set
            {
                if (!(this.m_mediaId != value))
                    return;
                this.m_mediaId = value;
                this.FirePropertyChanged(nameof(MediaId));
            }
        }

        public string Message
        {
            get => this.m_message;
            set
            {
                if (!(this.m_message != value))
                    return;
                this.m_message = value;
                this.FirePropertyChanged(nameof(Message));
            }
        }

        protected override void OnInvoked() => Microsoft.Zune.Service.Service.Instance.ReportAConcern(this.m_concernType, this.m_contentType, this.m_mediaId, this.m_message, new AsyncCompleteHandler(this.OnCompleteHandler));

        private void OnCompleteHandler(HRESULT hr) => Application.DeferredInvoke(new DeferredInvokeHandler(this.OnCompleteHandler), (object)hr);

        private void OnCompleteHandler(object arg)
        {
            this.LastError = (HRESULT)arg;
            if (this.Completed != null)
                this.Completed((object)this, (EventArgs)null);
            this.FirePropertyChanged("Completed");
            if (!this.LastError.IsError)
                return;
            ErrorDialogInfo.Show(this.LastError.Int, Shell.LoadString(StringId.IDS_PodcastConcernTitle));
        }
    }
}
