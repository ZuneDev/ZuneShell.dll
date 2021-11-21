using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 24)]
[NativeCppClass]
internal unsafe struct tagCOPYDATASTRUCT
{
	public ulong* dwData;
	public uint cbData;
	public void* lpData;
}
