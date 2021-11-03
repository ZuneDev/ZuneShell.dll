// Decompiled with JetBrains decompiler
// Type: ZuneUI.WirelessSync
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using MicrosoftZuneLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UIXControls;

namespace ZuneUI
{
    public class WirelessSync : ModelItem
    {
        private const int NS_E_INVALID_TRANSCODE_CACHE_SIZE = -1072876833;
        private const uint NS_E_WLAN_PROFILE_START = 3222082080;
        private const uint NS_E_WLAN_PROFILE_END = 3222082085;
        private static WirelessSync _singletonInstance;
        private Choice _existingNetworkChoice;
        private Choice wirelessNetworkTypesChoice;
        private AOSetProfile wirelessSetProfileHelper = new AOSetProfile();
        private AOGetNetworkList wirelessGetNetworkListHelper = new AOGetNetworkList();
        private AOGetConnectedNetwork wirelessGetConnectedNetworkHelper = new AOGetConnectedNetwork();
        private AOClearWirelessSettings wirelessClearHelper = new AOClearWirelessSettings();
        private AOGetDeviceProfile wirelessDeviceProfileHelper = new AOGetDeviceProfile();
        private string wirelessDeviceErrorDescription;
        private string wirelessDeviceErrorCaption;
        private HRESULT wirelessDeviceErrorCode = HRESULT._S_OK;
        private bool wirelessDeviceCanceled;
        private Category wirelessBlockedPage;
        private WlanProfile wirelessDeviceProfile;
        private WlanProfile wirelessConnectedProfile;
        private WlanProfile wirelessProfileToSave;
        private ArrayListDataSet wirelessNetworksList;
        private HRESULT wirelessGetConnectedProfileResult = HRESULT._S_OK;
        private HRESULT _profileSavedResult = HRESULT._E_PENDING;
        private static readonly WirelessType[] wirelessRadioGroupItems = new WirelessType[11]
        {
      new WirelessType(new WlanAuthCipherPair(WirelessAuthenticationTypes.Open, WirelessCiphers.None), Shell.LoadString(StringId.IDS_WIRELESS_NETWORK_NO_ENCRYPTION), true, true, true),
      new WirelessType(new WlanAuthCipherPair(WirelessAuthenticationTypes.Open, WirelessCiphers.WEP), Shell.LoadString(StringId.IDS_WIRELESS_NETWORK_WEP_OPEN), true, false, true),
      new WirelessType(new WlanAuthCipherPair(WirelessAuthenticationTypes.Open, WirelessCiphers.WEP40), Shell.LoadString(StringId.IDS_WIRELESS_NETWORK_WEP_OPEN), false, false, true),
      new WirelessType(new WlanAuthCipherPair(WirelessAuthenticationTypes.Open, WirelessCiphers.WEP104), Shell.LoadString(StringId.IDS_WIRELESS_NETWORK_WEP_OPEN), false, false, true),
      new WirelessType(new WlanAuthCipherPair(WirelessAuthenticationTypes.Shared, WirelessCiphers.WEP), Shell.LoadString(StringId.IDS_WIRELESS_NETWORK_WEP_SHARED), true, false, false),
      new WirelessType(new WlanAuthCipherPair(WirelessAuthenticationTypes.Shared, WirelessCiphers.WEP40), Shell.LoadString(StringId.IDS_WIRELESS_NETWORK_WEP_SHARED), false, false, true),
      new WirelessType(new WlanAuthCipherPair(WirelessAuthenticationTypes.Shared, WirelessCiphers.WEP104), Shell.LoadString(StringId.IDS_WIRELESS_NETWORK_WEP_SHARED), false, false, true),
      new WirelessType(new WlanAuthCipherPair(WirelessAuthenticationTypes.WPAPSK, WirelessCiphers.AES), Shell.LoadString(StringId.IDS_WIRELESS_NETWORK_WPA_AES), true, false, true),
      new WirelessType(new WlanAuthCipherPair(WirelessAuthenticationTypes.WPAPSK, WirelessCiphers.TKIP), Shell.LoadString(StringId.IDS_WIRELESS_NETWORK_WPA_TKIP), true, false, false),
      new WirelessType(new WlanAuthCipherPair(WirelessAuthenticationTypes.WPA2PSK, WirelessCiphers.AES), Shell.LoadString(StringId.IDS_WIRELESS_NETWORK_WPA2_AES), true, false, true),
      new WirelessType(new WlanAuthCipherPair(WirelessAuthenticationTypes.WPA2PSK, WirelessCiphers.TKIP), Shell.LoadString(StringId.IDS_WIRELESS_NETWORK_WPA2_TKIP), true, false, false)
        };

