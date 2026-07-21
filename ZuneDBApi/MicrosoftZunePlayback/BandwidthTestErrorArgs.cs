using System;

namespace MicrosoftZunePlayback;

public class BandwidthTestErrorArgs : EventArgs
{
    public int ErrorCode { get; }

    internal BandwidthTestErrorArgs(int errorCode)
    {
        ErrorCode = errorCode;
    }
}
