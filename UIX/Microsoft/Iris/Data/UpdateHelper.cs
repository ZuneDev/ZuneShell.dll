// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Data.UpdateHelper
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Session;
using Microsoft.Iris.ViewItems;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Iris.Data
{
    internal class UpdateHelper
    {
        private const int c_millisecondsBetweenUpdates = 100;
        private const int c_millisecondsAllowedPerUpdate = 10;
        private UpdateHelper.ItemDistanceComparer _itemDistanceComparer;
        private IVirtualList _virtualList;
        private List<int> _itemsToUpdate;
        private int _lastInterestIndex;
        private DispatcherTimer _timer;
        private bool _listIsDirty;
        private List<int> _outstandingUpdates;
        private int _throttle;

        public UpdateHelper(IVirtualList virtualList)
        {
            this._virtualList = virtualList;
            this._itemsToUpdate = new List<int>();
            this._throttle = Environment.ProcessorCount * 2;
            this._outstandingUpdates = new List<int>();
        }

        public void AddIndex(int index)
        {
            this._itemsToUpdate.Add(index);
            this._listIsDirty = true;
            this.TriggerNextUpdate();
        }

        public void AdjustIndices(int lowThreshold, int amount) => this.AdjustIndices(lowThreshold, int.MaxValue, amount);

        public void AdjustIndices(int lowThreshold, int highThreshold, int amt)
        {
            for (int index = 0; index < this._itemsToUpdate.Count; ++index)
            {
                int num = this._itemsToUpdate[index];
                if (num >= lowThreshold && num <= highThreshold)
                    this._itemsToUpdate[index] = num + amt;
            }
            if (this._lastInterestIndex < lowThreshold || this._lastInterestIndex > highThreshold)
                return;
            this._lastInterestIndex += amt;
        }

        public void Clear() => this._itemsToUpdate.Clear();

        public void RemoveIndex(int index)
        {
            int count = this._itemsToUpdate.Count;
            int index1 = 0;
            while (index1 < count && this._itemsToUpdate[index1] != index)
                ++index1;
            if (index1 >= count)
                return;
            int index2 = index1;
            for (int index3 = index1 + 1; index3 < count; ++index3)
            {
                int num = this._itemsToUpdate[index3];
                if (num != index)
                {
                    this._itemsToUpdate[index2] = num;
                    ++index2;
                }
            }
            this._itemsToUpdate.RemoveRange(index2, count - index2);
        }

        public void Dispose()
        {
            if (this._timer == null)
                return;
            this._timer.Enabled = false;
        }

        private void TriggerNextUpdate()
        {
            this.EnsureTimer();
            this._timer.Enabled = true;
        }

        private void DeliverNextUpdate(object senderObject, EventArgs unusedArgs)
        {
            if (this._itemsToUpdate.Count == 0)
                return;
            if (this._outstandingUpdates.Count == this._throttle)
            {
                this.TriggerNextUpdate();
            }
            else
            {
                Repeater repeaterHost = this._virtualList.RepeaterHost;
                if (repeaterHost == null)
                    return;
                int index1 = -1;
                repeaterHost.GetFocusedIndex(ref index1);
                if (repeaterHost.GetExtendedLayoutOutput(VisibleIndexRangeLayoutOutput.DataCookie) is VisibleIndexRangeLayoutOutput extendedLayoutOutput)
                {
                    if (index1 != -1 && (index1 < extendedLayoutOutput.BeginVisible || index1 > extendedLayoutOutput.EndVisible))
                        index1 = -1;
                    if (index1 == -1)
                        index1 = extendedLayoutOutput.BeginVisible;
                }
                if (index1 != -1)
                {
                    int dataIndex;
                    ListUtility.GetWrappedIndex(index1, this._virtualList.Count, out dataIndex, out int _);
                    if (this._lastInterestIndex != dataIndex)
                        this._listIsDirty = true;
                    this._lastInterestIndex = dataIndex;
                }
                if (this._listIsDirty)
                {
                    if (this._itemDistanceComparer == null)
                        this._itemDistanceComparer = new UpdateHelper.ItemDistanceComparer();
                    this._itemDistanceComparer.Initialize(this._lastInterestIndex, this._virtualList.Count);
                    this._itemsToUpdate.Sort(_itemDistanceComparer);
                    this._listIsDirty = false;
                }
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                int count = 0;
                do
                {
                    ++count;
                    int index2 = this._itemsToUpdate[this._itemsToUpdate.Count - count];
                    this._outstandingUpdates.Add(index2);
                    this._virtualList.NotifyRequestSlowData(index2);
                }
                while (stopwatch.ElapsedMilliseconds < 10L && this._itemsToUpdate.Count != count && this._outstandingUpdates.Count < this._throttle);
                this._itemsToUpdate.RemoveRange(this._itemsToUpdate.Count - count, count);
                if (this._itemsToUpdate.Count == 0)
                    return;
                this.TriggerNextUpdate();
            }
        }

        public int Throttle
        {
            get => this._throttle;
            set => this._throttle = value;
        }

        public bool NotifySlowDataAcquireComplete(int index) => this._outstandingUpdates.Remove(index);

        private void EnsureTimer()
        {
            if (this._timer != null)
                return;
            this._timer = new DispatcherTimer();
            this._timer.AutoRepeat = false;
            this._timer.Interval = 100;
            this._timer.Tick += new EventHandler(this.DeliverNextUpdate);
        }

        private class ItemDistanceComparer : IComparer<int>
        {
            private int _focusedIndex;
            private int _totalCount;
            private int _midPoint;

            public void Initialize(int focusedIndex, int totalCount)
            {
                this._focusedIndex = focusedIndex;
                this._totalCount = totalCount;
                this._midPoint = totalCount / 2;
            }

            public int Compare(int left, int right) => this.GetDistance(right) - this.GetDistance(left);

            private int GetDistance(int potential)
            {
                int num = Math.Abs(potential - this._focusedIndex);
                if (num > this._midPoint)
                    num = this._totalCount - num + 1;
                return num;
            }
        }
    }
}
