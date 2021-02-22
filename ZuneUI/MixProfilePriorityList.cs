// Decompiled with JetBrains decompiler
// Type: ZuneUI.MixProfilePriorityList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;

namespace ZuneUI
{
    public class MixProfilePriorityList : MixPriorityList
    {
        public MixProfilePriorityList(MixResult mixResultSeed)
          : base(mixResultSeed)
        {
        }

        public override void AddList(IList sourceList, string reason, int maxItems) => this.AddList(sourceList, reason, maxItems, new MixPriorityList.GetItemPriorityDelegate(MixResultProfile.GetItemPriority), new MixPriorityList.CreateItemInstanceDelegate(MixResultProfile.CreateInstance));
    }
}
