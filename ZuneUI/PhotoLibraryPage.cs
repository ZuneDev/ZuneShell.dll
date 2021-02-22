// Decompiled with JetBrains decompiler
// Type: ZuneUI.PhotoLibraryPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security;
using System.Threading;
using UIXControls;

namespace ZuneUI
{
    public class PhotoLibraryPage : LibraryPage, ISlideShowStateOwner, INotifyPropertyChanged
    {
        private string _invalidPathCharacters;
        private PhotosPanel _photosPanel;
        private PhotoFolderPanel _photoFolderPanel;
        private IList _selectedPhotoIds;
        private Dictionary<int, bool> _expandedFolderIds;
        private Dictionary<int, bool> _refreshFolderIds;
        private Dictionary<string, bool> _foldersToMonitor;
        private int _folderId;
        private int _deviceRootFolderId = -1;
        private int _deviceCameraRollFolderId = -1;
        private int _deviceSavedFolderId = -1;
        private int _deviceParentFolderId = -1;
        private int _libraryDefaultSaveFolderId = -1;
        private SlideShowState _slideShowState;
        private Command _deleteCommand;
        private Command _refreshPictures;
        private Command _editCommand;
        private Command _createCommand;
        private Command _deleteFromDeviceCommand;
        private int _actionDepth;
        private int _pendingFolderToExpandToRoot;
        private int _pendingFolderDeleteId;
        private int _pendingFolderId;
        private string _photoSort;
        private bool _hasSelectedFolder;

        public PhotoLibraryPage()
          : this(false)
        {
        }

        public PhotoLibraryPage(bool showDevice)
          : base(showDevice, MediaType.Photo)
        {
            this.UI = PhotoLibraryPage.LibraryTemplate;
            this.UIPath = "Collection\\Photos";
            if (showDevice)
            {
                this.PivotPreference = ZuneUI.Shell.MainFrame.Device.Photos;
                Deviceland.InitDevicePage((ZunePage)this);
            }
            else
                this.PivotPreference = ZuneUI.Shell.MainFrame.Collection.Photos;
            this.IsRootPage = true;
            this._photosPanel = new PhotosPanel(this);
            this._photoFolderPanel = new PhotoFolderPanel(this);
            this.ShowPlaylistIcon = false;
            this.TransportControlStyle = TransportControlStyle.Photo;
            this._slideShowState = new SlideShowState((IModelItemOwner)this);
            this._expandedFolderIds = new Dictionary<int, bool>();
            this._refreshFolderIds = new Dictionary<int, bool>();
            this._refreshPictures = new Command();
            this._pendingFolderToExpandToRoot = -1;
            this._pendingFolderDeleteId = -1;
            this._pendingFolderId = -1;
        }

        public PhotosPanel PhotosPanel => this._photosPanel;

        public PhotoFolderPanel PhotoFolderPanel => this._photoFolderPanel;

        public SlideShowState SlideShowState => this._slideShowState;

        public IList SelectedPhotoIds
        {
            get => this._selectedPhotoIds;
            set
            {
                if (this._selectedPhotoIds == value)
                    return;
                this._selectedPhotoIds = value;
                this.FirePropertyChanged(nameof(SelectedPhotoIds));
            }
        }

        public Command RefreshPictures => this._refreshPictures;

        public void ScrollToFolder()
        {
            this.FirePropertyChanged("ScrollingToFolder");
            if (this.ScrollingToFolder == null)
                return;
            this.ScrollingToFolder((object)this, new EventArgs());
        }

        public void ExpandFolder(int id)
        {
            if (this._expandedFolderIds.ContainsKey(id))
                return;
            this._expandedFolderIds.Add(id, true);
            this.FirePropertyChanged("ExpandedFolders");
            if (this.ExpandedFolders == null)
                return;
            this.ExpandedFolders((object)this, new EventArgs());
        }

