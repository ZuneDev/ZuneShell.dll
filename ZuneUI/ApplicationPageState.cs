// Decompiled with JetBrains decompiler
// Type: ZuneUI.ApplicationPageState
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class ApplicationPageState : DevicePivotManagingPageState
    {
        public ApplicationPageState(ApplicationLibraryPage page)
          : base((IDeviceContentsPage)page)
        {
        }

        public override IPage RestoreAndRelease()
        {
            IPage page = (IPage)null;
            if (!this.Page.ShowDeviceContents || SyncControls.Instance.CurrentDevice.SupportsSyncApplications)
                page = base.RestoreAndRelease();
            return page;
        }
    }
}
