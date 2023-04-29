#if OPENZUNE

using Microsoft.Iris;
using Microsoft.Zune.Playlist;
using StrixMusic.Sdk.AppModels;
using System;
using System.Threading;

namespace Microsoft.Zune.Library
{
	public class StrixLibraryDataProviderQuery : DataProviderQuery
	{
		private LibraryVirtualList m_virtualListResultSet;
		private bool m_disposed = false;
		private int m_requestGeneration;
		private string m_thumbnailFallbackImageUrl;

		//public static IStrixDataRoot Root {get; set;}

		public StrixLibraryDataProviderQuery(object typeCookie) : base(typeCookie)
        {
        }

        protected override void BeginExecute()
        {
			LibraryVirtualList virtualListResultSet = this.m_virtualListResultSet;
			if (virtualListResultSet != null)
			{
				((IDisposable)virtualListResultSet).Dispose();
				this.m_virtualListResultSet = null;
			}
			this.m_thumbnailFallbackImageUrl = null;
			this.m_requestGeneration++;
			this.Status = DataProviderQueryStatus.RequestingData;
			BeginExecuteWorker(m_requestGeneration);
		}

        private void BeginExecuteWorker(object state)
        {
			bool[] sortAscendings = null;
			string[] sortStrings = null;
			string str = null;
			int num = (int)state;
			if (true)//num == this.m_requestGeneration)
			{
				//QueryPropertyBag queryPropertyBag = new QueryPropertyBag();
				//IQueryPropertyBag* iqueryPropertyBag = queryPropertyBag.GetIQueryPropertyBag();
				//bool retainedList = false;
				//if (this.GetSortAttributes(out sortStrings, out sortAscendings))
				//{

				//}
				object property = this.GetProperty("ArtistIds");
				if (property != null)
				{

				}
				object property2 = this.GetProperty("GenreIds");
				if (property2 != null)
				{

				}
				object property3 = this.GetProperty("AlbumIds");
				if (property3 != null)
				{

				}
				object property4 = this.GetProperty("UserCardIds");
				if (property4 != null)
				{

				}
				object property5 = this.GetProperty("DeviceId");
				if (property5 != null)
				{

				}
				else
				{

				}
				object property6 = this.GetProperty("SyncMappedError");
				if (property6 != null)
				{

				}
				property6 = this.GetProperty("UserId");
				if (property6 != null)
				{

				}
				else
				{

				}
				object property7 = this.GetProperty("InLibrary");
				if (property7 != null)
				{

				}
				//object obj13 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvCdecl)(System.IntPtr, EQueryPropertyBagProp, System.Int32), iqueryPropertyBag, 2, 0, *(*(long*)iqueryPropertyBag + 56L));
				EQueryTypeView equeryTypeView = EQueryTypeView.eQueryTypeLibraryView;
				object property8 = this.GetProperty("ShowDeviceContents");
				if (property8 != null)
				{
					equeryTypeView = (((bool)property8) ? EQueryTypeView.eQueryTypeDeviceView : equeryTypeView);
				}
				property8 = this.GetProperty("DiscMediaView");
				if (property8 != null)
				{
					equeryTypeView = (((bool)property8) ? EQueryTypeView.eQueryTypeDiscMediaView : equeryTypeView);
				}
				property8 = this.GetProperty("Remaining");
				if (property8 != null)
				{
					equeryTypeView = (((bool)property8) ? EQueryTypeView.eQueryTypeSyncRemaining : equeryTypeView);
				}
				property8 = this.GetProperty("Complete");
				if (property8 != null)
				{
					equeryTypeView = (((bool)property8) ? EQueryTypeView.eQueryTypeSyncSucceeded : equeryTypeView);
				}
				property8 = this.GetProperty("Failed");
				if (property8 != null)
				{
					equeryTypeView = (((bool)property8) ? EQueryTypeView.eQueryTypeSyncFailed : equeryTypeView);
				}
				property8 = this.GetProperty("MultiSelect");
				if (property8 != null && (bool)property8)
				{
					equeryTypeView = ((EQueryTypeView.eQueryTypeDeviceView == equeryTypeView) ? EQueryTypeView.eQueryTypeDeviceMultiSelectView : EQueryTypeView.eQueryTypeLibraryMultiSelectView);
				}
				object property9 = this.GetProperty("RulesOnly");
				if (property9 != null)
				{
					equeryTypeView = (((bool)property9) ? EQueryTypeView.eQueryTypeDeviceSyncRuleView : equeryTypeView);
				}
				//object obj14 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvCdecl)(System.IntPtr, EQueryPropertyBagProp, System.Int32), iqueryPropertyBag, 15, equeryTypeView, *(*(long*)iqueryPropertyBag + 56L));
				object keywordsProp = this.GetProperty("Keywords");
				if (keywordsProp != null)
				{

				}
				object property11 = this.GetProperty("ContributingArtistId");
				if (property11 != null)
				{

				}
				property11 = this.GetProperty("ArtistId");
				if (property11 != null)
				{

				}
				property11 = this.GetProperty("GenreId");
				if (property11 != null)
				{

				}
				property11 = this.GetProperty("AlbumId");
				if (property11 != null)
				{

				}
				property11 = this.GetProperty("FolderId");
				if (property11 != null)
				{

				}
				property11 = this.GetProperty("RecurseIntoFolders");
				if (property11 != null && (int)property11 != 0)
				{

				}
				object property12 = this.GetProperty("FolderMediaType");
				if (property12 != null)
				{
					//EMediaTypes emediaTypes = LibraryDataProvider.NameToMediaType((string)property12);

				}
				object property13 = this.GetProperty("MediaType");
				if (property13 != null)
				{

				}
				object property14 = this.GetProperty("TOC");
				if (property14 != null)
				{

				}
				object property15 = this.GetProperty("SeriesId");
				if (property15 != null)
				{

				}
				property15 = this.GetProperty("WatchType");
				if (property15 != null)
				{

				}
				if (this.GetProperty("ExpiresOnly") != null)
				{

				}
				object property16 = this.GetProperty("Operation");
				if (property16 != null)
				{

				}
				property16 = this.GetProperty("InitTime");
				if (property16 != null)
				{

				}
				object property17 = this.GetProperty("PlaylistId");
				if (property17 != null)
				{

				}
				property17 = this.GetProperty("CategoryId");
				if (property17 != null)
				{

				}
				property17 = this.GetProperty("PlaylistType");
				if (property17 != null && !string.IsNullOrEmpty((string)property17))
				{
					PlaylistType playlistType = (PlaylistType)Enum.Parse(typeof(PlaylistType), (string)property17);
					//object obj32 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvCdecl)(System.IntPtr, EQueryPropertyBagProp, System.Int32), iqueryPropertyBag, 24, playlistType, *(*(long*)iqueryPropertyBag + 56L));
				}
				object property18 = this.GetProperty("PlaylistTypeMask");
				if (property18 != null)
				{
					int num9 = (int)property18;
					if (num9 != 0)
					{

					}
				}
				object property19 = this.GetProperty("MaxResultCount");
				if (property19 != null)
				{

				}
				property19 = this.GetProperty("DrmStateMask");
				if (property19 != null)
				{
					ulong num10 = (ulong)((long)property19);
					if (num10 != 0UL)
					{

					}
				}
				string text = (string)this.GetProperty("QueryType");
				EQueryType equeryType = EQueryType.eQueryTypeInvalid;
				if (keywordsProp != null)//queryPropertyBag.IsSet("Keywords"))
				{
					if (text == "Artist")
					{
						equeryType = EQueryType.eQueryTypeArtistsWithKeyword;
					}
					else if (text == "Album")
					{
						equeryType = EQueryType.eQueryTypeAlbumsWithKeyword;
					}
					else if (text == "Track")
					{
						equeryType = EQueryType.eQueryTypeTracksWithKeyword;
					}
					else if (text == "Playlist")
					{
						equeryType = EQueryType.eQueryTypePlaylistsWithKeyword;
					}
					else if (text == "Photo")
					{
						equeryType = EQueryType.eQueryTypePhotosWithKeyword;
					}
					else if (text == "PodcastSeries")
					{
						equeryType = EQueryType.eQueryTypeSubscriptionsSeriesWithKeyword;
					}
					else if (text == "PodcastEpisode")
					{
						equeryType = EQueryType.eQueryTypeSubscriptionsEpisodesWithKeyword;
					}
					else if (text == "Video")
					{
						equeryType = EQueryType.eQueryTypeVideoWithKeyword;
					}
				}
				else if (text == "Artist")
				{
					equeryType = EQueryType.eQueryTypeAllAlbumArtists;
				}
				else if (text == "Genres")
				{
					object property20 = this.GetProperty("MediaType");
					int num11;
					if (property20 != null)
					{
						num11 = (int)property20;
					}
					else
					{
						num11 = 3;
					}

					equeryType = EQueryType.eQueryTypeAllGenres;
				}
				else if (text == "Album")
				{
					property19 = this.GetProperty("ArtistId");
					//if ((property19 != null && (int)property19 != -1) || queryPropertyBag.IsSet("ArtistIds"))
					//{
					//	equeryType = EQueryType.eQueryTypeAlbumsForAlbumArtistId;
					//}
					//else
					//{
					//	property19 = this.GetProperty("GenreId");
					//	if ((property19 != null && (int)property19 != -1) || queryPropertyBag.IsSet("GenreIds"))
					//	{
					//		equeryType = EQueryType.eQueryTypeAlbumsByGenreId;
					//	}
					//	else
					//	{
					//		equeryType = EQueryType.eQueryTypeAllAlbums;
					//	}
					//}
					//retainedList = true;
				}
				else if (text == "Track")
				{
					object property21 = this.GetProperty("RulesOnly");
					if (property21 != null)
					{
						bool flag = (bool)property21;
						if (flag)
						{
							equeryType = EQueryType.eQueryTypeAllTracks;
							//goto IL_BDF;
						}
					}
					property21 = this.GetProperty("AlbumId");
					//if ((property21 != null && (int)property21 != -1) || queryPropertyBag.IsSet("AlbumIds"))
					//{
					//	property19 = this.GetProperty("ArtistId");
					//	if ((property19 != null && (int)property19 != -1) || queryPropertyBag.IsSet("ArtistIds"))
					//	{
					//		equeryType = EQueryType.eQueryTypeTracksForAlbumArtistId;
					//	}
					//	else
					//	{
					//		equeryType = EQueryType.eQueryTypeTracksForAlbumId;
					//	}
					//}
					//else
					//{
					//	property19 = this.GetProperty("ArtistId");
					//	if ((property19 != null && (int)property19 != -1) || queryPropertyBag.IsSet("ArtistIds"))
					//	{
					//		equeryType = EQueryType.eQueryTypeTracksForAlbumArtistId;
					//	}
					//	else
					//	{
					//		property19 = this.GetProperty("GenreId");
					//		if ((property19 != null && (int)property19 != -1) || queryPropertyBag.IsSet("GenreIds"))
					//		{
					//			equeryType = EQueryType.eQueryTypeTracksByGenreId;
					//		}
					//		else
					//		{
					//			property19 = this.GetProperty("Detailed");
					//			if (property19 != null && (bool)property19)
					//			{
					//				equeryType = EQueryType.eQueryTypeAllTracksDetailed;
					//			}
					//			else
					//			{
					//				property19 = this.GetProperty("TOC");
					//				if (property19 != null && !string.IsNullOrEmpty((string)property19))
					//				{
					//					equeryType = EQueryType.eQueryTypeTracksForTOC;
					//				}
					//				else
					//				{
					//					equeryType = EQueryType.eQueryTypeAllTracks;
					//				}
					//			}
					//		}
					//	}
					//}
				}
				else if (text == "AlbumByTOC")
				{
					equeryType = EQueryType.eQueryTypeAlbumsByTOC;
					//if (!queryPropertyBag.IsSet("TOC"))
					//{
					//	equeryType = EQueryType.eQueryTypeInvalid;
					//}
				}
				else if (text == "Photo")
				{
					equeryType = ((equeryTypeView == EQueryTypeView.eQueryTypeDeviceSyncRuleView) ? EQueryType.eQueryTypeAllPhotos : EQueryType.eQueryTypePhotosByFolderId);
				}
				else if (text == "MediaFolder")
				{
					equeryType = EQueryType.eQueryTypeMediaFolders;
				}
				else if (text == "Video")
				{
					property19 = this.GetProperty("CategoryId");
					if (property19 != null && (int)property19 != -1)
					{
						equeryType = EQueryType.eQueryTypeVideosByCategoryId;
					}
					else
					{
						equeryType = EQueryType.eQueryTypeAllVideos;
					}
				}
				else if (text == "PodcastSeries")
				{
					equeryType = EQueryType.eQueryTypeAllPodcastSeries;
				}
				else if (text == "PodcastEpisode")
				{
					property19 = this.GetProperty("SeriesId");
					if (property19 != null && (int)property19 != -1)
					{
						equeryType = EQueryType.eQueryTypeEpisodesForSeriesId;
					}
					else
					{
						equeryType = EQueryType.eQueryTypeAllPodcastEpisodes;
					}
				}
				else if (text == "SyncItem")
				{
					equeryType = EQueryType.eQueryTypeSyncProgress;
				}
				else if (text == "Playlist")
				{
					equeryType = EQueryType.eQueryTypeAllPlaylists;
				}
				else if (text == "PlaylistContent")
				{
					equeryType = EQueryType.eQueryTypePlaylistContentByPlaylistId;
				}
				else if (text == "UserCard")
				{
					equeryType = EQueryType.eQueryTypeUserCards;
				}
				else if (text == "Person")
				{
					equeryType = EQueryType.eQueryTypePersonsByTypeId;
					EMediaTypes emediaTypes2 = EMediaTypes.eMediaTypePersonArtist;
					emediaTypes2 = (((string)this.GetProperty("PersonType") == "Composer") ? EMediaTypes.eMediaTypePersonComposer : emediaTypes2);

				}
				else if (text == "ArtistsRanking")
				{
					equeryType = EQueryType.eQueryTypeArtistsRanking;
				}
				else if (text == "TVSeries")
				{
					equeryType = EQueryType.eQueryTypeVideoSeriesTitles;
				}
				else if (text == "Pin")
				{
					equeryType = EQueryType.eQueryTypePinsByPinType;
					EPinType epinType = EPinType.ePinTypeGeneric;
					property19 = this.GetProperty("PinType");
					if (property19 != null)
					{
						epinType = (EPinType)property19;
					}

				}
				else
				{
					equeryType = ((text == "App") ? EQueryType.eQueryTypeAllApps : equeryType);
				}


				ZuneQueryList queryList = new(equeryType);
				DeferredSetResult(new DeferredSetResultArgs(num, queryList, false));// retainedList));
			}
		}