        public void CollapseFolder(int id)
        {
            if (!this._expandedFolderIds.ContainsKey(id))
                return;
            this._expandedFolderIds.Remove(id);
            this.FirePropertyChanged("ExpandedFolders");
            if (this.ExpandedFolders == null)
                return;
            this.ExpandedFolders((object)this, new EventArgs());
        }

        public bool FolderIsExpanded(int id) => this._expandedFolderIds.ContainsKey(id);

        public void ToggleFolder(int id)
        {
            if (this.FolderIsExpanded(id))
                this.CollapseFolder(id);
            else
                this.ExpandFolder(id);
        }

        public void RefreshFolder(int id)
        {
            if (this._refreshFolderIds.ContainsKey(id))
                return;
            this._refreshFolderIds.Add(id, true);
            this.FirePropertyChanged("RefreshingFolders");
            if (this.RefreshingFolders == null)
                return;
            this.RefreshingFolders((object)this, new EventArgs());
        }

        public bool FolderHasRefreshPending(int id) => this._refreshFolderIds.ContainsKey(id);

        public void RefreshedFolder(int id)
        {
            if (!this._refreshFolderIds.ContainsKey(id))
                return;
            this._refreshFolderIds.Remove(id);
        }

        public bool HasSelectedFolder
        {
            get => this._hasSelectedFolder;
            set => this._hasSelectedFolder = value;
        }

        public int FolderId
        {
            get => this._folderId;
            set
            {
                if (this._folderId == value)
                    return;
                this._folderId = value;
                this.HasSelectedFolder = true;
                this.FirePropertyChanged(nameof(FolderId));
            }
        }

        public int DeviceCameraRollFolderId
        {
            get => this._deviceCameraRollFolderId;
            set
            {
                if (this._deviceCameraRollFolderId == value)
                    return;
                this._deviceCameraRollFolderId = value;
                this.FirePropertyChanged(nameof(DeviceCameraRollFolderId));
            }
        }

        public int DeviceSavedFolderId
        {
            get => this._deviceSavedFolderId;
            set
            {
                if (this._deviceSavedFolderId == value)
                    return;
                this._deviceSavedFolderId = value;
                this.FirePropertyChanged(nameof(DeviceSavedFolderId));
            }
        }

        public int DeviceParentFolderId
        {
            get => this._deviceParentFolderId;
            set
            {
                if (this._deviceParentFolderId == value)
                    return;
                this._deviceParentFolderId = value;
                this.FirePropertyChanged(nameof(DeviceParentFolderId));
            }
        }

        public int DeviceRootFolderId
        {
            get => this._deviceRootFolderId;
            set
            {
                if (this._deviceRootFolderId == value)
                    return;
                this._deviceRootFolderId = value;
                this.FirePropertyChanged(nameof(DeviceRootFolderId));
            }
        }

        public int LibraryRootFolderId => 0;

        public int LibraryDefaultSaveFolderId
        {
            get => this._libraryDefaultSaveFolderId;
            set
            {
                if (this._libraryDefaultSaveFolderId == value)
                    return;
                this._libraryDefaultSaveFolderId = value;
                this.FirePropertyChanged(nameof(LibraryDefaultSaveFolderId));
            }
        }

        public Command DeleteCommand
        {
            get => this._deleteCommand;
            set
            {
                if (this._deleteCommand == value)
                    return;
                this._deleteCommand = value;
                this.FirePropertyChanged(nameof(DeleteCommand));
            }
        }

        public Command EditCommand
        {
            get => this._editCommand;
            set
            {
                if (this._editCommand == value)
                    return;
                this._editCommand = value;
                this.FirePropertyChanged(nameof(EditCommand));
            }
        }

        public Command CreateCommand
        {
            get => this._createCommand;
            set
            {
                if (this._createCommand == value)
                    return;
                this._createCommand = value;
                this.FirePropertyChanged(nameof(CreateCommand));
            }
        }

        public Command DeleteFromDeviceCommand
        {
            get => this._deleteFromDeviceCommand;
            set
            {
                if (this._deleteFromDeviceCommand == value)
                    return;
                this._deleteFromDeviceCommand = value;
                this.FirePropertyChanged(nameof(DeleteFromDeviceCommand));
            }
        }

