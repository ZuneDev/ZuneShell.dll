using System;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary;

public class CallbackOnUIThreadPack
{
    private readonly object m_pCallback; // Stub for callback data
    private readonly int m_id;

    public CallbackOnUIThreadPack(object pCallback, int id)
    {
        m_pCallback = pCallback;
        m_id = id;
    }
}