        public static WirelessSync Instance
        {
            get
            {
                if (_singletonInstance == null)
                    _singletonInstance = new WirelessSync();
                return _singletonInstance;
            }
            set
            {
                if (_singletonInstance == value)
                    return;
                _singletonInstance = value;
            }
        }

        private UIDevice ActiveDevice => SyncControls.Instance.CurrentDeviceOverride;

        private WirelessSync()
          : base(ZuneShell.DefaultInstance.Management.DeviceManagement)
        {
        }

        public Choice ExistingNetworkChoice
        {
            get
            {
                if (this._existingNetworkChoice == null)
                {
                    this._existingNetworkChoice = new Choice(this);
                    this._existingNetworkChoice.Options = (new Command[2]
                    {
            new Command( this, Shell.LoadString(StringId.IDS_WIRELESS_USE_CONNECTED_YES),  null),
            new Command( this, Shell.LoadString(StringId.IDS_WIRELESS_USE_CONNECTED_NO),  null)
                    });
                    this._existingNetworkChoice.ChosenChanged += (sender, args) => this.FirePropertyChanged(nameof(ExistingNetworkChoice));
                }
                return this._existingNetworkChoice;
            }
        }

        public WlanProfile WirelessDeviceProfile
        {
            get
            {
                if (this.wirelessDeviceProfile == null)
                {
                    this.wirelessDeviceProfile = new WlanProfile();
                    this.wirelessDeviceProfile.SSID = string.Empty;
                    this.wirelessDeviceProfile.Key = string.Empty;
                    this.wirelessDeviceProfile.Auth = WirelessAuthenticationTypes.Open;
                    this.wirelessDeviceProfile.Cipher = WirelessCiphers.None;
                }
                return this.wirelessDeviceProfile;
            }
            private set
            {
                this.wirelessDeviceProfile = value;
                this.FirePropertyChanged(nameof(WirelessDeviceProfile));
            }
        }

        public string GetWirelessDeviceNetworkType()
        {
            WlanProfile wirelessDeviceProfile = this.WirelessDeviceProfile;
            string str = Shell.LoadString(StringId.IDS_TYPE_UNKNOWN);
            foreach (WirelessType wirelessRadioGroupItem in wirelessRadioGroupItems)
            {
                if (wirelessDeviceProfile.Auth == wirelessRadioGroupItem.Type.Auth && wirelessDeviceProfile.Cipher == wirelessRadioGroupItem.Type.Cipher)
                    str = wirelessRadioGroupItem.Description;
            }
            return str;
        }

        public int GetNetworkTypeIndex(WirelessAuthenticationTypes auth, WirelessCiphers cipher)
        {
            int num = 0;
            if (this.wirelessNetworkTypesChoice != null && this.wirelessNetworkTypesChoice.Options != null)
            {
                for (int index = 0; index < this.wirelessNetworkTypesChoice.Options.Count; ++index)
                {
                    WirelessNetworkTypeCommand option = this.wirelessNetworkTypesChoice.Options[index] as WirelessNetworkTypeCommand;
                    WlanAuthCipherPair wlanAuthCipherPair = null;
                    if (option != null)
                        wlanAuthCipherPair = option.NetworkType;
                    if (wlanAuthCipherPair != null && wlanAuthCipherPair.Auth == auth && (wlanAuthCipherPair.Cipher == cipher || this.IsWEP(wlanAuthCipherPair.Cipher) && this.IsWEP(cipher)))
                    {
                        num = index;
                        break;
                    }
                }
            }
            return num;
        }

        public WirelessStateResults RequestWirelessNetworksList()
        {
            this.wirelessDeviceErrorDescription = null;
            this.wirelessDeviceErrorCaption = Shell.LoadString(StringId.IDS_WIRELESS_SNIFF_FAILED);
            this.wirelessDeviceErrorCode = HRESULT._S_OK;
            WirelessStateResults result = this.wirelessGetNetworkListHelper.StartOperation(this.ActiveDevice, new AsyncOperation.AOComplete(this.GetNetworkListDone));
            this.WirelessHandleDeviceBusy(result);
            return result;
        }

