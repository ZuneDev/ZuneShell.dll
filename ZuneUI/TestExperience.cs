// Decompiled with JetBrains decompiler
// Type: ZuneUI.TestExperience
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;
using System.Collections;

namespace ZuneUI
{
    public class TestExperience : Experience
    {
        private ArrayListDataSet _nodes;
        private Node _stringTester;
        private Node _webHostTester;
        private bool _pivotHasBeenShownBefore;

        public TestExperience(Frame frameOwner)
          : base(frameOwner, StringId.IDS_TEST_PIVOT, SQMDataId.Invalid)
          => this._pivotHasBeenShownBefore = false;

        public override IList NodesList
        {
            get
            {
                if (this._nodes == null)
                {
                    this._nodes = new ArrayListDataSet(this);
                    this._nodes.Add(StringTester);
                    this._nodes.Add(WebHost);
                }
                return _nodes;
            }
        }

        public Node StringTester
        {
            get
            {
                if (this._stringTester == null)
                    this._stringTester = new Node(this, "string tester", "Test\\StringTester\\Home");
                return this._stringTester;
            }
        }

        public Node WebHost
        {
            get
            {
                if (this._webHostTester == null)
                    this._webHostTester = new Node(this, nameof(WebHost), "Test\\WebHost\\Home");
                return this._webHostTester;
            }
        }

        protected override void OnIsCurrentChanged() => this.UpdateShowTest();

        public void UpdateShowTest()
        {
            bool show = this.IsCurrent || this._pivotHasBeenShownBefore;
            ((MainFrame)this.Frame).ShowTest(show);
            if (!show)
                return;
            this._pivotHasBeenShownBefore = true;
        }
    }
}
