// Decompiled with JetBrains decompiler
// Type: ZuneUI.ShellMessagingNotifier
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Win32;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Messaging;
using System.Collections;

namespace ZuneUI
{
    public class ShellMessagingNotifier : ModelItem
    {
        private static ShellMessagingNotifier _instance;
        private MessagingNotifier _messagingNotifier;
        private int _newDeviceCartItemsCount;
        private int _newDeviceMessageCount;
        private IList _liveMessageItems;

        public static ShellMessagingNotifier Instance
        {
            get
            {
                if (ShellMessagingNotifier._instance == null)
                    ShellMessagingNotifier._instance = new ShellMessagingNotifier();
                return ShellMessagingNotifier._instance;
            }
        }

        public static bool HasInstance => ShellMessagingNotifier._instance != null;

        private ShellMessagingNotifier()
        {
            this._messagingNotifier = new MessagingNotifier();
            this._messagingNotifier.OnDeviceCartItemsPosted += new DeviceItemsPostedHandler(this.OnDeviceCartItemsPostedAsync);
            this._messagingNotifier.OnDeviceMessagesPosted += new DeviceItemsPostedHandler(this.OnDeviceMessagesPostedAsync);
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                this._messagingNotifier.OnDeviceMessagesPosted -= new DeviceItemsPostedHandler(this.OnDeviceMessagesPostedAsync);
                this._messagingNotifier.OnDeviceCartItemsPosted -= new DeviceItemsPostedHandler(this.OnDeviceCartItemsPostedAsync);
                this._messagingNotifier.Dispose();
                this._messagingNotifier = (MessagingNotifier)null;
            }
            base.OnDispose(disposing);
        }

        public int NewDeviceCartItemsCount => this._newDeviceCartItemsCount;

        public int NewDeviceMessageCount => this._newDeviceMessageCount;

        private void OnDeviceCartItemsPostedAsync(int iNewDeviceCartItems) => Application.DeferredInvoke(new DeferredInvokeHandler(this.OnDeviceCartItemsPosted), (object)iNewDeviceCartItems);

        private void OnDeviceMessagesPostedAsync(int iNewDeviceMessages) => Application.DeferredInvoke(new DeferredInvokeHandler(this.OnDeviceMessagesPosted), (object)iNewDeviceMessages);

        private void OnDeviceCartItemsPosted(object args)
        {
            this._newDeviceCartItemsCount = (int)args;
            this.FirePropertyChanged("NewDeviceCartItemsCount");
        }

        private void OnDeviceMessagesPosted(object args)
        {
            this._newDeviceMessageCount = (int)args;
            this.FirePropertyChanged("NewDeviceMessageCount");
        }

        public static int PersistedCartItemsCount
        {
            get => ClientConfiguration.Service.GetIntProperty(string.Format("{0}", (object)ClientConfiguration.Service.LastSignedInUserGuid), 0);
            set => ClientConfiguration.Service.SetIntProperty(string.Format("{0}", (object)ClientConfiguration.Service.LastSignedInUserGuid), value);
        }

        public static int CartItemsToUploadCount => new MessagingUserGuidConfiguration(RegistryHive.CurrentUser, ClientConfiguration.Messaging.ConfigurationPath, string.Format("{0}", (object)ClientConfiguration.Service.LastSignedInUserGuid)).CartItemsToUploadCount;

        public IList LiveMessageItems
        {
            get => this._liveMessageItems;
            set
            {
                if (this._liveMessageItems == value)
                    return;
                this._liveMessageItems = value;
                this.FirePropertyChanged(nameof(LiveMessageItems));
            }
        }
    }
}
