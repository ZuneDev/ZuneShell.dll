// Decompiled with JetBrains decompiler
// Type: ZuneUI.LanguageHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Globalization;

namespace ZuneUI
{
    internal class LanguageHelper
    {
        internal static string GetDisplayLanguageName(string languageCode)
        {
            string str = null;
            try
            {
                if (!string.IsNullOrEmpty(languageCode))
                {
                    CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(languageCode);
                    while (!cultureInfo.IsNeutralCulture)
                        cultureInfo = cultureInfo.Parent;
                    str = GetDisplayName(cultureInfo.ToString());
                }
            }
            catch (ArgumentException ex)
            {
            }
            return str;
        }

        internal static string GetDisplayName(string languageCode)
        {
            string str = string.Empty;
            if (!string.IsNullOrEmpty(languageCode))
            {
                try
                {
                    str = new CultureInfo(languageCode).DisplayName;
                }
                catch (ArgumentException ex)
                {
                }
            }
            return str;
        }

        internal static string GetAbbreviation(string languageName)
        {
            string str = string.Empty;
            if (!string.IsNullOrEmpty(languageName))
            {
                foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
                {
                    if (culture.DisplayName.Equals(languageName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        str = culture.TwoLetterISOLanguageName;
                        break;
                    }
                }
            }
            return str;
        }
    }
}
