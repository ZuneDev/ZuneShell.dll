// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Layout.SharedSize
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using System;

namespace Microsoft.Iris.Layout
{
    internal class SharedSize : NotifyObjectBase
    {
        private Size _minimumSize;
        private Size _maximumSize;
        private Size _desiredSize;
        private bool _accumulatingSize;
        private bool _applyPending;
        private Vector<ViewItem> _dependents;

        public SharedSize()
        {
            this._minimumSize = Size.Zero;
            this._maximumSize = Size.Zero;
            this._desiredSize = Size.Zero;
            this._accumulatingSize = true;
        }

        public Size MaximumSize
        {
            get => this._maximumSize;
            set
            {
                if (!(this._maximumSize != value))
                    return;
                Size size1 = this.Size;
                this._maximumSize = value;
                Size size2 = this.Size;
                this.FireNotification(NotificationID.MaximumSize);
                if (!(size1 != size2))
                    return;
                this.FireNotification(NotificationID.Size);
                this.InvalidateDependents(true);
            }
        }

        public Size MinimumSize
        {
            get => this._minimumSize;
            set
            {
                if (!(this._minimumSize != value))
                    return;
                Size size1 = this.Size;
                this._minimumSize = value;
                Size size2 = this.Size;
                this.FireNotification(NotificationID.MinimumSize);
                if (!(size1 != size2))
                    return;
                this.FireNotification(NotificationID.Size);
                this.InvalidateDependents(true);
            }
        }

        public Size Size
        {
            get
            {
                Size size = Size.Max(this._desiredSize, this.MinimumSize);
                Size maximumSize = this.MaximumSize;
                if (maximumSize.Width > 0)
                    size.Width = Math.Min(size.Width, maximumSize.Width);
                if (maximumSize.Height > 0)
                    size.Height = Math.Min(size.Height, maximumSize.Height);
                return size;
            }
            set => this.SetSize(value, true);
        }

        public void AutoSize()
        {
            this.SetSize(Size.Zero, false);
            this.InvalidateDependents(true);
        }

        private void SetSize(Size value, bool stopAccumulating)
        {
            if (this._desiredSize != value)
            {
                Size size1 = this.Size;
                this._desiredSize = value;
                Size size2 = this.Size;
                if (size1 != size2)
                {
                    this.FireNotification(NotificationID.Size);
                    if (stopAccumulating)
                        this.InvalidateDependents(true);
                }
            }
            this._accumulatingSize = !stopAccumulating;
        }

        public void Register(ViewItem viewitem)
        {
            if (this._dependents == null)
                this._dependents = new Vector<ViewItem>();
            this._dependents.Add(viewitem);
        }

        public void Unregister(ViewItem viewitem) => this._dependents.Remove(viewitem);

        public void AdjustConstraint(ref Size constraint, ref Size minSize, SharedSizePolicy policy)
        {
            Size size1 = this.Size;
            Size maximumSize = this.MaximumSize;
            Size size2 = constraint;
            Size size3 = size1;
            Size size4 = minSize;
            if ((policy & SharedSizePolicy.SharesWidth) != 0)
            {
                if (!this._accumulatingSize)
                {
                    int num = Math.Max(size4.Width, Math.Min(size1.Width, size2.Width));
                    if (num >= this.MinimumSize.Width && (num <= maximumSize.Width || maximumSize.Width == 0))
                    {
                        size4.Width = num;
                        size2.Width = num;
                    }
                    else
                    {
                        size2.Width = 0;
                        size4.Width = 1;
                    }
                }
                else
                {
                    if (maximumSize.Width != 0)
                        size2.Width = Math.Min(maximumSize.Width, size2.Width);
                    size4.Width = Math.Max(size3.Width, size4.Width);
                }
            }
            if ((policy & SharedSizePolicy.SharesHeight) != 0)
            {
                if (!this._accumulatingSize)
                {
                    int num = Math.Max(size4.Height, Math.Min(size1.Height, size2.Height));
                    if (num >= this.MinimumSize.Height && (num <= maximumSize.Height || maximumSize.Height == 0))
                    {
                        size4.Height = num;
                        size2.Height = num;
                    }
                    else
                    {
                        size2.Height = 0;
                        size4.Height = 1;
                    }
                }
                else
                {
                    if (maximumSize.Height != 0)
                        size2.Height = Math.Min(maximumSize.Height, size2.Height);
                    size4.Height = Math.Max(size3.Height, size4.Height);
                }
            }
            constraint = size2;
            minSize = size4;
        }

        public void AccumulateSize(Size size, SharedSizePolicy policy)
        {
            if (!this._accumulatingSize)
                return;
            Size size1 = this.Size;
            if ((policy & SharedSizePolicy.ContributesToWidth) != 0 && size.Width > size1.Width)
                size1.Width = size.Width;
            if ((policy & SharedSizePolicy.ContributesToHeight) != 0 && size.Height > size1.Height)
                size1.Height = size.Height;
            this.SetSize(size1, false);
            this.EnsureApplySize();
        }

        private void EnsureApplySize()
        {
            if (this._applyPending)
                return;
            this._applyPending = true;
            DeferredCall.Post(DispatchPriority.LayoutPass2, new SimpleCallback(this.ApplySize));
        }

        private void ApplySize()
        {
            this._accumulatingSize = false;
            this._applyPending = false;
            this.InvalidateDependents(false);
        }

        private void InvalidateDependents(bool forceInvalid)
        {
            if (this._dependents == null)
                return;
            Size size1 = this.Size;
            foreach (ViewItem dependent in this._dependents)
            {
                if (forceInvalid)
                {
                    dependent.MarkLayoutInvalid();
                }
                else
                {
                    SharedSizePolicy sharedSizePolicy = dependent.SharedSizePolicy;
                    Size size2 = ((ILayoutNode)dependent).DesiredSize - dependent.Margins.Size;
                    if (size1.Width != size2.Width && (sharedSizePolicy & SharedSizePolicy.SharesWidth) != 0 || size1.Height != size2.Height && (sharedSizePolicy & SharedSizePolicy.SharesHeight) != 0)
                        dependent.MarkLayoutInvalid();
                }
            }
        }
    }
}
