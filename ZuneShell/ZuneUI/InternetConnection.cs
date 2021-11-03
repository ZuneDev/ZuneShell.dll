// Decompiled with JetBrains decompiler
// Type: ZuneUI.InternetConnection
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Net.NetworkInformation;

namespace ZuneUI
{
    public class InternetConnection : NotifyPropertyChangedImpl
    {
        private static InternetConnection _instance;
        private bool _isConnected;
        private bool _initializationSucceeded;

        private InternetConnection() => this.Initialize();

        public static InternetConnection Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new InternetConnection();
                return _instance;
            }
        }

        public bool IsConnected
        {
            get
            {
                if (!this._initializationSucceeded)
                    this.IsConnected = Win32InternetConnection.IsConnected;
                return this._isConnected;
            }
            private set
            {
                if (this._isConnected == value)
                    return;
                this._isConnected = value;
                this.FirePropertyChanged(nameof(IsConnected));
            }
        }

        private void Initialize()
        {
            try
            {
                this._isConnected = NetworkInterface.GetIsNetworkAvailable();
                NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(this.OnNetworkAvailabilityChanged);
                this._initializationSucceeded = true;
            }
            catch (Exception ex)
            {
                this._initializationSucceeded = false;
            }
        }

        private void OnNetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e) => this.IsConnected = e.IsAvailable;
    }
}
