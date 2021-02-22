// Decompiled with JetBrains decompiler
// Type: ZuneUI.EmailSelectionStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.ErrorMapperApi;
using Microsoft.Zune.Service;
using System;
using System.Collections.Generic;

namespace ZuneUI
{
    public class EmailSelectionStep : AccountManagementStep
    {
        private bool _hasEmail;
        private bool _isEmailPassportId;
        private bool _pageLocked;
        private bool _skipOnce;
        private Dictionary<int, PropertyDescriptor> _errorMappings;

        public EmailSelectionStep(Wizard owner, AccountManagementWizardState state, bool parentAccount)
          : base(owner, state, parentAccount)
        {
            if (parentAccount)
                this.Description = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_PARENTAL_INPUT_HEAD);
            else
                this.Description = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_EMAIL_STEP);
            this.Initialize((WizardPropertyEditor)new EmailSelectionPropertyEditor());
        }

        public override string UI => "res://ZuneShellResources!CreatePassport.uix#EmailSelectionStep";

        public bool HasEmail
        {
            get => this._hasEmail;
            set
            {
                if (this._hasEmail == value)
                    return;
                this._hasEmail = value;
                this.FirePropertyChanged(nameof(HasEmail));
                this.FirePropertyChanged("NeedsPassportId");
            }
        }

        public string Email
        {
            get => this.GetUncommittedValue(EmailSelectionPropertyEditor.Email) as string;
            internal set
            {
                this.SetCommittedValue(EmailSelectionPropertyEditor.Email, (object)value);
                this.FirePropertyChanged(nameof(Email));
            }
        }

        private bool CheckedEmail => this.ServiceDeactivationRequestsDone;

        internal bool PageLocked
        {
            get => this._pageLocked;
            set
            {
                if (this._pageLocked == value)
                    return;
                this._pageLocked = value;
                this.FirePropertyChanged(nameof(PageLocked));
            }
        }

        internal bool SkipOnce
        {
            get => this._skipOnce;
            set
            {
                if (this._skipOnce == value)
                    return;
                this._skipOnce = value;
                this.FirePropertyChanged(nameof(SkipOnce));
            }
        }

        public bool IsEmailPassportId
        {
            get => this._isEmailPassportId;
            private set
            {
                if (this._isEmailPassportId == value)
                    return;
                this._isEmailPassportId = value;
                this.FirePropertyChanged(nameof(IsEmailPassportId));
                this.FirePropertyChanged("NeedsPassportId");
            }
        }

        public bool NeedsPassportId => !this.HasEmail || !this.IsEmailPassportId;

        public override bool IsValid
        {
            get
            {
                MetadataEditProperty property = this.WizardPropertyEditor.GetProperty(EmailSelectionPropertyEditor.Email);
                HRESULT hresult = HRESULT._S_OK;
                if (this.HasEmail)
                {
                    string email = this.Email;
                    if (this.HasEmail && this.ParentAccount && (email.Equals(this.State.EmailSelectionStep.Email, StringComparison.InvariantCultureIgnoreCase) || email.Equals(this.State.CreatePassportStep.Email, StringComparison.InvariantCultureIgnoreCase)))
                        hresult = HRESULT._ZUNE_E_SIGNUP_INVALID_PARENT_EMAIL;
                }
                if (hresult.IsError || property.ExternalError == HRESULT._ZUNE_E_SIGNUP_INVALID_PARENT_EMAIL)
                    property.ExternalError = hresult;
                return !this.HasEmail || base.IsValid;
            }
        }

        public override bool IsEnabled
        {
            get
            {
                bool flag = base.IsEnabled && !this.CreatePassportStep.CreatedPassport;
                if (flag)
                    flag = !this.ParentAccount || this.State.BasicAccountInfoStep.IsParentAccountNeeded;
                if (flag)
                    flag = !this.CheckedEmail || !this.PageLocked;
                return flag;
            }
        }

        internal override Dictionary<int, PropertyDescriptor> ErrorPropertyMappings
        {
            get
            {
                if (this._errorMappings == null)
                {
                    this._errorMappings = new Dictionary<int, PropertyDescriptor>(1);
                    this._errorMappings.Add(HRESULT._ZUNE_E_WINLIVE_UNAUTHORIZED_DOMAIN.Int, EmailSelectionPropertyEditor.Email);
                }
                return this._errorMappings;
            }
        }

        internal override ErrorMapperResult GetMappedErrorDescriptionAndUrl(HRESULT hr) => Microsoft.Zune.ErrorMapperApi.ErrorMapperApi.GetMappedErrorDescriptionAndUrl(hr.Int, eErrorCondition.eEC_WinLive);

        private CreatePassportStep CreatePassportStep => !this.ParentAccount ? this.State.CreatePassportStep : this.State.CreatePassportParentStep;

        protected override void OnActivate()
        {
            if (this.SkipOnce || this.PageLocked)
            {
                this.SkipOnce = false;
                this._owner.MoveNext();
            }
            else
            {
                this.ServiceDeactivationRequestsDone = false;
                if (string.IsNullOrEmpty(this.Email) && !this.ParentAccount)
                {
                    UIDevice appDevice = ApplicationMarketplaceHelper.FindAppDevice();
                    if (appDevice != UIDeviceList.NullDevice)
                    {
                        this.Email = appDevice.LiveId;
                        this.HasEmail = !string.IsNullOrEmpty(this.Email);
                    }
                }
                base.OnActivate();
            }
        }

        internal override bool OnMovingNext()
        {
            if (!this.HasEmail)
                this.ServiceDeactivationRequestsDone = true;
            if (this.ServiceDeactivationRequestsDone)
                return base.OnMovingNext();
            this.StartDeactivationRequests((object)this.Email);
            return false;
        }

        protected override void OnStartDeactivationRequests(object state) => this.EndDeactivationRequests((object)this.ValidatePassportId(state as string));

        protected override void OnEndDeactivationRequests(object args) => this.IsEmailPassportId = (bool)args;

        private bool ValidatePassportId(string email)
        {
            bool flag = false;
            ServiceError serviceError = (ServiceError)null;
            WinLiveAvailableInformation information;
            HRESULT hr = this.State.WinLiveSignup.CheckAvailableSigninName(email, false, (string)null, (string)null, out information, out serviceError);
            if (hr.IsError)
                this.SetError(hr, serviceError);
            else
                flag = !information.Available;
            return flag;
        }
    }
}
