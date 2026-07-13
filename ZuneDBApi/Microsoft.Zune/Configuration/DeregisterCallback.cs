using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Configuration
{
    [StructLayout(LayoutKind.Sequential, Size = 24)]
    [NativeCppClass]
    internal struct DeregisterCallback
    {
        private long _alignment;
    }
}