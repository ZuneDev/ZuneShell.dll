// Decompiled with JetBrains decompiler
// Type: ZuneUI.FirstConnectPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public abstract class FirstConnectPage : WizardPage
    {
        private bool _isEnabled = true;
        private bool _isPageComplete;

        internal FirstConnectPage(Wizard wizard)
          : base(wizard)
        {
        }

        public virtual bool IsPageComplete
        {
            get => this._isPageComplete;
            set
            {
                if (this._isPageComplete == value)
                    return;
                this._isPageComplete = value;
                this.FirePropertyChanged(nameof(IsPageComplete));
            }
        }

        public override bool IsEnabled => this._isEnabled;

        internal override bool OnMovingNext()
        {
            this.IsPageComplete = true;
            return base.OnMovingNext();
        }

        internal override bool OnMovingBack()
        {
            this.IsPageComplete = false;
            return base.OnMovingBack();
        }
    }
}
