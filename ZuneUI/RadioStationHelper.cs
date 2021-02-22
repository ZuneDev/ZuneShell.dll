// Decompiled with JetBrains decompiler
// Type: ZuneUI.RadioStationHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Win32;
using Microsoft.Zune.Util;
using System.Collections;

namespace ZuneUI
{
    public class RadioStationHelper : ModelItem
    {
        private const string _rootKeyPath = "Software\\Microsoft\\Zune\\Radio";
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
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Zune\\Radio");
            if (registryKey == null)
            {
                Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Zune\\Radio");
            }
            else
            {
                foreach (string subKeyName in registryKey.GetSubKeyNames())
                {
                    string keyName = "HKEY_CURRENT_USER\\Software\\Microsoft\\Zune\\Radio\\" + subKeyName;
                    string SourceURL = (string)Registry.GetValue(keyName, "SourceURL", "");
                    string ImagePath = (string)Registry.GetValue(keyName, "Image", "");
                    this.stationList.Add(new RadioStation(subKeyName, SourceURL, ImagePath));
                }
            }
            this.FirePropertyChanged("StationList");
        }
    }
}
