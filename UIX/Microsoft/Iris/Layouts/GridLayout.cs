// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Layouts.GridLayout
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Layout;
using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.ViewItems;
using System;

namespace Microsoft.Iris.Layouts
{
    internal class GridLayout : ILayout
    {
        private Orientation _orientation;
        private bool _allowWrap;
        private Size _referenceSize;
        private Size _spacing;
        private int _rows;
        private int _columns;
        private RepeatPolicy _repeat;
        private int _repeatGap;
        private ItemAlignment _defaultChildAlignment;

        public GridLayout()
          : this(Orientation.Horizontal)
        {
        }

        public GridLayout(Orientation o) => this._orientation = o;

        public Orientation Orientation
        {
            get => this._orientation;
            set
            {
                if (this._orientation == value)
                    return;
                this._orientation = value;
            }
        }

        public bool AllowWrap
        {
            get => this._allowWrap;
            set
            {
                if (this._allowWrap == value)
                    return;
                this._allowWrap = value;
            }
        }

        public Size ReferenceSize
        {
            get => this._referenceSize;
            set
            {
                if (!(this._referenceSize != value))
                    return;
                this._referenceSize = value;
            }
        }

        public int Rows
        {
            get => this._rows;
            set
            {
                if (this._rows == value)
                    return;
                this._rows = value;
            }
        }

        public int Columns
        {
            get => this._columns;
            set
            {
                if (this._columns == value)
                    return;
                this._columns = value;
            }
        }

        public Size Spacing
        {
            get => this._spacing;
            set
            {
                if (!(this._spacing != value))
                    return;
                this._spacing = value;
            }
        }

        public RepeatPolicy Repeat
        {
            get => this._repeat;
            set
            {
                if (this._repeat == value)
                    return;
                this._repeat = value;
            }
        }

        public int RepeatGap
        {
            get => this._repeatGap;
            set
            {
                if (this._repeatGap == value)
                    return;
                this._repeatGap = value;
            }
        }

        public ItemAlignment DefaultChildAlignment
        {
            get => this._defaultChildAlignment;
            set => this._defaultChildAlignment = value;
        }

        Size ILayout.Measure(ILayoutNode layoutNode, Size constraint)
        {
            GridLayout.GeneralFlowInfo generalFlowInfo = this.CalculateGeneralFlowInfo(layoutNode, constraint);
            layoutNode.MeasureData = generalFlowInfo;
            if (generalFlowInfo.itemsCount <= 0 || !generalFlowInfo.referenceExtent.IsEmpty)
                return generalFlowInfo.usedSize.ToSize(this.Orientation);
            layoutNode.RequestMoreChildren(1);
            return Size.Zero;
        }

