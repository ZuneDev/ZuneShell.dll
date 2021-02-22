// Decompiled with JetBrains decompiler
// Type: ZuneUI.ConfirmCloseDialog
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using UIXControls;

namespace ZuneUI
{
    public class ConfirmCloseDialog : DialogHelper
    {
        private EventHandler _handler;

        internal static void Show(string ui, EventHandler handler) => new ConfirmCloseDialog(ui, handler).Show();

        public static void ShowDefault() => ShowDefault("res://ZuneShellResources!ConfirmClose.uix#ConfirmCloseContentUI");

        public static void ShowDefault(string ui) => Show(ui, new EventHandler(ForceCloseHandler));

        private static void ForceCloseHandler(object sender, EventArgs args) => Application.Window.ForceClose();

        private ConfirmCloseDialog(string ui, EventHandler handler)
          : base(ui)
          => this._handler = handler;

        public void Close() => this._handler(this, null);
    }
}
