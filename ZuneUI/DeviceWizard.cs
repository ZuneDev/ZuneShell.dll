// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeviceWizard
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public abstract class DeviceWizard : Wizard
    {
        private bool _disconnected;
        private UIDevice _activeDevice;

        public DeviceWizard() => SingletonModelItem<UIDeviceList>.Instance.DeviceDisconnectedEvent += new DeviceListEventHandler(this.OnDeviceDisconnected);

        public UIDevice ActiveDevice
        {
            get
            {
                if (this._activeDevice == null)
                    this._activeDevice = DeviceManagement.SetupDevice != null ? DeviceManagement.SetupDevice : SyncControls.Instance.CurrentDeviceOverride;
                return this._activeDevice;
            }
        }

        public bool Disconnected
        {
            get => this._disconnected;
            set
            {
                if (this._disconnected == value)
                    return;
                this._disconnected = value;
                this.FirePropertyChanged(nameof(Disconnected));
            }
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
                SingletonModelItem<UIDeviceList>.Instance.DeviceDisconnectedEvent -= new DeviceListEventHandler(this.OnDeviceDisconnected);
            base.OnDispose(disposing);
        }

        public override void Cancel()
        {
            this.CancelSettings();
            if (this.ActiveDevice != UIDeviceList.NullDevice && (this.ActiveDevice.RequiresFirmwareUpdate || this.ActiveDevice.Relationship == DeviceRelationship.None || !this.ActiveDevice.InStandardMode))
            {
                SingletonModelItem<UIDeviceList>.Instance.HideDevice(this.ActiveDevice);
            }
            else
            {
                if (this.ActiveDevice.Relationship != DeviceRelationship.Guest)
                    return;
                ZuneShell.DefaultInstance.Management.DisposeDeviceManagement(false);
            }
        }

        private void OnDeviceDisconnected(object sender, DeviceListEventArgs args)
        {
            if (this.ActiveDevice != args.Device)
                return;
            this.Disconnected = true;
        }
    }
}
