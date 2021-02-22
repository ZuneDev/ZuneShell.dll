// Decompiled with JetBrains decompiler
// Type: ZuneUI.ListReleaseBehaviorManager
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using MicrosoftZuneLibrary;
using System.Collections;

namespace ZuneUI
{
    public class ListReleaseBehaviorManager
    {
        private LibraryVirtualList _virtualList;
        private ReleaseBehavior _cachedBehavior;

        public void KeepItemsInMemory(IList list)
        {
            if (list == null || list.Count <= 0)
                return;
            LibraryVirtualList owner = ((LibraryDataProviderListItem)list[0]).GetOwner();
            if (owner.VisualReleaseBehavior != ReleaseBehavior.ReleaseReference)
                return;
            this._cachedBehavior = owner.VisualReleaseBehavior;
            owner.VisualReleaseBehavior = ReleaseBehavior.KeepReference;
            this._virtualList = owner;
        }

        public void RestoreDefaultBehavior()
        {
            if (this._virtualList == null)
                return;
            this._virtualList.VisualReleaseBehavior = this._cachedBehavior;
            this._virtualList = (LibraryVirtualList)null;
        }
    }
}
