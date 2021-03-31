// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.DataProviderQuery
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Microsoft.Iris
{
    public abstract class DataProviderQuery :
      IDataProviderQuery,
      IDataProviderBaseObject,
      AssemblyObjectProxyHelper.IFrameworkProxyObject,
      IDisposableObject,
      INotifyPropertyChanged
    {
        private MarkupDataQuerySchema _typeSchema;
        private MarkupDataQuery _internalQuery;
        private Dictionary<string, object> _propertyValues;
        private object _result;
        private DataProviderQueryStatus _status;
        private bool _isDisposed;
        private bool _isInvalid;
        private bool _enabled = true;
        private bool _initialized;

        protected abstract void BeginExecute();

        public object ResultTypeCookie => _typeSchema.ResultType;

        public object Result
        {
            get => this._result;
            set
            {
                if (object.Equals(this._result, value))
                    return;
                this._result = value;
                this.FirePropertyChanged(nameof(Result));
            }
        }

        public DataProviderQueryStatus Status
        {
            get => this._status;
            set
            {
                if (this._status == value)
                    return;
                this._status = value;
                this.FirePropertyChanged(nameof(Status));
            }
        }

        public bool Enabled
        {
            get => this._enabled;
            set
            {
                if (this._enabled == value)
                    return;
                this._enabled = value;
                this.FirePropertyChanged(nameof(Enabled));
                if (!this._enabled || !this._isInvalid)
                    return;
                this.DeferredBeginExecute(null);
            }
        }

        protected bool Initialized => this._initialized;

        public void Refresh() => this.BeginExecute();

        public virtual object GetProperty(string propertyName)
        {
            lock (this.SynchronizedPropertyStorage)
            {
                if (this._propertyValues != null)
                {
                    object obj;
                    if (this._propertyValues.TryGetValue(propertyName, out obj))
                        return obj;
                }
            }
            return null;
        }

        public virtual void SetProperty(string propertyName, object value)
        {
            bool flag1 = false;
            lock (this.SynchronizedPropertyStorage)
            {
                bool flag2 = false;
                if (this._propertyValues == null)
                {
                    this._propertyValues = new Dictionary<string, object>();
                    flag2 = true;
                }
                if (!flag2 && this._propertyValues.ContainsKey(propertyName))
                {
                    if (object.Equals(this._propertyValues[propertyName], value))
                        goto label_8;
                }
                this._propertyValues[propertyName] = value;
                flag1 = true;
            }
        label_8:
            if (!flag1)
                return;
            this.FirePropertyChanged(propertyName);
        }

        protected void FirePropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(propertyName);
            if (this._internalQuery != null)
                this._internalQuery.FireNotificationThreadSafe(propertyName);
            bool invalidatesQuery = this._typeSchema.InvalidatesQuery(propertyName);
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new DataProviderPropertyChangedEventArgs(propertyName, invalidatesQuery));
            if (this._isInvalid || !invalidatesQuery)
                return;
            this._isInvalid = true;
            if (!this._enabled)
                return;
            Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredBeginExecute), DeferredInvokePriority.Low);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void DeferredBeginExecute(object args)
        {
            if (!this._isInvalid)
                return;
            this._isInvalid = false;
            this.BeginExecute();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
        }

        protected virtual void OnDispose()
        {
        }

        public bool IsDisposed => this._isDisposed;

        void IDisposableObject.DeclareOwner(object owner) => this.DeclareOwnerWorker(owner);

        void IDataProviderQuery.DeclareOwner(object owner) => this.DeclareOwnerWorker(owner);

        private void DeclareOwnerWorker(object owner)
        {
        }

        void IDisposableObject.TransferOwnership(object owner)
        {
        }

        void IDisposableObject.Dispose(object owner) => this.DisposeWorker(owner);

        void IDataProviderQuery.Dispose(object owner) => this.DisposeWorker(owner);

        private void DisposeWorker(object owner)
        {
            if (this._isDisposed)
                return;
            this.OnDispose();
            this._isDisposed = true;
        }

        protected DataProviderQuery(object typeCookie)
        {
            this._typeSchema = typeCookie as MarkupDataQuerySchema;
            if (this._typeSchema == null)
                throw new ArgumentException(nameof(typeCookie), "typeCookie must be the queryTypeCookie passed to ConstructQuery");
            this._isInvalid = true;
        }

        object AssemblyObjectProxyHelper.IFrameworkProxyObject.FrameworkObject => _internalQuery;

        internal string ProviderName => this._typeSchema.ProviderName;

        void IDataProviderQuery.SetInternalObject(MarkupDataQuery internalQuery) => this._internalQuery = internalQuery;

        void IDataProviderQuery.OnInitialize()
        {
            this._initialized = true;
            if (!this._enabled)
                return;
            this.DeferredBeginExecute(null);
        }

        public override string ToString() => this._typeSchema.Name;

        private object SynchronizedPropertyStorage => _internalQuery;
    }
}
