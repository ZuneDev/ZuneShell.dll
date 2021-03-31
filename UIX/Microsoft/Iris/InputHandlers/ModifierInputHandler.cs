// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.InputHandlers.ModifierInputHandler
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.InputHandlers
{
    internal class ModifierInputHandler : InputHandler
    {
        private InputHandlerModifiers _requiredModifiers;
        private InputHandlerModifiers _disallowedModifiers;
        private InputHandlerTransition _handlerTransition;

        public ModifierInputHandler()
        {
            this._handlerTransition = InputHandlerTransition.Up;
            this._requiredModifiers = InputHandlerModifiers.None;
            this._disallowedModifiers = InputHandlerModifiers.None;
        }

        public InputHandlerTransition HandlerTransition
        {
            get => this._handlerTransition;
            set
            {
                if (this._handlerTransition == value)
                    return;
                this._handlerTransition = value;
                this.FireNotification(NotificationID.HandlerTransition);
            }
        }

        public InputHandlerModifiers RequiredModifiers
        {
            get => this._requiredModifiers;
            set
            {
                if (this._requiredModifiers == value)
                    return;
                this._requiredModifiers = value;
                this.FireNotification(NotificationID.RequiredModifiers);
            }
        }

        public InputHandlerModifiers DisallowedModifiers
        {
            get => this._disallowedModifiers;
            set
            {
                if (this._disallowedModifiers == value)
                    return;
                this._disallowedModifiers = value;
                this.FireNotification(NotificationID.DisallowedModifiers);
            }
        }

        protected bool ShouldHandleEvent(InputHandlerModifiers modifiers) => (this._disallowedModifiers == InputHandlerModifiers.None || !Microsoft.Iris.Library.Bits.TestAnyFlags((uint)modifiers, (uint)this._disallowedModifiers)) && (this._requiredModifiers == InputHandlerModifiers.None || Microsoft.Iris.Library.Bits.TestAllFlags((uint)modifiers, (uint)this._requiredModifiers));
    }
}
