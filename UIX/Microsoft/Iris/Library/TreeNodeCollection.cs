// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Library.TreeNodeCollection
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System;
using System.Collections;

namespace Microsoft.Iris.Library
{
    internal struct TreeNodeCollection : IList, ICollection, IEnumerable
    {
        private TreeNode _nodeSubject;

        internal TreeNodeCollection(TreeNode nodeSubject) => this._nodeSubject = nodeSubject;

        public int Count => this._nodeSubject.ChildCount;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => (object)null;

        bool IList.IsFixedSize => false;

        bool IList.IsReadOnly => false;

        object IList.this[int index]
        {
            get => this[index];
            set
            {
            }
        }

        public TreeNode this[int childIndex]
        {
            get
            {
                int num = 0;
                foreach (TreeNode treeNode in this)
                {
                    if (childIndex == num)
                        return treeNode;
                    ++num;
                }
                return null;
            }
            set
            {
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public TreeNodeEnumerator GetEnumerator() => new TreeNodeEnumerator(this._nodeSubject);

        void ICollection.CopyTo(Array destList, int destIndex)
        {
            foreach (TreeNode treeNode in this)
                destList.SetValue(treeNode, destIndex++);
        }

        public void CopyTo(TreeNode[] destList, int destIndex) => ((ICollection)this).CopyTo(destList, destIndex);

        int IList.Add(object value)
        {
            this.Add((TreeNode)value);
            return this._nodeSubject.ChildCount - 1;
        }

        public void Add(TreeNode nodeChild) => nodeChild.ChangeParent(this._nodeSubject, null, TreeNode.LinkType.Last);

        public void Clear() => this._nodeSubject.RemoveAllChildren(true);

        bool IList.Contains(object nodeChild) => this.Contains((TreeNode)nodeChild);

        public bool Contains(TreeNode nodeChild) => nodeChild.Parent == this._nodeSubject;

        int IList.IndexOf(object nodeChild) => this.IndexOf((TreeNode)nodeChild);

        public int IndexOf(TreeNode nodeChild)
        {
            if (nodeChild.Parent != this._nodeSubject)
                return -1;
            int num = 0;
            foreach (TreeNode treeNode in this)
            {
                if (nodeChild == treeNode)
                    return num;
                ++num;
            }
            return -1;
        }

        void IList.Insert(int insertAtIndex, object nodeChild) => this.Insert(insertAtIndex, (TreeNode)nodeChild);

        public void Insert(int insertAtIndex, TreeNode nodeChild)
        {
            TreeNode nodeSibling = null;
            TreeNode.LinkType lt = TreeNode.LinkType.Last;
            if (insertAtIndex < this.Count)
            {
                nodeSibling = this[insertAtIndex];
                lt = TreeNode.LinkType.Before;
            }
            nodeChild.ChangeParent(this._nodeSubject, nodeSibling, lt);
        }

        void IList.Remove(object nodeChild) => this.Remove((TreeNode)nodeChild);

        public void Remove(TreeNode nodeChild) => nodeChild.ChangeParent(null);

        public void RemoveAt(int removeAtIndex) => this[removeAtIndex].ChangeParent(null);
    }
}
