﻿// Decompiled with JetBrains decompiler
// Type: UIXControls.OSInfo
// Assembly: UIXControls, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: 78800EA5-2757-404C-BA30-C33FCFC2852A
// Assembly location: C:\Program Files\Zune\UIXControls.dll

using System.Runtime.InteropServices;

namespace UIXControls
{
    public static class OSInfo
    {
        private const uint SPI_GETKEYBOARDSPEED = 10;
        private const uint SPI_GETKEYBOARDDELAY = 22;
        private static int s_defaultKeyDelay = OSInfo.GetDefaultKeyDelay();
        private static int s_defaultKeyRepeat = OSInfo.GetDefaultKeyRepeat();

        public static bool IsCapsLockOn() => (OSInfo.GetKeyState(20U) & 1) != 0;

        [DllImport("user32.dll")]
        private static extern ushort GetKeyState(uint nVirtKey);

        public static int DefaultKeyDelay => OSInfo.s_defaultKeyDelay;

        public static int DefaultKeyRepeat => OSInfo.s_defaultKeyRepeat;

        private static int GetDefaultKeyDelay()
        {
            int pParam;
            if (!OSInfo.SystemParametersInfo(22U, 0U, out pParam, 0))
                pParam = 1;
            return (pParam + 1) * 250;
        }

        private static int GetDefaultKeyRepeat()
        {
            int pParam;
            if (!OSInfo.SystemParametersInfo(10U, 0U, out pParam, 0))
                pParam = 1;
            return 31000 / (62 + 28 * pParam);
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SystemParametersInfo(
          uint uiAction,
          uint uiParam,
          out int pParam,
          int nWinIni);
    }
}
