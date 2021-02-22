// Decompiled with JetBrains decompiler
// Type: ZuneUI.Notification
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneUI
{
    public class Notification : ModelItem
    {
        private NotificationTask _task;
        private NotificationState _type;
        private uint _appearances;
        private int _displayTime;

        public Notification(NotificationTask taskType, NotificationState notificationType)
          : this(taskType, notificationType, 0)
        {
        }

        public Notification(
          NotificationTask taskType,
          NotificationState notificationType,
          int displayTime)
          : base(null)
        {
            this._task = taskType;
            this._type = notificationType;
            this._appearances = 0U;
            this._displayTime = displayTime <= 0 ? 5000 : displayTime;
        }

        public NotificationTask Task
        {
            get => this._task;
            set
            {
                if (this._task == value)
                    return;
                this._task = value;
                this.FirePropertyChanged(nameof(Task));
            }
        }

        public NotificationState Type
        {
            get => this._type;
            set
            {
                if (this._type == value)
                    return;
                this._type = value;
                this.FirePropertyChanged(nameof(Type));
            }
        }

        public uint Appearances => this._appearances;

        public int DisplayTime => this._displayTime;

        public void IncrementDisplayCount()
        {
            if (this._type == NotificationState.OneShot)
                NotificationArea.Instance.Remove(this);
            if (this._type != NotificationState.Completed)
                return;
            ++this._appearances;
            if (this._appearances < 5U)
                return;
            NotificationArea.Instance.Remove(this);
        }
    }
}
