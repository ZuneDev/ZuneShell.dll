// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Layout.LayoutNodeEnumerator
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Layout
{
    internal struct LayoutNodeEnumerator
    {
        private ILayoutNode _start;
        private ILayoutNode _current;
        private bool _haventStartedYet;

        public LayoutNodeEnumerator GetEnumerator() => this;

        public LayoutNodeEnumerator(ILayoutNode start)
        {
            this._start = start;
            this._current = null;
            this._haventStartedYet = true;
        }

        public bool MoveNext()
        {
            if (this._current != null)
                this._current = this._current.NextVisibleSibling;
            else if (this._haventStartedYet)
            {
                this._current = this._start;
                this._haventStartedYet = false;
            }
            return this._current != null;
        }

        public ILayoutNode Current => this._current;

        public void Reset()
        {
            this._current = null;
            this._haventStartedYet = true;
        }
    }
}
