// Decompiled with JetBrains decompiler
// Type: ZuneUI.Frame
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneUI
{
    public abstract class Frame : ModelItem
    {
        private bool _isCurrent;
        private Choice _experiences;

        public Frame(IModelItemOwner owner)
          : base(owner)
        {
        }

        public Choice Experiences
        {
            get
            {
                if (this._experiences == null)
                {
                    this._experiences = new Choice((IModelItemOwner)this);
                    this._experiences.Options = this.ExperiencesList;
                }
                return this._experiences;
            }
            set
            {
                if (this._experiences == value)
                    return;
                this._experiences = value;
                this.FirePropertyChanged(nameof(Experiences));
            }
        }

        public abstract IList ExperiencesList { get; }

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

        protected virtual void OnIsCurrentChanged()
        {
        }
    }
}
