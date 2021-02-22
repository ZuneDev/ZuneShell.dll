// Decompiled with JetBrains decompiler
// Type: ZuneXml.XmlDataProviderObjectFactory
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections.Generic;

namespace ZuneXml
{
    public class XmlDataProviderObjectFactory
    {
        private static IDictionary<object, ConstructObject> _mapMarkupCookieToConstructor;
        private static IDictionary<string, object> _mapTypeNameToMarkupCookie;

        public static void ClearBindings() => XmlDataProviderObjectFactory._mapMarkupCookieToConstructor = (IDictionary<object, ConstructObject>)null;

        public static void Bind(string typeName, object markupTypeCookie)
        {
            if (XmlDataProviderObjectFactory._mapTypeNameToMarkupCookie == null)
                XmlDataProviderObjectFactory._mapTypeNameToMarkupCookie = (IDictionary<string, object>)new Dictionary<string, object>(40);
            XmlDataProviderObjectFactory._mapTypeNameToMarkupCookie[typeName] = markupTypeCookie;
        }

        private static void RegisterTypeConstructor(string typeName, ConstructObject constructor)
        {
            object key = XmlDataProviderObjectFactory._mapTypeNameToMarkupCookie[typeName];
            if (XmlDataProviderObjectFactory._mapMarkupCookieToConstructor == null)
                XmlDataProviderObjectFactory._mapMarkupCookieToConstructor = (IDictionary<object, ConstructObject>)new Dictionary<object, ConstructObject>(40);
            XmlDataProviderObjectFactory._mapMarkupCookieToConstructor[key] = constructor;
        }

        internal static ConstructObject GetConstructor(object objectTypeCookie)
        {
            if (XmlDataProviderObjectFactory._mapMarkupCookieToConstructor == null)
            {
                XmlDataProviderObjectFactory.RegisterTypeConstructors();
                XmlDataProviderObjectFactory._mapTypeNameToMarkupCookie = (IDictionary<string, object>)null;
            }
            ConstructObject constructObject = (ConstructObject)null;
            if (XmlDataProviderObjectFactory._mapMarkupCookieToConstructor.ContainsKey(objectTypeCookie))
                constructObject = XmlDataProviderObjectFactory._mapMarkupCookieToConstructor[objectTypeCookie];
            return constructObject;
        }

        internal static XmlDataProviderObject CreateObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            ConstructObject constructor = XmlDataProviderObjectFactory.GetConstructor(objectTypeCookie);
            return constructor != null ? constructor(owner, objectTypeCookie) : new XmlDataProviderObject(owner, objectTypeCookie);
        }

