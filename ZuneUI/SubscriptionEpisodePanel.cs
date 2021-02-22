// Decompiled with JetBrains decompiler
// Type: ZuneUI.SubscriptionEpisodePanel
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneUI
{
    public class SubscriptionEpisodePanel : ListPanel
    {
        private ArrayList _selectedLibraryIds = new ArrayList();

        public override IList SelectedLibraryIds => (IList)this._selectedLibraryIds;

        public SubscriptionEpisodePanel(SubscriptionLibraryPage page)
          : base((IModelItemOwner)page)
        {
        }
    }
}
