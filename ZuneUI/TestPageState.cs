// Decompiled with JetBrains decompiler
// Type: ZuneUI.TestPageState
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneUI
{
    public class TestPageState : ModelItem, IPageState
    {
        private bool _isInvalid;
        private bool _canBeTrimmed;
        private string _mainUI;
        private string _backgroundUI;

        public TestPageState(
          string description,
          string mainUI,
          string backgroundUI,
          bool canBeTrimmed)
        {
            this.Description = description;
            this._mainUI = mainUI;
            this._backgroundUI = backgroundUI;
            this._canBeTrimmed = canBeTrimmed;
        }

        public IPage RestoreAndRelease()
        {
            TestPage testPage = new TestPage();
            testPage.Description = this.Description;
            testPage.UI = this._mainUI;
            testPage.BackgroundUI = this._backgroundUI;
            this.Release();
            return (IPage)testPage;
        }

        public void Release() => this._isInvalid = true;

        public bool CanBeTrimmed
        {
            get => this._canBeTrimmed;
            set => this._canBeTrimmed = value;
        }
    }
}
