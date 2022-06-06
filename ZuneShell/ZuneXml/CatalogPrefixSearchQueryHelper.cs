// Decompiled with JetBrains decompiler
// Type: ZuneXml.CatalogPrefixSearchQueryHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;
using System;
using System.Text;
using ZuneUI;

namespace ZuneXml
{
    internal class CatalogPrefixSearchQueryHelper : CatalogServiceQueryHelper
    {
        internal static ZuneServiceQueryHelper ConstructPrefixSearchQueryHelper(
          ZuneServiceQuery query)
        {
            return new CatalogPrefixSearchQueryHelper(query);
        }

        internal CatalogPrefixSearchQueryHelper(ZuneServiceQuery query)
          : base(query)
        {
        }

        internal override string GetResourceUri()
        {
            string property1 = (string)this.Query.GetProperty("Prefix");
            if (!Search.Instance.IsValidKeyword(property1))
                return null;
            string endPointUri = Microsoft.Zune.Service.Service2.GetEndPointUri(this._endPoint);
            StringBuilder stringBuilder = new StringBuilder(128);
            stringBuilder.Append(endPointUri);
            stringBuilder.Append("/?prefix=");
            stringBuilder.Append(Uri.EscapeDataString(property1));
            if ((bool)this.Query.GetProperty("OnlyIncludeZuneRadioArtists"))
            {
                if (FeatureEnablement.IsFeatureEnabled(Features.eMusic))
                    stringBuilder.Append("&includeZuneRadioArtists=true");
            }
            else
            {
                if (FeatureEnablement.IsFeatureEnabled(Features.eMusic))
                {
                    stringBuilder.Append("&includeTracks=true");
                    stringBuilder.Append("&includeAlbums=true");
                    stringBuilder.Append("&includeArtists=true");
                }
                if (FeatureEnablement.IsFeatureEnabled(Features.eVideos))
                {
                    stringBuilder.Append("&includeMovies=true");
                    stringBuilder.Append("&includeVideoShorts=true");
                }
                if (FeatureEnablement.IsFeatureEnabled(Features.eTV))
                    stringBuilder.Append("&includeTVSeries=true");
                if (FeatureEnablement.IsFeatureEnabled(Features.eMusicVideos))
                    stringBuilder.Append("&includeMusicVideos=true");
                if (FeatureEnablement.IsFeatureEnabled(Features.ePodcasts))
                    stringBuilder.Append("&includePodcasts=true");
                if (FeatureEnablement.IsFeatureEnabled(Features.eApps))
                    stringBuilder.Append("&includeApplications=true");
            }
            string property2 = (string)this.Query.GetProperty("ClientType");
            if (!string.IsNullOrEmpty(property2))
            {
                stringBuilder.Append("&clientType=");
                stringBuilder.Append(property2);
            }
            return stringBuilder.ToString();
        }
    }
}
