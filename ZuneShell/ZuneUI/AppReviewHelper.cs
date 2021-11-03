// Decompiled with JetBrains decompiler
// Type: ZuneUI.AppReviewHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Service;
using Microsoft.Zune.Util;
using System;

namespace ZuneUI
{
    public class AppReviewHelper : NotifyPropertyChangedImpl
    {
        private static float s_maxRating = 10f;
        private static float s_minRating = 0.0f;
        private HRESULT _lastError;
        private string _lastAuthor;
        private AccountManagement _accountManagement;

        public AppReviewHelper() => this.Reset();

        public event EventHandler ReviewPosted;

        public event EventHandler ReviewPostFailed;

        public static float MaxRating => s_maxRating;

        public static float MinRating => s_minRating;

        public HRESULT LastError
        {
            get => this._lastError;
            private set
            {
                if (!(this._lastError != value))
                    return;
                this._lastError = value;
                this.FirePropertyChanged(nameof(LastError));
            }
        }

        public string LastAuthor
        {
            get => this._lastAuthor;
            private set
            {
                if (!(this._lastAuthor != value))
                    return;
                this._lastAuthor = value;
                this.FirePropertyChanged(nameof(LastAuthor));
            }
        }

        public void AddReview(Guid mediaId, float rating, string title, string text)
        {
            if (mediaId == Guid.Empty || rating < (double)MinRating || rating > (double)MaxRating)
                this.OnReviewPostFailed(HRESULT._ZUNE_E_ADD_REVIEW_FAILED);
            else
                Service.Instance.PostAppReview(mediaId, title, text, (int)rating, new AsyncCompleteHandler(this.OnPostAddReviewComplete));
        }

        private void OnPostAddReviewComplete(HRESULT hr) => Application.DeferredInvoke(new DeferredInvokeHandler(this.HandleAddReviewResponse), hr, DeferredInvokePriority.Normal);

        private void Reset()
        {
            this.LastAuthor = string.Empty;
            this.LastError = HRESULT._S_OK;
        }

        private void HandleAddReviewResponse(object args)
        {
            HRESULT? nullable = (HRESULT?)args;
            if (!nullable.HasValue)
                this.OnReviewPostFailed(HRESULT._ZUNE_E_ADD_REVIEW_FAILED);
            else if (nullable.Value.IsSuccess)
                this.GetAuthorName();
            else
                this.OnReviewPostFailed(nullable.Value);
        }

        private void GetAuthorName()
        {
            if (this._accountManagement == null)
                this._accountManagement = new AccountManagement();
            if (!this._accountManagement.GetAccount(null, new GetAccountCompleteCallback(this.OnGetAccountSuccess), new AccountManagementErrorCallback(this.OnGetAccountError)).IsError)
                return;
            Application.DeferredInvoke(new DeferredInvokeHandler(this.HandleGetAccountComplete), string.Empty, DeferredInvokePriority.Normal);
        }

        private void OnGetAccountSuccess(AccountUser accountUser)
        {
            string str = string.Empty;
            if (accountUser != null)
                str = string.Format(Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_NAME_FORMAT), accountUser.FirstName, accountUser.LastName);
            Application.DeferredInvoke(new DeferredInvokeHandler(this.HandleGetAccountComplete), str, DeferredInvokePriority.Normal);
        }

        private void OnGetAccountError(HRESULT hr, ServiceError serviceError) => Application.DeferredInvoke(new DeferredInvokeHandler(this.HandleGetAccountComplete), string.Empty, DeferredInvokePriority.Normal);

        private void HandleGetAccountComplete(object args)
        {
            this.LastAuthor = args as string;
            this.OnReviewPosted();
        }

        private void OnReviewPosted()
        {
            if (this.ReviewPosted != null)
                this.ReviewPosted(this, null);
            this.FirePropertyChanged("ReviewPosted");
        }

        private void OnReviewPostFailed(HRESULT hr)
        {
            this.LastError = hr;
            if (this.ReviewPostFailed != null)
                this.ReviewPostFailed(this, null);
            this.FirePropertyChanged("ReviewPostFailed");
        }
    }
}
