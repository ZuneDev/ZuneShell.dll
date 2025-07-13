using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.VisualC;

[StructLayout(LayoutKind.Sequential, Size = 1)]
[NativeCppClass]
[MiscellaneousBits(64)]
[DebugInfoInPDB]
internal struct CSchemaMap
{
	[StructLayout(LayoutKind.Sequential, Size = 32)]
	[NativeCppClass]
	[MiscellaneousBits(65)]
	[DebugInfoInPDB]
	internal struct _SCHEMAMAPENTRY
	{
		private long _003Calignment_0020member_003E;
	}
}
