// Decompiled with JetBrains decompiler
// Type: ZuneUI.SyncGroup
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneUI
{
    public abstract class SyncGroup : ModelItem, IComparable, IComparable<SyncGroup>
    {
        private SyncGroupList _parentList;

        public SyncGroup(SyncGroupList parentList)
          : base(parentList)
          => this._parentList = parentList;

        public abstract string Title { get; }

        public abstract int ID { get; }

        public abstract SyncCategory Type { get; }

        public abstract long Size { get; }

        public abstract int Count { get; }

        public abstract SyncGroupState State { get; }

        public abstract bool IsActive { get; set; }

        public abstract bool IsVisible { get; }

        public abstract bool IsComplex { get; }

        public abstract void CommitChanges();

        public abstract void CancelChanges();

        public abstract void DataUpdated();

        public abstract void DataEdited();

        public string TypeAsGroupDescription
        {
            get
            {
                switch (this.Type)
                {
                    case SyncCategory.Music:
                        return Shell.LoadString(StringId.IDS_SONGS);
                    case SyncCategory.Video:
                        return Shell.LoadString(StringId.IDS_VIDEOS);
                    case SyncCategory.Photo:
                        return Shell.LoadString(StringId.IDS_PICTURES);
                    case SyncCategory.Podcast:
                        return Shell.LoadString(StringId.IDS_PODCAST_EPISODES);
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

        public int CompareTo(object obj) => !(obj is SyncGroup other) ? 0 : this.CompareTo(other);

        public int CompareTo(SyncGroup other) => this.Size.CompareTo(other.Size) * -1;

        protected SyncGroupList ParentList => this._parentList;
    }
}
