using Microsoft.Iris;

namespace MicrosoftZuneLibrary
{
	public class StaticLibraryDataProviderQuery : DataProviderQuery
	{
		public static DataProviderQuery CreateInstance(object queryTypeCookie)
		{
			return new StaticLibraryDataProviderQuery(queryTypeCookie);
		}

		protected override void BeginExecute()
		{
			int userId = 1;
			int deviceId = 1;
			int libraryId = -1;
			object property = GetProperty("UserId");
			if (property != null)
			{
				userId = (int)property;
			}
			object property2 = GetProperty("LibraryId");
			if (property2 != null)
			{
				libraryId = (int)property2;
			}
			object property3 = GetProperty("DeviceId");
			if (property3 != null)
			{
				deviceId = (int)property3;
			}
			string thumbnailFallbackImageUrl = (string)GetProperty("ThumbnailFallbackImageUrl");
			EListType eListType;
			StaticLibraryDataProviderObject staticLibraryDataProviderObject2;
			switch ((string)GetProperty("QueryType"))
			{
			case "Artist":
				eListType = EListType.eArtistList;
				goto IL_0118;
			case "Album":
				eListType = EListType.eAlbumList;
				goto IL_0118;
			case "Folder":
				eListType = EListType.eFolderList;
				goto IL_0118;
			case "Video":
				eListType = EListType.eVideoList;
				goto IL_0118;
			case "PodcastSeries":
				eListType = EListType.ePodcastList;
				goto IL_0118;
			case "Playlist":
				eListType = EListType.ePlaylistList;
				goto IL_0118;
			case "Genre":
				eListType = EListType.eGenreList;
				goto IL_0118;
			case "Pin":
				eListType = EListType.ePinList;
				goto IL_0118;
			case "Photo":
				eListType = EListType.ePhotoList;
				goto IL_0118;
			default:
				{
					Status = DataProviderQueryStatus.Error;
					break;
				}
				IL_0118:
				staticLibraryDataProviderObject2 = (StaticLibraryDataProviderObject)(Result = new StaticLibraryDataProviderObject(this, base.ResultTypeCookie, eListType, libraryId, userId, deviceId, thumbnailFallbackImageUrl));
				Status = DataProviderQueryStatus.RequestingData;
				Status = DataProviderQueryStatus.Complete;
				break;
			}
		}

		private StaticLibraryDataProviderQuery(object queryTypeCookie)
			: base(queryTypeCookie)
		{
		}
	}
}
