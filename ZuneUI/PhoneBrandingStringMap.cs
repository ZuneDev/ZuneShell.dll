// Decompiled with JetBrains decompiler
// Type: ZuneUI.PhoneBrandingStringMap
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections.Generic;

namespace ZuneUI
{
    internal class PhoneBrandingStringMap
    {
        private static readonly PhoneBrandingStringMap _instance = new PhoneBrandingStringMap();
        protected bool _isBrandingEnabled;
        protected Dictionary<StringId, StringId> _stringMap = new Dictionary<StringId, StringId>();

        protected PhoneBrandingStringMap()
        {
        }

        static PhoneBrandingStringMap() => PhoneBrandingStringMap._instance.Initialize();

        public static PhoneBrandingStringMap Instance => PhoneBrandingStringMap._instance;

        public bool BrandingEnabled
        {
            get => this._isBrandingEnabled;
            set
            {
                if (this._isBrandingEnabled == value)
                    return;
                this._isBrandingEnabled = value;
            }
        }

        public StringId TryGetMappedStringId(StringId stringId)
        {
            StringId stringId1;
            if (this._stringMap.TryGetValue(stringId, out stringId1))
                stringId = stringId1;
            return stringId;
        }

        public virtual void Initialize()
        {
            this._stringMap[StringId.IDS_FIRMWARE_CURRENT_VERSION] = StringId.IDS_PHONE_CURRENT_VERSION;
            this._stringMap[StringId.IDS_LIBRARY_QUESTION_N_ITEMS] = StringId.IDS_PHONE_LIBRARY_QUESTION_N_ITEMS;
            this._stringMap[StringId.IDS_FIRMWARE_UPDATE_WARNING] = StringId.IDS_PHONE_FIRMWARE_UPDATE_WARNING;
            this._stringMap[StringId.IDS_FIRMWARE_INSTALLING] = StringId.IDS_PHONE_FIRMWARE_INSTALLING;
            this._stringMap[StringId.IDS_FIRMWARE_UPDATE_UNAVAILABLE] = StringId.IDS_PHONE_FIRMWARE_UPDATE_UNAVAILABLE;
            this._stringMap[StringId.IDS_ASSOCIATE_ZUNE_OFFER_LOGGEDOUT] = StringId.IDS_PHONE_ASSOCIATE_ZUNE_OFFER_LOGGEDOUT;
            this._stringMap[StringId.IDS_ASSOCIATE_ZUNE_OFFER_LOGGEDIN] = StringId.IDS_PHONE_ASSOCIATE_ZUNE_OFFER_LOGGEDIN;
            this._stringMap[StringId.IDS_ASSOCIATE_ZUNE_OFFER_LOGGEDIN_YES] = StringId.IDS_PHONE_ASSOCIATE_ZUNE_OFFER_LOGGEDIN_YES;
            this._stringMap[StringId.IDS_ASSOCIATED_ZUNE] = StringId.IDS_PHONE_ASSOCIATED_ZUNE;
            this._stringMap[StringId.IDS_DELETE_DEVICE_BUTTON] = StringId.IDS_PHONE_DELETE_DEVICE_BUTTON;
            this._stringMap[StringId.IDS_FORMAT_IN_PROGRESS] = StringId.IDS_PHONE_FORMAT_IN_PROGRESS;
            this._stringMap[StringId.IDS_WIRELESS_SYNC_TEST_FAILED] = StringId.IDS_PHONE_WIRELESS_SYNC_TEST_FAILED;
            this._stringMap[StringId.IDS_FORMAT_DIALOG_TEXT] = StringId.IDS_PHONE_FORMAT_DIALOG_TEXT;
            this._stringMap[StringId.IDS_WIRELESS_SUGG_MAC_FILTERING] = StringId.IDS_PHONE_WIRELESS_SUGG_MAC_FILTERING;
            this._stringMap[StringId.IDS_FIRMWARE_NEW_VERSION_OPTIONAL] = StringId.IDS_PHONE_FIRMWARE_NEW_VERSION_OPTIONAL;
            this._stringMap[StringId.IDS_WIRELESS_ALREADY_CONFIGURED] = StringId.IDS_PHONE_WIRELESS_ALREADY_CONFIGURED;
            this._stringMap[StringId.IDS_WIRELESS_DEVICE_BUSY] = StringId.IDS_PHONE_WIRELESS_DEVICE_BUSY;
            this._stringMap[StringId.IDS_EULA_DIALOG_TITLE] = StringId.IDS_PHONE_EULA_DIALOG_TITLE;
            this._stringMap[StringId.IDS_EULA_DIALOG_TITLE_REQUIRED] = StringId.IDS_PHONE_EULA_DIALOG_TITLE_REQUIRED;
            this._stringMap[StringId.IDS_FIRMWARE_NEW_VERSION_REQUIRED] = StringId.IDS_PHONE_FIRMWARE_NEW_VERSION;
            this._stringMap[StringId.IDS_EULA_DIALOG_TEXTAREA_TITLE] = StringId.IDS_PHONE_EULA_DIALOG_TEXTAREA_TITLE;
            this._stringMap[StringId.IDS_FIRMWARE_UPDATE_AVAILABLE_TITLE] = StringId.IDS_PHONE_UPDATE_AVAILABLE_TITLE;
            this._stringMap[StringId.IDS_SPACE_RESERVATION_DESCRIPTION] = StringId.IDS_PHONE_SPACE_RESERVATION_DESCRIPTION;
            this._stringMap[StringId.IDS_SPACE_RESERVATION_DESCRIPTION_NO_FEATURES] = StringId.IDS_PHONE_SPACE_RESERVATION_DESCRIPTION_NO_MARKETPLACE;
            this._stringMap[StringId.IDS_SPACE_RESERVATION_DESCRIPTION_NO_MARKETPLACE] = StringId.IDS_PHONE_SPACE_RESERVATION_DESCRIPTION_NO_MARKETPLACE;
            this._stringMap[StringId.IDS_SPACE_RESERVATION_DESCRIPTION_NO_SOCIAL] = StringId.IDS_PHONE_SPACE_RESERVATION_DESCRIPTION;
            this._stringMap[StringId.IDS_FIRMWARE_UPDATE_SUCCESS_NOTICE] = StringId.IDS_DEVICE_RESTORE_MAY_DISCONNECT;
            this._stringMap[StringId.IDS_FIRMWARE_UPDATE_SUCCESS] = StringId.IDS_PHONE_UPDATE_SUCCESS;
            this._stringMap[StringId.IDS_MUSIC_DEVICE_EMPTY] = StringId.IDS_PHONE_MUSIC_DEVICE_EMPTY;
            this._stringMap[StringId.IDS_VIDEO_DEVICE_EMPTY] = StringId.IDS_PHONE_VIDEO_DEVICE_EMPTY;
            this._stringMap[StringId.IDS_PHOTO_DEVICE_EMPTY] = StringId.IDS_PHONE_PHOTO_DEVICE_EMPTY;
            this._stringMap[StringId.IDS_PLAYLIST_DEVICE_EMPTY_TITLE] = StringId.IDS_PHONE_PLAYLIST_DEVICE_EMPTY_TITLE;
            this._stringMap[StringId.IDS_FIRMWARE_TRANSFERRING] = StringId.IDS_PHONE_FIRMWARE_TRANSFERRING;
            this._stringMap[StringId.IDS_FIRMWARE_VERSION_PRE_TEXT] = StringId.IDS_PHONE_FIRMWARE_VERSION_PRE_TEXT;
            this._stringMap[StringId.IDS_ASSOCIATE_YOUR_ZUNE_DESCRIPTION2] = StringId.IDS_PHONE_ASSOCIATE_YOUR_ZUNE_DESCRIPTION2;
            this._stringMap[StringId.IDS_NO_SYNC_OPTIONS_INFO] = StringId.IDS_PHONE_NO_SYNC_OPTIONS_INFO;
            this._stringMap[StringId.IDS_TRANSCODE_VIDEO_PLAYBACK_OPTION] = StringId.IDS_PHONE_TRANSCODE_VIDEO_PLAYBACK_OPTION;
            this._stringMap[StringId.IDS_REMOVE_SINGLE_SYNC_GROUP_DIALOG_TEXT] = StringId.IDS_PHONE_REMOVE_SINGLE_SYNC_GROUP_DIALOG_TEXT;
            this._stringMap[StringId.IDS_REMOVE_MULTIPLE_SYNC_GROUP_DIALOG_TEXT] = StringId.IDS_PHONE_REMOVE_MULTIPLE_SYNC_GROUP_DIALOG_TEXT;
            this._stringMap[StringId.IDS_REMOVE_SYNC_ALL_GROUP_DIALOG_TEXT] = StringId.IDS_PHONE_REMOVE_SYNC_ALL_GROUP_DIALOG_TEXT;
            this._stringMap[StringId.IDS_FIRMWARE_UPDATE_SUCCESS] = StringId.IDS_PHONE_FIRMWARE_UPDATE_SUCCESS;
            this._stringMap[StringId.IDS_ENABLE_ZUNE_MARKETPLACE_TEXT] = StringId.IDS_PHONE_ENABLE_ZUNE_MARKETPLACE_TEXT;
            this._stringMap[StringId.IDS_ENABLE_ZUNE_MARKETPLACE_OPTION] = StringId.IDS_PHONE_ENABLE_ZUNE_MARKETPLACE_OPTION;
            this._stringMap[StringId.IDS_PICK_A_NAME_HEADER] = StringId.IDS_PHONE_PICK_A_NAME_HEADER;
            this._stringMap[StringId.IDS_DISABLE_MARKETPLACE_BUTTON] = StringId.IDS_PHONE_DISABLE_MARKETPLACE_BUTTON;
            this._stringMap[StringId.IDS_FIRMWARE_UPDATE_ERROR_NOTICE2] = StringId.IDS_PHONE_FIRMWARE_UPDATE_ERROR_NOTICE2;
            this._stringMap[StringId.IDS_FIRMWARE_NEW_VERSION_REQUIRED] = StringId.IDS_PHONE_FIRMWARE_NEW_VERSION_REQUIRED;
            this._stringMap[StringId.IDS_FIRMWARE_NOTICE] = StringId.IDS_PHONE_FIRMWARE_NOTICE;
            this._stringMap[StringId.IDS_FIRMWARE_UPDATE_SUCCESS_NOTICE] = StringId.IDS_PHONE_FIRMWARE_UPDATE_SUCCESS_NOTICE;
            this._stringMap[StringId.IDS_SYNC_CHAN_FRIEND_DISABLED_EXPLANATION] = StringId.IDS_PHONE_SYNC_CHAN_FRIEND_DISABLED_EXPLANATION;
            this._stringMap[StringId.IDS_FIRMWARE_MANDATE] = StringId.IDS_PHONE_FIRMWARE_MANDATE;
            this._stringMap[StringId.IDS_ZUNE_MARKETPLACE_LINKEDTO] = StringId.IDS_PHONE_ZUNE_MARKETPLACE_LINKEDTO;
            this._stringMap[StringId.IDS_ENABLE_ZUNE_SOCIAL_TEXT_NO_MP] = StringId.IDS_PHONE_ENABLE_ZUNE_SOCIAL_TEXT_NO_MP;
            this._stringMap[StringId.IDS_FRIENDS_EMPTY_DEVICE_HEADER] = StringId.IDS_PHONE_FRIENDS_EMPTY_DEVICE_HEADER;
            this._stringMap[StringId.IDS_RENT_TARGET_DEVICE] = StringId.IDS_PHONE_RENT_TARGET_DEVICE;
            this._stringMap[StringId.IDS_PODCAST_EMPTY_DEVICE_TITLE] = StringId.IDS_PHONE_PODCAST_EMPTY_DEVICE_TITLE;
            this._stringMap[StringId.IDS_CHANNELS_EMPTY_DEVICE_TITLE] = StringId.IDS_PHONE_CHANNELS_EMPTY_DEVICE_TITLE;
            this._stringMap[StringId.IDS_DEVICE_UP_TO_DATE] = StringId.IDS_PHONE_DEVICE_UP_TO_DATE;
            this._stringMap[StringId.IDS_DEVICE_NAME_EMPTY] = StringId.IDS_PHONE_DEVICE_NAME_EMPTY;
            this._stringMap[StringId.IDS_ITEMS_SYNCED_DESCRIPTION] = StringId.IDS_PHONE_ITEMS_SYNCED_DESCRIPTION;
            this._stringMap[StringId.IDS_ITEMS_REMAINING_DESCRIPTION] = StringId.IDS_PHONE_ITEMS_REMAINING_DESCRIPTION;
            this._stringMap[StringId.IDS_DEVICE_SETTINGS_NAME] = StringId.IDS_PHONE_DEVICE_SETTINGS_NAME;
            this._stringMap[StringId.IDS_ALL_SYNCED_ITEMS] = StringId.IDS_PHONE_ALL_SYNCED_ITEMS;
            this._stringMap[StringId.IDS_COMPUTER_ICON_DROP_TARGET_TOOLTIP] = StringId.IDS_PHONE_COMPUTER_ICON_DROP_TARGET_TOOLTIP;
            this._stringMap[StringId.IDS_ALL_PICTURE_ITEMS] = StringId.IDS_PHONE_ALL_PICTURE_ITEMS;
            this._stringMap[StringId.IDS_ALL_PODCAST_ITEMS] = StringId.IDS_PHONE_ALL_PODCAST_ITEMS;
            this._stringMap[StringId.IDS_DEVICE_HUNG] = StringId.IDS_PHONE_DEVICE_HUNG;
            this._stringMap[StringId.IDS_OUT_OF_SPACE_GAS_GAUGE_FORMAT] = StringId.IDS_PHONE_OUT_OF_SPACE_GAS_GAUGE_FORMAT;
            this._stringMap[StringId.IDS_GUEST_OUT_OF_SPACE_MESSAGE] = StringId.IDS_PHONE_GUEST_OUT_OF_SPACE_MESSAGE;
            this._stringMap[StringId.IDS_DEVICE_STATUS_SOCIAL_DESC_NO_ASSOC] = StringId.IDS_PHONE_DEVICE_STATUS_SOCIAL_DESC_NO_ASSOC;
            this._stringMap[StringId.IDS_NOT_OUT_OF_SPACE_GAS_GAUGE_FORMAT] = StringId.IDS_PHONE_NOT_OUT_OF_SPACE_GAS_GAUGE_FORMAT;
            this._stringMap[StringId.IDS_DEVICE_STATUS_SOCIAL_DESC_ASSOC] = StringId.IDS_PHONE_DEVICE_STATUS_SOCIAL_DESC_ASSOC;
            this._stringMap[StringId.IDS_DEVICE_SUMMARY_DEVICE_UPDATE_TITLE] = StringId.IDS_PHONE_DEVICE_SUMMARY_DEVICE_UPDATE_TITLE;
            this._stringMap[StringId.IDS_DEVICE_RENTAL_GUEST_NOT_SUPPORTED] = StringId.IDS_PHONE_DEVICE_RENTAL_GUEST_NOT_SUPPORTED;
            this._stringMap[StringId.IDS_HOW_TO_SYNC_TO_ZUNE_DEVICES] = StringId.IDS_HOW_TO_SYNC_TO_PHONE_DEVICES;
            this._stringMap[StringId.IDS_DEVICE_PIVOT] = StringId.IDS_PHONE_PIVOT;
            this._stringMap[StringId.IDS_ITEMS_SYNCED_TITLE] = StringId.IDS_PHONE_ITEMS_SYNCED_TITLE;
            this._stringMap[StringId.IDS_ITEMS_REMAINING_TITLE] = StringId.IDS_PHONE_ITEMS_REMAINING_TITLE;
            this._stringMap[StringId.IDS_ITEMS_REMOVED_TITLE] = StringId.IDS_PHONE_ITEMS_REMOVED_TITLE;
            this._stringMap[StringId.IDS_ITEMS_FAILED_TITLE] = StringId.IDS_PHONE_ITEMS_FAILED_TITLE;
            this._stringMap[StringId.IDS_NAME_ZUNE_HEADER] = StringId.IDS_NAME_PHONE_HEADER;
            this._stringMap[StringId.IDS_DEVICE_SUMMARY_AVAILABLE_FW_VERSION_HEADER] = StringId.IDS_DEVICE_SUMMARY_AVAILABLE_UPDATE_HEADER;
        }
    }
}
