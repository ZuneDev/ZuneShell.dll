// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ViewItems.Repeater
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Debug;
using Microsoft.Iris.Input;
using Microsoft.Iris.Layout;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.Navigation;
using Microsoft.Iris.RenderAPI.Drawing;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using System.Collections;
using System.Diagnostics;

namespace Microsoft.Iris.ViewItems
{
    internal class Repeater : ViewItem
    {
        internal const int c_defaultItemFocusOrderValue = 2147483646;
        private IList _source;
        private Vector<Repeater.RepeatedViewItemSet> _repeatedViewItems;
        private Vector<int> _outstandingDataIndexRequests;
        private int? _pendingIndexRequest;
        private Repeater.PendingIndexRequestType _pendingIndexRequestType;
        private int _sourceGeneration;
        private int _itemsCount;
        private string _contentName;
        private string _dividerName;
        private Repeater.ContentTypeHandler _ctproc;
        private bool _haveRequestedUpdate;
        private bool _ignoreCustomRepeaters;
        private bool _discardOffscreenVisuals;
        private bool _maintainFocusScreenLocation;
        private bool _focusNeedsRepairing;
        private int _defaultFocusIndex;
        private ViewItem _lastKeyFocusedItem;
        private ViewItem _lastMouseFocusedItem;
        private RepeaterContentSelector _contentSelector;
        private LayoutCompleteEventHandler _repeatedItemLayoutComplete;
        private static ChildFaultedInDelegate s_scrollIndexIntoViewHandler = new ChildFaultedInDelegate(Repeater.ScrollIndexIntoViewItemFaultedIn);
        private static ChildFaultedInDelegate s_navigateIntoIndexHandler = new ChildFaultedInDelegate(Repeater.NavigateIntoIndexItemFaultedIn);
        private static string[] s_repeatedItemParameters = new string[2]
        {
      "RepeatedItem",
      "RepeatedItemIndex"
        };
        private static string c_childIDSentinel = "Repeater child#";
        private static DeferredHandler s_listContentsChangedHandler = new DeferredHandler(Repeater.AsyncListContentsChangedHandler);
        private static object s_unavailableObject = new object();

        protected override void OnDispose()
        {
            if (this._source is INotifyList sourceA)
                sourceA.ContentsChanged -= new UIListContentsChangedHandler(this.QueueListContentsChanged);
            if (this._source is IVirtualList sourceB)
                sourceB.RepeaterHost = null;
            this._source = null;
            base.OnDispose();
        }

        protected override void OnZoneAttached()
        {
            base.OnZoneAttached();
            this.UI.DescendentKeyFocusChange += new InputEventHandler(this.OnDescendentKeyFocusChange);
            this.UI.DescendentMouseFocusChange += new InputEventHandler(this.OnDescendentMouseFocusChange);
        }

        protected override void OnZoneDetached()
        {
            base.OnZoneDetached();
            this.UI.DescendentKeyFocusChange -= new InputEventHandler(this.OnDescendentKeyFocusChange);
            this.UI.DescendentMouseFocusChange -= new InputEventHandler(this.OnDescendentMouseFocusChange);
        }

        public string ContentName
        {
            get => this._contentName;
            set
            {
                if (!(this._contentName != value))
                    return;
                this._contentName = value;
                this.RebuildChildren();
                this.FireNotification(NotificationID.ContentName);
            }
        }

        public string DividerName
        {
            get => this._dividerName;
            set
            {
                if (!(this._dividerName != value))
                    return;
                this._dividerName = value;
                this.RebuildChildren();
                this.FireNotification(NotificationID.DividerName);
            }
        }

        public IList Source
        {
            get => this._source;
            set
            {
                if (this._source == value)
                    return;
                if (this._source != null)
                {
                    if (this._source is INotifyList sourceA)
                        sourceA.ContentsChanged -= new UIListContentsChangedHandler(this.QueueListContentsChanged);
                    if (this._source is IVirtualList sourceB)
                        sourceB.RepeaterHost = null;
                }
                this._source = value;
                if (value != null)
                {
                    if (value is INotifyList notifyList)
                        notifyList.ContentsChanged += new UIListContentsChangedHandler(this.QueueListContentsChanged);
                    if (this._source is IVirtualList source)
                        source.RepeaterHost = this;
                }
                this.RebuildChildren();
                this.FireNotification(NotificationID.Source);
            }
        }

        public IList ContentSelectors
        {
            get
            {
                if (this._contentSelector == null)
                {
                    this._contentSelector = new RepeaterContentSelector(this);
                    this.PreRepeatHandler = this._contentSelector.ContentTypeHandler;
                }
                return this._contentSelector.Selectors;
            }
        }

        public Repeater.ContentTypeHandler PreRepeatHandler
        {
            get => this._ctproc;
            set => this._ctproc = value;
        }

        public bool IgnoreCustomRepeaters
        {
            get => this._ignoreCustomRepeaters;
            set => this._ignoreCustomRepeaters = value;
        }

        public override bool DiscardOffscreenVisuals
        {
            get => this._discardOffscreenVisuals;
            set
            {
                if (this._discardOffscreenVisuals == value)
                    return;
                if (this._repeatedItemLayoutComplete == null && this.HasChildren)
                    this._repeatedItemLayoutComplete = new LayoutCompleteEventHandler(this.OnRepeatedItemLayoutComplete);
                if (this._discardOffscreenVisuals)
                {
                    foreach (ViewItem child in this.Children)
                        child.LayoutComplete -= this._repeatedItemLayoutComplete;
                }
                this._discardOffscreenVisuals = value;
                if (this._discardOffscreenVisuals)
                {
                    foreach (ViewItem child in this.Children)
                        child.LayoutComplete += this._repeatedItemLayoutComplete;
                }
                this.FireNotification(NotificationID.DiscardOffscreenVisuals);
            }
        }

