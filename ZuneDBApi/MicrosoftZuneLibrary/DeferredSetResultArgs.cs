using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary;

internal class DeferredSetResultArgs(int requestGeneration, ZuneQueryList queryList, [MarshalAs(UnmanagedType.U1)] bool retainedList)
{
	public int RequestGeneration = requestGeneration;

	public ZuneQueryList QueryList = queryList;

	public bool RetainedList = retainedList;
}