        public ArrayListDataSet WirelessNetworksList
        {
            get
            {
                if (this.wirelessNetworksList == null)
                    this.wirelessNetworksList = new ArrayListDataSet();
                return this.wirelessNetworksList;
            }
            private set
            {
                if (value == this.wirelessNetworksList)
                    return;
                this.wirelessNetworksList = value;
                this.FirePropertyChanged(nameof(WirelessNetworksList));
            }
        }

        public void GetNetworkListDone(bool success)
        {
            if (this.WirelessUnblockPage())
                return;
            WlanProfileList networkList = this.wirelessGetNetworkListHelper.NetworkList;
            ArrayListDataSet arrayListDataSet = new ArrayListDataSet();
            if (success && networkList != null)
            {
                foreach (WlanProfile profile in networkList)
                    arrayListDataSet.Add(new WlanCommand(profile));
            }
            if (!success)
            {
                if (string.IsNullOrEmpty(this.wirelessDeviceErrorDescription))
                    this.wirelessDeviceErrorDescription = this.wirelessGetNetworkListHelper.Error;
                if (this.wirelessDeviceErrorCode == HRESULT._S_OK)
                    this.wirelessDeviceErrorCode = this.wirelessGetNetworkListHelper.Hr;
            }
            this.WirelessNetworksList = arrayListDataSet;
        }

        public bool SetConnectedNetwork() => this.SetWirelessSettings(this.wirelessConnectedProfile);

        public WirelessStateResults SetWirelessSettings()
        {
            WirelessStateResults result = WirelessStateResults.Error;
            WlanProfile wirelessProfileToSave = this.wirelessProfileToSave;
            this.wirelessDeviceErrorDescription = null;
            this.wirelessDeviceErrorCaption = Shell.LoadString(StringId.IDS_WIRELESS_SYNC_POST_SETUP_FAILED);
            this.wirelessDeviceErrorCode = HRESULT._S_OK;
            this.wirelessDeviceCanceled = false;
            if (wirelessProfileToSave != null)
            {
                result = this.wirelessSetProfileHelper.StartOperation(this.ActiveDevice, new AsyncOperation.AOComplete(this.SetProfileDone), wirelessProfileToSave);
                this.WirelessHandleDeviceBusy(result);
            }
            if (result != WirelessStateResults.Success)
                this.wirelessDeviceErrorDescription = wirelessProfileToSave == null ? Shell.LoadString(StringId.IDS_WIRELESS_SYNC_SETUP_FAILED_GENERIC) : string.Format(Shell.LoadString(StringId.IDS_WIRELESS_SYNC_SETUP_FAILED), wirelessProfileToSave.SSID);
            return result;
        }

        public bool SetWirelessSettings(object selected)
        {
            if (selected is WlanCommand wlanCommand)
                return this.SetWirelessSettings(wlanCommand.Profile);
            this.wirelessDeviceErrorDescription = Shell.LoadString(StringId.IDS_WIRELESS_SYNC_SETUP_FAILED_GENERIC);
            return false;
        }

        public WlanCommand CreateWlanCommand(string name, object networkType, string key)
        {
            WlanProfile wlanProfile = this.CreateWlanProfile(name, networkType, key);
            return wlanProfile != null ? new WlanCommand(wlanProfile) : null;
        }

        public void ClearWirelessOnDevice() => this.RequestClearWirelessOnDevice(new AsyncOperation.AOComplete(this.ClearWirelessOnDeviceDone), true);

        public void ClearWirelessOnDeviceDone(bool success)
        {
            if (this.WirelessUnblockPage() || success)
                return;
            if (string.IsNullOrEmpty(this.wirelessDeviceErrorDescription))
                this.wirelessDeviceErrorDescription = this.wirelessClearHelper.Error;
            if (!(this.wirelessDeviceErrorCode == HRESULT._S_OK))
                return;
            this.wirelessDeviceErrorCode = this.wirelessClearHelper.Hr;
        }

