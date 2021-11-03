// Decompiled with JetBrains decompiler
// Type: ZuneUI.MarketplaceVideoHistoryActionCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using ZuneXml;

namespace ZuneUI
{
    public abstract class MarketplaceVideoHistoryActionCommand : MarketplaceActionCommand
    {
        public MarketplaceVideoHistoryActionCommand() => this.AllowPlay = false;

        public override void FindInCollection()
        {
            if (!this.CanFindInCollection)
                return;
            VideoLibraryPage.FindInCollection(this.CollectionId);
        }

        internal VideoHistory VideoHistoryModel => (VideoHistory)this.Model;

        protected override void OnInvoked()
        {
            if (this.Downloading)
                return;
            base.OnInvoked();
        }

        protected override EContentType ContentType => EContentType.Video;
    }
}
