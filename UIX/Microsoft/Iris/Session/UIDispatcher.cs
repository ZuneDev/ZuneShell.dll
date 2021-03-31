// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Session.UIDispatcher
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Queues;
using Microsoft.Iris.Render;
using System;
using System.Threading;

namespace Microsoft.Iris.Session
{
    internal class UIDispatcher : Dispatcher, IDisposable, IRenderHost
    {
        private static Thread s_mainUIThread;
        private static bool s_exiting;
        private UISession _parentSession;
        private Thread _thread;
        private PriorityQueue _masterQueue;
        private TimeoutManager _timeoutManager;
        private Queue _rpcYieldQueue;
        private Queue _cleanupQueue;
        private UIDispatcher.MessageLoop _messageLoop;
        private bool _respectNativeQuitRequests;
        private PriorityQueue.HookProc _doBatchFlush;
        private bool _shutdown;

        internal UIDispatcher(bool isMainUIThread)
          : this(null, null, 0U, isMainUIThread)
        {
        }

        internal UIDispatcher(
          UISession parentSession,
          TimeoutHandler handlerTimeout,
          uint nTimeoutSec,
          bool isMainUIThread)
        {
            this._thread = Thread.CurrentThread;
            this._parentSession = parentSession;
            this._timeoutManager = new TimeoutManager();
            Queue[] queues = new Queue[17];
            if (parentSession != null)
                queues[6] = parentSession.InputManager.Queue;
            this._masterQueue = new PriorityQueue(queues);
            this._masterQueue.LoopHook = new PriorityQueue.HookProc(this.CheckInterthreadItems);
            this.SetQueueDrainHook(DispatchPriority.Normal, new PriorityQueue.HookProc(this.CheckLoopCondition));
            this.SetQueueDrainHook(DispatchPriority.RPC, new PriorityQueue.HookProc(this.ProcessNativeEvents));
            this.SetQueueDrainHook(DispatchPriority.Idle, new PriorityQueue.HookProc(this.ProcessTimeouts));
            this.SetQueueDrainHook(DispatchPriority.Sleep, new PriorityQueue.HookProc(this.WaitForWork));
            this._rpcYieldQueue = this._masterQueue.BuildSubsetQueue(new int[2]
            {
        5,
        16
            }, true);
            this._cleanupQueue = this._masterQueue.BuildSubsetQueue(new int[1], true);
            this._doBatchFlush = new PriorityQueue.HookProc(this.DoBatchFlush);
            if (!isMainUIThread)
                return;
            UIDispatcher.s_mainUIThread = Thread.CurrentThread;
        }

        public void ShutDown(bool flushRefs)
        {
            this.DoHousekeeping();
            if (flushRefs)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                this.DoHousekeeping();
            }
            if (this._shutdown)
                return;
            this._shutdown = true;
            this.FinalStopDispatch();
        }

        public new void Dispose()
        {
            this.ShutDown(false);
            if (UIDispatcher.s_mainUIThread == Thread.CurrentThread)
            {
                UIDispatcher.s_mainUIThread = null;
                UIDispatcher.s_exiting = true;
            }
            if (this._masterQueue != null)
            {
                this._masterQueue.Dispose();
                this._rpcYieldQueue.Dispose();
                this._cleanupQueue.Dispose();
                this._timeoutManager.Dispose();
            }
            base.Dispose();
        }

        public static UIDispatcher CurrentDispatcher => Dispatcher.CurrentDispatcher as UIDispatcher;

        public static bool IsUIThread => Thread.CurrentThread == UIDispatcher.s_mainUIThread;

        public static Thread MainUIThread => UIDispatcher.s_mainUIThread;

        public static bool Exiting => UIDispatcher.s_exiting;

        public UISession UISession => this._parentSession;

        public TimeoutManager TimeoutManager => this._timeoutManager;