        public bool MaintainFocusedItemOnSourceChanges
        {
            get => !this._maintainFocusScreenLocation;
            set
            {
                if (this._maintainFocusScreenLocation == value)
                    return;
                this._maintainFocusScreenLocation = value;
                this.FireNotification(NotificationID.MaintainFocusedItemOnSourceChanges);
            }
        }

        internal bool GetFocusedIndex(ref int index)
        {
            bool flag = false;
            if (this.GetExtendedLayoutOutput(VisibleIndexRangeLayoutOutput.DataCookie) is VisibleIndexRangeLayoutOutput extendedLayoutOutput && extendedLayoutOutput.FocusedItem.HasValue)
            {
                index = extendedLayoutOutput.FocusedItem.Value;
                flag = true;
            }
            return flag;
        }

        public override bool HideNamedChildren => true;

        public void ForceRefresh() => this.RebuildChildren();

        private int InternalCount
        {
            get => this._itemsCount;
            set
            {
                if (this._itemsCount == value)
                    return;
                this._itemsCount = value;
                this.SetLayoutInput(new CountLayoutInput(this._itemsCount));
            }
        }

        private void RebuildChildren()
        {
            if (this.HasVisual)
                this.UI.DestroyVisualTree(this, true);
            if (this._repeatedViewItems != null)
            {
                UIClass keyFocusDescendant = this.UI.KeyFocusDescendant;
                foreach (Repeater.RepeatedViewItemSet repeatedViewItem in this._repeatedViewItems)
                    this.DisposeRepeatedViewItemSet(repeatedViewItem, ref keyFocusDescendant);
            }
            this._repeatedViewItems = new Vector<Repeater.RepeatedViewItemSet>();
            this._outstandingDataIndexRequests = new Vector<int>();
            this._pendingIndexRequest = new int?();
            this._lastMouseFocusedItem = null;
            this._lastKeyFocusedItem = null;
            int num = 0;
            if (this._source != null)
                num = this._source.Count;
            this.InternalCount = num;
            ++this._sourceGeneration;
            this.RequestRepeatOfIndexUpdate();
        }

        public override void OnCommit()
        {
            if (this.LayoutRequestedCount <= 0 && this.LayoutRequestedIndices == null || this.LayoutRequestedCount > 0 && this.LayoutRequestedIndices == null && (this._repeatedViewItems != null && this.InternalCount == this._repeatedViewItems.Count))
                return;
            this.RequestRepeatOfIndexUpdate();
        }

        public void UpdateBinding()
        {
            if (this._source == null)
                return;
            if (this._itemsCount != this._source.Count)
            {
                this.RebuildChildren();
            }
            else
            {
                if (Microsoft.Iris.Debug.Trace.IsCategoryEnabled(TraceCategory.Repeating, 5))
                {
                    int num = this._pendingIndexRequest.HasValue ? 1 : 0;
                }
                Vector<int> vector = this.GetLayoutRepeatRequests();
                if (this._pendingIndexRequest.HasValue)
                {
                    if (vector == null)
                        vector = new Vector<int>();
                    vector.Add(this._pendingIndexRequest.Value);
                }
                if (vector == null)
                    return;
                foreach (int virtualIndex in vector)
                {
                    int dataIndex;
                    int generationValue;
                    object dataItemObject;
                    if (this.GetDataItem(virtualIndex, out dataIndex, out generationValue, out dataItemObject))
                    {
                        this.RepeatItem(virtualIndex, dataIndex, generationValue, dataItemObject);
                        if (this._pendingIndexRequest.HasValue && this._pendingIndexRequest.Value == virtualIndex)
                            this._pendingIndexRequest = new int?();
                    }
                }
            }
        }

        private Vector<int> GetLayoutRepeatRequests()
        {
            if (this.LayoutRequestedCount == 0 && this.LayoutRequestedIndices == null)
                return null;
            int layoutRequestedCount = this.LayoutRequestedCount;
            Vector<int> requestedIndices = this.LayoutRequestedIndices;
            Vector<int> indicesToRequest = null;
            if (layoutRequestedCount > 0)
                indicesToRequest = this.GetMissingIndices(layoutRequestedCount);
            if (requestedIndices != null)
            {
                foreach (int requestIndex in requestedIndices)
                    this.RequestRepeatOfIndex(ref indicesToRequest, requestIndex);
            }
            return indicesToRequest;
        }

        private Vector<int> GetMissingIndices(int howManyMoreCount)
        {
            Vector<int> indicesToRequest = null;
            int dataStartIndex = 0;
            if (!ListUtility.IsNullOrEmpty(_repeatedViewItems))
            {
                foreach (Repeater.RepeatedViewItemSet repeatedViewItem in this._repeatedViewItems)
                {
                    int dataIndex = repeatedViewItem.DataIndex;
                    if (dataIndex > dataStartIndex)
                        this.RequestRepeatOfIndexRange(ref indicesToRequest, dataStartIndex, dataIndex, ref howManyMoreCount);
                    else if (dataIndex < dataStartIndex)
                    {
                        this.RequestRepeatOfIndexRange(ref indicesToRequest, dataStartIndex, this._itemsCount, ref howManyMoreCount);
                        this.RequestRepeatOfIndexRange(ref indicesToRequest, 0, dataIndex, ref howManyMoreCount);
                    }
                    dataStartIndex = dataIndex + 1;
                    if (howManyMoreCount <= 0 || dataStartIndex >= this._itemsCount)
                        break;
                }
            }
            if (howManyMoreCount > 0 && dataStartIndex < this._itemsCount)
                this.RequestRepeatOfIndexRange(ref indicesToRequest, dataStartIndex, this._itemsCount, ref howManyMoreCount);
            return indicesToRequest;
        }

