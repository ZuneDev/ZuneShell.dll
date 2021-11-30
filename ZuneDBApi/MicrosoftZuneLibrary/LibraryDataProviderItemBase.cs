using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Iris;

namespace MicrosoftZuneLibrary
{
	public class LibraryDataProviderItemBase : DataProviderObject, IDatabaseMedia
	{
		protected uint m_uTraceId;

		private static Stack m_thumbnailsToProcess = new Stack();

		private static bool m_thumbnailWorkerAlive = false;

		private static object m_lockObject = new object();

		private AsyncGetThumbnailState[] m_fThumbnailRequested;

		private Image[] m_thumbnail;

		private long[] m_thumbnailSize;

		private Dictionary<string, object> m_setProperties;

		private bool m_fSlowDataThumbnailExtraction;

		private bool m_fHasGottenSlowDataRequest;

		private static int TinyThumbnailSize = 38;

		private static int SmallThumbnailSize = 86;

		private static int LargeThumbnailSize = 159;

		private static int SuperLargeThumbnailSize = 240;

		private static int NowPlayingThumbnailSize = 200;

		private static int c_MaxThumbnails = 3;

		private static int[] s_thumbnailSizes = new int[3] { 86, 159, 240 };

		internal unsafe LibraryDataProviderItemBase(DataProviderQuery query, object typeCookie)
			: base(query, typeCookie)
		{
			m_uTraceId = Module.MicrosoftZuneLibrary_002E_003FA0xea57f500_002Es_uNextTraceId;
			Module.MicrosoftZuneLibrary_002E_003FA0xea57f500_002Es_uNextTraceId++;
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 10, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId);
			}
		}

		public unsafe override object GetProperty(string propertyName)
		{
			object obj = null;
			DataProviderMapping value = null;
			Dictionary<string, object> setProperties = m_setProperties;
			if (setProperties != null)
			{
				obj = null;
				if (setProperties.TryGetValue(propertyName, out obj))
				{
					return obj;
				}
			}
			if (Mappings.TryGetValue(propertyName, out value))
			{
				if (!string.IsNullOrEmpty(value.Source))
				{
					if (value.Source == "ThumbnailPath")
					{
						throw new NotImplementedException();
					}
					int num = ThumbnailSizeIndexFromSizeName(value.Source, -1);
					if (num >= 0)
					{
						EnsureThumbnailStorage();
						Image[] thumbnail = m_thumbnail;
						if (thumbnail[num] != null)
						{
							if (((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
							{
								fixed (char* thumbnailSourcePtr = thumbnail[num].Source.ToCharArray())
								{
									ushort* a = (ushort*)thumbnailSourcePtr;
									try
									{
										if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
										{
											Module.WPP_SF_DdS(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 14, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId, num, a);
										}
									}
									catch
									{
										//try-fault
										a = null;
										throw;
									}
								}
							}
							return m_thumbnail[num];
						}
						if (num != 0 || !m_fSlowDataThumbnailExtraction)
						{
							BeginGetThumbnail(propertyName, num, slowDataQuery: false);
						}
						return null;
					}
					return GetFieldValue(value.PropertyType, value.Source, value.DefaultValue);
				}
				return value.DefaultValue;
			}
			return null;
		}

		public override void SetProperty(string propertyName, object value)
		{
			DataProviderMapping value2 = null;
			if (!Mappings.TryGetValue(propertyName, out value2) || string.IsNullOrEmpty(value2.Source))
			{
				return;
			}
			if (value2.Source == "ThumbnailPath")
			{
				string newThumbnail = (string)value;
				SetNewThumbnail(newThumbnail);
				return;
			}
			if (Equals(value, GetProperty(propertyName)))
			{
				return;
			}
			if (!(TypeName == "Playlist") || !(value2.Source == "Title"))
			{
				int num = SetFieldValue(value2.Source, value);
				if (num < 0)
				{
					Module.SQMAddNumbersToStream("IgnoredErrorEvent", 1u, (uint)num);
					goto IL_00eb;
				}
				if (value2.Source == "UserRating")
				{
					DateTime utcNow = DateTime.UtcNow;
					SetFieldValue("UserLastRatedDate", utcNow);
				}
			}
			if (m_setProperties == null)
			{
				m_setProperties = new Dictionary<string, object>();
			}
			m_setProperties[propertyName] = value;
			goto IL_00eb;
			IL_00eb:
			FirePropertyChanged(propertyName);
		}

		public virtual void GetMediaIdAndType(out int mediaId, out EMediaTypes mediaType)
		{
			if (TypeName == "PlaylistContentItem")
			{
				mediaId = (int)GetProperty("MediaId");
				mediaType = (EMediaTypes)GetProperty("MediaType");
			}
			else if (TypeName == "SyncItem")
			{
				mediaId = (int)GetProperty("LibraryId");
				mediaType = (EMediaTypes)GetProperty("MediaType");
			}
			else
			{
				mediaId = (int)GetProperty("LibraryId");
				mediaType = LibraryDataProvider.NameToMediaType(TypeName);
			}
		}

		public unsafe static string GetArtUrl(int MediaId, string typeName, [MarshalAs(UnmanagedType.U1)] bool fCacheOnly)
		{
			//IL_0003: Expected I, but got I8
			ushort* artUrl = null;
			EMediaTypes eMediaTypes = LibraryDataProvider.NameToMediaType(typeName);
			if (Module.LocateArt(MediaId, eMediaTypes, fCacheOnly, &artUrl) >= 0)
			{
				string result = new string((char*)artUrl);
				Module.SysFreeString(artUrl);
				return result;
			}
			return null;
		}

		public unsafe virtual void InvalidateAllProperties()
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 55, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId);
			}
			foreach (DataProviderMapping value in Mappings.Values)
			{
				FirePropertyChanged(value.PropertyName);
			}
			m_setProperties = null;
			if (HasThumbnailArt() && (TypeName == "Album" || TypeName == "Artist" || TypeName == "App" || TypeName == "Photo"))
			{
				CheckThumbnail();
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 56, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId);
			}
		}

		public void SetSlowDataThumbnailExtraction([MarshalAs(UnmanagedType.U1)] bool useSlowData)
		{
			m_fSlowDataThumbnailExtraction = useSlowData;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		protected unsafe bool SetNewThumbnail(string strThumbnailPath)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 28, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId);
			}
			int num = -2147418113;
			if (TypeName == "Album")
			{
				int mediaId = (int)GetFieldValue(typeof(int), 355u, -1);
				fixed (char* strThumbnailPathPtr = strThumbnailPath.ToCharArray())
				{
					ushort* ptr = (ushort*)strThumbnailPathPtr;
					try
					{
						num = Module.SetAlbumArt(mediaId, ptr);
						if (num < 0)
						{
							Module.SQMAddNumbersToStream("IgnoredErrorEvent", 1u, (uint)num);
						}
						SetSlowDataThumbnailExtraction(useSlowData: false);
						InvalidateAllProperties();
					}
					catch
					{
						//try-fault
						ptr = null;
						throw;
					}
				}
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_Dd(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 29, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId, num);
			}
			return (byte)((num >= 0) ? 1u : 0u) != 0;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool SetNewThumbnail(SafeBitmap safeBitmap)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 30, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId);
			}
			int num = -2147418113;
			if (TypeName == "Album" && safeBitmap != null)
			{
				int num2 = (int)GetFieldValue(typeof(int), 355u, -1);
				void* ptr = (void*)safeBitmap.DangerousGetHandle();
				num = Module.SetAlbumArt(num2, (HBITMAP*)ptr);
				if (num < 0)
				{
					Module.SQMAddNumbersToStream("IgnoredErrorEvent", 1u, (uint)num);
				}
				SetSlowDataThumbnailExtraction(useSlowData: false);
				InvalidateAllProperties();
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 31, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId);
			}
			return (byte)((num >= 0) ? 1u : 0u) != 0;
		}

		protected internal unsafe virtual void OnRequestSlowData()
		{
			DataProviderMapping value = null;
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 18, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId);
			}
			int thumbnailIndex = 0;
			_ = "";
			string typeName = TypeName;
			m_fHasGottenSlowDataRequest = true;
			string text;
			switch (typeName)
			{
			case "Album":
				text = "AlbumArtSmall";
				break;
			default:
				text = "";
				break;
			case "Photo":
			case "Video":
			case "MediaFolder":
			case "App":
				text = "Thumbnail";
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				if (Mappings.TryGetValue(text, out value) && !string.IsNullOrEmpty(value.Source))
				{
					thumbnailIndex = ThumbnailSizeIndexFromSizeName(value.Source, 0);
				}
				if (BeginGetThumbnail(text, thumbnailIndex, slowDataQuery: true))
				{
					goto IL_00fa;
				}
			}
			NotifySlowDataAcquireComplete();
			goto IL_00fa;
			IL_00fa:
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 19, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId);
			}
		}

		[return: MarshalAs(UnmanagedType.U1)]
		protected unsafe bool BeginGetThumbnail(string thumbnailPropertyName, int thumbnailIndex, [MarshalAs(UnmanagedType.U1)] bool slowDataQuery)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 23, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId);
			}
			EnsureThumbnailStorage();
			bool flag = false;
			if (m_thumbnail[thumbnailIndex] == null || TypeName == "Album")
			{
				try
				{
					Monitor.Enter(m_lockObject);
					AsyncGetThumbnailState asyncGetThumbnailState = m_fThumbnailRequested[thumbnailIndex];
					if (asyncGetThumbnailState == null)
					{
						if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
						{
							Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 24, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId);
							int num = 1;
						}
						else
						{
							int num = 0;
						}
						asyncGetThumbnailState = new AsyncGetThumbnailState(this);
						asyncGetThumbnailState.thumbnailPropertyName = thumbnailPropertyName;
						asyncGetThumbnailState.thumbnailIndex = thumbnailIndex;
						asyncGetThumbnailState.slowDataQuery = slowDataQuery;
						asyncGetThumbnailState.antialiasImageEdges = AntialiasImageEdges();
						asyncGetThumbnailState.MediaId = (int)GetFieldValue(typeof(int), 355u, -1);
						m_fThumbnailRequested[thumbnailIndex] = asyncGetThumbnailState;
						flag = true;
						goto IL_018d;
					}
					bool isComplete = asyncGetThumbnailState.isComplete;
					flag = !isComplete;
					if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
					{
						Module.WPP_SF_Dl(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 25, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId, isComplete ? 1 : 0);
						int num2 = 1;
					}
					else
					{
						int num2 = 0;
					}
					if (flag)
					{
						goto IL_018d;
					}
					goto end_IL_006c;
					IL_018d:
					if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
					{
						Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 26, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId);
						int num3 = 1;
					}
					else
					{
						int num3 = 0;
					}
					m_thumbnailsToProcess.Push(asyncGetThumbnailState);
					if (!m_thumbnailWorkerAlive)
					{
						m_thumbnailWorkerAlive = true;
						ThreadPool.QueueUserWorkItem(GetThumbnailOnWorkerThread, null);
					}
					end_IL_006c:;
				}
				finally
				{
					Monitor.Exit(m_lockObject);
				}
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 27, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId);
			}
			return flag;
		}

		protected unsafe static void GetThumbnailOnWorkerThread(object listItem)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 34, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids));
			}
			bool flag = true;
			while (flag)
			{
				AsyncGetThumbnailState asyncGetThumbnailState = null;
				bool flag2 = true;
				try
				{
					Monitor.Enter(m_lockObject);
					if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
					{
						Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 35, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_thumbnailsToProcess.Count);
						int num = 1;
					}
					else
					{
						int num = 0;
					}
					if (m_thumbnailsToProcess.Count > 0)
					{
						listItem = m_thumbnailsToProcess.Pop();
						asyncGetThumbnailState = (AsyncGetThumbnailState)listItem;
						if (asyncGetThumbnailState != null)
						{
							flag2 = asyncGetThumbnailState.isComplete;
							if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
							{
								Module.WPP_SF_Dll(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 36, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), asyncGetThumbnailState.listItem.m_uTraceId, flag2 ? 1 : 0, asyncGetThumbnailState.slowDataQuery ? 1 : 0);
								int num2 = 1;
							}
							else
							{
								int num2 = 0;
							}
							asyncGetThumbnailState.isComplete = true;
						}
					}
					else
					{
						m_thumbnailWorkerAlive = false;
						flag = false;
						flag2 = true;
						if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
						{
							Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 37, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids));
							int num3 = 1;
						}
						else
						{
							int num4 = 0;
						}
					}
				}
				finally
				{
					Monitor.Exit(m_lockObject);
				}
				if (asyncGetThumbnailState == null)
				{
					continue;
				}
				if (flag2)
				{
					if (!asyncGetThumbnailState.slowDataQuery)
					{
						continue;
					}
					LibraryDataProviderItemBase listItem2 = asyncGetThumbnailState.listItem;
					if (listItem2 != null)
					{
						if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
						{
							Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 38, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), listItem2.m_uTraceId);
						}
						Application.DeferredInvoke(asyncGetThumbnailState.listItem.NotifySlowDataAcquireCompleteDelegate, null);
					}
					continue;
				}
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
				{
					Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 39, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), asyncGetThumbnailState.listItem.m_uTraceId);
				}
				LibraryDataProviderItemBase listItem3 = asyncGetThumbnailState.listItem;
				asyncGetThumbnailState.thumbnail = null;
				if (listItem3.TypeName == "Album" || listItem3.TypeName == "Artist" || listItem3.TypeName == "App" || listItem3.TypeName == "MediaFolder" || listItem3.TypeName == "Photo" || listItem3.TypeName == "Video")
				{
					string artUrl = GetArtUrl(asyncGetThumbnailState.MediaId, listItem3.TypeName, fCacheOnly: false);
					if (!string.IsNullOrEmpty(artUrl))
					{
						if (!listItem3.ArtNeedsUpdating(artUrl, asyncGetThumbnailState.thumbnailIndex))
						{
							if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
							{
								Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 40, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), listItem3.m_uTraceId);
							}
							continue;
						}
						if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
						{
							Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 41, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), listItem3.m_uTraceId);
						}
						int num5 = s_thumbnailSizes[asyncGetThumbnailState.thumbnailIndex];
						asyncGetThumbnailState.thumbnail = new Image("file://" + artUrl, num5, num5, flippable: false, asyncGetThumbnailState.antialiasImageEdges);
					}
				}
				Application.DeferredInvoke(listItem3.UpdateThumbnail, asyncGetThumbnailState);
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 42, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids));
			}
		}

		protected unsafe virtual void UpdateThumbnail(object args)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 43, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids));
			}
			AsyncGetThumbnailState asyncGetThumbnailState = (AsyncGetThumbnailState)args;
			if (asyncGetThumbnailState.slowDataQuery)
			{
				NotifySlowDataAcquireComplete();
			}
			uint a = (uint)(((int?)asyncGetThumbnailState.listItem?.m_uTraceId) ?? (-1));
			if (asyncGetThumbnailState.thumbnail == null)
			{
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
				{
					Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 44, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), a);
				}
				string text = ThumbnailFallbackImageUrl();
				if (!string.IsNullOrEmpty(text))
				{
					asyncGetThumbnailState.thumbnail = new Image(text);
				}
			}
			int thumbnailIndex = asyncGetThumbnailState.thumbnailIndex;
			if (m_thumbnail[thumbnailIndex] != asyncGetThumbnailState.thumbnail)
			{
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
				{
					Module.WPP_SF_Dd(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 45, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), a, thumbnailIndex);
				}
				m_thumbnail[asyncGetThumbnailState.thumbnailIndex] = asyncGetThumbnailState.thumbnail;
				if (asyncGetThumbnailState.thumbnailPropertyName != null)
				{
					if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
					{
						Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 46, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), a);
					}
					FirePropertyChanged(asyncGetThumbnailState.thumbnailPropertyName);
				}
			}
			((IDisposable)asyncGetThumbnailState).Dispose();
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 47, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids));
			}
		}

		protected void CheckThumbnail()
		{
			string artUrl = GetArtUrl((int)GetFieldValue(typeof(int), 355u, -1), TypeName, fCacheOnly: true);
			if (!string.IsNullOrEmpty(artUrl) && ArtNeedsUpdating(artUrl))
			{
				Application.DeferredInvoke(ClearThumbnailCacheOnAppThread, null);
			}
			else if (string.IsNullOrEmpty(artUrl) && TypeName == "Photo")
			{
				Application.DeferredInvoke(ClearThumbnailCacheOnAppThread, null);
			}
		}

		protected unsafe void ClearThumbnailCacheOnAppThread(object stateObj)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 53, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId);
			}
			if (TypeName == "Album")
			{
				ClearThumbnailCache();
				FirePropertyChanged("AlbumArtSmall");
			}
			else if (TypeName == "Photo")
			{
				ClearThumbnailCache();
				FirePropertyChanged("Thumbnail");
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 54, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId);
			}
		}

		protected virtual object GetFieldValue(Type type, uint atom)
		{
			return null;
		}

		protected virtual object GetFieldValue(Type type, string atomName, object defaultValue)
		{
			return null;
		}

		protected virtual object GetFieldValue(Type type, uint atom, object defaultValue)
		{
			return null;
		}

		protected virtual int SetFieldValue(string atomName, object value)
		{
			return -2147467263;
		}

		protected virtual int SetFieldValue(uint atom, object value)
		{
			return -2147467263;
		}

		protected virtual void NotifySlowDataAcquireComplete()
		{
		}

		protected void NotifySlowDataAcquireCompleteDelegate(object P_0)
		{
			NotifySlowDataAcquireComplete();
		}

		[return: MarshalAs(UnmanagedType.U1)]
		protected virtual bool AntialiasImageEdges()
		{
			return false;
		}

		protected virtual string ThumbnailFallbackImageUrl()
		{
			return null;
		}

		private unsafe void EnsureThumbnailStorage()
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 15, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId);
			}
			if (m_thumbnail == null)
			{
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
				{
					Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 16, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId);
				}
				m_fThumbnailRequested = new AsyncGetThumbnailState[3];
				m_thumbnail = new Image[3];
				m_thumbnailSize = new long[3];
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 17, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId);
			}
		}

		private unsafe void ClearThumbnailCache()
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 32, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId);
			}
			if (m_thumbnail != null)
			{
				string text = null;
				bool antialiasEdges = AntialiasImageEdges();
				int num = 0;
				if (0 < (nint)s_thumbnailSizes.LongLength)
				{
					do
					{
						if (m_thumbnail[num] != null)
						{
							int num2 = s_thumbnailSizes[num];
							Image.RemoveCache(m_thumbnail[num].Source, num2, num2, flippable: false, antialiasEdges);
							text = m_thumbnail[num].Source;
							m_thumbnail[num] = null;
						}
						m_fThumbnailRequested[num] = null;
						m_thumbnailSize[num] = 0L;
						num++;
					}
					while (num < (nint)s_thumbnailSizes.LongLength);
				}
				if (text != null)
				{
					Image.RemoveCache(text, 200, 200, flippable: false, antialiasEdges);
					Image.RemoveCache(text, 38, 38, flippable: false, antialiasEdges);
				}
				if (m_fHasGottenSlowDataRequest)
				{
					SetSlowDataThumbnailExtraction(useSlowData: false);
				}
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 33, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId);
			}
		}

		[return: MarshalAs(UnmanagedType.U1)]
		private bool HasThumbnailArt()
		{
			int num = ((TypeName != "Album" || (bool)GetFieldValue(typeof(bool), 191u, false)) ? 1 : 0);
			return (byte)num != 0;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		private unsafe bool ArtNeedsUpdating(string strThumbnailPath, int thumbnailIndex)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_D(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 51, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId);
			}
			bool flag = false;
			if (m_thumbnailSize != null)
			{
				FileInfo fileInfo = new FileInfo(strThumbnailPath);
				if (fileInfo.Exists)
				{
					try
					{
						EnsureThumbnailStorage();
						long length = fileInfo.Length;
						if (thumbnailIndex != -1)
						{
							long[] thumbnailSize = m_thumbnailSize;
							long num = thumbnailSize[thumbnailIndex];
							if (num == 0L || length != num)
							{
								thumbnailSize[thumbnailIndex] = length;
								flag = true;
							}
						}
						else
						{
							flag = true;
							for (int i = 0; i < 3; i++)
							{
								long num2 = m_thumbnailSize[i];
								if (num2 != 0L && length == num2)
								{
									flag = false;
									break;
								}
							}
						}
					}
					catch (IOException)
					{
						int num3 = 0;
						do
						{
							m_thumbnailSize[num3] = 0L;
							num3++;
						}
						while (num3 < 3);
						flag = true;
					}
				}
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 92uL)) & 0x40u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 89uL) >= 7u)
			{
				Module.WPP_SF_Dl(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 80uL), 52, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xea57f500_002EWPP_LibraryDataProvider_cpp_Traceguids), m_uTraceId, flag ? 1 : 0);
			}
			return flag;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		private bool ArtNeedsUpdating(string strThumbnailPath)
		{
			return ArtNeedsUpdating(strThumbnailPath, -1);
		}

		private static int ThumbnailSizeIndexFromSizeName(string thumbnailSizeName, int defaultSizeIndex)
		{
            return thumbnailSizeName switch
            {
                "LargeThumbnail" => 1,
                "SuperLargeThumbnail" => 2,
                "SmallThumbnail" or "Thumbnail" => 0,
                _ => defaultSizeIndex,
            };
		}
	}
}
