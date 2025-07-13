using System;

namespace MicrosoftZunePlayback;

public class BandwidthTestErrorArgs(int errorCode) : EventArgs()
{
	private int _errorCode = errorCode;

	public int ErrorCode => errorCode;
}
