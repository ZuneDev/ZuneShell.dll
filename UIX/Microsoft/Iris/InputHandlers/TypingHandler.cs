// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.InputHandlers.TypingHandler
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Input;
using Microsoft.Iris.Markup;
using Microsoft.Iris.ModelItems;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.InputHandlers
{
    internal class TypingHandler : InputHandler
    {
        private EditableTextData _edit;
        private bool _submitOnEnter;
        private bool _treatEscapeAsBackspace;
        private bool _handlingBackspaceFlag;
        private bool _handlingDeleteFlag;

        protected override void ConfigureInteractivity()
        {
            base.ConfigureInteractivity();
            if (!this.HandleDirect)
                return;
            this.UI.MouseInteractive = true;
            this.UI.KeyInteractive = true;
        }

        public bool SubmitOnEnter
        {
            get => this._submitOnEnter;
            set
            {
                if (this._submitOnEnter == value)
                    return;
                this._submitOnEnter = value;
                this.FireNotification(NotificationID.SubmitOnEnter);
            }
        }

        public bool TreatEscapeAsBackspace
        {
            get => this._treatEscapeAsBackspace;
            set
            {
                if (this._treatEscapeAsBackspace == value)
                    return;
                this._treatEscapeAsBackspace = value;
                this.FireNotification(NotificationID.TreatEscapeAsBackspace);
            }
        }

        private void FireTypingInputRejectedEvent() => this.FireNotification(NotificationID.TypingInputRejected);

        public EditableTextData EditableTextData
        {
            get => this._edit;
            set
            {
                if (this._edit == value)
                    return;
                this._edit = value;
                this.FireNotification(NotificationID.EditableTextData);
            }
        }

        protected override void OnKeyDown(UIClass ui, KeyStateInfo info)
        {
            if (this.ShouldIgnoreInput(info) || this._edit == null)
                return;
            switch (info.Key)
            {
                case Keys.Back:
                    if (this.IsEditableTextEmpty() && !this._handlingBackspaceFlag)
                        break;
                    info.MarkHandled();
                    break;
                case Keys.Enter:
                    if (!this._submitOnEnter)
                        break;
                    this._edit.Submit();
                    info.MarkHandled();
                    break;
                case Keys.Escape:
                    if (!this._treatEscapeAsBackspace)
                        break;
                    goto case Keys.Back;
                case Keys.Delete:
                    if (this.IsEditableTextEmpty())
                        break;
                    this.RemoveChar();
                    info.MarkHandled();
                    this._handlingDeleteFlag = true;
                    break;
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
                    info.MarkHandled();
                    break;
            }
        }

        protected override void OnKeyCharacter(UIClass ui, KeyCharacterInfo info)
        {
            if (this.ShouldIgnoreInput(info) || this._edit == null)
                return;
            switch (info.Character)
            {
                case '\b':
                    if (!this.IsEditableTextEmpty())
                    {
                        this.RemoveChar();
                        info.MarkHandled();
                        this._handlingBackspaceFlag = true;
                        break;
                    }
                    if (!this._handlingBackspaceFlag)
                        break;
                    info.MarkHandled();
                    break;
                case '\t':
                    break;
                case '\r':
                    if (!this._submitOnEnter)
                        break;
                    info.MarkHandled();
                    break;
                case '\x001B':
                    if (!this._treatEscapeAsBackspace)
                        break;
                    goto case '\b';
                default:
                    this.InputChar(info.Character);
                    info.MarkHandled();
                    break;
            }
        }

        protected override void OnKeyUp(UIClass ui, KeyStateInfo info)
        {
            if (this.ShouldIgnoreInput(info) || this._edit == null)
                return;
            switch (info.Key)
            {
                case Keys.Back:
                    if (!this._handlingBackspaceFlag)
                        break;
                    info.MarkHandled();
                    this._handlingBackspaceFlag = false;
                    break;
                case Keys.Enter:
                    if (!this._submitOnEnter)
                        break;
                    info.MarkHandled();
                    break;
                case Keys.Escape:
                    if (!this._treatEscapeAsBackspace)
                        break;
                    goto case Keys.Back;
                case Keys.Delete:
                    if (!this._handlingDeleteFlag)
                        break;
                    info.MarkHandled();
                    this._handlingDeleteFlag = false;
                    break;
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
                    info.MarkHandled();
                    break;
            }
        }

        private void RemoveChar()
        {
            if (this._edit == null || this.IsEditableTextEmpty())
                return;
            if (!this._edit.ReadOnly)
                this._edit.Value = this._edit.Value.Substring(0, this._edit.Value.Length - 1);
            else
                this.FireTypingInputRejectedEvent();
        }

        private void Clear()
        {
            if (this._edit == null || this.IsEditableTextEmpty())
                return;
            if (!this._edit.ReadOnly)
                this._edit.Value = string.Empty;
            else
                this.FireTypingInputRejectedEvent();
        }

        private void InputChar(char ch)
        {
            if (this._edit == null)
                return;
            if (!this._edit.ReadOnly && (this.IsEditableTextEmpty() || this._edit.Value.Length < this._edit.MaxLength))
                this._edit.Value += (string)(object)ch;
            else
                this.FireTypingInputRejectedEvent();
        }

        private bool ShouldIgnoreInput(KeyActionInfo info) => (info.Modifiers & InputModifiers.ControlKey) == InputModifiers.ControlKey || (info.Modifiers & InputModifiers.AltKey) == InputModifiers.AltKey || (info.Modifiers & InputModifiers.WindowsKey) == InputModifiers.WindowsKey;

        private bool IsEditableTextEmpty() => string.IsNullOrEmpty(this._edit.Value);
    }
}
