// Decompiled with JetBrains decompiler
// Type: ZuneUI.PlaylistFactory
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Playlist;
using System;
using System.ComponentModel;

namespace ZuneUI
{
    public abstract class PlaylistFactory : INotifyPropertyChanged, IDisposable
    {
        private bool _ready;
        protected readonly bool _navigateOnCreate;

        public event PropertyChangedEventHandler PropertyChanged;

        protected PlaylistFactory(bool navigateOnCreate) => this._navigateOnCreate = navigateOnCreate;

        public virtual void Dispose()
        {
        }

        public abstract string GetUniqueTitle();

        public abstract PlaylistResult CreatePlaylist(
          string title,
          CreatePlaylistOption option);

        public bool Ready
        {
            get => this._ready;
            protected set
            {
                if (this._ready == value)
                    return;
                this._ready = value;
                this.NotifyPropertyChanged(nameof(Ready));
            }
        }

        public bool NavigateOnCreate => this._navigateOnCreate;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged == null)
                return;
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
