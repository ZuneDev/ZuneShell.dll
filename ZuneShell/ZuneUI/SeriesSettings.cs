// Decompiled with JetBrains decompiler
// Type: ZuneUI.SeriesSettings
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Subscription;
using System.Collections;

namespace ZuneUI
{
    public class SeriesSettings : ModelItem
    {
        private Choice m_playbackChoice;
        private Choice m_keepEpisodesChoice;
        private Choice m_syncChoice;
        private Choice m_keepEpisodesChoicePerPhone;
        private SubscriptionManager m_subscriptionManager;
        private int m_seriesId;
        private UIDevice m_device;
        private uint keepEpisodesOriginalValue;
        private ESeriesPlaybackOrder playbackOrderOriginalValue;
        private PodcastSyncLimit syncRuleOriginalValue;
        private int keepEpisodesPerPhoneOriginalValue;

        public SeriesSettings(SubscriptionManager subscriptionManager, int seriesId)
        {
            this.m_subscriptionManager = subscriptionManager;
            this.m_seriesId = seriesId;
            this.m_device = SyncControls.Instance.CurrentDevice;
            this.keepEpisodesOriginalValue = (uint)ClientConfiguration.Series.PodcastDefaultKeepEpisodes;
            this.playbackOrderOriginalValue = (ESeriesPlaybackOrder)ClientConfiguration.Series.PodcastDefaultPlaybackOrder;
            this.m_subscriptionManager.GetManagementSettings(this.m_seriesId, out this.keepEpisodesOriginalValue, out this.playbackOrderOriginalValue);
            this.m_keepEpisodesChoice = new Choice(this);
            this.m_keepEpisodesChoice.Options = NamedIntOption.PodcastKeepOptions;
            NamedIntOption.SelectOptionByValue(this.m_keepEpisodesChoice, (int)this.keepEpisodesOriginalValue);
            this.m_playbackChoice = new Choice(this);
            this.m_playbackChoice.Options = NamedIntOption.PodcastPlaybackOptions;
            NamedIntOption.SelectOptionByValue(this.m_playbackChoice, (int)this.playbackOrderOriginalValue);
            this.m_syncChoice = new Choice(this);
            this.m_syncChoice.Options = NamedIntOption.PodcastSyncOptions;
            if (this.m_device.IsValid)
            {
                this.syncRuleOriginalValue = this.m_device.GetPodcastSyncLimit(this.m_seriesId);
                NamedIntOption.SelectOptionByValue(this.m_syncChoice, (int)this.syncRuleOriginalValue);
            }
            this.m_keepEpisodesChoicePerPhone = new Choice(this);
            this.m_keepEpisodesChoicePerPhone.Options = NamedIntOption.PodcastKeepOptions;
            if (!this.m_device.IsValid)
                return;
            this.keepEpisodesPerPhoneOriginalValue = this.m_device.GetPodcastSyncLimitWithValue(this.m_seriesId);
            NamedIntOption.SelectOptionByValue(this.m_keepEpisodesChoicePerPhone, this.keepEpisodesPerPhoneOriginalValue);
        }

        public Choice PlaybackChoice => this.m_playbackChoice;

        public Choice KeepEpisodesChoice => this.m_keepEpisodesChoice;

        public Choice SyncChoice => this.m_syncChoice;

        public Choice KeepEpisodesChoicePerPhone => this.m_keepEpisodesChoicePerPhone;

        public void Apply()
        {
            bool flag = false;
            uint keepEpisodes = (uint)((NamedIntOption)this.m_keepEpisodesChoice.ChosenValue).Value;
            ESeriesPlaybackOrder playbackOrder = (ESeriesPlaybackOrder)((NamedIntOption)this.m_playbackChoice.ChosenValue).Value;
            if ((int)keepEpisodes != (int)this.keepEpisodesOriginalValue || playbackOrder != this.playbackOrderOriginalValue)
            {
                this.m_subscriptionManager.SetManagementSettings(this.m_seriesId, keepEpisodes, playbackOrder);
                flag = true;
            }
            if (this.m_device.IsValid)
            {
                PodcastSyncLimit limit = (PodcastSyncLimit)((NamedIntOption)this.m_syncChoice.ChosenValue).Value;
                if (limit != this.syncRuleOriginalValue)
                {
                    this.m_device.SetPodcastSyncLimit(this.m_seriesId, limit);
                    flag = true;
                }
            }
            if (!flag)
                return;
            SyncControls.Instance.CurrentDevice.BeginSync(true, false);
        }
    }
}
