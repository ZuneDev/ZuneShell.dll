// Decompiled with JetBrains decompiler
// Type: ZuneUI.ZuneTagHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Text.RegularExpressions;

namespace ZuneUI
{
    public static class ZuneTagHelper
    {
        private static int s_maxLength = 15;
        private static Regex s_zuneTagRegex = new Regex("^[A-Z]( ?[A-Z0-9]){0,14}$", RegexOptions.IgnoreCase);

        public static bool IsValid(string zuneTag) => !string.IsNullOrEmpty(zuneTag) && s_zuneTagRegex.IsMatch(zuneTag);

        public static int MaxLength => s_maxLength;
    }
}
