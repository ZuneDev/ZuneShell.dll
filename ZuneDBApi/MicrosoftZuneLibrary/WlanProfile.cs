using System;

namespace MicrosoftZuneLibrary;

public class WlanProfile : IDisposable
{
    private string m_strSSID;
    private WirelessAuthenticationTypes m_Auth;
    private WirelessCiphers m_Cipher;
    private bool m_fPassPhrase;
    private bool m_fEncrypted;
    private string m_strKey;
    private byte[] m_EncryptedKey;
    private uint m_uKeyIndex;
    private uint m_uKeyLength;
    private uint m_uSignalQuality;
    private bool m_fConnected;

    public bool Connected
    {
        get { return m_fConnected; }
        set { m_fConnected = value; }
    }

    public uint SignalQuality
    {
        get { return m_uSignalQuality; }
        set { m_uSignalQuality = value; }
    }

    public uint KeyIndex
    {
        get { return m_uKeyIndex; }
        set { m_uKeyIndex = value; }
    }

    public bool Encrypted => m_fEncrypted;

    public bool PassPhrase
    {
        get { return m_fPassPhrase; }
        set { m_fPassPhrase = value; }
    }

    public byte[] EncryptedKey
    {
        get { return m_EncryptedKey; }
        set
        {
            m_EncryptedKey = value;
            m_fEncrypted = true;
            m_strKey = null;
        }
    }

    public string Key
    {
        get { return m_strKey; }
        set
        {
            m_strKey = value;
            m_fEncrypted = false;
            m_EncryptedKey = null;
        }
    }

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

    public string SSID
    {
        get { return m_strSSID; }
        set { m_strSSID = value; }
    }

    public WlanProfile()
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
