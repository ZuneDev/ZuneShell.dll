// Decompiled with JetBrains decompiler
// Type: ZuneUI.ParentPaymentIntrumentStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;

namespace ZuneUI
{
    public class ParentPaymentIntrumentStep : PaymentInstrumentStepBase
    {
        public ParentPaymentIntrumentStep(Wizard owner, AccountManagementWizardState state)
          : base(owner, state, true)
        {
        }

        public override bool IsEnabled
        {
            get
            {
                bool flag = base.IsEnabled;
                if (flag)
                    flag = this.State.BasicAccountInfoStep.NewAccounType == AccountUserType.ChildWithoutSocial;
                return flag;
            }
        }

        protected override void OnActivate()
        {
            this.SelectedCountry = this.State.BasicAccountInfoStep.SelectedCountry;
            this.SetCommittedValue(PaymentInstrumentPropertyEditor.Country, SelectedCountry);
            this.SetCommittedValue(PaymentInstrumentPropertyEditor.Language, State.BasicAccountInfoStep.SelectedLanguage);
            this.SetCommittedValue(PaymentInstrumentPropertyEditor.Email, this.State.ContactInfoParentStep.GetCommittedValue(BaseContactInfoPropertyEditor.Email));
            base.OnActivate();
            this.ServiceDeactivationRequestsDone = false;
            MetadataEditProperty property1 = this.WizardPropertyEditor.GetProperty(PaymentInstrumentPropertyEditor.AccountHolderName);
            MetadataEditProperty property2 = this.WizardPropertyEditor.GetProperty(PaymentInstrumentPropertyEditor.PhoneNumber);
            MetadataEditProperty property3 = this.WizardPropertyEditor.GetProperty(PaymentInstrumentPropertyEditor.PhoneExtension);
            MetadataEditProperty property4 = this.WizardPropertyEditor.GetProperty(PaymentInstrumentPropertyEditor.PostalCode);
            if (string.IsNullOrEmpty(property1.Value))
            {
                string committedValue1 = this.State.ContactInfoParentStep.GetCommittedValue(BaseContactInfoPropertyEditor.FirstName) as string;
                string committedValue2 = this.State.ContactInfoParentStep.GetCommittedValue(BaseContactInfoPropertyEditor.LastName) as string;
                string format = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_NAME_FORMAT);
                property1.Value = string.Format(format, committedValue1, committedValue2);
            }
            property4.State = SelectedCountry;
            if (string.IsNullOrEmpty(property4.Value))
                property4.Value = this.State.BasicAccountInfoStep.GetCommittedValue(BasicAccountInfoPropertyEditor.PostalCode) as string;
            if (string.IsNullOrEmpty(property2.Value))
                property2.Value = this.State.ContactInfoParentStep.GetCommittedValue(BaseContactInfoPropertyEditor.PhoneNumber) as string;
            if (!string.IsNullOrEmpty(property3.Value))
                return;
            property3.Value = this.State.ContactInfoParentStep.GetCommittedValue(BaseContactInfoPropertyEditor.PhoneExtension) as string;
        }

        internal override bool OnMovingNext()
        {
            if (this.ServiceDeactivationRequestsDone)
                return base.OnMovingNext();
            ServiceData serviceData;
            serviceData.PassportIdentity = this.State.PassportPasswordParentStep.PassportIdentity;
            serviceData.CreditCard = this.CreateCreditCard();
            serviceData.CreditCard.ContactFirstName = this.State.ContactInfoParentStep.GetCommittedValue(BaseContactInfoPropertyEditor.FirstName) as string;
            serviceData.CreditCard.ContactLastName = this.State.ContactInfoParentStep.GetCommittedValue(BaseContactInfoPropertyEditor.LastName) as string;
            this.StartDeactivationRequests(serviceData);
            return false;
        }

        protected override void OnStartDeactivationRequests(object state)
        {
            ServiceData serviceData = (ServiceData)state;
            CreditCard creditCard = null;
            if (this.IsValidCreditCard(serviceData))
                creditCard = serviceData.CreditCard;
            this.EndDeactivationRequests(creditCard);
        }

        protected override void OnEndDeactivationRequests(object args) => this.CommittedCreditCard = (CreditCard)args;

        private bool IsValidCreditCard(ServiceData serviceData)
        {
            bool flag = true;
            if (serviceData.PassportIdentity != null)
            {
                ServiceError serviceError;
                HRESULT hr = this.State.AccountManagement.ValidateCreditCard(serviceData.PassportIdentity, serviceData.CreditCard, out serviceError);
                flag = hr.IsSuccess;
                if (!flag)
                    this.SetError(hr, serviceError);
            }
            return flag;
        }

        private struct ServiceData
        {
            public PassportIdentity PassportIdentity;
            public CreditCard CreditCard;
        }
    }
}
