// Decompiled with JetBrains decompiler
// Type: ZuneUI.SchemaSyncGroup
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public abstract class SchemaSyncGroup : SyncGroup
    {
        private SyncCategory _type;
        private bool _expanded;

        public SchemaSyncGroup(SyncGroupList list, SyncCategory type)
          : base(list)
          => this._type = type;

        public override string Title
        {
            get
            {
                switch (this.Type)
                {
                    case SyncCategory.Music:
                        return Shell.LoadString(StringId.IDS_ALL_MUSIC_SYNC_GROUP);
                    case SyncCategory.Video:
                        return Shell.LoadString(StringId.IDS_ALL_VIDEO_SYNC_GROUP);
                    case SyncCategory.Photo:
                        return Shell.LoadString(StringId.IDS_ALL_PHOTOS_SYNC_GROUP);
                    case SyncCategory.Podcast:
                        return Shell.LoadString(StringId.IDS_ALL_PODCASTS_SYNC_GROUP);
                    case SyncCategory.Friend:
                        return Shell.LoadString(StringId.IDS_ALL_FRIENDS_SYNC_GROUP);
                    case SyncCategory.Audiobook:
                        return Shell.LoadString(StringId.IDS_ALL_AUDIOBOOKS_SYNC_GROUP);
                    case SyncCategory.Channel:
                        return Shell.LoadString(StringId.IDS_ALL_CHANNELS_SYNC_GROUP);
                    case SyncCategory.Application:
                        return Shell.LoadString(StringId.IDS_ALL_APPLICATIONS_SYNC_GROUP);
                    case SyncCategory.Guest:
                        return Shell.LoadString(StringId.IDS_ALL_GUEST_SYNC_GROUP);
                    default:
                        return Shell.LoadString(StringId.IDS_GENERIC_ERROR);
                }
            }
        }

        public override int ID => -1;

        public override SyncCategory Type => this._type;

        public string TypeName
        {
            get
            {
                switch (this.Type)
                {
                    case SyncCategory.Music:
                        return Shell.LoadString(StringId.IDS_MUSIC);
                    case SyncCategory.Video:
                        return Shell.LoadString(StringId.IDS_VIDEOS);
                    case SyncCategory.Photo:
                        return Shell.LoadString(StringId.IDS_PICTURES);
                    case SyncCategory.Podcast:
                        return Shell.LoadString(StringId.IDS_PODCASTS);
                    case SyncCategory.Friend:
                        return Shell.LoadString(StringId.IDS_FRIENDS);
                    case SyncCategory.Channel:
                        return Shell.LoadString(StringId.IDS_CHANNELS);
                    case SyncCategory.Application:
                        return Shell.LoadString(StringId.IDS_APPLICATIONS);
                    default:
                        return Shell.LoadString(StringId.IDS_GENERIC_ERROR);
                }
            }
        }

        public override bool IsComplex => false;

        public bool IsExpanded
        {
            get => this._expanded;
            set
            {
                if (this._expanded == value)
                    return;
                this._expanded = value;
                this.FirePropertyChanged(nameof(IsExpanded));
            }
        }

        public override void DataEdited()
        {
        }
    }
}
