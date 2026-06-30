using System;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service;

public class CreditCard : PaymentInstrument
{
    private Address m_address;
    private CreditCardType m_creditCardType;
    private string m_contactFirstName;
    private string m_contactLastName;
    private string m_accountHolderName;
    private string m_accountNumber;
    private string m_ccvNumber;
    private DateTime m_expirationDate;
    private string m_phonePrefix;
    private string m_phoneNumber;
    private string m_phoneExtension;
    private string m_email;
    private string m_locale;
    private bool m_parentCreditCard;

    public bool ParentCreditCard
    {
        [return: MarshalAs(UnmanagedType.U1)]
        get { return m_parentCreditCard; }
        [param: MarshalAs(UnmanagedType.U1)]
        set { m_parentCreditCard = value; }
    }

    public string Email
    {
        get { return m_email; }
        set { m_email = value; }
    }

    public string Locale
    {
        get { return m_locale; }
        set { m_locale = value; }
    }

    public string PhoneExtension
    {
        get { return m_phoneExtension; }
        set { m_phoneExtension = value; }
    }

    public string PhoneNumber
    {
        get { return m_phoneNumber; }
        set { m_phoneNumber = value; }
    }

    public string PhonePrefix
    {
        get { return m_phonePrefix; }
        set { m_phonePrefix = value; }
    }

    public DateTime ExpirationDate
    {
        get { return m_expirationDate; }
        set { m_expirationDate = value; }
    }

    public string CCVNumber
    {
        get { return m_ccvNumber; }
        set { m_ccvNumber = value; }
    }

    public string AccountNumber
    {
        get { return m_accountNumber; }
        set { m_accountNumber = value; }
    }

    public string ContactLastName
    {
        get { return m_contactLastName; }
        set { m_contactLastName = value; }
    }

    public string ContactFirstName
    {
        get { return m_contactFirstName; }
        set { m_contactFirstName = value; }
    }

    public string AccountHolderName
    {
        get { return m_accountHolderName; }
        set { m_accountHolderName = value; }
    }

    public CreditCardType CreditCardType
    {
        get { return m_creditCardType; }
        set { m_creditCardType = value; }
    }

    public Address Address
    {
        get { return m_address; }
        set { m_address = value; }
    }

    internal CreditCard()
        : base(null, PaymentType.CreditCard)
    {
        m_address = new Address();
    }

    public CreditCard(string id, Address address, CreditCardType creditCardType, string accountHolderName, string accountNumber, string ccvNumber, DateTime expirationDate, string phonePrefix, string phoneNumber, string phoneExtension)
        : base(id, PaymentType.CreditCard)
    {
        m_address = address;
        m_creditCardType = creditCardType;
        m_accountNumber = accountNumber;
        m_ccvNumber = ccvNumber;
        m_expirationDate = expirationDate;
        m_phonePrefix = phonePrefix;
        m_phoneNumber = phoneNumber;
        m_phoneExtension = phoneExtension;
        m_accountHolderName = accountHolderName;
    }

    public override string ToString()
    {
        if (m_accountNumber != null && m_accountNumber.Length > 4)
        {
            return m_accountNumber.Substring(m_accountNumber.Length - 4, 4);
        }
        return m_accountNumber;
    }
}
