// Decompiled with JetBrains decompiler
// Type: ZuneUI.SubscriptionEventsListener
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Subscription;
using System;
using System.ComponentModel;

namespace ZuneUI
{
    public class SubscriptionEventsListener : INotifyPropertyChanged
    {
        private static volatile SubscriptionEventsListener m_instance;
        private static MessageNotification _notification;
        private static object myLock = new object();

        private SubscriptionEventsListener()
        {
        }

        public static SubscriptionEventsListener Instance
        {
            get
            {
                if (SubscriptionEventsListener.m_instance == null)
                {
                    lock (SubscriptionEventsListener.myLock)
                    {
                        if (SubscriptionEventsListener.m_instance == null)
                            SubscriptionEventsListener.m_instance = new SubscriptionEventsListener();
                    }
                }
                return SubscriptionEventsListener.m_instance;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void StartListening() => SubscriptionManager.Instance.OnForegroundSubscriptionChanged += new SubscriptionEventHandler(this.OnForegroundSubscriptionChanged);

        public void StopListening() => SubscriptionManager.Instance.OnForegroundSubscriptionChanged -= new SubscriptionEventHandler(this.OnForegroundSubscriptionChanged);

        public event EventHandler SubscriptionChanged;

        private void OnForegroundSubscriptionChanged(SubscriptonEventArguments args) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredOnForegroundSubscriptionChanged), (object)args, DeferredInvokePriority.Normal);

        private void DeferredOnForegroundSubscriptionChanged(object args)
        {
            SubscriptonEventArguments subscriptonEventArguments = (SubscriptonEventArguments)args;
            if (subscriptonEventArguments.Action != SubscriptionAction.Unsubscribed && subscriptonEventArguments.UserInitiated && (subscriptonEventArguments.MediaType == EMediaTypes.eMediaTypePodcastSeries || subscriptonEventArguments.MediaType == EMediaTypes.eMediaTypePlaylist))
            {
                if (SubscriptionEventsListener._notification == null)
                {
                    SubscriptionEventsListener._notification = new MessageNotification(NotificationTask.Podcast, NotificationState.Normal);
                    NotificationArea.Instance.RemoveAll(NotificationTask.Podcast, NotificationState.Completed);
                    NotificationArea.Instance.Add((Notification)SubscriptionEventsListener._notification);
                }
                SubscriptionEventsListener._notification.SubMessage = subscriptonEventArguments.SubscriptionTitle;
                switch (subscriptonEventArguments.Action)
                {
                    case SubscriptionAction.Subscribed:
                        if (EMediaTypes.eMediaTypePodcastSeries == subscriptonEventArguments.MediaType)
                            SubscriptionEventsListener._notification.Message = Shell.LoadString(StringId.IDS_PODCAST_SUBSCRIBED_NOTIFICATION);
                        else if (EMediaTypes.eMediaTypePlaylist == subscriptonEventArguments.MediaType)
                            SubscriptionEventsListener._notification.Message = Shell.LoadString(StringId.IDS_CHANNEL_SUBSCRIBED_NOTIFICATION);
                        SubscriptionEventsListener._notification.Type = NotificationState.OneShot;
                        SubscriptionEventsListener._notification = (MessageNotification)null;
                        break;
                    case SubscriptionAction.RefreshStarted:
                        if (EMediaTypes.eMediaTypePodcastSeries == subscriptonEventArguments.MediaType)
                            SubscriptionEventsListener._notification.Message = Shell.LoadString(StringId.IDS_PODCAST_REFRESH_START_NOTIFICATION);
                        else if (EMediaTypes.eMediaTypePlaylist == subscriptonEventArguments.MediaType)
                            SubscriptionEventsListener._notification.Message = Shell.LoadString(StringId.IDS_CHANNEL_REFRESH_START_NOTIFICATION);
                        SubscriptionEventsListener._notification.Type = NotificationState.Normal;
                        break;
                    case SubscriptionAction.RefreshFinished:
                        if (EMediaTypes.eMediaTypePodcastSeries == subscriptonEventArguments.MediaType)
                            SubscriptionEventsListener._notification.Message = Shell.LoadString(StringId.IDS_PODCAST_REFRESH_END_NOTIFICATION);
                        else if (EMediaTypes.eMediaTypePlaylist == subscriptonEventArguments.MediaType)
                            SubscriptionEventsListener._notification.Message = Shell.LoadString(StringId.IDS_CHANNEL_REFRESH_END_NOTIFICATION);
                        SubscriptionEventsListener._notification.Type = NotificationState.Completed;
                        SubscriptionEventsListener._notification = (MessageNotification)null;
                        break;
                    default:
                        return;
                }
            }
            if (this.PropertyChanged != null)
                this.PropertyChanged((object)this, new PropertyChangedEventArgs("SubscriptionChanged"));
            if (this.SubscriptionChanged == null)
                return;
            this.SubscriptionChanged((object)this, new EventArgs());
        }
    }
}
