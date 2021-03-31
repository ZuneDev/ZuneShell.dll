// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Queues.Dispatcher
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System;
using System.Threading;

namespace Microsoft.Iris.Queues
{
    internal abstract class Dispatcher
    {
        private static Interconnect s_interconnect = new Interconnect();
        [ThreadStatic]
        private static Dispatcher s_threadDispatcher;
        private Thread _owningThread;
        private uint _enterCount;
        private Feeder _feeder;
        private int _feederReadStamp;
        private int _feederWriteStamp;

        public Dispatcher()
        {
            this._owningThread = Thread.CurrentThread;
            this._feeder = this.EnterDispatch();
        }

        public void FinalStopDispatch()
        {
            this.LeaveDispatch();
            this._feeder = null;
        }

        public void Dispose()
        {
            if (this._enterCount != 1U)
                return;
            this.LeaveDispatch();
        }

        public static Dispatcher CurrentDispatcher => s_threadDispatcher;

        public static void PostItem_AnyThread(Thread thread, QueueItem item, int priority)
        {
            if (thread == null || thread == Thread.CurrentThread)
            {
                Dispatcher currentDispatcher = CurrentDispatcher;
                if (currentDispatcher != null)
                {
                    currentDispatcher.PostItem_SameThread(item, priority);
                    return;
                }
            }
            s_interconnect.PostItem(thread, item, priority);
        }

        public Thread DispatchThread => this._owningThread;

        public void MainLoop(Queue queue)
        {
            this.EnterDispatch();
            try
            {
                while (true)
                {
                    QueueItem nextItem = queue.GetNextItem();
                    if (nextItem != null)
                        nextItem.Dispatch();
                    else
                        break;
                }
            }
            finally
            {
                this.LeaveDispatch();
            }
        }

        protected abstract void PostItem_SameThread(QueueItem item, int priority);

        protected abstract void PostItems_SameThread(QueueItem.FIFO items, int priority);

        protected abstract void WakeDispatchThread();

        private Feeder EnterDispatch()
        {
            bool isRoot = this._enterCount == 0U;
            ++this._enterCount;
            if (isRoot)
                s_threadDispatcher = this;
            return s_interconnect.EnterDispatch(this, isRoot);
        }

        private void LeaveDispatch()
        {
            --this._enterCount;
            bool isRoot = this._enterCount == 0U;
            if (isRoot)
                s_threadDispatcher = null;
            s_interconnect.LeaveDispatch(this, isRoot);
        }

        public void NotifyFeederItems()
        {
            if (Thread.CurrentThread == this._owningThread)
                return;
            Interlocked.Increment(ref this._feederWriteStamp);
            this.WakeDispatchThread();
        }

        internal bool DrainFeeder()
        {
            int feederWriteStamp = this._feederWriteStamp;
            if (feederWriteStamp == this._feederReadStamp)
                return false;
            bool flag = false;
            this._feederReadStamp = feederWriteStamp;
            QueueItem.FIFO[] recycled = this._feeder.HandoffFIFOs();
            if (recycled != null)
            {
                for (int priority = 0; priority < recycled.Length; ++priority)
                {
                    QueueItem.FIFO items = recycled[priority];
                    if (items != null)
                    {
                        this.PostItems_SameThread(items, priority);
                        flag = true;
                    }
                }
                this._feeder.RecycleFIFOs(recycled);
            }
            return flag;
        }
    }
}
