// Decompiled with JetBrains decompiler
// Type: ZuneUI.OSInfo
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Runtime.InteropServices;

namespace ZuneUI
{
    public static class OSInfo
    {
        private const uint SPI_GETKEYBOARDSPEED = 10;
        private const uint SPI_GETKEYBOARDDELAY = 22;
        private static int s_defaultKeyDelay = GetDefaultKeyDelay();
        private static int s_defaultKeyRepeat = GetDefaultKeyRepeat();

        public static int DefaultKeyDelay => s_defaultKeyDelay;

        public static int DefaultKeyRepeat => s_defaultKeyRepeat;

        private static int GetDefaultKeyDelay()
        {
            int pParam;
            if (!SystemParametersInfo(22U, 0U, out pParam, 0))
                pParam = 1;
            return (pParam + 1) * 250;
        }

        private static int GetDefaultKeyRepeat()
        {
            int pParam;
            if (!SystemParametersInfo(10U, 0U, out pParam, 0))
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
