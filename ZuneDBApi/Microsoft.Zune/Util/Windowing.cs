using Microsoft.Iris.Session;

namespace Microsoft.Zune.Util;

public class Windowing
{
    public static void ForceSetForegroundWindow(IntPtr hwnd)
    {
        // Stub: Original used SendInput and SetForegroundWindow from user32.dll
        // This is a placeholder implementation
        
        UISession.Default.ForceSetForegroundWindow(hwnd);
    }
}
