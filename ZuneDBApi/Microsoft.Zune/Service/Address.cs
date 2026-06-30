using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service;

public class Address
{
    private string m_street1;
    private string m_street2;
    private string m_city;
    private string m_district;
    private string m_state;
    private string m_postalCode;

    public string PostalCode
    {
        get { return m_postalCode; }
        set { m_postalCode = value; }
    }

    public string State
    {
        get { return m_state; }
        set { m_state = value; }
    }

    public string District
    {
        get { return m_district; }
        set { m_district = value; }
    }

    public string City
    {
        get { return m_city; }
        set { m_city = value; }
    }

    public string Street2
    {
        get { return m_street2; }
        set { m_street2 = value; }
    }

    public string Street1
    {
        get { return m_street1; }
        set { m_street1 = value; }
    }

    internal Address()
    {
    }

    public Address(string street1, string street2, string city, string district, string state, string postalCode)
    {
        m_street1 = street1;
        m_street2 = street2;
        m_city = city;
        m_district = district;
        m_state = state;
        m_postalCode = postalCode;
    }
}
