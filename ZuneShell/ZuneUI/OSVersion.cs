// Decompiled with JetBrains decompiler
// Type: ZuneUI.OSVersion
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public class OSVersion
    {
        public static bool IsXP() => Environment.OSVersion.Version.Major == 5;

        public static bool IsVista() => Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 0;

        public static bool IsWin7()
        {
            if (Environment.OSVersion.Version.Major > 6)
                return true;
            return Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 1;
        }

#if NET6_0_OR_GREATER
        public static bool IsLessThanWin8() => !OperatingSystem.IsWindowsVersionAtLeast(6, 2);

        public static bool IsLessThanWin81() => !OperatingSystem.IsWindowsVersionAtLeast(6, 3);

        public static bool IsLessThanWin10() => !OperatingSystem.IsWindowsVersionAtLeast(10, 0);
#endif
    }
}
