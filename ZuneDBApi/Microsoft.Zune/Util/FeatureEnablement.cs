namespace Microsoft.Zune.Util;

// Original queries a native region/geo/feature-flag service. Not reverse engineered —
// see logs/Microsoft.Zune/Util/FeatureEnablement.md.
public class FeatureEnablement
{
    public static bool IsFeatureEnabled(Features eFeature) => false;

    public static void ForceFeatureOn(Features eFeature)
    {
    }

    public static string GetRegion() => nameof(GetRegion);

    public static uint GetGeoId() => GetInvalidGeoId();

    public static uint GetInvalidGeoId() => uint.MaxValue;

    public static bool HasValidRegionAndLanguage() => false;

    public static string GetMarketplaceCulture() => nameof(GetMarketplaceCulture);

    public static string GetLynxCulture() => nameof(GetLynxCulture);

    public static string GetTaxString() => nameof(GetTaxString);

    public static string GetCreditCardValidationString() => nameof(GetCreditCardValidationString);
}
