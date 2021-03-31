// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.UI.InputHandler
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Input;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.Session;
using System;

namespace Microsoft.Iris.UI
{
    internal abstract class InputHandler : DisposableNotifyObjectBase
    {
        private InputHandlerStage _handlerStage;
        private UIClass _ui;
        private bool _disabled;
        private string _name;

        public InputHandler() => this._handlerStage = InputHandlerStage.Direct;

        public InputHandlerStage HandlerStage
        {
            get => this._handlerStage;
            set
            {
                if (this._handlerStage == value)
                    return;
                this._handlerStage = value;
                this.FireNotification(NotificationID.HandlerStage);
            }
        }

        protected bool HandleDirect => (this.HandlerStage & InputHandlerStage.Direct) == InputHandlerStage.Direct;

        protected bool HandleRouted => (this.HandlerStage & InputHandlerStage.Routed) == InputHandlerStage.Routed;

        protected bool HandleBubbled => (this.HandlerStage & InputHandlerStage.Bubbled) == InputHandlerStage.Bubbled;

        private bool ShouldHandleStage(EventRouteStages stage)
        {
            switch (stage)
            {
                case EventRouteStages.Direct:
                    return this.HandleDirect;
                case EventRouteStages.Bubbled:
                    return this.HandleBubbled;
                case EventRouteStages.Routed:
                    return this.HandleRouted;
                default:
                    return false;
            }
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            this._ui = null;
        }

        protected override void OnOwnerDeclared(object owner)
        {
            base.OnOwnerDeclared(owner);
            this._ui = (UIClass)owner;
        }

        public void NotifyUIInitialized() => this.ConfigureInteractivity();

        protected virtual void ConfigureInteractivity()
        {
        }

        public virtual void OnZoneAttached()
        {
        }

        public virtual void OnZoneDetached()
        {
        }

        public bool Enabled
        {
            get => !this._disabled;
            set
            {
                if (this.Enabled == value)
                    return;
                this._disabled = !value;
                this.FireNotification(NotificationID.Enabled);
                this._ui.UpdateCursor();
            }
        }

        public string Name
        {
            get => this._name;
            set => this._name = NotifyService.CanonicalizeString(value);
        }

        public void DeliverInput(UIClass ui, InputInfo info, EventRouteStages stage)
        {
            if (!this.Enabled || !this.ShouldHandleStage(stage))
                return;
            switch (info.EventType)
            {
                case InputEventType.CommandDown:
                    this.OnCommandDown(ui, (KeyCommandInfo)info);
                    break;
                case InputEventType.CommandUp:
                    this.OnCommandUp(ui, (KeyCommandInfo)info);
                    break;
                case InputEventType.GainKeyFocus:
                    this.OnGainKeyFocus(ui, (KeyFocusInfo)info);
                    break;
                case InputEventType.LoseKeyFocus:
                    this.OnLoseKeyFocus(ui, (KeyFocusInfo)info);
                    break;
                case InputEventType.KeyDown:
                    this.OnKeyDown(ui, (KeyStateInfo)info);
                    break;
                case InputEventType.KeyUp:
                    this.OnKeyUp(ui, (KeyStateInfo)info);
                    break;
                case InputEventType.KeyCharacter:
                    this.OnKeyCharacter(ui, (KeyCharacterInfo)info);
                    break;
                case InputEventType.MouseMove:
                    this.OnMouseMove(ui, (MouseMoveInfo)info);
                    break;
                case InputEventType.GainMouseFocus:
                    this.OnGainMouseFocus(ui, (MouseFocusInfo)info);
                    break;
                case InputEventType.LoseMouseFocus:
                    this.OnLoseMouseFocus(ui, (MouseFocusInfo)info);
                    break;
                case InputEventType.MousePrimaryDown:
                    this.OnMousePrimaryDown(ui, (MouseButtonInfo)info);
                    break;
                case InputEventType.MouseSecondaryDown:
                    this.OnMouseSecondaryDown(ui, (MouseButtonInfo)info);
                    break;
                case InputEventType.MousePrimaryUp:
                    this.OnMousePrimaryUp(ui, (MouseButtonInfo)info);
                    break;
                case InputEventType.MouseSecondaryUp:
                    this.OnMouseSecondaryUp(ui, (MouseButtonInfo)info);
                    break;
                case InputEventType.MouseDoubleClick:
                    this.OnMouseDoubleClick(ui, (MouseButtonInfo)info);
                    break;
                case InputEventType.MouseWheel:
                    this.OnMouseWheel(ui, (MouseWheelInfo)info);
                    break;
                case InputEventType.DragEnter:
                    this.OnDragEnter(ui, (DragDropInfo)info);
                    break;
                case InputEventType.DragOver:
                    this.OnDragOver(ui, (DragDropInfo)info);
                    break;
                case InputEventType.DragLeave:
                    this.OnDragLeave(ui, (DragDropInfo)info);
                    break;
                case InputEventType.DragDropped:
                    this.OnDropped(ui, (DragDropInfo)info);
                    break;
                case InputEventType.DragComplete:
                    this.OnDragComplete(ui, (DragDropInfo)info);
                    break;
            }
        }

