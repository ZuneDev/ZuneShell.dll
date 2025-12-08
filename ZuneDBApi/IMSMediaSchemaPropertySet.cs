using Microsoft.VisualC;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
[DebugInfoInPDB]
[MiscellaneousBits(65)]
[NativeCppClass]
internal unsafe struct IMSMediaSchemaPropertySet
{
	[FieldOffset(8)]
	public delegate* unmanaged[Cdecl]<IntPtr, uint> addRef;

	[FieldOffset(16)]
	public delegate* unmanaged[Cdecl]<IntPtr, uint> dispose;

	[FieldOffset(48)]
	public delegate* unmanaged[Cdecl]<IntPtr, uint, uint, tagPROPVARIANT*, int> readValue;

	[FieldOffset(56)]
	public delegate* unmanaged[Cdecl]<IntPtr, uint, tagPROPVARIANT, int> setValue;
}
