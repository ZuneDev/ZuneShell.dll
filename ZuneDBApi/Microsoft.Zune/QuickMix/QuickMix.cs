using System;
using ZuneUI;

namespace Microsoft.Zune.QuickMix;

// Original wraps a native QuickMix engine (IQuickMixSession factory). Not reverse
// engineered — see logs/Microsoft.Zune/QuickMix/QuickMix.md.
public class QuickMix
{
    private static QuickMix sm_instance;

    public static QuickMix Instance => sm_instance ??= new QuickMix();

    public bool IsReady => false;

    public event QuickMixProgressHandler OnProgress;

    private QuickMix()
    {
    }

    public HRESULT CreateSession(EQuickMixMode eQuickMixMode, Guid serviceMediaId, EMediaTypes eMediaType, string mediaTitle, out QuickMixSession quickMixSession)
    {
        quickMixSession = null;
        return HRESULT._E_FAIL;
    }

    public HRESULT CreateSession(EQuickMixMode eQuickMixMode, int[] seedMediaIds, EMediaTypes eMediaType, out QuickMixSession quickMixSession)
    {
        quickMixSession = null;
        return HRESULT._E_FAIL;
    }
}
