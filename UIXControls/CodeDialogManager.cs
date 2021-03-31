// Decompiled with JetBrains decompiler
// Type: UIXControls.CodeDialogManager
// Assembly: UIXControls, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: 78800EA5-2757-404C-BA30-C33FCFC2852A
// Assembly location: C:\Program Files\Zune\UIXControls.dll

using Microsoft.Iris;
using System;
using System.Diagnostics.CodeAnalysis;

namespace UIXControls
{
    public class CodeDialogManager : ModelItem
    {
        private ArrayListDataSet _pendingCodeDialogs;
        private static CodeDialogManager s_instance = new CodeDialogManager();

        private CodeDialogManager() => this._pendingCodeDialogs = new ArrayListDataSet();

        public static CodeDialogManager Instance => CodeDialogManager.s_instance;

        public ArrayListDataSet PendingCodeDialogs => this._pendingCodeDialogs;

        internal void ShowCodeDialog(DialogHelper dialog)
        {
            if (this._pendingCodeDialogs.Contains(dialog))
                return;
            this._pendingCodeDialogs.Add(dialog);
        }

        public event EventHandler WindowCloseRequested;

        public event EventHandler WindowCloseNotBlocked;

        [SuppressMessage("Microsoft.Security", "CA2109", Justification = "FxCop misfire: This method IS referred to in other assemblies.")]
        public void OnWindowCloseRequested(object sender, WindowCloseRequestedEventArgs args)
        {
            args.BlockCloseRequest();
            if (this.WindowCloseRequested != null)
                this.WindowCloseRequested(this, EventArgs.Empty);
            this.FirePropertyChanged("WindowCloseRequested");
        }

        public void WindowCloseWasNotBlocked()
        {
            if (this.WindowCloseNotBlocked != null)
                this.WindowCloseNotBlocked(this, EventArgs.Empty);
            this.FirePropertyChanged("WindowCloseNotBlocked");
        }
    }
}
