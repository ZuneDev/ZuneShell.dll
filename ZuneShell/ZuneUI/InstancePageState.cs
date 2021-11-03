// Decompiled with JetBrains decompiler
// Type: ZuneUI.InstancePageState
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class InstancePageState : IPageState
    {
        private IPage _page;

        public InstancePageState(IPage page) => this._page = page;

        public IPage RestoreAndRelease() => this._page;

        public void Release() => this._page.Release();

        public bool CanBeTrimmed => true;
    }
}
