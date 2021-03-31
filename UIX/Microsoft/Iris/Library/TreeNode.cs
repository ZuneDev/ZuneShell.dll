// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Library.TreeNode
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using System;

namespace Microsoft.Iris.Library
{
    internal abstract class TreeNode : DisposableObject, ITreeNode
    {
        private static readonly DataCookie s_instanceIDProperty = DataCookie.ReserveSlot();
        private static readonly EventCookie s_deepParentChangeEvent = EventCookie.ReserveSlot();
        private TreeNode _nodeParent;
        private TreeNode _nodeFirstChild;
        private TreeNode _nodeNext;
        private TreeNode _nodePrevious;
        private UIZone _zone;
        private DynamicData _dataMap;

        public TreeNode()
        {
            this._dataMap = new DynamicData();
            this._dataMap.Create();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            this.ChangeParent(null);
            this.RemoveEventHandlers(s_deepParentChangeEvent);
        }

        public bool IsZoned => this._zone != null;

        public void ChangeParent(TreeNode nodeNewParent) => this.ChangeParent(nodeNewParent, null, LinkType.First);

        public void ChangeParent(TreeNode nodeNewParent, TreeNode nodeSibling, TreeNode.LinkType lt)
        {
            if (this._nodeParent == nodeNewParent)
                return;
            UIZone zone = null;
            TreeNode nodeParent = this._nodeParent;
            if (this._nodeParent != null)
            {
                DoUnlink(this);
                nodeParent.OnChildrenChanged();
            }
            if (nodeNewParent != null)
            {
                DoLink(nodeNewParent, this, nodeSibling, lt);
                zone = nodeNewParent.Zone;
                nodeNewParent.OnChildrenChanged();
            }
            this.PropagateZone(zone);
            this.FireTreeChangeWorker();
        }

        public void PropagateZone(UIZone zone)
        {
            if (this._zone == zone)
                return;
            if (this._zone != null)
                this.OnZoneDetached();
            this._zone = zone;
            if (zone != null)
                this.OnZoneAttached();
            foreach (TreeNode child in this.Children)
                child.PropagateZone(zone);
        }

        public void MoveNode(TreeNode nodeSibling, TreeNode.LinkType lt)
        {
            TreeNode nodeParent = this._nodeParent;
            DoUnlink(this);
            DoLink(nodeParent, this, nodeSibling, lt);
        }

        public void RemoveAllChildren(bool disposeChildrenFlag)
        {
            while (this._nodeFirstChild != null)
                this._nodeFirstChild.ChangeParent(null);
        }

        protected virtual void OnZoneAttached()
        {
        }

        protected virtual void OnZoneDetached()
        {
        }

        protected virtual void OnChildrenChanged()
        {
        }

        public event EventHandler DeepParentChange
        {
            add => this.AddEventHandler(s_deepParentChangeEvent, value);
            remove => this.RemoveEventHandler(s_deepParentChangeEvent, value);
        }

        public UIZone Zone => this._zone;

        UIZone ITreeNode.Zone => this._zone;

        public UISession UISession => this._zone.Session;

        public bool HasChildren => this._nodeFirstChild != null;

        public abstract bool IsRoot { get; }

        ITreeNode ITreeNode.Parent => _nodeParent;

        public TreeNode Parent => this._nodeParent;

        public TreeNode NextSibling => this._nodeNext;

        public TreeNode PreviousSibling => this._nodePrevious;

        public TreeNode FirstSibling => this._nodeParent == null ? this : this._nodeParent._nodeFirstChild;

        public TreeNode LastSibling
        {
            get
            {
                TreeNode treeNode = this;
                while (treeNode._nodeNext != null)
                    treeNode = treeNode._nodeNext;
                return treeNode;
            }
        }

        public TreeNode FirstChild => this._nodeFirstChild;

        public TreeNode LastChild => this._nodeFirstChild != null ? this._nodeFirstChild.LastSibling : null;

        public TreeNodeCollection Children => new TreeNodeCollection(this);

        public AncestorEnumerator Ancestors => new AncestorEnumerator(this);

