// Decompiled with JetBrains decompiler
// Type: ZuneUI.NewComplexSyncGroup
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public class NewComplexSyncGroup : SyncGroup
    {
        private string _title;
        private int _id;
        private bool _isActive;
        private SyncCategory _type;

        public NewComplexSyncGroup(SyncGroupList parentList, int id)
          : base(parentList)
        {
            this._id = id;
            this._isActive = true;
            this.UpdateInfo();
        }

        public override string Title => this._title;

        public override int ID => this._id;

        public override SyncCategory Type => this._type;

        public override long Size => -1;

        public override int Count => -1;

        public override SyncGroupState State => SyncGroupState.Pending;

        public override bool IsActive
        {
            get => this._isActive;
            set
            {
                if (this._isActive == value)
                    return;
                this._isActive = value;
                this.FirePropertyChanged(nameof(IsActive));
            }
        }

        public override bool IsVisible => true;

        public override bool IsComplex => true;

        public override void CommitChanges()
        {
            if (this.IsActive)
                this.ParentList.Device.AddSyncRule(new MediaIdAndType[1]
                {
          new MediaIdAndType(this.ID, MediaType.Playlist)
                });
            else
                this.CancelChanges();
        }

        public override void CancelChanges() => Shell.DeleteMedia(new List<MediaIdAndType>(1)
    {
      new MediaIdAndType(this.ID, MediaType.Playlist)
    }, false);

        public override void DataUpdated()
        {
        }

        public override void DataEdited() => this.UpdateInfo();

        private void UpdateInfo()
        {
            this._title = PlaylistManager.GetPlaylistName(this.ID);
            this.FirePropertyChanged("Title");
            this._type = UIDeviceList.MapMediaTypeToSyncCategory(PlaylistManager.GetAutoPlaylistSchema(this.ID));
            this.FirePropertyChanged("Type");
        }
    }
}
