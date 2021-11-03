// Decompiled with JetBrains decompiler
// Type: ZuneUI.StringParserHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ZuneUI
{
    public static class StringParserHelper
    {
        private static Regex s_notNumberRegex = new Regex("[^0-9]");
        private static string s_validNumberChars = "0123456789";

        public static int ParseInt32(string input)
        {
            int result;
            int.TryParse(input, out result);
            return result;
        }

        public static int NumberFilter(string input) => ParseInt32(s_notNumberRegex.Replace(input, string.Empty));

        public static bool TryParseDate(string input, DateTimeKind dateTimeKind, out DateTime dateTime)
        {
            switch (dateTimeKind)
            {
                case DateTimeKind.Utc:
                    if (!DateTime.TryParse(input, out dateTime) && !DateTime.TryParse(input, CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.RoundtripKind, out dateTime))
                    {
                        int result;
                        if (!int.TryParse(input, out result) || result < 1 || result > 9999)
                            return false;
                        dateTime = new DateTime(result, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    }
                    else
                        dateTime = new DateTime(dateTime.Ticks, DateTimeKind.Utc);
                    return true;
                case DateTimeKind.Local:
                    if (!DateTime.TryParse(input, out dateTime) && !DateTime.TryParse(input, CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.RoundtripKind, out dateTime))
                    {
                        int result;
                        if (!int.TryParse(input, out result) || result < 1 || result > 9999)
                            return false;
                        dateTime = new DateTime(result, 1, 1, 0, 0, 0, DateTimeKind.Local);
                        dateTime = dateTime.ToUniversalTime();
                    }
                    else
                        dateTime = dateTime.ToUniversalTime();
                    return true;
                default:
                    return TryParseDate(input, out dateTime);
            }
        }

        public static bool TryParseDate(string input, out DateTime dateTime)
        {
            if (!DateTime.TryParse(input, out dateTime) && !DateTime.TryParse(input, CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.RoundtripKind, out dateTime))
            {
                int result;
                if (!int.TryParse(input, out result) || result < 1 || result > 9999)
                    return false;
                dateTime = new DateTime(result, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            }
            else
                dateTime = dateTime.ToUniversalTime();
            return true;
        }

        public static string CoerceToNonNegativeInt(string input)
        {
            if (input != null)
            {
                StringBuilder stringBuilder = null;
                int index = 0;
                int startIndex = 0;
                for (; index < input.Length; ++index)
                {
                    if (s_validNumberChars.IndexOf(input[index]) < 0)
                    {
                        if (stringBuilder == null)
                            stringBuilder = new StringBuilder(input);
                        stringBuilder.Remove(startIndex, 1);
                    }
                    else
                        ++startIndex;
                }
                if (stringBuilder != null)
                    input = stringBuilder.ToString();
            }
            return input;
        }

        public static int ParseYear(string input)
        {
            int result = 0;
            if (input != null && input.Length <= 4 && int.TryParse(input, out result))
            {
                if (result >= 0)
                {
                    if (result < 20)
                        result += 2000;
                    else if (result < 100)
                        result += 1900;
                    else if (result < 1000)
                        result += 2000;
                }
                else
                    result = 0;
            }
            return result;
        }

        public static IList Split(string source, string splitCharacter) => source.Split(new string[1]
        {
      splitCharacter
        }, StringSplitOptions.None);

        public static bool IsNullOrEmptyOrBlank(string value) => string.IsNullOrEmpty(value) || value.Trim().Length == 0;

        public static string HtmlTagsToLowerCase(string html)
        {
            if (!string.IsNullOrEmpty(html))
            {
                StringBuilder stringBuilder = null;
                bool flag = false;
                for (int index = 0; index < html.Length; ++index)
                {
                    char c = html[index];
                    switch (c)
                    {
                        case '<':
                            flag = true;
                            break;
                        case '>':
                            flag = false;
                            break;
                        default:
                            if (!char.IsWhiteSpace(c))
                                break;
                            goto case '>';
                    }
                    if (flag && char.IsUpper(c))
                    {
                        if (stringBuilder == null)
                            stringBuilder = new StringBuilder(html);
                        stringBuilder[index] = char.ToLower(c);
                    }
                }
                if (stringBuilder != null)
                    html = stringBuilder.ToString();
            }
            return html;
        }

        public static string StripHtmlTags(string html)
        {
            if (!string.IsNullOrEmpty(html))
            {
                StringBuilder stringBuilder = null;
                bool flag = false;
                for (int index = 0; index < html.Length; ++index)
                {
                    char ch = html[index];
                    if (ch == '<')
                        flag = true;
                    if (!flag)
                    {
                        if (stringBuilder == null)
                            stringBuilder = new StringBuilder(html.Length);
                        stringBuilder.Append(ch);
                    }
                    if (ch == '>')
                        flag = false;
                }
                if (stringBuilder != null)
                    html = stringBuilder.ToString();
            }
            return html;
        }

        public static string FormatFirmwareVersion(string version)
        {
            if (string.IsNullOrEmpty(version))
                return string.Empty;
            string[] strArray = version.Split('.');
            if (strArray.Length < 3)
                return version;
            int int32_1 = ParseInt32(strArray[0]);
            int int32_2 = ParseInt32(strArray[1]);
            int int32_3 = ParseInt32(strArray[2]);
            version = int32_1 != 0 ? string.Format(Shell.LoadString(StringId.IDS_FIRMWARE_VERSION_FORMAT), int32_1, int32_2, int32_3) : Shell.LoadString(StringId.IDS_DEVICE_RECOVERY_MODE);
            return version;
        }

        public static int CompareFirmwareVersions(string versionA, string versionB)
        {
            string[] strArray1 = new string[3] { "0", "0", "0" };
            string[] strArray2;
            if (string.IsNullOrEmpty(versionA))
                strArray2 = strArray1;
            else
                strArray2 = versionA.Split('.');
            string[] strArray3;
            if (string.IsNullOrEmpty(versionB))
                strArray3 = strArray1;
            else
                strArray3 = versionB.Split('.');
            int int32_1 = ParseInt32(strArray2[0]);
            int int32_2 = ParseInt32(strArray2[1]);
            int int32_3 = ParseInt32(strArray2[2]);
            int int32_4 = ParseInt32(strArray3[0]);
            int int32_5 = ParseInt32(strArray3[1]);
            int int32_6 = ParseInt32(strArray3[2]);
            int num1 = int32_1.CompareTo(int32_4);
            int num2 = int32_2.CompareTo(int32_5);
            int num3 = int32_3.CompareTo(int32_6);
            if (num1 != 0)
                return num1;
            return num2 != 0 ? num2 : num3;
        }
    }
}
