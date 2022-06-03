using System;
using System.IO;
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

            Console.WriteLine("Starting Zune...");

            Thread zuneThread = new Thread(new ThreadStart(() =>
            {
                Microsoft.Zune.Shell.ZuneApplication.Launch(strArgs, IntPtr.Zero);
            }));
            zuneThread.SetApartmentState(ApartmentState.STA);
            zuneThread.Start();
        }
    }
}