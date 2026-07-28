using System.Collections.Generic;
using ZuneUI;

namespace Microsoft.Zune.Configuration
{
    /// <summary>
    /// Honest no-op fallback for platforms with no known file-association
    /// mechanism implemented yet (e.g. macOS). Reports that nothing is owned
    /// and that associations can't be changed, rather than throwing, so
    /// callers like <c>ZuneUI.Management</c> degrade gracefully instead of
    /// crashing the settings UI.
    /// </summary>
    // TODO: implement a macOS handler (Launch Services, e.g. via `duti` or
    // `lsregister`) when macOS support is prioritized; see CLAUDE.md's stage-3
    // rules on gating new platform-specific code.
    internal sealed class NullFileAssociationHandler : IFileAssociationHandler
    {
        public bool CanAssociationBeChanged() => false;

        public int GetFileAssociationInfoList(out IList<FileAssociationInfo> fileAssociationInfoList)
        {
            fileAssociationInfoList = new List<FileAssociationInfo>();
            return HRESULT._S_OK;
        }

        public int SetFileAssociationInfo(IList<FileAssociationInfo> fileAssociationInfoList) => HRESULT._E_FAIL;
    }
}
