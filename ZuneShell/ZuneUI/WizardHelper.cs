// Decompiled with JetBrains decompiler
// Type: ZuneUI.WizardHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using UIXControls;

namespace ZuneUI
{
    public class WizardHelper : DialogHelper, IWizardNavigation
    {
        private Wizard _wizard;
        private Command _backCommand;
        private Command _nextCommand;
        private Command _finishCommand;

        public WizardHelper()
          : this(null)
        {
        }

        public WizardHelper(EventHandler cancelCommandHandler)
        {
            this._backCommand = new Command(this);
            this._backCommand.Description = Shell.LoadString(StringId.IDS_BACK_BUTTON);
            this._nextCommand = new Command(this);
            this._nextCommand.Description = Shell.LoadString(StringId.IDS_NEXT_BUTTON);
            this._finishCommand = new Command(this);
            this._finishCommand.Description = Shell.LoadString(StringId.IDS_FINISH_BUTTON);
            if (cancelCommandHandler == null)
                return;
            this.Cancel.Invoked += cancelCommandHandler;
        }

        public Wizard Wizard
        {
            get => this._wizard;
            set
            {
                if (this._wizard == value)
                    return;
                if (this._wizard != null && this._wizard.CancelCommandHandler != null)
                    this.Cancel.Invoked -= this._wizard.CancelCommandHandler;
                this._wizard = value;
                this.FirePropertyChanged(nameof(Wizard));
                if (this._wizard.CancelCommandHandler == null)
                    return;
                this.Cancel.Invoked += this._wizard.CancelCommandHandler;
            }
        }

        public static void Show(EventHandler cancelCommandHandler) => new WizardHelper(cancelCommandHandler).Show();

        public Command Back => this._backCommand;

        public Command Next => this._nextCommand;

        public Command Finish => this._finishCommand;

        Command IWizardNavigation.Cancel => this.Cancel;
    }
}
