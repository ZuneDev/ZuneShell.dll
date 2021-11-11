using System;

namespace Microsoft.Zune.Service
{
	[Flags]
	public enum EClientTypeFlags
	{
		All = 0xFF,
		Zune3Device = 0x2,
		PC = 0x1,
		None = 0x0
	}
}
