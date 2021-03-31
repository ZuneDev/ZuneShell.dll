// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Queues.PriorityQueue
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System;

namespace Microsoft.Iris.Queues
{
    internal class PriorityQueue : Queue
    {
        public const int MAX_PRIORITIES = 32;
        private static readonly int[] s_lowestBitInNibble = new int[16]
        {
      4,
      0,
      1,
      0,
      2,
      0,
      1,
      0,
      3,
      0,
      1,
      0,
      2,
      0,
      1,
      0
        };
        private int _readyMask;
        private int _wakeMask;
        private int _hookMask;
        private int _lockMask;
        private int _allQueues;
        private Queue[] _queues;
        private PriorityQueue.HookProc _loopHook;
        private PriorityQueue.HookProc[] _drainHooks;

        public PriorityQueue(uint count) : this(new Queue[count])
        {
        }

        public PriorityQueue(Queue[] queues)
        {
            this._allQueues = this.InitChildQueues(queues);
            this._queues = queues;
            this._drainHooks = new PriorityQueue.HookProc[queues.Length];
            this._wakeMask = this._allQueues;
            this.UpdateReadyMask();
        }

        public override void Dispose()
        {
            Queue[] queues = this._queues;
            base.Dispose();
            if (queues == null)
                return;
            foreach (Queue queue in queues)
                queue?.Dispose();
        }

        private int InitChildQueues(Queue[] queues)
        {
            int num = 0;
            for (int priority = 0; priority < queues.Length; ++priority)
            {
                Queue queue = queues[priority];
                if (queue == null)
                {
                    queue = new SimpleQueue();
                    queues[priority] = queue;
                }
                PriorityQueue.WakeProxy wakeProxy = new PriorityQueue.WakeProxy(this, priority, queue);
                num |= 1 << priority;
            }
            return num;
        }

        public Queue this[int priority] => this._queues[priority];

        public PriorityQueue.HookProc GetDrainHook(int priority) => this._drainHooks[priority];

        public void SetDrainHook(int priority, PriorityQueue.HookProc hook)
        {
            this._drainHooks[priority] = hook;
            if (hook != null)
                this._hookMask |= 1 << priority;
            else
                this._hookMask &= ~(1 << priority);
            this.UpdateReadyMask();
        }

        public PriorityQueue.HookProc LoopHook
        {
            get => this._loopHook;
            set => this._loopHook = value;
        }

        public bool IsLocked(int priority) => (this._lockMask & 1 << priority) != 0;

        public void SetLock(int priority, bool value)
        {
            if (value)
                this._lockMask |= 1 << priority;
            else
                this._lockMask &= ~(1 << priority);
            this.UpdateReadyMask();
        }

        public void LockAll(bool value)
        {
            if (value)
                this._lockMask |= this._allQueues;
            else
                this._lockMask &= ~this._allQueues;
            this.UpdateReadyMask();
        }

        public Queue BuildSubsetQueue(int[] priorities, bool ignoreLocks)
        {
            int subsetMask = 0;
            for (int index = 0; index < priorities.Length; ++index)
                subsetMask |= 1 << priorities[index];
            return new PriorityQueue.SubsetQueue(this, subsetMask, ignoreLocks);
        }

        public override QueueItem GetNextItem() => this.GetNextItemWorker(this._allQueues, false);

        private QueueItem GetNextItemWorker(int subsetMask, bool ignoreLocks)
        {
            int mask = this.BeginReadLoop(subsetMask, ignoreLocks);
            QueueItem queueItem = null;
            while (mask != 0)
            {
                int lowestBit = FindLowestBit(mask);
                queueItem = this._queues[lowestBit].GetNextItem();
                if (queueItem == null)
                {
                    this.SetWake(lowestBit, false);
                    PriorityQueue.HookProc drainHook = this._drainHooks[lowestBit];
                    if (drainHook != null)
                    {
                        bool didWork;
                        bool abort;
                        drainHook(out didWork, out abort);
                        if (!abort)
                        {
                            if (didWork)
                            {
                                mask = this.BeginReadLoop(subsetMask, ignoreLocks);
                                continue;
                            }
                        }
                        else
                            break;
                    }
                    mask &= ~(1 << lowestBit);
                }
                else
                    break;
            }
            return queueItem;
        }

        private int BeginReadLoop(int subsetMask, bool ignoreLocks)
        {
            if (!ignoreLocks)
                subsetMask &= ~this._lockMask;
            subsetMask &= this._wakeMask | this._hookMask;
            if (subsetMask != 0 && this._loopHook != null)
            {
                bool didWork;
                bool abort;
                this._loopHook(out didWork, out abort);
                if (abort)
                    subsetMask = 0;
            }
            return subsetMask;
        }

        private void SetWake(int priority, bool value)
        {
            if (value)
                this._wakeMask |= 1 << priority;
            else
                this._wakeMask &= ~(1 << priority);
            this.UpdateReadyMask();
        }

        private void UpdateReadyMask()
        {
            bool flag = this._readyMask == 0;
            this._readyMask = (this._wakeMask | this._hookMask) & ~this._lockMask;
            if (!flag || this._readyMask == 0)
                return;
            this.OnWake();
        }

        private static int FindLowestBit(int mask)
        {
            int num = 0;
            if ((mask & ushort.MaxValue) == 0)
            {
                num += 16;
                mask >>= 16;
            }
            if ((mask & byte.MaxValue) == 0)
            {
                num += 8;
                mask >>= 8;
            }
            if ((mask & 15) == 0)
            {
                num += 4;
                mask >>= 4;
            }
            return num + s_lowestBitInNibble[mask & 15];
        }

        public delegate void HookProc(out bool didWork, out bool abort);

        private class SubsetQueue : Queue
        {
            private PriorityQueue _owner;
            private int _subsetMask;
            private bool _ignoreLocks;

            public SubsetQueue(PriorityQueue owner, int subsetMask, bool ignoreLocks)
            {
                this._owner = owner;
                this._subsetMask = subsetMask;
                this._ignoreLocks = ignoreLocks;
            }

            public override QueueItem GetNextItem() => this._owner.GetNextItemWorker(this._subsetMask, this._ignoreLocks);
        }

        private class WakeProxy
        {
            private PriorityQueue _owner;
            private int _priority;

            public WakeProxy(PriorityQueue owner, int priority, Queue queue)
            {
                this._owner = owner;
                this._priority = priority;
                queue.Wake += new EventHandler(this.OnChildWake);
            }

            private void OnChildWake(object sender, EventArgs args) => this._owner.SetWake(this._priority, true);
        }
    }
}
