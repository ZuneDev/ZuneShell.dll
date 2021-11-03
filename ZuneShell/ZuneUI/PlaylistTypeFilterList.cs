// Decompiled with JetBrains decompiler
// Type: ZuneUI.PlaylistTypeFilterList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Playlist;
using System.Collections;

namespace ZuneUI
{
    public class PlaylistTypeFilterList : FilterList
    {
        private IList _typesToInclude = AllTypes;
        private static int[] _allTypes = new int[0];

        public IList TypesToInclude
        {
            get => this._typesToInclude;
            set
            {
                if (this._typesToInclude == value)
                    return;
                this._typesToInclude = value;
                this.FirePropertyChanged(nameof(TypesToInclude));
                this.ProduceFilteredList();
            }
        }

        protected override bool ShouldIncludeItem(int sourceIndex, int targetIndex, object item)
        {
            if (this.TypesToInclude == AllTypes)
                return true;
            if (this.TypesToInclude == null || this.TypesToInclude.Count <= 0 || !(item is DataProviderObject dataProviderObject))
                return false;
            object property = dataProviderObject.GetProperty("PlaylistType");
            return property != null && property is int && this.TypesToInclude.Contains((PlaylistType)property);
        }

        public static IList AllTypes => _allTypes;
    }
}
