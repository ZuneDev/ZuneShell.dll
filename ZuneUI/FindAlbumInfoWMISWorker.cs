// Decompiled with JetBrains decompiler
// Type: ZuneUI.FindAlbumInfoWMISWorker
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Shell;
using MicrosoftZuneLibrary;

namespace ZuneUI
{
    public class FindAlbumInfoWMISWorker : ModelItem
    {
        private AlbumMetadata _metadata;
        private DataProviderQueryStatus _status;

        public void BeginGetAlbumFromWMIS(
          long wmisAlbumId,
          int wmisVolume,
          AlbumMetadata albumMetadata)
        {
            this.Status = DataProviderQueryStatus.RequestingData;
            ZuneApplication.ZuneLibrary.GetAlbumMetadataForAlbumId(wmisAlbumId, wmisVolume, albumMetadata, new GetAlbumForAlbumIdCompleteHandler(this.OnCompleteGetAlbumFromWMIS));
        }

        public void OnCompleteGetAlbumFromWMIS(
          long wmisAlbumId,
          int wmisVolume,
          int hr,
          AlbumMetadata albumMetadata)
        {
            Application.DeferredInvoke((DeferredInvokeHandler)delegate
           {
               if (hr == 0)
               {
                   this.Metadata = albumMetadata;
                   this.Status = DataProviderQueryStatus.Complete;
               }
               else
                   this.Status = DataProviderQueryStatus.Error;
           }, (object)null);
        }

        public AlbumMetadata Metadata
        {
            get => this._metadata;
            private set
            {
                this._metadata = value;
                this.FirePropertyChanged(nameof(Metadata));
            }
        }

        public DataProviderQueryStatus Status
        {
            get => this._status;
            internal set
            {
                this._status = value;
                this.FirePropertyChanged(nameof(Status));
            }
        }
    }
}
