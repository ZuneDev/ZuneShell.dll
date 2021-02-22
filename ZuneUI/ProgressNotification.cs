// Decompiled with JetBrains decompiler
// Type: ZuneUI.ProgressNotification
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class ProgressNotification : MessageNotification
    {
        private int _percentage;

        public ProgressNotification(
          string message,
          NotificationTask taskType,
          NotificationState notificationType)
          : base(message, taskType, notificationType)
        {
        }

        public ProgressNotification(
          string message,
          NotificationTask taskType,
          NotificationState notificationType,
          int percentage)
          : base(message, taskType, notificationType)
        {
            this._percentage = percentage;
        }

        public int Percentage
        {
            set
            {
                if (this._percentage == value)
                    return;
                this._percentage = value;
                this.FirePropertyChanged(nameof(Percentage));
            }
            get => this._percentage;
        }
    }
}
