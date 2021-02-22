// Decompiled with JetBrains decompiler
// Type: ZuneUI.DetailsBackedSchemaSyncGroup
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using MicrosoftZuneLibrary;
using System;

namespace ZuneUI
{
    public class DetailsBackedSchemaSyncGroup : SchemaSyncGroup
    {
        private SyncRulesView _view;
        private int _index;
        private SyncRuleDetails _model;
        private bool _inOverfill;
        private System.Collections.Generic.List<SyncGroup> _masterList;
        private System.Collections.Generic.List<SyncGroup> _sortedList;
        private Timer _resortTimer;
        private bool _active;
        private bool _updatingActive;

        public DetailsBackedSchemaSyncGroup(
          SyncGroupList list,
          SyncRulesView view,
          SyncCategory type,
          bool inOverfill)
          : base(list, type)
        {
            this._view = view;
            this._index = -1;
            this._inOverfill = inOverfill;
            this._resortTimer = new Timer((IModelItemOwner)this);
            this._resortTimer.AutoRepeat = false;
            this._resortTimer.Tick += new EventHandler(this.OnResortTimerTick);
            this._resortTimer.Interval = 527;
            this._masterList = new System.Collections.Generic.List<SyncGroup>();
            this._sortedList = new System.Collections.Generic.List<SyncGroup>();
            this._active = true;
        }

        public override long Size => this.State != SyncGroupState.Calculated ? -1L : this._model.totalSize;

        public override int Count => this.State != SyncGroupState.Calculated ? -1 : (int)this._model.totalItems;

        public override SyncGroupState State => this._model == null || !this._model.calculated ? SyncGroupState.Uncalculated : SyncGroupState.Calculated;

        public override bool IsActive
        {
            get => this._active;
            set
            {
                if (this._active == value)
                    return;
                this._active = value;
                this._updatingActive = true;
                foreach (SyncGroup master in this._masterList)
                {
                    if (master.IsVisible)
                        master.IsActive = this._active;
                }
                this._updatingActive = false;
                this.Sort(true);
                this.FirePropertyChanged(nameof(IsActive));
            }
        }

        public override bool IsVisible => true;

        public System.Collections.Generic.List<SyncGroup> List
        {
            get => this._sortedList;
            private set
            {
                if (this._sortedList == value)
                    return;
                this._sortedList = value;
                this.FirePropertyChanged(nameof(List));
            }
        }

        public override void CommitChanges()
        {
            foreach (SyncGroup master in this._masterList)
                master.CommitChanges();
        }

        public override void CancelChanges()
        {
            foreach (SyncGroup master in this._masterList)
                master.CancelChanges();
        }

        public override void DataUpdated()
        {
            if (this._index == -1)
                return;
            this._model = this._view.GetItem(this._index);
            this.FirePropertyChanged("Size");
            this.FirePropertyChanged("Count");
            this.FirePropertyChanged("State");
            this.FirePropertyChanged("IsActive");
        }

        public void AssignDetails(int index)
        {
            this._index = index;
            this.DataUpdated();
        }

        public void Add(SyncGroup group)
        {
            this._masterList.Add(group);
            this._sortedList.Add(group);
            this.Sort(false);
        }

        public void Sort(bool immediately)
        {
            if (!immediately)
            {
                if (this._resortTimer.Enabled)
                    return;
                this._resortTimer.Start();
            }
            else
            {
                this._resortTimer.Stop();
                System.Collections.Generic.List<SyncGroup> syncGroupList = new System.Collections.Generic.List<SyncGroup>(this._masterList.Count);
                foreach (SyncGroup master in this._masterList)
                {
                    if ((master.IsActive || this._inOverfill) && master.IsVisible)
                        syncGroupList.Add(master);
                }
                syncGroupList.Sort();
                this.List = syncGroupList;
                if (this.List.Count != 0)
                    return;
                this.IsExpanded = false;
            }
        }

        public void UpdateActiveState()
        {
            if (this._updatingActive)
                return;
            bool flag1 = false;
            bool flag2 = false;
            foreach (SyncGroup master in this._masterList)
            {
                if (master.IsVisible)
                {
                    flag2 = true;
                    if (master.IsActive)
                    {
                        flag1 = true;
                        break;
                    }
                }
            }
            this._active = flag1 || !flag2;
            this.FirePropertyChanged("IsActive");
        }

        private void OnResortTimerTick(object sender, EventArgs e) => this.Sort(true);
    }
}
