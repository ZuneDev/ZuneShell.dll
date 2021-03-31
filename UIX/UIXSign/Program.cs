using dnlib.DotNet;
using System.IO;

namespace UIXSign
{
    class Program
    {
        static void Main(string[] args)
        {
            string dll = args[0];
            string outputDir = Path.GetDirectoryName(dll);
            string dllCopy = Path.Combine(outputDir, "UIX_signed.dll");
            File.Copy(dll, dllCopy, true);

            var module = ModuleDefMD.Load(dllCopy);
            var asm = module.Assembly;
            asm.PublicKey = new PublicKey("00240000048000009400000006020000002400005253413100040000010001001dc70401884cdfad2010ce192e1f08a30fb034cf504759943eec3359d4ed09af3ce1616dbb124e479617ec73e4162903766e7a5e7bf1984bb318040118fe0f69dfb8b6e5c7c47a0e1bc9a8984b22f7221cc235986c09c74cab38ea3562c18adb8e3a95b73faf1ed71d7c309058b86d951af2165eb215b47de335e360a6a25da7");
            asm.Write(dll);

            module.Dispose();
            File.Delete(dllCopy);
        }
    }
}
