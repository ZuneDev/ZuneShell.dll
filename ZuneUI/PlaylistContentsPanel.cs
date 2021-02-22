// Decompiled with JetBrains decompiler
// Type: ZuneUI.PlaylistContentsPanel
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneUI
{
    public class PlaylistContentsPanel : ListPanel
    {
        private IList _selectedIds;
        private IList _selectedPlaylistIds;

        internal PlaylistContentsPanel(LibraryPage page)
          : base(page)
          => this.UI = PanelTemplate;

        public override MediaType MediaType => MediaType.PlaylistContentItem;

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

        public IList SelectedPlaylistIds
        {
            get => this._selectedPlaylistIds;
            set
            {
                if (this._selectedPlaylistIds == value)
                    return;
                PlaylistManager.Instance.UnfreezeAutoPlaylist(this.SelectedPlaylistId);
                this._selectedPlaylistIds = value;
                this.FirePropertyChanged(nameof(SelectedPlaylistIds));
                this.FirePropertyChanged("SelectedPlaylistId");
                PlaylistManager.Instance.FreezeAutoPlaylist(this.SelectedPlaylistId);
            }
        }

        public int SelectedPlaylistId => this._selectedPlaylistIds != null ? (int)this._selectedPlaylistIds[0] : -1;

        internal override void Release()
        {
            PlaylistManager.Instance.UnfreezeAutoPlaylist(this.SelectedPlaylistId);
            base.Release();
        }

        private static string PanelTemplate => "res://ZuneShellResources!PlaylistContentsPanel.uix#PlaylistContentsPanel";
    }
}
