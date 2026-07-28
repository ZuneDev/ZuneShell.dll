#if WINDOWS
using System;
using System.Collections.Generic;
using Microsoft.Iris.Data.Registry;
using ZuneUI;

namespace Microsoft.Zune.Configuration
{
    /// <summary>
    /// Registers/queries per-user file type associations via the classic
    /// per-user ProgId mechanism (<c>HKCU\Software\Classes\&lt;ext&gt;</c> ->
    /// <c>HKCU\Software\Classes\&lt;ProgId&gt;\shell\open\command</c>), built on
    /// top of the shared <see cref="IRegistryProvider"/> abstraction so it
    /// shares its persistence layer with the rest of ZuneDBApi's Configuration
    /// classes.
    /// </summary>
    /// <remarks>
    /// This deliberately does not attempt to write the Vista+
    /// <c>...\Explorer\FileExts\&lt;ext&gt;\UserChoice</c> key: since Windows 8,
    /// that key is protected by an undocumented per-write hash Explorer
    /// verifies, so programmatic writes to it are silently ignored by the
    /// shell. Writing the ProgId's <c>Software\Classes</c> entry is still
    /// useful — it wins whenever the user hasn't made an explicit UserChoice
    /// for that extension, and remains the only mechanism a non-elevated
    /// process can actually affect.
    /// </remarks>
    internal sealed class WindowsFileAssociationHandler : IFileAssociationHandler
    {
        private const string ClassesRoot = @"Software\Classes";

        public bool CanAssociationBeChanged()
        {
            // Functional probe rather than an OS-version guess: whether we can
            // actually write HKCU\Software\Classes is what actually determines
            // whether SetFileAssociationInfo can do anything.
            using IRegistryProvider? key = RegistryProviderFactory.TryOpen(RegistryHive.CurrentUser, ClassesRoot, writable: true);
            return key != null;
        }

        public int GetFileAssociationInfoList(out IList<FileAssociationInfo> fileAssociationInfoList)
        {
            var list = new List<FileAssociationInfo>(ZuneFileAssociations.All.Count);
            try
            {
                foreach (ZuneFileAssociations.Entry entry in ZuneFileAssociations.All)
                {
                    using IRegistryProvider? extKey = RegistryProviderFactory.TryOpen(RegistryHive.CurrentUser, $@"{ClassesRoot}\{entry.Extension}");
                    string? currentProgId = extKey?.GetStringValue(string.Empty, null);
                    bool isOwned = string.Equals(currentProgId, entry.ProgId, StringComparison.OrdinalIgnoreCase);
                    list.Add(new FileAssociationInfo(entry.Extension, entry.ProgId, entry.Description, entry.MediaType, isOwned));
                }
            }
            catch (UnauthorizedAccessException)
            {
                fileAssociationInfoList = list;
                return HRESULT._E_ACCESSDENIED;
            }

            fileAssociationInfoList = list;
            return HRESULT._S_OK;
        }

        public int SetFileAssociationInfo(IList<FileAssociationInfo> fileAssociationInfoList)
        {
            string? exePath = Environment.ProcessPath;
            try
            {
                foreach (FileAssociationInfo info in fileAssociationInfoList)
                {
                    ZuneFileAssociations.Entry? entry = ZuneFileAssociations.Find(info.Extension);
                    if (entry == null)
                        continue;

                    if (info.IsCurrentlyOwned)
                    {
                        if (exePath == null)
                            continue;
                        RegisterProgId(entry, exePath);
                        using IRegistryProvider extKey = RegistryProviderFactory.Open(RegistryHive.CurrentUser, $@"{ClassesRoot}\{entry.Extension}");
                        extKey.SetStringValue(string.Empty, entry.ProgId);
                    }
                    else
                    {
                        using IRegistryProvider? extKey = RegistryProviderFactory.TryOpen(RegistryHive.CurrentUser, $@"{ClassesRoot}\{entry.Extension}", writable: true);
                        string? currentProgId = extKey?.GetStringValue(string.Empty, null);
                        if (extKey != null && string.Equals(currentProgId, entry.ProgId, StringComparison.OrdinalIgnoreCase))
                            extKey.SetStringValue(string.Empty, string.Empty);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                return HRESULT._E_ACCESSDENIED;
            }

            return HRESULT._S_OK;
        }

        private static void RegisterProgId(ZuneFileAssociations.Entry entry, string exePath)
        {
            using IRegistryProvider progIdKey = RegistryProviderFactory.Open(RegistryHive.CurrentUser, $@"{ClassesRoot}\{entry.ProgId}");
            progIdKey.SetStringValue(string.Empty, entry.Description);

            using IRegistryProvider commandKey = RegistryProviderFactory.Open(RegistryHive.CurrentUser, $@"{ClassesRoot}\{entry.ProgId}\shell\open\command");
            commandKey.SetStringValue(string.Empty, $"\"{exePath}\" \"%1\"");
        }
    }
}
#endif
