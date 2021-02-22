// Decompiled with JetBrains decompiler
// Type: ZuneUI.SyncNewRuleAddedNotification
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public class SyncNewRuleAddedNotification : MessageNotification
    {
        private long _oldSize;
        private UIGasGauge _gauge;

        public SyncNewRuleAddedNotification(string message, long startingSize, UIGasGauge gasGauge)
          : base(message, NotificationTask.Sync, NotificationState.OneShot)
        {
            this._gauge = gasGauge;
            this._oldSize = startingSize;
        }

        public long OldSize => Math.Min(Math.Max(this._oldSize, 0L), this.PredictedGauge.TotalSpace);

        public UIGasGauge PredictedGauge => this._gauge;
    }
}
