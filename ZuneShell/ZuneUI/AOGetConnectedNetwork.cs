// Decompiled with JetBrains decompiler
// Type: ZuneUI.AOGetConnectedNetwork
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using MicrosoftZuneLibrary;
using System.Collections.Generic;

namespace ZuneUI
{
    internal class AOGetConnectedNetwork : AsyncOperation
    {
        private WirelessStates[] _getConnectedNetworkStates = new WirelessStates[1]
        {
      WirelessStates.GetComputerProfiles
        };
        private WlanProfileList _computerProfiles;
        private WlanProfile _connectedNetwork;

        public WlanProfile ConnectedNetwork => this.Finished ? this._connectedNetwork : null;

        private void ResetState()
        {
            if (!this.Idle && !this.Finished)
                return;
            this._computerProfiles = new WlanProfileList();
            this._connectedNetwork = null;
        }

        public WirelessStateResults StartOperation(
          UIDevice device,
          AOComplete completeFunc)
        {
            this.ResetState();
            return this.StartOperation(device, completeFunc, this._getConnectedNetworkStates);
        }

        protected override WirelessStateResults DoStep(WirelessStates state)
        {
            HRESULT hr = state != WirelessStates.GetComputerProfiles ? HRESULT._E_ABORT : this._device.LoadComputerWiFiProfiles();
            if (hr.IsSuccess)
                return WirelessStateResults.Success;
            this.SetResult(hr);
            return WirelessStateResults.Error;
        }

        protected override void EndOperation(WirelessStateResults result)
        {
            this._connectedNetwork = null;
            if (this._computerProfiles == null)
                return;
            foreach (WlanProfile computerProfile in _computerProfiles)
            {
                if (computerProfile.Connected)
                {
                    this._connectedNetwork = computerProfile;
                    break;
                }
            }
        }

        protected override void AddListeners() => this._device.ComputerWiFiProfilesLoadedEvent += new FallibleEventHandler(this.Device_GetWlanProfilesCompleteEvent);

        protected override void RemoveListeners() => this._device.ComputerWiFiProfilesLoadedEvent -= new FallibleEventHandler(this.Device_GetWlanProfilesCompleteEvent);

        private void Device_GetWlanProfilesCompleteEvent(object sender, FallibleEventArgs args)
        {
            HRESULT hr = args.HR;
            this.SetResult(hr);
            if (hr.IsSuccess)
                hr = this._device.GetWiFiProfileList(ref this._computerProfiles);
            if (hr.IsSuccess)
                this.StepComplete(WirelessStateResults.Success);
            else
                this.StepComplete(WirelessStateResults.Error);
        }
    }
}