        public void ClearWirelessOnDeviceForForget()
        {
            if (this.ActiveDevice.IsConnectedToClient && !this.ActiveDevice.IsGuest)
            {
                this.RequestClearWirelessOnDevice(new AsyncOperation.AOComplete(this.ClearWirelessOnDeviceForForgetDone), true);
            }
            else
            {
                string uuid = null;
                HRESULT hresult = this.ActiveDevice.GetDisconnectedWiFiUUID(ref uuid);
                if (hresult.IsError)
                    this.ClearWirelessOnDeviceForForgetDone(hresult.IsSuccess);
                else if (!string.IsNullOrEmpty(uuid))
                {
                    hresult = this.ActiveDevice.UnassociateWiFiUUID(uuid);
                    this.ClearWirelessOnDeviceForForgetDone(hresult.IsSuccess);
                }
                else
                    this.ClearWirelessOnDeviceForForgetDone(true);
            }
        }

        public void ClearWirelessOnDeviceForForgetDone(bool success)
        {
            if (success)
                SyncControls.Instance.DeleteCurrentDeviceWorker();
            else
                MessageBox.Show(Shell.LoadString(StringId.IDS_WIRELESS_CLEAR_UUID_FAILED_TITLE), Shell.LoadString(StringId.IDS_WIRELESS_CLEAR_UUID_FAILED), null);
            this.WirelessUnblockPage();
        }

        private void RequestClearWirelessOnDevice(
          AsyncOperation.AOComplete completeFunc,
          bool fIgnoreErrors)
        {
            this.wirelessDeviceErrorDescription = null;
            this.wirelessDeviceErrorCaption = Shell.LoadString(StringId.IDS_WIRELESS_SYNC_PRE_SETUP_FAILED);
            this.wirelessDeviceErrorCode = HRESULT._S_OK;
            WirelessStateResults wirelessStateResults;
            do
            {
                wirelessStateResults = this.wirelessClearHelper.StartOperation(this.ActiveDevice, completeFunc, fIgnoreErrors);
                if (wirelessStateResults == WirelessStateResults.NotAvailable)
                    Thread.Sleep(200);
            }
            while (wirelessStateResults == WirelessStateResults.NotAvailable);
        }

        public void SetProfileDone(bool success)
        {
            if (this.WirelessUnblockPage())
                return;
            if (success)
                this.WirelessDeviceProfile = this.wirelessProfileToSave;
            this._profileSavedResult = HRESULT._E_UNEXPECTED;
            if (success && this.wirelessSetProfileHelper.WlanTestSucceeded)
            {
                this.ProfileSavedResult = HRESULT._S_OK;
            }
            else
            {
                if (string.IsNullOrEmpty(this.wirelessDeviceErrorDescription))
                    this.wirelessDeviceErrorDescription = this.wirelessSetProfileHelper.Error;
                if (this.wirelessDeviceErrorCode == HRESULT._S_OK)
                    this.wirelessDeviceErrorCode = this.wirelessSetProfileHelper.Hr;
                this.wirelessDeviceCanceled = this.wirelessSetProfileHelper.Canceled;
                this.ProfileSavedResult = this.wirelessSetProfileHelper.Hr;
            }
        }

        public HRESULT ProfileSavedResult
        {
            get => this._profileSavedResult;
            set
            {
                if (!(this._profileSavedResult != value))
                    return;
                this._profileSavedResult = value;
                this.FirePropertyChanged(nameof(ProfileSavedResult));
            }
        }

        public string WirelessDeviceErrorDescription => this.wirelessDeviceErrorDescription;

        public bool WirelessDeviceShowError => this.wirelessDeviceErrorCode != HRESULT._S_OK;

        public bool WirelessDeviceShowErrorNow => (uint)this.wirelessDeviceErrorCode.Int >= 3222082080U && (uint)this.wirelessDeviceErrorCode.Int <= 3222082085U;

        public void WirelessDeviceShowErrorDialog()
        {
            if (!this.WirelessDeviceShowError)
                return;
            if (string.IsNullOrEmpty(this.wirelessDeviceErrorCaption))
                Shell.ShowErrorDialog(this.wirelessDeviceErrorCode.Int, StringId.IDS_WIRELESS_SYNC_GENERIC_SETUP_FAILED);
            else
                ErrorDialogInfo.Show(this.wirelessDeviceErrorCode.Int, this.wirelessDeviceErrorCaption);
        }

