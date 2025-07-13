using System;

namespace MicrosoftZuneLibrary;

public interface IRequestCallbackOnUIThread
{
	void CallbackOnUIThreadRequest(CallbackPriorityManaged priority, int id, IntPtr pData, IntPtr pInterface);
}
