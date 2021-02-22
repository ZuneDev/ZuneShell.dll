// Decompiled with JetBrains decompiler
// Type: ZuneUI.MessageNotification
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class MessageNotification : Notification
    {
        private string _message;
        private string _subMessage;

        public MessageNotification(NotificationTask taskType, NotificationState notificationType)
          : this(null, null, taskType, notificationType, 0)
        {
        }

        public MessageNotification(
          NotificationTask taskType,
          NotificationState notificationType,
          int displayTime)
          : this(null, null, taskType, notificationType, displayTime)
        {
        }

        public MessageNotification(
          string message,
          NotificationTask taskType,
          NotificationState notificationType)
          : this(message, null, taskType, notificationType, 0)
        {
        }

        public MessageNotification(
          string message,
          NotificationTask taskType,
          NotificationState notificationType,
          int displayTime)
          : this(message, null, taskType, notificationType, displayTime)
        {
        }

        public MessageNotification(
          string message,
          string subMessage,
          NotificationTask taskType,
          NotificationState notificationType)
          : this(message, subMessage, taskType, notificationType, 0)
        {
        }

        public MessageNotification(
          string message,
          string subMessage,
          NotificationTask taskType,
          NotificationState notificationType,
          int displayTime)
          : base(taskType, notificationType, displayTime)
        {
            this._message = message;
            this._subMessage = subMessage;
        }

        public string Message
        {
            set
            {
                if (!(this._message != value))
                    return;
                this._message = value;
                this.FirePropertyChanged(nameof(Message));
            }
            get => this._message;
        }

        public string SubMessage
        {
            set
            {
                if (!(this._subMessage != value))
                    return;
                this._subMessage = value;
                this.FirePropertyChanged(nameof(SubMessage));
            }
            get => this._subMessage;
        }
    }
}
