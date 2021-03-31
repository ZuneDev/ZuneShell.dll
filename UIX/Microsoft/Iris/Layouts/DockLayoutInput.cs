﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Layouts.DockLayoutInput
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layout;
using Microsoft.Iris.Library;

namespace Microsoft.Iris.Layouts
{
    internal class DockLayoutInput : ILayoutInput
    {
        public static readonly DockLayoutInput Left = new DockLayoutInput();
        public static readonly DockLayoutInput Top = new DockLayoutInput();
        public static readonly DockLayoutInput Right = new DockLayoutInput();
        public static readonly DockLayoutInput Bottom = new DockLayoutInput();
        public static readonly DockLayoutInput Client = new DockLayoutInput();

        private DockLayoutInput()
        {
        }

        DataCookie ILayoutInput.Data => DockLayoutInput.Data;

        internal static DataCookie Data => DockLayout.DockData;

        internal string PositionString
        {
            get
            {
                if (this == DockLayoutInput.Left)
                    return "Left";
                if (this == DockLayoutInput.Top)
                    return "Top";
                if (this == DockLayoutInput.Right)
                    return "Right";
                return this == DockLayoutInput.Bottom ? "Bottom" : "Client";
            }
        }

        public override string ToString() => InvariantString.Format("{0}(Position={1})", (object)this.GetType().Name, (object)this.PositionString);
    }
}