        public int ActionDepth
        {
            get => this._actionDepth;
            set
            {
                if (this._actionDepth == value)
                    return;
                this._actionDepth = value;
                this.FirePropertyChanged(nameof(ActionDepth));
            }
        }

        public int PendingFolderToDelete
        {
            get => this._pendingFolderDeleteId;
            set
            {
                if (this._pendingFolderDeleteId == value)
                    return;
                this._pendingFolderDeleteId = value;
                this.FirePropertyChanged(nameof(PendingFolderToDelete));
            }
        }

        public int PendingFolderToEdit
        {
            get => this._pendingFolderId;
            set
            {
                if (this._pendingFolderId == value)
                    return;
                this._pendingFolderId = value;
                this.FirePropertyChanged(nameof(PendingFolderToEdit));
            }
        }

        public int PendingFolderToExpandToRoot
        {
            get => this._pendingFolderToExpandToRoot;
            set
            {
                if (this._pendingFolderToExpandToRoot == value)
                    return;
                this._pendingFolderToExpandToRoot = value;
                this.FirePropertyChanged(nameof(PendingFolderToExpandToRoot));
            }
        }

        public string PhotoSort
        {
            get => this._photoSort;
            set
            {
                if (!(this._photoSort != value))
                    return;
                this._photoSort = value;
                this.FirePropertyChanged(nameof(PhotoSort));
            }
        }

        public string GetNextAvailableAlbumName(IList childList)
        {
            if (childList == null)
                return ZuneUI.Shell.LoadString(StringId.IDS_NEW_ALBUM_NAME);
            Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
            foreach (DataProviderObject child in (IEnumerable)childList)
                dictionary.Add(((string)child.GetProperty("Title")).ToLowerInvariant(), true);
            string str1 = string.Empty;
            for (int index = 1; index < int.MaxValue; ++index)
            {
                string empty = string.Empty;
                string str2 = index != 1 ? string.Format(ZuneUI.Shell.LoadString(StringId.IDS_NEW_ALBUM_NAME_MULTIPLE), (object)index) : ZuneUI.Shell.LoadString(StringId.IDS_NEW_ALBUM_NAME);
                if (!dictionary.ContainsKey(str2.ToLowerInvariant()))
                {
                    str1 = str2;
                    break;
                }
            }
            return str1;
        }

