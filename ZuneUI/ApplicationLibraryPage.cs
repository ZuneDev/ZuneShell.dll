// Decompiled with JetBrains decompiler
// Type: ZuneUI.ApplicationLibraryPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Collections;

namespace ZuneUI
{
    public class ApplicationLibraryPage : LibraryPage
    {
        private ApplicationsPanel _applicationsPanel;
        private object _selectedApplication;
        private IList _selectedApplicationIDs;

        public ApplicationLibraryPage()
          : this(false)
        {
        }

        protected override void OnDispose(bool disposing) => base.OnDispose(disposing);

        public ApplicationLibraryPage(bool showDevice)
          : base(showDevice, MediaType.Application)
        {
            this.UI = ApplicationLibraryPage.LibraryTemplate;
            if (showDevice)
            {
                this.UIPath = "Device\\Applications";
                this.PivotPreference = Shell.MainFrame.Device.Applications;
                Deviceland.InitDevicePage((ZunePage)this);
            }
            else
            {
                this.UIPath = "Collection\\Applications";
                this.PivotPreference = Shell.MainFrame.Collection.Applications;
            }
            this.IsRootPage = true;
            this.ShowPlaylistIcon = false;
            this.ShowCDIcon = false;
            this.TransportControlStyle = TransportControlStyle.None;
            this._applicationsPanel = new ApplicationsPanel(this);
        }

        public ApplicationsPanel ApplicationsPanel => this._applicationsPanel;

        public object SelectedApplication
        {
            get => this._selectedApplication;
            set
            {
                if (this._selectedApplication == value)
                    return;
                this._selectedApplication = value;
                this.FirePropertyChanged(nameof(SelectedApplication));
            }
        }

        public IList SelectedApplicationIDs
        {
            get => this._selectedApplicationIDs;
            set
            {
                if (this._selectedApplicationIDs == value)
                    return;
                this._selectedApplicationIDs = value;
                this.FirePropertyChanged(nameof(SelectedApplicationIDs));
            }
        }

        protected override void OnNavigatedToWorker()
        {
            if (this.NavigationArguments != null)
            {
                if (this.NavigationArguments.Contains((object)"ApplicationLibraryId"))
                    this._selectedApplicationIDs = (IList)new int[1]
                    {
            (int) this.NavigationArguments[(object) "ApplicationLibraryId"]
                    };
                this.NavigationArguments = (IDictionary)null;
            }
            base.OnNavigatedToWorker();
        }

        protected override void OnNavigatedAwayWorker(IPage destination)
        {
            base.OnNavigatedAwayWorker(destination);
            this.SelectedApplication = (object)null;
        }

        public static void FindInCollection(int applicationId) => ZuneShell.DefaultInstance.Execute("Collection\\Applications", (IDictionary)new Hashtable()
    {
      {
        (object) "ApplicationLibraryId",
        (object) applicationId
      }
    });

        public static bool DoesApplicationNeedUpdate(int applicationId, string serviceVersionString)
        {
            if (string.IsNullOrEmpty(serviceVersionString))
                return false;
            Version version = (Version)null;
            return ApplicationLibraryPage.TryParseVersion(serviceVersionString, out version) && ApplicationLibraryPage.DoesApplicationNeedUpdate(applicationId, version);
        }

        public static bool DoesApplicationNeedUpdate(int applicationId, Version serviceVersion)
        {
            if ((Version)null == serviceVersion)
                return false;
            bool flag = false;
            string fieldValue = PlaylistManager.GetFieldValue<string>(applicationId, EListType.eAppList, 376, (string)null);
            if (!string.IsNullOrEmpty(fieldValue))
            {
                Version version = (Version)null;
                if (ApplicationLibraryPage.TryParseVersion(fieldValue, out version) && version < serviceVersion)
                    flag = true;
            }
            return flag;
        }

        private static bool TryParseVersion(string versionString, out Version version)
        {
            bool flag = false;
            version = (Version)null;
            try
            {
                version = new Version(versionString);
                flag = true;
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
            return flag;
        }

        public override IPageState SaveAndRelease() => (IPageState)new ApplicationPageState(this);

        private static string LibraryTemplate => "res://ZuneShellResources!ApplicationLibrary.uix#ApplicationLibrary";
    }
}
