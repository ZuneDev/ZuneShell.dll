// Decompiled with JetBrains decompiler
// Type: ZuneUI.MobileWirelessSync
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;

namespace ZuneUI
{
    public class MobileWirelessSync : ModelItem
    {
        private UIDevice _mobileDevice;
        private string _mediaSyncSSID;
        private string _connectedSSID;
        private int _errorCode = HRESULT._S_OK.Int;

        public MobileWirelessSync(UIDevice endpoint)
          : base(ZuneShell.DefaultInstance.Management.DeviceManagement)
          => this._mobileDevice = endpoint;

        public string MediaSyncSSID
        {
            get
            {
                if (string.IsNullOrEmpty(this._mediaSyncSSID))
                {
                    string empty = string.Empty;
                    this._mediaSyncSSID = this._mobileDevice.GetWifiMediaSyncSSID(ref empty) == HRESULT._S_OK ? empty : string.Empty;
                }
                return this._mediaSyncSSID ?? string.Empty;
            }
            set
            {
                if (!(value != this._mediaSyncSSID))
                    return;
                HRESULT hresult = this._mobileDevice.SetWifiMediaSyncSSID(value);
                if (hresult == HRESULT._S_OK)
                {
                    this._mediaSyncSSID = value;
                }
                else
                {
                    this._mediaSyncSSID = string.Empty;
                    this.ErrorCode = hresult.Int;
                }
                this.FirePropertyChanged(nameof(MediaSyncSSID));
            }
        }

        public string ConnectedSSID
        {
            get
            {
                if (string.IsNullOrEmpty(this._connectedSSID))
                {
                    string empty = string.Empty;
                    HRESULT wifiConnectedSsid = this._mobileDevice.GetWifiConnectedSSID(ref empty);
                    if (wifiConnectedSsid == HRESULT._S_OK)
                    {
                        this._connectedSSID = empty;
                    }
                    else
                    {
                        this._connectedSSID = string.Empty;
                        this.ErrorCode = wifiConnectedSsid.Int;
                    }
                }
                return this._connectedSSID ?? string.Empty;
            }
        }

        public bool IsWlanDeviceDisabled
        {
            get
            {
                bool disabled = false;
                HRESULT hresult = this._mobileDevice.IsWlanDeviceDisabled(ref disabled);
                if (hresult != HRESULT._S_OK)
                    this.ErrorCode = hresult.Int;
                return disabled;
            }
        }

        public int ErrorCode
        {
            get => this._errorCode;
            private set
            {
                ShipAssert.Assert(value != HRESULT._S_OK.Int);
                if (this._errorCode == value)
                    return;
                this._errorCode = value;
                this.FirePropertyChanged(nameof(ErrorCode));
            }
        }

        public bool WiFiSetupSuccess => true;

        private void ResetErrorCode() => this._errorCode = HRESULT._S_OK.Int;

        public void TestMediaSyncConnection()
        {
            this.ResetErrorCode();
            this._mobileDevice.WiFiTestCompletedEvent += new FallibleEventHandler(this.WiFiTestCompleted);
            HRESULT hresult = this._mobileDevice.TestWiFi();
            if (!hresult.IsError)
                return;
            this._mobileDevice.WiFiTestCompletedEvent -= new FallibleEventHandler(this.WiFiTestCompleted);
            this.ErrorCode = hresult.Int;
        }

        public void RefreshConnectedNetwork()
        {
            this._connectedSSID = string.Empty;
            this.FirePropertyChanged("ConnectedSSID");
        }

        public void RefreshSyncNetwork()
        {
            this._mediaSyncSSID = string.Empty;
            this.FirePropertyChanged("MediaSyncSSID");
        }

        public void UnassociateNetwork()
        {
            this.ResetErrorCode();
            this._mobileDevice.WiFiRemovalCompletedEvent += new FallibleEventHandler(this.WiFiRemovalCompleted);
            HRESULT hresult = this._mobileDevice.RemoveWiFiAssociation();
            if (!hresult.IsError)
                return;
            this._mobileDevice.WiFiRemovalCompletedEvent -= new FallibleEventHandler(this.WiFiRemovalCompleted);
            this.ErrorCode = hresult.Int;
        }

        private void WiFiTestCompleted(object sender, FallibleEventArgs args)
        {
            this.ResetErrorCode();
            this._mobileDevice.WiFiTestCompletedEvent -= new FallibleEventHandler(this.WiFiTestCompleted);
            if (args.HR.IsSuccess)
            {
                this._mobileDevice.WiFiAssociationCompletedEvent += new FallibleEventHandler(this.WiFiAssociationCompleted);
                HRESULT hresult = this._mobileDevice.AssociateWiFi();
                if (!(hresult != HRESULT._S_OK))
                    return;
                this._mobileDevice.WiFiAssociationCompletedEvent -= new FallibleEventHandler(this.WiFiAssociationCompleted);
                this.ErrorCode = hresult.Int;
            }
            else
                this.ErrorCode = args.HR.Int;
        }

        private void WiFiAssociationCompleted(object sender, FallibleEventArgs args)
        {
            this.ResetErrorCode();
            this._mobileDevice.WiFiAssociationCompletedEvent -= new FallibleEventHandler(this.WiFiAssociationCompleted);
            if (args.HR.IsSuccess)
            {
                this.MediaSyncSSID = this.ConnectedSSID;
                this.FirePropertyChanged("WiFiSetupSuccess");
            }
            else
                this.ErrorCode = args.HR.Int;
        }

        private void WiFiRemovalCompleted(object sender, FallibleEventArgs args)
        {
            this.ResetErrorCode();
            this._mobileDevice.WiFiRemovalCompletedEvent -= new FallibleEventHandler(this.WiFiRemovalCompleted);
            if (args.HR.IsSuccess)
                this.MediaSyncSSID = string.Empty;
            else
                this.ErrorCode = args.HR.Int;
        }
    }
}
