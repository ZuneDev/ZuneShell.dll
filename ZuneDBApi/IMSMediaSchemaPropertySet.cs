using Microsoft.VisualC;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
[DebugInfoInPDB]
[MiscellaneousBits(65)]
[NativeCppClass]
internal unsafe struct IMSMediaSchemaPropertySet
{
	private long _alignment1;

	// 8
	public delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint> addRef;

	// 16
	public delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint> dispose;

	private long _alignment4;
	private long _alignment5;
	private long _alignment6;

	// 48
	public delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, uint, tagPROPVARIANT*, int> readValue;

	// 56
	public delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, tagPROPVARIANT, int> setValue;
}
