// Decompiled with JetBrains decompiler
// Type: ZuneUI.ArtistsPanel
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;

namespace ZuneUI
{
    public class ArtistsPanel : MusicLibraryListPanelBase
    {
        internal ArtistsPanel(MusicLibraryPage libraryPage)
          : base(libraryPage)
        {
        }

        public override MediaType MediaType => MediaType.Artist;

        public override IList SelectedLibraryIds
        {
            get => this.LibraryPage.SelectedArtistIds;
            set => this.LibraryPage.SelectedArtistIds = value;
        }
    }
}
