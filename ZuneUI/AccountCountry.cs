// Decompiled with JetBrains decompiler
// Type: ZuneUI.AccountCountry
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public class AccountCountry : CountryBaseDetails
    {
        private IDictionary<string, string> m_states;
        private string m_localizedStates;
        private static Hashtable m_countryFieldValidatorLookup;

        private AccountCountry(
          string abbreviation,
          string[] languageAbbreviations,
          int teenagerAge,
          int adultAge,
          bool showAccountSettings,
          bool usageCollection,
          CountryFieldValidator[] validators,
          string localizedStates)
          : base(abbreviation, languageAbbreviations, teenagerAge, adultAge, showAccountSettings, usageCollection, validators)
        {
            this.m_localizedStates = localizedStates;
        }

        public IList States
        {
            get
            {
                this.LoadStateData();
                ArrayList arrayList = null;
                if (this.m_states != null)
                {
                    arrayList = new ArrayList(this.m_states.Values.Count);
                    arrayList.AddRange((ICollection)this.m_states.Values);
                    arrayList.Sort(StringComparer.CurrentCultureIgnoreCase);
                }
                return arrayList;
            }
        }

        public CountryFieldValidator GetValidator(CountryFieldValidatorType type)
        {
            CountryFieldValidator countryFieldValidator = null;
            if (this.Validators != null && CountryFieldValidatorLookup.ContainsKey(type))
            {
                for (int index = 0; index < this.Validators.Length; ++index)
                {
                    CountryFieldValidator validator = this.Validators[index];
                    if (validator.Name == (string)CountryFieldValidatorLookup[type])
                    {
                        countryFieldValidator = validator;
                        break;
                    }
                }
            }
            return countryFieldValidator;
        }

        public IList StatesAbbreviations
        {
            get
            {
                this.LoadStateData();
                string[] array = null;
                if (this.m_states != null)
                {
                    array = new string[this.m_states.Keys.Count];
                    this.m_states.Keys.CopyTo(array, 0);
                }
                return array;
            }
        }

        public static AccountCountry Create(CountryBaseDetails details)
        {
            AccountCountry accountCountry = null;
            if (details != null)
            {
                string[] languageAbbreviations = details.LanguageAbbreviations;
                if (languageAbbreviations != null)
                    Array.Sort(languageAbbreviations, LanguageNameComparer.Instance);
                string localizedStates = null;
                if (details.Abbreviation.Equals("US", StringComparison.InvariantCultureIgnoreCase))
                    localizedStates = Shell.LoadString(StringId.IDS_BILLING_USA_STATES);
                else if (details.Abbreviation.Equals("CA", StringComparison.InvariantCultureIgnoreCase))
                    localizedStates = Shell.LoadString(StringId.IDS_BILLING_CA_PROVINCES);
                accountCountry = new AccountCountry(details.Abbreviation, details.LanguageAbbreviations, details.TeenagerAge, details.AdultAge, details.ShowNewsletterOptions, details.UsageCollection, details.Validators, localizedStates);
            }
            return accountCountry;
        }

        public static Hashtable CountryFieldValidatorLookup
        {
            get
            {
                if (m_countryFieldValidatorLookup == null)
                {
                    m_countryFieldValidatorLookup = new Hashtable();
                    m_countryFieldValidatorLookup.Add(CountryFieldValidatorType.FirstName, "firstName");
                    m_countryFieldValidatorLookup.Add(CountryFieldValidatorType.LastName, "lastName");
                    m_countryFieldValidatorLookup.Add(CountryFieldValidatorType.AccountHolderName, "accountHolderName");
                    m_countryFieldValidatorLookup.Add(CountryFieldValidatorType.Street1, "street1");
                    m_countryFieldValidatorLookup.Add(CountryFieldValidatorType.Street2, "street2");
                    m_countryFieldValidatorLookup.Add(CountryFieldValidatorType.City, "city");
                    m_countryFieldValidatorLookup.Add(CountryFieldValidatorType.State, "state");
                    m_countryFieldValidatorLookup.Add(CountryFieldValidatorType.PostalCode, "postalCode");
                    m_countryFieldValidatorLookup.Add(CountryFieldValidatorType.District, "district");
                    m_countryFieldValidatorLookup.Add(CountryFieldValidatorType.Country, "country");
                    m_countryFieldValidatorLookup.Add(CountryFieldValidatorType.PhoneType, "phoneType");
                    m_countryFieldValidatorLookup.Add(CountryFieldValidatorType.PhonePrefix, "phonePrefix");
                    m_countryFieldValidatorLookup.Add(CountryFieldValidatorType.PhoneNumber, "phoneNumber");
                    m_countryFieldValidatorLookup.Add(CountryFieldValidatorType.PhoneCountryCode, "phoneCountryCode");
                    m_countryFieldValidatorLookup.Add(CountryFieldValidatorType.PhoneExtension, "phoneExtension");
                }
                return m_countryFieldValidatorLookup;
            }
        }

        private void LoadStateData()
        {
            if (this.m_states != null || this.m_localizedStates == null)
                return;
            string[] strArray = this.m_localizedStates.Split(new char[1]
            {
        ';'
            }, StringSplitOptions.RemoveEmptyEntries);
            int capacity = strArray.Length / 2;
            this.m_states = new Dictionary<string, string>(capacity);
            int index1 = 0;
            int index2 = 1;
            for (int index3 = 0; index3 < capacity; ++index3)
            {
                if (!this.m_states.ContainsKey(strArray[index1]))
                    this.m_states.Add(strArray[index1], strArray[index2]);
                index1 += 2;
                index2 += 2;
            }
        }

        public string GetStateAbbreviation(string stateName)
        {
            this.LoadStateData();
            string str = null;
            if (this.m_states != null && stateName != null)
            {
                foreach (KeyValuePair<string, string> state in m_states)
                {
                    if (state.Value.Equals(stateName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        str = state.Key;
                        break;
                    }
                }
            }
            return str;
        }

        public string GetState(string stateAbbreviation)
        {
            this.LoadStateData();
            string str = null;
            if (this.m_states != null && stateAbbreviation != null && this.m_states.ContainsKey(stateAbbreviation))
                str = this.m_states[stateAbbreviation];
            return str;
        }
    }
}
