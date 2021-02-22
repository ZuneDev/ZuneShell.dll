// Decompiled with JetBrains decompiler
// Type: ZuneUI.AccountCreationFinishStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;

namespace ZuneUI
{
    public class AccountCreationFinishStep : AccountManagementFinishStep
    {
        private AccountCreationNextSteps _nextSteps = AccountCreationNextSteps.EditTile;

        public AccountCreationFinishStep(Wizard owner, AccountManagementWizardState state)
          : base(owner, state, Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_FINISH))
        {
        }

        public override string UI => "res://ZuneShellResources!AccountCreation.uix#AccountCreationFinishStep";

        internal AccountCreationNextSteps NextSteps
        {
            get => this._nextSteps;
            set
            {
                if (this._nextSteps == value)
                    return;
                this._nextSteps = value;
                this.FirePropertyChanged(nameof(NextSteps));
                this.FireNextStepChanges();
            }
        }

        public bool ShowPurchaseZunePass => this.CommittSucceeded && !this.ShowPurchaseZunePassTrial && FeatureEnablement.IsFeatureEnabled(Features.eSubscription) && (this.NextSteps & AccountCreationNextSteps.PurchaseZunePass) == AccountCreationNextSteps.PurchaseZunePass;

        public bool ShowPurchaseZunePassTrial => this.CommittSucceeded && FeatureEnablement.IsFeatureEnabled(Features.eSubscriptionTrial) && (this.NextSteps & AccountCreationNextSteps.PurchaseZunePassTrial) == AccountCreationNextSteps.PurchaseZunePassTrial;

        public bool ShowPurchaseEditTile => this.CommittSucceeded && FeatureEnablement.IsFeatureEnabled(Features.eSocial) && (this.NextSteps & AccountCreationNextSteps.EditTile) == AccountCreationNextSteps.EditTile;

        private void FireNextStepChanges()
        {
            this.FirePropertyChanged("ShowPurchaseZunePassTrial");
            this.FirePropertyChanged("ShowPurchaseZunePass");
            this.FirePropertyChanged("ShowPurchaseEditTile");
        }

        protected override void OnActivate()
        {
            if (this.State.PassportPasswordStep.CanCreateAccount || this.State.PassportPasswordStep.IsUpgradeNeeded)
                this.LoadStatus = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_STATUS_CREATION);
            else
                this.LoadStatus = (string)null;
            base.OnActivate();
        }

        protected override bool OnCommitChanges()
        {
            bool flag = !this.State.PassportPasswordStep.CanCreateAccount ? (!this.State.PassportPasswordStep.IsUpgradeNeeded ? this.State.PassportPasswordStep.IsZuneAccount : this.State.UpgradeZuneAccount(true)) : this.State.CreateZuneAccount();
            if (flag)
                Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredFireNextStepChanges), (object)null);
            return flag;
        }

        private void DeferredFireNextStepChanges(object unused) => this.FireNextStepChanges();
    }
}
