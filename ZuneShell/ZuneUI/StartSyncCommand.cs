// Decompiled with JetBrains decompiler
// Type: ZuneUI.StartSyncCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class StartSyncCommand : SyncCommandBase
    {
        public StartSyncCommand() => this._availableWhenSyncing = false;

        protected override void OnInvoked()
        {
            if (this.Device != null)
                this.Device.BeginSync(true, false);
            base.OnInvoked();
        }
    }
}
