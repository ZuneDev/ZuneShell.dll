using System;

namespace MicrosoftZuneLibrary;

public class CallbackOnUIThreadPack
{
	public readonly int _id;

	public readonly IntPtr _pData;

	public readonly IntPtr _pInterface;

	public CallbackOnUIThreadPack(int id, IntPtr pData, IntPtr pInterface)
	{
		_id = id;
		_pData = pData;
		_pInterface = pInterface;
	}
}
