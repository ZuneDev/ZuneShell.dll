using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Iris;

namespace MicrosoftZuneLibrary
{
	public class LibraryDataProvider
	{
		public static void Register()
		{
			Application.RegisterDataProvider("Library", ConstructQuery);
		}

		public static DataProviderQuery ConstructQuery(object queryTypeCookie)
		{
			return new LibraryDataProviderQuery(queryTypeCookie);
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe static bool ActOnItems(IList mediaList, BulkItemAction action, EventArgs args)
		{
			if (mediaList != null && mediaList.Count != 0)
			{
				int num = 0;
				IDatabaseMedia databaseMedia = mediaList[0] as IDatabaseMedia;
				if (databaseMedia == null)
				{
					do
					{
						num++;
						databaseMedia = mediaList[num] as IDatabaseMedia;
					}
					while (databaseMedia == null);
				}
				databaseMedia.GetMediaIdAndType(out var _, out var mediaType);
				ArrayList arrayList = null;
				List<int> list = new List<int>(mediaList.Count);
				int num2 = num;
				if (num < mediaList.Count)
				{
					do
					{
						IDatabaseMedia databaseMedia2 = mediaList[num2] as IDatabaseMedia;
						if (databaseMedia2 != null)
						{
							databaseMedia2.GetMediaIdAndType(out var mediaId2, out var mediaType2);
							if (mediaId2 >= 0)
							{
								if (mediaType == mediaType2)
								{
									list.Add(mediaId2);
								}
								else
								{
									if (arrayList == null)
									{
										arrayList = new ArrayList();
									}
									arrayList.Add(databaseMedia2);
								}
							}
						}
						num2++;
					}
					while (num2 < mediaList.Count);
				}
				bool flag = true;
				if (list.Count > 0)
				{
					switch (action)
					{
					case BulkItemAction.UnexcludeFromSync:
						flag = (byte)((byte)((((SyncEventArgs)args).Device.Rules.Unexclude(list.ToArray(), mediaType) >= 0) ? 1u : 0u) & (true ? 1u : 0u)) != 0;
						break;
					case BulkItemAction.ExcludeFromSync:
						flag = (byte)((byte)((((SyncEventArgs)args).Device.Rules.Exclude(list.ToArray(), mediaType) >= 0) ? 1u : 0u) & (true ? 1u : 0u)) != 0;
						break;
					case BulkItemAction.RemoveSyncRules:
						flag = (byte)((byte)((((SyncEventArgs)args).Device.Rules.Remove(list.ToArray(), mediaType, fDeviceFolderIds: false) >= 0) ? 1u : 0u) & (true ? 1u : 0u)) != 0;
						break;
					case BulkItemAction.AddSyncRules:
						flag = (byte)((byte)((((SyncEventArgs)args).Device.Rules.Add(list.ToArray(), mediaType) >= 0) ? 1u : 0u) & (true ? 1u : 0u)) != 0;
						if (((TypeDiscoveringSyncEventArgs)args).ContainedTypes == null)
						{
							((TypeDiscoveringSyncEventArgs)args).ContainedTypes = new List<EMediaTypes>();
						}
						((TypeDiscoveringSyncEventArgs)args).ContainedTypes.Add(mediaType);
						break;
					case BulkItemAction.DeleteFromDevice:
					case BulkItemAction.ReverseSync:
					{
						ESyncOperationStatus operationStatus = ESyncOperationStatus.osInvalid;
						flag = (byte)((action != BulkItemAction.DeleteFromDevice) ? ((byte)((((SyncEventArgs)args).Device.ReverseSync(list.ToArray(), mediaType, ref operationStatus) >= 0) ? 1u : 0u) & (true ? 1u : 0u)) : ((byte)((((SyncEventArgs)args).Device.DeleteMedia(list.ToArray(), mediaType, ref operationStatus) >= 0) ? 1u : 0u) & (true ? 1u : 0u))) != 0;
						((DeferrableSyncEventArgs)args).Status = operationStatus;
						break;
					}
					default:
						flag = false;
						break;
					case BulkItemAction.DeleteFromLibrary:
						fixed (int* ptr = &list.ToArray()[0])
						{
							try
							{
								flag = (byte)((byte)((Module.DeleteMedia(mediaType, ptr, list.Count, ((DeleteFromLibraryEventArgs)args).DeleteFromDisk ? 1 : 0, 1) >= 0) ? 1u : 0u) & (true ? 1u : 0u)) != 0;
							}
							catch
							{
								//try-fault
								ptr = null;
								throw;
							}
						}
						break;
					}
					LogAction(action, mediaType, list.Count);
				}
				return ActOnItems(arrayList, action, args) && flag;
			}
			return true;
		}

		public static EMediaTypes NameToMediaType(string typeName)
		{
			int result;
			switch (typeName)
			{
			case "Artist":
				return EMediaTypes.eMediaTypePersonArtist;
			case "Album":
				return EMediaTypes.eMediaTypeAudioAlbum;
			case "Track":
				return EMediaTypes.eMediaTypeAudio;
			case "Photo":
				return EMediaTypes.eMediaTypeImage;
			case "Video":
				return EMediaTypes.eMediaTypeVideo;
			case "PodcastSeries":
				return EMediaTypes.eMediaTypePodcastSeries;
			case "PodcastEpisode":
				return EMediaTypes.eMediaTypePodcastEpisode;
			case "Playlist":
				return EMediaTypes.eMediaTypePlaylist;
			case "MediaFolder":
				return EMediaTypes.eMediaTypeFolder;
			case "Genre":
				return EMediaTypes.eMediaTypeGenre;
			case "UserCard":
				return EMediaTypes.eMediaTypeUserCard;
			default:
				result = -1;
				break;
			case "App":
				result = 110;
				break;
			}
			return (EMediaTypes)result;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public static bool GetSortAttributes(string sortString, out string[] sorts, out bool[] ascendings)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(sortString))
			{
				int num = (sorts = sortString.Split(',')).Length;
				if (num > 0)
				{
					ascendings = new bool[num];
				}
				int num2 = 0;
				if (0 < num)
				{
					do
					{
						bool flag = true;
						if (sorts[num2][0] == '-')
						{
							flag = false;
						}
						else
						{
							if (sorts[num2][0] != '+')
							{
								goto IL_006c;
							}
							flag = true;
						}
						sorts[num2] = sorts[num2].Substring(1);
						goto IL_006c;
						IL_006c:
						ascendings[num2] = flag;
						num2++;
					}
					while (num2 < num);
				}
				result = true;
			}
			return result;
		}

		private LibraryDataProvider()
		{
		}

		private static void LogAction(BulkItemAction action, EMediaTypes mediaType, int count)
		{
			if (mediaType == EMediaTypes.eMediaTypeUserCard)
			{
				switch (action)
				{
				case BulkItemAction.DeleteFromDevice:
				case BulkItemAction.RemoveSyncRules:
				case BulkItemAction.ExcludeFromSync:
					Module.SQMAddWrapper("SocialSyncDeleteProfileCard", count);
					break;
				case BulkItemAction.AddSyncRules:
				case BulkItemAction.UnexcludeFromSync:
					Module.SQMAddWrapper("SocialSyncAddProfileCard", count);
					break;
				}
			}
		}
	}
}
