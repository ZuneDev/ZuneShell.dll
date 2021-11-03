// Decompiled with JetBrains decompiler
// Type: ZuneUI.ExistingSyncGroup
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Playlist;
using MicrosoftZuneLibrary;
using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public class ExistingSyncGroup : SyncGroup
    {
        private SyncRuleDetails _details;
        private SyncRulesView _view;
        private int _index;
        private bool _isExpanded;
        private string _title;
        private AutoPlaylistBuilder _originalComplexRuleSetup;
        private string _originalComplexRuleName;
        private SyncCategory _complexType;
        private bool _isEdited;

        public ExistingSyncGroup(SyncGroupList parentList, SyncRulesView view, int index)
          : this(parentList, view, index, false)
        {
        }

        public ExistingSyncGroup(
          SyncGroupList parentList,
          SyncRulesView view,
          int index,
          bool isExpandedEntry)
          : base(parentList)
        {
            this._details = view.GetItem(index);
            this._view = view;
            this._index = index;
            this._isExpanded = isExpandedEntry;
            this.UpdateTitle();
            if (!this.IsComplex)
                return;
            this.UpdateComplexType();
            this._originalComplexRuleName = this.Title;
            this._originalComplexRuleSetup = new AutoPlaylistBuilder(this.ID);
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing && this._originalComplexRuleSetup != null)
                this._originalComplexRuleSetup.Dispose();
            base.OnDispose(disposing);
        }

        public override string Title => this._title;

        public override int ID => this._details.mediaId;

        public override SyncCategory Type => this.IsComplex ? this._complexType : (SyncCategory)this._details.syncCategory;

        public override long Size => !this._details.calculated || this._isEdited ? -1L : this._details.totalSize;

        public override int Count => !this._details.calculated || this._isEdited ? -1 : (int)this._details.totalItems;

        public override SyncGroupState State
        {
            get
            {
                if (this._isEdited)
                    return SyncGroupState.Pending;
                return !this._details.calculated ? SyncGroupState.Uncalculated : SyncGroupState.Calculated;
            }
        }

        public override bool IsActive
        {
            get => this._details.included;
            set
            {
                this._view.UpdateItem(this._index, value);
                this.DataUpdated();
            }
        }

        public override bool IsVisible => !this._details.ignore;

        public override bool IsComplex => this._details.complex;

        public override void CommitChanges()
        {
            if (!this.IsVisible || this.IsActive)
                return;
            MediaIdAndType[] mediaIdAndTypeArray = new MediaIdAndType[1]
            {
        new MediaIdAndType(this.ID, (MediaType) this._details.mediaType)
            };
            if (this._isExpanded)
            {
                this.ParentList.Device.DeleteAndExclude(mediaIdAndTypeArray);
            }
            else
            {
                this.ParentList.Device.RemoveSyncRule(mediaIdAndTypeArray);
                if (!this.IsComplex)
                    return;
                Shell.DeleteMedia(new List<MediaIdAndType>(1)
        {
          new MediaIdAndType(this.ID, MediaType.Playlist)
        }, false);
            }
        }

        public override void CancelChanges()
        {
            if (!this.IsComplex || !this._isEdited)
                return;
            if (this.Title != this._originalComplexRuleName)
                PlaylistManager.Instance.RenamePlaylist(this.ID, this._originalComplexRuleName);
            this._originalComplexRuleSetup.SetRules(this.ID);
        }

        public override void DataUpdated()
        {
            this._details = this._view.GetItem(this._index);
            this.UpdateTitle();
            this.FirePropertyChanged("Size");
            this.FirePropertyChanged("Count");
            this.FirePropertyChanged("State");
            this.FirePropertyChanged("IsActive");
            this.FirePropertyChanged("IsVisible");
            if (!(this.ParentList.GetGroupForSchema(this.Type) is DetailsBackedSchemaSyncGroup groupForSchema))
                return;
            groupForSchema.UpdateActiveState();
        }

        public override void DataEdited()
        {
            this._isEdited = true;
            this.UpdateComplexType();
            this.DataUpdated();
        }

        private void UpdateTitle()
        {
            if (this.IsComplex)
            {
                this._title = PlaylistManager.GetPlaylistName(this.ID);
            }
            else
            {
                EListType listType = EListType.eDeviceContentList;
                SchemaMap schemaMap = SchemaMap.kiIndex_Title;
                string format = "{0}";
                switch ((MediaType)this._details.mediaType)
                {
                    case MediaType.Track:
                        listType = EListType.eTrackList;
                        format = Shell.LoadString(StringId.IDS_TRACK_RULE_BASE);
                        break;
                    case MediaType.Video:
                        listType = EListType.eVideoList;
                        format = Shell.LoadString(StringId.IDS_VIDEO_RULE_BASE);
                        break;
                    case MediaType.Photo:
                        listType = EListType.ePhotoList;
                        format = Shell.LoadString(StringId.IDS_PHOTO_RULE_BASE);
                        break;
                    case MediaType.Playlist:
                        listType = EListType.ePlaylistList;
                        format = Shell.LoadString(StringId.IDS_PLAYLIST_RULE_BASE);
                        break;
                    case MediaType.Album:
                        listType = EListType.eAlbumList;
                        schemaMap = SchemaMap.kiIndex_WMAlbumTitle;
                        format = Shell.LoadString(StringId.IDS_ALBUM_RULE_BASE);
                        break;
                    case MediaType.PodcastEpisode:
                        listType = EListType.ePodcastEpisodeList;
                        format = Shell.LoadString(StringId.IDS_PODCAST_EPISODE_RULE_BASE);
                        break;
                    case MediaType.Podcast:
                        listType = EListType.ePodcastList;
                        format = Shell.LoadString(StringId.IDS_PODCAST_SERIES_RULE_BASE);
                        break;
                    case MediaType.MediaFolder:
                        listType = EListType.eFolderList;
                        schemaMap = SchemaMap.kiIndex_SourceURL;
                        format = Shell.LoadString(StringId.IDS_FOLDER_RULE_BASE);
                        break;
                    case MediaType.Genre:
                        listType = EListType.eGenreList;
                        schemaMap = SchemaMap.kiIndex_WMGenre;
                        format = Shell.LoadString(StringId.IDS_GENRE_RULE_BASE);
                        break;
                    case MediaType.PlaylistChannel:
                        listType = EListType.ePlaylistList;
                        format = "{0}";
                        break;
                    case MediaType.Artist:
                        listType = EListType.eArtistList;
                        schemaMap = SchemaMap.kiIndex_WMAlbumArtist;
                        format = Shell.LoadString(StringId.IDS_ARTIST_RULE_BASE);
                        break;
                    case MediaType.UserCard:
                        listType = EListType.eUserCardList;
                        format = Shell.LoadString(StringId.IDS_FRIEND_RULE_BASE);
                        break;
                    case MediaType.Application:
                        listType = EListType.eAppList;
                        format = Shell.LoadString(StringId.IDS_APPLICATION_RULE_BASE);
                        break;
                    default:
                        this._title = Shell.LoadString(StringId.IDS_GENERIC_ERROR);
                        break;
                }
                if (listType != EListType.eDeviceContentList)
                    this._title = string.Format(format, PlaylistManager.GetFieldValue(this.ID, listType, (int)schemaMap, Shell.LoadString(StringId.IDS_GENERIC_ERROR)));
            }
            this.FirePropertyChanged("Title");
        }

        private void UpdateComplexType()
        {
            this._complexType = UIDeviceList.MapMediaTypeToSyncCategory(PlaylistManager.GetAutoPlaylistSchema(this.ID));
            this.FirePropertyChanged("Type");
        }
    }
}
