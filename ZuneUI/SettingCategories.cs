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
                if (SettingCategories._accountLinks == null)
                    SettingCategories._accountLinks = new Category(StringId.IDS_ACCOUNT_MENU_ITEM, "res://ZuneShellResources!ManagementAccount.uix");
                return SettingCategories._accountLinks;
            }
        }

        public static Category Burn
        {
            get
            {
                if (SettingCategories._burn == null)
                    SettingCategories._burn = new Category(StringId.IDS_BURN_MENU_ITEM, "res://ZuneShellResources!ManagementBurn.uix");
                return SettingCategories._burn;
            }
        }

        public static Category Display
        {
            get
            {
                if (SettingCategories._display == null)
                    SettingCategories._display = new Category(StringId.IDS_DISPLAY_MENU_ITEM, "res://ZuneShellResources!ManagementDisplay.uix");
                return SettingCategories._display;
            }
        }

        public static Category Collection
        {
            get
            {
                if (SettingCategories._collection == null)
                    SettingCategories._collection = new Category(StringId.IDS_COLLECTION_MENU_ITEM, "res://ZuneShellResources!ManagementCollection.uix");
                return SettingCategories._collection;
            }
        }

        public static Category Devices
        {
            get
            {
                if (SettingCategories._devices == null)
                    SettingCategories._devices = new Category(StringId.IDS_MARKETPLACE_MENU_ITEM, "res://ZuneShellResources!ManagementDevices.uix");
                return SettingCategories._devices;
            }
        }

        public static Category Filetype
        {
            get
            {
                if (SettingCategories._filetype == null)
                    SettingCategories._filetype = new Category(StringId.IDS_FILE_TYPE_MENU_ITEM, "res://ZuneShellResources!ManagementFileTypes.uix");
                return SettingCategories._filetype;
            }
        }

        public static Category FirmwareUpdate
        {
            get
            {
                if (SettingCategories._firmwareUpdate == null)
                    SettingCategories._firmwareUpdate = new Category(StringId.IDS_FIRMWARE_UPDATE_HEADER, "res://ZuneShellResources!DeviceFirmware.uix#Default");
                return SettingCategories._firmwareUpdate;
            }
        }

        public static Category General
        {
            get
            {
                if (SettingCategories._general == null)
                    SettingCategories._general = new Category(StringId.IDS_GENERAL_MENU_ITEM, "res://ZuneShellResources!ManagementGeneral.uix");
                return SettingCategories._general;
            }
        }

        public static Category NameDevice
        {
            get
            {
                if (SettingCategories._nameDevice == null)
                    SettingCategories._nameDevice = new Category(StringId.IDS_NAME_ZUNE_HEADER, "res://ZuneShellResources!DeviceName.uix#Default");
                return SettingCategories._nameDevice;
            }
        }

        public static Category DeviceMarketplace
        {
            get
            {
                if (SettingCategories._deviceMarketplace == null)
                    SettingCategories._deviceMarketplace = new Category(StringId.IDS_ENABLE_MARKETPLACE_ON_DEVICE_HEADER, "res://ZuneShellResources!DeviceMarketplace.uix#Default");
                return SettingCategories._deviceMarketplace;
            }
        }

        public static Category Photo
        {
            get
            {
                if (SettingCategories._photo == null)
                    SettingCategories._photo = new Category(StringId.IDS_PHOTO_MENU_ITEM, "res://ZuneShellResources!ManagementPhoto.uix");
                return SettingCategories._photo;
            }
        }

        public static Category Podcast
        {
            get
            {
                if (SettingCategories._podcast == null)
                    SettingCategories._podcast = new Category(StringId.IDS_PODCAST_MENU_ITEM, "res://ZuneShellResources!ManagementPodcast.uix");
                return SettingCategories._podcast;
            }
        }

        public static Category PurchaseHistory
        {
            get
            {
                if (SettingCategories._purchaseHistory == null)
                    SettingCategories._purchaseHistory = new Category(StringId.IDS_BILLING_PURCHASE_HISTORY_HEADER, "res://ZuneShellResources!ManagementPurchases.uix", false);
                return SettingCategories._purchaseHistory;
            }
        }

        public static Category Privacy
        {
            get
            {
                if (SettingCategories._privacy == null)
                    SettingCategories._privacy = new Category(StringId.IDS_PRIVACY_MENU_ITEM, "res://ZuneShellResources!ManagementPrivacy.uix");
                return SettingCategories._privacy;
            }
        }

        public static Category RentalHistory
        {
            get
            {
                if (SettingCategories._rentalHistory == null)
                    SettingCategories._rentalHistory = new Category(StringId.IDS_BILLING_RENTAL_HISTORY_HEADER, "res://ZuneShellResources!ManagementRentals.uix", false);
                return SettingCategories._rentalHistory;
            }
        }

        public static Category Rip
        {
            get
            {
                if (SettingCategories._rip == null)
                    SettingCategories._rip = new Category(StringId.IDS_RIP_MENU_ITEM, "res://ZuneShellResources!ManagementRip.uix");
                return SettingCategories._rip;
            }
        }

        public static Category Metadata
        {
            get
            {
                if (SettingCategories._metadata == null)
                    SettingCategories._metadata = new Category(FeatureEnablement.IsFeatureEnabled(Features.eSocial) ? StringId.IDS_METADATA_AND_RATINGS_MENU_ITEM : StringId.IDS_METADATA_MENU_ITEM, "res://ZuneShellResources!ManagementMetadata.uix");
                return SettingCategories._metadata;
            }
        }

        public static Category Sharing
        {
            get
            {
                if (SettingCategories._sharing == null)
                    SettingCategories._sharing = new Category(StringId.IDS_SHARING_MENU_ITEM, "res://ZuneShellResources!ManagementSharing.uix");
                return SettingCategories._sharing;
            }
        }

        public static Category PictureVideo
        {
            get
            {
                if (SettingCategories._pictureVideo == null)
                    SettingCategories._pictureVideo = new Category(StringId.IDS_DEVICE_SETTINGS_PICTURES, "res://ZuneShellResources!DevicePictureVideo.uix#Default");
                return SettingCategories._pictureVideo;
            }
        }

        public static Category MoreOnWeb
        {
            get
            {
                if (SettingCategories._moreOneWeb == null)
                    SettingCategories._moreOneWeb = new Category(StringId.IDS_DEVICE_SETTINGS_MORE_ON_WEB, "res://ZuneShellResources!DeviceMoreOnWeb.uix#Default");
                return SettingCategories._moreOneWeb;
            }
        }

        public static Category SpaceReservation
        {
            get
            {
                if (SettingCategories._spaceReservation == null)
                    SettingCategories._spaceReservation = new Category(StringId.IDS_DEVICE_SETTINGS_RESERVATION, "res://ZuneShellResources!DeviceSpaceReservation.uix#Default");
                return SettingCategories._spaceReservation;
            }
        }

        public static Category SubscriptionHistory
        {
            get
            {
                if (SettingCategories._subscriptionHistory == null)
                    SettingCategories._subscriptionHistory = new Category(StringId.IDS_BILLING_DOWNLOAD_HISTORY_HEADER, "res://ZuneShellResources!ManagementSubscription.uix", false);
                return SettingCategories._subscriptionHistory;
            }
        }

        public static Category SyncGroups
        {
            get
            {
                if (SettingCategories._syncGroups == null)
                    SettingCategories._syncGroups = new Category(StringId.IDS_SYNC_GROUPS_SETTINGS_HEADER, "res://ZuneShellResources!DeviceSyncGroups.uix#SyncGroups", false);
                return SettingCategories._syncGroups;
            }
        }

        public static Category SyncOptions
        {
            get
            {
                if (SettingCategories._syncOptions == null)
                    SettingCategories._syncOptions = new Category(StringId.IDS_SYNC_OPTIONS_HEADER, "res://ZuneShellResources!DeviceSyncOptions.uix#Default", false);
                return SettingCategories._syncOptions;
            }
        }

        public static Category Transcoding
        {
            get
            {
                if (SettingCategories._transcoding == null)
                    SettingCategories._transcoding = new Category(StringId.IDS_TRANSCODING_HEADER, "res://ZuneShellResources!DeviceTranscode.uix#Default");
                return SettingCategories._transcoding;
            }
        }

        public static Category WirelessSetup
        {
            get
            {
                if (SettingCategories._wirelessSetup == null)
                    SettingCategories._wirelessSetup = new Category(StringId.IDS_WIRELESS_SYNC_HEADER, "res://ZuneShellResources!DeviceWirelessSync.uix#Default");
                return SettingCategories._wirelessSetup;
            }
        }

        public static Category WirelessSetupMobile
        {
            get
            {
                if (SettingCategories._wirelessSetupMobile == null)
                    SettingCategories._wirelessSetupMobile = new Category(StringId.IDS_WIRELESS_SYNC_HEADER, "res://ZuneShellResources!DeviceWirelessSync.uix#MobileDefault");
                return SettingCategories._wirelessSetupMobile;
            }
        }

        public static Category WirelessSetupDevice
        {
            get
            {
                if (SettingCategories._wirelessSetupDevice == null)
                    SettingCategories._wirelessSetupDevice = new Category(StringId.IDS_WIRELESS_SYNC_HEADER, "res://ZuneShellResources!DeviceWirelessSync.uix#DeviceDefault");
                return SettingCategories._wirelessSetupDevice;
            }
        }

        public static Category WirelessSetupComplete
        {
            get
            {
                if (SettingCategories._wirelessSetupComplete == null)
                    SettingCategories._wirelessSetupComplete = new Category(StringId.IDS_WIRELESS_SYNC_HEADER, "res://ZuneShellResources!DeviceWirelessSync.uix#WirelessSyncComplete");
                return SettingCategories._wirelessSetupComplete;
            }
        }

        public static Category WirelessSetupManual
        {
            get
            {
                if (SettingCategories._wirelessSetupManual == null)
                    SettingCategories._wirelessSetupManual = new Category(StringId.IDS_WIRELESS_SYNC_HEADER, "res://ZuneShellResources!DeviceWirelessSync.uix#WirelessSyncManual");
                return SettingCategories._wirelessSetupManual;
            }
        }

        public static Category WirelessSetupNotConfigured
        {
            get
            {
                if (SettingCategories._wirelessSetupNotConfigured == null)
                    SettingCategories._wirelessSetupNotConfigured = new Category(StringId.IDS_WIRELESS_SYNC_HEADER, "res://ZuneShellResources!DeviceWirelessSync.uix#WirelessSyncNotConfigured");
                return SettingCategories._wirelessSetupNotConfigured;
            }
        }

        public static Category WirelessSetupCurrentConfig
        {
            get
            {
                if (SettingCategories._wirelessSetupCurrentConfig == null)
                    SettingCategories._wirelessSetupCurrentConfig = new Category(StringId.IDS_WIRELESS_SYNC_HEADER, "res://ZuneShellResources!DeviceWirelessSync.uix#WirelessSyncCurrentConfig");
                return SettingCategories._wirelessSetupCurrentConfig;
            }
        }

        public static Category WirelessSetupDeviceBusy
        {
            get
            {
                if (SettingCategories._wirelessSetupDeviceBusy == null)
                    SettingCategories._wirelessSetupDeviceBusy = new Category(StringId.IDS_WIRELESS_SYNC_HEADER, "res://ZuneShellResources!DeviceWirelessSync.uix#WirelessSyncDeviceBusy");
                return SettingCategories._wirelessSetupDeviceBusy;
            }
        }

        public static Category DevicePrivacy
        {
            get
            {
                if (SettingCategories._devicePrivacy == null)
                    SettingCategories._devicePrivacy = new Category(StringId.IDS_DEVICE_PRIVACY_MENU_ITEM, "res://ZuneShellResources!DevicePrivacy.uix#Default");
                return SettingCategories._devicePrivacy;
            }
        }
    }
}