        private void DeferredSetResult(object state)
        {
			DeferredSetResultArgs deferredSetResultArgs = (DeferredSetResultArgs)state;
			bool isEmpty = true;
			if (deferredSetResultArgs.QueryList != null)
			{
				object property = this.GetProperty("AutoRefresh");
				bool autoRefresh = property == null || (bool)property;
				object property2 = this.GetProperty("AntialiasImageEdges");
				bool antialiasEdges = property2 != null && (bool)property2;
				this.m_thumbnailFallbackImageUrl = (string)this.GetProperty("ThumbnailFallbackImageUrl");
				LibraryVirtualList virtualListResultSet = this.m_virtualListResultSet;
				if (virtualListResultSet != null)
				{
					((IDisposable)virtualListResultSet).Dispose();
				}
				LibraryVirtualList libraryVirtualList = new(this, deferredSetResultArgs.QueryList, autoRefresh, antialiasEdges);
				this.m_virtualListResultSet = libraryVirtualList;
				ReleaseBehavior visualReleaseBehavior = deferredSetResultArgs.RetainedList ? ReleaseBehavior.KeepReference : ReleaseBehavior.ReleaseReference;
				libraryVirtualList.VisualReleaseBehavior = visualReleaseBehavior;
				isEmpty = deferredSetResultArgs.QueryList.IsEmpty;
			}
			StrixLibraryDataProviderQueryResult libraryDataProviderQueryResult = new(this, this.m_virtualListResultSet, base.ResultTypeCookie);
			libraryDataProviderQueryResult.SetIsEmpty(isEmpty);
			this.Result = libraryDataProviderQueryResult;
			this.Status = DataProviderQueryStatus.Complete;
		}
    }

	internal class DeferredSetResultArgs
	{
		public DeferredSetResultArgs(int requestGeneration, ZuneQueryList queryList, bool retainedList)
		{
			RequestGeneration = requestGeneration;
			QueryList = queryList;
			RetainedList = retainedList;
		}

		public int RequestGeneration { get; }

		public ZuneQueryList QueryList { get; }

		public bool RetainedList { get; }
	}
}

#endif