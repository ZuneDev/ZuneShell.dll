// Decompiled with JetBrains decompiler
// Type: ZuneUI.DownloadsPageState
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;

namespace ZuneUI
{
    public class DownloadsPageState : IPageState
    {
        private DownloadsPage _page;

        public DownloadsPageState(IPage page) => this._page = (DownloadsPage)page;

        public IPage RestoreAndRelease()
        {
            if ((double)DownloadManager.Instance.Percentage >= 100.0)
                this.Release();
            return (IPage)this._page;
        }

        public void Release()
        {
            this._page.Release();
            this._page = (DownloadsPage)null;
        }

        public bool CanBeTrimmed => true;
    }
}
