using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Iris;

namespace Microsoft.Zune.Messaging
{
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
			//IL_007b: Expected I8, but got I
			//IL_0093: Expected I, but got I8
			//IL_00c5: Expected I4, but got I8
			//IL_00d5: Expected I8, but got I
			//IL_00ed: Expected I, but got I8
			//IL_010e: Expected I4, but got I8
			//IL_011d: Expected I8, but got I
			//IL_0133: Expected I, but got I8
			//IL_0142: Expected I8, but got I
			//IL_0156: Expected I, but got I8
			//IL_015a: Expected I, but got I8
			//IL_016e: Expected I, but got I8
			//IL_0172: Expected I, but got I8
			if (ppPropSet == null)
			{
				Module._ZuneShipAssert(1001u, 141u);
				return -2147467261;
			}
			IMSMediaSchemaPropertyList* ptr = null;
			int num = Module.CreatePropertySetList((_GUID*)Unsafe.AsPointer(ref Module.ID_MS_MEDIA_SCHEMA_PLAYLIST), 3229617665u, &ptr);
			if (num >= 0)
			{
				num = AddTracksToPropList(ptr);
			}
			IMSMediaSchemaPropertySet* ptr2 = null;
			if (num >= 0)
			{
				num = Module.CreatePropertySet((_GUID*)Unsafe.AsPointer(ref Module.ID_MS_MEDIA_SCHEMA_PLAYLIST), 3229617665u, &ptr2);
			}
			fixed (char* m_titlePtr = m_title.ToCharArray())
			{
				ushort* ptr3 = (ushort*)m_titlePtr;
				if (num >= 0)
				{
                    PROPVARIANT cComPropVariant;
                    // IL initblk instruction
                    Unsafe.InitBlock(&cComPropVariant, 0, 24);
					try
					{
						*(short*)(&cComPropVariant) = 8;
                        Unsafe.As<PROPVARIANT, long>(ref Unsafe.AddByteOffset(ref cComPropVariant, 8)) = (System.nint)Module.SysAllocString(ptr3);
						PROPVARIANT tagPROPVARIANT = (PROPVARIANT)cComPropVariant;
						IMSMediaSchemaPropertySet* intPtr = ptr2;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, PROPVARIANT, int>)(*(ulong*)(*(long*)ptr2 + 56)))((nint)intPtr, 16777217u, tagPROPVARIANT);
					}
					catch
					{
                        //try-fault
                        Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<PROPVARIANT*, void>)(&Module.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
						throw;
					}
					Module.CComPropVariant_002EClear(&cComPropVariant);
				}
				fixed (char* m_authorPtr = m_author.ToCharArray())
				{
					ushort* ptr4 = (ushort*)m_authorPtr;
					if (num >= 0)
					{
                        PROPVARIANT cComPropVariant2;
                        // IL initblk instruction
                        Unsafe.InitBlock(&cComPropVariant2, 0, 24);
						try
						{
							*(short*)(&cComPropVariant2) = 8;
                            Unsafe.As<PROPVARIANT, long>(ref Unsafe.AddByteOffset(ref cComPropVariant2, 8)) = (System.nint)Module.SysAllocString(ptr4);
							PROPVARIANT tagPROPVARIANT2 = (PROPVARIANT)cComPropVariant2;
							IMSMediaSchemaPropertySet* intPtr2 = ptr2;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, PROPVARIANT, int>)(*(ulong*)(*(long*)ptr2 + 56)))((nint)intPtr2, 16777219u, tagPROPVARIANT2);
						}
						catch
						{
                            //try-fault
                            Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<PROPVARIANT*, void>)(&Module.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant2);
							throw;
						}
						Module.CComPropVariant_002EClear(&cComPropVariant2);
					}
                    PROPVARIANT cComPropVariant3;
                    // IL initblk instruction
                    Unsafe.InitBlock(&cComPropVariant3, 0, 24);
					try
					{
						if (num < 0)
						{
							goto IL_0144;
						}
						*(short*)(&cComPropVariant3) = 13;
                        Unsafe.As<PROPVARIANT, long>(ref Unsafe.AddByteOffset(ref cComPropVariant3, 8)) = (System.nint)ptr;
						PROPVARIANT tagPROPVARIANT3 = (PROPVARIANT)cComPropVariant3;
						IMSMediaSchemaPropertySet* intPtr3 = ptr2;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, PROPVARIANT, int>)(*(ulong*)(*(long*)ptr2 + 56)))((nint)intPtr3, 3229617665u, tagPROPVARIANT3);
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
							IMSMediaSchemaPropertyList* intPtr4 = ptr;
							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr4 + 16)))((nint)intPtr4);
							ptr = null;
						}
						if (0L != (nint)ptr2)
						{
							IMSMediaSchemaPropertySet* intPtr5 = ptr2;
							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr5 + 16)))((nint)intPtr5);
							ptr2 = null;
						}
						end_IL_010e:;
					}
					catch
					{
                        //try-fault
                        Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<PROPVARIANT*, void>)(&Module.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant3);
						throw;
					}
					Module.CComPropVariant_002EClear(&cComPropVariant3);
					return num;
				}
			}
		}

		private unsafe int AddTracksToPropList(IMSMediaSchemaPropertyList* pPropList)
		{
			//IL_00fc: Expected I, but got I8
			//IL_0122: Expected I4, but got I8
			//IL_0132: Expected I8, but got I
			//IL_014a: Expected I, but got I8
			//IL_0178: Expected I, but got I8
			//IL_0178: Expected I, but got I8
			//IL_018b: Expected I, but got I8
			//IL_018f: Expected I, but got I8
			//The blocks IL_0024, IL_0035, IL_004d, IL_0063, IL_0075, IL_0091, IL_0098, IL_018f, IL_019a are reachable both inside and outside the pinned region starting at IL_009f. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
			//The blocks IL_0024, IL_0035, IL_004d, IL_0063, IL_0075, IL_0091, IL_0098, IL_018f, IL_019a are reachable both inside and outside the pinned region starting at IL_00cb. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
			//The blocks IL_0024, IL_0035, IL_004d, IL_0063, IL_0075, IL_0091, IL_0098, IL_018f, IL_019a are reachable both inside and outside the pinned region starting at IL_00f7. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
			if (pPropList == null)
			{
				Module._ZuneShipAssert(1001u, 45u);
				return -2147467261;
			}
			int num = 0;
			if (m_tracks != null)
			{
				int num2 = 0;
				while (num2 < m_tracks.Count)
				{
					DataProviderObject dataProviderObject = m_tracks[num2] as DataProviderObject;
					if (dataProviderObject != null)
					{
						_GUID gUID = Module.GUID_NULL;
						object property = dataProviderObject.GetProperty("ZuneMediaId");
						if (property != null)
						{
							gUID = (Guid)property;
						}
						string text = dataProviderObject.GetProperty("Title") as string;
						object s = ((!(text == (string)null)) ? text : "");
						while (true)
						{
							fixed (char* sPtr = ((string)s).ToCharArray())
							{
								ushort* ptr = (ushort*)sPtr;
								while (true)
								{
									if (num2 < m_tracks.Count)
									{
										dataProviderObject = m_tracks[num2] as DataProviderObject;
										if (dataProviderObject != null)
										{
											break;
										}
										num2++;
										if (num >= 0)
										{
											continue;
										}
									}
									return num;
								}
								gUID = Module.GUID_NULL;
								property = dataProviderObject.GetProperty("ZuneMediaId");
								if (property != null)
								{
									gUID = (Guid)property;
								}
								text = dataProviderObject.GetProperty("Title") as string;
								s = ((!(text == (string)null)) ? text : "");
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
				//base.Finalize();
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		~PlaylistMessageData()
		{
			Dispose(false);
		}
	}
}
