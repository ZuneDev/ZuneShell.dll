using Silk.NET.Windowing;
using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace ZuneHost
{
    internal class Program
    {
        static string? _zuneProgramFolder;

        static void Main(string[] args)
        {
            Console.WriteLine("Creating splash window...");

            string strArgs = string.Join(' ', args);

            // Make sure that ZuneDBApi can find all the Zune native libraries
            Console.WriteLine("Setting up dependency resolver...");
            _zuneProgramFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "Zune");
            foreach (string file in Directory.GetFiles(_zuneProgramFolder))
            {
                string fileName = Path.GetFileName(file);
                if ((fileName.StartsWith("Zune") || fileName.StartsWith("UIX"))
                    && file.EndsWith(".dll")
                    && fileName != "ZuneDbApi.dll")
                {
                    string targetPath = Path.Combine(Environment.CurrentDirectory, fileName);
                    if (!File.Exists(targetPath))
                        File.Copy(file, targetPath);
                }
            }
            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            Console.WriteLine("Starting Zune...");

            Thread zuneThread = new Thread(new ThreadStart(() =>
            {
                Microsoft.Zune.Shell.ZuneApplication.Launch(strArgs, IntPtr.Zero);
            }));
            zuneThread.SetApartmentState(ApartmentState.STA);
            zuneThread.Start();

            //Thread t = new(() =>
            //    Microsoft.Zune.Shell.ZuneApplication.Launch(strArgs, IntPtr.Zero));
            //t.SetApartmentState(ApartmentState.STA);
            //t.Start();
        }

        private static Assembly? CurrentDomain_AssemblyResolve(object? sender, ResolveEventArgs args)
        {
            string fileName = args.Name[..args.Name.IndexOf(",")];

            if (fileName == "ZuneDBApi")
            {
                string assemblyPath = Path.Combine(Directory.GetCurrentDirectory(), "ZuneDBApi.dll");
                bool exists = File.Exists(assemblyPath);
                return Assembly.LoadFrom(assemblyPath);
            }
            else if (!fileName.StartsWith("Zune"))
            {
                return null;
            }
            else
            {
                string assemblyPath = Path.Combine(
                    _zuneProgramFolder ?? throw new ArgumentNullException(),
                    fileName + ".dll");
                return Assembly.Load(assemblyPath); 
            }
        }
    }
}