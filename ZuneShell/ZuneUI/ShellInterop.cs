// Decompiled with JetBrains decompiler
// Type: ZuneUI.ShellInterop
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace ZuneUI
{
    internal static class ShellInterop
    {
        public static void OpenFolderAndSelectItem(string path)
        {
            Thread thread = new Thread(args =>
           {
               IntPtr ppidl;
               if (SHParseDisplayName(path, IntPtr.Zero, out ppidl, 0, IntPtr.Zero) != 0)
                   return;
               SHOpenFolderAndSelectItems(ppidl, 0, IntPtr.Zero, 0);
               ILFree(ppidl);
           });
            thread.TrySetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern int SHParseDisplayName(
          string path,
          IntPtr pbc,
          out IntPtr ppidl,
          int sfgaoIn,
          IntPtr psfgaoOut);

        [DllImport("shell32.dll")]
        private static extern int SHOpenFolderAndSelectItems(
          IntPtr pidl,
          int cidl,
          IntPtr apidl,
          int dwFlags);

        [DllImport("shell32.dll")]
        private static extern int ILFree(IntPtr pidl);
    }
}
