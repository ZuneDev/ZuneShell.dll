using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MicrosoftZuneInterop
{
	public class QueryPropertyBag : IDisposable
	{
		private unsafe IQueryPropertyBag* m_pPropertyBag;

		public unsafe QueryPropertyBag()
		{
			IQueryPropertyBag* pPropertyBag;
			Module.CreatePropertyBag(&pPropertyBag);
			m_pPropertyBag = pPropertyBag;
		}

		private unsafe void _0021QueryPropertyBag()
		{
			//IL_0019: Expected I, but got I8
			//IL_0022: Expected I, but got I8
			IQueryPropertyBag* pPropertyBag = m_pPropertyBag;
			if (0L != (nint)pPropertyBag)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pPropertyBag + 16)))((nint)pPropertyBag);
				m_pPropertyBag = null;
			}
		}

		private void _007EQueryPropertyBag()
		{
			_0021QueryPropertyBag();
		}

		public unsafe void SetValue(string propertyName, object value)
		{
			//IL_004e: Expected I, but got I8
			//IL_007d: Expected I, but got I8
			//IL_00b6: Expected I, but got I8
			EQueryPropertyBagProp eQueryPropertyBagProp = MapNameToProp(propertyName);
			if (eQueryPropertyBagProp == (EQueryPropertyBagProp)(-1) || value == null)
			{
				return;
			}
			int num = -2147024809;
			Type type = value.GetType();
			if (type == typeof(int))
			{
				IQueryPropertyBag* pPropertyBag = m_pPropertyBag;
				int num2 = (int)value;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)pPropertyBag + 56)))((nint)pPropertyBag, eQueryPropertyBagProp, num2);
			}
			else if (type == typeof(bool))
			{
				IQueryPropertyBag* pPropertyBag2 = m_pPropertyBag;
				bool num3 = (bool)value;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)pPropertyBag2 + 56)))((nint)pPropertyBag2, eQueryPropertyBagProp, num3 ? 1 : 0);
			}
			else
			{
				if (type != typeof(string))
				{
					goto IL_00c4;
				}
				fixed (char* valuePtr = ((string)value).ToCharArray())
				{
					ushort* ptr = (ushort*)valuePtr;
					try
					{
						long num4 = *(long*)m_pPropertyBag + 40;
						IQueryPropertyBag* pPropertyBag3 = m_pPropertyBag;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, ushort*, int>)(*(ulong*)num4))((nint)pPropertyBag3, eQueryPropertyBagProp, ptr);
					}
					catch
					{
						//try-fault
						ptr = null;
						throw;
					}
				}
			}
			if (num >= 0)
			{
				return;
			}
			goto IL_00c4;
			IL_00c4:
			throw new ApplicationException(Module.GetErrorDescription(num));
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool IsSet(string propertyName)
		{
			//IL_0025: Expected I, but got I8
			int num = 0;
			EQueryPropertyBagProp eQueryPropertyBagProp = MapNameToProp(propertyName);
			if (eQueryPropertyBagProp != (EQueryPropertyBagProp)(-1))
			{
				IQueryPropertyBag* pPropertyBag = m_pPropertyBag;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int*, int>)(*(ulong*)(*(long*)pPropertyBag + 104)))((nint)pPropertyBag, eQueryPropertyBagProp, &num);
			}
			return num == 1;
		}

		public unsafe EQueryPropertyBagProp MapNameToProp(string propertyName)
		{
			//IL_0017: Expected I, but got I8
			//IL_0023: Expected I, but got I8
			fixed (char* propertyNamePtr = propertyName.ToCharArray())
			{
				ushort* ptr2 = (ushort*)propertyNamePtr;
				int num = 0;
				PropIdMapEntry* ptr = (PropIdMapEntry*)Unsafe.AsPointer(ref Module.MicrosoftZuneInterop_002E_003FA0x52c37a46_002EkPropIdMap);
				EQueryPropertyBagProp eQueryPropertyBagProp;
				while (true)
				{
					if (Module._wcsicmp(ptr2, (ushort*)(*(ulong*)ptr)) != 0)
					{
						num++;
						ptr = (PropIdMapEntry*)((ulong)(nint)ptr + 16uL);
						if ((uint)num < 37u)
						{
							continue;
						}
					}
					else
					{
						eQueryPropertyBagProp = *(EQueryPropertyBagProp*)((long)num * 16L + Module.MicrosoftZuneInterop_002E_003FA0x52c37a46_002EkPropIdMap + 8);
						if (eQueryPropertyBagProp != (EQueryPropertyBagProp)(-1))
						{
							break;
						}
					}
					throw new ArgumentException("Invalid property name: " + propertyName, "propertyName");
				}
				return eQueryPropertyBagProp;
			}
		}

		public unsafe IQueryPropertyBag* GetIQueryPropertyBag()
		{
			return m_pPropertyBag;
		}

		public unsafe IDList* PackIDList(IList multiIds)
		{
			//IL_0022: Expected I, but got I8
			//IL_0064: Expected I8, but got I
			int count = multiIds.Count;
			IDList* ptr = (IDList*)Module.@new(16uL);
			IDList* ptr2;
			try
			{
				if (ptr != null)
				{
					*(long*)((ulong)(nint)ptr + 8uL) = 0L;
					ptr2 = ptr;
				}
				else
				{
					ptr2 = null;
				}
			}
			catch
			{
				//try-fault
				Module.delete(ptr);
				throw;
			}
			if (ptr2 != null)
			{
				*(int*)ptr2 = count;
				ulong num = (ulong)count;
				ulong num2 = ((num > 4611686018427387903L) ? ulong.MaxValue : (num * 4));
				*(long*)((ulong)(nint)ptr2 + 8uL) = (nint)Module.new_005B_005D(num2);
				int num3 = 0;
				long num4 = 0L;
				long num5 = count;
				if (0 < num5)
				{
					do
					{
						*(int*)(num4 * 4 + *(long*)((ulong)(nint)ptr2 + 8uL)) = (int)multiIds[num3];
						num3++;
						num4++;
					}
					while (num4 < num5);
				}
			}
			return ptr2;
		}

		public unsafe IMultiSortAttributes* PackMultiSortAttributes(string[] sortStrings, bool[] sortAscendings)
		{
			//IL_0008: Expected I, but got I8
			//IL_000c: Expected I, but got I8
			//IL_0026: Expected I, but got I8
			//IL_0036: Expected I, but got I8
			//IL_009a: Expected I, but got I8
			int num = sortStrings.Length;
			int* ptr = null;
			int* ptr2 = null;
			IMultiSortAttributes* ptr3;
			if (Module.CreateMultiSortAttributes(num, &ptr3) >= 0)
			{
				IMultiSortAttributes* intPtr = ptr3;
				ptr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*>)(*(ulong*)(*(long*)intPtr + 32)))((nint)intPtr);
				IMultiSortAttributes* intPtr2 = ptr3;
				ptr2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*>)(*(ulong*)(*(long*)intPtr2 + 40)))((nint)intPtr2);
			}
			else
			{
				num = 0;
			}
			int num2 = 0;
			long num3 = num;
			if (0 < num3)
			{
				int* ptr4 = ptr2;
				int* ptr5 = (int*)((byte*)ptr - (nuint)ptr2);
				ulong num4 = (ulong)num3;
				do
				{
					EQuerySortType eQuerySortType = (sortAscendings[num2] ? EQuerySortType.eQuerySortOrderAscending : EQuerySortType.eQuerySortOrderDescending);
					fixed (char* sortStringsNum2Ptr = sortStrings[num2].ToCharArray())
					{
						ushort* wszName = (ushort*)sortStringsNum2Ptr;
						try
						{
							int num5 = Module.CSchemaMap_002EGetIndex(wszName);
							eQuerySortType = ((num5 != -1) ? eQuerySortType : EQuerySortType.eQuerySortOrderNone);
							*(int*)((byte*)ptr5 + (nuint)ptr4) = num5;
							*ptr4 = (int)eQuerySortType;
						}
						catch
						{
							//try-fault
							wszName = null;
							throw;
						}
					}
					num2++;
					ptr4 = (int*)((ulong)(nint)ptr4 + 4uL);
					num4--;
				}
				while (num4 != 0);
			}
			return ptr3;
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_0021QueryPropertyBag();
				return;
			}
			try
			{
				_0021QueryPropertyBag();
			}
			finally
			{
				//base.Finalize();
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		~QueryPropertyBag()
		{
			Dispose(false);
		}
	}
}
