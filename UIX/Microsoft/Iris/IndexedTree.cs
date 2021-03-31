﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.IndexedTree
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System.Diagnostics;

namespace Microsoft.Iris
{
    internal class IndexedTree
    {
        private object _lockObj;
        private IndexedTree.TreeNode _root;
        private IndexedTree.TreeNode _poolHead;
        private int _lastSearchedIndex;

        public IndexedTree()
        {
            this._lastSearchedIndex = -1;
            this._lockObj = new object();
        }

        private void SetRoot(IndexedTree.TreeNode newValue)
        {
            if (this._root == newValue)
                return;
            this._root = newValue;
            this._lastSearchedIndex = -1;
        }

        private IndexedTree.TreeNode AcquireTreeNode()
        {
            IndexedTree.TreeNode treeNode = this._poolHead;
            if (this._poolHead != null)
            {
                this._poolHead = this._poolHead.parent;
                treeNode.parent = (IndexedTree.TreeNode)null;
            }
            else
                treeNode = new IndexedTree.TreeNode();
            return treeNode;
        }

        private void ReclaimTreeNode(IndexedTree.TreeNode node)
        {
            node.parent = this._poolHead;
            node.left = (IndexedTree.TreeNode)null;
            node.right = (IndexedTree.TreeNode)null;
            this._poolHead = node;
        }

        public void Store(int index, object data)
        {
            lock (this._lockObj)
            {
                IndexedTree.TreeNode newValue = this.Find(index);
                if (newValue == null)
                {
                    newValue = this.AcquireTreeNode();
                    newValue.delta = index;
                    newValue.parent = (IndexedTree.TreeNode)null;
                    if (this._root != null)
                    {
                        this._root.parent = newValue;
                        if (this._root.delta < index)
                        {
                            newValue.left = this._root;
                            if (this._root.right != null && this._root.right.delta + this._root.delta > index)
                            {
                                this._root.right.parent = newValue;
                                newValue.right = this._root.right;
                                newValue.right.delta = this._root.right.delta + this._root.delta - index;
                                this._root.right = (IndexedTree.TreeNode)null;
                            }
                        }
                        else
                        {
                            newValue.right = this._root;
                            if (this._root.left != null && this._root.left.delta + this._root.delta < index)
                            {
                                this._root.left.parent = newValue;
                                newValue.left = this._root.left;
                                newValue.left.delta = this._root.left.delta + this._root.delta - index;
                                this._root.left = (IndexedTree.TreeNode)null;
                            }
                        }
                        this._root.delta -= index;
                    }
                    this.SetRoot(newValue);
                }
                newValue.data = data;
            }
        }

        public void Remove(int index)
        {
            lock (this._lockObj)
            {
                IndexedTree.TreeNode node = this.Find(index);
                if (node == null)
                    return;
                IndexedTree.TreeNode treeNode;
                for (; node.left != null; node = treeNode)
                {
                    if (node.right == null)
                    {
                        IndexedTree.TreeNode left = node.left;
                        if (node == this._root)
                            this.SetRoot(left);
                        else if (node.IsRightBranch)
                            node.parent.right = left;
                        else
                            node.parent.left = left;
                        left.delta += node.delta;
                        left.parent = node.parent;
                        this.ReclaimTreeNode(node);
                        return;
                    }
                    treeNode = node.left;
                    int num1 = index + treeNode.delta;
                    while (treeNode.right != null)
                    {
                        treeNode = treeNode.right;
                        num1 += treeNode.delta;
                    }
                    node.data = treeNode.data;
                    node.delta = num1;
                    if (node == this._root)
                        this._lastSearchedIndex = -1;
                    int num2 = num1 - index;
                    node.left.delta -= num2;
                    node.right.delta -= num2;
                }
                IndexedTree.TreeNode right = node.right;
                if (node == this._root)
                    this.SetRoot(right);
                else if (node.IsRightBranch)
                    node.parent.right = right;
                else
                    node.parent.left = right;
                if (right != null)
                {
                    right.delta += node.delta;
                    right.parent = node.parent;
                }
                this.ReclaimTreeNode(node);
            }
        }

        public bool TryGetData(int index, out object data)
        {
            lock (this._lockObj)
            {
                IndexedTree.TreeNode treeNode = this.Find(index);
                data = treeNode == null ? (object)null : treeNode.data;
                return treeNode != null;
            }
        }

        public void InsertRange(int index, int count)
        {
            lock (this._lockObj)
            {
                this.Find(index);
                if (this._root == null)
                    return;
                IndexedTree.TreeNode node = this._root;
                if (this._root.delta < index)
                    node = this._root.right;
                this.ChangeIndex(node, count);
            }
        }

        private void ChangeIndex(IndexedTree.TreeNode node, int amount)
        {
            if (node == null)
                return;
            node.delta += amount;
            if (node.left == null)
                return;
            node.left.delta -= amount;
        }

        public void Insert(int index, bool setValue, object data)
        {
            lock (this._lockObj)
            {
                this.InsertRange(index, 1);
                if (!setValue)
                    return;
                this.Store(index, data);
            }
        }

        public void RemoveIndex(int index)
        {
            lock (this._lockObj)
            {
                IndexedTree.TreeNode treeNode = this.Find(index);
                if (treeNode != null)
                {
                    if (treeNode.right != null)
                        --treeNode.right.delta;
                    this.Remove(index);
                }
                else
                {
                    if (this._root == null)
                        return;
                    IndexedTree.TreeNode node = this._root;
                    if (this._root.delta < index)
                        node = this._root.right;
                    this.ChangeIndex(node, -1);
                }
            }
        }