        void ILayout.Arrange(ILayoutNode layoutNode, LayoutSlot slot)
        {
            GridLayout.GeneralFlowInfo measureData = (GridLayout.GeneralFlowInfo)layoutNode.MeasureData;
            measureData.slot = slot;
            MajorMinor majorMinor1 = new MajorMinor(slot.View.Size, this.Orientation);
            MajorMinor majorMinor2 = new MajorMinor(slot.PeripheralView.Size, this.Orientation);
            MajorMinor majorMinor3 = new MajorMinor(slot.View.Location.ToSize(), this.Orientation);
            bool flag1 = measureData.availableSpace.Major != majorMinor2.Major || majorMinor3.Major != 0;
            bool flag2 = measureData.availableSpace.Minor != majorMinor2.Minor || majorMinor3.Minor != 0;
            MajorMinor majorMinor4 = majorMinor2 + measureData.spacing;
            MajorMinor majorMinor5 = majorMinor1 + measureData.spacing;
            bool roundUp1 = flag1;
            bool roundUp2 = flag2;
            MajorMinor a = MajorMinor.Zero;
            a.Major = DivideIntegers(majorMinor5.Major, measureData.referenceWithSpacing.Major, roundUp1);
            a.Minor = DivideIntegers(majorMinor5.Minor, measureData.referenceWithSpacing.Minor, roundUp2);
            a.Minor = Math.Min(a.Minor, measureData.fitItems.Minor);
            bool flag3 = !measureData.wrapping ? measureData.totalItems.Major <= a.Major : measureData.totalItems.Minor <= a.Minor;
            bool flag4;
            switch (this._repeat)
            {
                case RepeatPolicy.WhenTooBig:
                    flag4 = !flag3;
                    break;
                case RepeatPolicy.WhenTooSmall:
                    flag4 = flag3;
                    break;
                case RepeatPolicy.Always:
                    flag4 = true;
                    break;
                default:
                    flag4 = false;
                    break;
            }
            if (!flag4)
                a = MajorMinor.Min(a, measureData.totalItems);
            else
                measureData.wrapping |= this.AllowWrap;
            measureData.repeating = flag4;
            measureData.allowPartialItemsMajor = roundUp1;
            measureData.allowPartialItemsMinor = roundUp2;
            measureData.viewSize = majorMinor2;
            if (measureData.itemsCount > 0)
            {
                GridLayout.IndexRangeInfo indicesInfo;
                Vector<int> indicesToDisplayList;
                this.ArrangeChildren(layoutNode, measureData, out indicesInfo, out indicesToDisplayList);
                if (indicesToDisplayList != null)
                    layoutNode.RequestSpecificChildren(indicesToDisplayList);
                layoutNode.SetVisibleIndexRange(indicesInfo.realBeginIndex, indicesInfo.realEndIndex, indicesInfo.beginIndex, indicesInfo.endIndex, measureData.focusedItemIndex);
            }
            MajorMinor usedSize = measureData.usedSize;
            if (!measureData.repeating)
                return;
            MajorMinor zero = MajorMinor.Zero;
            if (measureData.wrapping)
                zero.Minor = -usedSize.Minor / 2;
            else
                zero.Major = -usedSize.Major / 2;
            Rectangle rectangle = new Rectangle(zero.ToPoint(this.Orientation), usedSize.ToSize(this.Orientation));
            layoutNode.AddAreaOfInterest(new AreaOfInterest(rectangle, AreaOfInterestID.ScrollableRange));
        }

        private int GetItemCount(ILayoutNode layoutNode)
        {
            int num = -1;
            if (layoutNode.GetLayoutInput(CountLayoutInput.Data) is CountLayoutInput layoutInput)
                num = layoutInput.Count;
            if (num == -1)
                num = layoutNode.LayoutChildrenCount;
            return num;
        }

        private Size GetReferenceSize(ILayoutNode layoutNode, Size constraint)
        {
            Size referenceSize = this.ReferenceSize;
            if (referenceSize.Width == 0.0 && this.Columns != 0)
            {
                int num = (constraint.Width - this.Spacing.Width * (this.Columns - 1)) / this.Columns;
                if (num <= 0)
                    return Size.Zero;
                referenceSize.Width = num;
                constraint.Width = num;
            }
            if (referenceSize.Height == 0.0 && this.Rows != 0)
            {
                int num = (constraint.Height - this.Spacing.Height * (this.Rows - 1)) / this.Rows;
                if (num <= 0)
                    return Size.Zero;
                referenceSize.Height = num;
                constraint.Height = num;
            }
            if (referenceSize.IsEmpty && layoutNode.LayoutChildrenCount > 0)
            {
                foreach (ILayoutNode layoutChild in layoutNode.LayoutChildren)
                {
                    layoutChild.Measure(constraint);
                    if (referenceSize.Width == 0)
                        referenceSize.Width = layoutChild.DesiredSize.Width;
                    if (referenceSize.Height == 0)
                        referenceSize.Height = layoutChild.DesiredSize.Height;
                    if (!referenceSize.IsEmpty)
                        break;
                }
            }
            return referenceSize;
        }

