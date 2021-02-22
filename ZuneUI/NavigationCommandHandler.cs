// Decompiled with JetBrains decompiler
// Type: ZuneUI.NavigationCommandHandler
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;

namespace ZuneUI
{
    public class NavigationCommandHandler : NavigationCommandHandlerBase
    {
        private string _pageUI;
        private string _backgroundUI;
        private Node _pivotPreference;
        private bool _isRootPage;
        private PlaybackContext _playbackContext;

        public string UI
        {
            get => this._pageUI;
            set => this._pageUI = value;
        }

        public string BackgroundUI
        {
            get => this._backgroundUI;
            set => this._backgroundUI = value;
        }

        public Node PivotPreference
        {
            get => this._pivotPreference;
            set => this._pivotPreference = value;
        }

        public bool IsRootPage
        {
            get => this._isRootPage;
            set => this._isRootPage = value;
        }

        public PlaybackContext PlaybackContext
        {
            get => this._playbackContext;
            set => this._playbackContext = value;
        }

        protected override ZunePage GetPage(IDictionary args) => new ZunePage()
        {
            UI = this.UI,
            UIPath = this.UIPath,
            BackgroundUI = this.BackgroundUI,
            PivotPreference = this.PivotPreference,
            IsRootPage = this.IsRootPage,
            PlaybackContext = this.PlaybackContext
        };
    }
}
