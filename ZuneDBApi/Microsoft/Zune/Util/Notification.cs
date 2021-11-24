using System;
using Microsoft.Zune.Configuration;

namespace Microsoft.Zune.Util
{
	public class Notification
	{
		public static void BroadcastNowPlaying(EMediaTypes MediaType, string AlbumName, string ArtistName, string TrackTitle, int TrackNumber, Guid ZuneMediaId)
		{
			if (!ClientConfiguration.Playback.NotifyIMClient)
			{
				return;
			}
			switch (MediaType)
			{
			case EMediaTypes.eMediaTypePodcastEpisode:
				if (ClientConfiguration.Playback.NotifyIncludePodcasts)
				{
					goto default;
				}
				break;
			case EMediaTypes.eMediaTypeVideo:
				if (!ClientConfiguration.Playback.NotifyIncludeVideos)
				{
					break;
				}
				goto default;
			default:
				NotifyInstantMessenger(AlbumName, ArtistName, TrackTitle, TrackNumber, ZuneMediaId);
				break;
			case EMediaTypes.eMediaTypeInvalid:
				break;
			}
		}

		public static void ResetNowPlaying()
		{
			if (ClientConfiguration.Playback.NotifyIMClient)
			{
				ResetInstantMessenger();
			}
		}

		internal unsafe static void NotifyInstantMessenger(string AlbumName, string ArtistName, string TrackTitle, int TrackNumber, Guid ZuneMediaId)
		{
			string text = string.Format("ZUNE\\0Music\\01\\0{{0}} - {{1}}\\0{0}\\0{1}\\0{2}\\0zune:{{{3}}}\\0", new object[]
			{
				PrepareStringForBroadcast(TrackTitle),
				PrepareStringForBroadcast(ArtistName),
				PrepareStringForBroadcast(AlbumName),
				ZuneMediaId
			});
			fixed (char* textPtr = text)
			{
				ulong dwData = 1351L;
				tagCOPYDATASTRUCT tagCOPYDATASTRUCT2 = new()
				{
					dwData = &dwData,
					cbData = (uint)((text.Length + 1) * 2),
					lpData = (ushort*)textPtr
				};
				SendInstantMessengerMessage(&tagCOPYDATASTRUCT2);
			}
		}

		internal unsafe static void SendInstantMessengerMessage(tagCOPYDATASTRUCT* pcds)
		{
			fixed (char* textPtr = Module.WNDCLASS_MsnMsgrUIManager)
            {
				ushort* @class = (ushort*)textPtr;
				ushort* window = null;// (ushort*)(&Module.??_C@_11LOCGONAA@?$AA?$AA@);
				HWND* ptr = Module.FindWindowExW(null, null, @class, window);
				if (ptr != null)
				{
					ulong num;
					while (Module.SendMessageTimeoutW(ptr, 74U, 0UL, new IntPtr(pcds).ToInt64(), 2U, 300U, &num) != 0)
					{
						ptr = Module.FindWindowExW(null, ptr, @class, window);
						if (ptr == null)
						{
							return;
						}
					}
					uint lastError = Module.GetLastError();
					Module._ZuneShipAssert(1004, 284);
				}
			}
		}

		internal static string PrepareStringForBroadcast(string str)
		{
			if (str != null)
			{
				str = str.Replace("\\0", "");
				str = str.Replace("{", "[");
				return str.Replace("}", "]");
			}
			return str;
		}

		internal unsafe static void ResetInstantMessenger()
		{
			string text = "ZUNE\\0Music\\00\\0{0}\\0";
			fixed (char* ptr = text)
			{
                ulong dwData = 1351L;
                tagCOPYDATASTRUCT tagCOPYDATASTRUCT2 = new()
				{
					dwData = &dwData,
					cbData = (uint)((text.Length + 1) * 2),
					lpData = (ushort*)ptr
				};
				SendInstantMessengerMessage(&tagCOPYDATASTRUCT2);
			}
		}
	}
}
