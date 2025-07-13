using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Configuration;

public interface IFileAssociationHandler
{
	[return: MarshalAs(UnmanagedType.U1)]
	bool CanAssociationBeChanged();

	int GetFileAssociationInfoList(out IList<FileAssociationInfo> fileAssociationInfoList);

	int SetFileAssociationInfo(IList<FileAssociationInfo> fileAssociationInfoList);
}
