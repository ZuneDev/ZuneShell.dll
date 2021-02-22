// Decompiled with JetBrains decompiler
// Type: ZuneUI.CategoryPageState
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class CategoryPageState : IPageState
    {
        private IPage _page;

        public CategoryPageState(IPage page) => this._page = page;

        public IPage RestoreAndRelease()
        {
            if (CategoryPage.EntryPage != null)
                return this._page;
            this.Release();
            return (IPage)null;
        }

        public void Release() => this._page.Release();

        public bool CanBeTrimmed => true;
    }
}
