// Decompiled with JetBrains decompiler
// Type: Microsoft.Zune.Shell.AggregateDataProviderQuery
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Microsoft.Zune.Shell
{
    public class AggregateDataProviderQuery : DataProviderQuery
    {
        private List<DataProviderQuery> _currentQueries = new List<DataProviderQuery>();

        internal static void Register() => Application.RegisterDataProvider("Aggregate", new DataProviderQueryFactory(AggregateDataProviderQuery.ConstructAggregateDataProviderQuery));

        internal static DataProviderQuery ConstructAggregateDataProviderQuery(
          object queryTypeCookie)
        {
            return (DataProviderQuery)new AggregateDataProviderQuery(queryTypeCookie);
        }

        protected AggregateDataProviderQuery(object queryTypeCookie)
          : base(queryTypeCookie)
        {
        }

        protected override void BeginExecute()
        {
            this.InitializeCurrentQueries();
            this.RefreshCurrentQueries();
        }

        private void InitializeCurrentQueries()
        {
            this.Result = (object)null;
            this.UnsubscribeFromCurrentQueries();
            this._currentQueries.Clear();
            if (this.GetProperty("Queries") is IList property)
            {
                foreach (object obj in (IEnumerable)property)
                {
                    if (obj is DataProviderQuery dataProviderQuery)
                        this._currentQueries.Add(dataProviderQuery);
                }
            }
            this.SubscribeToCurrentQueries();
        }

        private void RefreshCurrentQueries()
        {
            foreach (DataProviderQuery currentQuery in this._currentQueries)
                currentQuery.Refresh();
        }

        private void SubscribeToCurrentQueries()
        {
            foreach (DataProviderQuery currentQuery in this._currentQueries)
                currentQuery.PropertyChanged += new PropertyChangedEventHandler(this.OnQueryPropertyChanged);
        }

        private void UnsubscribeFromCurrentQueries()
        {
            foreach (DataProviderQuery currentQuery in this._currentQueries)
                currentQuery.PropertyChanged -= new PropertyChangedEventHandler(this.OnQueryPropertyChanged);
        }

        private void OnQueryPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (!("Status" == args.PropertyName))
                return;
            this.UpdateStatusAndResult();
        }

        private void UpdateStatusAndResult()
        {
            int num = 0;
            bool flag = false;
            foreach (DataProviderQuery currentQuery in this._currentQueries)
            {
                if (DataProviderQueryStatus.Complete == currentQuery.Status || DataProviderQueryStatus.Error == currentQuery.Status)
                {
                    ++num;
                    if (DataProviderQueryStatus.Error == currentQuery.Status)
                        flag = true;
                }
            }
            if (this._currentQueries.Count == num)
            {
                this.Status = DataProviderQueryStatus.ProcessingData;
                ArrayList arrayList = new ArrayList();
                foreach (DataProviderQuery currentQuery in this._currentQueries)
                    arrayList.Add(currentQuery.Result);
                this.Result = (object)arrayList;
                this.Status = flag ? DataProviderQueryStatus.Error : DataProviderQueryStatus.Complete;
            }
            else
                this.Status = DataProviderQueryStatus.RequestingData;
        }

        protected override void OnDispose()
        {
            this.UnsubscribeFromCurrentQueries();
            base.OnDispose();
        }
    }
}
