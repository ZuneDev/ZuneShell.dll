// Decompiled with JetBrains decompiler
// Type: ZuneUI.DiscExperience
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;
using System.Collections;

namespace ZuneUI
{
    public class DiscExperience : Experience
    {
        private ArrayListDataSet _nodes = new ArrayListDataSet();
        private CDAlbumCommand _burnList;
        private CDAlbumCommand _noCD;

        public DiscExperience(Frame frameOwner)
          : base(frameOwner, StringId.IDS_DISC_PIVOT, SQMDataId.DiscClicks)
        {
        }

        public override IList NodesList => _nodes;

        public CDAlbumCommand BurnList
        {
            get
            {
                if (this._burnList == null)
                    this._burnList = new CDAlbumCommand(this, StringId.IDS_PLAYLIST_BURN_LIST);
                return this._burnList;
            }
        }

        public CDAlbumCommand NoCD
        {
            get
            {
                if (this._noCD == null)
                    this._noCD = new CDAlbumCommand(this, StringId.IDS_NO_CD);
                return this._noCD;
            }
        }

        public bool HasCD => this._nodes == null || this._nodes[0] != this.NoCD;

        protected override void OnIsCurrentChanged() => this.UpdateShowDisc();

        public void UpdateShowDisc() => ((MainFrame)this.Frame).ShowDisc(this.IsCurrent || CDAccess.Instance.HasLoadedMedia || !CDAccess.Instance.IsBurnListEmpty);

        public void RecalculateAvailableNodes()
        {
            this._nodes.Clear();
            foreach (CDAlbumCommand cd in CDAccess.Instance.CDs)
            {
                if (cd != null && cd.TOC != null)
                    this._nodes.Add(cd);
            }
            if (CDAccess.Instance.BurnListId >= 0)
                this._nodes.Add(BurnList);
            if (this._nodes.Count == 0)
                this._nodes.Add(NoCD);
            this.UpdateShowDisc();
        }

        public void UpdateNodes(CDAccess cdAccess, char changedLetter, bool mediaArrived)
        {
            this.RecalculateAvailableNodes();
            CDAlbumCommand nodes1 = (CDAlbumCommand)this.NodesList[0];
            if (this.IsCurrent)
            {
                if (!(ZuneShell.DefaultInstance.CurrentPage is CDLand currentPage))
                    return;
                bool flag = false;
                if (currentPage.Album.CDDevice != null && currentPage.Album.CDDevice.DrivePath == changedLetter)
                    flag = true;
                if (!flag && (!mediaArrived || !this.NoCD.IsCurrent))
                    return;
                nodes1.Invoke();
            }
            else
            {
                if (!mediaArrived)
                    return;
                foreach (CDAlbumCommand nodes2 in NodesList)
                {
                    if (nodes2.CDDevice != null && nodes2.CDDevice.DrivePath == changedLetter)
                    {
                        this.Nodes.ChosenValue = nodes2;
                        break;
                    }
                }
            }
        }
    }
}
