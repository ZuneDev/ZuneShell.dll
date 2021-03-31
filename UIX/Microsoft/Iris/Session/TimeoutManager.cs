// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Session.TimeoutManager
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Queues;
using System;
using System.Threading;

namespace Microsoft.Iris.Session
{
    internal class TimeoutManager
    {
        private TimeoutManager.PendingList _pending;
        private DateTime _lastSystemTime;
        private long _lastSystemMilliseconds;
        private static readonly DeferredHandler _cancelTimeoutInterthread = new DeferredHandler(CancelTimeoutInterthread);

        public TimeoutManager()
        {
            this._pending = new TimeoutManager.PendingList();
            this._lastSystemTime = TimeNow;
            this._lastSystemMilliseconds = DispatcherTimer.SystemTickCount.Milliseconds;
        }

        public void Dispose() => this._pending.Dispose();

        public static DateTime TimeNow => DateTime.UtcNow;

        public uint NextTimeoutMillis
        {
            get
            {
                this.SynchronizeSystemTime();
                uint num1 = uint.MaxValue;
                if (!this._pending.IsEmpty)
                {
                    TimeSpan timeSpan = this._pending.NextExpirationTime - TimeNow;
                    if (timeSpan > TimeSpan.Zero)
                    {
                        long ticks = timeSpan.Ticks;
                        long num2 = ticks / 10000L;
                        if (ticks % 10000L > 0L)
                            ++num2;
                        if (num2 < uint.MaxValue)
                            num1 = (uint)num2;
                    }
                    else
                        num1 = 0U;
                }
                return num1;
            }
        }

        public void SetTimeoutAbsolute(QueueItem item, DateTime when) => this.SetTimeoutWorker(TimeNow, null, item, when, false);

        public static void SetTimeoutAbsolute(Thread thread, QueueItem item, DateTime when)
        {
            DateTime timeNow = TimeNow;
            SetTimeoutOnThread(thread, timeNow, item, when, false);
        }

        public void SetTimeoutRelative(QueueItem item, TimeSpan delay)
        {
            DateTime timeNow = TimeNow;
            this.SetTimeoutWorker(timeNow, null, item, timeNow + delay, true);
        }

        public static void SetTimeoutRelative(Thread thread, QueueItem item, TimeSpan delay)
        {
            DateTime timeNow = TimeNow;
            SetTimeoutOnThread(thread, timeNow, item, timeNow + delay, true);
        }

        public void CancelTimeout(QueueItem item)
        {
            if (!UIDispatcher.IsUIThread)
                DeferredCall.Post(DispatchPriority.Normal, _cancelTimeoutInterthread, item);
            else
                this._pending.RemoveItem(item);
        }

        public bool ProcessPendingTimeouts()
        {
            bool flag = false;
            this.SynchronizeSystemTime();
            DateTime timeNow = TimeNow;
            while (true)
            {
                QueueItem queueItem = this._pending.RemoveNextExpired(timeNow);
                if (queueItem != null)
                {
                    UIDispatcher.Post(Thread.CurrentThread, DispatchPriority.Normal, queueItem);
                    flag = true;
                }
                else
                    break;
            }
            return flag;
        }

        private void SetTimeoutWorker(
          DateTime currentTime,
          QueueItem preWrapped,
          QueueItem item,
          DateTime when,
          bool isRelative)
        {
            if (!UIDispatcher.IsUIThread)
            {
                UIDispatcher.Post(UIDispatcher.MainUIThread, DispatchPriority.Normal, PendingList.GetInterthreadItem(item, when, isRelative));
            }
            else
            {
                this.SynchronizeSystemTime();
                if (preWrapped != null)
                    this._pending.AddItemInternal(preWrapped);
                else
                    this._pending.AddItem(item, when, isRelative);
            }
        }

        private static void SetTimeoutOnThread(
          Thread thread,
          DateTime currentTime,
          QueueItem item,
          DateTime when,
          bool isRelative)
        {
            if (thread == Thread.CurrentThread)
                DeliverToCurrentThread(currentTime, null, item, when, isRelative);
            else
                UIDispatcher.Post(thread, DispatchPriority.Normal, PendingList.GetInterthreadItem(item, when, isRelative));
        }

        private static void DeliverToCurrentThread(
          DateTime currentTime,
          QueueItem preWrapped,
          QueueItem item,
          DateTime when,
          bool isRelative)
        {
            TimeoutManagerForCurrentThread?.SetTimeoutWorker(currentTime, preWrapped, item, when, isRelative);
        }

        private static void CancelTimeoutInterthread(object param) => TimeoutManagerForCurrentThread?.CancelTimeout((QueueItem)param);

        private void SynchronizeSystemTime()
        {
            DateTime timeNow = TimeNow;
            long milliseconds = DispatcherTimer.SystemTickCount.Milliseconds;
            DateTime dateTime = this._lastSystemTime + TimeSpan.FromMilliseconds(milliseconds - this._lastSystemMilliseconds);
            this._lastSystemTime = timeNow;
            this._lastSystemMilliseconds = milliseconds;
            if (Math.Abs((timeNow - dateTime).TotalSeconds) <= 30.0)
                return;
            this._pending.ShiftRelativeTimeouts(timeNow - dateTime);
        }

