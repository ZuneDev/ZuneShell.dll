using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary
{
	internal class DeferredSetResultArgs
	{
		public int RequestGeneration;

		public ZuneQueryList QueryList;

		public bool RetainedList;

		public DeferredSetResultArgs(int requestGeneration, ZuneQueryList queryList, [MarshalAs(UnmanagedType.U1)] bool retainedList)
		{
			RequestGeneration = requestGeneration;
			QueryList = queryList;
			RetainedList = retainedList;
			base._002Ector();
		}
	}
}
