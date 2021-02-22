// Decompiled with JetBrains decompiler
// Type: ZuneUI.CommentHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Messaging;
using Microsoft.Zune.Service;
using System;
using System.Text;

namespace ZuneUI
{
    public class CommentHelper : NotifyPropertyChangedImpl
    {
        private string _author;
        private string _message;
        private DateTime _updated;
        private Guid _commentId;
        private bool _pendingAddComplete;

        public bool AddingComment
        {
            get => this._pendingAddComplete;
            private set
            {
                if (this._pendingAddComplete == value)
                    return;
                this._pendingAddComplete = value;
                this.FirePropertyChanged(nameof(AddingComment));
            }
        }

        public string CommentMessage
        {
            get => this._message;
            private set
            {
                if (!(this._message != value))
                    return;
                this._message = value;
                this.FirePropertyChanged(nameof(CommentMessage));
            }
        }

        public Guid CommentId
        {
            get => this._commentId;
            private set
            {
                if (!(this._commentId != value))
                    return;
                this._commentId = value;
                this.FirePropertyChanged(nameof(CommentId));
            }
        }

        public string CommentAuthor
        {
            get => this._author;
            private set
            {
                if (!(this._author != value))
                    return;
                this._author = value;
                this.FirePropertyChanged(nameof(CommentAuthor));
            }
        }

        public DateTime CommentUpdated
        {
            get => this._updated;
            private set
            {
                if (!(this._updated != value))
                    return;
                this._updated = value;
                this.FirePropertyChanged(nameof(CommentUpdated));
            }
        }

        public void AddComment(string recipient, string message)
        {
            if (this.AddingComment || string.IsNullOrEmpty(recipient) || (string.IsNullOrEmpty(message) || !SignIn.Instance.SignedIn))
                return;
            this.AddingComment = true;
            this.CommentAuthor = SignIn.Instance.ZuneTag;
            this.CommentMessage = message;
            this.CommentId = Guid.Empty;
            this.CommentUpdated = DateTime.UtcNow;
            StringBuilder stringBuilder = new StringBuilder(Service.GetEndPointUri(EServiceEndpointId.SEID_Comments));
            stringBuilder.Append("/members/");
            stringBuilder.Append(Uri.EscapeDataString(recipient));
            stringBuilder.Append("/comments");
            if (MessagingService.Instance.AddComment(stringBuilder.ToString(), recipient, this._message, new CommentCallback(this.OnAddCommentCompleted)))
                return;
            this.OnAddCommentCompleted(HRESULT._E_FAIL, Guid.Empty);
        }

        public void DeleteComment(string profileTag, Guid commentId)
        {
            if (!(commentId != Guid.Empty) || !SignIn.Instance.SignedIn)
                return;
            StringBuilder stringBuilder = new StringBuilder(Service.GetEndPointUri(EServiceEndpointId.SEID_Comments));
            stringBuilder.Append("/members/");
            stringBuilder.Append(Uri.EscapeDataString(profileTag));
            stringBuilder.Append("/comments/");
            stringBuilder.Append(Uri.EscapeDataString(commentId.ToString()));
            MessagingService.Instance.DeleteComment(stringBuilder.ToString(), profileTag, null);
        }

        private void OnAddCommentCompleted(HRESULT hr, Guid commentId) => Application.DeferredInvoke(new DeferredInvokeHandler(this.OnAddCommentCompletedDeferred), new object[2]
        {
       hr,
       commentId
        });

        private void OnAddCommentCompletedDeferred(object args)
        {
            object[] objArray = (object[])args;
            HRESULT hresult = (HRESULT)objArray[0];
            Guid guid = (Guid)objArray[1];
            if (hresult.IsSuccess)
            {
                this.CommentId = guid;
                this.CommentUpdated = DateTime.UtcNow;
            }
            else
                Shell.ShowErrorDialog(hresult.Int, "Error");
            this.AddingComment = false;
        }
    }
}
