// Decompiled with JetBrains decompiler
// Type: ZuneUI.AOGetDeviceProfile
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using MicrosoftZuneLibrary;

namespace ZuneUI
{
    internal class AOGetDeviceProfile : AsyncOperation
    {
        private WirelessStates[] _getDeviceProfileStates = new WirelessStates[1]
        {
      WirelessStates.GetDeviceProfiles
        };
        private WlanProfile _deviceProfile;

        public WlanProfile DeviceProfile => this.Finished ? this._deviceProfile : (WlanProfile)null;

        public WirelessStateResults StartOperation(
          UIDevice device,
          AsyncOperation.AOComplete completeFunc)
        {
            this.ResetState();
            return this.StartOperation(device, completeFunc, this._getDeviceProfileStates);
        }

        protected override WirelessStateResults DoStep(WirelessStates state)
        {
            HRESULT hr = state != WirelessStates.GetDeviceProfiles ? HRESULT._E_ABORT : this._device.ReceiveWiFiProfiles();
            if (hr.IsSuccess)
                return WirelessStateResults.Success;
            this.SetResult(hr);
            return WirelessStateResults.Error;
        }

        protected override void AddListeners() => this._device.WiFiProfilesReceivedEvent += new FallibleEventHandler(this.Device_GetDeviceWlanProfilesCompleteEvent);

        protected override void RemoveListeners() => this._device.WiFiProfilesReceivedEvent -= new FallibleEventHandler(this.Device_GetDeviceWlanProfilesCompleteEvent);

        private void ResetState() => this._deviceProfile = (WlanProfile)null;

        private void Device_GetDeviceWlanProfilesCompleteEvent(object sender, FallibleEventArgs args)
        {
            WlanProfileList list = new WlanProfileList();
            HRESULT hr = args.HR;
            this.SetResult(hr);
            if (hr.IsSuccess)
            {
                hr = this._device.GetWiFiProfileList(ref list);
                if (hr.IsSuccess && list.Count > 0)
                    this._deviceProfile = list[0];
            }
            if (hr.IsSuccess)
                this.StepComplete(WirelessStateResults.Success);
            else
                this.StepComplete(WirelessStateResults.Error);
        }
    }
}
