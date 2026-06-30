using System;

namespace Microsoft.Zune.Service;

public class AccountUser
{
    private string m_zuneTag;
    private string m_locale;
    private string m_firstName;
    private string m_lastName;
    private string m_email;
    private string m_phoneNumber;
    private string m_mobilePhoneNumber;
    private DateTime m_birthday;
    private Address m_address;
    private AccountSettings m_accountSettings;
    private PassportIdentity m_parentPassportIdentity;
    private CreditCard m_parentCreditCard;
    private AccountUserType m_accountUserType;

    public AccountUserType AccountUserType
    {
        get { return m_accountUserType; }
        set { m_accountUserType = value; }
    }

    public CreditCard ParentCreditCard
    {
        get { return m_parentCreditCard; }
        set { m_parentCreditCard = value; }
    }

    public PassportIdentity ParentPassportIdentity
    {
        get { return m_parentPassportIdentity; }
        set { m_parentPassportIdentity = value; }
    }

    public AccountSettings AccountSettings
    {
        get { return m_accountSettings; }
        set { m_accountSettings = value; }
    }

    public Address Address
    {
        get { return m_address; }
        set { m_address = value; }
    }

    public DateTime Birthday
    {
        get { return m_birthday; }
        set { m_birthday = value; }
    }

    public string MobilePhoneNumber
    {
        get { return m_mobilePhoneNumber; }
        set { m_mobilePhoneNumber = value; }
    }

    public string PhoneNumber
    {
        get { return m_phoneNumber; }
        set { m_phoneNumber = value; }
    }

    public string Email
    {
        get { return m_email; }
        set { m_email = value; }
    }

    public string LastName
    {
        get { return m_lastName; }
        set { m_lastName = value; }
    }

    public string FirstName
    {
        get { return m_firstName; }
        set { m_firstName = value; }
    }

    public string Locale
    {
        get { return m_locale; }
        set { m_locale = value; }
    }

    public string ZuneTag
    {
        get { return m_zuneTag; }
        set { m_zuneTag = value; }
    }

    internal AccountUser()
    {
        m_accountUserType = AccountUserType.Unknown;
    }
}
