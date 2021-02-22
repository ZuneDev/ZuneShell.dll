// Decompiled with JetBrains decompiler
// Type: ZuneUI.SoftwareUpdates
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Win32;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Service;
using Microsoft.Zune.Util;
using MicrosoftZuneLibrary;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading;

namespace ZuneUI
{
    public class SoftwareUpdates : ModelItem
    {
        private const string m_PID = "92510-320-9256355-04773";
        private bool m_checkingForUpdates;
        private bool m_backgroundCheck;
        private bool m_userInitiatedUpdate;
        private static SoftwareUpdates m_instance;
        private UpdateCheckEventArguments _lastUpdateCheckResult;

        public event EventHandler InstallInitiated;

        public static SoftwareUpdates Instance
        {
            get
            {
                if (SoftwareUpdates.m_instance == null)
                    SoftwareUpdates.m_instance = new SoftwareUpdates();
                return SoftwareUpdates.m_instance;
            }
        }

        public static string PID => "92510-320-9256355-04773";

        public void StartUp() => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredStartUp), DeferredInvokePriority.Low);

        public bool CheckingForUpdates
        {
            get => this.m_checkingForUpdates;
            private set
            {
                if (this.m_checkingForUpdates == value)
                    return;
                this.m_checkingForUpdates = value;
                this.FirePropertyChanged(nameof(CheckingForUpdates));
            }
        }

        public UpdateCheckEventArguments LastUpdateCheckResult
        {
            get => this._lastUpdateCheckResult;
            set
            {
                if (this._lastUpdateCheckResult == value)
                    return;
                this._lastUpdateCheckResult = value;
                this.FirePropertyChanged(nameof(LastUpdateCheckResult));
            }
        }

        public void CheckForUpdates(bool isUserInitiated)
        {
            if (!this.CheckingForUpdates)
            {
                this._lastUpdateCheckResult = (UpdateCheckEventArguments)null;
                this.CheckingForUpdates = true;
                this.m_userInitiatedUpdate = isUserInitiated;
                UpdateManager.Instance.BeginUpdateCheck(new UpdateProgressHandler(this.UpdateCheckCallback));
                if (!this.m_userInitiatedUpdate)
                    return;
                SQMLog.Log(SQMDataId.ManualCheckForUpdatesInvoked, 1);
            }
            else
                this.m_backgroundCheck = false;
        }

        public void CancelCheckForUpdates()
        {
            UpdateManager.Instance.CancelUpdateCheck();
            SoftwareUpdates.Instance.CheckingForUpdates = false;
        }

        public void InstallUpdates()
        {
            this.OnInstallInitiated();
            UpdateManager.Instance.InstallUpdate(new UpdateProgressHandler(this.UpdateCheckCallback));
        }

        public void ShowUpdateAvailabilityDialog()
        {
            if (!this.LastUpdateCheckResult.UpdateFound && this.m_backgroundCheck)
                return;
            if (this.IsWUClientUpToDate())
            {
                if (this.LastUpdateCheckResult.HR >= 0)
                    UpdateDialogInfo.Show(this.LastUpdateCheckResult.UpdateFound, this.LastUpdateCheckResult.CriticalUpdateFound, this.m_userInitiatedUpdate);
                else
                    Shell.ShowErrorDialog(this.LastUpdateCheckResult.HR, StringId.IDS_CHECK_FOR_UPDATES_FAILED);
            }
            else
                Shell.ShowErrorDialog(-1072885299, StringId.IDS_CHECK_FOR_UPDATES_FAILED, StringId.IDS_CHECK_FOR_UPDATES_FAILED_DESCRIPTION);
        }

        private bool IsWUClientUpToDate()
        {
            string str = Environment.ExpandEnvironmentVariables("%windir%\\System32\\wuaueng.dll");
            bool flag = false;
            if (str != null && System.IO.File.Exists(str) && new Version(FileVersionInfo.GetVersionInfo(str).ProductVersion).CompareTo(new Version("7.2.6001.784")) >= 0)
                flag = true;
            return flag;
        }

        private void OnInstallInitiated()
        {
            if (this.InstallInitiated != null)
                this.InstallInitiated((object)this, (EventArgs)null);
            this.FirePropertyChanged("InstallInitiated");
        }

        private void DeferredStartUp(object state)
        {
            this.CheckOsUpgrade();
            DateTime lastUpdateCheck = ClientConfiguration.Shell.LastUpdateCheck;
            uint num = Math.Min((uint)MachineConfiguration.Setup.UpdateCheckFrequency, 14U);
            if ((long)Math.Abs((DateTime.UtcNow - lastUpdateCheck).Days) >= (long)num && !Fue.Instance.IsFirstLaunch && !Shell.SettingsFrame.Wizard.IsCurrent)
            {
                this.m_backgroundCheck = true;
                this.CheckForUpdates(false);
            }
            this.CheckCodecAccounting();
        }

        private void DeferredUpdateCheckCallback(object args)
        {
            this.LastUpdateCheckResult = (UpdateCheckEventArguments)args;
            if (this.LastUpdateCheckResult.HR >= 0)
            {
                try
                {
                    ClientConfiguration.Shell.LastUpdateCheck = DateTime.UtcNow;
                }
                catch (ApplicationException ex)
                {
                }
            }
            if (this.m_backgroundCheck)
                this.ShowUpdateAvailabilityDialog();
            this.m_userInitiatedUpdate = false;
            this.m_backgroundCheck = false;
            this.CheckingForUpdates = false;
        }

        private void UpdateCheckCallback(UpdateCheckEventArguments args) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredUpdateCheckCallback), (object)args, DeferredInvokePriority.Normal);

        private void CheckOsUpgrade()
        {
            int num = Environment.OSVersion.Version.Major * 100 + Environment.OSVersion.Version.Minor;
            int osVersion = MachineConfiguration.Setup.OSVersion;
            if (num <= osVersion)
                return;
            if (osVersion < 600 && MachineConfiguration.HME.CurrentSharingUID != 0)
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.RepairNetworkSharingService), (object)num);
            if (osVersion >= 601 || num < 601)
                return;
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.CreatePodcastLibraryTemplate), (object)num);
        }

        private void RepairNetworkSharingService(object state)
        {
            int num = (int)state;
            HMESettings hmeSettings = new HMESettings();
            HRESULT hresult = (HRESULT)hmeSettings.Init();
            if (hresult.IsSuccess)
                hresult = (HRESULT)hmeSettings.RepairSharing();
            if (!hresult.IsSuccess)
                return;
            MachineConfiguration.Setup.OSVersion = num;
        }

        private void CreatePodcastLibraryTemplate(object state)
        {
            int num = (int)state;
            if (((HRESULT)Win7ShellManager.Instance.CreatePodcastLibraryTemplate()).IsSuccess)
                MachineConfiguration.Setup.OSVersion = num;
            Win7ShellManager.Instance.SyncLibraryFolders();
        }

        private void CheckCodecAccounting()
        {
            if (MachineConfiguration.Setup.CodecInfoSent)
                return;
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Zune");
            DateTime utcNow = DateTime.UtcNow;
            DateTime localTime = utcNow.ToLocalTime();
            long totalMilliseconds = (long)localTime.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            string str1 = "ZuneSetup.exe";
            string wtProfile = MachineConfiguration.Setup.WTProfile;
            string str2 = totalMilliseconds.ToString();
            string str3 = localTime.ToString("yyyy/MM/dd");
            string str4 = TimeZone.CurrentTimeZone.GetUtcOffset(localTime).Hours.ToString();
            string str5 = localTime.Hour.ToString();
            string ietfLanguageTag = CultureInfo.CurrentUICulture.IetfLanguageTag;
            string installationSource = MachineConfiguration.Setup.InstallationSource;
            string pid = SoftwareUpdates.PID;
            string version1 = (string)registryKey.GetValue("CurrentVersion");
            string oldVersion = MachineConfiguration.Setup.OldVersion;
            string str6 = string.Empty;
            string str7 = "New";
            if (!string.IsNullOrEmpty(version1))
            {
                try
                {
                    Version version2 = new Version(version1);
                    int major = version2.Major;
                    int minor = version2.Minor;
                    if (!string.IsNullOrEmpty(oldVersion))
                    {
                        Version version3 = new Version(oldVersion);
                        str7 = !(version2 == version3) ? (version2.Major <= version3.Major ? (version2.Major != version3.Major ? "ReInstall" : "Upgrade") : "MajorUpgrade") : "ReInstall";
                    }
                }
                catch (ArgumentException ex)
                {
                }
                catch (FormatException ex)
                {
                }
                catch (OverflowException ex)
                {
                }
            }
            try
            {
                using (CryptoHelper cryptoHelper = new CryptoHelper(pid))
                {
                    string data = utcNow.ToString("yyyy-MM-dd-hh-mm-ss");
                    str6 = Uri.EscapeDataString(cryptoHelper.Encrypt(data));
                }
            }
            catch (Win32Exception ex)
            {
            }
            string[] strArray = new string[27]
            {
        "http://m.webtrends.com/dcs8fe5yk00000s538qdxmbst_2t2k/dcs.gif",
        "?dcsdat=",
        str2,
        "&dcsuri=",
        str1,
        "&dcssip=",
        wtProfile,
        "&wt.tz=",
        str4,
        "&wt.bh=",
        str5,
        "&wt.date=",
        str3,
        "&wt.ul=",
        ietfLanguageTag,
        "&zune_cver=",
        version1,
        "&zune_isource=",
        installationSource,
        "&zune_itype=",
        str7,
        "&zune_over=",
        oldVersion,
        "&zune_pid=",
        pid,
        "&zune_gmthash=",
        str6
            };
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < strArray.Length; ++index)
                stringBuilder.Append(strArray[index]);
            try
            {
                Microsoft.Zune.Service.HttpWebRequest httpWebRequest = Microsoft.Zune.Service.HttpWebRequest.Create(new Uri(stringBuilder.ToString()));
                httpWebRequest.CancelOnShutdown = true;
                httpWebRequest.GetResponseAsync(new AsyncRequestComplete(this.OnRequestComplete), (object)null);
            }
            catch (Exception ex)
            {
            }
        }

        private void OnRequestComplete(Microsoft.Zune.Service.HttpWebResponse response, object requestArgs)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                MachineConfiguration.Setup.CodecInfoSent = true;
            }
            else
            {
                int statusCode = (int)response.StatusCode;
            }
        }
    }
}
