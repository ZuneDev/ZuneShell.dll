// Decompiled with JetBrains decompiler
// Type: ZuneXml.LinkPageInfo
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneXml
{
    internal class LinkPageInfo : IPageInfo
    {
        private string _nextPageUrl;
        private string _requestBody;

        public LinkPageInfo(string nextPageUrl, string requestBody)
        {
            this._nextPageUrl = nextPageUrl;
            this._requestBody = requestBody;
        }

        public string GetPageUrl(int startIndex) => this._nextPageUrl;

        public string GetPagePostBody(int startIndex) => this._requestBody;
    }
}
