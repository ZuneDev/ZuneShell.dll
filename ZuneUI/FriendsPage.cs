// Decompiled with JetBrains decompiler
// Type: ZuneUI.FriendsPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class FriendsPage : LibraryPage
    {
        public static readonly string FriendsPageTemplate = "res://ZuneShellResources!Friends.uix#FriendLibrary";
        private FriendsPanel _friendsPanel;
        private string _selectedZuneTag;

        public static FriendsPage CreateInstance() => new FriendsPage(false);

        public static FriendsPage CreateInstance(bool showDevice) => new FriendsPage(showDevice);

        public FriendsPage(bool showDevice)
          : base(showDevice, MediaType.UserCard)
        {
            if (showDevice)
            {
                Deviceland.InitDevicePage((ZunePage)this);
                this.PivotPreference = Shell.MainFrame.Device.Friends;
                this.ShowComputerIcon = ComputerIconState.Show;
            }
            else
                this.PivotPreference = Shell.MainFrame.Social.Friends;
            this.TransportControlStyle = TransportControlStyle.Music;
            this.PlaybackContext = PlaybackContext.Music;
            this.IsRootPage = true;
            this.UI = FriendsPage.FriendsPageTemplate;
            this.UIPath = "Social\\Friends";
            this._friendsPanel = new FriendsPanel(this);
        }

        public override void InvokeSettings()
        {
            if (Shell.MainFrame.Device.IsCurrent)
                Shell.SettingsFrame.Settings.Device.Invoke();
            else
                Shell.SettingsFrame.Settings.Account.Invoke();
        }

        public FriendsPanel FriendsPanel => this._friendsPanel;

        public string SelectedZuneTag
        {
            get => this._selectedZuneTag;
            set
            {
                if (!(this._selectedZuneTag != value))
                    return;
                this._selectedZuneTag = value;
                this.FirePropertyChanged(nameof(SelectedZuneTag));
            }
        }

        public string ZuneTag
        {
            get
            {
                string str = (string)null;
                if (!this.ShowDeviceContents)
                    str = SignIn.Instance.ZuneTag;
                else if (SyncControls.Instance.CurrentDevice.IsValid)
                    str = SyncControls.Instance.CurrentDevice.ZuneTag;
                return str;
            }
        }

        public override IPageState SaveAndRelease()
        {
            this._friendsPanel.Release();
            return base.SaveAndRelease();
        }
    }
}
