// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Session.DeferredCall
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Queues;
using Microsoft.Iris.Render;
using System;
using System.Threading;

namespace Microsoft.Iris.Session
{
    internal sealed class DeferredCall : QueueItem
    {
        private const int c_maxCacheCount = 100;
        private DeferredCall.CallType _callType;
        private Delegate _target;
        private object _param;
        private EventArgs _args;
        private static DeferredCall s_cachedList = null;
        private static int s_cachedCount = 0;
        private static object s_cacheLock = new object();

        private DeferredCall()
        {
        }

        private static DeferredCall AllocateFromCache()
        {
            DeferredCall deferredCall = null;
            lock (DeferredCall.s_cacheLock)
            {
                if (DeferredCall.s_cachedList != null)
                {
                    deferredCall = DeferredCall.s_cachedList;
                    DeferredCall.s_cachedList = (DeferredCall)deferredCall._next;
                    deferredCall._next = null;
                    --DeferredCall.s_cachedCount;
                }
            }
            if (deferredCall == null)
                deferredCall = new DeferredCall();
            return deferredCall;
        }

        public static DeferredCall Create(SimpleCallback callback)
        {
            DeferredCall deferredCall = DeferredCall.AllocateFromCache();
            deferredCall._callType = DeferredCall.CallType.Simple;
            deferredCall._target = callback;
            return deferredCall;
        }

        public static DeferredCall Create(DeferredHandler handler, object param)
        {
            DeferredCall deferredCall = DeferredCall.AllocateFromCache();
            deferredCall._callType = DeferredCall.CallType.OneParam;
            deferredCall._target = handler;
            deferredCall._param = param;
            return deferredCall;
        }

        public static DeferredCall Create(
          EventHandler handler,
          object sender,
          EventArgs args)
        {
            DeferredCall deferredCall = DeferredCall.AllocateFromCache();
            deferredCall._callType = DeferredCall.CallType.Event;
            deferredCall._target = handler;
            deferredCall._param = sender;
            deferredCall._args = args;
            return deferredCall;
        }

        public static DeferredCall Create(IDeferredInvokeItem item)
        {
            DeferredCall deferredCall = DeferredCall.AllocateFromCache();
            deferredCall._callType = DeferredCall.CallType.RenderItem;
            deferredCall._param = item;
            return deferredCall;
        }

        public override void Dispatch()
        {
            switch (this._callType)
            {
                case DeferredCall.CallType.Simple:
                    ((SimpleCallback)this._target)();
                    break;
                case DeferredCall.CallType.OneParam:
                    ((DeferredHandler)this._target)(this._param);
                    break;
                case DeferredCall.CallType.Event:
                    ((EventHandler)this._target)(this._param, this._args);
                    break;
                case DeferredCall.CallType.RenderItem:
                    ((IDeferredInvokeItem)this._param).Dispatch();
                    break;
                default:
                    throw new InvalidOperationException();
            }
            this._callType = DeferredCall.CallType.None;
            this._target = null;
            this._param = null;
            this._args = null;
            this._prev = null;
            this._next = null;
            this._owner = null;
            lock (DeferredCall.s_cacheLock)
            {
                if (DeferredCall.s_cachedCount >= 100)
                    return;
                this._next = s_cachedList;
                DeferredCall.s_cachedList = this;
                ++DeferredCall.s_cachedCount;
            }
        }

        public static void Post(DispatchPriority priority, SimpleCallback callback)
        {
            QueueItem queueItem = DeferredCall.Create(callback);
            UIDispatcher.Post(priority, queueItem);
        }

        public static void Post(Thread thread, DispatchPriority priority, SimpleCallback callback)
        {
            QueueItem queueItem = DeferredCall.Create(callback);
            UIDispatcher.Post(thread, priority, queueItem);
        }

        public static void Post(DispatchPriority priority, DeferredHandler handler)
        {
            QueueItem queueItem = DeferredCall.Create(handler, null);
            UIDispatcher.Post(priority, queueItem);
        }

        public static void Post(DispatchPriority priority, DeferredHandler handler, object param)
        {
            QueueItem queueItem = DeferredCall.Create(handler, param);
            UIDispatcher.Post(priority, queueItem);
        }

        public static void Post(TimeSpan delay, DeferredHandler handler, object param)
        {
            QueueItem queueItem = DeferredCall.Create(handler, param);
            UIDispatcher.Post(delay, queueItem);
        }

        public static void Post(
          Thread thread,
          DispatchPriority priority,
          DeferredHandler handler,
          object param)
        {
            QueueItem queueItem = DeferredCall.Create(handler, param);
            UIDispatcher.Post(thread, priority, queueItem);
        }

        private enum CallType
        {
            None,
            Simple,
            OneParam,
            Event,
            RenderItem,
        }
    }
}
