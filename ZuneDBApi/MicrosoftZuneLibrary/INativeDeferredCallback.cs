using System;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary;

public interface INativeDeferredCallback
{
    void Invoke(int hrResult, IntPtr pContext);
}
