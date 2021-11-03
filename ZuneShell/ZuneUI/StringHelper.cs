// Decompiled with JetBrains decompiler
// Type: ZuneUI.StringHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public class StringHelper
    {
        public static bool IsEqualCaseInsensitive(string first, string second) => string.Compare(first, second, StringComparison.InvariantCultureIgnoreCase) == 0;

        public static string[] Split(string value, string splitPattern)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            return value.Split(new string[1] { splitPattern }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static bool CaseInsensitiveCompare(string str1, string str2) => !string.IsNullOrEmpty(str1) && !string.IsNullOrEmpty(str2) && string.Compare(str1, str2, StringComparison.CurrentCultureIgnoreCase) == 0;
    }
}
