// Decompiled with JetBrains decompiler
// Type: ZuneUI.DatePropertyDescriptor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public class DatePropertyDescriptor : PropertyDescriptor
    {
        private DateTimeKind timeZoneOverride;

        public DatePropertyDescriptor(string name, string multiValueString, string unknownString)
          : base(name, multiValueString, unknownString, 1000, false, DateTime.MinValue)
        {
        }

        public DatePropertyDescriptor(
          string name,
          string multiValueString,
          string unknownString,
          DateTimeKind timeZoneOverride)
          : base(name, multiValueString, unknownString, 1000, false, DateTime.MinValue)
        {
            this.timeZoneOverride = timeZoneOverride;
        }

        public DatePropertyDescriptor(
          string name,
          string multiValueString,
          string unknownString,
          bool required)
          : base(name, multiValueString, unknownString, 1000, required, DateTime.MinValue)
        {
        }

        public override string ConvertToString(object value) => StringFormatHelper.FormatShortDate((DateTime)value, this.UnknownString);

        public override object ConvertFromString(string value)
        {
            DateTime dateTime;
            return StringParserHelper.TryParseDate(value, this.timeZoneOverride, out dateTime) ? dateTime : (object)DateTime.MinValue;
        }

        public override bool IsValidInternal(string value) => StringParserHelper.IsNullOrEmptyOrBlank(value) || StringParserHelper.TryParseDate(value, out DateTime _);
    }
}
