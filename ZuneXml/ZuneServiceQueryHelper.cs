// Decompiled with JetBrains decompiler
// Type: ZuneXml.ZuneServiceQueryHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Text;

namespace ZuneXml
{
    internal class ZuneServiceQueryHelper
    {
        private ZuneServiceQuery _query;

        internal static ZuneServiceQueryHelper ConstructZuneServiceQueryHelper(
          ZuneServiceQuery query)
        {
            return new ZuneServiceQueryHelper(query);
        }

        internal ZuneServiceQueryHelper(ZuneServiceQuery query) => this._query = query;

        internal ZuneServiceQuery Query => this._query;

        internal virtual string GetResourceUri() => this.Query.GetProperty("URI") as string;

        internal virtual object GetComputedProperty(string propertyName) => (object)null;

        internal virtual string GetQueryPostBody() => (string)null;

        internal virtual bool HandleQueryBeginExecute() => false;

        internal virtual void OnQueryPropertyChanged(string propertyName)
        {
        }

        internal virtual bool OnQueryFilterDataProviderObject(XmlDataProviderObject dataObject) => false;

        protected static void AppendParam(
          StringBuilder requestUri,
          string name,
          string value,
          ref bool fFirst)
        {
            requestUri.Append(fFirst ? "?" : "&");
            requestUri.Append(name);
            requestUri.Append("=");
            requestUri.Append(value);
            fFirst = false;
        }
    }
}
