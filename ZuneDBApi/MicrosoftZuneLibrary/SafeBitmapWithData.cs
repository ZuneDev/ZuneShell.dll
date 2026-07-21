using System;
using Microsoft.Iris;

namespace MicrosoftZuneLibrary;

// Original queries the bitmap's actual dimensions via GDI's GetObjectW and calls
// ZuneLibraryExports.CopyThumbnailBitmapData/GetThumbnailBitmapData to clone/create
// bitmaps — neither reverse engineered. See logs/MicrosoftZuneLibrary/SafeBitmap.md.
public class SafeBitmapWithData : SafeBitmap
{
    private unsafe void* _pImageData;
    private int _iHeight;
    private int _iWidth;

    public int ScanLineWidth => ((_iWidth + 1) * 3) & -4;

    public int Height => _iHeight;

    public int Width => _iWidth;

    internal unsafe SafeBitmapWithData(int iHeight, int iWidth, IntPtr pData, IntPtr hBitmap)
        : base(hBitmap)
    {
        _iHeight = iHeight;
        _iWidth = iWidth;
        _pImageData = pData.ToPointer();
    }

    protected unsafe override bool ReleaseHandle()
    {
        _pImageData = null;
        return base.ReleaseHandle();
    }

    public unsafe IntPtr GetData() => (IntPtr)_pImageData;

    public Image CreateImage(bool antialiasEdges)
    {
        IntPtr data = GetData();
        if (Width != 0 && Height != 0 && data != IntPtr.Zero)
        {
            return new Image(null, Width, Height, -ScanLineWidth, RawImageFormat.R8G8B8, data, 0, 0, false, antialiasEdges);
        }
        return null;
    }

    public Image CreateImage() => CreateImage(false);

    public SafeBitmapWithData Clone(int srcX, int srcY, int srcWidth, int srcHeight, int dstWidth, int dstHeight) => null;

    public static SafeBitmapWithData CreateThumbnailBitmap(string strFilename) => null;
}