        private GridLayout.GeneralFlowInfo CalculateGeneralFlowInfo(
          ILayoutNode layoutNode,
          Size constraint)
        {
            int itemCount = this.GetItemCount(layoutNode);
            Size size1 = this.GetReferenceSize(layoutNode, constraint);
            GridLayout.GeneralFlowInfo generalFlowInfo = new GridLayout.GeneralFlowInfo(itemCount, size1, constraint);
            if (itemCount == 0 || size1.IsEmpty)
                return generalFlowInfo;
            if (size1.Width > constraint.Width || size1.Height > constraint.Height)
            {
                foreach (ILayoutNode layoutChild in layoutNode.LayoutChildren)
                    layoutChild.MarkHidden();
                generalFlowInfo.referenceExtent = Size.Zero;
                return generalFlowInfo;
            }
            bool flag = this.AllowWrap;
            if (this.Orientation == Orientation.Vertical && this.Columns > 1)
                flag = true;
            else if (this.Orientation == Orientation.Horizontal && this.Rows > 1)
                flag = true;
            MajorMinor majorMinor1 = new MajorMinor(size1, this.Orientation);
            MajorMinor a = new MajorMinor(0, majorMinor1.Minor);
            if (!flag && new MajorMinor(this.ReferenceSize, this.Orientation).Minor == 0)
            {
                majorMinor1.Minor = new MajorMinor(constraint, this.Orientation).Minor;
                size1 = majorMinor1.ToSize(this.Orientation);
            }
            foreach (ILayoutNode layoutChild in layoutNode.LayoutChildren)
            {
                Size size2 = layoutChild.Measure(size1);
                a = MajorMinor.Max(a, new MajorMinor(size2, this.Orientation));
            }
            if (!flag)
            {
                majorMinor1.Minor = a.Minor;
                size1 = majorMinor1.ToSize(this.Orientation);
            }
            MajorMinor majorMinor2 = new MajorMinor(size1, this.Orientation);
            MajorMinor majorMinor3 = new MajorMinor(this.Spacing, this.Orientation);
            MajorMinor majorMinor4 = majorMinor2 + majorMinor3;
            MajorMinor majorMinor5 = new MajorMinor(constraint, this.Orientation);
            MajorMinor majorMinor6 = majorMinor5 + majorMinor3;
            MajorMinor zero1 = MajorMinor.Zero;
            zero1.Major = DivideIntegers(majorMinor6.Major, majorMinor4.Major, false);
            zero1.Minor = DivideIntegers(majorMinor6.Minor, majorMinor4.Minor, false);
            if (zero1.Major >= itemCount)
                flag = false;
            MajorMinor zero2 = MajorMinor.Zero;
            zero2.Major = Math.Min(itemCount, zero1.Major);
            zero2.Minor = 1;
            if (flag && zero2.Major > 0)
            {
                zero2.Minor = DivideIntegers(itemCount, zero2.Major, true);
                zero2.Minor = Math.Min(zero2.Minor, zero1.Minor);
            }
            MajorMinor majorMinor7 = majorMinor4 * zero2 - majorMinor3;
            MajorMinor majorMinor8 = new MajorMinor(this.RepeatGap, 0);
            if (flag)
                majorMinor8 = majorMinor8.Swap();
            MajorMinor majorMinor9 = majorMinor7 + majorMinor8;
            MajorMinor majorMinor10 = majorMinor7;
            if (this.Repeat == RepeatPolicy.WhenTooSmall || this.Repeat == RepeatPolicy.Always)
            {
                if (flag)
                    majorMinor10.Minor = majorMinor5.Minor;
                else
                    majorMinor10.Major = majorMinor5.Major;
            }
            generalFlowInfo.wrapping = flag;
            generalFlowInfo.reference = majorMinor2;
            generalFlowInfo.spacing = majorMinor3;
            generalFlowInfo.referenceWithSpacing = majorMinor4;
            generalFlowInfo.availableSpace = majorMinor5;
            generalFlowInfo.fitItems = zero1;
            generalFlowInfo.totalItems = zero2;
            generalFlowInfo.repeatInstanceSize = majorMinor9;
            generalFlowInfo.usedSize = majorMinor10;
            return generalFlowInfo;
        }