        private void RequestRepeatOfIndexRange(
          ref Vector<int> indicesToRequest,
          int dataStartIndex,
          int dataEndIndex,
          ref int howManyMoreCount)
        {
            if (howManyMoreCount <= 0)
                return;
            for (int requestIndex = dataStartIndex; requestIndex < dataEndIndex; ++requestIndex)
            {
                if (this.RequestRepeatOfIndex(ref indicesToRequest, requestIndex))
                    --howManyMoreCount;
                if (howManyMoreCount <= 0)
                    break;
            }
        }

        private bool RequestRepeatOfIndex(ref Vector<int> indicesToRequest, int requestIndex)
        {
            if (indicesToRequest != null && indicesToRequest.Contains(requestIndex) || this.HasVirtualIndexBeenRepeated(requestIndex))
                return false;
            if (indicesToRequest == null)
                indicesToRequest = new Vector<int>();
            indicesToRequest.Add(requestIndex);
            return true;
        }

        private bool GetDataItem(
          int virtualIndex,
          out int dataIndex,
          out int generationValue,
          out object dataItemObject)
        {
            bool flag = false;
            ListUtility.GetWrappedIndex(virtualIndex, this._source.Count, out dataIndex, out generationValue);
            if (!ListUtility.IsValidIndex(this._source, dataIndex))
                dataItemObject = null;
            else if (this._source is IVirtualList source && !source.IsItemAvailable(dataIndex))
            {
                if (!this._outstandingDataIndexRequests.Contains(dataIndex))
                {
                    this._outstandingDataIndexRequests.Add(dataIndex);
                    source.RequestItem(dataIndex, this.QueryHandler);
                }
                dataItemObject = null;
            }
            else
            {
                dataItemObject = this._source[dataIndex];
                flag = true;
            }
            return flag;
        }

        private void RepeatItem(
          int virtualIndex,
          int dataIndex,
          int generationValue,
          object dataItemObject)
        {
            if (dataItemObject == Repeater.UnavailableItem)
                return;
            bool flag = false;
            IVirtualList source = null;
            if (_source is IVirtualList)
            {
                source = (IVirtualList)_source;
                flag = !this.HasDataIndexBeenRepeated(dataIndex);
            }
            Index index = new Index(virtualIndex, dataIndex, this);
            ErrorManager.EnterContext(UI.TypeSchema);
            try
            {
                ViewItem repeatedItem;
                ViewItem dividerItem;
                this.CreateRepeatedItem(index, dataItemObject, out repeatedItem, out dividerItem);
                int repeatedItemIndex;
                int viewItemIndex;
                this.GetInsertIndex(index.Value, out repeatedItemIndex, out viewItemIndex);
                if (repeatedItem != null)
                    this.Children.Insert(viewItemIndex, repeatedItem);
                if (dividerItem != null)
                    this.Children.Insert(viewItemIndex, dividerItem);
                if (repeatedItem == null)
                    return;
                this._repeatedViewItems.Insert(repeatedItemIndex, new Repeater.RepeatedViewItemSet(index, generationValue, repeatedItem, dividerItem));
                if (flag)
                    source.NotifyVisualsCreated(dataIndex);
                if (virtualIndex == this._defaultFocusIndex)
                    this.SetAsDefaultFocusRecipient(repeatedItem);
                if (!this._discardOffscreenVisuals)
                    return;
                if (this._repeatedItemLayoutComplete == null)
                    this._repeatedItemLayoutComplete = new LayoutCompleteEventHandler(this.OnRepeatedItemLayoutComplete);
                repeatedItem.LayoutComplete += this._repeatedItemLayoutComplete;
            }
            finally
            {
                ErrorManager.ExitContext();
            }
        }

        private void CreateRepeatedItem(
          Index index,
          object dataItemObject,
          out ViewItem repeatedItem,
          out ViewItem dividerItem)
        {
            ParameterContext parameterContext = new ParameterContext(Repeater.s_repeatedItemParameters, new object[2]
            {
        dataItemObject,
         index
            });
            string contentTypeName = null;
            this.GetContentTypeForRepeatedItem(dataItemObject, out contentTypeName);
            repeatedItem = null;
            if (!string.IsNullOrEmpty(contentTypeName))
            {
                repeatedItem = this.UI.ConstructNamedContent(contentTypeName, parameterContext);
                if (repeatedItem == null)
                {
                    if (contentTypeName[0] == '#')
                        ErrorManager.ReportError("Repeater unable to create inline content");
                    else
                        ErrorManager.ReportError("Repeater failed to find content to repeat. ContentName was '{0}'", contentTypeName);
                }
            }
            else
                ErrorManager.ReportError("Repeater has no content to repeat");
            dividerItem = null;
            if (index.SourceValue != 0 && this._dividerName != null)
            {
                dividerItem = this.UI.ConstructNamedContent(this._dividerName, parameterContext);
                if (dividerItem == null)
                    ErrorManager.ReportError("Repeater failed to find divider content to repeat (DividerName was '{0}')", _dividerName);
            }
            if (repeatedItem != null)
                repeatedItem.SetLayoutInput(new IndexLayoutInput(index, IndexType.Content));
            if (dividerItem == null)
                return;
            dividerItem.SetLayoutInput(new IndexLayoutInput(index, IndexType.Divider));
        }

        private void RequestRepeatOfIndexUpdate()
        {
            if (this._haveRequestedUpdate)
                return;
            DeferredCall.Post(DispatchPriority.RepeatItem, new SimpleCallback(this.UpdateBindingCallback));
            this._haveRequestedUpdate = true;
        }

        private void UpdateBindingCallback()
        {
            this._haveRequestedUpdate = false;
            this.UpdateBinding();
        }

        private void ItemRequestCallback(object senderObject, int dataIndex, object dataItemObject)
        {
            IVirtualList virtualList = (IVirtualList)senderObject;
            if (this.IsDisposed || virtualList != this._source || !this._outstandingDataIndexRequests.Contains(dataIndex))
                return;
            this._outstandingDataIndexRequests.Remove(dataIndex);
            this.RequestRepeatOfIndexUpdate();
        }

