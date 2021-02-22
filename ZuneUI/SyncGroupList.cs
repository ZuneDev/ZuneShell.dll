// Decompiled with JetBrains decompiler
// Type: ZuneUI.SyncGroupList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;
using MicrosoftZuneLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UIXControls;

namespace ZuneUI
{
    public class SyncGroupList : ModelItem
    {
        private UIDevice _device;
        private SyncRulesView _snapshot;
        private bool _inOverfill;
        private bool _inManagement;
        private UIGasGauge _gauge;
        private Dictionary<SyncRuleDetails, SyncGroup> _existingRulesList;
        private Dictionary<int, SyncGroup> _complexRulesList;
        private ProxySettingDelegate _commitDelegate;
        private bool _committed;
        private SchemaSyncGroup _music;
        private SchemaSyncGroup _video;
        private SchemaSyncGroup _photo;
        private SchemaSyncGroup _podcast;
        private SchemaSyncGroup _friend;
        private SchemaSyncGroup _channel;
        private SchemaSyncGroup _application;
        private SchemaSyncGroup _audiobook;
        private SchemaSyncGroup _guest;
        private System.Collections.Generic.List<SchemaSyncGroup> _schemas;

        public SyncGroupList(
          IModelItemOwner parent,
          UIDevice device,
          SyncRulesView snapshot,
          bool inOverfill)
          : base(parent)
        {
            this._device = device;
            this._snapshot = snapshot;
            this._inOverfill = inOverfill;
            this._inManagement = parent is DeviceManagement;
            this._commitDelegate = new ProxySettingDelegate(this.CommitChanges);
            this._gauge = this._snapshot == null ? new UIGasGauge((IModelItemOwner)this, (MicrosoftZuneLibrary.GasGauge)null) : new UIGasGauge((IModelItemOwner)this, this._snapshot.PredictedGasGauge);
            this._existingRulesList = new Dictionary<SyncRuleDetails, SyncGroup>((IEqualityComparer<SyncRuleDetails>)new SyncGroupList.SyncRuleDetailsHasher());
            this._complexRulesList = new Dictionary<int, SyncGroup>();
            this._schemas = new System.Collections.Generic.List<SchemaSyncGroup>(8);
            this._music = (SchemaSyncGroup)new DetailsBackedSchemaSyncGroup(this, this._snapshot, SyncCategory.Music, this._inOverfill);
            this._schemas.Add(this._music);
            this._video = (SchemaSyncGroup)new DetailsBackedSchemaSyncGroup(this, this._snapshot, SyncCategory.Video, this._inOverfill);
            this._schemas.Add(this._video);
            this._photo = (SchemaSyncGroup)new DetailsBackedSchemaSyncGroup(this, this._snapshot, SyncCategory.Photo, this._inOverfill);
            this._schemas.Add(this._photo);
            this._podcast = (SchemaSyncGroup)new DetailsBackedSchemaSyncGroup(this, this._snapshot, SyncCategory.Podcast, this._inOverfill);
            this._schemas.Add(this._podcast);
            if (this.Device.UserId != 0)
            {
                if (FeatureEnablement.IsFeatureEnabled(Features.eSocial) && device.SupportsUserCards)
                {
                    this._friend = (SchemaSyncGroup)new DetailsBackedSchemaSyncGroup(this, this._snapshot, SyncCategory.Friend, this._inOverfill);
                    this._schemas.Add(this._friend);
                }
                if (FeatureEnablement.IsFeatureEnabled(Features.eChannels) && device.SupportsChannels)
                {
                    this._channel = (SchemaSyncGroup)new DetailsBackedSchemaSyncGroup(this, this._snapshot, SyncCategory.Channel, this._inOverfill);
                    this._schemas.Add(this._channel);
                }
            }
            if (FeatureEnablement.IsFeatureEnabled(Features.eGames) && this.Device.SupportsSyncApplications)
            {
                this._application = (SchemaSyncGroup)new DetailsBackedSchemaSyncGroup(this, this._snapshot, SyncCategory.Application, this._inOverfill);
                this._schemas.Add(this._application);
            }
            if (this.InOverfill)
            {
                this._guest = (SchemaSyncGroup)new GuestSchemaSyncGroup(this, this.GasGauge.GuestSpace);
                this._audiobook = (SchemaSyncGroup)new DetailsBackedSchemaSyncGroup(this, this._snapshot, SyncCategory.Audiobook, this._inOverfill);
                this._schemas.Add(this._audiobook);
            }
            if (this._snapshot == null)
                return;
            this._snapshot.ItemAddedEvent += new SyncRulesViewItemAddedHandler(this.ItemAdded);
            this._snapshot.ItemUpdatedEvent += new SyncRulesViewItemUpdatedHandler(this.ItemUpdated);
            if (this._snapshot.Count <= 0)
                return;
            for (int indexInRulesSnapshot = 0; indexInRulesSnapshot < this._snapshot.Count; ++indexInRulesSnapshot)
                this.AddExistingSyncGroup(indexInRulesSnapshot);
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                if (this._snapshot != null)
                {
                    this._snapshot.ItemAddedEvent -= new SyncRulesViewItemAddedHandler(this.ItemAdded);
                    this._snapshot.ItemUpdatedEvent -= new SyncRulesViewItemUpdatedHandler(this.ItemUpdated);
                    this._snapshot.Dispose();
                }
                if (!this._committed)
                {
                    if (this._inManagement)
                        ZuneShell.DefaultInstance.Management.CommitList.Remove((object)this._commitDelegate);
                    this.CancelChanges();
                }
            }
            base.OnDispose(disposing);
        }

