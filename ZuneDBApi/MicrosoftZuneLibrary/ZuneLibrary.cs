using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MicrosoftZuneInterop;
using ZuneUI;

namespace MicrosoftZuneLibrary
{
	public class ZuneLibrary : IDisposable
	{
		private static object m_shutdownLock = new object();

		private static bool s_fShutdown = false;

		private void _007EZuneLibrary()
		{
			ManagedLock managedLock = null;
			ManagedLock managedLock2 = new ManagedLock(m_shutdownLock);
			try
			{
				managedLock = managedLock2;
				s_fShutdown = true;
			}
			catch
			{
				//try-fault
				((IDisposable)managedLock).Dispose();
				throw;
			}
			((IDisposable)managedLock).Dispose();
			Module.StopGroveler(false);
			Module.ShutdownZuneNativeLib();
			Module.WppCleanupUm();
			Module.ZuneEtwShutdown();
		}

		public unsafe int Initialize(string path, out bool dbRebuilt)
		{
			Module.ZuneEtwInit();
            Module.WPP_INIT_CONTROL_ARRAY((WPP_PROJECT_CONTROL_BLOCK*)Unsafe.AsPointer(ref Module.WPP_MAIN_CB));
			Module.WPP_INIT_GUID_ARRAY((_GUID**)Unsafe.AsPointer(ref Module.WPP_REGISTRATION_GUIDS));
			Module.WPP_GLOBAL_Control = (WPP_PROJECT_CONTROL_BLOCK*)Unsafe.AsPointer(ref Module.WPP_MAIN_CB);
			Module.WppInitUm((ushort*)Unsafe.AsPointer(ref Module.WPP_APPNAME));
			fixed (char* pathPtr = path.ToCharArray())
			{
				int dbRebuiltResult = 0;
				int result = Module.StartupZuneNativeLib((ushort*)pathPtr, &dbRebuiltResult);
				bool flag = dbRebuilt = (dbRebuiltResult != 0);
				return result;
			}
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public bool Phase2Initialization(out int hr)
		{
			hr = 0;
			return (byte)(((hr = Module.Phase2Initialization()) >= 0) ? 1u : 0u) != 0;
		}

		public unsafe static string LoadStringFromResource(uint dwResourceNumber)
		{
			HINSTANCE* ptr = Module.GetLocResourceInstance();
			if (ptr == null)
			{
				return null;
			}
			object result = null;
			IntPtr intPtr = default(IntPtr);
			int len = Module.LoadStringW(ptr, dwResourceNumber, (ushort*)(&intPtr), 0);
			if (intPtr != IntPtr.Zero)
			{
				result = Marshal.PtrToStringUni(intPtr, len);
			}
			return (string)result;
		}

		public unsafe ZuneQueryList QueryDatabase(EQueryType QueryType, int LibraryView, EQuerySortType SortType, uint SortAtom, QueryPropertyBag propertyBag)
		{
			//IL_0003: Expected I, but got I8
			//IL_005e: Expected I, but got I8
			//IL_005e: Expected I, but got I8
			//IL_0073: Expected I, but got I8
			//IL_0073: Expected I, but got I8
			//IL_0088: Expected I, but got I8
			//IL_0088: Expected I, but got I8
			//IL_0096: Expected I, but got I8
			//IL_0096: Expected I, but got I8
			//IL_00b6: Expected I, but got I8
			IDatabaseQueryResults* ptr = null;
			CComPtrNtv_003CIQueryPropertyBag_003E cComPtrNtv_003CIQueryPropertyBag_003E;
			*(long*)(&cComPtrNtv_003CIQueryPropertyBag_003E) = 0L;
			ZuneQueryList result;
			try
			{
				if (propertyBag != null)
				{
					Module.CComPtrNtv_003CIQueryPropertyBag_003E_002E_003D(&cComPtrNtv_003CIQueryPropertyBag_003E, propertyBag.GetIQueryPropertyBag());
				}
				else
				{
					int num = Module.CreatePropertyBag((IQueryPropertyBag**)(&cComPtrNtv_003CIQueryPropertyBag_003E));
					if (num < 0)
					{
						result = null;
						goto IL_003e;
					}
				}
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIQueryPropertyBag_003E*, void>)(&Module.CComPtrNtv_003CIQueryPropertyBag_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIQueryPropertyBag_003E);
				throw;
			}
			ZuneQueryList result2;
			try
			{
				long num2 = *(long*)(&cComPtrNtv_003CIQueryPropertyBag_003E);
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIQueryPropertyBag_003E)) + 56)))((nint)num2, (EQueryPropertyBagProp)21, (int)SortAtom);
				long num3 = *(long*)(&cComPtrNtv_003CIQueryPropertyBag_003E);
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIQueryPropertyBag_003E)) + 56)))((nint)num3, (EQueryPropertyBagProp)22, (int)SortType);
				long num4 = *(long*)(&cComPtrNtv_003CIQueryPropertyBag_003E);
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIQueryPropertyBag_003E)) + 56)))((nint)num4, (EQueryPropertyBagProp)15, LibraryView);
				int num = Module.QueryDatabase(QueryType, (IQueryPropertyBag*)(*(ulong*)(&cComPtrNtv_003CIQueryPropertyBag_003E)), &ptr, null);
				if (num >= 0)
				{
					result2 = new ZuneQueryList(ptr, "*** Test Only ***");
					IDatabaseQueryResults* intPtr = ptr;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
					goto IL_00c7;
				}
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIQueryPropertyBag_003E*, void>)(&Module.CComPtrNtv_003CIQueryPropertyBag_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIQueryPropertyBag_003E);
				throw;
			}
			ZuneQueryList result3;
			try
			{
				result3 = null;
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIQueryPropertyBag_003E*, void>)(&Module.CComPtrNtv_003CIQueryPropertyBag_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIQueryPropertyBag_003E);
				throw;
			}
			Module.CComPtrNtv_003CIQueryPropertyBag_003E_002ERelease(&cComPtrNtv_003CIQueryPropertyBag_003E);
			return result3;
			IL_003e:
			Module.CComPtrNtv_003CIQueryPropertyBag_003E_002ERelease(&cComPtrNtv_003CIQueryPropertyBag_003E);
			return result;
			IL_00c7:
			Module.CComPtrNtv_003CIQueryPropertyBag_003E_002ERelease(&cComPtrNtv_003CIQueryPropertyBag_003E);
			return result2;
		}

		public unsafe ZuneLibraryCDDeviceList GetCDDeviceList()
		{
			//IL_0003: Expected I, but got I8
			//IL_0021: Expected I, but got I8
			IWMPCDDeviceList* deviceList = null;
			if (Module.GetCDDeviceList(&deviceList) >= 0)
			{
				ZuneLibraryCDDeviceList zuneLibraryCDDeviceList = new ZuneLibraryCDDeviceList(deviceList);
				IWMPCDDeviceList* intPtr = deviceList;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
				zuneLibraryCDDeviceList?.AddRef();
				return zuneLibraryCDDeviceList;
			}
			return null;
		}

		public unsafe ZuneLibraryCDRecorder GetRecorder()
		{
			//IL_0003: Expected I, but got I8
			//IL_0026: Expected I, but got I8
			IRecordManager* ptr = null;
			if (Module.GetSingleton(Module.GUID_IRecordManager, (void**)(&ptr)) >= 0)
			{
				ZuneLibraryCDRecorder zuneLibraryCDRecorder = new ZuneLibraryCDRecorder(ptr);
				IRecordManager* intPtr = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
				zuneLibraryCDRecorder?.AddRef();
				return zuneLibraryCDRecorder;
			}
			return null;
		}

		public unsafe ZuneQueryList GetTracksByArtist(int LibraryView, int ArtistId, EQuerySortType SortOrder, uint SortAtom)
		{
			//IL_0003: Expected I, but got I8
			//IL_001e: Expected I, but got I8
			//IL_0030: Expected I, but got I8
			//IL_0041: Expected I, but got I8
			//IL_0052: Expected I, but got I8
			//IL_005f: Expected I, but got I8
			//IL_0071: Expected I, but got I8
			//IL_0075: Expected I, but got I8
			//IL_0090: Expected I, but got I8
			IDatabaseQueryResults* ptr = null;
			IQueryPropertyBag* ptr2;
			if (Module.CreatePropertyBag(&ptr2) < 0)
			{
				return null;
			}
			IQueryPropertyBag* intPtr = ptr2;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)ptr2 + 56)))((nint)intPtr, (EQueryPropertyBagProp)3, ArtistId);
			IQueryPropertyBag* intPtr2 = ptr2;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)ptr2 + 56)))((nint)intPtr2, (EQueryPropertyBagProp)21, (int)SortAtom);
			IQueryPropertyBag* intPtr3 = ptr2;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)ptr2 + 56)))((nint)intPtr3, (EQueryPropertyBagProp)22, (int)SortOrder);
			IQueryPropertyBag* intPtr4 = ptr2;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)ptr2 + 56)))((nint)intPtr4, (EQueryPropertyBagProp)15, LibraryView);
			int num = Module.QueryDatabase(EQueryType.eQueryTypeTracksForAlbumArtistId, ptr2, &ptr, null);
			if (0L != (nint)ptr2)
			{
				IQueryPropertyBag* intPtr5 = ptr2;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr5 + 16)))((nint)intPtr5);
				ptr2 = null;
			}
			if (num >= 0)
			{
				ZuneQueryList result = new ZuneQueryList(ptr, "TracksByArtist");
				IDatabaseQueryResults* intPtr6 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr6 + 16)))((nint)intPtr6);
				return result;
			}
			return null;
		}

		public unsafe ZuneQueryList GetTracksByAlbum(int LibraryView, int AlbumId, EQuerySortType SortOrder, uint SortAtom)
		{
			//IL_0003: Expected I, but got I8
			//IL_0006: Expected I, but got I8
			//IL_0021: Expected I, but got I8
			//IL_0033: Expected I, but got I8
			//IL_0044: Expected I, but got I8
			//IL_0055: Expected I, but got I8
			//IL_0062: Expected I, but got I8
			//IL_0074: Expected I, but got I8
			//IL_0078: Expected I, but got I8
			//IL_0093: Expected I, but got I8
			IDatabaseQueryResults* ptr = null;
			IQueryPropertyBag* ptr2 = null;
			if (Module.CreatePropertyBag(&ptr2) < 0)
			{
				return null;
			}
			IQueryPropertyBag* intPtr = ptr2;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)ptr2 + 56)))((nint)intPtr, (EQueryPropertyBagProp)6, AlbumId);
			IQueryPropertyBag* intPtr2 = ptr2;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)ptr2 + 56)))((nint)intPtr2, (EQueryPropertyBagProp)21, (int)SortAtom);
			IQueryPropertyBag* intPtr3 = ptr2;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)ptr2 + 56)))((nint)intPtr3, (EQueryPropertyBagProp)22, (int)SortOrder);
			IQueryPropertyBag* intPtr4 = ptr2;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)ptr2 + 56)))((nint)intPtr4, (EQueryPropertyBagProp)15, LibraryView);
			int num = Module.QueryDatabase(EQueryType.eQueryTypeTracksForAlbumId, ptr2, &ptr, null);
			if (0L != (nint)ptr2)
			{
				IQueryPropertyBag* intPtr5 = ptr2;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr5 + 16)))((nint)intPtr5);
				ptr2 = null;
			}
			if (num >= 0)
			{
				ZuneQueryList result = new ZuneQueryList(ptr, "TracksByAlbum");
				IDatabaseQueryResults* intPtr6 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr6 + 16)))((nint)intPtr6);
				return result;
			}
			return null;
		}

		public ZuneQueryList GetTracksByArtists(IList artistIds, string sort)
		{
			return ExecuteQueryHelper(EQueryType.eQueryTypeTracksForAlbumArtistId, (EQueryPropertyBagProp)4, artistIds, sort);
		}

		public ZuneQueryList GetTracksByGenres(IList genreIds, string sort)
		{
			return ExecuteQueryHelper(EQueryType.eQueryTypeTracksByGenreId, (EQueryPropertyBagProp)12, genreIds, sort);
		}

		public ZuneQueryList GetTracksByAlbums(IList albumIds, string sort)
		{
			return ExecuteQueryHelper(EQueryType.eQueryTypeTracksForAlbumId, (EQueryPropertyBagProp)7, albumIds, sort);
		}

		public unsafe ZuneQueryList GetTracksByPlaylist(int LibraryView, int PlaylistId, EQuerySortType SortOrder, uint SortAtom)
		{
			//IL_0003: Expected I, but got I8
			//IL_001f: Expected I, but got I8
			//IL_0031: Expected I, but got I8
			//IL_0042: Expected I, but got I8
			//IL_0053: Expected I, but got I8
			//IL_0057: Expected I, but got I8
			//IL_0063: Expected I, but got I8
			//IL_0075: Expected I, but got I8
			//IL_0079: Expected I, but got I8
			//IL_0094: Expected I, but got I8
			IQueryPropertyBag* ptr = null;
			if (Module.CreatePropertyBag(&ptr) < 0)
			{
				return null;
			}
			IQueryPropertyBag* intPtr = ptr;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)ptr + 56)))((nint)intPtr, (EQueryPropertyBagProp)10, PlaylistId);
			IQueryPropertyBag* intPtr2 = ptr;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)ptr + 56)))((nint)intPtr2, (EQueryPropertyBagProp)21, (int)SortAtom);
			IQueryPropertyBag* intPtr3 = ptr;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)ptr + 56)))((nint)intPtr3, (EQueryPropertyBagProp)22, (int)SortOrder);
			IQueryPropertyBag* intPtr4 = ptr;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)ptr + 56)))((nint)intPtr4, (EQueryPropertyBagProp)15, LibraryView);
			IDatabaseQueryResults* ptr2 = null;
			int num = Module.QueryDatabase(EQueryType.eQueryTypePlaylistContentByPlaylistId, ptr, &ptr2, null);
			if (0L != (nint)ptr)
			{
				IQueryPropertyBag* intPtr5 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr5 + 16)))((nint)intPtr5);
				ptr = null;
			}
			if (num >= 0)
			{
				ZuneQueryList result = new ZuneQueryList(ptr2, "TracksByPlaylist");
				IDatabaseQueryResults* intPtr6 = ptr2;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr6 + 16)))((nint)intPtr6);
				return result;
			}
			return null;
		}

		public ZuneQueryList GetAlbumsByArtists(IList artistIds, string sort)
		{
			return ExecuteQueryHelper(EQueryType.eQueryTypeAlbumsForAlbumArtistId, (EQueryPropertyBagProp)4, artistIds, sort);
		}

		public ZuneQueryList GetAlbumsByGenres(IList genreIds, string sort)
		{
			return ExecuteQueryHelper(EQueryType.eQueryTypeAlbumsByGenreId, (EQueryPropertyBagProp)12, genreIds, sort);
		}

		public unsafe AlbumMetadata GetAlbumMetadata(int iAlbumId)
		{
			//IL_0003: Expected I, but got I8
			//IL_002f: Expected I, but got I8
			IAlbumInfo* ptr = null;
			int num = Module.GetAlbumMetadata(iAlbumId, true, &ptr);
			if (num != 0)
			{
				throw new COMException("IAlbumInfo::GetAlbumMetadata failed", num);
			}
			AlbumMetadata result = new AlbumMetadata(ptr);
			IAlbumInfo* intPtr = ptr;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
			return result;
		}

		public unsafe void UpdateAlbumMetadata(int iAlbumId, AlbumMetadata albumMetadata)
		{
			if (albumMetadata == null)
			{
				throw new COMException("UpdateAlbumMetadata: albumMetadata is null", -2147024809);
			}
			IAlbumInfo* albumInfo = albumMetadata.AlbumInfo;
			int num = Module.UpdateAlbumMetadata(iAlbumId, albumInfo);
			if (num != 0)
			{
				throw new COMException("IAlbumInfo::UpdateAlbumMetadata failed", num);
			}
		}

		public unsafe void GetAlbumMetadataForAlbumId(long WMISAlbumId, int WMISVolume, AlbumMetadata dbAlbumMetadata, GetAlbumForAlbumIdCompleteHandler handler)
		{
			//IL_0024: Expected I, but got I8
			//IL_0030: Expected I, but got I8
			//IL_0051: Expected I, but got I8
			if (!(handler == null))
			{
				WMISGetAlbumForAlbumIdCallbackWrapper* ptr = (WMISGetAlbumForAlbumIdCallbackWrapper*)Module.@new(24uL);
				WMISGetAlbumForAlbumIdCallbackWrapper* callback;
				try
				{
					callback = ((ptr == null) ? null : Module.MicrosoftZuneLibrary_002EWMISGetAlbumForAlbumIdCallbackWrapper_002E_007Bctor_007D(ptr, handler));
				}
				catch
				{
					//try-fault
					Module.delete(ptr);
					throw;
				}
				IAlbumInfo* albumInfo = null;
				if (dbAlbumMetadata != null)
				{
					albumInfo = dbAlbumMetadata.AlbumInfo;
				}
				Module.GetAlbumMetadataForAlbumId(WMISAlbumId, WMISVolume, albumInfo, (IWMISGetAlbumForAlbumIdCallback*)callback);
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)callback + 16)))((nint)callback);
			}
		}

		public static void SplitAudioTrack(int iTrackMediaId)
		{
			int num = Module.SplitAudioTrack(iTrackMediaId);
			if (num < 0)
			{
				throw new COMException("ZuneLibraryExports::SplitAudioTrack failed", num);
			}
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool CanAddFromFolder(string folder)
		{
			ManagedLock managedLock = null;
			ManagedLock managedLock2 = new ManagedLock(m_shutdownLock);
			bool result;
			try
			{
				managedLock = managedLock2;
				if (!s_fShutdown)
				{
					fixed (char* folderPtr = folder.ToCharArray())
					{
						ushort* ptr = (ushort*)folderPtr;
						try
						{
							result = Module.CanAddFromFolder(ptr) == 0;
						}
						catch
						{
							//try-fault
							ptr = null;
							throw;
						}
					}
					goto IL_003c;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)managedLock).Dispose();
				throw;
			}
			((IDisposable)managedLock).Dispose();
			return false;
			IL_003c:
			((IDisposable)managedLock).Dispose();
			return result;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool CanAddMedia(string filename, EMediaTypes mediaType)
		{
			ManagedLock managedLock = null;
			ManagedLock managedLock2 = new ManagedLock(m_shutdownLock);
			bool result;
			try
			{
				managedLock = managedLock2;
				if (!s_fShutdown)
				{
					fixed (char* filenamePtr = filename.ToCharArray())
					{
						ushort* ptr = (ushort*)filenamePtr;
						try
						{
							result = (byte)((Module.CanAddMedia(ptr, mediaType) != 0) ? 1u : 0u) != 0;
						}
						catch
						{
							//try-fault
							ptr = null;
							throw;
						}
					}
					goto IL_0041;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)managedLock).Dispose();
				throw;
			}
			((IDisposable)managedLock).Dispose();
			return false;
			IL_0041:
			((IDisposable)managedLock).Dispose();
			return result;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool AddGrovelerScanDirectory(string path, EMediaTypes mediaType)
		{
			ManagedLock managedLock = null;
			ManagedLock managedLock2 = new ManagedLock(m_shutdownLock);
			bool result;
			try
			{
				managedLock = managedLock2;
				if (!s_fShutdown)
				{
					fixed (char* pathPtr = path.ToCharArray())
					{
						ushort* ptr = (ushort*)pathPtr;
						try
						{
							result = Module.AddGrovelerScanDirectory(ptr, mediaType) == 0;
						}
						catch
						{
							//try-fault
							ptr = null;
							throw;
						}
					}
					goto done;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)managedLock).Dispose();
				throw;
			}
			((IDisposable)managedLock).Dispose();
			return false;
			done:
			((IDisposable)managedLock).Dispose();
			return result;
		}

		public unsafe int AddMedia(string filename)
		{
			//IL_0030: Expected I, but got I8
			//IL_003d: Expected I, but got I8
			//IL_006d: Expected I, but got I8
			//IL_007e: Expected I, but got I8
			ManagedLock managedLock = null;
			int num = -1;
			int num2 = 0;
			ManagedLock managedLock2 = new ManagedLock(m_shutdownLock);
			int result;
			try
			{
				managedLock = managedLock2;
				if (!s_fShutdown)
				{
					fixed (char* filenamePtr = filename.ToCharArray())
					{
						ushort* ptr = (ushort*)filenamePtr;
						try
						{
							num2 = Module.AddMedia(ptr, EMediaTypes.eMediaTypeInvalid, 0u, null, &num);
						}
						catch
						{
							//try-fault
							ptr = null;
							throw;
						}
					}
				}
				IMetadataManager* ptr2 = null;
				if (num2 >= 0)
				{
					if (!s_fShutdown)
					{
						num2 = Module.GetSingleton(Module.GUID_IMetadataManager, (void**)(&ptr2));
					}
					if (num2 >= 0 && !s_fShutdown)
					{
						IMetadataManager* intPtr = ptr2;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)intPtr + 32)))((nint)intPtr);
					}
					if (ptr2 != null)
					{
						IMetadataManager* intPtr2 = ptr2;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
					}
				}
				result = num;
			}
			catch
			{
				//try-fault
				((IDisposable)managedLock).Dispose();
				throw;
			}
			((IDisposable)managedLock).Dispose();
			return result;
		}

		public unsafe int AddTrack(Guid guidTrackServiceMediaId, Guid guidAlbumServiceMediaId, int iTrackNumber, string strTitle, TimeSpan duration, string strAlbum, string strArtist, string strGenre)
		{
			//IL_0032: Expected I, but got I8
			//IL_006d: Expected I, but got I8
			int result = -1;
			fixed (char* strTitlePtr = strTitle.ToCharArray())
			{
				ushort* title = (ushort*)strTitlePtr;
				fixed (char* strAlbumPtr = strAlbum.ToCharArray())
				{
					ushort* album = (ushort*)strAlbumPtr;
					fixed (char* strArtistPtr = strArtist.ToCharArray())
					{
						ushort* artist = (ushort*)strArtistPtr;
						fixed (char* strGenrePtr = strGenre.ToCharArray())
						{
							ushort* genre = (ushort*)strGenrePtr;
							int durationMill = (int)duration.TotalMilliseconds;
							IMSMediaSchemaPropertySet* set = null;
							_GUID gUID = guidAlbumServiceMediaId;
							if (Module.CreateTrackPropSet(guidTrackServiceMediaId, gUID, iTrackNumber, title, durationMill, album, artist, genre, &set) >= 0)
							{
								Module.AddMedia(set, EMediaTypes.eMediaTypeAudio, &result);
							}
							if (set != null)
							{
								IMSMediaSchemaPropertySet* intPtr = set;
								((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
							}
							return result;
						}
					}
				}
			}
		}

		public unsafe int AddVideo(Guid guidVideoMediaId, string strTitle, TimeSpan duration)
		{
			//IL_0034: Expected I, but got I8
			int num = -1;
			fixed (char* strTitlePtr = strTitle.ToCharArray())
			{
				ushort* title = (ushort*)strTitlePtr;
				int durationMill = (int)duration.TotalMilliseconds;
				CComPtrNtv_003CIMSMediaSchemaPropertySet_003E cComPtrNtv_003CIMSMediaSchemaPropertySet_003E;
				*(long*)(&cComPtrNtv_003CIMSMediaSchemaPropertySet_003E) = 0L;
				int result;
				try
				{
					if (Module.CreateVideoPropSet(guidVideoMediaId, title, durationMill, (IMSMediaSchemaPropertySet**)(&cComPtrNtv_003CIMSMediaSchemaPropertySet_003E)) >= 0)
					{
						Module.AddMedia((IMSMediaSchemaPropertySet*)(*(ulong*)(&cComPtrNtv_003CIMSMediaSchemaPropertySet_003E)), EMediaTypes.eMediaTypeVideo, &num);
					}
					result = num;
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIMSMediaSchemaPropertySet_003E*, void>)(&Module.CComPtrNtv_003CIMSMediaSchemaPropertySet_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIMSMediaSchemaPropertySet_003E);
					throw;
				}
				Module.CComPtrNtv_003CIMSMediaSchemaPropertySet_003E_002ERelease(&cComPtrNtv_003CIMSMediaSchemaPropertySet_003E);
				return result;
			}
		}

		public unsafe int AddAlbum(Guid guidServiceMediaId, string strAlbum, string strArtist)
		{
			//IL_0013: Expected I, but got I8
			//IL_0040: Expected I, but got I8
			int result = -1;
			fixed (char* strAlbumPtr = strAlbum.ToCharArray())
			{
				ushort* album = (ushort*)strAlbumPtr;
				fixed (char* strArtistPtr = strArtist.ToCharArray())
				{
					ushort* artist = (ushort*)strArtistPtr;
					IMSMediaSchemaPropertySet* propSet = null;
					if (Module.CreateAlbumPropSet(guidServiceMediaId, album, artist, &propSet) >= 0)
					{
						Module.AddMedia(propSet, EMediaTypes.eMediaTypeAudioAlbum, &result);
					}
					if (propSet != null)
					{
						IMSMediaSchemaPropertySet* intPtr = propSet;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
					}
					return result;
				}
			}
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool AddTransientMedia(string filename, EMediaTypes mediaType, out int libraryID, out bool fFileAlreadyExists)
		{
			libraryID = -1;
			fixed (char* filenamePtr = filename.ToCharArray())
			{
				ushort* ptr = (ushort*)filenamePtr;
				fixed (int* ptr2 = &libraryID)
				{
					int num = Module.AddTransientMedia(ptr, mediaType, ptr2);
					if (num < 0)
					{
						Module.SQMAddNumbersToStream("IgnoredErrorEvent", 1u, (uint)num);
					}
					int num2 = ((fFileAlreadyExists = num == 1) ? 1 : 0);
					return (byte)((num >= 0) ? 1u : 0u) != 0;
				}
			}
		}

		public void CleanupTransientMedia()
		{
			int num = Module.CleanupTransientMedia();
			if (num < 0)
			{
				Module.SQMAddNumbersToStream("IgnoredErrorEvent", 1u, (uint)num);
			}
		}

		public void MarkAllDRMFilesAsNeedingLicenseRefresh()
		{
			int num = Module.MarkAllDRMFilesAsNeedingLicenseRefresh();
			if (num < 0)
			{
				Module.SQMAddNumbersToStream("IgnoredErrorEvent", 1u, (uint)num);
			}
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool DeleteMedia(int[] mediaIds, EMediaTypes mediaType, [MarshalAs(UnmanagedType.U1)] bool fDeleteFileOnDisk)
		{
			if (mediaIds == null)
			{
				return false;
			}
			fixed (int* mediaIdsPtr = &mediaIds[0])
			{
				return (byte)((Module.DeleteMedia(mediaType, mediaIdsPtr, mediaIds.Length, fDeleteFileOnDisk ? 1 : 0, 1) >= 0) ? 1u : 0u) != 0;
			}
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool DeleteRootFolder(string folderName, EMediaTypes mediaType)
		{
			fixed (char* folderNamePtr = folderName.ToCharArray())
			{
				int num = Module.DeleteRootFolder((ushort*)folderNamePtr, mediaType);
				if (num < 0)
				{
					Module.SQMAddNumbersToStream("IgnoredErrorEvent", 1u, (uint)num);
				}
				return num == 0;
			}
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public bool DeleteFilesystemFolder(int folderId, EMediaTypes mediaType)
		{
			return Module.DeleteFSFolder(folderId, mediaType) == 0;
		}

		public static void ScanAndClearDeletedMedia()
		{
			int num = Module.ScanAndClearDeletedMedia();
			if (num < 0)
			{
				Module.SQMAddNumbersToStream("IgnoredErrorEvent", 1u, (uint)num);
			}
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public static bool ImportSharedRatingsForUser(int iUserId, EMediaTypes mediaType)
		{
			return (byte)((Module.ImportSharedRatingsForUser(iUserId, mediaType) >= 0) ? 1u : 0u) != 0;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public static bool ExportUserRatings(int iUserId, EMediaTypes mediaType)
		{
			return (byte)((Module.ExportUserRatings(iUserId, mediaType) >= 0) ? 1u : 0u) != 0;
		}

		public static HRESULT GetFieldValues(int iMediaId, EListType eList, int cValues, int[] columnIndexes, object[] fieldValues, QueryPropertyBag propertyBag)
		{
			return GetFieldValues(iMediaId, eList, cValues, columnIndexes, fieldValues, null, propertyBag);
		}

		public unsafe static HRESULT GetFieldValues(int iMediaId, EListType eList, int cValues, int[] columnIndexes, object[] fieldValues, bool[] isEmptyValues, QueryPropertyBag propertyBag)
		{
			//IL_0074: Expected I, but got I8
			//IL_0096: Expected I, but got I8
			//IL_00d1: Expected I, but got I8
			//IL_00de: Expected I, but got I8
			//IL_00fa: Expected I, but got I8
			//IL_0111: Expected I, but got I8
			if (propertyBag == null)
			{
				return new HRESULT(-2147024809);
			}
			int num = 0;
			ulong num2 = (ulong)cValues;
			ulong num3 = ((num2 > 576460752303423487L) ? ulong.MaxValue : (num2 * 32));
			DBPropertyRequestStruct* ptr = (DBPropertyRequestStruct*)Module.new_005B_005D((num3 > 18446744073709551607uL) ? ulong.MaxValue : (num3 + 8));
			DBPropertyRequestStruct* ptr3;
			try
			{
				if (ptr != null)
				{
					uint count = (uint)(*(int*)ptr = (int)num2);
					DBPropertyRequestStruct* ptr2 = (DBPropertyRequestStruct*)((ulong)(nint)ptr + 8uL);
					Module.__ehvec_ctor(ptr2, 32uL, (int)count, (delegate*<void*, void>)(delegate*<DBPropertyRequestStruct*, void>)(&Module.DBPropertyRequestStruct_002E__dflt_ctor_closure), (delegate*<void*, void>)(delegate*<DBPropertyRequestStruct*, void>)(&Module.DBPropertyRequestStruct_002E_007Bdtor_007D));
					ptr3 = ptr2;
				}
				else
				{
					ptr3 = null;
				}
			}
			catch
			{
				//try-fault
				Module.delete_005B_005D(ptr);
				throw;
			}
			DBPropertyRequestStruct* ptr4 = ptr3;
			CComPtrNtv_003CIQueryPropertyBag_003E cComPtrNtv_003CIQueryPropertyBag_003E;
			Module.CComPtrNtv_003CIQueryPropertyBag_003E_002E_007Bctor_007D(&cComPtrNtv_003CIQueryPropertyBag_003E, propertyBag.GetIQueryPropertyBag());
			HRESULT result;
			try
			{
				try
				{
					for (int i = 0; i < cValues; i++)
					{
						*(int*)(i * 32L + (nint)ptr4) = columnIndexes[i];
					}
					IQueryPropertyBag* ptr5 = (IQueryPropertyBag*)(*(ulong*)(&cComPtrNtv_003CIQueryPropertyBag_003E));
					num = Module.GetFieldValues(iMediaId, eList, cValues, ptr4, (IQueryPropertyBag*)(*(ulong*)(&cComPtrNtv_003CIQueryPropertyBag_003E)));
					if (num >= 0)
					{
						for (int j = 0; j < cValues; j++)
						{
							DBPropertyRequestStruct* ptr6 = (DBPropertyRequestStruct*)(j * 32L + (nint)ptr4);
							int num4 = *(int*)((ulong)(nint)ptr6 + 4uL);
							if (num4 >= 0)
							{
								DBPropertyRequestStruct* ptr7 = (DBPropertyRequestStruct*)((ulong)(nint)ptr6 + 8uL);
								ushort num5 = *(ushort*)ptr7;
								if (num5 != 1 && num5 != 0)
								{
									Type type = null;
									if (fieldValues[j] != null)
									{
										type = fieldValues[j].GetType();
									}
									else
									{
										switch (num5)
										{
										case 20:
											type = typeof(long);
											break;
										case 11:
											type = typeof(bool);
											break;
										case 8:
											type = typeof(string);
											break;
										case 3:
											type = typeof(int);
											break;
										}
									}
									fieldValues[j] = ZuneQueryList.MarshalResult(type, (PROPVARIANT)ptr7, fieldValues[j]);
								}
								else if (isEmptyValues != null)
								{
									isEmptyValues[j] = true;
								}
							}
							else
							{
								num = ((num >= 0) ? num4 : num);
							}
						}
					}
				}
				finally
				{
					if (ptr4 != null)
					{
						Module.DBPropertyRequestStruct_002E__vecDelDtor(ptr4, 3u);
					}
				}
				result = new HRESULT(num);
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIQueryPropertyBag_003E*, void>)(&Module.CComPtrNtv_003CIQueryPropertyBag_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIQueryPropertyBag_003E);
				throw;
			}
			Module.CComPtrNtv_003CIQueryPropertyBag_003E_002ERelease(&cComPtrNtv_003CIQueryPropertyBag_003E);
			return result;
		}

		public unsafe static void SetFieldValues(int iMediaId, EListType eList, int cValues, int[] columnIndexes, object[] fieldValues, QueryPropertyBag propertyBag)
		{
			//Discarded unreachable code: IL_0197
			//IL_0066: Expected I, but got I8
			//IL_0088: Expected I, but got I8
			//IL_00ef: Expected I, but got I8
			//IL_0111: Expected I, but got I8
			//IL_0141: Expected I, but got I8
			//IL_0152: Expected I, but got I8
			//IL_0167: Expected I8, but got I
			//IL_0179: Expected I, but got I8
			if (propertyBag == null)
			{
				return;
			}
			ulong num = (ulong)cValues;
			ulong num2 = num;
			ulong num3 = ((num2 > 1152921504606846975L) ? ulong.MaxValue : (num2 * 16));
			DBPropertySubmitStruct* ptr = (DBPropertySubmitStruct*)Module.new_005B_005D((num3 > 18446744073709551607uL) ? ulong.MaxValue : (num3 + 8));
			DBPropertySubmitStruct* ptr3;
			try
			{
				if (ptr != null)
				{
					uint count = (uint)(*(int*)ptr = (int)num2);
					DBPropertySubmitStruct* ptr2 = (DBPropertySubmitStruct*)((ulong)(nint)ptr + 8uL);
					Module.__ehvec_ctor(ptr2, 16uL, (int)count, (delegate*<void*, void>)(delegate*<DBPropertySubmitStruct*, void>)(&Module.DBPropertySubmitStruct_002E__dflt_ctor_closure), (delegate*<void*, void>)(delegate*<DBPropertySubmitStruct*, void>)(&Module.DBPropertySubmitStruct_002E_007Bdtor_007D));
					ptr3 = ptr2;
				}
				else
				{
					ptr3 = null;
				}
			}
			catch
			{
				//try-fault
				Module.delete_005B_005D(ptr);
				throw;
			}
			DBPropertySubmitStruct* ptr4 = ptr3;
			ulong num4 = num;
			ulong num5 = ((num4 > 768614336404564650L) ? ulong.MaxValue : (num4 * 24));
			PROPVARIANT* ptr5 = (PROPVARIANT*)Module.new_005B_005D((num5 > 18446744073709551607uL) ? ulong.MaxValue : (num5 + 8));
			PROPVARIANT* ptr7;
			try
			{
				if (ptr5 != null)
				{
					uint count2 = (uint)(*(int*)ptr5 = (int)num4);
					PROPVARIANT* ptr6 = (PROPVARIANT*)((ulong)(nint)ptr5 + 8uL);
					Module.__ehvec_ctor(ptr6, 24uL, (int)count2, (delegate*<void*, void>)(delegate*<PROPVARIANT*, PROPVARIANT*>)(&Module.CComPropVariant_002E_007Bctor_007D), (delegate*<void*, void>)(delegate*<PROPVARIANT*, void>)(&Module.CComPropVariant_002E_007Bdtor_007D));
					ptr7 = ptr6;
				}
				else
				{
					ptr7 = null;
				}
			}
			catch
			{
				//try-fault
				Module.delete_005B_005D(ptr5);
				throw;
			}
			PROPVARIANT* ptr8 = ptr7;
			CComPtrNtv_003CIQueryPropertyBag_003E cComPtrNtv_003CIQueryPropertyBag_003E;
			Module.CComPtrNtv_003CIQueryPropertyBag_003E_002E_007Bctor_007D(&cComPtrNtv_003CIQueryPropertyBag_003E, propertyBag.GetIQueryPropertyBag());
			try
			{
				try
				{
					for (int i = 0; i < cValues; i++)
					{
						long num6 = i;
						DBPropertySubmitStruct* ptr9 = (DBPropertySubmitStruct*)(num6 * 16 + (nint)ptr4);
						*(int*)ptr9 = columnIndexes[i];
						PROPVARIANT* ptr10 = (PROPVARIANT*)(num6 * 24 + (nint)ptr8);
                        ZuneQueryList.ConvertTypeToPropVariant(null, fieldValues[i], (PROPVARIANT)ptr10);
						*(long*)((ulong)(nint)ptr9 + 8uL) = (nint)ptr10;
					}
					Module.SetFieldValues(iMediaId, eList, cValues, ptr4, (IQueryPropertyBag*)(*(ulong*)(&cComPtrNtv_003CIQueryPropertyBag_003E)));
				}
				finally
				{
					if (ptr4 != null)
					{
						Module.DBPropertySubmitStruct_002E__vecDelDtor(ptr4, 3u);
					}
					if (ptr8 != null)
					{
						Module.CComPropVariant_002E__vecDelDtor(ptr8, 3u);
					}
				}
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIQueryPropertyBag_003E*, void>)(&Module.CComPtrNtv_003CIQueryPropertyBag_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIQueryPropertyBag_003E);
				throw;
			}
			Module.CComPtrNtv_003CIQueryPropertyBag_003E_002ERelease(&cComPtrNtv_003CIQueryPropertyBag_003E);
		}

		public unsafe int GetKnownFolders(out string[] music, out string[] videos, out string[] pictures, out string[] podcasts, out string[] applications, out string ripFolder, out string videoMediaFolder, out string photoMediaFolder, out string podcastMediaFolder, out string applicationsFolder)
		{
			//IL_002c: Expected I, but got I8
			//IL_002f: Expected I, but got I8
			//IL_0032: Expected I, but got I8
			//IL_0035: Expected I, but got I8
			//IL_0038: Expected I, but got I8
			DynamicArray_003Cunsigned_0020short_0020_002A_003E dynMusic;
			Module.DynamicArray_003Cunsigned_0020short_0020_002A_003E_002E_007Bctor_007D(&dynMusic);
			int result;
			try
			{
				DynamicArray_003Cunsigned_0020short_0020_002A_003E dynVideos;
				Module.DynamicArray_003Cunsigned_0020short_0020_002A_003E_002E_007Bctor_007D(&dynVideos);
				try
				{
					DynamicArray_003Cunsigned_0020short_0020_002A_003E dynPictures;
					Module.DynamicArray_003Cunsigned_0020short_0020_002A_003E_002E_007Bctor_007D(&dynPictures);
					try
					{
						DynamicArray_003Cunsigned_0020short_0020_002A_003E dynPodcasts;
						Module.DynamicArray_003Cunsigned_0020short_0020_002A_003E_002E_007Bctor_007D(&dynPodcasts);
						try
						{
							DynamicArray_003Cunsigned_0020short_0020_002A_003E dynApplications;
							Module.DynamicArray_003Cunsigned_0020short_0020_002A_003E_002E_007Bctor_007D(&dynApplications);
							try
							{
								ushort* ripFolderPtr = null;
								ushort* videoPtr = null;
								ushort* photoPtr = null;
								ushort* podcastPtr = null;
								ushort* applicationPtr = null;
								result = Module.GetKnownFolders(&dynMusic, &dynVideos, &dynPictures, &dynPodcasts, &dynApplications, &ripFolderPtr, &videoPtr, &photoPtr, &podcastPtr, &applicationPtr);
								music = Module.BstrArrayToStringArray(&dynMusic);
								videos = Module.BstrArrayToStringArray(&dynVideos);
								pictures = Module.BstrArrayToStringArray(&dynPictures);
								podcasts = Module.BstrArrayToStringArray(&dynPodcasts);
								applications = Module.BstrArrayToStringArray(&dynApplications);
								ripFolder = new string((char*)ripFolderPtr);
								Module.SysFreeString(ripFolderPtr);
								videoMediaFolder = new string((char*)videoPtr);
								Module.SysFreeString(videoPtr);
								photoMediaFolder = new string((char*)photoPtr);
								Module.SysFreeString(photoPtr);
								podcastMediaFolder = new string((char*)podcastPtr);
								Module.SysFreeString(podcastPtr);
								applicationsFolder = new string((char*)applicationPtr);
								Module.SysFreeString(applicationPtr);
							}
							catch
							{
								//try-fault
								Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<DynamicArray_003Cunsigned_0020short_0020_002A_003E*, void>)(&Module.DynamicArray_003Cunsigned_0020short_0020_002A_003E_002E_007Bdtor_007D), &dynApplications);
								throw;
							}
							Module.DynamicArray_003Cunsigned_0020short_0020_002A_003E_002E_007Bdtor_007D(&dynApplications);
						}
						catch
						{
							//try-fault
							Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<DynamicArray_003Cunsigned_0020short_0020_002A_003E*, void>)(&Module.DynamicArray_003Cunsigned_0020short_0020_002A_003E_002E_007Bdtor_007D), &dynPodcasts);
							throw;
						}
						Module.DynamicArray_003Cunsigned_0020short_0020_002A_003E_002E_007Bdtor_007D(&dynPodcasts);
					}
					catch
					{
						//try-fault
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<DynamicArray_003Cunsigned_0020short_0020_002A_003E*, void>)(&Module.DynamicArray_003Cunsigned_0020short_0020_002A_003E_002E_007Bdtor_007D), &dynPictures);
						throw;
					}
					Module.DynamicArray_003Cunsigned_0020short_0020_002A_003E_002E_007Bdtor_007D(&dynPictures);
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<DynamicArray_003Cunsigned_0020short_0020_002A_003E*, void>)(&Module.DynamicArray_003Cunsigned_0020short_0020_002A_003E_002E_007Bdtor_007D), &dynVideos);
					throw;
				}
				Module.DynamicArray_003Cunsigned_0020short_0020_002A_003E_002E_007Bdtor_007D(&dynVideos);
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<DynamicArray_003Cunsigned_0020short_0020_002A_003E*, void>)(&Module.DynamicArray_003Cunsigned_0020short_0020_002A_003E_002E_007Bdtor_007D), &dynMusic);
				throw;
			}
			Module.DynamicArray_003Cunsigned_0020short_0020_002A_003E_002E_007Bdtor_007D(&dynMusic);
			return result;
		}

		public unsafe int GetLocalizedPathOfFolder([In] string physicalPath, [In][MarshalAs(UnmanagedType.U1)] bool fNetworkPathsAllowed, out string localizedPath)
		{
			//IL_0003: Expected I, but got I8
			ushort* localizedPathPtr = null;
			localizedPath = null;
			fixed (char* physicalPathPtr = physicalPath.ToCharArray())
			{
				int num = Module.GetLocalizedPathOfFolder((ushort*)physicalPathPtr, fNetworkPathsAllowed, &localizedPathPtr);
				if (num >= 0)
				{
					localizedPath = new string((char*)localizedPathPtr);
					Module.SysFreeString(localizedPathPtr);
				}
				return num;
			}
		}

		public unsafe static int CompareWithoutArticles(string prefix, string @string)
		{
			fixed (char* prefixPtr = prefix.ToCharArray())
			{
				ushort* ptr = (ushort*)prefixPtr;
				fixed (char* @stringPtr = @string.ToCharArray())
				{
					ushort* ptr2 = (ushort*)@stringPtr;
					int num = ((Module.CompareWithoutArticles(ptr, ptr2, &num) != 0) ? (-1) : num);
					return num;
				}
			}
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe static bool DoesFileExist(string path)
		{
			fixed (char* pathPtr = path.ToCharArray())
			{
				ushort* ptr = (ushort*)pathPtr;
				return Module.DoesFileExist(ptr, out int exists) >= 0 && exists != 0;
			}
		}

		private unsafe ZuneQueryList ExecuteQueryHelper(EQueryType eQueryType, EQueryPropertyBagProp idPropType, IList ids, string sort)
		{
			//IL_000a: Expected I, but got I8
			//IL_003e: Expected I, but got I8
			//IL_003e: Expected I, but got I8
			//IL_0074: Expected I, but got I8
			//IL_0074: Expected I, but got I8
			//IL_008e: Expected I, but got I8
			//IL_008e: Expected I, but got I8
			//IL_00a7: Expected I, but got I8
			//IL_00a7: Expected I, but got I8
			//IL_00c2: Expected I, but got I8
			//IL_00c2: Expected I, but got I8
			//IL_00e0: Expected I, but got I8
			//IL_00e0: Expected I, but got I8
			//IL_00f4: Expected I, but got I8
			//IL_00f4: Expected I, but got I8
			//IL_0108: Expected I, but got I8
			//IL_0108: Expected I, but got I8
			//IL_0135: Expected I, but got I8
			//IL_0135: Expected I, but got I8
			//IL_0146: Expected I, but got I8
			//IL_014a: Expected I, but got I8
			//IL_016d: Expected I, but got I8
			//IL_017a: Expected I, but got I8
			//IL_0192: Expected I, but got I8
			//IL_0192: Expected I, but got I8
			//IL_01a0: Expected I, but got I8
			//IL_01a0: Expected I, but got I8
			//IL_01ca: Expected I, but got I8
			bool[] ascendings = null;
			string[] sorts = null;
			IDatabaseQueryResults* ptr = null;
			QueryPropertyBag queryPropertyBag = new QueryPropertyBag();
			CComPtrNtv_003CIQueryPropertyBag_003E cComPtrNtv_003CIQueryPropertyBag_003E;
			Module.CComPtrNtv_003CIQueryPropertyBag_003E_002E_007Bctor_007D(&cComPtrNtv_003CIQueryPropertyBag_003E, queryPropertyBag.GetIQueryPropertyBag());
			ZuneQueryList result;
			try
			{
				if (ids.Count == 1)
				{
					long num = *(long*)(&cComPtrNtv_003CIQueryPropertyBag_003E);
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIQueryPropertyBag_003E)) + 56)))((nint)num, (EQueryPropertyBagProp)15, 0);
					int num2 = (int)ids[0];
					switch (eQueryType)
					{
					default:
					{
						long num5 = *(long*)(&cComPtrNtv_003CIQueryPropertyBag_003E);
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIQueryPropertyBag_003E)) + 56)))((nint)num5, (EQueryPropertyBagProp)6, num2);
						break;
					}
					case EQueryType.eQueryTypeTracksByGenreId:
					case EQueryType.eQueryTypeAlbumsByGenreId:
					{
						long num4 = *(long*)(&cComPtrNtv_003CIQueryPropertyBag_003E);
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIQueryPropertyBag_003E)) + 56)))((nint)num4, (EQueryPropertyBagProp)11, num2);
						break;
					}
					case EQueryType.eQueryTypeAlbumsForAlbumArtistId:
					case EQueryType.eQueryTypeTracksForAlbumArtistId:
					{
						long num3 = *(long*)(&cComPtrNtv_003CIQueryPropertyBag_003E);
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIQueryPropertyBag_003E)) + 56)))((nint)num3, (EQueryPropertyBagProp)3, num2);
						break;
					}
					}
				}
				else
				{
					long num6 = *(long*)(&cComPtrNtv_003CIQueryPropertyBag_003E);
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIQueryPropertyBag_003E)) + 56)))((nint)num6, (EQueryPropertyBagProp)15, 150994944);
					long num7 = *(long*)(*(ulong*)(&cComPtrNtv_003CIQueryPropertyBag_003E)) + 24;
					long num8 = *(long*)(&cComPtrNtv_003CIQueryPropertyBag_003E);
					IDList* intPtr = queryPropertyBag.PackIDList(ids);
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, IDList*, int>)(*(ulong*)num7))((nint)num8, idPropType, intPtr);
					long num9 = *(long*)(&cComPtrNtv_003CIQueryPropertyBag_003E);
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIQueryPropertyBag_003E)) + 56)))((nint)num9, (EQueryPropertyBagProp)3, -1);
					long num10 = *(long*)(&cComPtrNtv_003CIQueryPropertyBag_003E);
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIQueryPropertyBag_003E)) + 56)))((nint)num10, (EQueryPropertyBagProp)6, -1);
				}
				if (LibraryDataProvider.GetSortAttributes(sort, out sorts, out ascendings))
				{
					IMultiSortAttributes* ptr2 = queryPropertyBag.PackMultiSortAttributes(sorts, ascendings);
					long num11 = *(long*)(&cComPtrNtv_003CIQueryPropertyBag_003E);
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, IMultiSortAttributes*, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIQueryPropertyBag_003E)) + 32)))((nint)num11, (EQueryPropertyBagProp)23, ptr2);
					if (ptr2 != null)
					{
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
					}
				}
				IService* ptr3 = null;
				if (Module.GetSingleton(Module.GUID_IService, (void**)(&ptr3)) >= 0)
				{
					IService* intPtr2 = ptr3;
					_GUID gUID;
					int num12;
					int num13 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, int*, int>)(*(ulong*)(*(long*)ptr3 + 384)))((nint)intPtr2, &gUID, &num12);
					IService* intPtr3 = ptr3;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr3 + 16)))((nint)intPtr3);
					if (num13 >= 0)
					{
						long num14 = *(long*)(&cComPtrNtv_003CIQueryPropertyBag_003E);
						int num15 = num12;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIQueryPropertyBag_003E)) + 56)))((nint)num14, 0, num15);
					}
				}
				int num16 = Module.QueryDatabase(eQueryType, (IQueryPropertyBag*)(*(ulong*)(&cComPtrNtv_003CIQueryPropertyBag_003E)), &ptr, null);
				((IDisposable)queryPropertyBag)?.Dispose();
				if (num16 >= 0)
				{
					result = new ZuneQueryList(ptr, "ExecuteQueryHelper");
					IDatabaseQueryResults* intPtr4 = ptr;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr4 + 16)))((nint)intPtr4);
					goto IL_01db;
				}
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIQueryPropertyBag_003E*, void>)(&Module.CComPtrNtv_003CIQueryPropertyBag_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIQueryPropertyBag_003E);
				throw;
			}
			ZuneQueryList result2;
			try
			{
				result2 = null;
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIQueryPropertyBag_003E*, void>)(&Module.CComPtrNtv_003CIQueryPropertyBag_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIQueryPropertyBag_003E);
				throw;
			}
			Module.CComPtrNtv_003CIQueryPropertyBag_003E_002ERelease(&cComPtrNtv_003CIQueryPropertyBag_003E);
			return result2;
			IL_01db:
			Module.CComPtrNtv_003CIQueryPropertyBag_003E_002ERelease(&cComPtrNtv_003CIQueryPropertyBag_003E);
			return result;
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_007EZuneLibrary();
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
