// Decompiled with JetBrains decompiler
// Type: ZuneXml.XmlDataProviders
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneXml
{
    internal static class XmlDataProviders
    {
        internal static void Register()
        {
            Application.RegisterDataProvider("ZuneService", new DataProviderQueryFactory(ZuneServiceQuery.ConstructZuneServiceQuery));
            Application.RegisterDataProvider("WMISFAI", new DataProviderQueryFactory(WMISServiceDataProviderQuery.ConstructWmisQuery));
            Application.RegisterDataProvider("InboxImage", new DataProviderQueryFactory(InboxImageQuery.ConstructQuery));
        }
    }
}
