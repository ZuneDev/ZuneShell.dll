// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ImageInset
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Render;

namespace Microsoft.Iris
{
    public struct ImageInset
    {
        private int _left;
        private int _top;
        private int _right;
        private int _bottom;

        public ImageInset(int left, int top, int right, int bottom)
        {
            this._left = left;
            this._top = top;
            this._right = right;
            this._bottom = bottom;
        }

        public int Left => this._left;

        public int Top => this._top;

        public int Right => this._right;

        public int Bottom => this._bottom;

        internal Inset ToInset() => new Inset(this._left, this._top, this._right, this._bottom);
    }
}
