// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.UI.UIClass
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Accessibility;
using Microsoft.Iris.Animations;
using Microsoft.Iris.Debug;
using Microsoft.Iris.Drawing;
using Microsoft.Iris.Input;
using Microsoft.Iris.InputHandlers;
using Microsoft.Iris.Layout;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
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

namespace Microsoft.Iris.UI
{
    internal class UIClass :
      Microsoft.Iris.Library.TreeNode,
      ICookedInputSite,
      IMarkupTypeBase,
      IDisposableOwner,
      INotifyObject,
      ISchemaInfo
    {
        public const string UIStateReservedSymbolName = "UI";
        private BitVector32 _bits;
        private Host _ownerHost;
        private int _uiIDFactory;
        private ViewItem _rootItem;
        private InputHandlerList _inputHandlers;
        private UIClassTypeSchema _typeSchema;
        private Dictionary<object, object> _storage;
        private Vector<IDisposableObject> _disposables;
        private MarkupListeners _listeners;
        private NotifyService _notifier = new NotifyService();
        private ScriptRunScheduler _scriptRunScheduler = new ScriptRunScheduler();
        private static DeferredHandler s_executePendingScriptsHandler = new DeferredHandler(UIClass.ExecutePendingScripts);
        private static readonly FocusStateHandler s_updateMouseFocusStates = new FocusStateHandler(UIClass.UpdateMouseFocusStates);
        private static readonly FocusStateHandler s_updateKeyFocusStates = new FocusStateHandler(UIClass.UpdateKeyFocusStates);
        private static readonly DataCookie s_cursorProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_cursorOverrideProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_accProxyProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_pendingFocusRestoreProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_focusInterestTargetProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_focusInterestTargetMarginsProperty = DataCookie.ReserveSlot();
        private static readonly EventCookie s_descendantKeyFocusChangedEvent = EventCookie.ReserveSlot();
        private static readonly EventCookie s_descendantMouseFocusChangedEvent = EventCookie.ReserveSlot();
        private EventContext _eventContext;

        public UIClass(UIClassTypeSchema type)
        {
            this._typeSchema = type;
            this._storage = new Dictionary<object, object>(type.TotalPropertiesAndLocalsCount);
            this._bits = new BitVector32();
            this.SetBit(UIClass.Bits.Flippable, true);
            this.SetBit(UIClass.Bits.CreateInterestOnFocus, true);
            this.SetBit(UIClass.Bits.KeyFocusOnMouseDown, true);
            this.SetBit(UIClass.Bits.AllowDoubleClicks, true);
            this.SetBit(UIClass.Bits.Enabled, true);
            this.SetBit(UIClass.Bits.ScriptEnabled, true);
        }

        public bool Initialized => this.GetBit(UIClass.Bits.Initialized);

        public void RegisterDisposable(IDisposableObject disposable)
        {
            if (disposable is ViewItem || disposable is InputHandler)
                return;
            if (this._disposables == null)
                this._disposables = new Vector<IDisposableObject>();
            this._disposables.Add(disposable);
        }

        public bool UnregisterDisposable(ref IDisposableObject disposable)
        {
            if (!(disposable is ViewItem) && !(disposable is InputHandler) && this._disposables != null)
            {
                int index = this._disposables.IndexOf(disposable);
                if (index != -1)
                {
                    disposable = this._disposables[index];
                    this._disposables.RemoveAt(index);
                    return true;
                }
            }
            return false;
        }

        protected override void OnDispose()
        {
            this._typeSchema.RunFinalEvaluates(this);
            base.OnDispose();
            if (this.Initialized)
                AccessibleProxy.NotifyDestroyed(this);
            this.SetBit(UIClass.Bits.ScriptEnabled, false);
            if (this._listeners != null)
            {
                this._listeners.Dispose(this);
                this._listeners = null;
            }
            this._notifier.ClearListeners();
            if (this._rootItem != null)
            {
                this.DisposeViewItemTree(this._rootItem);
                this._rootItem = null;
            }
            this.DisposeInputHandlers();
            this._storage.Clear();
            if (this._disposables != null)
            {
                for (int index = 0; index < this._disposables.Count; ++index)
                    this._disposables[index].Dispose(this);
            }
            this.RemoveEventHandlers(UIClass.s_descendantMouseFocusChangedEvent);
            this.RemoveEventHandlers(UIClass.s_descendantKeyFocusChangedEvent);
        }

        private void DisposeInputHandlers()
        {
            if (this._inputHandlers == null)
                return;
            foreach (DisposableObject inputHandler in this._inputHandlers)
                inputHandler.Dispose(this);
        }

        internal void DisposeViewItemTree(ViewItem item)
        {
            if (item.UI != this)
                return;
            ViewItem nextSibling;
            for (ViewItem viewItem = (ViewItem)item.FirstChild; viewItem != null; viewItem = nextSibling)
            {
                nextSibling = (ViewItem)viewItem.NextSibling;
                this.DisposeViewItemTree(viewItem);
            }
            item.Dispose(this);
        }

        internal void DestroyVisualTree(ViewItem subjectItem) => this.DestroyVisualTree(subjectItem, false);

        internal void DestroyVisualTree(ViewItem subjectItem, bool markLayoutOutputDirty) => this.DestroyVisualTree(subjectItem, true, markLayoutOutputDirty);

        private void DestroyVisualTree(
          ViewItem subjectItem,
          bool allowAnimations,
          bool markLayoutOutputDirty)
        {
            OrphanedVisualCollection orphans = new OrphanedVisualCollection(this.UISession.AnimationManager);
            this.DestroyVisualTreeWorker(subjectItem, orphans, allowAnimations, markLayoutOutputDirty);
            orphans.OnLayoutApplyComplete();
        }

        private void DestroyVisualTreeWorker(
          ViewItem subjectItem,
          OrphanedVisualCollection orphans,
          bool allowAnimations,
          bool markLayoutOutputDirty)
        {
            subjectItem.RendererVisual.MouseOptions = MouseOptions.None;
            foreach (ViewItem child in subjectItem.Children)
            {
                if (child.HasVisual)
                    this.DestroyVisualTreeWorker(child, orphans, allowAnimations, markLayoutOutputDirty);
            }
            if (markLayoutOutputDirty)
                subjectItem.MarkLayoutOutputDirty(false);
            if (allowAnimations)
                this.AnimateDestroyedVisual(subjectItem, orphans);
            if (subjectItem == subjectItem.UI.RootItem && subjectItem.UI.RootItem.HasVisual)
                subjectItem.UI.RevalidateUsage(true, true);
            subjectItem.OrphanVisuals(orphans);
        }

        [Conditional("DEBUG")]
        private static void DEBUG_VerifyVisualTreeDestroyed(ViewItem vi)
        {
            foreach (ViewItem child in vi.Children)
                ;
        }

        public int AllocateUIID(ViewItem item) => this._uiIDFactory++;

        public override bool IsRoot => this.Zone.RootUI == this;

        public UIClass Parent => (UIClass)base.Parent;

        protected override void OnZoneAttached()
        {
            base.OnZoneAttached();
            this.RevalidateUsage(true, false);
            bool enabledFlag = true;
            UIClass parent = this.Parent;
            if (parent != null)
                enabledFlag = parent.FullyEnabled;
            this.DeliverFullyEnabled(enabledFlag);
            if (this._inputHandlers != null)
            {
                foreach (InputHandler inputHandler in this._inputHandlers)
                    inputHandler.OnZoneAttached();
            }
            AccessibleProxy.NotifyTreeChanged(this);
        }

        protected override void OnZoneDetached()
        {
            base.OnZoneDetached();
            this.ChangeBit(UIClass.Bits.AppFullyEnabled, false);
            this.RevalidateUsage(true, true);
            if (this._inputHandlers != null)
            {
                foreach (InputHandler inputHandler in this._inputHandlers)
                    inputHandler.OnZoneDetached();
            }
            if (this._rootItem == null || !this._rootItem.HasVisual)
                return;
            this.DestroyVisualTree(this._rootItem);
        }

        protected override void OnChildrenChanged()
        {
            base.OnChildrenChanged();
            if (!this.IsZoned)
                return;
            AccessibleProxy.NotifyTreeChanged(this);
        }

        public bool HasAccessibleProxy => this.GetBit(UIClass.Bits.HasAccProxy);

        protected virtual AccessibleProxy OnCreateAccessibleProxy(
          UIClass ui,
          Accessible data)
        {
            return new AccessibleProxy(ui, data);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public AccessibleProxy AccessibleProxy
        {
            get
            {
                AccessibleProxy accessibleProxy;
                if (this.HasAccessibleProxy)
                {
                    accessibleProxy = (AccessibleProxy)this.GetData(UIClass.s_accProxyProperty);
                }
                else
                {
                    Accessible data = null;
                    foreach (KeyValuePair<object, object> keyValuePair in this._storage)
                    {
                        if (keyValuePair.Value is Accessible accessible)
                        {
                            data = accessible;
                            break;
                        }
                    }
                    if (data == null)
                        data = new Accessible();
                    accessibleProxy = this.OnCreateAccessibleProxy(this, data);
                    this.SetData(UIClass.s_accProxyProperty, accessibleProxy);
                    this.SetBit(UIClass.Bits.HasAccProxy, true);
                }
                return accessibleProxy;
            }
        }

        internal static bool ShouldPlayAnimation(IAnimationProvider provider)
        {
            if (provider == null)
                return false;
            bool flag = true;
            if (provider is NonIntensiveAnimation)
                flag = false;
            return AnimationSystem.Enabled || !flag;
        }

        private void AnimateNewVisual(
          ViewItem vi,
          Vector3 startPosVector,
          Vector2 startSizeVector,
          Vector3 startScaleVector,
          Rotation startRotation)
        {
            vi.VisualPosition = startPosVector;
            vi.VisualSize = startSizeVector;
            vi.VisualScale = startScaleVector;
            vi.VisualRotation = startRotation;
            this.AnimateNewVisual(vi);
        }

        private void AnimateNewVisual(ViewItem vi)
        {
            vi.PlayShowAnimation();
            if (!this.DirectKeyFocus)
                return;
            vi.PlayAnimation(AnimationEventType.GainFocus);
        }

        private void AnimateDestroyedVisual(ViewItem vi, OrphanedVisualCollection orphans) => vi.PlayHideAnimation(orphans);

        private void AnimateVisual(
          ViewItem vi,
          Vector3 newPosVector,
          Vector2 newSizeVector,
          Vector3 newScaleVector,
          Rotation newRotation)
        {
            Vector3 visualPosition = vi.VisualPosition;
            Vector2 visualSize = vi.VisualSize;
            Vector3 visualScale = vi.VisualScale;
            Rotation visualRotation = vi.VisualRotation;
            float visualAlpha = vi.VisualAlpha;
            AnimationArgs args = new AnimationArgs(vi, visualPosition, visualSize, visualScale, visualRotation, visualAlpha, newPosVector, newSizeVector, newScaleVector, newRotation, visualAlpha);
            if (visualPosition != newPosVector)
                vi.ApplyAnimatableValue(AnimationEventType.Move, ref args);
            if (visualSize != newSizeVector)
                vi.ApplyAnimatableValue(AnimationEventType.Size, ref args);
            if (visualScale != newScaleVector)
                vi.ApplyAnimatableValue(AnimationEventType.Scale, ref args);
            if (!(visualRotation != newRotation))
                return;
            vi.ApplyAnimatableValue(AnimationEventType.Rotate, ref args);
        }

        private void TryToPlayAnimationOnTree(ViewItem vi, AnimationEventType type)
        {
            if (!vi.HasVisual)
                return;
            vi.PlayAnimation(type);
            foreach (ViewItem child in vi.Children)
            {
                if (child.UI == this)
                    this.TryToPlayAnimationOnTree(child, type);
            }
        }

        internal object SaveKeyFocus()
        {
            if (this.KeyFocus)
            {
                UIClass keyFocusDescendant = this.KeyFocusDescendant;
                if (keyFocusDescendant != null && keyFocusDescendant.RootItem != null)
                    return new UIClass.SavedFocusState(keyFocusDescendant.RootItem);
            }
            return null;
        }

        internal void RestoreKeyFocus(object obj) => this.RestoreKeyFocus(obj, true);

        internal void RestoreKeyFocus(object obj, bool allowDeferralFlag)
        {
            if (!(obj is UIClass.SavedFocusState focusState))
                return;
            ViewItem resultItem;
            ViewItemID failedComponent;
            switch (this.RootItem.FindChildFromPath(focusState.Path, out resultItem, out failedComponent))
            {
                case FindChildResult.Success:
                    if (resultItem == null)
                        break;
                    UIClass ui = resultItem.UI;
                    if (!this.HasDescendant(ui) || !ui.IsKeyFocusable())
                        break;
                    resultItem.NavigateInto(focusState.FocusIsDefault);
                    break;
                case FindChildResult.PotentiallyFaultIn:
                    if (!allowDeferralFlag)
                        break;
                    new UIClass.DeferredKeyFocusRestoreHelper(this, focusState).FaultInChild(resultItem, failedComponent);
                    break;
            }
        }

        private UIClass.DeferredKeyFocusRestoreHelper PendingFocusRestore
        {
            get => this.GetData(UIClass.s_pendingFocusRestoreProperty) as UIClass.DeferredKeyFocusRestoreHelper;
            set => this.SetData(UIClass.s_pendingFocusRestoreProperty, value);
        }

        private static bool CheckHandled(InputInfo info, InputHandler inputHandler) => info.Handled;

        public void SetAreaOfInterest(AreaOfInterestID id) => this.SetAreaOfInterest(id, this.RootItem, Inset.Zero);

        public void SetAreaOfInterest(AreaOfInterestID id, ViewItem target, Inset margins) => target.SetAreaOfInterest(id, margins);

        public void ClearAreaOfInterest(AreaOfInterestID id) => this.ClearAreaOfInterest(id, this.RootItem);

        public void ClearAreaOfInterest(AreaOfInterestID id, ViewItem target) => target.ClearAreaOfInterest(id);

        internal void UpdateMouseHandling(ViewItem childItem)
        {
            if (childItem == null)
            {
                childItem = this.RootItem;
                if (childItem == null)
                    return;
            }
            if (!childItem.HasVisual)
                return;
            MouseOptions mouseOptions = MouseOptions.None;
            if (!childItem.IsOffscreen && childItem.AllowMouseInput && (childItem != this.RootItem || this.IsInputBranchEnabled() && this.InputEnabled))
                mouseOptions |= MouseOptions.Traversable;
            if (childItem.MouseInteractive && this.MouseInteractive)
                mouseOptions |= MouseOptions.Hittable;
            if (childItem == this.RootItem)
                mouseOptions |= MouseOptions.Returnable;
            if (childItem.ClipMouse)
                mouseOptions |= MouseOptions.ClipChildren;
            childItem.RendererVisual.MouseOptions = mouseOptions;
        }

        internal void OnHostVisibilityChanged()
        {
            this.OnInputEnabledChanged();
            AccessibleProxy.NotifyVisibilityChange(this, this.Visible);
        }

        public IList EnsureInputHandlerStorage()
        {
            if (this._inputHandlers == null)
                this._inputHandlers = new InputHandlerList();
            return _inputHandlers;
        }

        internal bool Visible => this.IsRoot || this._ownerHost.HasVisual;

        internal bool InputEnabled
        {
            get
            {
                if (this.IsRoot)
                    return true;
                return this.Visible && this._ownerHost.InputEnabled;
            }
        }

        public bool Enabled
        {
            get => this.GetBit(UIClass.Bits.Enabled);
            set
            {
                if (!this.ChangeBit(UIClass.Bits.Enabled, value))
                    return;
                this.UpdateMouseHandling(null);
                this.RevalidateUsage(true, !value);
                this.FireNotification(NotificationID.Enabled);
                if (!this.IsZoned || this.Parent != null && !this.Parent.FullyEnabled)
                    return;
                this.NotifyFullyEnabledChange();
            }
        }

        private bool HostEnabled => this._ownerHost == null ? this.IsValid : this._ownerHost.InputEnabled;

        public bool FullyEnabled => this.GetBit(UIClass.Bits.AppFullyEnabled);

        public bool DirectMouseFocus => this.GetBit(UIClass.Bits.DirectMouseFocus);

        public bool MouseFocus => this.GetBit(UIClass.Bits.MouseFocus);

        public bool DirectKeyFocus => this.GetBit(UIClass.Bits.DirectKeyFocus);

        public bool KeyFocus => this.GetBit(UIClass.Bits.KeyFocus);

        public UIClass KeyFocusDescendant
        {
            get
            {
                //uiClass = (UIClass)null;
                if (this.KeyFocus && this.UISession.InputManager.RawInstantaneousKeyFocus is UIClass uiClass)
                {
                    if (uiClass.IsDisposed)
                        return null;
                    else
                        return uiClass;
                }
                else
                {
                    return null;
                }
            }
        }

        public uint PaintOrder
        {
            get => this._ownerHost.Layer;
            set => this._ownerHost.Layer = value;
        }

        public bool CreateInterestOnFocus
        {
            get => this.GetBit(UIClass.Bits.CreateInterestOnFocus);
            set
            {
                if (!this.ChangeBit(UIClass.Bits.CreateInterestOnFocus, value))
                    return;
                this.FireNotification(NotificationID.CreateInterestOnFocus);
            }
        }

        public ViewItem FocusInterestTarget
        {
            get => (ViewItem)this.GetData(UIClass.s_focusInterestTargetProperty);
            set
            {
                if (this.FocusInterestTarget == value)
                    return;
                this.SetData(UIClass.s_focusInterestTargetProperty, value);
                this.FireNotification(NotificationID.FocusInterestTarget);
            }
        }

        public Inset FocusInterestTargetMargins
        {
            get
            {
                object data = this.GetData(UIClass.s_focusInterestTargetMarginsProperty);
                return data != null ? (Inset)data : Inset.Zero;
            }
            set
            {
                if (!(this.FocusInterestTargetMargins != value))
                    return;
                this.SetData(UIClass.s_focusInterestTargetMarginsProperty, value);
                this.FireNotification(NotificationID.FocusInterestTargetMargins);
            }
        }

        public CursorID Cursor
        {
            get
            {
                CursorID cursorId = CursorID.NotSpecified;
                object data = this.GetData(UIClass.s_cursorProperty);
                if (data != null)
                    cursorId = (CursorID)data;
                return cursorId;
            }
            set
            {
                if (this.Cursor == value)
                    return;
                this.SetData(UIClass.s_cursorProperty, value);
                this.FireNotification(NotificationID.Cursor);
                if (!this.IsZoned || this.OverrideCursor != CursorID.NotSpecified)
                    return;
                this.Zone.UpdateCursor(this);
            }
        }

        private CursorID OverrideCursor
        {
            get
            {
                CursorID cursorId = CursorID.NotSpecified;
                object data = this.GetData(UIClass.s_cursorOverrideProperty);
                if (data != null)
                    cursorId = (CursorID)data;
                return cursorId;
            }
            set
            {
                CursorID overrideCursor = this.OverrideCursor;
                if (overrideCursor == value)
                    return;
                this.SetData(UIClass.s_cursorOverrideProperty, value);
                if (!this.IsZoned || overrideCursor == CursorID.NotSpecified && value == this.Cursor)
                    return;
                this.Zone.UpdateCursor(this);
            }
        }

        public CursorID EffectiveCursor
        {
            get
            {
                CursorID cursorId = this.OverrideCursor;
                if (cursorId == CursorID.NotSpecified)
                    cursorId = this.Cursor;
                return cursorId;
            }
        }

        public void UpdateCursor()
        {
            CursorID cursorId = CursorID.NotSpecified;
            if (this._inputHandlers != null)
            {
                foreach (InputHandler inputHandler in this._inputHandlers)
                {
                    if (inputHandler.Enabled)
                    {
                        cursorId = inputHandler.GetCursor();
                        if (cursorId != CursorID.NotSpecified)
                            break;
                    }
                }
            }
            this.OverrideCursor = cursorId;
        }

        public bool KeyFocusOnMouseEnter
        {
            get => this.GetBit(UIClass.Bits.KeyFocusOnMouseEnter);
            set => this.SetBit(UIClass.Bits.KeyFocusOnMouseEnter, value);
        }

        public bool KeyFocusOnMouseDown
        {
            get => this.GetBit(UIClass.Bits.KeyFocusOnMouseDown);
            set => this.SetBit(UIClass.Bits.KeyFocusOnMouseDown, value);
        }

        public bool AllowDoubleClicks
        {
            get => this.GetBit(UIClass.Bits.AllowDoubleClicks);
            set => this.SetBit(UIClass.Bits.AllowDoubleClicks, value);
        }

        internal void NotifyFullyEnabledChange() => this.Zone.ScheduleFullyEnabledChangeNotifications();

        internal void DeliverFullyEnabled(bool enabledFlag)
        {
            if (!this.Enabled || !this.HostEnabled)
                enabledFlag = false;
            if (this.ChangeBit(UIClass.Bits.AppFullyEnabled, enabledFlag))
                this.FireNotification(NotificationID.FullyEnabled);
            foreach (UIClass child in this.Children)
                child.DeliverFullyEnabled(enabledFlag);
        }

        public void EnableRawInput(bool enableFlag)
        {
            if (!this.ChangeBit(UIClass.Bits.RawInputDisabled, !enableFlag))
                return;
            this.UpdateMouseHandling(null);
            this.RevalidateUsage(true, !enableFlag);
        }

        public bool IsInputBranchEnabled() => this.GetBit(UIClass.Bits.Enabled) && !this.GetBit(UIClass.Bits.RawInputDisabled);

        public bool IsEligibleForInput() => this.IsEligibleForInput(out UIClass _);

        public bool IsEligibleForInput(out UIClass failurePoint)
        {
            failurePoint = null;
            UIClass uiClass = this;
            while (true)
            {
                failurePoint = uiClass;
                if (uiClass.IsInputBranchEnabled() && uiClass.InputEnabled)
                {
                    UIClass parent = uiClass.Parent;
                    if (parent != null)
                        uiClass = parent;
                    else
                        goto label_5;
                }
                else
                    break;
            }
            return false;
        label_5:
            failurePoint = null;
            return this.IsZoned;
        }

        public bool MouseInteractive
        {
            get => this.GetBit(UIClass.Bits.MouseInteractive);
            set => this.SetMouseInteractive(value, false);
        }

        public void SetMouseInteractive(bool value, bool fromScript)
        {
            if (fromScript)
                this.SetBit(UIClass.Bits.MouseInteractiveSet, true);
            if (!fromScript && this.GetBit(UIClass.Bits.MouseInteractiveSet) || !this.ChangeBit(UIClass.Bits.MouseInteractive, value))
                return;
            if (value && this._rootItem != null && !this.HasMouseInteractiveContent())
                this._rootItem.MouseInteractive = true;
            this.UpdateMouseHandling(null);
            this.RevalidateUsage(false, !value);
            this.FireNotification(NotificationID.MouseInteractive);
            if (!DebugOutlines.Enabled)
                return;
            DebugOutlines.NotifyInteractivityChange(Host);
        }

        public bool IsMouseFocusable() => this.MouseInteractive && this.IsEligibleForInput();

        public bool KeyInteractive
        {
            get => this.GetBit(UIClass.Bits.KeyInteractive);
            set
            {
                if (!this.ChangeBit(UIClass.Bits.KeyInteractive, value))
                    return;
                this.RevalidateUsage(false, !value);
                this.FireNotification(NotificationID.KeyInteractive);
                if (!DebugOutlines.Enabled)
                    return;
                DebugOutlines.NotifyInteractivityChange(Host);
            }
        }

        public event SessionInputHandler SessionInput
        {
            add => this.Zone.SessionInput += value;
            remove => this.Zone.SessionInput -= value;
        }

        public bool HostActivated => this.Zone.Form.ActivationState;

        public bool IsKeyFocusable() => this.KeyInteractive && this.IsEligibleForInput();

        public UIClass FindKeyFocusableAncestor()
        {
            if (!this.IsValid)
                return null;
            UIClass uiClass;
            for (uiClass = this; uiClass != null; uiClass = uiClass.Parent)
            {
                UIClass failurePoint = null;
                if (uiClass.KeyInteractive && uiClass.IsEligibleForInput(out failurePoint))
                    return uiClass;
                if (failurePoint != null)
                    uiClass = failurePoint;
            }
            return uiClass;
        }

        public void NavigateInto() => this.NavigateInto(false);

        public void NavigateInto(bool isDefault) => this.RequestKeyFocus(isDefault ? KeyFocusReason.Default : KeyFocusReason.Other);

        public void RequestKeyFocus() => this.RequestKeyFocus(KeyFocusReason.Other);

        private void RequestKeyFocus(KeyFocusReason keyfocusReason)
        {
            if (!this.IsKeyFocusable())
                return;
            this.UISession.InputManager.Queue.RequestKeyFocus(this, keyfocusReason);
        }

        public bool IsValid => !this.IsDisposed;

        public static FocusStateHandler GetFocusUpdateProc(InputDeviceType focusType)
        {
            switch (focusType)
            {
                case InputDeviceType.Keyboard:
                    return UIClass.s_updateKeyFocusStates;
                case InputDeviceType.Mouse:
                    return UIClass.s_updateMouseFocusStates;
                default:
                    return null;
            }
        }

        private void RevalidateUsage(bool recursiveFlag, bool knownDisabledFlag)
        {
            if (!this.IsZoned)
                return;
            this.UISession.InputManager.RevalidateInputSiteUsage(this, recursiveFlag, knownDisabledFlag);
        }

        internal void OnInputEnabledChanged()
        {
            if (!this.IsZoned)
                return;
            this.RevalidateUsage(true, !this.InputEnabled);
            this.UpdateMouseHandling(null);
            if (!this.Enabled || this.Parent != null && !this.Parent.FullyEnabled)
                return;
            this.NotifyFullyEnabledChange();
        }

        IRawInputSite ICookedInputSite.RawInputSource
        {
            get
            {
                IRawInputSite rawInputSite = null;
                if (this._rootItem != null)
                    rawInputSite = _rootItem.RendererVisual;
                return rawInputSite;
            }
        }

        public void DeliverInput(InputInfo info, EventRouteStages stage)
        {
            if (stage != EventRouteStages.Unhandled)
            {
                switch (info.EventType)
                {
                    case InputEventType.CommandDown:
                    case InputEventType.CommandUp:
                    case InputEventType.KeyDown:
                    case InputEventType.KeyUp:
                    case InputEventType.KeyCharacter:
                    case InputEventType.MouseMove:
                    case InputEventType.MousePrimaryUp:
                    case InputEventType.MouseSecondaryUp:
                    case InputEventType.MouseWheel:
                    case InputEventType.DragEnter:
                    case InputEventType.DragOver:
                    case InputEventType.DragLeave:
                    case InputEventType.DragDropped:
                    case InputEventType.DragComplete:
                        this.DeliverInputEvent(info, stage);
                        break;
                    case InputEventType.GainKeyFocus:
                        this.DeliverGainKeyFocus(info, stage);
                        break;
                    case InputEventType.LoseKeyFocus:
                        this.DeliverLoseKeyFocus(info, stage);
                        break;
                    case InputEventType.GainMouseFocus:
                        this.DeliverGainMouseFocus(info, stage);
                        break;
                    case InputEventType.LoseMouseFocus:
                        this.DeliverFocusChange(info, stage);
                        break;
                    case InputEventType.MousePrimaryDown:
                    case InputEventType.MouseSecondaryDown:
                    case InputEventType.MouseDoubleClick:
                        this.DeliverMouseButtonDown(info, stage);
                        break;
                }
            }
            else
            {
                if (!(info is KeyStateInfo keyStateInfo) || keyStateInfo.Action != KeyAction.Down || (keyStateInfo.SystemKey || !this.KeyNavigate(keyStateInfo.Key, keyStateInfo.Modifiers)))
                    return;
                keyStateInfo.MarkHandled();
            }
        }

        private void DeliverInputEvent(InputInfo info, EventRouteStages stage)
        {
            if (info.Handled)
                return;
            this.ForwardEventToInputHandlers(info, stage);
        }

        private void ForwardEventToInputHandlers(InputInfo info, EventRouteStages stage)
        {
            if (this._inputHandlers == null)
                return;
            foreach (InputHandler inputHandler in this._inputHandlers)
            {
                inputHandler.DeliverInput(this, info, stage);
                if (UIClass.CheckHandled(info, inputHandler))
                    break;
            }
        }

        private void DeliverFocusChange(InputInfo info, EventRouteStages stage)
        {
            if (stage == EventRouteStages.Routed)
            {
                int num = info.Handled ? 1 : 0;
                this.DeliverCodeNotifications(info);
            }
            this.ForwardEventToInputHandlers(info, stage);
        }

        private void DeliverGainKeyFocus(InputInfo info, EventRouteStages stage)
        {
            if (stage == EventRouteStages.Direct)
                this.OnGainKeyFocus();
            this.DeliverFocusChange(info, stage);
        }

        private void DeliverLoseKeyFocus(InputInfo info, EventRouteStages stage)
        {
            if (stage == EventRouteStages.Direct)
                this.OnLoseKeyFocus();
            this.DeliverFocusChange(info, stage);
        }

        private void DeliverGainMouseFocus(InputInfo info, EventRouteStages stage)
        {
            if (stage == EventRouteStages.Direct && this.GetBit(UIClass.Bits.KeyFocusOnMouseEnter))
                this.RequestKeyFocus(KeyFocusReason.MouseEnter);
            this.DeliverFocusChange(info, stage);
        }

        private void DeliverMouseButtonDown(InputInfo args, EventRouteStages stage)
        {
            if (stage == EventRouteStages.Direct && this.GetBit(UIClass.Bits.KeyFocusOnMouseDown))
                this.RequestKeyFocus(KeyFocusReason.MouseDown);
            this.DeliverInputEvent(args, stage);
        }

        private void OnGainKeyFocus()
        {
            NavigationServices.SeedDefaultFocus(RootItem);
            if (this.CreateInterestOnFocus)
            {
                this.SetAreaOfInterest(AreaOfInterestID.Focus);
                ViewItem focusInterestTarget = this.FocusInterestTarget;
                if (focusInterestTarget != null && focusInterestTarget != this.RootItem)
                    this.SetAreaOfInterest(AreaOfInterestID.FocusOverride, focusInterestTarget, this.FocusInterestTargetMargins);
            }
            this.TryToPlayAnimationOnTree(this.RootItem, AnimationEventType.GainFocus);
        }

        private void OnLoseKeyFocus()
        {
            if (this.RootItem == null)
                return;
            if (this.CreateInterestOnFocus)
            {
                this.ClearAreaOfInterest(AreaOfInterestID.Focus);
                ViewItem focusInterestTarget = this.FocusInterestTarget;
                if (focusInterestTarget != null && focusInterestTarget != this.RootItem)
                    this.ClearAreaOfInterest(AreaOfInterestID.FocusOverride, focusInterestTarget);
            }
            this.TryToPlayAnimationOnTree(this.RootItem, AnimationEventType.LoseFocus);
        }

        private void OnGainDeepKeyFocus()
        {
            if (this._inputHandlers == null)
                return;
            foreach (InputHandler inputHandler in this._inputHandlers)
                inputHandler.NotifyGainDeepKeyFocus();
        }

        private void OnLoseDeepKeyFocus()
        {
            if (this._inputHandlers == null)
                return;
            foreach (InputHandler inputHandler in this._inputHandlers)
                inputHandler.NotifyLoseDeepKeyFocus();
        }

        private static void UpdateKeyFocusStates(
          UIClass recipient,
          bool deepFocusFlag,
          bool directFocusFlag)
        {
            if (recipient.ChangeBit(UIClass.Bits.KeyFocus, deepFocusFlag))
            {
                recipient.FireNotification(NotificationID.KeyFocus);
                if (deepFocusFlag)
                    recipient.OnGainDeepKeyFocus();
                else
                    recipient.OnLoseDeepKeyFocus();
            }
            if (!recipient.ChangeBit(UIClass.Bits.DirectKeyFocus, directFocusFlag))
                return;
            recipient.FireNotification(NotificationID.DirectKeyFocus);
            if (DebugOutlines.Enabled)
                DebugOutlines.NotifyInteractivityChange(recipient.Host);
            if (!directFocusFlag)
                return;
            AccessibleProxy.NotifyFocus(recipient);
        }

        public event InputEventHandler DescendentKeyFocusChange
        {
            add => this.AddEventHandler(UIClass.s_descendantKeyFocusChangedEvent, value);
            remove => this.RemoveEventHandler(UIClass.s_descendantKeyFocusChangedEvent, value);
        }

        private static void UpdateMouseFocusStates(
          UIClass recipient,
          bool deepFocusFlag,
          bool directFocusFlag)
        {
            if (recipient.ChangeBit(UIClass.Bits.MouseFocus, deepFocusFlag))
                recipient.FireNotification(NotificationID.MouseFocus);
            if (!recipient.ChangeBit(UIClass.Bits.DirectMouseFocus, directFocusFlag))
                return;
            recipient.FireNotification(NotificationID.DirectMouseFocus);
            if (!DebugOutlines.Enabled)
                return;
            DebugOutlines.NotifyInteractivityChange(recipient.Host);
        }

        public event InputEventHandler DescendentMouseFocusChange
        {
            add => this.AddEventHandler(UIClass.s_descendantMouseFocusChangedEvent, value);
            remove => this.RemoveEventHandler(UIClass.s_descendantMouseFocusChangedEvent, value);
        }

        private void DeliverCodeNotifications(InputInfo info)
        {
            EventCookie focusChangedEvent = EventCookie.NULL;
            switch (info.EventType)
            {
                case InputEventType.GainKeyFocus:
                case InputEventType.LoseKeyFocus:
                    focusChangedEvent = UIClass.s_descendantKeyFocusChangedEvent;
                    break;
                case InputEventType.GainMouseFocus:
                case InputEventType.LoseMouseFocus:
                    focusChangedEvent = UIClass.s_descendantMouseFocusChangedEvent;
                    break;
            }
            if (!(focusChangedEvent != EventCookie.NULL) || !(this.GetEventHandler(focusChangedEvent) is InputEventHandler eventHandler))
                return;
            eventHandler(this, info);
        }

        public void ApplyLayoutOutput(
          ViewItem subjectItem,
          bool parentFullyVisible,
          bool parentOffscreen,
          bool allowAnimations,
          out bool visibilityChange,
          out bool offscreenChange)
        {
            bool flag1 = subjectItem.LayoutVisible && parentFullyVisible;
            bool hasVisual = subjectItem.HasVisual;
            if (!flag1 && !hasVisual)
            {
                visibilityChange = false;
                offscreenChange = false;
            }
            else
            {
                visibilityChange = flag1 != hasVisual;
                bool flag2 = visibilityChange && hasVisual;
                bool flag3 = visibilityChange && !hasVisual;
                if (flag3)
                    subjectItem.CreateVisual(this.Zone.Session.RenderSession);
                Rectangle layoutBounds = subjectItem.LayoutBounds;
                Vector2 vector2 = new Vector2(layoutBounds.Width, layoutBounds.Height);
                Vector3 vector3_1 = new Vector3(layoutBounds.X, layoutBounds.Y, 0.0f);
                Vector3 vector3_2 = subjectItem.LayoutScale * subjectItem.Scale;
                Rotation layoutRotation = subjectItem.LayoutRotation;
                Vector3 oldScaleVector;
                if (!hasVisual)
                {
                    this.AnimateNewVisual(subjectItem, vector3_1, vector2, vector3_2, layoutRotation);
                    oldScaleVector = vector3_2;
                }
                else
                {
                    Vector2 visualSize = subjectItem.VisualSize;
                    oldScaleVector = subjectItem.VisualScale;
                }
                bool flag4 = subjectItem.LayoutOffscreen || parentOffscreen;
                offscreenChange = flag4 != subjectItem.IsOffscreen;
                if (offscreenChange)
                    subjectItem.IsOffscreen = flag4;
                if (offscreenChange || flag3)
                    this.UpdateMouseHandling(subjectItem);
                bool flag5 = hasVisual;
                if (flag5 && flag2)
                    flag5 = false;
                if (flag5)
                    this.AnimateVisual(subjectItem, vector3_1, vector2, vector3_2, layoutRotation);
                if (flag2)
                {
                    if (subjectItem.IsRoot)
                        return;
                    this.DestroyVisualTree(subjectItem, allowAnimations, false);
                }
                else
                {
                    if (!(oldScaleVector != vector3_2) && hasVisual)
                        return;
                    subjectItem.OnScaleChange(oldScaleVector, vector3_2);
                }
            }
        }

        public bool FindNextFocusablePeer(
          Direction searchDirection,
          RectangleF startRectangleF,
          out UIClass resultUI)
        {
            if (this._rootItem != null)
                return this.FindNextFocusablePeerWorker(this._rootItem, searchDirection, startRectangleF, out resultUI);
            resultUI = null;
            return false;
        }

        private bool FindNextFocusablePeerWorker(
          ViewItem startItem,
          Direction searchDirection,
          RectangleF startRectangleF,
          out UIClass resultUI)
        {
            INavigationSite resultSite;
            bool nextPeer = NavigationServices.FindNextPeer(startItem, searchDirection, startRectangleF, out resultSite);
            resultUI = null;
            if (resultSite != null && resultSite is ViewItem viewItem)
                resultUI = viewItem.UI;
            return nextPeer;
        }

        internal bool FindNextFocusableWithin(
          Direction searchDirection,
          RectangleF startRectangleF,
          out UIClass resultUI)
        {
            INavigationSite resultSite;
            bool nextWithin = NavigationServices.FindNextWithin(_rootItem, searchDirection, startRectangleF, out resultSite);
            resultUI = null;
            if (resultSite != null && resultSite is ViewItem viewItem)
                resultUI = viewItem.UI;
            return nextWithin;
        }

        internal NavigationClass GetNavigability(ViewItem subjectItem)
        {
            NavigationClass navigationClass = NavigationClass.None;
            if (subjectItem == this._rootItem && this.IsKeyFocusable())
                navigationClass = NavigationClass.Direct;
            return navigationClass;
        }

        public void NotifyNavigationDestination(KeyFocusReason keyFocusReason) => this.RequestKeyFocus(keyFocusReason);

        public bool InboundKeyNavigation(
          Direction searchDirection,
          RectangleF startRectangleF,
          bool defaultFlag)
        {
            UIClass resultUI;
            if (!this.FindNextFocusableWithin(searchDirection, startRectangleF, out resultUI) || resultUI == null)
                return false;
            resultUI.NotifyNavigationDestination(defaultFlag ? KeyFocusReason.Default : KeyFocusReason.Other);
            return true;
        }

        public bool NavigateDirection(Direction direction, KeyFocusReason reason)
        {
            UIClass resultUI;
            if (!this.FindNextFocusablePeer(direction, RectangleF.Zero, out resultUI) || resultUI == null)
                return false;
            UIClass uiClass = this;
            resultUI.NotifyNavigationDestination(reason);
            return true;
        }

        private bool KeyNavigate(Keys key, InputModifiers modifiers)
        {
            InputHandlerModifiers modifiers1 = InputHandler.GetModifiers(modifiers);
            KeyHandler.TranslateKey(ref key, ref modifiers1);
            KeyFocusReason reason = KeyFocusReason.Directional;
            Direction direction;
            switch (key)
            {
                case Keys.Tab:
                    direction = (modifiers1 & InputHandlerModifiers.Shift) != InputHandlerModifiers.Shift ? Direction.Next : Direction.Previous;
                    reason = KeyFocusReason.Tab;
                    break;
                case Keys.Left:
                    direction = Direction.West;
                    break;
                case Keys.Up:
                    direction = Direction.North;
                    break;
                case Keys.Right:
                    direction = Direction.East;
                    break;
                case Keys.Down:
                    direction = Direction.South;
                    break;
                default:
                    return false;
            }
            return this.NavigateDirection(direction, reason);
        }

        public TypeSchema TypeSchema => _typeSchema;

        public void NotifyInitialized()
        {
            if (this._inputHandlers != null)
            {
                foreach (InputHandler inputHandler in this._inputHandlers)
                    inputHandler.NotifyUIInitialized();
            }
            if (this._rootItem != null)
            {
                if (this.MouseInteractive)
                {
                    if (!this.HasMouseInteractiveContent())
                        this._rootItem.MouseInteractive = true;
                }
                else if (this.HasMouseInteractiveContent())
                    this.MouseInteractive = true;
            }
            this.SetBit(UIClass.Bits.Initialized, true);
            AccessibleProxy.NotifyCreated(this);
        }

        public object ReadSymbol(SymbolReference symbolRef)
        {
            object obj = null;
            switch (symbolRef.Origin)
            {
                case SymbolOrigin.Properties:
                case SymbolOrigin.Locals:
                    obj = this._storage[symbolRef.Symbol];
                    break;
                case SymbolOrigin.Input:
                    if (this._inputHandlers != null)
                    {
                        foreach (InputHandler inputHandler in this._inputHandlers)
                        {
                            if (inputHandler.Name != null && object.ReferenceEquals(inputHandler.Name, symbolRef.Symbol))
                            {
                                obj = inputHandler;
                                break;
                            }
                        }
                        break;
                    }
                    break;
                case SymbolOrigin.Content:
                    obj = this.FindViewItemByName(this.RootItem, symbolRef.Symbol);
                    break;
                case SymbolOrigin.Reserved:
                    if (symbolRef.Symbol == "UI")
                    {
                        obj = this;
                        break;
                    }
                    break;
            }
            return obj;
        }

        public ViewItem FindViewItemByName(ViewItem item, string name)
        {
            if (object.ReferenceEquals(item.Name, name))
                return item;
            if (item.HideNamedChildren)
                return null;
            foreach (ViewItem child in item.Children)
            {
                if (child.UI == this)
                {
                    ViewItem viewItemByName = this.FindViewItemByName(child, name);
                    if (viewItemByName != null)
                        return viewItemByName;
                }
            }
            return null;
        }

        public void WriteSymbol(SymbolReference symbolRef, object value)
        {
            string symbol = symbolRef.Symbol;
            if (this._storage.ContainsKey(symbol) && Utility.IsEqual(this._storage[symbol], value))
                return;
            this._storage[symbol] = value;
            if (symbolRef.Origin != SymbolOrigin.Properties && symbolRef.Origin != SymbolOrigin.Locals)
                return;
            bool surfaceViaHost = symbolRef.Origin == SymbolOrigin.Properties;
            this.FireNotification(symbol, surfaceViaHost);
        }

        public object GetProperty(string name) => this._storage[name];

        public void SetProperty(string name, object value)
        {
            if (this._storage.ContainsKey(name) && Utility.IsEqual(this._storage[name], value))
                return;
            this._storage[name] = value;
            this.FireNotification(name, true);
        }

        public Dictionary<object, object> Storage => this._storage;

        public MarkupListeners Listeners
        {
            get => this._listeners;
            set => this._listeners = value;
        }

        public void ScheduleScriptRun(uint scriptId, bool ignoreErrors)
        {
            if (!this._scriptRunScheduler.Pending)
                DeferredCall.Post(DispatchPriority.Script, UIClass.s_executePendingScriptsHandler, this);
            this._scriptRunScheduler.ScheduleRun(scriptId, ignoreErrors);
        }

        private static void ExecutePendingScripts(object args)
        {
            UIClass uiClass = (UIClass)args;
            uiClass._scriptRunScheduler.Execute(uiClass);
        }

        public object RunScript(uint scriptId, bool ignoreErrors, ParameterContext parameterContext) => this._typeSchema.Run(this, scriptId, ignoreErrors, parameterContext);

        public void NotifyScriptErrors()
        {
            this.SetBit(UIClass.Bits.ScriptEnabled, false);
            this._ownerHost.NotifyChildUIScriptErrors();
            ErrorManager.ReportWarning("Script runtime failure: Scripting has been disabled for '{0}' due to runtime scripting errors", _typeSchema.Name);
        }

        public bool ScriptEnabled => this.GetBit(UIClass.Bits.ScriptEnabled);

        protected void FireNotification(string id) => this.FireNotification(id, false);

        private void FireNotification(string id, bool surfaceViaHost)
        {
            this._notifier.Fire(id);
            if (!surfaceViaHost || this._ownerHost == null)
                return;
            this._ownerHost.FireChildUINotification(id);
        }

        void INotifyObject.AddListener(Listener listener) => this._notifier.AddListener(listener);

        public ViewItem ConstructNamedContent(
          string contentName,
          ParameterContext parameterContext)
        {
            return this._typeSchema.ConstructNamedContent(contentName, this, parameterContext);
        }

        private bool GetBit(UIClass.Bits lookupBit) => this._bits[(int)lookupBit];

        private void SetBit(UIClass.Bits changeBit, bool value) => this._bits[(int)changeBit] = value;

        private bool ChangeBit(UIClass.Bits bit, bool value)
        {
            if (this._bits[(int)bit] == value)
                return false;
            this._bits[(int)bit] = value;
            return true;
        }

        public ViewItem RootItem => this._rootItem;

        public string ID => this._ownerHost.Name;

        public Host Host => this._ownerHost;

        public void DeclareHost(Host ownerHost) => this._ownerHost = ownerHost;

        public bool Flippable
        {
            get => this.GetBit(UIClass.Bits.Flippable);
            set => this.SetBit(UIClass.Bits.Flippable, value);
        }

        internal void SetRootItem(ViewItem newRootItem) => this._rootItem = newRootItem;

        internal bool HasMouseInteractiveContent() => this._rootItem != null && this.HasMouseInteractiveContentWorker(this._rootItem);

        private bool HasMouseInteractiveContentWorker(ViewItem checkItem)
        {
            if (checkItem.MouseInteractive)
                return true;
            foreach (ViewItem child in checkItem.Children)
            {
                if (child.UI == this && this.HasMouseInteractiveContentWorker(child))
                    return true;
            }
            return false;
        }

        public object GetEventContext() => this._eventContext != null ? this._eventContext.Value : null;

        internal void SetEventContext(EventContext eventContext) => this._eventContext = eventContext;

        public override string ToString() => this._typeSchema.ToString();

        private class SavedFocusState
        {
            private ViewItemID[] _path;
            private bool _focusIsDefault;

            public SavedFocusState(ViewItem item)
            {
                this._path = item.IDPath;
                this._focusIsDefault = item.UISession.InputManager.RawKeyFocusIsDefault;
            }

            public ViewItemID[] Path => this._path;

            public bool FocusIsDefault => this._focusIsDefault;
        }

        private class DeferredKeyFocusRestoreHelper
        {
            private UIClass _ui;
            private UIClass.SavedFocusState _focusState;
            private ICookedInputSite _lastFocus;

            public DeferredKeyFocusRestoreHelper(UIClass ui, UIClass.SavedFocusState focusState)
            {
                this._ui = ui;
                this._focusState = focusState;
                this._lastFocus = ui.UISession.InputManager.RawKeyFocus;
            }

            public void FaultInChild(ViewItem bottleneck, ViewItemID item)
            {
                this._ui.PendingFocusRestore = this;
                bottleneck.FaultInChild(item, new ChildFaultedInDelegate(this.ChildFaultedIn));
            }

            private void ChildFaultedIn(ViewItem bottleneckItem, ViewItem faultedInItem)
            {
                if (!this._ui.IsZoned || this._ui.PendingFocusRestore != this)
                    return;
                this._ui.PendingFocusRestore = null;
                InputManager inputManager = this._ui.UISession.InputManager;
                if (!inputManager.RawKeyFocusIsDefault && inputManager.RawKeyFocus != this._lastFocus || (faultedInItem == null || faultedInItem.IsDisposed))
                    return;
                this._ui.RestoreKeyFocus(_focusState);
            }
        }

        private enum Bits
        {
            Initialized = 1,
            IsDisposed = 2,
            CreateInterestOnFocus = 4,
            Flippable = 8,
            KeyInteractive = 16, // 0x00000010
            MouseInteractive = 32, // 0x00000020
            MouseInteractiveSet = 64, // 0x00000040
            Enabled = 128, // 0x00000080
            AppFullyEnabled = 256, // 0x00000100
            RawInputDisabled = 512, // 0x00000200
            DirectKeyFocus = 1024, // 0x00000400
            DirectMouseFocus = 2048, // 0x00000800
            KeyFocusOnMouseEnter = 4096, // 0x00001000
            KeyFocusOnMouseDown = 8192, // 0x00002000
            AllowDoubleClicks = 16384, // 0x00004000
            KeyFocus = 32768, // 0x00008000
            MouseFocus = 65536, // 0x00010000
            HasAccProxy = 131072, // 0x00020000
            ScriptEnabled = 262144, // 0x00040000
            HasAutomationInstanceId = 524288, // 0x00080000
        }
    }
}
