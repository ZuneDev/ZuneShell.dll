// Decompiled with JetBrains decompiler
// Type: ZuneUI.ZuneTagStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public class ZuneTagStep : AccountManagementStep
    {
        private IList _tagSuggestions;
        private Dictionary<int, PropertyDescriptor> _errorMappings;

        public ZuneTagStep(Wizard owner, AccountManagementWizardState state, bool parentAccount)
          : base(owner, state, parentAccount)
        {
            this.Description = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_ZUNE_TAG_HEADER);
            this.Initialize((WizardPropertyEditor)new ZuneTagPropertyEditor());
        }

        public override string UI => "res://ZuneShellResources!AccountInfo.uix#ZuneTagStep";

        public IList TagSuggestions
        {
            get => this._tagSuggestions;
            private set
            {
                if (this._tagSuggestions == value)
                    return;
                this._tagSuggestions = value;
                this.FirePropertyChanged(nameof(TagSuggestions));
            }
        }

        public string EmailAddress => this.State != null ? this.State.GetEmailAddress() : string.Empty;

        public override bool IsEnabled
        {
            get
            {
                bool flag = base.IsEnabled;
                if (flag)
                    flag = this.State.PassportPasswordStep.CanCreateAccount;
                return flag;
            }
        }

        internal override Dictionary<int, PropertyDescriptor> ErrorPropertyMappings
        {
            get
            {
                if (this._errorMappings == null)
                {
                    this._errorMappings = new Dictionary<int, PropertyDescriptor>(3);
                    this._errorMappings.Add(HRESULT._ZEST_E_ACCOUNT_ZUNETAG_OCCUPIED.Int, (PropertyDescriptor)null);
                    this._errorMappings.Add(HRESULT._ZEST_E_INVALID_ARG_ZUNETAG.Int, (PropertyDescriptor)null);
                    this._errorMappings.Add(HRESULT._ZEST_E_ZUNETAG_OFFENSIVE.Int, (PropertyDescriptor)null);
                }
                return this._errorMappings;
            }
        }

        protected override void OnActivate()
        {
            this.ServiceDeactivationRequestsDone = false;
            base.OnActivate();
        }

        internal override bool OnMovingNext()
        {
            string uncommittedValue = this.GetUncommittedValue(ZuneTagPropertyEditor.ZuneTag) as string;
            string committedValue = this.GetCommittedValue(ZuneTagPropertyEditor.ZuneTag) as string;
            if (uncommittedValue == committedValue || this.TagSuggestions != null && this.TagSuggestions.Contains((object)uncommittedValue))
            {
                this.TagSuggestions = (IList)null;
                this.ServiceDeactivationRequestsDone = true;
            }
            if (this.ServiceDeactivationRequestsDone)
            {
                if (this.TagSuggestions == null || this.TagSuggestions.Count == 0)
                    return base.OnMovingNext();
                this.ServiceDeactivationRequestsDone = false;
                return false;
            }
            ZuneTagStep.ServiceData serviceData;
            serviceData.ZuneTag = uncommittedValue;
            serviceData.CountryCode = this.State.BasicAccountInfoStep.SelectedCountry;
            this.StartDeactivationRequests((object)serviceData);
            return false;
        }

        protected override void OnStartDeactivationRequests(object state) => this.EndDeactivationRequests((object)this.ValidateUniqueZuneTag((ZuneTagStep.ServiceData)state));

        protected override void OnEndDeactivationRequests(object args) => this.TagSuggestions = args as IList;

        private IList ValidateUniqueZuneTag(ZuneTagStep.ServiceData serviceData)
        {
            ServiceError serviceError = (ServiceError)null;
            IList suggestedNames = (IList)null;
            HRESULT hr = this.State.AccountManagement.ReserveZuneTag(serviceData.ZuneTag, serviceData.CountryCode, out suggestedNames, out serviceError);
            if (hr.IsError && hr != HRESULT._ZEST_E_ACCOUNT_ZUNETAG_OCCUPIED)
                this.SetError(hr, serviceError);
            return suggestedNames;
        }

        private struct ServiceData
        {
            public string ZuneTag;
            public string CountryCode;
        }
    }
}
