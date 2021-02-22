// Decompiled with JetBrains decompiler
// Type: ZuneUI.AOGetNetworkList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using MicrosoftZuneLibrary;
using System.Collections.Generic;

namespace ZuneUI
{
    internal class AOGetNetworkList : AsyncOperation
    {
        private WlanProfileList _deviceNetworks;
        private WlanProfileList _computerNetworks;
        private WlanProfileList _networkList;
        private WirelessStates[] _getNetworkListStates = new WirelessStates[2]
        {
      WirelessStates.GetComputerProfiles,
      WirelessStates.SniffNetworks
        };

        public WlanProfileList NetworkList => this.Finished ? this._networkList : null;

        private void ResetState()
        {
            if (!this.Idle && !this.Finished)
                return;
            this._computerNetworks = new WlanProfileList();
            this._deviceNetworks = new WlanProfileList();
            this._networkList = new WlanProfileList();
        }

        public WirelessStateResults StartOperation(
          UIDevice device,
          AOComplete completeFunc)
        {
            this.ResetState();
            return this.StartOperation(device, completeFunc, this._getNetworkListStates);
        }

        protected override WirelessStateResults DoStep(WirelessStates state)
        {
            HRESULT hr;
            switch (state)
            {
                case WirelessStates.SniffNetworks:
                    hr = this._device.ScanForWiFiNetworks();
                    break;
                case WirelessStates.GetComputerProfiles:
                    hr = this._device.LoadComputerWiFiProfiles();
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
            Dictionary<string, WlanProfile> dictionary = new Dictionary<string, WlanProfile>();
            if (this._deviceNetworks != null)
            {
                foreach (WlanProfile deviceNetwork in _deviceNetworks)
                {
                    if (!dictionary.ContainsKey(deviceNetwork.SSID))
                    {
                        WlanProfile wlanProfile = deviceNetwork;
                        foreach (WlanProfile computerNetwork in _computerNetworks)
                        {
                            if (computerNetwork.SSID == deviceNetwork.SSID)
                            {
                                wlanProfile = computerNetwork;
                                break;
                            }
                        }
                        if (!wlanProfile.Connected)
                            wlanProfile.Key = string.Empty;
                        dictionary.Add(wlanProfile.SSID, wlanProfile);
                        this._networkList.Add(wlanProfile);
                    }
                }
            }
            this._networkList.Sort(new WlanSignalStrenghComparer());
        }

        protected override void AddListeners()
        {
            this._device.WiFiScanCompletedEvent += new FallibleEventHandler(this.Device_GetDeviceWlanNetworksCompleteEvent);
            this._device.ComputerWiFiProfilesLoadedEvent += new FallibleEventHandler(this.Device_GetWlanProfilesCompleteEvent);
        }

        protected override void RemoveListeners()
        {
            this._device.WiFiScanCompletedEvent -= new FallibleEventHandler(this.Device_GetDeviceWlanNetworksCompleteEvent);
            this._device.ComputerWiFiProfilesLoadedEvent -= new FallibleEventHandler(this.Device_GetWlanProfilesCompleteEvent);
        }

        private void Device_GetWlanProfilesCompleteEvent(object sender, FallibleEventArgs args)
        {
            HRESULT hr = args.HR;
            this.SetResult(hr);
            if (hr.IsSuccess)
                this._device.GetWiFiProfileList(ref this._computerNetworks);
            this.StepComplete(WirelessStateResults.Success);
        }

        private void Device_GetDeviceWlanNetworksCompleteEvent(object sender, FallibleEventArgs args)
        {
            HRESULT hr = args.HR;
            this.SetResult(hr);
            if (hr.IsSuccess)
                hr = this._device.GetWiFiProfileList(ref this._deviceNetworks);
            if (hr.IsSuccess)
                this.StepComplete(WirelessStateResults.Success);
            else
                this.StepComplete(WirelessStateResults.Error);
        }
    }
}
