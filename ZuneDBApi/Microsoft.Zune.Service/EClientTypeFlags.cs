using System;

namespace Microsoft.Zune.Service;

[Flags]
public enum EClientTypeFlags
{
	All = 0xFF,
	Zune3Device = 2,
	PC = 1,
	None = 0
}
