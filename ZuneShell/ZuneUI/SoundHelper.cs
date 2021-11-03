// Decompiled with JetBrains decompiler
// Type: ZuneUI.SoundHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Runtime.InteropServices;

namespace ZuneUI
{
    public static class SoundHelper
    {
        private const uint SND_ASYNC = 1;
        private const uint SND_NODEFAULT = 2;
        private const uint SND_RESOURCE = 262148;
        private const uint SND_SYSTEM = 2097152;
        private const uint LOAD_LIBRARY_AS_DATAFILE = 2;
        private static IntPtr s_hModZuneShellResources;

        public static void Play(SoundId soundId)
        {
            if (!((Shell)ZuneShell.DefaultInstance).PlaySounds)
                return;
            string pszSound = null;
            switch (soundId)
            {
                case SoundId.DownloadComplete:
                    pszSound = "Download.wav";
                    break;
                case SoundId.BurnComplete:
                    pszSound = "BurnComplete.wav";
                    break;
                case SoundId.RipComplete:
                    pszSound = "RipComplete.wav";
                    break;
                case SoundId.Inbox:
                    pszSound = "Inbox.wav";
                    break;
            }
            if (pszSound == null)
                return;
            uint fdwSound = 262151;
            if (Environment.OSVersion.Version.Major >= 6)
                fdwSound |= 2097152U;
            if (s_hModZuneShellResources == IntPtr.Zero)
                s_hModZuneShellResources = LoadLibraryEx("ZuneShellResources.dll", IntPtr.Zero, 2U);
            PlaySound(pszSound, s_hModZuneShellResources, fdwSound);
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibraryEx(
          string lpModuleName,
          IntPtr mustBeZero,
          uint dwFlags);

        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool PlaySound(string pszSound, IntPtr hmod, uint fdwSound);
    }
}
