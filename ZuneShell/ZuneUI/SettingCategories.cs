// Decompiled with JetBrains decompiler
// Type: ZuneUI.SettingCategories
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;

namespace ZuneUI
{
    public class SettingCategories
    {
        private static Category _accountLinks;
        private static Category _burn;
        private static Category _display;
        private static Category _collection;
        private static Category _devices;
        private static Category _filetype;
        private static Category _firmwareUpdate;
        private static Category _general;
        private static Category _nameDevice;
        private static Category _deviceMarketplace;
        private static Category _devicePrivacy;
        private static Category _photo;
        private static Category _podcast;
        private static Category _privacy;
        private static Category _purchaseHistory;
        private static Category _metadata;
        private static Category _rentalHistory;
        private static Category _rip;
        private static Category _sharing;
        private static Category _pictureVideo;
        private static Category _moreOneWeb;
        private static Category _spaceReservation;
        private static Category _subscriptionHistory;
        private static Category _syncGroups;
        private static Category _syncOptions;
        private static Category _transcoding;
        private static Category _wirelessSetup;
        private static Category _wirelessSetupMobile;
        private static Category _wirelessSetupDevice;
        private static Category _wirelessSetupComplete;
        private static Category _wirelessSetupManual;
        private static Category _wirelessSetupNotConfigured;
        private static Category _wirelessSetupCurrentConfig;
        private static Category _wirelessSetupDeviceBusy;

        public static Category AccountLinks
        {
            get
            {
                if (_accountLinks == null)
                    _accountLinks = new Category(StringId.IDS_ACCOUNT_MENU_ITEM, "res://ZuneShellResources!ManagementAccount.uix");
                return _accountLinks;
            }
        }

        public static Category Burn
        {
            get
            {
                if (_burn == null)
                    _burn = new Category(StringId.IDS_BURN_MENU_ITEM, "res://ZuneShellResources!ManagementBurn.uix");
                return _burn;
            }
        }

        public static Category Display
        {
            get
            {
                if (_display == null)
                    _display = new Category(StringId.IDS_DISPLAY_MENU_ITEM, "res://ZuneShellResources!ManagementDisplay.uix");
                return _display;
            }
        }

        public static Category Collection
        {
            get
            {
                if (_collection == null)
                    _collection = new Category(StringId.IDS_COLLECTION_MENU_ITEM, "res://ZuneShellResources!ManagementCollection.uix");
                return _collection;
            }
        }

        public static Category Devices
        {
            get
            {
                if (_devices == null)
                    _devices = new Category(StringId.IDS_MARKETPLACE_MENU_ITEM, "res://ZuneShellResources!ManagementDevices.uix");
                return _devices;
            }
        }

        public static Category Filetype
        {
            get
            {
                if (_filetype == null)
                    _filetype = new Category(StringId.IDS_FILE_TYPE_MENU_ITEM, "res://ZuneShellResources!ManagementFileTypes.uix");
                return _filetype;
            }
        }

        public static Category FirmwareUpdate
        {
            get
            {
                if (_firmwareUpdate == null)
                    _firmwareUpdate = new Category(StringId.IDS_FIRMWARE_UPDATE_HEADER, "res://ZuneShellResources!DeviceFirmware.uix#Default");
                return _firmwareUpdate;
            }
        }

        public static Category General
        {
            get
            {
                if (_general == null)
                    _general = new Category(StringId.IDS_GENERAL_MENU_ITEM, "res://ZuneShellResources!ManagementGeneral.uix");
                return _general;
            }
        }

        public static Category NameDevice
        {
            get
            {
                if (_nameDevice == null)
                    _nameDevice = new Category(StringId.IDS_NAME_ZUNE_HEADER, "res://ZuneShellResources!DeviceName.uix#Default");
                return _nameDevice;
            }
        }

        public static Category DeviceMarketplace
        {
            get
            {
                if (_deviceMarketplace == null)
                    _deviceMarketplace = new Category(StringId.IDS_ENABLE_MARKETPLACE_ON_DEVICE_HEADER, "res://ZuneShellResources!DeviceMarketplace.uix#Default");
                return _deviceMarketplace;
            }
        }

