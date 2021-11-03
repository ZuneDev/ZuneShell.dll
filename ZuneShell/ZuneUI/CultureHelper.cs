// Decompiled with JetBrains decompiler
// Type: ZuneUI.CultureHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Configuration;
using Microsoft.Zune.Util;
using System;
using System.Globalization;
using System.Text;
using System.Threading;
using UIXControls;

namespace ZuneUI
{
    public class CultureHelper
    {
        private static bool s_marketplaceCultureChecked = false;
        private static bool? useAlternateStyling;

        public static void CheckMarketplaceCulture()
        {
            if (s_marketplaceCultureChecked)
                return;
            string marketplaceCulture1 = FeatureEnablement.GetMarketplaceCulture();
            if (string.IsNullOrEmpty(marketplaceCulture1))
                return;
            s_marketplaceCultureChecked = true;
            string strB = null;
            if (marketplaceCulture1.Length > 1)
                strB = marketplaceCulture1.Substring(0, 2);
            string strA1 = CultureInfo.CurrentUICulture.ToString();
            string strA2 = null;
            if (strA1.Length > 1)
                strA2 = strA1.Substring(0, 2);
            bool flag1 = 0 == string.Compare(strA2, strB, StringComparison.InvariantCultureIgnoreCase);
            bool flag2 = false;
            bool flag3 = false;
            if (!flag1)
            {
                string marketplaceCulture2 = ClientConfiguration.Shell.LastMarketplaceCulture;
                flag2 = 0 != string.Compare(marketplaceCulture1, marketplaceCulture2, StringComparison.InvariantCultureIgnoreCase);
                string lastClientCulture = ClientConfiguration.Shell.LastClientCulture;
                flag3 = 0 != string.Compare(strA1, lastClientCulture, StringComparison.InvariantCultureIgnoreCase);
            }
            ClientConfiguration.Shell.LastMarketplaceCulture = marketplaceCulture1;
            ClientConfiguration.Shell.LastClientCulture = strA1;
            if (flag1 || !flag2 && !flag3)
                return;
            MessageBox.Show(Shell.LoadString(StringId.IDS_MARKETPLACE_CULTURE_MISMATCH_TITLE), Shell.LoadString(StringId.IDS_MARKETPLACE_CULTURE_MISMATCH), null);
        }

        public static void CheckValidRegionAndLanguage()
        {
            if (FeatureEnablement.HasValidRegionAndLanguage())
                return;
            ErrorDialogInfo.Show(HRESULT._ZUNE_E_UNKNOWN_REGION_OR_LANGUAGE.Int, Shell.LoadString(StringId.IDS_UNKNOWN_REGION_OR_LANGUAGE_TITLE));
        }

        public static bool ShowYomiSortFields()
        {
            string letterIsoLanguageName = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            string region = FeatureEnablement.GetRegion();
            return letterIsoLanguageName.Equals("ja", StringComparison.OrdinalIgnoreCase) || region.Equals("jp", StringComparison.OrdinalIgnoreCase);
        }

        public static string GetCurrentRegionDisplayName()
        {
            string region = FeatureEnablement.GetRegion();
            try
            {
                return new RegionInfo(FeatureEnablement.GetRegion()).DisplayName;
            }
            catch (ArgumentException ex)
            {
                return region;
            }
        }

        public static bool IsEnUs()
        {
            bool flag1 = Thread.CurrentThread.CurrentUICulture.Equals(new CultureInfo("en-US"));
            bool flag2 = StringHelper.IsEqualCaseInsensitive(FeatureEnablement.GetRegion(), "US");
            return flag1 && flag2;
        }

        public static bool UseAlternateStyling()
        {
            if (!useAlternateStyling.HasValue)
            {
                switch (CultureInfo.CurrentUICulture.LCID)
                {
                    case 4:
                    case 17:
                    case 18:
                    case 1028:
                    case 1041:
                    case 1042:
                    case 2052:
                    case 3076:
                    case 4100:
                    case 5124:
                    case 31748:
                        useAlternateStyling = new bool?(true);
                        break;
                    default:
                        useAlternateStyling = new bool?(false);
                        break;
                }
            }
            return useAlternateStyling.Value;
        }

        public static uint GeoId() => FeatureEnablement.GetGeoId();

