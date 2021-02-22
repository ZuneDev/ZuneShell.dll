// Decompiled with JetBrains decompiler
// Type: ZuneUI.MainFrame
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;
using System.Collections;

namespace ZuneUI
{
    public class MainFrame : Frame
    {
        private ArrayListDataSet _experiences;
        private QuickplayExperience _quickplay;
        private CollectionExperience _collection;
        private DeviceExperience _device;
        private MarketplaceExperience _marketplace;
        private SocialExperience _social;
        private TestExperience _test;
        private DiscExperience _disc;

        public MainFrame(IModelItemOwner owner)
          : base(owner)
        {
        }

        public override IList ExperiencesList
        {
            get
            {
                if (this._experiences == null)
                {
                    this._experiences = new ArrayListDataSet((IModelItemOwner)this);
                    if (FeatureEnablement.IsFeatureEnabled(Features.eQuickplay))
                        this._experiences.Add((object)this.Quickplay);
                    this._experiences.Add((object)this.Collection);
                    if (FeatureEnablement.IsFeatureEnabled(Features.eMarketplace))
                        this._experiences.Add((object)this.Marketplace);
                    if (FeatureEnablement.IsFeatureEnabled(Features.eSocial))
                        this._experiences.Add((object)this.Social);
                }
                return (IList)this._experiences;
            }
        }

        public QuickplayExperience Quickplay
        {
            get
            {
                if (this._quickplay == null)
                    this._quickplay = new QuickplayExperience((Frame)this);
                return this._quickplay;
            }
        }

        public CollectionExperience Collection
        {
            get
            {
                if (this._collection == null)
                    this._collection = new CollectionExperience((Frame)this);
                return this._collection;
            }
        }

        public MarketplaceExperience Marketplace
        {
            get
            {
                if (this._marketplace == null)
                    this._marketplace = new MarketplaceExperience((Frame)this);
                return this._marketplace;
            }
        }

        public SocialExperience Social
        {
            get
            {
                if (this._social == null)
                    this._social = new SocialExperience((Frame)this);
                return this._social;
            }
        }

        public DeviceExperience Device
        {
            get
            {
                if (this._device == null)
                {
                    this._device = new DeviceExperience((Frame)this);
                    this._device.UpdateShowDevice();
                }
                return this._device;
            }
        }

        public TestExperience Test
        {
            get
            {
                if (this._test == null)
                    this._test = new TestExperience((Frame)this);
                return this._test;
            }
        }

        public DiscExperience Disc
        {
            get
            {
                if (this._disc == null)
                {
                    this._disc = new DiscExperience((Frame)this);
                    IList experiencesList = this.ExperiencesList;
                    if (experiencesList == null || experiencesList[experiencesList.Count - 1] != this._disc)
                        this._disc.Available = false;
                }
                return this._disc;
            }
        }

        internal void ShowDevice(bool show)
        {
            IList experiencesList = this.ExperiencesList;
            int index = experiencesList.Count - 1;
            bool flag = experiencesList[index] == this.Device || index > 0 && experiencesList[index - 1] == this.Device;
            if (show == flag)
                return;
            this.Device.Available = show;
            if (show)
            {
                if (experiencesList[index] == this.Disc)
                    experiencesList.Insert(index, (object)this.Device);
                else
                    experiencesList.Add((object)this.Device);
            }
            else
                experiencesList.Remove((object)this.Device);
        }

        internal void ShowDisc(bool show)
        {
            IList experiencesList = this.ExperiencesList;
            int index = experiencesList.Count - 1;
            bool flag = experiencesList[index] == this.Disc;
            if (show == flag)
                return;
            this.Disc.Available = show;
            if (show)
                experiencesList.Add((object)this.Disc);
            else
                experiencesList.RemoveAt(index);
        }

        internal void ShowTest(bool show)
        {
            IList experiencesList = this.ExperiencesList;
            bool flag = experiencesList.Contains((object)this.Test);
            int index = experiencesList.Count - 1;
            while (index >= 0 && (experiencesList[index] == this.Disc || experiencesList[index] == this.Device))
                --index;
            if (show == flag)
                return;
            this.Test.Available = show;
            if (show)
                experiencesList.Insert(index + 1, (object)this.Test);
            else
                experiencesList.Remove((object)this.Test);
        }
    }
}
