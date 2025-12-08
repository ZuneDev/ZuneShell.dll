using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 24)]
internal struct CComPropVariant
{
	[FieldOffset(0)]
	public VARTYPE vt;

	[FieldOffset(8)]
	public ulong val1;

	[FieldOffset(16)]
	public ulong val2;
}