        private void ArrangeChildren(
          ILayoutNode layoutNode,
          GridLayout.GeneralFlowInfo generalInfo,
          out GridLayout.IndexRangeInfo indicesInfo,
          out Vector<int> indicesToDisplayList)
        {
            indicesInfo = this.GetIndexRange(layoutNode, generalInfo);
            indicesToDisplayList = null;
            if (indicesInfo.nonEmptyRange)
            {
                indicesToDisplayList = layoutNode.GetSpecificChildrenRequestList();
                for (int beginIndex = indicesInfo.beginIndex; beginIndex <= indicesInfo.endIndex; ++beginIndex)
                    indicesToDisplayList.Add(beginIndex);
            }
            if (layoutNode.LayoutChildrenCount == 0)
                return;
            Size referenceExtent = generalInfo.referenceExtent;
            if (!generalInfo.wrapping)
            {
                if (this.Orientation == Orientation.Horizontal)
                    referenceExtent.Height = generalInfo.slot.Bounds.Height;
                else
                    referenceExtent.Width = generalInfo.slot.Bounds.Width;
            }
            Rectangle rectangle1 = new Rectangle(Point.Zero, generalInfo.slot.Bounds);
            int num = 0;
            foreach (ILayoutNode layoutChild in layoutNode.LayoutChildren)
            {
                int idx = num;
                if (layoutChild.GetLayoutInput(IndexLayoutInput.Data) is IndexLayoutInput layoutInput)
                {
                    idx = layoutInput.Index.Value;
                    int type = (int)layoutInput.Type;
                }
                Point offset = this.PositionFromIndex(layoutNode, idx, generalInfo, out MajorMinor _);
                if (indicesToDisplayList != null)
                    indicesToDisplayList.Remove(idx);
                Rectangle rectangle2 = new Rectangle(offset, referenceExtent);
                if (generalInfo.repeating || rectangle1.Contains(rectangle2))
                {
                    layoutChild.Arrange(generalInfo.slot, rectangle2);
                    if (layoutChild.ContainsAreaOfInterest(AreaOfInterestID.Focus))
                        generalInfo.focusedItemIndex = new int?(idx);
                }
                else
                    layoutChild.MarkHidden();
                ++num;
            }
        }

        private GridLayout.IndexRangeInfo GetIndexRange(
          ILayoutNode layoutNode,
          GridLayout.GeneralFlowInfo generalInfo)
        {
            if (generalInfo.totalItems.IsEmpty)
                return new GridLayout.IndexRangeInfo();
            MajorMinor position1 = new MajorMinor(generalInfo.slot.PeripheralView.Location.ToSize(), this.Orientation);
            MajorMinor position2 = new MajorMinor(generalInfo.slot.View.Location.ToSize(), this.Orientation);
            MajorMinor majorMinor1 = new MajorMinor(generalInfo.slot.PeripheralView.Size, this.Orientation);
            MajorMinor majorMinor2 = new MajorMinor(generalInfo.slot.View.Size, this.Orientation);
            MajorMinor position3 = position1 + majorMinor1;
            MajorMinor position4 = position2 + majorMinor2;
            bool flag1 = true;
            bool flag2 = true;
            int index1;
            bool flag3 = flag1 & this.GetViewIntersectionFromPosition(layoutNode, position1, generalInfo, ViewIntersectionType.BeginOffscreen, out index1);
            int index2;
            bool flag4 = flag2 & this.GetViewIntersectionFromPosition(layoutNode, position2, generalInfo, ViewIntersectionType.BeginOnscreen, out index2);
            int index3;
            bool flag5 = flag3 & this.GetViewIntersectionFromPosition(layoutNode, position3, generalInfo, ViewIntersectionType.EndOnscreen, out index3);
            int index4;
            bool flag6 = flag4 & this.GetViewIntersectionFromPosition(layoutNode, position4, generalInfo, ViewIntersectionType.EndOffscreen, out index4);
            return !flag5 ? new GridLayout.IndexRangeInfo() : new GridLayout.IndexRangeInfo(index1, index3, index2, index4, generalInfo);
        }

        private Point PositionFromIndex(
          ILayoutNode layoutNode,
          int idx,
          GridLayout.GeneralFlowInfo generalInfo,
          out MajorMinor index)
        {
            int dataIndex;
            int generationValue;
            ListUtility.GetWrappedIndex(idx, generalInfo.itemsCount, out dataIndex, out generationValue);
            index = this.PositionFromIndex(dataIndex, generalInfo.totalItems);
            MajorMinor majorMinor1 = index * generalInfo.referenceWithSpacing;
            MajorMinor majorMinor2 = generalInfo.repeatInstanceSize * new MajorMinor(generationValue, generationValue);
            if (generalInfo.wrapping)
                majorMinor2.Major = 0;
            else
                majorMinor2.Minor = 0;
            return (majorMinor1 + majorMinor2).ToPoint(this.Orientation);
        }

