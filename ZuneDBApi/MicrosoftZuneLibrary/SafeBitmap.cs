using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace MicrosoftZuneLibrary;

public class SafeBitmap : SafeHandleZeroOrMinusOneIsInvalid
{
    [DllImport("gdi32.dll")]
    private static extern bool DeleteObject(IntPtr hObject);

    public SafeBitmap(IntPtr hBitmap)
        : base(ownsHandle: true)
    {
        handle = hBitmap;
    }

    protected override bool ReleaseHandle()
    {
        return DeleteObject(handle);
    }
}
