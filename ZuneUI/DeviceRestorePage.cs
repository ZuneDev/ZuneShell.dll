// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeviceRestorePage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public abstract class DeviceRestorePage : WizardPage
    {
        protected DeviceRestorePage(DeviceRestoreWizard wizard)
          : base((ZuneUI.Wizard)wizard)
        {
        }

        protected DeviceRestoreWizard Wizard => (DeviceRestoreWizard)this._owner;
    }
}