        private void OnDescendentMouseFocusChange(UIClass sender, InputInfo inputInfo)
        {
            MouseFocusInfo mouseFocusInfo = (MouseFocusInfo)inputInfo;
            ViewItem childFromDescendant = this.GetDirectChildFromDescendant(mouseFocusInfo.State ? mouseFocusInfo.Target as UIClass : null);
            if (childFromDescendant == this._lastMouseFocusedItem)
                return;
            this.UpdateKeepAliveState(ref this._lastMouseFocusedItem, childFromDescendant, this._lastKeyFocusedItem);
        }

        private void OnDescendentKeyFocusChange(UIClass sender, InputInfo inputInfo)
        {
            KeyFocusInfo keyFocusInfo = (KeyFocusInfo)inputInfo;
            if (!keyFocusInfo.State)
                return;
            ViewItem childFromDescendant = this.GetDirectChildFromDescendant(keyFocusInfo.Target as UIClass);
            if (childFromDescendant == null || childFromDescendant == this._lastKeyFocusedItem)
                return;
            this.UpdateKeepAliveState(ref this._lastKeyFocusedItem, childFromDescendant, this._lastMouseFocusedItem);
        }

        private void UpdateKeepAliveState(
          ref ViewItem currentItem,
          ViewItem newItem,
          ViewItem otherItem)
        {
            if (currentItem != null && currentItem != otherItem && !currentItem.IsDisposed)
                currentItem.UnlockVisible();
            currentItem = newItem;
            if (currentItem == null)
                return;
            currentItem.LockVisible();
        }

        private ViewItem GetDirectChildFromDescendant(UIClass ui)
        {
            ViewItem viewItem = null;
            if (ui != null)
            {
                viewItem = ui.RootItem;
                while (viewItem != null && viewItem.Parent != this)
                    viewItem = viewItem.Parent;
            }
            return viewItem;
        }

        private void OnRepeatedItemLayoutComplete(object sender)
        {
            ViewItem viewItem = (ViewItem)sender;
            if (viewItem.HasVisual || viewItem.LayoutVisible)
                return;
            int virtualIndex = ((IndexLayoutInput)viewItem.GetLayoutInput(IndexLayoutInput.Data)).Index.Value;
            if (!viewItem.IsOffscreen)
                return;
            Repeater.RepeatedViewItemSet repeatedInstance = this.GetRepeatedInstance(virtualIndex);
            DeferredCall.Post(DispatchPriority.Housekeeping, new DeferredHandler(this.DeferredDisposeViewItem), repeatedInstance);
            this._repeatedViewItems.Remove(repeatedInstance);
            viewItem.LayoutComplete -= this._repeatedItemLayoutComplete;
            int dataIndex = repeatedInstance.DataIndex;
            if (this.HasDataIndexBeenRepeated(dataIndex) || !(this._source is IVirtualList source))
                return;
            source.NotifyVisualsReleased(dataIndex);
        }

        private void DeferredDisposeViewItem(object arg) => this.DisposeRepeatedViewItemSet((Repeater.RepeatedViewItemSet)arg);

        private void DisposeRepeatedViewItemSet(Repeater.RepeatedViewItemSet viewItemSet)
        {
            UIClass keyFocusDescendant = this.UI.KeyFocusDescendant;
            this.DisposeRepeatedViewItemSet(viewItemSet, ref keyFocusDescendant);
        }

        private void DisposeRepeatedViewItemSet(
          Repeater.RepeatedViewItemSet viewItemSet,
          ref UIClass keyFocusDescendant)
        {
            if (viewItemSet.Repeated == this._lastKeyFocusedItem)
                this._lastKeyFocusedItem = null;
            if (viewItemSet.Repeated == this._lastMouseFocusedItem)
                this._lastMouseFocusedItem = null;
            viewItemSet.DisposeViewItems();
            if (keyFocusDescendant == null || keyFocusDescendant.IsValid)
                return;
            keyFocusDescendant = null;
            this.FireNotification(NotificationID.FocusedItemDiscarded);
        }

        private void GetRepeatedStats(out int minRepeatedIndex, out int maxRepeatedIndex)
        {
            if (this._repeatedViewItems.Count == 0)
            {
                minRepeatedIndex = maxRepeatedIndex = -1;
            }
            else
            {
                Repeater.RepeatedViewItemSet repeatedViewItem1 = this._repeatedViewItems[0];
                Repeater.RepeatedViewItemSet repeatedViewItem2 = this._repeatedViewItems[this._repeatedViewItems.Count - 1];
                minRepeatedIndex = repeatedViewItem1.VirtualIndex;
                maxRepeatedIndex = repeatedViewItem2.VirtualIndex;
            }
        }

        private bool HasDataIndexBeenRepeated(int dataIndex)
        {
            foreach (Repeater.RepeatedViewItemSet repeatedViewItem in this._repeatedViewItems)
            {
                if (dataIndex == repeatedViewItem.DataIndex)
                    return true;
            }
            return false;
        }

        private bool HasVirtualIndexBeenRepeated(int virtualIndex)
        {
            foreach (Repeater.RepeatedViewItemSet repeatedViewItem in this._repeatedViewItems)
            {
                if (virtualIndex == repeatedViewItem.VirtualIndex)
                    return true;
            }
            return false;
        }

        private void GetInsertIndex(int virtualIndex, out int repeatedItemIndex, out int viewItemIndex)
        {
            repeatedItemIndex = this.GetIndexOfClosestRepeatedItem(virtualIndex);
            viewItemIndex = this.GetViewItemIndex(repeatedItemIndex);
        }

        private int GetViewItemIndex(int repeatedItemIndex) => this._dividerName == null ? repeatedItemIndex : (repeatedItemIndex != 0 ? repeatedItemIndex * 2 - 1 : 0);

