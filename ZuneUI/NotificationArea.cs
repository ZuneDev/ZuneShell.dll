// Decompiled with JetBrains decompiler
// Type: ZuneUI.NotificationArea
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections.Generic;

namespace ZuneUI
{
    public class NotificationArea : ModelItem
    {
        private bool _paused;
        private List<Notification> _notificationList;
        private Notification _currentNotification;
        private Notification _overrideNotification;
        private Timer _timer;
        private static NotificationArea _singletonInstance;

        private NotificationArea(IModelItemOwner owner)
          : base(owner)
          => this._notificationList = new List<Notification>();

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                if (this._timer != null)
                    this._timer.Tick -= new EventHandler(this.OnTick);
                for (int index = 0; index < this._notificationList.Count; ++index)
                {
                    if (this._notificationList[index] != null)
                        this._notificationList[index].Dispose();
                }
                this._currentNotification = (Notification)null;
                if (this._overrideNotification != null)
                {
                    this._overrideNotification.Dispose();
                    this._overrideNotification = (Notification)null;
                }
            }
            base.OnDispose(disposing);
            NotificationArea._singletonInstance = (NotificationArea)null;
        }

        public static NotificationArea Instance
        {
            get
            {
                if (NotificationArea._singletonInstance == null)
                    NotificationArea._singletonInstance = new NotificationArea((IModelItemOwner)ZuneShell.DefaultInstance);
                return NotificationArea._singletonInstance;
            }
        }

        public Notification CurrentNotification
        {
            private set
            {
                if (this._currentNotification == value)
                    return;
                this._currentNotification = value;
                if (this._currentNotification != null)
                    this.TickTimer.Interval = this._currentNotification.DisplayTime;
                if (this._overrideNotification != null)
                    return;
                this.FirePropertyChanged(nameof(CurrentNotification));
            }
            get => this._overrideNotification != null ? this._overrideNotification : this._currentNotification;
        }

        public bool NotificationsReady => this._notificationList.Count > 0;

        public int NotificationCount => this._notificationList.Count;

        private Timer TickTimer
        {
            get
            {
                if (this._timer == null)
                {
                    this._timer = new Timer((IModelItemOwner)this, "Notification Area Change Timer");
                    this._timer.Interval = 600000;
                    this._timer.AutoRepeat = true;
                    this._timer.Tick += new EventHandler(this.OnTick);
                }
                return this._timer;
            }
        }

        public void Add(Notification notification)
        {
            if (notification == null || this._notificationList.Contains(notification))
                return;
            if (!this.NotificationsReady)
            {
                this._notificationList.Add(notification);
                if (!this.Paused)
                    this.TickTimer.Start();
            }
            else
            {
                int index = this._notificationList.IndexOf(this._currentNotification);
                if (index >= 0 && index < this._notificationList.Count)
                    this._notificationList.Insert(index, notification);
                else
                    this._notificationList.Add(notification);
            }
            this.FirePropertyChanged("NotificationCount");
            if (this.Paused && this.CurrentNotification != null)
                return;
            this.CurrentNotification = notification;
        }

        public void Override(Notification notification)
        {
            if (this._overrideNotification == notification)
                return;
            if (this._overrideNotification != null)
                this._overrideNotification.Dispose();
            this._overrideNotification = notification;
            this.FirePropertyChanged("CurrentNotification");
            if (!this.TickTimer.Enabled)
                return;
            this.TickTimer.Stop();
        }

        public void EndOverride(Notification notification)
        {
            if (notification == null || this._overrideNotification != notification)
                return;
            this.Override((Notification)null);
            if (!this.NotificationsReady)
                return;
            this.TickTimer.Start();
        }

        public void ClearAll()
        {
            this.TickTimer.Stop();
            this._notificationList.Clear();
            this.CurrentNotification = (Notification)null;
        }

        public void RemoveAll(NotificationTask taskType, NotificationState notificationType)
        {
            for (int index = this._notificationList.Count - 1; index >= 0; --index)
            {
                if (this._notificationList[index].Task == taskType && this._notificationList[index].Type == notificationType)
                {
                    if (this._notificationList[index] == this._currentNotification)
                        this.IncrementNotification();
                    this._notificationList[index].Dispose();
                    this._notificationList.RemoveAt(index);
                    if (!this.NotificationsReady)
                    {
                        this.CurrentNotification = (Notification)null;
                        this.TickTimer.Stop();
                    }
                }
            }
        }

        public void Remove(Notification notification)
        {
            if (notification == null || notification.IsDisposed)
                return;
            if (notification == this._currentNotification)
                this.IncrementNotification();
            if (this._notificationList.Remove(notification))
                notification.Dispose();
            if (this.NotificationsReady)
                return;
            this.CurrentNotification = (Notification)null;
            this.TickTimer.Stop();
        }

        public void Replace(Notification oldNotification, Notification newNotification)
        {
            if (oldNotification == null || newNotification == null || (oldNotification.IsDisposed || newNotification.IsDisposed))
                return;
            int index = this._notificationList.IndexOf(oldNotification);
            if (index < 0)
                return;
            this._notificationList.RemoveAt(index);
            this._notificationList.Insert(index, newNotification);
            if (oldNotification == this._currentNotification)
                this.CurrentNotification = newNotification;
            oldNotification.Dispose();
        }

        public void ForceToFront(Notification notification)
        {
            if (this._notificationList.IndexOf(notification) < 0)
                return;
            if (notification == this._currentNotification)
            {
                this.TickTimer.Stop();
                this.TickTimer.Interval = this._currentNotification.DisplayTime;
                this.TickTimer.Start();
            }
            else
                this.CurrentNotification = notification;
        }

        public bool Paused
        {
            get => this._paused;
            set
            {
                if (this._paused == value)
                    return;
                this._paused = value;
                if (this._paused)
                {
                    this.TickTimer.Enabled = false;
                }
                else
                {
                    if (!this.NotificationsReady)
                        return;
                    this.TickTimer.Enabled = true;
                }
            }
        }

        private void OnTick(object sender, EventArgs args)
        {
            if (this._currentNotification == null)
                return;
            Notification currentNotification = this._currentNotification;
            this.IncrementNotification();
            currentNotification.IncrementDisplayCount();
        }

        public void IncrementNotification()
        {
            int index = this._notificationList.IndexOf(this._currentNotification) + 1;
            if (index >= this._notificationList.Count)
                index = 0;
            if (index < 0 || index >= this._notificationList.Count)
                return;
            this.CurrentNotification = this._notificationList[index];
        }
    }
}
