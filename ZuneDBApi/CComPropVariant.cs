using Microsoft.VisualC;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 24)]
[NativeCppClass]
[DebugInfoInPDB]
[MiscellaneousBits(64)]
internal struct CComPropVariant
{
	public VARTYPE vt;

	public ushort wReserved1;
	public ushort wReserved2;
	public ushort wReserved3;

	public ulong val1;
	public ulong val2;
}
