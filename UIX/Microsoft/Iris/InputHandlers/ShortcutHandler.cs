// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.InputHandlers.ShortcutHandler
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Input;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.ModelItems;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.InputHandlers
{
    internal class ShortcutHandler : InputHandler
    {
        private ShortcutHandlerCommand _shortcut;
        private IUICommand _command;
        private bool _handle;

        public ShortcutHandler() => this._handle = true;

        protected override void ConfigureInteractivity()
        {
            base.ConfigureInteractivity();
            if (!this.HandleDirect)
                return;
            this.UI.KeyInteractive = true;
        }

        public ShortcutHandlerCommand Shortcut
        {
            get => this._shortcut;
            set
            {
                if (this._shortcut == value)
                    return;
                this._shortcut = value;
                this.FireNotification(NotificationID.Shortcut);
            }
        }

        public IUICommand Command
        {
            get => this._command;
            set
            {
                if (this._command == value)
                    return;
                this._command = value;
                this.FireNotification(NotificationID.Command);
            }
        }

        public bool Handle
        {
            get => this._handle;
            set
            {
                if (this._handle == value)
                    return;
                this._handle = value;
                this.FireNotification(NotificationID.Handle);
            }
        }

        protected override void OnCommandDown(UIClass ui, KeyCommandInfo info)
        {
            if (info.Command != (CommandCode)this._shortcut)
                return;
            this.InvokeCommand();
            if (!this._handle)
                return;
            info.MarkHandled();
        }

        protected override void OnCommandUp(UIClass ui, KeyCommandInfo info)
        {
            if (info.Command != (CommandCode)this._shortcut || !this._handle)
                return;
            info.MarkHandled();
        }

        private void InvokeCommand()
        {
            this.FireNotification(NotificationID.Invoked);
            if (this._command == null)
                return;
            this._command.Invoke();
        }

        public override string ToString() => InvariantString.Format("{0}({1})", this.GetType().Name, _shortcut);
    }
}
