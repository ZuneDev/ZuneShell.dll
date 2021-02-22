// Decompiled with JetBrains decompiler
// Type: ZuneUI.BooleanInputChoice
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneUI
{
    public class BooleanInputChoice : BooleanChoice
    {
        private bool available = true;

        internal BooleanInputChoice(ModelItem owner, string description, bool isAvailable)
          : base((IModelItemOwner)owner, description)
          => this.Available = isAvailable;

        public bool Available
        {
            get => this.available;
            set
            {
                if (this.available == value)
                    return;
                this.available = value;
                this.FirePropertyChanged(nameof(Available));
            }
        }
    }
}
