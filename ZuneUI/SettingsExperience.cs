// Decompiled with JetBrains decompiler
// Type: ZuneUI.SettingsExperience
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace ZuneUI
{
    public class SettingsExperience : Experience
    {
        private ArrayListDataSet _nodes;
        private CategoryPageNode _software;
        private CategoryPageNode _device;
        private CategoryPageNode _account;
        private Category _nameYourDevice;

        public SettingsExperience(Frame frameOwner)
          : base(frameOwner, StringId.IDS_SETTINGS_PIVOT, SQMDataId.Invalid)
          => SyncControls.Instance.PropertyChanged += new PropertyChangedEventHandler(this.OnSyncPropertyChanged);

        protected override void OnInvoked()
        {
            if (Shell.SettingsFrame.Wizard.IsCurrent)
                return;
            base.OnInvoked();
        }

        protected override void OnDispose(bool disposing)
        {
            SyncControls.Instance.PropertyChanged -= new PropertyChangedEventHandler(this.OnSyncPropertyChanged);
            base.OnDispose(disposing);
        }

        private void OnSyncPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (!(args.PropertyName == "CurrentDevice"))
                return;
            this.Device.Description = SyncControls.Instance.CurrentDevice.PivotDescription;
            if (this._nameYourDevice == null)
                return;
            this._nameYourDevice.Description = Shell.LoadString(StringId.IDS_NAME_ZUNE_HEADER);
        }

        public override IList NodesList
        {
            get
            {
                if (this._nodes == null)
                {
                    this._nodes = new ArrayListDataSet((IModelItemOwner)this);
                    this._nodes.Add((object)this.Software);
                    if (FeatureEnablement.IsFeatureEnabled(Features.eDevice))
                        this._nodes.Add((object)this.Device);
                    if (FeatureEnablement.IsFeatureEnabled(Features.eSignInAvailable))
                        this._nodes.Add((object)this.Account);
                }
                return (IList)this._nodes;
            }
        }

        public CategoryPageNode Software
        {
            get
            {
                if (this._software == null)
                    this._software = new CategoryPageNode((Experience)this, StringId.IDS_SOFTWARE_PIVOT, (IList)new Category[11]
                    {
            SettingCategories.Collection,
            SettingCategories.Filetype,
            SettingCategories.Privacy,
            SettingCategories.Podcast,
            SettingCategories.Sharing,
            SettingCategories.Photo,
            SettingCategories.Display,
            SettingCategories.Rip,
            SettingCategories.Burn,
            SettingCategories.Metadata,
            SettingCategories.General
                    }, SQMDataId.Invalid, true, false);
                return this._software;
            }
        }

        public CategoryPageNode Device
        {
            get
            {
                if (this._device == null)
                {
                    List<Category> categoryList = new List<Category>();
                    categoryList.Add(SettingCategories.SyncOptions);
                    categoryList.Add(SettingCategories.SyncGroups);
                    this._nameYourDevice = SettingCategories.NameDevice;
                    categoryList.Add(this._nameYourDevice);
                    categoryList.Add(SettingCategories.MoreOnWeb);
                    if (SettingsExperience.ShouldShowDeviceMarketplaceCategory)
                        categoryList.Add(SettingCategories.DeviceMarketplace);
                    categoryList.Add(SettingCategories.FirmwareUpdate);
                    categoryList.Add(SettingCategories.WirelessSetup);
                    categoryList.Add(SettingCategories.PictureVideo);
                    categoryList.Add(SettingCategories.Transcoding);
                    categoryList.Add(SettingCategories.SpaceReservation);
                    categoryList.Add(SettingCategories.DevicePrivacy);
                    this._device = new CategoryPageNode((Experience)this, StringId.IDS_DEVICE_PIVOT, (IList)categoryList, SQMDataId.Invalid, true, false);
                }
                return this._device;
            }
        }

        public CategoryPageNode Account
        {
            get
            {
                if (this._account == null)
                {
                    List<Category> categoryList = new List<Category>();
                    categoryList.Add(SettingCategories.AccountLinks);
                    if (FeatureEnablement.IsFeatureEnabled(Features.eMarketplace))
                    {
                        if (FeatureEnablement.IsFeatureEnabled(Features.eSubscription) || FeatureEnablement.IsFeatureEnabled(Features.eApps))
                            categoryList.Add(SettingCategories.Devices);
                        categoryList.Add(SettingCategories.PurchaseHistory);
                        categoryList.Add(SettingCategories.RentalHistory);
                        if (FeatureEnablement.IsFeatureEnabled(Features.eSubscription))
                            categoryList.Add(SettingCategories.SubscriptionHistory);
                    }
                    this._account = new CategoryPageNode((Experience)this, StringId.IDS_ACCOUNT_PIVOT, (IList)categoryList, SQMDataId.Invalid, true, false);
                }
                return this._account;
            }
        }

        internal void ShowDevice(bool show)
        {
            IList nodesList = this.NodesList;
            bool flag = false;
            foreach (object obj in (IEnumerable)nodesList)
            {
                if (obj == this.Device)
                {
                    flag = true;
                    break;
                }
            }
            if (show == flag)
                return;
            this.Device.Available = show;
            if (show)
            {
                if (nodesList.Count > 1 && nodesList[0] == this.Software)
                    nodesList.Insert(1, (object)this.Device);
                else
                    nodesList.Add((object)this.Device);
            }
            else
                nodesList.Remove((object)this.Device);
        }

        public static bool ShouldShowDeviceMarketplaceCategory => FeatureEnablement.IsFeatureEnabled(Features.eMarketplace) || FeatureEnablement.IsFeatureEnabled(Features.eSocial);
    }
}
