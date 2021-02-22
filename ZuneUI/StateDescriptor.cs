// Decompiled with JetBrains decompiler
// Type: ZuneUI.StateDescriptor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Globalization;

namespace ZuneUI
{
    public class StateDescriptor : CountryFieldValidationPropertyDescriptor
    {
        public StateDescriptor(string name)
          : base(name, CountryFieldValidatorType.State)
        {
        }

        public override object ConvertFromString(string value)
        {
            string str = SignIn.Instance.SignedIn ? SignIn.Instance.CountryCode : RegionInfo.CurrentRegion.TwoLetterISORegionName;
            return this.ConvertFromString(value, (object)str);
        }

        public override object ConvertFromString(string value, object country)
        {
            string str = (string)null;
            AccountCountry country1 = AccountCountryList.Instance.GetCountry(country as string);
            if (country1 != null)
                str = country1.GetStateAbbreviation(value);
            return (object)str ?? (object)value;
        }

        public override string ConvertToString(object value)
        {
            string str = SignIn.Instance.SignedIn ? SignIn.Instance.CountryCode : RegionInfo.CurrentRegion.TwoLetterISORegionName;
            return this.ConvertToString(value, (object)str);
        }

        public override string ConvertToString(object value, object country)
        {
            string str = (string)null;
            AccountCountry country1 = AccountCountryList.Instance.GetCountry(country as string);
            if (country1 != null)
                str = country1.GetState(value as string);
            return str ?? value as string;
        }
    }
}