        public bool WirelessDeviceCanceled => this.wirelessDeviceCanceled;

        public void CancelSetWirelessSettings() => this.wirelessSetProfileHelper.Cancel();

        public Choice WirelessNetworkTypes
        {
            get
            {
                WlanAuthCipherPairList list = new WlanAuthCipherPairList();
                IList<WirelessNetworkTypeCommand> networkTypeCommandList = new List<WirelessNetworkTypeCommand>();
                if (this.ActiveDevice.IsConnectedToClient)
                    this.ActiveDevice.GetWiFiAuthorizationCipherList(ref list);
                foreach (WirelessType wirelessRadioGroupItem in wirelessRadioGroupItems)
                {
                    if (wirelessRadioGroupItem.DisplayType && (wirelessRadioGroupItem.AlwaysSupported || this.DeviceSupportsType(list, wirelessRadioGroupItem.Type)))
                    {
                        WirelessNetworkTypeCommand networkTypeCommand = new WirelessNetworkTypeCommand(this, wirelessRadioGroupItem.Description, null, wirelessRadioGroupItem.Type);
                        networkTypeCommandList.Add(networkTypeCommand);
                    }
                }
                this.wirelessNetworkTypesChoice = new Choice(this);
                this.wirelessNetworkTypesChoice.Options = (IList)networkTypeCommandList;
                return this.wirelessNetworkTypesChoice;
            }
        }

        public WirelessStateResults RequestDeviceWirelessProfile()
        {
            this.wirelessDeviceErrorDescription = null;
            this.wirelessDeviceErrorCaption = Shell.LoadString(StringId.IDS_WIRELESS_SYNC_WLAN_UUID_FAILED);
            this.wirelessDeviceErrorCode = HRESULT._S_OK;
            WirelessStateResults result = this.wirelessDeviceProfileHelper.StartOperation(this.ActiveDevice, new AsyncOperation.AOComplete(this.GetDeviceWirelessProfileDone));
            this.WirelessHandleDeviceBusy(result);
            return result;
        }

        public void GetDeviceWirelessProfileDone(bool success)
        {
            if (this.WirelessUnblockPage())
                return;
            if (success)
            {
                this.WirelessDeviceProfile = this.wirelessDeviceProfileHelper.DeviceProfile;
            }
            else
            {
                this.WirelessDeviceProfile = null;
                if (string.IsNullOrEmpty(this.wirelessDeviceErrorDescription))
                    this.wirelessDeviceErrorDescription = this.wirelessDeviceProfileHelper.Error;
                if (!(this.wirelessDeviceErrorCode == HRESULT._S_OK))
                    return;
                this.wirelessDeviceErrorCode = this.wirelessDeviceProfileHelper.Hr;
            }
        }

        public WirelessStateResults RequestConnectedWirelessNetwork()
        {
            this.wirelessDeviceErrorDescription = null;
            this.wirelessDeviceErrorCaption = Shell.LoadString(StringId.IDS_WIRELESS_SYNC_PRE_SETUP_FAILED);
            this.wirelessDeviceErrorCode = HRESULT._S_OK;
            this.wirelessGetConnectedProfileResult = HRESULT._S_OK;
            WirelessStateResults result = this.wirelessGetConnectedNetworkHelper.StartOperation(this.ActiveDevice, new AsyncOperation.AOComplete(this.GetConnectedNetworkDone));
            this.WirelessHandleDeviceBusy(result);
            return result;
        }

        public string ConnectedWirelessNetwork => this.wirelessConnectedProfile != null ? this.wirelessConnectedProfile.SSID : null;

        public void GetConnectedNetworkDone(bool success)
        {
            if (this.WirelessUnblockPage())
                return;
            WlanProfile connectedNetwork = this.wirelessGetConnectedNetworkHelper.ConnectedNetwork;
            if (success)
            {
                this.wirelessConnectedProfile = connectedNetwork;
            }
            else
            {
                this.wirelessGetConnectedProfileResult = this.wirelessGetConnectedNetworkHelper.Hr;
                this.wirelessConnectedProfile = null;
                if (string.IsNullOrEmpty(this.wirelessDeviceErrorDescription))
                    this.wirelessDeviceErrorDescription = this.wirelessGetConnectedNetworkHelper.Error;
                if (this.wirelessDeviceErrorCode == HRESULT._S_OK)
                    this.wirelessDeviceErrorCode = this.wirelessGetConnectedNetworkHelper.Hr;
            }
            this.FirePropertyChanged("ConnectedWirelessNetwork");
        }

