// Decompiled with JetBrains decompiler
// Type: ZuneUI.AOSetProfile
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;
using MicrosoftZuneLibrary;
using System.Threading;

namespace ZuneUI
{
    internal class AOSetProfile : AsyncOperation
    {
        private WirelessStates[] _setProfileStates = new WirelessStates[4]
        {
      WirelessStates.GetDeviceProfiles,
      WirelessStates.AssociateWlanDevice,
      WirelessStates.CommitProfileToDevice,
      WirelessStates.TestDeviceProfile
        };
        private WlanProfile _deviceProfile;
        private WlanProfile _attemptingProfile;
        private AutoResetEvent _restore = new AutoResetEvent(false);
        private string _attemptingName = string.Empty;
        private bool _fProfileSaved;
        private string _wlanTestResult;
        private HRESULT _wlanTestResultCode;

        public string AttemptingName => this._attemptingName;

        public bool WlanTestSucceeded => this.Finished && string.IsNullOrEmpty(this._wlanTestResult);

        public override void Cancel()
        {
            base.Cancel();
            if (this.Idle || this.Finished || this._setProfileStates[AsyncOperation._iCurrentState] != WirelessStates.TestDeviceProfile)
                return;
            this._device.CancelWiFiTest();
        }

        private void ResetState(WlanProfile profile)
        {
            if (!this.Idle && !this.Finished)
                return;
            this._attemptingName = profile.SSID;
            this._attemptingProfile = profile;
            this._deviceProfile = (WlanProfile)null;
            this._fProfileSaved = false;
            this._wlanTestResult = (string)null;
            this._wlanTestResultCode = HRESULT._S_OK;
            this._restore.Reset();
        }

        public WirelessStateResults StartOperation(
          UIDevice device,
          AsyncOperation.AOComplete completeFunc,
          WlanProfile newProfile)
        {
            if (newProfile == null)
                return WirelessStateResults.Error;
            this.ResetState(newProfile);
            return this.StartOperation(device, completeFunc, this._setProfileStates);
        }

        protected override WirelessStateResults DoStep(WirelessStates state)
        {
            HRESULT hr;
            switch (state)
            {
                case WirelessStates.CommitProfileToDevice:
                    WlanProfileList list = new WlanProfileList();
                    list.Add(this._attemptingProfile);
                    hr = this._device.SetWiFiProfileList(list);
                    if (hr.IsSuccess)
                    {
                        hr = this._device.SendWiFiProfiles();
                        break;
                    }
                    break;
                case WirelessStates.TestDeviceProfile:
                    hr = this._device.TestWiFi();
                    break;
                case WirelessStates.GetDeviceProfiles:
                    hr = this._device.ReceiveWiFiProfiles();
                    break;
                case WirelessStates.AssociateWlanDevice:
                    hr = this._device.AssociateWiFi();
                    break;
                default:
                    hr = HRESULT._E_ABORT;
                    break;
            }
            if (hr.IsSuccess)
                return WirelessStateResults.Success;
            this.SetResult(hr);
            return WirelessStateResults.Error;
        }

        protected override void EndOperation(WirelessStateResults result)
        {
            if (result != WirelessStateResults.Finished)
            {
                if (this._fProfileSaved)
                    this.RestoreProfile();
                if (result == WirelessStateResults.Canceled)
                {
                    this._error = Shell.LoadString(StringId.IDS_WIRELESS_SYNC_SETUP_CANCELED);
                    this._detailedError = (string)null;
                    this._hr = HRESULT._S_OK;
                }
                else if (this._attemptingProfile != null)
                    this._error = string.Format(Shell.LoadString(StringId.IDS_WIRELESS_SYNC_SETUP_FAILED), (object)this._attemptingProfile.SSID);
                else
                    this._error = Shell.LoadString(StringId.IDS_WIRELESS_SYNC_SETUP_FAILED_GENERIC);
            }
            else if (!string.IsNullOrEmpty(this._wlanTestResult))
            {
                this._error = string.Format(Shell.LoadString(StringId.IDS_WIRELESS_SYNC_TEST_FAILED), (object)this._attemptingProfile.SSID);
                this._detailedError = this._wlanTestResult;
                this._hr = this._wlanTestResultCode;
            }
            else
                SQMLog.Log(SQMDataId.DeviceWirelessSetup, 1);
        }

