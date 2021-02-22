// Decompiled with JetBrains decompiler
// Type: ZuneUI.CategoryPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using UIXControls;

namespace ZuneUI
{
    public class CategoryPage : DialogPage
    {
        private int _currentMenuItemIndex;
        private bool _isWizard;
        private bool _allowAdvance = true;
        private bool _allowCancel = true;
        private bool _requireSecurityIcon;
        private bool _isMenuDisabled;
        private Choice _menu;
        private CategoryPageNode _node;
        private Category _currentCategory;
        private static bool _inSettings = false;
        private static CategoryPage _entryPage;
        private bool _hideDeviceOnCancel;

        internal static CategoryPage EntryPage => _entryPage;

        public CategoryPage(CategoryPageNode node)
        {
            this.UI = "res://ZuneShellResources!Management.uix";
            this.BackgroundUI = "res://ZuneShellResources!Management.uix#Background";
            this.TransportControlStyle = TransportControlStyle.None;
            this.PivotPreference = node;
            this.ShowCDIcon = false;
            this.ShowDeviceIcon = false;
            this.ShowPlaylistIcon = false;
            this.ShowSettings = false;
            this.ShowSearch = false;
            this.ShowNowPlayingBackgroundOnIdle = false;
            this.CanEnterCompactMode = false;
            this.NotificationAreaVisible = false;
            this.TransportControlsVisible = false;
            if (_entryPage == null)
                _entryPage = this;
            this._node = node;
            if (node.Experience == Shell.SettingsFrame.Wizard)
                this._isWizard = true;
            this._hideDeviceOnCancel = node.HideDeviceOnCancel;
            this._menu = new Choice(this);
            this._menu.Options = node.Categories;
            this._menu.ChosenChanged += new EventHandler(this.CurrentCategoryChosenChanged);
            this.ShowBackArrow = node.AllowBackNavigation;
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing && this._menu != null)
            {
                this._menu.ChosenChanged -= new EventHandler(this.CurrentCategoryChosenChanged);
                this._menu.Dispose();
                this._menu = null;
            }
            base.OnDispose(disposing);
        }

        private void CurrentCategoryChosenChanged(object sender, EventArgs args) => this.CurrentCategory = (Category)this.Menu.ChosenValue;

        public Choice Menu => this._menu;

        public Category CurrentCategory
        {
            get => this._currentCategory;
            set
            {
                if (ZuneShell.DefaultInstance.NavigationLocked)
                {
                    ZuneShell.DefaultInstance.DeferredNavigateCategory = value;
                    ZuneShell.DefaultInstance.BlockedByNavigationLock = true;
                }
                else
                {
                    if (this._currentCategory == value)
                        return;
                    this._currentCategory = value;
                    int num = this._menu.Options.IndexOf(value);
                    if (num != -1)
                    {
                        this._menu.ChosenIndex = num;
                        this.CurrentMenuItemIndex = num;
                    }
                    this.FirePropertyChanged(nameof(CurrentCategory));
                }
            }
        }

        public void ReleaseDeferredNavigation()
        {
            ZuneShell defaultInstance = ZuneShell.DefaultInstance;
            if (defaultInstance.DeferredNavigateCategory != null)
            {
                Category navigateCategory = defaultInstance.DeferredNavigateCategory;
                defaultInstance.DeferredNavigateCategory = null;
                this.CurrentCategory = navigateCategory;
            }
            else
            {
                if (defaultInstance.DeferredNavigateNode == null)
                    return;
                Node deferredNavigateNode = defaultInstance.DeferredNavigateNode;
                defaultInstance.DeferredNavigateNode = null;
                deferredNavigateNode.Invoke();
            }
        }

        protected override void OnNavigatedAwayWorker(IPage destination)
        {
            base.OnNavigatedAwayWorker(destination);
            if (ZuneShell.DefaultInstance == null)
                return;
            Management management = ZuneShell.DefaultInstance.Management;
            management.RemoveNSSDeviceListChangeEvent();
            management.CurrentCategoryPage = null;
        }

        protected override void OnNavigatedToWorker()
        {
            if (this.NavigationArguments != null && this.NavigationArguments.Contains("Host"))
            {
                this._menu.ChosenValue = (Category)this.NavigationArguments["Host"];
                this.NavigationArguments.Remove("Host");
            }
            ZuneShell.DefaultInstance.Management.CurrentCategoryPage = this;
            if (!_inSettings)
            {
                _inSettings = true;
                this.PauseSyncIfNecessary();
            }
            base.OnNavigatedToWorker();
        }

        public override IPageState SaveAndRelease() => new CategoryPageState(this);

        public override bool HandleBack()
        {
            bool flag = _entryPage == this;
            if (flag && ZuneShell.DefaultInstance.Management.HasPendingCommits)
            {
                Command yesCommand = new Command(this, Shell.LoadString(StringId.IDS_DIALOG_YES), null);
                yesCommand.Invoked += (sender, args) => this.SaveAndExit();
                Command noCommand = new Command(this, Shell.LoadString(StringId.IDS_DIALOG_NO), null);
                noCommand.Invoked += (sender, args) => this.CancelAndExit();
                MessageBox.Show(Shell.LoadString(StringId.IDS_SAVE_CHANGES_DIALOG_TITLE), Shell.LoadString(StringId.IDS_SAVE_CHANGES_ON_BACK_DIALOG_TEXT), yesCommand, noCommand, null);
                return true;
            }
            if (!flag)
                return false;
            _entryPage = null;
            this.CancelAndExit();
            return true;
        }