        public bool WirelessGetConnectedProfileFailed => this.wirelessGetConnectedProfileResult != HRESULT._S_OK;

        public string WirelessGetConnectedProfileResult => this.wirelessGetConnectedProfileResult.Int.ToString("X");

        private bool SetWirelessSettings(WlanProfile profile)
        {
            if (profile == null)
                return false;
            this.wirelessProfileToSave = profile;
            return true;
        }

        private WlanProfile CreateWlanProfile(string name, object networkType, string key)
        {
            WlanAuthCipherPairList list = new WlanAuthCipherPairList();
            WirelessNetworkTypeCommand networkTypeCommand = networkType as WirelessNetworkTypeCommand;
            bool flag = false;
            if (string.IsNullOrEmpty(name) || networkTypeCommand == null || (networkTypeCommand.NetworkType == null || this.ActiveDevice == null))
            {
                this.wirelessDeviceErrorDescription = Shell.LoadString(StringId.IDS_WIRELESS_SYNC_SETUP_FAILED_GENERIC);
                return null;
            }
            HRESULT authorizationCipherList = this.ActiveDevice.GetWiFiAuthorizationCipherList(ref list);
            if (authorizationCipherList.IsSuccess)
            {
                foreach (WlanAuthCipherPair wlanAuthCipherPair in list)
                {
                    if (wlanAuthCipherPair.Auth == networkTypeCommand.NetworkType.Auth && wlanAuthCipherPair.Cipher == networkTypeCommand.NetworkType.Cipher)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (!flag)
            {
                this.wirelessDeviceErrorDescription = string.Format(Shell.LoadString(StringId.IDS_WIRELESS_ERROR_UNSUPPORTED_AUTH), name);
                this.wirelessDeviceErrorCode = authorizationCipherList;
                return null;
            }
            if (string.IsNullOrEmpty(name))
            {
                this.wirelessDeviceErrorDescription = Shell.LoadString(StringId.IDS_WIRELESS_ERROR_INVALID_NAME);
                return null;
            }
            return new WlanProfile()
            {
                SSID = name,
                Auth = networkTypeCommand.NetworkType.Auth,
                Cipher = networkTypeCommand.NetworkType.Cipher,
                Key = key
            };
        }

        private bool IsWEP(WirelessCiphers cipher) => cipher == WirelessCiphers.WEP || cipher == WirelessCiphers.WEP40 || cipher == WirelessCiphers.WEP104;

        private bool IsWPA(WirelessAuthenticationTypes auth) => auth == WirelessAuthenticationTypes.WPA2PSK || auth == WirelessAuthenticationTypes.WPAPSK;

        private bool DeviceSupportsType(
          WlanAuthCipherPairList supportedList,
          WlanAuthCipherPair displayType)
        {
            foreach (WlanAuthCipherPair supported in supportedList)
            {
                if (displayType.Cipher == WirelessCiphers.WEP)
                {
                    if (supported.Auth == displayType.Auth && this.IsWEP(supported.Cipher))
                        return true;
                }
                else if (supported.Auth == displayType.Auth && supported.Cipher == displayType.Cipher)
                    return true;
            }
            return false;
        }

        public string SavingProfileName => this.wirelessSetProfileHelper.AttemptingName;

        private void WirelessHandleDeviceBusy(WirelessStateResults result)
        {
            if (result != WirelessStateResults.NotAvailable)
                return;
            this.wirelessBlockedPage = ZuneShell.DefaultInstance.Management.CurrentCategoryPage.CurrentCategory;
            Management.NavigateToCategory(SettingCategories.WirelessSetupDeviceBusy);
        }

        private bool WirelessUnblockPage()
        {
            if (this.wirelessBlockedPage == null)
                return false;
            Management.NavigateToCategory(this.wirelessBlockedPage);
            this.wirelessBlockedPage = null;
            return true;
        }
    }
}
