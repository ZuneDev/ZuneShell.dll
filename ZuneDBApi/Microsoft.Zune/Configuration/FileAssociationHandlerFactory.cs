using System.Runtime.InteropServices;

namespace Microsoft.Zune.Configuration
{
    public class FileAssociationHandlerFactory
    {
        public static IFileAssociationHandler CreateFileAssociationHandler()
        {
#if WINDOWS
            return new WindowsFileAssociationHandler();
#else
            if (false && RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return new XdgFileAssociationHandler();

            // TODO: macOS (Launch Services) not implemented yet; see NullFileAssociationHandler.
            return new NullFileAssociationHandler();
#endif
        }
    }
}