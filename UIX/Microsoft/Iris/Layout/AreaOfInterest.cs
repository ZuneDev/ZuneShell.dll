// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Layout.AreaOfInterest
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Render;

namespace Microsoft.Iris.Layout
{
    internal struct AreaOfInterest
    {
        private Rectangle _rectangle;
        private Rectangle _displayRectangle;
        private AreaOfInterestID _id;

        public AreaOfInterest(Size size, AreaOfInterestID id)
          : this(size, Inset.Zero, id)
        {
        }

        public AreaOfInterest(Size size, Inset margins, AreaOfInterestID id)
          : this(new Rectangle(Point.Zero, size), margins, id)
        {
        }

        public AreaOfInterest(Rectangle rectangle, AreaOfInterestID id)
          : this(rectangle, rectangle, id)
        {
        }

        public AreaOfInterest(Rectangle rectangle, Inset margins, AreaOfInterestID id)
          : this(rectangle, Rectangle.Inflate(rectangle, margins.ToRect), id)
        {
        }

        public AreaOfInterest(Rectangle rectangle, Rectangle displayRectangle, AreaOfInterestID id)
        {
            this._rectangle = rectangle;
            this._displayRectangle = displayRectangle;
            this._id = id;
        }

        public AreaOfInterest Transform(Point offset) => new AreaOfInterest(Rectangle.Offset(this._rectangle, offset), Rectangle.Offset(this._displayRectangle, offset), this.Id);

        public static void AddAreaOfInterest(
          AreaOfInterest interest,
          ref Vector<AreaOfInterest> areasOfInterestList)
        {
            if (areasOfInterestList == null)
                areasOfInterestList = new Vector<AreaOfInterest>();
            for (int index = 0; index < areasOfInterestList.Count; ++index)
            {
                if (areasOfInterestList[index].Id == interest.Id)
                    return;
            }
            areasOfInterestList.Add(interest);
        }

        public override string ToString() => InvariantString.Format("AreaOfInterest(\"{0}\", {1}, {2})", Id, Rectangle, DisplayRectangle);

        public override int GetHashCode() => this._rectangle.GetHashCode() ^ this._displayRectangle.GetHashCode() ^ this._id.GetHashCode();

        public override bool Equals(object other)
        {
            try
            {
                return this == (AreaOfInterest)other;
            }
            catch
            {
                return false;
            }
        }

        public bool Equals(AreaOfInterest other) => this._rectangle == other._rectangle && this._displayRectangle == other._displayRectangle && this._id == other._id;

        public static bool operator ==(AreaOfInterest a, AreaOfInterest b) => a._rectangle == b._rectangle && a._displayRectangle == b._displayRectangle && a._id == b._id;

        public static bool operator !=(AreaOfInterest a, AreaOfInterest b) => a._rectangle != b._rectangle || a._displayRectangle != b._displayRectangle || a._id != b._id;

        public Rectangle Rectangle => this._rectangle;

        public Rectangle DisplayRectangle
        {
            get => this._displayRectangle;
            set => this._displayRectangle = value;
        }

        public AreaOfInterestID Id => this._id;
    }
}
