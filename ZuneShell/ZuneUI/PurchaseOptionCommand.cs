// Decompiled with JetBrains decompiler
// Type: ZuneUI.PurchaseOptionCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneUI
{
    public class PurchaseOptionCommand : Command
    {
        private string _text = string.Empty;
        private int _points;

        public string Text
        {
            get => this._text;
            set
            {
                if (!(this._text != value))
                    return;
                this._text = value;
                this.FirePropertyChanged(nameof(Text));
            }
        }

        public int Points
        {
            get => this._points;
            set
            {
                if (this._points == value)
                    return;
                this._points = value;
                this.FirePropertyChanged(nameof(Points));
            }
        }
    }
}
