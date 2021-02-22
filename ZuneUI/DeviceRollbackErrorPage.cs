// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeviceRollbackErrorPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class DeviceRollbackErrorPage : WizardErrorPage
    {
        internal DeviceRollbackErrorPage(DeviceRollbackWizard wizard)
          : base(wizard)
          => this.Description = Shell.LoadString(StringId.IDS_DEVICE_RESTORE_ERROR_TITLE);

        public override string UI => "res://ZuneShellResources!DeviceRollback.uix#DeviceRollbackErrorPage";
    }
}
