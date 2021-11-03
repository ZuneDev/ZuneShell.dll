// Decompiled with JetBrains decompiler
// Type: ZuneUI.FormatCompletionListener
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.ComponentModel;

namespace ZuneUI
{
    public class FormatCompletionListener : ModelItem
    {
        private Command _completed;
        private UIDevice _device;

        public FormatCompletionListener()
        {
            this._completed = new Command(this);
            SyncControls.Instance.PropertyChanged += new PropertyChangedEventHandler(this.OnDeviceChanged);
            this._device = SyncControls.Instance.CurrentDevice;
            this.SetCurrentDevice();
        }

        protected override void OnDispose(bool disposing)
        {
            base.OnDispose(disposing);
            if (!disposing || !this._device.IsValid)
                return;
            this._device.FormatCompletedEvent -= new FallibleEventHandler(this.FormatCompleted);
        }

        private void SetCurrentDevice()
        {
            if (this._device.IsValid)
                this._device.FormatCompletedEvent -= new FallibleEventHandler(this.FormatCompleted);
            this._device = SyncControls.Instance.CurrentDevice;
            if (!this._device.IsValid)
                return;
            this._device.FormatCompletedEvent += new FallibleEventHandler(this.FormatCompleted);
        }

        private void FormatCompleted(object sender, FallibleEventArgs args)
        {
            if (this._device.IsConnectedToClient)
            {
                this._device.EnumeratedEvent -= new FallibleEventHandler(this.EnumerationCompleted);
                this._device.EnumeratedEvent += new FallibleEventHandler(this.EnumerationCompleted);
                this._device.Enumerate();
            }
            else
                this.FinishFormat(HRESULT._S_OK);
        }

        private void EnumerationCompleted(object sender, FallibleEventArgs args) => this.FinishFormat(args.HR);

        private void FinishFormat(HRESULT hResult)
        {
            this._device.EnumeratedEvent -= new FallibleEventHandler(this.EnumerationCompleted);
            this.Completed.Invoke();
        }

        private void OnDeviceChanged(object sender, PropertyChangedEventArgs args)
        {
            if (!(args.PropertyName == "CurrentDevice"))
                return;
            this.SetCurrentDevice();
        }

        public bool IsFormatting => this._device.IsFormatting;

        public Command Completed => this._completed;
    }
}
