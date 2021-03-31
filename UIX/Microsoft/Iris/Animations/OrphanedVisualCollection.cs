// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Animations.OrphanedVisualCollection
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using System;

namespace Microsoft.Iris.Animations
{
    internal class OrphanedVisualCollection : DisposableObject
    {
        private int _countEventsRemaining;
        private Vector<IVisual> _orphansList;
        private Vector<ActiveSequence> _sequenceList;
        private AnimationManager _animationManager;

        public OrphanedVisualCollection(AnimationManager aniManager)
        {
            this.DeclareOwner((object)aniManager);
            this._orphansList = new Vector<IVisual>();
            this._sequenceList = new Vector<ActiveSequence>();
            this._animationManager = aniManager;
            this._countEventsRemaining = 1;
            this._animationManager.RegisterAnimatedOrphans(this);
        }

        protected override void OnDispose()
        {
            foreach (ActiveSequence sequence in this._sequenceList)
            {
                sequence.Stop();
                sequence.Dispose((object)this);
            }
            this._sequenceList.Clear();
            foreach (IVisual orphans in this._orphansList)
            {
                orphans.Remove();
                orphans.UnregisterUsage((object)this);
            }
            this._orphansList.Clear();
            this._animationManager = (AnimationManager)null;
            base.OnDispose();
        }

        public void AddOrphan(IVisual visual)
        {
            visual.RegisterUsage((object)this);
            this._orphansList.Add(visual);
        }

        public void RegisterWaitForAnimation(ActiveSequence aseq, bool transfer)
        {
            aseq.AnimationCompleted += new EventHandler(this.OnDestroyAnimationComplete);
            ++this._countEventsRemaining;
            this.RegisterAnimation(aseq, transfer);
        }

        public void RegisterAnimation(ActiveSequence aseq, bool transfer)
        {
            if (transfer)
                aseq.TransferOwnership((object)this);
            else
                aseq.DeclareOwner((object)this);
            this._sequenceList.Add(aseq);
        }

        public void OnLayoutApplyComplete() => this.OnEventComplete((object)null, EventArgs.Empty);

        private void OnDestroyAnimationComplete(object sender, EventArgs args)
        {
            ActiveSequence activeSequence = sender as ActiveSequence;
            activeSequence.AnimationCompleted -= new EventHandler(this.OnDestroyAnimationComplete);
            this._sequenceList.Remove(activeSequence);
            activeSequence.Dispose((object)this);
            this.OnEventComplete(sender, args);
        }

        private void OnEventComplete(object sender, EventArgs args)
        {
            --this._countEventsRemaining;
            if (this.Waiting)
                return;
            if (this._animationManager != null)
                this._animationManager.UnregisterAnimatedOrphans(this);
            this.Dispose((object)this._animationManager);
        }

        public bool Waiting => this._countEventsRemaining > 0;

        public override string ToString() => InvariantString.Format("Orphans(WaitCount={0}, OrphanCount={1})", this._sequenceList != null ? (object)this._sequenceList.Count.ToString() : (object)"None", (object)this._orphansList.Count);
    }
}
