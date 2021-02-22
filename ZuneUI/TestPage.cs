// Decompiled with JetBrains decompiler
// Type: ZuneUI.TestPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class TestPage : ZunePage
    {
        private int _refCount;
        private bool _saveToBackStack;
        private bool _canBeTrimmed;
        private bool _sharedInstance;

        public TestPage()
        {
            this._saveToBackStack = true;
            this._canBeTrimmed = true;
        }

        public bool SaveToBackStack
        {
            get => this._saveToBackStack;
            set
            {
                if (this._saveToBackStack == value)
                    return;
                this._saveToBackStack = value;
                this.FirePropertyChanged(nameof(SaveToBackStack));
            }
        }

        public bool SharedInstance
        {
            get => this._sharedInstance;
            set
            {
                if (this._sharedInstance == value)
                    return;
                this._sharedInstance = value;
                this.FirePropertyChanged(nameof(SharedInstance));
            }
        }

        public bool CanBeTrimmed
        {
            get => this._canBeTrimmed;
            set
            {
                if (this._canBeTrimmed == value)
                    return;
                this._canBeTrimmed = value;
                this.FirePropertyChanged(nameof(CanBeTrimmed));
            }
        }

        public override IPageState SaveAndRelease()
        {
            IPageState pageState = (IPageState)null;
            if (this._saveToBackStack)
            {
                if (this._sharedInstance)
                {
                    pageState = (IPageState)new InstancePageState((IPage)this);
                }
                else
                {
                    pageState = (IPageState)new TestPageState(this.Description, this.UI, this.BackgroundUI, this.CanBeTrimmed);
                    this.Release();
                }
            }
            else
                this.Release();
            return pageState;
        }

        public override void Release() => --this._refCount;

        protected override void OnNavigatedToWorker()
        {
            if (this.IsValid)
            {
                int num = this._sharedInstance ? 1 : 0;
            }
            ++this._refCount;
            base.OnNavigatedToWorker();
        }

        protected override void OnNavigatedAwayWorker(IPage destination) => base.OnNavigatedAwayWorker(destination);

        protected bool IsValid => this._refCount > 0;
    }
}
