// Decompiled with JetBrains decompiler
// Type: ZuneUI.FolderBrowseDialog
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ZuneUI
{
    public static class FolderBrowseDialog
    {
        private const int BIF_RETURNONLYFSDIRS = 1;
        private const int BIF_EDITBOX = 16;
        private const int BIF_NEWDIALOGSTYLE = 64;
        private const int BIF_VALIDATE = 32;
        private const int BFFM_INITIALIZED = 1;
        private const int BFFM_SELCHANGED = 2;
        private const int BFFM_VALIDATEFAILED = 4;
        private const int BFFM_ENABLEOK = 1125;
        private const int MAX_PATH = 260;
        private static readonly IntPtr NoValidation = IntPtr.Zero;
        private static readonly IntPtr ValidateWritable = new IntPtr(1);

        public static void Show(string title, DeferredInvokeHandler callback) => Show(title, callback, false);

        public static void Show(string title, DeferredInvokeHandler callback, bool validate)
        {
            IntPtr winHandle = Application.Window.Handle;
            Thread thread = new Thread(args =>
           {
               BROWSEINFO bi = new BROWSEINFO();
               bi.hwndOwner = winHandle;
               bi.pidlRoot = IntPtr.Zero;
               bi.pszDisplayName = new string(' ', 261);
               if (!string.IsNullOrEmpty(title))
                   bi.pszTitle = title;
               bi.ulFlags = 112U;
               bi.lpfn = new BFFCALLBACK(Validate);
               bi.lParam = validate ? ValidateWritable : NoValidation;
               bi.iImage = 0;
               IntPtr num = SHBrowseForFolder(ref bi);
               string str = null;
               if (num != IntPtr.Zero)
               {
                   StringBuilder Path = new StringBuilder(261);
                   if (SHGetPathFromIDList(num, Path) != 0)
                       str = Path.ToString();
               }
               Marshal.FreeCoTaskMem(num);
               Application.DeferredInvoke(callback, str);
           });
            thread.TrySetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private static int Validate(IntPtr hwnd, uint uMsg, IntPtr lParam, IntPtr lpData)
        {
            switch (uMsg)
            {
                case 2:
                    if (lpData == ValidateWritable)
                    {
                        bool lParam1 = false;
                        StringBuilder Path = new StringBuilder(260);
                        if (SHGetPathFromIDList(lParam, Path) != 0)
                            lParam1 = CanWriteToFolder(Path.ToString());
                        SendMessage(hwnd, 1125, 0, lParam1);
                        break;
                    }
                    break;
                case 4:
                    return 1;
            }
            return 0;
        }

        public static bool CanWriteToFolder(string folder)
        {
            try
            {
                string pathRoot = Path.GetPathRoot(folder);
                if (pathRoot.Length == 1 || pathRoot.Length >= 2 && pathRoot[1] == Path.VolumeSeparatorChar)
                {
                    DriveInfo driveInfo = new DriveInfo(pathRoot);
                    if (driveInfo.DriveType == DriveType.CDRom || driveInfo.DriveType == DriveType.Removable)
                        return false;
                }
                StringBuilder tmpFileName = new StringBuilder(260);
                if (GetTempFileName(folder, "tmp", 0U, tmpFileName) == 0U)
                    return false;
                using (FileStream fileStream = File.Create(tmpFileName.ToString(), 2, FileOptions.DeleteOnClose))
                    fileStream.WriteByte(65);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SHBrowseForFolder(ref BROWSEINFO bi);

        [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
        private static extern int SHGetPathFromIDList(IntPtr pidl, [Out] StringBuilder Path);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, bool lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        internal static extern uint GetTempFileName(
          string tmpPath,
          string prefix,
          uint uniqueIdOrZero,
          StringBuilder tmpFileName);

        private delegate int BFFCALLBACK(IntPtr hwnd, uint uMsg, IntPtr lParam, IntPtr lpData);

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        private struct BROWSEINFO
        {
            public IntPtr hwndOwner;
            public IntPtr pidlRoot;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszDisplayName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszTitle;
            public uint ulFlags;
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public BFFCALLBACK lpfn;
            public IntPtr lParam;
            public int iImage;
        }
    }
}
