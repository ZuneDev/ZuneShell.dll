// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeviceChoice
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;
using System.ComponentModel;

namespace ZuneUI
{
    public class DeviceChoice : Choice
    {
        public DeviceChoice()
          : this(null)
        {
        }

        public DeviceChoice(IModelItemOwner owner)
          : base(owner)
        {
            UIDeviceList instance = SingletonModelItem<UIDeviceList>.Instance;
            instance.DeviceAddedEvent += new DeviceListEventHandler(this.ListChanged);
            instance.DeviceRemovedEvent += new DeviceListEventHandler(this.ListChanged);
            SyncControls.Instance.PropertyChanged += new PropertyChangedEventHandler(this.OnSyncPropertyChanged);
            this.BuildOptions();
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                SyncControls.Instance.PropertyChanged -= new PropertyChangedEventHandler(this.OnSyncPropertyChanged);
                UIDeviceList instance = SingletonModelItem<UIDeviceList>.Instance;
                instance.DeviceAddedEvent -= new DeviceListEventHandler(this.ListChanged);
                instance.DeviceRemovedEvent -= new DeviceListEventHandler(this.ListChanged);
            }
            base.OnDispose(disposing);
        }

        private void ListChanged(object sender, DeviceListEventArgs args) => this.BuildOptions();

        private void OnSyncPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (!(args.PropertyName == "CurrentDevice"))
                return;
            for (int index = 0; index < this.Options.Count; ++index)
            {
                if ((UIDevice)this.Options[index] == SyncControls.Instance.CurrentDevice)
                {
                    this.ChosenIndex = index;
                    break;
                }
            }
        }

        private void BuildOptions()
        {
            UIDeviceList instance = SingletonModelItem<UIDeviceList>.Instance;
            this.Options = new ArrayListDataSet(this);
            foreach (UIDevice uiDevice in instance)
            {
                this.Options.Add(uiDevice);
                if (uiDevice == SyncControls.Instance.CurrentDevice)
                    this.ChosenValue = uiDevice;
            }
        }

        protected override void OnChosenChanged()
        {
            base.OnChosenChanged();
            if (this.ChosenValue == null)
                return;
            SyncControls.Instance.SetCurrentDevice((UIDevice)this.ChosenValue);
        }
    }
}
