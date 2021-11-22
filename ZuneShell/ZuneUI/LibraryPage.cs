// Decompiled with JetBrains decompiler
// Type: ZuneUI.LibraryPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Shell;
using MicrosoftZuneLibrary;
using System.Collections;
using System.ComponentModel;
using System.Threading;

namespace ZuneUI
{
    public class LibraryPage : ZunePage, IDeviceContentsPage, IPage
    {
        private Choice _views;
        private Choice _contentTypes;
        private BooleanChoice _showContentTypes;
        private bool _showDeviceContents;
        private MediaType _mediaType;
        private bool _canAddMedia;
        private CanAddMediaArgs _canAddMediaArgs;
        private bool _isEmpty;
        private Command _escapePressed;
        private QueryTracker _tracker;
        private long _drmStateMask;

        public LibraryPage()
          : this(MediaType.Undefined)
        {
        }

        public LibraryPage(MediaType mediaType)
          : this(false, mediaType)
        {
        }

        public LibraryPage(bool showDeviceContents, MediaType mediaType)
        {
            this.UI = LibraryTemplate;
            this.ShowDeviceContents = showDeviceContents;
            this._mediaType = mediaType;
            this._escapePressed = new Command(this);
            this._tracker = new QueryTracker();
            this._tracker.PropertyChanged += new PropertyChangedEventHandler(this.TrackerPropertyChanged);
            this._drmStateMask = ZuneUI.DrmStateMask.All();
            this._showContentTypes = new BooleanChoice(this);
        }

        public Choice Views
        {
            get => this._views;
            set
            {
                if (this._views == value)
                    return;
                this._views = value;
                this.FirePropertyChanged(nameof(Views));
            }
        }

        public Choice ContentTypes
        {
            get => this._contentTypes;
            set
            {
                if (this._contentTypes == value)
                    return;
                this._contentTypes = value;
                this.FirePropertyChanged(nameof(ContentTypes));
            }
        }

        public BooleanChoice ShowContentTypes => this._showContentTypes;

        public long DrmStateMask
        {
            get => this._drmStateMask;
            set
            {
                if (this._drmStateMask == value)
                    return;
                this._drmStateMask = value;
                this.FirePropertyChanged(nameof(DrmStateMask));
            }
        }

        public Command EscapePressed => this._escapePressed;

        public bool IsEmpty
        {
            get => this._isEmpty;
            set
            {
                if (this._isEmpty == value)
                    return;
                this._isEmpty = value;
                this.FirePropertyChanged(nameof(IsEmpty));
            }
        }

        public bool ShowDeviceContents
        {
            get => this._showDeviceContents;
            set => this._showDeviceContents = value;
        }

        public MediaType MediaType => this._mediaType;

        public bool CanAddMedia
        {
            get => this._canAddMedia;
            set
            {
                if (this._canAddMedia != value)
                {
                    this._canAddMedia = value;
                    this.FirePropertyChanged(nameof(CanAddMedia));
                }
                this.StopCheckingCanAddMedia();
            }
        }

        public QueryTracker Tracker => this._tracker;

        private void TrackerPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (!(args.PropertyName == "Status") || !QueryHelper.HasCompletedOrFailed(this.Tracker.Status))
                return;
            ZuneApplication.PageLoadComplete();
        }

        public virtual void CheckCanAddMedia(IList filenames)
        {
            this.CanAddMedia = false;
            if (this.ShowDeviceContents)
                return;
            ArrayList tempFilenames = new ArrayList(filenames);
            CanAddMediaArgs args = new CanAddMediaArgs();
            this._canAddMediaArgs = args;
            ThreadPool.QueueUserWorkItem(ignored =>
           {
               bool canAddMedia = ZuneApplication.CanAddMedia(tempFilenames, this.MediaType, args);
               if (args.Aborted)
                   return;
               Application.DeferredInvoke(delegate
         {
             if (args.Aborted)
                 return;
             this.CanAddMedia = canAddMedia;
         }, null);
           });
        }

        public void StopCheckingCanAddMedia()
        {
            if (this._canAddMediaArgs == null)
                return;
            this._canAddMediaArgs.Aborted = true;
            this._canAddMediaArgs = null;
        }

        public virtual void AddMedia(IList filenames)
        {
            ArrayList tempFilenames = new ArrayList(filenames);
            ThreadPool.QueueUserWorkItem(o =>
            {
                if (ZuneApplication.AddMedia(tempFilenames, this.MediaType))
                    return;
                Application.DeferredInvoke(delegate
                {
                    NotificationArea.Instance.Add(new MessageNotification(Shell.LoadString(StringId.IDS_LIBRARY_ADD_FILE_FAILED), NotificationTask.Library, NotificationState.OneShot)
                    {
                        SubMessage = Shell.LoadString(StringId.IDS_LIBRARY_ADD_FILE_FAILED_SUBMESSAGE)
                    });
                }, null);
           });
        }

        public override IPageState SaveAndRelease() => new DevicePivotManagingPageState(this);

        public static void BlockUpdatesFromDBList(
          IList list,
          BlockListUpdatesReason reason,
          bool block)
        {
            if (!(list is VirtualDatabaseList virtualDatabaseList))
                return;
            virtualDatabaseList.SetBlockChangesFlag((int)reason, block);
        }

        public static void DisableAutoRefresh(IList list)
        {
            if (!(list is LibraryVirtualList libraryVirtualList))
                return;
            libraryVirtualList.DisableAutoRefresh();
        }

        public override void InvokeSettings()
        {
            if (this.ShowDeviceContents)
                Shell.SettingsFrame.Settings.Device.Invoke();
            else if (this.MediaType == MediaType.Photo)
                Shell.SettingsFrame.Settings.Software.Invoke(SettingCategories.Photo);
            else if (this.MediaType == MediaType.PodcastEpisode || this.MediaType == MediaType.Podcast)
                Shell.SettingsFrame.Settings.Software.Invoke(SettingCategories.Podcast);
            else
                base.InvokeSettings();
        }

        private static string LibraryTemplate => "res://ZuneShellResources!Library.uix#Library";
    }
}
