using System.Collections.Specialized;
using MicrosoftZuneLibrary;

namespace Microsoft.Zune.Util;

public class Clipboard
{
    private Clipboard()
    {
    }

    private static object CopyData(ClipboardDataType type, IntPtr handle)
    {
        // Stub: Original had complex native interop
        return null;
    }

    [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.U1)]
    public static bool ContainsData(ClipboardDataType type)
    {
        // Stub: Original used OpenClipboard/IsClipboardFormatAvailable/CloseClipboard
        return false;
    }

    [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.U1)]
    public static bool ContainsImage()
    {
        return ContainsData(ClipboardDataType.Image);
    }

    [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.U1)]
    public static bool ContainsFileDropList()
    {
        return ContainsData(ClipboardDataType.FileDropList);
    }

    public static object GetData(ClipboardDataType type)
    {
        // Stub
        return null;
    }

    public static SafeBitmap GetImage()
    {
        return GetData(ClipboardDataType.Image) as SafeBitmap;
    }

    public static StringCollection GetFileDropList()
    {
        return GetData(ClipboardDataType.FileDropList) as StringCollection;
    }
}
