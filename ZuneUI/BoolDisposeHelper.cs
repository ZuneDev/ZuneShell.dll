// Decompiled with JetBrains decompiler
// Type: ZuneUI.BoolDisposeHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneUI
{
    public class BoolDisposeHelper : ModelItem
    {
        private BooleanChoice _choice;
        private bool _resetOnDispose;

        public BooleanChoice Choice
        {
            get => this._choice;
            set
            {
                if (this._choice == value)
                    return;
                this._choice = value;
                this.FirePropertyChanged(nameof(Choice));
            }
        }

        public bool ResetOnDispose
        {
            get => this._resetOnDispose;
            set
            {
                if (this._resetOnDispose == value)
                    return;
                this._resetOnDispose = value;
                this.FirePropertyChanged(nameof(ResetOnDispose));
            }
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing && this._choice != null && this._resetOnDispose)
                this._choice.Value = false;
            base.OnDispose(disposing);
        }
    }
}
