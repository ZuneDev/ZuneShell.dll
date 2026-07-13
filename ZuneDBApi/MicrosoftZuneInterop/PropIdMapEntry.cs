using System.Runtime.InteropServices;

namespace MicrosoftZuneInterop;

[StructLayout(LayoutKind.Sequential, Size = 16)]
internal struct PropIdMapEntry
{
    private long _alignment;
}
