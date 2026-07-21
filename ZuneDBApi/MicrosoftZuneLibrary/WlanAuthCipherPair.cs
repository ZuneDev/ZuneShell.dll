using System;

namespace MicrosoftZuneLibrary;

public class WlanAuthCipherPair : IDisposable
{
    private WirelessAuthenticationTypes m_Auth;
    private WirelessCiphers m_Cipher;

    public WirelessCiphers Cipher
    {
        get { return m_Cipher; }
        set { m_Cipher = value; }
    }

    public WirelessAuthenticationTypes Auth
    {
        get { return m_Auth; }
        set { m_Auth = value; }
    }

    public WlanAuthCipherPair(WirelessAuthenticationTypes auth, WirelessCiphers cipher)
    {
        m_Cipher = cipher;
        m_Auth = auth;
    }

    public WlanAuthCipherPair()
    {
        m_Auth = WirelessAuthenticationTypes.Open;
        m_Cipher = WirelessCiphers.None;
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
