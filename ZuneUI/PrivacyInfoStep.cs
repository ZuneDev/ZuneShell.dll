// Decompiled with JetBrains decompiler
// Type: ZuneUI.PrivacyInfoStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Service;
using Microsoft.Zune.Util;
using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public class PrivacyInfoStep : AccountManagementStep
    {
        private BooleanChoice _allowMicrosoftCommunications;
        private BooleanChoice _allowPartnerCommunications;
        private BooleanChoice _usageCollection;
        private PrivacyInfoSettings _showSettings;
        private AccountSettings _committedSettings;
        private FamilySettings _familySettings;
        private IList _privacySettings;
        private IList _familySettingsChoices;

        public PrivacyInfoStep(
          Wizard owner,
          AccountManagementWizardState state,
          bool parentAccount,
          PrivacyInfoSettings showSettings)
          : base(owner, state, parentAccount)
        {
            this._showSettings = showSettings;
            if (this.ShowingNewsletterSettings && !parentAccount)
                this.Description = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_ACCOUNT_INFO_STEP);
            else if (parentAccount || SignIn.Instance.SignedIn && SignIn.Instance.IsParentallyControlled)
                this.Description = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_ZUNE_FAMILY_HEADER);
            else
                this.Description = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_ACCOUNT_INFO_STEP);
            this.Initialize((WizardPropertyEditor)null);
            this.CanNavigateInto = false;
        }

        public override string UI => "res://ZuneShellResources!AccountInfo.uix#PrivacyInfoStep";

        public override bool IsEnabled => this.ShowingSettings;

        public AccountSettings CommittedSettings
        {
            get => this._committedSettings;
            set
            {
                if (this._committedSettings == value)
                    return;
                this._committedSettings = value;
                this.SetUncommittedSettings();
                this.FirePropertyChanged(nameof(CommittedSettings));
            }
        }

        public PrivacyInfoSettings ShowSettings
        {
            get => this._showSettings;
            set
            {
                if (this._showSettings == value)
                    return;
                this._showSettings = value;
                this.ResetSettings();
                this.FirePropertyChanged(nameof(ShowSettings));
            }
        }

        public FamilySettings FamilySettings
        {
            get => this._familySettings;
            private set
            {
                if (this._familySettings == value)
                    return;
                this._familySettings = value;
                this.FirePropertyChanged(nameof(FamilySettings));
            }
        }

        public IList PrivacySettings
        {
            get => this._privacySettings;
            private set
            {
                if (this._privacySettings == value)
                    return;
                this._privacySettings = value;
                this.FirePropertyChanged(nameof(PrivacySettings));
            }
        }

        public IList FamilySettingsChoices
        {
            get => this._familySettingsChoices;
            private set
            {
                if (this._familySettingsChoices == value)
                    return;
                this._familySettingsChoices = value;
                this.FirePropertyChanged(nameof(FamilySettingsChoices));
            }
        }

        public bool ShowingCommunicationSetting => (this.ShowSettings & PrivacyInfoSettings.Communications) != PrivacyInfoSettings.None || (this.ShowSettings & PrivacyInfoSettings.AllSocial) != PrivacyInfoSettings.None;

        public bool ShowingCreateNewSettings => this.ShowSettings == PrivacyInfoSettings.CreateNewAccount || this.ShowSettings == PrivacyInfoSettings.CreateNewAccountWithSocial;

        public bool ShowingFriendsListSharingSetting => (this.ShowSettings & PrivacyInfoSettings.FriendsSharing) != PrivacyInfoSettings.None || (this.ShowSettings & PrivacyInfoSettings.AllSocial) != PrivacyInfoSettings.None;

        public bool ShowingMusingSharingSetting => (this.ShowSettings & PrivacyInfoSettings.MusicSharing) != PrivacyInfoSettings.None || (this.ShowSettings & PrivacyInfoSettings.AllSocial) != PrivacyInfoSettings.None;

        public bool ShowingNewsletterSettings => this.ShowSettings == PrivacyInfoSettings.AllowMicrosoftCommunications;

        public bool ShowingProfileCustomizationSetting => (this.ShowSettings & PrivacyInfoSettings.ProfileCustomization) != PrivacyInfoSettings.None || (this.ShowSettings & PrivacyInfoSettings.AllSocial) != PrivacyInfoSettings.None;

        public BooleanChoice AllowMicrosoftCommunications
        {
            get => this._allowMicrosoftCommunications;
            set
            {
                if (this._allowMicrosoftCommunications == value)
                    return;
                this._allowMicrosoftCommunications = value;
                this.FirePropertyChanged(nameof(AllowMicrosoftCommunications));
            }
        }

        public BooleanChoice AllowPartnerCommunications
        {
            get => this._allowPartnerCommunications;
            set
            {
                if (this._allowPartnerCommunications == value)
                    return;
                this._allowPartnerCommunications = value;
                this.FirePropertyChanged(nameof(AllowPartnerCommunications));
            }
        }

        public BooleanChoice UsageCollection
        {
            get => this._usageCollection;
            set
            {
                if (this._usageCollection == value)
                    return;
                this._usageCollection = value;
                this.FirePropertyChanged(nameof(UsageCollection));
            }
        }

        private bool CountrySupportsNewsletterOptions
        {
            get
            {
                bool flag = true;
                if (this.State.BasicAccountInfoStep.IsEnabled)
                {
                    AccountCountry country = AccountCountryList.Instance.GetCountry(this.State.BasicAccountInfoStep.SelectedCountry);
                    if (country != null)
                        flag = country.ShowNewsletterOptions;
                }
                return flag;
            }
        }

        private bool ShowingSettings => this.ShowSettings != PrivacyInfoSettings.None && (this.ShowSettings & PrivacyInfoSettings.NoNewsletterSettings) == PrivacyInfoSettings.None ? this.CountrySupportsNewsletterOptions : this.ShowSettings != PrivacyInfoSettings.None;

        internal override bool OnMovingNext()
        {
            string str = (string)null;
            if (this.PrivacySettings != null)
            {
                foreach (PrivacySettingChoice privacySetting in (IEnumerable)this.PrivacySettings)
                {
                    if (privacySetting.ChosenValue == null)
                    {
                        str = privacySetting.Description;
                        break;
                    }
                }
            }
            if (this.FamilySettingsChoices != null)
            {
                foreach (FamilySettingChoice familySettingsChoice in (IEnumerable)this.FamilySettingsChoices)
                {
                    if (familySettingsChoice.ChosenValue == null)
                    {
                        str = familySettingsChoice.Title;
                        break;
                    }
                }
            }
            if (str == null)
            {
                this.StatusMessage = (string)null;
                return base.OnMovingNext();
            }
            this.StatusMessage = string.Format(Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_SELECT_VALUE), (object)str);
            return false;
        }

        protected override void OnActivate()
        {
            this.ServiceActivationRequestsDone = false;
            base.OnActivate();
        }

        internal override void Deactivate()
        {
            base.Deactivate();
            this.SetCommittedSettings();
        }

        protected override void OnStartActivationRequests(object state)
        {
            this.InitializeHelpersOnWorkerThread();
            base.OnStartActivationRequests(state);
        }

        protected void InitializeHelpersOnWorkerThread()
        {
            AccountCountryList.Instance.LoadDataOnWorkerThread();
            RatingSystemList.Instance.LoadDataOnWorkerThread();
        }

        protected override void OnEndActivationRequests(object args)
        {
            this.InitializeSettings();
            if (this.CountrySupportsNewsletterOptions)
                return;
            this.AllowMicrosoftCommunications = (BooleanChoice)null;
            this.AllowPartnerCommunications = (BooleanChoice)null;
            if ((this.ShowSettings & PrivacyInfoSettings.NoNewsletterSettings) != PrivacyInfoSettings.None)
                return;
            this._owner.MoveNext();
        }

        private void SetCommittedSettings()
        {
            this.SetDefaultSettings();
            if (this.AllowMicrosoftCommunications != null)
                this.CommittedSettings.AllowZuneEmails = this.AllowMicrosoftCommunications.Value;
            if (this.AllowPartnerCommunications != null)
                this.CommittedSettings.AllowPartnerEmails = this.AllowPartnerCommunications.Value;
            if (this.UsageCollection != null)
                this.CommittedSettings.PrivacySettings[PrivacySettingId.UsageCollection] = this.UsageCollection.Value ? PrivacySettingValue.Allow : PrivacySettingValue.Deny;
            if (this._privacySettings != null)
            {
                foreach (PrivacySettingChoice privacySetting in (IEnumerable)this._privacySettings)
                {
                    if (privacySetting.SettingId == PrivacySettingId.Unknown)
                        this.SetCommittedSpecialProperties(privacySetting);
                    else
                        this.CommittedSettings.PrivacySettings[privacySetting.SettingId] = privacySetting.SettingValue;
                }
            }
            if (this.FamilySettingsChoices == null || this.FamilySettingsChoices.Count <= 0)
                return;
            foreach (FamilySettingChoice familySettingsChoice in (IEnumerable)this.FamilySettingsChoices)
            {
                if (familySettingsChoice.SettingValue != null)
                    this.FamilySettings.SetSetting(familySettingsChoice.SettingId, familySettingsChoice.SettingValue.Value, familySettingsChoice.BlockUnrated.Value);
            }
        }

        private void SetCommittedSpecialProperties(PrivacySettingChoice multiSetting)
        {
            if (this.CommittedSettings == null || multiSetting == null || (multiSetting.SettingId != PrivacySettingId.Unknown || multiSetting.InfoSettings != PrivacyInfoSettings.AllSocial))
                return;
            this.CommittedSettings.PrivacySettings[PrivacySettingId.Communication] = multiSetting.SettingValue;
            this.CommittedSettings.PrivacySettings[PrivacySettingId.FriendsListSharing] = multiSetting.SettingValue;
            this.CommittedSettings.PrivacySettings[PrivacySettingId.MusicSharing] = multiSetting.SettingValue;
            this.CommittedSettings.PrivacySettings[PrivacySettingId.ProfileCustomization] = multiSetting.SettingValue;
        }

        private void SetDefaultSettings()
        {
            if (this.CommittedSettings == null)
            {
                this.CommittedSettings = new AccountSettings();
                this.CommittedSettings.AllowZuneEmails = true;
                if (this.State.BasicAccountInfoStep.NewAccounType == AccountUserType.Adult)
                {
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.ExplicitContent] = PrivacySettingValue.Allow;
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.OnlineFriends] = PrivacySettingValue.Allow;
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.PremiumContent] = PrivacySettingValue.Allow;
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.UsageCollection] = this.UsageCollectionDefault;
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.Communication] = this.ShowingCommunicationSetting ? PrivacySettingValue.Unknown : PrivacySettingValue.Deny;
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.FriendsListSharing] = this.ShowingFriendsListSharingSetting ? PrivacySettingValue.Unknown : PrivacySettingValue.Deny;
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.ProfileCustomization] = this.ShowingProfileCustomizationSetting ? PrivacySettingValue.Unknown : PrivacySettingValue.Deny;
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.MusicSharing] = this.ShowingMusingSharingSetting ? PrivacySettingValue.Unknown : PrivacySettingValue.Deny;
                }
                else if (this.State.BasicAccountInfoStep.NewAccounType == AccountUserType.ChildWithSocial)
                {
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.ExplicitContent] = PrivacySettingValue.Unknown;
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.OnlineFriends] = PrivacySettingValue.Unknown;
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.PremiumContent] = PrivacySettingValue.Unknown;
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.Communication] = PrivacySettingValue.Unknown;
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.FriendsListSharing] = PrivacySettingValue.Unknown;
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.ProfileCustomization] = PrivacySettingValue.Unknown;
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.MusicSharing] = PrivacySettingValue.Unknown;
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.UsageCollection] = this.UsageCollectionDefault;
                }
                else
                {
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.ExplicitContent] = PrivacySettingValue.Unknown;
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.OnlineFriends] = PrivacySettingValue.Deny;
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.PremiumContent] = PrivacySettingValue.Unknown;
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.Communication] = PrivacySettingValue.Deny;
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.FriendsListSharing] = PrivacySettingValue.Deny;
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.ProfileCustomization] = PrivacySettingValue.Deny;
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.MusicSharing] = PrivacySettingValue.Deny;
                    this.CommittedSettings.PrivacySettings[PrivacySettingId.UsageCollection] = this.UsageCollectionDefault;
                }
            }
            if (this.FamilySettings != null)
                return;
            this.FamilySettings = new FamilySettings();
            if (!SignIn.Instance.SignedIn)
                return;
            this.FamilySettings.UserId = SignIn.Instance.LastSignedInUserId;
        }

        private void SetUncommittedSettings()
        {
            if (this.IsDisposed)
                return;
            this.SetDefaultSettings();
            if (this.AllowMicrosoftCommunications != null)
                this.AllowMicrosoftCommunications.Value = this.CommittedSettings.AllowZuneEmails;
            if (this.AllowPartnerCommunications != null)
                this.AllowPartnerCommunications.Value = this.CommittedSettings.AllowPartnerEmails;
            if (this.UsageCollection != null && this.CommittedSettings.PrivacySettings.ContainsKey(PrivacySettingId.UsageCollection))
                this.UsageCollection.Value = this.CommittedSettings.PrivacySettings[PrivacySettingId.UsageCollection] == PrivacySettingValue.Allow;
            if (this._privacySettings != null)
            {
                foreach (PrivacySettingChoice privacySetting in (IEnumerable)this._privacySettings)
                {
                    if (privacySetting.SettingId == PrivacySettingId.Unknown)
                        this.SetUncommittedSpecialProperties(privacySetting);
                    else if (this.CommittedSettings.PrivacySettings.ContainsKey(privacySetting.SettingId))
                        privacySetting.SettingValue = this.CommittedSettings.PrivacySettings[privacySetting.SettingId];
                }
            }
            if (this.FamilySettingsChoices == null)
                return;
            FamilySettings familySettings = SignIn.Instance.FamilySettings;
            if (familySettings == null && this.State.TermsOfServiceStep.IsEnabled && !string.IsNullOrEmpty(this.State.TermsOfServiceStep.Username))
            {
                int idFromPassportId = SignIn.GetUserIdFromPassportId(this.State.TermsOfServiceStep.Username);
                if (idFromPassportId != 0)
                    familySettings = new FamilySettings(idFromPassportId);
            }
            if (familySettings == null)
                return;
            this.FamilySettings.Settings = new Dictionary<string, FamilySetting>((IDictionary<string, FamilySetting>)familySettings.Settings);
            foreach (FamilySettingChoice familySettingsChoice in (IEnumerable)this.FamilySettingsChoices)
            {
                RatingSystemList.Instance.GetRatingSystem(familySettingsChoice.SettingId);
                if (familySettings.Settings.ContainsKey(familySettingsChoice.SettingId))
                {
                    familySettingsChoice.SettingValue = familySettingsChoice.GetSettingValueById(familySettings.Settings[familySettingsChoice.SettingId].RatingLevel);
                    familySettingsChoice.BlockUnrated.Value = familySettings.Settings[familySettingsChoice.SettingId].BlockUnrated;
                }
            }
        }

        private PrivacySettingValue UsageCollectionDefault
        {
            get
            {
                bool flag = false;
                string abbreviation = (string)null;
                if (SignIn.Instance.SignedIn)
                    abbreviation = SignIn.Instance.CountryCode;
                else if (this.State.BasicAccountInfoStep.IsEnabled)
                    abbreviation = this.State.BasicAccountInfoStep.SelectedCountry;
                if (abbreviation != null)
                {
                    AccountCountry country = AccountCountryList.Instance.GetCountry(abbreviation);
                    if (country != null)
                        flag = country.UsageCollection;
                }
                return !flag ? PrivacySettingValue.Deny : PrivacySettingValue.Allow;
            }
        }

        private void SetUncommittedSpecialProperties(PrivacySettingChoice multiSetting)
        {
            if (this.CommittedSettings == null || multiSetting == null || (multiSetting.SettingId != PrivacySettingId.Unknown || multiSetting.InfoSettings != PrivacyInfoSettings.AllSocial))
                return;
            multiSetting.SettingValue = PrivacySettingValue.Unknown;
            PrivacySettingValue privacySettingValue1 = this.CommittedSettings.PrivacySettings.ContainsKey(PrivacySettingId.Communication) ? this.CommittedSettings.PrivacySettings[PrivacySettingId.Communication] : PrivacySettingValue.Unknown;
            PrivacySettingValue privacySettingValue2 = this.CommittedSettings.PrivacySettings.ContainsKey(PrivacySettingId.FriendsListSharing) ? this.CommittedSettings.PrivacySettings[PrivacySettingId.FriendsListSharing] : PrivacySettingValue.Unknown;
            PrivacySettingValue privacySettingValue3 = this.CommittedSettings.PrivacySettings.ContainsKey(PrivacySettingId.MusicSharing) ? this.CommittedSettings.PrivacySettings[PrivacySettingId.MusicSharing] : PrivacySettingValue.Unknown;
            PrivacySettingValue privacySettingValue4 = this.CommittedSettings.PrivacySettings.ContainsKey(PrivacySettingId.ProfileCustomization) ? this.CommittedSettings.PrivacySettings[PrivacySettingId.ProfileCustomization] : PrivacySettingValue.Unknown;
            if (privacySettingValue1 != PrivacySettingValue.Unknown && (multiSetting.SettingValue == PrivacySettingValue.Unknown || multiSetting.SettingValue > privacySettingValue1))
                multiSetting.SettingValue = privacySettingValue1;
            if (privacySettingValue2 != PrivacySettingValue.Unknown && (multiSetting.SettingValue == PrivacySettingValue.Unknown || multiSetting.SettingValue > privacySettingValue2))
                multiSetting.SettingValue = privacySettingValue2;
            if (privacySettingValue3 != PrivacySettingValue.Unknown && (multiSetting.SettingValue == PrivacySettingValue.Unknown || multiSetting.SettingValue > privacySettingValue3))
                multiSetting.SettingValue = privacySettingValue3;
            if (privacySettingValue4 == PrivacySettingValue.Unknown || multiSetting.SettingValue != PrivacySettingValue.Unknown && multiSetting.SettingValue <= privacySettingValue4)
                return;
            multiSetting.SettingValue = privacySettingValue4;
        }

        private void ResetSettings()
        {
            this.AllowMicrosoftCommunications = (BooleanChoice)null;
            this.AllowPartnerCommunications = (BooleanChoice)null;
            this.UsageCollection = (BooleanChoice)null;
            this.PrivacySettings = (IList)null;
            this.FamilySettingsChoices = (IList)null;
            this.FamilySettings = (FamilySettings)null;
            if (this._owner.CurrentPage != this)
                return;
            this.InitializeSettings();
        }

        protected void InitializeSettings()
        {
            if (this.IsDisposed)
                return;
            if ((this.ShowSettings & PrivacyInfoSettings.AllowMicrosoftCommunications) == PrivacyInfoSettings.AllowMicrosoftCommunications && this._allowMicrosoftCommunications == null && this.CountrySupportsNewsletterOptions)
                this._allowMicrosoftCommunications = new BooleanChoice((IModelItemOwner)this, Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_ZUNE_COMM_DESC));
            if ((this.ShowSettings & PrivacyInfoSettings.AllowPartnerCommunications) == PrivacyInfoSettings.AllowPartnerCommunications && this._allowPartnerCommunications == null && this.CountrySupportsNewsletterOptions)
                this._allowPartnerCommunications = new BooleanChoice((IModelItemOwner)this, Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_ZUNE_PARTNER_DESC));
            bool flag = (this.ShowSettings & PrivacyInfoSettings.NoNewsletterSettings) != PrivacyInfoSettings.None;
            if (this.PrivacySettings == null && flag)
            {
                ArrayList arrayList = new ArrayList();
                if ((this.ShowSettings & PrivacyInfoSettings.AllSocial) == PrivacyInfoSettings.AllSocial)
                    arrayList.Add((object)new PrivacySettingChoice((IModelItemOwner)this, Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_ZUNE_ALL_SOCIAL_DESC), Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_ZUNE_ALL_SOCIAL_HEAD), PrivacySettingChoice.AllowFriendDenyChoices, PrivacySettingId.Unknown, PrivacyInfoSettings.AllSocial));
                if ((this.ShowSettings & PrivacyInfoSettings.AllowExplicitContent) == PrivacyInfoSettings.AllowExplicitContent)
                {
                    StringId stringId1 = StringId.IDS_ACCOUNT_CREATION_ZUNE_CHILD_EXPLICIT_DESC;
                    StringId stringId2 = StringId.IDS_ACCOUNT_CREATION_ZUNE_CHILD_EXPLICIT_HEAD;
                    string linkDescription = string.Empty;
                    string linkUrl = string.Empty;
                    if (FeatureEnablement.IsFeatureEnabled(Features.eApps) || FeatureEnablement.IsFeatureEnabled(Features.eGames))
                    {
                        linkDescription = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_ZUNE_CHILD_EXPLICIT_LINK);
                        linkUrl = "http://go.microsoft.com/fwlink/?LinkId=218881";
                        if (FeatureEnablement.IsFeatureEnabled(Features.eMusic))
                        {
                            stringId1 = StringId.IDS_ACCOUNT_CREATION_ZUNE_CHILD_EXPLICIT_DESC2;
                            stringId2 = StringId.IDS_ACCOUNT_CREATION_ZUNE_CHILD_EXPLICIT_HEAD2;
                        }
                        else
                        {
                            stringId1 = StringId.IDS_ACCOUNT_CREATION_ZUNE_CHILD_EXPLICIT_DESC3;
                            stringId2 = StringId.IDS_ACCOUNT_CREATION_ZUNE_CHILD_EXPLICIT_HEAD3;
                        }
                    }
                    arrayList.Add((object)new PrivacySettingChoice((IModelItemOwner)this, Shell.LoadString(stringId1), Shell.LoadString(stringId2), PrivacySettingChoice.AllowDenyChoices, PrivacySettingId.ExplicitContent, PrivacyInfoSettings.AllowExplicitContent, linkDescription, linkUrl));
                }
                if ((this.ShowSettings & PrivacyInfoSettings.AllowFriends) == PrivacyInfoSettings.AllowFriends)
                    arrayList.Add((object)new PrivacySettingChoice((IModelItemOwner)this, Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_ZUNE_CHILD_NEWFRIEND_DESC), Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_ZUNE_CHILD_NEWFRIEND_HEAD), PrivacySettingChoice.AllowDenyChoices, PrivacySettingId.OnlineFriends, PrivacyInfoSettings.AllowFriends));
                if ((this.ShowSettings & PrivacyInfoSettings.AllowPurchase) == PrivacyInfoSettings.AllowPurchase)
                    arrayList.Add((object)new PrivacySettingChoice((IModelItemOwner)this, Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_ZUNE_CHILD_PURCHASE_DESC), Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_ZUNE_CHILD_PURCHASE_HEAD), PrivacySettingChoice.AllowDenyChoices, PrivacySettingId.PremiumContent, PrivacyInfoSettings.AllowPurchase));
                if ((this.ShowSettings & PrivacyInfoSettings.Communications) == PrivacyInfoSettings.Communications)
                    arrayList.Add((object)new PrivacySettingChoice((IModelItemOwner)this, Shell.LoadString(this.ParentAccount ? StringId.IDS_ACCOUNT_CREATION_ZUNE_CHILD_COMM_DESC : StringId.IDS_ACCOUNT_CREATION_ZUNE_SOCIAL_COMM_DESC), Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_ZUNE_SOCIAL_COMM_HEADER), PrivacySettingChoice.AllowFriendDenyChoices, PrivacySettingId.Communication, PrivacyInfoSettings.Communications));
                if ((this.ShowSettings & PrivacyInfoSettings.FriendsSharing) == PrivacyInfoSettings.FriendsSharing)
                    arrayList.Add((object)new PrivacySettingChoice((IModelItemOwner)this, Shell.LoadString(this.ParentAccount ? StringId.IDS_ACCOUNT_CREATION_ZUNE_CHILD_FRIENDS_DESC : StringId.IDS_ACCOUNT_CREATION_ZUNE_FRIENDS_DESC), Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_ZUNE_FRIENDS_HEAD), PrivacySettingChoice.AllowFriendDenyChoices, PrivacySettingId.FriendsListSharing, PrivacyInfoSettings.FriendsSharing));
                if ((this.ShowSettings & PrivacyInfoSettings.MusicSharing) == PrivacyInfoSettings.MusicSharing)
                    arrayList.Add((object)new PrivacySettingChoice((IModelItemOwner)this, Shell.LoadString(this.ParentAccount ? StringId.IDS_ACCOUNT_CREATION_ZUNE_CHILD_MUSIC_DESC : StringId.IDS_ACCOUNT_CREATION_SOCIAL_PROP_DESC), Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_SOCIAL_PROP_HEADER), PrivacySettingChoice.AllowFriendDenyChoices, PrivacySettingId.MusicSharing, PrivacyInfoSettings.MusicSharing));
                if ((this.ShowSettings & PrivacyInfoSettings.ProfileCustomization) == PrivacyInfoSettings.ProfileCustomization)
                    arrayList.Add((object)new PrivacySettingChoice((IModelItemOwner)this, Shell.LoadString(this.ParentAccount ? StringId.IDS_ACCOUNT_CREATION_ZUNE_CHILD_PROFILE_DESC : StringId.IDS_ACCOUNT_CREATION_ZUNE_PROFILE_DESC), Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_ZUNE_PROFILE_HEAD), PrivacySettingChoice.AllowFriendDenyChoices, PrivacySettingId.ProfileCustomization, PrivacyInfoSettings.ProfileCustomization));
                if ((this.ShowSettings & PrivacyInfoSettings.UsageCollection) == PrivacyInfoSettings.UsageCollection && this._usageCollection == null)
                    this._usageCollection = new BooleanChoice((IModelItemOwner)this, Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_PERSONALIZE_DESC));
                this.PrivacySettings = (IList)arrayList;
            }
            if (this.FamilySettingsChoices == null && flag && (RatingSystemList.Instance.RatingSystems != null && (this.ShowSettings & PrivacyInfoSettings.AllowExplicitContent) == PrivacyInfoSettings.AllowExplicitContent))
            {
                ArrayList arrayList1 = new ArrayList();
                foreach (string ratingSystem1 in (IEnumerable)RatingSystemList.Instance.RatingSystems)
                {
                    RatingSystem ratingSystem2 = RatingSystemList.Instance.GetRatingSystem(ratingSystem1);
                    ArrayList arrayList2 = new ArrayList();
                    foreach (RatingValue rating in ratingSystem2.Ratings)
                    {
                        if (!rating.TreatAsUnrated)
                            arrayList2.Add((object)new FamilySettingValue(ratingSystem2.GetRatingName(rating.Order), rating.Order));
                    }
                    arrayList1.Add((object)new FamilySettingChoice((IModelItemOwner)this, ratingSystem2.Title, ratingSystem2.Description, ratingSystem2.BlockText, ratingSystem2.Name, ratingSystem2.ShowBlockUnrated, (IList)arrayList2));
                }
                this.FamilySettingsChoices = (IList)arrayList1;
            }
            this.SetUncommittedSettings();
        }
    }
}
