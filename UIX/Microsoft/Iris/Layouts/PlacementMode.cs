// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Layouts.PlacementMode
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Layouts
{
    internal class PlacementMode
    {
        private PopupPosition[] _popupPositions;
        private MouseTarget _mouseTarget;
        private bool _usesTargetSize;
        private static PlacementMode s_origin;
        private static PlacementMode s_left;
        private static PlacementMode s_right;
        private static PlacementMode s_top;
        private static PlacementMode s_bottom;
        private static PlacementMode s_center;
        private static PlacementMode s_mouseOrigin;
        private static PlacementMode s_mouseBottom;
        private static PlacementMode s_followMouseOrigin;
        private static PlacementMode s_followMouseBottom;

        public PopupPosition[] PopupPositions
        {
            get => this._popupPositions;
            set
            {
                this._popupPositions = value;
                this._usesTargetSize = false;
                if (this._popupPositions == null)
                    return;
                for (int index = 0; index < this._popupPositions.Length; ++index)
                {
                    if (this._popupPositions[index].Target != InterestPoint.TopLeft)
                    {
                        this._usesTargetSize = true;
                        break;
                    }
                }
            }
        }

        public MouseTarget MouseTarget
        {
            get => this._mouseTarget;
            set => this._mouseTarget = value;
        }

        internal bool UsesTargetSize => this._usesTargetSize;

        public override string ToString()
        {
            if (this == PlacementMode.s_origin)
                return "Origin";
            if (this == PlacementMode.s_bottom)
                return "Bottom";
            if (this == PlacementMode.s_top)
                return "Top";
            if (this == PlacementMode.s_left)
                return "Left";
            if (this == PlacementMode.s_right)
                return "Right";
            if (this == PlacementMode.s_center)
                return "Center";
            if (this == PlacementMode.s_mouseOrigin)
                return "MouseOrigin";
            if (this == PlacementMode.s_mouseBottom)
                return "MouseBottom";
            if (this == PlacementMode.s_followMouseOrigin)
                return "FollowMouseOrigin";
            return this == PlacementMode.s_followMouseBottom ? "FollowMouseBottom" : base.ToString();
        }

        public static PlacementMode Origin
        {
            get
            {
                if (PlacementMode.s_origin == null)
                {
                    PlacementMode.s_origin = new PlacementMode();
                    PlacementMode.s_origin.PopupPositions = new PopupPosition[1]
                    {
            new PopupPosition(InterestPoint.TopLeft, InterestPoint.TopLeft, FlipDirection.None)
                    };
                }
                return PlacementMode.s_origin;
            }
        }

        public static PlacementMode Bottom
        {
            get
            {
                if (PlacementMode.s_bottom == null)
                {
                    PlacementMode.s_bottom = new PlacementMode();
                    PlacementMode.s_bottom.PopupPositions = new PopupPosition[2]
                    {
            new PopupPosition(InterestPoint.BottomLeft, InterestPoint.TopLeft, FlipDirection.None),
            new PopupPosition(InterestPoint.TopLeft, InterestPoint.BottomLeft, FlipDirection.Vertical)
                    };
                }
                return PlacementMode.s_bottom;
            }
        }

        public static PlacementMode Center
        {
            get
            {
                if (PlacementMode.s_center == null)
                {
                    PlacementMode.s_center = new PlacementMode();
                    PlacementMode.s_center.PopupPositions = new PopupPosition[1]
                    {
            new PopupPosition(InterestPoint.Center, InterestPoint.Center, FlipDirection.None)
                    };
                }
                return PlacementMode.s_center;
            }
        }

        public static PlacementMode Right
        {
            get
            {
                if (PlacementMode.s_right == null)
                {
                    PlacementMode.s_right = new PlacementMode();
                    PlacementMode.s_right.PopupPositions = new PopupPosition[4]
                    {
            new PopupPosition(InterestPoint.TopRight, InterestPoint.TopLeft, FlipDirection.None),
            new PopupPosition(InterestPoint.BottomRight, InterestPoint.BottomLeft, FlipDirection.Vertical),
            new PopupPosition(InterestPoint.TopLeft, InterestPoint.TopRight, FlipDirection.Horizontal),
            new PopupPosition(InterestPoint.BottomLeft, InterestPoint.BottomRight, FlipDirection.Both)
                    };
                }
                return PlacementMode.s_right;
            }
        }

        public static PlacementMode Left
        {
            get
            {
                if (PlacementMode.s_left == null)
                {
                    PlacementMode.s_left = new PlacementMode();
                    PlacementMode.s_left.PopupPositions = new PopupPosition[4]
                    {
            new PopupPosition(InterestPoint.TopLeft, InterestPoint.TopRight, FlipDirection.None),
            new PopupPosition(InterestPoint.BottomLeft, InterestPoint.BottomRight, FlipDirection.Vertical),
            new PopupPosition(InterestPoint.TopRight, InterestPoint.TopLeft, FlipDirection.Horizontal),
            new PopupPosition(InterestPoint.BottomRight, InterestPoint.BottomLeft, FlipDirection.Both)
                    };
                }
                return PlacementMode.s_left;
            }
        }

        public static PlacementMode Top
        {
            get
            {
                if (PlacementMode.s_top == null)
                {
                    PlacementMode.s_top = new PlacementMode();
                    PlacementMode.s_top.PopupPositions = new PopupPosition[2]
                    {
            new PopupPosition(InterestPoint.TopLeft, InterestPoint.BottomLeft, FlipDirection.None),
            new PopupPosition(InterestPoint.BottomLeft, InterestPoint.TopLeft, FlipDirection.Vertical)
                    };
                }
                return PlacementMode.s_top;
            }
        }

        public static PlacementMode MouseOrigin
        {
            get
            {
                if (PlacementMode.s_mouseOrigin == null)
                {
                    PlacementMode.s_mouseOrigin = new PlacementMode();
                    PlacementMode.s_mouseOrigin.MouseTarget = MouseTarget.Fixed;
                    PlacementMode.s_mouseOrigin.PopupPositions = new PopupPosition[4]
                    {
            new PopupPosition(InterestPoint.TopLeft, InterestPoint.TopLeft, FlipDirection.None),
            new PopupPosition(InterestPoint.TopLeft, InterestPoint.TopRight, FlipDirection.Horizontal),
            new PopupPosition(InterestPoint.TopLeft, InterestPoint.BottomLeft, FlipDirection.Vertical),
            new PopupPosition(InterestPoint.TopLeft, InterestPoint.BottomRight, FlipDirection.Both)
                    };
                }
                return PlacementMode.s_mouseOrigin;
            }
        }

        public static PlacementMode MouseBottom
        {
            get
            {
                if (PlacementMode.s_mouseBottom == null)
                {
                    PlacementMode.s_mouseBottom = new PlacementMode();
                    PlacementMode.s_mouseBottom.MouseTarget = MouseTarget.Fixed;
                    PlacementMode.s_mouseBottom.PopupPositions = PlacementMode.Bottom.PopupPositions;
                }
                return PlacementMode.s_mouseBottom;
            }
        }

        public static PlacementMode FollowMouseOrigin
        {
            get
            {
                if (PlacementMode.s_followMouseOrigin == null)
                {
                    PlacementMode.s_followMouseOrigin = new PlacementMode();
                    PlacementMode.s_followMouseOrigin.MouseTarget = MouseTarget.Follow;
                    PlacementMode.s_followMouseOrigin.PopupPositions = PlacementMode.MouseOrigin.PopupPositions;
                }
                return PlacementMode.s_followMouseOrigin;
            }
        }

        public static PlacementMode FollowMouseBottom
        {
            get
            {
                if (PlacementMode.s_followMouseBottom == null)
                {
                    PlacementMode.s_followMouseBottom = new PlacementMode();
                    PlacementMode.s_followMouseBottom.MouseTarget = MouseTarget.Follow;
                    PlacementMode.s_followMouseBottom.PopupPositions = PlacementMode.MouseBottom.PopupPositions;
                }
                return PlacementMode.s_followMouseBottom;
            }
        }
    }
}
