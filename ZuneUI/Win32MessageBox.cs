// Decompiled with JetBrains decompiler
// Type: ZuneUI.Win32MessageBox
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace ZuneUI
{
    public static class Win32MessageBox
    {
        public static void Show(
          string text,
          string caption,
          Win32MessageBoxType type,
          DeferredInvokeHandler callback)
        {
            IntPtr winHandle = Application.Window.Handle;
            new Thread((ParameterizedThreadStart)(args =>
           {
               int num = Win32MessageBox.MessageBox(winHandle, text, caption, type);
               if (callback == null)
                   return;
               Application.DeferredInvoke(callback, (object)num);
           })).Start();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int MessageBox(
          IntPtr hWnd,
          string text,
          string caption,
          Win32MessageBoxType type);
    }
}
