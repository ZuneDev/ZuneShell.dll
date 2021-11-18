using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Zune.Util;

namespace MicrosoftZunePlayback
{
	public class PlayerInterop : IDisposable
	{
		private unsafe IMCPlayer* _uPlayer = null;

		private unsafe IMCTransport* _uTransport = null;

		private unsafe IMCPlayerSetUri* _uSetUri = null;

		private unsafe IMCDynamicImage* _uDynamicImage = null;

		private unsafe IMCVolumeControl* _uVolumeControl;

		private unsafe IZuneSpectrumMgr* _uZuneSpectrumMgr = null;

		private unsafe CPlayerInteropEventSink* _uEventSink;

		private MCPlayerState _state = MCPlayerState.Uninitialized;

		private long _duration = 0L;

		private bool _isReady = false;

		private Announcement _firstDenial;

		private string _currentUri;

		private int _currentUriID;

		private Dictionary<string, object> _properties;

		private MCTransportState _transportState = MCTransportState.Invalid;

		private float _rate = 1f;

		private bool _endOfMedia = false;

		private long _position = 0L;

		private long _minSeekPosition = 0L;

		private long _maxSeekPosition = 0L;

		private bool _resetOnStop = false;

		private TimeSpan _positionEventInterval = TimeSpan.FromMilliseconds(100.0);

		private int _nDynamicImage = 0;

		private int _volume;

		private bool _mute;

		private bool _canChangeVideoRate = false;

		private bool _canSeek = true;

		private IntPtr _windowHandle;

		private VideoWindow _windowHost;

		private static PlayerInterop _singletonInstance = null;

		private unsafe void* _gotStateCloseEvent;

		private unsafe void* _gotStateUninitializeEvent;

		private volatile bool _fShuttingDown;

		private PlayerPropertyChangedEventHandler _003Cbacking_store_003EPlayerPropertyChanged;

		private PlayerBandwithUpdateEventHandler _003Cbacking_store_003EPlayerBandwithUpdate;

		private EventHandler _003Cbacking_store_003EStatusChanged;

		private AnnouncementHandler _003Cbacking_store_003EAlertSent;

		private EventHandler _003Cbacking_store_003ETransportStatusChanged;

		private EventHandler _003Cbacking_store_003ETransportPositionChanged;

		private EventHandler _003Cbacking_store_003EUriSet;

		public int CurrentUriID => _currentUriID;

		public string CurrentUri => _currentUri;

