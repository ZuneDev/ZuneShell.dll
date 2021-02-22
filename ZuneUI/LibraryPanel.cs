// Decompiled with JetBrains decompiler
// Type: ZuneUI.LibraryPanel
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneUI
{
    public class LibraryPanel : PropertySet
    {
        private string _ui;
        private LibraryPage _libraryPage;

        public LibraryPanel()
          : this((IModelItemOwner)null)
        {
        }

        public LibraryPanel(IModelItemOwner owner)
          : base(owner)
          => this._libraryPage = owner as LibraryPage;

        public string UI
        {
            get => this._ui;
            set
            {
                if (!(this._ui != value))
                    return;
                this._ui = value;
                this.FirePropertyChanged(nameof(UI));
            }
        }

        public virtual MediaType MediaType => this.LibraryPage.MediaType;

        public virtual SyncCategory SyncCategory => UIDeviceList.MapMediaTypeToSyncCategory(this.MediaType);

        public LibraryPage LibraryPage => this._libraryPage;

        internal virtual void Release()
        {
        }
    }
}
