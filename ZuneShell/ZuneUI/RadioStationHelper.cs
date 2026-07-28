// Decompiled with JetBrains decompiler
// Type: ZuneUI.RadioStationHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Win32;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Util;
using System.Collections;
using Microsoft.Iris.Data.Registry;

namespace ZuneUI
{
    public class RadioStationHelper : ModelItem
    {
        private static RadioStationHelper _instance;
        private ArrayList stationList;

        public static RadioStationHelper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RadioStationHelper();
                return _instance;
            }
        }

        private RadioStationHelper()
        {
        }

        public ArrayList StationList
        {
            get
            {
                if (this.stationList == null)
                    this.RefreshStationList();
                return this.stationList;
            }
            private set
            {
                if (value == this.stationList)
                    return;
                this.stationList = value;
            }
        }

        public void AddStation(string title, string sourceUrl, string image)
        {
            RadioStationProgressHandler radioStationProgressHandler = new RadioStationProgressHandler(this.RadioStationAsyncCallback);
            RadioStationManager.Instance.AddStation(title, sourceUrl, image, radioStationProgressHandler);
        }

        public void DeleteStation(string title)
        {
            RadioStationProgressHandler radioStationProgressHandler = new RadioStationProgressHandler(this.RadioStationAsyncCallback);
            RadioStationManager.Instance.DeleteStation(title, radioStationProgressHandler);
        }

        private void RadioStationAsyncCallback(HRESULT hr)
        {
            if (!hr.IsSuccess)
                Shell.ShowErrorDialog(hr.Int, StringId.IDS_RADIO_ERROR);
            this.RefreshStationList();
        }

        private void RefreshStationList()
        {
            this.stationList = new ArrayList();
            // Create(...) opens-or-creates, so a freshly-created key simply
            // yields no subkeys below, matching the original's create-and-skip branch.
            using (IRegistryProvider registryKey = ZuneConfigurationRegistry.Open(RegistryHive.CurrentUser, "Radio"))
            {
                foreach (string subKeyName in registryKey.GetSubKeyNames())
                {
                    using IRegistryProvider stationKey = registryKey.OpenSubKey(subKeyName);
                    string SourceURL = stationKey?.GetStringValue("SourceURL", "") ?? "";
                    string ImagePath = stationKey?.GetStringValue("Image", "") ?? "";
                    this.stationList.Add(new RadioStation(subKeyName, SourceURL, ImagePath));
                }
            }
            this.FirePropertyChanged("StationList");
        }
    }
}
