// Decompiled with JetBrains decompiler
// Type: ZuneUI.ProfilePageState
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Collections;

namespace ZuneUI
{
    public class ProfilePageState : IPageState
    {
        private string _zuneTag;
        private Category _profilePivot;
        private int _chosenIndexSortFriends;
        private Node _pivotPreference;
        private string _selectedFriendTag;
        private Guid _selectedPlaylistTrack;

        public ProfilePageState(ProfilePage page)
        {
            this._zuneTag = page.ProfilePanel.ZuneTag;
            this._profilePivot = page.ProfilePanel.SelectedPivot;
            this._chosenIndexSortFriends = page.ProfilePanel.ChosenIndexSortFriends;
            this._pivotPreference = page.PivotPreference;
            this._selectedFriendTag = page.ProfilePanel.SelectedFriendTag;
            this._selectedPlaylistTrack = page.ProfilePanel.SelectedPlaylistTrack;
        }

        public ProfilePage Restore()
        {
            Hashtable hashtable = new Hashtable();
            hashtable["ZuneTag"] = _zuneTag;
            hashtable["Pivot"] = _profilePivot;
            hashtable["ChosenIndexSortFriends"] = _chosenIndexSortFriends;
            hashtable["FriendTag"] = _selectedFriendTag;
            hashtable["PlaylistTrack"] = _selectedPlaylistTrack;
            ProfilePage profilePage = new ProfilePage(this._pivotPreference);
            profilePage.NavigationArguments = hashtable;
            return profilePage;
        }

        public IPage RestoreAndRelease() => this.Restore();

        public void Release()
        {
        }

        public bool CanBeTrimmed => true;
    }
}
