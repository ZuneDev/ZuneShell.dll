// Decompiled with JetBrains decompiler
// Type: ZuneUI.QuickplayExperience
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;
using System.Collections;

namespace ZuneUI
{
    public class QuickplayExperience : Experience
    {
        private ArrayListDataSet _nodes;
        private Node _default;

        public QuickplayExperience(Frame frameOwner)
          : base(frameOwner, StringId.IDS_QUICKPLAY_PIVOT, SQMDataId.Invalid)
        {
        }

        public override IList NodesList
        {
            get
            {
                if (this._nodes == null)
                {
                    this._nodes = new ArrayListDataSet(this);
                    this._nodes.Add(Default);
                }
                return _nodes;
            }
        }

        public Node Default
        {
            get
            {
                if (this._default == null)
                    this._default = new Node(this, this.DefaultUIPath, SQMDataId.QuickPlayClicks);
                return this._default;
            }
        }

        public override string DefaultUIPath => "Quickplay\\Default";
    }
}
