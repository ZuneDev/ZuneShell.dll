// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Layouts.AnchorLayout
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layout;
using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using System;

namespace Microsoft.Iris.Layouts
{
    internal class AnchorLayout : ILayout
    {
        private bool _sizeToHorizontalChildrenFlag = true;
        private bool _sizeToVerticalChildrenFlag = true;
        private ItemAlignment _defaultChildAlignment;
        private static readonly DataCookie s_dataProperty = DataCookie.ReserveSlot();
        private static AnchorEdge s_anchorParent0 = new AnchorEdge("Parent", 0.0f);
        private static AnchorEdge s_anchorParent1 = new AnchorEdge("Parent", 1f);
        private static AnchorEdge s_anchorParentActual0 = new AnchorEdge("ParentActual", 0.0f);
        private static AnchorEdge s_anchorParentActual1 = new AnchorEdge("ParentActual", 1f);

        public bool SizeToHorizontalChildren
        {
            get => this._sizeToHorizontalChildrenFlag;
            set => this._sizeToHorizontalChildrenFlag = value;
        }

        public bool SizeToVerticalChildren
        {
            get => this._sizeToVerticalChildrenFlag;
            set => this._sizeToVerticalChildrenFlag = value;
        }

        public ItemAlignment DefaultChildAlignment
        {
            get => this._defaultChildAlignment;
            set => this._defaultChildAlignment = value;
        }

