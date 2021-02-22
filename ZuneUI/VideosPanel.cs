// Decompiled with JetBrains decompiler
// Type: ZuneUI.VideosPanel
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneUI
{
    public class VideosPanel : ListPanel
    {
        internal VideosPanel(VideoLibraryPage library)
          : base((IModelItemOwner)library)
        {
        }

        protected VideoLibraryPage LibraryPage => base.LibraryPage as VideoLibraryPage;

        public override IList SelectedLibraryIds
        {
            get => this.LibraryPage.SelectedVideoIds;
            set => this.LibraryPage.SelectedVideoIds = value;
        }
    }
}
