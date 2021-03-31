// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.CodeModel.Cpp.DllProxyList
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.OS;
using Microsoft.Iris.ViewItems;
using System;
using System.Collections;

namespace Microsoft.Iris.CodeModel.Cpp
{
    internal class DllProxyList :
      DllInterfaceProxy,
      IVirtualList,
      INotifyList,
      IList,
      ICollection,
      IEnumerable,
      IUIXListCallbacks
    {
        private UpdateHelper _updater;
        private Repeater _repeater;
        private bool _wantSlowDataRequests;
        private bool _callbacksRegistered;
        private SlowDataAcquireCompleteHandler _slowDataAcquireCompleteHandler;
        private UIListContentsChangedHandler _contentsChanged;

        protected override void LoadWorker(IntPtr nativeObject, IntPtr nativeMarshalAs)
        {
            base.LoadWorker(nativeObject, nativeMarshalAs);
            NativeApi.SUCCEEDED(NativeApi.SpUIXListWantSlowDataRequests(this._interface, out this._wantSlowDataRequests));
            if (!this._wantSlowDataRequests)
                return;
            this._updater = new UpdateHelper(this);
        }

        protected override void OnDispose()
        {
            this.UpdateCallbackRegistration(true);
            base.OnDispose();
        }

        public unsafe int Add(object value)
        {
            int count = 0;
            UIXVariant uixVariant;
            UIXVariant.MarshalObject(value, &uixVariant);
            NativeApi.SUCCEEDED(NativeApi.SpUIXListAdd(this._interface, &uixVariant, out count));
            UIXVariant.CleanupMarshalledObject(&uixVariant);
            return count;
        }

        public bool Contains(object value) => this.IndexOf(value) >= 0;

        public unsafe int IndexOf(object value)
        {
            int index = -1;
            UIXVariant uixVariant;
            UIXVariant.MarshalObject(value, &uixVariant);
            NativeApi.SUCCEEDED(NativeApi.SpUIXListIndexOf(this._interface, &uixVariant, out index));
            UIXVariant.CleanupMarshalledObject(&uixVariant);
            return index;
        }

        public void Clear() => NativeApi.SUCCEEDED(NativeApi.SpUIXListClear(this._interface));

        public unsafe void Insert(int index, object value)
        {
            UIXVariant uixVariant;
            UIXVariant.MarshalObject(value, &uixVariant);
            NativeApi.SUCCEEDED(NativeApi.SpUIXListInsert(this._interface, index, &uixVariant));
            UIXVariant.CleanupMarshalledObject(&uixVariant);
        }

        public unsafe void Remove(object value)
        {
            UIXVariant uixVariant;
            UIXVariant.MarshalObject(value, &uixVariant);
            NativeApi.SUCCEEDED(NativeApi.SpUIXListRemove(this._interface, &uixVariant));
            UIXVariant.CleanupMarshalledObject(&uixVariant);
        }

        public void RemoveAt(int index) => NativeApi.SUCCEEDED(NativeApi.SpUIXListRemoveAt(this._interface, index));

        public void CopyTo(Array array, int index) => throw new NotImplementedException();

        public int Count
        {
            get
            {
                int count = 0;
                NativeApi.SUCCEEDED(NativeApi.SpUIXListGetCount(this._interface, out count));
                return count;
            }
        }

        public bool IsFixedSize => false;

        public bool IsReadOnly => false;

        public bool IsSynchronized => false;

        bool IVirtualList.IsItemAvailable(int index)
        {
            bool isAvailable;
            NativeApi.SUCCEEDED(NativeApi.SpUIXListIsItemAvailable(this._interface, index, out isAvailable));
            return isAvailable;
        }

        public void RequestItem(int index, ItemRequestCallback callback)
        {
            object obj = this[index];
            callback(this, index, obj);
        }

        public unsafe object this[int index]
        {
            get
            {
                object obj = null;
                UIXVariant inboundObject;
                if (NativeApi.SUCCEEDED(NativeApi.SpUIXListGetItem(this._interface, index, out inboundObject)))
                    obj = UIXVariant.GetValue(inboundObject, OwningLoadResult);
                return obj;
            }
            set
            {
                UIXVariant uixVariant;
                UIXVariant.MarshalObject(value, &uixVariant);
                NativeApi.SUCCEEDED(NativeApi.SpUIXListSetItem(this._interface, index, &uixVariant));
                UIXVariant.CleanupMarshalledObject(&uixVariant);
            }
        }

        public object SyncRoot => (object)null;

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public StackIListEnumerator GetEnumerator() => new StackIListEnumerator(this);

        public event UIListContentsChangedHandler ContentsChanged
        {
            add
            {
                if (this._contentsChanged == null)
                    this.UpdateCallbackRegistration();
                this._contentsChanged += value;
            }
            remove
            {
                this._contentsChanged -= value;
                if (this._contentsChanged != null)
                    return;
                this.UpdateCallbackRegistration();
            }
        }

        public void Move(int oldIndex, int newIndex)
        {
            int num = (int)NativeApi.SpUIXListMove(this._interface, oldIndex, newIndex);
        }

        public void ListChanged(int nativeType, int oldIndex, int newIndex, int count)
        {
            bool flag1 = true;
            bool flag2 = true;
            UIListContentsChangeType type = (UIListContentsChangeType)nativeType;
            switch (type)
            {
                case UIListContentsChangeType.Add:
                case UIListContentsChangeType.AddRange:
                case UIListContentsChangeType.Remove:
                case UIListContentsChangeType.Insert:
                case UIListContentsChangeType.InsertRange:
                case UIListContentsChangeType.Clear:
                case UIListContentsChangeType.Reset:
                    if (!flag1)
                        break;
                    if (this._contentsChanged != null)
                        this._contentsChanged(this, new UIListContentsChangedArgs(type, oldIndex, newIndex, count));
                    if (flag2)
                        this.FireNotification(NotificationID.Count);
                    if (this._updater == null)
                        break;
                    this.ForwardListChangeToUpdater(type, oldIndex, newIndex, count);
                    break;
                case UIListContentsChangeType.Move:
                case UIListContentsChangeType.Modified:
                    flag2 = false;
                    goto case UIListContentsChangeType.Add;
                default:
                    flag1 = false;
                    goto case UIListContentsChangeType.Add;
            }
        }

        private void ForwardListChangeToUpdater(
          UIListContentsChangeType type,
          int oldIndex,
          int newIndex,
          int count)
        {
            switch (type)
            {
                case UIListContentsChangeType.Remove:
                    this._updater.RemoveIndex(oldIndex);
                    this._updater.AdjustIndices(oldIndex, -1);
                    break;
                case UIListContentsChangeType.Move:
                    int lowThreshold;
                    int highThreshold;
                    int amt;
                    if (oldIndex < newIndex)
                    {
                        lowThreshold = oldIndex;
                        highThreshold = newIndex;
                        amt = -1;
                    }
                    else
                    {
                        lowThreshold = newIndex;
                        highThreshold = oldIndex;
                        amt = 1;
                    }
                    this._updater.RemoveIndex(oldIndex);
                    this._updater.AdjustIndices(lowThreshold, highThreshold, amt);
                    this._updater.AddIndex(newIndex);
                    break;
                case UIListContentsChangeType.Insert:
                    this._updater.AdjustIndices(newIndex, 1);
                    break;
                case UIListContentsChangeType.InsertRange:
                    this._updater.AdjustIndices(newIndex, count);
                    break;
                case UIListContentsChangeType.Clear:
                case UIListContentsChangeType.Reset:
                    this._updater.Clear();
                    break;
            }
        }

        public bool SlowDataRequestsEnabled => this._wantSlowDataRequests;

        public void NotifyRequestSlowData(int index) => NativeApi.SUCCEEDED(NativeApi.SpUIXListFetchSlowData(this._interface, index));

        public void SlowDataAcquireComplete(int index)
        {
            bool flag = false;
            if (this._slowDataAcquireCompleteHandler != null)
                flag = this._slowDataAcquireCompleteHandler(this, index);
            if (flag)
                return;
            this._updater.NotifySlowDataAcquireComplete(index);
        }

        public SlowDataAcquireCompleteHandler SlowDataAcquireCompleteHandler
        {
            get => this._slowDataAcquireCompleteHandler;
            set
            {
                if (!(value != this._slowDataAcquireCompleteHandler))
                    return;
                this._slowDataAcquireCompleteHandler = value;
                this.UpdateCallbackRegistration();
            }
        }

        public void NotifyVisualsCreated(int index)
        {
            if (this._updater != null)
                this._updater.AddIndex(index);
            NativeApi.SUCCEEDED(NativeApi.SpUIXListNotifyVisualsCreated(this._interface, index));
        }

        public void NotifyVisualsReleased(int index)
        {
            if (this._updater != null)
                this._updater.RemoveIndex(index);
            NativeApi.SUCCEEDED(NativeApi.SpUIXListNotifyVisualsReleased(this._interface, index));
        }

        public Repeater RepeaterHost
        {
            get => this._repeater;
            set
            {
                if (value == this._repeater)
                    return;
                this._repeater = value;
                this.UpdateCallbackRegistration();
            }
        }

        private void UpdateCallbackRegistration() => this.UpdateCallbackRegistration(false);

        private void UpdateCallbackRegistration(bool inDispose)
        {
            bool flag = (this._slowDataAcquireCompleteHandler != null || this._contentsChanged != null || this._repeater != null) & !inDispose;
            if (flag == this._callbacksRegistered)
                return;
            if (flag)
            {
                int num1 = (int)NativeApi.SpUIXListRegisterCallbacks(this._interface, this);
            }
            else
            {
                int num2 = (int)NativeApi.SpUIXListUnregisterCallbacks(this._interface, this);
                if (this._updater != null)
                    this._updater.Clear();
            }
            this._callbacksRegistered = flag;
        }
    }
}
