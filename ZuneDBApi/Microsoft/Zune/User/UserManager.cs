using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Microsoft.Zune.User
{
	public class UserManager
	{
		private static UserManager sm_instance = null;

		private static object sm_lock = new object();

		public static UserManager Instance
		{
			get
			{
				if (sm_instance == null)
				{
					try
					{
						Monitor.Enter(sm_lock);
						if (sm_instance == null)
						{
							UserManager userManager = new UserManager();
							Thread.MemoryBarrier();
							sm_instance = userManager;
						}
					}
					finally
					{
						Monitor.Exit(sm_lock);
					}
				}
				return sm_instance;
			}
		}

		public unsafe int GetUserIdList(IList userIdList)
		{
			//IL_0003: Expected I, but got I8
			//IL_0040: Expected I, but got I8
			//IL_0088: Expected I, but got I8
			//IL_008c: Expected I, but got I8
			IUserManager* ptr = null;
			DynamicArray_003Cint_003E dynamicArray_003Cint_003E;
			*(long*)(&dynamicArray_003Cint_003E) = (nint)Unsafe.AsPointer(ref Module._003F_003F_7_003F_0024DynamicArray_0040H_0040_00406B_0040);
            Unsafe.As<DynamicArray_003Cint_003E, long>(ref Unsafe.AddByteOffset(ref dynamicArray_003Cint_003E, 8)) = 0L;
            Unsafe.As<DynamicArray_003Cint_003E, int>(ref Unsafe.AddByteOffset(ref dynamicArray_003Cint_003E, 16)) = 0;
            Unsafe.As<DynamicArray_003Cint_003E, int>(ref Unsafe.AddByteOffset(ref dynamicArray_003Cint_003E, 20)) = 0;
			int num;
			try
			{
				num = Module.GetSingleton(Module.GUID_IUserManager, (void**)(&ptr));
				if (num >= 0)
				{
					IUserManager* intPtr = ptr;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, DynamicArray_003Cint_003E*, int>)(*(ulong*)(*(long*)ptr + 24)))((nint)intPtr, &dynamicArray_003Cint_003E);
					if (num >= 0)
					{
						int num2 = 0;
						if (0 < Unsafe.As<DynamicArray_003Cint_003E, int>(ref Unsafe.AddByteOffset(ref dynamicArray_003Cint_003E, 16)))
						{
							do
							{
								int* ptr2 = Module.DynamicArray_003Cint_003E_002E_005B_005D(&dynamicArray_003Cint_003E, num2);
								userIdList.Add(*ptr2);
								num2++;
							}
							while (num2 < Unsafe.As<DynamicArray_003Cint_003E, int>(ref Unsafe.AddByteOffset(ref dynamicArray_003Cint_003E, 16)));
						}
					}
				}
				if (0L != (nint)ptr)
				{
					IUserManager* intPtr2 = ptr;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
					ptr = null;
				}
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<DynamicArray_003Cint_003E*, void>)(&Module.DynamicArray_003Cint_003E_002E_007Bdtor_007D), &dynamicArray_003Cint_003E);
				throw;
			}
			Module.DynamicArray_003Cint_003E_002E_007Bdtor_007D(&dynamicArray_003Cint_003E);
			return num;
		}

		public unsafe int FindUserByPassportId(string passportId, out int userId)
		{
			//IL_0006: Expected I, but got I8
			//IL_0033: Expected I, but got I8
			//IL_004e: Expected I, but got I8
			userId = -1;
			IUserManager* ptr = null;
			int num = -1;
			fixed (char* passportIdPtr = passportId.ToCharArray())
			{
				ushort* ptr2 = (ushort*)passportIdPtr;
				int num2 = Module.GetSingleton(Module.GUID_IUserManager, (void**)(&ptr));
				if (num2 >= 0)
				{
					long num3 = *(long*)ptr + 40;
					IUserManager* intPtr = ptr;
					num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int*, int>)(*(ulong*)num3))((nint)intPtr, ptr2, &num);
				}
				if (0 == num2)
				{
					userId = num;
				}
				if (0L != (nint)ptr)
				{
					IUserManager* intPtr2 = ptr;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
				}
				return num2;
			}
		}

		public unsafe int RefreshUserTile(int userId)
		{
			//IL_0003: Expected I, but got I8
			//IL_0022: Expected I, but got I8
			//IL_0035: Expected I, but got I8
			IUserManager* ptr = null;
			int num = Module.GetSingleton(Module.GUID_IUserManager, (void**)(&ptr));
			if (num >= 0)
			{
				IUserManager* intPtr = ptr;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)ptr + 48)))((nint)intPtr, userId);
			}
			if (0L != (nint)ptr)
			{
				IUserManager* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
			return num;
		}

		public unsafe int CleanupUserData(int userId)
		{
			//IL_0003: Expected I, but got I8
			//IL_0022: Expected I, but got I8
			//IL_0035: Expected I, but got I8
			IUserManager* ptr = null;
			int num = Module.GetSingleton(Module.GUID_IUserManager, (void**)(&ptr));
			if (num >= 0)
			{
				IUserManager* intPtr = ptr;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)ptr + 56)))((nint)intPtr, userId);
			}
			if (0L != (nint)ptr)
			{
				IUserManager* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
			return num;
		}
	}
}
