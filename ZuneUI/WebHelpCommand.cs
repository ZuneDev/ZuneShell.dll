// Decompiled with JetBrains decompiler
// Type: ZuneUI.WebHelpCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Runtime.InteropServices;

namespace ZuneUI
{
    public class WebHelpCommand : Command
    {
        private const int SW_SHOWNORMAL = 1;
        private string _url;

        public string Url
        {
            get => this._url;
            set
            {
                if (!(this._url != value))
                    return;
                this._url = value;
                this.FirePropertyChanged(nameof(Url));
            }
        }

        protected override void OnInvoked()
        {
            if (this._url != null)
                WebHelpCommand.ShellExecute(IntPtr.Zero, "open", this._url, (string)null, (string)null, 1);
            base.OnInvoked();
        }

        [DllImport("shell32.dll")]
        private static extern IntPtr ShellExecute(
          IntPtr hwnd,
          string lpOperation,
          string lpFile,
          string lpParameters,
          string lpDirectory,
          int nShowCmd);
    }
}
