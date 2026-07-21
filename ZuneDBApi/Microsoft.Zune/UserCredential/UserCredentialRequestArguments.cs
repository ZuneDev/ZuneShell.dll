using System.Net;

namespace Microsoft.Zune.UserCredential;

public class UserCredentialRequestArguments
{
    private string m_targetUrl;
    private string m_host;
    private int m_lastError;
    private string m_realm;
    private string m_lastUserName;
    private AuthenticationSchemes m_authScheme;
    private bool m_isAuthSchemeSafe;
    private NetworkCredential m_networkCredential;
    private bool m_save;

    public bool Save
    {
        get { return m_save; }
        set { m_save = value; }
    }

    public NetworkCredential Credential
    {
        get { return m_networkCredential; }
        set { m_networkCredential = value; }
    }

    public bool IsAuthenticationSchemeSafe => m_isAuthSchemeSafe;

    public AuthenticationSchemes AuthScheme => m_authScheme;

    public int LastError => m_lastError;

    public string LastUserName => m_lastUserName;

    public string Realm => m_realm;

    public string Host => m_host;

    public string TargetUrl => m_targetUrl;

    internal UserCredentialRequestArguments(string targetUrl, string host, AuthenticationSchemes authScheme, bool isAuthSchemeSafe, string realm, string lastUserName, int lastError)
    {
        m_targetUrl = targetUrl;
        m_host = host;
        m_authScheme = authScheme;
        m_isAuthSchemeSafe = isAuthSchemeSafe;
        m_realm = realm;
        m_lastUserName = lastUserName;
        m_lastError = lastError;
        m_save = false;
        m_networkCredential = null;
    }
}