        private void GetContentTypeForRepeatedItem(object repeatObject, out string contentTypeName)
        {
            contentTypeName = this._contentName;
            if (!this._ignoreCustomRepeaters && repeatObject is ICustomRepeatedItem customRepeatedItem && customRepeatedItem.UIName != null)
                contentTypeName = customRepeatedItem.UIName;
            if (this._ctproc == null)
                return;
            this._ctproc(repeatObject, ref contentTypeName);
        }

        private Microsoft.Iris.Data.ItemRequestCallback QueryHandler => new Microsoft.Iris.Data.ItemRequestCallback(this.ItemRequestCallback);

        protected override NavigationPolicies ForcedNavigationFlags => NavigationPolicies.Group;

        private void QueueListContentsChanged(IList senderList, UIListContentsChangedArgs args)
        {
            RepeaterListContentsChangedArgs contentsChangedArgs = new RepeaterListContentsChangedArgs(args, this, this._sourceGeneration);
            DeferredCall.Post(DispatchPriority.Normal, Repeater.s_listContentsChangedHandler, contentsChangedArgs);
        }

        private static void AsyncListContentsChangedHandler(object args)
        {
            RepeaterListContentsChangedArgs contentsChangedArgs = (RepeaterListContentsChangedArgs)args;
            Repeater target = contentsChangedArgs.Target;
            if (target.IsDisposed || target._sourceGeneration != contentsChangedArgs.Generation)
                return;
            target.OnListContentsChanged(contentsChangedArgs.ChangedArgs);
        }

        private void OnListContentsChanged(UIListContentsChangedArgs args)
        {
            int oldIndex = args.OldIndex;
            int newIndex = args.NewIndex;
            int count = args.Count;
            int? nullable = new int?();
            IndexLayoutInput indexLayoutInput = null;
            if (this._lastKeyFocusedItem != null)
            {
                indexLayoutInput = this._lastKeyFocusedItem.GetLayoutInput(IndexLayoutInput.Data) as IndexLayoutInput;
                nullable = new int?(indexLayoutInput.Index.Value);
            }
            bool flag = false;
            switch (args.Type)
            {
                case UIListContentsChangeType.Add:
                case UIListContentsChangeType.AddRange:
                    this.InternalCount += count;
                    this.RequestRepeatOfIndexUpdate();
                    this.ShiftRepeatedItemDataIndices(this._itemsCount, count, this.InternalCount);
                    flag = true;
                    break;
                case UIListContentsChangeType.Remove:
                    Vector<Repeater.RepeatedViewItemSet> repeatedInstances1 = this.GetAllRepeatedInstances(oldIndex);
                    if (!ListUtility.IsNullOrEmpty(repeatedInstances1))
                    {
                        foreach (Repeater.RepeatedViewItemSet viewItemSet in repeatedInstances1)
                        {
                            this.DisposeRepeatedViewItemSet(viewItemSet);
                            this._repeatedViewItems.Remove(viewItemSet);
                        }
                    }
                    this.ShiftPendingRequestIndices(oldIndex, -1);
                    --this.InternalCount;
                    this.ShiftRepeatedItemDataIndices(oldIndex, -1, this.InternalCount);
                    flag = true;
                    break;
                case UIListContentsChangeType.Move:
                    if (oldIndex == newIndex || this.MoveRepeatedViewItemSet(oldIndex, newIndex))
                        break;
                    goto default;
                case UIListContentsChangeType.Insert:
                case UIListContentsChangeType.InsertRange:
                    this.InternalCount += count;
                    this.ShiftRepeatedItemDataIndices(newIndex, count, this.InternalCount);
                    this.ShiftPendingRequestIndices(newIndex, count);
                    this.RequestRepeatOfIndexUpdate();
                    flag = true;
                    break;
                case UIListContentsChangeType.Clear:
                    this.RebuildChildren();
                    break;
                case UIListContentsChangeType.Modified:
                    Vector<Repeater.RepeatedViewItemSet> repeatedInstances2 = this.GetAllRepeatedInstances(oldIndex);
                    if (!ListUtility.IsNullOrEmpty(repeatedInstances2))
                    {
                        foreach (Repeater.RepeatedViewItemSet viewItemSet in repeatedInstances2)
                        {
                            this.DisposeRepeatedViewItemSet(viewItemSet);
                            this._repeatedViewItems.Remove(viewItemSet);
                        }
                        break;
                    }
                    break;
                default:
                    this.RebuildChildren();
                    break;
            }
            if (nullable.HasValue && nullable.Value != indexLayoutInput.Index.Value && (this.UI.KeyFocus && this.UI.KeyFocusDescendant != null) && this.HasDescendant(UI.KeyFocusDescendant.RootItem))
                NavigationServices.SeedDefaultFocus(_lastKeyFocusedItem);
            if (!this._maintainFocusScreenLocation || !flag || this._focusNeedsRepairing)
                return;
            RectangleF descendantFocusRect = this.GetDescendantFocusRect();
            if (!(descendantFocusRect != RectangleF.Zero))
                return;
            this._focusNeedsRepairing = true;
            bool keyFocusIsDefault = this.UISession.InputManager.Queue.PendingKeyFocusIsDefault;
            DeferredCall.Post(DispatchPriority.LayoutSync, new DeferredHandler(this.PatchUpFocus), new Repeater.FocusRepairArgs(descendantFocusRect, keyFocusIsDefault));
            this.UI.UISession.InputManager.SuspendInputUntil(DispatchPriority.LayoutSync);
        }

        private void PatchUpFocus(object focusArgs)
        {
            if (!this._focusNeedsRepairing)
                return;
            this._focusNeedsRepairing = false;
            Repeater.FocusRepairArgs focusRepairArgs = (Repeater.FocusRepairArgs)focusArgs;
            INavigationSite result;
            if (!NavigationServices.FindFromPoint(this, focusRepairArgs.focusBounds.Center, out result) || result == null || !(result is ViewItem viewItem))
                return;
            viewItem.UI.NotifyNavigationDestination(focusRepairArgs.focusIsDefault ? KeyFocusReason.Default : KeyFocusReason.Other);
        }

