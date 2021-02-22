// Decompiled with JetBrains decompiler
// Type: ZuneUI.SyncabilityHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class SyncabilityHelper
    {
        private UIDevice _device;
        private DelegateFuture<bool> _isGuest;
        private DelegateFuture<int> _userId;
        private DelegateFuture<bool> _supportsUserCards;
        private DelegateFuture<bool> _supportsChannels;
        private DelegateFuture<bool> _supportsApplications;
        private DelegateFuture<bool> _supportsRental;
        private DelegateFuture<bool> _supportsHD;
        private DelegateFuture<bool> _isUnlinked;
        private DelegateFuture<bool> _canSyncUserCards;
        private DelegateFuture<bool> _canSyncChannels;
        private DelegateFuture<bool> _canSyncApplications;
        private DelegateFuture<bool> _canSyncRentalVideo;
        private DelegateFuture<bool> _canSyncHDRentalVideo;

        public SyncabilityHelper(UIDevice device)
        {
            this._device = device ?? UIDeviceList.NullDevice;
            this._isGuest = new DelegateFuture<bool>((CalculateValue<bool>)(() => this._device.IsGuest));
            this._userId = new DelegateFuture<int>((CalculateValue<int>)(() => this._device.UserId));
            this._supportsUserCards = new DelegateFuture<bool>((CalculateValue<bool>)(() => this._device.SupportsUserCards));
            this._supportsChannels = new DelegateFuture<bool>((CalculateValue<bool>)(() => this._device.SupportsChannels));
            this._supportsApplications = new DelegateFuture<bool>((CalculateValue<bool>)(() => this._device.SupportsSyncApplications));
            this._supportsRental = new DelegateFuture<bool>((CalculateValue<bool>)(() => this._device.SupportsRental));
            this._supportsHD = new DelegateFuture<bool>((CalculateValue<bool>)(() => this._device.SupportsHD));
            this._isUnlinked = new DelegateFuture<bool>((CalculateValue<bool>)(() => this.UserID == 0));
            this._canSyncUserCards = new DelegateFuture<bool>(new CalculateValue<bool>(this.GetCanSyncUserCards));
            this._canSyncChannels = new DelegateFuture<bool>(new CalculateValue<bool>(this.GetCanSyncChannels));
            this._canSyncApplications = new DelegateFuture<bool>(new CalculateValue<bool>(this.GetCanSyncApplications));
            this._canSyncRentalVideo = new DelegateFuture<bool>(new CalculateValue<bool>(this.GetCanSyncRentalVideo));
            this._canSyncHDRentalVideo = new DelegateFuture<bool>(new CalculateValue<bool>(this.GetCanSyncHDRentalVideo));
        }

        public bool IsGuest => this._isGuest.Value;

        public int UserID => this._userId.Value;

        public bool SupportsUserCards => this._supportsUserCards.Value;

        public bool SupportsChannels => this._supportsChannels.Value;

        public bool SupportsSyncApplications => this._supportsApplications.Value;

        public bool SupportsRental => this._supportsRental.Value;

        public bool SupportsHD => this._supportsHD.Value;

        public bool IsUnlinked => this._isUnlinked.Value;

        public bool CanSyncUserCards => this._canSyncUserCards.Value;

        public bool CanSyncChannels => this._canSyncChannels.Value;

        public bool CanSyncApplications => this._canSyncApplications.Value;

        public bool CanSyncRentalVideo => this._canSyncRentalVideo.Value;

        public bool CanSyncHDRentalVideo => this._canSyncHDRentalVideo.Value;

        private bool GetCanSyncUserCards() => !this.IsGuest && this.UserID == SignIn.Instance.LastSignedInUserId && this.SupportsUserCards;

        private bool GetCanSyncChannels() => !this.IsGuest && this.UserID == SignIn.Instance.LastSignedInUserId && SignIn.Instance.LastSignedInUserHadActiveSubscription && this.SupportsChannels;

        private bool GetCanSyncApplications() => this.SupportsSyncApplications;

        private bool GetCanSyncRentalVideo() => this.SupportsRental;

        private bool GetCanSyncHDRentalVideo() => this.CanSyncRentalVideo && this.SupportsHD;
    }
}
