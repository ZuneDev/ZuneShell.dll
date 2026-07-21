using System;

namespace MicrosoftZunePlayback;

// Original wraps a native bandwidth-test COM object (a stripped-down cousin of
// PlayerInterop, per the naming, but for a MBR download-speed probe rather than
// playback). Not reverse engineered — see logs/MicrosoftZunePlayback/PlayerInterop.md.
public class BandwidthTestInterop
{
    public event BandwidthTestUpdateEventHandler BandwidthTestUpdate;
    public event BandwidthTestErrorEventHandler BandwidthTestError;

    public void Start(string uri, int testTimeoutSeconds)
    {
        BandwidthTestError?.Invoke(this, new BandwidthTestErrorArgs(unchecked((int)0x80004005)));
    }

    public void Cancel()
    {
    }
}
