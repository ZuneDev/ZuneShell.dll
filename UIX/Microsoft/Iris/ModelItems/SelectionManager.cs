// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ModelItems.SelectionManager
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.Session;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Iris.ModelItems
{
    internal class SelectionManager : DisposableNotifyObjectBase
    {
        private Vector<Range> _selected;
        private int? _anchor;
        private IList _sourceList;
        private int _count;
        private IList _selectedIndicesCache;
        private IList _selectedItemsCache;
        private bool _singleSelect;
        public static readonly object[] s_emptyList = new object[0];

        public SelectionManager() => this._selected = new Vector<Range>();

        protected override void OnDispose()
        {
            base.OnDispose();
            this.UnhookContentsChangedHandler();
        }

        public bool IsSelected(int index)
        {
            foreach (Range range in this._selected)
            {
                if (range.Contains(index))
                    return true;
            }
            return false;
        }

        public bool IsRangeSelected(int begin, int end) => this.IsRangeSelected(new Range(begin, end));

        private bool IsRangeSelected(Range range)
        {
            for (int begin = range.Begin; begin <= range.End; ++begin)
            {
                if (!this.IsSelected(begin))
                    return false;
            }
            return true;
        }

        public IList SelectedIndices
        {
            get
            {
                if (this._selectedIndicesCache == null)
                {
                    List<int> list = new List<int>();
                    foreach (Range range in this._selected)
                    {
                        foreach (int num in range.ToList())
                        {
                            if (!IntListUtility.Contains(list, num))
                                list.Add(num);
                        }
                    }
                    list.Sort();
                    this._selectedIndicesCache = new SelectionManager.ReadOnlyList(list, nameof(SelectedIndices));
                }
                return this._selectedIndicesCache;
            }
        }

        public IList SelectedItems
        {
            get
            {
                if (this._selectedItemsCache == null)
                {
                    IList originalList = null;
                    if (this.Count > 0 && this.SourceList != null)
                    {
                        originalList = new List<object>();
                        foreach (int original in (List<int>)((SelectionManager.ReadOnlyList)this.SelectedIndices).OriginalList)
                        {
                            if (this.IsValidIndex(original))
                                originalList.Add(this.SourceList[original]);
                        }
                    }
                    if (originalList == null)
                        originalList = s_emptyList;
                    this._selectedItemsCache = new SelectionManager.ReadOnlyList(originalList, nameof(SelectedItems));
                }
                return this._selectedItemsCache;
            }
        }

        public int SelectedIndex
        {
            get => this.Count > 0 ? (int)this.SelectedIndices[0] : -1;
            set
            {
                if (!this.IsValidIndex(value) || this.Count == 1 && this.IsSelected(value))
                    return;
                this.Clear();
                if (value == -1)
                    return;
                this.Select(value, true);
            }
        }

        public object SelectedItem => this.Count > 0 ? this.SelectedItems[0] : null;

        public int Count => this._count;

        public IList SourceList
        {
            get => this._sourceList;
            set
            {
                if (this._sourceList == value)
                    return;
                this.Clear(true);
                this.UnhookContentsChangedHandler();
                this._sourceList = value;
                if (value != null && value is INotifyList notifyList)
                    notifyList.ContentsChanged += new UIListContentsChangedHandler(this.OnListContentsChanged);
                this.FireNotification(NotificationID.SourceList);
            }
        }

        private void UnhookContentsChangedHandler()
        {
            if (this._sourceList == null || !(this._sourceList is INotifyList sourceList))
                return;
            sourceList.ContentsChanged -= new UIListContentsChangedHandler(this.OnListContentsChanged);
        }

        public int Anchor
        {
            get => !this._anchor.HasValue ? 0 : this._anchor.Value;
            set => this.SetAnchor(new int?(value));
        }

        private void SetAnchor(int? value)
        {
            if (this._anchor.HasValue == value.HasValue && (!this._anchor.HasValue || this._anchor.Value == value.Value))
                return;
            this._anchor = value;
            this.FireNotification(NotificationID.Anchor);
        }

        public bool SingleSelect
        {
            get => this._singleSelect;
            set
            {
                if (this._singleSelect == value)
                    return;
                if (value && this.Count > 1)
                    this.SelectedIndex = (int)this.SelectedIndices[0];
                this._singleSelect = value;
                this.FireNotification(NotificationID.SingleSelect);
            }
        }

        private bool IsValidIndex(int index)
        {
            if (this.SourceList == null)
                return true;
            return index >= 0 && index < this.SourceList.Count;
        }

        private void ValidateMultiSelect(string operation)
        {
            if (!this.SingleSelect)
                return;
            ErrorManager.ReportError("Calling {0} is not supported on a SelectionManager in single selection modes.", operation);
        }

        private void OnListContentsChanged(IList senderList, UIListContentsChangedArgs args)
        {
            UIListContentsChangeType type = args.Type;
            int oldIndex = args.OldIndex;
            int newIndex = args.NewIndex;
            int count = args.Count;
            bool flag1 = false;
            bool countChanged = false;
            switch (type)
            {
                case UIListContentsChangeType.Add:
                case UIListContentsChangeType.AddRange:
                case UIListContentsChangeType.Insert:
                case UIListContentsChangeType.InsertRange:
                    Vector<Range> vector = new Vector<Range>();
                    for (int index = 0; index < this._selected.Count; ++index)
                    {
                        Range range = this._selected[index];
                        if (range.End >= newIndex)
                        {
                            int num = range.Begin;
                            if (range.Contains(newIndex - 1))
                            {
                                Range before;
                                range.Split(newIndex - 1, out before, out Range _);
                                vector.Add(before);
                                num = newIndex;
                            }
                            this._selected[index] = new Range(num + count, range.End + count);
                            flag1 = true;
                        }
                    }
                    if (vector.Count > 0)
                    {
                        foreach (Range range in vector)
                            this._selected.Add(range);
                    }
                    if (this.Anchor >= newIndex)
                    {
                        this.Anchor += count;
                        break;
                    }
                    break;
                case UIListContentsChangeType.Remove:
                    if (this.SourceList.Count > 0)
                    {
                        if (this.IsSelected(oldIndex))
                        {
                            this.RemoveRange(new Range(oldIndex, oldIndex), false);
                            flag1 = true;
                            countChanged = true;
                        }
                        for (int index = 0; index < this._selected.Count; ++index)
                        {
                            Range range = this._selected[index];
                            if (range.End > oldIndex)
                            {
                                this._selected[index] = new Range(range.Begin - count, range.End - count);
                                flag1 = true;
                            }
                        }
                        if (this.Anchor >= oldIndex)
                        {
                            this.Anchor -= count;
                            break;
                        }
                        break;
                    }
                    this.Clear(true);
                    break;
                case UIListContentsChangeType.Move:
                    if (oldIndex != newIndex)
                    {
                        bool flag2 = this.IsSelected(oldIndex);
                        if (flag2)
                        {
                            this.Select(oldIndex, false, false, false);
                            flag1 = true;
                        }
                        bool flag3 = this.IsSelected(newIndex);
                        if (flag3)
                        {
                            this.Select(newIndex, false, false, false);
                            flag1 = true;
                        }
                        int num = oldIndex < newIndex ? -1 : 1;
                        Range other = new Range(oldIndex, newIndex);
                        for (int index = 0; index < this._selected.Count; ++index)
                        {
                            Range range = this._selected[index];
                            if (range.Intersects(other))
                            {
                                this._selected[index] = new Range(range.Begin + num, range.End + num);
                                flag1 = true;
                            }
                        }
                        if (flag2)
                            this.Select(newIndex, true, false, false);
                        if (flag3)
                            this.Select(newIndex + num, true, false, false);
                        if (this.Anchor == oldIndex)
                        {
                            this.Anchor = newIndex;
                            break;
                        }
                        if (other.Contains(this.Anchor))
                        {
                            this.Anchor += num;
                            break;
                        }
                        break;
                    }
                    break;
                case UIListContentsChangeType.Clear:
                case UIListContentsChangeType.Reset:
                    this.Clear(true);
                    break;
            }
            if (!flag1)
                return;
            this.OnSelectionChanged(countChanged);
        }

        public void Clear() => this.Clear(false);

        private void Clear(bool resetAnchor)
        {
            if (this._selected.Count > 0)
            {
                this._selected.Clear();
                this._count = 0;
                this.OnSelectedIndicesChanged();
            }
            if (!resetAnchor)
                return;
            this.SetAnchor(new int?());
        }

        public bool Select(int index, bool select) => this.Select(index, select, true, true);

        private bool Select(int index, bool select, bool rememberAnchor, bool fireSelectionChanged)
        {
            if (!this.IsValidIndex(index))
                return false;
            if (this.IsSelected(index) == select)
                return true;
            Range range = new Range(index, index);
            if (select)
            {
                if (this.SingleSelect)
                    this.Clear();
                this.AddRange(range, fireSelectionChanged);
            }
            else
                this.RemoveRange(range, fireSelectionChanged);
            if (rememberAnchor)
                this.Anchor = index;
            return true;
        }

        public bool Select(IList indices, bool select)
        {
            this.ValidateMultiSelect("Select(IList, bool)");
            bool flag = true;
            foreach (object index in indices)
                flag &= this.Select((int)index, select, false, true);
            return flag;
        }

        public bool ToggleSelect(int item) => this.Select(item, !this.IsSelected(item));

        public bool ToggleSelect(IList items)
        {
            this.ValidateMultiSelect("ToggleSelect(IList)");
            bool flag = true;
            foreach (int index in items)
                flag &= this.Select(index, !this.IsSelected(index), false, true);
            return flag;
        }

        public bool SelectRange(int begin, int end) => this.SelectRange(new Range(begin, end), begin);

        private bool SelectRange(Range range, int anchor)
        {
            this.ValidateMultiSelect(nameof(SelectRange));
            if (!this.IsValidIndex(range.Begin) || !this.IsValidIndex(range.End))
                return false;
            this.AddRange(range, true);
            this.Anchor = anchor;
            return true;
        }

        public bool SelectRangeFromAnchor(int end)
        {
            this.ValidateMultiSelect(nameof(SelectRangeFromAnchor));
            int num = this.Anchor;
            if (this.SourceList != null)
                num = Math.Max(0, Math.Min(num, this.SourceList.Count - 1));
            return this.SelectRange(new Range(num, end), num);
        }

        public bool SelectRangeFromAnchor(int rangeStart, int rangeEnd)
        {
            this.ValidateMultiSelect(nameof(SelectRangeFromAnchor));
            Range range = new Range(rangeStart, rangeEnd);
            if (range.Begin > this.Anchor)
                return this.SelectRangeFromAnchor(range.End);
            return this.Anchor > range.End ? this.SelectRangeFromAnchor(range.Begin) : this.SelectRange(range, rangeStart);
        }

        public bool ToggleSelectRange(int begin, int end)
        {
            this.ValidateMultiSelect(nameof(ToggleSelectRange));
            if (!this.IsValidIndex(begin) || !this.IsValidIndex(end))
                return false;
            Range range = new Range(begin, end);
            if (this.IsRangeSelected(range))
                this.RemoveRange(range, true);
            else
                this.AddRange(range, true);
            this.Anchor = range.Begin;
            return true;
        }

        private void AddRange(Range addRange, bool fireSelectionChanged)
        {
            int count = this.Count;
            for (int begin = addRange.Begin; begin <= addRange.End; ++begin)
            {
                if (!this.IsSelected(begin))
                    ++this._count;
            }
            if (count == this._count)
                return;
            this._selected.Add(addRange);
            if (!fireSelectionChanged)
                return;
            this.OnSelectedIndicesChanged();
        }

        private void RemoveRange(Range removeRange, bool fireSelectionChanged)
        {
            int count = this.Count;
            for (int begin = removeRange.Begin; begin <= removeRange.End; ++begin)
            {
                if (this.IsSelected(begin))
                    --this._count;
            }
            if (count == this._count)
                return;
            Vector<Range> vector1 = new Vector<Range>();
            Vector<Range> vector2 = new Vector<Range>();
            foreach (Range other in this._selected)
            {
                if (removeRange.Intersects(other))
                {
                    Range range;
                    if (other.Contains(removeRange.Begin - 1))
                    {
                        Range before;
                        other.Split(removeRange.Begin - 1, out before, out range);
                        vector2.Add(before);
                    }
                    Range after;
                    if (other.Contains(removeRange.End) && other.Split(removeRange.End, out range, out after))
                        vector2.Add(after);
                    vector1.Add(other);
                }
            }
            if (vector1.Count <= 0 && vector2.Count <= 0)
                return;
            foreach (Range range in vector1)
                this._selected.Remove(range);
            foreach (Range range in vector2)
                this._selected.Add(range);
            if (!fireSelectionChanged)
                return;
            this.OnSelectedIndicesChanged();
        }

        private void OnSelectedIndicesChanged() => this.OnSelectionChanged(true);

        private void OnSelectionChanged(bool countChanged)
        {
            this._selectedIndicesCache = null;
            this.FireNotification(NotificationID.SelectedIndices);
            this.FireNotification(NotificationID.SelectedIndex);
            if (!countChanged)
                return;
            this.FireNotification(NotificationID.Count);
            if (this.SourceList == null)
                return;
            this._selectedItemsCache = null;
            this.FireNotification(NotificationID.SelectedItems);
            this.FireNotification(NotificationID.SelectedItem);
        }

        internal class ReadOnlyList : IList, ICollection, IEnumerable
        {
            private IList _originalList;
            private string _listName;

            public ReadOnlyList(IList originalList, string listName)
            {
                this._originalList = originalList;
                this._listName = listName;
            }

            public IList OriginalList => this._originalList;

            public object this[int index]
            {
                get => this._originalList[index];
                set => ErrorManager.ReportError("Cannot modify selection through the list returned by {0}.  Use the methods on SelectionManager instead.", _listName);
            }

            public int Count => this._originalList.Count;

            public bool Contains(object value) => this._originalList.Contains(value);

            public int IndexOf(object value) => this._originalList.IndexOf(value);

            public bool IsFixedSize => true;

            public bool IsReadOnly => true;

            public bool IsSynchronized => false;

            public object SyncRoot => _originalList;

            public IEnumerator GetEnumerator() => this._originalList.GetEnumerator();

            public void CopyTo(Array array, int index) => this._originalList.CopyTo(array, index);

            public int Add(object value)
            {
                ErrorManager.ReportError("Cannot modify selection through the list returned by {0}.  Use the methods on SelectionManager instead.", _listName);
                return -1;
            }

            public void Clear() => ErrorManager.ReportError("Cannot modify selection through the list returned by {0}.  Use the methods on SelectionManager instead.", _listName);

            public void Insert(int index, object value) => ErrorManager.ReportError("Cannot modify selection through the list returned by {0}.  Use the methods on SelectionManager instead.", _listName);

            public void Remove(object value) => ErrorManager.ReportError("Cannot modify selection through the list returned by {0}.  Use the methods on SelectionManager instead.", _listName);

            public void RemoveAt(int index) => ErrorManager.ReportError("Cannot modify selection through the list returned by {0}.  Use the methods on SelectionManager instead.", _listName);
        }
    }
}
