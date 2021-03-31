// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Library.AncestorEnumerator
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System.Collections;

namespace Microsoft.Iris.Library
{
    internal struct AncestorEnumerator : IEnumerator, IEnumerable
    {
        private TreeNode _nodeStart;
        private TreeNode _nodeCurrent;
        private TreeNode _nodeNext;

        internal AncestorEnumerator(TreeNode nodeStart)
        {
            this._nodeStart = nodeStart;
            this._nodeCurrent = null;
            this._nodeNext = this._nodeStart;
        }

        object IEnumerator.Current => _nodeCurrent;

        public TreeNode Current => this._nodeCurrent;

        IEnumerator IEnumerable.GetEnumerator() => this;

        public AncestorEnumerator GetEnumerator() => this;

        public void Reset()
        {
            this._nodeCurrent = null;
            this._nodeNext = this._nodeStart;
        }

        public bool MoveNext()
        {
            this._nodeCurrent = this._nodeNext;
            if (this._nodeNext == null)
                return false;
            this._nodeNext = this._nodeNext.Parent;
            return true;
        }
    }
}
