using System;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary;

public interface IRequestCallbackOnUIThread
{
    void CallbackOnUIThreadRequest(CallbackPriorityManaged priority, int id, IntPtr pv, IntPtr pInterface);
}
