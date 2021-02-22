// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeviceOverfillLand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneUI
{
    public class DeviceOverfillLand : DialogPage
    {
        private UIDevice _device;
        private SyncGroupList _list;
        private bool _deviceWasLocked;

        public DeviceOverfillLand(UIDevice device)
        {
            this._device = device;
            this._list = this.Device.GenerateSyncGroupList((IModelItemOwner)this, true);
            Deviceland.InitDevicePage((ZunePage)this);
            this.ShowBackArrow = false;
            this.ShowComputerIcon = ComputerIconState.Hide;
            this.ShowNowPlayingX = false;
            this.ShowPivots = false;
            this.ShowSearch = false;
            this.ShowSettings = false;
            this.UI = "res://ZuneShellResources!DeviceSyncGroups.uix#SyncGroupsPage";
            this.BottomBarUI = "res://ZuneShellResources!DeviceSyncGroups.uix#GasGauge";
        }

        public UIDevice Device => this._device;

        public SyncGroupList List => this._list;

        public override bool IsWizard => false;

        public override bool AllowAdvance
        {
            get => true;
            set => throw new NotSupportedException();
        }

        public override bool AllowCancel
        {
            get => true;
            set => throw new NotSupportedException();
        }

        public override bool RequireSecurityIcon
        {
            get => false;
            set => throw new NotSupportedException();
        }

        public override void NavigatePage(bool forward) => throw new NotSupportedException();

        public override bool NavigationAvailable(bool forward) => false;

        public override void Save()
        {
            this.List.CommitChanges((object)null);
            if (this._deviceWasLocked)
                return;
            this.Device.IsLockedAgainstSyncing = false;
            if (this.List.GasGauge.FreeSpace < 0L)
                return;
            this.Device.BeginSync(true, false);
        }

        public override void Exit() => ZuneShell.DefaultInstance.NavigateBack();

        public override void SaveAndExit()
        {
            this.Save();
            this.Exit();
        }

        public override void CancelAndExit()
        {
            this.Device.IsLockedAgainstSyncing = this._deviceWasLocked;
            this.Exit();
        }

        protected override void OnNavigatedToWorker()
        {
            this._deviceWasLocked = this.Device.IsLockedAgainstSyncing;
            if (!this._deviceWasLocked)
                this.Device.IsLockedAgainstSyncing = true;
            base.OnNavigatedToWorker();
        }
    }
}
