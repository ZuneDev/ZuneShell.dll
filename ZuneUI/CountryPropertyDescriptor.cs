// Decompiled with JetBrains decompiler
// Type: ZuneUI.CountryPropertyDescriptor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class CountryPropertyDescriptor : PropertyDescriptor
    {
        public CountryPropertyDescriptor(
          string name,
          string multiValueString,
          string unknownString,
          bool required)
          : base(name, multiValueString, unknownString, required)
        {
            this.DefaultValue = CultureHelper.GetDefaultCountry();
        }

        public override object ConvertFromString(string value) => CountryHelper.GetAbbreviation(value);

        public override string ConvertToString(object value) => CountryHelper.GetDisplayName(value as string);
    }
}
