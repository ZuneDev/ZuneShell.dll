// Decompiled with JetBrains decompiler
// Type: ZuneUI.BirthdayPropertyDescriptor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public class BirthdayPropertyDescriptor : DatePropertyDescriptor
    {
        public BirthdayPropertyDescriptor(string name, string multiValueString, string unknownString)
          : base(name, multiValueString, unknownString)
        {
        }

        public BirthdayPropertyDescriptor(
          string name,
          string multiValueString,
          string unknownString,
          bool required)
          : base(name, multiValueString, unknownString, required)
        {
        }

        public override bool IsValidInternal(string value) => this.IsValidInternal(value, null);

        public override string ConvertToString(object value, object state)
        {
            string unknownString = this.UnknownString;
            DateTime? nullable = (DateTime?)value;
            if (nullable.HasValue && nullable.Value != DateTime.MinValue)
                unknownString = DateTimeHelper.ToString((DateTime)value, state as string, DateTimeFormatType.ShortDate);
            return unknownString;
        }

        public override object ConvertFromString(string value, object state)
        {
            DateTime dateTime = DateTime.MinValue;
            string cultureString = state as string;
            DateTimeHelper.TryParse(value, cultureString, out dateTime);
            return dateTime;
        }

        public override bool IsValidInternal(string value, object state)
        {
            string cultureString = state as string;
            bool flag;
            if (StringParserHelper.IsNullOrEmptyOrBlank(value))
            {
                flag = !this.IsRequired(state);
            }
            else
            {
                DateTime dateTime;
                flag = DateTimeHelper.TryParse(value, cultureString, out dateTime);
                if (flag && dateTime > DateTime.Now)
                    flag = false;
            }
            return flag;
        }

        internal override string GetOverlayString(object state)
        {
            string cultureString = state as string;
            return !string.IsNullOrEmpty(cultureString) ? DateTimeHelper.GetDisplayPattern(cultureString) : null;
        }
    }
}
