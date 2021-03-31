// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.GroupedList
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Session;
using System;
using System.Collections;

namespace Microsoft.Iris
{
    public class GroupedList : VirtualList
    {
        private IList _source;
        private IComparer _comparer;
        private Vector<Group> _groups = new Vector<Group>();
        private bool _repairGroupsPending;
        private bool _adjustCountPending;

        public GroupedList()
          : base(true)
        {
        }

        public GroupedList(IList source, IComparer comparer, int count)
          : base(true)
        {
            this.Comparer = comparer;
            this.SetSource(source, count);
        }

        public IList Source
        {
            get => this._source;
            set => this.SetSource(value, 1, false);
        }

        public void SetSource(IList source, int groupCount) => this.SetSource(source, groupCount, false);

        private void SetSource(IList value, int groupCount, bool disposing)
        {
            if (this._source == value)
                return;
            if (this._source is INotifyList sourceNotifyA)
                sourceNotifyA.ContentsChanged -= new UIListContentsChangedHandler(this.SourceListModified);
            if (this._source is IVirtualList sourceVirtual && sourceVirtual.SlowDataRequestsEnabled)
                sourceVirtual.SlowDataAcquireCompleteHandler = (SlowDataAcquireCompleteHandler)null;
            this._source = value;
            if (this._source is INotifyList sourceNotifyB)
                sourceNotifyB.ContentsChanged += new UIListContentsChangedHandler(this.SourceListModified);
            if (this._source is IVirtualList sourceVirtualB && sourceVirtualB.SlowDataRequestsEnabled)
                sourceVirtualB.SlowDataAcquireCompleteHandler = new SlowDataAcquireCompleteHandler(this.SourceSlowDataAcquired);
            if (disposing)
                return;
            this.FirePropertyChanged("Source");
            this.Regroup(groupCount);
        }

        public IComparer Comparer
        {
            get => this._comparer;
            set
            {
                if (this._comparer == value)
                    return;
                this._comparer = value;
                this.Regroup();
                this.FirePropertyChanged(nameof(Comparer));
            }
        }

        public void Regroup() => this.Regroup(1);

        public void Regroup(int groupCount)
        {
            foreach (ModelItem group in this._groups)
                group.Dispose();
            this._groups.Clear();
            this.Clear();
            this.Count = this.Source == null || this.Source.Count <= 0 ? 0 : Math.Max(1, groupCount);
        }

        protected override object OnRequestItem(int index)
        {
            if (this._repairGroupsPending && index >= this._groups.Count)
                return (object)null;
            this.EnsureGroup(index);
            return index >= this._groups.Count ? (object)null : (object)this._groups[index];
        }

        private void ScheduleAdjustCount()
        {
            if (this._adjustCountPending || this.GetCountAdjustment() == 0)
                return;
            DeferredCall.Post(DispatchPriority.Housekeeping, new DeferredHandler(this.AdjustCount));
        }

        private int GetCountAdjustment()
        {
            int num1 = 0;
            int num2 = this.Source != null ? this.Source.Count : 0;
            Group lastGroup = this.GetLastGroup();
            int num3 = lastGroup != null ? lastGroup.EndIndex + 1 : 0;
            int num4 = num2 - num3;
            int num5 = this.Count - this._groups.Count;
            if (this._groups.Count == this.Count && num4 > 0)
            {
                int num6 = num3 / this._groups.Count;
                num1 = Math.Max(1, num4 / num6);
            }
            else if (lastGroup == null || num5 > num4)
                num1 = num4 - num5;
            return num1;
        }

        private void AdjustCount(object args)
        {
            this._adjustCountPending = false;
            int countAdjustment = this.GetCountAdjustment();
            if (countAdjustment > 0)
            {
                this.AddRange(countAdjustment);
            }
            else
            {
                if (countAdjustment >= 0)
                    return;
                int num = -countAdjustment;
                for (int index = 0; index < num; ++index)
                    this.RemoveAt(this.Count - 1);
            }
        }

        private bool SourceSlowDataAcquired(IVirtualList list, int sourceIndex)
        {
            Group groupForSourceIndex = this.GetGroupForSourceIndex(sourceIndex);
            groupForSourceIndex?.NotifySlowDataAcquireComplete(sourceIndex - groupForSourceIndex.StartIndex);
            return false;
        }