        private void ShiftRepeatedItemDataIndices(int baseIndex, int offsetValue, int updatedItemCount)
        {
            foreach (Repeater.RepeatedViewItemSet repeatedViewItem in this._repeatedViewItems)
            {
                int dataIndex = repeatedViewItem.DataIndex;
                if (dataIndex >= baseIndex)
                    dataIndex += offsetValue;
                int unwrappedIndex = ListUtility.GetUnwrappedIndex(dataIndex, repeatedViewItem.Generation, updatedItemCount);
                repeatedViewItem.SetIndex(unwrappedIndex, dataIndex);
            }
        }

        private void ShiftPendingRequestIndices(int shiftBaseIndex, int adjustmentValue)
        {
            if (this._pendingIndexRequest.HasValue)
            {
                if (adjustmentValue < 0 && this._pendingIndexRequest.Value == shiftBaseIndex)
                    this._pendingIndexRequest = new int?();
                else if (this._pendingIndexRequest.Value >= shiftBaseIndex)
                    this._pendingIndexRequest = new int?(this._pendingIndexRequest.Value + adjustmentValue);
            }
            if (this.LayoutRequestedIndices == null)
                return;
            if (adjustmentValue < 0)
                this.LayoutRequestedIndices.Remove(shiftBaseIndex);
            this.ShiftIndices(this.LayoutRequestedIndices, shiftBaseIndex, adjustmentValue);
        }

        private void ShiftIndices(Vector<int> indices, int shiftBaseIndex, int adjustmentValue)
        {
            for (int index1 = 0; index1 < indices.Count; ++index1)
            {
                int index2 = indices[index1];
                if (index2 >= shiftBaseIndex)
                {
                    int num = index2 + adjustmentValue;
                    indices[index1] = num;
                }
            }
        }

        private Vector<Repeater.RepeatedViewItemSet> GetAllRepeatedInstances(int dataIndex)
        {
            Vector<Repeater.RepeatedViewItemSet> vector = null;
            foreach (Repeater.RepeatedViewItemSet repeatedViewItem in this._repeatedViewItems)
            {
                if (repeatedViewItem.DataIndex == dataIndex)
                {
                    if (vector == null)
                        vector = new Vector<Repeater.RepeatedViewItemSet>();
                    vector.Add(repeatedViewItem);
                }
            }
            return vector;
        }

        private Repeater.RepeatedViewItemSet GetRepeatedInstance(int virtualIndex)
        {
            if (this._repeatedViewItems != null)
            {
                foreach (Repeater.RepeatedViewItemSet repeatedViewItem in this._repeatedViewItems)
                {
                    if (repeatedViewItem.VirtualIndex == virtualIndex)
                        return repeatedViewItem;
                }
            }
            return null;
        }

        private int GetIndexOfClosestRepeatedItem(int virtualIndex)
        {
            int num = 0;
            for (int index = this._repeatedViewItems.Count - 1; index >= 0; --index)
            {
                if (this._repeatedViewItems[index].VirtualIndex < virtualIndex)
                {
                    num = index + 1;
                    break;
                }
            }
            return num;
        }

        private bool MoveRepeatedViewItemSet(int dataOldIndex, int dataNewIndex)
        {
            Map<int, Repeater.RepeatedAndIndex> map1 = new Map<int, Repeater.RepeatedAndIndex>();
            Map<int, Repeater.RepeatedAndIndex> map2 = new Map<int, Repeater.RepeatedAndIndex>();
            for (int index = 0; index < this._repeatedViewItems.Count; ++index)
            {
                Repeater.RepeatedViewItemSet repeatedViewItem = this._repeatedViewItems[index];
                if (repeatedViewItem.DataIndex == dataOldIndex)
                    map1[repeatedViewItem.Generation] = new Repeater.RepeatedAndIndex(repeatedViewItem, index);
                else if (repeatedViewItem.DataIndex == dataNewIndex)
                    map2[repeatedViewItem.Generation] = new Repeater.RepeatedAndIndex(repeatedViewItem, index);
            }
            Vector<Repeater.RepeatedViewItemSet> vector = new Vector<Repeater.RepeatedViewItemSet>();
            bool flag = true;
            foreach (int key in map1.Keys)
            {
                Repeater.RepeatedAndIndex repeatedAndIndex1 = map1[key];
                if (!map2.ContainsKey(key))
                {
                    flag = false;
                }
                else
                {
                    Repeater.RepeatedAndIndex repeatedAndIndex2 = map2[key];
                    if (!this.MoveRepeatedViewItemSetWorker(repeatedAndIndex1.repeated, repeatedAndIndex2.repeated, dataOldIndex, dataNewIndex))
                    {
                        flag = false;
                    }
                    else
                    {
                        this._repeatedViewItems.Remove(repeatedAndIndex1.repeated);
                        vector.Add(repeatedAndIndex1.repeated);
                        int unwrappedIndex = ListUtility.GetUnwrappedIndex(dataNewIndex, key, this.InternalCount);
                        repeatedAndIndex1.repeated.SetIndex(unwrappedIndex, dataNewIndex);
                    }
                }
            }
            foreach (int key in map2.Keys)
            {
                if (!map1.ContainsKey(key))
                {
                    Repeater.RepeatedAndIndex repeatedAndIndex = map2[key];
                    flag = false;
                }
            }
            this.ShiftRepeatedItemDataIndices(dataOldIndex + 1, -1, this.InternalCount);
            this.ShiftRepeatedItemDataIndices(dataNewIndex, 1, this.InternalCount);
            if (vector.Count > 0)
            {
                int index1 = 0;
                Repeater.RepeatedViewItemSet repeatedViewItemSet = vector[index1];
                for (int index2 = 0; index2 < this._repeatedViewItems.Count; ++index2)
                {
                    if (this._repeatedViewItems[index2].VirtualIndex > repeatedViewItemSet.VirtualIndex)
                    {
                        this._repeatedViewItems.Insert(index2, repeatedViewItemSet);
                        ++index1;
                        if (index1 < vector.Count)
                            repeatedViewItemSet = vector[index1];
                        else
                            break;
                    }
                }
                for (; index1 < vector.Count; ++index1)
                    this._repeatedViewItems.Add(vector[index1]);
            }
            if (!flag)
                return false;
            this.MarkLayoutInvalid();
            return true;
        }

