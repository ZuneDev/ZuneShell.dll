// Decompiled with JetBrains decompiler
// Type: ZuneUI.MonitorDetector
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ZuneUI
{
    internal class MonitorDetector
    {
        private List<MonitorSize> _listInProgress;

        public List<MonitorSize> DetectMonitors()
        {
            List<MonitorSize> monitorSizeList = new List<MonitorSize>();
            this._listInProgress = monitorSizeList;
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, new MonitorEnumProc(this.MonitorEnumerated), IntPtr.Zero);
            this._listInProgress = null;
            return monitorSizeList;
        }

        private bool MonitorEnumerated(
          IntPtr hMonitor,
          IntPtr hdcMonitor,
          [In] ref RECT lprcMonitor,
          IntPtr dwData)
        {
            MONITORINFO lpmi = new MONITORINFO();
            lpmi.cbSize = Marshal.SizeOf(lpmi);
            if (GetMonitorInfo(hMonitor, ref lpmi))
            {
                --lpmi.rcMonitor.Right;
                --lpmi.rcMonitor.Bottom;
                --lpmi.rcWorkArea.Right;
                --lpmi.rcWorkArea.Bottom;
                this._listInProgress.Add(new MonitorSize(lpmi.rcMonitor, lpmi.rcWorkArea));
            }
            return true;
        }

        [DllImport("user32.dll")]
        private static extern bool EnumDisplayMonitors(
          IntPtr hdc,
          IntPtr lprcClip,
          MonitorEnumProc lpfnEnum,
          IntPtr dwData);

        [DllImport("user32.dll")]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        private delegate bool MonitorEnumProc(
          IntPtr hMonitor,
          IntPtr hdcMonitor,
          [In] ref RECT lprcMonitor,
          IntPtr dwData);

        private struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWorkArea;
            public int dwFlags;
        }
    }
}