        public bool RespectNativeQuitRequests
        {
            get => this._respectNativeQuitRequests;
            set => this._respectNativeQuitRequests = value;
        }

        void IRenderHost.DeferredInvoke(
          Microsoft.Iris.Render.DeferredInvokePriority priority,
          IDeferredInvokeItem item,
          TimeSpan delay)
        {
            if (delay == TimeSpan.Zero)
            {
                DispatchPriority priority1 = DispatchPriority.Idle;
                switch (priority)
                {
                    case Microsoft.Iris.Render.DeferredInvokePriority.High:
                        priority1 = DispatchPriority.High;
                        break;
                    case Microsoft.Iris.Render.DeferredInvokePriority.Normal:
                        priority1 = DispatchPriority.Normal;
                        break;
                    case Microsoft.Iris.Render.DeferredInvokePriority.VisualUpdate:
                        priority1 = DispatchPriority.Render;
                        break;
                    case Microsoft.Iris.Render.DeferredInvokePriority.Low:
                        priority1 = DispatchPriority.Idle;
                        break;
                    case Microsoft.Iris.Render.DeferredInvokePriority.Idle:
                        priority1 = DispatchPriority.Idle;
                        break;
                }
                UIDispatcher.Post(priority1, DeferredCall.Create(item));
            }
            else
                UIDispatcher.Post(delay, DeferredCall.Create(item));
        }

        public static void Post(DateTime when, QueueItem item)
        {
            Thread mainUiThread = UIDispatcher.MainUIThread;
            if (mainUiThread == null)
                return;
            UIDispatcher.Post(mainUiThread, when, item);
        }

        public static void Post(TimeSpan delay, QueueItem item)
        {
            Thread mainUiThread = UIDispatcher.MainUIThread;
            if (mainUiThread == null)
                return;
            UIDispatcher.Post(mainUiThread, delay, item);
        }

        public static void Post(DispatchPriority priority, QueueItem item)
        {
            Thread mainUiThread = UIDispatcher.MainUIThread;
            if (mainUiThread == null)
                return;
            UIDispatcher.Post(mainUiThread, priority, item);
        }

        public static void Post(Thread thread, DateTime when, QueueItem item) => TimeoutManager.SetTimeoutAbsolute(thread, item, when);

        public static void Post(Thread thread, TimeSpan delay, QueueItem item) => TimeoutManager.SetTimeoutRelative(thread, item, delay);

        public static void Post(Thread thread, DispatchPriority priority, QueueItem item) => Dispatcher.PostItem_AnyThread(thread, item, (int)priority);

        public void Run(LoopCondition condition) => this.MainLoop(_masterQueue, condition);

        public void StopCurrentMessageLoop() => this._messageLoop.QuitPending = true;

        public static void StopCurrentMessageLoop(Thread thread)
        {
            if (thread != null && thread != Thread.CurrentThread)
                DeferredCall.Post(thread, DispatchPriority.Normal, new SimpleCallback(UIDispatcher.StopMessageLoopHandler));
            else
                UIDispatcher.CurrentDispatcher?.StopCurrentMessageLoop();
        }

        public void RPCYield(LoopCondition condition) => this.MainLoop(this._rpcYieldQueue, condition);

        public void DoHousekeeping() => this.MainLoop(this._cleanupQueue, null);

        private void SetQueueLock(DispatchPriority priority, bool value) => this._masterQueue.SetLock((int)priority, value);

        internal void TemporarilyBlockRPCs()
        {
            this.SetQueueLock(DispatchPriority.RPC, true);
            this.RequestBatchFlush();
        }

        public void BlockInputQueue(bool value) => this.SetQueueLock(DispatchPriority.Input, value);

        public bool IsQueueLocked(DispatchPriority priority) => this._masterQueue.IsLocked((int)priority);

        private void SetQueueDrainHook(DispatchPriority priority, PriorityQueue.HookProc hook) => this._masterQueue.SetDrainHook((int)priority, hook);

