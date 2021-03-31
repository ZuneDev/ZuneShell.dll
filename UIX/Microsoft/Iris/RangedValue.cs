// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.RangedValue
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.ModelItems;
using System;
using System.ComponentModel;

namespace Microsoft.Iris
{
    public class RangedValue :
      ModelItem,
      IValueRange,
      IModelItem,
      INotifyPropertyChanged,
      IModelItemOwner,
      IUIRangedValue,
      IUIValueRange,
      INotifyObject,
      AssemblyObjectProxyHelper.IFrameworkProxyObject,
      AssemblyObjectProxyHelper.IAssemblyProxyObject,
      IDisposableObject
    {
        private Microsoft.Iris.ModelItems.RangedValue _rangedValue;
        private NotifyService _notifier = new NotifyService();
        private CodeListeners _listeners;

        public RangedValue(IModelItemOwner owner, string description)
          : base(owner, description)
          => this.Initialize();

        public RangedValue(IModelItemOwner owner)
          : this(owner, null)
        {
        }

        public RangedValue()
          : this(null)
        {
        }

        public float Value
        {
            get
            {
                using (this.ThreadValidator)
                    return this._rangedValue.Value;
            }
            set
            {
                using (this.ThreadValidator)
                    this._rangedValue.Value = value;
            }
        }

        object IValueRange.Value
        {
            get
            {
                using (this.ThreadValidator)
                    return ((IUIValueRange)this._rangedValue).ObjectValue;
            }
        }

        object IUIValueRange.ObjectValue => ((IUIValueRange)this._rangedValue).ObjectValue;

        public float MinValue
        {
            get
            {
                using (this.ThreadValidator)
                    return this._rangedValue.MinValue;
            }
            set
            {
                using (this.ThreadValidator)
                    this._rangedValue.MinValue = value <= (double)this.MaxValue ? value : throw new ArgumentException(InvariantString.Format("MinValue must be less than or equal to MaxValue.  Value Supplied was {0}, MaxValue is {1}", value, MaxValue));
            }
        }

        public float MaxValue
        {
            get
            {
                using (this.ThreadValidator)
                    return this._rangedValue.MaxValue;
            }
            set
            {
                using (this.ThreadValidator)
                    this._rangedValue.MaxValue = value >= (double)this.MinValue ? value : throw new ArgumentException(InvariantString.Format("MaxValue must be greater than or equal to MinValue.  Value Supplied was {0}, MinValue is {1}", value, MinValue));
            }
        }

        public float Step
        {
            get
            {
                using (this.ThreadValidator)
                    return this._rangedValue.Step;
            }
            set
            {
                using (this.ThreadValidator)
                    this._rangedValue.Step = value;
            }
        }

        public float Range
        {
            get
            {
                using (this.ThreadValidator)
                    return this._rangedValue.Range;
            }
        }

        public bool HasNextValue
        {
            get
            {
                using (this.ThreadValidator)
                    return this._rangedValue.HasNextValue;
            }
        }

        public bool HasPreviousValue
        {
            get
            {
                using (this.ThreadValidator)
                    return this._rangedValue.HasPreviousValue;
            }
        }

        public void NextValue()
        {
            using (this.ThreadValidator)
                this._rangedValue.NextValue();
        }

        public void PreviousValue()
        {
            using (this.ThreadValidator)
                this._rangedValue.PreviousValue();
        }

        protected override void OnDispose(bool disposing)
        {
            base.OnDispose(disposing);
            if (disposing)
            {
                this._notifier.ClearListeners();
                this._listeners.Dispose(this);
            }
            this._rangedValue = null;
        }

        object AssemblyObjectProxyHelper.IFrameworkProxyObject.FrameworkObject => this;

        object AssemblyObjectProxyHelper.IAssemblyProxyObject.AssemblyObject => this;

        private void Initialize()
        {
            this._rangedValue = this.CreateInternalRangedValue();
            Vector<Listener> listeners = new Vector<Listener>(7);
            DelegateListener.OnNotifyCallback callback = new DelegateListener.OnNotifyCallback(this.OnInternalPropertyChanged);
            listeners.Add(new DelegateListener(_rangedValue, NotificationID.MinValue, callback));
            listeners.Add(new DelegateListener(_rangedValue, NotificationID.MaxValue, callback));
            listeners.Add(new DelegateListener(_rangedValue, NotificationID.Step, callback));
            listeners.Add(new DelegateListener(_rangedValue, NotificationID.Range, callback));
            listeners.Add(new DelegateListener(_rangedValue, NotificationID.Value, callback));
            listeners.Add(new DelegateListener(_rangedValue, NotificationID.HasPreviousValue, callback));
            listeners.Add(new DelegateListener(_rangedValue, NotificationID.HasNextValue, callback));
            this._listeners = new CodeListeners(listeners);
            this._listeners.DeclareOwner(this);
        }

        internal virtual Microsoft.Iris.ModelItems.RangedValue CreateInternalRangedValue() => new Microsoft.Iris.ModelItems.RangedValue();

        protected override void OnPropertyChanged(string property)
        {
            base.OnPropertyChanged(property);
            this._notifier.FireThreadSafe(property);
        }

        private void OnInternalPropertyChanged(DelegateListener listener) => this.FirePropertyChanged(listener.Watch);

        void IDisposableObject.DeclareOwner(object owner)
        {
        }

        void IDisposableObject.TransferOwnership(object owner)
        {
        }

        void IDisposableObject.Dispose(object owner) => this.Dispose();

        void INotifyObject.AddListener(Listener listener) => this._notifier.AddListener(listener);
    }
}
