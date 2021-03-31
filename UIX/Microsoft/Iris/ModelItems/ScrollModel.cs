// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ModelItems.ScrollModel
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Input;
using Microsoft.Iris.Layout;
using Microsoft.Iris.Layouts;
using Microsoft.Iris.Markup;
using Microsoft.Iris.Navigation;
using Microsoft.Iris.Render;
using Microsoft.Iris.RenderAPI.Drawing;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.ModelItems
{
    internal class ScrollModel : ScrollModelBase
    {
        private ScrollingLayoutInput _input;
        private ScrollingLayoutOutput _output;
        private ScrollIntoViewDisposition _userDisposition;
        private bool _useUserDisposition;
        private ViewItem _targetItem;
        private ViewItem _lastFocusedItem;
        private UIClass _uiWithAreaOfInterestToClear;
        private bool _isCamping;
        private bool _canDoMoveUp;
        private bool _canDoMoveDown;
        private bool _pageSizedScrollStepFlag;
        private int _scrollStepValue;
        private Orientation _scrollOrientation;
        private ScrollModel.PostLayoutAction _postLayoutAction;
        private ScrollModel.NavigateAction _navigateAction;
        private ScrollModel.AssignFocusAction _assignFocusAction;

        public ScrollModel()
        {
            this._input = new ScrollingLayoutInput();
            this._output = new ScrollingLayoutOutput();
            this.ScrollStep = 50;
            this.PageStep = 0.8f;
            this._userDisposition = new ScrollIntoViewDisposition(0);
            this._userDisposition.Enabled = true;
            this._useUserDisposition = true;
            this.ActualScrollIntoViewDisposition = new ScrollIntoViewDisposition(0);
            this.ActualScrollIntoViewDisposition.Enabled = true;
            this._canDoMoveUp = true;
            this._canDoMoveDown = true;
            this._scrollOrientation = Orientation.Horizontal;
        }

        public void AttachToViewItem(ViewItem vi)
        {
            vi.LayoutInput = _input;
            vi.LayoutComplete += new LayoutCompleteEventHandler(this.OnLayoutComplete);
            this._targetItem = vi;
        }

        public void DetachFromViewItem(ViewItem vi)
        {
            this._targetItem.LayoutComplete -= new LayoutCompleteEventHandler(this.OnLayoutComplete);
            this._targetItem.SetLayoutInput(ScrollingLayoutInput.Data, null);
            this._targetItem = null;
            this._output = null;
            this._lastFocusedItem = null;
            this._uiWithAreaOfInterestToClear = null;
            this._postLayoutAction = null;
            this._navigateAction = null;
            this._assignFocusAction = null;
        }

        public ViewItem TargetViewItem
        {
            get => this._targetItem;
            set
            {
                if (this._targetItem == value)
                    return;
                this.AttachToViewItem(value);
            }
        }

        internal Orientation ScrollOrientation
        {
            get => this._scrollOrientation;
            set
            {
                if (this._scrollOrientation == value)
                    return;
                this._scrollOrientation = value;
            }
        }

        public bool Enabled
        {
            get => this._input.Enabled;
            set
            {
                if (this._input.Enabled == value)
                    return;
                this._input.Enabled = value;
                this.OnLayoutInputChanged();
                this.FireNotification(NotificationID.Enabled);
            }
        }

        public override int ScrollStep
        {
            get => this._scrollStepValue;
            set
            {
                if (this._scrollStepValue == value)
                    return;
                this._scrollStepValue = value;
                this.FireNotification(NotificationID.ScrollStep);
            }
        }

        public float PageStep
        {
            get => this._input.PageStep;
            set
            {
                if (_input.PageStep == (double)value)
                    return;
                this._input.PageStep = value;
                this.OnLayoutInputChanged();
                this.FireNotification(NotificationID.PageStep);
            }
        }

        public bool PageSizedScrollStep
        {
            get => this._pageSizedScrollStepFlag;
            set
            {
                if (this._pageSizedScrollStepFlag == value)
                    return;
                this._pageSizedScrollStepFlag = value;
                this.FireNotification(NotificationID.PageSizedScrollStep);
            }
        }

        public override void Scroll(int amount)
        {
            this.DisableScrollIntoView();
            this._input.Scroll(amount);
            this.OnLayoutInputChanged();
        }

        public override void ScrollUp() => this.ScrollUp(1);

        public void ScrollUp(bool attemptFocusBehavior)
        {
            if (attemptFocusBehavior)
                this.MoveDirection(true);
            else
                this.ScrollUp();
        }

        public void ScrollUp(int numTimes)
        {
            if (this._pageSizedScrollStepFlag)
                this.PageUp(numTimes);
            else
                this.Scroll(numTimes * -this._scrollStepValue);
        }

        public override void ScrollDown() => this.ScrollDown(1);

        public void ScrollDown(bool attemptFocusBehavior)
        {
            if (attemptFocusBehavior)
                this.MoveDirection(false);
            else
                this.ScrollDown();
        }

        public void ScrollDown(int numTimes)
        {
            if (this._pageSizedScrollStepFlag)
                this.PageDown(numTimes);
            else
                this.Scroll(numTimes * this._scrollStepValue);
        }

        public override void PageUp() => this.PageUp(1);

        public void PageUp(bool attemptFocusBehavior)
        {
            if (attemptFocusBehavior)
                this.MovePage(true);
            else
                this.PageUp();
        }

        public void PageUp(int numTimes)
        {
            this.DisableScrollIntoView();
            for (; numTimes > 0; --numTimes)
                this._input.PageUp();
            this.OnLayoutInputChanged();
        }

        public override void PageDown() => this.PageDown(1);

        public void PageDown(bool attemptFocusBehavior)
        {
            if (attemptFocusBehavior)
                this.MovePage(false);
            else
                this.PageDown();
        }

        public void PageDown(int numTimes)
        {
            this.DisableScrollIntoView();
            for (; numTimes > 0; --numTimes)
                this._input.PageDown();
            this.OnLayoutInputChanged();
        }

        public override void Home()
        {
            this.DisableScrollIntoView();
            this._input.Home();
            this.OnLayoutInputChanged();
        }

        public void Home(bool attemptFocusBehavior)
        {
            if (attemptFocusBehavior)
                this.MoveToEndPoint(true);
            else
                this.Home();
        }

        public override void End()
        {
            this.DisableScrollIntoView();
            this._input.End();
            this.OnLayoutInputChanged();
        }

        public void End(bool attemptFocusBehavior)
        {
            if (attemptFocusBehavior)
                this.MoveToEndPoint(false);
            else
                this.End();
        }

        public override void ScrollToPosition(float position)
        {
            if (position < 0.0)
                position = 0.0f;
            else if (position > 1.0)
                position = 1f;
            this.DisableScrollIntoView();
            this._input.ScrollToPosition(position);
            this.OnLayoutInputChanged();
        }

        public void ScrollFocusIntoView() => this.EnableScrollIntoView();

        internal ScrollIntoViewDisposition ScrollIntoViewDisposition
        {
            get => this._userDisposition;
            set
            {
                if (this._userDisposition.Equals(value))
                    return;
                this._userDisposition = value;
                this.EnableScrollIntoView();
            }
        }

        private ScrollIntoViewDisposition ActualScrollIntoViewDisposition
        {
            get => this._input.ScrollIntoViewDisposition;
            set => this._input.ScrollIntoViewDisposition = value;
        }

        public int BeginPadding
        {
            get => this.ScrollIntoViewDisposition.BeginPadding;
            set
            {
                if (this.ScrollIntoViewDisposition.BeginPadding == value)
                    return;
                this.ScrollIntoViewDisposition.BeginPadding = value;
                this.EnableScrollIntoView();
                this.FireNotification(NotificationID.BeginPadding);
            }
        }

        public int EndPadding
        {
            get => this.ScrollIntoViewDisposition.EndPadding;
            set
            {
                if (this.ScrollIntoViewDisposition.EndPadding == value)
                    return;
                this.ScrollIntoViewDisposition.EndPadding = value;
                this.EnableScrollIntoView();
                this.FireNotification(NotificationID.EndPadding);
            }
        }

        public RelativeEdge BeginPaddingRelativeTo
        {
            get => this.ScrollIntoViewDisposition.BeginPaddingRelativeTo;
            set
            {
                if (this.ScrollIntoViewDisposition.BeginPaddingRelativeTo == value)
                    return;
                this.ScrollIntoViewDisposition.BeginPaddingRelativeTo = value;
                this.EnableScrollIntoView();
                this.FireNotification(NotificationID.BeginPaddingRelativeTo);
            }
        }

        public RelativeEdge EndPaddingRelativeTo
        {
            get => this.ScrollIntoViewDisposition.EndPaddingRelativeTo;
            set
            {
                if (this.ScrollIntoViewDisposition.EndPaddingRelativeTo == value)
                    return;
                this.ScrollIntoViewDisposition.EndPaddingRelativeTo = value;
                this.EnableScrollIntoView();
                this.FireNotification(NotificationID.EndPaddingRelativeTo);
            }
        }

        public float LockedPosition
        {
            get => this.ScrollIntoViewDisposition.LockedPosition;
            set
            {
                if (ScrollIntoViewDisposition.LockedPosition == (double)value)
                    return;
                this.ScrollIntoViewDisposition.LockedPosition = value;
                this.EnableScrollIntoView();
                this.FireNotification(NotificationID.LockedPosition);
            }
        }

        public bool Locked
        {
            get => this.ScrollIntoViewDisposition.Locked;
            set
            {
                if (this.ScrollIntoViewDisposition.Locked == value)
                    return;
                this.ScrollIntoViewDisposition.Locked = value;
                this.EnableScrollIntoView();
                this.FireNotification(NotificationID.Locked);
            }
        }

        public ContentPositioningPolicy ContentPositioningBehavior
        {
            get => this.ScrollIntoViewDisposition.ContentPositioningBehavior;
            set
            {
                if (this.ScrollIntoViewDisposition.ContentPositioningBehavior == value)
                    return;
                this.ScrollIntoViewDisposition.ContentPositioningBehavior = value;
                this.EnableScrollIntoView();
                this.FireNotification(NotificationID.ContentPositioningBehavior);
            }
        }

        public float LockedAlignment
        {
            get => this.ScrollIntoViewDisposition.LockedAlignment;
            set
            {
                if (ScrollIntoViewDisposition.LockedAlignment == (double)value)
                    return;
                this.ScrollIntoViewDisposition.LockedAlignment = value;
                this.EnableScrollIntoView();
                this.FireNotification(NotificationID.LockedAlignment);
            }
        }

        public override bool CanScrollUp => this.Enabled && this._output.CanScrollNegative;

        public override bool CanScrollDown => this.Enabled && this._output.CanScrollPositive;

        public override float CurrentPage => this._output.CurrentPage;

        public override float TotalPages => this._output.TotalPages;

        public override float ViewNear => this._output.ViewNear;

        public override float ViewFar => this._output.ViewFar;

        private void MovePage(bool nearDirection)
        {
            if (this.HadFocus())
            {
                if (!this.CanMoveDirection(nearDirection))
                    return;
                bool flag = true;
                switch (this.GetLastFocusLocation())
                {
                    case ScrollModel.ItemLocation.OffscreenInNearDirection:
                        flag = nearDirection;
                        break;
                    case ScrollModel.ItemLocation.Onscreen:
                        flag = !this.PotentialNavigationTargetIsOnscreen(this.NearFarToDirection(nearDirection), out UIClass _);
                        if (!flag && this._useUserDisposition && this.NonDefaultUserDisposition())
                        {
                            flag = true;
                            break;
                        }
                        break;
                    case ScrollModel.ItemLocation.OffscreenInFarDirection:
                        flag = !nearDirection;
                        break;
                }
                ScrollModel.AssignFocusAction instance = ScrollModel.AssignFocusAction.GetInstance(this, this.GetAssignFocusPoint(nearDirection), nearDirection, false);
                if (!flag)
                {
                    instance.Go();
                }
                else
                {
                    float num = !nearDirection ? 0.0f : 1f;
                    this.ActualScrollIntoViewDisposition.Reset();
                    this.ActualScrollIntoViewDisposition.Enabled = true;
                    this.ActualScrollIntoViewDisposition.LockedPosition = num;
                    this.ActualScrollIntoViewDisposition.LockedAlignment = num;
                    this.ActualScrollIntoViewDisposition.Locked = true;
                    this.SetPendingFocusAreaOfInterest(this._lastFocusedItem.UI);
                    this.OnLayoutInputChanged();
                    this.SetPostLayoutAction(instance);
                }
            }
            else if (nearDirection)
            {
                if (!this.CanScrollUp)
                    return;
                this.PageUp();
            }
            else
            {
                if (!this.CanScrollDown)
                    return;
                this.PageDown();
            }
        }

        public void NotifyFocusChange(UIClass newUI)
        {
            if (newUI == null || newUI == this._uiWithAreaOfInterestToClear || this._lastFocusedItem != null && this._lastFocusedItem.UI == newUI)
                return;
            this.EnableScrollIntoView();
        }

        private void SetPostLayoutAction(ScrollModel.PostLayoutAction action) => this._postLayoutAction = action;

        private void DeliverPostLayoutAction()
        {
            if (this._postLayoutAction == null)
                return;
            this._postLayoutAction.Go();
            this._postLayoutAction = null;
        }

        private Direction NearFarToDirection(bool near)
        {
            if (this.ScrollOrientation == Orientation.Vertical)
                return !near ? Direction.South : Direction.North;
            near ^= this._targetItem.Zone.Session.IsRtl;
            return !near ? Direction.East : Direction.West;
        }

        private bool LastFocusIsOnscreen() => this.GetItemLocation(this._lastFocusedItem) == ScrollModel.ItemLocation.Onscreen;

        private ScrollModel.ItemLocation GetLastFocusLocation() => this.GetItemLocation(this._lastFocusedItem);

        private bool ItemIsOnscreen(ViewItem item) => this.GetItemLocation(item) == ScrollModel.ItemLocation.Onscreen;

        private ScrollModel.ItemLocation GetItemLocation(ViewItem item)
        {
            RectangleF scrollerRect = this.GetScrollerRect(false);
            RectangleF viewItemRect = this.GetViewItemRect(item, false);
            return !(RectangleF.Intersect(scrollerRect, viewItemRect) == viewItemRect) ? (this.ScrollOrientation != Orientation.Horizontal ? (viewItemRect.Top >= (double)scrollerRect.Top ? ScrollModel.ItemLocation.OffscreenInFarDirection : ScrollModel.ItemLocation.OffscreenInNearDirection) : (viewItemRect.Left < (double)scrollerRect.Left || this._targetItem.Zone.Session.IsRtl && viewItemRect.Right > (double)scrollerRect.Right ? ScrollModel.ItemLocation.OffscreenInNearDirection : ScrollModel.ItemLocation.OffscreenInFarDirection)) : ScrollModel.ItemLocation.Onscreen;
        }

        private bool PotentialNavigationTargetIsOnscreen(Direction dir, out UIClass navigationResult) => this.PotentialNavigationTargetIsOnscreen(this._lastFocusedItem.UI, dir, out navigationResult);

        private bool PotentialNavigationTargetIsOnscreen(
          UIClass ui,
          Direction dir,
          out UIClass navigationResult)
        {
            bool flag = false;
            if (this.PotentialNavigationTargetIsDescendant(ui, dir, out navigationResult))
                flag = this.ItemIsOnscreen(navigationResult.RootItem);
            return flag;
        }

        private bool PotentialNavigationTargetIsDescendant(Direction dir, out UIClass navigationResult) => this.PotentialNavigationTargetIsDescendant(this._lastFocusedItem.UI, dir, out navigationResult);

        private bool PotentialNavigationTargetIsDescendant(
          UIClass ui,
          Direction dir,
          out UIClass navigationResult)
        {
            bool flag = false;
            if (ui.FindNextFocusablePeer(dir, RectangleF.Zero, out navigationResult) && navigationResult != null && this._targetItem.HasDescendant(navigationResult.RootItem))
                flag = true;
            return flag;
        }

        private PointF GetMoveToEndpointPoint(bool near)
        {
            PointF zero = PointF.Zero;
            RectangleF scrollerRect = this.GetScrollerRect(true);
            bool isRtl = this._targetItem.Zone.Session.IsRtl;
            if (near)
            {
                if (isRtl)
                    zero.X = scrollerRect.Right;
            }
            else
            {
                zero.X = isRtl ? scrollerRect.Left : scrollerRect.Right;
                zero.Y = scrollerRect.Bottom;
            }
            return zero;
        }

        private PointF GetAssignFocusPoint(bool near)
        {
            RectangleF lastFocusRect = this.GetLastFocusRect(true);
            RectangleF scrollerRect = this.GetScrollerRect(true);
            PointF center = lastFocusRect.Center;
            float x;
            float y;
            if (this.ScrollOrientation == Orientation.Vertical)
            {
                x = center.X;
                float num = lastFocusRect.Height / 2f;
                y = !near ? scrollerRect.Bottom - num : scrollerRect.Top + num;
            }
            else
            {
                y = center.Y;
                float num = lastFocusRect.Width / 2f;
                x = !(near ^ this._targetItem.Zone.Session.IsRtl) ? scrollerRect.Right - num : scrollerRect.Left + num;
            }
            return new PointF(x, y);
        }

        private void MoveToEndPoint(bool home)
        {
            bool flag1 = this.HadFocus();
            bool flag2 = false;
            bool flag3 = false;
            if (flag1)
                flag3 = this.CanMoveDirection(home);
            if (flag3 || !flag1)
                flag2 = !home ? this.CanScrollDown : this.CanScrollUp;
            if (flag2)
            {
                if (home)
                    this.Home();
                else
                    this.End();
            }
            if (!flag3)
                return;
            ScrollModel.AssignFocusAction instance = ScrollModel.AssignFocusAction.GetInstance(this, PointF.Zero, home, true);
            if (!flag2)
                instance.Go();
            else
                this.SetPostLayoutAction(instance);
        }

        private void MoveDirection(bool nearDirection)
        {
            if (this.HadFocus())
            {
                if (!this.CanMoveDirection(nearDirection))
                    return;
                Direction direction = this.NearFarToDirection(nearDirection);
                UIClass ui = this._lastFocusedItem.UI;
                bool flag = true;
                UIClass navigationResult;
                if (this.LastFocusIsOnscreen() && this.PotentialNavigationTargetIsOnscreen(direction, out navigationResult))
                {
                    this.NavigateToUI(navigationResult);
                    this.EnableScrollIntoView();
                    flag = false;
                }
                if (!flag)
                    return;
                this.ActualScrollIntoViewDisposition.Reset();
                this.ActualScrollIntoViewDisposition.Enabled = true;
                this.SetPendingFocusAreaOfInterest(this._lastFocusedItem.UI);
                this.SetPostLayoutAction(ScrollModel.NavigateAction.GetInstance(this, nearDirection, direction));
                this.OnLayoutInputChanged();
            }
            else if (nearDirection)
            {
                if (!this.CanScrollUp)
                    return;
                this.ScrollUp();
            }
            else
            {
                if (!this.CanScrollDown)
                    return;
                this.ScrollDown();
            }
        }

        private void DisableScrollIntoView()
        {
            this.ActualScrollIntoViewDisposition.Enabled = false;
            this._useUserDisposition = false;
        }

        private void EnableScrollIntoView()
        {
            this.ActualScrollIntoViewDisposition.CopyFrom(this._userDisposition);
            this._useUserDisposition = true;
            this.OnLayoutInputChanged();
        }

        private void OnLayoutInputChanged()
        {
            if (this._targetItem == null)
                return;
            this._targetItem.MarkLayoutInvalid();
        }

        private void OnLayoutOutputChanged()
        {
            ScrollingLayoutOutput output = this._output;
            if (!(this._targetItem.GetExtendedLayoutOutput(ScrollingLayoutOutput.DataCookie) is ScrollingLayoutOutput scrollingLayoutOutput))
                scrollingLayoutOutput = new ScrollingLayoutOutput();
            this._output = scrollingLayoutOutput;
            if (this._output.ProcessedExplicitScrollIntoViewRequest || !this._output.ScrollFocusIntoView)
                this.DisableScrollIntoView();
            if (this._output.CanScrollNegative != output.CanScrollNegative)
                this.FireNotification(NotificationID.CanScrollUp);
            if (this._output.CanScrollPositive != output.CanScrollPositive)
                this.FireNotification(NotificationID.CanScrollDown);
            if (_output.CurrentPage != (double)output.CurrentPage)
                this.FireNotification(NotificationID.CurrentPage);
            if (_output.TotalPages != (double)output.TotalPages)
                this.FireNotification(NotificationID.TotalPages);
            if (_output.ViewNear != (double)output.ViewNear)
                this.FireNotification(NotificationID.ViewNear);
            if (_output.ViewFar == (double)output.ViewFar)
                return;
            this.FireNotification(NotificationID.ViewFar);
        }

        private void OnLayoutComplete(object sender)
        {
            this.OnLayoutOutputChanged();
            UIClass keyFocusDescendant = this._targetItem.UI.KeyFocusDescendant;
            if (keyFocusDescendant != null && keyFocusDescendant != this._targetItem.UI && this._targetItem.HasDescendant(keyFocusDescendant.RootItem))
                this._lastFocusedItem = keyFocusDescendant.RootItem;
            this.ClearPendingFocusAreaOfInterest();
            if (!this._useUserDisposition)
            {
                this.ActualScrollIntoViewDisposition.Reset();
                this.ActualScrollIntoViewDisposition.Enabled = false;
            }
            else
                this.ActualScrollIntoViewDisposition.CopyFrom(this._userDisposition);
            this._input.OnLayoutComplete();
            this.DeliverPostLayoutAction();
        }

        private void ClearPendingFocusAreaOfInterest()
        {
            if (this._uiWithAreaOfInterestToClear == null)
                return;
            if (!this._uiWithAreaOfInterestToClear.IsDisposed)
                this._uiWithAreaOfInterestToClear.ClearAreaOfInterest(AreaOfInterestID.PendingFocus);
            this._uiWithAreaOfInterestToClear = null;
        }

        private void SetPendingFocusAreaOfInterest(UIClass ui)
        {
            this.ClearPendingFocusAreaOfInterest();
            if (ui.DirectKeyFocus)
                return;
            ui.SetAreaOfInterest(AreaOfInterestID.PendingFocus);
            this._uiWithAreaOfInterestToClear = ui;
        }

        private bool HadFocus()
        {
            if (this._lastFocusedItem != null && this._lastFocusedItem.IsDisposed)
                this._lastFocusedItem = null;
            return this._lastFocusedItem != null;
        }

        private bool NonDefaultUserDisposition() => !this._userDisposition.IsDefault;

        private RectangleF GetLastFocusRect(bool forNavigation) => this.GetViewItemRect(this._lastFocusedItem, forNavigation);

        private RectangleF GetScrollerRect(bool forNavigation) => this.GetViewItemRect(this._targetItem, forNavigation);

        private RectangleF GetViewItemRect(ViewItem item, bool forNavigation)
        {
            if (!forNavigation)
                return RectangleF.FromRectangle(((ITrackableUIElement)item).EstimatePosition(_targetItem));
            Vector3 positionPxlVector;
            Vector3 sizePxlVector;
            ((INavigationSite)item).ComputeBounds(out positionPxlVector, out sizePxlVector);
            return new RectangleF(positionPxlVector.X, positionPxlVector.Y, sizePxlVector.X, sizePxlVector.Y);
        }

        private void NavigateToUI(UIClass ui)
        {
            ui.NotifyNavigationDestination(KeyFocusReason.Directional);
            this.SetPendingFocusAreaOfInterest(ui);
        }

        public void BeginCamp() => this._isCamping = true;

        public void EndCamp()
        {
            this._isCamping = false;
            this._canDoMoveUp = true;
            this._canDoMoveDown = true;
        }

        private bool CanMoveDirection(bool nearDirection) => nearDirection ? this._canDoMoveUp : this._canDoMoveDown;

        private void NoteFutileMoveAttempt(bool near)
        {
            if (!this._isCamping)
                return;
            if (near)
                this._canDoMoveUp = false;
            else
                this._canDoMoveDown = false;
        }

        private enum ItemLocation
        {
            OffscreenInNearDirection,
            Onscreen,
            OffscreenInFarDirection,
        }

        private abstract class PostLayoutAction
        {
            private ScrollModel _data;
            private bool _nearDirection;

            protected PostLayoutAction(ScrollModel data) => this._data = data;

            protected void Initialize(bool nearDirection) => this._nearDirection = nearDirection;

            protected bool NearDirection => this._nearDirection;

            protected ScrollModel Data => this._data;

            protected UIClass Origin => this._data._lastFocusedItem.UI;

            protected ViewItem Target => this._data._targetItem;

            protected void NoteFutileMoveAttempt() => this.Data.NoteFutileMoveAttempt(this._nearDirection);

            public abstract void Go();
        }

        private class NavigateAction : ScrollModel.PostLayoutAction
        {
            private Direction _direction;

            private NavigateAction(ScrollModel data)
              : base(data)
            {
            }

            public static ScrollModel.NavigateAction GetInstance(
              ScrollModel data,
              bool nearDirection,
              Direction direction)
            {
                if (data._navigateAction == null)
                    data._navigateAction = new ScrollModel.NavigateAction(data);
                ScrollModel.NavigateAction navigateAction = data._navigateAction;
                navigateAction.Initialize(nearDirection);
                navigateAction._direction = direction;
                return navigateAction;
            }

            public override void Go()
            {
                if (this.Origin == null || this.Origin.IsDisposed)
                    return;
                UIClass uiClass = this.Origin.DirectKeyFocus ? this.Origin : null;
                UIClass resultUI;
                if (this.Origin.FindNextFocusablePeer(this._direction, RectangleF.Zero, out resultUI) && resultUI != null && (resultUI != uiClass && this.Target.HasDescendant(resultUI.RootItem)))
                {
                    this.Data.NavigateToUI(resultUI);
                    this.Data.EnableScrollIntoView();
                }
                else
                    this.NoteFutileMoveAttempt();
            }
        }

        private class AssignFocusAction : ScrollModel.PostLayoutAction
        {
            private bool _tryNonBiasedSearchFirst;
            private PointF _assignPoint;
            private float _lockedPosition;

            private AssignFocusAction(ScrollModel data)
              : base(data)
            {
            }

            public static ScrollModel.AssignFocusAction GetInstance(
              ScrollModel data,
              PointF restorePoint,
              bool nearEndpoint,
              bool forceToEndpoint)
            {
                if (data._assignFocusAction == null)
                    data._assignFocusAction = new ScrollModel.AssignFocusAction(data);
                ScrollModel.AssignFocusAction assignFocusAction = data._assignFocusAction;
                assignFocusAction.Initialize(nearEndpoint);
                assignFocusAction.LockToEdge(nearEndpoint);
                assignFocusAction._assignPoint = restorePoint;
                assignFocusAction._tryNonBiasedSearchFirst = !forceToEndpoint;
                return assignFocusAction;
            }

            public override void Go()
            {
                UIClass ui = null;
                if (this._tryNonBiasedSearchFirst)
                    ui = this.FindNavigationResult(true);
                if (ui == null)
                    ui = this.FindNavigationResult(false);
                if (ui != null)
                    this.Data.NavigateToUI(ui);
                else
                    this.NoteFutileMoveAttempt();
                this.ApplyLockedPosition();
            }

            private UIClass FindNavigationResult(bool findNearest)
            {
                UIClass uiClass = null;
                INavigationSite result;
                bool fromPoint;
                if (findNearest)
                {
                    fromPoint = NavigationServices.FindFromPoint(Target, this._assignPoint, out result);
                }
                else
                {
                    Direction direction = this.Data.NearFarToDirection(this.NearDirection);
                    this._assignPoint = this.Data.GetMoveToEndpointPoint(this.NearDirection);
                    fromPoint = NavigationServices.FindFromPoint(Target, direction, this._assignPoint, out result);
                }
                if (fromPoint && result != null)
                {
                    ViewItem viewItem = result as ViewItem;
                    if (viewItem.UI != this.Origin || !this.Origin.DirectKeyFocus)
                        uiClass = viewItem.UI;
                }
                return uiClass;
            }

            private void LockToEdge(bool near)
            {
                if (near)
                    this._lockedPosition = 0.0f;
                else
                    this._lockedPosition = 1f;
            }

            protected void ApplyLockedPosition()
            {
                if (this.Data.NonDefaultUserDisposition())
                    this.Data._input.SecondaryScrollIntoViewDisposition = this.Data.ScrollIntoViewDisposition;
                this.Data.ActualScrollIntoViewDisposition.LockedPosition = this._lockedPosition;
                this.Data.ActualScrollIntoViewDisposition.LockedAlignment = this._lockedPosition;
                this.Data.ActualScrollIntoViewDisposition.Locked = true;
                this.Data.ActualScrollIntoViewDisposition.ContentPositioningBehavior = ContentPositioningPolicy.ShowMaximalContent;
                this.Data.ActualScrollIntoViewDisposition.Enabled = true;
                this.Data._useUserDisposition = true;
                this.Data.OnLayoutInputChanged();
            }
        }
    }
}
