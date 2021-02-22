// Decompiled with JetBrains decompiler
// Type: ZuneUI.NavigateToInstanceCommandHandler
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;

namespace ZuneUI
{
    public class NavigateToInstanceCommandHandler : NavigationCommandHandlerBase
    {
        private ZunePage _page;

        public ZunePage Page
        {
            get => this._page;
            set => this._page = value;
        }

        protected override ZunePage GetPage(IDictionary args) => this._page;
    }
}