        public static Category Photo
        {
            get
            {
                if (_photo == null)
                    _photo = new Category(StringId.IDS_PHOTO_MENU_ITEM, "res://ZuneShellResources!ManagementPhoto.uix");
                return _photo;
            }
        }

        public static Category Podcast
        {
            get
            {
                if (_podcast == null)
                    _podcast = new Category(StringId.IDS_PODCAST_MENU_ITEM, "res://ZuneShellResources!ManagementPodcast.uix");
                return _podcast;
            }
        }

        public static Category PurchaseHistory
        {
            get
            {
                if (_purchaseHistory == null)
                    _purchaseHistory = new Category(StringId.IDS_BILLING_PURCHASE_HISTORY_HEADER, "res://ZuneShellResources!ManagementPurchases.uix", false);
                return _purchaseHistory;
            }
        }

        public static Category Privacy
        {
            get
            {
                if (_privacy == null)
                    _privacy = new Category(StringId.IDS_PRIVACY_MENU_ITEM, "res://ZuneShellResources!ManagementPrivacy.uix");
                return _privacy;
            }
        }

        public static Category RentalHistory
        {
            get
            {
                if (_rentalHistory == null)
                    _rentalHistory = new Category(StringId.IDS_BILLING_RENTAL_HISTORY_HEADER, "res://ZuneShellResources!ManagementRentals.uix", false);
                return _rentalHistory;
            }
        }

        public static Category Rip
        {
            get
            {
                if (_rip == null)
                    _rip = new Category(StringId.IDS_RIP_MENU_ITEM, "res://ZuneShellResources!ManagementRip.uix");
                return _rip;
            }
        }

        public static Category Metadata
        {
            get
            {
                if (_metadata == null)
                    _metadata = new Category(FeatureEnablement.IsFeatureEnabled(Features.eSocial) ? StringId.IDS_METADATA_AND_RATINGS_MENU_ITEM : StringId.IDS_METADATA_MENU_ITEM, "res://ZuneShellResources!ManagementMetadata.uix");
                return _metadata;
            }
        }

        public static Category Sharing
        {
            get
            {
                if (_sharing == null)
                    _sharing = new Category(StringId.IDS_SHARING_MENU_ITEM, "res://ZuneShellResources!ManagementSharing.uix");
                return _sharing;
            }
        }

        public static Category PictureVideo
        {
            get
            {
                if (_pictureVideo == null)
                    _pictureVideo = new Category(StringId.IDS_DEVICE_SETTINGS_PICTURES, "res://ZuneShellResources!DevicePictureVideo.uix#Default");
                return _pictureVideo;
            }
        }

        public static Category MoreOnWeb
        {
            get
            {
                if (_moreOneWeb == null)
                    _moreOneWeb = new Category(StringId.IDS_DEVICE_SETTINGS_MORE_ON_WEB, "res://ZuneShellResources!DeviceMoreOnWeb.uix#Default");
                return _moreOneWeb;
            }
        }

        public static Category SpaceReservation
        {
            get
            {
                if (_spaceReservation == null)
                    _spaceReservation = new Category(StringId.IDS_DEVICE_SETTINGS_RESERVATION, "res://ZuneShellResources!DeviceSpaceReservation.uix#Default");
                return _spaceReservation;
            }
        }

        public static Category SubscriptionHistory
        {
            get
            {
                if (_subscriptionHistory == null)
                    _subscriptionHistory = new Category(StringId.IDS_BILLING_DOWNLOAD_HISTORY_HEADER, "res://ZuneShellResources!ManagementSubscription.uix", false);
                return _subscriptionHistory;
            }
        }

        public static Category SyncGroups
        {
            get
            {
                if (_syncGroups == null)
                    _syncGroups = new Category(StringId.IDS_SYNC_GROUPS_SETTINGS_HEADER, "res://ZuneShellResources!DeviceSyncGroups.uix#SyncGroups", false);
                return _syncGroups;
            }
        }

