// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Layouts.PopupLayoutInput
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layout;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.Render;
using Microsoft.Iris.RenderAPI.Drawing;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Layouts
{
    internal class PopupLayoutInput : NotifyObjectBase, ILayoutInput
    {
        private ViewItem _target;
        private PlacementMode _placementMode;
        private Point _offset;
        private bool _stayInBounds;
        private bool _respectMenuDropAlignment;
        private bool _constrainToTarget;
        private RectangleF _mouseRect = RectangleF.Zero;
        private bool _flippedHorizontally;
        private bool _flippedVertically;
        private static PopupLayoutInput s_default;

        public PopupLayoutInput()
        {
            this._stayInBounds = true;
            this._respectMenuDropAlignment = true;
            this.Placement = PlacementMode.Origin;
        }

        public ViewItem PlacementTarget
        {
            get => this._target;
            set => this._target = value;
        }

        public PlacementMode Placement
        {
            get => this._placementMode;
            set => this._placementMode = value;
        }

        public Point Offset
        {
            get => this._offset;
            set => this._offset = value;
        }

        public bool StayInBounds
        {
            get => this._stayInBounds;
            set => this._stayInBounds = value;
        }

        public bool RespectMenuDropAlignment
        {
            get => this._respectMenuDropAlignment;
            set => this._respectMenuDropAlignment = value;
        }

        public bool ConstrainToTarget
        {
            get => this._constrainToTarget;
            set => this._constrainToTarget = value;
        }

        DataCookie ILayoutInput.Data => PopupLayoutInput.Data;

        internal static DataCookie Data => PopupLayout.DataCookie;

        internal RectangleF MouseRect
        {
            get => this._mouseRect;
            set => this._mouseRect = value;
        }

        public bool FlippedHorizontally
        {
            get => this._flippedHorizontally;
            set
            {
                if (this._flippedHorizontally == value)
                    return;
                this._flippedHorizontally = value;
                this.FireNotification(NotificationID.FlippedHorizontally);
            }
        }

        public bool FlippedVertically
        {
            get => this._flippedVertically;
            set
            {
                if (this._flippedVertically == value)
                    return;
                this._flippedVertically = value;
                this.FireNotification(NotificationID.FlippedVertically);
            }
        }

        internal bool TargetIsMouse => this._placementMode != null && this._placementMode.MouseTarget == MouseTarget.Fixed;

        internal bool TargetIsFollowMouse => this._placementMode != null && this._placementMode.MouseTarget == MouseTarget.Follow;

        internal static PopupLayoutInput Default
        {
            get
            {
                if (PopupLayoutInput.s_default == null)
                    PopupLayoutInput.s_default = new PopupLayoutInput();
                return PopupLayoutInput.s_default;
            }
        }
    }
}
