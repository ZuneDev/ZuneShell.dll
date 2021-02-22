// Decompiled with JetBrains decompiler
// Type: ZuneUI.IntYearPropertyDescriptor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class IntYearPropertyDescriptor : YearPropertyDescriptor
    {
        public IntYearPropertyDescriptor(
          string name,
          string multiValueString,
          string unknownString,
          int maxTextLength)
          : base(name, multiValueString, unknownString, maxTextLength)
        {
        }

        public override string ConvertToString(object value)
        {
            if (value == null)
                return (string)null;
            int num = (int)value;
            return num < 0 ? (string)null : num.ToString();
        }

        public override object ConvertFromString(string value)
        {
            int result;
            return int.TryParse(value, out result) ? (object)result : (object)0;
        }
    }
}
