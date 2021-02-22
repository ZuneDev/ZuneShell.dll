// Decompiled with JetBrains decompiler
// Type: ZuneUI.Wizard
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace ZuneUI
{
    public abstract class Wizard : ModelItem
    {
        private List<WizardPage> _pages;
        private int _currentPageIndex;
        private EventHandler _cancelCommandHandler;
        private HRESULT _error;
        private bool _errorPageIsEnabled;
        protected BreadcrumbFactory _breadcrumbFactory;
        private bool _finished;
        private IDictionary<ProxySettingDelegate, object> _settings;

        public event WizardStateChangeHandler StateChanged;

        protected Wizard()
        {
            this._settings = (IDictionary<ProxySettingDelegate, object>)null;
            this._pages = new List<WizardPage>();
            this._currentPageIndex = -1;
            this._error = HRESULT._S_OK;
            this._finished = false;
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                foreach (ModelItem page in this._pages)
                    page.Dispose();
                if (this._settings != null)
                {
                    foreach (object obj in (IEnumerable<object>)this._settings.Values)
                    {
                        if (obj is IDisposable)
                            ((IDisposable)obj).Dispose();
                    }
                }
            }
            base.OnDispose(disposing);
        }

        protected void AddPage(WizardPage page)
        {
            this._pages.Add(page);
            if (this._currentPageIndex == -1 && page.IsEnabled)
            {
                this._currentPageIndex = this._pages.Count - 1;
                this.CurrentPage.Activate();
            }
            this.FirePropertyChanged("Pages");
        }

        protected void AddPage(WizardPage page, string breadcrumbTitle)
        {
            if (!string.IsNullOrEmpty(breadcrumbTitle) && page.IsEnabled)
            {
                page.BreadcrumbTitle = breadcrumbTitle;
                if (this._breadcrumbFactory == null)
                    this._breadcrumbFactory = new BreadcrumbFactory();
                this._breadcrumbFactory.AddCrumb(new Breadcrumb(page));
            }
            this.AddPage(page);
        }

        public IList Breadcrumbs => this._breadcrumbFactory == null ? (IList)null : this._breadcrumbFactory.Breadcrumbs;

        public void MarkBreadcrumbsComplete()
        {
            if (this.Breadcrumbs == null)
                return;
            foreach (Breadcrumb breadcrumb in (IEnumerable)this.Breadcrumbs)
                breadcrumb.Complete = true;
        }

        public virtual bool IsValid
        {
            get
            {
                foreach (WizardPage page in this._pages)
                {
                    if (page.IsEnabled && !page.IsValid)
                        return false;
                }
                return true;
            }
        }

        public bool ShowNavigation => this.CurrentPage != null && this.CurrentPage.ShowNavigation;

        public bool ShowClose => this.CurrentPage != null && this.CurrentPage.ShowClose;

        public bool CanAdvancePageIndex => this.GetNextEnabledPageIndex() != -1;

        public virtual void NotifyStateChanged()
        {
            if (this.StateChanged != null)
                this.StateChanged();
            this.FirePropertyChanged("CanAdvancePageIndex");
        }

        public virtual bool CanMoveNext => this.CurrentPage.IsValid && this.CanAdvancePageIndex;

        public virtual bool CanStart => true;

        public void Start()
        {
            if (!this.CanStart || !this.OnStart() || this.CurrentPageIndex >= 0)
                return;
            this.CurrentPageIndex = this.GetNextEnabledPageIndex(-1);
        }

        protected virtual bool OnStart() => true;

        public virtual bool MoveNext()
        {
            bool flag = false;
            WizardPage currentPage1 = this.CurrentPage;
            if (!this.CanMoveNext)
                throw new ApplicationException("cannot move next in this state");
            if (this.CurrentPage == null || this.CurrentPage.OnMovingNext())
            {
                this.CurrentPageIndex = this.GetNextEnabledPageIndex();
                flag = true;
            }
            WizardPage currentPage2 = this.CurrentPage;
            if (this._breadcrumbFactory != null)
                this._breadcrumbFactory.UpdateState(currentPage1, currentPage2, true);
            return flag;
        }

        public IList Pages => (IList)this._pages.AsReadOnly();

        public HRESULT Error
        {
            get => this._error;
            private set
            {
                if (!(this._error != value))
                    return;
                this._error = value;
                this.FirePropertyChanged(nameof(Error));
            }
        }

        public void ShowErrorPage(int hr) => this.ShowErrorPage(new HRESULT(hr));

        public void ShowErrorPage(HRESULT hr)
        {
            this.SetError(hr, (object)null);
            this.ErrorPageIsEnabled = true;
            this.MoveNext();
        }

        public bool ErrorPageIsEnabled
        {
            get => this._errorPageIsEnabled;
            protected set
            {
                if (this._errorPageIsEnabled == value)
                    return;
                this._errorPageIsEnabled = value;
                this.FirePropertyChanged(nameof(ErrorPageIsEnabled));
            }
        }

        private int GetNextEnabledPageIndex() => this.GetNextEnabledPageIndex(this.CurrentPageIndex);

        private int GetNextEnabledPageIndex(int startIndex)
        {
            int index = startIndex;
            while (++index < this._pages.Count)
            {
                if (this._pages[index].IsEnabled)
                    return index;
            }
            return -1;
        }

        public virtual bool CanMoveBack => this.GetPrevEnabledPageIndex() != -1;

        public virtual bool MoveBack()
        {
            bool flag = false;
            WizardPage currentPage1 = this.CurrentPage;
            if (!this.CanMoveBack)
                throw new ApplicationException("cannot move back in this state");
            if (this.CurrentPage == null || this.CurrentPage.OnMovingBack())
            {
                this.CurrentPageIndex = this.GetPrevEnabledPageIndex();
                flag = true;
            }
            WizardPage currentPage2 = this.CurrentPage;
            if (this._breadcrumbFactory != null)
                this._breadcrumbFactory.UpdateState(currentPage1, currentPage2, false);
            return flag;
        }

        private int GetPrevEnabledPageIndex()
        {
            int currentPageIndex = this.CurrentPageIndex;
            while (--currentPageIndex >= 0)
            {
                if (this._pages[currentPageIndex].IsEnabled)
                    return currentPageIndex;
            }
            return -1;
        }

        public virtual bool CanCommitChanges => this.IsValid && !this.CanAdvancePageIndex;

        public bool CommitChanges()
        {
            if (!this.CanCommitChanges)
                return false;
            foreach (WizardPage page in this._pages)
            {
                if (!page.CommitChanges())
                {
                    this.CurrentPageIndex = this._pages.IndexOf(page);
                    return false;
                }
            }
            if (!this.OnCommitChanges())
                return false;
            if (this._settings != null)
            {
                foreach (ProxySettingDelegate key in (IEnumerable<ProxySettingDelegate>)this._settings.Keys)
                    key(this._settings[key]);
                this.ClearSettings();
            }
            return true;
        }

        public bool AsyncCommitChanges() => this.CanCommitChanges && ThreadPool.QueueUserWorkItem(new WaitCallback(this.AsyncCommitChangesPrivate), (object)this._currentPageIndex);

        private void AsyncCommitChangesPrivate(object state)
        {
            int currentPageIndex = this._currentPageIndex;
            int num = -1;
            foreach (WizardPage page in this._pages)
            {
                if (!page.CommitChanges())
                {
                    num = this._pages.IndexOf(page);
                    break;
                }
            }
            if (!this.OnCommitChanges())
                num = currentPageIndex;
            Application.DeferredInvoke(new DeferredInvokeHandler(this.OnAsyncCommitCompletedPrivate), (object)num);
        }

        private void OnAsyncCommitCompletedPrivate(object args)
        {
            int num = (int)args;
            bool success = num < 0;
            if (success)
            {
                if (this._settings != null)
                {
                    foreach (ProxySettingDelegate key in (IEnumerable<ProxySettingDelegate>)this._settings.Keys)
                        key(this._settings[key]);
                    this.ClearSettings();
                }
            }
            else if (this.CurrentPageIndex == num)
                this.CurrentPage.Activate();
            else
                this.CurrentPageIndex = num;
            this.OnAsyncCommitCompleted(success);
        }

        protected virtual void OnAsyncCommitCompleted(bool success)
        {
        }

        protected virtual bool OnCommitChanges() => true;

        protected virtual void OnSetError(HRESULT hr, object state)
        {
        }

        public virtual bool CanCancel => !this.HasPages || this.CurrentPage == null || this.CurrentPage.CanCancel;

        public EventHandler CancelCommandHandler
        {
            get
            {
                if (this._cancelCommandHandler == null)
                    this._cancelCommandHandler = new EventHandler(this.CancelInvokedHandler);
                return this._cancelCommandHandler;
            }
        }

        public virtual void Cancel()
        {
            if (!this.CanCancel)
                throw new ApplicationException("cannot cancel in this state");
            this.CancelSettings();
        }

        public bool HasPages => this._pages.Count > 0;

        public WizardPage CurrentPage => this._pages.Count != 0 && this._currentPageIndex >= 0 ? this._pages[this._currentPageIndex] : (WizardPage)null;

        public int CurrentPageIndex
        {
            get => this._currentPageIndex;
            set
            {
                if (this._currentPageIndex == value)
                    return;
                if (this.CurrentPage != null)
                    this.CurrentPage.Deactivate();
                this._currentPageIndex = value;
                if (this._currentPageIndex == -1)
                    return;
                this.CurrentPage.Activate();
                this.FirePropertyChanged(nameof(CurrentPageIndex));
                this.FirePropertyChanged("CanAdvancePageIndex");
                this.FirePropertyChanged("CurrentPage");
                this.FirePropertyChanged("CanMoveBack");
                this.FirePropertyChanged("CanMoveNext");
                this.FirePropertyChanged("CanCommitChanges");
            }
        }

        public bool HasEntries => this._settings != null && this._settings.Count > 0;

        public virtual void RecordSetting(ProxySettingDelegate proxy, object data)
        {
            if (this._settings == null)
                this._settings = (IDictionary<ProxySettingDelegate, object>)new Dictionary<ProxySettingDelegate, object>();
            this._settings[proxy] = data;
        }

        public virtual void CancelSettings() => this.ClearSettings();

        public void ResetError() => this.SetError(HRESULT._S_OK, (object)null);

        public void SetError(HRESULT hr, object state)
        {
            if (Application.IsApplicationThread)
            {
                this.Error = hr;
                this.OnSetError(hr, state);
            }
            else
                Application.DeferredInvoke(new DeferredInvokeHandler(this.AsyncSetError), (object)new object[2]
                {
          (object) hr,
          state
                });
        }

        private void AsyncSetError(object args)
        {
            object[] objArray = (object[])args;
            this.SetError((HRESULT)objArray[0], objArray[1]);
        }

        private void ClearSettings()
        {
            if (this._settings == null)
                return;
            this._settings.Clear();
            this._settings = (IDictionary<ProxySettingDelegate, object>)null;
        }

        private void CancelInvokedHandler(object sender, EventArgs args) => this.Cancel();

        public bool Finished
        {
            get => this._finished;
            set
            {
                if (this._finished == value)
                    return;
                this._finished = value;
                this.FirePropertyChanged(nameof(Finished));
            }
        }
    }
}
