// Decompiled with JetBrains decompiler
// Type: ZuneXml.WebRequestHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using System;
using System.Globalization;
using System.Text;

namespace ZuneXml
{
    internal class WebRequestHelper
    {
        private static string AcceptLanguage;

        private static void SetCommonHeaders(
          HttpWebRequest request,
          EPassportPolicyId passportTicketType,
          HttpRequestCachePolicy cachePolicy,
          bool acceptGZipEncoding)
        {
            request.CachePolicy = cachePolicy;
            if (passportTicketType != EPassportPolicyId.None)
            {
                string passportTicket = ZuneApplication.Service.GetPassportTicket(passportTicketType);
                if (!string.IsNullOrEmpty(passportTicket))
                    request.Authorization = "WLID1.0 " + passportTicket;
            }
            request.AcceptGZipEncoding = acceptGZipEncoding;
            request.AcceptLanguage = Locale;
        }

        public static HttpWebRequest ConstructWebRequest(
          string requestUri,
          EPassportPolicyId passportTicketType,
          HttpRequestCachePolicy cachePolicy,
          bool fKeepAlive,
          bool acceptGZipEncoding)
        {
            string absoluteUri = new Uri(requestUri).AbsoluteUri;
            if (absoluteUri != requestUri)
            {
                int num = TraceSwitches.DataProviderSwitch.TraceWarning ? 1 : 0;
                requestUri = absoluteUri;
            }
            if (cachePolicy == HttpRequestCachePolicy.Default && UriResourceTracker.Instance.IsResourceModified(requestUri))
            {
                cachePolicy = HttpRequestCachePolicy.Refresh;
                UriResourceTracker.Instance.SetResourceModified(requestUri, false);
            }
            HttpWebRequest request = HttpWebRequest.Create(requestUri);
            request.KeepAlive = fKeepAlive;
            request.CancelOnShutdown = true;
            SetCommonHeaders(request, passportTicketType, cachePolicy, acceptGZipEncoding);
            int num1 = TraceSwitches.DataProviderSwitch.TraceWarning ? 1 : 0;
            return request;
        }

        public static HttpWebRequest ConstructWebPostRequest(
          string requestUri,
          string requestBody,
          EPassportPolicyId passportTicketType,
          HttpRequestCachePolicy cachePolicy,
          bool fKeepAlive,
          bool acceptGZipEncoding)
        {
            HttpWebRequest request = HttpWebRequest.Create(requestUri);
            request.KeepAlive = fKeepAlive;
            request.CachePolicy = cachePolicy;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            SetCommonHeaders(request, passportTicketType, cachePolicy, acceptGZipEncoding);
            byte[] bytes = new ASCIIEncoding().GetBytes(requestBody);
            request.ContentLength = bytes.Length;
            request.GetRequestStream().Write(bytes, 0, bytes.Length);
            int num = TraceSwitches.DataProviderSwitch.TraceWarning ? 1 : 0;
            return request;
        }

        public static string Locale
        {
            get
            {
                if (AcceptLanguage == null)
                    AcceptLanguage = CultureInfo.CurrentUICulture.Name;
                return AcceptLanguage;
            }
        }
    }
}
