// Decompiled with JetBrains decompiler
// Type: ZuneUI.PlaylistsPanel
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneUI
{
    public class PlaylistsPanel : ListPanel
    {
        private IList _selectedIds;

        internal PlaylistsPanel(MusicLibraryPage page)
          : base(page)
          => this.UI = PanelTemplate;

        public override IList SelectedLibraryIds
        {
            get => this._selectedIds;
            set
            {
                if (this._selectedIds == value)
                    return;
                this._selectedIds = value;
                this.FirePropertyChanged(nameof(SelectedLibraryIds));
            }
        }

        protected MusicLibraryPage LibraryPage => base.LibraryPage as MusicLibraryPage;

        private static string PanelTemplate => "res://ZuneShellResources!PlaylistsPanel.uix#PlaylistsPanel";
    }
}
