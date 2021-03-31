// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ModelItems.TextScrollModel
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.Session;
using System;

namespace Microsoft.Iris.ModelItems
{
    internal class TextScrollModel : ScrollModelBase
    {
        private ITextScrollModelCallback _handler;
        private int _scrollStep;
        private int _min;
        private int _extent;
        private int _viewExtent;
        private int _scrollAmount;
        private bool _canScrollUp;
        private bool _canScrollDown;

        public void AttachCallbacks(ITextScrollModelCallback handler)
        {
            if (this._handler != null)
                ErrorManager.ReportError("TextScrollModel can't be attached to multiple TextEditingHandlers");
            this._handler = handler;
        }

        public void DetachCallbacks() => this._handler = (ITextScrollModelCallback)null;

        public override int ScrollStep
        {
            get => this._scrollStep;
            set
            {
                if (this._scrollStep == value)
                    return;
                this._scrollStep = value;
                this.FireNotification(NotificationID.ScrollStep);
            }
        }

        public override void Scroll(int amount)
        {
            if (this._handler == null || this.AvailableScrollSpace <= 0)
                return;
            this._handler.ScrollToPosition(this, Math2.Clamp(this._scrollAmount + amount, 0, this.AvailableScrollSpace));
        }

        public override void ScrollUp()
        {
            if (this._handler == null)
                return;
            if (this._scrollStep == 0)
                this._handler.ScrollUp(this);
            else
                this.Scroll(-this._scrollStep);
        }

        public override void ScrollDown()
        {
            if (this._handler == null)
                return;
            if (this._scrollStep == 0)
                this._handler.ScrollDown(this);
            else
                this.Scroll(this._scrollStep);
        }

        public override void PageUp()
        {
            if (this._handler == null)
                return;
            this._handler.PageUp(this);
        }

        public override void PageDown()
        {
            if (this._handler == null)
                return;
            this._handler.PageDown(this);
        }

        public override void Home()
        {
            if (this._handler == null || this.AvailableScrollSpace <= 0)
                return;
            this._handler.ScrollToPosition(this, 0);
        }

        public override void End()
        {
            if (this._handler == null || this.AvailableScrollSpace <= 0)
                return;
            this._handler.ScrollToPosition(this, this.AvailableScrollSpace);
        }

        public override void ScrollToPosition(float scrollAmount)
        {
            if (this._handler == null || this.AvailableScrollSpace <= 0)
                return;
            this._handler.ScrollToPosition(this, (int)((double)this.AvailableScrollSpace * (double)scrollAmount));
        }

        public override bool CanScrollUp => this._canScrollUp;

        public override bool CanScrollDown => this._canScrollDown;

        public override float CurrentPage => this._viewExtent != 0 ? (float)((double)this._scrollAmount / (double)this._viewExtent + 1.0) : 0.0f;

        public override float TotalPages => this._viewExtent != 0 ? (float)((double)this.AvailableScrollSpace / (double)this._viewExtent + 1.0) : 0.0f;

        public override float ViewNear => this._extent != 0 ? Math.Max((float)this._scrollAmount / (float)this._extent, 0.0f) : 0.0f;

        public override float ViewFar => this._extent != 0 ? Math.Min((float)(this._scrollAmount + this._viewExtent) / (float)this._extent, 1f) : 0.0f;

        private int AvailableScrollSpace => this._extent - this._viewExtent + 1;

        public void UpdateState(TextScrollModel.State newState)
        {
            if (newState.ViewStuffValid)
            {
                bool flag = false;
                if (newState.Min != this._min)
                {
                    this._min = newState.Min;
                    flag = true;
                }
                if (newState.Extent != this._extent)
                {
                    this._extent = newState.Extent;
                    flag = true;
                }
                if (newState.ViewExtent != this._viewExtent)
                {
                    this._viewExtent = newState.ViewExtent;
                    flag = true;
                }
                int num = newState.ScrollAmount;
                if (num + this._viewExtent > this._extent)
                    num = this._extent - this._viewExtent;
                if (num != this._scrollAmount)
                {
                    this._scrollAmount = num;
                    flag = true;
                }
                if (flag)
                {
                    this.FireNotification(NotificationID.ViewNear);
                    this.FireNotification(NotificationID.ViewFar);
                    this.FireNotification(NotificationID.CurrentPage);
                    this.FireNotification(NotificationID.TotalPages);
                }
            }
            if (!newState.EnabledStuffValid)
                return;
            if (newState.CanScrollUp != this._canScrollUp)
            {
                this._canScrollUp = newState.CanScrollUp;
                this.FireNotification(NotificationID.CanScrollUp);
            }
            if (newState.CanScrollDown == this._canScrollDown)
                return;
            this._canScrollDown = newState.CanScrollDown;
            this.FireNotification(NotificationID.CanScrollDown);
        }

        public struct State
        {
            private bool _viewStuffValid;
            private bool _enabledStuffValid;
            private bool _canScrollUp;
            private bool _canScrollDown;
            private int _min;
            private int _extent;
            private int _viewExtent;
            private int _scrollAmount;

            public int Min
            {
                get => this._min;
                set
                {
                    this._min = value;
                    this._viewStuffValid = true;
                }
            }

            public int Extent
            {
                get => this._extent;
                set
                {
                    this._extent = value;
                    this._viewStuffValid = true;
                }
            }

            public int ViewExtent
            {
                get => this._viewExtent;
                set
                {
                    this._viewExtent = value;
                    this._viewStuffValid = true;
                }
            }

            public int ScrollAmount
            {
                get => this._scrollAmount;
                set
                {
                    this._scrollAmount = value;
                    this._viewStuffValid = true;
                }
            }

            public bool CanScrollUp
            {
                get => this._canScrollUp;
                set
                {
                    this._canScrollUp = value;
                    this._enabledStuffValid = true;
                }
            }

            public bool CanScrollDown
            {
                get => this._canScrollDown;
                set
                {
                    this._canScrollDown = value;
                    this._enabledStuffValid = true;
                }
            }

            public bool EnabledStuffValid => this._enabledStuffValid;

            public bool ViewStuffValid => this._viewStuffValid;
        }
    }
}
