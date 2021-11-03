// Decompiled with JetBrains decompiler
// Type: ZuneUI.Win32Window
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Runtime.InteropServices;

namespace ZuneUI
{
    public static class Win32Window
    {
        public static void Close(IntPtr hWnd) => PostMessage(hWnd, 16, 0, false);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hWnd, int msg, int wParam, bool lParam);
    }
}
