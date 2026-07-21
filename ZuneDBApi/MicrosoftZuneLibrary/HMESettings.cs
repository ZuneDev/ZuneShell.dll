using System;

namespace MicrosoftZuneLibrary;

// Original wraps native IHMESettings/INSSManager/INSSDevices COM interfaces for Windows
// Media Player "Home Media Experience" (network media sharing). Not reverse
// engineered — see logs/MicrosoftZuneLibrary/HMESettings.md.
public class HMESettings : IDisposable
{
    public event NSSDeviceListChangeHandler NSSDeviceListChangeEvent;

    public bool VelaSharingEnabled => false;

    public bool SharingEnableRequiresLoginAsAdmin => false;

    public bool SharingEnableRequiresElevation => false;

    public bool SharingEnabled => false;

    public bool SharingBroken => false;

    public int Init() => unchecked((int)0x80004005);

    public int RepairSharing() => unchecked((int)0x80004005);

    public int EnableSharingForUser() => unchecked((int)0x80004005);

    public int DisableSharingForUser() => unchecked((int)0x80004005);

    public int DisableSharingForMachine() => unchecked((int)0x80004005);

    public int SetSharedFoldersList(bool fForce) => unchecked((int)0x80004005);

    public int GetDisplayName(ref string strName)
    {
        strName = null;
        return unchecked((int)0x80004005);
    }

    public int SetDisplayName(string strName) => unchecked((int)0x80004005);

    public bool GetSharingEnabledForMediaType(EMediaTypes mediaType) => false;

    public int SetSharingEnabledForMediaType(EMediaTypes mediaType, bool bEnabled) => unchecked((int)0x80004005);

    public bool GetAllDevicesEnabled() => false;

    public int SetAllDevicesEnabled(bool bEnabled) => unchecked((int)0x80004005);

    public uint GetDeviceCount() => 0;

    public int GetDeviceProps(uint dwIndex, ref string strName, ref string strMAC, ref string strSerialNumber)
    {
        strName = null;
        strMAC = null;
        strSerialNumber = null;
        return unchecked((int)0x80004005);
    }

    public bool GetDeviceEnabled(uint dwIndex) => false;

    public int EnableDevice(uint dwIndex, bool bEnabled) => unchecked((int)0x80004005);

    internal void NSSDeviceListChange()
    {
        NSSDeviceListChangeEvent?.Invoke();
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
