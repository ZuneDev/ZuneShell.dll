using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Iris;
using Microsoft.Zune.Playlist;
using MicrosoftZuneInterop;

namespace MicrosoftZuneLibrary
{
	internal class LibraryDataProviderQuery : DataProviderQuery
	{
		protected LibraryVirtualList m_virtualListResultSet;

		private bool m_disposed = false;

		private int m_requestGeneration;

		private string m_thumbnailFallbackImageUrl;

		private static WorkerQueue m_libQueriesQueue = WorkerQueue.CreateInstance();

		public string ThumbnailFallbackImageUrl => m_thumbnailFallbackImageUrl;

		internal LibraryDataProviderQuery(object queryTypeCookie)
			: base(queryTypeCookie)
		{
			LibraryDataProviderQueryResult libraryDataProviderQueryResult = new LibraryDataProviderQueryResult(this, null, ResultTypeCookie);
			libraryDataProviderQueryResult.SetIsEmpty(isEmpty: true);
			Result = libraryDataProviderQueryResult;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public bool GetSortAttributes(out string[] sorts, out bool[] ascendings)
		{
			return LibraryDataProvider.GetSortAttributes((string)GetProperty("Sort"), out sorts, out ascendings);
		}

		protected unsafe override void BeginExecute()
		{
			if (m_disposed)
			{
				return;
			}
			if (Unsafe.As<EtwControlerState, byte>(ref Unsafe.AddByteOffset(ref Module.g_EtwControlerState, 8)) > 1u && ((uint)Unsafe.As<EtwControlerState, int>(ref Unsafe.AddByteOffset(ref Module.g_EtwControlerState, 4)) & 0x10u) != 0)
			{
				fixed (char* stringPtr = ToString().ToCharArray())
				{
					ushort* pwszDetail = (ushort*)stringPtr;
					try
					{
						Module.PERFTRACE_COLLECTIONEVENT((_COLLECTION_EVENT)28, pwszDetail);
					}
					catch
					{
						//try-fault
						pwszDetail = null;
						throw;
					}
				}
			}
			LibraryVirtualList virtualListResultSet = m_virtualListResultSet;
			if (virtualListResultSet != null)
			{
				((IDisposable)virtualListResultSet).Dispose();
				m_virtualListResultSet = null;
			}
			m_thumbnailFallbackImageUrl = null;
			m_requestGeneration++;
			Status = DataProviderQueryStatus.RequestingData;
			m_libQueriesQueue.QueueSequentialWorkItem(BeginExecuteWorker, m_requestGeneration);
		}

		protected override void OnDispose()
		{
			m_disposed = true;
			LibraryVirtualList virtualListResultSet = m_virtualListResultSet;
			if (virtualListResultSet != null)
			{
				((IDisposable)virtualListResultSet).Dispose();
				m_virtualListResultSet = null;
			}
		}

		private unsafe void BeginExecuteWorker(object state)
		{
			//IL_005b: Expected I, but got I8
			//IL_006e: Expected I, but got I8
			//IL_00a0: Expected I, but got I8
			//IL_00d3: Expected I, but got I8
			//IL_0105: Expected I, but got I8
			//IL_0138: Expected I, but got I8
			//IL_0160: Expected I, but got I8
			//IL_0172: Expected I, but got I8
			//IL_019b: Expected I, but got I8
			//IL_01c3: Expected I, but got I8
			//IL_01d5: Expected I, but got I8
			//IL_01fe: Expected I, but got I8
			//IL_020e: Expected I, but got I8
			//IL_0340: Expected I, but got I8
			//IL_0375: Expected I, but got I8
			//IL_03a8: Expected I, but got I8
			//IL_03d0: Expected I, but got I8
			//IL_03f9: Expected I, but got I8
			//IL_0421: Expected I, but got I8
			//IL_044a: Expected I, but got I8
			//IL_0476: Expected I, but got I8
			//IL_04a7: Expected I, but got I8
			//IL_04d0: Expected I, but got I8
			//IL_0505: Expected I, but got I8
			//IL_0538: Expected I, but got I8
			//IL_0561: Expected I, but got I8
			//IL_057f: Expected I, but got I8
			//IL_05a8: Expected I, but got I8
			//IL_05dd: Expected I, but got I8
			//IL_0611: Expected I, but got I8
			//IL_063a: Expected I, but got I8
			//IL_0689: Expected I, but got I8
			//IL_06ba: Expected I, but got I8
			//IL_06e0: Expected I, but got I8
			//IL_070e: Expected I, but got I8
			//IL_0830: Expected I, but got I8
			//IL_0b62: Expected I, but got I8
			//IL_0bc8: Expected I, but got I8
			//IL_0be6: Expected I, but got I8
			//IL_0bff: Expected I, but got I8
			//IL_0c87: Expected I, but got I8
			//IL_0c8c: Expected I, but got I8
			bool[] ascendings = null;
			string[] sorts = null;
			string text = null;
			int num = (int)state;
			if (num != m_requestGeneration)
			{
				return;
			}
			QueryPropertyBag queryPropertyBag = new QueryPropertyBag();
			IQueryPropertyBag* iQueryPropertyBag = queryPropertyBag.GetIQueryPropertyBag();
			bool retainedList = false;
			if (GetSortAttributes(out sorts, out ascendings))
			{
				IMultiSortAttributes* ptr = queryPropertyBag.PackMultiSortAttributes(sorts, ascendings);
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, IMultiSortAttributes*, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 32)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)23, ptr);
				if (ptr != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr + 16)))((nint)ptr);
				}
			}
			object property = GetProperty("ArtistIds");
			if (property != null)
			{
				long num2 = *(long*)iQueryPropertyBag + 24;
				IDList* intPtr = queryPropertyBag.PackIDList((IList)property);
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, IDList*, int>)(*(ulong*)num2))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)4, intPtr);
			}
			object property2 = GetProperty("GenreIds");
			if (property2 != null)
			{
				long num3 = *(long*)iQueryPropertyBag + 24;
				IDList* intPtr2 = queryPropertyBag.PackIDList((IList)property2);
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, IDList*, int>)(*(ulong*)num3))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)12, intPtr2);
			}
			object property3 = GetProperty("AlbumIds");
			if (property3 != null)
			{
				long num4 = *(long*)iQueryPropertyBag + 24;
				IDList* intPtr3 = queryPropertyBag.PackIDList((IList)property3);
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, IDList*, int>)(*(ulong*)num4))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)7, intPtr3);
			}
			object property4 = GetProperty("UserCardIds");
			if (property4 != null)
			{
				long num5 = *(long*)iQueryPropertyBag + 24;
				IDList* intPtr4 = queryPropertyBag.PackIDList((IList)property4);
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, IDList*, int>)(*(ulong*)num5))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)30, intPtr4);
			}
			object property5 = GetProperty("DeviceId");
			if (property5 != null)
			{
				int num6 = (int)property5;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)1, num6);
			}
			else
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)1, 1);
			}
			object property6 = GetProperty("SyncMappedError");
			if (property6 != null)
			{
				int num7 = (int)property6;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)18, num7);
			}
			property6 = GetProperty("UserId");
			if (property6 != null)
			{
				int num8 = (int)property6;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, 0, num8);
			}
			else
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, 0, 1);
			}
			object property7 = GetProperty("InLibrary");
			if (property7 != null)
			{
				int num9 = (int)property7;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)26, num9);
			}
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)2, 0);
			EQueryTypeView eQueryTypeView = EQueryTypeView.eQueryTypeLibraryView;
			object property8 = GetProperty("ShowDeviceContents");
			if (property8 != null)
			{
				eQueryTypeView = (((bool)property8) ? EQueryTypeView.eQueryTypeDeviceView : eQueryTypeView);
			}
			property8 = GetProperty("DiscMediaView");
			if (property8 != null)
			{
				eQueryTypeView = (((bool)property8) ? EQueryTypeView.eQueryTypeDiscMediaView : eQueryTypeView);
			}
			property8 = GetProperty("Remaining");
			if (property8 != null)
			{
				eQueryTypeView = (((bool)property8) ? EQueryTypeView.eQueryTypeSyncRemaining : eQueryTypeView);
			}
			property8 = GetProperty("Complete");
			if (property8 != null)
			{
				eQueryTypeView = (((bool)property8) ? EQueryTypeView.eQueryTypeSyncSucceeded : eQueryTypeView);
			}
			property8 = GetProperty("Failed");
			if (property8 != null)
			{
				eQueryTypeView = (((bool)property8) ? EQueryTypeView.eQueryTypeSyncFailed : eQueryTypeView);
			}
			property8 = GetProperty("MultiSelect");
			if (property8 != null && (bool)property8)
			{
				eQueryTypeView = ((EQueryTypeView.eQueryTypeDeviceView == eQueryTypeView) ? EQueryTypeView.eQueryTypeDeviceMultiSelectView : EQueryTypeView.eQueryTypeLibraryMultiSelectView);
			}
			object property9 = GetProperty("RulesOnly");
			if (property9 != null)
			{
				eQueryTypeView = (((bool)property9) ? EQueryTypeView.eQueryTypeDeviceSyncRuleView : eQueryTypeView);
			}
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)15, (int)eQueryTypeView);
			object property10 = GetProperty("Keywords");
			if (property10 != null)
			{
				fixed (char* property10Ptr = ((string)property10).ToCharArray())
				{
					ushort* ptr2 = (ushort*)property10Ptr;
					try
					{
						long num10 = *(long*)iQueryPropertyBag + 40;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, ushort*, int>)(*(ulong*)num10))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)19, ptr2);
					}
					catch
					{
						//try-fault
						ptr2 = null;
						throw;
					}
				}
			}
			object property11 = GetProperty("ContributingArtistId");
			if (property11 != null)
			{
				int num11 = (int)property11;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)5, num11);
			}
			property11 = GetProperty("ArtistId");
			if (property11 != null)
			{
				int num12 = (int)property11;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)3, num12);
			}
			property11 = GetProperty("GenreId");
			if (property11 != null)
			{
				int num13 = (int)property11;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)11, num13);
			}
			property11 = GetProperty("AlbumId");
			if (property11 != null)
			{
				int num14 = (int)property11;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)6, num14);
			}
			property11 = GetProperty("FolderId");
			if (property11 != null)
			{
				int num15 = (int)property11;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)9, num15);
			}
			property11 = GetProperty("RecurseIntoFolders");
			if (property11 != null && (int)property11 != 0)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)36, 1);
			}
			object property12 = GetProperty("FolderMediaType");
			if (property12 != null)
			{
				EMediaTypes eMediaTypes = LibraryDataProvider.NameToMediaType((string)property12);
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)13, (int)eMediaTypes);
			}
			object property13 = GetProperty("MediaType");
			if (property13 != null)
			{
				int num16 = (int)property13;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)13, num16);
			}
			object property14 = GetProperty("TOC");
			if (property14 != null)
			{
				fixed (char* property14Ptr = ((string)property14).ToCharArray())
				{
					ushort* ptr3 = (ushort*)property14Ptr;
					try
					{
						long num17 = *(long*)iQueryPropertyBag + 40;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, ushort*, int>)(*(ulong*)num17))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)20, ptr3);
					}
					catch
					{
						//try-fault
						ptr3 = null;
						throw;
					}
				}
			}
			object property15 = GetProperty("SeriesId");
			if (property15 != null)
			{
				int num18 = (int)property15;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)8, num18);
			}
			property15 = GetProperty("WatchType");
			if (property15 != null)
			{
				int num19 = (int)property15;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)32, num19);
			}
			if (GetProperty("ExpiresOnly") != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)33, 1);
			}
			object property16 = GetProperty("Operation");
			if (property16 != null)
			{
				int num20 = (int)property16;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)16, num20);
			}
			property16 = GetProperty("InitTime");
			if (property16 != null)
			{
				fixed (char* property16Ptr = ((string)property16).ToCharArray())
				{
					ushort* ptr4 = (ushort*)property16Ptr;
					try
					{
						long num21 = *(long*)iQueryPropertyBag + 40;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, ushort*, int>)(*(ulong*)num21))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)17, ptr4);
					}
					catch
					{
						//try-fault
						ptr4 = null;
						throw;
					}
				}
			}
			object property17 = GetProperty("PlaylistId");
			if (property17 != null)
			{
				int num22 = (int)property17;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)10, num22);
			}
			property17 = GetProperty("CategoryId");
			if (property17 != null)
			{
				int num23 = (int)property17;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)27, num23);
			}
			property17 = GetProperty("PlaylistType");
			if (property17 != null && !string.IsNullOrEmpty((string)property17))
			{
				PlaylistType playlistType = (PlaylistType)Enum.Parse(typeof(PlaylistType), (string)property17);
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)24, (int)playlistType);
			}
			object property18 = GetProperty("PlaylistTypeMask");
			if (property18 != null)
			{
				int num24 = (int)property18;
				if (num24 != 0)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)25, num24);
				}
			}
			object property19 = GetProperty("MaxResultCount");
			if (property19 != null)
			{
				int num25 = (int)property19;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)31, num25);
			}
			property19 = GetProperty("DrmStateMask");
			if (property19 != null)
			{
				ulong num26 = (ulong)(long)property19;
				if (num26 != 0L)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, ulong, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 48)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)34, num26);
				}
			}
			string text2 = (string)GetProperty("QueryType");
			EQueryType eQueryType = EQueryType.eQueryTypeInvalid;
			if (queryPropertyBag.IsSet("Keywords"))
			{
				switch (text2)
				{
				case "Artist":
					eQueryType = EQueryType.eQueryTypeArtistsWithKeyword;
					break;
				case "Album":
					eQueryType = EQueryType.eQueryTypeAlbumsWithKeyword;
					break;
				case "Track":
					eQueryType = EQueryType.eQueryTypeTracksWithKeyword;
					break;
				case "Playlist":
					eQueryType = EQueryType.eQueryTypePlaylistsWithKeyword;
					break;
				case "Photo":
					eQueryType = EQueryType.eQueryTypePhotosWithKeyword;
					break;
				case "PodcastSeries":
					eQueryType = EQueryType.eQueryTypeSubscriptionsSeriesWithKeyword;
					break;
				case "PodcastEpisode":
					eQueryType = EQueryType.eQueryTypeSubscriptionsEpisodesWithKeyword;
					break;
				case "Video":
					eQueryType = EQueryType.eQueryTypeVideoWithKeyword;
					break;
				}
			}
			else
			{
				int num27;
				switch (text2)
				{
				case "Artist":
					eQueryType = EQueryType.eQueryTypeAllAlbumArtists;
					break;
				case "Genres":
				{
					object property21 = GetProperty("MediaType");
					int num28 = ((property21 == null) ? 3 : ((int)property21));
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)13, num28);
					eQueryType = EQueryType.eQueryTypeAllGenres;
					break;
				}
				case "Album":
					property19 = GetProperty("ArtistId");
					if ((property19 != null && (int)property19 != -1) || queryPropertyBag.IsSet("ArtistIds"))
					{
						eQueryType = EQueryType.eQueryTypeAlbumsForAlbumArtistId;
					}
					else
					{
						property19 = GetProperty("GenreId");
						eQueryType = (((property19 == null || (int)property19 == -1) && !queryPropertyBag.IsSet("GenreIds")) ? EQueryType.eQueryTypeAllAlbums : EQueryType.eQueryTypeAlbumsByGenreId);
					}
					retainedList = true;
					break;
				case "Track":
				{
					object property20 = GetProperty("RulesOnly");
					if (property20 != null && (bool)property20)
					{
						eQueryType = EQueryType.eQueryTypeAllTracks;
						break;
					}
					property20 = GetProperty("AlbumId");
					if ((property20 != null && (int)property20 != -1) || queryPropertyBag.IsSet("AlbumIds"))
					{
						property19 = GetProperty("ArtistId");
						eQueryType = (((property19 == null || (int)property19 == -1) && !queryPropertyBag.IsSet("ArtistIds")) ? EQueryType.eQueryTypeTracksForAlbumId : EQueryType.eQueryTypeTracksForAlbumArtistId);
						break;
					}
					property19 = GetProperty("ArtistId");
					if ((property19 != null && (int)property19 != -1) || queryPropertyBag.IsSet("ArtistIds"))
					{
						eQueryType = EQueryType.eQueryTypeTracksForAlbumArtistId;
						break;
					}
					property19 = GetProperty("GenreId");
					if ((property19 != null && (int)property19 != -1) || queryPropertyBag.IsSet("GenreIds"))
					{
						eQueryType = EQueryType.eQueryTypeTracksByGenreId;
						break;
					}
					property19 = GetProperty("Detailed");
					if (property19 != null && (bool)property19)
					{
						eQueryType = EQueryType.eQueryTypeAllTracksDetailed;
						break;
					}
					property19 = GetProperty("TOC");
					eQueryType = ((property19 != null && !string.IsNullOrEmpty((string)property19)) ? EQueryType.eQueryTypeTracksForTOC : EQueryType.eQueryTypeAllTracks);
					break;
				}
				case "AlbumByTOC":
					eQueryType = EQueryType.eQueryTypeAlbumsByTOC;
					if (!queryPropertyBag.IsSet("TOC"))
					{
						eQueryType = EQueryType.eQueryTypeInvalid;
					}
					break;
				case "Photo":
					eQueryType = ((eQueryTypeView == EQueryTypeView.eQueryTypeDeviceSyncRuleView) ? EQueryType.eQueryTypeAllPhotos : EQueryType.eQueryTypePhotosByFolderId);
					break;
				case "MediaFolder":
					eQueryType = EQueryType.eQueryTypeMediaFolders;
					break;
				case "Video":
					property19 = GetProperty("CategoryId");
					eQueryType = ((property19 == null || (int)property19 == -1) ? EQueryType.eQueryTypeAllVideos : EQueryType.eQueryTypeVideosByCategoryId);
					break;
				case "PodcastSeries":
					eQueryType = EQueryType.eQueryTypeAllPodcastSeries;
					break;
				case "PodcastEpisode":
					property19 = GetProperty("SeriesId");
					eQueryType = ((property19 == null || (int)property19 == -1) ? EQueryType.eQueryTypeAllPodcastEpisodes : EQueryType.eQueryTypeEpisodesForSeriesId);
					break;
				case "SyncItem":
					eQueryType = EQueryType.eQueryTypeSyncProgress;
					break;
				case "Playlist":
					eQueryType = EQueryType.eQueryTypeAllPlaylists;
					break;
				case "PlaylistContent":
					eQueryType = EQueryType.eQueryTypePlaylistContentByPlaylistId;
					break;
				case "UserCard":
					eQueryType = EQueryType.eQueryTypeUserCards;
					break;
				case "Person":
				{
					eQueryType = EQueryType.eQueryTypePersonsByTypeId;
					EMediaTypes eMediaTypes2 = EMediaTypes.eMediaTypePersonArtist;
					eMediaTypes2 = (((string)GetProperty("PersonType") == "Composer") ? EMediaTypes.eMediaTypePersonComposer : eMediaTypes2);
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)28, (int)eMediaTypes2);
					break;
				}
				case "ArtistsRanking":
					eQueryType = EQueryType.eQueryTypeArtistsRanking;
					break;
				case "TVSeries":
					eQueryType = EQueryType.eQueryTypeVideoSeriesTitles;
					break;
				case "Pin":
				{
					eQueryType = EQueryType.eQueryTypePinsByPinType;
					EPinType ePinType = EPinType.ePinTypeGeneric;
					property19 = GetProperty("PinType");
					if (property19 != null)
					{
						ePinType = (EPinType)property19;
					}
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)35, (int)ePinType);
					break;
				}
				default:
					num27 = (int)eQueryType;
					goto IL_0bde;
				case "App":
					{
						num27 = 38;
						goto IL_0bde;
					}
					IL_0bde:
					eQueryType = (EQueryType)num27;
					break;
				}
			}
			int num29 = 0;
			IDatabaseQueryResults* ptr5 = null;
			if ((byte)((num != m_requestGeneration) ? 1 : 0) == 0)
			{
				if (eQueryType != EQueryType.eQueryTypeInvalid)
				{
					ushort* ptr6 = null;
					num29 = Module.QueryDatabase(eQueryType, iQueryPropertyBag, &ptr5, &ptr6);
					text = new string((char*)ptr6);
					Module.SysFreeString(ptr6);
				}
				else
				{
					num29 = 0;
				}
			}
			if ((byte)((num != m_requestGeneration) ? 1 : 0) == 0 && num29 >= 0)
			{
				ZuneQueryList queryList = null;
				if (ptr5 != null)
				{
					queryList = new ZuneQueryList(ptr5, text2);
				}
				Application.DeferredInvoke(DeferredSetResult, new DeferredSetResultArgs(num, queryList, retainedList));
			}
			((IDisposable)queryPropertyBag)?.Dispose();
			if (0L != (nint)ptr5)
			{
				IDatabaseQueryResults* intPtr5 = ptr5;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr5 + 16)))((nint)intPtr5);
				ptr5 = null;
			}
			if (num29 < 0)
			{
				throw new COMException("Could not perform query " + text, num29);
			}
		}

		private unsafe void DeferredSetResult(object state)
		{
			DeferredSetResultArgs deferredSetResultArgs = (DeferredSetResultArgs)state;
			if (!m_disposed && deferredSetResultArgs.RequestGeneration == m_requestGeneration)
			{
				bool isEmpty = true;
				if (deferredSetResultArgs.QueryList != null)
				{
					object property = GetProperty("AutoRefresh");
					bool autoRefresh = property == null || (bool)property;
					object property2 = GetProperty("AntialiasImageEdges");
					bool antialiasEdges = property2 != null && (bool)property2;
					m_thumbnailFallbackImageUrl = (string)GetProperty("ThumbnailFallbackImageUrl");
					((IDisposable)m_virtualListResultSet)?.Dispose();
					ReleaseBehavior releaseBehavior2 = ((m_virtualListResultSet = new LibraryVirtualList(this, deferredSetResultArgs.QueryList, autoRefresh, antialiasEdges)).VisualReleaseBehavior = ((!deferredSetResultArgs.RetainedList) ? ReleaseBehavior.ReleaseReference : ReleaseBehavior.KeepReference));
					isEmpty = deferredSetResultArgs.QueryList.IsEmpty;
				}
				LibraryDataProviderQueryResult libraryDataProviderQueryResult = new LibraryDataProviderQueryResult(this, m_virtualListResultSet, ResultTypeCookie);
				libraryDataProviderQueryResult.SetIsEmpty(isEmpty);
				Result = libraryDataProviderQueryResult;
				Status = DataProviderQueryStatus.Complete;
				if (Unsafe.As<EtwControlerState, byte>(ref Unsafe.AddByteOffset(ref Module.g_EtwControlerState, 8)) <= 1u || (Unsafe.As<EtwControlerState, int>(ref Unsafe.AddByteOffset(ref Module.g_EtwControlerState, 4)) & 0x10) == 0)
				{
					return;
				}
				fixed (char* stringPtr = ToString().ToCharArray())
				{
					ushort* pwszDetail = (ushort*)stringPtr;
					try
					{
						Module.PERFTRACE_COLLECTIONEVENT((_COLLECTION_EVENT)29, pwszDetail);
					}
					catch
					{
						//try-fault
						pwszDetail = null;
						throw;
					}
				}
			}
			else
			{
				((IDisposable)deferredSetResultArgs.QueryList)?.Dispose();
			}
		}
	}
}
