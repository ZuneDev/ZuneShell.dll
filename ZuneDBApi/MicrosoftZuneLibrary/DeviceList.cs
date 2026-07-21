using System;
using System.Collections;

namespace MicrosoftZuneLibrary;

// Original wraps a native IEndpointHostManager*/EndpointHostManagerMediator pair that
// enumerates connected devices. Not reverse engineered — see
// logs/MicrosoftZuneLibrary/Device.md. The list is always empty since there's no
// native endpoint host to enumerate.
public class DeviceList : IDisposable
{
    private static DeviceList m_singletonInstance;

    private SortedList m_slDevices = new();

    public static DeviceList Instance => m_singletonInstance ??= new DeviceList();

    public bool Initialized { get; set; }

    public int Count => m_slDevices.Count;

    public event DeviceAddedHandler Added;

    private DeviceList()
    {
    }

    public int InitializeAndEnumerate() => unchecked((int)0x80004005);

    public Device GetItem(int idx) => (Device)m_slDevices.GetByIndex(idx);

    public void HideDevice(Device device)
    {
    }

    public void UnhideDevice(Device device)
    {
    }

    public int GetTranscodedFilesCachePath(ref string strCachePath)
    {
        strCachePath = null;
        return unchecked((int)0x80004005);
    }

    public int SetTranscodedFilesCachePath(string strCachePath) => unchecked((int)0x80004005);

    public int SetTranscodedFilesCacheSize(int lCacheSize) => unchecked((int)0x80004005);

    public int ClearTranscodeCache() => unchecked((int)0x80004005);

    protected void raise_Added(Device value0)
    {
        Added?.Invoke(value0);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