        public UIDevice Device => this._device;

        public bool InOverfill => this._inOverfill;

        public System.Collections.Generic.List<SchemaSyncGroup> List
        {
            get => this._schemas;
            private set
            {
                if (this._schemas == value)
                    return;
                this._schemas = value;
                this.FirePropertyChanged(nameof(List));
            }
        }

        public UIGasGauge GasGauge => this._gauge;

        public SchemaSyncGroup GetGroupForSchema(SyncCategory schema)
        {
            switch (schema)
            {
                case SyncCategory.Music:
                    return this._music;
                case SyncCategory.Video:
                    return this._video;
                case SyncCategory.Photo:
                    return this._photo;
                case SyncCategory.Podcast:
                    return this._podcast;
                case SyncCategory.Friend:
                    return this._friend;
                case SyncCategory.Audiobook:
                    return this._audiobook;
                case SyncCategory.Channel:
                    return this._channel;
                case SyncCategory.Application:
                    return this._application;
                case SyncCategory.Guest:
                    return this._guest;
                default:
                    return (SchemaSyncGroup)null;
            }
        }

        public void AddNewComplexSyncGroup(int playlistID)
        {
            NewComplexSyncGroup complexSyncGroup = new NewComplexSyncGroup(this, playlistID);
            this.AddSyncGroup((SyncGroup)complexSyncGroup);
            if (this.GetGroupForSchema(complexSyncGroup.Type) is DetailsBackedSchemaSyncGroup groupForSchema)
                groupForSchema.IsExpanded = true;
            if (!this._inManagement)
                return;
            ZuneShell.DefaultInstance.Management.CommitList[(object)this._commitDelegate] = (object)this._device.ID;
        }

        public void DeleteGroup(SyncGroup group) => this.DeleteGroups((IList)new SyncGroup[1]
        {
      group
        });

        public void DeleteGroups(IList groupList)
        {
            if (groupList == null || groupList.Count < 1)
                return;
            if (groupList.Count == 1)
            {
                MessageBox.Show(Shell.LoadString(StringId.IDS_REMOVE_SYNC_GROUP_DIALOG_TITLE), string.Format(Shell.LoadString(StringId.IDS_REMOVE_SINGLE_SYNC_GROUP_DIALOG_TEXT), (object)((SyncGroup)groupList[0]).Title), (EventHandler)delegate
             {
                 this.DeleteGroupsConfirmed(groupList);
             });
            }
            else
            {
                if (groupList.Count <= 1)
                    return;
                MessageBox.Show(Shell.LoadString(StringId.IDS_REMOVE_SYNC_GROUP_DIALOG_TITLE), Shell.LoadString(StringId.IDS_REMOVE_MULTIPLE_SYNC_GROUP_DIALOG_TEXT), (EventHandler)delegate
               {
                   this.DeleteGroupsConfirmed(groupList);
               });
            }
        }

