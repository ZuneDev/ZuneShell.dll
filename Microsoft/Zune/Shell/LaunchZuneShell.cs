// Decompiled with JetBrains decompiler
// Type: Microsoft.Zune.Shell.LaunchZuneShell
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Shell
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("53048eb3-0906-4bd2-890d-48daf1f25413")]
    [ComDefaultInterface(typeof(ILaunchZuneShell))]
    public sealed class LaunchZuneShell : ILaunchZuneShell
    {
        private static LaunchDelegate s_launch;
        private static string s_args;
        private static IntPtr s_hWndSplashScreen;

        [STAThread]
        public IntPtr GetLaunchDelegate(string args, IntPtr hWndSplashScreen)
        {
            s_args = args;
            s_hWndSplashScreen = hWndSplashScreen;
            s_launch = new LaunchDelegate(this.LaunchZuneShellHelper);
            return Marshal.GetFunctionPointerForDelegate(s_launch);
        }

        private int LaunchZuneShellHelper() => ZuneApplication.Launch(s_args, s_hWndSplashScreen);

        public IntPtr GetRenderWindow() => ZuneApplication.GetRenderWindow();

        public void ProcessMessageFromCommandLine(string args) => ZuneApplication.ProcessMessageFromCommandLine(args);

        public void SetDesktopLockState(bool locked) => ZuneApplication.SetDesktopLockState(locked);

        private delegate int LaunchDelegate();
    }
}
