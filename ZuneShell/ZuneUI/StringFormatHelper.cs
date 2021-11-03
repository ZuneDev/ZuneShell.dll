// Decompiled with JetBrains decompiler
// Type: ZuneUI.StringFormatHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Globalization;

namespace ZuneUI
{
    public static class StringFormatHelper
    {
        private static string _unknown = Shell.LoadString(StringId.IDS_TYPE_UNKNOWN);

        private static string Format(DateTime date, string format, string unknown) => date == DateTime.MinValue ? unknown : date.ToString(format);

        public static string Format(DateTime date, string format) => Format(date, format, _unknown);

        public static string FormatShortDate(DateTime date, string unknown) => Format(date, ShortDateFormat, unknown);

        public static string FormatShortDate(DateTime date) => FormatShortDate(date, _unknown);

        public static string ShortDateFormat => "d";

        public static string ShortDatePatternForDisplay => DateTimeFormatInfo.CurrentInfo.ShortDatePattern.ToLower();

        public static string ShortTimeFormat => "t";

        public static string FormatYear(DateTime date, string unknown) => Format(date, YearFormat, unknown);

        public static string FormatYear(DateTime date) => FormatYear(date, _unknown);

        public static string YearFormat => "yyyy";

        public static string NumericMonthDayPattern => Shell.LoadString(StringId.IDS_DATETIME_NUMERIC_MONTH_DAY_PATTERN);

        public static string NumericMonthYearPattern => Shell.LoadString(StringId.IDS_DATETIME_NUMERIC_MONTH_YEAR_PATTERN);

        public static string FriendlyMonthYearPattern => Shell.LoadString(StringId.IDS_DATETIME_MONTH_DAY_YEAR_PATTERN);

        public static string FormatPrice(double price, string currencyCode)
        {
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            bool flag = string.IsNullOrEmpty(currencyCode);
            if (!flag)
            {
                try
                {
                    flag = new RegionInfo(cultureInfo.LCID).ISOCurrencySymbol.Equals(currencyCode, StringComparison.InvariantCultureIgnoreCase);
                }
                catch (ArgumentException ex)
                {
                }
            }
            if (!flag)
            {
                foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.InstalledWin32Cultures))
                {
                    try
                    {
                        if (new RegionInfo(culture.LCID).ISOCurrencySymbol.Equals(currencyCode, StringComparison.InvariantCultureIgnoreCase))
                        {
                            cultureInfo = culture;
                            break;
                        }
                    }
                    catch (ArgumentException ex)
                    {
                    }
                }
            }
            return string.Format(cultureInfo, "{0:c}", (object)price);
        }
    }
}