        internal static void RegisterTypeConstructors()
        {
            XmlDataProviderObjectFactory.RegisterTypeConstructor("BadgeData", new ConstructObject(BadgeData.ConstructBadgeDataObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("TrackPurchaseHistory", new ConstructObject(TrackPurchaseHistory.ConstructTrackPurchaseHistoryObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("AppCapabilities", new ConstructObject(AppCapabilities.ConstructAppCapabilitiesObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("Artist", new ConstructObject(Artist.ConstructArtistObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("PlaylistTrack", new ConstructObject(PlaylistTrack.ConstructPlaylistTrackObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("ZuneHDApp", new ConstructObject(ZuneHDApp.ConstructZuneHDAppObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("MessageDetails", new ConstructObject(MessageDetails.ConstructMessageDetailsObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("Season", new ConstructObject(Season.ConstructSeasonObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("MusicVideo", new ConstructObject(MusicVideo.ConstructMusicVideoObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("MediaInstance", new ConstructObject(MediaInstance.ConstructMediaInstanceObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("SeriesCategory", new ConstructObject(SeriesCategory.ConstructSeriesCategoryObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("RecommendedTrack", new ConstructObject(RecommendedTrack.ConstructRecommendedTrackObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("AppCapability", new ConstructObject(AppCapability.ConstructAppCapabilityObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("MovieGenre", new ConstructObject(MovieGenre.ConstructMovieGenreObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("MarketplaceRadioStation", new ConstructObject(MarketplaceRadioStation.ConstructMarketplaceRadioStationObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("RecommendedAlbum", new ConstructObject(RecommendedAlbum.ConstructRecommendedAlbumObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("Episode", new ConstructObject(Episode.ConstructEpisodeObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("AppMediaRights", new ConstructObject(AppMediaRights.ConstructAppMediaRightsObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("Movie", new ConstructObject(Movie.ConstructMovieObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("Track", new ConstructObject(Track.ConstructTrackObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("Right", new ConstructObject(Right.ConstructRightObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("Album", new ConstructObject(Album.ConstructAlbumObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("MovieStudio", new ConstructObject(MovieStudio.ConstructMovieStudioObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("Review", new ConstructObject(Review.ConstructReviewObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("AppGenre", new ConstructObject(AppGenre.ConstructAppGenreObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("ReviewListEntry", new ConstructObject(ReviewListEntry.ConstructReviewListEntryObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("ChannelReason", new ConstructObject(ChannelReason.ConstructChannelReasonObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("MiniAlbum", new ConstructObject(MiniAlbum.ConstructMiniAlbumObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("ZuneHDAppData", new ConstructObject(ZuneHDAppData.ConstructZuneHDAppDataObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("ArtistEvent", new ConstructObject(ArtistEvent.ConstructArtistEventObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("AppData", new ConstructObject(AppData.ConstructAppDataObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("WinPhoneAppPurchaseHistory", new ConstructObject(WinPhoneAppPurchaseHistory.ConstructWinPhoneAppPurchaseHistoryObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("Series", new ConstructObject(Series.ConstructSeriesObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("VideoCategory", new ConstructObject(VideoCategory.ConstructVideoCategoryObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("MessageRoot", new ConstructObject(MessageRoot.ConstructMessageRootObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("App", new ConstructObject(App.ConstructAppObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("Genre", new ConstructObject(Genre.ConstructGenreObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("VideoHistory", new ConstructObject(VideoHistory.ConstructVideoHistoryObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("WinPhoneAppGenre", new ConstructObject(WinPhoneAppGenre.ConstructWinPhoneAppGenreObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("ChannelTrack", new ConstructObject(ChannelTrack.ConstructChannelTrackObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("ZuneHDAppGenre", new ConstructObject(ZuneHDAppGenre.ConstructZuneHDAppGenreObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("ProfileTrack", new ConstructObject(ProfileTrack.ConstructProfileTrackObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("WinPhoneAppData", new ConstructObject(WinPhoneAppData.ConstructWinPhoneAppDataObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("TrackDownloadHistory", new ConstructObject(TrackDownloadHistory.ConstructTrackDownloadHistoryObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("PodcastSeries", new ConstructObject(PodcastSeries.ConstructPodcastSeriesObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("MiniArtist", new ConstructObject(MiniArtist.ConstructMiniArtistObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("WinPhoneApp", new ConstructObject(WinPhoneApp.ConstructWinPhoneAppObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("Mood", new ConstructObject(Mood.ConstructMoodObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("MediaRights", new ConstructObject(MediaRights.ConstructMediaRightsObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("Reason", new ConstructObject(Reason.ConstructReasonObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("AppScreenshot", new ConstructObject(AppScreenshot.ConstructAppScreenshotObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("ZplTrack", new ConstructObject(ZplTrack.ConstructZplTrackObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("Network", new ConstructObject(Network.ConstructNetworkObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("RecommendedArtist", new ConstructObject(RecommendedArtist.ConstructRecommendedArtistObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("MovieTrailer", new ConstructObject(MovieTrailer.ConstructMovieTrailerObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("ArtistEventList", new ConstructObject(ArtistEventList.ConstructArtistEventListObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("Short", new ConstructObject(Short.ConstructShortObject));
            XmlDataProviderObjectFactory.RegisterTypeConstructor("Contributor", new ConstructObject(Contributor.ConstructContributorObject));
        }
    }
}
