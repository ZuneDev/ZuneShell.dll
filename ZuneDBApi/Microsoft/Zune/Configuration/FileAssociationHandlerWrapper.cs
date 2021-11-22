using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Configuration
{
	internal class FileAssociationHandlerWrapper : IFileAssociationHandler, IDisposable
	{
		private unsafe global::IFileAssociationHandler* m_pFileAssociationHandler;

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe virtual bool CanAssociationBeChanged()
		{
			//IL_0012: Expected I, but got I8
			global::IFileAssociationHandler* pFileAssociationHandler = m_pFileAssociationHandler;
			return (byte)((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pFileAssociationHandler + 40)))((nint)pFileAssociationHandler) != 0) ? 1u : 0u) != 0;
		}

		public unsafe virtual int GetFileAssociationInfoList(out IList<FileAssociationInfo> fileAssociationInfoList)
		{
			//IL_0003: Expected I, but got I8
			//IL_0025: Expected I, but got I8
			//IL_0025: Expected I, but got I8
			//IL_0053: Expected I4, but got I8
			//IL_006a: Expected I, but got I8
			//IL_007d: Expected I, but got I8
			//IL_0088: Expected I, but got I8
			//IL_0092: Expected I, but got I8
			//IL_009b: Expected I, but got I8
			//IL_00d3: Expected I, but got I8
			global::FileAssociationInfo* ptr = null;
			uint num = 0u;
			fileAssociationInfoList = new List<FileAssociationInfo>();
			global::IFileAssociationHandler* pFileAssociationHandler = m_pFileAssociationHandler;
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, global::FileAssociationInfo*, uint*, int>)(*(ulong*)(*(long*)pFileAssociationHandler + 24)))((nint)pFileAssociationHandler, null, &num);
			if (num2 >= 0)
			{
				ptr = (global::FileAssociationInfo*)Module.new_005B_005D(num * 32uL);
				if (ptr == null)
				{
					num2 = -2147024882;
				}
				else
				{
					// IL initblk instruction
					*ptr = default;
					pFileAssociationHandler = m_pFileAssociationHandler;
					num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, global::FileAssociationInfo*, uint*, int>)(*(ulong*)(*(long*)pFileAssociationHandler + 24)))((nint)pFileAssociationHandler, ptr, &num);
					if (num2 >= 0)
					{
						uint num3 = 0u;
						if (0 < num)
						{
							global::FileAssociationInfo* ptr2 = (global::FileAssociationInfo*)((ulong)(nint)ptr + 16uL);
							do
							{
								FileAssociationInfo item = new FileAssociationInfo(new string((char*)(*(ulong*)((ulong)(nint)ptr2 - 16uL))), new string((char*)(*(ulong*)((ulong)(nint)ptr2 - 8uL))), new string((char*)(*(ulong*)ptr2)), isCurrentlyOwned: (*(int*)((ulong)(nint)ptr2 + 8uL) != 0) ? true : false, mediaType: *(EMediaTypes*)((ulong)(nint)ptr2 + 12uL));
								fileAssociationInfoList.Add(item);
								num3++;
								ptr2 = (global::FileAssociationInfo*)((ulong)(nint)ptr2 + 32uL);
							}
							while (num3 < num);
						}
					}
				}
			}
			CleanupFileInfoArray(ptr, num);
			return num2;
		}

		public unsafe virtual int SetFileAssociationInfo(IList<FileAssociationInfo> fileAssociationInfoList)
		{
			//IL_0039: Expected I, but got I8
			//IL_005f: Expected I, but got I8
			uint count = (uint)fileAssociationInfoList.Count;
			global::FileAssociationInfo* ptr = (global::FileAssociationInfo*)Module.new_005B_005D(count * 32uL);
			int num;
			if (ptr == null)
			{
				num = -2147024882;
			}
			else
			{
				uint num2 = 0u;
				if (0 >= count)
				{
					goto IL_0046;
				}
				while (true)
				{
					num = FileInfoToStruct(fileAssociationInfoList[(int)num2], (global::FileAssociationInfo*)(num2 * 32L + (nint)ptr));
					if (num < 0)
					{
						break;
					}
					num2++;
					if (num2 < count)
					{
						continue;
					}
					goto IL_0046;
				}
			}
			goto IL_0060;
			IL_0046:
			global::IFileAssociationHandler* pFileAssociationHandler = m_pFileAssociationHandler;
			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, global::FileAssociationInfo*, uint, int>)(*(ulong*)(*(long*)pFileAssociationHandler + 32)))((nint)pFileAssociationHandler, ptr, count);
			goto IL_0060;
			IL_0060:
			CleanupFileInfoArray(ptr, count);
			return num;
		}

		private unsafe void _007EFileAssociationHandlerWrapper()
		{
			//IL_0017: Expected I, but got I8
			global::IFileAssociationHandler* pFileAssociationHandler = m_pFileAssociationHandler;
			if (pFileAssociationHandler != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pFileAssociationHandler + 16)))((nint)pFileAssociationHandler);
			}
		}

		internal unsafe FileAssociationHandlerWrapper(global::IFileAssociationHandler* pFilessociationManager)
		{
			m_pFileAssociationHandler = pFilessociationManager;
		}

		internal unsafe int FileInfoToStruct(FileAssociationInfo fileAssocInfo, global::FileAssociationInfo* pFileAssocInfo)
		{
			//IL_002c: Expected I8, but got I
			//IL_0037: Expected I8, but got I
			//IL_0045: Expected I8, but got I
			fixed (char* fileAssocInfoExtensionPtr = fileAssocInfo.Extension.ToCharArray())
			{
				ushort* ptr = (ushort*)fileAssocInfoExtensionPtr;
				fixed (char* fileAssocInfoProgIdPtr = fileAssocInfo.ProgId.ToCharArray())
				{
					ushort* ptr2 = (ushort*)fileAssocInfoProgIdPtr;
					fixed (char* fileAssocInfoDescriptionPtr = fileAssocInfo.Description.ToCharArray())
					{
						ushort* ptr3 = (ushort*)fileAssocInfoDescriptionPtr;
						*(long*)pFileAssocInfo = (nint)Module.SysAllocString(ptr);
						*(long*)((ulong)(nint)pFileAssocInfo + 8uL) = (nint)Module.SysAllocString(ptr2);
						ushort* ptr4 = Module.SysAllocString(ptr3);
						*(long*)((ulong)(nint)pFileAssocInfo + 16uL) = (nint)ptr4;
						*(int*)((ulong)(nint)pFileAssocInfo + 24uL) = (fileAssocInfo.IsCurrentlyOwned ? 1 : 0);
						if (*(long*)pFileAssocInfo != 0L && *(long*)((ulong)(nint)pFileAssocInfo + 8uL) != 0L && ptr4 != null)
						{
							return 0;
						}
						return -2147024882;
					}
				}
			}
		}

		internal unsafe void CleanupFileInfoArray(global::FileAssociationInfo* rgFileInfo, uint cFileInfo)
		{
			//IL_000d: Expected I, but got I8
			//IL_0022: Expected I, but got I8
			//IL_0031: Expected I, but got I8
			//IL_003d: Expected I, but got I8
			//IL_0043: Expected I, but got I8
			if (rgFileInfo == null)
			{
				return;
			}
			if (0 < cFileInfo)
			{
				global::FileAssociationInfo* ptr = (global::FileAssociationInfo*)((ulong)(nint)rgFileInfo + 16uL);
				uint num = cFileInfo;
				do
				{
					long num2 = *(long*)((ulong)(nint)ptr - 16uL);
					if (num2 != 0L)
					{
						Module.SysFreeString((ushort*)num2);
					}
					long num3 = *(long*)((ulong)(nint)ptr - 8uL);
					if (num3 != 0L)
					{
						Module.SysFreeString((ushort*)num3);
					}
					ulong num4 = *(ulong*)ptr;
					if (num4 != 0L)
					{
						Module.SysFreeString((ushort*)num4);
					}
					ptr = (global::FileAssociationInfo*)((ulong)(nint)ptr + 32uL);
					num += uint.MaxValue;
				}
				while (num != 0);
			}
			Module.delete_005B_005D(rgFileInfo);
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_007EFileAssociationHandlerWrapper();
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
