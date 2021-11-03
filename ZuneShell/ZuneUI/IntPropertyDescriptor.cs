// Decompiled with JetBrains decompiler
// Type: ZuneUI.IntPropertyDescriptor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class IntPropertyDescriptor : PropertyDescriptor
    {
        public IntPropertyDescriptor(
          string name,
          string multiValueString,
          string unknownString,
          int maxTextLength)
          : base(name, multiValueString, unknownString, maxTextLength)
        {
        }

        public IntPropertyDescriptor(string name, string multiValueString, string unknownString)
          : base(name, multiValueString, unknownString)
        {
        }

        public IntPropertyDescriptor(
          string name,
          string multiValueString,
          string unknownString,
          bool required)
          : base(name, multiValueString, unknownString, required)
        {
        }

        public override object ConvertFromString(string value)
        {
            int result;
            return int.TryParse(value, out result) ? result : (object)0;
        }

        public override bool IsValidInternal(string value)
        {
            int result;
            return string.IsNullOrEmpty(value) || int.TryParse(value, out result) && result >= 0;
        }
    }
}
