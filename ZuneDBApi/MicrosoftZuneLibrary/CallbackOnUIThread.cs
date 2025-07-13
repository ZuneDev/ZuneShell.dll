using System;
using Microsoft.Iris;

namespace MicrosoftZuneLibrary;

public class CallbackOnUIThread : IRequestCallbackOnUIThread
{
	private CallbackOnUIThreadBimodalManaged_DONOTUSE bimodalHelper;

	public CallbackOnUIThread()
	{
		bimodalHelper = new CallbackOnUIThreadBimodalManaged_DONOTUSE(this);
	}

	public virtual void CallbackOnUIThreadRequest(CallbackPriorityManaged priority, int id, IntPtr pData, IntPtr pInterface)
	{
		CallbackOnUIThreadPack args = new CallbackOnUIThreadPack(id, pData, pInterface);
		DeferredInvokePriority priority2 = ((priority != CallbackPriorityManaged.eCallbackPriorityNormal) ? DeferredInvokePriority.Low : DeferredInvokePriority.Normal);
		Application.DeferredInvoke(DeferredCallback, args, priority2);
	}

	private void DeferredCallback(object args)
	{
		CallbackOnUIThreadPack callbackOnUIThreadPack = (CallbackOnUIThreadPack)args;
		bimodalHelper.Callback(callbackOnUIThreadPack._id, callbackOnUIThreadPack._pData, callbackOnUIThreadPack._pInterface);
	}
}
