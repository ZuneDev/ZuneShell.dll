// Decompiled with JetBrains decompiler
// Type: ZuneUI.ContactInfoParentStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public class ContactInfoParentStep : RegionInfoStep
    {
        private bool _isAdult;

        public ContactInfoParentStep(Wizard owner, AccountManagementWizardState state)
          : base(owner, state, true)
        {
            this.NextTextOverride = Shell.LoadString(StringId.IDS_I_ACCEPT_BUTTON);
            this.Description = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_PARENT_CONTACT_HEAD);
            this.Initialize(new ContactInfoParentPropertyEditor());
        }

        public override string UI => "res://ZuneShellResources!AccountInfo.uix#ParentContactInfoStep";

        public override bool IsEnabled
        {
            get
            {
                bool flag = base.IsEnabled;
                if (flag)
                    flag = this.State.BasicAccountInfoStep.IsParentAccountNeeded;
                return flag;
            }
        }

        public bool IsAdult
        {
            get => this._isAdult;
            protected set
            {
                if (this._isAdult == value)
                    return;
                this._isAdult = value;
                this.FirePropertyChanged(nameof(IsAdult));
            }
        }

        protected override PropertyDescriptor CountryDescriptor => ContactInfoParentPropertyEditor.Country;

        protected override void OnCountryChanged()
        {
            if (this.WizardPropertyEditor == null)
                return;
            this.WizardPropertyEditor.SetPropertyState(BaseContactInfoPropertyEditor.FirstName, SelectedCountry);
            this.WizardPropertyEditor.SetPropertyState(BaseContactInfoPropertyEditor.LastName, SelectedCountry);
            this.WizardPropertyEditor.SetPropertyState(BaseContactInfoPropertyEditor.PhoneNumber, SelectedCountry);
            this.WizardPropertyEditor.SetPropertyState(BaseContactInfoPropertyEditor.PhoneExtension, SelectedCountry);
        }

        protected override void OnActivate()
        {
            this.SelectedCountry = this.State.BasicAccountInfoStep.SelectedCountry;
            this.ServiceDeactivationRequestsDone = false;
            this.SetPropertyState(ContactInfoParentPropertyEditor.Birthday, State.BasicAccountInfoStep.SelectedLocale);
            this.SetUncommittedValue(ContactInfoParentPropertyEditor.Birthday, this.GetCommittedValue(ContactInfoParentPropertyEditor.Birthday));
            MetadataEditProperty property = this.WizardPropertyEditor.GetProperty(BaseContactInfoPropertyEditor.Email);
            if (string.IsNullOrEmpty(property.Value))
            {
                if (this.State.EmailSelectionParentStep.IsEmailPassportId)
                {
                    property.Value = this.State.EmailSelectionParentStep.GetCommittedValue(EmailSelectionPropertyEditor.Email) as string;
                }
                else
                {
                    string committedValue1 = this.State.CreatePassportParentStep.GetCommittedValue(CreatePassportPropertyEditor.PassportId) as string;
                    string committedValue2 = this.State.CreatePassportParentStep.GetCommittedValue(CreatePassportPropertyEditor.PassportDomain) as string;
                    if (committedValue1 != null && committedValue2 != null)
                        property.Value = committedValue1 + "@" + committedValue2;
                }
            }
            base.OnActivate();
        }

        internal override bool OnMovingNext()
        {
            if (this.ServiceDeactivationRequestsDone)
            {
                this.WizardPropertyEditor.GetProperty(ContactInfoParentPropertyEditor.Birthday).ExternalError = !this.IsAdult ? HRESULT._ZUNE_E_SIGNUP_INVALID_PARENT_AGE : HRESULT._S_OK;
                if (this.IsAdult)
                    return base.OnMovingNext();
                this.ShowValidation();
                this.ServiceDeactivationRequestsDone = false;
                return false;
            }
            this.StartDeactivationRequests(((DateTime?)GetUncommittedValue(ContactInfoParentPropertyEditor.Birthday)).Value);
            return false;
        }

        protected override void OnStartDeactivationRequests(object state) => this.EndDeactivationRequests(this.ObtainIsAdult((DateTime)state));

        protected override void OnEndDeactivationRequests(object args) => this.IsAdult = (bool)args;

        private bool ObtainIsAdult(DateTime birthday)
        {
            AccountCountry country = AccountCountryList.Instance.GetCountry(this.State.BasicAccountInfoStep.SelectedCountry);
            bool flag = false;
            if (country != null)
                flag = this.ObtainAge(birthday) >= country.AdultAge;
            return flag;
        }

        private int ObtainAge(DateTime birthday)
        {
            DateTime today = DateTime.Today;
            int num = today.Year - birthday.Year;
            if (today.Month < birthday.Month || today.Month == birthday.Month && today.Day < birthday.Day)
                --num;
            return num;
        }
    }
}