        Size ILayout.Measure(ILayoutNode layoutNode, Size constraint)
        {
            if (!(layoutNode.MeasureData is AnchorLayout.Packet packet))
            {
                packet = new AnchorLayout.Packet();
                layoutNode.MeasureData = (object)packet;
                packet.ParentRecord = new AnchorLayout.Record("Parent", new Rectangle(Point.Zero, constraint), AnchorLayout.LayoutPhase.Done);
                packet.ParentActualRecord = new AnchorLayout.Record("ParentActual", Rectangle.Zero, AnchorLayout.LayoutPhase.Arrange);
                packet.AreaOfInterestRecord = new AnchorLayout.Record("Focus", Rectangle.Zero, AnchorLayout.LayoutPhase.Arrange);
                packet.Records = new Vector<AnchorLayout.Record>(layoutNode.LayoutChildrenCount);
            }
            else
            {
                packet.ParentRecord.Bounds = new Rectangle(Point.Zero, constraint);
                packet.CircularitiesDetected = false;
                packet.CircularityBreakerRecord = (AnchorLayout.Record)null;
            }
            packet.Subject = layoutNode;
            packet.Constraint = constraint;
            packet.TotalBounds = new Rectangle(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
            Size zero = Size.Zero;
            if (layoutNode.LayoutChildrenCount == 0)
            {
                packet.Records.Clear();
                if (!this._sizeToHorizontalChildrenFlag)
                    zero.Width = constraint.Width;
                if (!this._sizeToVerticalChildrenFlag)
                    zero.Height = constraint.Height;
                layoutNode.RequestMoreChildren(int.MaxValue);
                return zero;
            }
            int index = 0;
            foreach (ILayoutNode layoutChild in layoutNode.LayoutChildren)
            {
                if (index < packet.Records.Count)
                {
                    packet.Records[index].Initialize(layoutChild);
                }
                else
                {
                    AnchorLayout.Record record = new AnchorLayout.Record(layoutChild);
                    packet.Records.Add(record);
                }
                ++index;
            }
            packet.Records.RemoveRange(index, packet.Records.Count - index);
            foreach (AnchorLayout.Record record in packet.Records)
            {
                if (record.Phase == AnchorLayout.LayoutPhase.Untouched)
                {
                    packet.CircularityBreakerRecord = (AnchorLayout.Record)null;
                    this.MeasureChild(packet, record);
                }
            }
            if (packet.CircularitiesDetected)
            {
                foreach (AnchorLayout.Record record in packet.Records)
                {
                    if (record.Phase == AnchorLayout.LayoutPhase.Circular)
                        record.Phase = AnchorLayout.LayoutPhase.Untouched;
                }
                packet.CircularitiesDetected = false;
                foreach (AnchorLayout.Record record in packet.Records)
                {
                    if (record.Phase == AnchorLayout.LayoutPhase.Untouched)
                        this.MeasureChild(packet, record);
                }
            }
            Size size = constraint;
            if (this._sizeToHorizontalChildrenFlag || this._sizeToVerticalChildrenFlag)
            {
                if (this._sizeToHorizontalChildrenFlag)
                    size.Width = packet.TotalBounds.Width;
                if (this._sizeToVerticalChildrenFlag)
                    size.Height = packet.TotalBounds.Height;
                foreach (AnchorLayout.Record record in packet.Records)
                {
                    Rectangle bounds = record.Bounds;
                    if (this._sizeToHorizontalChildrenFlag)
                        bounds.X -= packet.TotalBounds.X;
                    if (this._sizeToVerticalChildrenFlag)
                        bounds.Y -= packet.TotalBounds.Y;
                    record.Bounds = bounds;
                }
            }
            layoutNode.RequestMoreChildren(int.MaxValue);
            return size;
        }

        private void MeasureChild(AnchorLayout.Packet packet, AnchorLayout.Record record)
        {
            AnchorLayoutInput input = record.Input;
            record.Phase = AnchorLayout.LayoutPhase.InProgress;
            bool allArrangePhase = true;
            bool anyArrangePhase = false;
            int edge1 = this.ComputeEdge(packet, "Left", input.Left, AnchorLayout.s_anchorParent0, Orientation.Horizontal, record, out AnchorLayout.Record _, ref allArrangePhase, ref anyArrangePhase);
            int edge2 = this.ComputeEdge(packet, "Top", input.Top, AnchorLayout.s_anchorParent0, Orientation.Vertical, record, out AnchorLayout.Record _, ref allArrangePhase, ref anyArrangePhase);
            int edge3 = this.ComputeEdge(packet, "Right", input.Right, AnchorLayout.s_anchorParent1, Orientation.Horizontal, record, out AnchorLayout.Record _, ref allArrangePhase, ref anyArrangePhase);
            int edge4 = this.ComputeEdge(packet, "Bottom", input.Bottom, AnchorLayout.s_anchorParent1, Orientation.Vertical, record, out AnchorLayout.Record _, ref allArrangePhase, ref anyArrangePhase);
            if (anyArrangePhase)
            {
                if (!allArrangePhase)
                {
                    ErrorManager.ReportError("All AnchorEdges must refer to actual positions on AnchorLayoutInput {0}.", (object)record.Input);
                    record.Invalid = true;
                }
                if (record.Input.ContributesToHeight || record.Input.ContributesToWidth)
                {
                    ErrorManager.ReportError("AnchorLayoutInput {0} cannot contribute to width or height.", (object)record.Input);
                    record.Invalid = true;
                }
                record.Phase = AnchorLayout.LayoutPhase.Arrange;
            }
            else
            {
                AnchorLayout.Normalize(ref edge1, ref edge3);
                AnchorLayout.Normalize(ref edge2, ref edge4);
                int constraintMaxValue1;
                AnchorLayout.ComputeConstraint(edge1, edge3, out constraintMaxValue1);
                int constraintMaxValue2;
                AnchorLayout.ComputeConstraint(edge2, edge4, out constraintMaxValue2);
                Size constraint = new Size(constraintMaxValue1, constraintMaxValue2);
                record.LayoutNode.Measure(constraint);
                int size1;
                int x = AnchorLayout.LocationFromEdges(input.Left, input.Right, edge1, edge3, record.LayoutNode.DesiredSize.Width, record.LayoutNode.AlignedSize.Width, record.LayoutNode.AlignmentOffset.X, out size1);
                int size2;
                int y = AnchorLayout.LocationFromEdges(input.Top, input.Bottom, edge2, edge4, record.LayoutNode.DesiredSize.Height, record.LayoutNode.AlignedSize.Height, record.LayoutNode.AlignmentOffset.Y, out size2);
                record.Bounds = new Rectangle(x, y, size1, size2);
                bool flag = packet.CircularityBreakerRecord == record;
                if (record.Phase != AnchorLayout.LayoutPhase.InProgress && !flag)
                    return;
                record.Phase = AnchorLayout.LayoutPhase.Done;
                Rectangle bounds = record.Bounds;
                if (!input.ContributesToWidth)
                {
                    bounds.X = int.MaxValue;
                    bounds.Width = int.MinValue;
                }
                if (!input.ContributesToHeight)
                {
                    bounds.Y = int.MaxValue;
                    bounds.Height = int.MinValue;
                }
                packet.TotalBounds = Rectangle.Union(packet.TotalBounds, bounds);
            }
        }

        private int ComputeEdge(
          AnchorLayout.Packet packet,
          string edgeName,
          AnchorEdge anchor,
          AnchorEdge fallback,
          Orientation orientation,
          AnchorLayout.Record record,
          out AnchorLayout.Record recordRef,
          ref bool allArrangePhase,
          ref bool anyArrangePhase)
        {
            anchor = anchor != (AnchorEdge)null ? anchor : fallback;
            recordRef = this.GetReferenceRecord(packet, anchor.Id);
            if (recordRef == null)
            {
                allArrangePhase = false;
                return 0;
            }
            bool flag = recordRef.Phase == AnchorLayout.LayoutPhase.Arrange;
            allArrangePhase &= flag;
            anyArrangePhase |= flag;
            record.Visible &= recordRef.Visible;
            Rectangle bounds = recordRef.Bounds;
            if (recordRef.Phase != AnchorLayout.LayoutPhase.Done && recordRef.Phase != AnchorLayout.LayoutPhase.Arrange)
            {
                record.Phase = AnchorLayout.LayoutPhase.Circular;
                bounds = packet.ParentRecord.Bounds;
                if (recordRef.Phase == AnchorLayout.LayoutPhase.InProgress && packet.CircularityBreakerRecord == null)
                {
                    packet.CircularityBreakerRecord = recordRef;
                    packet.CircularitiesDetected = true;
                }
            }
            int val1 = AnchorLayout.Weigh(orientation == Orientation.Horizontal ? bounds.Width : bounds.Height, anchor.Percent) + (orientation == Orientation.Horizontal ? bounds.X : bounds.Y) + anchor.Offset;
            int num = orientation == Orientation.Horizontal ? packet.ParentRecord.Bounds.Width : packet.ParentRecord.Bounds.Height;
            if (anchor.MaximumSet)
            {
                int val2 = AnchorLayout.Weigh(num, anchor.MaximumPercent) + anchor.MaximumOffset;
                val1 = Math.Min(val1, val2);
            }
            if (anchor.MinimumSet)
            {
                int val2 = AnchorLayout.Weigh(num, anchor.MinimumPercent) + anchor.MinimumOffset;
                val1 = Math.Max(val1, val2);
            }
            return val1;
        }

        private static void ComputeConstraint(int nearValue, int farValue, out int constraintMaxValue) => constraintMaxValue = farValue - nearValue;

        private static int LocationFromEdges(
          AnchorEdge near,
          AnchorEdge far,
          int nearValue,
          int farValue,
          int desiredSize,
          int alignedSize,
          int alignmentOffset,
          out int size)
        {
            size = desiredSize;
            int num;
            if (near == (AnchorEdge)null && far == (AnchorEdge)null)
                num = (nearValue + farValue - desiredSize) / 2;
            else if (near != (AnchorEdge)null && far == (AnchorEdge)null)
                num = nearValue;
            else if (near == (AnchorEdge)null && far != (AnchorEdge)null)
            {
                num = farValue - desiredSize;
            }
            else
            {
                num = nearValue + alignmentOffset;
                size = alignedSize;
            }
            return num;
        }

        private static void Normalize(ref int nearValue, ref int farValue)
        {
            if (farValue >= nearValue)
                return;
            farValue = nearValue;
        }

        private static int Weigh(int value, float percentValue) => (int)((double)value * (double)percentValue);

        void ILayout.Arrange(ILayoutNode layoutNode, LayoutSlot slot)
        {
            AnchorLayout.Packet measureData = (AnchorLayout.Packet)layoutNode.MeasureData;
            if (measureData.Records == null)
                return;
            measureData.ParentActualRecord.Bounds = new Rectangle(Point.Zero, slot.Bounds);
            measureData.AreaOfInterestRecord.Bounds = Rectangle.Zero;
            Vector<AnchorLayout.Record> vector = (Vector<AnchorLayout.Record>)null;
            AnchorLayout.Record record1 = (AnchorLayout.Record)null;
            foreach (AnchorLayout.Record record2 in measureData.Records)
            {
                if (record2.Phase != AnchorLayout.LayoutPhase.Arrange)
                {
                    record2.LayoutNode.Arrange(slot, record2.Bounds);
                    if (record1 == null && (record2.LayoutNode.ContainsAreaOfInterest(AreaOfInterestID.FocusOverride) || record2.LayoutNode.ContainsAreaOfInterest(AreaOfInterestID.Focus)))
                        record1 = record2;
                }
                else if (!record2.Invalid)
                {
                    if (vector == null)
                        vector = new Vector<AnchorLayout.Record>();
                    vector.Add(record2);
                }
                else
                    record2.LayoutNode.MarkHidden();
            }
            if (vector == null)
                return;
            if (record1 != null)
            {
                AreaOfInterest area;
                if (!record1.LayoutNode.TryGetAreaOfInterest(AreaOfInterestID.FocusOverride, out area))
                    record1.LayoutNode.TryGetAreaOfInterest(AreaOfInterestID.Focus, out area);
                measureData.AreaOfInterestRecord.Bounds = Rectangle.Offset(area.DisplayRectangle, record1.Bounds.Location);
            }
            foreach (AnchorLayout.Record record2 in vector)
            {
                AnchorLayoutInput input = record2.Input;
                bool flag = false;
                AnchorLayout.Record recordRef;
                int edge1 = this.ComputeEdge(measureData, "Left", input.Left, (AnchorEdge)null, Orientation.Horizontal, record2, out recordRef, ref flag, ref flag);
                int edge2 = this.ComputeEdge(measureData, "Top", input.Top, (AnchorEdge)null, Orientation.Vertical, record2, out recordRef, ref flag, ref flag);
                int edge3 = this.ComputeEdge(measureData, "Right", input.Right, (AnchorEdge)null, Orientation.Horizontal, record2, out recordRef, ref flag, ref flag);
                int edge4 = this.ComputeEdge(measureData, "Bottom", input.Bottom, (AnchorEdge)null, Orientation.Vertical, record2, out recordRef, ref flag, ref flag);
                int width = edge3 - edge1;
                int height = edge4 - edge2;
                record2.LayoutNode.Measure(new Size(width, height));
                record2.LayoutNode.Arrange(slot, new Rectangle(edge1, edge2, width, height));
            }
        }

        private AnchorLayout.Record GetReferenceRecord(
          AnchorLayout.Packet packet,
          string id)
        {
            if (id == "Parent")
                return packet.ParentRecord;
            if (id == "ParentActual")
                return packet.ParentActualRecord;
            if (id == "Focus")
                return packet.AreaOfInterestRecord;
            foreach (AnchorLayout.Record record in packet.Records)
            {
                if (record.ID == id)
                {
                    if (record.Phase == AnchorLayout.LayoutPhase.Untouched)
                        this.MeasureChild(packet, record);
                    return record;
                }
            }
            ErrorManager.ReportError("Anchor layout: {0} cannot find the '{1}' child", (object)this.GetType().Name, (object)id);
            return (AnchorLayout.Record)null;
        }

        internal static DataCookie InputData => AnchorLayout.s_dataProperty;

        internal enum LayoutPhase
        {
            Untouched,
            InProgress,
            Circular,
            Done,
            Arrange,
        }

        internal class Record
        {
            public ILayoutNode LayoutNode;
            public string ID;
            public AnchorLayoutInput Input;
            public Rectangle Bounds;
            public AnchorLayout.LayoutPhase Phase;
            public bool Visible = true;
            public bool Invalid;
            private static AnchorLayoutInput s_defaultInput = new AnchorLayoutInput();

            public Record(ILayoutNode layoutNode) => this.Initialize(layoutNode);

            public void Initialize(ILayoutNode layoutNode)
            {
                this.LayoutNode = layoutNode;
                this.Input = layoutNode.GetLayoutInput(AnchorLayout.InputData) as AnchorLayoutInput;
                if (this.Input == null)
                    this.Input = AnchorLayout.Record.s_defaultInput;
                this.ID = ((ViewItem)layoutNode).Name;
                this.Bounds = Rectangle.Zero;
                this.Phase = AnchorLayout.LayoutPhase.Untouched;
                this.Visible = true;
                this.Invalid = false;
            }

            public Record(string idName, Rectangle bounds, AnchorLayout.LayoutPhase phaseOverride)
            {
                this.ID = idName;
                this.Bounds = bounds;
                this.Phase = phaseOverride;
            }

            public override string ToString() => string.Format("AnchorLayout.Record({0}, Phase={1})", (object)this.ID, (object)this.Phase);
        }

        private class Packet
        {
            public ILayoutNode Subject;
            public Vector<AnchorLayout.Record> Records;
            public AnchorLayout.Record ParentRecord;
            public AnchorLayout.Record ParentActualRecord;
            public AnchorLayout.Record AreaOfInterestRecord;
            public Size Constraint;
            public Rectangle TotalBounds;
            public bool CircularitiesDetected;
            public AnchorLayout.Record CircularityBreakerRecord;

            public override string ToString() => string.Format("AnchorLayout.Packet({0})", (object)this.Subject);
        }
    }
}
