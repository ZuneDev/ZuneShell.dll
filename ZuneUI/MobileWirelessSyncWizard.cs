// Decompiled with JetBrains decompiler
// Type: ZuneUI.MobileWirelessSyncWizard
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class MobileWirelessSyncWizard : Wizard
    {
        private MobileWirelessSync _mobileWirelessSync;

        public MobileWirelessSyncWizard()
        {
            this.AddPage((WizardPage)new MobileWirelessSyncConfirmPage((Wizard)this));
            this.AddPage((WizardPage)new MobileWirelessSyncSummaryPage((Wizard)this));
            this.AddPage((WizardPage)new MobileWirelessSyncErrorPage((Wizard)this));
        }

        protected override void OnSetError(HRESULT hr, object state)
        {
            if (hr == HRESULT._S_OK)
                this.ErrorPageIsEnabled = false;
            base.OnSetError(hr, state);
        }

        public MobileWirelessSync MobileWirelessSync
        {
            get => this._mobileWirelessSync;
            set
            {
                if (this._mobileWirelessSync == value)
                    return;
                this._mobileWirelessSync = value;
                this.FirePropertyChanged(nameof(MobileWirelessSync));
            }
        }

        public override void Cancel()
        {
            if (this.CurrentPage is MobileWirelessSyncWizardPage currentWizardPage)
                currentWizardPage.OnCancel();
            if (this.CurrentPage is MobileWirelessSyncErrorPage currentErrorPage)
                currentErrorPage.OnCancel();
            base.Cancel();
        }
    }
}
