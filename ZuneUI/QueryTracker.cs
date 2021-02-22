// Decompiled with JetBrains decompiler
// Type: ZuneUI.QueryTracker
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using ZuneXml;

namespace ZuneUI
{
    public class QueryTracker : ModelItem
    {
        private List<QueryStatus> _listStatus;
        private DataProviderQueryStatus _status;
        private string _busyString;
        private List<QueryTracker.QueryReference> _queries;
        private Dictionary<DataProviderQueryStatus, int> _statusPriority;
        private Dictionary<DataProviderQueryStatus, int> _statusCount;
        private List<object> _errorCodes;
        private PropertyChangedEventHandler _handler;
        private bool _deferredStatusCheck;

        public QueryTracker()
        {
            this._queries = new List<QueryTracker.QueryReference>();
            this._handler = new PropertyChangedEventHandler(this.QueryPropertyChanged);
            this._statusPriority = new Dictionary<DataProviderQueryStatus, int>();
            this._statusPriority[DataProviderQueryStatus.Error] = 0;
            this._statusPriority[DataProviderQueryStatus.RequestingData] = 1;
            this._statusPriority[DataProviderQueryStatus.ProcessingData] = 2;
            this._statusPriority[DataProviderQueryStatus.Complete] = 3;
            this._statusPriority[DataProviderQueryStatus.Idle] = 4;
            this._statusCount = new Dictionary<DataProviderQueryStatus, int>();
        }

        public DataProviderQueryStatus Status
        {
            get => this._status;
            private set
            {
                if (this._status == value)
                    return;
                this._status = value;
                this.FirePropertyChanged(nameof(Status));
            }
        }

        public string BusyString
        {
            get => this._busyString;
            private set
            {
                if (!(this._busyString != value))
                    return;
                this._busyString = value;
                this.FirePropertyChanged(nameof(BusyString));
            }
        }

        public List<QueryStatus> ListStatus
        {
            get => this._listStatus;
            private set
            {
                if (this._listStatus == value)
                    return;
                this._listStatus = value;
                this.FirePropertyChanged(nameof(ListStatus));
            }
        }

        public List<object> ErrorCodes
        {
            get => this._errorCodes;
            private set
            {
                if (this._errorCodes == value)
                    return;
                this._errorCodes = value;
                this.FirePropertyChanged(nameof(ErrorCodes));
            }
        }

        public int QueryCount => this._queries.Count;

        public int ErrorCount
        {
            get
            {
                int num;
                this._statusCount.TryGetValue(DataProviderQueryStatus.Error, out num);
                return num;
            }
        }

        public int CompleteCount
        {
            get
            {
                int num;
                this._statusCount.TryGetValue(DataProviderQueryStatus.Complete, out num);
                return num;
            }
        }

        public int IdleCount
        {
            get
            {
                int num;
                this._statusCount.TryGetValue(DataProviderQueryStatus.Idle, out num);
                return num;
            }
        }

        public void Register(string name, DataProviderQuery query) => this.Register(name, query, "");

        public void Register(string name, DataProviderQuery query, string busyString)
        {
            INotifyPropertyChanged notifyPropertyChanged = (INotifyPropertyChanged)query;
            if (notifyPropertyChanged != null)
                notifyPropertyChanged.PropertyChanged += this._handler;
            this._queries.Add(new QueryTracker.QueryReference(name, query, busyString));
            this.FirePropertyChanged("QueryCount");
            this.EnqueueStatusCheck();
        }

        private void QueryPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (!(args.PropertyName == "Status"))
                return;
            this.EnqueueStatusCheck();
        }

        private void EnqueueStatusCheck()
        {
            if (this._deferredStatusCheck)
                return;
            Application.DeferredInvoke(new DeferredInvokeHandler(this.CheckStatus), (object)null);
            this._deferredStatusCheck = true;
        }

        private void CheckStatus(object args)
        {
            this._deferredStatusCheck = false;
            DataProviderQueryStatus key = DataProviderQueryStatus.Idle;
            int num1 = this._statusPriority[key];
            this._statusCount = new Dictionary<DataProviderQueryStatus, int>();
            List<QueryStatus> queryStatusList = new List<QueryStatus>();
            List<QueryTracker.QueryReference> queryReferenceList = new List<QueryTracker.QueryReference>();
            List<object> objectList = (List<object>)null;
            string str = (string)null;
            foreach (QueryTracker.QueryReference query1 in this._queries)
            {
                DataProviderQuery query2 = query1.Value;
                if (query2 == null)
                    queryReferenceList.Add(query1);
                else if (query2.IsDisposed)
                {
                    queryReferenceList.Add(query1);
                }
                else
                {
                    object errorCode = QueryTracker.GetErrorCode(query2);
                    if (errorCode != null)
                    {
                        if (objectList == null)
                            objectList = new List<object>();
                        objectList.Add(errorCode);
                    }
                    DataProviderQueryStatus providerQueryStatus = query2.Status;
                    if (query2 == null)
                        providerQueryStatus = DataProviderQueryStatus.Idle;
                    int num2 = this._statusPriority[providerQueryStatus];
                    if (num2 < num1)
                    {
                        key = providerQueryStatus;
                        num1 = num2;
                    }
                    queryStatusList.Add(new QueryStatus(query1.Name, providerQueryStatus));
                    int num3 = 0;
                    this._statusCount.TryGetValue(providerQueryStatus, out num3);
                    this._statusCount[providerQueryStatus] = num3 + 1;
                    if (QueryHelper.IsBusy(query2.Status))
                        str = query1.BusyString;
                }
            }
            foreach (QueryTracker.QueryReference queryReference in queryReferenceList)
            {
                this._queries.Remove(queryReference);
                this.FirePropertyChanged("QueryCount");
            }
            this.Status = key;
            this.ListStatus = queryStatusList;
            this.ErrorCodes = objectList;
            this.BusyString = str;
            this.FirePropertyChanged("ErrorCount");
            this.FirePropertyChanged("CompleteCount");
            this.FirePropertyChanged("IdleCount");
        }

        public static object GetErrorCode(DataProviderQuery query) => query is XmlDataProviderQuery dataProviderQuery ? dataProviderQuery.ErrorCode : (object)null;

        public static bool Is404(object errorCode) => errorCode is HttpStatusCode.NotFound;

        public static bool Is410(object errorCode) => errorCode is HttpStatusCode.Gone;

        public static bool Is407(object errorCode) => errorCode is HttpStatusCode.ProxyAuthenticationRequired;

        private struct QueryReference
        {
            private WeakReference _value;
            private string _name;
            private string _busyString;

            public QueryReference(string name, DataProviderQuery value, string busyString)
            {
                this._name = name;
                this._value = new WeakReference((object)value);
                this._busyString = busyString;
            }

            public string Name => this._name;

            public DataProviderQuery Value => this._value.Target as DataProviderQuery;

            public string BusyString => this._busyString;
        }
    }
}
