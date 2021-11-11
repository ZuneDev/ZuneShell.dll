using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using MicrosoftZuneLibrary;
using ZuneUI;

namespace Microsoft.Zune.Playlist
{
	public class AutoPlaylistBuilder : IDisposable
	{
		private unsafe IAutoPlaylistRules* m_pAutoPlaylistRules;

		private int m_currentRuleSetGroup;

		private Dictionary<GroupAndAtom, AtomRules> m_groupAndAtomToRules;

		private EMediaTypes m_type;

		public EMediaTypes Schema => m_type;

		public unsafe AutoPlaylistBuilder(int playlistId)
		{
			//IL_002b: Expected I, but got I8
			//IL_0059: Expected I4, but got I8
			//IL_0074: Expected I, but got I8
			//IL_00d4: Expected I4, but got I8
			//IL_00e6: Expected I, but got I8
			//IL_0136: Expected I4, but got I8
			//IL_0149: Expected I, but got I8
			m_currentRuleSetGroup = 0;
			IPlaylistManager* nativePlaylistManager = PlaylistManager.Instance.NativePlaylistManager;
			IAutoPlaylistRules* ptr;
			if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, IAutoPlaylistRules**, int>)(*(ulong*)(*(long*)nativePlaylistManager + 112)))((nint)nativePlaylistManager, playlistId, &ptr) < 0)
			{
				return;
			}
			m_pAutoPlaylistRules = ptr;
			PlaylistManager.Instance.GetAutoPlaylistSchema(playlistId, out m_type);
			int num = 0;
			int num2 = 0;
			bool flag = true;
			do
			{
				CComPropVariant cComPropVariant;
				// IL initblk instruction
				System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant, 0, 24);
				try
				{
					IAutoPlaylistRules* intPtr = ptr;
					EMediaTypes type = m_type;
					uint atom;
					EAutoPlaylistRuleOperators op;
					int num3 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaTypes, uint, uint, uint*, EAutoPlaylistRuleOperators*, tagPROPVARIANT*, int>)(*(ulong*)(*(long*)ptr + 40)))((nint)intPtr, type, (uint)num, (uint)num2, &atom, &op, (tagPROPVARIANT*)(&cComPropVariant));
					if (num3 >= 0)
					{
						if (num3 == 1)
						{
							if (num2 == 0)
							{
								flag = false;
							}
							else
							{
								num2 = 0;
								num++;
							}
						}
						else
						{
							CacheCriterion(num, (int)atom, (tagPROPVARIANT*)(&cComPropVariant), op);
							num2++;
						}
						goto IL_00b3;
					}
				}
				catch
				{
					//try-fault
					_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
					throw;
				}
				_003CModule_003E.CComPropVariant_002EClear(&cComPropVariant);
				break;
				IL_00b3:
				_003CModule_003E.CComPropVariant_002EClear(&cComPropVariant);
			}
			while (flag);
			int num4 = 0;
			CComPropVariant cComPropVariant2;
			// IL initblk instruction
			System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant2, 0, 24);
			int num5;
			uint atom2;
			try
			{
				IAutoPlaylistRules* intPtr2 = ptr;
				num5 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, uint*, tagPROPVARIANT*, int>)(*(ulong*)(*(long*)ptr + 72)))((nint)intPtr2, 0u, &atom2, (tagPROPVARIANT*)(&cComPropVariant2));
				if (num5 < 0)
				{
					goto IL_0160;
				}
			}
			catch
			{
				//try-fault
				_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant2);
				throw;
			}
			while (true)
			{
				try
				{
					if (num5 == 1)
					{
						break;
					}
					CacheCriterion(0, (int)atom2, (tagPROPVARIANT*)(&cComPropVariant2), (EAutoPlaylistRuleOperators)8);
					num4++;
					goto IL_0126;
				}
				catch
				{
					//try-fault
					_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant2);
					throw;
				}
				IL_0126:
				_003CModule_003E.CComPropVariant_002EClear(&cComPropVariant2);
				// IL initblk instruction
				System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant2, 0, 24);
				try
				{
					IAutoPlaylistRules* intPtr3 = ptr;
					num5 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, uint*, tagPROPVARIANT*, int>)(*(ulong*)(*(long*)ptr + 72)))((nint)intPtr3, (uint)num4, &atom2, (tagPROPVARIANT*)(&cComPropVariant2));
					if (num5 >= 0)
					{
						continue;
					}
				}
				catch
				{
					//try-fault
					_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant2);
					throw;
				}
				break;
			}
			goto IL_0160;
			IL_0160:
			_003CModule_003E.CComPropVariant_002EClear(&cComPropVariant2);
		}

		public AutoPlaylistBuilder(EMediaTypes type)
		{
			InitializeNewBuilder(type);
		}

		public AutoPlaylistBuilder()
		{
			InitializeNewBuilder(EMediaTypes.eMediaTypeAudio);
		}

		private unsafe void _007EAutoPlaylistBuilder()
		{
			//IL_0019: Expected I, but got I8
			//IL_0022: Expected I, but got I8
			IAutoPlaylistRules* pAutoPlaylistRules = m_pAutoPlaylistRules;
			if (0L != (nint)pAutoPlaylistRules)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pAutoPlaylistRules + 16)))((nint)pAutoPlaylistRules);
				m_pAutoPlaylistRules = null;
			}
		}

		public unsafe void Clear()
		{
			//IL_0019: Expected I, but got I8
			//IL_0022: Expected I4, but got I8
			//IL_0054: Expected I, but got I8
			m_currentRuleSetGroup = 0;
			IAutoPlaylistRules* pAutoPlaylistRules = m_pAutoPlaylistRules;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pAutoPlaylistRules + 24)))((nint)pAutoPlaylistRules);
			CComPropVariant cComPropVariant;
			// IL initblk instruction
			System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant, 0, 24);
			try
			{
				*(short*)(&cComPropVariant) = 3;
				System.Runtime.CompilerServices.Unsafe.As<CComPropVariant, short>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cComPropVariant, 8)) = 1;
				IAutoPlaylistRules* pAutoPlaylistRules2 = m_pAutoPlaylistRules;
				EMediaTypes type = m_type;
				int currentRuleSetGroup = m_currentRuleSetGroup;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaTypes, uint, uint, EAutoPlaylistRuleOperators, tagPROPVARIANT*, int>)(*(ulong*)(*(long*)pAutoPlaylistRules2 + 32)))((nint)pAutoPlaylistRules2, type, (uint)currentRuleSetGroup, 204u, (EAutoPlaylistRuleOperators)1, (tagPROPVARIANT*)(&cComPropVariant));
			}
			catch
			{
				//try-fault
				_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
				throw;
			}
			_003CModule_003E.CComPropVariant_002EClear(&cComPropVariant);
		}

		public unsafe HRESULT AddCriterion(string atomName, PlaylistRuleOperator op, object value)
		{
			//IL_000f: Expected I4, but got I8
			//IL_0041: Expected I, but got I8
			int num = ZuneQueryList.AtomNameToAtom(atomName);
			CComPropVariant cComPropVariant;
			// IL initblk instruction
			System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant, 0, 24);
			HRESULT result;
			try
			{
				int num2 = ZuneQueryList.ConvertTypeToPropVariant(null, value, (tagPROPVARIANT*)(&cComPropVariant));
				if (num2 >= 0)
				{
					IAutoPlaylistRules* pAutoPlaylistRules = m_pAutoPlaylistRules;
					EMediaTypes type = m_type;
					int currentRuleSetGroup = m_currentRuleSetGroup;
					num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaTypes, uint, uint, EAutoPlaylistRuleOperators, tagPROPVARIANT*, int>)(*(ulong*)(*(long*)pAutoPlaylistRules + 32)))((nint)pAutoPlaylistRules, type, (uint)currentRuleSetGroup, (uint)num, (EAutoPlaylistRuleOperators)op, (tagPROPVARIANT*)(&cComPropVariant));
				}
				result = num2;
			}
			catch
			{
				//try-fault
				_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
				throw;
			}
			_003CModule_003E.CComPropVariant_002EClear(&cComPropVariant);
			return result;
		}

		public unsafe HRESULT AddSort(string sort)
		{
			//IL_004a: Expected I, but got I8
			bool[] ascendings = null;
			string[] sorts = null;
			int num = 0;
			if (LibraryDataProvider.GetSortAttributes(sort, out sorts, out ascendings))
			{
				int num2 = 0;
				if (0 < (nint)sorts.LongLength)
				{
					do
					{
						int num3 = ZuneQueryList.AtomNameToAtom(sorts[num2]);
						IAutoPlaylistRules* pAutoPlaylistRules = m_pAutoPlaylistRules;
						EMediaTypes type = m_type;
						int currentRuleSetGroup = m_currentRuleSetGroup;
						bool num4 = ascendings[num2];
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaTypes, uint, uint, int, int>)(*(ulong*)(*(long*)pAutoPlaylistRules + 48)))((nint)pAutoPlaylistRules, type, (uint)currentRuleSetGroup, (uint)num3, num4 ? 1 : 0);
						if (num < 0)
						{
							break;
						}
						num2++;
					}
					while (num2 < (nint)sorts.LongLength);
				}
			}
			return num;
		}

		public unsafe HRESULT AddFilter(string atomName, object value)
		{
			//IL_000f: Expected I4, but got I8
			//IL_0034: Expected I, but got I8
			int num = ZuneQueryList.AtomNameToAtom(atomName);
			CComPropVariant cComPropVariant;
			// IL initblk instruction
			System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant, 0, 24);
			HRESULT result;
			try
			{
				int num2 = ZuneQueryList.ConvertTypeToPropVariant(null, value, (tagPROPVARIANT*)(&cComPropVariant));
				if (num2 >= 0)
				{
					IAutoPlaylistRules* pAutoPlaylistRules = m_pAutoPlaylistRules;
					num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, tagPROPVARIANT*, int>)(*(ulong*)(*(long*)pAutoPlaylistRules + 64)))((nint)pAutoPlaylistRules, (uint)num, (tagPROPVARIANT*)(&cComPropVariant));
				}
				result = num2;
			}
			catch
			{
				//try-fault
				_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
				throw;
			}
			_003CModule_003E.CComPropVariant_002EClear(&cComPropVariant);
			return result;
		}

		public unsafe HRESULT SetRules(int playlistId)
		{
			//IL_001f: Expected I, but got I8
			IPlaylistManager* nativePlaylistManager = PlaylistManager.Instance.NativePlaylistManager;
			IAutoPlaylistRules* pAutoPlaylistRules = m_pAutoPlaylistRules;
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, IAutoPlaylistRules*, int>)(*(ulong*)(*(long*)nativePlaylistManager + 120)))((nint)nativePlaylistManager, playlistId, pAutoPlaylistRules);
		}

		public AtomRules GetCriterionByAtomName(int ruleSetGroup, string atomName)
		{
			AtomRules value = null;
			GroupAndAtom key = new GroupAndAtom(ruleSetGroup, ZuneQueryList.AtomNameToAtom(atomName));
			Dictionary<GroupAndAtom, AtomRules> groupAndAtomToRules = m_groupAndAtomToRules;
			if (groupAndAtomToRules != null && groupAndAtomToRules.TryGetValue(key, out value))
			{
				return value;
			}
			return null;
		}

		public object GetFilterByAtomName(string atomName)
		{
			return GetCriterionByAtomName(0, atomName)?.values[0];
		}

		public unsafe string GetSort(int ruleSetGroup)
		{
			//IL_0024: Expected I, but got I8
			//IL_0091: Expected I, but got I8
			StringBuilder stringBuilder = null;
			int num = 0;
			IAutoPlaylistRules* pAutoPlaylistRules = m_pAutoPlaylistRules;
			IAutoPlaylistRules* intPtr = pAutoPlaylistRules;
			EMediaTypes type = m_type;
			uint atom;
			int num2;
			int num3 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaTypes, uint, uint, uint*, int*, int>)(*(ulong*)(*(long*)pAutoPlaylistRules + 56)))((nint)intPtr, type, (uint)ruleSetGroup, 0u, &atom, &num2);
			if (num3 >= 0)
			{
				while (num3 != 1)
				{
					string value = ((num2 == 0) ? "-" : "+") + ZuneQueryList.AtomToAtomName((int)atom);
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(value);
					}
					else
					{
						stringBuilder.Append(",");
						stringBuilder.Append(value);
					}
					num++;
					pAutoPlaylistRules = m_pAutoPlaylistRules;
					IAutoPlaylistRules* intPtr2 = pAutoPlaylistRules;
					EMediaTypes type2 = m_type;
					num3 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaTypes, uint, uint, uint*, int*, int>)(*(ulong*)(*(long*)pAutoPlaylistRules + 56)))((nint)intPtr2, type2, (uint)ruleSetGroup, (uint)num, &atom, &num2);
					if (num3 < 0)
					{
						break;
					}
				}
			}
			return stringBuilder?.ToString();
		}

		private unsafe void InitializeNewBuilder(EMediaTypes type)
		{
			//IL_001d: Expected I, but got I8
			IPlaylistManager* nativePlaylistManager = PlaylistManager.Instance.NativePlaylistManager;
			IAutoPlaylistRules* pAutoPlaylistRules;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IAutoPlaylistRules**, int>)(*(ulong*)(*(long*)nativePlaylistManager + 128)))((nint)nativePlaylistManager, &pAutoPlaylistRules);
			m_pAutoPlaylistRules = pAutoPlaylistRules;
			m_currentRuleSetGroup = 0;
			m_type = type;
			if (num < 0)
			{
				throw new ApplicationException(_003CModule_003E.GetErrorDescription(num));
			}
		}

		private unsafe void CacheCriterion(int group, int atom, tagPROPVARIANT* vtValue, EAutoPlaylistRuleOperators op)
		{
			AtomRules value = null;
			if (m_groupAndAtomToRules == null)
			{
				m_groupAndAtomToRules = new Dictionary<GroupAndAtom, AtomRules>();
			}
			GroupAndAtom key = new GroupAndAtom(group, atom);
			if (!m_groupAndAtomToRules.TryGetValue(key, out value))
			{
				value = new AtomRules();
				m_groupAndAtomToRules[key] = value;
			}
			object value2 = PropVariantToObject(vtValue);
			value.values.Add(value2);
			value.operators.Add((PlaylistRuleOperator)op);
		}

		private unsafe static object PropVariantToObject(tagPROPVARIANT* pvtValue)
		{
			Type type;
			switch (*(ushort*)pvtValue)
			{
			case 8:
				type = typeof(string);
				break;
			case 3:
				type = typeof(int);
				break;
			case 0:
			case 1:
				return null;
			default:
				type = null;
				break;
			}
			if (type != null)
			{
				return ZuneQueryList.MarshalResult(type, pvtValue, null);
			}
			return null;
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_007EAutoPlaylistBuilder();
			}
			else
			{
				Finalize();
			}
		}

		public sealed override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
