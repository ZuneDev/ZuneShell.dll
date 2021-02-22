// Decompiled with JetBrains decompiler
// Type: ZuneUI.DevicePivotManagingPageState
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class DevicePivotManagingPageState : IPageState
    {
        private IDeviceContentsPage _page;

        public DevicePivotManagingPageState(IDeviceContentsPage page) => this._page = page;

        public virtual IPage RestoreAndRelease()
        {
            if (this._page.ShowDeviceContents)
                Shell.MainFrame.ShowDevice(true);
            return (IPage)this._page;
        }

        public void Release() => this._page.Release();

        public bool CanBeTrimmed => false;

        protected IDeviceContentsPage Page => this._page;
    }
}
