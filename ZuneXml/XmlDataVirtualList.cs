// Decompiled with JetBrains decompiler
// Type: ZuneXml.XmlDataVirtualList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace ZuneXml
{
    public class XmlDataVirtualList : VirtualList, IXmlDataProviderObject
    {
        private DataProviderQuery _owner;
        private int _currentIndex;
        private object _itemTypeCookie;
        private ConstructObject _itemConstructor;
        private int _chunkStartIndex;
        private List<XmlDataProviderObject> _chunkItems;
        private string _encodedSortBy;
        private string[] _sortBy;
        private bool[] _sortAscending;
        private IPageInfo _nextPage;
        private bool _pageToEnd;

        public XmlDataVirtualList(DataProviderQuery owner, object itemTypeCookie)
          : base(true)
        {
            this._owner = owner;
            this._itemTypeCookie = itemTypeCookie;
            this._itemConstructor = itemTypeCookie == null ? (ConstructObject)null : XmlDataProviderObjectFactory.GetConstructor(itemTypeCookie);
            this._currentIndex = -1;
            this._chunkStartIndex = 0;
            this._encodedSortBy = (string)null;
            this._sortAscending = (bool[])null;
            this._sortBy = (string[])null;
        }

        internal IPageInfo NextPage
        {
            get => this._nextPage;
            set => this._nextPage = value;
        }

        public bool PageToEnd
        {
            get => this._pageToEnd;
            set
            {
                if (this._pageToEnd == value)
                    return;
                this._pageToEnd = value;
                if (!this._pageToEnd)
                    return;
                this.GetNextChunk();
            }
        }

        protected override object OnRequestItem(int index) => this.Data[(object)index];

        protected override void OnRequestSlowData(int index)
        {
            this.NotifySlowDataAcquireComplete(index);
            if (index != this.Count - 1 || this._chunkStartIndex == this.Count || (this.NextPage == null || this.PageToEnd))
                return;
            this.GetNextChunk();
        }

        private void GetNextChunk()
        {
            if (this.NextPage == null)
                return;
            this._chunkStartIndex = this.Count;
            this._currentIndex = -1;
            if (!(this._owner is XmlDataProviderQuery owner))
                return;
            ThreadPool.QueueUserWorkItem(new WaitCallback(XmlDataVirtualList.GetNextChunkThread), (object)new XmlDataVirtualList.GetNextChunkArgs(owner, this.NextPage, this._chunkStartIndex));
        }

        private static void GetNextChunkThread(object obj)
        {
            XmlDataVirtualList.GetNextChunkArgs getNextChunkArgs = (XmlDataVirtualList.GetNextChunkArgs)obj;
            string pageUrl = getNextChunkArgs.PageInfo.GetPageUrl(getNextChunkArgs.StartIndex);
            string pagePostBody = getNextChunkArgs.PageInfo.GetPagePostBody(getNextChunkArgs.StartIndex);
            if (string.IsNullOrEmpty(pageUrl))
                return;
            getNextChunkArgs.Query.GetDataFromResource(pageUrl, pagePostBody, false);
        }

        public bool ProcessXPath(
          string currentXPath,
          Hashtable attributes,
          List<XmlDataProviderQuery.XPathMatch> matches)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(currentXPath))
            {
                if (this._chunkItems == null)
                    this._chunkItems = new List<XmlDataProviderObject>();
                ++this._currentIndex;
                if (this._itemConstructor != null)
                    this._chunkItems.Add(this._itemConstructor(this._owner, this._itemTypeCookie));
                else
                    this._chunkItems.Add(new XmlDataProviderObject(this._owner, this._itemTypeCookie));
            }
            else
            {
                IXmlDataProviderObject chunkItem = (IXmlDataProviderObject)this._chunkItems[this._currentIndex];
                if (chunkItem != null)
                    flag = chunkItem.ProcessXPath(currentXPath, attributes, matches);
            }
            return flag;
        }

        internal void TransferToAppThread()
        {
            if (this._chunkItems == null)
                return;
            foreach (XmlDataProviderObject chunkItem in this._chunkItems)
                chunkItem.TransferToAppThread();
            int count = 0;
            foreach (XmlDataProviderObject chunkItem in this._chunkItems)
            {
                if (!(this._owner is XmlDataProviderQuery owner) || !owner.FilterDataProviderObject(chunkItem))
                    this.Data[(object)(this._chunkStartIndex + count++)] = (object)chunkItem;
            }
            this.AddRange(count);
            this.Sort();
            this._chunkItems.Clear();
        }

        internal void OnQueryComplete()
        {
            if (this.NextPage == null || !this.PageToEnd)
                return;
            this.GetNextChunk();
        }

        private void Sort()
        {
            if (this._sortBy == null || this._sortBy.Length <= 0 || this.Count <= 1)
                return;
            IPageInfo nextPage = this.NextPage;
            XmlDataProviderObject[] array = new XmlDataProviderObject[this.Count];
            if (array == null)
                return;
            for (int index = 0; index < this.Count; ++index)
                array[index] = (XmlDataProviderObject)this.Data[(object)index];
            Array.Sort<XmlDataProviderObject>(array, new Comparison<XmlDataProviderObject>(this.CompareUsingSortBy));
            for (int index = 0; index < this.Count; ++index)
            {
                this.Data[(object)index] = (object)array[index];
                this.Modified(index);
            }
        }

        public string SortBy
        {
            get => this._encodedSortBy;
            set
            {
                if (!(this._encodedSortBy != value))
                    return;
                string[] strArray1 = (string[])null;
                bool[] flagArray = (bool[])null;
                if (!string.IsNullOrEmpty(value))
                {
                    string[] strArray2 = value.Split(',');
                    strArray1 = new string[strArray2.Length];
                    flagArray = new bool[strArray2.Length];
                    for (int index = 0; index < strArray2.Length; ++index)
                        this.ExtractSortData(strArray2[index], out strArray1[index], out flagArray[index]);
                }
                this._sortBy = strArray1;
                this._sortAscending = flagArray;
                this.Sort();
            }
        }

        private void ExtractSortData(string sort, out string filteredValue, out bool sortAscending)
        {
            filteredValue = sort;
            sortAscending = true;
            if (string.IsNullOrEmpty(sort))
                return;
            if (sort[0] == '-')
            {
                filteredValue = sort.Substring(1);
                sortAscending = false;
            }
            else
            {
                if (sort[0] != '+')
                    return;
                filteredValue = sort.Substring(1);
                sortAscending = true;
            }
        }

        private int CompareUsingSortBy(XmlDataProviderObject x, XmlDataProviderObject y)
        {
            int num = 0;
            for (int index = 0; index < this._sortBy.Length; ++index)
            {
                num = XmlDataVirtualList.Compare(x, y, this._sortBy[index], this._sortAscending[index]);
                if (num != 0)
                    break;
            }
            return num;
        }

        private static int Compare(
          XmlDataProviderObject x,
          XmlDataProviderObject y,
          string propertyName,
          bool ascending)
        {
            object property1 = x.GetProperty(propertyName);
            object property2 = y.GetProperty(propertyName);
            IComparable comparable1 = property1 as IComparable;
            IComparable comparable2 = property2 as IComparable;
            if (comparable1 == null || comparable2 == null)
                return 0;
            int num = comparable1.CompareTo((object)comparable2);
            return ascending ? num : -num;
        }

        private class GetNextChunkArgs
        {
            public readonly XmlDataProviderQuery Query;
            public readonly IPageInfo PageInfo;
            public readonly int StartIndex;

            public GetNextChunkArgs(XmlDataProviderQuery query, IPageInfo pageInfo, int startIndex)
            {
                this.Query = query;
                this.PageInfo = pageInfo;
                this.StartIndex = startIndex;
            }
        }
    }
}
