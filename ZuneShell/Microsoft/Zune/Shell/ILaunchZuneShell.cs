// Decompiled with JetBrains decompiler
// Type: Microsoft.Zune.Shell.ILaunchZuneShell
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Shell
{
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("79493c51-2a79-410c-ab77-da1ead887f11")]
    [ComImport]
    internal interface ILaunchZuneShell
    {
        IntPtr GetLaunchDelegate(string args, IntPtr hWndSplashScreen);

        IntPtr GetRenderWindow();

        void ProcessMessageFromCommandLine(string cmdLine);

        void SetDesktopLockState(bool locked);
    }
}
