using System;

namespace Microsoft.Zune.Configuration
{
	public class EventArgsHR : EventArgs
	{
		private int _003Cbacking_store_003EHResult;

		public int HResult
		{
			get
			{
				return _003Cbacking_store_003EHResult;
			}
			set
			{
				_003Cbacking_store_003EHResult = value;
			}
		}

		public EventArgsHR()
		{
			HResult = 0;
		}
	}
}
