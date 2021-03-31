// Decompiled with JetBrains decompiler
// Type: UIXControls.MessageBox
// Assembly: UIXControls, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: 78800EA5-2757-404C-BA30-C33FCFC2852A
// Assembly location: C:\Program Files\Zune\UIXControls.dll

using Microsoft.Iris;
using System;

namespace UIXControls
{
    public class MessageBox : DialogHelper
    {
        private string _title;
        private string _message;
        private bool _isOKDefault;
        private Command _okCommand;
        private Command _yesCommand;
        private Command _noCommand;
        private BooleanChoice _doNotAskMeAgain;

        public string Title
        {
            get => this._title;
            set => this._title = value;
        }

        public string Message
        {
            get => this._message;
            set => this._message = value;
        }

        public Command Yes => this._yesCommand;

        public Command No => this._noCommand;

        public Command Ok => this._okCommand;

        public BooleanChoice DoNotAskMeAgain => this._doNotAskMeAgain;

        public bool IsOKDefault => this._isOKDefault;

        public static MessageBox Show(
          string title,
          string message,
          EventHandler okCommandHandler)
        {
            MessageBox dialog = new MessageBox(title, message, okCommandHandler, (EventHandler)null, (EventHandler)null, (EventHandler)null, (BooleanChoice)null);
            MessageBox.ShowCodeDialog(dialog);
            return dialog;
        }

        public static MessageBox Show(
          string title,
          string message,
          EventHandler yesCommandHandler,
          EventHandler noCommandHandler)
        {
            MessageBox dialog = new MessageBox(title, message, (EventHandler)null, yesCommandHandler, noCommandHandler, (EventHandler)null, (BooleanChoice)null);
            MessageBox.ShowCodeDialog(dialog);
            return dialog;
        }

        public static MessageBox Show(
          string title,
          string message,
          EventHandler okCommandHandler,
          EventHandler yesCommandHandler,
          EventHandler noCommandHandler,
          EventHandler cancelCommandHandler)
        {
            MessageBox dialog = new MessageBox(title, message, okCommandHandler, yesCommandHandler, noCommandHandler, cancelCommandHandler, (BooleanChoice)null);
            MessageBox.ShowCodeDialog(dialog);
            return dialog;
        }

        public static MessageBox Show(
          string title,
          string message,
          EventHandler okCommandHandler,
          EventHandler yesCommandHandler,
          EventHandler noCommandHandler,
          EventHandler cancelCommandHandler,
          BooleanChoice doNotAskMeAgain)
        {
            MessageBox dialog = new MessageBox(title, message, okCommandHandler, yesCommandHandler, noCommandHandler, cancelCommandHandler, doNotAskMeAgain);
            MessageBox.ShowCodeDialog(dialog);
            return dialog;
        }

        public static MessageBox Show(
          string title,
          string message,
          Command yesCommand,
          Command noCommand,
          BooleanChoice doNotAskMeAgain)
        {
            MessageBox dialog = new MessageBox(title, message, (string)null, false, (Command)null, yesCommand, noCommand, (EventHandler)null, doNotAskMeAgain);
            MessageBox.ShowCodeDialog(dialog);
            return dialog;
        }

        public static MessageBox ShowYesNo(string title, string message, Command yesCommand)
        {
            MessageBox dialog = new MessageBox(title, message, DialogHelper.DialogNo, false, (Command)null, yesCommand, (Command)null, (EventHandler)null, (BooleanChoice)null);
            MessageBox.ShowCodeDialog(dialog);
            return dialog;
        }

        public static MessageBox Show(
          string title,
          string message,
          Command okCommand,
          BooleanChoice doNotAskMeAgain)
        {
            MessageBox dialog = new MessageBox(title, message, (string)null, false, okCommand, (Command)null, (Command)null, (EventHandler)null, doNotAskMeAgain);
            MessageBox.ShowCodeDialog(dialog);
            return dialog;
        }

        public static MessageBox Show(
          string title,
          string message,
          Command okCommand,
          string cancelText,
          bool isOKDefault)
        {
            MessageBox dialog = new MessageBox(title, message, cancelText, isOKDefault, okCommand, (Command)null, (Command)null, (EventHandler)null, (BooleanChoice)null);
            MessageBox.ShowCodeDialog(dialog);
            return dialog;
        }

