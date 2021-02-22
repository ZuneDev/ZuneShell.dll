// Decompiled with JetBrains decompiler
// Type: ZuneUI.Page
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneUI
{
    public class Page : ModelItem, IPage
    {
        private bool _isCurrentPage;

        public bool IsCurrentPage
        {
            get => this._isCurrentPage;
            private set
            {
                if (this._isCurrentPage == value)
                    return;
                this._isCurrentPage = value;
                this.FirePropertyChanged(nameof(IsCurrentPage));
            }
        }

        public void OnNavigatedTo() => this.OnNavigatedToWorker();

        public void OnNavigatedAway(IPage destination) => this.OnNavigatedAwayWorker(destination);

        public event EventHandler NavigatedTo;

        public event EventHandler NavigatedAway;

        public virtual IPageState SaveAndRelease() => new InstancePageState(this);

        public virtual void Release() => this.Dispose();

        protected virtual void OnNavigatedToWorker()
        {
            this.IsCurrentPage = true;
            if (this.NavigatedTo == null)
                return;
            this.NavigatedTo(this, EventArgs.Empty);
        }

        protected virtual void OnNavigatedAwayWorker(IPage destination)
        {
            this.IsCurrentPage = false;
            if (this.NavigatedAway == null)
                return;
            this.NavigatedAway(this, EventArgs.Empty);
        }
    }
}
