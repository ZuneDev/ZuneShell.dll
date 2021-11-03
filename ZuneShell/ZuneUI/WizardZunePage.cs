// Decompiled with JetBrains decompiler
// Type: ZuneUI.WizardZunePage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneUI
{
    public class WizardZunePage : ZunePage, IWizardNavigation
    {
        private Wizard _wizard;
        private Command _backCommand;
        private Command _nextCommand;
        private Command _finishCommand;
        private Command _cancelCommand;

        internal WizardZunePage(Node node, Wizard wizard)
        {
            this._wizard = wizard;
            this.UI = "res://ZuneShellResources!WizardControls.uix#WizardZunePage";
            this.BackgroundUI = "res://ZuneShellResources!WizardControls.uix#WizardBackground";
            this.TransportControlStyle = TransportControlStyle.None;
            this.PivotPreference = node;
            this.ShowCDIcon = false;
            this.ShowDeviceIcon = false;
            this.ShowPlaylistIcon = false;
            this.ShowSettings = false;
            this.ShowSearch = false;
            this.ShowNowPlayingBackgroundOnIdle = false;
            this.CanEnterCompactMode = false;
            this.NotificationAreaVisible = false;
            this.TransportControlsVisible = false;
            this.Cancel.Invoked += new EventHandler(this.OnCancelInvoked);
            this.Finish.Invoked += new EventHandler(this.OnFinishInvoked);
        }

        public Wizard Wizard => this._wizard;

        public Command Back
        {
            get
            {
                if (this._backCommand == null)
                {
                    this._backCommand = new Command(this);
                    this._backCommand.Description = Shell.LoadString(StringId.IDS_BACK_BUTTON);
                }
                return this._backCommand;
            }
        }

        public Command Next
        {
            get
            {
                if (this._nextCommand == null)
                {
                    this._nextCommand = new Command(this);
                    this._nextCommand.Description = Shell.LoadString(StringId.IDS_NEXT_BUTTON);
                }
                return this._nextCommand;
            }
        }

        public Command Finish
        {
            get
            {
                if (this._finishCommand == null)
                {
                    this._finishCommand = new Command(this);
                    this._finishCommand.Description = Shell.LoadString(StringId.IDS_FINISH_BUTTON);
                }
                return this._finishCommand;
            }
        }

        public Command Cancel
        {
            get
            {
                if (this._cancelCommand == null)
                {
                    this._cancelCommand = new Command(this);
                    this._cancelCommand.Description = Shell.LoadString(StringId.IDS_CANCEL_BUTTON);
                }
                return this._cancelCommand;
            }
        }

        public override bool HandleBack() => false;

        public override IPageState SaveAndRelease() => (IPageState)null;

        private void LeaveWizard() => ZuneShell.DefaultInstance.NavigateBack();

        private void OnCancelInvoked(object sender, EventArgs e) => this.LeaveWizard();

        private void OnFinishInvoked(object sender, EventArgs e) => this.LeaveWizard();
    }
}