        public static MessageBox Show(
          string title,
          string message,
          Command okCommand,
          string cancelText,
          EventHandler cancelCommand,
          bool isOKDefault)
        {
            MessageBox dialog = new MessageBox(title, message, cancelText, isOKDefault, okCommand, (Command)null, (Command)null, cancelCommand, (BooleanChoice)null);
            MessageBox.ShowCodeDialog(dialog);
            return dialog;
        }

        public static MessageBox Show(
          string title,
          string message,
          string cancelText,
          bool isOKDefault,
          Command okCommand,
          Command yesCommand,
          Command noCommand,
          EventHandler cancelCommand,
          BooleanChoice doNotAskMeAgain)
        {
            MessageBox dialog = new MessageBox(title, message, cancelText, isOKDefault, okCommand, yesCommand, noCommand, cancelCommand, doNotAskMeAgain);
            MessageBox.ShowCodeDialog(dialog);
            return dialog;
        }

        public MessageBox()
          : this((string)null, (string)null)
        {
        }

        protected MessageBox(string title, string message)
          : base("res://UIXControls!Dialog.uix#MessageBoxContentUI")
        {
            this._title = title;
            this._message = message;
        }

        protected MessageBox(
          string title,
          string message,
          EventHandler okCommandHandler,
          EventHandler yesCommandHandler,
          EventHandler noCommandHandler,
          EventHandler cancelCommandHandler,
          BooleanChoice doNotAskMeAgain)
          : this(title, message)
        {
            this._doNotAskMeAgain = doNotAskMeAgain;
            EventHandler eventHandler = new EventHandler(this.OnInvoked);
            if (okCommandHandler == null && noCommandHandler == null)
            {
                if (yesCommandHandler == null)
                {
                    this.Cancel.Description = DialogHelper.DialogOk;
                    this.Cancel.Invoked += eventHandler;
                }
                else
                    this.Cancel.Description = DialogHelper.DialogNo;
            }
            if (okCommandHandler != null)
            {
                this._okCommand = new Command((IModelItemOwner)this, DialogHelper.DialogOk, okCommandHandler);
                this._okCommand.Invoked += eventHandler;
            }
            if (yesCommandHandler != null)
            {
                this._yesCommand = new Command((IModelItemOwner)this, DialogHelper.DialogYes, yesCommandHandler);
                this._yesCommand.Invoked += eventHandler;
            }
            if (noCommandHandler != null)
            {
                this._noCommand = new Command((IModelItemOwner)this, DialogHelper.DialogNo, noCommandHandler);
                this._noCommand.Invoked += eventHandler;
            }
            if (cancelCommandHandler == null)
                return;
            this.Cancel.Invoked += cancelCommandHandler;
        }

        protected MessageBox(
          string title,
          string message,
          string cancelText,
          bool isOKDefault,
          Command okCommand,
          Command yesCommand,
          Command noCommand,
          EventHandler cancelHandler,
          BooleanChoice doNotAskMeAgain)
          : this(title, message)
        {
            this._doNotAskMeAgain = doNotAskMeAgain;
            EventHandler eventHandler = new EventHandler(this.OnInvoked);
            if (okCommand == null && yesCommand == null && (noCommand == null && cancelHandler == null))
            {
                this.Cancel.Description = DialogHelper.DialogOk;
                this.Cancel.Invoked += eventHandler;
            }
            if (okCommand != null)
            {
                this._okCommand = okCommand;
                this._okCommand.Invoked += eventHandler;
            }
            if (yesCommand != null)
            {
                this._yesCommand = yesCommand;
                this._yesCommand.Invoked += eventHandler;
            }
            if (noCommand != null)
            {
                this._noCommand = noCommand;
                this._noCommand.Invoked += eventHandler;
            }
            if (cancelHandler != null)
                this.Cancel.Invoked += cancelHandler;
            if (!string.IsNullOrEmpty(cancelText))
                this.Cancel.Description = cancelText;
            this._isOKDefault = isOKDefault;
        }

        private void OnInvoked(object sender, EventArgs args) => this.Hide();

        protected static void ShowCodeDialog(MessageBox dialog) => CodeDialogManager.Instance.ShowCodeDialog((DialogHelper)dialog);
    }
}
