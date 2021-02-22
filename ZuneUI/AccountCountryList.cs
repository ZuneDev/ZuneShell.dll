// Decompiled with JetBrains decompiler
// Type: ZuneUI.AccountCountryList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Service;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace ZuneUI
{
    public class AccountCountryList : NotifyPropertyChangedImpl
    {
        private static AccountCountryList s_instance;
        private IDictionary<string, AccountCountry> _countries;

        private AccountCountryList()
        {
        }

        public static AccountCountryList Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = new AccountCountryList();
                return s_instance;
            }
        }

        public IList CountryAbbreviations
        {
            get
            {
                string[] array = null;
                if (this._countries != null)
                {
                    array = new string[this._countries.Keys.Count];
                    this._countries.Keys.CopyTo(array, 0);
                }
                return array;
            }
        }

        public bool Loaded => this._countries != null;

        public AccountCountry GetCountry(string abbreviation)
        {
            AccountCountry accountCountry = null;
            if (this._countries != null && abbreviation != null && this._countries.ContainsKey(abbreviation))
                accountCountry = this._countries[abbreviation];
            return accountCountry;
        }

        public void LoadCountryData() => ThreadPool.QueueUserWorkItem(new WaitCallback(this.LoadDataOnWorkerThread));

        private void LoadDataOnWorkerThread(object state) => this.LoadDataOnWorkerThread();

        internal bool LoadDataOnWorkerThread()
        {
            bool flag = true;
            if (this._countries == null)
            {
                CountryBaseDetails[] countryDetails = Service.Instance.GetCountryDetails();
                SortedDictionary<string, AccountCountry> sortedDictionary = null;
                if (countryDetails != null && countryDetails.Length > 0)
                {
                    sortedDictionary = new SortedDictionary<string, AccountCountry>(CountryNameComparer.Instance);
                    foreach (CountryBaseDetails details in countryDetails)
                        sortedDictionary[details.Abbreviation] = AccountCountry.Create(details);
                }
                if (sortedDictionary == null || sortedDictionary.Count == 0)
                    flag = false;
                else
                    this._countries = sortedDictionary;
            }
            if (flag)
                Application.DeferredInvoke(new DeferredInvokeHandler(this.NotifyLoaded), null);
            return flag;
        }

        private void NotifyLoaded(object args)
        {
            this.FirePropertyChanged("CountryAbbreviations");
            this.FirePropertyChanged("Loaded");
        }
    }
}
