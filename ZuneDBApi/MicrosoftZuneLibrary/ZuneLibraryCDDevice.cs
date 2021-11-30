using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using MicrosoftZuneInterop;
using ZuneUI;

namespace MicrosoftZuneLibrary
{
	public class ZuneLibraryCDDevice : IDisposable
	{
		private unsafe IWMPCDDevice* m_pDevice;

		private unsafe IBurnPublisher* m_pBurnPublisher;

		private int m_hrBurnPublisherCreate;

		private int m_fIsBurner;

		private bool _disposed;

		private uint m_dwBurnAdviseCookie;

		private OnSessionProgressHandler m_SessionProgressHandler;

		private OnItemProgressHandler m_ItemProgressHandler;

		private OnItemErrorHandler m_ItemErrorHandler;

		private OnBurnStateChangeHandler m_BurnStateChangeHandler;

		private OnSetDriveLockedForBurningHandler m_SetDriveLockedForBurningHandler;

		private OnQueryCancelHandler m_QueryCancelHandler;

		private unsafe IBurnPublisher* BurnPublisher
		{
			get
			{
				//IL_001a: Expected I, but got I8
				//IL_0039: Expected I, but got I8
				if (m_pBurnPublisher == null && m_pDevice != null)
				{
					fixed (int* ptr = &m_fIsBurner)
					{
						try
						{
							IBurnPublisher* pBurnPublisher = null;
							long num = *(long*)m_pDevice + 192;
							IWMPCDDevice* pDevice = m_pDevice;
							if ((m_hrBurnPublisherCreate = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, IBurnPublisher**, int>)(*(ulong*)num))((nint)pDevice, ptr, &pBurnPublisher)) >= 0)
							{
								m_pBurnPublisher = pBurnPublisher;
							}
						}
						catch
						{
							//try-fault
							//ptr = null;
							throw;
						}
					}
				}
				return m_pBurnPublisher;
			}
		}

		public unsafe static bool IsImapiv2Installed
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_001e: Expected I, but got I8
				//IL_0037: Expected I, but got I8
				bool flag = true;
				if (!Module.IsLonghornOrBetter())
				{
					IUnknown* ptr;
					flag = (byte)((Module.CoCreateInstance((_GUID*)Unsafe.AsPointer(ref Module.CLSID_MsftDiscMaster2), null, 23u, (_GUID*)Unsafe.AsPointer(ref Module._GUID_27354130_7f64_5b0f_8f00_5d77afbe261e), (void**)(&ptr)) >= 0) ? 1u : 0u) != 0;
					if (flag)
					{
						IUnknown* intPtr = ptr;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
					}
				}
				return flag;
			}
		}

		public unsafe EBurnState CurrentBurnState
		{
			get
			{
				//IL_001b: Expected I, but got I8
				EBurnState eBurnState = EBurnState.ebsUnknown;
				IBurnPublisher* pBurnPublisher = m_pBurnPublisher;
				if (pBurnPublisher != null)
				{
					__MIDL___MIDL_itf_wmpcd_0000_0014_0001 _MIDL___MIDL_itf_wmpcd_0000_0014_;
					eBurnState = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, __MIDL___MIDL_itf_wmpcd_0000_0014_0001*, int>)(*(ulong*)(*(long*)pBurnPublisher + 120)))((nint)pBurnPublisher, &_MIDL___MIDL_itf_wmpcd_0000_0014_) >= 0) ? ((EBurnState)_MIDL___MIDL_itf_wmpcd_0000_0014_) : eBurnState);
				}
				return eBurnState;
			}
		}

		public unsafe bool IsBurner
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_0026: Expected I, but got I8
				bool flag = false;
				IWMPCDDevice* pDevice = m_pDevice;
				if (pDevice != null)
				{
					if (m_pBurnPublisher == null)
					{
						int num;
						if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)pDevice + 200)))((nint)pDevice, &num) >= 0 && num != 0)
						{
							flag = true;
						}
					}
					else
					{
						flag = m_fIsBurner != 0 || flag;
					}
				}
				return flag;
			}
		}

		public unsafe long SpaceAvailable
		{
			get
			{
				//IL_001c: Expected I, but got I8
				long num = 0L;
				IBurnPublisher* burnPublisher = BurnPublisher;
				if (burnPublisher != null)
				{
					ulong num2;
					num = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong*, int>)(*(ulong*)(*(long*)burnPublisher + 24)))((nint)burnPublisher, &num2) >= 0) ? ((long)num2) : num);
				}
				return num;
			}
		}

		public unsafe uint TimeAvailable
		{
			get
			{
				//IL_001b: Expected I, but got I8
				uint result = 0u;
				IBurnPublisher* burnPublisher = BurnPublisher;
				if (burnPublisher != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)burnPublisher + 40)))((nint)burnPublisher, &result);
				}
				return result;
			}
		}

		public unsafe bool IsDoorOpen
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_001b: Expected I, but got I8
				bool result = false;
				IWMPCDDevice* pDevice = m_pDevice;
				int num;
				if (pDevice != null && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)pDevice + 88)))((nint)pDevice, &num) >= 0)
				{
					bool flag = ((num != 0) ? true : false);
					result = flag;
				}
				return result;
			}
		}

		public unsafe bool IsDVD
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_001b: Expected I, but got I8
				bool result = false;
				IBurnPublisher* burnPublisher = BurnPublisher;
				uint num;
				if (burnPublisher != null && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)burnPublisher + 72)))((nint)burnPublisher, &num) >= 0)
				{
					bool flag = (byte)((num >> 5) & (true ? 1u : 0u)) != 0;
					result = flag;
				}
				return result;
			}
		}

		public unsafe bool IsCDRW
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_001b: Expected I, but got I8
				bool result = false;
				IBurnPublisher* burnPublisher = BurnPublisher;
				uint num;
				if (burnPublisher != null && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)burnPublisher + 72)))((nint)burnPublisher, &num) >= 0)
				{
					bool flag = (byte)((num >> 4) & (true ? 1u : 0u)) != 0;
					result = flag;
				}
				return result;
			}
		}

		public unsafe bool IsWriteable
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_001b: Expected I, but got I8
				bool result = false;
				IBurnPublisher* burnPublisher = BurnPublisher;
				uint num;
				if (burnPublisher != null && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)burnPublisher + 72)))((nint)burnPublisher, &num) >= 0)
				{
					bool flag = (byte)((num >> 3) & (true ? 1u : 0u)) != 0;
					result = flag;
				}
				return result;
			}
		}

		public unsafe bool IsBlank
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_001b: Expected I, but got I8
				bool result = false;
				IBurnPublisher* burnPublisher = BurnPublisher;
				uint num;
				if (burnPublisher != null && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)burnPublisher + 72)))((nint)burnPublisher, &num) >= 0)
				{
					bool flag = (byte)((num >> 2) & (true ? 1u : 0u)) != 0;
					result = flag;
				}
				return result;
			}
		}

		public unsafe bool IsDriveReady
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_001e: Expected I, but got I8
				bool result = false;
				IWMPCDDevice* pDevice = m_pDevice;
				int num;
				if (pDevice != null && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)pDevice + 184)))((nint)pDevice, &num) >= 0)
				{
					bool flag = ((num != 0) ? true : false);
					result = flag;
				}
				return result;
			}
		}

		public unsafe bool IsMediaLoaded
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_001b: Expected I, but got I8
				bool result = false;
				IWMPCDDevice* pDevice = m_pDevice;
				int num;
				if (pDevice != null && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, int>)(*(ulong*)(*(long*)pDevice + 80)))((nint)pDevice, &num) >= 0)
				{
					bool flag = ((num != 0) ? true : false);
					result = flag;
				}
				return result;
			}
		}

		public unsafe char DrivePath
		{
			[return: MarshalAs(UnmanagedType.U2)]
			get
			{
				//IL_0017: Expected I, but got I8
				IWMPCDDevice* pDevice = m_pDevice;
				if (pDevice != null)
				{
					return (char)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort>)(*(ulong*)(*(long*)pDevice + 32)))((nint)pDevice);
				}
				return '\0';
			}
		}

		public unsafe string TOC
		{
			get
			{
				//IL_0028: Expected I, but got I8
				//IL_002e: Expected I, but got I8
				//IL_003d: Expected I, but got I8
				//IL_005a: Expected I, but got I8
				string result = null;
				if (m_pDevice != null && IsMediaLoaded)
				{
					IWMPCDDevice* pDevice = m_pDevice;
					IWMPCDMediaInfo* ptr;
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IWMPCDMediaInfo**, int>)(*(ulong*)(*(long*)pDevice + 112)))((nint)pDevice, &ptr) >= 0)
					{
						ushort* ptr2 = null;
						IWMPCDMediaInfo* intPtr = ptr;
						if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)ptr + 32)))((nint)intPtr, &ptr2) >= 0)
						{
							result = new string((char*)ptr2);
							Module.SysFreeString(ptr2);
						}
						IWMPCDMediaInfo* intPtr2 = ptr;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
					}
				}
				return result;
			}
		}

		[SpecialName]
		public unsafe virtual event OnQueryCancelHandler QueryCancelHandler
		{
			add
			{
				m_QueryCancelHandler = (OnQueryCancelHandler)Delegate.Combine(m_QueryCancelHandler, value);
			}
			remove
			{
				m_QueryCancelHandler = (OnQueryCancelHandler)Delegate.Remove(m_QueryCancelHandler, value);
			}
		}

		[SpecialName]
		public virtual event OnSetDriveLockedForBurningHandler SetDriveLockedForBurningHandler
		{
			add
			{
				m_SetDriveLockedForBurningHandler = (OnSetDriveLockedForBurningHandler)Delegate.Combine(m_SetDriveLockedForBurningHandler, value);
			}
			remove
			{
				m_SetDriveLockedForBurningHandler = (OnSetDriveLockedForBurningHandler)Delegate.Remove(m_SetDriveLockedForBurningHandler, value);
			}
		}

		[SpecialName]
		public virtual event OnBurnStateChangeHandler BurnStateChangeHandler
		{
			add
			{
				m_BurnStateChangeHandler = (OnBurnStateChangeHandler)Delegate.Combine(m_BurnStateChangeHandler, value);
			}
			remove
			{
				m_BurnStateChangeHandler = (OnBurnStateChangeHandler)Delegate.Remove(m_BurnStateChangeHandler, value);
			}
		}

		[SpecialName]
		public virtual event OnItemErrorHandler ItemErrorHandler
		{
			add
			{
				m_ItemErrorHandler = (OnItemErrorHandler)Delegate.Combine(m_ItemErrorHandler, value);
			}
			remove
			{
				m_ItemErrorHandler = (OnItemErrorHandler)Delegate.Remove(m_ItemErrorHandler, value);
			}
		}

		[SpecialName]
		public virtual event OnItemProgressHandler ItemProgressHandler
		{
			add
			{
				m_ItemProgressHandler = (OnItemProgressHandler)Delegate.Combine(m_ItemProgressHandler, value);
			}
			remove
			{
				m_ItemProgressHandler = (OnItemProgressHandler)Delegate.Remove(m_ItemProgressHandler, value);
			}
		}

		[SpecialName]
		public virtual event OnSessionProgressHandler SessionProgressHandler
		{
			add
			{
				m_SessionProgressHandler = (OnSessionProgressHandler)Delegate.Combine(m_SessionProgressHandler, value);
			}
			remove
			{
				m_SessionProgressHandler = (OnSessionProgressHandler)Delegate.Remove(m_SessionProgressHandler, value);
			}
		}

		public unsafe ZuneLibraryCDDevice(IWMPCDDevice* pDevice)
		{
			//IL_000f: Expected I, but got I8
			m_pDevice = pDevice;
			m_pBurnPublisher = null;
			m_fIsBurner = 0;
		}

		private void _007EZuneLibraryCDDevice()
		{
			_0021ZuneLibraryCDDevice();
		}

		private unsafe void _0021ZuneLibraryCDDevice()
		{
			//IL_001f: Expected I, but got I8
			//IL_0028: Expected I, but got I8
			//IL_003f: Expected I, but got I8
			//IL_0048: Expected I, but got I8
			if (!_disposed)
			{
				IWMPCDDevice* pDevice = m_pDevice;
				if (pDevice != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pDevice + 16)))((nint)pDevice);
					m_pDevice = null;
				}
				IBurnPublisher* pBurnPublisher = m_pBurnPublisher;
				if (pBurnPublisher != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pBurnPublisher + 16)))((nint)pBurnPublisher);
					m_pBurnPublisher = null;
				}
				_disposed = true;
			}
		}

		public unsafe int GetTrackUrl(uint dwTrackNum, StringBuilder strBuilder)
		{
			//IL_0033: Expected I, but got I8
			//IL_003b: Expected I, but got I8
			//IL_004b: Expected I, but got I8
			//IL_0070: Expected I, but got I8
			int num = -2147418113;
			strBuilder.Length = 0;
			if (m_pDevice != null && IsMediaLoaded)
			{
				IWMPCDDevice* pDevice = m_pDevice;
				IWMPCDMediaInfo* ptr;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IWMPCDMediaInfo**, int>)(*(ulong*)(*(long*)pDevice + 112)))((nint)pDevice, &ptr);
				if (num >= 0)
				{
					ushort* ptr2 = null;
					IWMPCDMediaInfo* intPtr = ptr;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, ushort**, int>)(*(ulong*)(*(long*)ptr + 56)))((nint)intPtr, dwTrackNum, &ptr2);
					if (num >= 0)
					{
						strBuilder.Append(new string((char*)ptr2));
						Module.SysFreeString(ptr2);
					}
					IWMPCDMediaInfo* intPtr2 = ptr;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
				}
			}
			return num;
		}

		public unsafe HRESULT SetBurnPlaylist(int iPlaylistId)
		{
			//IL_0037: Expected I, but got I8
			//IL_0048: Expected I, but got I8
			//IL_0059: Expected I, but got I8
			//IL_006e: Expected I, but got I8
			//IL_0072: Expected I, but got I8
			//IL_0075: Expected I, but got I8
			//IL_0081: Expected I, but got I8
			//IL_00a7: Expected I, but got I8
			//IL_00cb: Expected I4, but got I8
			//IL_00d3: Expected I4, but got I8
			//IL_00e9: Expected I, but got I8
			//IL_0106: Expected I, but got I8
			//IL_0172: Expected I, but got I8
			//IL_0185: Expected I, but got I8
			//IL_0189: Expected I, but got I8
			//IL_019b: Expected I, but got I8
			//IL_019f: Expected I, but got I8
			IBurnPublisher* burnPublisher = BurnPublisher;
			if (burnPublisher == null)
			{
				return m_hrBurnPublisherCreate;
			}
			QueryPropertyBag queryPropertyBag = new QueryPropertyBag();
			IQueryPropertyBag* iQueryPropertyBag = queryPropertyBag.GetIQueryPropertyBag();
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)15, 0);
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)10, iPlaylistId);
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)22, 1);
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)iQueryPropertyBag + 56)))((nint)iQueryPropertyBag, (EQueryPropertyBagProp)21, 437);
			IPlaylist* ptr = null;
			IDatabaseQueryResults* ptr2 = null;
			int num = Module.QueryDatabase(EQueryType.eQueryTypePlaylistContentByPlaylistId, iQueryPropertyBag, &ptr2, null);
			if (num >= 0)
			{
				num = Module.CreateEmptyPlaylist(&ptr);
			}
			uint num2 = 0u;
			if (num >= 0)
			{
				IDatabaseQueryResults* intPtr = ptr2;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint*, int>)(*(ulong*)(*(long*)ptr2 + 88)))((nint)intPtr, &num2);
				if (num >= 0)
				{
					if (num2 != 0)
					{
						uint num3 = 0u;
						while (num3 < num2)
						{
                            PROPVARIANT cComPropVariant = new();
							try
							{
                                PROPVARIANT cComPropVariant2 = new();
								try
								{
									IDatabaseQueryResults* intPtr2 = ptr2;
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, uint, PROPVARIANT, int>)(*(ulong*)(*(long*)ptr2 + 48)))((nint)intPtr2, num3, 233u, cComPropVariant);
									if (num >= 0)
									{
										IDatabaseQueryResults* intPtr3 = ptr2;
										num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, uint, PROPVARIANT, int>)(*(ulong*)(*(long*)ptr2 + 48)))((nint)intPtr3, num3, 234u, cComPropVariant2);
										if (num >= 0)
										{
											num = Module.AddItemToPlaylist(Unsafe.As<PROPVARIANT, int>(ref Unsafe.AddByteOffset(ref cComPropVariant, 8)), Unsafe.As<PROPVARIANT, int>(ref Unsafe.AddByteOffset(ref cComPropVariant2, 8)), ptr);
										}
									}
								}
								catch
								{
                                    //try-fault
                                    Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<PROPVARIANT*, void>)(&Module.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant2);
									throw;
								}
								cComPropVariant2.Clear();
							}
							catch
							{
                                //try-fault
                                Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<PROPVARIANT*, void>)(&Module.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
								throw;
							}
							cComPropVariant.Clear();
							num3++;
							if (num < 0)
							{
								break;
							}
						}
					}
					if (num >= 0)
					{
						if (ptr == null)
						{
							goto IL_0189;
						}
						IPlaylist* intPtr4 = ptr;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPlaylist*, int>)(*(ulong*)(*(long*)burnPublisher + 88)))((nint)burnPublisher, intPtr4);
					}
				}
			}
			if (0L != (nint)ptr)
			{
				IPlaylist* intPtr5 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr5 + 16)))((nint)intPtr5);
				ptr = null;
			}
			goto IL_0189;
			IL_0189:
			if (0L != (nint)ptr2)
			{
				IDatabaseQueryResults* intPtr6 = ptr2;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr6 + 16)))((nint)intPtr6);
				ptr2 = null;
			}
			((IDisposable)queryPropertyBag)?.Dispose();
			return num;
		}

		public unsafe HRESULT StartBurn()
		{
			//IL_0032: Expected I, but got I8
			//IL_0047: Expected I, but got I8
			IBurnPublisher* burnPublisher = BurnPublisher;
			if (burnPublisher == null)
			{
				return m_hrBurnPublisherCreate;
			}
			int num = AdviseForBurnPublisherEvents();
			if (num >= 0)
			{
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)burnPublisher + 144)))((nint)burnPublisher, 1);
				if (num >= 0)
				{
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)burnPublisher + 208)))((nint)burnPublisher);
					if (num >= 0)
					{
						goto IL_0052;
					}
				}
			}
			UnadviseForBurnPublisherEvents();
			goto IL_0052;
			IL_0052:
			return num;
		}

		public unsafe int StopBurn()
		{
			//IL_0021: Expected I, but got I8
			IBurnPublisher* burnPublisher = BurnPublisher;
			if (burnPublisher == null)
			{
				return m_hrBurnPublisherCreate;
			}
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)burnPublisher + 216)))((nint)burnPublisher);
		}

		public unsafe HRESULT EraseDisc()
		{
			//IL_0031: Expected I, but got I8
			IBurnPublisher* burnPublisher = BurnPublisher;
			if (burnPublisher == null)
			{
				return m_hrBurnPublisherCreate;
			}
			int num = AdviseForBurnPublisherEvents();
			if (num >= 0)
			{
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)burnPublisher + 136)))((nint)burnPublisher);
				if (num >= 0)
				{
					goto IL_003c;
				}
			}
			UnadviseForBurnPublisherEvents();
			goto IL_003c;
			IL_003c:
			return new HRESULT(num);
		}

		public unsafe int SetActive([MarshalAs(UnmanagedType.U1)] bool fActive)
		{
			//IL_002b: Expected I, but got I8
			IBurnPublisher* burnPublisher = BurnPublisher;
			if (burnPublisher == null)
			{
				return m_hrBurnPublisherCreate;
			}
			int num = (fActive ? 1 : 0);
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)burnPublisher + 144)))((nint)burnPublisher, num);
		}

		public unsafe int Eject()
		{
			//IL_0020: Expected I, but got I8
			int result = -2147418113;
			IWMPCDDevice* pDevice = m_pDevice;
			if (pDevice != null)
			{
				result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pDevice + 168)))((nint)pDevice);
			}
			return result;
		}

		public unsafe HRESULT SetVolumeLabelW(string strVolumeLabel)
		{
			//IL_0053: Expected I, but got I8
			IBurnPublisher* burnPublisher = BurnPublisher;
			if (burnPublisher == null)
			{
				return m_hrBurnPublisherCreate;
			}
			if (string.IsNullOrEmpty(strVolumeLabel))
			{
				return -2147024809;
			}
			fixed (char* strVolumeLabelPtr = strVolumeLabel.ToCharArray())
			{
				ushort* ptr = (ushort*)strVolumeLabelPtr;
				ushort* ptr2 = Module.SysAllocString(ptr);
				if (ptr2 == null)
				{
					return -2147024882;
				}
				HRESULT result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)(*(long*)burnPublisher + 64)))((nint)burnPublisher, ptr2);
				Module.SysFreeString(ptr2);
				return result;
			}
		}

		public unsafe int Close()
		{
			//IL_0020: Expected I, but got I8
			int result = -2147418113;
			IWMPCDDevice* pDevice = m_pDevice;
			if (pDevice != null)
			{
				result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pDevice + 176)))((nint)pDevice);
			}
			return result;
		}

		internal void ItemProgress(int lMediaIndex, EBurnProgressStatus status, int nPercent)
		{
			m_ItemProgressHandler?.Invoke(lMediaIndex, status, nPercent);
		}

		internal void ItemError(int lMediaIndex, int hrError)
		{
			m_ItemErrorHandler?.Invoke(lMediaIndex, hrError);
		}

		internal void SessionProgress(int lSessonSecondsRemaining, int lTotalSessionSeconds)
		{
			m_SessionProgressHandler?.Invoke(lSessonSecondsRemaining, lTotalSessionSeconds);
		}

		internal void BurnStateChange(EBurnState burnState)
		{
			m_BurnStateChangeHandler?.Invoke(burnState);
			if (burnState == EBurnState.ebsStopped)
			{
				UnadviseForBurnPublisherEvents();
				SetActive(fActive: false);
			}
		}

		internal void SetDriveLockedForBurning([MarshalAs(UnmanagedType.U1)] bool fLocked)
		{
			m_SetDriveLockedForBurningHandler?.Invoke(fLocked);
		}

		internal unsafe void QueryCancel(bool* pfCancel)
		{
			m_QueryCancelHandler?.Invoke(pfCancel);
		}

		private unsafe int AdviseForBurnPublisherEvents()
		{
			//IL_0019: Expected I, but got I8
			//IL_0046: Expected I, but got I8
			CBurnPublisherCallback* ptr = (CBurnPublisherCallback*)Module.@new(24uL);
			CBurnPublisherCallback* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : Module.MicrosoftZuneLibrary_002ECBurnPublisherCallback_002E_007Bctor_007D(ptr, this));
			}
			catch
			{
				//try-fault
				Module.delete(ptr);
				throw;
			}
			int result;
			if (ptr2 == null)
			{
				result = -2147024882;
			}
			else
			{
				IBurnPublisher* burnPublisher = BurnPublisher;
				uint dwBurnAdviseCookie;
				result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IBurnPublisherCallback*, uint*, int>)(*(ulong*)(*(long*)burnPublisher + 192)))((nint)burnPublisher, (IBurnPublisherCallback*)ptr2, &dwBurnAdviseCookie);
				m_dwBurnAdviseCookie = dwBurnAdviseCookie;
			}
			return result;
		}

		private unsafe void UnadviseForBurnPublisherEvents()
		{
			//IL_0025: Expected I, but got I8
			if (m_dwBurnAdviseCookie != 0)
			{
				IBurnPublisher* burnPublisher = BurnPublisher;
				uint dwBurnAdviseCookie = m_dwBurnAdviseCookie;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, int>)(*(ulong*)(*(long*)burnPublisher + 200)))((nint)burnPublisher, dwBurnAdviseCookie);
				m_dwBurnAdviseCookie = 0u;
			}
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_0021ZuneLibraryCDDevice();
				return;
			}
			try
			{
				_0021ZuneLibraryCDDevice();
			}
			finally
			{
				////base.Finalize();
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		~ZuneLibraryCDDevice()
		{
			Dispose(false);
		}
	}
}
