// Decompiled with JetBrains decompiler
// Type: ZuneUI.GuestSchemaSyncGroup
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class GuestSchemaSyncGroup : SchemaSyncGroup
    {
        private long _size;
        private bool _active;

        public GuestSchemaSyncGroup(SyncGroupList list, long size)
          : base(list, SyncCategory.Guest)
        {
            this._size = size;
            this._active = true;
        }

        public override long Size => this._size;

        public override int Count => 0;

        public override SyncGroupState State => SyncGroupState.Calculated;

        public override bool IsActive
        {
            get => this._active;
            set
            {
                if (this._active == value)
                    return;
                this._active = value;
                this.FirePropertyChanged(nameof(IsActive));
                this.ParentList.GasGauge.HideGuestSpace = !this._active;
            }
        }

        public override bool IsVisible => true;

        public override void CommitChanges()
        {
            if (this._active)
                return;
            this.ParentList.Device.DeleteAllGuestContent();
        }

        public override void CancelChanges()
        {
        }

        public override void DataUpdated()
        {
        }
    }
}
