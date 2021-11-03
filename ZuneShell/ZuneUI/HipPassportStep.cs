// Decompiled with JetBrains decompiler
// Type: ZuneUI.HipPassportStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.ErrorMapperApi;
using Microsoft.Zune.Service;
using System.Collections.Generic;

namespace ZuneUI
{
    public class HipPassportStep : AccountManagementStep
    {
        private bool _firstView;
        private WinLiveInformation _winLiveHip;
        private Dictionary<int, PropertyDescriptor> _errorMappings;

        public HipPassportStep(Wizard owner, AccountManagementWizardState state, bool parentAccount)
          : base(owner, state, parentAccount)
        {
            this._firstView = true;
            if (parentAccount)
                this.Description = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_PARENTAL_INPUT_HEAD);
            else
                this.Description = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_PASSPORT_STEP);
            this.Initialize(new HipPropertyEditor());
        }

        public WinLiveInformation WinLiveHip
        {
            get => this._winLiveHip;
            private set
            {
                if (this._winLiveHip == value)
                    return;
                this._winLiveHip = value;
                this.FirePropertyChanged(nameof(WinLiveHip));
                this.FirePropertyChanged("IsEnabled");
            }
        }

        public override string UI => "res://ZuneShellResources!CreatePassport.uix#HipPassportStep";

        public override bool IsEnabled
        {
            get
            {
                bool flag = base.IsEnabled && !this.CreatePassportStep.CreatedPassport;
                if (flag)
                    flag = this.CreatePassportStep.IsEnabled && (this._owner.CurrentPage != this || this.WinLiveHip != null);
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
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_HIP_SOLUTION_INVALID.Int, null);
                }
                return this._errorMappings;
            }
        }

        private CreatePassportStep CreatePassportStep => !this.ParentAccount ? this.State.CreatePassportStep : this.State.CreatePassportParentStep;

        public void Refresh()
        {
            if (!this.IsEnabled)
                return;
            this.OnActivate();
        }

        protected override void OnActivate()
        {
            this.SetCommittedValue(HipPropertyEditor.HipCharacters, "");
            if (this._firstView)
            {
                this._firstView = false;
                this.WinLiveHip = this.CreatePassportStep.WinLiveInformation;
            }
            else
                this.WinLiveHip = null;
            this.ServiceActivationRequestsDone = this.WinLiveHip != null;
            if (this.ServiceActivationRequestsDone)
                return;
            this.StartActivationRequests(State.BasicAccountInfoStep.SelectedLocale);
        }

        internal override ErrorMapperResult GetMappedErrorDescriptionAndUrl(HRESULT hr) => ErrorMapperApi.GetMappedErrorDescriptionAndUrl(hr.Int, eErrorCondition.eEC_WinLive);

        protected override void OnStartActivationRequests(object state) => this.EndActivationRequests(this.ObtainWinLiveHip(state as string));

        protected override void OnEndActivationRequests(object args)
        {
            if (args == null)
                this.NavigateToErrorHandler();
            else
                this.WinLiveHip = args as WinLiveInformation;
        }

        private WinLiveInformation ObtainWinLiveHip(string local)
        {
            WinLiveInformation information1 = null;
            ServiceError serviceError = null;
            HRESULT information2 = this.State.WinLiveSignup.GetInformation(local, EHipType.Image, out information1, out serviceError);
            if (information2.IsError)
                this.SetError(information2, serviceError);
            return information1;
        }
    }
}
