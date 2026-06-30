using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.VisualC;

namespace Microsoft.Zune.Configuration
{
    [StructLayout(LayoutKind.Sequential, Size = 24)]
    [MiscellaneousBits(64)]
    [NativeCppClass]
    [DebugInfoInPDB]
    internal struct NotificationMarshaller
    {
        private long _alignment;
    }
}