        private Dictionary<string, bool> GetFoldersToMonitor(
          IList paths,
          ref string fileParentFolder)
        {
            Management management = ZuneShell.DefaultInstance.Management;
            Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
            bool flag = false;
            foreach (object path in (IEnumerable)paths)
            {
                string str = path as string;
                if (!string.IsNullOrEmpty(str) && !dictionary.ContainsKey(str))
                {
                    if (Directory.Exists(str))
                    {
                        if (!management.IsMonitored(management.MonitoredPhotoFolders, str))
                            dictionary.Add(str, true);
                    }
                    else if (!flag)
                    {
                        try
                        {
                            FileInfo fileInfo = new FileInfo(str);
                            fileParentFolder = fileInfo.DirectoryName;
                            flag = true;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return dictionary;
        }

        public override void CheckCanAddMedia(IList filenames)
        {
            this.CanAddMedia = false;
            if (this.ShowDeviceContents || filenames == null || filenames.Count == 0)
                return;
            Management management = ZuneShell.DefaultInstance.Management;
            List<string> stringList = new List<string>();
            string empty = string.Empty;
            if (this.GetFoldersToMonitor(filenames, ref empty).Keys.Count > 0)
                this.CanAddMedia = true;
            else if (string.IsNullOrEmpty(empty) || management.IsMonitored(management.MonitoredPhotoFolders, empty))
            {
                this.CanAddMedia = false;
            }
            else
            {
                foreach (string filename in (IEnumerable)filenames)
                    stringList.Add(filename);
                base.CheckCanAddMedia((IList)stringList);
            }
        }

        public override void AddMedia(IList filenames)
        {
            if (this.ShowDeviceContents || filenames == null || filenames.Count == 0)
                return;
            string empty = string.Empty;
            this._foldersToMonitor = this.GetFoldersToMonitor(filenames, ref empty);
            if (!string.IsNullOrEmpty(empty))
                this._foldersToMonitor[empty] = true;
            Command yesCommand = new Command((IModelItemOwner)this);
            yesCommand.Invoked += (EventHandler)delegate
           {
               if (this._foldersToMonitor == null || this._foldersToMonitor.Count == 0)
                   return;
               List<string> stringList = new List<string>((IEnumerable<string>)this._foldersToMonitor.Keys);
               stringList.Sort((IComparer<string>)StringComparer.CurrentCultureIgnoreCase);
               Management management = ZuneShell.DefaultInstance.Management;
               foreach (string path in stringList)
               {
                   if (management.UsingWin7Libraries)
                       Win7ShellManager.Instance.AddLocationToLibrary(EWin7LibraryKind.ePicturesLibrary, false, path);
                   else
                       management.AddMonitoredFolder(management.MonitoredPhotoFolders, path, true);
               }
           };
            yesCommand.Description = ZuneUI.Shell.LoadString(StringId.IDS_PHOTO_ADD_FOLDER_BUTTON);
            MessageBox.Show(ZuneUI.Shell.LoadString(StringId.IDS_PHOTO_ADD_FOLDER_TITLE), ZuneUI.Shell.LoadString(StringId.IDS_PHOTO_ADD_FOLDER_DESCRIPTION), yesCommand, (Command)null, (BooleanChoice)null);
        }

        protected override void OnNavigatedAwayWorker(IPage destination)
        {
            this._deviceCameraRollFolderId = -1;
            this._deviceSavedFolderId = -1;
            this._deviceRootFolderId = -1;
            this._libraryDefaultSaveFolderId = -1;
            base.OnNavigatedAwayWorker(destination);
        }

        protected override void OnNavigatedToWorker()
        {
            if (this.NavigationArguments != null)
            {
                int nMediaId = -1;
                int nFolderId = -1;
                if (this.NavigationArguments.Contains((object)"PhotoLibraryId"))
                    nMediaId = (int)this.NavigationArguments[(object)"PhotoLibraryId"];
                if (this.NavigationArguments.Contains((object)"FolderId"))
                {
                    int navigationArgument = (int)this.NavigationArguments[(object)"FolderId"];
                    if (navigationArgument > -1)
                        nFolderId = navigationArgument;
                }
                this._selectedPhotoIds = (IList)null;
                if (nMediaId > -1)
                    this._selectedPhotoIds = (IList)new int[1]
                    {
            nMediaId
                    };
                if (nMediaId > -1 && nFolderId == -1)
                {
                    PhotoManager.Instance.FindPhotoContainer(nMediaId, out nFolderId);
                    this.PendingFolderToExpandToRoot = nFolderId;
                }
                if (nFolderId > -1)
                {
                    this.PendingFolderToExpandToRoot = nFolderId;
                    this.FolderId = nFolderId;
                }
                this.NavigationArguments = (IDictionary)null;
            }
            base.OnNavigatedToWorker();
        }

        public static void FindInCollection(int folderId, int photoId)
        {
            if (photoId >= 0 && folderId < 0)
                folderId = -1;
            Hashtable hashtable = new Hashtable();
            hashtable.Add((object)"FolderId", (object)folderId);
            if (photoId >= 0)
                hashtable.Add((object)"PhotoLibraryId", (object)photoId);
            ZuneShell.DefaultInstance.Execute("Collection\\Photos", (IDictionary)hashtable);
        }

        public override IPageState SaveAndRelease()
        {
            this._photosPanel.Release();
            this._photoFolderPanel.Release();
            return base.SaveAndRelease();
        }

        public void Rename(DataProviderObject source, string folderName)
        {
            if (string.IsNullOrEmpty(folderName) || source == null || source.TypeName != "MediaFolder")
                return;
            int id = (int)source.GetProperty("LibraryId");
            if (id <= 0)
                return;
            int parentId = (int)source.GetProperty("ParentId");
            if (parentId < 0)
                return;
            string property = (string)source.GetProperty("FolderPath");
            if (!Directory.Exists(property))
                return;
            try
            {
                if (string.Compare(new DirectoryInfo(property).Name, folderName, StringComparison.CurrentCulture) == 0)
                    return;
            }
            catch (SecurityException ex)
            {
            }
            catch (ArgumentException ex)
            {
                return;
            }
            ZuneShell.DefaultInstance.Management.RemoveChildMonitoredFolders(property, true);
            Application.DeferredInvoke((DeferredInvokeHandler)delegate
           {
               if (!PhotoManager.Instance.RenameFolder(id, folderName).IsSuccess)
                   return;
               this.RefreshFolder(parentId);
               this.RefreshPictures.Invoke();
           }, (object)null);
        }

        public void MoveFolder(DataProviderObject source, DataProviderObject target)
        {
            if (source == null || target == null || (source.TypeName != "MediaFolder" || target.TypeName != "MediaFolder"))
                return;
            int property1 = (int)source.GetProperty("LibraryId");
            if (property1 <= 0)
                return;
            int property2 = (int)source.GetProperty("ParentId");
            if (property2 < 0)
                return;
            int property3 = (int)target.GetProperty("LibraryId");
            if (property3 <= 0)
                return;
            string property4 = (string)source.GetProperty("FolderPath");
            string property5 = (string)target.GetProperty("FolderPath");
            this.MoveFolder(property1, property2, property4, property3, property5);
        }

        private void MoveFolder(
          int sourceId,
          int sourceParentId,
          string sourceFolderPath,
          int targetId,
          string targetFolderPath)
        {
            if (sourceId <= 0 || !Directory.Exists(sourceFolderPath))
                return;
            ZuneShell.DefaultInstance.Management.RemoveChildMonitoredFolders(sourceFolderPath, true);
            int[] sourceIds = new int[1] { sourceId };
            string resultantFolderPath = Path.Combine(targetFolderPath, Path.GetFileName(sourceFolderPath));
            Application.DeferredInvoke((DeferredInvokeHandler)delegate
           {
               if (!PhotoManager.Instance.Move(sourceIds, EMediaTypes.eMediaTypeFolder, targetId).IsSuccess)
                   return;
               ZuneApplication.ZuneLibrary.AddGrovelerScanDirectory(sourceFolderPath, EMediaTypes.eMediaTypeImage);
               this.RefreshFolder(sourceParentId);
               ZuneApplication.ZuneLibrary.AddGrovelerScanDirectory(resultantFolderPath, EMediaTypes.eMediaTypeImage);
               this.RefreshFolder(targetId);
           }, (object)null);
        }

        public void Import(IList shellItems, int destinationFolderId)
        {
            foreach (object shellItem in (IEnumerable)shellItems)
            {
                string path = shellItem as string;
                if (!string.IsNullOrEmpty(path) && destinationFolderId > 0)
                {
                    if (File.Exists(path) && ZuneApplication.ZuneLibrary.CanAddMedia(path, EMediaTypes.eMediaTypeImage))
                        Application.DeferredInvoke((DeferredInvokeHandler)delegate
                       {
                           PhotoManager.Instance.Import(path, EMediaTypes.eMediaTypeImage, destinationFolderId);
                           this.FolderId = destinationFolderId;
                           this.RefreshPictures.Invoke();
                       }, (object)null);
                    else if (Directory.Exists(path))
                    {
                        if (this.FindFolder(path) > 0)
                            ZuneShell.DefaultInstance.Management.RemoveChildMonitoredFolders(path, true);
                        Application.DeferredInvoke((DeferredInvokeHandler)delegate
                       {
                           if (!PhotoManager.Instance.Import(path, EMediaTypes.eMediaTypeFolder, destinationFolderId).IsSuccess || destinationFolderId <= 0)
                               return;
                           ZuneApplication.ZuneLibrary.AddGrovelerScanDirectory(path, EMediaTypes.eMediaTypeImage);
                           this.FolderId = destinationFolderId;
                           this.RefreshFolder(destinationFolderId);
                           this.ExpandFolder(destinationFolderId);
                       }, (object)null);
                    }
                }
            }
        }

        public static bool DeleteFolder(DataProviderObject folderItem)
        {
            if (folderItem == null || folderItem.TypeName != "MediaFolder")
                return false;
            int property = (int)folderItem.GetProperty("LibraryId");
            if (property <= 0)
                return false;
            if (ZuneShell.DefaultInstance.Management.RemoveChildMonitoredFolders((string)folderItem.GetProperty("FolderPath"), true))
                Thread.Sleep(500);
            return ZuneApplication.ZuneLibrary.DeleteFilesystemFolder(property, EMediaTypes.eMediaTypeImage);
        }

        public void MovePhotos(IList sourceDataProviderList, int targetId)
        {
            int[] sourceIds = new int[sourceDataProviderList.Count];
            for (int index = 0; index < sourceDataProviderList.Count; ++index)
            {
                DataProviderObject sourceDataProvider = (DataProviderObject)sourceDataProviderList[index];
                sourceIds[index] = (int)sourceDataProvider.GetProperty("LibraryId");
            }
            Application.DeferredInvoke((DeferredInvokeHandler)delegate
           {
               PhotoManager.Instance.Move(sourceIds, EMediaTypes.eMediaTypeImage, targetId);
               this.RefreshPictures.Invoke();
           }, (object)null);
        }

        public int CreateFolder(string name, int targetId)
        {
            int nCreatedFolderId = 0;
            PhotoManager.Instance.CreateFolder(name, targetId, out nCreatedFolderId);
            return nCreatedFolderId;
        }

        public int FindFolder(string folderName)
        {
            int nFolderId;
            return !PhotoManager.Instance.FindFolder(folderName, out nFolderId).IsSuccess ? -1 : nFolderId;
        }

        public bool CanDropShellFolder(string treePath, string dropPath)
        {
            try
            {
                if (!Directory.Exists(treePath) || !Directory.Exists(dropPath))
                    return false;
                DirectoryInfo directoryInfo1 = new DirectoryInfo(treePath);
                DirectoryInfo directoryInfo2 = new DirectoryInfo(dropPath);
                if ((directoryInfo1.FullName + (object)Path.DirectorySeparatorChar).IndexOf(directoryInfo2.FullName + (object)Path.DirectorySeparatorChar, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    return false;
                if (string.Compare(directoryInfo1.FullName, directoryInfo2.Parent.FullName, StringComparison.CurrentCultureIgnoreCase) == 0)
                    return false;
            }
            catch
            {
                return false;
            }
            return true;
        }

        public string GetInvalidPathCharacters()
        {
            if (string.IsNullOrEmpty(this._invalidPathCharacters))
            {
                char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
                if (invalidFileNameChars != null && invalidFileNameChars.Length > 0)
                {
                    bool flag = true;
                    for (int index = 0; index < invalidFileNameChars.Length; ++index)
                    {
                        if (!char.IsControl(invalidFileNameChars[index]))
                        {
                            if (flag)
                            {
                                this._invalidPathCharacters = invalidFileNameChars[index].ToString();
                                flag = false;
                            }
                            else
                                this._invalidPathCharacters += string.Format(" {0}", (object)invalidFileNameChars[index]);
                        }
                    }
                }
            }
            return this._invalidPathCharacters;
        }

        public bool ValidateFilename(string filename) => !string.IsNullOrEmpty(filename) && filename.IndexOfAny(Path.GetInvalidFileNameChars()) == -1;

        public event EventHandler ExpandedFolders;

        public event EventHandler RefreshingFolders;

        public event EventHandler ScrollingToFolder;

        private static string LibraryTemplate => "res://ZuneShellResources!PhotoLibrary.uix#PhotoLibrary";
    }
}
