// Decompiled with JetBrains decompiler
// Type: ZuneXml.DataProviderQueryHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Net;

namespace ZuneXml
{
    public class DataProviderQueryHelper
    {
        public static bool ResponseIsForbiddenOrUnauthorized(DataProviderQuery failedQuery)
        {
            if (!(failedQuery is XmlDataProviderQuery) || !(((XmlDataProviderQuery)failedQuery).ErrorCode is HttpStatusCode errorCode))
                return false;
            return errorCode == HttpStatusCode.Forbidden || errorCode == HttpStatusCode.Unauthorized;
        }

        public static bool ResponseIsNotFound(DataProviderQuery failedQuery) => failedQuery is XmlDataProviderQuery && ((XmlDataProviderQuery)failedQuery).ErrorCode is HttpStatusCode errorCode && errorCode == HttpStatusCode.NotFound;
    }
}
