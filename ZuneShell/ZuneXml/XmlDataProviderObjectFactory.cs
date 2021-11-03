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

        public static void ClearBindings() => _mapMarkupCookieToConstructor = null;

        public static void Bind(string typeName, object markupTypeCookie)
        {
            if (_mapTypeNameToMarkupCookie == null)
                _mapTypeNameToMarkupCookie = new Dictionary<string, object>(40);
            _mapTypeNameToMarkupCookie[typeName] = markupTypeCookie;
        }

        private static void RegisterTypeConstructor(string typeName, ConstructObject constructor)
        {
            object key = _mapTypeNameToMarkupCookie[typeName];
            if (_mapMarkupCookieToConstructor == null)
                _mapMarkupCookieToConstructor = new Dictionary<object, ConstructObject>(40);
            _mapMarkupCookieToConstructor[key] = constructor;
        }

        internal static ConstructObject GetConstructor(object objectTypeCookie)
        {
            if (_mapMarkupCookieToConstructor == null)
            {
                RegisterTypeConstructors();
                _mapTypeNameToMarkupCookie = null;
            }
            ConstructObject constructObject = null;
            if (_mapMarkupCookieToConstructor.ContainsKey(objectTypeCookie))
                constructObject = _mapMarkupCookieToConstructor[objectTypeCookie];
            return constructObject;
        }

        internal static XmlDataProviderObject CreateObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            ConstructObject constructor = GetConstructor(objectTypeCookie);
            return constructor != null ? constructor(owner, objectTypeCookie) : new XmlDataProviderObject(owner, objectTypeCookie);
        }

        internal static void RegisterTypeConstructors()
        {
            RegisterTypeConstructor("BadgeData", new ConstructObject(BadgeData.ConstructBadgeDataObject));
            RegisterTypeConstructor("TrackPurchaseHistory", new ConstructObject(TrackPurchaseHistory.ConstructTrackPurchaseHistoryObject));
            RegisterTypeConstructor("AppCapabilities", new ConstructObject(AppCapabilities.ConstructAppCapabilitiesObject));
            RegisterTypeConstructor("Artist", new ConstructObject(Artist.ConstructArtistObject));
            RegisterTypeConstructor("PlaylistTrack", new ConstructObject(PlaylistTrack.ConstructPlaylistTrackObject));
            RegisterTypeConstructor("ZuneHDApp", new ConstructObject(ZuneHDApp.ConstructZuneHDAppObject));
            RegisterTypeConstructor("MessageDetails", new ConstructObject(MessageDetails.ConstructMessageDetailsObject));
            RegisterTypeConstructor("Season", new ConstructObject(Season.ConstructSeasonObject));
            RegisterTypeConstructor("MusicVideo", new ConstructObject(MusicVideo.ConstructMusicVideoObject));
            RegisterTypeConstructor("MediaInstance", new ConstructObject(MediaInstance.ConstructMediaInstanceObject));
            RegisterTypeConstructor("SeriesCategory", new ConstructObject(SeriesCategory.ConstructSeriesCategoryObject));
            RegisterTypeConstructor("RecommendedTrack", new ConstructObject(RecommendedTrack.ConstructRecommendedTrackObject));
            RegisterTypeConstructor("AppCapability", new ConstructObject(AppCapability.ConstructAppCapabilityObject));
            RegisterTypeConstructor("MovieGenre", new ConstructObject(MovieGenre.ConstructMovieGenreObject));
            RegisterTypeConstructor("MarketplaceRadioStation", new ConstructObject(MarketplaceRadioStation.ConstructMarketplaceRadioStationObject));
            RegisterTypeConstructor("RecommendedAlbum", new ConstructObject(RecommendedAlbum.ConstructRecommendedAlbumObject));
            RegisterTypeConstructor("Episode", new ConstructObject(Episode.ConstructEpisodeObject));
            RegisterTypeConstructor("AppMediaRights", new ConstructObject(AppMediaRights.ConstructAppMediaRightsObject));
            RegisterTypeConstructor("Movie", new ConstructObject(Movie.ConstructMovieObject));
            RegisterTypeConstructor("Track", new ConstructObject(Track.ConstructTrackObject));
            RegisterTypeConstructor("Right", new ConstructObject(Right.ConstructRightObject));
            RegisterTypeConstructor("Album", new ConstructObject(Album.ConstructAlbumObject));
            RegisterTypeConstructor("MovieStudio", new ConstructObject(MovieStudio.ConstructMovieStudioObject));
            RegisterTypeConstructor("Review", new ConstructObject(Review.ConstructReviewObject));
            RegisterTypeConstructor("AppGenre", new ConstructObject(AppGenre.ConstructAppGenreObject));
            RegisterTypeConstructor("ReviewListEntry", new ConstructObject(ReviewListEntry.ConstructReviewListEntryObject));
            RegisterTypeConstructor("ChannelReason", new ConstructObject(ChannelReason.ConstructChannelReasonObject));
            RegisterTypeConstructor("MiniAlbum", new ConstructObject(MiniAlbum.ConstructMiniAlbumObject));
            RegisterTypeConstructor("ZuneHDAppData", new ConstructObject(ZuneHDAppData.ConstructZuneHDAppDataObject));
            RegisterTypeConstructor("ArtistEvent", new ConstructObject(ArtistEvent.ConstructArtistEventObject));
            RegisterTypeConstructor("AppData", new ConstructObject(AppData.ConstructAppDataObject));
            RegisterTypeConstructor("WinPhoneAppPurchaseHistory", new ConstructObject(WinPhoneAppPurchaseHistory.ConstructWinPhoneAppPurchaseHistoryObject));
            RegisterTypeConstructor("Series", new ConstructObject(Series.ConstructSeriesObject));
            RegisterTypeConstructor("VideoCategory", new ConstructObject(VideoCategory.ConstructVideoCategoryObject));
            RegisterTypeConstructor("MessageRoot", new ConstructObject(MessageRoot.ConstructMessageRootObject));
            RegisterTypeConstructor("App", new ConstructObject(App.ConstructAppObject));
            RegisterTypeConstructor("Genre", new ConstructObject(Genre.ConstructGenreObject));
            RegisterTypeConstructor("VideoHistory", new ConstructObject(VideoHistory.ConstructVideoHistoryObject));
            RegisterTypeConstructor("WinPhoneAppGenre", new ConstructObject(WinPhoneAppGenre.ConstructWinPhoneAppGenreObject));
            RegisterTypeConstructor("ChannelTrack", new ConstructObject(ChannelTrack.ConstructChannelTrackObject));
            RegisterTypeConstructor("ZuneHDAppGenre", new ConstructObject(ZuneHDAppGenre.ConstructZuneHDAppGenreObject));
            RegisterTypeConstructor("ProfileTrack", new ConstructObject(ProfileTrack.ConstructProfileTrackObject));
            RegisterTypeConstructor("WinPhoneAppData", new ConstructObject(WinPhoneAppData.ConstructWinPhoneAppDataObject));
            RegisterTypeConstructor("TrackDownloadHistory", new ConstructObject(TrackDownloadHistory.ConstructTrackDownloadHistoryObject));
            RegisterTypeConstructor("PodcastSeries", new ConstructObject(PodcastSeries.ConstructPodcastSeriesObject));
            RegisterTypeConstructor("MiniArtist", new ConstructObject(MiniArtist.ConstructMiniArtistObject));
            RegisterTypeConstructor("WinPhoneApp", new ConstructObject(WinPhoneApp.ConstructWinPhoneAppObject));
            RegisterTypeConstructor("Mood", new ConstructObject(Mood.ConstructMoodObject));
            RegisterTypeConstructor("MediaRights", new ConstructObject(MediaRights.ConstructMediaRightsObject));
            RegisterTypeConstructor("Reason", new ConstructObject(Reason.ConstructReasonObject));
            RegisterTypeConstructor("AppScreenshot", new ConstructObject(AppScreenshot.ConstructAppScreenshotObject));
            RegisterTypeConstructor("ZplTrack", new ConstructObject(ZplTrack.ConstructZplTrackObject));
            RegisterTypeConstructor("Network", new ConstructObject(Network.ConstructNetworkObject));
            RegisterTypeConstructor("RecommendedArtist", new ConstructObject(RecommendedArtist.ConstructRecommendedArtistObject));
            RegisterTypeConstructor("MovieTrailer", new ConstructObject(MovieTrailer.ConstructMovieTrailerObject));
            RegisterTypeConstructor("ArtistEventList", new ConstructObject(ArtistEventList.ConstructArtistEventListObject));
            RegisterTypeConstructor("Short", new ConstructObject(Short.ConstructShortObject));
            RegisterTypeConstructor("Contributor", new ConstructObject(Contributor.ConstructContributorObject));
        }
    }
}
