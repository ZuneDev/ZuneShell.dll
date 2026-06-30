using System;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary;

public class SafeBitmapWithData : SafeBitmap
{
    private readonly IntPtr m_pBits;
    private readonly int m_stride;

    public IntPtr PBits => m_pBits;
    public int Stride => m_stride;

    public SafeBitmapWithData(IntPtr hBitmap, IntPtr pBits, int stride)
        : base(hBitmap)
    {
        m_pBits = pBits;
        m_stride = stride;
    }
}
