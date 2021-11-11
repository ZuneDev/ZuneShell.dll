using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using ZuneUI;

namespace Microsoft.Zune.Util
{
	public class PhotoManager
	{
		private static PhotoManager sm_PhotoManager = null;

		private static object sm_lock = new object();

		public static PhotoManager Instance
		{
			get
			{
				if (sm_PhotoManager == null)
				{
					try
					{
						Monitor.Enter(sm_lock);
						if (sm_PhotoManager == null)
						{
							PhotoManager photoManager = new PhotoManager();
							Thread.MemoryBarrier();
							sm_PhotoManager = photoManager;
						}
					}
					finally
					{
						Monitor.Exit(sm_lock);
					}
				}
				return sm_PhotoManager;
			}
		}

		public unsafe HRESULT SetWindowHandle(IntPtr hWnd)
		{
			//IL_0005: Expected I, but got I8
			//IL_002f: Expected I, but got I8
			//IL_0044: Expected I, but got I8
			//IL_0048: Expected I, but got I8
			int num = 0;
			IMetadataManager* ptr = null;
			try
			{
				num = _003CModule_003E.GetSingleton((_GUID)_003CModule_003E._GUID_6dd7146d_7a19_4fbb_9235_9e6c382fcc71, (void**)(&ptr));
				if (num >= 0)
				{
					long num2 = *(long*)ptr + 440;
					IMetadataManager* intPtr = ptr;
					void* intPtr2 = hWnd.ToPointer();
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HWND__*, int>)(*(ulong*)num2))((nint)intPtr, (HWND__*)intPtr2);
				}
			}
			finally
			{
				if (0L != (nint)ptr)
				{
					IMetadataManager* intPtr3 = ptr;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr3 + 16)))((nint)intPtr3);
					ptr = null;
				}
			}
			return new HRESULT(num);
		}

		public unsafe HRESULT FindFolder(string szFolderName, out int nFolderId)
		{
			//IL_0008: Expected I, but got I8
			//IL_000b: Expected I, but got I8
			//IL_0030: Expected I, but got I8
			//IL_0042: Expected I4, but got I8
			//IL_0062: Expected I, but got I8
			//IL_008b: Expected I, but got I8
			//IL_008f: Expected I, but got I8
			//IL_00a1: Expected I, but got I8
			//IL_00a5: Expected I, but got I8
			int num = 1;
			nFolderId = -1;
			IMetadataManager* ptr = null;
			IFolderProvider* ptr2 = null;
			try
			{
				num = _003CModule_003E.GetSingleton((_GUID)_003CModule_003E._GUID_6dd7146d_7a19_4fbb_9235_9e6c382fcc71, (void**)(&ptr));
				if (num >= 0)
				{
					IMetadataManager* intPtr = ptr;
					__s_GUID gUID_a2889317_d0c7_41d8_abc7_1eb4cb8d46d = _003CModule_003E._GUID_a2889317_d0c7_41d8_abc7_1eb4cb8d46d6;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID, void**, int>)(*(ulong*)(*(long*)ptr + 24)))((nint)intPtr, (_GUID)gUID_a2889317_d0c7_41d8_abc7_1eb4cb8d46d, (void**)(&ptr2));
				}
				tagVARIANT tagVARIANT;
				*(short*)(&tagVARIANT) = 0;
				// IL initblk instruction
				System.Runtime.CompilerServices.Unsafe.InitBlockUnaligned(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagVARIANT, 2), 0, 22);
				if (num >= 0)
				{
					fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref _003CModule_003E.PtrToStringChars(szFolderName)))
					{
						try
						{
							long num2 = *(long*)ptr2 + 88;
							IFolderProvider* intPtr2 = ptr2;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, EMediaTypes, tagVARIANT*, int>)(*(ulong*)num2))((nint)intPtr2, ptr3, EMediaTypes.eMediaTypeImage, &tagVARIANT);
						}
						catch
						{
							//try-fault
							ptr3 = null;
							throw;
						}
					}
					if (num >= 0)
					{
						nFolderId = System.Runtime.CompilerServices.Unsafe.As<tagVARIANT, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagVARIANT, 8));
					}
				}
			}
			finally
			{
				if (0L != (nint)ptr2)
				{
					IFolderProvider* intPtr3 = ptr2;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr3 + 16)))((nint)intPtr3);
					ptr2 = null;
				}
				if (0L != (nint)ptr)
				{
					IMetadataManager* intPtr4 = ptr;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr4 + 16)))((nint)intPtr4);
					ptr = null;
				}
			}
			return new HRESULT(num);
		}

		public unsafe HRESULT FindPhotoContainer([In] int nMediaId, out int nFolderId)
		{
			//IL_0008: Expected I, but got I8
			//IL_000b: Expected I, but got I8
			//IL_0030: Expected I, but got I8
			//IL_0042: Expected I4, but got I8
			//IL_005a: Expected I, but got I8
			//IL_007a: Expected I, but got I8
			//IL_007e: Expected I, but got I8
			//IL_0090: Expected I, but got I8
			//IL_0094: Expected I, but got I8
			int num = 1;
			nFolderId = -1;
			IMetadataManager* ptr = null;
			IFileProvider* ptr2 = null;
			try
			{
				num = _003CModule_003E.GetSingleton((_GUID)_003CModule_003E._GUID_6dd7146d_7a19_4fbb_9235_9e6c382fcc71, (void**)(&ptr));
				if (num >= 0)
				{
					IMetadataManager* intPtr = ptr;
					__s_GUID gUID_16a9f8be_e76c_4391_ad74_8df74b7a3c = _003CModule_003E._GUID_16a9f8be_e76c_4391_ad74_8df74b7a3c21;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID, void**, int>)(*(ulong*)(*(long*)ptr + 24)))((nint)intPtr, (_GUID)gUID_16a9f8be_e76c_4391_ad74_8df74b7a3c, (void**)(&ptr2));
				}
				tagVARIANT tagVARIANT;
				*(short*)(&tagVARIANT) = 0;
				// IL initblk instruction
				System.Runtime.CompilerServices.Unsafe.InitBlockUnaligned(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagVARIANT, 2), 0, 22);
				if (num >= 0)
				{
					IFileProvider* intPtr2 = ptr2;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, EMediaTypes, tagVARIANT*, int>)(*(ulong*)(*(long*)ptr2 + 528)))((nint)intPtr2, nMediaId, EMediaTypes.eMediaTypeImage, &tagVARIANT);
					if (num >= 0)
					{
						nFolderId = System.Runtime.CompilerServices.Unsafe.As<tagVARIANT, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagVARIANT, 8));
					}
				}
			}
			finally
			{
				if (0L != (nint)ptr2)
				{
					IFileProvider* intPtr3 = ptr2;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr3 + 16)))((nint)intPtr3);
					ptr2 = null;
				}
				if (0L != (nint)ptr)
				{
					IMetadataManager* intPtr4 = ptr;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr4 + 16)))((nint)intPtr4);
					ptr = null;
				}
			}
			return new HRESULT(num);
		}

		public unsafe HRESULT CreateFolder(string szFolderName, int nParentFolderId, out int nCreatedFolderId)
		{
			//IL_0016: Expected I, but got I8
			//IL_0019: Expected I, but got I8
			//IL_003e: Expected I, but got I8
			//IL_0050: Expected I4, but got I8
			//IL_0073: Expected I, but got I8
			//IL_009c: Expected I, but got I8
			//IL_00a0: Expected I, but got I8
			//IL_00b2: Expected I, but got I8
			//IL_00b6: Expected I, but got I8
			int num = 1;
			nCreatedFolderId = -1;
			if (string.IsNullOrEmpty(szFolderName))
			{
				return HRESULT._DB_E_BADPARAMETERNAME;
			}
			IMetadataManager* ptr = null;
			IFolderProvider* ptr2 = null;
			try
			{
				num = _003CModule_003E.GetSingleton((_GUID)_003CModule_003E._GUID_6dd7146d_7a19_4fbb_9235_9e6c382fcc71, (void**)(&ptr));
				if (num >= 0)
				{
					IMetadataManager* intPtr = ptr;
					__s_GUID gUID_a2889317_d0c7_41d8_abc7_1eb4cb8d46d = _003CModule_003E._GUID_a2889317_d0c7_41d8_abc7_1eb4cb8d46d6;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID, void**, int>)(*(ulong*)(*(long*)ptr + 24)))((nint)intPtr, (_GUID)gUID_a2889317_d0c7_41d8_abc7_1eb4cb8d46d, (void**)(&ptr2));
				}
				tagVARIANT tagVARIANT;
				*(short*)(&tagVARIANT) = 0;
				// IL initblk instruction
				System.Runtime.CompilerServices.Unsafe.InitBlockUnaligned(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagVARIANT, 2), 0, 22);
				if (num >= 0)
				{
					fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref _003CModule_003E.PtrToStringChars(szFolderName)))
					{
						try
						{
							long num2 = *(long*)ptr2 + 224;
							IFolderProvider* intPtr2 = ptr2;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int, tagVARIANT*, int>)(*(ulong*)num2))((nint)intPtr2, ptr3, nParentFolderId, &tagVARIANT);
						}
						catch
						{
							//try-fault
							ptr3 = null;
							throw;
						}
					}
					if (num >= 0)
					{
						nCreatedFolderId = System.Runtime.CompilerServices.Unsafe.As<tagVARIANT, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagVARIANT, 8));
					}
				}
			}
			finally
			{
				if (0L != (nint)ptr2)
				{
					IFolderProvider* intPtr3 = ptr2;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr3 + 16)))((nint)intPtr3);
					ptr2 = null;
				}
				if (0L != (nint)ptr)
				{
					IMetadataManager* intPtr4 = ptr;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr4 + 16)))((nint)intPtr4);
					ptr = null;
				}
			}
			return new HRESULT(num);
		}

		public unsafe HRESULT RenameFolder(int nFolderId, string szNewFolderName)
		{
			//IL_0013: Expected I, but got I8
			//IL_0016: Expected I, but got I8
			//IL_003b: Expected I, but got I8
			//IL_005e: Expected I, but got I8
			//IL_007c: Expected I, but got I8
			//IL_0080: Expected I, but got I8
			//IL_0092: Expected I, but got I8
			//IL_0096: Expected I, but got I8
			int num = 1;
			if (string.IsNullOrEmpty(szNewFolderName))
			{
				return HRESULT._DB_E_BADPARAMETERNAME;
			}
			IMetadataManager* ptr = null;
			IFolderProvider* ptr2 = null;
			try
			{
				num = _003CModule_003E.GetSingleton((_GUID)_003CModule_003E._GUID_6dd7146d_7a19_4fbb_9235_9e6c382fcc71, (void**)(&ptr));
				if (num >= 0)
				{
					IMetadataManager* intPtr = ptr;
					__s_GUID gUID_a2889317_d0c7_41d8_abc7_1eb4cb8d46d = _003CModule_003E._GUID_a2889317_d0c7_41d8_abc7_1eb4cb8d46d6;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID, void**, int>)(*(ulong*)(*(long*)ptr + 24)))((nint)intPtr, (_GUID)gUID_a2889317_d0c7_41d8_abc7_1eb4cb8d46d, (void**)(&ptr2));
					if (num >= 0)
					{
						fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref _003CModule_003E.PtrToStringChars(szNewFolderName)))
						{
							try
							{
								long num2 = *(long*)ptr2 + 232;
								IFolderProvider* intPtr2 = ptr2;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, ushort*, byte, int>)(*(ulong*)num2))((nint)intPtr2, nFolderId, ptr3, 0);
							}
							catch
							{
								//try-fault
								ptr3 = null;
								throw;
							}
						}
					}
				}
			}
			finally
			{
				if (0L != (nint)ptr2)
				{
					IFolderProvider* intPtr3 = ptr2;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr3 + 16)))((nint)intPtr3);
					ptr2 = null;
				}
				if (0L != (nint)ptr)
				{
					IMetadataManager* intPtr4 = ptr;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr4 + 16)))((nint)intPtr4);
					ptr = null;
				}
			}
			return new HRESULT(num);
		}

		public unsafe HRESULT Move(int[] mediaIds, EMediaTypes mediaType, int nDestinationFolderId)
		{
			//IL_0018: Expected I, but got I8
			//IL_001b: Expected I, but got I8
			//IL_0040: Expected I, but got I8
			//IL_0066: Expected I, but got I8
			//IL_0084: Expected I, but got I8
			//IL_0088: Expected I, but got I8
			//IL_009a: Expected I, but got I8
			//IL_009e: Expected I, but got I8
			if (mediaIds == null)
			{
				return HRESULT._DB_E_BADPARAMETERNAME;
			}
			if (mediaIds.Length == 0)
			{
				return HRESULT._S_OK;
			}
			int num = 1;
			IMetadataManager* ptr = null;
			IFolderProvider* ptr2 = null;
			try
			{
				num = _003CModule_003E.GetSingleton((_GUID)_003CModule_003E._GUID_6dd7146d_7a19_4fbb_9235_9e6c382fcc71, (void**)(&ptr));
				if (num >= 0)
				{
					IMetadataManager* intPtr = ptr;
					__s_GUID gUID_a2889317_d0c7_41d8_abc7_1eb4cb8d46d = _003CModule_003E._GUID_a2889317_d0c7_41d8_abc7_1eb4cb8d46d6;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID, void**, int>)(*(ulong*)(*(long*)ptr + 24)))((nint)intPtr, (_GUID)gUID_a2889317_d0c7_41d8_abc7_1eb4cb8d46d, (void**)(&ptr2));
					if (num >= 0)
					{
						fixed (int* ptr3 = &mediaIds[0])
						{
							try
							{
								int* ptr4 = ptr3;
								IFolderProvider* intPtr2 = ptr2;
								IntPtr intPtr3 = (nint)mediaIds.LongLength;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaTypes, int*, int, int, int>)(*(ulong*)(*(long*)ptr2 + 256)))((nint)intPtr2, mediaType, ptr4, (int)(nint)intPtr3, nDestinationFolderId);
							}
							catch
							{
								//try-fault
								ptr3 = null;
								throw;
							}
						}
					}
				}
			}
			finally
			{
				if (0L != (nint)ptr2)
				{
					IFolderProvider* intPtr4 = ptr2;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr4 + 16)))((nint)intPtr4);
					ptr2 = null;
				}
				if (0L != (nint)ptr)
				{
					IMetadataManager* intPtr5 = ptr;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr5 + 16)))((nint)intPtr5);
					ptr = null;
				}
			}
			return new HRESULT(num);
		}

		public unsafe HRESULT Import(string szPath, EMediaTypes mediaType, int nDestinationFolderId)
		{
			//IL_0013: Expected I, but got I8
			//IL_0016: Expected I, but got I8
			//IL_003b: Expected I, but got I8
			//IL_005e: Expected I, but got I8
			//IL_007c: Expected I, but got I8
			//IL_0080: Expected I, but got I8
			//IL_0092: Expected I, but got I8
			//IL_0096: Expected I, but got I8
			if (string.IsNullOrEmpty(szPath))
			{
				return HRESULT._DB_E_BADPARAMETERNAME;
			}
			int num = 1;
			IMetadataManager* ptr = null;
			IFolderProvider* ptr2 = null;
			try
			{
				num = _003CModule_003E.GetSingleton((_GUID)_003CModule_003E._GUID_6dd7146d_7a19_4fbb_9235_9e6c382fcc71, (void**)(&ptr));
				if (num >= 0)
				{
					IMetadataManager* intPtr = ptr;
					__s_GUID gUID_a2889317_d0c7_41d8_abc7_1eb4cb8d46d = _003CModule_003E._GUID_a2889317_d0c7_41d8_abc7_1eb4cb8d46d6;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID, void**, int>)(*(ulong*)(*(long*)ptr + 24)))((nint)intPtr, (_GUID)gUID_a2889317_d0c7_41d8_abc7_1eb4cb8d46d, (void**)(&ptr2));
					if (num >= 0)
					{
						fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref _003CModule_003E.PtrToStringChars(szPath)))
						{
							try
							{
								long num2 = *(long*)ptr2 + 264;
								IFolderProvider* intPtr2 = ptr2;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, EMediaTypes, ushort*, int>)(*(ulong*)num2))((nint)intPtr2, nDestinationFolderId, mediaType, ptr3);
							}
							catch
							{
								//try-fault
								ptr3 = null;
								throw;
							}
						}
					}
				}
			}
			finally
			{
				if (0L != (nint)ptr2)
				{
					IFolderProvider* intPtr3 = ptr2;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr3 + 16)))((nint)intPtr3);
					ptr2 = null;
				}
				if (0L != (nint)ptr)
				{
					IMetadataManager* intPtr4 = ptr;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr4 + 16)))((nint)intPtr4);
					ptr = null;
				}
			}
			return new HRESULT(num);
		}
	}
}
