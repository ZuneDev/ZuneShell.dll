// Decompiled with JetBrains decompiler
// Type: ZuneUI.WirelessSyncWizard
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class WirelessSyncWizard : Wizard
    {
        private string _progressText;

        public WirelessSyncWizard()
        {
            this.AddPage(new WirelessSyncUseExistingPage(this));
            this.AddPage(new WirelessSyncChooseNetworkPage(this));
            this.AddPage(new WirelessSyncSummaryPage(this));
            this.AddPage(new WirelessSyncErrorPage(this));
        }

        public string ProgressText
        {
            get => this._progressText;
            set
            {
                if (!(this._progressText != value))
                    return;
                this._progressText = value;
                this.FirePropertyChanged(nameof(ProgressText));
            }
        }
    }
}
