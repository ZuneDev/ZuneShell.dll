// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Layouts.ScrollingLayoutInput
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layout;
using Microsoft.Iris.Library;

namespace Microsoft.Iris.Layouts
{
    internal class ScrollingLayoutInput : ILayoutInput
    {
        private static readonly DataCookie s_dataProperty = DataCookie.ReserveSlot();
        private int _scrollAmount;
        private int _pendingScrollAmount;
        private int _pendingPageCommands;
        private float _pendingScrollPosition;
        private bool _havePendingScrollPosition;
        private bool _enabled;
        private float _pageStep;
        private ScrollIntoViewDisposition _scrollIntoView;
        private ScrollIntoViewDisposition _secondaryScrollIntoView;

        public ScrollingLayoutInput()
        {
            this._pageStep = 1f;
            this._enabled = true;
            this._scrollIntoView = new ScrollIntoViewDisposition();
        }

        public bool Enabled
        {
            get => this._enabled;
            set
            {
                if (value == this._enabled)
                    return;
                this._enabled = value;
                if (!value)
                    return;
                this._scrollAmount = 0;
                this._pendingScrollAmount = 0;
                this._pendingPageCommands = 0;
                this._pendingScrollPosition = 0.0f;
                this._havePendingScrollPosition = false;
            }
        }

        public void Scroll(int amount) => this._scrollAmount += amount;

        public void PageUp() => --this._pendingPageCommands;

        public void PageDown() => ++this._pendingPageCommands;

        public void Home() => this.ScrollToPosition(0.0f);

        public void End() => this.ScrollToPosition(1f);

        public void ScrollToPosition(float position)
        {
            this._havePendingScrollPosition = true;
            this._pendingScrollPosition = position;
        }

        public bool GetPendingScrollPosition(out float position)
        {
            if (!this._havePendingScrollPosition)
            {
                position = 0.0f;
                return false;
            }
            position = this._pendingScrollPosition;
            return true;
        }

        public bool GetPendingPageRequests(out int amount)
        {
            amount = this._pendingPageCommands;
            return this._pendingPageCommands != 0;
        }

        internal int ScrollAmount => this._scrollAmount;

        internal void SetScrollAmount(int scrollAmount) => this._pendingScrollAmount = scrollAmount;

        internal void OnLayoutComplete()
        {
            this.SecondaryScrollIntoViewDisposition = (ScrollIntoViewDisposition)null;
            this._havePendingScrollPosition = false;
            this._pendingPageCommands = 0;
            this._scrollAmount = this._pendingScrollAmount;
        }

        internal float PageStep
        {
            get => this._pageStep;
            set => this._pageStep = value;
        }

        public ScrollIntoViewDisposition ScrollIntoViewDisposition
        {
            get => this._scrollIntoView;
            set => this._scrollIntoView = value;
        }

        public ScrollIntoViewDisposition SecondaryScrollIntoViewDisposition
        {
            get => this._secondaryScrollIntoView;
            set => this._secondaryScrollIntoView = value;
        }

        DataCookie ILayoutInput.Data => ScrollingLayoutInput.Data;

        public static DataCookie Data => ScrollingLayoutInput.s_dataProperty;

        public override string ToString() => InvariantString.Format("{0}(ScrollAmount={1}, PageAmount={2}, PageStep={3}, Disposition=({4}))", (object)this.GetType().Name, (object)this._pendingScrollAmount, (object)this._pendingPageCommands, (object)this._pageStep, (object)this._scrollIntoView);
    }
}
