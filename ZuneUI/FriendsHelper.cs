// Decompiled with JetBrains decompiler
// Type: ZuneUI.FriendsHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Subscription;
using System;

namespace ZuneUI
{
    public class FriendsHelper : ModelItem
    {
        private bool m_refreshCompleted;

        public FriendsHelper() => this.StartListening();

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
                this.StopListening();
            base.OnDispose(disposing);
        }

        public bool RefreshCompleted
        {
            get => this.m_refreshCompleted;
            private set
            {
                if (this.m_refreshCompleted == value)
                    return;
                this.m_refreshCompleted = value;
                this.FirePropertyChanged(nameof(RefreshCompleted));
            }
        }

        public void Refresh(Guid userGuid, string feedUri)
        {
            if (userGuid != Guid.Empty && SignIn.Instance.LastSignedInUserGuid == userGuid)
                this.RefreshCompleted = new HRESULT(SubscriptionManager.Instance.Refresh(SignIn.Instance.LastSignedInUserId, EMediaTypes.eMediaTypeUser, false)).IsError;
            else
                this.RefreshCompleted = true;
        }

        private void StartListening() => SubscriptionManager.Instance.OnForegroundSubscriptionChanged += new SubscriptionEventHandler(this.OnForegroundSubscriptionChanged);

        private void StopListening() => SubscriptionManager.Instance.OnForegroundSubscriptionChanged -= new SubscriptionEventHandler(this.OnForegroundSubscriptionChanged);

        private void OnForegroundSubscriptionChanged(SubscriptonEventArguments args) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredOnForegroundSubscriptionChanged), (object)args, DeferredInvokePriority.Normal);

        private void DeferredOnForegroundSubscriptionChanged(object args)
        {
            SubscriptonEventArguments subscriptonEventArguments = (SubscriptonEventArguments)args;
            if (subscriptonEventArguments.Action != SubscriptionAction.RefreshFinished || subscriptonEventArguments.MediaType != EMediaTypes.eMediaTypeUser || subscriptonEventArguments.UserInitiated)
                return;
            this.RefreshCompleted = true;
        }
    }
}
