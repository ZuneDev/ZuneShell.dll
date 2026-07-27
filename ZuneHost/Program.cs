using System;
using System.IO;
using System.Linq;
using Microsoft.Iris;
using Microsoft.Zune.Shell;

namespace ZuneHost
{
    internal class Program
    {
        static DirectoryInfo _zuneProgramDir;

        [STAThread]
        static int Main(string[] args)
        {
            string strArgs = string.Join(" ", args);

#if NETFRAMEWORK
            Console.WriteLine("Running legacy build on .NET Framework. If your OS supports .NET Core, please use OpenZune.");
#endif

            if (args.Contains("--copyFiles", StringComparer.InvariantCultureIgnoreCase))
            {
                // Make sure that ZuneDBApi can find all the Zune native libraries
                Console.WriteLine("Copying Zune files...");
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
            }
            
            Application.ErrorReport += ApplicationOnErrorReport;

            Application.Initialized += delegate
            {
                Application.AddResourceRedirect("res://ZuneShellResources!", "clr-res://ZuneShell!RCDATA/");
                Application.AddResourceRedirect("res://ZuneMarketplaceResources!", "clr-res://ZuneShell!RCDATA.Marketplace/");
                UIXControls.Helpers.AddUIXControlsClrRedirect();
            };
            
            Console.WriteLine("Starting Zune...");

            try
            {
                return ZuneApplication.Launch(strArgs, IntPtr.Zero);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.FileName);
                throw;
            }
            catch (Exception ex)
            {
                if (ex is ZuneShellException zException)
                    Console.WriteLine(zException.Context);

                throw;
            }
        }

        private static void ApplicationOnErrorReport(Error[] errors)
        {
            foreach (var error in errors)
                Console.WriteLine(error);
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