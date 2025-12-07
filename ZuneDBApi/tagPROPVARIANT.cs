using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.VisualC;

[StructLayout(LayoutKind.Sequential, Size = 24)]
[MiscellaneousBits(65)]
[NativeCppClass]
[DebugInfoInPDB]
internal struct tagPROPVARIANT
{
    public VARTYPE vt;

    private ushort _alignment1;
    private ushort _alignment2;
    private ushort _alignment3;

    public long val1;
    public long val2;
}