        public void Clear()
        {
            lock (this._lockObj)
                this.SetRoot((IndexedTree.TreeNode)null);
        }

        public bool Contains(int index)
        {
            lock (this._lockObj)
                return this.Find(index) != null;
        }

        public object this[int index]
        {
            get
            {
                lock (this._lockObj)
                {
                    object data;
                    this.TryGetData(index, out data);
                    return data;
                }
            }
            set
            {
                lock (this._lockObj)
                    this.Store(index, value);
            }
        }

        private IndexedTree.TreeNode Find(int index)
        {
            if (this._lastSearchedIndex == index && this._root != null)
                return this._root.delta != index ? (IndexedTree.TreeNode)null : this._root;
            this._lastSearchedIndex = index;
            IndexedTree.TreeNode treeNode = this._root;
            IndexedTree.TreeNode node = (IndexedTree.TreeNode)null;
            int num = 0;
            bool flag = false;
            while (!flag && treeNode != null)
            {
                num += treeNode.delta;
                node = treeNode;
                if (num == index)
                    flag = true;
                else
                    treeNode = num <= index ? treeNode.right : treeNode.left;
            }
            if (node != null)
                this.Splay(node);
            this._lastSearchedIndex = index;
            return !flag ? (IndexedTree.TreeNode)null : treeNode;
        }

        private void Splay(IndexedTree.TreeNode node)
        {
            for (IndexedTree.TreeNode parent = node.parent; parent != null; parent = node.parent)
            {
                if (parent.parent == null)
                    this.Zig(node);
                else if (node.IsLeftBranch == parent.IsLeftBranch)
                    this.ZigZig(node, parent);
                else
                    this.ZigZag(node);
            }
            this.SetRoot(node);
        }

        private void Zig(IndexedTree.TreeNode item) => this.Rotate(item);

        private void ZigZig(IndexedTree.TreeNode item, IndexedTree.TreeNode parent)
        {
            this.Rotate(parent);
            this.Rotate(item);
        }

        private void ZigZag(IndexedTree.TreeNode item)
        {
            this.Rotate(item);
            this.Rotate(item);
        }

        private void Rotate(IndexedTree.TreeNode child)
        {
            IndexedTree.TreeNode parent = child.parent;
            int num1 = -child.delta;
            int num2 = child.delta + parent.delta;
            IndexedTree.TreeNode treeNode;
            if (child.IsLeftBranch)
            {
                treeNode = child.right;
                parent.left = treeNode;
                child.parent = parent.parent;
                child.right = parent;
            }
            else
            {
                treeNode = child.left;
                parent.right = treeNode;
                child.parent = parent.parent;
                child.left = parent;
            }
            if (treeNode != null)
            {
                treeNode.parent = parent;
                treeNode.delta += child.delta;
            }
            if (child.parent != null)
            {
                if (num2 < 0)
                    child.parent.left = child;
                else
                    child.parent.right = child;
            }
            child.delta = num2;
            parent.delta = num1;
            parent.parent = child;
        }

        public IndexedTree.TreeNode Root => this._root;

        public IndexedTree.TreeEnumerator GetEnumerator() => new IndexedTree.TreeEnumerator(this);

        [Conditional("DEBUG")]
        private void DEBUG_BumpGeneration()
        {
        }

        public class TreeNode
        {
            public int delta;
            public IndexedTree.TreeNode left;
            public IndexedTree.TreeNode right;
            public IndexedTree.TreeNode parent;
            public object data;

            public bool IsLeftBranch => this.delta < 0;

            public bool IsRightBranch => this.delta > 0;
        }

        public struct TreeEntry
        {
            private int _index;
            private object _data;

            public TreeEntry(int index, object data)
            {
                this._index = index;
                this._data = data;
            }

            public int Index => this._index;

            public object Value => this._data;
        }

        public struct TreeEnumerator
        {
            private IndexedTree _tree;
            private bool _started;
            private IndexedTree.TreeNode _current;
            private int _index;

            public TreeEnumerator(IndexedTree tree)
            {
                this._tree = tree;
                this._current = this._tree._root;
                this._started = false;
                this._index = this._current != null ? this._current.delta : -1;
            }

            [Conditional("DEBUG")]
            private void DEBUG_CheckGeneration()
            {
            }

            private void MoveToLeftmostChild()
            {
                if (this._current == null)
                    return;
                while (this._current.left != null)
                {
                    this._current = this._current.left;
                    this._index += this._current.delta;
                }
            }

            public bool MoveNext()
            {
                lock (this._tree._lockObj)
                {
                    if (!this._started)
                    {
                        this._started = true;
                        this.MoveToLeftmostChild();
                    }
                    else if (this._current.right != null)
                    {
                        this._current = this._current.right;
                        this._index += this._current.delta;
                        this.MoveToLeftmostChild();
                    }
                    else if (this._current.IsLeftBranch)
                    {
                        this._index -= this._current.delta;
                        this._current = this._current.parent;
                    }
                    else
                    {
                        for (; this._current != null && this._current.IsRightBranch; this._current = this._current.parent)
                            this._index -= this._current.delta;
                        if (this._current != null)
                        {
                            this._index -= this._current.delta;
                            this._current = this._current.parent;
                        }
                    }
                    return this._current != null;
                }
            }

            public IndexedTree.TreeEntry Current => new IndexedTree.TreeEntry(this._index, this._current.data);
        }
    }
}