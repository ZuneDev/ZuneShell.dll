// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.AssemblyObjectProxyHelper
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup.UIX;
using Microsoft.Iris.ModelItems;
using Microsoft.Iris.ViewItems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Microsoft.Iris.Markup
{
    internal static class AssemblyObjectProxyHelper
    {
        private static AssemblyObjectProxyHelper.ProxyTypeInfo[] s_proxyTypeInfoTable;
        private static Type s_typeofString;

        public static void InitializeStatics()
        {
            s_typeofString = typeof(string);
            s_proxyTypeInfoTable = new AssemblyObjectProxyHelper.ProxyTypeInfo[7]
            {
        new AssemblyObjectProxyHelper.ProxyTypeInfo(typeof (ICommand), typeof (AssemblyObjectProxyHelper.ProxyCommand),  CommandSchema.Type),
        new AssemblyObjectProxyHelper.ProxyTypeInfo(typeof (IValueRange), typeof (AssemblyObjectProxyHelper.ProxyValueRange),  ValueRangeSchema.Type),
        new AssemblyObjectProxyHelper.ProxyTypeInfo(typeof (IList), typeof (AssemblyObjectProxyHelper.ProxyList),  ListSchema.Type),
        new AssemblyObjectProxyHelper.ProxyTypeInfo(typeof (IDictionary), typeof (AssemblyObjectProxyHelper.ProxyDictionary),  DictionarySchema.Type),
        new AssemblyObjectProxyHelper.ProxyTypeInfo(typeof (IEnumerator), typeof (AssemblyObjectProxyHelper.ProxyListEnumerator),  EnumeratorSchema.Type),
        new AssemblyObjectProxyHelper.ProxyTypeInfo(typeof (IDisposable), typeof (AssemblyObjectProxyHelper.ProxyObject),  null),
        new AssemblyObjectProxyHelper.ProxyTypeInfo(typeof (INotifyPropertyChanged), typeof (AssemblyObjectProxyHelper.ProxyObject),  null)
            };
        }

        public static AssemblyTypeSchema CreateProxySchema(Type assemblyType)
        {
            foreach (AssemblyObjectProxyHelper.ProxyTypeInfo proxyTypeInfo in s_proxyTypeInfoTable)
            {
                if (proxyTypeInfo.type.IsAssignableFrom(assemblyType))
                {
                    AssemblyTypeSchema assemblyTypeSchema = new FrameworkCompatibleAssemblyTypeSchema(assemblyType, proxyTypeInfo.proxyType);
                    if (proxyTypeInfo.equivalents != null)
                        assemblyTypeSchema.ShareEquivalents(proxyTypeInfo.equivalents);
                    return assemblyTypeSchema;
                }
            }
            return new StandardAssemblyTypeSchema(assemblyType);
        }

        internal static Type ProxyListType => typeof(AssemblyObjectProxyHelper.ProxyList);

        internal static Type ProxyDictionaryType => typeof(AssemblyObjectProxyHelper.ProxyDictionary);

        internal static Type ProxyCommandType => typeof(AssemblyObjectProxyHelper.ProxyCommand);

        internal static Type ProxyValueRangeType => typeof(AssemblyObjectProxyHelper.ProxyValueRange);

        internal static object WrapObject(TypeSchema typeSchema, object instance)
        {
            if (instance == null)
                return null;
            if (instance.GetType().IsPrimitive)
                return instance;
            switch (instance)
            {
                case Type type:
                    return AssemblyLoadResult.MapType(type);
                case AssemblyObjectProxyHelper.IFrameworkProxyObject frameworkProxyObject:
                    return frameworkProxyObject.FrameworkObject;
                default:
                    AssemblyObjectProxyHelper.ProxyObject proxyObject = null;
                    bool isDisposable = instance is IDisposable;
                    bool notifiesOnChange = instance is INotifyPropertyChanged;
                    switch (instance)
                    {
                        case ICommand _:
                            proxyObject = new AssemblyObjectProxyHelper.ProxyCommand(instance);
                            break;
                        case IValueRange _:
                            proxyObject = new AssemblyObjectProxyHelper.ProxyValueRange(instance);
                            break;
                        case IDictionary _:
                            proxyObject = new AssemblyObjectProxyHelper.ProxyDictionary(instance);
                            break;
                        case IList _:
                            proxyObject = !(instance is Group) ? (!(instance is IVirtualList) ? (!(instance is INotifyList) ? new AssemblyObjectProxyHelper.ProxyList(instance) : new AssemblyObjectProxyHelper.ProxyNotifyList(instance)) : new AssemblyObjectProxyHelper.ProxyVirtualNotifyList(instance, instance is INotifyList)) : new AssemblyObjectProxyHelper.ProxyGroup(instance, instance is INotifyList);
                            break;
                        default:
                            if (isDisposable || notifiesOnChange)
                            {
                                proxyObject = new AssemblyObjectProxyHelper.ProxyObject(instance);
                                break;
                            }
                            break;
                    }
                    if (proxyObject == null)
                        return instance;
                    proxyObject.SetIntrinsicState(isDisposable, notifiesOnChange);
                    return proxyObject;
            }
        }

        internal static object UnwrapObject(object instance)
        {
            if (instance == null)
                return null;
            Type type = instance.GetType();
            if (type.IsPrimitive || type == s_typeofString)
                return instance;
            switch (instance)
            {
                case AssemblyObjectProxyHelper.IAssemblyProxyObject assemblyProxyObject:
                    return assemblyProxyObject.AssemblyObject;
                case IList uixList:
                    return new AssemblyObjectProxyHelper.ReverseProxyList(uixList);
                case IDictionary _:
                case IDisposable _:
                    return new AssemblyObjectProxyHelper.WrappedFrameworkObject(instance);
                default:
                    return instance;
            }
        }

        private struct ProxyTypeInfo
        {
            public Type type;
            public Type proxyType;
            public Vector<TypeSchema> equivalents;

            public ProxyTypeInfo(Type type, Type proxyType, TypeSchema equivalence)
            {
                this.type = type;
                this.proxyType = proxyType;
                this.equivalents = null;
                if (equivalence == null)
                    return;
                this.equivalents = new Vector<TypeSchema>(1);
                this.equivalents.Add(equivalence);
            }
        }

        public interface IFrameworkProxyObject
        {
            object FrameworkObject { get; }
        }

        private class WrappedFrameworkObject : AssemblyObjectProxyHelper.IFrameworkProxyObject
        {
            private object _frameworkObject;

            public WrappedFrameworkObject(object frameworkObject) => this._frameworkObject = frameworkObject;

            public object FrameworkObject => this._frameworkObject;
        }

        public interface IAssemblyProxyObject
        {
            object AssemblyObject { get; }
        }

        private class ProxyObject :
          DisposableObject,
          INotifyObject,
          AssemblyObjectProxyHelper.IAssemblyProxyObject,
          IModelItemOwner,
          ISchemaInfo
        {
            protected object _assemblyObject;
            private bool _isDisposable;
            private bool _isModelItemOwner;
            private bool _hasPropertyChangedHandlerAttached;
            private NotifyService _notifier = new NotifyService();

            public ProxyObject(object assemblyObject) => this._assemblyObject = assemblyObject;

            public void SetIntrinsicState(bool isDisposable, bool notifiesOnChange) => this._isDisposable = isDisposable;

            protected override void OnDispose()
            {
                if (this._hasPropertyChangedHandlerAttached)
                {
                    ((INotifyPropertyChanged)this._assemblyObject).PropertyChanged -= new PropertyChangedEventHandler(this.OnPropertyChanged);
                    this._hasPropertyChangedHandlerAttached = false;
                }
                this._notifier.ClearListeners();
                if (this._isDisposable && (!(this._assemblyObject is ModelItem) || this._isModelItemOwner))
                    ((IDisposable)this._assemblyObject).Dispose();
                base.OnDispose();
            }

            protected override void OnOwnerDeclared(object owner)
            {
                base.OnOwnerDeclared(owner);
                if (!(this._assemblyObject is ModelItem assemblyObject))
                    return;
                assemblyObject.Owner = this;
            }

            public void AddListener(Listener listener)
            {
                if (!this._hasPropertyChangedHandlerAttached)
                {
                    ((INotifyPropertyChanged)this._assemblyObject).PropertyChanged += new PropertyChangedEventHandler(this.OnPropertyChanged);
                    this._hasPropertyChangedHandlerAttached = true;
                }
                this._notifier.AddListener(listener);
            }

            private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
            {
                this._notifier.FireThreadSafe(args.PropertyName);
                if (this._notifier.HasListeners)
                    return;
                ((INotifyPropertyChanged)this._assemblyObject).PropertyChanged -= new PropertyChangedEventHandler(this.OnPropertyChanged);
                this._hasPropertyChangedHandlerAttached = false;
            }

            public void RegisterObject(ModelItem modelItem)
            {
                if (modelItem != this._assemblyObject)
                    throw new ArgumentException("Unexpected model item provided for ownership registration");
                this._isModelItemOwner = true;
            }

            public void UnregisterObject(ModelItem modelItem)
            {
                if (!this._isModelItemOwner || modelItem != this._assemblyObject)
                    throw new ArgumentException("Unexpected model item provided for ownership unregistration");
                this._isModelItemOwner = false;
            }

            public object AssemblyObject => this._assemblyObject;

            public TypeSchema TypeSchema => AssemblyLoadResult.MapType(this._assemblyObject.GetType());

            public override bool Equals(object rhs)
            {
                if (rhs is AssemblyObjectProxyHelper.IAssemblyProxyObject)
                    rhs = ((AssemblyObjectProxyHelper.IAssemblyProxyObject)rhs).AssemblyObject;
                return this._assemblyObject.Equals(rhs);
            }

            public override int GetHashCode() => this._assemblyObject.GetHashCode();

            public override string ToString() => this._assemblyObject.ToString();
        }

        private class ProxyCommand : AssemblyObjectProxyHelper.ProxyObject, IUICommand
        {
            public ProxyCommand(object assemblyObject)
              : base(assemblyObject)
            {
            }

            protected ICommand ExternalCommand => (ICommand)this._assemblyObject;

            public bool Available
            {
                get => this.ExternalCommand.Available;
                set => this.ExternalCommand.Available = value;
            }

            public InvokePriority Priority
            {
                get
                {
                    switch (this.ExternalCommand.Priority)
                    {
                        case InvokePolicy.Synchronous:
                        case InvokePolicy.AsynchronousNormal:
                            return InvokePriority.Normal;
                        default:
                            return InvokePriority.Low;
                    }
                }
                set
                {
                    if (value == InvokePriority.Normal)
                        this.ExternalCommand.Priority = InvokePolicy.AsynchronousNormal;
                    else
                        this.ExternalCommand.Priority = InvokePolicy.AsynchronousLowPri;
                }
            }

            public void Invoke() => this.ExternalCommand.Invoke();
        }

        private class ProxyValueRange : AssemblyObjectProxyHelper.ProxyObject, IUIValueRange
        {
            public ProxyValueRange(object assemblyObject)
              : base(assemblyObject)
            {
            }

            protected IValueRange ExternalRange => (IValueRange)this._assemblyObject;

            public object ObjectValue => AssemblyLoadResult.WrapObject(this.ExternalRange.Value);

            public bool HasPreviousValue => this.ExternalRange.HasPreviousValue;

            public bool HasNextValue => this.ExternalRange.HasNextValue;

            public void PreviousValue() => this.ExternalRange.PreviousValue();

            public void NextValue() => this.ExternalRange.NextValue();
        }

        private class ProxyList :
          AssemblyObjectProxyHelper.ProxyObject,
          IUIList,
          IList,
          ICollection,
          IEnumerable
        {
            private bool _canSearch;

            public ProxyList(object assemblyObject)
              : base(assemblyObject)
              => this._canSearch = assemblyObject is ISearchableList;

            protected IList ExternalList => (IList)this._assemblyObject;

            public int Add(object value) => this.ExternalList.Add(AssemblyLoadResult.UnwrapObject(value));

            public void Clear() => this.ExternalList.Clear();

            public bool Contains(object value) => this.ExternalList.Contains(AssemblyLoadResult.UnwrapObject(value));

            public int IndexOf(object value) => this.ExternalList.IndexOf(AssemblyLoadResult.UnwrapObject(value));

            public void Insert(int index, object value) => this.ExternalList.Insert(index, AssemblyLoadResult.UnwrapObject(value));

            public bool IsFixedSize => this.ExternalList.IsFixedSize;

            public bool IsReadOnly => this.ExternalList.IsReadOnly;

            public void Remove(object value) => this.ExternalList.Remove(AssemblyLoadResult.UnwrapObject(value));

            public void RemoveAt(int index) => this.ExternalList.RemoveAt(index);

            public object this[int index]
            {
                get => AssemblyLoadResult.WrapObject(this.ExternalList[index]);
                set => this.ExternalList[index] = AssemblyLoadResult.UnwrapObject(value);
            }

            public void CopyTo(Array array, int index)
            {
                int index1 = index;
                for (int index2 = 0; index2 < this.ExternalList.Count; ++index2)
                {
                    array.SetValue(AssemblyLoadResult.WrapObject(this.ExternalList[index2]), index1);
                    ++index1;
                }
            }

            public int Count => this.ExternalList.Count;

            public bool IsSynchronized => this.ExternalList.IsSynchronized;

            public object SyncRoot => this.ExternalList.SyncRoot;

            public IEnumerator GetEnumerator() => new AssemblyObjectProxyHelper.ProxyListEnumerator(this.ExternalList.GetEnumerator());

            public bool CanSearch => this._canSearch;

            public int SearchForString(string searchString) => this._canSearch ? ((ISearchableList)this.ExternalList).SearchForString(searchString) : -1;

            public virtual void Move(int oldIndex, int newIndex)
            {
                object external = this.ExternalList[oldIndex];
                this.RemoveAt(oldIndex);
                this.Insert(newIndex, external);
            }
        }

        private class ProxyListEnumerator : IEnumerator, AssemblyObjectProxyHelper.IAssemblyProxyObject
        {
            private IEnumerator _assemblyEnumerator;

            public ProxyListEnumerator(object assemblyObject) => this._assemblyEnumerator = (IEnumerator)assemblyObject;

            public bool MoveNext() => this._assemblyEnumerator.MoveNext();

            public void Reset() => this._assemblyEnumerator.Reset();

            public object Current => AssemblyLoadResult.WrapObject(this._assemblyEnumerator.Current);

            public object AssemblyObject => _assemblyEnumerator;
        }

        private class ProxyNotifyList :
          AssemblyObjectProxyHelper.ProxyList,
          INotifyList,
          IList,
          ICollection,
          IEnumerable
        {
            private bool _isNotifyList;
            private UIListContentsChangedHandler _listContentsChangedHandler;
            private Delegate _handlersAttachedToMe;

            public ProxyNotifyList(object assemblyObject, bool isNotifyList)
              : base(assemblyObject)
              => this._isNotifyList = isNotifyList;

            public ProxyNotifyList(object assemblyObject)
              : this(assemblyObject, true)
            {
            }

            protected INotifyList ExternalNotifyList => (INotifyList)this._assemblyObject;

            public override void Move(int oldIndex, int newIndex) => this.ExternalNotifyList.Move(oldIndex, newIndex);

            public event UIListContentsChangedHandler ContentsChanged
            {
                add
                {
                    if (!this._isNotifyList)
                        return;
                    if (this._listContentsChangedHandler == null)
                    {
                        this._listContentsChangedHandler = new UIListContentsChangedHandler(this.OnListContentsChanged);
                        this.ExternalNotifyList.ContentsChanged += this._listContentsChangedHandler;
                    }
                    this._handlersAttachedToMe = Delegate.Combine(this._handlersAttachedToMe, value);
                }
                remove
                {
                    if (!this._isNotifyList)
                        return;
                    this._handlersAttachedToMe = Delegate.Remove(this._handlersAttachedToMe, value);
                    if ((object)this._handlersAttachedToMe != null)
                        return;
                    this.ExternalNotifyList.ContentsChanged -= this._listContentsChangedHandler;
                    this._listContentsChangedHandler = null;
                }
            }

            private void OnListContentsChanged(IList senderList, UIListContentsChangedArgs args) => ((UIListContentsChangedHandler)this._handlersAttachedToMe)(this, args);
        }

        private class ProxyVirtualNotifyList :
          AssemblyObjectProxyHelper.ProxyNotifyList,
          IVirtualList,
          INotifyList,
          IList,
          ICollection,
          IEnumerable
        {
            private ItemRequestCallback _onItemGeneratedCallback;
            private Dictionary<object, object> _itemCallbacks;

            public ProxyVirtualNotifyList(object assemblyObject, bool isNotifyList)
              : base(assemblyObject, isNotifyList)
            {
            }

            protected IVirtualList ExternalVirtualList => (IVirtualList)this._assemblyObject;

            public void RequestItem(int index, ItemRequestCallback callback)
            {
                if (this._itemCallbacks == null)
                    this._itemCallbacks = new Dictionary<object, object>();
                this._itemCallbacks[index] = callback;
                if (this._onItemGeneratedCallback == null)
                    this._onItemGeneratedCallback = new ItemRequestCallback(this.OnItemGenerated);
                this.ExternalVirtualList.RequestItem(index, this._onItemGeneratedCallback);
            }

            public bool IsItemAvailable(int index) => this.ExternalVirtualList.IsItemAvailable(index);

            public void NotifyVisualsCreated(int index) => this.ExternalVirtualList.NotifyVisualsCreated(index);

            public void NotifyVisualsReleased(int index) => this.ExternalVirtualList.NotifyVisualsReleased(index);

            public bool SlowDataRequestsEnabled => this.ExternalVirtualList.SlowDataRequestsEnabled;

            public void NotifyRequestSlowData(int index) => this.ExternalVirtualList.NotifyRequestSlowData(index);

            public Repeater RepeaterHost
            {
                get => this.ExternalVirtualList.RepeaterHost;
                set => this.ExternalVirtualList.RepeaterHost = value;
            }

            public SlowDataAcquireCompleteHandler SlowDataAcquireCompleteHandler
            {
                get => this.ExternalVirtualList.SlowDataAcquireCompleteHandler;
                set => this.ExternalVirtualList.SlowDataAcquireCompleteHandler = value;
            }

            private void OnItemGenerated(object sender, int index, object item)
            {
                ItemRequestCallback itemCallback = (ItemRequestCallback)this._itemCallbacks[index];
                this._itemCallbacks.Remove(index);
                itemCallback(this, index, AssemblyLoadResult.WrapObject(item));
            }
        }

        private class ProxyGroup :
          AssemblyObjectProxyHelper.ProxyVirtualNotifyList,
          IUIGroup,
          IList,
          ICollection,
          IEnumerable
        {
            public ProxyGroup(object assemblyObject, bool isNotifyList)
              : base(assemblyObject, isNotifyList)
            {
            }

            protected Group ExternalGroup => (Group)this._assemblyObject;

            public int StartIndex => this.ExternalGroup.StartIndex;

            public int EndIndex => this.ExternalGroup.EndIndex;
        }

        private class ProxyDictionary :
          AssemblyObjectProxyHelper.ProxyObject,
          IDictionary,
          ICollection,
          IEnumerable
        {
            public ProxyDictionary(object assemblyObject)
              : base(assemblyObject)
            {
            }

            protected IDictionary ExternalDictionary => (IDictionary)this._assemblyObject;

            public bool IsReadOnly => this.ExternalDictionary.IsReadOnly;

            public bool Contains(object key) => this.ExternalDictionary.Contains(AssemblyLoadResult.WrapObject(key));

            public bool IsFixedSize => false;

            public void Remove(object key)
            {
            }

            public void Clear() => this.ExternalDictionary.Clear();

            public void Add(object key, object value) => this.ExternalDictionary.Add(AssemblyLoadResult.WrapObject(key), AssemblyLoadResult.WrapObject(value));

            public ICollection Keys => (ICollection)null;

            public ICollection Values => (ICollection)null;

            public object this[object key]
            {
                get => AssemblyLoadResult.UnwrapObject(this.ExternalDictionary[AssemblyLoadResult.WrapObject(key)]);
                set => this.ExternalDictionary[AssemblyLoadResult.WrapObject(key)] = AssemblyLoadResult.WrapObject(value);
            }

            public void CopyTo(Array array, int index)
            {
            }

            public int Count => this.ExternalDictionary.Count;

            public bool IsSynchronized => false;

            public object SyncRoot => (object)null;

            public IEnumerator GetEnumerator() => (IEnumerator)null;

            IDictionaryEnumerator IDictionary.GetEnumerator() => (IDictionaryEnumerator)null;
        }

        private class ReverseProxyList :
          IList,
          ICollection,
          IEnumerable,
          AssemblyObjectProxyHelper.IFrameworkProxyObject
        {
            private IList _uixList;

            public ReverseProxyList(IList uixList) => this._uixList = uixList;

            public object FrameworkObject => _uixList;

            public int Add(object value) => this._uixList.Add(AssemblyLoadResult.WrapObject(value));

            public void Clear() => this._uixList.Clear();

            public bool Contains(object value) => this._uixList.Contains(AssemblyLoadResult.WrapObject(value));

            public int IndexOf(object value) => this._uixList.IndexOf(AssemblyLoadResult.WrapObject(value));

            public void Insert(int index, object value) => this._uixList.Insert(index, AssemblyLoadResult.WrapObject(value));

            public bool IsFixedSize => this._uixList.IsFixedSize;

            public bool IsReadOnly => this._uixList.IsReadOnly;

            public void Remove(object value) => this._uixList.Remove(AssemblyLoadResult.WrapObject(value));

            public void RemoveAt(int index) => this._uixList.RemoveAt(index);

            public object this[int index]
            {
                get => AssemblyLoadResult.UnwrapObject(this._uixList[index]);
                set => this._uixList[index] = AssemblyLoadResult.WrapObject(value);
            }

            public void CopyTo(Array array, int index)
            {
                int index1 = index;
                for (int index2 = 0; index2 < this._uixList.Count; ++index2)
                {
                    array.SetValue(AssemblyLoadResult.UnwrapObject(this._uixList[index2]), index1);
                    ++index1;
                }
            }

            public int Count => this._uixList.Count;

            public bool IsSynchronized => this._uixList.IsSynchronized;

            public object SyncRoot => this._uixList.SyncRoot;

            public IEnumerator GetEnumerator() => new AssemblyObjectProxyHelper.ReverseProxyListEnumerator(this._uixList.GetEnumerator());
        }

        private class ReverseProxyListEnumerator :
          IEnumerator,
          AssemblyObjectProxyHelper.IFrameworkProxyObject
        {
            private IEnumerator _uixEnumerator;

            public ReverseProxyListEnumerator(IEnumerator uixEnumerator) => this._uixEnumerator = uixEnumerator;

            public bool MoveNext() => this._uixEnumerator.MoveNext();

            public void Reset() => this._uixEnumerator.Reset();

            public object Current => AssemblyLoadResult.UnwrapObject(this._uixEnumerator.Current);

            public object FrameworkObject => _uixEnumerator;
        }
    }
}
