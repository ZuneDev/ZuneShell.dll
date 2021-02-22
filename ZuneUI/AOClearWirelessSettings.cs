// Decompiled with JetBrains decompiler
// Type: ZuneUI.AOClearWirelessSettings
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;
using MicrosoftZuneLibrary;

namespace ZuneUI
{
    internal class AOClearWirelessSettings : AsyncOperation
    {
        private WirelessStates[] _clearWirelessSettingsStatesMethod1 = new WirelessStates[2]
        {
      WirelessStates.CommitProfileToDevice,
      WirelessStates.UnassociateWlanDevice
        };
        private WirelessStates[] _clearWirelessSettingsStatesMethod2 = new WirelessStates[2]
        {
      WirelessStates.UnassociateWlanDevice,
      WirelessStates.UnassociateNetwork
        };
        private bool _fIgnoreErrors;

        public WirelessStateResults StartOperation(
          UIDevice device,
          AsyncOperation.AOComplete completeFunc,
          bool fIgnoreErrors)
        {
            this.ResetState(fIgnoreErrors);
            WirelessStates[] states;
            if (device.SupportsWirelessSetupMethod1)
                states = this._clearWirelessSettingsStatesMethod1;
            else if (device.SupportsWirelessSetupMethod2)
            {
                states = this._clearWirelessSettingsStatesMethod2;
            }
            else
            {
                ShipAssert.Assert(false);
                return WirelessStateResults.Error;
            }
            return this.StartOperation(device, completeFunc, states);
        }

        protected override WirelessStateResults DoStep(WirelessStates state)
        {
            HRESULT hr;
            switch (state)
            {
                case WirelessStates.CommitProfileToDevice:
                    hr = this._device.SetWiFiProfileList(new WlanProfileList());
                    if (hr.IsSuccess)
                    {
                        hr = this._device.SendWiFiProfiles();
                        break;
                    }
                    break;
                case WirelessStates.UnassociateWlanDevice:
                    hr = this._device.RemoveWiFiAssociation();
                    break;
                case WirelessStates.UnassociateNetwork:
                    hr = this._device.SetWifiMediaSyncSSID(string.Empty);
                    if (hr.IsSuccess || this._fIgnoreErrors)
                    {
                        ShipAssert.Assert(hr.IsSuccess);
                        this.StepComplete(WirelessStateResults.Success);
                        break;
                    }
                    break;
                default:
                    hr = HRESULT._E_ABORT;
                    break;
            }
            if (hr.IsSuccess || this._fIgnoreErrors)
            {
                ShipAssert.Assert(hr.IsSuccess);
                return WirelessStateResults.Success;
            }
            this.SetResult(hr);
            return WirelessStateResults.Error;
        }

        protected override void AddListeners()
        {
            this._device.WiFiProfilesSentEvent += new FallibleEventHandler(this.Device_SetDeviceWlanProfilesCompleteEvent);
            this._device.WiFiRemovalCompletedEvent += new FallibleEventHandler(this.Device_UnassociateWlanDeviceCompleteEvent);
        }

        protected override void RemoveListeners()
        {
            this._device.WiFiProfilesSentEvent -= new FallibleEventHandler(this.Device_SetDeviceWlanProfilesCompleteEvent);
            this._device.WiFiRemovalCompletedEvent -= new FallibleEventHandler(this.Device_UnassociateWlanDeviceCompleteEvent);
        }

        private void ResetState(bool fIgnoreErrors) => this._fIgnoreErrors = fIgnoreErrors;

        private void Device_SetDeviceWlanProfilesCompleteEvent(object sender, FallibleEventArgs args)
        {
            if (args.HR.IsSuccess || this._fIgnoreErrors)
            {
                this.StepComplete(WirelessStateResults.Success);
            }
            else
            {
                this.SetResult(args.HR);
                this.StepComplete(WirelessStateResults.Error);
            }
        }

        private void Device_UnassociateWlanDeviceCompleteEvent(object sender, FallibleEventArgs args)
        {
            if (args.HR.IsSuccess || this._fIgnoreErrors)
            {
                this.StepComplete(WirelessStateResults.Success);
            }
            else
            {
                this.SetResult(args.HR);
                this.StepComplete(WirelessStateResults.Error);
            }
        }
    }
}
