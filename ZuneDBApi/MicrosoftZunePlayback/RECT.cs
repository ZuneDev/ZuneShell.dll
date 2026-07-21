using System.Runtime.InteropServices;

namespace MicrosoftZunePlayback;

// Standard Win32 RECT (originally tagRECT in the decompiled VideoPosition setter) — a
// well-documented, stable layout, unlike the project-specific structs in this folder.
[StructLayout(LayoutKind.Sequential)]
internal struct RECT
{
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;

    public RECT(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }
}
