// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Group
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;

namespace Microsoft.Iris
{
    public class Group : VirtualList
    {
        private GroupedList _groupedList;
        private int _startIndex;

        internal Group(GroupedList groupedList, int startIndex, int count)
          : base(groupedList, true, null)
        {
            this._groupedList = groupedList;
            this._startIndex = startIndex;
            this.Count = count;
        }

        public int StartIndex
        {
            get => this._startIndex;
            internal set
            {
                if (this._startIndex == value)
                    return;
                this._startIndex = value;
                this.FirePropertyChanged(nameof(StartIndex));
                this.FirePropertyChanged("EndIndex");
            }
        }

        public int EndIndex => this.StartIndex + this.Count - 1;

        internal override void OnCountChanged() => this.FirePropertyChanged("EndIndex");

        private int GetSourceIndex(int groupedIndex) => this.StartIndex + groupedIndex;

        internal bool ContainsSourceIndex(int sourceIndex) => this.StartIndex <= sourceIndex && sourceIndex <= this.EndIndex;

        protected override object OnRequestItem(int index) => this._groupedList.Source[this.GetSourceIndex(index)];

        protected override void OnRequestSlowData(int groupedIndex)
        {
            if (!(this._groupedList.Source is IVirtualList source) || !source.SlowDataRequestsEnabled)
                return;
            int sourceIndex = this.GetSourceIndex(groupedIndex);
            if (sourceIndex >= source.Count)
                return;
            source.NotifyRequestSlowData(sourceIndex);
        }

        public override string ToString() => "Group [" + StartIndex + "-" + EndIndex + "]";
    }
}
