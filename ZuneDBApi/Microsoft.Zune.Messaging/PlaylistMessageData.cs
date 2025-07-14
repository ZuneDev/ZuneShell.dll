using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Iris;
using ZuneDBApi.Interop;

namespace Microsoft.Zune.Messaging;

public class PlaylistMessageData : IPropertySetMessageData
{
	private string m_title;

	private string m_author;

	private IList m_tracks;

	private unsafe IMSMediaSchemaPropertySet* m_pPlaylistPropSet;

	public unsafe PlaylistMessageData(string title, string author, IList tracks)
	{
		//IL_0023: Expected I, but got I8
		m_title = title;
		m_author = author;
		m_tracks = tracks;
		m_pPlaylistPropSet = null;
	}

	private void _007EPlaylistMessageData()
	{
		_0021PlaylistMessageData();
	}

	private unsafe void _0021PlaylistMessageData()
	{
		//IL_0019: Expected I, but got I8
		//IL_0022: Expected I, but got I8
		IMSMediaSchemaPropertySet* pPlaylistPropSet = m_pPlaylistPropSet;
		if (0L != (nint)pPlaylistPropSet)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pPlaylistPropSet + 16)))((nint)pPlaylistPropSet);
			m_pPlaylistPropSet = null;
		}
	}

	public unsafe virtual int GetPropertySet(IMSMediaSchemaPropertySet** ppPropSet)
	{
		//IL_001b: Expected I, but got I8
		//IL_003c: Expected I, but got I8
		//IL_006b: Expected I4, but got I8
		//IL_010e: Expected I4, but got I8
		//IL_00c5: Expected I4, but got I8
		//IL_007b: Expected I8, but got I
		//IL_0093: Expected I, but got I8
		//IL_00d5: Expected I8, but got I
		//IL_00ed: Expected I, but got I8
		//IL_011d: Expected I8, but got I
		//IL_0133: Expected I, but got I8
		//IL_0156: Expected I, but got I8
		//IL_015a: Expected I, but got I8
		//IL_0142: Expected I8, but got I
		//IL_016e: Expected I, but got I8
		//IL_0172: Expected I, but got I8
		if (ppPropSet == null)
		{
			global::_003CModule_003E._ZuneShipAssert(1001u, 141u);
			return -2147467261;
		}
		IMSMediaSchemaPropertyList* ptr = null;
		int num = ZuneLibraryExports.CreatePropertySetList((_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E.ID_MS_MEDIA_SCHEMA_PLAYLIST), 3229617665u, &ptr);
		if (num >= 0)
		{
			num = AddTracksToPropList(ptr);
		}
		IMSMediaSchemaPropertySet* ptr2 = null;
		if (num >= 0)
		{
			num = ZuneLibraryExports.CreatePropertySet((_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E.ID_MS_MEDIA_SCHEMA_PLAYLIST), 3229617665u, &ptr2);
		}
		fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_title)))
		{
			if (num >= 0)
			{
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPropVariant cComPropVariant);
				// IL initblk instruction
				System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant, 0, 24);
				try
				{
					*(short*)(&cComPropVariant) = 8;
					System.Runtime.CompilerServices.Unsafe.As<CComPropVariant, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cComPropVariant, 8)) = (nint)global::_003CModule_003E.SysAllocString(ptr3);
					tagPROPVARIANT tagPROPVARIANT = (tagPROPVARIANT)cComPropVariant;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, tagPROPVARIANT, int>)(*(ulong*)(*(long*)ptr2 + 56)))((nint)ptr2, 16777217u, tagPROPVARIANT);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&global::_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
					throw;
				}
				global::_003CModule_003E.CComPropVariant_002EClear(&cComPropVariant);
			}
			fixed (ushort* ptr4 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(m_author)))
			{
				if (num >= 0)
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPropVariant cComPropVariant2);
					// IL initblk instruction
					System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant2, 0, 24);
					try
					{
						*(short*)(&cComPropVariant2) = 8;
						System.Runtime.CompilerServices.Unsafe.As<CComPropVariant, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cComPropVariant2, 8)) = (nint)global::_003CModule_003E.SysAllocString(ptr4);
						tagPROPVARIANT tagPROPVARIANT2 = (tagPROPVARIANT)cComPropVariant2;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, tagPROPVARIANT, int>)(*(ulong*)(*(long*)ptr2 + 56)))((nint)ptr2, 16777219u, tagPROPVARIANT2);
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&global::_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant2);
						throw;
					}
					global::_003CModule_003E.CComPropVariant_002EClear(&cComPropVariant2);
				}
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPropVariant cComPropVariant3);
				// IL initblk instruction
				System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant3, 0, 24);
				try
				{
					if (num < 0)
					{
						goto IL_0144;
					}
					*(short*)(&cComPropVariant3) = 13;
					System.Runtime.CompilerServices.Unsafe.As<CComPropVariant, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cComPropVariant3, 8)) = (nint)ptr;
					tagPROPVARIANT tagPROPVARIANT3 = (tagPROPVARIANT)cComPropVariant3;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, tagPROPVARIANT, int>)(*(ulong*)(*(long*)ptr2 + 56)))((nint)ptr2, 3229617665u, tagPROPVARIANT3);
					if (num < 0)
					{
						goto IL_0144;
					}
					m_pPlaylistPropSet = ptr2;
					*(long*)ppPropSet = (nint)ptr2;
					goto end_IL_010e;
					IL_0144:
					if (0L != (nint)ptr)
					{
						IMSMediaSchemaPropertyList* intPtr = ptr;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
						ptr = null;
					}
					if (0L != (nint)ptr2)
					{
						IMSMediaSchemaPropertySet* intPtr2 = ptr2;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
						ptr2 = null;
					}
					end_IL_010e:;
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&global::_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant3);
					throw;
				}
				global::_003CModule_003E.CComPropVariant_002EClear(&cComPropVariant3);
				return num;
			}
		}
	}

	private unsafe int AddTracksToPropList(IMSMediaSchemaPropertyList* pPropList)
	{
		//IL_00fc: Expected I, but got I8
		//IL_0122: Expected I4, but got I8
		//IL_018b: Expected I, but got I8
		//IL_018f: Expected I, but got I8
		//IL_0132: Expected I8, but got I
		//IL_014a: Expected I, but got I8
		//IL_0178: Expected I, but got I8
		//IL_0178: Expected I, but got I8
		//The blocks IL_0024, IL_0035, IL_004d, IL_0063, IL_0075, IL_0091, IL_0098, IL_018f, IL_019a are reachable both inside and outside the pinned region starting at IL_009f. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
		if (pPropList == null)
		{
			global::_003CModule_003E._ZuneShipAssert(1001u, 45u);
			return -2147467261;
		}
		int num = 0;
		if (m_tracks != null)
		{
			int num2 = 0;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPropVariant cComPropVariant);
			while (num2 < m_tracks.Count)
			{
				DataProviderObject dataProviderObject = m_tracks[num2] as DataProviderObject;
				if (dataProviderObject != null)
				{
					_GUID gUID = global::_003CModule_003E.GUID_NULL;
					object property = dataProviderObject.GetProperty("ZuneMediaId");
					if (property != null)
					{
						gUID = global::_003CModule_003E.GuidToGUID((Guid)property);
					}
					string text = dataProviderObject.GetProperty("Title") as string;
					object s = ((!(text == null)) ? text : "");
					while (true)
					{
						fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars((string)s)))
						{
							string text2 = dataProviderObject.GetProperty("ArtistName") as string;
							fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars((!(text2 == null)) ? text2 : "")))
							{
								string text3 = dataProviderObject.GetProperty("AlbumName") as string;
								fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars((!(text3 == null)) ? text3 : "")))
								{
									IMSMediaSchemaPropertySet* ptr4 = null;
									num = ZuneLibraryExports.CreateTrackPropSet(gUID, global::_003CModule_003E.GUID_NULL, 0, ptr, 0, ptr3, ptr2, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_C_0040_11LOCGONAA_0040_003F_0024AA_003F_0024AA_0040), &ptr4);
									if (num >= 0)
									{
										// IL initblk instruction
										System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant, 0, 24);
										try
										{
											*(short*)(&cComPropVariant) = 8;
											System.Runtime.CompilerServices.Unsafe.As<CComPropVariant, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cComPropVariant, 8)) = (nint)global::_003CModule_003E.SysAllocString(ptr2);
											tagPROPVARIANT tagPROPVARIANT = (tagPROPVARIANT)cComPropVariant;
											num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, tagPROPVARIANT, int>)(*(ulong*)(*(long*)ptr4 + 56)))((nint)ptr4, 16785409u, tagPROPVARIANT);
										}
										catch
										{
											//try-fault
											global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&global::_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
											throw;
										}
										global::_003CModule_003E.CComPropVariant_002EClear(&cComPropVariant);
										if (num >= 0)
										{
											num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, IMSMediaSchemaPropertySet*, IMSMediaSchemaPropertySet**, int>)(*(ulong*)(*(long*)pPropList + 72)))((nint)pPropList, 0u, ptr4, null);
										}
									}
									if (0L != (nint)ptr4)
									{
										IMSMediaSchemaPropertySet* intPtr = ptr4;
										((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
										ptr4 = null;
									}
									do
									{
										num2++;
										if (num >= 0 && num2 < m_tracks.Count)
										{
											dataProviderObject = m_tracks[num2] as DataProviderObject;
											continue;
										}
										return num;
									}
									while (dataProviderObject == null);
									gUID = global::_003CModule_003E.GUID_NULL;
									property = dataProviderObject.GetProperty("ZuneMediaId");
									if (property != null)
									{
										gUID = global::_003CModule_003E.GuidToGUID((Guid)property);
									}
									text = dataProviderObject.GetProperty("Title") as string;
									s = ((!(text == null)) ? text : "");
								}
							}
						}
					}
				}
				num2++;
				if (num < 0)
				{
					break;
				}
			}
		}
		return num;
	}

	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
	{
		if (P_0)
		{
			_0021PlaylistMessageData();
			return;
		}
		try
		{
			_0021PlaylistMessageData();
		}
		finally
		{
			base.Finalize();
		}
	}

	public virtual sealed void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	~PlaylistMessageData()
	{
		Dispose(false);
	}
}
