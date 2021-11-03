// Decompiled with JetBrains decompiler
// Type: ZuneUI.CountryHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Globalization;

namespace ZuneUI
{
    internal class CountryHelper
    {
        internal static string GetDisplayName(string countryCode)
        {
            string str = string.Empty;
            if (!string.IsNullOrEmpty(countryCode))
            {
                try
                {
                    str = new RegionInfo(countryCode).DisplayName;
                }
                catch (ArgumentException ex)
                {
                }
            }
            return str;
        }

        internal static string GetAbbreviation(string countryName)
        {
            string str = string.Empty;
            if (!string.IsNullOrEmpty(countryName))
            {
                foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(culture.Name))
                        {
                            RegionInfo regionInfo = new RegionInfo(culture.Name);
                            if (regionInfo.DisplayName.Equals(countryName, StringComparison.CurrentCultureIgnoreCase))
                            {
                                str = regionInfo.TwoLetterISORegionName;
                                break;
                            }
                        }
                    }
                    catch (ArgumentException ex)
                    {
                    }
                }
            }
            return str;
        }
    }
}
