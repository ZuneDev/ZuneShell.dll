// Decompiled with JetBrains decompiler
// Type: ZuneUI.UIDeviceList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using MicrosoftZuneLibrary;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace ZuneUI
{
    public class UIDeviceList : SingletonModelItem<UIDeviceList>, IEnumerable<UIDevice>, IEnumerable
    {
        private static UIDevice _nullDevice;
        private DeviceList _deviceList;
        private Dictionary<Device, UIDevice> _deviceToUiMap;
        private Dictionary<UIDevice, Device> _uiToDeviceMap;
        private bool _allowUnreadyDevices = true;
        private bool _allowDeviceConnections = true;
        private List<UIDevice> _deferredConnectedDevices;
        private List<UIDevice> _deferredUnreadyDevices;
        private bool _isDisposed;
        private object _initializationLock;

        internal event DeviceListEventHandler DeviceAddedEvent;

        internal event DeviceListEventHandler DeviceRemovedEvent;

        public event DeviceListEventHandler DeviceConnectedEvent;

        public event DeviceListEventHandler DeviceDisconnectedEvent;

        public UIDeviceList()
        {
            this._deviceToUiMap = new Dictionary<Device, UIDevice>();
            this._uiToDeviceMap = new Dictionary<UIDevice, Device>();
            this._deferredConnectedDevices = new List<UIDevice>();
            this._deferredUnreadyDevices = new List<UIDevice>();
            this._initializationLock = new object();
        }

        public void Phase2Init() => ThreadPool.QueueUserWorkItem((WaitCallback)delegate
       {
           lock (this._initializationLock)
           {
               if (this._isDisposed)
                   return;
               this._deviceList = DeviceList.Instance;
               if (this._deviceList == null)
                   return;
               this._deviceList.Added += new DeviceAddedHandler(this.OnDeviceAdded);
               if (this._deviceList.Initialized)
                   return;
               HRESULT hresult = (HRESULT)this._deviceList.InitializeAndEnumerate();
           }
       }, (object)null);

        protected override void OnDispose(bool disposing)
        {
            if (this._deviceList != null)
            {
                lock (this._initializationLock)
                {
                    this._isDisposed = true;
                    this._deviceList.Added -= new DeviceAddedHandler(this.OnDeviceAdded);
                }
                foreach (ModelItem modelItem in this._deviceToUiMap.Values)
                    modelItem.PropertyChanged -= new PropertyChangedEventHandler(this.OnUIDevicePropertyChanged);
            }
            base.OnDispose(disposing);
        }

        public static UIDevice NullDevice
        {
            get
            {
                if (UIDeviceList._nullDevice == null)
                    UIDeviceList._nullDevice = new UIDevice((IModelItemOwner)SingletonModelItem<UIDeviceList>.Instance, (Device)null);
                return UIDeviceList._nullDevice;
            }
        }

        public void HideDevice(UIDevice device)
        {
            if (!this.IsListReady)
                return;
            this._deviceList.HideDevice(this._uiToDeviceMap[device]);
        }

        public HRESULT DeleteDevice(UIDevice device)
        {
            HRESULT hresult = HRESULT._E_UNEXPECTED;
            if (this.IsListReady)
            {
                hresult = (HRESULT)this._uiToDeviceMap[device].ClearCache();
                if (this.DeviceDisconnectedEvent != null)
                {
                    this.DeviceDisconnectedEvent((object)this, new DeviceListEventArgs(device));
                    this.FirePropertyChanged("DeviceDisconnectedEvent");
                }
                if (this.DeviceRemovedEvent != null)
                    this.DeviceRemovedEvent((object)this, new DeviceListEventArgs(device));
            }
            return hresult;
        }

        public bool AllowUnreadyDevices
        {
            get => this._allowUnreadyDevices;
            set
            {
                if (this._allowUnreadyDevices == value)
                    return;
                this._allowUnreadyDevices = value;
                this.FirePropertyChanged(nameof(AllowUnreadyDevices));
                if (!this._allowUnreadyDevices)
                    return;
                if (this._deferredUnreadyDevices.Count > 0)
                {
                    this._deferredConnectedDevices.AddRange((IEnumerable<UIDevice>)this._deferredUnreadyDevices);
                    this._deferredUnreadyDevices.Clear();
                }
                Application.DeferredInvoke((DeferredInvokeHandler)delegate
               {
                   this.HandleDeferredConnectedDevices();
               }, DeferredInvokePriority.Low);
            }
        }

        public bool AllowDeviceConnections
        {
            get => this._allowDeviceConnections;
            set
            {
                if (this._allowDeviceConnections == value)
                    return;
                this._allowDeviceConnections = value;
                this.FirePropertyChanged(nameof(AllowDeviceConnections));
                if (!this._allowDeviceConnections)
                    return;
                Application.DeferredInvoke((DeferredInvokeHandler)delegate
               {
                   this.HandleDeferredConnectedDevices();
               }, DeferredInvokePriority.Low);
            }
        }

        public string TranscodedFilesCachePath
        {
            get
            {
                string empty = string.Empty;
                if (this.IsListReady)
                    this._deviceList.GetTranscodedFilesCachePath(ref empty);
                return empty;
            }
            set
            {
                if (!this.IsListReady)
                    return;
                this._deviceList.SetTranscodedFilesCachePath(value);
                this.FirePropertyChanged("TranscodeCachePath");
            }
        }

        public int TranscodedFilesCacheSize
        {
            get
            {
                int num = 0;
                if (this.IsListReady)
                    num = ClientConfiguration.Transcode.TranscodedFilesCacheSize;
                return num;
            }
            set
            {
                if (!this.IsListReady || this.TranscodedFilesCacheSize == value)
                    return;
                this._deviceList.SetTranscodedFilesCacheSize(value);
                this.FirePropertyChanged(nameof(TranscodedFilesCacheSize));
            }
        }

        public HRESULT ClearTranscodeCache() => !this.IsListReady ? HRESULT._E_UNEXPECTED : (HRESULT)this._deviceList.ClearTranscodeCache();

        public IEnumerator<UIDevice> GetEnumerator()
        {
            if (this.IsListReady)
            {
                for (int i = 0; i < this._deviceList.Count; ++i)
                {
                    UIDevice device = this.GetUIDevice(this._deviceList.GetItem(i));
                    if (device.IsConnectedToPC || !device.IsGuest)
                        yield return device;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => (IEnumerator)this.GetEnumerator();

        private void OnDeviceAdded(Device rawDevice) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           if (this.IsDisposed)
               return;
           this.GetUIDevice(rawDevice);
       }, (object)null);

        private void OnUIDevicePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (!(sender is UIDevice device))
                return;
            if (args.PropertyName == "IsValid")
            {
                if (!device.IsValid)
                    return;
                this.OnDeviceValid(device);
            }
            else
            {
                if (!(args.PropertyName == "IsConnectedToClient"))
                    return;
                if (device.IsConnectedToClient)
                    this.OnDeviceConnected(device);
                else
                    this.OnDeviceDisconnected(device);
            }
        }

        private void OnDeviceValid(UIDevice device)
        {
            if (!device.IsGuest && this.DeviceAddedEvent != null)
                this.DeviceAddedEvent((object)this, new DeviceListEventArgs(device));
            if (!device.SupportsSyncApplications)
                return;
            ClientConfiguration.Shell.ShowApplicationPivot = true;
            Shell.MainFrame.Collection.UpdateApplicationPivot();
        }

        private void OnDeviceConnected(UIDevice device)
        {
            if (!this.AllowDeviceConnections)
            {
                if (this._deferredConnectedDevices.Contains(device))
                    return;
                this._deferredConnectedDevices.Add(device);
            }
            else if (!this.AllowUnreadyDevices && !UIDeviceList.IsSuitableForConnection(device))
            {
                if (this._deferredUnreadyDevices.Contains(device))
                    return;
                this._deferredUnreadyDevices.Add(device);
            }
            else
            {
                if (device.IsGuest && this.DeviceAddedEvent != null)
                    this.DeviceAddedEvent((object)this, new DeviceListEventArgs(device));
                if (this.DeviceConnectedEvent == null)
                    return;
                this.DeviceConnectedEvent((object)this, new DeviceListEventArgs(device));
                this.FirePropertyChanged("DeviceConnectedEvent");
            }
        }

        private void OnDeviceDisconnected(UIDevice device)
        {
            if (this._deferredConnectedDevices.Contains(device))
                this._deferredConnectedDevices.Remove(device);
            if (this._deferredUnreadyDevices.Contains(device))
                this._deferredUnreadyDevices.Remove(device);
            if (this.DeviceDisconnectedEvent != null)
            {
                this.DeviceDisconnectedEvent((object)this, new DeviceListEventArgs(device));
                this.FirePropertyChanged("DeviceDisconnectedEvent");
            }
            if (!device.IsGuest || this.DeviceRemovedEvent == null)
                return;
            this.DeviceRemovedEvent((object)this, new DeviceListEventArgs(device));
        }

        private UIDevice GetUIDevice(Device device)
        {
            if (!this._deviceToUiMap.ContainsKey(device))
            {
                UIDevice key = new UIDevice((IModelItemOwner)this, device);
                this._deviceToUiMap.Add(device, key);
                this._uiToDeviceMap.Add(key, device);
                key.PropertyChanged += new PropertyChangedEventHandler(this.OnUIDevicePropertyChanged);
            }
            return this._deviceToUiMap[device];
        }

        private void HandleDeferredConnectedDevices()
        {
            if (!this.AllowDeviceConnections)
                return;
            for (int index = 0; index < this._deferredConnectedDevices.Count; ++index)
                this.OnDeviceConnected(this._deferredConnectedDevices[index]);
            this._deferredConnectedDevices.Clear();
        }

        public static bool IsSuitableForConnection(UIDevice device)
        {
            bool requiresFirmwareUpdate = device.RequiresFirmwareUpdate;
            bool flag = device.Relationship == DeviceRelationship.None;
            return !requiresFirmwareUpdate && !flag;
        }

        private bool IsListReady => this._deviceList != null && this._deviceList.Initialized;

        public static SyncCategory MapMediaTypeToSyncCategory(MediaType mediaType)
        {
            switch (mediaType)
            {
                case MediaType.Track:
                case MediaType.Playlist:
                case MediaType.Album:
                case MediaType.Genre:
                case MediaType.PlaylistContentItem:
                case MediaType.Artist:
                    return SyncCategory.Music;
                case MediaType.Video:
                    return SyncCategory.Video;
                case MediaType.Photo:
                case MediaType.MediaFolder:
                    return SyncCategory.Photo;
                case MediaType.PodcastEpisode:
                case MediaType.Podcast:
                    return SyncCategory.Podcast;
                case MediaType.UserCard:
                    return SyncCategory.Friend;
                case MediaType.Application:
                    return SyncCategory.Application;
                default:
                    return SyncCategory.Undefined;
            }
        }

        public static MediaType MapSyncCategoryToMediaType(SyncCategory mediaType)
        {
            switch (mediaType)
            {
                case SyncCategory.Music:
                    return MediaType.Track;
                case SyncCategory.Video:
                    return MediaType.Video;
                case SyncCategory.Photo:
                    return MediaType.Photo;
                case SyncCategory.Podcast:
                    return MediaType.PodcastEpisode;
                case SyncCategory.Friend:
                    return MediaType.UserCard;
                case SyncCategory.Application:
                    return MediaType.Application;
                default:
                    return MediaType.Undefined;
            }
        }
    }
}
