// Decompiled with JetBrains decompiler
// Type: ZuneUI.FamilySettingValue
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class FamilySettingValue
    {
        private string _text;
        private int _value;

        public FamilySettingValue(string text, int value)
        {
            this._text = text;
            this._value = value;
        }

        public int Value => this._value;

        public string Text => this._text;
    }
}
