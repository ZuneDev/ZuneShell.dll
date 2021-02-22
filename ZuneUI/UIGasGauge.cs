// Decompiled with JetBrains decompiler
// Type: ZuneUI.UIGasGauge
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using MicrosoftZuneLibrary;

namespace ZuneUI
{
    public class UIGasGauge : ModelItem
    {
        private GasGauge _gauge;
        private int _dividerCount;
        private bool _hideGuestSpace;

        public UIGasGauge(IModelItemOwner owner, GasGauge gauge)
          : base(owner)
        {
            this._gauge = gauge;
            if (this._gauge != null)
            {
                this._gauge.CategorySpaceUsedUpdatedEvent += new CategorySpaceUsedUpdatedHandler(this.OnCategorySpaceUpdated);
                this._gauge.ReservedSpaceUpdatedEvent += new ReservedSpaceUpdatedHandler(this.OnReservedSpaceUpdated);
            }
            UIDevice uiDevice = owner as UIDevice;
            if (uiDevice == null && owner is SyncGroupList syncGroupList)
                uiDevice = syncGroupList.Device;
            if (uiDevice == null)
                return;
            int advertisedCapacity = uiDevice.AdvertisedCapacity;
            while (advertisedCapacity > 8)
            {
                if (advertisedCapacity % 10 == 0)
                    advertisedCapacity /= 10;
                else if (advertisedCapacity % 5 == 0)
                    advertisedCapacity /= 5;
                else if (advertisedCapacity % 3 == 0)
                    advertisedCapacity /= 3;
                else
                    advertisedCapacity /= 2;
            }
            this._dividerCount = advertisedCapacity;
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing && this._gauge != null)
            {
                this._gauge.CategorySpaceUsedUpdatedEvent -= new CategorySpaceUsedUpdatedHandler(this.OnCategorySpaceUpdated);
                this._gauge.ReservedSpaceUpdatedEvent -= new ReservedSpaceUpdatedHandler(this.OnReservedSpaceUpdated);
            }
            base.OnDispose(disposing);
        }

        public long TotalSpace => this._gauge == null ? 0L : (long)this._gauge.Capacity;

        public long MusicSpace => this._gauge == null ? 0L : this._gauge.GetSpaceUsedByCategory(ESyncCategory.eSyncCategoryMusic);

        public long VideoSpace => this._gauge == null ? 0L : this._gauge.GetSpaceUsedByCategory(ESyncCategory.eSyncCategoryVideo);

        public long PhotoSpace => this._gauge == null ? 0L : this._gauge.GetSpaceUsedByCategory(ESyncCategory.eSyncCategoryPhotos);

        public long PodcastSpace => this._gauge == null ? 0L : this._gauge.GetSpaceUsedByCategory(ESyncCategory.eSyncCategoryPodcasts);

        public long FriendSpace => this._gauge == null ? 0L : this._gauge.GetSpaceUsedByCategory(ESyncCategory.eSyncCategoryFriends);

        public long ChannelSpace => this._gauge == null ? 0L : this._gauge.GetSpaceUsedByCategory(ESyncCategory.eSyncCategoryChannels);

        public long AudiobookSpace => this._gauge == null ? 0L : this._gauge.GetSpaceUsedByCategory(ESyncCategory.eSyncCategoryAudiobooks);

        public long ApplicationSpace => this._gauge == null ? 0L : this._gauge.GetSpaceUsedByCategory(ESyncCategory.eSyncCategoryApps);

        public long GuestSpace => this._gauge == null || this._hideGuestSpace ? 0L : this._gauge.GetSpaceUsedByCategory(ESyncCategory.eSyncCategoryGuest);

        public long InboxSpace => this._gauge == null ? 0L : this._gauge.GetSpaceUsedByCategory(ESyncCategory.eSyncCategoryInbox);

        public long ReservedSpace => this._gauge == null ? 0L : this._gauge.SpaceReserved;

        public long OtherSpace
        {
            get
            {
                long num1 = 0;
                if (this._gauge != null)
                {
                    long num2 = this.ReservedSpace - this.AudiobookSpace;
                    num1 = (num2 > this.InboxSpace ? num2 : this.InboxSpace) + this.GuestSpace;
                }
                return num1;
            }
        }

        public long FreeSpace
        {
            get
            {
                long num = 0;
                if (this._gauge != null)
                {
                    num = this._gauge.SpaceAvailable;
                    if (this._hideGuestSpace)
                        num += this._gauge.GetSpaceUsedByCategory(ESyncCategory.eSyncCategoryGuest);
                }
                return num;
            }
        }

        public long UsedSpace => this.TotalSpace - this.FreeSpace;

        public int DividerCount => this._dividerCount;

        public bool HideGuestSpace
        {
            get => this._hideGuestSpace;
            set
            {
                if (this._hideGuestSpace == value)
                    return;
                this._hideGuestSpace = value;
                this.FirePropertyChanged(nameof(HideGuestSpace));
                this.FirePropertyChanged("GuestSpace");
                this.FirePropertyChanged("OtherSpace");
                this.FirePropertyChanged("UsedSpace");
                this.FirePropertyChanged("FreeSpace");
            }
        }

        private void OnCategorySpaceUpdated(
          GasGauge gasGauge,
          ESyncCategory category,
          long llNewSchemaSpace,
          long llNewFreeSpace)
        {
            Application.DeferredInvoke((DeferredInvokeHandler)delegate
           {
               if (this.IsDisposed)
                   return;
               this.FirePropertyChanged("UsedSpace");
               this.FirePropertyChanged("FreeSpace");
               this.FirePropertyChanged("OtherSpace");
               switch (category)
               {
                   case ESyncCategory.eSyncCategoryMusic:
                       this.FirePropertyChanged("MusicSpace");
                       break;
                   case ESyncCategory.eSyncCategoryVideo:
                       this.FirePropertyChanged("VideoSpace");
                       break;
                   case ESyncCategory.eSyncCategoryPhotos:
                       this.FirePropertyChanged("PhotoSpace");
                       break;
                   case ESyncCategory.eSyncCategoryPodcasts:
                       this.FirePropertyChanged("PodcastSpace");
                       break;
                   case ESyncCategory.eSyncCategoryFriends:
                       this.FirePropertyChanged("FriendSpace");
                       break;
                   case ESyncCategory.eSyncCategoryAudiobooks:
                       this.FirePropertyChanged("AudiobookSpace");
                       break;
                   case ESyncCategory.eSyncCategoryChannels:
                       this.FirePropertyChanged("ChannelSpace");
                       break;
                   case ESyncCategory.eSyncCategoryApps:
                       this.FirePropertyChanged("ApplicationSpace");
                       break;
                   case ESyncCategory.eSyncCategoryGuest:
                       this.FirePropertyChanged("GuestSpace");
                       break;
               }
           }, (object)null);
        }

        private void OnReservedSpaceUpdated(
          GasGauge gasGauge,
          long llNewReservedSpace,
          long llNewFreeSpace)
        {
            Application.DeferredInvoke((DeferredInvokeHandler)delegate
           {
               if (this.IsDisposed)
                   return;
               this.FirePropertyChanged("ReservedSpace");
               this.FirePropertyChanged("UsedSpace");
               this.FirePropertyChanged("FreeSpace");
               this.FirePropertyChanged("OtherSpace");
           }, (object)null);
        }
    }
}
