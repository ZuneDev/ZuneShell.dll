// Decompiled with JetBrains decompiler
// Type: ZuneUI.DialogPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public abstract class DialogPage : ZunePage
    {
        public abstract bool IsWizard { get; }

        public abstract bool AllowAdvance { get; set; }

        public abstract bool AllowCancel { get; set; }

        public abstract bool RequireSecurityIcon { get; set; }

        public abstract void Save();

        public abstract void Exit();

        public abstract void SaveAndExit();

        public abstract void CancelAndExit();

        public abstract void NavigatePage(bool forward);

        public abstract bool NavigationAvailable(bool forward);
    }
}