        public override bool HandleEscape()
        {
            ZuneShell.DefaultInstance.NavigateBack();
            return true;
        }

        public bool InFUE => this._node == Shell.SettingsFrame.Wizard.FUE;

        public bool InDeviceSettings => this._node == Shell.SettingsFrame.Settings.Device;

        public bool MenuDisabled
        {
            get => this._isMenuDisabled;
            set
            {
                if (value == this._isMenuDisabled)
                    return;
                this._isMenuDisabled = value;
                this.FirePropertyChanged(nameof(MenuDisabled));
            }
        }

        public override bool IsWizard => this._isWizard;

        public bool MenuItemsAvailable => this._menu != null;

        private int CurrentMenuItemIndex
        {
            get => this._currentMenuItemIndex;
            set
            {
                if (this._currentMenuItemIndex == value)
                    return;
                this._currentMenuItemIndex = value;
                this.AllowAdvance = true;
            }
        }

        public override bool AllowAdvance
        {
            get => this._allowAdvance;
            set
            {
                if (this._allowAdvance == value)
                    return;
                this._allowAdvance = value;
                this.FirePropertyChanged(nameof(AllowAdvance));
            }
        }

        public override bool AllowCancel
        {
            get => this._allowCancel;
            set
            {
                if (this._allowCancel == value)
                    return;
                this._allowCancel = value;
                this.FirePropertyChanged(nameof(AllowCancel));
            }
        }

        public override bool RequireSecurityIcon
        {
            get => this._requireSecurityIcon;
            set
            {
                if (this._requireSecurityIcon == value)
                    return;
                this._requireSecurityIcon = value;
                this.FirePropertyChanged(nameof(RequireSecurityIcon));
            }
        }

        public override void NavigatePage(bool forward)
        {
            if (this.Menu.Options.Count == 1)
                return;
            if (forward)
                ++this.CurrentMenuItemIndex;
            else
                --this.CurrentMenuItemIndex;
            this.Menu.ChosenValue = this.Menu.Options[this.CurrentMenuItemIndex];
        }

        public override bool NavigationAvailable(bool forward)
        {
            if (!this.IsWizard)
                return false;
            return forward ? this._currentMenuItemIndex < this.Menu.Options.Count - 1 : this._currentMenuItemIndex > 0;
        }

        public override void Save()
        {
            ZuneShell.DefaultInstance.Management.CommitListSave();
            ZuneShell.DefaultInstance.Management.DeviceManagement.SetupComplete(false);
        }

        public override void Exit()
        {
            _entryPage = null;
            if (!this.InFUE)
                ZuneShell.DefaultInstance.NavigateBack();
            this.RestartSyncIfNecessary();
            _inSettings = false;
            DeviceManagement.NavigatingToWizard = false;
            if (ZuneShell.DefaultInstance != null)
                ZuneShell.DefaultInstance.DisposeManagement();
            ZuneShell.DefaultInstance.NavigationLocked = false;
            ZuneShell.DefaultInstance.Management.DeviceManagement.SetupComplete(false);
            Application.DeferredInvoke(delegate
           {
               DeviceManagement.HandleSetupQueue();
           }, DeferredInvokePriority.Low);
            this.CancelAllFirmwareUpdates();
        }

        private void CancelAllFirmwareUpdates()
        {
            foreach (UIDevice uiDevice in SingletonModelItem<UIDeviceList>.Instance)
            {
                if (uiDevice.UIFirmwareUpdater != null && uiDevice.UIFirmwareUpdater.IsCheckingForUpdates)
                    uiDevice.UIFirmwareUpdater.CancelFirmwareUpdate();
            }
        }

        public override void SaveAndExit()
        {
            this.Save();
            this.Exit();
        }

        public override void CancelAndExit()
        {
            this.Exit();
            if (DeviceManagement.SetupDevice == null || !this._hideDeviceOnCancel && !DeviceManagement.SetupDevice.RequiresFirmwareUpdate)
                return;
            if (SyncControls.Instance.ChangeIntoSetupDevice)
            {
                SyncControls.Instance.ChangeIntoSetupDevice = false;
                DeviceManagement.SetupDevice = null;
            }
            else
                DeviceManagement.HideSetupDevice();
        }

        public void PauseSyncIfNecessary()
        {
            if (DeviceManagement.NavigatingToWizard)
                return;
            SyncControls.Instance.CurrentDevice.IsLockedAgainstSyncing = true;
        }

        public void RestartSyncIfNecessary()
        {
            UIDevice currentDevice = SyncControls.Instance.CurrentDevice;
            if (DeviceManagement.NavigatingToWizard || !currentDevice.IsLockedAgainstSyncing)
                return;
            currentDevice.IsLockedAgainstSyncing = false;
            if (!currentDevice.IsReadyForSync)
                return;
            currentDevice.BeginSync(true, false);
        }
    }
}
