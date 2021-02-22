// Decompiled with JetBrains decompiler
// Type: ZuneUI.DownloadsPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;

namespace ZuneUI
{
    public class DownloadsPage : ZunePage
    {
        public DownloadsPage(bool collection)
        {
            this.UI = "res://ZuneMarketplaceResources!DownloadsData.uix#MarketplaceDownloads";
            if (collection || !FeatureEnablement.IsFeatureEnabled(Features.eMarketplace))
            {
                this.PivotPreference = Shell.MainFrame.Collection.Downloads;
                this.UIPath = "Collection\\Downloads";
            }
            else
            {
                this.PivotPreference = Shell.MainFrame.Marketplace.Downloads;
                this.UIPath = "Marketplace\\Downloads\\Home";
            }
            this.IsRootPage = true;
        }

        public override IPageState SaveAndRelease() => new DownloadsPageState(this);
    }
}
