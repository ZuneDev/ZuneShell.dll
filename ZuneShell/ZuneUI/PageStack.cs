// Decompiled with JetBrains decompiler
// Type: ZuneUI.PageStack
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;

namespace ZuneUI
{
    public class PageStack : ModelItem
    {
        private IPage _currentPage;
        private ArrayList _pageStack;
        private NavigationDirection _navDirection;
        private uint _maxStackSize;

        public PageStack()
          : this(null)
        {
        }

        public PageStack(IModelItemOwner owner)
          : base(owner)
          => this._pageStack = new ArrayList();

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                if (this._currentPage != null)
                {
                    this._currentPage.OnNavigatedAway(null);
                    this._currentPage.Release();
                    this._currentPage = null;
                }
                if (this._pageStack != null)
                {
                    foreach (IPageState page in this._pageStack)
                        page.Release();
                    this._pageStack = null;
                }
            }
            base.OnDispose(disposing);
        }

        public IPage CurrentPage => this._currentPage;

        public bool CanNavigateBack => this._pageStack.Count > 0;

        public uint MaximumStackSize
        {
            get => this._maxStackSize;
            set
            {
                if ((int)this._maxStackSize == (int)value)
                    return;
                this._maxStackSize = value;
                this.FirePropertyChanged(nameof(MaximumStackSize));
                this.TrimStackSize();
            }
        }

        public NavigationDirection LastNavigationDirection => this._navDirection;

        private void SetLastNavigationDirection(NavigationDirection value)
        {
            if (this._navDirection == value)
                return;
            this._navDirection = value;
            this.FirePropertyChanged("LastNavigationDirection");
        }

        public void NavigateToPage(IPage page)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));
            if (this._currentPage != null)
            {
                this._currentPage.OnNavigatedAway(page);
                this.PushToStack(this._currentPage);
            }
            this.SetCurrentPage(page);
            this.SetLastNavigationDirection(NavigationDirection.Forward);
            this.TrimStackSize();
        }

        public void NavigateBack()
        {
            IPage page1 = null;
            while (this.CanNavigateBack && page1 == null)
            {
                int index = this._pageStack.Count - 1;
                IPageState page2 = (IPageState)this._pageStack[index];
                this._pageStack.RemoveAt(index);
                if (page2 != null)
                    page1 = page2.RestoreAndRelease();
                if (page2 != null)
                    ;
            }
            if (!this.CanNavigateBack)
                this.FirePropertyChanged("CanNavigateBack");
            if (page1 == null)
                return;
            this._currentPage.OnNavigatedAway(page1);
            this._currentPage.Release();
            this.SetCurrentPage(page1);
            this.SetLastNavigationDirection(NavigationDirection.Back);
        }

        public void PushToStack(IPage page)
        {
            IPageState pageState = page != null ? page.SaveAndRelease() : throw new ArgumentNullException(nameof(page));
            if (pageState == null)
                return;
            this._pageStack.Add(pageState);
            if (this._pageStack.Count != 1)
                return;
            this.FirePropertyChanged("CanNavigateBack");
        }

        private void SetCurrentPage(IPage page)
        {
            this._currentPage = page;
            this.FirePropertyChanged("CurrentPage");
            this._currentPage.OnNavigatedTo();
        }

        private void TrimStackSize()
        {
            if (this._maxStackSize == 0U)
                return;
            int num = this._pageStack.Count - (int)this._maxStackSize;
            if (num <= 0)
                return;
            int index = 0;
            while (num > 0 && index < this._pageStack.Count)
            {
                IPageState page = (IPageState)this._pageStack[index];
                if (page.CanBeTrimmed)
                {
                    this._pageStack.RemoveAt(index);
                    page.Release();
                    --num;
                }
                else
                    ++index;
            }
        }
    }
}