        public bool HasDescendant(TreeNode nodeOther)
        {
            for (; nodeOther != null; nodeOther = nodeOther._nodeParent)
            {
                if (nodeOther == this)
                    return true;
            }
            return false;
        }

        public int ChildCount
        {
            get
            {
                int num = 0;
                for (TreeNode treeNode = this._nodeFirstChild; treeNode != null; treeNode = treeNode._nodeNext)
                    ++num;
                return num;
            }
        }

        private static void DoLink(
          TreeNode nodeParent,
          TreeNode nodeChange,
          TreeNode nodeSibling,
          TreeNode.LinkType lt)
        {
            nodeChange._nodeParent = nodeParent;
            TreeNode nodeFirstChild = nodeParent._nodeFirstChild;
            if (nodeFirstChild == null)
            {
                nodeParent._nodeFirstChild = nodeChange;
            }
            else
            {
                switch (lt)
                {
                    case LinkType.Before:
                        nodeChange._nodeNext = nodeSibling;
                        nodeChange._nodePrevious = nodeSibling._nodePrevious;
                        nodeSibling._nodePrevious = nodeChange;
                        if (nodeChange._nodePrevious != null)
                        {
                            nodeChange._nodePrevious._nodeNext = nodeChange;
                            break;
                        }
                        nodeParent._nodeFirstChild = nodeChange;
                        break;
                    case LinkType.Behind:
                        nodeChange._nodePrevious = nodeSibling;
                        nodeChange._nodeNext = nodeSibling._nodeNext;
                        nodeSibling._nodeNext = nodeChange;
                        if (nodeChange._nodeNext == null)
                            break;
                        nodeChange._nodeNext._nodePrevious = nodeChange;
                        break;
                    case LinkType.First:
                        nodeParent._nodeFirstChild = nodeChange;
                        nodeChange._nodeNext = nodeFirstChild;
                        if (nodeFirstChild == null)
                            break;
                        nodeFirstChild._nodePrevious = nodeChange;
                        break;
                    case LinkType.Last:
                        TreeNode lastSibling = nodeFirstChild.LastSibling;
                        lastSibling._nodeNext = nodeChange;
                        nodeChange._nodePrevious = lastSibling;
                        break;
                }
            }
        }

        private static void DoUnlink(TreeNode nodeChange)
        {
            if (nodeChange._nodeParent._nodeFirstChild == nodeChange)
                nodeChange._nodeParent._nodeFirstChild = nodeChange._nodeNext;
            if (nodeChange._nodeNext != null)
                nodeChange._nodeNext._nodePrevious = nodeChange._nodePrevious;
            if (nodeChange._nodePrevious != null)
                nodeChange._nodePrevious._nodeNext = nodeChange._nodeNext;
            nodeChange._nodeParent = null;
            nodeChange._nodeNext = null;
            nodeChange._nodePrevious = null;
        }

        private void FireTreeChangeWorker()
        {
            foreach (TreeNode child in this.Children)
                child.FireTreeChangeWorker();
            if (!(this.GetEventHandler(s_deepParentChangeEvent) is EventHandler eventHandler))
                return;
            eventHandler(this, EventArgs.Empty);
        }

        protected object GetData(DataCookie cookie) => this._dataMap.GetData(cookie);

        protected void SetData(DataCookie cookie, object value) => this._dataMap.SetData(cookie, value);

        protected Delegate GetEventHandler(EventCookie cookie) => this._dataMap.GetEventHandler(cookie);

        protected bool AddEventHandler(EventCookie cookie, Delegate handlerToAdd) => this._dataMap.AddEventHandler(cookie, handlerToAdd);

        protected bool RemoveEventHandler(EventCookie cookie, Delegate handlerToRemove) => this._dataMap.RemoveEventHandler(cookie, handlerToRemove);

        protected void RemoveEventHandlers(EventCookie cookie) => this._dataMap.RemoveEventHandlers(cookie);

        private static uint GetKey(EventCookie cookie) => EventCookie.ToUInt32(cookie);

        public enum LinkType
        {
            Before = 1,
            Behind = 2,
            First = 3,
            Last = 4,
        }
    }
}