        private void DeleteGroupsConfirmed(IList groupList)
        {
            if (groupList == null)
                return;
            System.Collections.Generic.List<SyncCategory> syncCategoryList = new System.Collections.Generic.List<SyncCategory>();
            foreach (object group in (IEnumerable)groupList)
            {
                if (group is SyncGroup syncGroup)
                {
                    syncGroup.IsActive = false;
                    if (!syncCategoryList.Contains(syncGroup.Type))
                        syncCategoryList.Add(syncGroup.Type);
                }
            }
            if (this._inManagement)
                ZuneShell.DefaultInstance.Management.CommitList[(object)this._commitDelegate] = (object)this._device.ID;
            foreach (SyncCategory schema in syncCategoryList)
            {
                if (this.GetGroupForSchema(schema) is DetailsBackedSchemaSyncGroup groupForSchema)
                    groupForSchema.Sort(true);
            }
        }

        public void ComplexGroupEdited(int playlistID)
        {
            if (this._complexRulesList.ContainsKey(playlistID))
            {
                SyncGroup complexRules = this._complexRulesList[playlistID];
                complexRules.DataEdited();
                if (this.GetGroupForSchema(complexRules.Type) is DetailsBackedSchemaSyncGroup groupForSchema)
                    groupForSchema.Sort(true);
            }
            if (!this._inManagement)
                return;
            ZuneShell.DefaultInstance.Management.CommitList[(object)this._commitDelegate] = (object)this._device.ID;
        }

        public void CommitChanges(object throwaway)
        {
            if (this.IsDisposed)
                return;
            foreach (SyncGroup schema in this._schemas)
                schema.CommitChanges();
            this._committed = true;
        }

        public void CancelChanges()
        {
            foreach (SyncGroup schema in this._schemas)
                schema.CancelChanges();
        }

        private void ItemUpdated(SyncRulesView syncRulesView, int iItem) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           if (this.IsDisposed)
               return;
           SyncRuleDetails key = syncRulesView.GetItem(iItem);
           if (!this._existingRulesList.ContainsKey(key))
               return;
           SyncGroup existingRules = this._existingRulesList[key];
           existingRules.DataUpdated();
           if (key.allMedia || !(this.GetGroupForSchema(existingRules.Type) is DetailsBackedSchemaSyncGroup groupForSchema))
               return;
           groupForSchema.Sort(false);
       }, (object)null);

        private void ItemAdded(SyncRulesView syncRulesView, int iItem) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           if (this.IsDisposed || this._existingRulesList.ContainsKey(syncRulesView.GetItem(iItem)))
               return;
           this.AddExistingSyncGroup(iItem);
       }, (object)null);

        private void AddExistingSyncGroup(int indexInRulesSnapshot)
        {
            SyncRuleDetails key = this._snapshot.GetItem(indexInRulesSnapshot);
            if (this._existingRulesList.ContainsKey(key))
                return;
            if (key.allMedia)
            {
                if (!(this.GetGroupForSchema((SyncCategory)key.syncCategory) is DetailsBackedSchemaSyncGroup groupForSchema))
                    return;
                this._existingRulesList.Add(key, (SyncGroup)groupForSchema);
                groupForSchema.AssignDetails(indexInRulesSnapshot);
            }
            else
            {
                SyncCategory syncCategory = !key.complex ? (SyncCategory)key.syncCategory : UIDeviceList.MapMediaTypeToSyncCategory(PlaylistManager.GetAutoPlaylistSchema(key.mediaId));
                bool isExpandedEntry = this.Device.IsSyncAllFor(syncCategory) || this.Device.IsManualFor(syncCategory);
                if (!this.InOverfill && isExpandedEntry)
                    return;
                ExistingSyncGroup existingSyncGroup = new ExistingSyncGroup(this, this._snapshot, indexInRulesSnapshot, isExpandedEntry);
                this._existingRulesList.Add(key, (SyncGroup)existingSyncGroup);
                this.AddSyncGroup((SyncGroup)existingSyncGroup);
            }
        }

        private void AddSyncGroup(SyncGroup group)
        {
            if (group.IsComplex)
                this._complexRulesList.Add(group.ID, group);
            if (!(this.GetGroupForSchema(group.Type) is DetailsBackedSchemaSyncGroup groupForSchema))
                return;
            groupForSchema.Add(group);
        }

        private class SyncRuleDetailsHasher : IEqualityComparer<SyncRuleDetails>
        {
            public bool Equals(SyncRuleDetails x, SyncRuleDetails y) => x.mediaId == y.mediaId && x.mediaType == y.mediaType && x.syncCategory == y.syncCategory;

            public int GetHashCode(SyncRuleDetails obj) => obj.mediaId ^ (int)obj.mediaType << 24 ^ (int)obj.syncCategory << 16;
        }
    }
}
