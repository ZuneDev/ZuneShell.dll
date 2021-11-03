// Decompiled with JetBrains decompiler
// Type: ZuneUI.ApplicationsPanel
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneUI
{
    public class ApplicationsPanel : ListPanel
    {
        internal ApplicationsPanel(ApplicationLibraryPage library)
          : base(library)
        {
        }

        protected ApplicationLibraryPage LibraryPage => base.LibraryPage as ApplicationLibraryPage;

        public override IList SelectedLibraryIds
        {
            get => this.LibraryPage.SelectedApplicationIDs;
            set => this.LibraryPage.SelectedApplicationIDs = value;
        }
    }
}
