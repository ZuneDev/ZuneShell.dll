// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Library.TreeNodeEnumerator
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System.Collections;

namespace Microsoft.Iris.Library
{
    internal struct TreeNodeEnumerator : IEnumerator
    {
        private TreeNode _nodeParent;
        private TreeNode _nodeCurrent;
        private TreeNode _nodeNext;

        internal TreeNodeEnumerator(TreeNode nodeParent)
        {
            this._nodeParent = nodeParent;
            this._nodeCurrent = (TreeNode)null;
            this._nodeNext = nodeParent.FirstChild;
        }

        object IEnumerator.Current => (object)this._nodeCurrent;

        public TreeNode Current => this._nodeCurrent;

        public void Reset()
        {
            this._nodeCurrent = (TreeNode)null;
            this._nodeNext = this._nodeParent.FirstChild;
        }

        public bool MoveNext()
        {
            this._nodeCurrent = this._nodeNext;
            if (this._nodeNext == null)
                return false;
            this._nodeNext = this._nodeNext.NextSibling;
            return true;
        }
    }
}
