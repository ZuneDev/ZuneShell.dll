using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service;

public class PriceInfo
{
    // Reads every recovered IPriceInfo slot (see IPriceInfo.cs) — matches the original
    // PriceInfo.Init(IPriceInfo*) decompiled body exactly (ILSpy decompilation of
    // Microsoft.Zune.Service.PriceInfo in ZuneShell/lib/ZuneDBApi.dll): points price,
    // currency price, HasPoints/HasCurrency, then display price and currency code as
    // BSTR out-params. A null pPriceInfo (the "free" case) keeps the original's
    // defaults: HasPoints = true, HasCurrency = false, both prices zero.
    internal PriceInfo(IPriceInfo? pPriceInfo)
        : this()
    {
        if (pPriceInfo is null)
            return;

        PointsPrice = pPriceInfo.GetPointsPrice();
        CurrencyPrice = pPriceInfo.GetCurrencyPrice();
        HasPoints = pPriceInfo.HasPoints() != 0;
        HasCurrency = pPriceInfo.HasCurrency() != 0;

        if (pPriceInfo.GetDisplayPrice(out string displayPrice) >= 0)
            DisplayPrice = displayPrice;
        if (pPriceInfo.GetCurrencyCode(out string currencyCode) >= 0)
            CurrencyCode = currencyCode;
    }

    public string CurrencyCode { get; }

    public string DisplayPrice { get; private set; }

    public bool HasCurrency { [return: MarshalAs(UnmanagedType.U1)] get; }

    public bool HasPoints { [return: MarshalAs(UnmanagedType.U1)] get; } = true;

    public double CurrencyPrice { get; private set; }

    public int PointsPrice { get; private set; }

    internal PriceInfo()
    {
    }

    public PriceInfo(int pointsPrice)
    {
        PointsPrice = pointsPrice;
    }

    public void MakeFree()
    {
        PointsPrice = 0;
        CurrencyPrice = 0.0;
        DisplayPrice = null;
    }

    public static PriceInfo FreeWithPoints()
    {
        return new PriceInfo();
    }
}
