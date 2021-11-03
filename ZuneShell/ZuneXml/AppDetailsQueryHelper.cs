// Decompiled with JetBrains decompiler
// Type: ZuneXml.AppDetailsQueryHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Text;

namespace ZuneXml
{
    internal class AppDetailsQueryHelper : CatalogServiceQueryHelper
    {
        internal static AppDetailsQueryHelper ConstructAppDetailsQueryHelper(
          ZuneServiceQuery query)
        {
            return new AppDetailsQueryHelper(query);
        }

        internal AppDetailsQueryHelper(ZuneServiceQuery query)
          : base(query)
        {
        }

        protected override void AppendStuffAfterRepresentation(
          StringBuilder requestUri,
          ref bool fFirst)
        {
            base.AppendStuffAfterRepresentation(requestUri, ref fFirst);
            string property = (string)this.Query.GetProperty("Version");
            if (string.IsNullOrEmpty(property))
                return;
            AppendParam(requestUri, "version", property, ref fFirst);
        }
    }
}