        private bool MoveRepeatedViewItemSetWorker(
          Repeater.RepeatedViewItemSet item,
          Repeater.RepeatedViewItemSet itemFinal,
          int dataOldIndex,
          int dataNewIndex)
        {
            Microsoft.Iris.Library.TreeNode.LinkType lt = Microsoft.Iris.Library.TreeNode.LinkType.Before;
            if (dataNewIndex > dataOldIndex)
                lt = Microsoft.Iris.Library.TreeNode.LinkType.Behind;
            ViewItem viewItem = itemFinal.Repeated;
            if (itemFinal.Divider != null && lt == Microsoft.Iris.Library.TreeNode.LinkType.Before)
                viewItem = itemFinal.Divider;
            item.Repeated.MoveNode(viewItem, lt);
            if (this.DividerName != null)
            {
                ViewItem divider = item.Divider;
                ViewItem repeated = item.Repeated;
                if (divider == null)
                {
                    Repeater.RepeatedViewItemSet repeatedViewItem = this._repeatedViewItems[this.GetIndexOfClosestRepeatedItem(item.VirtualIndex)];
                    if (repeatedViewItem == null)
                        return false;
                    divider = repeatedViewItem.Divider;
                    item.Divider = repeatedViewItem.Divider;
                    repeatedViewItem.Divider = null;
                }
                if (itemFinal.Divider == null)
                {
                    repeated = itemFinal.Repeated;
                    itemFinal.Divider = item.Divider;
                    item.Divider = null;
                }
                divider?.MoveNode(repeated, Microsoft.Iris.Library.TreeNode.LinkType.Before);
            }
            return true;
        }

        protected override ViewItemID IDForChild(ViewItem childItem) => new ViewItemID((childItem.GetLayoutInput(IndexLayoutInput.Data) as IndexLayoutInput).Index.Value, Repeater.c_childIDSentinel);

        protected override FindChildResult ChildForID(
          ViewItemID part,
          out ViewItem resultItem)
        {
            resultItem = null;
            FindChildResult findChildResult = FindChildResult.Failure;
            if (part.IDValid && part.StringPartValid && part.StringPart == Repeater.c_childIDSentinel)
            {
                resultItem = this.GetRepeatedItemForVirtualIndex(part.ID);
                findChildResult = resultItem == null || !resultItem.HasVisual ? FindChildResult.PotentiallyFaultIn : FindChildResult.Success;
            }
            return findChildResult;
        }

        internal int DefaultFocusIndex
        {
            get => this._defaultFocusIndex;
            set
            {
                if (this._defaultFocusIndex == value)
                    return;
                ViewItem itemForVirtualIndex1 = this.GetRepeatedItemForVirtualIndex(this._defaultFocusIndex);
                if (itemForVirtualIndex1 != null && itemForVirtualIndex1.FocusOrder == 2147483646)
                    itemForVirtualIndex1.FocusOrder = int.MaxValue;
                this._defaultFocusIndex = value;
                ViewItem itemForVirtualIndex2 = this.GetRepeatedItemForVirtualIndex(this._defaultFocusIndex);
                if (itemForVirtualIndex2 != null)
                    this.SetAsDefaultFocusRecipient(itemForVirtualIndex2);
                this.FireNotification(NotificationID.DefaultFocusIndex);
            }
        }

        internal void ScrollIndexIntoView(int index)
        {
            ViewItem itemForVirtualIndex = this.GetRepeatedItemForVirtualIndex(index);
            if (itemForVirtualIndex != null)
                itemForVirtualIndex.ScrollIntoView();
            else
                this.FaultInChild(index, Repeater.PendingIndexRequestType.ScrollIndexIntoView, Repeater.s_scrollIndexIntoViewHandler);
        }

        private static void ScrollIndexIntoViewItemFaultedIn(ViewItem repeater, ViewItem faultedItem)
        {
            if (faultedItem == null || faultedItem.IsDisposed)
                return;
            faultedItem.ScrollIntoView();
        }

        private void SetAsDefaultFocusRecipient(ViewItem childItem) => childItem.FocusOrder = 2147483646;

        internal void NavigateIntoIndex(int index) => this.NavigateIntoIndex(index, true);

        internal void NavigateIntoIndex(int index, bool allowFaultIn)
        {
            ViewItem itemForVirtualIndex = this.GetRepeatedItemForVirtualIndex(index);
            if (itemForVirtualIndex != null && itemForVirtualIndex.HasVisual)
            {
                itemForVirtualIndex.NavigateInto();
            }
            else
            {
                if (!allowFaultIn)
                    return;
                this.FaultInChild(index, Repeater.PendingIndexRequestType.NavigateIntoIndex, Repeater.s_navigateIntoIndexHandler);
            }
        }

        private static void NavigateIntoIndexItemFaultedIn(ViewItem repeater, ViewItem faultedItem)
        {
            if (faultedItem == null || faultedItem.IsDisposed)
                return;
            faultedItem.NavigateInto();
        }

        internal override void FaultInChild(ViewItemID child, ChildFaultedInDelegate handler) => this.FaultInChild(child.ID, Repeater.PendingIndexRequestType.FaultInChild, handler);