        private PriorityQueue.HookProc GetQueueDrainHook(DispatchPriority priority) => this._masterQueue.GetDrainHook((int)priority);

        private void MainLoop(Queue queue, LoopCondition condition)
        {
            using (new UIDispatcher.MessageLoop(this, condition))
                this.MainLoop(queue);
        }

        private static void StopMessageLoopHandler() => UIDispatcher.CurrentDispatcher?.StopCurrentMessageLoop();

        internal void RequestBatchFlush()
        {
            if (this.GetQueueDrainHook(DispatchPriority.RenderSync) != null)
                return;
            this.SetQueueDrainHook(DispatchPriority.RenderSync, this._doBatchFlush);
        }

        private void DoBatchFlush(out bool didWork, out bool abort)
        {
            this.SetQueueDrainHook(DispatchPriority.RenderSync, null);
            this.UISession.FlushBatch();
            didWork = true;
            abort = false;
            if (!this.IsQueueLocked(DispatchPriority.RPC))
                return;
            this.SetQueueLock(DispatchPriority.RPC, false);
            didWork = true;
        }

        protected override void PostItem_SameThread(QueueItem item, int priority)
        {
            if (!(this._masterQueue[priority] is SimpleQueue master))
                return;
            master.PostItem(item);
        }

        protected override void PostItems_SameThread(QueueItem.FIFO items, int priority)
        {
            if (!(this._masterQueue[priority] is SimpleQueue master))
                return;
            master.PostItems(items);
        }

        protected override void WakeDispatchThread() => this.UISession.InterThreadWake();

        private void CheckInterthreadItems(out bool didWork, out bool abort)
        {
            didWork = this.DrainFeeder();
            abort = false;
        }

        private void CheckLoopCondition(out bool didWork, out bool abort)
        {
            didWork = false;
            abort = this._messageLoop.QuitPending;
        }

        private void ProcessNativeEvents(out bool didWork, out bool abort)
        {
            didWork = this.UISession.ProcessNativeEvents();
            abort = false;
            if (didWork)
                return;
            this.SetQueueLock(DispatchPriority.RPC, true);
        }

        private void ProcessTimeouts(out bool didWork, out bool abort)
        {
            didWork = this._timeoutManager.ProcessPendingTimeouts();
            abort = false;
        }

        private void WaitForWork(out bool didWork, out bool abort)
        {
            didWork = true;
            abort = false;
            if (this._messageLoop.QuitPending)
            {
                didWork = false;
                abort = true;
            }
            else
            {
                uint nextTimeoutMillis = this._timeoutManager.NextTimeoutMillis;
                if (nextTimeoutMillis == 0U)
                    this._timeoutManager.ProcessPendingTimeouts();
                else
                    this.UISession.WaitForWork(nextTimeoutMillis);
            }
            this.SetQueueLock(DispatchPriority.RPC, false);
        }

        internal static void VerifyOnApplicationThread()
        {
            if (!UIDispatcher.IsUIThread)
                throw new InvalidOperationException("Operation must be performed on the application thread");
        }

        private class MessageLoop : IDisposable
        {
            private UIDispatcher _dispatcher;
            private LoopCondition _condition;
            private UIDispatcher.MessageLoop _parent;
            private bool _quitPending;

            public MessageLoop(UIDispatcher dispatcher, LoopCondition condition)
            {
                this._dispatcher = dispatcher;
                this._condition = condition;
                this._parent = this._dispatcher._messageLoop;
                this._dispatcher._messageLoop = this;
            }

            public void Dispose() => this._dispatcher._messageLoop = this._parent;

            public UIDispatcher.MessageLoop Parent => this._parent;

            public bool QuitPending
            {
                get
                {
                    if (!this._quitPending && this._condition != null)
                        this._quitPending = !this._condition();
                    return this._quitPending;
                }
                set => this._quitPending = value;
            }
        }
    }
}
