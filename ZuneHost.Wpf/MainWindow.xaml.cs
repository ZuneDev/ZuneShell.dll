using Microsoft.Iris.Session;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using IrisApp = Microsoft.Iris.Application;

namespace ZuneHost.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DirectoryInfo _zuneProgramDir;
        string decompResultDir = Path.Combine(Environment.CurrentDirectory, "DecompileResults");
        string dataMapDir = Path.Combine(Environment.CurrentDirectory, "DataMappings");

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            AppDomain.CurrentDomain.UnhandledException += DumpErrorsOnEvent;
        }

        private void DumpErrorsOnEvent(object sender, EventArgs e)
        {
            PrintErrors(ErrorManager.GetErrors());
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var debug = false;

            // Construct a single string of args. Be sure to skip executing path.
            var args = Environment.GetCommandLineArgs().Skip(1);

            if (debug)
                args = args.Concat(new[] { $"-uixdebuguri", });

            string strArgs = string.Join(" ", args.ToArray());

            // Make sure that ZuneDBApi can find all the Zune native libraries
            _zuneProgramDir = new(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "Zune"));
            if (_zuneProgramDir.Exists)
            {
                foreach (var info in _zuneProgramDir.GetFileSystemInfos())
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
            }

            IrisApp.DebugSettings.UseDecompiler = false;
            if (IrisApp.DebugSettings.UseDecompiler)
            {
                if (Directory.Exists(decompResultDir))
                    Directory.Delete(decompResultDir, true);
                Directory.CreateDirectory(decompResultDir);
                IrisApp.DebugSettings.DecompileResults.CollectionChanged += DecompileResults_CollectionChanged;
            }

            IrisApp.DebugSettings.GenerateDataMappingModels = false;
            if (IrisApp.DebugSettings.GenerateDataMappingModels)
            {
                if (Directory.Exists(dataMapDir))
                    Directory.Delete(dataMapDir, true);
                Directory.CreateDirectory(dataMapDir);
                IrisApp.DebugSettings.DataMappingModels.CollectionChanged += DataMappingModels_CollectionChanged;
            }

            if (debug)
            {
                // Set decompiler breakpoints
                IrisApp.DebugSettings.Breakpoints.Add(new("clr-res://ZuneShell!QuickplayStrip.uix", 172, 25, false));
                IrisApp.DebugSettings.Breakpoints.Add(new("clr-res://ZuneMarketplaceResources!SelectionActions.uix", 121, 14, false));
                IrisApp.DebugSettings.Breakpoints.Add(new("clr-res://ZuneShell!Quickplay.uix", 917, 62, false));
            }

            ErrorManager.OnErrors += PrintErrors;

            IntPtr hWnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            Thread zuneThread = new(new ThreadStart(() =>
            {
                IrisApp.Initialized += delegate
                {
                    IrisApp.AddImportRedirect("res://ZuneShellResources!", "clr-res://ZuneShell!");
                };

                Microsoft.Zune.Shell.ZuneApplication.Launch(strArgs, hWnd);
            }));
            zuneThread.Start();
        }

        private void DecompileResults_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var result in e.NewItems.Cast<Microsoft.Iris.Debug.Data.DecompilationResult>())
            {
                int count = 0;
                string ctx = Path.GetFileName(result.Context.Substring(result.Context.LastIndexOf('/') + 1));

                FileInfo file = new(Path.Combine(decompResultDir, ctx + ".uix"));
                while (file.Exists)
                    file = new(Path.Combine(decompResultDir, $"{ctx}_{++count}.uix"));

                using var stream = file.OpenWrite();
                result.Doc.Save(stream);
                stream.Flush();
            }
        }

        private void Bridge_InterpreterStep(object sender, Microsoft.Iris.Debug.Data.InterpreterEntry e)
        {
            Debug.WriteLine(e.ToString());
        }

        private void DataMappingModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in e.NewItems.Cast<Microsoft.Iris.Debug.Data.DataMappingModel>())
            {
                FileInfo file = new(Path.Combine(dataMapDir, $"{item.Provider}_{item.Type}.cs"));
                if (file.Exists) file.Delete();

                using var stream = file.Open(FileMode.Create);
                using var writer = new StreamWriter(stream);
                writer.Write(item.GeneratedCode);
                writer.Flush();
            }
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

        private static void PrintErrors(IEnumerable errors)
        {
            foreach (var error in errors.OfType<ErrorRecord>())
            {
                if (error.Warning) continue;

                Debug.Write(error.Warning ? "WARNING: " : "ERROR: ");
                Debug.WriteLine(error.Message);
            }
        }
    }
}