        private void FaultInChild(
          int virtualIndex,
          Repeater.PendingIndexRequestType type,
          ChildFaultedInDelegate handler)
        {
            if (this._pendingIndexRequest.HasValue && type < this._pendingIndexRequestType)
                return;
            this._pendingIndexRequest = new int?(virtualIndex);
            this._pendingIndexRequestType = type;
            this.RequestRepeatOfIndexUpdate();
            DeferredCall.Post(DispatchPriority.LayoutSync, new Repeater.FaultInChildThunk(this, virtualIndex, handler).Thunk);
        }

        internal ViewItem GetRepeatedItemForVirtualIndex(int index)
        {
            ViewItem viewItem = null;
            Repeater.RepeatedViewItemSet repeatedInstance = this.GetRepeatedInstance(index);
            if (repeatedInstance != null)
                viewItem = repeatedInstance.Repeated;
            return viewItem;
        }

        [Conditional("DEBUG")]
        private void DEBUG_DumpRepeatedItems(string st, byte level)
        {
            if (!Microsoft.Iris.Debug.Trace.IsCategoryEnabled(TraceCategory.Repeating, level))
                return;
            for (int index = 0; index < this._repeatedViewItems.Count; ++index)
            {
                Repeater.RepeatedViewItemSet repeatedViewItem = this._repeatedViewItems[index];
            }
            if (!Microsoft.Iris.Debug.Trace.IsCategoryEnabled(TraceCategory.Repeating, (byte)(level + 1U)))
                return;
            foreach (ViewItem child in this.Children)
                ;
        }

        [Conditional("DEBUG")]
        private void DEBUG_DumpRepeatedItem(Repeater.RepeatedViewItemSet item, byte level)
        {
            if (!Microsoft.Iris.Debug.Trace.IsCategoryEnabled(TraceCategory.Repeating, level) || !ListUtility.IsValidIndex(this._source, item.DataIndex))
                return;
            this._source[item.DataIndex]?.ToString();
        }

        [Conditional("DEBUG")]
        private void DEBUG_DumpRepeatedViewItem(ViewItem vi, byte level)
        {
        }

        [Conditional("DEBUG")]
        private void DEBUG_ValidateSortedByVirtualIndex(Vector<Repeater.RepeatedViewItemSet> list)
        {
            if (list == null)
                return;
            int num = int.MinValue;
            for (int index = 0; index < list.Count; ++index)
                num = list[index].VirtualIndex;
        }

        public static object UnavailableItem => Repeater.s_unavailableObject;

        public delegate void ContentTypeHandler(object repeatObject, ref string contentName);

        internal class RepeatedViewItemSet
        {
            private Index _index;
            private int _generationValue;
            private ViewItem _repeatedItem;
            private ViewItem _dividerItem;

            public RepeatedViewItemSet(
              Index index,
              int generationValue,
              ViewItem repeatedItem,
              ViewItem dividerItem)
            {
                this._index = index;
                this._generationValue = generationValue;
                this._repeatedItem = repeatedItem;
                this._dividerItem = dividerItem;
            }

            public int DataIndex => this._index.SourceValue;

            public int VirtualIndex => this._index.Value;

            public int Generation => this._generationValue;

            public ViewItem Repeated => this._repeatedItem;

            public ViewItem Divider
            {
                get => this._dividerItem;
                set => this._dividerItem = value;
            }

            public void SetIndex(int virtualIndex, int dataIndex) => this._index.SetValue(virtualIndex, dataIndex);

            public void DisposeViewItems()
            {
                this.DisposeViewItem(this._repeatedItem);
                if (this._dividerItem == null)
                    return;
                this.DisposeViewItem(this._dividerItem);
            }

            private void DisposeViewItem(ViewItem vi)
            {
                UIClass ui = vi.UI;
                if (vi.HasVisual)
                    ui.DestroyVisualTree(vi);
                ui.DisposeViewItemTree(vi);
            }

            public override string ToString() => this.ToString(string.Empty);

            public string ToString(string dataName)
            {
                string str = null;
                if (this._dividerItem != null)
                    str = InvariantString.Format(", Divider({0})", _dividerItem.GetType().Name);
                return InvariantString.Format("[{0},{1}] {2} ({3} {4})", VirtualIndex, DataIndex, dataName, _repeatedItem.GetType().Name, str);
            }
        }

        internal struct RepeatedAndIndex
        {
            public Repeater.RepeatedViewItemSet repeated;
            public int index;

            public RepeatedAndIndex(Repeater.RepeatedViewItemSet repeated, int index)
            {
                this.repeated = repeated;
                this.index = index;
            }
        }

        private class FaultInChildThunk
        {
            private Repeater _repeater;
            private int _virtualIndex;
            private ChildFaultedInDelegate _faultInHandler;

            public FaultInChildThunk(
              Repeater repeater,
              int virtualIndex,
              ChildFaultedInDelegate faultInHandler)
            {
                this._repeater = repeater;
                this._virtualIndex = virtualIndex;
                this._faultInHandler = faultInHandler;
            }

            public SimpleCallback Thunk => new SimpleCallback(this.CallFaultInHandler);

            private void CallFaultInHandler()
            {
                ViewItem childItem = null;
                if (!this._repeater.IsDisposed)
                    childItem = this._repeater.GetRepeatedItemForVirtualIndex(this._virtualIndex);
                this._faultInHandler(_repeater, childItem);
            }
        }

        private struct FocusRepairArgs
        {
            public RectangleF focusBounds;
            public bool focusIsDefault;

            public FocusRepairArgs(RectangleF focusBounds, bool focusIsDefault)
            {
                this.focusBounds = focusBounds;
                this.focusIsDefault = focusIsDefault;
            }
        }

        private enum PendingIndexRequestType
        {
            ScrollIndexIntoView,
            FaultInChild,
            NavigateIntoIndex,
        }
    }
}
