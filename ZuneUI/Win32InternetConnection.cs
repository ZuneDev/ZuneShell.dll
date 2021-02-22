// Decompiled with JetBrains decompiler
// Type: ZuneUI.Win32InternetConnection
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Runtime.InteropServices;

namespace ZuneUI
{
    internal class Win32InternetConnection
    {
        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetGetConnectedState(
          out InternetGetConnectedStateFlags Description,
          int ReservedValue);

        public static bool IsConnected => InternetGetConnectedState(out InternetGetConnectedStateFlags _, 0);

        [Flags]
        private enum InternetGetConnectedStateFlags
        {
            INTERNET_CONNECTION_MODEM = 1,
            INTERNET_CONNECTION_LAN = 2,
            INTERNET_CONNECTION_PROXY = 4,
            INTERNET_CONNECTION_RAS_INSTALLED = 16, // 0x00000010
            INTERNET_CONNECTION_OFFLINE = 32, // 0x00000020
            INTERNET_CONNECTION_CONFIGURED = 64, // 0x00000040
        }
    }
}