        protected override void AddListeners()
        {
            this._device.WiFiProfilesReceivedEvent += new FallibleEventHandler(this.Device_GetDeviceWlanProfilesCompleteEvent);
            this._device.WiFiTestCompletedEvent += new FallibleEventHandler(this.Device_TestDeviceWlanCompleteEvent);
            this._device.WiFiProfilesSentEvent += new FallibleEventHandler(this.Device_SetDeviceWlanProfilesCompleteEvent);
            this._device.WiFiAssociationCompletedEvent += new FallibleEventHandler(this.Device_AssociateWlanDeviceCompleteEvent);
        }

        protected override void RemoveListeners()
        {
            this._device.WiFiProfilesReceivedEvent -= new FallibleEventHandler(this.Device_GetDeviceWlanProfilesCompleteEvent);
            this._device.WiFiTestCompletedEvent -= new FallibleEventHandler(this.Device_TestDeviceWlanCompleteEvent);
            this._device.WiFiProfilesSentEvent -= new FallibleEventHandler(this.Device_SetDeviceWlanProfilesCompleteEvent);
            this._device.WiFiAssociationCompletedEvent -= new FallibleEventHandler(this.Device_AssociateWlanDeviceCompleteEvent);
        }

        private bool IsWirelessTestFailure(HRESULT hr) => hr == HRESULT._NS_E_MTPZ_WLAN_TEST_FAIL_NO_CONFIG || hr == HRESULT._NS_E_MTPZ_WLAN_TEST_FAIL_ASSOCIATE || (hr == HRESULT._NS_E_MTPZ_WLAN_TEST_FAIL_DHCP || hr == HRESULT._NS_E_MTPZ_WLAN_TEST_FAIL_TIMEOUT) || (hr == HRESULT._NS_E_MTPZ_WLAN_TEST_FAIL_INTERNAL || hr == HRESULT._NS_E_MTPZ_WLAN_TEST_RUNNING || hr == HRESULT._NS_E_MTPZ_WLAN_TEST_UNKNOWN);

        private void RestoreProfile()
        {
            WlanProfileList list = new WlanProfileList();
            this._device.WiFiProfilesSentEvent += new FallibleEventHandler(this.Device_RestoreDeviceWlanProfilesCompleteEvent);
            if (this._deviceProfile != null)
                list.Add(this._deviceProfile);
            if (this._device.SetWiFiProfileList(list).IsSuccess)
                this._device.SendWiFiProfiles();
            this._restore.WaitOne(5000, false);
            this._device.WiFiProfilesSentEvent -= new FallibleEventHandler(this.Device_RestoreDeviceWlanProfilesCompleteEvent);
        }

        private void Device_GetDeviceWlanProfilesCompleteEvent(object sender, FallibleEventArgs args)
        {
            HRESULT hr = args.HR;
            this.SetResult(hr);
            if (hr.IsSuccess)
            {
                WlanProfileList list = new WlanProfileList();
                hr = this._device.GetWiFiProfileList(ref list);
                this._deviceProfile = list.Count <= 0 ? (WlanProfile)null : list[0];
            }
            if (hr.IsSuccess)
                this.StepComplete(WirelessStateResults.Success);
            else
                this.StepComplete(WirelessStateResults.Error);
        }

        private void Device_AssociateWlanDeviceCompleteEvent(object sender, FallibleEventArgs args)
        {
            this.SetResult(args.HR);
            if (args.HR.IsSuccess)
                this.StepComplete(WirelessStateResults.Success);
            else
                this.StepComplete(WirelessStateResults.Error);
        }

        private void Device_SetDeviceWlanProfilesCompleteEvent(object sender, FallibleEventArgs args)
        {
            this.SetResult(args.HR);
            if (args.HR.IsSuccess && this._attemptingProfile != null)
            {
                this._fProfileSaved = true;
                this.StepComplete(WirelessStateResults.Success);
            }
            else
                this.StepComplete(WirelessStateResults.Error);
        }

        private void Device_RestoreDeviceWlanProfilesCompleteEvent(
          object sender,
          FallibleEventArgs args)
        {
            this._restore.Set();
        }

        private void Device_TestDeviceWlanCompleteEvent(object sender, FallibleEventArgs args)
        {
            HRESULT hr = args.HR;
            bool flag = HRESULT._NS_E_MTPZ_WLAN_TEST_FAIL_CANCELLED == hr;
            this.SetResult(hr);
            if (this.IsWirelessTestFailure(hr))
            {
                this._wlanTestResult = this._detailedError;
                this._wlanTestResultCode = this._hr;
                hr = HRESULT._S_OK;
                this.ClearResult();
            }
            if (hr.IsSuccess && this._attemptingProfile != null)
                this.StepComplete(WirelessStateResults.Success);
            else if (flag)
                this.StepComplete(WirelessStateResults.Canceled);
            else
                this.StepComplete(WirelessStateResults.Error);
        }
    }
}
