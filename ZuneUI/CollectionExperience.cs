// Decompiled with JetBrains decompiler
// Type: ZuneUI.CollectionExperience
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Util;
using System.Collections;

namespace ZuneUI
{
    public class CollectionExperience : Experience
    {
        private ArrayListDataSet _nodes;
        private Node _music;
        private Node _videos;
        private Node _photos;
        private Node _podcasts;
        private Node _channels;
        private Node _applications;
        private Node _radio;
        private Node _downloads;
        private bool _isDevice;

        public CollectionExperience(Frame frameOwner)
          : base(frameOwner, StringId.IDS_COLLECTION_PIVOT, SQMDataId.CollectionClicks)
        {
        }

        public CollectionExperience(Frame frameOwner, bool isDevice)
          : base(frameOwner, StringId.IDS_DEVICE_PIVOT, SQMDataId.DeviceClicks)
          => this._isDevice = isDevice;

        public override IList NodesList
        {
            get
            {
                if (this._nodes == null)
                {
                    this._nodes = new ArrayListDataSet(this);
                    this._nodes.Add(Music);
                    this._nodes.Add(Videos);
                    this._nodes.Add(Photos);
                    this._nodes.Add(Podcasts);
                    if (FeatureEnablement.IsFeatureEnabled(Features.eChannels))
                        this._nodes.Add(Channels);
                    if (FeatureEnablement.IsFeatureEnabled(Features.eRadio))
                        this._nodes.Add(Radio);
                    this.UpdateApplicationPivot();
                }
                return _nodes;
            }
        }

        public Node Music
        {
            get
            {
                if (this._music == null)
                    this._music = new Node(this, StringId.IDS_MUSIC_PIVOT, this._isDevice ? "Device\\Music" : "Collection\\Music\\Default", this._isDevice ? SQMDataId.DeviceMusicClicks : SQMDataId.CollectionMusicClicks);
                return this._music;
            }
        }

        public Node Videos
        {
            get
            {
                if (this._videos == null)
                    this._videos = new Node(this, StringId.IDS_VIDEO_PIVOT, this._isDevice ? "Device\\Videos" : "Collection\\Videos\\Default", this._isDevice ? SQMDataId.DeviceVideoClicks : SQMDataId.CollectionVideoClicks);
                return this._videos;
            }
        }

        public Node Photos
        {
            get
            {
                if (this._photos == null)
                    this._photos = new Node(this, StringId.IDS_PICTURES_PIVOT, this._isDevice ? "Device\\Photos" : "Collection\\Photos", this._isDevice ? SQMDataId.DevicePicturesClicks : SQMDataId.CollectionPicturesClicks);
                return this._photos;
            }
        }

        public Node Podcasts
        {
            get
            {
                if (this._podcasts == null)
                    this._podcasts = new Node(this, StringId.IDS_PODCASTS_PIVOT, this._isDevice ? "Device\\Podcasts" : "Collection\\Podcasts", this._isDevice ? SQMDataId.DevicePodcastsClicks : SQMDataId.CollectionPodcastsClicks);
                return this._podcasts;
            }
        }

        public Node Channels
        {
            get
            {
                if (this._channels == null)
                    this._channels = new Node(this, StringId.IDS_CHANNELS_PIVOT, this._isDevice ? "Device\\Channels" : "Collection\\Channels", this._isDevice ? SQMDataId.DeviceChannelsClicks : SQMDataId.CollectionChannelsClicks);
                return this._channels;
            }
        }

        public Node Applications
        {
            get
            {
                if (this._applications == null)
                    this._applications = new Node(this, StringId.IDS_APPS_PIVOT, this._isDevice ? "Device\\Applications" : "Collection\\Applications", SQMDataId.Invalid);
                return this._applications;
            }
        }

        public Node Radio
        {
            get
            {
                if (this._radio == null)
                    this._radio = new Node(this, StringId.IDS_RADIO_PIVOT, this._isDevice ? "Device\\Radio" : "Collection\\Radio", SQMDataId.Invalid);
                return this._radio;
            }
        }

        public Node Downloads
        {
            get
            {
                if (this._downloads == null)
                    this._downloads = new Node(this, StringId.IDS_DOWNLOADS_PIVOT, "Collection\\Downloads", SQMDataId.MarketplaceDownloadsClicks);
                return this._downloads;
            }
        }

        public void UpdateApplicationPivot()
        {
            if (FeatureEnablement.IsFeatureEnabled(Features.eGames) && ClientConfiguration.Shell.ShowApplicationPivot)
            {
                if (this.NodesList.Contains(Applications))
                    return;
                this.NodesList.Add(Applications);
            }
            else
            {
                if (!this.NodesList.Contains(Applications))
                    return;
                this.NodesList.Remove(Applications);
            }
        }

        public void UpdateDownloadPivot(bool show)
        {
            int nodeIndex = this.GetNodeIndex(this.Downloads);
            bool flag = nodeIndex != -1;
            if (show == flag)
                return;
            this.Downloads.Available = show;
            if (show)
                this.NodesList.Add(Downloads);
            else
                this.NodesList.RemoveAt(nodeIndex);
        }

        public override string DefaultUIPath => "Collection\\Default";
    }
}
