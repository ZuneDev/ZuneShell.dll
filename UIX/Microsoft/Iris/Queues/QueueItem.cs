// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Queues.QueueItem
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System.Diagnostics;

namespace Microsoft.Iris.Queues
{
    internal abstract class QueueItem
    {
        protected QueueItem _prev;
        protected QueueItem _next;
        protected QueueItem.Chain _owner;

        public bool IsPending => this._owner != null;

        public abstract void Dispatch();

        public override string ToString() => this.GetType().Name;

        internal class Chain
        {
            public virtual void Dispose()
            {
            }

            protected static void ValidateAdd(QueueItem item)
            {
            }

            protected void ValidateRemove(QueueItem item)
            {
            }

            protected void Link(QueueItem item, QueueItem anchor, bool before)
            {
                this.UpdateOwners(item, item, null, this);
                this.LinkItems(item, item, anchor, before);
            }

            protected void Unlink(QueueItem item)
            {
                this.UnlinkItems(item, item);
                this.UpdateOwners(item, item, this, null);
            }

            protected void TransferFromChain(
              QueueItem.Chain oldOwner,
              QueueItem first,
              QueueItem last,
              QueueItem anchor,
              bool before)
            {
                this.UnlinkItems(first, last);
                this.UpdateOwners(first, last, oldOwner, this);
                this.LinkItems(first, last, anchor, before);
            }

            protected static bool IsOnlyChild(QueueItem item) => item._next == item;

            protected static QueueItem NextItem(QueueItem item) => item._next;

            protected static QueueItem PrevItem(QueueItem item) => item._prev;

            private void LinkItems(QueueItem first, QueueItem last, QueueItem anchor, bool before)
            {
                QueueItem queueItem1 = last;
                QueueItem queueItem2 = first;
                if (anchor != null)
                {
                    if (before)
                    {
                        queueItem1 = anchor._prev;
                        queueItem2 = anchor;
                    }
                    else
                    {
                        queueItem1 = anchor;
                        queueItem2 = anchor._next;
                    }
                    queueItem1._next = first;
                    queueItem2._prev = last;
                }
                first._prev = queueItem1;
                last._next = queueItem2;
            }

            private void UnlinkItems(QueueItem first, QueueItem last)
            {
                if (first._prev != last)
                {
                    first._prev._next = last._next;
                    last._next._prev = first._prev;
                }
                first._prev = null;
                last._next = null;
            }

            private void UpdateOwners(
              QueueItem first,
              QueueItem last,
              QueueItem.Chain oldOwner,
              QueueItem.Chain newOwner)
            {
                QueueItem queueItem = first;
                while (true)
                {
                    queueItem._owner = newOwner;
                    if (queueItem != last)
                        queueItem = queueItem._next;
                    else
                        break;
                }
            }

            [Conditional("DEBUG")]
            protected internal static void DEBUG_AssertLinked(QueueItem item)
            {
            }

            public struct ChainEnumerator
            {
                private QueueItem _currentItem;
                private QueueItem _stopItem;

                public ChainEnumerator(QueueItem tail)
                {
                    this._currentItem = null;
                    this._stopItem = tail;
                }

                public QueueItem Current => this._currentItem;

                public bool MoveNext()
                {
                    if (this._stopItem == null)
                    {
                        this._currentItem = null;
                        return false;
                    }
                    if (this._currentItem != null)
                    {
                        if (this._currentItem == this._stopItem)
                        {
                            this._currentItem = this._stopItem = null;
                            return false;
                        }
                        this._currentItem = this._currentItem._next;
                    }
                    else
                        this._currentItem = this._stopItem._next;
                    return true;
                }
            }
        }

        internal sealed class FIFO : QueueItem.Chain
        {
            private QueueItem _tail;

            public QueueItem Head
            {
                get
                {
                    QueueItem queueItem = null;
                    if (this._tail != null)
                        queueItem = this._tail._next;
                    return queueItem;
                }
            }

            public override void Dispose()
            {
                while (this._tail != null)
                    this.Remove(this._tail);
                base.Dispose();
            }

            public bool Append(QueueItem item)
            {
                ValidateAdd(item);
                bool flag = this._tail == null;
                this.Link(item, this._tail, false);
                this._tail = item;
                return flag;
            }

            public bool Append(QueueItem.FIFO items)
            {
                bool flag = this._tail == null;
                QueueItem tail = items._tail;
                if (tail == null)
                    return false;
                this.TransferFromChain(items, items.Head, tail, this._tail, false);
                items._tail = null;
                this._tail = tail;
                return flag;
            }

            public void Remove(QueueItem item)
            {
                this.ValidateRemove(item);
                if (item == this._tail)
                    this._tail = IsOnlyChild(this._tail) ? null : PrevItem(this._tail);
                this.Unlink(item);
            }

            public void Advance()
            {
                if (this._tail == null)
                    return;
                this._tail = NextItem(this._tail);
            }
        }

        internal sealed class Stack : QueueItem.Chain
        {
            private QueueItem _top;

            public override void Dispose()
            {
                while (this._top != null)
                    this.Pop();
                base.Dispose();
            }

            public void Push(QueueItem item)
            {
                ValidateAdd(item);
                this.Link(item, this._top, true);
                this._top = item;
            }

            public QueueItem Peek() => this._top;

            public QueueItem Pop()
            {
                QueueItem top = this._top;
                if (top != null)
                {
                    this._top = top._next;
                    if (this._top == top)
                        this._top = null;
                    this.Unlink(top);
                }
                return top;
            }
        }
    }
}
