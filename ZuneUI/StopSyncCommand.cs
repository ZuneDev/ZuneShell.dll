// Decompiled with JetBrains decompiler
// Type: ZuneUI.StopSyncCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class StopSyncCommand : SyncCommandBase
    {
        public StopSyncCommand() => this._availableWhenSyncing = true;

        protected override void OnInvoked()
        {
            if (this.Device != null)
                this.Device.EndSync(true);
            base.OnInvoked();
        }
    }
}
