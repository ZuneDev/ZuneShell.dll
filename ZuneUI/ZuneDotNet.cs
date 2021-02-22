// Decompiled with JetBrains decompiler
// Type: ZuneUI.ZuneDotNet
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;
using System;
using System.Collections;

namespace ZuneUI
{
    public static class ZuneDotNet
    {
        public static string GetViewTrackUri(string trackId) => ZuneDotNet.GetViewUri(trackId, "track");

        public static void ViewTrack(Guid trackId) => ZuneShell.DefaultInstance.Execute(ZuneDotNet.GetViewTrackUri(trackId.ToString()), (IDictionary)null);

        public static string GetViewAlbumUri(string albumId) => ZuneDotNet.GetViewUri(albumId, "album");

        public static void ViewAlbum(Guid albumId) => ZuneShell.DefaultInstance.Execute(ZuneDotNet.GetViewAlbumUri(albumId.ToString()), (IDictionary)null);

        public static string GetViewArtistUri(string artistId) => ZuneDotNet.GetViewUri(artistId, "artist");

        public static void ViewArtist(Guid artistId) => ZuneShell.DefaultInstance.Execute(ZuneDotNet.GetViewArtistUri(artistId.ToString()), (IDictionary)null);

        public static string GetViewPodcastSeriesUri(string podcastSeriesId) => ZuneDotNet.GetViewUri(podcastSeriesId, "podcastSeries");

        public static void ViewPodcastSeries(Guid podcastSeriesId) => ZuneShell.DefaultInstance.Execute(ZuneDotNet.GetViewPodcastSeriesUri(podcastSeriesId.ToString()), (IDictionary)null);

        private static string GetViewUri(string id, string type)
        {
            string endPointUri = Microsoft.Zune.Service.Service.GetEndPointUri(Microsoft.Zune.Service.EServiceEndpointId.SEID_Lynx);
            string str = FeatureEnablement.IsFeatureEnabled(Features.eSocial) ? "View" : "ViewUnsupportedMarket";
            return "Web\\" + UrlHelper.MakeUrlEx(endPointUri + "/redirect", nameof(type), type, nameof(id), id, "target", "web", "action", str);
        }
    }
}
