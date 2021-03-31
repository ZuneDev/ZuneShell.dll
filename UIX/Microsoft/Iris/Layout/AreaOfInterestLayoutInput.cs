// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Layout.AreaOfInterestLayoutInput
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Render;

namespace Microsoft.Iris.Layout
{
    internal class AreaOfInterestLayoutInput : ILayoutInput
    {
        private AreaOfInterestID _id;
        private Inset _margins;
        private static readonly DataCookie s_dataProperty = DataCookie.ReserveSlot();

        public AreaOfInterestLayoutInput(AreaOfInterestID idName, Inset margins)
        {
            this._id = idName;
            this._margins = margins;
        }

        public AreaOfInterestLayoutInput()
        {
        }

        public AreaOfInterestID Id
        {
            get => this._id;
            set => this._id = value;
        }

        public Inset Margins => this._margins;

        DataCookie ILayoutInput.Data => AreaOfInterestLayoutInput.Data;

        public static DataCookie Data => AreaOfInterestLayoutInput.s_dataProperty;

        public override string ToString() => InvariantString.Format("{0}({1})", this.GetType().Name, _id);
    }
}
