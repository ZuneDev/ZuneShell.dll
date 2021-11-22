using System;

namespace MicrosoftZunePlayback
{
	public class BandwidthTestErrorArgs : EventArgs
	{
		private int _errorCode;

		public int ErrorCode => _errorCode;

		public BandwidthTestErrorArgs(int errorCode)
		{
			_errorCode = errorCode;
		}
	}
}
