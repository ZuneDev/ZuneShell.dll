// Decompiled with JetBrains decompiler
// Type: ZuneUI.NonBlankPropertyDescriptor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class NonBlankPropertyDescriptor : PropertyDescriptor
    {
        private int _minLength = 1;

        public NonBlankPropertyDescriptor(string name, string multiValueString, string unknownString)
          : this(name, multiValueString, unknownString, 1)
        {
        }

        public NonBlankPropertyDescriptor(
          string name,
          string multiValueString,
          string unknownString,
          int minLength)
          : this(name, multiValueString, unknownString, minLength, false)
        {
        }

        public NonBlankPropertyDescriptor(
          string name,
          string multiValueString,
          string unknownString,
          int minLength,
          bool required)
          : base(name, multiValueString, unknownString, required)
        {
            this._minLength = minLength;
        }

        public override bool IsValidInternal(string value) => value != null && value.Length >= this._minLength;

        public int MinLength
        {
            set => this._minLength = value;
        }
    }
}
