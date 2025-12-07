using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.VisualC;

namespace Microsoft.Zune.Subscription;

[StructLayout(LayoutKind.Sequential, Size = 16)]
[NativeCppClass]
[DebugInfoInPDB]
[MiscellaneousBits(65)]
internal unsafe struct PROPERTY_TO_PID_MAP
{
	public ushort* propertyName;
	public uint propertyId;
}
