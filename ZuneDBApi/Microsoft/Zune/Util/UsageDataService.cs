using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Util
{
	public class UsageDataService
	{
		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe static bool GetPostUsageDataFlagForSignedInUser()
		{
			//IL_0005: Expected I, but got I8
			//IL_0025: Expected I, but got I8
			//IL_0042: Expected I, but got I8
			bool flag = false;
			IUsageDataManager* ptr = null;
			if (Module.GetSingleton((_GUID)Module._GUID_2f36e709_c431_4836_ab2b_ab57aef0cf1a, (void**)(&ptr)) >= 0)
			{
				int num = 0;
				IUsageDataManager* intPtr = ptr;
				if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)ptr + 24)))((nint)intPtr, &num) == 0)
				{
					flag = num != 0 || flag;
				}
			}
			if (0L != (nint)ptr)
			{
				IUsageDataManager* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
			return flag;
		}

		public unsafe static void SetPostUsageDataFlagForSignedInUser([MarshalAs(UnmanagedType.U1)] bool fCanPostUsageData)
		{
			//IL_0003: Expected I, but got I8
			//IL_0020: Expected I, but got I8
			//IL_0033: Expected I, but got I8
			IUsageDataManager* ptr = null;
			if (Module.GetSingleton((_GUID)Module._GUID_2f36e709_c431_4836_ab2b_ab57aef0cf1a, (void**)(&ptr)) >= 0)
			{
				IUsageDataManager* intPtr = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)ptr + 32)))((nint)intPtr, fCanPostUsageData ? 1 : 0);
			}
			if (0L != (nint)ptr)
			{
				IUsageDataManager* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
		}

		public unsafe static void ReportTrackSubscriptionPlayback(Guid guidTrackId, string strReferrer)
		{
			//IL_0003: Expected I, but got I8
			//IL_0032: Expected I, but got I8
			//IL_004e: Expected I, but got I8
			IUsageDataManager* ptr = null;
			if (Module.GetSingleton((_GUID)Module._GUID_2f36e709_c431_4836_ab2b_ab57aef0cf1a, (void**)(&ptr)) >= 0)
			{
				_GUID gUID = Module.GuidToGUID(guidTrackId);
				fixed (char* strReferrerPtr = strReferrer.ToCharArray())
				{
					ushort* ptr2 = (ushort*)strReferrerPtr;
					try
					{
						long num = *(long*)ptr + 104;
						IUsageDataManager* intPtr = ptr;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, ushort*, int>)(*(ulong*)num))((nint)intPtr, &gUID, ptr2);
					}
					catch
					{
						//try-fault
						ptr2 = null;
						throw;
					}
				}
			}
			if (0L != (nint)ptr)
			{
				IUsageDataManager* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
		}

		public unsafe static void ReportTrackPreviewPlayback(Guid guidTrackId, string strReferrer)
		{
			//IL_0003: Expected I, but got I8
			//IL_0032: Expected I, but got I8
			//IL_004e: Expected I, but got I8
			IUsageDataManager* ptr = null;
			if (Module.GetSingleton((_GUID)Module._GUID_2f36e709_c431_4836_ab2b_ab57aef0cf1a, (void**)(&ptr)) >= 0)
			{
				_GUID gUID = Module.GuidToGUID(guidTrackId);
				fixed (char* strReferrerPtr = strReferrer.ToCharArray())
				{
					ushort* ptr2 = (ushort*)strReferrerPtr;
					try
					{
						long num = *(long*)ptr + 112;
						IUsageDataManager* intPtr = ptr;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, ushort*, int>)(*(ulong*)num))((nint)intPtr, &gUID, ptr2);
					}
					catch
					{
						//try-fault
						ptr2 = null;
						throw;
					}
				}
			}
			if (0L != (nint)ptr)
			{
				IUsageDataManager* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
		}

		public unsafe static void ReportTrackSubscriptionSkipPlay(Guid guidTrackId, string strReferrer)
		{
			//IL_0003: Expected I, but got I8
			//IL_0032: Expected I, but got I8
			//IL_004e: Expected I, but got I8
			IUsageDataManager* ptr = null;
			if (Module.GetSingleton((_GUID)Module._GUID_2f36e709_c431_4836_ab2b_ab57aef0cf1a, (void**)(&ptr)) >= 0)
			{
				_GUID gUID = Module.GuidToGUID(guidTrackId);
				fixed (char* strReferrerPtr = strReferrer.ToCharArray())
				{
					ushort* ptr2 = (ushort*)strReferrerPtr;
					try
					{
						long num = *(long*)ptr + 120;
						IUsageDataManager* intPtr = ptr;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, ushort*, int>)(*(ulong*)num))((nint)intPtr, &gUID, ptr2);
					}
					catch
					{
						//try-fault
						ptr2 = null;
						throw;
					}
				}
			}
			if (0L != (nint)ptr)
			{
				IUsageDataManager* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
		}

		public unsafe static void ReportTrackPreviewSkipPlay(Guid guidTrackId, string strReferrer)
		{
			//IL_0003: Expected I, but got I8
			//IL_0035: Expected I, but got I8
			//IL_0051: Expected I, but got I8
			IUsageDataManager* ptr = null;
			if (Module.GetSingleton((_GUID)Module._GUID_2f36e709_c431_4836_ab2b_ab57aef0cf1a, (void**)(&ptr)) >= 0)
			{
				_GUID gUID = Module.GuidToGUID(guidTrackId);
				fixed (char* strReferrerPtr = strReferrer.ToCharArray())
				{
					ushort* ptr2 = (ushort*)strReferrerPtr;
					try
					{
						long num = *(long*)ptr + 128;
						IUsageDataManager* intPtr = ptr;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, ushort*, int>)(*(ulong*)num))((nint)intPtr, &gUID, ptr2);
					}
					catch
					{
						//try-fault
						ptr2 = null;
						throw;
					}
				}
			}
			if (0L != (nint)ptr)
			{
				IUsageDataManager* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
		}

		public unsafe static void ReportTrackAddToCollection(Guid guidMediaId, string strReferrer)
		{
			//IL_0003: Expected I, but got I8
			//IL_0035: Expected I, but got I8
			//IL_0051: Expected I, but got I8
			IUsageDataManager* ptr = null;
			if (Module.GetSingleton((_GUID)Module._GUID_2f36e709_c431_4836_ab2b_ab57aef0cf1a, (void**)(&ptr)) >= 0)
			{
				fixed (char* strReferrerPtr = strReferrer.ToCharArray())
				{
					ushort* ptr2 = (ushort*)strReferrerPtr;
					try
					{
						_GUID gUID = Module.GuidToGUID(guidMediaId);
						long num = *(long*)ptr + 136;
						IUsageDataManager* intPtr = ptr;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, ushort*, int>)(*(ulong*)num))((nint)intPtr, &gUID, ptr2);
					}
					catch
					{
						//try-fault
						ptr2 = null;
						throw;
					}
				}
			}
			if (0L != (nint)ptr)
			{
				IUsageDataManager* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
		}

		public unsafe static void ReportPlaylistDownload([MarshalAs(UnmanagedType.U1)] bool purchase, Guid guidMediaId)
		{
			//IL_0003: Expected I, but got I8
			//IL_0029: Expected I, but got I8
			//IL_003c: Expected I, but got I8
			IUsageDataManager* ptr = null;
			if (Module.GetSingleton((_GUID)Module._GUID_2f36e709_c431_4836_ab2b_ab57aef0cf1a, (void**)(&ptr)) >= 0)
			{
				_GUID gUID = Module.GuidToGUID(guidMediaId);
				IUsageDataManager* intPtr = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, _GUID*, int>)(*(ulong*)(*(long*)ptr + 40)))((nint)intPtr, purchase ? 1 : 0, &gUID);
			}
			if (0L != (nint)ptr)
			{
				IUsageDataManager* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
		}

		private UsageDataService()
		{
		}
	}
}
