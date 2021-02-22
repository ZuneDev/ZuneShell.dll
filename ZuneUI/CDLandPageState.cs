// Decompiled with JetBrains decompiler
// Type: ZuneUI.CDLandPageState
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class CDLandPageState : IPageState
    {
        private CDLand _page;

        public CDLandPageState(IPage page) => this._page = (CDLand)page;

        public IPage RestoreAndRelease()
        {
            if (this._page.Album != Shell.MainFrame.Disc.BurnList && this._page.Album != Shell.MainFrame.Disc.NoCD && !this._page.Album.IsMediaLoaded)
            {
                this._page.Dispose();
                return (IPage)null;
            }
            if (this._page.Album != Shell.MainFrame.Disc.NoCD || !Shell.MainFrame.Disc.HasCD)
                return (IPage)this._page;
            this._page.Dispose();
            return (IPage)null;
        }

        public void Release() => this._page.Release();

        public bool CanBeTrimmed => true;
    }
}
