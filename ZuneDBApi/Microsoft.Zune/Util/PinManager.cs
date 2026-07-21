using System;
using ZuneUI;

namespace Microsoft.Zune.Util;

// Original wraps a native IPinProvider*. Not reverse engineered — see
// logs/Microsoft.Zune/Util/DownloadManager.md.
public class PinManager : IDisposable
{
    private static PinManager sm_PinManager;

    public static PinManager Instance => sm_PinManager ??= new PinManager();

    private PinManager()
    {
    }

    public HRESULT AddPin(EPinType ePinType, string szPinServiceRef, string szDescription, EServiceMediaType ePinServiceTypeId, int nUserId, int nOrdinal, out int nPinId)
    {
        nPinId = -1;
        return HRESULT._E_FAIL;
    }

    public HRESULT AddPin(EPinType ePinType, int nPinMediaId, EMediaTypes ePinTypeId, int nUserId, int nOrdinal, out int pinId)
    {
        pinId = -1;
        return HRESULT._E_FAIL;
    }

    public HRESULT FindPin(EPinType ePinType, string szPinServiceRef, EServiceMediaType ePinServiceTypeId, int nUserId, int nMaxAge, out int nPinId)
    {
        nPinId = -1;
        return HRESULT._E_FAIL;
    }

    public HRESULT FindPin(EPinType ePinType, int nPinMediaId, EMediaTypes ePinTypeId, int nUserId, int nMaxAge, out int nPinId)
    {
        nPinId = -1;
        return HRESULT._E_FAIL;
    }

    public HRESULT DeletePin(int nPinId) => HRESULT._E_FAIL;

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
