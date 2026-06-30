using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service;

public class AccountSettings
{
    private IDictionary<PrivacySettingId, PrivacySettingValue> m_privacySettings;
    private EmailFormat m_emailFormat;
    private bool m_allowZuneEmails;
    private bool m_allowPartnerEmails;

    public IDictionary<PrivacySettingId, PrivacySettingValue> PrivacySettings
    {
        get
        {
            if (m_privacySettings == null)
            {
                m_privacySettings = new Dictionary<PrivacySettingId, PrivacySettingValue>();
            }
            return m_privacySettings;
        }
    }

    public bool AllowPartnerEmails
    {
        [return: MarshalAs(UnmanagedType.U1)]
        get { return m_allowPartnerEmails; }
        [param: MarshalAs(UnmanagedType.U1)]
        set { m_allowPartnerEmails = value; }
    }

    public bool AllowZuneEmails
    {
        [return: MarshalAs(UnmanagedType.U1)]
        get { return m_allowZuneEmails; }
        [param: MarshalAs(UnmanagedType.U1)]
        set { m_allowZuneEmails = value; }
    }

    public EmailFormat EmailFormat
    {
        get { return m_emailFormat; }
        set { m_emailFormat = value; }
    }

    internal AccountSettings()
    {
    }
}
