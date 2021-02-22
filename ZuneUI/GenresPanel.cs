// Decompiled with JetBrains decompiler
// Type: ZuneUI.GenresPanel
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;

namespace ZuneUI
{
    public class GenresPanel : MusicLibraryListPanelBase
    {
        internal GenresPanel(MusicLibraryPage libraryPage)
          : base(libraryPage)
        {
        }

        public override MediaType MediaType => MediaType.Genre;

        public override IList SelectedLibraryIds
        {
            get => this.LibraryPage.SelectedGenreIds;
            set => this.LibraryPage.SelectedGenreIds = value;
        }
    }
}
