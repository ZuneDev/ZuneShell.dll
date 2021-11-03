// Decompiled with JetBrains decompiler
// Type: ZuneUI.YearPropertyDescriptor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public class YearPropertyDescriptor : PropertyDescriptor
    {
        public YearPropertyDescriptor(
          string name,
          string multiValueString,
          string unknownString,
          int maxTextLength)
          : base(name, multiValueString, unknownString, maxTextLength)
        {
        }

        public override string ConvertToString(object value) => value != null ? StringFormatHelper.FormatYear((DateTime)value, this.UnknownString) : null;

        public override object ConvertFromString(string value)
        {
            DateTime dateTime;
            return StringParserHelper.TryParseDate(value, out dateTime) ? dateTime : (object)null;
        }

        public override bool IsValidInternal(string value)
        {
            int result;
            return int.TryParse(value, out result) && result > 0 && result <= 9999 && (result <= 99 || result >= 1800);
        }
    }
}