        public static bool IsValidGeoId(uint geoId) => (int)FeatureEnablement.GetInvalidGeoId() != (int)geoId;

        public static int GetLCIDFromCultureString(string culture, bool useDefault)
        {
            int num = -1;
            if (!string.IsNullOrEmpty(culture))
            {
                try
                {
                    num = new CultureInfo(culture).LCID;
                }
                catch (Exception ex)
                {
                    if (culture.Equals("es-US", StringComparison.InvariantCultureIgnoreCase))
                        num = 21514;
                }
            }
            if (num == -1 && useDefault)
                num = CultureInfo.CurrentUICulture.LCID;
            return num;
        }

        public static string GetHelpUrl()
        {
            string uri = Shell.LoadString(StringId.IDS_WWW_ZUNE_NET_SUPPORT_URL);
            string lynxCulture = FeatureEnablement.GetLynxCulture();
            if (!string.IsNullOrEmpty(lynxCulture))
                uri = AppendFwlinkCulture(uri, lynxCulture);
            StringBuilder args = new StringBuilder(uri);
            string uiPath = ZuneShell.DefaultInstance.CurrentPage.UIPath;
            if (!string.IsNullOrEmpty(uiPath))
                UrlHelper.AppendParam(false, args, "path", uiPath);
            foreach (UIDevice uiDevice in SingletonModelItem<UIDeviceList>.Instance)
            {
                if (uiDevice.IsConnectedToPC || SignIn.Instance.SignedIn && uiDevice.UserId == SignIn.Instance.LastSignedInUserId)
                {
                    UrlHelper.AppendParam(false, args, "mfr", uiDevice.Manufacturer);
                    UrlHelper.AppendParam(false, args, "mdl", uiDevice.ModelName);
                }
            }
            return args.ToString();
        }

        public static string GetPrivacyUrl()
        {
            string uri = Shell.LoadString(StringId.IDS_PRIVACY_STATEMENT_URL);
            string lynxCulture = FeatureEnablement.GetLynxCulture();
            if (!string.IsNullOrEmpty(lynxCulture))
                uri = AppendFwlinkCulture(uri, lynxCulture);
            return uri;
        }

        public static string AppendFwlinkCulture(string uri, int lcid)
        {
            if (lcid >= 0)
                uri = string.Format("{0}&clcid=0x{1:x}", uri, lcid);
            return uri;
        }

        public static string AppendFwlinkCulture(string uri, string culture)
        {
            if (!string.IsNullOrEmpty(culture))
                uri = AppendFwlinkCulture(uri, GetLCIDFromCultureString(culture, false));
            return uri;
        }

        public static string AppendFwlinkCulture(string uri)
        {
            uri = AppendFwlinkCulture(uri, GetLCIDFromCultureString(null, true));
            return uri;
        }

        public static string AppendFwlinkLynxCulture(string uri)
        {
            string lynxCulture = FeatureEnablement.GetLynxCulture();
            if (!string.IsNullOrEmpty(lynxCulture))
                uri = AppendFwlinkCulture(uri, lynxCulture);
            return uri;
        }

        public static string AppendLynxCultureQueryString(string uri) => AppendLynxCultureQueryString(uri, false);

        public static string AppendLynxCultureQueryString(string uri, bool first)
        {
            string lynxCulture = FeatureEnablement.GetLynxCulture();
            if (string.IsNullOrEmpty(lynxCulture))
                return uri;
            return first ? string.Format("{0}?culture={1}", uri, lynxCulture) : string.Format("{0}&culture={1}", uri, lynxCulture);
        }

        internal static string GetDefaultCountry() => FeatureEnablement.GetRegion();

        internal static string GetDefaultLanguage()
        {
            string str = null;
            CultureInfo currentUiCulture = CultureInfo.CurrentUICulture;
            if (currentUiCulture != null)
                str = currentUiCulture.TwoLetterISOLanguageName;
            return str;
        }

        public static CultureInfo GetCulture(string cultureString)
        {
            CultureInfo cultureInfo = null;
            if (!string.IsNullOrEmpty(cultureString))
            {
                try
                {
                    cultureInfo = new CultureInfo(cultureString, false);
                }
                catch
                {
                }
            }
            if (cultureInfo == null)
                cultureInfo = CultureInfo.CurrentCulture;
            return cultureInfo;
        }
    }
}
