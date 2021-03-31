// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Queues.Feeder
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Queues
{
    internal class Feeder
    {
        private Dispatcher _dispatcher;
        private bool _hasItems;
        private QueueItem.FIFO[] _fifos;

        public bool HasItems => this._hasItems;

        public void EnterDispatch(Dispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
            if (!this._hasItems)
                return;
            this._dispatcher.NotifyFeederItems();
        }

        public void LeaveDispatch(Dispatcher dispatcher) => this._dispatcher = (Dispatcher)null;

        public void PostItem(QueueItem item, int priority)
        {
            bool flag = false;
            lock (this)
            {
                if (this._fifos == null)
                    this._fifos = new QueueItem.FIFO[32];
                (this._fifos[priority] ?? (this._fifos[priority] = new QueueItem.FIFO())).Append(item);
                if (!this._hasItems)
                {
                    this._hasItems = true;
                    if (this._dispatcher != null)
                        flag = true;
                }
            }
            if (!flag)
                return;
            this._dispatcher.NotifyFeederItems();
        }

        public QueueItem.FIFO[] HandoffFIFOs()
        {
            lock (this)
            {
                QueueItem.FIFO[] fifos = this._fifos;
                this._fifos = (QueueItem.FIFO[])null;
                this._hasItems = false;
                return fifos;
            }
        }

        public void RecycleFIFOs(QueueItem.FIFO[] recycled)
        {
            if (this._fifos != null)
                return;
            lock (this)
            {
                if (this._fifos != null)
                    return;
                this._fifos = recycled;
            }
        }
    }
}
