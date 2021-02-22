// Decompiled with JetBrains decompiler
// Type: ZuneUI.SyncOverrideNotification
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class SyncOverrideNotification : MessageNotification
    {
        public SyncOverrideNotification()
          : this((string)null)
        {
        }

        public SyncOverrideNotification(string message)
          : base(message, NotificationTask.Sync, NotificationState.Normal)
        {
        }
    }
}
