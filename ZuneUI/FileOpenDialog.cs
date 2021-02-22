// Decompiled with JetBrains decompiler
// Type: ZuneUI.FileOpenDialog
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace ZuneUI
{
    public class FileOpenDialog
    {
        private string m_filePath;

        private FileOpenDialog()
        {
        }

        public string FilePath
        {
            get => this.m_filePath;
            private set => this.m_filePath = value;
        }

        public static FileOpenDialog Show(
          string title,
          string initialPath,
          Command doneCommand)
        {
            return FileOpenDialog.Show(title, initialPath, (string[])null, doneCommand);
        }

        public static FileOpenDialog Show(
          string title,
          string initialPath,
          string[] fileFilters,
          Command doneCommand)
        {
            FileOpenDialog dialog = new FileOpenDialog();
            FileOpenDialog.Show(title, initialPath, fileFilters, (DeferredInvokeHandler)(args =>
           {
               dialog.FilePath = (string)args;
               doneCommand?.Invoke();
           }));
            return dialog;
        }

        public static void Show(string title, string initialPath, DeferredInvokeHandler callback) => FileOpenDialog.Show(title, initialPath, (string[])null, callback);

        public static void Show(
          string title,
          string initialPath,
          string[] fileFilters,
          DeferredInvokeHandler callback)
        {
            IntPtr winHandle = Application.Window.Handle;
            Thread thread = new Thread((ParameterizedThreadStart)(args =>
           {
               FileOpenDialog.OpenFileName ofn = new FileOpenDialog.OpenFileName(winHandle);
               try
               {
                   if (!string.IsNullOrEmpty(title))
                       ofn.lpstrTitle = title;
                   if (!string.IsNullOrEmpty(initialPath))
                       ofn.lpstrInitialDir = initialPath;
                   if (fileFilters != null)
                       ofn.lpstrFilter = string.Join("\0", fileFilters) + "\0";
                   string str = (string)null;
                   if (FileOpenDialog.GetOpenFileName(ofn))
                       str = Marshal.PtrToStringUni(ofn.lpstrFile);
                   Application.DeferredInvoke(callback, (object)str);
               }
               finally
               {
                   ofn?.Dispose();
               }
           }));
            thread.TrySetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        public static string MyPicturesPath => Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

        [DllImport("comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetOpenFileName([In, Out] FileOpenDialog.OpenFileName ofn);

        private delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private class OpenFileName : IDisposable
        {
            public int lStructSize;
            public IntPtr hwndOwner;
            public IntPtr hInstance;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpstrFilter;
            public IntPtr lpstrCustomFilter;
            public int nMaxCustFilter;
            public int nFilterIndex;
            public IntPtr lpstrFile;
            public int nMaxFile;
            public IntPtr lpstrFileTitle;
            public int nMaxFileTitle;
            public string lpstrInitialDir;
            public string lpstrTitle;
            public int Flags;
            public short nFileOffset;
            public short nFileExtension;
            public string lpstrDefExt;
            public IntPtr lCustData;
            public FileOpenDialog.WndProc lpfnHook;
            public string lpTemplateName;
            public IntPtr pvReserved;
            public int dwReserved;
            public int FlagsEx;

            public OpenFileName(IntPtr handle)
            {
                IntPtr ptr = Marshal.AllocHGlobal(520);
                Marshal.WriteInt32(ptr, 0);
                this.lpstrCustomFilter = IntPtr.Zero;
                this.nMaxFile = 260;
                this.lpstrFile = ptr;
                this.lpstrFileTitle = IntPtr.Zero;
                this.nMaxFileTitle = 260;
                this.lCustData = IntPtr.Zero;
                this.pvReserved = IntPtr.Zero;
                this.hwndOwner = handle;
                this.lStructSize = Marshal.SizeOf(typeof(FileOpenDialog.OpenFileName));
            }

            public void Dispose() => Marshal.FreeHGlobal(this.lpstrFile);
        }

        private enum OpenFileNameFlags
        {
            OFN_READONLY = 1,
            OFN_OVERWRITEPROMPT = 2,
            OFN_HIDEREADONLY = 4,
            OFN_NOCHANGEDIR = 8,
            OFN_SHOWHELP = 16, // 0x00000010
            OFN_ENABLEHOOK = 32, // 0x00000020
            OFN_ENABLETEMPLATE = 64, // 0x00000040
            OFN_ENABLETEMPLATEHANDLE = 128, // 0x00000080
            OFN_NOVALIDATE = 256, // 0x00000100
            OFN_ALLOWMULTISELECT = 512, // 0x00000200
            OFN_EXTENSIONDIFFERENT = 1024, // 0x00000400
            OFN_PATHMUSTEXIST = 2048, // 0x00000800
            OFN_FILEMUSTEXIST = 4096, // 0x00001000
            OFN_CREATEPROMPT = 8192, // 0x00002000
            OFN_SHAREAWARE = 16384, // 0x00004000
            OFN_NOREADONLYRETURN = 32768, // 0x00008000
            OFN_NOTESTFILECREATE = 65536, // 0x00010000
            OFN_NONETWORKBUTTON = 131072, // 0x00020000
            OFN_NOLONGNAMES = 262144, // 0x00040000
            OFN_EXPLORER = 524288, // 0x00080000
            OFN_NODEREFERENCELINKS = 1048576, // 0x00100000
            OFN_LONGNAMES = 2097152, // 0x00200000
            OFN_ENABLEINCLUDENOTIFY = 4194304, // 0x00400000
            OFN_ENABLESIZING = 8388608, // 0x00800000
            OFN_DONTADDTORECENT = 33554432, // 0x02000000
            OFN_FORCESHOWHIDDEN = 268435456, // 0x10000000
        }
    }
}
