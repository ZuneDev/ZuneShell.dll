// Decompiled with JetBrains decompiler
// Type: ZuneUI.VideoNavigationCommandHandler
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;

namespace ZuneUI
{
    public class VideoNavigationCommandHandler : DeviceAwareNavigationHandler
    {
        private VideoLibraryView _view = VideoLibraryView.Invalid;

        protected override ZunePage GetPage(IDictionary args) => new VideoLibraryPage(this.ShowDeviceContents, this._view);

        public VideoLibraryView View
        {
            get => this._view;
            set => this._view = value;
        }
    }
}
