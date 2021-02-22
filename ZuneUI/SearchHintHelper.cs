// Decompiled with JetBrains decompiler
// Type: ZuneUI.SearchHintHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using MicrosoftZuneLibrary;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public class SearchHintHelper : AutoCompleteHelper
    {
        private int _resultsQueueCount;
        private Hashtable _resultsCache = new Hashtable();
        private DataProviderTitleList _mergedTitles;
        private Timer _updateTimer;
        private bool _blockResults = true;
        private bool _filterForKeyword;

        public SearchHintHelper()
        {
            this.UseEntrySeparator = false;
            this.Applyfilter = false;
            this._updateTimer = new Timer();
            this._updateTimer.AutoRepeat = false;
            this._updateTimer.Interval = 150;
            this._updateTimer.Tick += new EventHandler(this.OnUpdateTimerTick);
        }

        public bool BlockResults
        {
            get => this._blockResults;
            set
            {
                if (this._blockResults == value)
                    return;
                this._blockResults = value;
            }
        }

        public bool FilterForKeyword
        {
            get => this._filterForKeyword;
            set
            {
                if (this._filterForKeyword == value)
                    return;
                this._filterForKeyword = value;
            }
        }

        public void ClearResultsByType(SearchHintResultType resultType) => this._resultsCache[(object)resultType] = (object)new List<string>();

        public virtual void MergeResults(IList queryResults, SearchHintResultType resultType)
        {
            if (queryResults == null)
                return;
            DataProviderTitleList providerTitleList = new DataProviderTitleList();
            for (int index = 0; index < queryResults.Count; ++index)
            {
                DataProviderObject queryResult = queryResults[index] as DataProviderObject;
                string property = (string)queryResult.GetProperty("Title");
                if (!string.IsNullOrEmpty(property) && !providerTitleList.Contains((object)property) && (!this.FilterForKeyword || this.ContainsKeyword(property)))
                    providerTitleList.DataProviders.Add((object)queryResult);
            }
            this._resultsCache[(object)resultType] = (object)providerTitleList;
            ++this._resultsQueueCount;
            Application.DeferredInvoke((DeferredInvokeHandler)delegate
           {
               this.HandleResultsQueue();
           }, (object)null);
        }

        private bool ContainsKeyword(string title)
        {
            title = title.ToLower();
            string[] strArray = this.Entry.Split(' ');
            bool flag = false;
            for (int index = 0; index < strArray.Length; ++index)
            {
                string lower = strArray[index].ToLower();
                if (!string.IsNullOrEmpty(lower) && title.Contains(lower))
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        public object MergeSearchResultDataDelegate(object item1, object item2) => item1 == null || item2 is LibraryDataProviderListItem ? item2 : item1;

        private void HandleResultsQueue()
        {
            if (this._resultsQueueCount == 0)
                return;
            --this._resultsQueueCount;
            this._mergedTitles = new DataProviderTitleList();
            foreach (DictionaryEntry dictionaryEntry in this._resultsCache)
            {
                DataProviderTitleList providerTitleList = dictionaryEntry.Value as DataProviderTitleList;
                this._mergedTitles.InitializeDataProviders(ListHelper.Merge(this._mergedTitles.DataProviders, providerTitleList == null ? (IList)null : providerTitleList.DataProviders, false, (IComparer)new SearchResultDataComparer(), new ListHelper.MergeObjects(this.MergeSearchResultDataDelegate)));
            }
            Application.DeferredInvoke((DeferredInvokeHandler)delegate
           {
               this.HandleResultsQueue();
           }, (object)null);
            this._updateTimer.Enabled = true;
        }

        private void OnUpdateTimerTick(object sender, EventArgs args) => this.Options = (IList)this._mergedTitles;
    }
}
