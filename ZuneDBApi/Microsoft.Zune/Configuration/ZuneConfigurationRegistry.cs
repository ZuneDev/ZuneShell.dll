using Microsoft.Iris.Data.Registry;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
    /// <summary>
    /// Opens subkeys of the Zune configuration root — <c>Software\Microsoft\Zune</c> —
    /// for consumers that store their settings under the shared Zune registry
    /// tree (app settings, shell settings, radio stations, etc). This is the
    /// Zune-specific convention layered on top of the generic, product-agnostic
    /// <see cref="RegistryProviderFactory"/>.
    /// </summary>
    public static class ZuneConfigurationRegistry
    {
        // TODO: the original native ZuneDBApi.dll's actual root registry path
        // is not yet recovered (needs Ghidra inspection of the string constants
        // it passes to the Win32 registry APIs). "Software\Microsoft\Zune" is
        // the fewest-predicates assumption per CLAUDE.md (standard
        // HKCU/HKLM\Software\Microsoft\<Product> convention) — logged in
        // logs/Microsoft.Zune/Configuration/RegistryAbstraction.md. Corroborated
        // by ZuneShell/ZuneUI/Shell.cs's decompiled SettingsRegistryPath
        // ("HKEY_CURRENT_USER\Software\Microsoft\Zune\Shell"), which is exactly
        // this root plus a subkey.
        private const string ZuneRootKeyPath = "Software\\Microsoft\\Zune";

        /// <summary>
        /// Opens (creating it if missing) a subkey of the Zune configuration
        /// root — <c>Software\Microsoft\Zune\&lt;subKeyPath&gt;</c> — for
        /// read/write access. Used by app-settings consumers such as
        /// <see cref="CConfigurationManagedBase"/>.
        /// </summary>
        public static IRegistryProvider Open(RegistryHive hive, string subKeyPath) =>
            RegistryProviderFactory.Open(hive, CombineZuneRoot(subKeyPath));

        private static string CombineZuneRoot(string subKeyPath) =>
            string.IsNullOrEmpty(subKeyPath) ? ZuneRootKeyPath : ZuneRootKeyPath + "\\" + subKeyPath;
    }
}
