// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ModelItem
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace Microsoft.Iris
{
    public class ModelItem :
      IModelItem,
      INotifyPropertyChanged,
      IModelItemOwner,
      IDisposable,
      IThreadSafeObject
    {
        private static readonly EventCookie s_propertyChangedEvent = EventCookie.ReserveSlot();
        private static readonly EventCookie s_focusRequestedEvent = EventCookie.ReserveSlot();
        private static readonly DataCookie s_descriptionProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_uniqueIdProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_selectedProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_selectionPolicyProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_ownedObjectsProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_extraDataProperty = DataCookie.ReserveSlot();
        private IModelItemOwner _owner;
        private DynamicData _dataMap;
        private bool _isDisposed;
        private Thread _affinity;

        public ModelItem(IModelItemOwner owner, string description)
        {
            ThreadSafety.InitializeObject((IThreadSafeObject)this);
            this._dataMap = new DynamicData();
            this._dataMap.Create();
            this.SetData(ModelItem.s_descriptionProperty, (object)description);
            this.Owner = owner;
        }

        public ModelItem(IModelItemOwner owner)
          : this(owner, (string)null)
        {
        }

        public ModelItem()
          : this((IModelItemOwner)null)
        {
        }

        ~ModelItem()
        {
            string name = this.GetType().Name;
            string data = (string)this.GetData(ModelItem.s_descriptionProperty);
            this.OnDispose(false);
        }

        public void Dispose() => this.Dispose(ModelItemDisposeMode.RemoveOwnerReference);

        public void Dispose(ModelItemDisposeMode disposeMode)
        {
            using (this.ThreadValidator)
            {
                if (this._isDisposed)
                    return;
                if (this._owner != null)
                {
                    if (disposeMode == ModelItemDisposeMode.RemoveOwnerReference)
                        this._owner.UnregisterObject(this);
                    this._owner = (IModelItemOwner)null;
                }
                try
                {
                    GC.SuppressFinalize((object)this);
                    this.OnDispose(true);
                }
                finally
                {
                    try
                    {
                        this.DisposeOwnedObjects();
                    }
                    finally
                    {
                        this._isDisposed = true;
                    }
                }
            }
        }

        protected virtual void OnDispose(bool disposing)
        {
        }

        protected ThreadSafetyBlock ThreadValidator => new ThreadSafetyBlock((IThreadSafeObject)this);

        Thread IThreadSafeObject.Affinity
        {
            get => this._affinity;
            set => this._affinity = value;
        }

        public IModelItemOwner Owner
        {
            get
            {
                using (this.ThreadValidator)
                    return this._owner;
            }
            set
            {
                using (this.ThreadValidator)
                {
                    if (this._owner == value)
                        return;
                    IModelItemOwner owner = this._owner;
                    owner?.UnregisterObject(this);
                    this._owner = value;
                    if (this._owner != null)
                        this._owner.RegisterObject(this);
                    this.OnOwnerChanged(this._owner, owner);
                }
            }
        }

        public bool IsDisposed
        {
            get
            {
                using (this.ThreadValidator)
                    return this._isDisposed;
            }
        }

        public string Description
        {
            get
            {
                using (this.ThreadValidator)
                    return (string)this.GetData(ModelItem.s_descriptionProperty);
            }
            set
            {
                using (this.ThreadValidator)
                {
                    if (!(this.Description != value))
                        return;
                    this.SetData(ModelItem.s_descriptionProperty, (object)value);
                    this.FirePropertyChanged(nameof(Description));
                }
            }
        }

        public Guid UniqueId
        {
            get
            {
                using (this.ThreadValidator)
                {
                    object data = this.GetData(ModelItem.s_uniqueIdProperty);
                    return data == null ? Guid.Empty : (Guid)data;
                }
            }
            set
            {
                using (this.ThreadValidator)
                {
                    if (!(this.UniqueId != value))
                        return;
                    this.SetData(ModelItem.s_uniqueIdProperty, (object)value);
                    this.FirePropertyChanged(nameof(UniqueId));
                }
            }
        }

        public IDictionary Data
        {
            get
            {
                using (this.ThreadValidator)
                {
                    IDictionary dictionary = (IDictionary)this.GetData(ModelItem.s_extraDataProperty);
                    if (dictionary == null)
                    {
                        dictionary = (IDictionary)new HybridDictionary();
                        this.SetData(ModelItem.s_extraDataProperty, (object)dictionary);
                    }
                    return dictionary;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                using (this.ThreadValidator)
                    this.AddEventHandler(ModelItem.s_propertyChangedEvent, (Delegate)value);
            }
            remove
            {
                using (this.ThreadValidator)
                    this.RemoveEventHandler(ModelItem.s_propertyChangedEvent, (Delegate)value);
            }
        }

        protected void FirePropertyChanged(string property)
        {
            using (this.ThreadValidator)
            {
                if (property == null)
                    throw new ArgumentNullException(nameof(property));
                this.OnPropertyChanged(property);
                if (!(this.GetEventHandler(ModelItem.s_propertyChangedEvent) is PropertyChangedEventHandler eventHandler))
                    return;
                eventHandler((object)this, new PropertyChangedEventArgs(property));
            }
        }

        protected virtual void OnPropertyChanged(string property)
        {
        }

        protected virtual void OnOwnerChanged(IModelItemOwner newOwner, IModelItemOwner oldOwner)
        {
        }

        void IModelItemOwner.RegisterObject(ModelItem item)
        {
            using (this.ThreadValidator)
            {
                if (item == null)
                    throw new ArgumentNullException(nameof(item));
                if (item == this)
                    throw new ArgumentException("Cannot make a ModelItem the owner of itself");
                this.GetOwnedObjects(true).Add(item);
            }
        }

        void IModelItemOwner.UnregisterObject(ModelItem item)
        {
            using (this.ThreadValidator)
            {
                if (item == null)
                    throw new ArgumentNullException(nameof(item));
                Vector<ModelItem> ownedObjects = this.GetOwnedObjects(false);
                if (ownedObjects == null || !ownedObjects.Contains(item))
                    throw new ArgumentException(InvariantString.Format("Cannot unregister an object that was never registered.  Owner \"{0}\" was unable to identify \"{1}\".", (object)this, (object)item));
                ownedObjects.Remove(item);
            }
        }

        private void DisposeOwnedObjects()
        {
            Vector<ModelItem> ownedObjects = this.GetOwnedObjects(false);
            if (ownedObjects == null)
                return;
            foreach (ModelItem modelItem in ownedObjects)
                modelItem.Dispose(ModelItemDisposeMode.KeepOwnerReference);
            this.SetData(ModelItem.s_ownedObjectsProperty, (object)null);
        }

        private Vector<ModelItem> GetOwnedObjects(bool createIfNoneFlag)
        {
            Vector<ModelItem> vector = (Vector<ModelItem>)this.GetData(ModelItem.s_ownedObjectsProperty);
            if (vector == null && createIfNoneFlag)
            {
                vector = new Vector<ModelItem>();
                this.SetData(ModelItem.s_ownedObjectsProperty, (object)vector);
            }
            return vector;
        }

        public bool Selected
        {
            get
            {
                using (this.ThreadValidator)
                {
                    object data = this.GetData(ModelItem.s_selectedProperty);
                    return data != null && (bool)data;
                }
            }
            set
            {
                using (this.ThreadValidator)
                {
                    if (this.Selected == value)
                        return;
                    this.SetData(ModelItem.s_selectedProperty, (object)value);
                    this.FirePropertyChanged(nameof(Selected));
                }
            }
        }

        public override string ToString()
        {
            using (this.ThreadValidator)
            {
                string name = this.GetType().Name;
                string description = this.Description;
                return description != null ? InvariantString.Format("{0}:\"{1}\"", (object)name, (object)description) : name;
            }
        }

        internal object GetData(DataCookie cookie) => this._dataMap.GetData(cookie);

        internal void SetData(DataCookie cookie, object value) => this._dataMap.SetData(cookie, value);

        internal Delegate GetEventHandler(EventCookie cookie) => this._dataMap.GetEventHandler(cookie);

        internal void AddEventHandler(EventCookie cookie, Delegate handlerToAdd) => this._dataMap.AddEventHandler(cookie, handlerToAdd);

        internal void RemoveEventHandler(EventCookie cookie, Delegate handlerToRemove) => this._dataMap.RemoveEventHandler(cookie, handlerToRemove);

        internal void RemoveEventHandlers(EventCookie cookie) => this._dataMap.RemoveEventHandlers(cookie);

        private static uint GetKey(EventCookie cookie)
        {
            uint uint32 = EventCookie.ToUInt32(cookie);
            return uint32 != 0U ? uint32 : throw new ArgumentException("invalid event key");
        }
    }
}
