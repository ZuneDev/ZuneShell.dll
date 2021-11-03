// Decompiled with JetBrains decompiler
// Type: ZuneUI.SyncCompletedNotification
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class SyncCompletedNotification : MessageNotification
    {
        private int _count;
        private string _pluralMessage;
        private string _deviceName;

        public SyncCompletedNotification(
          string message,
          string pluralMessage,
          int count,
          string deviceName)
          : base(message, NotificationTask.Sync, NotificationState.OneShot)
        {
            this._pluralMessage = pluralMessage;
            this._count = count;
            this._deviceName = deviceName;
        }

        public string PluralMessage => this._pluralMessage;

        public int Count => this._count;

        public string DeviceName => this._deviceName;
    }
}
