using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using _003CCppImplementationDetails_003E;

namespace MicrosoftZuneLibrary
{
	public class ZuneQueryList : IDisposable
	{
		protected unsafe IDatabaseQueryResults* m_pResults;

		protected unsafe ResultSetEventRelay* m_pRelay;

		private unsafe ushort* m_bstrQueryName = null;

		private int m_RefCount = 0;

		private bool m_disposed = false;

		public bool IsDisposed
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_disposed;
			}
		}

		public unsafe bool IsEmpty
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_0017: Expected I, but got I8
				IDatabaseQueryResults* pResults = m_pResults;
				if (pResults != null)
				{
					return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pResults + 96)))((nint)pResults) == 1;
				}
				return true;
			}
		}

		public unsafe int Count
		{
			get
			{
				//IL_001b: Expected I, but got I8
				int result = 0;
				IDatabaseQueryResults* pResults = m_pResults;
				if (pResults != null && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)pResults + 88)))((nint)pResults, (uint*)(&result)) >= 0)
				{
					return result;
				}
				return result;
			}
		}

		public unsafe ZuneQueryList(IDatabaseQueryResults* pResults, string queryName)
		{
			//IL_0008: Expected I, but got I8
			//IL_004c: Expected I, but got I8
			//IL_0065: Expected I, but got I8
			m_pResults = pResults;
			fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref _003CModule_003E.PtrToStringChars(queryName)))
			{
				m_bstrQueryName = _003CModule_003E.SysAllocString(ptr);
				IDatabaseQueryResults* pResults2 = m_pResults;
				if (pResults2 != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pResults2 + 8)))((nint)pResults2);
					ResultSetEventRelay* ptr2 = (ResultSetEventRelay*)_003CModule_003E.@new(56uL);
					ResultSetEventRelay* pRelay;
					try
					{
						pRelay = ((ptr2 == null) ? null : _003CModule_003E.MicrosoftZuneLibrary_002EResultSetEventRelay_002E_007Bctor_007D(ptr2));
					}
					catch
					{
						//try-fault
						_003CModule_003E.delete(ptr2);
						throw;
					}
					m_pRelay = pRelay;
				}
			}
		}

		private unsafe void _007EZuneQueryList()
		{
			//IL_0021: Expected I, but got I8
			//IL_003a: Expected I, but got I8
			//IL_0043: Expected I, but got I8
			//IL_005f: Expected I, but got I8
			//IL_0079: Expected I, but got I8
			m_disposed = true;
			IDatabaseQueryResults* pResults = m_pResults;
			if (pResults != null)
			{
				IDatabaseQueryResults* intPtr = pResults;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, void>)(*(ulong*)(*(long*)intPtr + 160)))((nint)intPtr);
				pResults = m_pResults;
				if (0L != (nint)pResults)
				{
					IDatabaseQueryResults* intPtr2 = pResults;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
					m_pResults = null;
				}
			}
			ResultSetEventRelay* pRelay = m_pRelay;
			if (0L != (nint)pRelay)
			{
				_003CModule_003E.MicrosoftZuneLibrary_002EResultSetEventRelay_002E__delDtor(pRelay, 1u);
				m_pRelay = null;
			}
			ushort* bstrQueryName = m_bstrQueryName;
			if (0L != (nint)bstrQueryName)
			{
				_003CModule_003E.SysFreeString(bstrQueryName);
				m_bstrQueryName = null;
			}
		}

		private unsafe void _0021ZuneQueryList()
		{
			//IL_0033: Expected I, but got I8
			//IL_004c: Expected I, but got I8
			//IL_0055: Expected I, but got I8
			//IL_006f: Expected I, but got I8
			_ = string.Empty;
			ushort* bstrQueryName = m_bstrQueryName;
			if (bstrQueryName != null)
			{
				new string((char*)bstrQueryName);
			}
			ResultSetEventRelay* pRelay = m_pRelay;
			if (0L != (nint)pRelay)
			{
				_003CModule_003E.MicrosoftZuneLibrary_002EResultSetEventRelay_002E__delDtor(pRelay, 1u);
				m_pRelay = null;
			}
			IDatabaseQueryResults* pResults = m_pResults;
			if (0L != (nint)pResults)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pResults + 16)))((nint)pResults);
				m_pResults = null;
			}
			ushort* bstrQueryName2 = m_bstrQueryName;
			if (0L != (nint)bstrQueryName2)
			{
				_003CModule_003E.SysFreeString(bstrQueryName2);
				m_bstrQueryName = null;
			}
		}

		public uint AddRef()
		{
			return (uint)Interlocked.Increment(ref m_RefCount);
		}

		public uint Release()
		{
			int num = Interlocked.Decrement(ref m_RefCount);
			if (0 == num)
			{
				((IDisposable)this).Dispose();
			}
			return (uint)num;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool CheckItemIndex(uint index)
		{
			//IL_0015: Expected I, but got I8
			IDatabaseQueryResults* pResults = m_pResults;
			return (byte)((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, int>)(*(ulong*)(*(long*)pResults + 24)))((nint)pResults, index) >= 0) ? 1u : 0u) != 0;
		}

		public unsafe void Advise(IQueryListEvents listener)
		{
			_003CModule_003E.MicrosoftZuneLibrary_002EResultSetEventRelay_002EAdvise(m_pRelay, listener, m_pResults);
		}

		public unsafe void Unadvise(IQueryListEvents listener)
		{
			_003CModule_003E.MicrosoftZuneLibrary_002EResultSetEventRelay_002EUnAdvise(m_pRelay, listener, m_pResults);
		}

		public unsafe void EndBulkEventsComplete([MarshalAs(UnmanagedType.U1)] bool fAbandon)
		{
			//IL_0021: Expected I, but got I8
			int num = (fAbandon ? 1 : 0);
			IDatabaseQueryResults* pResults = m_pResults;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, void>)(*(ulong*)(*(long*)pResults + 128)))((nint)pResults, num);
		}

		public object GetFieldValue(uint index, Type type, string AtomName, object defaultValue)
		{
			int num = AtomNameToAtom(AtomName);
			if (num == -1)
			{
				return defaultValue;
			}
			return GetFieldValue(index, type, (uint)num, defaultValue);
		}

		public object GetFieldValue(uint index, Type type, string AtomName)
		{
			return GetFieldValue(index, type, AtomName, null);
		}

		public unsafe object GetFieldValue(uint index, Type type, uint Atom, object defaultValue)
		{
			//IL_008d: Expected I, but got I8
			//IL_00a5: Expected I, but got I8
			//IL_00b7: Expected I4, but got I8
			//IL_00cf: Expected I, but got I8
			if (m_disposed)
			{
				if (Atom != 340 && Atom != 140 && _003CModule_003E.WPP_GLOBAL_Control != System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 28uL)) & 2u) != 0 && (uint)(*(byte*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 25uL)) >= 5u)
				{
					_003CModule_003E.WPP_SF_D(*(ulong*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 16uL), 10, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003FA0xfab9896e_002EWPP_ZuneDBList_cpp_Traceguids), Atom);
				}
				return defaultValue;
			}
			CComPropVariant cComPropVariant;
			object result;
			if (m_pResults != null)
			{
				if (type == typeof(string))
				{
					WMT_ATTR_DATATYPE wMT_ATTR_DATATYPE = *(WMT_ATTR_DATATYPE*)((long)(int)Atom * 32L + (ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _003CModule_003E._003Fs_rgSchemaMapEntry_0040CSchemaMap_0040_00400QBU_SCHEMAMAPENTRY_00401_0040B, 16)));
					if (wMT_ATTR_DATATYPE == (WMT_ATTR_DATATYPE)1)
					{
						ushort* value = null;
						IDatabaseQueryResults* pResults = m_pResults;
						if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, uint, ushort**, int>)(*(ulong*)(*(long*)pResults + 32)))((nint)pResults, index, Atom, &value) >= 0)
						{
							return new string((char*)value);
						}
					}
				}
				// IL initblk instruction
				System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant, 0, 24);
				try
				{
					IDatabaseQueryResults* pResults2 = m_pResults;
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, uint, tagPROPVARIANT*, int>)(*(ulong*)(*(long*)pResults2 + 48)))((nint)pResults2, index, Atom, (tagPROPVARIANT*)(&cComPropVariant)) >= 0)
					{
						result = MarshalResult(type, (tagPROPVARIANT*)(&cComPropVariant), defaultValue);
						goto IL_00ef;
					}
				}
				catch
				{
					//try-fault
					_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
					throw;
				}
				_003CModule_003E.CComPropVariant_002EClear(&cComPropVariant);
			}
			return defaultValue;
			IL_00ef:
			_003CModule_003E.CComPropVariant_002EClear(&cComPropVariant);
			return result;
		}

		public object GetFieldValue(uint index, Type type, uint Atom)
		{
			return GetFieldValue(index, type, Atom, null);
		}

		public int SetFieldValue(uint index, string AtomName, object Value)
		{
			int num = AtomNameToAtom(AtomName);
			if (num == -1)
			{
				return -2147024809;
			}
			return SetFieldValue(index, (uint)num, Value);
		}

		public unsafe int SetFieldValue(uint index, uint Atom, object Value)
		{
			//IL_0016: Expected I4, but got I8
			//IL_0046: Expected I, but got I8
			int num = -2147024809;
			if (m_pResults != null)
			{
				CComPropVariant cComPropVariant;
				// IL initblk instruction
				System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant, 0, 24);
				try
				{
					if (Value == null)
					{
						goto IL_002e;
					}
					num = ConvertTypeToPropVariant(Value.GetType(), Value, (tagPROPVARIANT*)(&cComPropVariant));
					if (num >= 0)
					{
						goto IL_002e;
					}
					goto end_IL_0016;
					IL_002e:
					IDatabaseQueryResults* pResults = m_pResults;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, uint, tagPROPVARIANT*, int>)(*(ulong*)(*(long*)pResults + 64)))((nint)pResults, index, Atom, (tagPROPVARIANT*)(&cComPropVariant));
					end_IL_0016:;
				}
				catch
				{
					//try-fault
					_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
					throw;
				}
				_003CModule_003E.CComPropVariant_002EClear(&cComPropVariant);
			}
			return num;
		}

		public unsafe uint GetIndexForLibraryId(int indexHint, int LibraryId)
		{
			//IL_0029: Expected I, but got I8
			uint num = uint.MaxValue;
			fixed (uint* ptr = &System.Runtime.CompilerServices.Unsafe.AsRef<uint>(&num))
			{
				IDatabaseQueryResults* pResults = m_pResults;
				if (pResults != null)
				{
					long num2 = *(long*)pResults + 144;
					IDatabaseQueryResults* pResults2 = m_pResults;
					num = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, int, uint*, int>)(*(ulong*)num2))((nint)pResults2, (uint)indexHint, LibraryId, ptr) != 0) ? uint.MaxValue : num);
				}
				return num;
			}
		}

		public unsafe ArrayList GetUniqueIds()
		{
			//IL_001e: Expected I, but got I8
			//IL_001e: Expected I, but got I8
			//IL_0079: Expected I, but got I8
			//IL_00bf: Expected I, but got I8
			ArrayList arrayList = null;
			IDatabaseQueryResults* pResults = m_pResults;
			int num;
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int*, int>)(*(ulong*)(*(long*)pResults + 152)))((nint)pResults, null, &num);
			if (num2 >= 0)
			{
				ulong num3 = (ulong)num;
				int* ptr = (int*)_003CModule_003E.new_005B_005D((num3 > 4611686018427387903L) ? ulong.MaxValue : (num3 * 4));
				num2 = (((long)(nint)ptr == 0) ? (-2147024882) : num2);
				if (num2 >= 0)
				{
					pResults = m_pResults;
					num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int*, int>)(*(ulong*)(*(long*)pResults + 152)))((nint)pResults, ptr, &num);
					if (num2 >= 0)
					{
						arrayList = new ArrayList(num);
						num2 = ((arrayList == null) ? (-2147024882) : num2);
						if (num2 >= 0)
						{
							int num4 = 0;
							if (0 < num)
							{
								int* ptr2 = ptr;
								do
								{
									arrayList.Add(*ptr2);
									num4++;
									ptr2 = (int*)((ulong)(nint)ptr2 + 4uL);
								}
								while (num4 < num);
							}
						}
					}
				}
				if (ptr != null)
				{
					_003CModule_003E.delete(ptr);
				}
			}
			return arrayList;
		}

		public unsafe int SearchForString(uint Atom, [MarshalAs(UnmanagedType.U1)] bool ascending, string SearchString)
		{
			//IL_002f: Expected I, but got I8
			int num = -1;
			if (m_pResults != null)
			{
				fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref _003CModule_003E.PtrToStringChars(SearchString)))
				{
					try
					{
						long num2 = *(long*)m_pResults + 72;
						IDatabaseQueryResults* pResults = m_pResults;
						uint num3;
						num = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, int, ushort*, uint*, int>)(*(ulong*)num2))((nint)pResults, Atom, ascending ? 1 : 0, ptr, &num3) >= 0) ? ((int)num3) : num);
					}
					catch
					{
						//try-fault
						ptr = null;
						throw;
					}
				}
			}
			return num;
		}

		public unsafe int ClientBusy([MarshalAs(UnmanagedType.U1)] bool fBusy)
		{
			//IL_001a: Expected I, but got I8
			int result = 0;
			IDatabaseQueryResults* pResults = m_pResults;
			if (pResults != null)
			{
				result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)pResults + 120)))((nint)pResults, fBusy ? 1 : 0);
			}
			return result;
		}

		public unsafe static object MarshalResult(Type type, tagPROPVARIANT* propVariant, object defaultValue)
		{
			//IL_0104: Expected I, but got I8
			//IL_01d2: Expected I, but got I8
			//IL_0205: Expected I, but got I8
			//IL_0267: Expected I, but got I8
			//IL_02b0: Expected I, but got I8
			if (type == typeof(int))
			{
				switch (*(ushort*)propVariant)
				{
				case 3:
					return *(int*)((ulong)(nint)propVariant + 8uL);
				case 11:
					return (*(short*)((ulong)(nint)propVariant + 8uL) == -1) ? 1 : 0;
				}
			}
			else if (type == typeof(uint))
			{
				ushort num = *(ushort*)propVariant;
				if (num == 3 || num == 19)
				{
					return *(uint*)((ulong)(nint)propVariant + 8uL);
				}
			}
			else if (type == typeof(long))
			{
				switch (*(ushort*)propVariant)
				{
				case 20:
					return *(long*)((ulong)(nint)propVariant + 8uL);
				case 3:
					return (long)(*(int*)((ulong)(nint)propVariant + 8uL));
				case 19:
					return (long)(uint)(*(int*)((ulong)(nint)propVariant + 8uL));
				}
			}
			else if (type == typeof(ulong))
			{
				ushort num2 = *(ushort*)propVariant;
				if (num2 == 20 || num2 == 21)
				{
					return *(ulong*)((ulong)(nint)propVariant + 8uL);
				}
			}
			else if (type == typeof(string))
			{
				if (*(ushort*)propVariant == 72)
				{
					_003CModule_003E.ZuneLibraryExports_002EZunePropVariantChangeType(propVariant, propVariant, 0, 8);
				}
				if (*(ushort*)propVariant == 8)
				{
					return new string((char*)(*(ulong*)((ulong)(nint)propVariant + 8uL)));
				}
			}
			else if (type == typeof(DateTime))
			{
				if (*(ushort*)propVariant == 7)
				{
					return DateTime.FromOADate(*(double*)((ulong)(nint)propVariant + 8uL));
				}
			}
			else if (type == typeof(TimeSpan))
			{
				switch (*(ushort*)propVariant)
				{
				case 3:
					return TimeSpan.FromMilliseconds(*(int*)((ulong)(nint)propVariant + 8uL));
				case 20:
				case 21:
					return TimeSpan.FromMilliseconds(*(ulong*)((ulong)(nint)propVariant + 8uL));
				}
			}
			else if (type == typeof(bool))
			{
				if (*(ushort*)propVariant == 11)
				{
					return (byte)((*(short*)((ulong)(nint)propVariant + 8uL) != 0) ? 1u : 0u) != 0;
				}
			}
			else if (type == typeof(Guid))
			{
				if (*(ushort*)propVariant == 8)
				{
					ValueType valueType = default(Guid);
					(Guid)valueType = new Guid(new string((char*)(*(ulong*)((ulong)(nint)propVariant + 8uL))));
					return valueType;
				}
			}
			else if (typeof(IList).IsAssignableFrom(type))
			{
				switch (*(ushort*)propVariant)
				{
				case 8200:
				{
					tagSAFEARRAY* ptr2 = (tagSAFEARRAY*)(*(ulong*)((ulong)(nint)propVariant + 8uL));
					if (_003CModule_003E.SafeArrayGetDim(ptr2) != 1)
					{
						return defaultValue;
					}
					ArrayList arrayList2 = new ArrayList();
					if (arrayList2 != null)
					{
						int num5 = 0;
						int num6 = 0;
						ushort** ptr3;
						if (_003CModule_003E.SafeArrayGetLBound(ptr2, 1u, &num5) < 0 || _003CModule_003E.SafeArrayGetUBound(ptr2, 1u, &num6) < 0 || _003CModule_003E.SafeArrayAccessData(ptr2, (void**)(&ptr3)) < 0)
						{
							return defaultValue;
						}
						uint num7 = (uint)(num6 - num5 + 1);
						if (0 < num7)
						{
							long num8 = 0L;
							uint num9 = num7;
							do
							{
								arrayList2.Add(new string((char*)(*(ulong*)(num8 + (nint)ptr3))));
								num8 += 8;
								num9 += uint.MaxValue;
							}
							while (num9 != 0);
						}
						_003CModule_003E.SafeArrayUnaccessData(ptr2);
					}
					return arrayList2;
				}
				case 4099:
				{
					ArrayList arrayList = new ArrayList();
					if (arrayList != null)
					{
						uint num3 = 0u;
						if (0u < (uint)(*(int*)((ulong)(nint)propVariant + 8uL)))
						{
							tagPROPVARIANT* ptr = (tagPROPVARIANT*)((ulong)(nint)propVariant + 16uL);
							long num4 = 0L;
							do
							{
								arrayList.Add(*(int*)(num4 + *(long*)ptr));
								num3++;
								num4 += 4;
							}
							while (num3 < (uint)(*(int*)((ulong)(nint)propVariant + 8uL)));
						}
					}
					return arrayList;
				}
				}
			}
			else if (type.IsEnum)
			{
				ushort num10 = *(ushort*)propVariant;
				if (num10 == 3 || num10 == 19)
				{
					return Enum.ToObject(type, *(int*)((ulong)(nint)propVariant + 8uL));
				}
			}
			return defaultValue;
		}

		public unsafe static int ConvertTypeToPropVariant(Type type, object value, tagPROPVARIANT* propVariant)
		{
			//IL_003a: Expected I8, but got I
			//IL_020e: Expected I4, but got I8
			//IL_0297: Expected I8, but got I
			//The blocks IL_02a0 are reachable both inside and outside the pinned region starting at IL_002c. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
			//The blocks IL_0217, IL_0225, IL_0270, IL_0278, IL_0299, IL_02a0 are reachable both inside and outside the pinned region starting at IL_0244. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
			int result = -2147467259;
			if (type == null)
			{
				type = value.GetType();
			}
			if (type == typeof(string))
			{
				*(short*)propVariant = 8;
				fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref _003CModule_003E.PtrToStringChars((string)value)))
				{
					*(long*)((ulong)(nint)propVariant + 8uL) = (nint)_003CModule_003E.SysAllocString(ptr);
					return 0;
				}
			}
			if (type == typeof(int))
			{
				*(short*)propVariant = 3;
				*(int*)((ulong)(nint)propVariant + 8uL) = (int)value;
				result = 0;
			}
			else if (type == typeof(uint))
			{
				*(short*)propVariant = 19;
				*(uint*)((ulong)(nint)propVariant + 8uL) = (uint)value;
				result = 0;
			}
			else if (type == typeof(long))
			{
				*(short*)propVariant = 20;
				*(long*)((ulong)(nint)propVariant + 8uL) = (long)value;
				result = 0;
			}
			else if (type == typeof(ulong))
			{
				*(short*)propVariant = 20;
				*(long*)((ulong)(nint)propVariant + 8uL) = (long)value;
				result = 0;
			}
			else if (type == typeof(DateTime))
			{
				DateTime dateTime = (DateTime)value;
				if (dateTime != DateTime.MinValue)
				{
					if (dateTime.Year >= 0 && dateTime.Year < 100)
					{
						dateTime = ((dateTime.Year >= 30) ? dateTime.AddYears(1900) : dateTime.AddYears(2000));
					}
					*(short*)propVariant = 7;
					*(double*)((ulong)(nint)propVariant + 8uL) = dateTime.ToOADate();
				}
				result = 0;
			}
			else if (type == typeof(TimeSpan))
			{
				*(short*)propVariant = 3;
				*(int*)((ulong)(nint)propVariant + 8uL) = (int)((TimeSpan)(TimeSpan)value).TotalMilliseconds;
				result = 0;
			}
			else if (type == typeof(bool))
			{
				*(short*)propVariant = 11;
				int num = -1;
				if (!(bool)value)
				{
					num = ~num;
				}
				short num2 = (short)num;
				*(short*)((ulong)(nint)propVariant + 8uL) = num2;
				result = 0;
			}
			else if (typeof(IList).IsAssignableFrom(type))
			{
				IList list = (IList)value;
				if (list != null)
				{
					int count = list.Count;
					if (count == 0)
					{
						_003CModule_003E.PropVariantClear(propVariant);
					}
					else
					{
						_0024ArrayType_0024_0024_0024BY00UtagSAFEARRAYBOUND_0040_0040 _0024ArrayType_0024_0024_0024BY00UtagSAFEARRAYBOUND_0040_0040;
						System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00UtagSAFEARRAYBOUND_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY00UtagSAFEARRAYBOUND_0040_0040, 4)) = 0;
						*(int*)(&_0024ArrayType_0024_0024_0024BY00UtagSAFEARRAYBOUND_0040_0040) = count;
						tagSAFEARRAY* ptr2 = _003CModule_003E.SafeArrayCreate(8, 1u, (tagSAFEARRAYBOUND*)(&_0024ArrayType_0024_0024_0024BY00UtagSAFEARRAYBOUND_0040_0040));
						if (ptr2 == null)
						{
							return -2147024882;
						}
						_0024ArrayType_0024_0024_0024BY01J _0024ArrayType_0024_0024_0024BY01J;
						*(int*)(&_0024ArrayType_0024_0024_0024BY01J) = 0;
						// IL initblk instruction
						System.Runtime.CompilerServices.Unsafe.InitBlockUnaligned(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY01J, 4), 0, 4);
						int index = 0;
						if (0 < count)
						{
							object obj = list[index];
							if (obj != null)
							{
								if (obj.GetType() == typeof(string))
								{
									while (true)
									{
										fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref _003CModule_003E.PtrToStringChars((string)obj)))
										{
											IL_0217:
											obj = list[index];
											if (obj != null)
											{
												if (obj.GetType() != typeof(string))
												{
													result = -2147467259;
													goto IL_0299;
												}
												continue;
											}
											result = -2147467259;
											goto IL_0299;
											IL_0299:
											_003CModule_003E.SafeArrayDestroy(ptr2);
											goto IL_02a0;
											IL_024d:
											ushort* ptr4 = _003CModule_003E.SysAllocString(ptr3);
											if (ptr4 == null)
											{
												goto IL_0280;
											}
											goto IL_0253;
											IL_0253:
											*(int*)(&_0024ArrayType_0024_0024_0024BY01J) = index;
											result = _003CModule_003E.SafeArrayPutElement(ptr2, (int*)(&_0024ArrayType_0024_0024_0024BY01J), ptr4);
											if (result < 0)
											{
												goto IL_0299;
											}
											goto IL_0266;
											IL_0266:
											index++;
											if (index < count)
											{
												goto IL_0217;
											}
											goto IL_0286;
											IL_0280:
											result = -2147024882;
											goto IL_0286;
											IL_0286:
											if (result < 0)
											{
												goto IL_0299;
											}
											goto IL_028a;
											IL_028a:
											*(short*)propVariant = 8200;
											*(long*)((ulong)(nint)propVariant + 8uL) = (nint)ptr2;
											goto IL_02a0;
											IL_02a0:
											return result;
										}
									}
								}
								result = -2147467259;
							}
							else
							{
								result = -2147467259;
							}
						}
						_003CModule_003E.SafeArrayDestroy(ptr2);
					}
				}
			}
			return result;
		}

		public unsafe static int AtomNameToAtom(string AtomName)
		{
			fixed (ushort* wszName = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref _003CModule_003E.PtrToStringChars(AtomName)))
			{
				return _003CModule_003E.CSchemaMap_002EGetIndex(wszName);
			}
		}

		public unsafe static string AtomToAtomName(int atom)
		{
			//IL_0012: Expected I, but got I8
			return new string((char*)(*(ulong*)((long)atom * 32L + (ref _003CModule_003E._003Fs_rgSchemaMapEntry_0040CSchemaMap_0040_00400QBU_SCHEMAMAPENTRY_00401_0040B))));
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_007EZuneQueryList();
				return;
			}
			try
			{
				_0021ZuneQueryList();
			}
			finally
			{
				base.Finalize();
			}
		}

		public sealed override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~ZuneQueryList()
		{
			Dispose(false);
		}
	}
}
