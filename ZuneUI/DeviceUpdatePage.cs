// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeviceUpdatePage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public abstract class DeviceUpdatePage : WizardPage
    {
        protected DeviceUpdatePage(DeviceUpdateWizard wizard)
          : base((ZuneUI.Wizard)wizard)
        {
        }

        protected DeviceUpdateWizard Wizard => (DeviceUpdateWizard)this._owner;

        public override bool CanCancel
        {
            get
            {
                if (this.Wizard.Disconnected)
                    return true;
                return this.Wizard.UIFirmwareUpdater != null && this.Wizard.UIFirmwareUpdater.CanCancel;
            }
        }
    }
}
