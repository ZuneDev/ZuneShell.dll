// Decompiled with JetBrains decompiler
// Type: ZuneUI.RedeemCodeWizard
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;

namespace ZuneUI
{
    public class RedeemCodeWizard : AccountManagementWizard
    {
        private RedeemCodeFinishStep _finishStep;
        private AccountManagementErrorPage _errorStep;

        public RedeemCodeWizard()
        {
            this.State.ContactInfoStep.LightWeightOnly = true;
            this._finishStep = new RedeemCodeFinishStep(this, this.State);
            this._errorStep = new AccountManagementErrorPage(this, Shell.LoadString(StringId.IDS_BILLING_PREPAID_CODE_ERROR_TITLE), Shell.LoadString(StringId.IDS_BILLING_PREPAID_CODE_ERROR_DESC));
            this.AddPage(State.RedeemCodeStep);
            this.AddPage(State.ContactInfoStep);
            this.AddPage(_finishStep);
            this.AddPage(_errorStep);
        }

        protected override void OnAsyncCommitCompleted(bool success)
        {
            base.OnAsyncCommitCompleted(success);
            if (!success)
                return;
            switch (this.State.RedeemCodeStep.ConfirmedTokenDetails != null ? this.State.RedeemCodeStep.ConfirmedTokenDetails.TokenType : ETokenType.Unknown)
            {
                case ETokenType.Points:
                    this._finishStep.ClosingMessage = string.Format(Shell.LoadString(StringId.IDS_BILLING_PREPAID_CODE_POINTS_SUCCESS), State.RedeemCodeStep.ConfirmedTokenDetails.OfferName);
                    break;
                case ETokenType.Subscription:
                    this._finishStep.ClosingMessage = string.Format(Shell.LoadString(StringId.IDS_BILLING_PREPAID_CODE_PASS_SUCCESS), State.RedeemCodeStep.ConfirmedTokenDetails.OfferName);
                    break;
                default:
                    this._finishStep.ClosingMessage = Shell.LoadString(StringId.IDS_BILLING_PREPAID_CODE_DEFAULT_SUCCESS);
                    break;
            }
        }
    }
}
