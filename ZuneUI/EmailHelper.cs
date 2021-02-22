// Decompiled with JetBrains decompiler
// Type: ZuneUI.EmailHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Text.RegularExpressions;

namespace ZuneUI
{
    public static class EmailHelper
    {
        private static Regex s_emailRegex = new Regex("^[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,6}$", RegexOptions.IgnoreCase);
        private static Regex s_emailDomainRegex = new Regex("^[A-Z0-9.-]+\\.[A-Z]{2,6}$", RegexOptions.IgnoreCase);

        public static bool IsValid(string email) => !string.IsNullOrEmpty(email) && s_emailRegex.IsMatch(email);

        public static bool IsValidDomain(string domain) => !string.IsNullOrEmpty(domain) && s_emailDomainRegex.IsMatch(domain);
    }
}
