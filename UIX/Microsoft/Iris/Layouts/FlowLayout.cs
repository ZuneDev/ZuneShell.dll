// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Layouts.FlowLayout
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
    internal class FlowLayout : ILayout
    {
        private Orientation _orientationFlow;
        private MajorMinor _spacingMajorMinor;
        private MajorMinor _repeatGapMajorMinor;
        private RepeatPolicy _repeat;
        private StripAlignment _stripAlignment;
        private bool _allowWrapFlag;
        private MissingItemPolicy _policyForMissingItems;
        private int _minimumSampleSizeValue;
        private ItemAlignment _defaultChildAlignment;

        public FlowLayout()
        {
            this._orientationFlow = Orientation.Horizontal;
            this._minimumSampleSizeValue = 3;
        }

        public Orientation Orientation
        {
            get => this._orientationFlow;
            set => this._orientationFlow = value;
        }

        public MajorMinor Spacing
        {
            get => this._spacingMajorMinor;
            set => this._spacingMajorMinor = value;
        }

        public bool AllowWrap
        {
            get => this._allowWrapFlag;
            set => this._allowWrapFlag = value;
        }

        public StripAlignment StripAlignment
        {
            get => this._stripAlignment;
            set => this._stripAlignment = value;
        }

        public RepeatPolicy Repeat
        {
            get => this._repeat;
            set => this._repeat = value;
        }

        public MajorMinor RepeatGap
        {
            get => this._repeatGapMajorMinor;
            set => this._repeatGapMajorMinor = value;
        }

        public MissingItemPolicy MissingItemPolicy
        {
            get => this._policyForMissingItems;
            set => this._policyForMissingItems = value;
        }

        public int MinimumSampleSize
        {
            get => this._minimumSampleSizeValue;
            set => this._minimumSampleSizeValue = value;
        }

        public ItemAlignment DefaultChildAlignment
        {
            get => this._defaultChildAlignment;
            set => this._defaultChildAlignment = value;
        }

        Size ILayout.Measure(ILayoutNode layoutNode, Size constraint)
        {
            if (!(layoutNode.MeasureData is FlowLayout.Packet packet))
            {
                packet = new FlowLayout.Packet();
                layoutNode.MeasureData = (object)packet;
            }
            else
                packet.Clear();
            packet.Subject = layoutNode;
            packet.Constraint = constraint;
            packet.Available = new MajorMinor(constraint, this.Orientation);
            packet.Cache = (FlowSizeMemoryLayoutInput)packet.Subject.GetLayoutInput(FlowSizeMemoryLayoutInput.Data);
            packet.RepeatOrientation = this.Orientation;
            if (this.AllowWrap)
                packet.RepeatOrientation = this.Orientation == Orientation.Vertical ? Orientation.Horizontal : Orientation.Vertical;
            this.DetermineCount(packet);
            if (layoutNode.LayoutChildrenCount == 0)
            {
                if (packet.PotentialCount > 0)
                    layoutNode.RequestMoreChildren(1);
                return Size.Zero;
            }
            this.CreateRecordsFromChildren(packet);
            this.HandleMissingRecords(packet);
            this.MeasureRecords(packet);
            if (packet.DesiredSize.IsEmpty && packet.PotentialCount != layoutNode.LayoutChildrenCount)
                layoutNode.RequestMoreChildren(1);
            return packet.DesiredSize.ToSize(this.Orientation);
        }

        void ILayout.Arrange(ILayoutNode layoutNode, LayoutSlot slot)
        {
            if (layoutNode.LayoutChildrenCount == 0)
                return;
            FlowLayout.Packet measureData = (FlowLayout.Packet)layoutNode.MeasureData;
            measureData.Slot = slot;
            Rectangle peripheralView = slot.PeripheralView;
            Rectangle view = slot.View;
            measureData.PeripheralStart = new MajorMinor(new Size(peripheralView.Location), measureData.RepeatOrientation);
            measureData.PeripheralSize = new MajorMinor(peripheralView.Size, measureData.RepeatOrientation);
            measureData.PeripheralEnd = measureData.PeripheralStart + measureData.PeripheralSize;
            measureData.ViewStart = new MajorMinor(new Size(view.Location), measureData.RepeatOrientation);
            measureData.ViewSize = new MajorMinor(view.Size, measureData.RepeatOrientation);
            measureData.ViewEnd = measureData.ViewStart + measureData.ViewSize;
            this.DetermineRepeatPolicy(measureData);
            this.ArrangeChildren(measureData);
            this.CalculateVisualIntersections(measureData);
            this.RequestMissingChildren(measureData);
            MajorMinor majorMinor = measureData.DesiredSize;
            if (measureData.Repeat)
            {
                majorMinor = new MajorMinor(measureData.Available.Major, measureData.DesiredSize.Minor);
                Rectangle rectangle = new Rectangle(new MajorMinor(-measureData.Available.Major / 2, 0).ToPoint(this._orientationFlow), majorMinor.ToSize(this._orientationFlow));
                layoutNode.AddAreaOfInterest(new AreaOfInterest(rectangle, AreaOfInterestID.ScrollableRange));
            }
            layoutNode.SetVisibleIndexRange(measureData.BeginVisible, measureData.EndVisible, measureData.BeginVisibleOffscreen, measureData.EndVisibleOffscreen, measureData.FocusedItem);
        }

        private void DetermineCount(FlowLayout.Packet packet)
        {
            int num = !(packet.Subject.GetLayoutInput(CountLayoutInput.Data) is CountLayoutInput layoutInput) ? packet.Subject.LayoutChildrenCount : layoutInput.Count;
            packet.PotentialCount = num;
        }

        private void InitializeRecordList(ref Vector<FlowLayout.Record> recordList, int count)
        {
            if (recordList == null)
            {
                recordList = new Vector<FlowLayout.Record>(count);
            }
            else
            {
                int count1 = recordList.Count - count;
                if (count1 > 0)
                    recordList.RemoveRange(count, count1);
                foreach (FlowLayout.Record record in recordList)
                    record?.Clear();
            }
            for (int count1 = recordList.Count; count1 < count; ++count1)
                recordList.Add((FlowLayout.Record)null);
        }

        private void CreateRecordsFromChildren(FlowLayout.Packet packet)
        {
            this.InitializeRecordList(ref packet.Records, packet.PotentialCount);
            Vector<int> availableDataIndices = packet.AvailableDataIndices;
            Vector<int> availableVirtualIndices = packet.AvailableVirtualIndices;
            bool flag = false;
            int childIndex = 0;
            foreach (ILayoutNode layoutChild in packet.Subject.LayoutChildren)
            {
                int virtualIndex;
                int dataIndex;
                IndexType type;
                this.GetIndex(layoutChild, childIndex, packet.PotentialCount, out virtualIndex, out dataIndex, out int _, out type);
                Vector<FlowLayout.Record> vector = packet.Records;
                if (type == IndexType.Content)
                {
                    availableVirtualIndices.Add(virtualIndex);
                }
                else
                {
                    if (!flag)
                    {
                        this.InitializeRecordList(ref packet.Dividers, packet.PotentialCount);
                        flag = true;
                    }
                    vector = packet.Dividers;
                }
                FlowLayout.Record record1 = vector[dataIndex];
                if (FlowLayout.Record.IsNullOrEmpty(record1))
                {
                    FlowLayout.Record record2 = record1;
                    if (record2 == null)
                        vector[dataIndex] = record2 = new FlowLayout.Record();
                    record2.Initialize(FlowLayout.RecordSourceType.LayoutNode);
                    if (record2.Nodes == null)
                        record2.Nodes = new Vector<ILayoutNode>(1);
                    record2.Nodes.Add(layoutChild);
                    record2.Index = dataIndex;
                    if (type == IndexType.Content)
                    {
                        availableDataIndices.Add(record2.Index);
                    }
                    else
                    {
                        layoutChild.Measure(packet.Constraint);
                        record2.Size = new MajorMinor(layoutChild.DesiredSize, this.Orientation);
                        record2.SizeValid = true;
                    }
                }
                else
                    record1.Nodes.Add(layoutChild);
                ++childIndex;
            }
            if (packet.Cache == null)
            {
                packet.Cache = new FlowSizeMemoryLayoutInput();
                packet.Subject.SetLayoutInput((ILayoutInput)packet.Cache, false);
            }
            else
            {
                int index = 0;
                foreach (Size knownSiz in packet.Cache.KnownSizes)
                {
                    if (index >= packet.Records.Count)
                        break;
                    if (!knownSiz.IsEmpty)
                    {
                        MajorMinor majorMinor = new MajorMinor(knownSiz, this.Orientation);
                        if (!this.AllowWrap)
                            majorMinor = new MajorMinor(majorMinor.Major, 0);
                        FlowLayout.Record record = packet.Records[index];
                        if (!FlowLayout.Record.IsNullOrEmpty(record))
                        {
                            record.CachedSize = majorMinor;
                        }
                        else
                        {
                            if (record == null)
                                packet.Records[index] = record = new FlowLayout.Record();
                            record.Initialize(FlowLayout.RecordSourceType.SizeCache);
                            record.Index = index;
                            record.CachedSize = majorMinor;
                        }
                        ++index;
                    }
                }
            }
        }

        private void GetIndex(
          ILayoutNode childNode,
          int childIndex,
          int itemsCount,
          out int virtualIndex,
          out int dataIndex,
          out int generationValue,
          out IndexType type)
        {
            IndexLayoutInput layoutInput = (IndexLayoutInput)childNode.GetLayoutInput(IndexLayoutInput.Data);
            if (layoutInput != null)
            {
                virtualIndex = layoutInput.Index.Value;
                ListUtility.GetWrappedIndex(virtualIndex, itemsCount, out dataIndex, out generationValue);
                type = layoutInput.Type;
            }
            else
            {
                virtualIndex = childIndex;
                dataIndex = childIndex;
                generationValue = 0;
                type = IndexType.Content;
            }
        }

        private void HandleMissingRecords(FlowLayout.Packet packet)
        {
            int count = packet.AvailableDataIndices.Count;
            if (packet.PotentialCount - count == 0)
                return;
            MissingItemPolicy missingItemPolicy = this.MissingItemPolicy;
            if (count < this._minimumSampleSizeValue)
                missingItemPolicy = MissingItemPolicy.Wait;
            MajorMinor a = this.MissingItemPolicy != MissingItemPolicy.SizeToSmallest ? MajorMinor.Zero : new MajorMinor(16777215, 16777215);
            for (int index = 0; index < packet.Records.Count; ++index)
            {
                FlowLayout.Record record = packet.Records[index];
                if (!FlowLayout.Record.IsNullOrEmpty(record))
                {
                    if (ListUtility.IsNullOrEmpty((IVector)record.Nodes))
                        ++count;
                    switch (missingItemPolicy)
                    {
                        case MissingItemPolicy.SizeToAverage:
                            a += record.CachedSize;
                            continue;
                        case MissingItemPolicy.SizeToSmallest:
                            a = MajorMinor.Min(a, record.CachedSize);
                            continue;
                        case MissingItemPolicy.SizeToLargest:
                            a = MajorMinor.Max(a, record.CachedSize);
                            continue;
                        default:
                            continue;
                    }
                }
            }
            if (packet.PotentialCount == count)
                return;
            if (missingItemPolicy == MissingItemPolicy.SizeToAverage)
            {
                a.Major = (int)Math.Round((double)a.Major / (double)count);
                a.Minor = (int)Math.Round((double)a.Minor / (double)count);
            }
            for (int index = 0; index < packet.Records.Count; ++index)
            {
                FlowLayout.Record record = packet.Records[index];
                if (FlowLayout.Record.IsNullOrEmpty(record))
                {
                    if (record == null)
                        packet.Records[index] = record = new FlowLayout.Record();
                    record.Initialize(FlowLayout.RecordSourceType.Fake);
                    record.Index = index;
                    record.CachedSize = a;
                }
            }
        }

        private void MeasureRecords(FlowLayout.Packet packet)
        {
            MajorMinor offset = MajorMinor.Zero;
            MajorMinor zero = MajorMinor.Zero;
            int num1 = 0;
            int num2 = 0;
            MajorMinor stripSize = MajorMinor.Zero;
            int lastLaidOutIndex = -1;
            int count = packet.Records.Count;
            for (int index = 0; index < packet.Records.Count; ++index)
            {
                FlowLayout.Record record = packet.Records[index];
                this.MeasureRecord(packet, record, offset);
                MajorMinor majorMinor1 = offset + record.Size;
                MajorMinor majorMinor2 = packet.Available - majorMinor1;
                if (majorMinor2.Minor < 0)
                {
                    record.SizeValid = false;
                    break;
                }
                if (majorMinor2.Major < 0)
                {
                    if (this.AllowWrap)
                    {
                        if (num2 == 0)
                        {
                            record.SizeValid = false;
                            break;
                        }
                        this.FinalizeStrip(packet, stripSize, offset, ref zero);
                        ++num1;
                        num2 = 0;
                        stripSize = MajorMinor.Zero;
                        offset = new MajorMinor(0, zero.Minor + this.Spacing.Minor);
                        --index;
                    }
                    else
                        break;
                }
                else
                {
                    record.Strip = num1;
                    record.Offset = offset;
                    FlowLayout.Record divider = this.GetDivider(packet, record);
                    if (divider != null)
                    {
                        if (num2 > 0)
                        {
                            MajorMinor majorMinor3 = offset;
                            majorMinor3.Major -= (this.Spacing.Major + divider.Size.Major) / 2;
                            divider.Offset = majorMinor3;
                            divider.Strip = num1;
                        }
                        else
                            divider.Size = MajorMinor.Zero;
                    }
                    ++num2;
                    stripSize = new MajorMinor(majorMinor1.Major, Math.Max(record.Size.Minor, stripSize.Minor));
                    offset.Major = majorMinor1.Major + this.Spacing.Major;
                    lastLaidOutIndex = index;
                }
            }
            if (num2 != 0)
            {
                this.HandleLastItem(packet, ref lastLaidOutIndex, ref offset, ref stripSize);
                this.FinalizeStrip(packet, stripSize, offset, ref zero);
            }
            packet.LastLaidOutRecord = lastLaidOutIndex;
            packet.StoppedAtRecord = count;
            packet.DesiredSize = zero;
        }

        private void MeasureRecord(
          FlowLayout.Packet packet,
          FlowLayout.Record record,
          MajorMinor offset)
        {
            if (record.SizeValid)
                return;
            Size childConstraint = this.GetChildConstraint(packet, offset);
            record.Size = MajorMinor.Zero;
            if (record.Nodes != null && record.Nodes.Count > 0)
            {
                for (int index = 0; index < record.Nodes.Count; ++index)
                {
                    ILayoutNode node = record.Nodes[index];
                    this.GetIndex(node, 0, packet.PotentialCount, out int _, out int _, out int _, out IndexType _);
                    node.Measure(childConstraint);
                    MajorMinor a = new MajorMinor(node.DesiredSize, this.Orientation);
                    record.Size = index <= 0 || a.Equals((object)record.Size) ? a : MajorMinor.Max(a, record.Size);
                }
                packet.Cache.KnownSizes.ExpandTo(record.Index + 1);
                packet.Cache.KnownSizes[record.Index] = record.Size.ToSize(this.Orientation);
            }
            else
                record.Size = record.CachedSize;
            record.SizeValid = true;
        }

        private Size GetChildConstraint(FlowLayout.Packet packet, MajorMinor offset)
        {
            Size constraint = packet.Constraint;
            return this.AllowWrap ? constraint : (packet.Available - offset).ToSize(this.Orientation);
        }

        private void FinalizeStrip(
          FlowLayout.Packet packet,
          MajorMinor stripSize,
          MajorMinor offset,
          ref MajorMinor desiredSize)
        {
            packet.StripSizes.Add(stripSize);
            desiredSize = new MajorMinor(Math.Max(desiredSize.Major, stripSize.Major), offset.Minor + stripSize.Minor);
        }

        private FlowLayout.Record GetDivider(
          FlowLayout.Packet packet,
          FlowLayout.Record record)
        {
            return packet.Dividers == null ? (FlowLayout.Record)null : packet.Dividers[record.Index];
        }

        private void HandleLastItem(
          FlowLayout.Packet packet,
          ref int lastLaidOutIndex,
          ref MajorMinor offset,
          ref MajorMinor stripSize)
        {
            int index = lastLaidOutIndex + 1;
            if (index == packet.Records.Count)
                return;
            FlowLayout.Record record = packet.Records[index];
            if (ListUtility.IsNullOrEmpty((IVector)record.Nodes))
                return;
            Size size = (packet.Available - offset).ToSize(this.Orientation);
            foreach (ILayoutNode node in record.Nodes)
                node.Measure(size);
            Size desiredSize = record.Nodes[0].DesiredSize;
            if (desiredSize.Width > size.Width || desiredSize.Height > size.Height)
                return;
            record.Size = new MajorMinor(desiredSize, this.Orientation);
            record.SizeValid = true;
            int major = offset.Major + record.Size.Major;
            offset = new MajorMinor(major + this.Spacing.Major, offset.Minor);
            lastLaidOutIndex = index;
            stripSize = new MajorMinor(major, Math.Max(record.Size.Minor, stripSize.Minor));
        }

        private void DetermineRepeatPolicy(FlowLayout.Packet packet)
        {
            MajorMinor majorMinor = new MajorMinor(packet.Slot.View.Size, this.Orientation);
            bool flag1 = packet.DesiredSize.Major <= majorMinor.Major;
            bool flag2;
            switch (this.Repeat)
            {
                case RepeatPolicy.WhenTooBig:
                    flag2 = !flag1;
                    break;
                case RepeatPolicy.WhenTooSmall:
                    flag2 = flag1;
                    break;
                case RepeatPolicy.Always:
                    flag2 = true;
                    break;
                default:
                    flag2 = false;
                    break;
            }
            packet.Repeat = flag2;
        }

        private void CalculateVisualIntersections(FlowLayout.Packet packet)
        {
            if (packet.DesiredSize.IsEmpty)
            {
                packet.BeginVisible = packet.BeginVisibleOffscreen = 0;
                packet.EndVisible = packet.EndVisibleOffscreen = packet.LastLaidOutRecord + 1;
            }
            else
            {
                int major1 = packet.PeripheralStart.Major;
                int major2 = packet.ViewStart.Major;
                int major3 = packet.ViewEnd.Major;
                int major4 = packet.PeripheralEnd.Major;
                packet.BeginVisibleOffscreen = this.IndexFromPosition(packet, major1, false) - 1;
                packet.BeginVisible = packet.BeginVisibleOffscreen;
                if (major2 != major1)
                    packet.BeginVisible = this.IndexFromPosition(packet, major2, false) - 1;
                packet.EndVisible = this.IndexFromPosition(packet, major3, false);
                packet.EndVisibleOffscreen = packet.EndVisible;
                if (major4 == major3)
                    return;
                packet.EndVisibleOffscreen = this.IndexFromPosition(packet, major4, false);
            }
        }

        private int GetRepeatMajor(FlowLayout.Packet packet, MajorMinor majorMinor) => this.Orientation != packet.RepeatOrientation ? majorMinor.Minor : majorMinor.Major;

        private int IndexFromPosition(FlowLayout.Packet packet, int position, bool exactMatch)
        {
            int dataIndex1 = position;
            int generationValue = 0;
            if (packet.PotentialCount == packet.LastLaidOutRecord + 1)
            {
                int itemsCount = this.GetRepeatMajor(packet, packet.DesiredSize) + this.GetRepeatMajor(packet, this.RepeatGap);
                ListUtility.GetWrappedIndex(position, itemsCount, out dataIndex1, out generationValue);
            }
            int dataIndex2 = packet.PotentialCount;
            for (int index = 0; index < packet.Records.Count; ++index)
            {
                FlowLayout.Record record = packet.Records[index];
                if (exactMatch && this.GetRepeatMajor(packet, record.Offset) <= dataIndex1 && this.GetRepeatMajor(packet, record.Offset) + this.GetRepeatMajor(packet, record.Size) >= dataIndex1)
                {
                    dataIndex2 = record.Index;
                    break;
                }
                if (index == packet.StoppedAtRecord)
                {
                    dataIndex2 = packet.PotentialCount;
                    break;
                }
                if (this.GetRepeatMajor(packet, record.Offset) >= dataIndex1 || index > packet.LastLaidOutRecord)
                {
                    dataIndex2 = record.Index;
                    break;
                }
            }
            int num = dataIndex2;
            if (packet.PotentialCount == packet.LastLaidOutRecord + 1)
                num = ListUtility.GetUnwrappedIndex(dataIndex2, generationValue, packet.PotentialCount);
            return num;
        }

        private void FilterIntersectionPoints(FlowLayout.Packet packet)
        {
            int num1 = packet.BeginVisibleOffscreen;
            int num2 = packet.EndVisibleOffscreen;
            if (!packet.Repeat)
            {
                num1 = 0;
                num2 = packet.PotentialCount;
            }
            if (packet.LastLaidOutRecord < packet.PotentialCount - 1)
            {
                num1 = Math.Max(Math.Min(num1, packet.LastLaidOutRecord), 0);
                num2 = Math.Max(Math.Min(num2, packet.LastLaidOutRecord + 2), num1 + 1);
            }
            if (num1 == packet.BeginVisibleOffscreen && num2 == packet.EndVisibleOffscreen)
                return;
            packet.BeginVisibleOffscreen = Math2.Clamp(packet.BeginVisibleOffscreen, num1, num2);
            packet.BeginVisible = Math2.Clamp(packet.BeginVisible, num1, num2);
            packet.BeginVisible = Math.Max(packet.BeginVisible, packet.BeginVisibleOffscreen);
            packet.EndVisible = Math.Max(packet.EndVisible, packet.BeginVisible + 1);
            packet.EndVisible = Math2.Clamp(packet.EndVisible, num1, num2);
            packet.EndVisibleOffscreen = Math2.Clamp(packet.EndVisibleOffscreen, num1, num2);
            packet.EndVisibleOffscreen = Math.Max(packet.EndVisibleOffscreen, packet.EndVisible);
        }

        private void RequestMissingChildren(FlowLayout.Packet packet)
        {
            this.FilterIntersectionPoints(packet);
            int visibleOffscreen = packet.BeginVisibleOffscreen;
            int num1 = packet.EndVisibleOffscreen - 1;
            int num2 = -1;
            if (packet.AvailableDataIndices.Count < this._minimumSampleSizeValue)
                num2 = this._minimumSampleSizeValue;
            Vector<int> indiciesList = (Vector<int>)null;
            for (int index = visibleOffscreen; index <= num1; ++index)
            {
                if (!IntListUtility.Contains(packet.AvailableVirtualIndices, index))
                {
                    if (indiciesList == null)
                        indiciesList = packet.Subject.GetSpecificChildrenRequestList();
                    indiciesList.Add(index);
                    if (num2 > 0 && indiciesList.Count >= num2)
                        break;
                }
            }
            if (ListUtility.IsNullOrEmpty((IVector)indiciesList))
                return;
            packet.Subject.RequestSpecificChildren(indiciesList);
        }

        private void ArrangeChildren(FlowLayout.Packet packet)
        {
            MajorMinor majorMinor1 = MajorMinor.Zero;
            int strip = -1;
            int childIndex = 0;
            foreach (ILayoutNode layoutChild in packet.Subject.LayoutChildren)
            {
                int virtualIndex;
                int dataIndex;
                int generationValue;
                IndexType type;
                this.GetIndex(layoutChild, childIndex, packet.PotentialCount, out virtualIndex, out dataIndex, out generationValue, out type);
                FlowLayout.Record record = type != IndexType.Content ? packet.Dividers[dataIndex] : packet.Records[dataIndex];
                if (!record.SizeValid)
                {
                    layoutChild.MarkHidden();
                }
                else
                {
                    if (type == IndexType.Content && strip != record.Strip)
                    {
                        strip = record.Strip;
                        majorMinor1 = this.GetStripOffset(packet, strip);
                    }
                    MajorMinor majorMinor2 = this.PositionFromIndex(packet, record.Offset, generationValue);
                    majorMinor2.Major += majorMinor1.Major;
                    Size size = new MajorMinor(new MajorMinor(layoutChild.DesiredSize, this.Orientation).Major, majorMinor1.Minor).ToSize(this.Orientation);
                    Rectangle bounds = new Rectangle(majorMinor2.ToPoint(this.Orientation), size);
                    layoutChild.Arrange(packet.Slot, bounds);
                    if (layoutChild.ContainsAreaOfInterest(AreaOfInterestID.Focus))
                        packet.FocusedItem = new int?(virtualIndex);
                    ++childIndex;
                }
            }
        }

        private MajorMinor GetStripOffset(FlowLayout.Packet packet, int strip)
        {
            MajorMinor zero = MajorMinor.Zero;
            if (strip >= 0 && strip < packet.StripSizes.Count)
            {
                MajorMinor stripSiz = packet.StripSizes[strip];
                MajorMinor majorMinor1 = new MajorMinor(packet.Slot.Bounds, this.Orientation);
                switch (this.StripAlignment)
                {
                    case StripAlignment.Center:
                        zero.Major = (majorMinor1.Major - stripSiz.Major) / 2;
                        break;
                    case StripAlignment.Far:
                        zero.Major = majorMinor1.Major - stripSiz.Major;
                        break;
                }
                if (!this.AllowWrap)
                {
                    MajorMinor majorMinor2 = new MajorMinor(packet.Slot.Bounds, this.Orientation);
                    zero.Minor = majorMinor2.Minor;
                }
                else
                    zero.Minor = stripSiz.Minor;
            }
            return zero;
        }

        private MajorMinor PositionFromIndex(
          FlowLayout.Packet packet,
          MajorMinor recordoffset,
          int generationValue)
        {
            if (generationValue == 0)
                return recordoffset;
            MajorMinor majorMinor = recordoffset;
            if (this.Orientation == packet.RepeatOrientation)
                majorMinor.Major = ListUtility.GetUnwrappedIndex(majorMinor.Major, generationValue, packet.DesiredSize.Major + this.RepeatGap.Major);
            else
                majorMinor.Minor = ListUtility.GetUnwrappedIndex(majorMinor.Minor, generationValue, packet.DesiredSize.Minor + this.RepeatGap.Minor);
            return majorMinor;
        }

        internal class Record
        {
            public int Index;
            public Vector<ILayoutNode> Nodes;
            public MajorMinor Size;
            public bool SizeValid;
            public MajorMinor CachedSize;
            public MajorMinor Offset;
            public int Strip;

            public static bool IsNullOrEmpty(FlowLayout.Record record) => record == null || record.Index == int.MinValue;

            public void Clear() => this.Initialize(FlowLayout.RecordSourceType.Unspecified);

            public void Initialize(FlowLayout.RecordSourceType source)
            {
                this.Index = int.MinValue;
                if (this.Nodes != null)
                    this.Nodes.Clear();
                this.Size = MajorMinor.Zero;
                this.SizeValid = false;
                this.CachedSize = MajorMinor.Zero;
                this.Offset = MajorMinor.Zero;
                this.Strip = 0;
            }

            public override string ToString() => InvariantString.Format("Record[{0}] Size:{1}", (object)this.Index, (object)this.Size);
        }

        internal enum RecordSourceType
        {
            Unspecified,
            LayoutNode,
            SizeCache,
            Fake,
        }

        private class Packet
        {
            public ILayoutNode Subject;
            public Size Constraint;
            public LayoutSlot Slot;
            public MajorMinor Available;
            public FlowSizeMemoryLayoutInput Cache;
            public Orientation RepeatOrientation;
            public MajorMinor PeripheralStart;
            public MajorMinor PeripheralSize;
            public MajorMinor PeripheralEnd;
            public MajorMinor ViewStart;
            public MajorMinor ViewSize;
            public MajorMinor ViewEnd;
            public Vector<FlowLayout.Record> Records;
            public Vector<FlowLayout.Record> Dividers;
            public int PotentialCount;
            public Vector<int> AvailableVirtualIndices = new Vector<int>();
            public Vector<int> AvailableDataIndices = new Vector<int>();
            public MajorMinor DesiredSize;
            public Vector<MajorMinor> StripSizes = new Vector<MajorMinor>();
            public int LastLaidOutRecord;
            public int StoppedAtRecord;
            public bool Repeat;
            public int BeginVisibleOffscreen;
            public int BeginVisible;
            public int EndVisible;
            public int EndVisibleOffscreen;
            public int? FocusedItem;

            public void Clear()
            {
                this.PeripheralStart = MajorMinor.Zero;
                this.PeripheralSize = MajorMinor.Zero;
                this.PeripheralEnd = MajorMinor.Zero;
                this.ViewStart = MajorMinor.Zero;
                this.ViewSize = MajorMinor.Zero;
                this.ViewEnd = MajorMinor.Zero;
                this.PotentialCount = 0;
                this.AvailableVirtualIndices.Clear();
                this.AvailableDataIndices.Clear();
                this.DesiredSize = MajorMinor.Zero;
                this.StripSizes.Clear();
                this.LastLaidOutRecord = 0;
                this.StoppedAtRecord = 0;
                this.Repeat = false;
                this.BeginVisibleOffscreen = 0;
                this.BeginVisible = 0;
                this.EndVisible = 0;
                this.EndVisibleOffscreen = 0;
                this.FocusedItem = new int?();
            }
        }
    }
}
