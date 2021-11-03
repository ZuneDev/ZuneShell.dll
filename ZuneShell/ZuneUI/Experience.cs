// Decompiled with JetBrains decompiler
// Type: ZuneUI.Experience
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;
using System;
using System.Collections;

namespace ZuneUI
{
    public abstract class Experience : Command
    {
        private bool _isCurrent;
        private Choice _nodes;
        private SQMDataId _sqmClickId;

        public Experience(Frame frameOwner)
          : base(frameOwner, "", null)
          => this._sqmClickId = SQMDataId.Invalid;

        public Experience(Frame frameOwner, StringId nameId, SQMDataId SQMClickId)
          : base(frameOwner, Shell.LoadString(nameId), null)
          => this._sqmClickId = SQMClickId;

        public Choice Nodes
        {
            get
            {
                if (this._nodes == null)
                {
                    this._nodes = new Choice(this);
                    this._nodes.Options = this.NodesList;
                }
                return this._nodes;
            }
            set
            {
                if (this._nodes == value)
                    return;
                this._nodes = value;
                this.FirePropertyChanged(nameof(Nodes));
            }
        }

        public abstract IList NodesList { get; }

        public Frame Frame => (Frame)this.Owner;

        public bool IsCurrent
        {
            get => this._isCurrent;
            set
            {
                if (this._isCurrent == value)
                    return;
                this._isCurrent = value;
                this.OnIsCurrentChanged();
                this.FirePropertyChanged(nameof(IsCurrent));
            }
        }

        public virtual string DefaultUIPath => "";

        protected int GetNodeIndex(Node node)
        {
            for (int index = 0; index < this.NodesList.Count; ++index)
            {
                if (this.NodesList[index] == node)
                    return index;
            }
            return -1;
        }

        protected virtual void OnIsCurrentChanged()
        {
        }

        protected override void OnInvoked()
        {
            if (this.IsCurrent)
                return;
            ((Frame)this.Owner).Experiences.ChosenValue = this;
            Node node = (Node)this.Nodes.ChosenValue;
            if (node == null && this.NodesList != null && this.NodesList.Count > 0)
                node = (Node)this.NodesList[0];
            node?.Invoke(InvokePolicy.Synchronous);
            if (this._sqmClickId != SQMDataId.Invalid)
                SQMLog.Log(this._sqmClickId, 1);
            base.OnInvoked();
        }
    }
}
