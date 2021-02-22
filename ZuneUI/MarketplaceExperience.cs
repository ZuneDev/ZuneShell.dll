// Decompiled with JetBrains decompiler
// Type: ZuneUI.MarketplaceExperience
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;
using System.Collections;

namespace ZuneUI
{
    public class MarketplaceExperience : Experience
    {
        private ArrayListDataSet _nodes;
        private Node _recommendations;
        private Node _music;
        private Node _videos;
        private Node _podcasts;
        private Node _apps;
        private Node _channels;
        private Node _cart;
        private Node _downloads;
        private int _cartItemsCount;
        private bool _cartItemsCountInitialized;

        public MarketplaceExperience(Frame frameOwner)
          : base(frameOwner, StringId.IDS_MARKETPLACE_PIVOT, SQMDataId.MarketplaceClicks)
        {
        }

        public override IList NodesList
        {
            get
            {
                if (this._nodes == null)
                {
                    this._nodes = new ArrayListDataSet((IModelItemOwner)this);
                    if (FeatureEnablement.IsFeatureEnabled(Features.ePicks))
                        this._nodes.Add((object)this.Recommendations);
                    if (FeatureEnablement.IsFeatureEnabled(Features.eMusic))
                        this._nodes.Add((object)this.Music);
                    if (FeatureEnablement.IsFeatureEnabled(Features.eVideos))
                        this._nodes.Add((object)this.Videos);
                    if (FeatureEnablement.IsFeatureEnabled(Features.ePodcasts))
                        this._nodes.Add((object)this.Podcasts);
                    if (FeatureEnablement.IsFeatureEnabled(Features.eChannels))
                        this._nodes.Add((object)this.Channels);
                    if (FeatureEnablement.IsFeatureEnabled(Features.eGames) || FeatureEnablement.IsFeatureEnabled(Features.eApps))
                        this._nodes.Add((object)this.Apps);
                }
                return (IList)this._nodes;
            }
        }

        protected override void OnIsCurrentChanged()
        {
            base.OnIsCurrentChanged();
            if (!this.IsCurrent)
                return;
            CultureHelper.CheckMarketplaceCulture();
        }

        public Node Recommendations
        {
            get
            {
                if (this._recommendations == null)
                    this._recommendations = new Node((Experience)this, StringId.IDS_RECOMMENDATIONS_PIVOT, "Marketplace\\Recommendations\\Home", SQMDataId.MarketplaceRecommendationsClicks);
                return this._recommendations;
            }
        }

        public Node Music
        {
            get
            {
                if (this._music == null)
                    this._music = new Node((Experience)this, StringId.IDS_MUSIC_PIVOT, "Marketplace\\Music\\Home", SQMDataId.MarketplaceMusicClicks);
                return this._music;
            }
        }

        public Node Videos
        {
            get
            {
                if (this._videos == null)
                    this._videos = new Node((Experience)this, StringId.IDS_VIDEO_PIVOT, "Marketplace\\Videos\\Home", SQMDataId.MarketplaceVideosClicks);
                return this._videos;
            }
        }

        public Node Podcasts
        {
            get
            {
                if (this._podcasts == null)
                    this._podcasts = new Node((Experience)this, StringId.IDS_PODCASTS_PIVOT, "Marketplace\\Podcasts\\Home", SQMDataId.MarketplacePodcastsClicks);
                return this._podcasts;
            }
        }

        public Node Apps
        {
            get
            {
                if (this._apps == null)
                    this._apps = new Node((Experience)this, StringId.IDS_APPS_PIVOT, "Marketplace\\Apps\\Home", SQMDataId.Invalid);
                return this._apps;
            }
        }

        public Node Channels
        {
            get
            {
                if (this._channels == null)
                    this._channels = new Node((Experience)this, StringId.IDS_CHANNELS_PIVOT, "Marketplace\\Channels\\Home", SQMDataId.MarketplaceChannelsClicks);
                return this._channels;
            }
        }

        public Node Cart
        {
            get
            {
                if (this._cart == null)
                    this._cart = new Node((Experience)this, StringId.IDS_CART_PIVOT, "Marketplace\\Cart", SQMDataId.MarketplaceCartClicks);
                return this._cart;
            }
        }

        public Node Downloads
        {
            get
            {
                if (this._downloads == null)
                    this._downloads = new Node((Experience)this, StringId.IDS_DOWNLOADS_PIVOT, "Marketplace\\Downloads\\Home", SQMDataId.MarketplaceDownloadsClicks);
                return this._downloads;
            }
        }

        public int CartItemsCount
        {
            get => this._cartItemsCount;
            set
            {
                if (this._cartItemsCount == value)
                    return;
                this._cartItemsCount = value;
                this.FirePropertyChanged(nameof(CartItemsCount));
                Shell.MainFrame.Marketplace.UpdatePivots();
            }
        }

        public bool CartItemsCountInitialized
        {
            get => this._cartItemsCountInitialized;
            set
            {
                if (this._cartItemsCountInitialized == value)
                    return;
                this._cartItemsCountInitialized = value;
                this.FirePropertyChanged(nameof(CartItemsCountInitialized));
            }
        }

        public void UpdatePivots() => this.ShowCart(FeatureEnablement.IsFeatureEnabled(Features.eMusic) && this.CartItemsCount > 0);

        public void UpdateDownloadPivot(bool show)
        {
            int nodeIndex = this.GetNodeIndex(this.Downloads);
            bool flag = nodeIndex != -1;
            if (show == flag)
                return;
            this.Downloads.Available = show;
            if (show)
                this.NodesList.Add((object)this.Downloads);
            else
                this.NodesList.RemoveAt(nodeIndex);
        }

        private void ShowCart(bool show)
        {
            int nodeIndex = this.GetNodeIndex(this.Cart);
            bool flag = nodeIndex != -1;
            if (show == flag)
                return;
            this.Cart.Available = show;
            if (show)
            {
                int index = this.GetNodeIndex(this.Downloads);
                if (index == -1)
                    index = this.NodesList.Count;
                this.NodesList.Insert(index, (object)this.Cart);
            }
            else
                this.NodesList.RemoveAt(nodeIndex);
        }

        protected override void OnInvoked()
        {
            if (!this.IsCurrent && (Node)this.Nodes.ChosenValue == this._recommendations && !SignIn.Instance.SignedIn)
                this.Nodes.ChosenIndex = 1;
            base.OnInvoked();
        }

        public override string DefaultUIPath
        {
            get
            {
                if (FeatureEnablement.IsFeatureEnabled(Features.eMusic))
                    return "Marketplace\\Music\\Home";
                if (FeatureEnablement.IsFeatureEnabled(Features.eVideos))
                    return "Marketplace\\Videos\\Home";
                return FeatureEnablement.IsFeatureEnabled(Features.eApps) ? "Marketplace\\Apps\\Home" : "Marketplace\\Default";
            }
        }
    }
}