        private MajorMinor PositionFromIndex(int idx, MajorMinor totalItems)
        {
            if (totalItems.IsEmpty)
                return MajorMinor.Zero;
            MajorMinor zero = MajorMinor.Zero;
            zero.Minor = idx / totalItems.Major;
            zero.Major = idx - zero.Minor * totalItems.Major;
            return zero;
        }

        private bool GetViewIntersectionFromPosition(
          ILayoutNode layoutNode,
          MajorMinor position,
          GridLayout.GeneralFlowInfo generalInfo,
          GridLayout.ViewIntersectionType intersectionType,
          out int index)
        {
            MajorMinor repeatInstanceSize = generalInfo.repeatInstanceSize;
            MajorMinor majorMinor1 = position;
            index = 0;
            bool biasUp = false;
            bool flag1 = false;
            switch (intersectionType)
            {
                case ViewIntersectionType.BeginOffscreen:
                case ViewIntersectionType.BeginOnscreen:
                    if (!generalInfo.repeating)
                        flag1 = position.Major >= repeatInstanceSize.Major || position.Minor >= repeatInstanceSize.Minor;
                    if (generalInfo.wrapping)
                        position.Major = 0;
                    biasUp = true;
                    break;
                case ViewIntersectionType.EndOnscreen:
                case ViewIntersectionType.EndOffscreen:
                    if (!generalInfo.repeating)
                        flag1 = position.Major <= 0 || position.Minor <= 0;
                    if (generalInfo.wrapping)
                        position.Major = repeatInstanceSize.Major - 1;
                    biasUp = false;
                    break;
            }
            if (!flag1)
            {
                if (!generalInfo.repeating)
                    position = !biasUp ? MajorMinor.Min(position, repeatInstanceSize - new MajorMinor(1, 1)) : MajorMinor.Max(position, MajorMinor.Zero);
                bool flag2 = position.Major != majorMinor1.Major;
                bool flag3 = position.Minor != majorMinor1.Minor;
                int dataIndex1;
                int generationValue1;
                ListUtility.GetWrappedIndex(position.Major, repeatInstanceSize.Major, out dataIndex1, out generationValue1);
                int dataIndex2;
                int generationValue2;
                ListUtility.GetWrappedIndex(position.Minor, repeatInstanceSize.Minor, out dataIndex2, out generationValue2);
                int generation = generationValue1;
                if (generalInfo.wrapping)
                    generation = generationValue2;
                MajorMinor majorMinor2 = new MajorMinor(dataIndex1, dataIndex2);
                MajorMinor zero = MajorMinor.Zero;
                int result1;
                zero.Major = Math.DivRem(majorMinor2.Major, generalInfo.referenceWithSpacing.Major, out result1);
                int result2;
                zero.Minor = Math.DivRem(majorMinor2.Minor, generalInfo.referenceWithSpacing.Minor, out result2);
                if (generalInfo.wrapping)
                {
                    if (!flag3)
                        zero.Minor = this.ApplyEdgeBias(biasUp, result2, generalInfo.reference.Minor, generalInfo.allowPartialItemsMinor, zero.Minor);
                    int max = generalInfo.totalItems.Major - 1;
                    bool flag4 = zero.Minor == generalInfo.totalItems.Minor - 1;
                    bool flag5 = generalInfo.totalItems.Minor * generalInfo.totalItems.Major > generalInfo.itemsCount;
                    if (flag4 && flag5)
                    {
                        int num = generalInfo.itemsCount % generalInfo.totalItems.Major;
                        if (num != 0)
                            max = num - 1;
                    }
                    zero.Major = Math2.Clamp(zero.Major, 0, max);
                }
                else
                {
                    zero.Minor = 0;
                    if (!flag2)
                        zero.Major = this.ApplyEdgeBias(biasUp, result1, generalInfo.reference.Major, generalInfo.allowPartialItemsMajor, zero.Major);
                }
                index = this.IndexFromGridPosition(zero, generation, generalInfo, out int _);
            }
            return !flag1;
        }

        private int ApplyEdgeBias(
          bool biasUp,
          int remainder,
          int reference,
          bool allowPartialItems,
          int currentGridPosition)
        {
            if (biasUp && (remainder >= reference || remainder != 0 && !allowPartialItems))
                ++currentGridPosition;
            else if (!biasUp && (remainder == 0 || remainder < reference && !allowPartialItems))
                --currentGridPosition;
            return currentGridPosition;
        }

