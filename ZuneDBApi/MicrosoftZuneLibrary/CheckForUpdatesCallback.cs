using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.VisualC;

namespace MicrosoftZuneLibrary;

[StructLayout(LayoutKind.Sequential, Size = 40)]
[MiscellaneousBits(64)]
[DebugInfoInPDB]
[NativeCppClass]
internal struct CheckForUpdatesCallback
{
	private long _003Calignment_0020member_003E;
}
