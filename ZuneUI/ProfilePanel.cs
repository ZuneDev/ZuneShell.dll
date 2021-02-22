// Decompiled with JetBrains decompiler
// Type: ZuneUI.ProfilePanel
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;

namespace ZuneUI
{
    public class ProfilePanel : LibraryPanel
    {
        private string _zuneTag;
        private UserRelationship _relationshipToSignedInUser;
        private DataProviderObject _profileData;
        private PropertyEditProfile _propertyEditProfile;
        private IList _friends;
        private IList _pivotData;
        private Category _selectedPivot;
        private Guid _selectedPlaylistTrack;
        private int _chosenIndexSortFriends;
        private string _selectedFriendTag;
        private bool _canChangeSelectedItem;
        private bool _loaded;
        private bool _forceLoadStatus;

        internal ProfilePanel(
          ProfilePage page,
          string zuneTag,
          Category profilePivot,
          Guid playlistTrack,
          int chosenSortFriends,
          string selectedFriendTag)
          : base((IModelItemOwner)page)
        {
            this._zuneTag = zuneTag;
            this._selectedPivot = profilePivot;
            this._relationshipToSignedInUser = UserRelationship.Unknown;
            this._chosenIndexSortFriends = chosenSortFriends;
            this._selectedFriendTag = selectedFriendTag;
            this._selectedPlaylistTrack = playlistTrack;
            this._canChangeSelectedItem = true;
        }

        public string ZuneTag => string.IsNullOrEmpty(this._zuneTag) ? SignIn.Instance.ZuneTag : this._zuneTag;

        public Category SelectedPivot
        {
            get => this._selectedPivot;
            set
            {
                if (this._selectedPivot == value)
                    return;
                this._selectedPivot = value;
                this.FirePropertyChanged(nameof(SelectedPivot));
                this.SelectedPlaylistTrack = Guid.Empty;
            }
        }

        public Guid SelectedPlaylistTrack
        {
            get => this._selectedPlaylistTrack;
            set
            {
                if (!(this._selectedPlaylistTrack != value))
                    return;
                this._selectedPlaylistTrack = value;
                this.FirePropertyChanged(nameof(SelectedPlaylistTrack));
            }
        }

        public bool CanChangeSelectedItem
        {
            get => this._canChangeSelectedItem;
            set
            {
                if (this._canChangeSelectedItem == value)
                    return;
                this._canChangeSelectedItem = value;
                this.FirePropertyChanged(nameof(CanChangeSelectedItem));
            }
        }

        public string SelectedFriendTag
        {
            get => this._selectedFriendTag;
            set
            {
                if (!(this._selectedFriendTag != value))
                    return;
                this._selectedFriendTag = value;
                this.FirePropertyChanged(nameof(SelectedFriendTag));
            }
        }

        public int ChosenIndexSortFriends
        {
            get => this._chosenIndexSortFriends;
            set
            {
                if (this._chosenIndexSortFriends == value)
                    return;
                this._chosenIndexSortFriends = value;
                this.FirePropertyChanged(nameof(ChosenIndexSortFriends));
            }
        }

        public bool IsSignedInUser => this.RelationshipToSignedInUser == UserRelationship.Self;

        public UserRelationship RelationshipToSignedInUser
        {
            get
            {
                if (this._relationshipToSignedInUser == UserRelationship.Unknown && SignIn.Instance.IsSignedInUser(this.ZuneTag))
                    this.RelationshipToSignedInUser = UserRelationship.Self;
                return this._relationshipToSignedInUser;
            }
            set
            {
                if (this._relationshipToSignedInUser == value)
                    return;
                this._relationshipToSignedInUser = value;
                this.FirePropertyChanged(nameof(RelationshipToSignedInUser));
                this.FirePropertyChanged("IsSignedInUser");
            }
        }

        public DataProviderObject ProfileData
        {
            get => this._profileData;
            set
            {
                if (this._profileData == value)
                    return;
                this._profileData = value;
                this._propertyEditProfile = (PropertyEditProfile)null;
                this.FirePropertyChanged(nameof(ProfileData));
                this.FirePropertyChanged("PropertyEditProfile");
            }
        }

        public PropertyEditProfile PropertyEditProfile
        {
            get
            {
                if (this._propertyEditProfile == null)
                    this._propertyEditProfile = new PropertyEditProfile(this._profileData);
                return this._propertyEditProfile;
            }
        }

        public IList Friends
        {
            get => this._friends;
            set
            {
                if (this._friends == value)
                    return;
                this._friends = value;
                this.FirePropertyChanged(nameof(Friends));
            }
        }

        public IList PivotData
        {
            get => this._pivotData;
            set
            {
                if (this._pivotData == value)
                    return;
                this._pivotData = value;
                this.FirePropertyChanged(nameof(PivotData));
            }
        }

        public bool Loaded
        {
            get => this._loaded;
            set
            {
                if (this._loaded == value)
                    return;
                this._loaded = value;
                this.FirePropertyChanged(nameof(Loaded));
            }
        }

        public bool ForceLoadStatus
        {
            get => this._forceLoadStatus;
            set
            {
                if (this._forceLoadStatus == value)
                    return;
                this._forceLoadStatus = value;
                this.FirePropertyChanged(nameof(ForceLoadStatus));
            }
        }

        public int GetFriendIndexFromZuneTag(string tagToFind)
        {
            int num1 = -1;
            int num2 = 0;
            if (this._friends != null && !string.IsNullOrEmpty(tagToFind))
            {
                foreach (object friend in (IEnumerable)this._friends)
                {
                    if (friend is ProfileCardData profileCardData && SignIn.TagsMatch(profileCardData.ZuneTag, tagToFind))
                    {
                        num1 = num2;
                        break;
                    }
                    ++num2;
                }
            }
            return num1;
        }
    }
}
