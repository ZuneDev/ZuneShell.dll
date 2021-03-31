// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Queues.SimpleQueue
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Queues
{
    internal class SimpleQueue : Queue
    {
        private QueueItem.FIFO _fifo;

        public SimpleQueue() => this._fifo = new QueueItem.FIFO();

        public override void Dispose()
        {
            this._fifo.Dispose();
            base.Dispose();
        }

        public bool IsEmpty => this._fifo.Head == null;

        public override QueueItem GetNextItem()
        {
            QueueItem head = this._fifo.Head;
            if (head != null)
                this._fifo.Remove(head);
            return head;
        }

        public void PostItem(QueueItem item)
        {
            if (!this._fifo.Append(item))
                return;
            this.OnWake();
        }

        internal void PostItems(QueueItem.FIFO items)
        {
            if (!this._fifo.Append(items))
                return;
            this.OnWake();
        }
    }
}
