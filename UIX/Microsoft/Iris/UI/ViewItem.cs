// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.UI.ViewItem
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Data;
using Microsoft.Iris.Debug;
using Microsoft.Iris.Drawing;
using Microsoft.Iris.Input;
using Microsoft.Iris.Layout;
using Microsoft.Iris.Layouts;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.ModelItems;
using Microsoft.Iris.Navigation;
using Microsoft.Iris.Render;
using Microsoft.Iris.RenderAPI.Drawing;
using Microsoft.Iris.Session;
using Microsoft.Iris.ViewItems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;

namespace Microsoft.Iris.UI
{
    internal abstract class ViewItem :
      Microsoft.Iris.Library.TreeNode,
      INotifyObject,
      INavigationSite,
      IZoneDisplayChild,
      ITrackableUIElement,
      ITrackableUIElementEvents,
      IAnimatableOwner,
      ILayoutNode
    {
        private static readonly DataCookie s_maxSizeProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_minSizeProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_marginsProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_paddingProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_alignmentProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_childAlignmentProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_sharedSizeProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_sharedSizePolicyProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_layoutOutputProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_scaleProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_alphaProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_rotationProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_centerPointPercentProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_layerProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_activeAnimationsProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_animationBuildersProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_idleAnimationsProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_animationHandlesProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_navModeProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_navCacheProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_focusOrderProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_nameProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_effectProperty = DataCookie.ReserveSlot();
        protected static readonly DataCookie s_scaleNoSendProperty = DataCookie.ReserveSlot();
        protected static readonly DataCookie s_alphaNoSendProperty = DataCookie.ReserveSlot();
        protected static readonly DataCookie s_rotationNoSendProperty = DataCookie.ReserveSlot();
        protected static readonly DataCookie s_positionNoSendProperty = DataCookie.ReserveSlot();
        protected static readonly DataCookie s_sizeNoSendProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_cameraProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_debugOutlineProperty = DataCookie.ReserveSlot();
        private static SingleArrayListCache s_pathListCache;
        private static EventCookie s_layoutCompleteEvent = EventCookie.ReserveSlot();
        private static EventCookie s_paintEvent = EventCookie.ReserveSlot();
        private static EventCookie s_paintInvalidEvent = EventCookie.ReserveSlot();
        private static EventCookie s_propertyChangedEvent = EventCookie.ReserveSlot();
        private static EventCookie s_deepLayoutChangeEvent = EventCookie.ReserveSlot();
        private UIClass _ownerUI;
        private ILayout _layout;
        private int _uiID;
        private BitVector32 _bits;
        private BitVector32 _bits2;
        private NotifyService _notifier = new NotifyService();
        private IVisualContainer _container;
        private ISprite _backgroundSprite;
        private Color _backgroundColor;
        private int _visibleChildCount;
        private Size _constraint;
        private Size _desiredSize;
        private object _measureData;
        private Size _alignedSize;
        private Point _measureAlignment;
        private LayoutSlot _slot;
        private Rectangle _bounds;
        private Vector3 _scale = Vector3.UnitVector;
        private Rotation _rotation = Rotation.Default;
        private Rectangle _location;
        private Visibility _visible;
        private Point _alignedOffset;
        private Vector<AreaOfInterest> _externallySetAreasOfInterest;
        private AreaOfInterestID _ownedAreasOfInterest;
        private AreaOfInterestID _containedAreasOfInterest;
        private int _requestedCount;
        private Vector<int> _requestedIndices;
        private ExtendedLayoutOutput _extendedOutputs;
        private static DeferredHandler s_scrollIntoViewCleanup = new DeferredHandler(ViewItem.CleanUpAfterScrollIntoView);

        public ViewItem()
        {
            this._bits = new BitVector32();
            this._bits2 = new BitVector32();
            this.SetBit(ViewItem.Bits.LayoutInputVisible, true);
            this._layout = (ILayout)DefaultLayout.Instance;
            this._backgroundColor = Color.Transparent;
        }

        protected override void OnOwnerDeclared(object owner)
        {
            this._ownerUI = (UIClass)owner;
            this._uiID = this._ownerUI.AllocateUIID(this);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            this.RemoveEventHandlers(ViewItem.s_layoutCompleteEvent);
            this.RemoveEventHandlers(ViewItem.s_paintEvent);
            this.RemoveEventHandlers(ViewItem.s_paintInvalidEvent);
            this.RemoveEventHandlers(ViewItem.s_propertyChangedEvent);
            this.RemoveEventHandlers(ViewItem.s_deepLayoutChangeEvent);
            this.SharedSize?.Unregister(this);
            Vector<ActiveSequence> activeAnimations = this.GetActiveAnimations(false);
            if (activeAnimations != null)
            {
                foreach (DisposableObject disposableObject in activeAnimations)
                    disposableObject.Dispose((object)this);
            }
            Vector<ActiveSequence> idleAnimations = this.GetIdleAnimations(false);
            if (idleAnimations != null)
            {
                foreach (DisposableObject disposableObject in idleAnimations)
                    disposableObject.Dispose((object)this);
            }
            this.Effect?.DoneWithRenderEffects((object)this);
            this._ownerUI = (UIClass)null;
            this._layout = (ILayout)null;
            if (!this.GetBit(ViewItem.Bits2.HasCamera))
                return;
            Camera data = (Camera)this.GetData(ViewItem.s_cameraProperty);
            if (data == null)
                return;
            this.SetData(ViewItem.s_cameraProperty, (object)null);
            data.UnregisterUsage((object)this);
        }

        protected void FireNotification(string id) => this._notifier.Fire(id);

        public void AddListener(Listener listener) => this._notifier.AddListener(listener);

        public bool HasVisual => this._container != null;

        public IVisual RendererVisual => (IVisual)this._container;

        IAnimatable IAnimatableOwner.AnimationTarget => (IAnimatable)this._container;

        public virtual Color Background
        {
            get => this._backgroundColor;
            set
            {
                if (!(this._backgroundColor != value))
                    return;
                this.ForceContentChange();
                this._backgroundColor = value;
                this.MarkPaintInvalid();
                this.FireNotification(NotificationID.Background);
            }
        }

        protected virtual ISprite ContentVisual => this._backgroundSprite;

        public virtual void OrphanVisuals(OrphanedVisualCollection orphans)
        {
            if (this._container != null)
            {
                orphans.AddOrphan((IVisual)this._container);
                this._container.UnregisterUsage((object)this);
                this._container = (IVisualContainer)null;
            }
            this.DisposeBackgroundContent(false);
        }

        public Vector2 VisualSize
        {
            get => this.GetBit(ViewItem.Bits2.HasSizeNoSend) ? (Vector2)this.GetData(ViewItem.s_sizeNoSendProperty) : this._container.Size;
            set
            {
                bool bit = this.GetBit(ViewItem.Bits2.HasSizeNoSend);
                this._container.SetSize(value, bit);
                if (bit)
                    this.SetDynamicValue((object)value, true, ViewItem.Bits2.HasSizeNoSend, ViewItem.s_sizeNoSendProperty, nameof(VisualSize));
                this.MarkPaintInvalid();
            }
        }

        private Vector2 VisualSizeNoSend
        {
            set => this.SetDynamicValue((object)value, false, ViewItem.Bits2.HasSizeNoSend, ViewItem.s_sizeNoSendProperty, "VisualSize");
        }

        public Vector3 VisualPosition
        {
            get => this.GetBit(ViewItem.Bits2.HasPositionNoSend) ? (Vector3)this.GetData(ViewItem.s_positionNoSendProperty) : this._container.Position;
            set
            {
                bool bit = this.GetBit(ViewItem.Bits2.HasPositionNoSend);
                this._container.SetPosition(value, bit);
                if (!bit)
                    return;
                this.SetDynamicValue((object)value, true, ViewItem.Bits2.HasPositionNoSend, ViewItem.s_positionNoSendProperty, nameof(VisualPosition));
            }
        }

        private Vector3 VisualPositionNoSend
        {
            set => this.SetDynamicValue((object)value, false, ViewItem.Bits2.HasPositionNoSend, ViewItem.s_positionNoSendProperty, "VisualPosition");
        }

        public Vector3 VisualScale
        {
            get => this.GetBit(ViewItem.Bits2.HasScaleNoSend) ? (Vector3)this.GetData(ViewItem.s_scaleNoSendProperty) : this._container.Scale;
            set
            {
                bool bit = this.GetBit(ViewItem.Bits2.HasScaleNoSend);
                this._container.SetScale(value, bit);
                if (!bit)
                    return;
                this.SetDynamicValue((object)value, true, ViewItem.Bits2.HasScaleNoSend, ViewItem.s_scaleNoSendProperty, nameof(VisualScale));
            }
        }

        private Vector3 VisualScaleNoSend
        {
            set => this.SetDynamicValue((object)value, false, ViewItem.Bits2.HasScaleNoSend, ViewItem.s_scaleNoSendProperty, "VisualScale");
        }

        public Rotation VisualRotation
        {
            get
            {
                if (this.GetBit(ViewItem.Bits2.HasRotationNoSend))
                    return (Rotation)this.GetData(ViewItem.s_rotationNoSendProperty);
                AxisAngle rotation = this._container.Rotation;
                return new Rotation(rotation.Angle, rotation.Axis);
            }
            set
            {
                bool bit = this.GetBit(ViewItem.Bits2.HasRotationNoSend);
                this._container.SetRotation(new AxisAngle(value.Axis, value.AngleRadians), bit);
                if (!bit)
                    return;
                this.SetDynamicValue((object)value, true, ViewItem.Bits2.HasRotationNoSend, ViewItem.s_rotationNoSendProperty, nameof(VisualRotation));
            }
        }

        private Rotation VisualRotationNoSend
        {
            set => this.SetDynamicValue((object)value, false, ViewItem.Bits2.HasRotationNoSend, ViewItem.s_rotationNoSendProperty, "VisualRotation");
        }

        public float VisualAlpha
        {
            get => this.GetBit(ViewItem.Bits2.HasAlphaNoSend) ? (float)this.GetData(ViewItem.s_alphaNoSendProperty) : this._container.Alpha;
            set
            {
                bool fullyVisible = this.FullyVisible;
                bool bit = this.GetBit(ViewItem.Bits2.HasAlphaNoSend);
                this._container.SetAlpha(value, bit);
                if (bit)
                    this.SetDynamicValue((object)value, true, ViewItem.Bits2.HasAlphaNoSend, ViewItem.s_alphaNoSendProperty, nameof(VisualAlpha));
                if (fullyVisible == this.FullyVisible)
                    return;
                this.OnVisibilityChange();
            }
        }

        private float VisualAlphaNoSend
        {
            set
            {
                bool fullyVisible = this.FullyVisible;
                this.SetDynamicValue((object)value, false, ViewItem.Bits2.HasAlphaNoSend, ViewItem.s_alphaNoSendProperty, "VisualAlpha");
                if (fullyVisible == this.FullyVisible)
                    return;
                this.OnVisibilityChange();
            }
        }

        public Vector3 VisualCenterPoint
        {
            get => this._container.CenterPoint;
            set => this._container.CenterPoint = value;
        }

        public uint VisualLayer
        {
            get => this._container.Layer;
            set => this._container.Layer = value;
        }

        private void SetDynamicValue(
          object value,
          bool fValueIsDefault,
          ViewItem.Bits2 bit,
          DataCookie cookie,
          string stTracePropertyName)
        {
            if (fValueIsDefault)
                this.SetData(cookie, (object)null);
            else
                this.SetData(cookie, value);
            this.SetBit(bit, !fValueIsDefault);
        }

        public override bool IsRoot => this.IsZoned && this.Zone.RootViewItem == this;

        public ViewItem Parent => (ViewItem)base.Parent;

        public virtual bool HideNamedChildren => false;

        public UIClass UI => this._ownerUI;

        protected override void OnZoneAttached()
        {
            this.UI.UpdateMouseHandling(this);
            this.MarkLayoutInvalid();
            if (this.GetBit(ViewItem.Bits.OutputSelfDirty))
                this.MarkLayoutOutputDirty(true);
            this.OnEffectiveScaleChange();
        }

        protected override void OnChildrenChanged()
        {
            base.OnChildrenChanged();
            this.MarkLayoutInvalid();
        }

        public void MarkPaintInvalid()
        {
            if (!this.IsZoned || this.GetBit(ViewItem.Bits.PaintInvalid) || !this.HasVisual)
                return;
            this.SetBit(ViewItem.Bits.PaintInvalid, true);
            if (!this.IsRoot)
                this.Parent.MarkPaintChildrenInvalid();
            this.Zone.ScheduleUiTask(UiTask.Painting);
            if (!(this.GetEventHandler(ViewItem.s_paintInvalidEvent) is EventHandler eventHandler))
                return;
            eventHandler((object)this, EventArgs.Empty);
        }

        private void MarkPaintChildrenInvalid()
        {
            if (this.ChildrenPaintInvalid)
                return;
            this.SetBit(ViewItem.Bits2.PaintChildrenInvalid, true);
            if (this.IsRoot)
                return;
            this.Parent.MarkPaintChildrenInvalid();
        }

        public bool ChildrenPaintInvalid => this.GetBit(ViewItem.Bits2.PaintChildrenInvalid);

        [Conditional("DEBUG")]
        private static void DEBUG_CountPaintInvalid()
        {
        }

        [Conditional("DEBUG")]
        internal static void DEBUG_ClearPaintInvalid()
        {
        }

        public void ResendExistingContentTree()
        {
            if (this.HasVisual)
                this.MarkPaintInvalid();
            foreach (ViewItem child in this.Children)
                child.ResendExistingContentTree();
        }

        public void OnVisibilityChange() => this.ResendExistingContentTree();

        public bool FullyVisible
        {
            get
            {
                if (!this.HasVisual || !this.Visible)
                    return false;
                return (double)this.VisualAlpha > 0.0 || this.GetBit(ViewItem.Bits2.IsAlphaAnimationPlaying);
            }
        }

        public void PaintTree(bool visible)
        {
            visible &= this.FullyVisible;
            this.PaintSelf(visible);
            if (!this.ChildrenPaintInvalid)
                return;
            this.SetBit(ViewItem.Bits2.PaintChildrenInvalid, false);
            foreach (ViewItem child in this.Children)
                child.PaintTree(visible);
        }

        private void PaintSelf(bool visible)
        {
            if (!this.GetBit(ViewItem.Bits.PaintInvalid))
                return;
            this.SetBit(ViewItem.Bits.PaintInvalid, false);
            if (!this.HasVisual)
                return;
            this.OnPaint(visible);
        }

        protected virtual void OnPaint(bool visible)
        {
            if (this.GetEventHandler(ViewItem.s_paintEvent) is ViewItem.PaintHandler eventHandler)
                eventHandler(this);
            bool flag = visible && this._backgroundColor.A != (byte)0;
            if (flag && this._backgroundSprite == null)
                this.CreateBackgroundContent();
            else if (!flag && this._backgroundSprite != null)
                this.DisposeBackgroundContent(true);
            if (this._backgroundSprite == null)
                return;
            this._backgroundSprite.Effect.SetProperty("ColorElem.Color", this._backgroundColor.RenderConvert());
        }

        public event ViewItem.PaintHandler Paint
        {
            add => this.AddEventHandler(ViewItem.s_paintEvent, (Delegate)value);
            remove => this.RemoveEventHandler(ViewItem.s_paintEvent, (Delegate)value);
        }

        public event EventHandler PaintInvalid
        {
            add => this.AddEventHandler(ViewItem.s_paintInvalidEvent, (Delegate)value);
            remove => this.RemoveEventHandler(ViewItem.s_paintInvalidEvent, (Delegate)value);
        }

        protected virtual void DisposeAllContent() => this.DisposeBackgroundContent(true);

        private void DisposeBackgroundContent(bool removeFromTree)
        {
            if (this._backgroundSprite == null)
                return;
            if (removeFromTree)
                this._backgroundSprite.Remove();
            this._backgroundSprite.UnregisterUsage((object)this);
            this._backgroundSprite = (ISprite)null;
        }

        private void CreateBackgroundContent()
        {
            this._backgroundSprite = UISession.Default.RenderSession.CreateSprite((object)this, (object)this);
            this.VisualContainer.AddChild((IVisual)this._backgroundSprite, (IVisual)null, VisualOrder.Last);
            IEffect colorFillEffect = EffectManager.CreateColorFillEffect((object)this, this._backgroundColor);
            this._backgroundSprite.Effect = colorFillEffect;
            this._backgroundSprite.RelativeSize = true;
            this._backgroundSprite.Size = Vector2.UnitVector;
            colorFillEffect.UnregisterUsage((object)this);
        }

        public Vector3 Scale
        {
            get
            {
                Vector3 vector3 = Vector3.UnitVector;
                if (this.GetBit(ViewItem.Bits.HasScale))
                {
                    object data = this.GetData(ViewItem.s_scaleProperty);
                    if (data != null)
                        vector3 = (Vector3)data;
                }
                return vector3;
            }
            set
            {
                if (!(value != this.Scale))
                    return;
                if (value != Vector3.UnitVector)
                {
                    this.SetData(ViewItem.s_scaleProperty, (object)value);
                    this.SetBit(ViewItem.Bits.HasScale, true);
                }
                else if (this.GetBit(ViewItem.Bits.HasScale))
                {
                    this.SetData(ViewItem.s_scaleProperty, (object)null);
                    this.SetBit(ViewItem.Bits.HasScale, false);
                }
                if (this.HasVisual)
                {
                    var args = new AnimationArgs(this)
                    {
                        NewScale = this._scale * value
                    };
                    this.ApplyAnimatableValue(AnimationEventType.Scale, ref args);
                }
                else
                    this.MarkLayoutInvalid();
                this.NotifyEffectiveScaleChange(false);
                this.FireNotification(NotificationID.Scale);
            }
        }

        public float Alpha
        {
            get
            {
                float num = 1f;
                if (this.GetBit(ViewItem.Bits2.HasAlpha))
                    num = (float)this.GetData(ViewItem.s_alphaProperty);
                return num;
            }
            set
            {
                float alpha = this.Alpha;
                if ((double)value == (double)alpha)
                    return;
                bool flag = Math2.WithinEpsilon(value, 1f);
                object obj = flag ? (object)null : (object)value;
                this.SetData(ViewItem.s_alphaProperty, obj);
                this.SetBit(ViewItem.Bits2.HasAlpha, !flag);
                if (this.HasVisual)
                {
                    var args = new AnimationArgs(this)
                    {
                        OldAlpha = this.VisualAlpha,
                        NewAlpha = value
                    };
                    this.ApplyAnimatableValue(AnimationEventType.Alpha, ref args);
                    if ((double)alpha == 0.0 || (double)value == 0.0)
                        this.OnVisibilityChange();
                }
                this.FireNotification(NotificationID.Alpha);
            }
        }

        public Camera Camera
        {
            get
            {
                Camera camera = (Camera)null;
                if (this.GetBit(ViewItem.Bits2.HasCamera))
                    camera = (Camera)this.GetData(ViewItem.s_cameraProperty);
                return camera;
            }
            set
            {
                if (value == this.Camera)
                    return;
                bool flag = value == null;
                object obj = flag ? (object)(Camera)null : (object)value;
                this.SetData(ViewItem.s_cameraProperty, obj);
                this.SetBit(ViewItem.Bits2.HasCamera, !flag);
                value?.RegisterUsage((object)this);
                if (this._container != null)
                    this._container.Camera = value == null ? (ICamera)null : value.APICamera;
                this.FireNotification(NotificationID.Camera);
            }
        }

        public Rotation Rotation
        {
            get
            {
                Rotation data = Rotation.Default;
                if (this.GetBit(ViewItem.Bits2.HasRotation))
                    data = (Rotation)this.GetData(ViewItem.s_rotationProperty);
                return data;
            }
            set
            {
                if (!(value != this.Rotation))
                    return;
                bool flag = value == Rotation.Default;
                object obj = flag ? (object)null : (object)value;
                this.SetData(ViewItem.s_rotationProperty, obj);
                this.SetBit(ViewItem.Bits2.HasRotation, !flag);
                this.FireNotification(NotificationID.Rotation);
                this.MarkLayoutInvalid();
            }
        }

        public Vector3 CenterPointPercent
        {
            get
            {
                Vector3 vector3 = new Vector3();
                if (this.GetBit(ViewItem.Bits2.HasCenterPointPercent))
                    vector3 = (Vector3)this.GetData(ViewItem.s_centerPointPercentProperty);
                return vector3;
            }
            set
            {
                if (!(value != this.CenterPointPercent))
                    return;
                bool flag = value == new Vector3();
                object obj = flag ? (object)null : (object)value;
                this.SetData(ViewItem.s_centerPointPercentProperty, obj);
                this.SetBit(ViewItem.Bits2.HasCenterPointPercent, !flag);
                if (this.HasVisual)
                    this.VisualCenterPoint = value;
                this.FireNotification(NotificationID.CenterPointPercent);
            }
        }

        public uint Layer
        {
            get
            {
                uint num = 0;
                if (this.GetBit(ViewItem.Bits2.HasLayer))
                    num = (uint)this.GetData(ViewItem.s_layerProperty);
                return num;
            }
            set
            {
                if ((int)value == (int)this.Layer)
                    return;
                bool flag = value == 0U;
                object obj = flag ? (object)null : (object)value;
                this.SetData(ViewItem.s_layerProperty, obj);
                this.SetBit(ViewItem.Bits2.HasLayer, !flag);
                if (!this.HasVisual)
                    return;
                this.VisualLayer = value;
            }
        }

        public EffectClass Effect
        {
            get => this.GetBit(ViewItem.Bits2.HasEffect) ? (EffectClass)this.GetData(ViewItem.s_effectProperty) : (EffectClass)null;
            set
            {
                EffectClass effect = this.Effect;
                if (effect == value)
                    return;
                effect?.DoneWithRenderEffects((object)this);
                this.SetData(ViewItem.s_effectProperty, (object)value);
                this.SetBit(ViewItem.Bits2.HasEffect, value != null);
                this.OnEffectChanged();
                this.MarkPaintInvalid();
                this.FireNotification(NotificationID.Effect);
            }
        }

        protected virtual void OnEffectChanged()
        {
        }

        public Vector3 ComputeEffectiveScale()
        {
            Vector3 hostDisplayScale = this.Zone.HostDisplayScale;
            ViewItem viewItem = this;
            do
            {
                if (viewItem.HasVisual)
                {
                    Vector3 visualScale = viewItem.VisualScale;
                    hostDisplayScale *= new Vector3(visualScale.X, visualScale.Y, visualScale.Z);
                }
                else if (viewItem.GetBit(ViewItem.Bits.HasScale))
                    hostDisplayScale *= viewItem.Scale;
                viewItem = viewItem.Parent;
            }
            while (viewItem != null);
            return hostDisplayScale;
        }

        protected virtual void OnEffectiveScaleChange()
        {
        }

        internal void NotifyEffectiveScaleChange(bool forceFlag)
        {
            if (!this.IsZoned || !this.ChangeBit(ViewItem.Bits.ScaleChanged, true) && !forceFlag)
                return;
            this.Zone.ScheduleScaleChangeNotifications();
        }

        internal void DeliverEffectiveScaleChange(bool parentChangedFlag)
        {
            if (parentChangedFlag)
                this.SetBit(ViewItem.Bits.ScaleChanged, false);
            else
                parentChangedFlag = this.ChangeBit(ViewItem.Bits.ScaleChanged, false);
            if (parentChangedFlag)
                this.OnEffectiveScaleChange();
            foreach (ViewItem child in this.Children)
                child.DeliverEffectiveScaleChange(parentChangedFlag);
        }

        public bool Visible
        {
            get => this.GetBit(ViewItem.Bits.LayoutInputVisible);
            set
            {
                bool fullyVisible = this.FullyVisible;
                if (!this.ChangeBit(ViewItem.Bits.LayoutInputVisible, value))
                    return;
                this.MarkLayoutInvalid();
                if (fullyVisible != this.FullyVisible)
                    this.OnVisibilityChange();
                this.FireNotification(NotificationID.Visible);
            }
        }

        internal Size MaximumSize => (Size)this.MaximumSizeObject;

        public object MaximumSizeObject
        {
            get => this.GetBit(ViewItem.Bits.LayoutInputMaxSize) ? this.GetData(ViewItem.s_maxSizeProperty) : Size.ZeroBox;
            set
            {
                if (!(this.MaximumSize != (Size)value))
                    return;
                this.SetLayoutData(ViewItem.s_maxSizeProperty, ViewItem.Bits.LayoutInputMaxSize, value, Size.ZeroBox);
                this.FireNotification(NotificationID.MaximumSize);
            }
        }

        internal Size MinimumSize => (Size)this.MinimumSizeObject;

        public object MinimumSizeObject
        {
            get => this.GetBit(ViewItem.Bits.LayoutInputMinSize) ? this.GetData(ViewItem.s_minSizeProperty) : Size.ZeroBox;
            set
            {
                if (!(this.MinimumSize != (Size)value))
                    return;
                this.SetLayoutData(ViewItem.s_minSizeProperty, ViewItem.Bits.LayoutInputMinSize, value, Size.ZeroBox);
                this.FireNotification(NotificationID.MinimumSize);
                this.OnMinimumSizeChanged();
            }
        }

        protected virtual void OnMinimumSizeChanged()
        {
        }

        public ItemAlignment Alignment
        {
            get => this.GetBit(ViewItem.Bits.LayoutAlignment) ? (ItemAlignment)this.GetData(ViewItem.s_alignmentProperty) : ItemAlignment.Default;
            set
            {
                if (!(this.Alignment != value))
                    return;
                this.SetLayoutData(ViewItem.s_alignmentProperty, ViewItem.Bits.LayoutAlignment, (object)value, (object)ItemAlignment.Default);
                this.FireNotification(NotificationID.Alignment);
            }
        }

        public ItemAlignment ChildAlignment
        {
            get => this.GetBit(ViewItem.Bits.LayoutChildAlignment) ? (ItemAlignment)this.GetData(ViewItem.s_childAlignmentProperty) : ItemAlignment.Default;
            set
            {
                if (!(this.ChildAlignment != value))
                    return;
                this.SetLayoutData(ViewItem.s_childAlignmentProperty, ViewItem.Bits.LayoutChildAlignment, (object)value, (object)ItemAlignment.Default);
                this.FireNotification(NotificationID.ChildAlignment);
            }
        }

        internal ItemAlignment GetEffectiveAlignment()
        {
            ItemAlignment alignment = this.Alignment;
            if (this.Parent != null && (alignment.Horizontal == Microsoft.Iris.Layout.Alignment.Unspecified || alignment.Vertical == Microsoft.Iris.Layout.Alignment.Unspecified))
            {
                alignment = ItemAlignment.Merge(alignment, this.Parent.ChildAlignment);
                if (this.Parent.Layout != null)
                    alignment = ItemAlignment.Merge(alignment, this.Parent.Layout.DefaultChildAlignment);
            }
            return alignment;
        }

        public SharedSize SharedSize
        {
            get => this.GetBit(ViewItem.Bits.LayoutInputSharedSize) ? (SharedSize)this.GetData(ViewItem.s_sharedSizeProperty) : (SharedSize)null;
            set
            {
                SharedSize sharedSize = this.SharedSize;
                if (sharedSize == value)
                    return;
                sharedSize?.Unregister(this);
                this.SetLayoutData(ViewItem.s_sharedSizeProperty, ViewItem.Bits.LayoutInputSharedSize, (object)value, (object)null);
                value?.Register(this);
                this.FireNotification(NotificationID.SharedSize);
            }
        }

        public SharedSizePolicy SharedSizePolicy
        {
            get => this.GetBit(ViewItem.Bits.LayoutInputSharedSizePolicy) ? (SharedSizePolicy)this.GetData(ViewItem.s_sharedSizePolicyProperty) : SharedSizePolicy.Default;
            set
            {
                if (this.SharedSizePolicy == value)
                    return;
                this.SetLayoutData(ViewItem.s_sharedSizePolicyProperty, ViewItem.Bits.LayoutInputSharedSizePolicy, (object)value, (object)SharedSizePolicy.Default);
                this.FireNotification(NotificationID.SharedSizePolicy);
            }
        }

        public Inset Margins
        {
            get => this.GetInset(ViewItem.s_marginsProperty, ViewItem.Bits.LayoutInputMargins);
            set
            {
                if (!(this.Margins != value))
                    return;
                this.SetLayoutData(ViewItem.s_marginsProperty, ViewItem.Bits.LayoutInputMargins, (object)value, (object)Inset.Zero);
                this.FireNotification(NotificationID.Margins);
            }
        }

        public Inset Padding
        {
            get => this.GetInset(ViewItem.s_paddingProperty, ViewItem.Bits.LayoutInputPadding);
            set
            {
                if (!(this.Padding != value))
                    return;
                this.SetLayoutData(ViewItem.s_paddingProperty, ViewItem.Bits.LayoutInputPadding, (object)value, (object)Inset.Zero);
                this.FireNotification(NotificationID.Padding);
            }
        }

        private Inset GetInset(DataCookie changeProperty, ViewItem.Bits changeBit) => this.GetBit(changeBit) ? (Inset)this.GetData(changeProperty) : Inset.Zero;

        private void SetLayoutData(
          DataCookie changeProperty,
          ViewItem.Bits changeBit,
          object newValue,
          object emptyValue)
        {
            uint bitAsUint = this.GetBitAsUInt(changeBit);
            uint num = newValue.Equals(emptyValue) ? 0U : 1U;
            if (num == 0U && (int)bitAsUint == (int)num)
                return;
            switch (bitAsUint << 1 | num)
            {
                case 0:
                    return;
                case 1:
                    this.SetData(changeProperty, newValue);
                    this.SetBit(changeBit, true);
                    break;
                case 2:
                    this.SetData(changeProperty, (object)null);
                    this.SetBit(changeBit, false);
                    break;
                case 3:
                    if (this.GetData(changeProperty) == newValue)
                        return;
                    this.SetData(changeProperty, newValue);
                    this.SetBit(changeBit, true);
                    break;
            }
            this.MarkLayoutInvalid();
        }

        public ILayout Layout
        {
            get => this._layout;
            set
            {
                if (this._layout == value)
                    return;
                this._layout = value;
                this.MarkLayoutInvalid();
                this.FireNotification(NotificationID.Layout);
            }
        }

        public ILayoutInput GetLayoutInput(DataCookie inputID) => (ILayoutInput)this.GetData(inputID);

        public void SetLayoutInput(ILayoutInput newValue) => this.SetLayoutInput(newValue.Data, newValue, true);

        public void SetLayoutInput(ILayoutInput newValue, bool invalidateLayout) => this.SetLayoutInput(newValue.Data, newValue, invalidateLayout);

        public void SetLayoutInput(DataCookie inputID, ILayoutInput newValue) => this.SetLayoutInput(inputID, newValue, true);

        public void SetLayoutInput(DataCookie inputID, ILayoutInput newValue, bool invalidateLayout)
        {
            if (this.GetData(inputID) == newValue)
                return;
            this.SetData(inputID, (object)newValue);
            if (!invalidateLayout)
                return;
            this.MarkLayoutInvalid();
        }

        public LayoutOutput LayoutOutput
        {
            get
            {
                if (this.ChangeBit(ViewItem.Bits2.HasLayoutOutput, true))
                    this.SetData(ViewItem.s_layoutOutputProperty, (object)new LayoutOutput(this.LayoutSize));
                return (LayoutOutput)this.GetData(ViewItem.s_layoutOutputProperty);
            }
        }

        public Rectangle LayoutBounds => this._location;

        public Size LayoutSize => this.LayoutBounds.Size;

        public Point LayoutPosition => this.LayoutBounds.Location;

        public Vector3 LayoutScale => this._scale;

        public Rotation LayoutRotation => this._rotation;

        public bool LayoutOffscreen
        {
            get => this.GetBit(ViewItem.Bits2.LayoutOffscreen);
            private set => this.SetBit(ViewItem.Bits2.LayoutOffscreen, value);
        }

        public int LayoutRequestedCount => this._requestedCount;

        public Vector<int> LayoutRequestedIndices => this._requestedIndices;

        public bool LayoutVisible => this._visible == Visibility.Visible;

        Size ILayoutNode.DesiredSize => this.DesiredSize;

        private Size DesiredSize
        {
            get
            {
                Size desiredSize = this._desiredSize;
                if (!this.LayoutContributesToWidth)
                    desiredSize.Width = 0;
                return desiredSize;
            }
        }

        bool ILayoutNode.LayoutContributesToWidth
        {
            get => this.LayoutContributesToWidth;
            set => this.LayoutContributesToWidth = value;
        }

        private bool LayoutContributesToWidth
        {
            get => this.GetBit(ViewItem.Bits2.ContributesToWidth);
            set => this.SetBit(ViewItem.Bits2.ContributesToWidth, value);
        }

        public Size LayoutMaximumSize
        {
            get
            {
                Size maximumSize = this.MaximumSize;
                if (!this.LayoutContributesToWidth && (maximumSize.Width == 0 || maximumSize.Width > this._desiredSize.Width))
                    maximumSize.Width = this._desiredSize.Width;
                return maximumSize;
            }
        }

        object ILayoutNode.MeasureData
        {
            get => this._measureData;
            set => this._measureData = value;
        }

        Point ILayoutNode.AlignmentOffset => this._measureAlignment;

        Size ILayoutNode.AlignedSize => this._alignedSize;

        internal RectangleF GetDescendantFocusRect()
        {
            RectangleF rectangleF = RectangleF.Zero;
            if (this.UISession.InputManager.Queue.PendingKeyFocus is UIClass pendingKeyFocus)
            {
                ViewItem rootItem = pendingKeyFocus.RootItem;
                if (this.HasDescendant((Microsoft.Iris.Library.TreeNode)rootItem))
                {
                    Vector3 positionPxlVector;
                    Vector3 sizePxlVector;
                    ((INavigationSite)rootItem).ComputeBounds(out positionPxlVector, out sizePxlVector);
                    rectangleF = new RectangleF(positionPxlVector.X, positionPxlVector.Y, sizePxlVector.X, sizePxlVector.Y);
                }
            }
            return rectangleF;
        }

        public bool IsOffscreen
        {
            get => this.GetBit(ViewItem.Bits2.IsOffscreen);
            set => this.SetBit(ViewItem.Bits2.IsOffscreen, value);
        }

        public ILayoutInput LayoutInput
        {
            set => this.SetLayoutInput(value.Data, value);
        }

        public bool LayoutInvalid => this.GetBit(ViewItem.Bits.LayoutInvalid);

        public void MarkLayoutInvalid()
        {
            if (!this.IsZoned || this.Zone.InfiniteLayoutLoopDetected)
                return;
            ViewItem viewItem = this;
            while (!viewItem.LayoutInvalid)
            {
                viewItem.ClearLayoutInfo();
                viewItem.SetBit(ViewItem.Bits.LayoutInvalid, true);
                viewItem = viewItem.Parent;
                if (viewItem == null)
                {
                    this.Zone.ScheduleUiTask(UiTask.LayoutComputation);
                    if (!DebugOutlines.Enabled)
                        break;
                    DebugOutlines.NotifyLayoutChange(this);
                    break;
                }
            }
        }

        private void ClearLayoutInfo()
        {
            this.SetBit(ViewItem.Bits2.BuiltLayoutChildren, false);
            this._visibleChildCount = 0;
            this.Measured = false;
            this._constraint = Size.Zero;
            this.Arranged = false;
            this._slot = new LayoutSlot();
            this._bounds = Rectangle.Zero;
        }

        public void ResetLayoutInvalid()
        {
            if (!this.ChangeBit(ViewItem.Bits.LayoutInvalid, false))
                return;
            foreach (ViewItem child in this.Children)
                child.ResetLayoutInvalid();
        }

        public event EventHandler DeepLayoutChange
        {
            add
            {
                if (!this.AddEventHandler(ViewItem.s_deepLayoutChangeEvent, (Delegate)value))
                    return;
                this.EnableDeepLayoutNotifications(true);
            }
            remove
            {
                if (!this.RemoveEventHandler(ViewItem.s_deepLayoutChangeEvent, (Delegate)value))
                    return;
                this.EnableDeepLayoutNotifications(false);
            }
        }

        private void EnableDeepLayoutNotifications(bool enableFlag)
        {
            if (!enableFlag)
            {
                this.SetBit(ViewItem.Bits.DeepLayoutNotifySelf, false);
            }
            else
            {
                if (this.GetBit(ViewItem.Bits.DeepLayoutNotifySelf))
                    return;
                this.SetBit(ViewItem.Bits.DeepLayoutNotifySelf, true);
                ViewItem viewItem = this;
                do
                {
                    viewItem.SetBit(ViewItem.Bits.DeepLayoutNotifyTree, true);
                    ViewItem parent = viewItem.Parent;
                    if (parent == null)
                        break;
                    viewItem = parent;
                }
                while (!viewItem.GetBit(ViewItem.Bits.DeepLayoutNotifyTree));
            }
        }

        private void BuildLayoutChildren()
        {
            if (this.GetBit(ViewItem.Bits2.BuiltLayoutChildren))
                return;
            this.SetBit(ViewItem.Bits2.BuiltLayoutChildren, true);
            this._visibleChildCount = 0;
            foreach (ILayoutNode child in this.Children)
            {
                if (child.Visible)
                    ++this._visibleChildCount;
                else
                    child.MarkHidden();
            }
        }

        int ILayoutNode.LayoutChildrenCount => this.LayoutChildrenCount;

        private int LayoutChildrenCount
        {
            get
            {
                this.BuildLayoutChildren();
                return this._visibleChildCount;
            }
        }

        LayoutNodeEnumerator ILayoutNode.LayoutChildren => this.LayoutChildren;

        private LayoutNodeEnumerator LayoutChildren
        {
            get
            {
                this.BuildLayoutChildren();
                ILayoutNode start = (ILayoutNode)null;
                if (this._visibleChildCount > 0)
                {
                    start = (ILayoutNode)this.FirstChild;
                    if (!start.Visible)
                        start = start.NextVisibleSibling;
                }
                return new LayoutNodeEnumerator(start);
            }
        }

        ILayoutNode ILayoutNode.NextVisibleSibling
        {
            get
            {
                ILayoutNode nextSibling = (ILayoutNode)this.NextSibling;
                while (nextSibling != null && !nextSibling.Visible)
                    nextSibling = nextSibling.NextSibling;
                return nextSibling;
            }
        }

        ILayoutNode ILayoutNode.NextSibling => (ILayoutNode)this.NextSibling;

        Vector<int> ILayoutNode.GetSpecificChildrenRequestList()
        {
            Vector<int> vector = this._requestedIndices;
            if (vector == null)
                vector = new Vector<int>();
            else
                vector.Clear();
            return vector;
        }

        void ILayoutNode.RequestSpecificChildren(Vector<int> requestedIndices) => this._requestedIndices = requestedIndices;

        void ILayoutNode.RequestMoreChildren(int childrenCount) => this._requestedCount = childrenCount;

        public ExtendedLayoutOutput GetExtendedLayoutOutput(DataCookie outputID)
        {
            ExtendedLayoutOutput extendedLayoutOutput = this._extendedOutputs;
            while (extendedLayoutOutput != null && !(extendedLayoutOutput.OutputID == outputID))
                extendedLayoutOutput = extendedLayoutOutput.nextOutput;
            return extendedLayoutOutput;
        }

        void ILayoutNode.SetExtendedLayoutOutput(ExtendedLayoutOutput newDataOutput)
        {
            DataCookie outputId = newDataOutput.OutputID;
            ExtendedLayoutOutput extendedLayoutOutput1 = this._extendedOutputs;
            ExtendedLayoutOutput extendedLayoutOutput2 = (ExtendedLayoutOutput)null;
            ExtendedLayoutOutput extendedLayoutOutput3 = (ExtendedLayoutOutput)null;
            for (; extendedLayoutOutput1 != null; extendedLayoutOutput1 = extendedLayoutOutput3)
            {
                extendedLayoutOutput3 = extendedLayoutOutput1.nextOutput;
                if (!(extendedLayoutOutput1.OutputID == outputId))
                    extendedLayoutOutput2 = extendedLayoutOutput1;
                else
                    break;
            }
            if (extendedLayoutOutput2 != null)
                extendedLayoutOutput2.nextOutput = newDataOutput;
            else
                this._extendedOutputs = newDataOutput;
            newDataOutput.nextOutput = extendedLayoutOutput3;
            if (extendedLayoutOutput1 == null)
                return;
            extendedLayoutOutput1.nextOutput = (ExtendedLayoutOutput)null;
        }

        void ILayoutNode.AddAreaOfInterest(AreaOfInterest interest) => AreaOfInterest.AddAreaOfInterest(interest, ref this._externallySetAreasOfInterest);

        void ILayoutNode.SetVisibleIndexRange(
          int beginVisible,
          int endVisible,
          int beginVisibleOffscreen,
          int endVisibleOffscreen,
          int? focusedItem)
        {
            if (!(this.GetExtendedLayoutOutput(VisibleIndexRangeLayoutOutput.DataCookie) is VisibleIndexRangeLayoutOutput rangeLayoutOutput))
            {
                rangeLayoutOutput = new VisibleIndexRangeLayoutOutput();
                ((ILayoutNode)this).SetExtendedLayoutOutput((ExtendedLayoutOutput)rangeLayoutOutput);
            }
            rangeLayoutOutput.Initialize(beginVisible, endVisible, beginVisibleOffscreen, endVisibleOffscreen, focusedItem);
        }

        void ILayoutNode.MarkHidden() => this.MarkHidden();

        private void MarkHidden()
        {
            this._visible = Visibility.ExplicitlyHidden;
            this._desiredSize = Size.Zero;
            this.Measured = true;
            this.Arranged = true;
            this.Committed = false;
        }

        Size ILayoutNode.Measure(Size constraint)
        {
            if (this.Measured && this._constraint == constraint)
                return this._desiredSize;
            this.ResetMeasureInfo();
            this._constraint = constraint;
            Size size1 = this.Margins.Size;
            constraint.Deflate(size1);
            Size layoutMaximumSize = this.LayoutMaximumSize;
            if (layoutMaximumSize.Width != 0)
                constraint.Width = Math.Min(layoutMaximumSize.Width, constraint.Width);
            if (layoutMaximumSize.Height != 0)
                constraint.Height = Math.Min(layoutMaximumSize.Height, constraint.Height);
            Size minimumSize = this.MinimumSize;
            SharedSize sharedSize = this.SharedSize;
            SharedSizePolicy sharedSizePolicy = this.SharedSizePolicy;
            sharedSize?.AdjustConstraint(ref constraint, ref minimumSize, sharedSizePolicy);
            Inset padding = this.Padding;
            Size size2 = padding.Size;
            constraint.Deflate(size2);
            minimumSize.Deflate(size2);
            if (constraint.IsEmpty || constraint.Width < minimumSize.Width || constraint.Height < minimumSize.Height)
            {
                this.MarkHidden();
                return Size.Zero;
            }
            Size sz2 = this.Layout.Measure((ILayoutNode)this, constraint);
            Size size3 = Size.Max(Size.Min(constraint, sz2), minimumSize);
            Size size4 = size3;
            if (!size3.IsZero)
            {
                Size sz1 = new Size(1, 1);
                Size size5 = Size.Max(sz1, size4 + padding.Size);
                sharedSize?.AccumulateSize(size5, sharedSizePolicy);
                size4 = Size.Max(sz1, size5 + size1);
            }
            this._desiredSize = size4;
            this.Measured = true;
            ItemAlignment effectiveAlignment = this.GetEffectiveAlignment();
            this._alignedSize.Width = this.Align(effectiveAlignment, Orientation.Horizontal, this._constraint, ref this._measureAlignment);
            this._alignedSize.Height = this.Align(effectiveAlignment, Orientation.Vertical, this._constraint, ref this._measureAlignment);
            return this.DesiredSize;
        }

        private void ResetMeasureInfo()
        {
            this._desiredSize = Size.Zero;
            this.LayoutContributesToWidth = true;
            this._alignedSize = Size.Zero;
            this._measureAlignment = Point.Zero;
            this._visible = Visibility.Visible;
            this._externallySetAreasOfInterest = (Vector<AreaOfInterest>)null;
            this._ownedAreasOfInterest = (AreaOfInterestID)0;
            this._containedAreasOfInterest = (AreaOfInterestID)0;
            this._extendedOutputs = (ExtendedLayoutOutput)null;
            this._requestedCount = 0;
            this._requestedIndices = (Vector<int>)null;
            this.ResetArrangeInfo();
        }

        private bool Measured
        {
            get => this.GetBit(ViewItem.Bits2.Measured);
            set => this.SetBit(ViewItem.Bits2.Measured, value);
        }

        private bool Arranged
        {
            get => this.GetBit(ViewItem.Bits2.Arranged);
            set => this.SetBit(ViewItem.Bits2.Arranged, value);
        }

        private bool Committed
        {
            get => this.GetBit(ViewItem.Bits2.Committed);
            set => this.SetBit(ViewItem.Bits2.Committed, value);
        }

        [Conditional("DEBUG")]
        private void CheckMeasured()
        {
        }

        void ILayoutNode.Arrange(LayoutSlot parentSlot) => ((ILayoutNode)this).Arrange(parentSlot, new Rectangle(Point.Zero, parentSlot.Bounds));

        void ILayoutNode.Arrange(LayoutSlot parentSlot, Rectangle bounds) => ((ILayoutNode)this).Arrange(parentSlot, bounds, Vector3.UnitVector, Rotation.Default);

        void ILayoutNode.Arrange(
          LayoutSlot parentSlot,
          Rectangle bounds,
          Vector3 scale,
          Rotation rotation)
        {
            if (bounds.Width == 0 || bounds.Height == 0)
            {
                this.MarkHidden();
            }
            else
            {
                if (this._visible == Visibility.ExplicitlyHidden)
                    return;
                rotation = rotation != Rotation.Default ? rotation : this.Rotation;
                if (this.Arranged && this._visible == Visibility.Visible && (parentSlot == this._slot && bounds == this._bounds) && (scale == this._scale && rotation == this._rotation))
                    return;
                this.ResetArrangeInfo();
                ItemAlignment effectiveAlignment = this.GetEffectiveAlignment();
                int num1 = this.Align(effectiveAlignment, Orientation.Horizontal, bounds.Size, ref this._alignedOffset);
                int num2 = this.Align(effectiveAlignment, Orientation.Vertical, bounds.Size, ref this._alignedOffset);
                Inset margins = this.Margins;
                int num3 = bounds.X + this._alignedOffset.X + margins.Left;
                int num4 = bounds.Y + this._alignedOffset.Y + margins.Top;
                int x = parentSlot.Offset.X + num3;
                int y = parentSlot.Offset.Y + num4;
                int width = num1 - (margins.Left + margins.Right);
                int height = num2 - (margins.Top + margins.Bottom);
                if (width <= 0 || height <= 0)
                {
                    this._location = Rectangle.Zero;
                    this._visible = Visibility.ImplicitlyHidden;
                    this.LayoutOffscreen = false;
                }
                else
                {
                    this._location = new Rectangle(x, y, width, height);
                    bool flag = KeepAliveLayoutInput.ShouldKeepVisible((ILayoutNode)this);
                    if (!flag && this.Parent != null)
                    {
                        ViewItem parent = this.Parent;
                        flag = parent.GetBit(ViewItem.Bits2.KeepAlive) && !parent.DiscardOffscreenVisuals;
                    }
                    this.SetBit(ViewItem.Bits2.KeepAlive, flag);
                    this.SharedSize?.AccumulateSize(this._location.Size, this.SharedSizePolicy);
                    Inset padding = this.Padding;
                    Size size = padding.Size;
                    Size extent = new Size(width, height);
                    extent.Width -= size.Width;
                    extent.Height -= size.Height;
                    bool processChildren = extent.Width > 0 && extent.Height > 0;
                    if (processChildren)
                    {
                        Point offset = new Point(padding.Left, padding.Top);
                        Point pos = new Point(-(num3 + offset.X), -(num4 + offset.Y));
                        Rectangle viewBounds = Rectangle.Offset(parentSlot.View, pos);
                        Rectangle viewPeripheralBounds = Rectangle.Offset(parentSlot.PeripheralView, pos);
                        this.Layout.Arrange((ILayoutNode)this, new LayoutSlot(extent, offset, viewBounds, viewPeripheralBounds));
                    }
                    this.ProcessAreasOfInterest(processChildren);
                    Rectangle location = this._location;
                    AreaOfInterest area;
                    if (((ILayoutNode)this).TryGetAreaOfInterest(AreaOfInterestID.ScrollableRange, out area))
                        location.Union(area.Rectangle);
                    this._visible = location.IntersectsWith(parentSlot.PeripheralView) || flag ? Visibility.Visible : Visibility.ImplicitlyHidden;
                    this.SetBit(ViewItem.Bits2.LayoutOffscreen, !location.IntersectsWith(parentSlot.View));
                }
                this._slot = parentSlot;
                this._bounds = bounds;
                this._scale = scale;
                this._rotation = rotation;
                this.Arranged = true;
            }
        }

        private void ResetArrangeInfo()
        {
            this.Arranged = false;
            this._externallySetAreasOfInterest = (Vector<AreaOfInterest>)null;
            this._ownedAreasOfInterest = (AreaOfInterestID)0;
            this._containedAreasOfInterest = (AreaOfInterestID)0;
            this.Committed = false;
        }

        private int Align(
          ItemAlignment alignment,
          Orientation orientation,
          Size slotSize,
          ref Point alignmentOffset)
        {
            int dimension1 = slotSize.GetDimension(orientation);
            int num = this.DesiredSize.GetDimension(orientation);
            switch (alignment.GetAlignment(orientation))
            {
                case Microsoft.Iris.Layout.Alignment.Unspecified:
                case Microsoft.Iris.Layout.Alignment.Fill:
                    int dimension2 = this.LayoutMaximumSize.GetDimension(orientation);
                    if (dimension2 > 0)
                    {
                        int val2 = dimension2 + this.Margins.Size.GetDimension(orientation);
                        num = Math.Min(dimension1, val2);
                        goto case Microsoft.Iris.Layout.Alignment.Near;
                    }
                    else
                    {
                        num = dimension1;
                        goto case Microsoft.Iris.Layout.Alignment.Near;
                    }
                case Microsoft.Iris.Layout.Alignment.Near:
                    alignmentOffset.SetDimension(orientation, 0);
                    break;
                case Microsoft.Iris.Layout.Alignment.Center:
                    alignmentOffset.SetDimension(orientation, (dimension1 - num) / 2);
                    break;
                case Microsoft.Iris.Layout.Alignment.Far:
                    alignmentOffset.SetDimension(orientation, dimension1 - num);
                    break;
            }
            return num;
        }

        private void ProcessAreasOfInterest(bool processChildren)
        {
            this._ownedAreasOfInterest = (AreaOfInterestID)0;
            AreaOfInterestLayoutInput layoutInput = (AreaOfInterestLayoutInput)this.GetLayoutInput(AreaOfInterestLayoutInput.Data);
            if (layoutInput != null)
                this._ownedAreasOfInterest = layoutInput.Id;
            if (this.PendingScrollIntoView)
                this._ownedAreasOfInterest |= AreaOfInterestID.ScrollIntoViewRequest;
            if (this._externallySetAreasOfInterest != null)
            {
                foreach (AreaOfInterest areaOfInterest in this._externallySetAreasOfInterest)
                    this._ownedAreasOfInterest |= areaOfInterest.Id;
            }
            this._containedAreasOfInterest = (AreaOfInterestID)0;
            if (!processChildren)
                return;
            foreach (ViewItem layoutChild in this.LayoutChildren)
                this._containedAreasOfInterest |= layoutChild._ownedAreasOfInterest | layoutChild._containedAreasOfInterest;
        }

        bool ILayoutNode.ContainsAreaOfInterest(AreaOfInterestID id) => (this._ownedAreasOfInterest & id) != (AreaOfInterestID)0 || (this._containedAreasOfInterest & id) != (AreaOfInterestID)0;

        bool ILayoutNode.TryGetAreaOfInterest(AreaOfInterestID id, out AreaOfInterest area)
        {
            if ((this._ownedAreasOfInterest & id) != (AreaOfInterestID)0)
            {
                AreaOfInterestLayoutInput layoutInput = (AreaOfInterestLayoutInput)this.GetLayoutInput(AreaOfInterestLayoutInput.Data);
                if (layoutInput != null && layoutInput.Id == id)
                {
                    area = new AreaOfInterest(new Rectangle(Point.Zero, this._location.Size), layoutInput.Margins, id);
                    return true;
                }
                if (id == AreaOfInterestID.ScrollIntoViewRequest)
                {
                    area = new AreaOfInterest(new Rectangle(Point.Zero, this._location.Size), Inset.Zero, AreaOfInterestID.ScrollIntoViewRequest);
                    return true;
                }
                if (this._externallySetAreasOfInterest != null)
                {
                    foreach (AreaOfInterest areaOfInterest in this._externallySetAreasOfInterest)
                    {
                        if (areaOfInterest.Id == id)
                        {
                            area = areaOfInterest;
                            return true;
                        }
                    }
                }
            }
            else if ((this._containedAreasOfInterest & id) != (AreaOfInterestID)0)
            {
                foreach (ILayoutNode layoutChild in this.LayoutChildren)
                {
                    AreaOfInterest area1;
                    if (layoutChild.TryGetAreaOfInterest(id, out area1))
                    {
                        Point layoutPosition = layoutChild.LayoutPosition;
                        AreaOfInterest interest = area1.Transform(layoutPosition);
                        this.ClipAreaOfInterest(ref interest, this._location.Size);
                        area = interest;
                        return true;
                    }
                }
            }
            area = new AreaOfInterest();
            return false;
        }

        void ILayoutNode.Commit()
        {
            if (this.Committed)
                return;
            this.Committed = true;
            this.MarkLayoutOutputDirty(false);
            this.OnCommit();
            foreach (ILayoutNode child in this.Children)
                child.Commit();
        }

        public virtual void OnCommit()
        {
        }

        public void MarkLayoutOutputDirty(bool forceFlag)
        {
            if (!forceFlag && this.GetBit(ViewItem.Bits.OutputSelfDirty))
                return;
            this.SetBit(ViewItem.Bits.OutputSelfDirty, true);
            ViewItem viewItem = this;
            do
            {
                viewItem.SetBit(ViewItem.Bits.OutputTreeDirty, true);
                ViewItem parent = viewItem.Parent;
                if (parent == null)
                {
                    this.Zone.ScheduleUiTask(UiTask.LayoutApplication);
                    break;
                }
                viewItem = parent;
            }
            while (!viewItem.GetBit(ViewItem.Bits.OutputTreeDirty));
        }

        [Conditional("DEBUG")]
        public void DEBUG_ValidatePostLayoutState()
        {
            foreach (ViewItem child in this.Children)
                ;
        }

        public void ApplyLayoutOutputs(bool visibilityChanging)
        {
            if (!visibilityChanging && !this.GetBit(ViewItem.Bits.OutputTreeDirty))
                return;
            var args = new LayoutApplyParams()
            {
                fullyVisible = this.Zone.ZonePhysicalVisible,
                parentOffscreen = false,
                visibilityChanging = visibilityChanging,
                allowAnimations = true
            };
            this.ApplyLayoutOutputWorker(ref args);
        }

        private void ApplyLayoutOutputWorker(ref ViewItem.LayoutApplyParams selfApplyParams)
        {
            this.SetBit(ViewItem.Bits.OutputTreeDirty, false);
            bool flag1 = false;
            if (selfApplyParams.visibilityChanging || selfApplyParams.offscreenChanging || this.GetBit(ViewItem.Bits.OutputSelfDirty))
            {
                this.SetBit(ViewItem.Bits.OutputSelfDirty, false);
                bool flag2 = false;
                bool offscreenChange = false;
                if (this._ownerUI == null)
                {
                    if (this is RootViewItem rootViewItem)
                    {
                        rootViewItem.ApplyRootLayoutOutput(selfApplyParams.fullyVisible, out flag2);
                        flag1 = true;
                        if (flag2)
                            selfApplyParams.allowAnimations = false;
                    }
                }
                else
                {
                    this._ownerUI.ApplyLayoutOutput(this, selfApplyParams.fullyVisible, selfApplyParams.parentOffscreen, selfApplyParams.allowAnimations, out flag2, out offscreenChange);
                    flag1 = true;
                }
                if (flag2 && !selfApplyParams.visibilityChanging)
                    selfApplyParams.visibilityChanging = true;
                if (offscreenChange && !selfApplyParams.offscreenChanging)
                    selfApplyParams.offscreenChanging = true;
            }
            selfApplyParams.deepLayoutChanged |= flag1;
            if (!this.LayoutVisible)
                selfApplyParams.fullyVisible = false;
            if (this.LayoutOffscreen)
                selfApplyParams.parentOffscreen = true;
            if (this.HasChildren)
            {
                foreach (ViewItem child in this.Children)
                {
                    if (selfApplyParams.visibilityChanging || selfApplyParams.offscreenChanging || child.GetBit(ViewItem.Bits.OutputTreeDirty) || selfApplyParams.deepLayoutChanged && child.GetBit(ViewItem.Bits.DeepLayoutNotifyTree))
                    {
                        ViewItem.LayoutApplyParams selfApplyParams1 = new ViewItem.LayoutApplyParams();
                        selfApplyParams1.fullyVisible = selfApplyParams.fullyVisible;
                        selfApplyParams1.visibilityChanging = selfApplyParams.visibilityChanging;
                        selfApplyParams1.parentOffscreen = selfApplyParams.parentOffscreen;
                        selfApplyParams1.offscreenChanging = selfApplyParams.offscreenChanging;
                        selfApplyParams1.allowAnimations = selfApplyParams.allowAnimations;
                        selfApplyParams1.deepLayoutChanged = selfApplyParams.deepLayoutChanged;
                        child.ApplyLayoutOutputWorker(ref selfApplyParams1);
                        selfApplyParams.anyDeepChangesDelivered |= selfApplyParams1.anyDeepChangesDelivered;
                    }
                }
            }
            if (selfApplyParams.deepLayoutChanged && this.GetBit(ViewItem.Bits.DeepLayoutNotifyTree))
            {
                if (this.GetBit(ViewItem.Bits.DeepLayoutNotifySelf))
                {
                    if (this.GetEventHandler(ViewItem.s_deepLayoutChangeEvent) is EventHandler eventHandler)
                    {
                        eventHandler((object)this, EventArgs.Empty);
                        selfApplyParams.anyDeepChangesDelivered = true;
                    }
                    else
                        this.SetBit(ViewItem.Bits.DeepLayoutNotifySelf, false);
                }
                if (!selfApplyParams.anyDeepChangesDelivered)
                    this.SetBit(ViewItem.Bits.DeepLayoutNotifyTree, false);
            }
            if (!flag1)
                return;
            this.OnLayoutComplete(this);
        }

        public void OnScaleChange(Vector3 oldScaleVector, Vector3 newScaleVector) => this.NotifyEffectiveScaleChange(false);

        public void SetAreaOfInterest(AreaOfInterestID id, Inset margins) => this.SetLayoutInput((ILayoutInput)new AreaOfInterestLayoutInput(id, margins));

        public void ClearAreaOfInterest(AreaOfInterestID id)
        {
            if (!(this.GetLayoutInput(AreaOfInterestLayoutInput.Data) is AreaOfInterestLayoutInput layoutInput) || layoutInput.Id != id)
                return;
            this.SetLayoutInput(AreaOfInterestLayoutInput.Data, (ILayoutInput)null);
        }

        bool ITrackableUIElement.IsUIVisible => this.IsVisibleToRenderer;

        Rectangle ITrackableUIElement.EstimatePosition(
          IZoneDisplayChild ancestor)
        {
            Vector3 positionPxlVector;
            Vector3 sizePxlVector;
            return this.ComputeBounds(ancestor, out positionPxlVector, out sizePxlVector) ? new Rectangle(positionPxlVector.X, positionPxlVector.Y, sizePxlVector.X, sizePxlVector.Y) : Rectangle.Zero;
        }

        event EventHandler ITrackableUIElementEvents.UIChange
        {
            add
            {
                this.DeepParentChange += value;
                this.DeepLayoutChange += value;
            }
            remove
            {
                this.DeepParentChange -= value;
                this.DeepLayoutChange -= value;
            }
        }

        public bool PlayAnimation(AnimationEventType type)
        {
            IAnimationProvider animation = this.GetAnimation(type);
            return animation != null && this.PlayAnimation(animation, this.GetAnimationHandle(type));
        }

        public bool PlayAnimation(IAnimationProvider ab, AnimationHandle animationHandle)
        {
            if (!this.HasVisual)
                return false;
            AnimationArgs args = new AnimationArgs(this);
            return this.PlayAnimation(ab, ref args, UIClass.ShouldPlayAnimation(ab), animationHandle);
        }

        public void ApplyAnimatableValue(AnimationEventType type, ref AnimationArgs args)
        {
            bool flag = false;
            IAnimationProvider animation = this.GetAnimation(type);
            if (animation != null)
                flag = this.PlayAnimation(animation, ref args, UIClass.ShouldPlayAnimation(animation), this.GetAnimationHandle(type));
            bool applyNow = !flag;
            if (applyNow)
                this.StopOverlappingAnimations((ActiveSequence)null, ActiveSequence.ConvertToActiveTransition(type));
            this.SetVisualValue(type, ref args, applyNow);
        }

        private void SetVisualValue(AnimationEventType type, ref AnimationArgs args, bool applyNow)
        {
            switch (type)
            {
                case AnimationEventType.Move:
                    if (!applyNow)
                    {
                        this.VisualPositionNoSend = args.NewPosition;
                        break;
                    }
                    this.VisualPosition = args.NewPosition;
                    break;
                case AnimationEventType.Size:
                    if (!applyNow)
                    {
                        this.VisualSizeNoSend = args.NewSize;
                        break;
                    }
                    this.VisualSize = args.NewSize;
                    break;
                case AnimationEventType.Scale:
                    if (!applyNow)
                    {
                        this.VisualScaleNoSend = args.NewScale;
                        break;
                    }
                    this.VisualScale = args.NewScale;
                    break;
                case AnimationEventType.Rotate:
                    if (!applyNow)
                    {
                        this.VisualRotationNoSend = args.NewRotation;
                        break;
                    }
                    this.VisualRotation = args.NewRotation;
                    break;
                case AnimationEventType.Alpha:
                    if (!applyNow)
                    {
                        this.VisualAlphaNoSend = args.NewAlpha;
                        break;
                    }
                    this.VisualAlpha = args.NewAlpha;
                    break;
            }
        }

        [Conditional("DEBUG")]
        private void DEBUG_ValidateSetVisualValue(
          AnimationEventType type,
          object oldValue,
          object newValue)
        {
        }

        private bool PlayAnimation(
          IAnimationProvider ab,
          ref AnimationArgs args,
          bool shouldPlayAnimation,
          AnimationHandle animationHandle)
        {
            AnimationTemplate anim = this.BuildAnimation(ab, ref args);
            if (anim == null)
                return false;
            if (shouldPlayAnimation)
            {
                this.PlayAnimation(anim, ref args, (EventHandler)null, animationHandle);
            }
            else
            {
                this.ApplyFinalAnimationState(anim, ref args);
                animationHandle?.FireCompleted();
            }
            return true;
        }

        private void PlayAnimation(
          AnimationTemplate anim,
          ref AnimationArgs args,
          EventHandler onCompleteHandler,
          AnimationHandle animationHandle)
        {
            ActiveSequence instance = anim.CreateInstance((IAnimatable)this.RendererVisual, ref args);
            if (instance == null)
                return;
            instance.DeclareOwner((object)this);
            if (onCompleteHandler != null)
                instance.AnimationCompleted += onCompleteHandler;
            animationHandle?.AssociateWithAnimationInstance(instance);
            instance.AnimationCompleted += new EventHandler(this.OnAnimationComplete);
            this.PlayAnimationWorker(instance, true);
            if (!(anim is Animation animation) || !animation.DisableMouseInput)
                return;
            this.UI.UpdateMouseHandling(this);
        }

        public void PlayShowAnimation()
        {
            IAnimationProvider ab = (IAnimationProvider)null;
            if (this.GetBit(ViewItem.Bits2.InsideContentChange))
            {
                ab = this.GetAnimation(AnimationEventType.ContentChangeShow);
                this.SetBit(ViewItem.Bits2.InsideContentChange, false);
            }
            if (ab == null)
                ab = this.GetAnimation(AnimationEventType.Show);
            if (ab != null)
                this.PlayAnimation(ab, this.GetAnimationHandle(AnimationEventType.Show));
            else
                this.TryToPlayIdleAnimation();
        }

        public void PlayHideAnimation(OrphanedVisualCollection orphans)
        {
            IAnimationProvider animationProvider = (IAnimationProvider)null;
            if (this.GetBit(ViewItem.Bits2.InsideContentChange))
                animationProvider = this.GetAnimation(AnimationEventType.ContentChangeHide);
            if (animationProvider == null)
                animationProvider = this.GetAnimation(AnimationEventType.Hide);
            if (animationProvider == null || !UIClass.ShouldPlayAnimation(animationProvider))
                return;
            AnimationArgs args = new AnimationArgs(this);
            AnimationTemplate animationTemplate = this.BuildAnimation(animationProvider, ref args);
            if (animationTemplate == null)
                return;
            if (animationTemplate.Loop == -1)
                animationTemplate.Loop = 0;
            ActiveSequence instance = animationTemplate.CreateInstance((IAnimatable)this.RendererVisual, ref args);
            if (instance == null)
                return;
            orphans.RegisterWaitForAnimation(instance, false);
            this.PlayAnimationWorker(instance, false);
            this.TransferActiveAnimations(orphans);
        }

        private void TransferActiveAnimations(OrphanedVisualCollection orphans)
        {
            Vector<ActiveSequence> activeAnimations = this.GetActiveAnimations(false);
            this.TransferAnimationsList(orphans, activeAnimations, new EventHandler(this.OnAnimationComplete));
            this.SetData(ViewItem.s_activeAnimationsProperty, (object)null);
            this.SetBit(ViewItem.Bits.ActiveAnimations, false);
            Vector<ActiveSequence> idleAnimations = this.GetIdleAnimations(false);
            this.TransferAnimationsList(orphans, idleAnimations, new EventHandler(this.OnIdleAnimationComplete));
            this.SetData(ViewItem.s_idleAnimationsProperty, (object)null);
            this.SetBit(ViewItem.Bits.IdleAnimations, false);
            this.OnAnimationListChanged();
        }

        private void TransferAnimationsList(
          OrphanedVisualCollection orphans,
          Vector<ActiveSequence> animationsList,
          EventHandler handler)
        {
            if (animationsList == null)
                return;
            foreach (ActiveSequence animations in animationsList)
            {
                orphans.RegisterAnimation(animations, true);
                if (handler != null)
                    animations.AnimationCompleted -= handler;
                if (animations.Template.Loop == -1)
                    animations.Stop();
            }
        }

        public Dictionary<AnimationEventType, IAnimationProvider> GetAnimationSet() => !this.GetBit(ViewItem.Bits.AnimationBuilders) ? (Dictionary<AnimationEventType, IAnimationProvider>)null : (Dictionary<AnimationEventType, IAnimationProvider>)this.GetData(ViewItem.s_animationBuildersProperty);

        public IAnimationProvider GetAnimation(AnimationEventType type)
        {
            Dictionary<AnimationEventType, IAnimationProvider> animationSet = this.GetAnimationSet();
            if (animationSet == null)
                return (IAnimationProvider)null;
            IAnimationProvider animationProvider;
            animationSet.TryGetValue(type, out animationProvider);
            return animationProvider;
        }

        public RelativeTo SnapshotPosition() => (RelativeTo)new SnapshotRelativeTo(this.BoundsRelativeToAncestor((ViewItem)null));

        public void AttachAnimation(IAnimationProvider animation) => this.SetAnimationData(animation.Type, animation);

        public void AttachAnimation(IAnimationProvider animation, AnimationHandle animationHandle)
        {
            this.AttachAnimation(animation);
            this.SetAnimationHandle(animation.Type, animationHandle);
        }

        public void DetachAnimation(AnimationEventType type)
        {
            this.SetAnimationData(type, (IAnimationProvider)null);
            this.SetAnimationHandle(type, (AnimationHandle)null);
        }

        private Dictionary<AnimationEventType, AnimationHandle> GetAnimationHandleSet() => !this.GetBit(ViewItem.Bits2.AnimationHandles) ? (Dictionary<AnimationEventType, AnimationHandle>)null : (Dictionary<AnimationEventType, AnimationHandle>)this.GetData(ViewItem.s_animationHandlesProperty);

        public AnimationHandle GetAnimationHandle(AnimationEventType type)
        {
            Dictionary<AnimationEventType, AnimationHandle> animationHandleSet = this.GetAnimationHandleSet();
            if (animationHandleSet == null)
                return (AnimationHandle)null;
            AnimationHandle animationHandle;
            animationHandleSet.TryGetValue(type, out animationHandle);
            return animationHandle;
        }

        public void SetAnimationHandle(AnimationEventType type, AnimationHandle animationHandle)
        {
            bool flag = animationHandle != null;
            Dictionary<AnimationEventType, AnimationHandle> dictionary = this.GetAnimationHandleSet();
            if (dictionary == null && flag)
            {
                dictionary = new Dictionary<AnimationEventType, AnimationHandle>();
                this.SetData(ViewItem.s_animationHandlesProperty, (object)dictionary);
                this.SetBit(ViewItem.Bits2.AnimationHandles, true);
            }
            if (dictionary == null)
                return;
            if (flag)
            {
                dictionary[type] = animationHandle;
            }
            else
            {
                dictionary.Remove(type);
                if (dictionary.Count != 0)
                    return;
                this.SetData(ViewItem.s_animationHandlesProperty, (object)null);
                this.SetBit(ViewItem.Bits2.AnimationHandles, false);
            }
        }

        private void PlayAnimationWorker(ActiveSequence newSequence, bool addToActiveAnimationList)
        {
            this.StopIdleAnimation();
            this.StopOverlappingAnimations(newSequence, newSequence.GetActiveTransitions());
            if (addToActiveAnimationList)
            {
                this.GetActiveAnimations(true).Add(newSequence);
                this.OnAnimationListChanged();
            }
            newSequence.Play();
        }

        private AnimationTemplate BuildAnimation(
          IAnimationProvider ab,
          ref AnimationArgs args)
        {
            return ab.Build(ref args);
        }

        private void StopOverlappingAnimations(
          ActiveSequence newSequence,
          ActiveTransitions newTransitions)
        {
            Vector<ActiveSequence> activeAnimations = this.GetActiveAnimations(false);
            if (activeAnimations == null)
                return;
            StopCommandSet stopCommand = (StopCommandSet)null;
            foreach (ActiveSequence playingSequence in activeAnimations)
                this.StopAnimationIfOverlapping(playingSequence, newSequence, newTransitions, ref stopCommand);
        }

        private bool StopAnimationIfOverlapping(
          ActiveSequence playingSequence,
          ActiveSequence newSequence,
          ActiveTransitions newTransitions,
          ref StopCommandSet stopCommand)
        {
            IAnimatable animatable = (IAnimatable)this.VisualContainer;
            if (newSequence != null)
                animatable = newSequence.Target;
            if (playingSequence.Target == animatable)
            {
                ActiveTransitions activeTransitions = playingSequence.GetActiveTransitions();
                if ((newTransitions & activeTransitions) != ActiveTransitions.None)
                {
                    if (stopCommand == null && newSequence != null)
                    {
                        stopCommand = newSequence.GetStopCommandSet();
                        StopCommandSet stopCommandSet = stopCommand;
                    }
                    playingSequence.Stop(stopCommand);
                    return true;
                }
            }
            return false;
        }

        private void OnAnimationComplete(object sender, EventArgs args)
        {
            ActiveSequence activeSequence = sender as ActiveSequence;
            activeSequence.AnimationCompleted -= new EventHandler(this.OnAnimationComplete);
            Vector<ActiveSequence> activeAnimations = this.GetActiveAnimations(false);
            activeAnimations.Remove(activeSequence);
            this.OnAnimationListChanged();
            if (activeAnimations.Count == 0)
            {
                this.SetData(ViewItem.s_activeAnimationsProperty, (object)null);
                this.SetBit(ViewItem.Bits.ActiveAnimations, false);
                this.TryToPlayIdleAnimation();
            }
            if (activeSequence.Template is Animation template && template.DisableMouseInput)
                this.UI.UpdateMouseHandling(this);
            activeSequence.Dispose((object)this);
        }

        private void OnIdleAnimationComplete(object sender, EventArgs args)
        {
            ActiveSequence activeSequence = sender as ActiveSequence;
            activeSequence.AnimationCompleted -= new EventHandler(this.OnIdleAnimationComplete);
            Vector<ActiveSequence> idleAnimations = this.GetIdleAnimations(false);
            idleAnimations.Remove(activeSequence);
            this.OnAnimationListChanged();
            if (idleAnimations.Count == 0)
            {
                this.SetData(ViewItem.s_idleAnimationsProperty, (object)null);
                this.SetBit(ViewItem.Bits.IdleAnimations, false);
            }
            activeSequence.Dispose((object)this);
        }

        private bool TryToPlayIdleAnimation()
        {
            if (!this.HasVisual)
                return false;
            Vector<ActiveSequence> idleAnimations = this.GetIdleAnimations(false);
            ActiveSequence playingSequence = (ActiveSequence)null;
            if (idleAnimations != null)
                playingSequence = idleAnimations[idleAnimations.Count - 1];
            IAnimationProvider animation = this.GetAnimation(AnimationEventType.Idle);
            if (animation == null)
                return false;
            AnimationArgs args = new AnimationArgs(this);
            AnimationTemplate anim = this.BuildAnimation(animation, ref args);
            if (anim == null)
                return false;
            if (!UIClass.ShouldPlayAnimation(animation))
            {
                this.ApplyFinalAnimationState(anim, ref args);
                return false;
            }
            ActiveSequence instance = anim.CreateInstance((IAnimatable)this.RendererVisual, ref args);
            if (instance == null)
                return false;
            instance.DeclareOwner((object)this);
            if (playingSequence != null && playingSequence.Playing)
            {
                ActiveTransitions activeTransitions = instance.GetActiveTransitions();
                StopCommandSet stopCommand = (StopCommandSet)null;
                if (this.StopAnimationIfOverlapping(playingSequence, instance, activeTransitions, ref stopCommand))
                    playingSequence = (ActiveSequence)null;
            }
            instance.Play();
            if (playingSequence != null && playingSequence.Template.Loop == -1)
                playingSequence.Stop();
            if (instance != null)
            {
                this.GetIdleAnimations(true).Add(instance);
                this.OnAnimationListChanged();
                instance.AnimationCompleted += new EventHandler(this.OnIdleAnimationComplete);
            }
            return instance != null;
        }

        private void StopIdleAnimation()
        {
            Vector<ActiveSequence> idleAnimations = this.GetIdleAnimations(false);
            idleAnimations?[idleAnimations.Count - 1].Stop();
        }

        private void StopActiveAnimations()
        {
            if (!this.GetBit(ViewItem.Bits.ActiveAnimations))
                return;
            foreach (ActiveSequence activeAnimation in this.GetActiveAnimations(true))
                activeAnimation.Stop();
        }

        internal void ApplyFinalAnimationState(AnimationTemplate anim, ref AnimationArgs args)
        {
            if (!this.HasVisual)
                return;
            BaseKeyframe[] baseKeyframeArray = new BaseKeyframe[20];
            foreach (BaseKeyframe keyframe in anim.Keyframes)
            {
                BaseKeyframe baseKeyframe = baseKeyframeArray[(uint)keyframe.Type];
                if (baseKeyframe == null || (double)baseKeyframe.Time <= (double)keyframe.Time)
                    baseKeyframeArray[(uint)keyframe.Type] = keyframe;
            }
            foreach (BaseKeyframe baseKeyframe in baseKeyframeArray)
                baseKeyframe?.Apply((IAnimatableOwner)this, ref args);
        }

        private void SetAnimationData(AnimationEventType type, IAnimationProvider anim)
        {
            bool flag = anim != null;
            Dictionary<AnimationEventType, IAnimationProvider> dictionary = this.GetAnimationSet();
            if (dictionary == null && flag)
            {
                dictionary = new Dictionary<AnimationEventType, IAnimationProvider>();
                this.SetData(ViewItem.s_animationBuildersProperty, (object)dictionary);
                this.SetBit(ViewItem.Bits.AnimationBuilders, true);
            }
            if (dictionary != null)
            {
                if (flag)
                {
                    dictionary[type] = anim;
                }
                else
                {
                    dictionary.Remove(type);
                    if (dictionary.Count == 0)
                    {
                        this.SetData(ViewItem.s_animationBuildersProperty, (object)null);
                        this.SetBit(ViewItem.Bits.AnimationBuilders, false);
                    }
                }
            }
            if (type != AnimationEventType.Idle)
                return;
            if (flag)
            {
                if (this.GetBit(ViewItem.Bits.ActiveAnimations))
                    return;
                this.TryToPlayIdleAnimation();
            }
            else
                this.StopIdleAnimation();
        }

        private Vector<ActiveSequence> GetActiveAnimations(bool createIfNone) => this.GetAnimationSequence(ViewItem.Bits.ActiveAnimations, ViewItem.s_activeAnimationsProperty, createIfNone);

        private Vector<ActiveSequence> GetIdleAnimations(bool createIfNone) => this.GetAnimationSequence(ViewItem.Bits.IdleAnimations, ViewItem.s_idleAnimationsProperty, createIfNone);

        private Vector<ActiveSequence> GetAnimationSequence(
          ViewItem.Bits propertyHint,
          DataCookie dynamicProperty,
          bool createIfNone)
        {
            Vector<ActiveSequence> vector = (Vector<ActiveSequence>)null;
            if (!this.GetBit(propertyHint))
            {
                if (createIfNone)
                {
                    vector = new Vector<ActiveSequence>();
                    this.SetData(dynamicProperty, (object)vector);
                    this.SetBit(propertyHint, true);
                }
            }
            else
                vector = (Vector<ActiveSequence>)this.GetData(dynamicProperty);
            return vector;
        }

        private void OnAnimationListChanged()
        {
            if (!this.ChangeBit(ViewItem.Bits2.IsAlphaAnimationPlaying, ViewItem.DoesAnimationListContainAnimationType(this.GetActiveAnimations(false), ActiveTransitions.Alpha) || ViewItem.DoesAnimationListContainAnimationType(this.GetIdleAnimations(false), ActiveTransitions.Alpha)) || (double)this.Alpha != 0.0)
                return;
            this.OnVisibilityChange();
        }

        private static bool DoesAnimationListContainAnimationType(
          Vector<ActiveSequence> animationList,
          ActiveTransitions type)
        {
            if (animationList != null)
            {
                foreach (ActiveSequence animation in animationList)
                {
                    if ((animation.GetActiveTransitions() & type) == type)
                        return true;
                }
            }
            return false;
        }

        public bool MouseInteractive
        {
            get => this.GetBit(ViewItem.Bits.MouseInteractive);
            set
            {
                if (!this.ChangeBit(ViewItem.Bits.MouseInteractive, value))
                    return;
                this.UI.UpdateMouseHandling(this);
                this.FireNotification(NotificationID.MouseInteractive);
            }
        }

        public bool ClipMouse
        {
            get => this.GetBit(ViewItem.Bits.ClipMouse);
            set
            {
                if (!this.ChangeBit(ViewItem.Bits.ClipMouse, value) || this.UI == null)
                    return;
                this.UI.UpdateMouseHandling(this);
            }
        }

        public bool AllowMouseInput
        {
            get
            {
                Vector<ActiveSequence> activeAnimations = this.GetActiveAnimations(false);
                if (activeAnimations != null)
                {
                    foreach (ActiveSequence activeSequence in activeAnimations)
                    {
                        if (activeSequence.Template is Animation template && template.DisableMouseInput)
                            return false;
                    }
                }
                return true;
            }
        }

        protected virtual void CreateVisualContainer(IRenderSession renderSession)
        {
            if (this._container != null)
                return;
            this.VisualContainer = renderSession.CreateVisualContainer((object)this, (object)this);
        }

        internal void CreateVisual(IRenderSession renderSession)
        {
            this.CreateVisualContainer(renderSession);
            this.AddVisualToParent(renderSession, (IVisual)this._container);
            this.VisualScale = this.Scale;
            this.VisualAlpha = this.Alpha;
            this.VisualRotation = this.Rotation;
            this.VisualCenterPoint = this.CenterPointPercent;
            if (this.GetBit(ViewItem.Bits.PendingNavigateInto) && !this.GetBit(ViewItem.Bits.PendingNavigateIntoScheduled))
                this.ScheduleNavigateInto();
            this.MarkPaintInvalid();
        }

        protected virtual VisualOrder GetVisualOrder() => VisualOrder.Last;

        private void AddVisualToParent(IRenderSession renderSession, IVisual visual)
        {
            ViewItem viewItem = this;
            do
            {
                viewItem = (ViewItem)viewItem.NextSibling;
            }
            while (viewItem != null && !viewItem.HasVisual);
            ViewItem parent = this.Parent;
            VisualOrder nOrder = parent.GetVisualOrder();
            IVisual vSibling = (IVisual)null;
            if (viewItem != null)
            {
                vSibling = viewItem.RendererVisual;
                nOrder = VisualOrder.Before;
            }
            if (vSibling == null)
            {
                vSibling = (IVisual)parent.ContentVisual;
                nOrder = vSibling == null ? nOrder : VisualOrder.Before;
            }
            parent.VisualContainer.AddChild(visual, vSibling, nOrder);
        }

        protected IVisualContainer VisualContainer
        {
            get => this._container;
            set
            {
                this._container = value;
                if (!this.GetBit(ViewItem.Bits2.HasCamera))
                    return;
                this._container.Camera = ((Camera)this.GetData(ViewItem.s_cameraProperty)).APICamera;
            }
        }

        private bool IsVisibleToRenderer => this.HasVisual || this.GetBit(ViewItem.Bits2.InsideContentChange);

        public void ForceContentChange()
        {
            if (!this.HasVisual)
                return;
            Dictionary<AnimationEventType, IAnimationProvider> animationSet = this.GetAnimationSet();
            if (animationSet == null || !animationSet.ContainsKey(AnimationEventType.ContentChangeShow) && !animationSet.ContainsKey(AnimationEventType.ContentChangeHide))
                return;
            this.SetBit(ViewItem.Bits2.InsideContentChange, true);
            if (this.UI == null)
                return;
            this.UI.DestroyVisualTree(this, true);
        }

        TransformSet IZoneDisplayChild.Transforms
        {
            get
            {
                TransformSet transformSet = new TransformSet();
                if (this.HasVisual)
                {
                    transformSet.positionPxlVector = this.VisualPosition;
                    transformSet.sizePxlVector = new Vector3(this.VisualSize.X, this.VisualSize.Y, 0.0f);
                    transformSet.scaleVector = this.VisualScale;
                    transformSet.centerPointScaleVector = this.VisualCenterPoint;
                }
                else
                    transformSet.scaleVector = Vector3.UnitVector;
                return transformSet;
            }
        }

        bool INavigationSite.ComputeBounds(
          out Vector3 positionPxlVector,
          out Vector3 sizePxlVector)
        {
            return this.ComputeBounds((IZoneDisplayChild)null, out positionPxlVector, out sizePxlVector);
        }

        private bool ComputeBounds(
          IZoneDisplayChild ancestor,
          out Vector3 positionPxlVector,
          out Vector3 sizePxlVector)
        {
            positionPxlVector = Vector3.Zero;
            sizePxlVector = Vector3.Zero;
            if (!this.HasVisual)
                return false;
            Vector3 parentOffsetPxlVector;
            Vector3 scaleVector;
            ViewItem.GetAccumulatedOffsetAndScale((IZoneDisplayChild)this, ancestor, out parentOffsetPxlVector, out scaleVector);
            positionPxlVector = parentOffsetPxlVector;
            Vector2 visualSize = this.VisualSize;
            sizePxlVector = new Vector3(visualSize.X, visualSize.Y, 0.0f) * scaleVector;
            return true;
        }

        public static void GetAccumulatedOffsetAndScale(
          IZoneDisplayChild childStart,
          IZoneDisplayChild childStop,
          out Vector3 parentOffsetPxlVector,
          out Vector3 scaleVector)
        {
            parentOffsetPxlVector = Vector3.Zero;
            scaleVector = Vector3.UnitVector;
            ArrayList arrayList = ViewItem.s_pathListCache.Acquire();
            if (!((ViewItem)childStart).GetParentChain(childStop as ViewItem, arrayList))
            {
                ViewItem.s_pathListCache.Release(arrayList);
            }
            else
            {
                for (int index = arrayList.Count - 1; index >= 0; --index)
                {
                    Vector3 vector3_1 = Vector3.Zero;
                    Vector3 vector3_2 = Vector3.UnitVector;
                    IZoneDisplayChild zoneDisplayChild = arrayList[index] as IZoneDisplayChild;
                    if (arrayList[index] is ViewItem viewItem && !viewItem.HasVisual)
                    {
                        Point layoutPosition = viewItem.LayoutPosition;
                        vector3_1 = new Vector3((float)layoutPosition.X, (float)layoutPosition.Y, 0.0f);
                    }
                    else if (zoneDisplayChild != null)
                    {
                        TransformSet transforms = zoneDisplayChild.Transforms;
                        vector3_1 = transforms.positionPxlVector;
                        vector3_2 = transforms.scaleVector;
                    }
                    parentOffsetPxlVector += vector3_1 * scaleVector;
                    scaleVector *= vector3_2;
                }
                ViewItem.s_pathListCache.Release(arrayList);
            }
        }

        public RectangleF TransformFromAncestor(ViewItem ancestor, RectangleF rect)
        {
            Vector3 parentOffsetPxlVector;
            Vector3 scaleVector;
            ViewItem.GetAccumulatedOffsetAndScale((IZoneDisplayChild)this, (IZoneDisplayChild)ancestor, out parentOffsetPxlVector, out scaleVector);
            rect.X -= parentOffsetPxlVector.X;
            rect.Y -= parentOffsetPxlVector.Y;
            rect.X /= scaleVector.X;
            rect.Y /= scaleVector.Y;
            rect.Width /= scaleVector.X;
            rect.Height /= scaleVector.Y;
            return rect;
        }

        public RectangleF TransformToAncestor(ViewItem ancestor, RectangleF rect)
        {
            Vector3 parentOffsetPxlVector;
            Vector3 scaleVector;
            ViewItem.GetAccumulatedOffsetAndScale((IZoneDisplayChild)this, (IZoneDisplayChild)ancestor, out parentOffsetPxlVector, out scaleVector);
            rect.X *= scaleVector.X;
            rect.Y *= scaleVector.Y;
            rect.Width *= scaleVector.X;
            rect.Height *= scaleVector.Y;
            rect.X += parentOffsetPxlVector.X;
            rect.Y += parentOffsetPxlVector.Y;
            return rect;
        }

        public RectangleF BoundsRelativeToAncestor(ViewItem ancestor)
        {
            Vector3 parentOffsetPxlVector;
            Vector3 scaleVector;
            ViewItem.GetAccumulatedOffsetAndScale((IZoneDisplayChild)this, (IZoneDisplayChild)ancestor, out parentOffsetPxlVector, out scaleVector);
            Vector2 vector2 = this.HasVisual ? this.VisualSize : Vector2.Zero;
            return new RectangleF(parentOffsetPxlVector.X, parentOffsetPxlVector.Y, vector2.X * scaleVector.X, vector2.Y * scaleVector.Y);
        }

        public Point ScreenToClient(Point screenPoint)
        {
            this.Zone.Form.ScreenToClient(ref screenPoint);
            return this.TransformFromAncestor((ViewItem)null, new RectangleF(screenPoint, Size.Zero)).Location.ToPoint();
        }

        public Point WindowToClient(Point windowPoint) => this.TransformFromAncestor((ViewItem)null, new RectangleF(windowPoint, Size.Zero)).Location.ToPoint();

        public Point ClientToWindow(Point clientPoint) => this.TransformToAncestor((ViewItem)null, new RectangleF(clientPoint, Size.Zero)).Location.ToPoint();

        public Point ClientToScreen(Point clientPoint)
        {
            Point window = this.ClientToWindow(clientPoint);
            this.Zone.Form.ClientToScreen(ref window);
            return window;
        }

        public virtual void ClipAreaOfInterest(ref AreaOfInterest interest, Size usedSize)
        {
        }

        private bool GetParentChain(ViewItem itemStop, ArrayList pathList)
        {
            pathList.Clear();
            ViewItem parent;
            for (ViewItem viewItem = this; viewItem != itemStop; viewItem = parent)
            {
                pathList.Add((object)viewItem);
                parent = viewItem.Parent;
                if (parent == null)
                {
                    if (!viewItem.IsRoot)
                        return false;
                    break;
                }
            }
            return true;
        }

        protected virtual void OnLayoutComplete(ViewItem sender)
        {
            if (this.GetEventHandler(ViewItem.s_layoutCompleteEvent) is LayoutCompleteEventHandler eventHandler)
                eventHandler((object)sender);
            if (!this.GetBit(ViewItem.Bits2.HasLayoutOutput))
                return;
            this.LayoutOutput.OnLayoutComplete(this.LayoutSize);
        }

        private void HACK_RemoveCachedScrollIntoViewAreasOfInterest()
        {
            for (ViewItem viewItem = this; viewItem != null; viewItem = viewItem.Parent)
            {
                viewItem._ownedAreasOfInterest &= ~AreaOfInterestID.ScrollIntoViewRequest;
                viewItem._containedAreasOfInterest &= ~AreaOfInterestID.ScrollIntoViewRequest;
            }
        }

        public event LayoutCompleteEventHandler LayoutComplete
        {
            add => this.AddEventHandler(ViewItem.s_layoutCompleteEvent, (Delegate)value);
            remove => this.RemoveEventHandler(ViewItem.s_layoutCompleteEvent, (Delegate)value);
        }

        internal void ClearStickyFocus() => NavigationServices.ClearDefaultFocus((INavigationSite)this);

        public void ScrollIntoView()
        {
            if (this.PendingScrollIntoView)
                return;
            this.SetBit(ViewItem.Bits2.PendingScrollIntoView, true);
            this.LockVisible(true);
            DeferredCall.Post(DispatchPriority.LayoutSync, ViewItem.s_scrollIntoViewCleanup, (object)this);
        }

        private static void CleanUpAfterScrollIntoView(object obj) => ((ViewItem)obj).CleanUpAfterScrollIntoView();

        private void CleanUpAfterScrollIntoView()
        {
            if (!this.PendingScrollIntoView)
                return;
            this.SetBit(ViewItem.Bits2.PendingScrollIntoView, false);
            this.UnlockVisible();
            this.HACK_RemoveCachedScrollIntoViewAreasOfInterest();
        }

        public bool PendingScrollIntoView => this.GetBit(ViewItem.Bits2.PendingScrollIntoView);

        public virtual bool DiscardOffscreenVisuals
        {
            get => false;
            set
            {
            }
        }

        public void LockVisible() => this.LockVisible(false);

        private void LockVisible(bool invalidateLayout)
        {
            KeepAliveLayoutInput aliveLayoutInput = (KeepAliveLayoutInput)this.GetLayoutInput(KeepAliveLayoutInput.Data);
            if (aliveLayoutInput == null)
            {
                aliveLayoutInput = new KeepAliveLayoutInput();
                this.SetLayoutInput((ILayoutInput)aliveLayoutInput, invalidateLayout);
            }
            ++aliveLayoutInput.Count;
        }

        public void UnlockVisible()
        {
            KeepAliveLayoutInput layoutInput = (KeepAliveLayoutInput)this.GetLayoutInput(KeepAliveLayoutInput.Data);
            --layoutInput.Count;
            if (layoutInput.Count != 0)
                return;
            this.SetLayoutInput(KeepAliveLayoutInput.Data, (ILayoutInput)null, false);
        }

        public void NavigateInto() => this.NavigateInto(false);

        public void NavigateInto(bool isDefault)
        {
            if (this.ChangeBit(ViewItem.Bits.PendingNavigateInto, true))
            {
                this.LockVisible(!this.IsVisibleToRenderer);
                if (this.IsVisibleToRenderer)
                    this.ScheduleNavigateInto();
            }
            this.SetBit(ViewItem.Bits.PendingNavigateIntoIsDefault, isDefault);
        }

        private void ScheduleNavigateInto()
        {
            DeferredCall.Post(DispatchPriority.LayoutSync, new SimpleCallback(this.NavigateIntoWorker));
            this.SetBit(ViewItem.Bits.PendingNavigateIntoScheduled, true);
        }

        private void NavigateIntoWorker()
        {
            if (this.IsDisposed)
                return;
            this.UnlockVisible();
            this.SetBit(ViewItem.Bits.PendingNavigateInto, false);
            this.SetBit(ViewItem.Bits.PendingNavigateIntoScheduled, false);
            INavigationSite resultSite;
            if (!NavigationServices.FindNextWithin((INavigationSite)this, Direction.Next, RectangleF.Zero, out resultSite) || resultSite == null || !(resultSite is ViewItem viewItem))
                return;
            viewItem.UI.NotifyNavigationDestination(this.GetBit(ViewItem.Bits.PendingNavigateIntoIsDefault) ? KeyFocusReason.Default : KeyFocusReason.Other);
        }

        public NavigationPolicies Navigation
        {
            get
            {
                NavigationPolicies navigationPolicies = NavigationPolicies.None;
                if (this.GetBit(ViewItem.Bits.HasNavMode))
                {
                    object data = this.GetData(ViewItem.s_navModeProperty);
                    if (data != null)
                        navigationPolicies = (NavigationPolicies)data;
                }
                return navigationPolicies;
            }
            set
            {
                if (value != NavigationPolicies.None)
                {
                    this.SetData(ViewItem.s_navModeProperty, (object)value);
                    this.SetBit(ViewItem.Bits.HasNavMode, true);
                    this.FireNotification(NotificationID.Navigation);
                }
                else
                {
                    if (!this.GetBit(ViewItem.Bits.HasNavMode))
                        return;
                    this.SetData(ViewItem.s_navModeProperty, (object)null);
                    this.SetBit(ViewItem.Bits.HasNavMode, false);
                    this.FireNotification(NotificationID.Navigation);
                }
            }
        }

        protected virtual NavigationPolicies ForcedNavigationFlags => NavigationPolicies.None;

        public int FocusOrder
        {
            get
            {
                int num = int.MaxValue;
                if (this.GetBit(ViewItem.Bits.HasFocusOrder))
                {
                    object data = this.GetData(ViewItem.s_focusOrderProperty);
                    if (data != null)
                        num = (int)data;
                }
                return num;
            }
            set
            {
                if (value != int.MaxValue)
                {
                    this.SetData(ViewItem.s_focusOrderProperty, (object)value);
                    this.SetBit(ViewItem.Bits.HasFocusOrder, true);
                    this.FireNotification(NotificationID.FocusOrder);
                }
                else
                {
                    if (!this.GetBit(ViewItem.Bits.HasFocusOrder))
                        return;
                    this.SetData(ViewItem.s_focusOrderProperty, (object)null);
                    this.SetBit(ViewItem.Bits.HasFocusOrder, false);
                    this.FireNotification(NotificationID.FocusOrder);
                }
            }
        }

        object INavigationSite.UniqueId => (object)this.IDPath;

        public ViewItemID[] IDPath
        {
            get
            {
                int length = 0;
                for (ViewItem viewItem = this; viewItem.Parent != null; viewItem = viewItem.Parent)
                    ++length;
                if (length == 0)
                    return (ViewItemID[])null;
                ViewItemID[] viewItemIdArray = new ViewItemID[length];
                int index = length - 1;
                for (ViewItem childItem = this; childItem.Parent != null; childItem = childItem.Parent)
                {
                    viewItemIdArray[index] = childItem.Parent.IDForChild(childItem);
                    --index;
                }
                return viewItemIdArray;
            }
        }

        INavigationSite INavigationSite.Parent => (INavigationSite)this.Parent;

        ICollection INavigationSite.Children => (ICollection)this.Children;

        bool INavigationSite.Visible => this.IsVisibleToRenderer;

        NavigationClass INavigationSite.Navigability
        {
            get
            {
                NavigationClass navigationClass = NavigationClass.None;
                if (this._ownerUI != null)
                    navigationClass = this._ownerUI.GetNavigability(this);
                return navigationClass;
            }
        }

        NavigationPolicies INavigationSite.Mode => this.Navigation | this.ForcedNavigationFlags;

        int INavigationSite.FocusOrder => this.FocusOrder;

        bool INavigationSite.IsLogicalJunction => this is Host;

        string INavigationSite.Description => this.Name;

        object INavigationSite.StateCache
        {
            get => this.GetData(ViewItem.s_navCacheProperty);
            set => this.SetData(ViewItem.s_navCacheProperty, value);
        }

        internal virtual void FaultInChild(ViewItemID component, ChildFaultedInDelegate handler)
        {
        }

        internal FindChildResult FindChildFromPath(
          ViewItemID[] parts,
          out ViewItem resultItem,
          out ViewItemID failedComponent)
        {
            FindChildResult findChildResult = FindChildResult.Failure;
            resultItem = (ViewItem)this.Zone.RootViewItem;
            failedComponent = new ViewItemID();
            for (int index = 0; index < parts.Length; ++index)
            {
                ViewItem resultItem1;
                findChildResult = resultItem.ChildForID(parts[index], out resultItem1);
                if (findChildResult == FindChildResult.Success)
                {
                    resultItem = resultItem1;
                }
                else
                {
                    failedComponent = parts[index];
                    break;
                }
            }
            return findChildResult;
        }

        INavigationSite INavigationSite.LookupChildById(
          object uniqueIDObject)
        {
            INavigationSite navigationSite = (INavigationSite)null;
            ViewItem resultItem;
            if (uniqueIDObject is ViewItemID[] parts && this.FindChildFromPath(parts, out resultItem, out ViewItemID _) == FindChildResult.Success)
                navigationSite = (INavigationSite)resultItem;
            return navigationSite;
        }

        protected virtual ViewItemID IDForChild(ViewItem childItem) => new ViewItemID(childItem._uiID);

        protected virtual FindChildResult ChildForID(
          ViewItemID part,
          out ViewItem resultItem)
        {
            resultItem = (ViewItem)null;
            if (part.IDValid && !part.StringPartValid)
            {
                foreach (ViewItem child in this.Children)
                {
                    if (child._uiID == part.ID)
                    {
                        resultItem = child;
                        break;
                    }
                }
            }
            return resultItem == null ? FindChildResult.Failure : FindChildResult.Success;
        }

        private bool GetBit(ViewItem.Bits lookupBit) => this._bits[(int)lookupBit];

        protected bool GetBit(ViewItem.Bits2 lookupBit) => this._bits2[(int)lookupBit];

        private uint GetBitAsUInt(ViewItem.Bits lookupBit) => ((ViewItem.Bits)this._bits.Data & lookupBit) == ~(ViewItem.Bits.PendingNavigateInto | ViewItem.Bits.PendingNavigateIntoIsDefault | ViewItem.Bits.PendingNavigateIntoScheduled | ViewItem.Bits.ClipMouse | ViewItem.Bits.MouseInteractive | ViewItem.Bits.PaintInvalid | ViewItem.Bits.HasScale | ViewItem.Bits.ScaleChanged | ViewItem.Bits.LayoutInputMaxSize | ViewItem.Bits.LayoutInputMinSize | ViewItem.Bits.LayoutInputMargins | ViewItem.Bits.LayoutInputPadding | ViewItem.Bits.LayoutInputVisible | ViewItem.Bits.LayoutAlignment | ViewItem.Bits.LayoutChildAlignment | ViewItem.Bits.LayoutInputSharedSize | ViewItem.Bits.LayoutInputSharedSizePolicy | ViewItem.Bits.OutputSelfDirty | ViewItem.Bits.OutputTreeDirty | ViewItem.Bits.LayoutInvalid | ViewItem.Bits.ActiveAnimations | ViewItem.Bits.AnimationBuilders | ViewItem.Bits.IdleAnimations | ViewItem.Bits.HasNavMode | ViewItem.Bits.HasFocusOrder | ViewItem.Bits.DeepLayoutNotifySelf | ViewItem.Bits.DeepLayoutNotifyTree | ViewItem.Bits.Unused1 | ViewItem.Bits.Unused2 | ViewItem.Bits.Unused3 | ViewItem.Bits.Unused4 | ViewItem.Bits.Unused5) ? 0U : 1U;

        protected uint GetBitAsUInt(ViewItem.Bits2 lookupBit) => ((ViewItem.Bits2)this._bits2.Data & lookupBit) == (ViewItem.Bits2)0 ? 0U : 1U;

        private void SetBit(ViewItem.Bits changeBit, bool value) => this._bits[(int)changeBit] = value;

        private void SetBit(ViewItem.Bits2 changeBit, bool value) => this._bits2[(int)changeBit] = value;

        private bool ChangeBit(ViewItem.Bits bit, bool value)
        {
            if (this._bits[(int)bit] == value)
                return false;
            this._bits[(int)bit] = value;
            return true;
        }

        private bool ChangeBit(ViewItem.Bits2 bit, bool value)
        {
            if (this._bits2[(int)bit] == value)
                return false;
            this._bits2[(int)bit] = value;
            return true;
        }

        private static UIClass ValidateOwner(UIClass ui) => ui;

        public string Name
        {
            get => (string)this.GetData(ViewItem.s_nameProperty);
            set
            {
                if (!((string)this.GetData(ViewItem.s_nameProperty) != value))
                    return;
                string str = NotifyService.CanonicalizeString(value);
                this.SetData(ViewItem.s_nameProperty, (object)str);
            }
        }

        public Color DebugOutline
        {
            get
            {
                object data = this.GetData(ViewItem.s_debugOutlineProperty);
                return data == null ? Color.Transparent : (Color)data;
            }
            set
            {
                if (!(this.DebugOutline != value))
                    return;
                this.SetData(ViewItem.s_debugOutlineProperty, (object)value);
                this.FireNotification(NotificationID.DebugOutline);
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(this.GetType().Name);
            stringBuilder.Append(":");
            string name = this.Name;
            if (name != null)
            {
                stringBuilder.Append(" (");
                stringBuilder.Append(name);
                stringBuilder.Append(")");
            }
            return stringBuilder.ToString();
        }

        [Conditional("DEBUG")]
        private void DEBUG_DumpAnimation(ActiveSequence aseq)
        {
            foreach (BaseKeyframe keyframe in aseq.Template.Keyframes)
                ;
        }

        public delegate void PaintHandler(ViewItem senderItem);

        private struct LayoutApplyParams
        {
            public bool fullyVisible;
            public bool visibilityChanging;
            public bool parentOffscreen;
            public bool offscreenChanging;
            public bool allowAnimations;
            public bool deepLayoutChanged;
            public bool anyDeepChangesDelivered;
        }

        private enum Bits : uint
        {
            PendingNavigateInto = 1,
            PendingNavigateIntoIsDefault = 2,
            PendingNavigateIntoScheduled = 4,
            ClipMouse = 8,
            MouseInteractive = 16, // 0x00000010
            PaintInvalid = 32, // 0x00000020
            HasScale = 64, // 0x00000040
            ScaleChanged = 128, // 0x00000080
            LayoutInputMaxSize = 256, // 0x00000100
            LayoutInputMinSize = 512, // 0x00000200
            LayoutInputMargins = 1024, // 0x00000400
            LayoutInputPadding = 2048, // 0x00000800
            LayoutInputVisible = 4096, // 0x00001000
            LayoutAlignment = 8192, // 0x00002000
            LayoutChildAlignment = 16384, // 0x00004000
            LayoutInputSharedSize = 32768, // 0x00008000
            LayoutInputSharedSizePolicy = 65536, // 0x00010000
            OutputSelfDirty = 131072, // 0x00020000
            OutputTreeDirty = 262144, // 0x00040000
            LayoutInvalid = 524288, // 0x00080000
            ActiveAnimations = 1048576, // 0x00100000
            AnimationBuilders = 2097152, // 0x00200000
            IdleAnimations = 4194304, // 0x00400000
            HasNavMode = 8388608, // 0x00800000
            HasFocusOrder = 16777216, // 0x01000000
            DeepLayoutNotifySelf = 33554432, // 0x02000000
            DeepLayoutNotifyTree = 67108864, // 0x04000000
            Unused1 = 134217728, // 0x08000000
            Unused2 = 268435456, // 0x10000000
            Unused3 = 536870912, // 0x20000000
            Unused4 = 1073741824, // 0x40000000
            Unused5 = 2147483648, // 0x80000000
        }

        protected enum Bits2 : uint
        {
            HasAlpha = 1,
            HasCenterPointPercent = 2,
            HasLayer = 4,
            HasRotation = 8,
            IsOffscreen = 16, // 0x00000010
            PendingScrollIntoView = 32, // 0x00000020
            PaintChildrenInvalid = 64, // 0x00000040
            HasPositionNoSend = 128, // 0x00000080
            HasScaleNoSend = 256, // 0x00000100
            HasSizeNoSend = 512, // 0x00000200
            HasRotationNoSend = 1024, // 0x00000400
            HasAlphaNoSend = 2048, // 0x00000800
            HasEffect = 4096, // 0x00001000
            HasLayoutOutput = 8192, // 0x00002000
            AnimationHandles = 16384, // 0x00004000
            IsAlphaAnimationPlaying = 32768, // 0x00008000
            InsideContentChange = 65536, // 0x00010000
            HasCamera = 131072, // 0x00020000
            BuiltLayoutChildren = 262144, // 0x00040000
            Measured = 524288, // 0x00080000
            Arranged = 1048576, // 0x00100000
            Committed = 2097152, // 0x00200000
            ContributesToWidth = 4194304, // 0x00400000
            LayoutOffscreen = 8388608, // 0x00800000
            KeepAlive = 16777216, // 0x01000000
        }
    }
}
