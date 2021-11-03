// Decompiled with JetBrains decompiler
// Type: ZuneUI.DateTimeHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ZuneUI
{
    public static class DateTimeHelper
    {
        private static string _unknown = Shell.LoadString(StringId.IDS_TYPE_UNKNOWN);

        public static bool IsEmpty(DateTime date) => date == DateTime.MinValue;

        public static DateTime Empty => DateTime.MinValue;

        public static string GetDisplayPattern(string cultureString)
        {
            string str1 = null;
            CultureInfo cultureInfo = null;
            if (!string.IsNullOrEmpty(cultureString))
            {
                try
                {
                    cultureInfo = new CultureInfo(cultureString, false);
                }
                catch
                {
                }
            }
            if (cultureInfo == null)
                cultureInfo = CultureInfo.CurrentUICulture;
            if (cultureInfo != null)
            {
                string newValue = Shell.LoadString(StringId.IDS_DATETIME_YEAR_ABBREVIATION);
                string str2 = Shell.LoadString(StringId.IDS_DATETIME_MONTH_ABBREVIATION);
                string str3 = Shell.LoadString(StringId.IDS_DATETIME_DAY_ABBREVIATION);
                str1 = Regex.Replace(Regex.Replace(cultureInfo.DateTimeFormat.ShortDatePattern.Replace("y", newValue), "M{1,2}", str2 + str2), "d{1,2}", str3 + str3);
            }
            return str1;
        }

        public static bool TryParse(string dateTimeString, string cultureString, out DateTime dateTime)
        {
            bool flag = false;
            CultureInfo culture = CultureHelper.GetCulture(cultureString);
            dateTime = DateTime.MaxValue;
            if (culture != null)
            {
                string format = Regex.Replace(Regex.Replace(culture.DateTimeFormat.ShortDatePattern, "M{1,2}", "M"), "d{1,2}", "d");
                flag = DateTime.TryParseExact(dateTimeString, format, culture, DateTimeStyles.None, out dateTime);
            }
            return flag;
        }

        public static string ToString(DateTime dateTime, DateTimeFormatType format) => ToString(dateTime, null, format);

        public static string ToString(
          DateTime dateTime,
          string cultureString,
          DateTimeFormatType format)
        {
            string str = null;
            CultureInfo culture = CultureHelper.GetCulture(cultureString);
            try
            {
                if (culture != null)
                {
                    string format1;
                    switch (format)
                    {
                        case DateTimeFormatType.ShortDate:
                            format1 = culture.DateTimeFormat.ShortDatePattern;
                            break;
                        case DateTimeFormatType.LongDate:
                            format1 = culture.DateTimeFormat.LongDatePattern;
                            break;
                        case DateTimeFormatType.FullLongDateLongTime:
                            format1 = culture.DateTimeFormat.FullDateTimePattern;
                            break;
                        case DateTimeFormatType.MonthDay:
                            format1 = culture.DateTimeFormat.MonthDayPattern;
                            break;
                        case DateTimeFormatType.ShortTime:
                            format1 = culture.DateTimeFormat.ShortTimePattern;
                            break;
                        case DateTimeFormatType.LongTime:
                            format1 = culture.DateTimeFormat.LongTimePattern;
                            break;
                        case DateTimeFormatType.YearMonth:
                            format1 = culture.DateTimeFormat.YearMonthPattern;
                            break;
                        default:
                            throw new ArgumentException();
                    }
                    str = dateTime.ToString(format1, culture);
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                str = _unknown;
            }
            return str;
        }
    }
}
