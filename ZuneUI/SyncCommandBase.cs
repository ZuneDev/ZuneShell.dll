// Decompiled with JetBrains decompiler
// Type: ZuneUI.SyncCommandBase
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.ComponentModel;

namespace ZuneUI
{
    public class SyncCommandBase : Command
    {
        private UIDevice _device;
        protected bool _availableWhenSyncing;

        public UIDevice Device
        {
            get => this._device;
            set
            {
                if (this._device == value)
                    return;
                if (this._device != null)
                    this._device.PropertyChanged -= new PropertyChangedEventHandler(this.OnDevicePropertyChanged);
                this._device = value;
                if (this._device != null)
                    this._device.PropertyChanged += new PropertyChangedEventHandler(this.OnDevicePropertyChanged);
                this.UpdateAvailability();
                this.FirePropertyChanged(nameof(Device));
            }
        }

        private void OnDevicePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (!(args.PropertyName == "IsReadyForSync") && !(args.PropertyName == "IsSyncing"))
                return;
            this.UpdateAvailability();
        }

        protected override void OnInvoked()
        {
            this.Available = false;
            base.OnInvoked();
        }

        private void UpdateAvailability() => this.Available = this.Device != null && this.Device.IsReadyForSync && this.Device.IsSyncing == this._availableWhenSyncing;
    }
}
