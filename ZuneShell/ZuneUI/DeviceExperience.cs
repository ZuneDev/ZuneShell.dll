// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeviceExperience
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;
using System.Collections;
using System.ComponentModel;

namespace ZuneUI
{
    public class DeviceExperience : CollectionExperience
    {
        private ArrayListDataSet _nodes;
        private Node _status;
        private Node _friends;

        public DeviceExperience(Frame frameOwner)
          : base(frameOwner, true)
          => SyncControls.Instance.PropertyChanged += new PropertyChangedEventHandler(this.OnSyncPropertyChanged);

        protected override void OnDispose(bool disposing)
        {
            SyncControls.Instance.PropertyChanged -= new PropertyChangedEventHandler(this.OnSyncPropertyChanged);
            base.OnDispose(disposing);
        }

        public override IList NodesList
        {
            get
            {
                if (this._nodes == null)
                {
                    this._nodes = new ArrayListDataSet(this);
                    this._nodes.Add(Status);
                    this._nodes.Add(Music);
                    this._nodes.Add(Videos);
                    this._nodes.Add(Photos);
                    this._nodes.Add(Podcasts);
                    this.UpdateDeviceDependentPivots();
                }
                return _nodes;
            }
        }

        public Node Status
        {
            get
            {
                if (this._status == null)
                    this._status = new Node(this, StringId.IDS_SYNC_STATUS_PIVOT, "Device\\Status", SQMDataId.DeviceStatusClicks);
                return this._status;
            }
        }

        public Node Friends
        {
            get
            {
                if (this._friends == null)
                    this._friends = new Node(this, StringId.IDS_FRIENDS_PIVOT, "Device\\Friends", SQMDataId.DeviceFriendsClicks);
                return this._friends;
            }
        }

        protected override void OnInvoked()
        {
            ((MainFrame)this.Frame).ShowDevice(true);
            base.OnInvoked();
        }

        protected override void OnIsCurrentChanged()
        {
            this.UpdateShowDevice();
            base.OnIsCurrentChanged();
        }

        public void UpdateShowDevice() => ((MainFrame)this.Frame).ShowDevice(this.IsCurrent || this.AreAnyDevicesConnected());

        public bool AreAnyDevicesConnected()
        {
            foreach (UIDevice uiDevice in SingletonModelItem<UIDeviceList>.Instance)
            {
                if (uiDevice.IsConnectedToPC)
                    return true;
            }
            return false;
        }

        private void OnSyncPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (!(args.PropertyName == "CurrentDevice"))
                return;
            this.UpdateDeviceDependentPivots();
            this.Description = SyncControls.Instance.CurrentDevice.PivotDescription;
        }

        private void UpdateDeviceDependentPivots()
        {
            UIDevice currentDevice = SyncControls.Instance.CurrentDevice;
            if (this.NodesList.Contains(Applications))
                this.NodesList.Remove(Applications);
            if (this.NodesList.Contains(Friends))
                this.NodesList.Remove(Friends);
            if (this.NodesList.Contains(Channels))
                this.NodesList.Remove(Channels);
            if (FeatureEnablement.IsFeatureEnabled(Features.eSocial) && currentDevice.SupportsUserCards)
                this.NodesList.Add(Friends);
            if (FeatureEnablement.IsFeatureEnabled(Features.eChannels) && currentDevice.SupportsChannels)
                this.NodesList.Add(Channels);
            if (!FeatureEnablement.IsFeatureEnabled(Features.eGames) || !currentDevice.SupportsSyncApplications)
                return;
            this.NodesList.Add(Applications);
        }

        public override string DefaultUIPath => "Device\\Status";
    }
}