		public unsafe TimeSpan PositionEventInterval
		{
			get
			{
				return _positionEventInterval;
			}
			set
			{
				//IL_0030: Expected I, but got I8
				if (_uTransport != null)
				{
					_positionEventInterval = value;
					long num = *(long*)_uTransport + 96;
					IMCTransport* uTransport = _uTransport;
					double totalMilliseconds = value.TotalMilliseconds;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, int>)(*(ulong*)num))((nint)uTransport, (uint)totalMilliseconds);
				}
			}
		}

		public unsafe bool ResetOnStop
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return _resetOnStop;
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				//IL_001f: Expected I, but got I8
				IMCTransport* uTransport = _uTransport;
				if (uTransport != null)
				{
					_resetOnStop = value;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte, int>)(*(ulong*)(*(long*)uTransport + 88)))((nint)uTransport, value ? ((byte)1) : ((byte)0));
				}
			}
		}

		public long MaxSeekPosition => _maxSeekPosition;

		public long MinSeekPosition => _minSeekPosition;

		public long Position => _position;

		public bool EndOfMedia
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return _endOfMedia;
			}
		}

		public unsafe float Rate
		{
			get
			{
				return _rate;
			}
			set
			{
				//IL_0018: Expected I, but got I8
				IMCTransport* uTransport = _uTransport;
				if (uTransport != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, float, int>)(*(ulong*)(*(long*)uTransport + 80)))((nint)uTransport, value);
				}
			}
		}

		public MCTransportState TransportState => _transportState;

		public unsafe bool ShowGDIVideo
		{
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				//IL_0018: Expected I, but got I8
				IMCDynamicImage* uDynamicImage = _uDynamicImage;
				if (uDynamicImage != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte, int>)(*(ulong*)(*(long*)uDynamicImage + 40)))((nint)uDynamicImage, value ? ((byte)1) : ((byte)0));
				}
			}
		}

		public unsafe VideoWindow VideoPosition
		{
			set
			{
				//IL_004a: Expected I, but got I8
				_windowHost = value;
				RECT tagRECT;
				*(int*)(&tagRECT) = value.Left;
                Unsafe.As<RECT, int>(ref Unsafe.AddByteOffset(ref tagRECT, 4)) = value.Top;
                Unsafe.As<RECT, int>(ref Unsafe.AddByteOffset(ref tagRECT, 8)) = value.Right;
                Unsafe.As<RECT, int>(ref Unsafe.AddByteOffset(ref tagRECT, 12)) = value.Bottom;
				IMCDynamicImage* uDynamicImage = _uDynamicImage;
				if (uDynamicImage != null)
				{
					RECT tagRECT2 = tagRECT;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, RECT, int>)(*(ulong*)(*(long*)uDynamicImage + 32)))((nint)uDynamicImage, tagRECT2);
				}
			}
		}

		public IntPtr WindowHandle
		{
			get
			{
				return _windowHandle;
			}
			set
			{
				_windowHandle = value;
			}
		}

		public bool CanSeek
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return _canSeek;
			}
		}

		public bool CanChangeVideoRate
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return _canChangeVideoRate;
			}
		}

		public unsafe bool Mute
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return _mute;
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				//IL_001f: Expected I, but got I8
				_mute = value;
				IMCVolumeControl* uVolumeControl = _uVolumeControl;
				if (uVolumeControl != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte, int>)(*(ulong*)(*(long*)uVolumeControl + 32)))((nint)uVolumeControl, value ? ((byte)1) : ((byte)0));
				}
			}
		}

		public unsafe int Volume
		{
			get
			{
				return _volume;
			}
			set
			{
				//IL_0027: Expected I, but got I8
				_volume = value;
				if (!_mute)
				{
					IMCVolumeControl* uVolumeControl = _uVolumeControl;
					if (uVolumeControl != null)
					{
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)uVolumeControl + 24)))((nint)uVolumeControl, value);
					}
				}
			}
		}

		public int DynamicImage
		{
			get
			{
				return _nDynamicImage;
			}
			set
			{
				_nDynamicImage = value;
			}
		}

		public Announcement FirstDenial => _firstDenial;

		public bool IsReady
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return _isReady;
			}
		}

		public long Duration => _duration;

		public MCPlayerState State => _state;

		public static PlayerInterop Instance
		{
			get
			{
				if (_singletonInstance == null)
				{
					_singletonInstance = new PlayerInterop();
				}
				return _singletonInstance;
			}
		}

		[SpecialName]
		public event EventHandler UriSet
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				_003Cbacking_store_003EUriSet = (EventHandler)Delegate.Combine(_003Cbacking_store_003EUriSet, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				_003Cbacking_store_003EUriSet = (EventHandler)Delegate.Remove(_003Cbacking_store_003EUriSet, value);
			}
		}

		[SpecialName]
		public event EventHandler TransportPositionChanged
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				_003Cbacking_store_003ETransportPositionChanged = (EventHandler)Delegate.Combine(_003Cbacking_store_003ETransportPositionChanged, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				_003Cbacking_store_003ETransportPositionChanged = (EventHandler)Delegate.Remove(_003Cbacking_store_003ETransportPositionChanged, value);
			}
		}

		[SpecialName]
		public event EventHandler TransportStatusChanged
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				_003Cbacking_store_003ETransportStatusChanged = (EventHandler)Delegate.Combine(_003Cbacking_store_003ETransportStatusChanged, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				_003Cbacking_store_003ETransportStatusChanged = (EventHandler)Delegate.Remove(_003Cbacking_store_003ETransportStatusChanged, value);
			}
		}

		[SpecialName]
		public event AnnouncementHandler AlertSent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				_003Cbacking_store_003EAlertSent = (AnnouncementHandler)Delegate.Combine(_003Cbacking_store_003EAlertSent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				_003Cbacking_store_003EAlertSent = (AnnouncementHandler)Delegate.Remove(_003Cbacking_store_003EAlertSent, value);
			}
		}

		[SpecialName]
		public event EventHandler StatusChanged
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				_003Cbacking_store_003EStatusChanged = (EventHandler)Delegate.Combine(_003Cbacking_store_003EStatusChanged, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				_003Cbacking_store_003EStatusChanged = (EventHandler)Delegate.Remove(_003Cbacking_store_003EStatusChanged, value);
			}
		}

		[SpecialName]
		public virtual event PlayerBandwithUpdateEventHandler PlayerBandwithUpdate
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				_003Cbacking_store_003EPlayerBandwithUpdate = (PlayerBandwithUpdateEventHandler)Delegate.Combine(_003Cbacking_store_003EPlayerBandwithUpdate, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				_003Cbacking_store_003EPlayerBandwithUpdate = (PlayerBandwithUpdateEventHandler)Delegate.Remove(_003Cbacking_store_003EPlayerBandwithUpdate, value);
			}
		}

		[SpecialName]
		public virtual event PlayerPropertyChangedEventHandler PlayerPropertyChanged
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				_003Cbacking_store_003EPlayerPropertyChanged = (PlayerPropertyChangedEventHandler)Delegate.Combine(_003Cbacking_store_003EPlayerPropertyChanged, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				_003Cbacking_store_003EPlayerPropertyChanged = (PlayerPropertyChangedEventHandler)Delegate.Remove(_003Cbacking_store_003EPlayerPropertyChanged, value);
			}
		}

		[return: MarshalAs(UnmanagedType.U1)]
		private unsafe static bool HResultDenialsEqual(MCHResultAnnouncement* a, Announcement b)
		{
			//The blocks IL_0000, IL_0069, IL_006e, IL_0071, IL_0095, IL_009f, IL_00b1, IL_00b5, IL_00b8, IL_00c9, IL_00d7, IL_00dc, IL_00df are reachable both inside and outside the pinned region starting at IL_004f. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
			//The blocks IL_00b1, IL_00b5, IL_00b8, IL_00d7, IL_00dc, IL_00df are reachable both inside and outside the pinned region starting at IL_0097. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
			//The blocks IL_00dc, IL_00df are reachable both inside and outside the pinned region starting at IL_00c2. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
			//The blocks IL_00dc, IL_00df are reachable both inside and outside the pinned region starting at IL_007d. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
			//The blocks IL_00b1, IL_00b5, IL_00b8, IL_00d7, IL_00dc, IL_00df are reachable both inside and outside the pinned region starting at IL_0097. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
			//The blocks IL_00dc, IL_00df are reachable both inside and outside the pinned region starting at IL_00c2. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
			if (a == null)
			{
				if (b == null)
				{
					return true;
				}
			}
			else if (b != null)
			{
				bool result;
				fixed (char* b.IdPtr = b.Id.ToCharArray())
				{
					ushort* ptr = (ushort*)b.IdPtr;
					try
					{
						fixed (char* b.SourceFilePtr = b.SourceFile.ToCharArray())
						{
							ushort* ptr3 = (ushort*)b.SourceFilePtr;
							try
							{
								int num7;
								if (*(int*)((ulong)(nint)a + 12uL) == b.HResult && *(int*)((ulong)(nint)a + 32uL) == (int)b.SourceLine)
								{
									short num2;
									short num3;
									ref ushort reference;
									long num;
									ref ushort reference2;
									fixed (ushort* ptr2 = &Unsafe.AsRef<ushort>(ptr))
									{
										num = *(long*)a;
										num2 = Unsafe.ReadUnaligned<short>((void*)num);
										num3 = Unsafe.ReadUnaligned<short>(ptr2);
										if (num2 >= num3 && num2 <= num3)
										{
											if (num2 == 0)
											{
												long num4;
												short num5;
												short num6;
												fixed (ushort* ptr4 = &Unsafe.AsRef<ushort>(ptr3))
												{
													num4 = *(long*)((ulong)(nint)a + 24uL);
													num5 = Unsafe.ReadUnaligned<short>((void*)num4);
													num6 = Unsafe.ReadUnaligned<short>(ptr4);
													if (num5 < num6 || num5 > num6)
													{
														num7 = 0;
														goto IL_00df;
													}
													if (num5 == 0)
													{
														num7 = 1;
														goto IL_00df;
													}
													num4 += 2;
													reference = ref *ptr4;
													goto end_IL_009f;
													IL_00df:
													result = (byte)num7 != 0;
													goto end_IL_001e;
													end_IL_009f:;
												}
												while (true)
												{
													fixed (ushort* ptr4 = &Unsafe.Add(ref reference, 1))
													{
														IL_00b1:
														if (num5 > num6)
														{
															goto IL_00dc;
														}
														if (num5 != 0)
														{
															num4 += 2;
															reference = ref *ptr4;
															continue;
														}
														num7 = 1;
														goto IL_00df_2;
														IL_00df_2:
														result = (byte)num7 != 0;
														goto end_IL_001e;
														IL_00dc:
														num7 = 0;
														goto IL_00df_2;
														IL_00c9:
														num5 = Unsafe.ReadUnaligned<short>((void*)num4);
														num6 = Unsafe.ReadUnaligned<short>(ptr4);
														if (num5 >= num6)
														{
															goto IL_00b1;
														}
														goto IL_00dc;
													}
												}
											}
											num += 2;
											reference2 = ref *ptr2;
											goto end_IL_0053;
										}
										num7 = 0;
										result = (byte)num7 != 0;
										goto end_IL_001e;
										end_IL_0053:;
									}
									while (true)
									{
										fixed (ushort* ptr2 = &Unsafe.Add(ref reference2, 1))
										{
											if (num2 <= num3)
											{
												if (num2 != 0)
												{
													num += 2;
													reference2 = ref *ptr2;
													continue;
												}
												long num4;
												short num5;
												short num6;
												fixed (ushort* ptr4 = &Unsafe.AsRef<ushort>(ptr3))
												{
													num4 = *(long*)((ulong)(nint)a + 24uL);
													num5 = Unsafe.ReadUnaligned<short>((void*)num4);
													num6 = Unsafe.ReadUnaligned<short>(ptr4);
													if (num5 < num6 || num5 > num6)
													{
														num7 = 0;
														goto IL_00df_3;
													}
													if (num5 == 0)
													{
														num7 = 1;
														goto IL_00df_3;
													}
													num4 += 2;
													reference = ref *ptr4;
													goto end_IL_009f_2;
													IL_00df_3:
													result = (byte)num7 != 0;
													goto end_IL_001e;
													end_IL_009f_2:;
												}
												while (true)
												{
													fixed (ushort* ptr4 = &Unsafe.Add(ref reference, 1))
													{
														IL_00b1_2:
														if (num5 > num6)
														{
															goto IL_00dc_2;
														}
														if (num5 != 0)
														{
															num4 += 2;
															reference = ref *ptr4;
															continue;
														}
														num7 = 1;
														goto IL_00df_4;
														IL_00df_4:
														result = (byte)num7 != 0;
														goto end_IL_001e;
														IL_00dc_2:
														num7 = 0;
														goto IL_00df_4;
														IL_00c9_2:
														num5 = Unsafe.ReadUnaligned<short>((void*)num4);
														num6 = Unsafe.ReadUnaligned<short>(ptr4);
														if (num5 >= num6)
														{
															goto IL_00b1_2;
														}
														goto IL_00dc_2;
													}
												}
											}
											num7 = 0;
											result = (byte)num7 != 0;
											goto end_IL_001e;
										}
									}
								}
								num7 = 0;
								result = (byte)num7 != 0;
							}
							catch
							{
								//try-fault
								ptr3 = null;
								throw;
							}
						}
						end_IL_001e:;
					}
					catch
					{
						//try-fault
						ptr = null;
						throw;
					}
				}
				return result;
			}
			return false;
		}

		private unsafe static Announcement MarshalAlert(MCHResultAnnouncement* hrAlert)
		{
			//IL_000f: Expected I, but got I8
			//IL_0034: Expected I, but got I8
			Announcement announcement = new Announcement();
			IntPtr ptr = new IntPtr((void*)(*(ulong*)hrAlert));
			announcement.Id = Marshal.PtrToStringUni(ptr);
			announcement.HResult = *(int*)((ulong)(nint)hrAlert + 12uL);
			IntPtr ptr2 = new IntPtr((void*)(*(ulong*)((ulong)(nint)hrAlert + 24uL)));
			announcement.SourceFile = Marshal.PtrToStringUni(ptr2);
			announcement.SourceLine = *(uint*)((ulong)(nint)hrAlert + 32uL);
			announcement.PlaybackID = *(int*)((ulong)(nint)hrAlert + 16uL);
			return announcement;
		}

		private unsafe PlayerInterop()
		{
			//IL_0008: Expected I, but got I8
			//IL_0010: Expected I, but got I8
			//IL_0018: Expected I, but got I8
			//IL_0020: Expected I, but got I8
			//IL_0028: Expected I, but got I8
			//IL_00ae: Expected I, but got I8
			//IL_00bd: Expected I, but got I8
			//IL_00c5: Expected I, but got I8
			ref IntPtr windowHandle = ref _windowHandle;
			windowHandle = new IntPtr(null);
			_windowHost = null;
			_gotStateCloseEvent = null;
			_gotStateUninitializeEvent = null;
			_fShuttingDown = false;
			base._002Ector();
		}

		internal unsafe void OnAlertOccurred(MCHResultAnnouncement* pAlert)
		{
			raise_AlertSent(MarshalAlert(pAlert));
		}

		internal unsafe void OnStatusChanged(MCPlayerStatus* pStatus)
		{
			//IL_008d: Expected I, but got I8
			//IL_009a: Expected I, but got I8
			bool flag = false;
			long num = *(long*)((ulong)(nint)pStatus + 8uL);
			if (_duration != num)
			{
				_duration = num;
				flag = true;
			}
			int num2 = *(int*)pStatus;
			if (_state != (MCPlayerState)num2)
			{
				_state = (MCPlayerState)num2;
				flag = true;
			}
			byte b = *(byte*)((ulong)(nint)pStatus + 16uL);
			int num3 = ((b != 0) ? 1 : 0);
			if ((_isReady ? 1 : 0) != num3)
			{
				int num4 = ((_isReady = ((b != 0) ? true : false)) ? 1 : 0);
				flag = true;
			}
			ulong num5 = *(ulong*)((ulong)(nint)pStatus + 24uL);
			if (num5 == 0L)
			{
				if (_firstDenial != null)
				{
					_firstDenial = null;
					flag = true;
				}
			}
			else if (!HResultDenialsEqual((MCHResultAnnouncement*)num5, _firstDenial))
			{
				raise_AlertSent(_firstDenial = MarshalAlert((MCHResultAnnouncement*)(*(ulong*)((ulong)(nint)pStatus + 24uL))));
			}
			if (!_fShuttingDown && flag)
			{
				raise_StatusChanged(this, EventArgs.Empty);
			}
			if (_fShuttingDown)
			{
				switch (_state)
				{
				case MCPlayerState.Closed:
					Module.SetEvent(_gotStateCloseEvent);
					break;
				case MCPlayerState.Uninitialized:
					Module.SetEvent(_gotStateUninitializeEvent);
					break;
				}
			}
		}

		internal unsafe void OnPropertyChanged(ushort* wzKey, byte* pData, uint dataLength)
		{
			object value = null;
			IntPtr ptr = new IntPtr(wzKey);
			string text = Marshal.PtrToStringUni(ptr);
			if (pData != null)
			{
				if (string.Compare(text, "presentationinfo", StringComparison.Ordinal) == 0)
				{
					byte needOverscan = (byte)((*(bool*)((ulong)(nint)pData + 16uL)) ? 1 : 0);
					value = new PresentationInfo((int)(double)(*(float*)pData), (int)(double)(*(float*)((ulong)(nint)pData + 4uL)), *(int*)((ulong)(nint)pData + 8uL), *(int*)((ulong)(nint)pData + 12uL), needOverscan != 0);
				}
				else if (string.Compare(text, "volumeinfo", StringComparison.Ordinal) == 0)
				{
					_volume = *(int*)pData;
					int num = ((_mute = ((*(int*)((ulong)(nint)pData + 4uL) != 0) ? true : false)) ? 1 : 0);
				}
				else if (string.Compare(text, "canchangevideorate", StringComparison.Ordinal) == 0)
				{
					value = (_canChangeVideoRate = *pData != 0);
				}
				else if (string.Compare(text, "mbrheuristicsdata", StringComparison.Ordinal) == 0)
				{
					_MBRHEURISTICDATA mBRHEURISTICDATA;
                    // IL cpblk instruction
                    Unsafe.CopyBlock(ref mBRHEURISTICDATA, pData, 48);
					BandwidthUpdateArgs value2 = new BandwidthUpdateArgs(*(long*)(&mBRHEURISTICDATA), Unsafe.As<_MBRHEURISTICDATA, float>(ref Unsafe.AddByteOffset(ref mBRHEURISTICDATA, 8)), Unsafe.As<_MBRHEURISTICDATA, int>(ref Unsafe.AddByteOffset(ref mBRHEURISTICDATA, 12)), Unsafe.As<_MBRHEURISTICDATA, int>(ref Unsafe.AddByteOffset(ref mBRHEURISTICDATA, 16)), Unsafe.As<_MBRHEURISTICDATA, int>(ref Unsafe.AddByteOffset(ref mBRHEURISTICDATA, 20)), Unsafe.As<_MBRHEURISTICDATA, int>(ref Unsafe.AddByteOffset(ref mBRHEURISTICDATA, 24)), Unsafe.As<_MBRHEURISTICDATA, int>(ref Unsafe.AddByteOffset(ref mBRHEURISTICDATA, 28)), Unsafe.As<_MBRHEURISTICDATA, int>(ref Unsafe.AddByteOffset(ref mBRHEURISTICDATA, 32)), Unsafe.As<_MBRHEURISTICDATA, int>(ref Unsafe.AddByteOffset(ref mBRHEURISTICDATA, 36)), Unsafe.As<_MBRHEURISTICDATA, int>(ref Unsafe.AddByteOffset(ref mBRHEURISTICDATA, 40)), Unsafe.As<_MBRHEURISTICDATA, MBRHeuristicState>(ref Unsafe.AddByteOffset(ref mBRHEURISTICDATA, 44)));
					raise_PlayerBandwithUpdate(this, value2);
				}
			}
			_properties[text] = value;
			PlayerPropertyChangedEventArgs value3 = new PlayerPropertyChangedEventArgs(text, value);
			raise_PlayerPropertyChanged(this, value3);
		}

		internal unsafe void OnTransportStatusChanged(MCTransportStatus* pTransportStatus)
		{
			_transportState = *(MCTransportState*)pTransportStatus;
			_rate = *(float*)((ulong)(nint)pTransportStatus + 4uL);
			int num = ((_endOfMedia = ((*(bool*)((ulong)(nint)pTransportStatus + 8uL)) ? true : false)) ? 1 : 0);
			int num2 = ((_canSeek = ((*(bool*)((ulong)(nint)pTransportStatus + 9uL)) ? true : false)) ? 1 : 0);
			raise_TransportStatusChanged(this, EventArgs.Empty);
		}

		internal void OnTransportPositionChanged(long position, long minSeekPosition, long maxSeekPosition)
		{
			_position = position;
			_minSeekPosition = minSeekPosition;
			_maxSeekPosition = maxSeekPosition;
			raise_TransportPositionChanged(this, EventArgs.Empty);
		}

		internal unsafe void OnUriSet(ushort* wzUri, uint dwParam)
		{
			_currentUriID = (int)dwParam;
			_currentUri = new string((char*)wzUri);
			raise_UriSet(this, EventArgs.Empty);
		}

		[SpecialName]
		protected virtual void raise_PlayerPropertyChanged(object value0, PlayerPropertyChangedEventArgs value1)
		{
			_003Cbacking_store_003EPlayerPropertyChanged?.Invoke(value0, value1);
		}

		[SpecialName]
		protected virtual void raise_PlayerBandwithUpdate(object value0, BandwidthUpdateArgs value1)
		{
			_003Cbacking_store_003EPlayerBandwithUpdate?.Invoke(value0, value1);
		}

		private void _007EPlayerInterop()
		{
			_firstDenial = null;
			Uninitialize();
		}

		public unsafe void Initialize()
		{
			//IL_0003: Expected I, but got I8
			//IL_0007: Expected I, but got I8
			//IL_000b: Expected I, but got I8
			//IL_000e: Expected I, but got I8
			//IL_0012: Expected I, but got I8
			//IL_0016: Expected I, but got I8
			//IL_0021: Expected I, but got I8
			//IL_0021: Expected I, but got I8
			//IL_0046: Expected I, but got I8
			//IL_0046: Expected I, but got I8
			//IL_00a2: Expected I, but got I8
			//IL_00d3: Expected I, but got I8
			//IL_0104: Expected I, but got I8
			//IL_012c: Expected I, but got I8
			//IL_0157: Expected I, but got I8
			//IL_0188: Expected I, but got I8
			//IL_01bb: Expected I, but got I8
			//IL_01fd: Expected I, but got I8
			//IL_0221: Expected I, but got I8
			//IL_0227: Expected I, but got I8
			//IL_0240: Expected I, but got I8
			//IL_0265: Expected I, but got I8
			//IL_026b: Expected I, but got I8
			//IL_0284: Expected I, but got I8
			//IL_02bc: Expected I, but got I8
			IMCPlayer* ptr = null;
			IMCTransport* ptr2 = null;
			IMCPlayerSetUri* ptr3 = null;
			IMCDynamicImage* ptr4 = null;
			IMCVolumeControl* ptr5 = null;
			IZuneSpectrumMgr* ptr6 = null;
			if ((_gotStateCloseEvent = Module.CreateEventW(null, 0, 0, null)) == null)
			{
				throw new COMException("PlayerInterop failed to CreateEvent for close", 0);
			}
			if ((_gotStateUninitializeEvent = Module.CreateEventW(null, 0, 0, null)) == null)
			{
				throw new COMException("PlayerInterop failed to CreateEvent for uninit", 0);
			}
			int num = Module.WmpCoreInitialize();
			if (num < 0)
			{
				throw new COMException("Playback core initialization failed.", num);
			}
			num = Module.CWmpPlayer_GetInstance(&ptr);
			if (num >= 0 && ptr != null)
			{
				_uPlayer = ptr;
				IMCPlayer* intPtr = ptr;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, void**, int>)(*(ulong*)(*(ulong*)ptr)))((nint)intPtr, (_GUID*)Unsafe.AsPointer(ref Module._GUID_2f33a725_95cb_4080_adef_93a067a707ba), (void**)(&ptr2));
				if (num >= 0 && ptr2 != null)
				{
					_uTransport = ptr2;
					IMCPlayer* uPlayer = _uPlayer;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, void**, int>)(*(ulong*)(*(ulong*)uPlayer)))((nint)uPlayer, (_GUID*)Unsafe.AsPointer(ref Module._GUID_58864c93_45f9_4c6d_aa3f_80f6caa08281), (void**)(&ptr3));
					if (num >= 0 && ptr3 != null)
					{
						_uSetUri = ptr3;
						IMCPlayer* uPlayer2 = _uPlayer;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, void**, int>)(*(ulong*)(*(ulong*)uPlayer2)))((nint)uPlayer2, (_GUID*)Unsafe.AsPointer(ref Module._GUID_102e281e_28ad_4688_aaff_f560f8053d90), (void**)(&ptr4));
						if (num >= 0 && ptr4 != null)
						{
							_uDynamicImage = ptr4;
							IMCDynamicImage* intPtr2 = ptr4;
							int nDynamicImage = _nDynamicImage;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)ptr4 + 24)))((nint)intPtr2, nDynamicImage);
							if (num < 0)
							{
								throw new COMException("PlayerInterop failed to initialize IMCDynamicImage", num);
							}
							IMCPlayer* uPlayer3 = _uPlayer;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, void**, int>)(*(ulong*)(*(ulong*)uPlayer3)))((nint)uPlayer3, (_GUID*)Unsafe.AsPointer(ref Module._GUID_f6ba930c_78c3_488c_924d_2d3fc1e8fb70), (void**)(&ptr5));
							if (num >= 0 && ptr5 != null)
							{
								_uVolumeControl = ptr5;
								IMCPlayer* uPlayer4 = _uPlayer;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, void**, int>)(*(ulong*)(*(ulong*)uPlayer4)))((nint)uPlayer4, (_GUID*)Unsafe.AsPointer(ref Module._GUID_aff1732d_13f3_45e5_a52f_a854729e8730), (void**)(&ptr6));
								if (num >= 0 && ptr6 != null)
								{
									_uZuneSpectrumMgr = ptr6;
									CPlayerInteropEventSink* ptr7 = (CPlayerInteropEventSink*)Module.@new(48uL);
									CPlayerInteropEventSink* ptr8;
									try
									{
										ptr8 = ((ptr7 == null) ? null : Module.MicrosoftZunePlayback_002ECPlayerInteropEventSink_002E_007Bctor_007D(ptr7, this));
									}
									catch
									{
										//try-fault
										Module.delete(ptr7);
										throw;
									}
									_uEventSink = ptr8;
									if (ptr8 == null)
									{
										throw new OutOfMemoryException();
									}
									long num2 = *(long*)_uPlayer + 24;
									IMCPlayer* uPlayer5 = _uPlayer;
									void* intPtr3 = _windowHandle.ToPointer();
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HWND*, uint, IMCPlayerEvents*, int>)(*(ulong*)num2))((nint)uPlayer5, (HWND*)intPtr3, 0u, (IMCPlayerEvents*)ptr8);
									if (num < 0)
									{
										throw new COMException("PlayerInterop failed to initialize IMCPlayer", num);
									}
									CPlayerInteropEventSink* uEventSink = _uEventSink;
									CPlayerInteropEventSink* ptr9 = (CPlayerInteropEventSink*)((uEventSink == null) ? 0 : ((ulong)(nint)uEventSink + 8uL));
									IMCTransport* uTransport = _uTransport;
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IMCTransportEvents*, int>)(*(ulong*)(*(long*)uTransport + 24)))((nint)uTransport, (IMCTransportEvents*)ptr9);
									if (num < 0)
									{
										throw new COMException("PlayerInterop failed to initialize IMCTransport", num);
									}
									CPlayerInteropEventSink* uEventSink2 = _uEventSink;
									CPlayerInteropEventSink* ptr10 = (CPlayerInteropEventSink*)((uEventSink2 == null) ? 0 : ((ulong)(nint)uEventSink2 + 16uL));
									IMCPlayerSetUri* uSetUri = _uSetUri;
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IMCPlayerSetUriEvents*, int>)(*(ulong*)(*(long*)uSetUri + 24)))((nint)uSetUri, (IMCPlayerSetUriEvents*)ptr10);
									if (num < 0)
									{
										throw new COMException("PlayerInterop failed to initialize IMCPlayerSetUri", num);
									}
									long num3 = *(long*)_uTransport + 96;
									IMCTransport* uTransport2 = _uTransport;
									double totalMilliseconds = _positionEventInterval.TotalMilliseconds;
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, int>)(*(ulong*)num3))((nint)uTransport2, (uint)totalMilliseconds);
									if (num < 0 && num != -2147467263)
									{
										throw new COMException("Attempt to set initial position event firing interval failed.", num);
									}
									_properties = new Dictionary<string, object>();
									return;
								}
								throw new COMException("PlayerInterop failed to get IZuneSpectrumMgr instance.", num);
							}
							throw new COMException("PlayerInterop failed to get IMCVolumeControl instance.", num);
						}
						throw new COMException("PlayerInterop failed to get IMCDynamicImage instance.", num);
					}
					throw new COMException("PlayerInterop failed to get IMCTransport instance.", num);
				}
				throw new COMException("PlayerInterop failed to get IMCTransport instance.", num);
			}
			throw new COMException("PlayerInterop failed to get IMCPlayer instance.", num);
		}

		public unsafe void Uninitialize()
		{
			//IL_0028: Expected I, but got I8
			//IL_004c: Expected I, but got I8
			//IL_009c: Expected I, but got I8
			//IL_00b1: Expected I, but got I8
			//IL_00c5: Expected I, but got I8
			//IL_00e7: Expected I, but got I8
			//IL_00f0: Expected I, but got I8
			//IL_010c: Expected I, but got I8
			//IL_0115: Expected I, but got I8
			//IL_0131: Expected I, but got I8
			//IL_013a: Expected I, but got I8
			//IL_0156: Expected I, but got I8
			//IL_015f: Expected I, but got I8
			//IL_0178: Expected I, but got I8
			//IL_0181: Expected I, but got I8
			//IL_019a: Expected I, but got I8
			//IL_01a3: Expected I, but got I8
			//IL_01bc: Expected I, but got I8
			//IL_01c5: Expected I, but got I8
			bool flag = false;
			if (_uPlayer != null)
			{
				_fShuttingDown = true;
				IMCPlayer* uPlayer = _uPlayer;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)uPlayer + 40)))((nint)uPlayer);
				Module.WaitForSingleObject(_gotStateCloseEvent, 5000u);
				IMCPlayer* uPlayer2 = _uPlayer;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)uPlayer2 + 32)))((nint)uPlayer2);
				byte condition = (byte)((Module.WaitForSingleObject(_gotStateUninitializeEvent, 5000u) == 0 && _state == MCPlayerState.Uninitialized) ? 1 : 0);
				ShipAssert.AssertId(condition != 0, 80000u, 0u);
				flag = _state != 0 || flag;
				IMCPlayer* uPlayer3 = _uPlayer;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)uPlayer3 + 64)))((nint)uPlayer3);
			}
			Module.CloseHandle(_gotStateCloseEvent);
			_gotStateCloseEvent = null;
			Module.CloseHandle(_gotStateUninitializeEvent);
			_gotStateUninitializeEvent = null;
			if (!flag)
			{
				CPlayerInteropEventSink* uEventSink = _uEventSink;
				if (0L != (nint)uEventSink)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)uEventSink + 16)))((nint)uEventSink);
					_uEventSink = null;
				}
				IMCPlayer* uPlayer4 = _uPlayer;
				if (0L != (nint)uPlayer4)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)uPlayer4 + 16)))((nint)uPlayer4);
					_uPlayer = null;
				}
				IMCTransport* uTransport = _uTransport;
				if (0L != (nint)uTransport)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)uTransport + 16)))((nint)uTransport);
					_uTransport = null;
				}
				IMCPlayerSetUri* uSetUri = _uSetUri;
				if (0L != (nint)uSetUri)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)uSetUri + 16)))((nint)uSetUri);
					_uSetUri = null;
				}
				IMCDynamicImage* uDynamicImage = _uDynamicImage;
				if (0L != (nint)uDynamicImage)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)uDynamicImage + 16)))((nint)uDynamicImage);
					_uDynamicImage = null;
				}
				IMCVolumeControl* uVolumeControl = _uVolumeControl;
				if (0L != (nint)uVolumeControl)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)uVolumeControl + 16)))((nint)uVolumeControl);
					_uVolumeControl = null;
				}
				IZuneSpectrumMgr* uZuneSpectrumMgr = _uZuneSpectrumMgr;
				if (0L != (nint)uZuneSpectrumMgr)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)uZuneSpectrumMgr + 16)))((nint)uZuneSpectrumMgr);
					_uZuneSpectrumMgr = null;
				}
				Module.WmpCoreDeinitialize();
			}
		}

		public unsafe void Close()
		{
			//IL_0017: Expected I, but got I8
			IMCPlayer* uPlayer = _uPlayer;
			if (uPlayer != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)uPlayer + 40)))((nint)uPlayer);
			}
		}

		[SpecialName]
		protected void raise_StatusChanged(object value0, EventArgs value1)
		{
			_003Cbacking_store_003EStatusChanged?.Invoke(value0, value1);
		}

		[SpecialName]
		protected void raise_AlertSent(Announcement value0)
		{
			_003Cbacking_store_003EAlertSent?.Invoke(value0);
		}

		public unsafe void Play()
		{
			//IL_002a: Expected I, but got I8
			//IL_0040: Expected I, but got I8
			//IL_0053: Expected I, but got I8
			IMCVolumeControl* uVolumeControl = _uVolumeControl;
			if (uVolumeControl != null && _uTransport != null)
			{
				bool mute = _mute;
				if (mute)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte, int>)(*(ulong*)(*(long*)uVolumeControl + 32)))((nint)uVolumeControl, mute ? ((byte)1) : ((byte)0));
				}
				else
				{
					int volume = _volume;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)uVolumeControl + 24)))((nint)uVolumeControl, volume);
				}
				IMCTransport* uTransport = _uTransport;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)uTransport + 40)))((nint)uTransport);
			}
		}

		public unsafe void Pause()
		{
			//IL_0017: Expected I, but got I8
			IMCTransport* uTransport = _uTransport;
			if (uTransport != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)uTransport + 32)))((nint)uTransport);
			}
		}

		public unsafe void Stop()
		{
			//IL_0017: Expected I, but got I8
			IMCTransport* uTransport = _uTransport;
			if (uTransport != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)uTransport + 48)))((nint)uTransport);
			}
		}

		public unsafe void SeekToRelativePosition(long offsetIn100nsUnits)
		{
			//IL_0018: Expected I, but got I8
			IMCTransport* uTransport = _uTransport;
			if (uTransport != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, long, int>)(*(ulong*)(*(long*)uTransport + 56)))((nint)uTransport, offsetIn100nsUnits);
			}
		}

		public unsafe void SeekToAbsolutePosition(long offsetIn100nsUnits)
		{
			//IL_0018: Expected I, but got I8
			IMCTransport* uTransport = _uTransport;
			if (uTransport != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, long, int>)(*(ulong*)(*(long*)uTransport + 64)))((nint)uTransport, offsetIn100nsUnits);
			}
		}

		public unsafe void SeekToRelativeFrame(long offsetInFrames)
		{
			//IL_0018: Expected I, but got I8
			IMCTransport* uTransport = _uTransport;
			if (uTransport != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, long, int>)(*(ulong*)(*(long*)uTransport + 72)))((nint)uTransport, offsetInFrames);
			}
		}

		[SpecialName]
		protected void raise_TransportStatusChanged(object value0, EventArgs value1)
		{
			_003Cbacking_store_003ETransportStatusChanged?.Invoke(value0, value1);
		}

		[SpecialName]
		protected void raise_TransportPositionChanged(object value0, EventArgs value1)
		{
			_003Cbacking_store_003ETransportPositionChanged?.Invoke(value0, value1);
		}

		public unsafe void SetUri(string uri, long startPositionIn100nsUnits, int UriID)
		{
			//IL_0033: Expected I, but got I8
			if (!(uri != (string)null))
			{
				return;
			}
			fixed (char* uriPtr = uri.ToCharArray())
			{
				ushort* ptr = (ushort*)uriPtr;
				try
				{
					IMCPlayerSetUri* uSetUri = _uSetUri;
					if (uSetUri != null)
					{
						long num = *(long*)uSetUri + 32;
						IMCPlayerSetUri* uSetUri2 = _uSetUri;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, long, uint, int>)(*(ulong*)num))((nint)uSetUri2, ptr, startPositionIn100nsUnits, (uint)UriID);
					}
				}
				catch
				{
					//try-fault
					ptr = null;
					throw;
				}
			}
		}

		public unsafe void SetNextUri(string uri, long startPositionIn100nsUnits, int UriID)
		{
			//IL_0033: Expected I, but got I8
			if (!(uri != (string)null))
			{
				return;
			}
			fixed (char* uriPtr = uri.ToCharArray())
			{
				ushort* ptr = (ushort*)uriPtr;
				try
				{
					IMCPlayerSetUri* uSetUri = _uSetUri;
					if (uSetUri != null)
					{
						long num = *(long*)uSetUri + 40;
						IMCPlayerSetUri* uSetUri2 = _uSetUri;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, long, uint, int>)(*(ulong*)num))((nint)uSetUri2, ptr, startPositionIn100nsUnits, (uint)UriID);
					}
				}
				catch
				{
					//try-fault
					ptr = null;
					throw;
				}
			}
		}

		public unsafe void CancelNext()
		{
			//IL_0017: Expected I, but got I8
			IMCPlayerSetUri* uSetUri = _uSetUri;
			if (uSetUri != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)uSetUri + 48)))((nint)uSetUri);
			}
		}

		[SpecialName]
		protected void raise_UriSet(object value0, EventArgs value1)
		{
			_003Cbacking_store_003EUriSet?.Invoke(value0, value1);
		}

		public unsafe void ConnectAnimationsToSpectrumAnalyzer(uint uixAnimationsId, uint bandCount, [MarshalAs(UnmanagedType.U1)] bool outputFrequencyData, [MarshalAs(UnmanagedType.U1)] bool outputWaveformData, [MarshalAs(UnmanagedType.U1)] bool enableStereoOutput)
		{
			//IL_001e: Expected I, but got I8
			IZuneSpectrumMgr* uZuneSpectrumMgr = _uZuneSpectrumMgr;
			if (uZuneSpectrumMgr != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, uint, byte, byte, byte, int>)(*(ulong*)(*(long*)uZuneSpectrumMgr + 24)))((nint)uZuneSpectrumMgr, uixAnimationsId, bandCount, outputFrequencyData ? ((byte)1) : ((byte)0), outputWaveformData ? ((byte)1) : ((byte)0), enableStereoOutput ? ((byte)1) : ((byte)0));
			}
		}

		public unsafe void DisconnectAnimationsFromSpectrumAnalyzer(uint uixAnimationsId)
		{
			//IL_0018: Expected I, but got I8
			IZuneSpectrumMgr* uZuneSpectrumMgr = _uZuneSpectrumMgr;
			if (uZuneSpectrumMgr != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, int>)(*(ulong*)(*(long*)uZuneSpectrumMgr + 32)))((nint)uZuneSpectrumMgr, uixAnimationsId);
			}
		}

		public unsafe void ProgressivePlaybackReleaseFile()
		{
			//IL_0032: Expected I, but got I8
			//IL_0032: Expected I, but got I8
			//IL_0033: Expected I8, but got I
			//IL_005e: Expected I, but got I8
			//IL_0074: Expected I, but got I8
			if (_uTransport == null)
			{
				return;
			}
			CPlayerInteropEventSink* uEventSink = _uEventSink;
			if (uEventSink != null && *(long*)((ulong)(nint)uEventSink + 40uL) == 0L)
			{
				*(long*)((ulong)(nint)_uEventSink + 40uL) = (nint)Module.CreateEventW(null, 0, 0, null);
				if (*(long*)((ulong)(nint)_uEventSink + 40uL) == 0L)
				{
					throw new COMException("PlayerInterop failed to CreateEvent for ProgressivePlaybackReleaseFile", 0);
				}
				IMCTransport* uTransport = _uTransport;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)uTransport + 104)))((nint)uTransport);
				byte condition = ((Module.WaitForSingleObject((void*)(*(ulong*)((ulong)(nint)_uEventSink + 40uL)), 30000u) == 0) ? ((byte)1) : ((byte)0));
				ShipAssert.AssertId(condition != 0, 80001u, 0u);
			}
		}

		public unsafe void ProgressivePlaybackReopenFile()
		{
			//IL_0029: Expected I, but got I8
			//IL_003a: Expected I, but got I8
			IMCTransport* uTransport = _uTransport;
			if (uTransport != null)
			{
				CPlayerInteropEventSink* uEventSink = _uEventSink;
				if (uEventSink != null && *(long*)((ulong)(nint)uEventSink + 40uL) != 0L)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)uTransport + 112)))((nint)uTransport);
					Module.CloseHandle((void*)(*(ulong*)((ulong)(nint)_uEventSink + 40uL)));
					*(long*)((ulong)(nint)_uEventSink + 40uL) = 0L;
				}
			}
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_007EPlayerInterop();
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
