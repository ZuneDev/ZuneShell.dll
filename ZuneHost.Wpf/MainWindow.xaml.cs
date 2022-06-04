using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace ZuneHost.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _zuneProgramFolder;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Construct a single string of args. Be sure to skip executing path.
            var args = Environment.GetCommandLineArgs().Skip(1);
#if DEBUG
            args.Concat(new[] { "-debug:true" });
#endif
            string strArgs = string.Join(" ", args.ToArray());

            // Make sure that ZuneDBApi can find all the Zune native libraries
            _zuneProgramFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "Zune");
            foreach (var info in new DirectoryInfo(_zuneProgramFolder).GetFileSystemInfos())
            {
                if (info is DirectoryInfo dirInfo)
                {
                    CopyAll(dirInfo, new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, dirInfo.Name)));
                }
                else if (info is FileInfo fileInfo)
                {
                    string fileName = fileInfo.Name;
                    if (fileInfo.Extension == ".dll")
                    {
                        string targetPath = Path.Combine(Environment.CurrentDirectory, fileName);
                        if (!File.Exists(targetPath) || fileName == "ZuneDbApi.dll")
                            fileInfo.CopyTo(targetPath);
                    }
                }
            }

            IntPtr hWnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            Thread zuneThread = new Thread(new ThreadStart(() =>
                Microsoft.Zune.Shell.ZuneApplication.Launch(strArgs, hWnd)));
            zuneThread.Start();
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy all the files into the new directory
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }


            // Copy all the sub directories using recursion
            foreach (DirectoryInfo diSourceDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetDir = target.CreateSubdirectory(diSourceDir.Name);
                CopyAll(diSourceDir, nextTargetDir);
            }
        }
    }
}
