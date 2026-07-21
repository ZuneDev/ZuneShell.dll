using Microsoft.Win32;

namespace ZuneDBApi.Abstractions
{
    /// <summary>
    /// Selects the <see cref="IRegistryProvider"/> implementation for the
    /// current platform.
    /// </summary>
    internal static class RegistryProviderFactory
    {
        // TODO: the original native ZuneDBApi.dll's actual root registry path
        // is not yet recovered (needs Ghidra inspection of the string constants
        // it passes to the Win32 registry APIs). "Software\Microsoft\Zune" is
        // the fewest-predicates assumption per CLAUDE.md (standard
        // HKCU/HKLM\Software\Microsoft\<Product> convention) — logged in
        // logs/Microsoft.Zune/Configuration/RegistryAbstraction.md.
        private const string RootKeyPath = "Software\\Microsoft\\Zune";

        public static IRegistryProvider Create(RegistryHive hive, string subKeyPath)
        {
#if WINDOWS
            return new Win32RegistryProvider(hive, RootKeyPath + "\\" + subKeyPath);
#else
            return new InMemoryRegistryProvider();
#endif
        }
    }
}