        private static TimeoutManager TimeoutManagerForCurrentThread
        {
            get
            {
                TimeoutManager timeoutManager = null;
                UIDispatcher currentDispatcher = UIDispatcher.CurrentDispatcher;
                if (currentDispatcher != null)
                    timeoutManager = currentDispatcher.TimeoutManager;
                return timeoutManager;
            }
        }

        internal class PendingList : QueueItem.Chain
        {
            private TimeoutManager.PendingList.PendingItem _head;

            public override void Dispose() => base.Dispose();

            public bool IsEmpty => this._head == null;

            public DateTime NextExpirationTime => this._head.expireTime;

            public bool NextItemIs(QueueItem innerItem)
            {
                QueueItem queueItem = null;
                if (this._head != null)
                    queueItem = this._head.innerItem;
                return queueItem == innerItem;
            }

            public void AddItem(QueueItem innerItem, DateTime expireTime, bool isRelative) => this.AddWorker(null, innerItem, expireTime, isRelative);

            public void AddItemInternal(QueueItem outerItem)
            {
                TimeoutManager.PendingList.PendingItem outerItem1 = outerItem as TimeoutManager.PendingList.PendingItem;
                this.AddWorker(outerItem1, outerItem1.innerItem, outerItem1.expireTime, outerItem1.isRelative);
            }

            public static QueueItem GetInterthreadItem(
              QueueItem innerItem,
              DateTime expireTime,
              bool isRelative)
            {
                return new TimeoutManager.PendingList.PendingItem(innerItem, expireTime, isRelative);
            }

            public void ShiftRelativeTimeouts(TimeSpan spanTime)
            {
                if (this.IsEmpty)
                    return;
                Vector<TimeoutManager.PendingList.PendingItem> vector = new Vector<TimeoutManager.PendingList.PendingItem>();
                foreach (TimeoutManager.PendingList.PendingItem pendingItem in this)
                {
                    if (pendingItem.isRelative)
                        vector.Add(pendingItem);
                }
                foreach (TimeoutManager.PendingList.PendingItem outerItem in vector)
                    this.RemoveWorker(outerItem);
                foreach (TimeoutManager.PendingList.PendingItem pendingItem in vector)
                    this.AddItem(pendingItem.innerItem, pendingItem.expireTime + spanTime, true);
            }

            public bool RemoveItem(QueueItem innerItem)
            {
                foreach (TimeoutManager.PendingList.PendingItem outerItem in this)
                {
                    if (outerItem.innerItem == innerItem)
                    {
                        this.RemoveWorker(outerItem);
                        return true;
                    }
                }
                return false;
            }

            public QueueItem RemoveNextExpired(DateTime threshold)
            {
                QueueItem queueItem = null;
                TimeoutManager.PendingList.PendingItem head = this._head;
                if (head != null && head.expireTime <= threshold)
                {
                    this.RemoveWorker(head);
                    queueItem = head.innerItem;
                }
                return queueItem;
            }

            public QueueItem.Chain.ChainEnumerator GetEnumerator()
            {
                QueueItem tail = _head;
                if (tail != null)
                    tail = PrevItem(tail);
                return new QueueItem.Chain.ChainEnumerator(tail);
            }

            private void AddWorker(
              TimeoutManager.PendingList.PendingItem outerItem,
              QueueItem innerItem,
              DateTime expireTime,
              bool isRelative)
            {
                ValidateAdd(innerItem);
                TimeoutManager.PendingList.PendingItem pendingItem1 = null;
                TimeoutManager.PendingList.PendingItem pendingItem2 = null;
                if (this._head != null)
                {
                    foreach (TimeoutManager.PendingList.PendingItem pendingItem3 in this)
                    {
                        if (expireTime < pendingItem3.expireTime)
                        {
                            pendingItem1 = pendingItem3;
                            break;
                        }
                    }
                    pendingItem2 = pendingItem1 ?? this._head;
                }
                if (outerItem == null)
                    outerItem = new TimeoutManager.PendingList.PendingItem(innerItem, expireTime, isRelative);
                this.Link(innerItem, null, false);
                this.Link(outerItem, pendingItem2, true);
                if (this._head != pendingItem1)
                    return;
                this._head = outerItem;
            }

            private void RemoveWorker(TimeoutManager.PendingList.PendingItem outerItem)
            {
                if (this._head == outerItem)
                    this._head = IsOnlyChild(_head) ? null : NextItem(_head) as TimeoutManager.PendingList.PendingItem;
                this.Unlink(outerItem);
                this.Unlink(outerItem.innerItem);
            }

            internal class PendingItem : QueueItem
            {
                public QueueItem innerItem;
                public DateTime expireTime;
                public bool isRelative;

                public PendingItem(QueueItem innerItem, DateTime expireTime, bool isRelative)
                {
                    this.innerItem = innerItem;
                    this.expireTime = expireTime;
                    this.isRelative = isRelative;
                }

                public override void Dispatch() => DeliverToCurrentThread(TimeNow, this, this.innerItem, this.expireTime, this.isRelative);
            }
        }
    }
}