        protected virtual void OnMousePrimaryDown(UIClass ui, MouseButtonInfo info)
        {
        }

        protected virtual void OnMouseSecondaryDown(UIClass ui, MouseButtonInfo info)
        {
        }

        protected virtual void OnMousePrimaryUp(UIClass ui, MouseButtonInfo info)
        {
        }

        protected virtual void OnMouseSecondaryUp(UIClass ui, MouseButtonInfo info)
        {
        }

        protected virtual void OnMouseDoubleClick(UIClass ui, MouseButtonInfo info)
        {
        }

        protected virtual void OnMouseMove(UIClass ui, MouseMoveInfo info)
        {
        }

        protected virtual void OnLoseMouseFocus(UIClass ui, MouseFocusInfo info)
        {
        }

        protected virtual void OnGainMouseFocus(UIClass ui, MouseFocusInfo info)
        {
        }

        protected virtual void OnMouseWheel(UIClass ui, MouseWheelInfo info)
        {
        }

        protected virtual void OnDragEnter(UIClass ui, DragDropInfo info)
        {
        }

        protected virtual void OnDragOver(UIClass ui, DragDropInfo info)
        {
        }

        protected virtual void OnDragLeave(UIClass ui, DragDropInfo info)
        {
        }

        protected virtual void OnDropped(UIClass ui, DragDropInfo info)
        {
        }

        protected virtual void OnDragComplete(UIClass ui, DragDropInfo info)
        {
        }

        protected virtual void OnKeyDown(UIClass ui, KeyStateInfo info)
        {
        }

        protected virtual void OnKeyUp(UIClass ui, KeyStateInfo info)
        {
        }

        protected virtual void OnKeyCharacter(UIClass ui, KeyCharacterInfo info)
        {
        }

        protected virtual void OnLoseKeyFocus(UIClass ui, KeyFocusInfo info)
        {
        }

        protected virtual void OnGainKeyFocus(UIClass ui, KeyFocusInfo info)
        {
        }

        protected virtual void OnLoseDeepKeyFocus()
        {
        }

        protected virtual void OnGainDeepKeyFocus()
        {
        }

        protected virtual void OnCommandDown(UIClass ui, KeyCommandInfo info)
        {
        }

        protected virtual void OnCommandUp(UIClass ui, KeyCommandInfo info)
        {
        }

        internal void NotifyLoseDeepKeyFocus() => this.OnLoseDeepKeyFocus();

        internal void NotifyGainDeepKeyFocus() => this.OnGainDeepKeyFocus();

        internal virtual CursorID GetCursor() => CursorID.NotSpecified;

        protected void UpdateCursor()
        {
            if (this._ui == null)
                return;
            this._ui.UpdateCursor();
        }

        public static InputHandlerModifiers GetModifiers(
          InputModifiers rawModifiers)
        {
            InputHandlerModifiers handlerModifiers = InputHandlerModifiers.None;
            if ((rawModifiers & InputModifiers.ControlKey) != InputModifiers.None)
                handlerModifiers |= InputHandlerModifiers.Ctrl;
            if ((rawModifiers & InputModifiers.ShiftKey) != InputModifiers.None)
                handlerModifiers |= InputHandlerModifiers.Shift;
            if ((rawModifiers & InputModifiers.AltKey) != InputModifiers.None)
                handlerModifiers |= InputHandlerModifiers.Alt;
            if ((rawModifiers & InputModifiers.WindowsKey) != InputModifiers.None)
                handlerModifiers |= InputHandlerModifiers.Windows;
            return handlerModifiers;
        }

        protected void SetEventContext(
          ICookedInputSite source,
          ref WeakReference context,
          string contextName)
        {
            object obj = context != null ? context.Target : null;
            object eventContext = this.GetEventContext(source);
            if (eventContext == obj)
                return;
            context = new WeakReference(eventContext);
            this.FireNotification(contextName);
        }

        protected object CheckEventContext(ref WeakReference context)
        {
            object obj = null;
            if (context != null)
            {
                obj = context.Target;
                if (obj is IDisposableObject disposableObject && disposableObject.IsDisposed)
                {
                    context = null;
                    obj = null;
                }
            }
            return obj;
        }

        protected object GetEventContext(ICookedInputSite clickTarget)
        {
            if (!(clickTarget is UIClass uiClass) || !uiClass.IsValid)
                return null;
            object eventContext;
            for (eventContext = uiClass.GetEventContext(); eventContext == null && uiClass != this.UI; eventContext = uiClass.GetEventContext())
                uiClass = uiClass.Parent;
            return eventContext;
        }

        public override string ToString() => this.GetType().Name;

        protected UIClass UI => this._ui;
    }
}
