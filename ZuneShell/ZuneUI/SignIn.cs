// Decompiled with JetBrains decompiler
// Type: ZuneUI.SignIn
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.ErrorMapperApi;
using Microsoft.Zune.Shell;
using Microsoft.Zune.User;
using Microsoft.Zune.Util;
using MicrosoftZuneLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace ZuneUI
{
    public class SignIn : ModelItem
    {
        private const int m_iInvalidUserId = 0;
        private static SignIn s_instance;
        private static object s_cs = new object();
        private static TimeSpan s_subscriptionEndingWarning = TimeSpan.FromDays(7.0);
        private static TimeSpan s_subscriptionFreeTrackFirstExpireWarning = TimeSpan.FromDays(7.0);
        private static TimeSpan s_subscriptionFreeTrackSecondExpireWarning = TimeSpan.FromDays(2.0);
        private bool m_initialized;
        private bool m_fSigningIn;
        private bool m_fSignedIn;
        private bool m_fSignedInWithSubscription;
        private string m_strSignedInUsername;
        private uint m_dwSignedInGeoId;
        private int m_iLastSignedInUserId;
        private string m_lastSignedInUsername;
        private Guid m_lastSignedInUserGuid;
        private bool m_lastSignedInUserHadActiveSubscription;
        private string m_zunetag;
        private string m_countryCode;
        private Image m_zuneTile;
        private string m_zuneTilePath;
        private Guid m_userguid;
        private string m_tempPasswordStorage;
        private int m_pointsBalance;
        private int m_subscriptionFreeTrackBalance;
        private DateTime m_subscriptionFreeTrackExpiration;
        private bool m_subscriptionFreeTrackExpiring;
        private bool m_subscriptionFreeTrackExpiringWithinDays;
        private IList m_persistedUsernames;
        private string m_strErrorMessage;
        private string m_strErrorWebHelpUrl;
        private HRESULT m_hrError;
        private bool m_fErrorTermsOfService;
        private bool m_fErrorTermsOfServiceForChild;
        private bool m_fRegionMismatchError;
        private bool m_fErrorHttpGone;
        private bool m_fErrorCredentials;
        private bool m_fSubscriptionMachineCountExceeded;
        private bool m_fShowLabelTakedownWarning;
        private bool m_fSubscriptionAvailable;
        private bool m_fSubscriptionBillingViolation;
        private bool m_fSubscriptionExpiring;
        private bool m_fSubscriptionExpired;
        private bool m_fSubscriptionTrialsAvailable;
        private bool m_fIsParentallyControlled;
        private bool m_fIsLightWeight;
        private bool m_noZuneAccountError;
        private DateTime m_subscriptionEndDate;
        private ulong m_subscriptionId;
        private ulong m_subscriptionRenewalId;
        private FamilySettings m_familySettings;
        private Version m_maxConnectedPhoneVersion;
        private Version m_maxRegisteredPhoneVersion;
        private Version m_maxWindowsPhoneVersion;
        private ITunerInfoHandler m_tunerHandler;

        public static SignIn Instance
        {
            get
            {
                if (s_instance == null)
                {
                    lock (s_cs)
                    {
                        if (s_instance == null)
                            s_instance = new SignIn();
                    }
                }
                return s_instance;
            }
        }

        internal event EventHandler SignInStatusUpdatedEvent;

        public bool Initialized
        {
            get => this.m_initialized;
            private set
            {
                if (this.m_initialized == value)
                    return;
                this.m_initialized = value;
                this.FirePropertyChanged(nameof(Initialized));
            }
        }

        public bool SigningIn
        {
            get => this.m_fSigningIn;
            set
            {
                if (this.m_fSigningIn == value)
                    return;
                this.m_fSigningIn = value;
                this.FirePropertyChanged(nameof(SigningIn));
            }
        }

        public bool SignedIn
        {
            get => this.m_fSignedIn;
            set
            {
                if (this.m_fSignedIn == value)
                    return;
                this.m_fSignedIn = value;
                this.FirePropertyChanged(nameof(SignedIn));
            }
        }

        public bool SignedInWithSubscription
        {
            get => this.m_fSignedInWithSubscription;
            set
            {
                if (this.m_fSignedInWithSubscription == value)
                    return;
                this.m_fSignedInWithSubscription = value;
                this.FirePropertyChanged(nameof(SignedInWithSubscription));
            }
        }

        public string SignedInUsername
        {
            get => this.m_strSignedInUsername;
            set
            {
                if (!(this.m_strSignedInUsername != value))
                    return;
                this.m_strSignedInUsername = value;
                this.FirePropertyChanged(nameof(SignedInUsername));
            }
        }

        public uint SignedInGeoId
        {
            get => this.m_dwSignedInGeoId;
            set
            {
                if ((int)this.m_dwSignedInGeoId == (int)value)
                    return;
                this.m_dwSignedInGeoId = value;
                this.FirePropertyChanged(nameof(SignedInGeoId));
            }
        }

        public int LastSignedInUserId
        {
            get
            {
                this.GetLastSignedIdUser();
                return this.m_iLastSignedInUserId;
            }
            set
            {
                if (this.m_iLastSignedInUserId == value)
                    return;
                this.m_iLastSignedInUserId = value;
                this.FirePropertyChanged(nameof(LastSignedInUserId));
            }
        }

        public string LastSignedInUsername
        {
            get
            {
                if (this.m_lastSignedInUsername == null && this.LastSignedInUserId > 0)
                    this.m_lastSignedInUsername = GetPassportIdFromUserId(this.LastSignedInUserId);
                return this.m_lastSignedInUsername;
            }
            private set
            {
                if (!(this.m_lastSignedInUsername != value))
                    return;
                this.m_lastSignedInUsername = value;
                this.FirePropertyChanged(nameof(LastSignedInUsername));
            }
        }

        public Guid LastSignedInUserGuid
        {
            get
            {
                this.GetLastSignedIdUser();
                return this.m_lastSignedInUserGuid;
            }
            set
            {
                if (!(this.m_lastSignedInUserGuid != value))
                    return;
                this.m_lastSignedInUserGuid = value;
                this.FirePropertyChanged(nameof(LastSignedInUserGuid));
            }
        }

        public bool LastSignedInUserHadActiveSubscription
        {
            get => this.m_lastSignedInUserHadActiveSubscription;
            private set
            {
                if (this.m_lastSignedInUserHadActiveSubscription == value)
                    return;
                this.m_lastSignedInUserHadActiveSubscription = value;
                this.FirePropertyChanged(nameof(LastSignedInUserHadActiveSubscription));
            }
        }

        public Image ZuneTile
        {
            get => this.m_zuneTile;
            set
            {
                if (this.m_zuneTile == value)
                    return;
                this.m_zuneTile = value;
                this.FirePropertyChanged(nameof(ZuneTile));
            }
        }

        public string ZuneTag
        {
            get => this.m_zunetag;
            set
            {
                if (!(this.m_zunetag != value))
                    return;
                this.m_zunetag = value;
                this.FirePropertyChanged(nameof(ZuneTag));
            }
        }

        public string CountryCode
        {
            get => this.m_countryCode;
            set
            {
                if (!(this.m_countryCode != value))
                    return;
                this.m_countryCode = value;
                this.FirePropertyChanged(nameof(CountryCode));
            }
        }

        public Guid UserGuid
        {
            get => this.m_userguid;
            set
            {
                if (!(this.m_userguid != value))
                    return;
                this.m_userguid = value;
                this.FirePropertyChanged(nameof(UserGuid));
            }
        }

        public string TempPasswordStorage
        {
            get => this.m_tempPasswordStorage;
            set
            {
                if (!(this.m_tempPasswordStorage != value))
                    return;
                this.m_tempPasswordStorage = value;
                this.FirePropertyChanged(nameof(TempPasswordStorage));
            }
        }

        public int PointsBalance
        {
            get
            {
                this.UpdatePointsBalance();
                return this.m_pointsBalance;
            }
        }

        private void UpdatePointsBalance()
        {
            int pointsBalance = ZuneApplication.Service.GetPointsBalance();
            if (pointsBalance == this.m_pointsBalance)
                return;
            this.m_pointsBalance = pointsBalance;
            this.FirePropertyChanged("PointsBalance");
        }

        public DateTime SubscriptionFreeTrackExpiration
        {
            get
            {
                this.UpdateSubscriptionFreeTrackExpiration();
                return this.m_subscriptionFreeTrackExpiration;
            }
            private set
            {
                if (!(this.m_subscriptionFreeTrackExpiration != value))
                    return;
                this.m_subscriptionFreeTrackExpiration = value;
                this.FirePropertyChanged(nameof(SubscriptionFreeTrackExpiration));
            }
        }

        public bool SubscriptionFreeTrackExpiring
        {
            get => this.m_subscriptionFreeTrackExpiring;
            private set
            {
                if (this.m_subscriptionFreeTrackExpiring == value)
                    return;
                this.m_subscriptionFreeTrackExpiring = value;
                this.FirePropertyChanged(nameof(SubscriptionFreeTrackExpiring));
            }
        }

        public bool SubscriptionFreeTrackExpiringWithinDays
        {
            get => this.m_subscriptionFreeTrackExpiringWithinDays;
            private set
            {
                if (this.m_subscriptionFreeTrackExpiringWithinDays == value)
                    return;
                this.m_subscriptionFreeTrackExpiringWithinDays = value;
                this.FirePropertyChanged(nameof(SubscriptionFreeTrackExpiringWithinDays));
            }
        }

        public int SubscriptionFreeTrackBalance
        {
            get
            {
                this.UpdateSubscriptionFreeTrackBalance();
                return this.m_subscriptionFreeTrackBalance;
            }
            private set
            {
                if (this.m_subscriptionFreeTrackBalance == value)
                    return;
                this.m_subscriptionFreeTrackBalance = value;
                this.FirePropertyChanged(nameof(SubscriptionFreeTrackBalance));
            }
        }

        public void UpdateSubscriptionFreeTrackBalance() => this.SubscriptionFreeTrackBalance = ZuneApplication.Service.GetSubscriptionFreeTrackBalance();

        public IList PersistedUsernames
        {
            get
            {
                if (this.m_persistedUsernames == null)
                    this.m_persistedUsernames = ZuneApplication.Service.GetPersistedUsernames();
                return this.m_persistedUsernames;
            }
            set
            {
                if (this.m_persistedUsernames == value)
                    return;
                this.m_persistedUsernames = value;
                this.FirePropertyChanged(nameof(PersistedUsernames));
            }
        }

        public bool SubscriptionMachineCountExceeded
        {
            get => this.m_fSubscriptionMachineCountExceeded;
            private set
            {
                if (this.m_fSubscriptionMachineCountExceeded == value)
                    return;
                this.m_fSubscriptionMachineCountExceeded = value;
                this.FirePropertyChanged(nameof(SubscriptionMachineCountExceeded));
            }
        }

        public bool ShowLabelTakedownWarning
        {
            get => this.m_fShowLabelTakedownWarning;
            private set
            {
                if (this.m_fShowLabelTakedownWarning == value)
                    return;
                this.m_fShowLabelTakedownWarning = value;
                this.FirePropertyChanged(nameof(ShowLabelTakedownWarning));
            }
        }

        public bool SubscriptionBillingViolation
        {
            get => this.m_fSubscriptionBillingViolation;
            private set
            {
                if (this.m_fSubscriptionBillingViolation == value)
                    return;
                this.m_fSubscriptionBillingViolation = value;
                this.FirePropertyChanged(nameof(SubscriptionBillingViolation));
            }
        }

        public bool SubscriptionAvailable
        {
            get => this.m_fSubscriptionAvailable;
            private set
            {
                if (this.m_fSubscriptionAvailable == value)
                    return;
                this.m_fSubscriptionAvailable = value;
                this.FirePropertyChanged(nameof(SubscriptionAvailable));
            }
        }

        public bool SubscriptionTrialsAvailable
        {
            get => this.m_fSubscriptionTrialsAvailable;
            private set
            {
                if (this.m_fSubscriptionTrialsAvailable == value)
                    return;
                this.m_fSubscriptionTrialsAvailable = value;
                this.FirePropertyChanged(nameof(SubscriptionTrialsAvailable));
            }
        }

        public HRESULT SignInError
        {
            get => this.m_hrError;
            private set
            {
                if (!(this.m_hrError != value))
                    return;
                this.m_hrError = value;
                this.FirePropertyChanged(nameof(SignInError));
            }
        }

        public string SignInErrorMessage
        {
            get => this.m_strErrorMessage;
            set
            {
                if (!(this.m_strErrorMessage != value))
                    return;
                this.m_strErrorMessage = value;
                this.FirePropertyChanged(nameof(SignInErrorMessage));
            }
        }

        public string SignInErrorWebHelpUrl
        {
            get => this.m_strErrorWebHelpUrl;
            set
            {
                if (!(this.m_strErrorWebHelpUrl != value))
                    return;
                this.m_strErrorWebHelpUrl = value;
                this.FirePropertyChanged(nameof(SignInErrorWebHelpUrl));
            }
        }

        public bool SignInTermsOfServiceError
        {
            get => this.m_fErrorTermsOfService;
            set
            {
                if (this.m_fErrorTermsOfService == value)
                    return;
                this.m_fErrorTermsOfService = value;
                this.FirePropertyChanged(nameof(SignInTermsOfServiceError));
            }
        }

        public bool SignInRegionMismatchError
        {
            get => this.m_fRegionMismatchError;
            set
            {
                if (this.m_fRegionMismatchError == value)
                    return;
                this.m_fRegionMismatchError = value;
                this.FirePropertyChanged(nameof(SignInRegionMismatchError));
            }
        }

        public bool SignInTermsOfServiceErrorForChild
        {
            get => this.m_fErrorTermsOfServiceForChild;
            set
            {
                if (this.m_fErrorTermsOfServiceForChild == value)
                    return;
                this.m_fErrorTermsOfServiceForChild = value;
                this.FirePropertyChanged(nameof(SignInTermsOfServiceErrorForChild));
            }
        }

        public bool SignInHttpGoneError
        {
            get => this.m_fErrorHttpGone;
            set
            {
                if (this.m_fErrorHttpGone == value)
                    return;
                this.m_fErrorHttpGone = value;
                this.FirePropertyChanged(nameof(SignInHttpGoneError));
            }
        }

        public bool SignInCredentialsError
        {
            get => this.m_fErrorCredentials;
            set
            {
                if (this.m_fErrorCredentials == value)
                    return;
                this.m_fErrorCredentials = value;
                this.FirePropertyChanged(nameof(SignInCredentialsError));
            }
        }

        public bool SignInNoZuneAccountError
        {
            get => this.m_noZuneAccountError;
            private set
            {
                if (this.m_noZuneAccountError == value)
                    return;
                this.m_noZuneAccountError = value;
                this.FirePropertyChanged(nameof(SignInNoZuneAccountError));
            }
        }

        public bool IsParentallyControlled
        {
            get => this.m_fIsParentallyControlled;
            private set
            {
                if (this.m_fIsParentallyControlled == value)
                    return;
                this.m_fIsParentallyControlled = value;
                this.FirePropertyChanged(nameof(IsParentallyControlled));
            }
        }

        public bool IsLightWeight
        {
            get => this.m_fIsLightWeight;
            private set
            {
                if (this.m_fIsLightWeight == value)
                    return;
                this.m_fIsLightWeight = value;
                this.FirePropertyChanged(nameof(IsLightWeight));
            }
        }

        public bool SubscriptionExpiring
        {
            get => this.m_fSubscriptionExpiring;
            private set
            {
                if (this.m_fSubscriptionExpiring == value)
                    return;
                this.m_fSubscriptionExpiring = value;
                this.FirePropertyChanged(nameof(SubscriptionExpiring));
            }
        }

        public bool SubscriptionExpired
        {
            get => this.m_fSubscriptionExpired;
            private set
            {
                if (this.m_fSubscriptionExpired == value)
                    return;
                this.m_fSubscriptionExpired = value;
                this.FirePropertyChanged(nameof(SubscriptionExpired));
            }
        }

        public DateTime SubscriptionEndDate
        {
            get => this.m_subscriptionEndDate;
            private set
            {
                if (!(this.m_subscriptionEndDate != value))
                    return;
                this.m_subscriptionEndDate = value;
                this.FirePropertyChanged("SubscriptionExpired");
                this.FirePropertyChanged("SubscriptionHasEndDate");
            }
        }

        public ulong SubscriptionId
        {
            get => this.m_subscriptionId;
            private set
            {
                if ((long)this.m_subscriptionId == (long)value)
                    return;
                this.m_subscriptionId = value;
                this.FirePropertyChanged(nameof(SubscriptionId));
                this.FirePropertyChanged("HasSubscriptionId");
            }
        }

        public bool HasSubscriptionId => this.m_subscriptionId != 0UL;

        public ulong SubscriptionRenewalId
        {
            get => this.m_subscriptionRenewalId;
            private set
            {
                if ((long)this.m_subscriptionRenewalId == (long)value)
                    return;
                this.m_subscriptionRenewalId = value;
                this.FirePropertyChanged(nameof(SubscriptionRenewalId));
            }
        }

        public bool SubscriptionHasEndDate => this.m_subscriptionEndDate != DateTime.MaxValue && this.m_subscriptionEndDate.Year < 9999;

        private Version MaxWindowsPhoneVersion
        {
            get => this.m_maxWindowsPhoneVersion;
            set
            {
                if (!(this.m_maxWindowsPhoneVersion != value))
                    return;
                this.m_maxWindowsPhoneVersion = value;
                this.FirePropertyChanged("WindowsPhoneClientType");
            }
        }

        private void UpdateMaxWindowsPhoneVersion()
        {
            if (this.MaxConnectedPhoneVersion == null)
                this.MaxWindowsPhoneVersion = this.MaxRegisteredPhoneVersion;
            else if (this.MaxRegisteredPhoneVersion == null)
                this.MaxWindowsPhoneVersion = this.MaxConnectedPhoneVersion;
            else
                this.MaxWindowsPhoneVersion = this.MaxRegisteredPhoneVersion > this.MaxConnectedPhoneVersion ? this.MaxRegisteredPhoneVersion : this.MaxConnectedPhoneVersion;
        }

        private Version MaxConnectedPhoneVersion
        {
            get => this.m_maxConnectedPhoneVersion;
            set
            {
                if (!(this.m_maxConnectedPhoneVersion != value))
                    return;
                this.m_maxConnectedPhoneVersion = value;
                this.UpdateMaxWindowsPhoneVersion();
            }
        }

        private Version MaxRegisteredPhoneVersion
        {
            get => this.m_maxRegisteredPhoneVersion;
            set
            {
                if (!(this.m_maxRegisteredPhoneVersion != value))
                    return;
                this.m_maxRegisteredPhoneVersion = value;
                this.UpdateMaxWindowsPhoneVersion();
            }
        }

        public string WindowsPhoneClientType => this.GetWindowsPhoneClientType(this.MaxWindowsPhoneVersion);

        private string GetWindowsPhoneClientType(Version phoneVersion)
        {
            string strPhoneOsVersion = string.Empty;
            if (phoneVersion != null)
                strPhoneOsVersion = string.Format("{0}.{1}", phoneVersion.Major, phoneVersion.Minor);
            return Microsoft.Zune.Service.Service.Instance.GetPhoneClientType(strPhoneOsVersion);
        }

        public void CheckConnectedPhoneForNewerFirmware()
        {
            this.MaxConnectedPhoneVersion = this.GetConnectedPhoneFirmwareVersion();
            this.UpdateMaxWindowsPhoneVersion();
        }

        private Version GetConnectedPhoneFirmwareVersion()
        {
            Version version = null;
            UIDevice connectedPaidAppDevice = ApplicationMarketplaceHelper.FindConnectedPaidAppDevice();
            if (connectedPaidAppDevice != UIDeviceList.NullDevice && connectedPaidAppDevice.Class == DeviceClass.WindowsPhone)
            {
                if (!string.IsNullOrEmpty(connectedPaidAppDevice.FirmwareVersion))
                {
                    try
                    {
                        version = new Version(connectedPaidAppDevice.FirmwareVersion.Split('-')[0]);
                    }
                    catch
                    {
                    }
                }
            }
            return version;
        }

        private ITunerInfoHandler TunerHandler
        {
            get
            {
                if (this.m_tunerHandler == null)
                {
                    this.m_tunerHandler = TunerInfoHandlerFactory.CreateTunerInfoHandler();
                    this.m_tunerHandler.OnChanged += new EventHandler(this.OnTunerInfoChanged);
                }
                return this.m_tunerHandler;
            }
        }

        private void OnTunerInfoChanged(object oSenderUNUSED, EventArgs eargs) => Application.DeferredInvoke(new DeferredInvokeHandler(this.UpdatedAssociatedPhoneVersion), DeferredInvokePriority.Normal);

        private void UpdatedAssociatedPhoneVersion(object argsUNUSED)
        {
            Version version1 = null;
            foreach (TunerInfo appStoreDevices in this.TunerHandler.GetAppStoreDevicesList())
            {
                if (appStoreDevices.TunerType == TunerType.MobileDevice)
                {
                    Version version2 = new Version(appStoreDevices.TunerVersion);
                    if (version1 == null || version2 > version1)
                        version1 = version2;
                }
            }
            this.MaxRegisteredPhoneVersion = version1;
            this.MaxConnectedPhoneVersion = this.GetConnectedPhoneFirmwareVersion();
            this.UpdateMaxWindowsPhoneVersion();
        }

        public bool PasswordRequired(string strUsername) => ZuneApplication.Service.SignInPasswordRequired(strUsername);

        public bool SignInAtStartup(string strUsername) => ZuneApplication.Service.SignInAtStartup(strUsername);

        public bool RememberUsername(string strUsername)
        {
            bool flag = this.PersistedUsernames != null && this.PersistedUsernames.Contains(strUsername);
            if (!flag)
                flag = !this.PasswordRequired(strUsername);
            return flag;
        }

        public void RefreshAccount()
        {
            if (!ZuneApplication.Service.IsSignedIn())
                return;
            ZuneApplication.Service.RefreshAccount(new AsyncCompleteHandler(this.OnManualSignIn));
        }

        public void SignInUser(string strUsername, string strPassword)
        {
            bool fRememberUsername = this.RememberUsername(strUsername);
            bool fRememberPassword = !this.PasswordRequired(strUsername);
            bool fSignInAtStartup = this.SignInAtStartup(strUsername);
            this.SignInUser(strUsername, strPassword, fRememberUsername, fRememberPassword, fSignInAtStartup);
        }

        public void SignInUser(
          string strUsername,
          string strPassword,
          bool fRememberUsername,
          bool fRememberPassword,
          bool fSignInAtStartup)
        {
            if (this.SigningIn)
                ZuneApplication.Service.CancelSignIn();
            else if (this.SignedIn)
                ZuneApplication.Service.SignOut();
            if (strPassword == null || strPassword == this.PseudoPassword)
                strPassword = string.Empty;
            ZuneApplication.Service.SignIn(strUsername, strPassword, fRememberUsername, fRememberPassword, fSignInAtStartup, new AsyncCompleteHandler(this.OnManualSignIn));
            this.UpdateState();
        }

        public void SwitchToUser(string strUsername)
        {
            Guid guidFromPassportId = GetGuidFromPassportId(strUsername);
            int iUserId = 0;
            if (!ZuneApplication.Service.SetLastSignedInUserGuid(ref guidFromPassportId, out iUserId))
                return;
            if (this.SigningIn)
                ZuneApplication.Service.CancelSignIn();
            else if (this.SignedIn)
                ZuneApplication.Service.SignOut();
            this.LastSignedInUserId = iUserId;
            this.LastSignedInUserGuid = guidFromPassportId;
            this.UpdateState();
        }

        public void CancelSignIn()
        {
            ZuneApplication.Service.CancelSignIn();
            this.UpdateState();
        }

        public void SignOut() => this.SignOut(false);

        private void SignOut(bool forget)
        {
            ZuneApplication.Service.SignOut();
            if (forget)
                this.ClearLastSignedIdUser();
            this.UpdateState();
        }

        public string PseudoPassword => "********";

        public void RemovePersistedUsername(string persistedUsername)
        {
            ZuneApplication.Service.RemovePersistedUsername(persistedUsername);
            int idFromPassportId = GetUserIdFromPassportId(persistedUsername);
            if (idFromPassportId > 0)
                UserManager.Instance.CleanupUserData(idFromPassportId);
            this.PersistedUsernames = null;
            if (!TagsMatch(this.ZuneTag, GetZuneTagFromPassportId(persistedUsername)))
                return;
            this.SignOut(true);
        }

        public static bool TagsMatch(string zuneTag1, string zuneTag2) => StringHelper.CaseInsensitiveCompare(zuneTag1, zuneTag2);

        public static bool LiveIdsMatch(string liveId1, string liveId2) => StringHelper.CaseInsensitiveCompare(liveId1, liveId2);

        public bool IsSignedInUser(string zuneTag) => this.SignedIn && TagsMatch(zuneTag, this.m_zunetag);

        public bool IsSignedInLiveId(string liveId) => this.SignedIn && LiveIdsMatch(liveId, this.SignedInUsername);

        public bool IsLastSignedInLiveId(string liveId) => LiveIdsMatch(liveId, this.LastSignedInUsername);

        private static object GetUserFieldValues(
          int userId,
          SchemaMap columnIndex,
          object defaultValue)
        {
            object obj = null;
            if (userId > 0)
            {
                int[] columnIndexes = new int[1]
                {
          (int) columnIndex
                };
                object[] fieldValues = new object[1] { defaultValue };
                if (ZuneLibrary.GetFieldValues(userId, EListType.eUserList, columnIndexes.Length, columnIndexes, fieldValues, PlaylistManager.Instance.QueryContext).IsSuccess)
                    obj = fieldValues[0];
            }
            return obj;
        }

        public FamilySettings FamilySettings
        {
            get
            {
                if (this.SignedIn)
                {
                    if (this.m_familySettings == null)
                        this.m_familySettings = new FamilySettings(this.LastSignedInUserId);
                }
                else
                    this.m_familySettings = null;
                return this.m_familySettings;
            }
            private set
            {
                this.m_familySettings = value;
                this.FirePropertyChanged(nameof(FamilySettings));
            }
        }

        public static string GetPassportIdFromUserId(int userId) => GetUserFieldValues(userId, SchemaMap.kiIndex_PassportID, string.Empty) as string;

        public static Guid GetGuidFromPassportId(string passportId) => GetGuidFromUserId(GetUserIdFromPassportId(passportId));

        public static Guid GetGuidFromUserId(int userId) => GetUserFieldValues(userId, SchemaMap.kiIndex_ZuneMediaID, Guid.Empty) is Guid userFieldValues ? userFieldValues : Guid.Empty;

        public static int GetUserIdFromPassportId(string passportId)
        {
            int userId = 0;
            if (!string.IsNullOrEmpty(passportId) && new HRESULT(UserManager.Instance.FindUserByPassportId(passportId, out userId)).IsError)
                userId = 0;
            return userId;
        }

        public static string GetZuneTagFromPassportId(string passportId) => GetZuneTagFromUserId(GetUserIdFromPassportId(passportId));

        public static string GetZuneTagFromUserId(int userId) => GetUserFieldValues(userId, SchemaMap.kiIndex_ZuneTag, string.Empty) as string;

        public static string GetImagePathFromPassportId(string passportId) => GetImagePathFromUserId(GetUserIdFromPassportId(passportId));

        public static string GetImagePathFromUserId(int userId)
        {
            string str = GetUserFieldValues(userId, SchemaMap.kiIndex_ArtUrl, string.Empty) as string;
            if (!string.IsNullOrEmpty(str) && !str.StartsWith("file://"))
                str = "file://" + str;
            return str;
        }

        public static Image GetImageFromPassportId(string passportId) => GetImageFromUserId(GetUserIdFromPassportId(passportId));

        public static Image GetImageFromUserId(int userId)
        {
            Image image = null;
            string imagePathFromUserId = GetImagePathFromUserId(userId);
            if (!string.IsNullOrEmpty(imagePathFromUserId))
            {
                bool antialiasEdges = Application.RenderingType != RenderingType.GDI;
                image = new Image(imagePathFromUserId, ProfileImage.DefaultTileSize.Width, ProfileImage.DefaultTileSize.Height, false, antialiasEdges);
            }
            return image;
        }

        public static void ErrorMessageRegionInvalid() => ErrorDialogInfo.Show(HRESULT._NS_E_SIGNIN_INVALID_REGION.Int, Shell.LoadString(StringId.IDS_SIGNIN_REGION_INVALID_TITLE), Shell.LoadString(StringId.IDS_SIGNIN_REGION_INVALID_MESSAGE));

        internal void SetError(HRESULT hrError)
        {
            ErrorMapperResult descriptionAndUrl = ErrorMapperApi.GetMappedErrorDescriptionAndUrl(hrError.Int, eErrorCondition.eEC_SignIn);
            this.SignInTermsOfServiceErrorForChild = HRESULT._ZUNE_E_SIGNIN_TERMS_OF_SERVICE_CHILD == descriptionAndUrl.Hr;
            this.SignInTermsOfServiceError = HRESULT._NS_E_SIGNIN_TERMS_OF_SERVICE == descriptionAndUrl.Hr;
            this.SignInRegionMismatchError = HRESULT._NS_E_SIGNIN_INVALID_REGION == descriptionAndUrl.Hr;
            this.SignInHttpGoneError = HRESULT._NS_E_SIGNIN_HTTP_GONE == descriptionAndUrl.Hr;
            this.SignInCredentialsError = HRESULT._NS_E_SERVER_ACCESSDENIED == descriptionAndUrl.Hr || HRESULT._NS_E_PASSPORT_LOGIN_FAILED == descriptionAndUrl.Hr || HRESULT._NS_E_SUBSCRIPTIONSERVICE_LOGIN_FAILED == descriptionAndUrl.Hr || HRESULT._NS_E_INVALID_USERNAME_AND_PASSWORD == descriptionAndUrl.Hr;
            this.SignInNoZuneAccountError = HRESULT._ZEST_E_UNAUTHENTICATED == descriptionAndUrl.Hr;
            this.SignInError = descriptionAndUrl.Hr;
            this.SignInErrorMessage = this.SignInTermsOfServiceError || this.SignInTermsOfServiceErrorForChild ? null : descriptionAndUrl.Description;
            this.SignInErrorWebHelpUrl = this.SignInTermsOfServiceError || this.SignInTermsOfServiceErrorForChild ? null : descriptionAndUrl.WebHelpUrl;
            this.SigningIn = false;
            if (this.SignInStatusUpdatedEvent == null)
                return;
            this.SignInStatusUpdatedEvent(this, EventArgs.Empty);
        }

        internal void UpdateState()
        {
            this.SubscriptionMachineCountExceeded = ZuneApplication.Service.IsSignedInWithSubscription() && !ZuneApplication.Service.CanDownloadSubscriptionContent();
            this.SubscriptionBillingViolation = ZuneApplication.Service.IsSignedInWithSubscription() && ZuneApplication.Service.HasSignInBillingViolation();
            this.ShowLabelTakedownWarning = ZuneApplication.Service.HasSignInLabelTakedown();
            this.SignedIn = ZuneApplication.Service.IsSignedIn();
            this.SigningIn = !this.SignedIn && ZuneApplication.Service.IsSigningIn();
            this.SignedInUsername = ZuneApplication.Service.GetSignedInUsername();
            this.SignedInGeoId = ZuneApplication.Service.GetSignedInGeoId();
            this.SignInErrorMessage = null;
            this.SignInErrorWebHelpUrl = null;
            this.SignInError = HRESULT._S_OK;
            this.SignInTermsOfServiceError = false;
            this.SignInTermsOfServiceErrorForChild = false;
            this.SignInRegionMismatchError = false;
            this.SignInHttpGoneError = false;
            this.SignInCredentialsError = false;
            this.SignInNoZuneAccountError = false;
            this.UserGuid = (Guid)ZuneApplication.Service.GetUserGuid();
            this.UpdatePointsBalance();
            this.UpdateSubscriptionFreeTrackBalance();
            this.UpdateSubscriptionFreeTrackExpiration();
            this.FamilySettings = null;
            this.LastSignedInUsername = null;
            this.CountryCode = null;
            if (this.SignedIn)
            {
                int iUserId;
                Guid guidUserGuid;
                ZuneApplication.Service.GetLastSignedInUserGuid(out iUserId, out guidUserGuid);
                this.LastSignedInUserId = iUserId;
                this.LastSignedInUserGuid = guidUserGuid;
                this.ZuneTag = ZuneApplication.Service.GetZuneTag();
                this.IsParentallyControlled = ZuneApplication.Service.IsParentallyControlled();
                this.IsLightWeight = ZuneApplication.Service.IsLightWeight();
                string locale = ZuneApplication.Service.GetLocale();
                if (!string.IsNullOrEmpty(locale))
                {
                    string[] strArray = locale.Split('-');
                    if (strArray.Length >= 2)
                        this.CountryCode = strArray[1];
                }
                this.PersistedUsernames = null;
                if (this.TunerHandler.CanQueryTunerList())
                    this.TunerHandler.RefreshTunerList();
            }
            else
            {
                this.ZuneTag = GetZuneTagFromUserId(this.LastSignedInUserId);
                this.IsParentallyControlled = false;
                this.IsLightWeight = false;
                this.MaxRegisteredPhoneVersion = null;
                this.MaxConnectedPhoneVersion = this.GetConnectedPhoneFirmwareVersion();
                this.UpdateMaxWindowsPhoneVersion();
            }
            this.UpdateSubscriptionState();
            this.UpdateStateAsyncStart();
            if (this.SignInStatusUpdatedEvent != null)
                this.SignInStatusUpdatedEvent(this, EventArgs.Empty);
            this.Initialized = true;
        }

        private void UpdateStateAsyncStart() => ThreadPool.QueueUserWorkItem(new WaitCallback(this.UpdateStateAsync), new object[3]
        {
       m_zuneTile,
       m_zuneTilePath,
       LastSignedInUserId
        });

        private void UpdateStateAsync(object args)
        {
            object[] objArray = (object[])args;
            Image image1 = objArray[0] as Image;
            string str = objArray[1] as string;
            string imagePathFromUserId = GetImagePathFromUserId((int)objArray[2]);
            Image image2 = image1;
            if (imagePathFromUserId != str)
            {
                if (!string.IsNullOrEmpty(imagePathFromUserId))
                {
                    bool antialiasEdges = Application.RenderingType != RenderingType.GDI;
                    image2 = new Image(imagePathFromUserId, ProfileImage.DefaultTileSize.Width, ProfileImage.DefaultTileSize.Height, false, antialiasEdges);
                }
                else
                    image2 = null;
            }
            objArray[0] = image2;
            objArray[1] = imagePathFromUserId;
            Application.DeferredInvoke(new DeferredInvokeHandler(this.UpdateStateAsyncEnd), objArray, DeferredInvokePriority.Low);
        }

        private void UpdateStateAsyncEnd(object args)
        {
            object[] objArray = (object[])args;
            this.ZuneTile = objArray[0] as Image;
            this.m_zuneTilePath = objArray[1] as string;
        }

        private void UpdateSubscriptionFreeTrackExpiration()
        {
            bool flag1 = false;
            bool flag2 = false;
            DateTime dateTime1 = DateTime.MaxValue;
            DateTime maxValue1 = DateTime.MaxValue;
            DateTime maxValue2 = DateTime.MaxValue;
            if (this.SignedIn)
            {
                dateTime1 = ZuneApplication.Service.GetSubscriptionFreeTrackExpiration();
                DateTime dateTime2 = !(dateTime1 >= DateTime.MinValue.Add(s_subscriptionFreeTrackFirstExpireWarning)) ? DateTime.MinValue : dateTime1.Subtract(s_subscriptionFreeTrackFirstExpireWarning);
                DateTime dateTime3 = !(dateTime1 >= DateTime.MinValue.Add(s_subscriptionFreeTrackSecondExpireWarning)) ? DateTime.MinValue : dateTime1.Subtract(s_subscriptionFreeTrackSecondExpireWarning);
                flag1 = dateTime2 <= DateTime.UtcNow;
                flag2 = dateTime3 <= DateTime.UtcNow;
            }
            this.SubscriptionFreeTrackExpiration = dateTime1;
            this.SubscriptionFreeTrackExpiring = flag1;
            this.SubscriptionFreeTrackExpiringWithinDays = flag2;
        }

        private void UpdateSubscriptionState()
        {
            bool flag1 = false;
            bool flag2 = false;
            DateTime dateTime = DateTime.MaxValue;
            this.SignedInWithSubscription = ZuneApplication.Service.IsSignedInWithSubscription();
            if (this.SignedIn)
            {
                this.LastSignedInUserHadActiveSubscription = this.SignedInWithSubscription;
                this.SubscriptionEndDate = ZuneApplication.Service.GetSubscriptionEndDate();
                this.SubscriptionId = ZuneApplication.Service.GetSubscriptionOfferId();
                this.SubscriptionRenewalId = ZuneApplication.Service.GetSubscriptionRenewalOfferId();
                if (ZuneApplication.Service.SubscriptionPendingCancel() || this.SubscriptionRenewalId == 0UL)
                {
                    dateTime = !(this.SubscriptionEndDate >= DateTime.MinValue.Add(s_subscriptionEndingWarning)) ? DateTime.MinValue : this.SubscriptionEndDate.Subtract(s_subscriptionEndingWarning);
                    flag1 = !this.SignedInWithSubscription && this.SubscriptionHasEndDate && this.SubscriptionEndDate <= DateTime.Today;
                    flag2 = this.SignedInWithSubscription && this.SubscriptionHasEndDate && this.SubscriptionEndDate >= DateTime.Today;
                }
            }
            else
            {
                bool activeSubscription;
                ulong subscriptionId;
                ZuneApplication.Service.GetLastSignedInUserSubscriptionState(out activeSubscription, out subscriptionId);
                this.LastSignedInUserHadActiveSubscription = activeSubscription;
                this.SubscriptionId = subscriptionId;
                this.SubscriptionEndDate = DateTime.MaxValue;
                this.SubscriptionRenewalId = 0UL;
            }
            this.SubscriptionExpired = flag1;
            this.SubscriptionExpiring = flag2;
            this.SubscriptionTrialsAvailable = !this.SignedInWithSubscription && this.SubscriptionId == 0UL && FeatureEnablement.IsFeatureEnabled(Features.eSubscriptionTrial);
            this.SubscriptionAvailable = !this.SignedInWithSubscription && FeatureEnablement.IsFeatureEnabled(Features.eSubscription);
            if (!this.SubscriptionExpired && (!this.SubscriptionExpiring || !(dateTime <= DateTime.Today)))
                return;
            SubscriptionEndingDialog.Show(this.SubscriptionEndDate);
        }

        public void UpdateUserTile()
        {
            if (this.LastSignedInUserId <= 0)
                return;
            string imagePathFromUserId = GetImagePathFromUserId(this.LastSignedInUserId);
            if (!string.IsNullOrEmpty(imagePathFromUserId))
            {
                bool antialiasEdges = Application.RenderingType != RenderingType.GDI;
                Image.RemoveCache(imagePathFromUserId, ProfileImage.DefaultTileSize.Height, ProfileImage.DefaultTileSize.Width, false, antialiasEdges);
            }
            UserManager.Instance.RefreshUserTile(this.LastSignedInUserId);
        }

        public void OnManualSignIn(HRESULT hr)
        {
            if (hr.IsSuccess || hr == HRESULT._E_ABORT)
                Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredUpdateStatus), null);
            else
                Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredSetError), hr);
        }

        public void OnAutomaticSignIn(HRESULT hr) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredUpdateStatus), null);

        private void DeferredUpdateStatus(object args) => this.UpdateState();

        private void DeferredSetError(object args) => this.SetError((HRESULT)args);

        internal void Phase3Init()
        {
            if (this.AutomaticSignIn())
                return;
            Application.DeferredInvoke(delegate
           {
               this.UpdateState();
           }, DeferredInvokePriority.Low);
        }

        private bool AutomaticSignIn()
        {
            bool flag = false;
            string atStartupUsername = ZuneApplication.Service.GetSignInAtStartupUsername();
            if (atStartupUsername != null)
            {
                flag = true;
                ZuneApplication.Service.SignIn(atStartupUsername, "", true, true, true, new AsyncCompleteHandler(this.OnAutomaticSignIn));
            }
            return flag;
        }

        private SignIn()
        {
            this.m_iLastSignedInUserId = 0;
            this.m_lastSignedInUserGuid = Guid.Empty;
        }

        private void GetLastSignedIdUser()
        {
            if (!(this.m_lastSignedInUserGuid == Guid.Empty) && this.m_iLastSignedInUserId != 0)
                return;
            ZuneApplication.Service.GetLastSignedInUserGuid(out this.m_iLastSignedInUserId, out this.m_lastSignedInUserGuid);
        }

        private void ClearLastSignedIdUser()
        {
            if (!ZuneApplication.Service.ClearLastSignedInUser())
                return;
            this.m_lastSignedInUserGuid = Guid.Empty;
            this.m_iLastSignedInUserId = 0;
        }
    }
}
