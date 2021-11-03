// Decompiled with JetBrains decompiler
// Type: ZuneUI.Breadcrumb
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneUI
{
    public class Breadcrumb : ModelItem
    {
        private WizardPage _page;
        private bool _active;
        private bool _complete;

        public Breadcrumb(WizardPage page) => this._page = page;

        public WizardPage Page => this._page;

        public bool Active
        {
            get => this._active;
            set
            {
                if (this._active == value)
                    return;
                this._active = value;
                this.FirePropertyChanged(nameof(Active));
            }
        }

        public bool Complete
        {
            get => this._complete;
            set
            {
                if (this._complete == value)
                    return;
                this._complete = value;
                this.FirePropertyChanged(nameof(Complete));
            }
        }
    }
}
