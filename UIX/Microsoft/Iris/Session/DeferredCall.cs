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
            lock (s_cacheLock)
            {
                if (s_cachedList != null)
                {
                    deferredCall = s_cachedList;
                    s_cachedList = (DeferredCall)deferredCall._next;
                    deferredCall._next = null;
                    --s_cachedCount;
                }
            }
            if (deferredCall == null)
                deferredCall = new DeferredCall();
            return deferredCall;
        }

        public static DeferredCall Create(SimpleCallback callback)
        {
            DeferredCall deferredCall = AllocateFromCache();
            deferredCall._callType = CallType.Simple;
            deferredCall._target = callback;
            return deferredCall;
        }

        public static DeferredCall Create(DeferredHandler handler, object param)
        {
            DeferredCall deferredCall = AllocateFromCache();
            deferredCall._callType = CallType.OneParam;
            deferredCall._target = handler;
            deferredCall._param = param;
            return deferredCall;
        }

        public static DeferredCall Create(
          EventHandler handler,
          object sender,
          EventArgs args)
        {
            DeferredCall deferredCall = AllocateFromCache();
            deferredCall._callType = CallType.Event;
            deferredCall._target = handler;
            deferredCall._param = sender;
            deferredCall._args = args;
            return deferredCall;
        }

        public static DeferredCall Create(IDeferredInvokeItem item)
        {
            DeferredCall deferredCall = AllocateFromCache();
            deferredCall._callType = CallType.RenderItem;
            deferredCall._param = item;
            return deferredCall;
        }

        public override void Dispatch()
        {
            switch (this._callType)
            {
                case CallType.Simple:
                    ((SimpleCallback)this._target)();
                    break;
                case CallType.OneParam:
                    ((DeferredHandler)this._target)(this._param);
                    break;
                case CallType.Event:
                    ((EventHandler)this._target)(this._param, this._args);
                    break;
                case CallType.RenderItem:
                    ((IDeferredInvokeItem)this._param).Dispatch();
                    break;
                default:
                    throw new InvalidOperationException();
            }
            this._callType = CallType.None;
            this._target = null;
            this._param = null;
            this._args = null;
            this._prev = null;
            this._next = null;
            this._owner = null;
            lock (s_cacheLock)
            {
                if (s_cachedCount >= 100)
                    return;
                this._next = s_cachedList;
                s_cachedList = this;
                ++s_cachedCount;
            }
        }

        public static void Post(DispatchPriority priority, SimpleCallback callback)
        {
            QueueItem queueItem = Create(callback);
            UIDispatcher.Post(priority, queueItem);
        }

        public static void Post(Thread thread, DispatchPriority priority, SimpleCallback callback)
        {
            QueueItem queueItem = Create(callback);
            UIDispatcher.Post(thread, priority, queueItem);
        }

        public static void Post(DispatchPriority priority, DeferredHandler handler)
        {
            QueueItem queueItem = Create(handler, null);
            UIDispatcher.Post(priority, queueItem);
        }

        public static void Post(DispatchPriority priority, DeferredHandler handler, object param)
        {
            QueueItem queueItem = Create(handler, param);
            UIDispatcher.Post(priority, queueItem);
        }

        public static void Post(TimeSpan delay, DeferredHandler handler, object param)
        {
            QueueItem queueItem = Create(handler, param);
            UIDispatcher.Post(delay, queueItem);
        }

        public static void Post(
          Thread thread,
          DispatchPriority priority,
          DeferredHandler handler,
          object param)
        {
            QueueItem queueItem = Create(handler, param);
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
