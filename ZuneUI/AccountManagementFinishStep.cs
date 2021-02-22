// Decompiled with JetBrains decompiler
// Type: ZuneUI.AccountManagementFinishStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class AccountManagementFinishStep : AccountManagementStep
    {
        private string _closingMessage;
        private bool _hideOnComplete;
        private string _oldDescription;

        public AccountManagementFinishStep(Wizard owner, AccountManagementWizardState state)
          : this(owner, state, null)
        {
        }

        public AccountManagementFinishStep(
          Wizard owner,
          AccountManagementWizardState state,
          string description)
          : this(owner, state, description, null)
        {
            this.Description = description;
        }

        public AccountManagementFinishStep(
          Wizard owner,
          AccountManagementWizardState state,
          string description,
          string detailDescription)
          : base(owner, state, false)
        {
            this.Description = description;
            this.DetailDescription = detailDescription;
        }

        public override string UI => "res://ZuneShellResources!AccountCreation.uix#AccountManagementFinishStep";

        public string ClosingMessage
        {
            get => this._closingMessage;
            set
            {
                if (!(this._closingMessage != value))
                    return;
                this._closingMessage = value;
                this.FirePropertyChanged(nameof(ClosingMessage));
            }
        }

        public bool HideOnComplete
        {
            get => this._hideOnComplete;
            set
            {
                if (this._hideOnComplete == value)
                    return;
                this._hideOnComplete = value;
                this.FirePropertyChanged(nameof(HideOnComplete));
                this.UpdateDescription();
            }
        }

        internal bool CommittSucceeded
        {
            get
            {
                bool flag = false;
                if (this._owner is AccountManagementWizard)
                    flag = ((AccountManagementWizard)this._owner).CommitSucceeded;
                return flag;
            }
        }

        private void UpdateDescription()
        {
            if (this.HideOnComplete)
            {
                this._oldDescription = this.Description;
                this.Description = Shell.LoadString(StringId.IDS_PLEASE_WAIT_TITLE);
            }
            else
                this.Description = this._oldDescription;
        }
    }
}
