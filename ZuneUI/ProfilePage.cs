// Decompiled with JetBrains decompiler
// Type: ZuneUI.ProfilePage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Collections;

namespace ZuneUI
{
    public class ProfilePage : LibraryPage
    {
        private ProfilePanel _profilePanel;
        public static readonly string ProfilePageTemplate = "res://ZuneShellResources!Profile.uix#ProfileLibrary";
        public static readonly string ProfileBackgroundTemplate = "res://ZuneShellResources!Profile.uix#ProfileBackground";

        public static ProfilePage CreateInstance(IDictionary args)
        {
            string zuneTag = args != null ? args[(object)"ZuneTag"] as string : (string)null;
            return new ProfilePage(string.IsNullOrEmpty(zuneTag) || SignIn.Instance.IsSignedInUser(zuneTag) ? Shell.MainFrame.Social.Me : Shell.MainFrame.Social.Friends);
        }

        public ProfilePage(Node pivotPreference)
        {
            this.PivotPreference = pivotPreference;
            this.IsRootPage = pivotPreference == Shell.MainFrame.Social.Me;
            this.UI = ProfilePage.ProfilePageTemplate;
            this.UIPath = "Social\\Profile";
            this.BackgroundUI = ProfilePage.ProfileBackgroundTemplate;
            this.TransportControlStyle = TransportControlStyle.Music;
            this.PlaybackContext = PlaybackContext.Music;
            SignIn.Instance.SignInStatusUpdatedEvent += new EventHandler(this.OnSignedInStatusChange);
        }

        protected override void OnDispose(bool fDisposing)
        {
            if (fDisposing)
                SignIn.Instance.SignInStatusUpdatedEvent -= new EventHandler(this.OnSignedInStatusChange);
            base.OnDispose(fDisposing);
        }

        private void OnSignedInStatusChange(object sender, EventArgs args)
        {
            if (!SignIn.Instance.SignedIn || this.PivotPreference != Shell.MainFrame.Social.Me)
                return;
            this.CreateProfilePanel((string)null, ProfileCategories.RecentlyPlayed, Guid.Empty, 0, (string)null);
        }

        protected override void OnNavigatedToWorker()
        {
            string zuneTag = this.NavigationArguments != null ? this.NavigationArguments[(object)"ZuneTag"] as string : (string)null;
            string selectedFriendTag = this.NavigationArguments != null ? this.NavigationArguments[(object)"FriendTag"] as string : (string)null;
            Category profilePivot = ProfileCategories.RecentlyPlayed;
            object obj1 = this.NavigationArguments != null ? this.NavigationArguments[(object)"Pivot"] : (object)null;
            if (obj1 != null)
                profilePivot = (Category)obj1;
            int chosenSortFriends = 0;
            object obj2 = this.NavigationArguments != null ? this.NavigationArguments[(object)"ChosenIndexSortFriends"] : (object)null;
            if (obj2 != null)
                chosenSortFriends = (int)obj2;
            Guid playlistTrack = Guid.Empty;
            object obj3 = this.NavigationArguments != null ? this.NavigationArguments[(object)"PlaylistTrack"] : (object)null;
            if (obj3 != null)
                playlistTrack = (Guid)obj3;
            this.CreateProfilePanel(zuneTag, profilePivot, playlistTrack, chosenSortFriends, selectedFriendTag);
            base.OnNavigatedToWorker();
        }

        private void CreateProfilePanel(
          string zuneTag,
          Category profilePivot,
          Guid playlistTrack,
          int chosenSortFriends,
          string selectedFriendTag)
        {
            if (this.PivotPreference == Shell.MainFrame.Social.Me)
            {
                zuneTag = (string)null;
                if (this.NavigationArguments != null)
                    this.NavigationArguments.Remove((object)"ZuneTag");
            }
            this.ProfilePanel = new ProfilePanel(this, zuneTag, profilePivot, playlistTrack, chosenSortFriends, selectedFriendTag);
        }

        public ProfilePanel ProfilePanel
        {
            get => this._profilePanel;
            set
            {
                if (this._profilePanel == value)
                    return;
                this._profilePanel = value;
                this.FirePropertyChanged(nameof(ProfilePanel));
            }
        }

        public override void InvokeSettings() => Shell.SettingsFrame.Settings.Account.Invoke();

        public override IPageState SaveAndRelease()
        {
            ProfilePageState profilePageState = new ProfilePageState(this);
            this._profilePanel.Release();
            this.Dispose();
            return (IPageState)profilePageState;
        }

        public static void NavigateTo(
          string zuneTag,
          Category profilePivot,
          Guid track,
          string serviceContext)
        {
            bool flag = false;
            if (ZuneShell.DefaultInstance.CurrentPage is ProfilePage)
            {
                ProfilePage currentPage = (ProfilePage)ZuneShell.DefaultInstance.CurrentPage;
                if (SignIn.TagsMatch(currentPage.ProfilePanel.ZuneTag, zuneTag))
                {
                    currentPage.ProfilePanel.SelectedPivot = profilePivot;
                    currentPage.ProfilePanel.SelectedPlaylistTrack = track;
                    currentPage.ProfilePanel.CanChangeSelectedItem = true;
                    flag = true;
                }
            }
            if (flag)
                return;
            Hashtable hashtable = new Hashtable();
            hashtable.Add((object)"ZuneTag", (object)zuneTag);
            if (profilePivot != null)
                hashtable.Add((object)"Pivot", (object)profilePivot);
            if (!GuidHelper.IsEmpty(track))
                hashtable.Add((object)"PlaylistTrack", (object)track);
            if (!string.IsNullOrEmpty(serviceContext))
                hashtable.Add((object)"ServiceContext", (object)serviceContext);
            ZuneShell.DefaultInstance.Execute("Social\\Profile", (IDictionary)hashtable);
        }
    }
}
