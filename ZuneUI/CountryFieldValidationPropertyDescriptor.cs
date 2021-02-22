// Decompiled with JetBrains decompiler
// Type: ZuneUI.CountryFieldValidationPropertyDescriptor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using System.Text.RegularExpressions;

namespace ZuneUI
{
    public class CountryFieldValidationPropertyDescriptor : PropertyDescriptor
    {
        private CountryFieldValidatorType _fieldValidatorType;

        public CountryFieldValidationPropertyDescriptor(
          string name,
          CountryFieldValidatorType fieldValidatorType)
          : base(name, string.Empty, string.Empty, false)
        {
            this._fieldValidatorType = fieldValidatorType;
        }

        internal override string GetOverlayString(object state)
        {
            string str = string.Empty;
            AccountCountry accountCountry = this.GetAccountCountry(state);
            if (accountCountry != null)
            {
                CountryFieldValidator validator = accountCountry.GetValidator(this._fieldValidatorType);
                if (validator != null)
                    str = validator.FriendlyFormat;
            }
            return str;
        }

        internal override string GetLabelString(object state)
        {
            string str = string.Empty;
            AccountCountry accountCountry = this.GetAccountCountry(state);
            if (accountCountry != null)
            {
                CountryFieldValidator validator = accountCountry.GetValidator(this._fieldValidatorType);
                if (validator != null)
                    str = Shell.LoadString(validator.NameStringId);
            }
            return str;
        }

        public override bool IsValidInternal(string value, object state)
        {
            string pattern = (string)null;
            AccountCountry accountCountry = this.GetAccountCountry(state);
            if (accountCountry == null)
                return true;
            if (accountCountry != null)
            {
                CountryFieldValidator validator = accountCountry.GetValidator(this._fieldValidatorType);
                if (validator != null && !string.IsNullOrEmpty(validator.Regex))
                    pattern = validator.Regex;
            }
            return string.IsNullOrEmpty(pattern) || value != null && Regex.IsMatch(value, pattern);
        }

        public override bool IsRequiredInternal(object state)
        {
            AccountCountry accountCountry = this.GetAccountCountry(state);
            if (accountCountry != null)
            {
                CountryFieldValidator validator = accountCountry.GetValidator(this._fieldValidatorType);
                if (validator != null && !string.IsNullOrEmpty(validator.Regex))
                    return true;
            }
            return false;
        }

        private AccountCountry GetAccountCountry(object state)
        {
            AccountCountry accountCountry = (AccountCountry)null;
            if (state != null)
                accountCountry = AccountCountryList.Instance.GetCountry(state as string);
            return accountCountry;
        }
    }
}