        private void EnsureGroup(int maxGroup)
        {
            if (this.Comparer != null && this.Source != null && this.Source.Count > 0)
            {
                int previousGroupIndex = this._groups.Count - 1;
                Group lastGroup = this.GetLastGroup();
                this.GroupItems(ref lastGroup, ref previousGroupIndex, this.Source.Count - 1, maxGroup, true, false);
            }
            this.ScheduleAdjustCount();
        }

        private void GroupItems(
          ref Group previousGroup,
          ref int previousGroupIndex,
          int endIndex,
          int maxGroup,
          bool createGroups,
          bool notify)
        {
            if (this._groups.Count > maxGroup)
                return;
            if (previousGroup == null)
            {
                ++previousGroupIndex;
                previousGroup = this.InsertGroup(previousGroupIndex, 0, notify);
            }
            for (int index = previousGroup.EndIndex + 1; index <= endIndex; ++index)
            {
                if (!this.TryInsertItemIntoGroup(previousGroup, index))
                {
                    if (!createGroups || this._groups.Count > maxGroup)
                        break;
                    ++previousGroupIndex;
                    previousGroup = this.InsertGroup(previousGroupIndex, index, notify);
                }
            }
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
                this.SetSource((IList)null, 0, true);
            base.OnDispose(disposing);
        }

        private void SourceListModified(IList listSender, UIListContentsChangedArgs args)
        {
            if (this.Comparer == null)
                return;
            bool flag = false;
            switch (args.Type)
            {
                case UIListContentsChangeType.Add:
                case UIListContentsChangeType.AddRange:
                case UIListContentsChangeType.Insert:
                case UIListContentsChangeType.InsertRange:
                    if (this.Count != 0)
                    {
                        Group lastGroup = this.GetLastGroup();
                        if (lastGroup != null && args.NewIndex <= lastGroup.EndIndex + 1)
                        {
                            flag = true;
                            int newIndex = args.NewIndex;
                            int num = args.NewIndex + args.Count - 1;
                            int groupIndex;
                            Group groupForSourceIndex = this.GetGroupForSourceIndex(newIndex - 1, out groupIndex);
                            for (int index = groupIndex + 1; index < this._groups.Count; ++index)
                                this._groups[index].StartIndex += args.Count;
                            if (groupForSourceIndex != null && groupForSourceIndex.ContainsSourceIndex(newIndex))
                            {
                                this.SplitGroup(groupForSourceIndex, groupIndex, newIndex - 1, num + 1);
                                break;
                            }
                            break;
                        }
                        break;
                    }
                    this.Count = 1;
                    break;
                case UIListContentsChangeType.Remove:
                    int groupIndex1;
                    Group groupForSourceIndex1 = this.GetGroupForSourceIndex(args.OldIndex, out groupIndex1);
                    for (int index = groupIndex1 + 1; index < this._groups.Count; ++index)
                        --this._groups[index].StartIndex;
                    if (groupForSourceIndex1 != null)
                    {
                        if (groupForSourceIndex1.Count == 1)
                        {
                            this.RemoveGroup(groupIndex1);
                            flag = true;
                            break;
                        }
                        groupForSourceIndex1.RemoveAt(args.OldIndex - groupForSourceIndex1.StartIndex);
                        break;
                    }
                    break;
                case UIListContentsChangeType.Move:
                case UIListContentsChangeType.Modified:
                    throw new NotImplementedException();
                default:
                    this.Regroup();
                    break;
            }
            if (!flag || this._repairGroupsPending)
                return;
            this._repairGroupsPending = true;
            DeferredCall.Post(DispatchPriority.Housekeeping, new DeferredHandler(this.RepairGroups));
        }

