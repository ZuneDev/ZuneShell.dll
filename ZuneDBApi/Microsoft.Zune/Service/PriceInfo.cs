using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service;

public class PriceInfo
{
    private int m_pointsPrice;
    private double m_currencyPrice;
    private bool m_hasPoints = true;
    private bool m_hasCurrency;
    private string m_displayPrice;
    private string m_currencyCode;

    public string CurrencyCode => m_currencyCode;

    public string DisplayPrice => m_displayPrice;

    public bool HasCurrency
    {
        [return: MarshalAs(UnmanagedType.U1)]
        get { return m_hasCurrency; }
    }

    public bool HasPoints
    {
        [return: MarshalAs(UnmanagedType.U1)]
        get { return m_hasPoints; }
    }

    public double CurrencyPrice => m_currencyPrice;

    public int PointsPrice => m_pointsPrice;

    internal PriceInfo()
    {
    }

    public PriceInfo(int pointsPrice)
    {
        m_pointsPrice = pointsPrice;
    }

    public void MakeFree()
    {
        m_pointsPrice = 0;
        m_currencyPrice = 0.0;
        m_displayPrice = null;
    }

    public static PriceInfo FreeWithPoints()
    {
        return new PriceInfo();
    }
}