        public static Category SyncOptions
        {
            get
            {
                if (_syncOptions == null)
                    _syncOptions = new Category(StringId.IDS_SYNC_OPTIONS_HEADER, "res://ZuneShellResources!DeviceSyncOptions.uix#Default", false);
                return _syncOptions;
            }
        }

        public static Category Transcoding
        {
            get
            {
                if (_transcoding == null)
                    _transcoding = new Category(StringId.IDS_TRANSCODING_HEADER, "res://ZuneShellResources!DeviceTranscode.uix#Default");
                return _transcoding;
            }
        }

        public static Category WirelessSetup
        {
            get
            {
                if (_wirelessSetup == null)
                    _wirelessSetup = new Category(StringId.IDS_WIRELESS_SYNC_HEADER, "res://ZuneShellResources!DeviceWirelessSync.uix#Default");
                return _wirelessSetup;
            }
        }

        public static Category WirelessSetupMobile
        {
            get
            {
                if (_wirelessSetupMobile == null)
                    _wirelessSetupMobile = new Category(StringId.IDS_WIRELESS_SYNC_HEADER, "res://ZuneShellResources!DeviceWirelessSync.uix#MobileDefault");
                return _wirelessSetupMobile;
            }
        }

        public static Category WirelessSetupDevice
        {
            get
            {
                if (_wirelessSetupDevice == null)
                    _wirelessSetupDevice = new Category(StringId.IDS_WIRELESS_SYNC_HEADER, "res://ZuneShellResources!DeviceWirelessSync.uix#DeviceDefault");
                return _wirelessSetupDevice;
            }
        }

        public static Category WirelessSetupComplete
        {
            get
            {
                if (_wirelessSetupComplete == null)
                    _wirelessSetupComplete = new Category(StringId.IDS_WIRELESS_SYNC_HEADER, "res://ZuneShellResources!DeviceWirelessSync.uix#WirelessSyncComplete");
                return _wirelessSetupComplete;
            }
        }

        public static Category WirelessSetupManual
        {
            get
            {
                if (_wirelessSetupManual == null)
                    _wirelessSetupManual = new Category(StringId.IDS_WIRELESS_SYNC_HEADER, "res://ZuneShellResources!DeviceWirelessSync.uix#WirelessSyncManual");
                return _wirelessSetupManual;
            }
        }

        public static Category WirelessSetupNotConfigured
        {
            get
            {
                if (_wirelessSetupNotConfigured == null)
                    _wirelessSetupNotConfigured = new Category(StringId.IDS_WIRELESS_SYNC_HEADER, "res://ZuneShellResources!DeviceWirelessSync.uix#WirelessSyncNotConfigured");
                return _wirelessSetupNotConfigured;
            }
        }

        public static Category WirelessSetupCurrentConfig
        {
            get
            {
                if (_wirelessSetupCurrentConfig == null)
                    _wirelessSetupCurrentConfig = new Category(StringId.IDS_WIRELESS_SYNC_HEADER, "res://ZuneShellResources!DeviceWirelessSync.uix#WirelessSyncCurrentConfig");
                return _wirelessSetupCurrentConfig;
            }
        }

        public static Category WirelessSetupDeviceBusy
        {
            get
            {
                if (_wirelessSetupDeviceBusy == null)
                    _wirelessSetupDeviceBusy = new Category(StringId.IDS_WIRELESS_SYNC_HEADER, "res://ZuneShellResources!DeviceWirelessSync.uix#WirelessSyncDeviceBusy");
                return _wirelessSetupDeviceBusy;
            }
        }

        public static Category DevicePrivacy
        {
            get
            {
                if (_devicePrivacy == null)
                    _devicePrivacy = new Category(StringId.IDS_DEVICE_PRIVACY_MENU_ITEM, "res://ZuneShellResources!DevicePrivacy.uix#Default");
                return _devicePrivacy;
            }
        }
    }
}