        private int IndexFromGridPosition(
          MajorMinor gridPosition,
          int generation,
          GridLayout.GeneralFlowInfo generalInfo,
          out int normalizedIndex)
        {
            normalizedIndex = gridPosition.Minor * generalInfo.totalItems.Major + gridPosition.Major;
            int num = generalInfo.itemsCount * generation;
            return normalizedIndex + num;
        }

        private static int DivideIntegers(int a, int b, bool roundUp)
        {
            double num = a / (double)b;
            return roundUp ? (int)Math.Ceiling(num) : (int)Math.Floor(num);
        }

        private enum ViewIntersectionType
        {
            BeginOffscreen,
            BeginOnscreen,
            EndOnscreen,
            EndOffscreen,
        }

        private class GeneralFlowInfo
        {
            public Size constraint;
            public LayoutSlot slot;
            public int itemsCount;
            public Size referenceExtent;
            public bool repeating;
            public bool wrapping;
            public bool allowPartialItemsMajor;
            public bool allowPartialItemsMinor;
            public int? focusedItemIndex;
            public MajorMinor reference;
            public MajorMinor spacing;
            public MajorMinor referenceWithSpacing;
            public MajorMinor availableSpace;
            public MajorMinor viewSize;
            public MajorMinor fitItems;
            public MajorMinor totalItems;
            public MajorMinor repeatInstanceSize;
            public MajorMinor usedSize;

            public GeneralFlowInfo(int itemsCount, Size referenceExtent, Size constraint)
            {
                this.itemsCount = itemsCount;
                this.referenceExtent = referenceExtent;
                this.constraint = constraint;
                this.repeating = false;
                this.reference = MajorMinor.Zero;
                this.spacing = MajorMinor.Zero;
                this.referenceWithSpacing = MajorMinor.Zero;
                this.availableSpace = MajorMinor.Zero;
                this.viewSize = MajorMinor.Zero;
                this.totalItems = MajorMinor.Zero;
                this.repeatInstanceSize = MajorMinor.Zero;
                this.usedSize = MajorMinor.Zero;
                this.wrapping = false;
                this.allowPartialItemsMajor = false;
                this.allowPartialItemsMinor = false;
                this.focusedItemIndex = new int?();
            }
        }

        private struct IndexRangeInfo
        {
            private int _beginIndex;
            private int _endIndex;
            private int _realBeginIndex;
            private int _realEndIndex;
            private bool _isValid;

            public IndexRangeInfo(
              int beginIndex,
              int endIndex,
              int realBeginIndex,
              int realEndIndex,
              GridLayout.GeneralFlowInfo generalInfo)
            {
                if (!generalInfo.repeating)
                {
                    NormalizeRange(beginIndex, endIndex, generalInfo.itemsCount, out beginIndex, out endIndex);
                    NormalizeRange(realBeginIndex, realEndIndex, generalInfo.itemsCount, out realBeginIndex, out realEndIndex);
                }
                this._beginIndex = beginIndex;
                this._endIndex = endIndex;
                this._realBeginIndex = realBeginIndex;
                this._realEndIndex = realEndIndex;
                this._isValid = true;
            }

            private static void NormalizeRange(
              int begin,
              int end,
              int itemsCount,
              out int normalizedBegin,
              out int normalizedEnd)
            {
                int generationValue1;
                ListUtility.GetWrappedIndex(begin, itemsCount, out normalizedBegin, out generationValue1);
                int generationValue2;
                ListUtility.GetWrappedIndex(end, itemsCount, out normalizedEnd, out generationValue2);
                if (generationValue2 != generationValue1)
                {
                    normalizedEnd = ListUtility.GetUnwrappedIndex(normalizedEnd, generationValue2 - generationValue1, itemsCount);
                    normalizedBegin -= normalizedEnd - itemsCount;
                    normalizedEnd = itemsCount;
                }
                normalizedBegin = Math2.Clamp(normalizedBegin, 0, itemsCount);
                normalizedEnd = Math2.Clamp(normalizedEnd, 0, itemsCount);
            }

            public int beginIndex => this._beginIndex;

            public int endIndex => this._endIndex;

            public int realBeginIndex => this._realBeginIndex;

            public int realEndIndex => this._realEndIndex;

            public bool nonEmptyRange => this._isValid;
        }
    }
}
