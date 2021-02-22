// Decompiled with JetBrains decompiler
// Type: ZuneUI.MusicNavigationCommandHandler
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;

namespace ZuneUI
{
    public class MusicNavigationCommandHandler : DeviceAwareNavigationHandler
    {
        private MusicLibraryView _view;

        protected override ZunePage GetPage(IDictionary args) => new MusicLibraryPage(this.ShowDeviceContents, this._view);

        public MusicLibraryView View
        {
            get => this._view;
            set => this._view = value;
        }
    }
}