        private void RepairGroups(object args)
        {
            this._repairGroupsPending = false;
            for (int previousGroupIndex = -1; previousGroupIndex < this._groups.Count; ++previousGroupIndex)
            {
                Group previousGroup = previousGroupIndex > -1 ? this._groups[previousGroupIndex] : (Group)null;
                Group group = previousGroupIndex + 1 < this._groups.Count ? this._groups[previousGroupIndex + 1] : (Group)null;
                int num1 = previousGroup != null ? previousGroup.EndIndex + 1 : 0;
                int num2 = group != null ? group.StartIndex - 1 : this.Source.Count - 1;
                if (group != null)
                {
                    while (num2 >= num1 && this.TryInsertItemIntoGroup(group, num2))
                        --num2;
                }
                if (num1 <= num2)
                    this.GroupItems(ref previousGroup, ref previousGroupIndex, num2, int.MaxValue, group != null, true);
                if (this.TryMergeWithNext(previousGroupIndex))
                    --previousGroupIndex;
            }
            this.AdjustCount((object)null);
        }

        private bool IsEqualToNext(int sourceIndex) => this.Comparer.Compare(this.Source[sourceIndex], this.Source[sourceIndex + 1]) == 0;

        private bool TryMergeWithNext(int i)
        {
            if (i >= 0 && i < this._groups.Count - 1)
            {
                Group group1 = this._groups[i];
                if (this.IsEqualToNext(group1.EndIndex))
                {
                    Group group2 = this._groups[i + 1];
                    group1.AddRange(group2.Count);
                    this.RemoveGroup(i + 1);
                    return true;
                }
            }
            return false;
        }

        private void RemoveGroup(int groupIndex)
        {
            this._groups[groupIndex].Dispose();
            this._groups.RemoveAt(groupIndex);
            this.RemoveAt(groupIndex);
        }

        private bool TryInsertItemIntoGroup(Group group, int index)
        {
            int sourceIndex = index - 1;
            if (index < group.StartIndex)
                sourceIndex = index;
            if (!this.IsEqualToNext(sourceIndex))
                return false;
            group.StartIndex = Math.Min(group.StartIndex, index);
            group.Insert(index - group.StartIndex);
            return true;
        }

        private Group SplitGroup(
          Group group,
          int groupIndex,
          int firstGroupSourceEndIndex,
          int secondGroupSourceStartIndex)
        {
            int count = group.EndIndex - firstGroupSourceEndIndex;
            for (int index = 0; index < count; ++index)
                group.RemoveAt(group.Count - 1);
            return this.InsertGroup(groupIndex + 1, secondGroupSourceStartIndex, count, true);
        }

        private Group InsertGroup(int groupInsertIndex, int sourceIndex, bool notify) => this.InsertGroup(groupInsertIndex, sourceIndex, 1, notify);

        private Group InsertGroup(
          int groupInsertIndex,
          int startSourceIndex,
          int count,
          bool notify)
        {
            Group group = new Group(this, startSourceIndex, count);
            group.Count = count;
            this._groups.Insert(groupInsertIndex, group);
            if (notify)
                this.Insert(groupInsertIndex);
            return group;
        }

        private Group GetLastGroup() => this._groups.Count <= 0 ? (Group)null : this._groups[this._groups.Count - 1];

        private Group GetGroupForSourceIndex(int sourceIndex, out int groupIndex)
        {
            int num1 = 0;
            int num2 = this._groups.Count - 1;
            groupIndex = num1 + (num2 - num1) / 2;
            while (num1 <= num2)
            {
                Group group = this._groups[groupIndex];
                if (group.StartIndex <= sourceIndex && sourceIndex <= group.EndIndex)
                    return group;
                if (group.StartIndex < sourceIndex)
                    num1 = groupIndex + 1;
                else
                    num2 = groupIndex - 1;
                groupIndex = num1 + (num2 - num1) / 2;
            }
            --groupIndex;
            return (Group)null;
        }

        public Group GetGroupForSourceIndex(int sourceIndex) => this.GetGroupForSourceIndex(sourceIndex, out int _);

        public int GetGroupIndexForSourceIndex(int sourceIndex)
        {
            for (int index = 0; index < this.Count; ++index)
            {
                Group group = (Group)this[index];
                if (group != null)
                {
                    if (sourceIndex < group.Count)
                        return index;
                    sourceIndex -= group.Count;
                }
                else
                    break;
            }
            return -1;
        }
    }
}
