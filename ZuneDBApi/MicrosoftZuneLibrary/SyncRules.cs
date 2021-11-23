using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary
{
	public class SyncRules : IDisposable
	{
		private readonly CComPtrMgd_003CIEndpointHost_003E m_spEndpointHost;

		private int m_iDeviceID;

		private ArrayList m_listAllowedToAdd;

		private ArrayList m_listAllowedToExclude;

		internal unsafe SyncRules(IEndpointHost* pEndpointHost)
		{
			//IL_0083: Expected I, but got I8
			CComPtrMgd_003CIEndpointHost_003E spEndpointHost = new CComPtrMgd_003CIEndpointHost_003E();
			try
			{
				m_spEndpointHost = spEndpointHost;
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 0x200u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 10, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x09bc4a52_002EWPP_SyncRulesAPI_cpp_Traceguids));
				}
				m_spEndpointHost.op_Assign(pEndpointHost);
				IEndpointHost* p = m_spEndpointHost.p;
				int iDeviceID;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostProperty, int*, int>)(*(ulong*)(*(long*)p + 136)))((nint)p, EEndpointHostProperty.eEndpointHostPropertyDatabaseEndpointId, &iDeviceID);
				m_iDeviceID = iDeviceID;
				(m_listAllowedToAdd = new ArrayList()).Add(EMediaTypes.eMediaTypeAudio);
				m_listAllowedToAdd.Add(EMediaTypes.eMediaTypeAudioAlbum);
				m_listAllowedToAdd.Add(EMediaTypes.eMediaTypePersonArtist);
				m_listAllowedToAdd.Add(EMediaTypes.eMediaTypeImage);
				m_listAllowedToAdd.Add(EMediaTypes.eMediaTypeVideo);
				m_listAllowedToAdd.Add(EMediaTypes.eMediaTypePodcastEpisode);
				m_listAllowedToAdd.Add(EMediaTypes.eMediaTypePodcastSeries);
				m_listAllowedToAdd.Add(EMediaTypes.eMediaTypePlaylist);
				m_listAllowedToAdd.Add(EMediaTypes.eMediaTypeFolder);
				m_listAllowedToAdd.Add(EMediaTypes.eMediaTypeGenre);
				m_listAllowedToAdd.Add(EMediaTypes.eMediaTypeUserCard);
				m_listAllowedToAdd.Add(EMediaTypes.eMediaTypeApp);
				(m_listAllowedToExclude = new ArrayList()).Add(EMediaTypes.eMediaTypeAudio);
				m_listAllowedToExclude.Add(EMediaTypes.eMediaTypeAudioAlbum);
				m_listAllowedToExclude.Add(EMediaTypes.eMediaTypePersonArtist);
				m_listAllowedToExclude.Add(EMediaTypes.eMediaTypeImage);
				m_listAllowedToExclude.Add(EMediaTypes.eMediaTypeVideo);
				m_listAllowedToExclude.Add(EMediaTypes.eMediaTypePodcastEpisode);
				m_listAllowedToExclude.Add(EMediaTypes.eMediaTypePodcastSeries);
				m_listAllowedToExclude.Add(EMediaTypes.eMediaTypePlaylist);
				m_listAllowedToExclude.Add(EMediaTypes.eMediaTypeFolder);
				m_listAllowedToExclude.Add(EMediaTypes.eMediaTypeGenre);
				m_listAllowedToExclude.Add(EMediaTypes.eMediaTypeUserCard);
				m_listAllowedToExclude.Add(EMediaTypes.eMediaTypeApp);
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 0x200u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 11, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x09bc4a52_002EWPP_SyncRulesAPI_cpp_Traceguids));
				}
			}
			catch
			{
				//try-fault
				((IDisposable)m_spEndpointHost).Dispose();
				throw;
			}
		}

		private unsafe void _007ESyncRules()
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 0x200u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 12, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x09bc4a52_002EWPP_SyncRulesAPI_cpp_Traceguids));
			}
			m_spEndpointHost.Release();
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 0x200u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 13, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x09bc4a52_002EWPP_SyncRulesAPI_cpp_Traceguids));
			}
		}

		public unsafe int Add(int[] rgIds, EMediaTypes mediaType)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 0x200u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 14, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x09bc4a52_002EWPP_SyncRulesAPI_cpp_Traceguids));
			}
			int num;
			if (!m_listAllowedToAdd.Contains(mediaType))
			{
				num = -2147024809;
			}
			else
			{
				num = RemoveInternal(rgIds, mediaType, EDeviceSyncRuleType.eDeviceSyncRuleTypeExclude, fDeviceFolderIds: false);
				if (num >= 0)
				{
					num = AddInternal(rgIds, mediaType, EDeviceSyncRuleType.eDeviceSyncRuleTypeIncludeAll, fAutoSelectRuleType: true);
				}
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 0x200u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 15, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x09bc4a52_002EWPP_SyncRulesAPI_cpp_Traceguids), num);
			}
			return num;
		}

		public unsafe int Add(int[] rgIds, EMediaTypes mediaType, EDeviceSyncRuleType ruleType)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 0x200u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 16, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x09bc4a52_002EWPP_SyncRulesAPI_cpp_Traceguids));
			}
			int num;
			if (EMediaTypes.eMediaTypePodcastSeries != mediaType)
			{
				num = -2147024809;
			}
			else
			{
				num = RemoveInternal(rgIds, EMediaTypes.eMediaTypePodcastSeries, EDeviceSyncRuleType.eDeviceSyncRuleTypeExclude, fDeviceFolderIds: false);
				if (num >= 0)
				{
					num = Remove(rgIds, EMediaTypes.eMediaTypePodcastSeries, fDeviceFolderIds: false);
					if (num >= 0)
					{
						num = AddInternal(rgIds, EMediaTypes.eMediaTypePodcastSeries, ruleType, fAutoSelectRuleType: false);
					}
				}
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 0x200u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 17, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x09bc4a52_002EWPP_SyncRulesAPI_cpp_Traceguids), num);
			}
			return num;
		}

		public unsafe int AddDeviceSyncRuleWithValue(int[] rgIds, int value)
		{
			int num = 0;
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 0x200u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 18, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x09bc4a52_002EWPP_SyncRulesAPI_cpp_Traceguids));
			}
			int num2 = 0;
			if (0 < (nint)rgIds.LongLength)
			{
				while (num >= 0)
				{
					num = Module.AddDeviceSyncRuleWithValue(EDeviceSyncRuleType.eDeviceSyncRuleTypeSyncEpisodesCount, m_iDeviceID, EMediaTypes.eMediaTypePodcastSeries, rgIds[num2], value);
					if (num < 0)
					{
						break;
					}
					num2++;
					if (num2 >= (nint)rgIds.LongLength)
					{
						break;
					}
				}
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 0x200u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 19, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x09bc4a52_002EWPP_SyncRulesAPI_cpp_Traceguids), num);
			}
			return num;
		}

		public unsafe int Remove(int[] rgIds, EMediaTypes mediaType, [MarshalAs(UnmanagedType.U1)] bool fDeviceFolderIds)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 0x200u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 24, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x09bc4a52_002EWPP_SyncRulesAPI_cpp_Traceguids));
			}
			int num = (m_listAllowedToAdd.Contains(mediaType) ? RemoveInternal(rgIds, mediaType, EDeviceSyncRuleType.eDeviceSyncRuleTypeIncludeAll, fDeviceFolderIds) : (-2147024809));
			if (EMediaTypes.eMediaTypePodcastSeries == mediaType && num >= 0)
			{
				num = RemoveInternal(rgIds, EMediaTypes.eMediaTypePodcastSeries, EDeviceSyncRuleType.eDeviceSyncRuleTypeNone, fDeviceFolderIds);
				if (num >= 0)
				{
					num = RemoveInternal(rgIds, EMediaTypes.eMediaTypePodcastSeries, EDeviceSyncRuleType.eDeviceSyncRuleTypeAllUnplayed, fDeviceFolderIds);
					if (num >= 0)
					{
						num = RemoveInternal(rgIds, EMediaTypes.eMediaTypePodcastSeries, EDeviceSyncRuleType.eDeviceSyncRuleTypeFirstUnplayed, fDeviceFolderIds);
					}
				}
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 0x200u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 25, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x09bc4a52_002EWPP_SyncRulesAPI_cpp_Traceguids), num);
			}
			return num;
		}

		public unsafe int Exclude(int[] rgIds, EMediaTypes mediaType)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 0x200u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 20, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x09bc4a52_002EWPP_SyncRulesAPI_cpp_Traceguids));
			}
			int num;
			if (!m_listAllowedToExclude.Contains(mediaType))
			{
				num = -2147024809;
			}
			else
			{
				num = Remove(rgIds, mediaType, fDeviceFolderIds: false);
				if (num >= 0)
				{
					num = AddInternal(rgIds, mediaType, EDeviceSyncRuleType.eDeviceSyncRuleTypeExclude, fAutoSelectRuleType: false);
				}
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 0x200u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 21, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x09bc4a52_002EWPP_SyncRulesAPI_cpp_Traceguids), num);
			}
			return num;
		}

		public unsafe int Unexclude(int[] rgIds, EMediaTypes mediaType)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 0x200u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 26, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x09bc4a52_002EWPP_SyncRulesAPI_cpp_Traceguids));
			}
			int num = (m_listAllowedToExclude.Contains(mediaType) ? RemoveInternal(rgIds, mediaType, EDeviceSyncRuleType.eDeviceSyncRuleTypeExclude, fDeviceFolderIds: false) : (-2147024809));
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 0x200u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 27, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x09bc4a52_002EWPP_SyncRulesAPI_cpp_Traceguids), num);
			}
			return num;
		}

		public unsafe int GetSyncRuleForMedia(EMediaTypes mediaType, int iMediaItemId, ref EDeviceSyncRuleType ruleType)
		{
			EDeviceSyncRuleType eDeviceSyncRuleType;
			int num = Module.GetSyncRuleForMedia(m_iDeviceID, mediaType, iMediaItemId, &eDeviceSyncRuleType);
			if (num >= 0)
			{
				ruleType = eDeviceSyncRuleType;
			}
			return num;
		}

		public unsafe int GetSyncRuleValueForMedia(int iMediaItemId, ref int iValue)
		{
			int value;
			int num2 = Module.GetSyncRuleValueForMedia(EDeviceSyncRuleType.eDeviceSyncRuleTypeSyncEpisodesCount, m_iDeviceID, EMediaTypes.eMediaTypePodcastSeries, iMediaItemId, &value);
			if (num2 >= 0)
			{
				iValue = value;
			}
			return num2;
		}

		public unsafe int GetCategorySyncMode(ESyncCategory cat, ref ESyncMode mode, [MarshalAs(UnmanagedType.U1)] bool fEstablishingPartnership)
		{
			//IL_0058: Expected I, but got I8
			//IL_0058: Expected I, but got I8
			//IL_007d: Expected I, but got I8
			//IL_007d: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 454u);
				return -2147418113;
			}
			CComPtrNtv_003CIMetadataManager_003E cComPtrNtv_003CIMetadataManager_003E;
			*(long*)(&cComPtrNtv_003CIMetadataManager_003E) = 0L;
			int num;
			try
			{
				CComPtrNtv_003CIDeviceSyncRulesProvider_003E cComPtrNtv_003CIDeviceSyncRulesProvider_003E;
				*(long*)(&cComPtrNtv_003CIDeviceSyncRulesProvider_003E) = 0L;
				try
				{
					mode = ESyncMode.eSyncModeInvalid;
					num = Module.GetSingleton(Module.GUID_IMetadataManager, (void**)(&cComPtrNtv_003CIMetadataManager_003E));
					if (num >= 0)
					{
						long num2 = *(long*)(&cComPtrNtv_003CIMetadataManager_003E);
						_GUID guid_GUID_IDeviceSyncRulesProvider = Module.GUID_IDeviceSyncRulesProvider;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID, void**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIMetadataManager_003E)) + 24)))((nint)num2, (_GUID)guid_GUID_IDeviceSyncRulesProvider, (void**)(&cComPtrNtv_003CIDeviceSyncRulesProvider_003E));
						if (num >= 0)
						{
							ESyncMode eSyncMode = ESyncMode.eSyncModeInvalid;
							long num3 = *(long*)(&cComPtrNtv_003CIDeviceSyncRulesProvider_003E);
							int iDeviceID = m_iDeviceID;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, ESyncCategory, ESyncMode*, byte, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIDeviceSyncRulesProvider_003E)) + 128)))((nint)num3, iDeviceID, cat, &eSyncMode, fEstablishingPartnership ? ((byte)1) : ((byte)0));
							if (num >= 0)
							{
								mode = eSyncMode;
							}
						}
					}
					if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 0x200u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
					{
						Module.WPP_SF_LLd(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 30, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x09bc4a52_002EWPP_SyncRulesAPI_cpp_Traceguids), (uint)cat, (uint)mode, num);
					}
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIDeviceSyncRulesProvider_003E*, void>)(&Module.CComPtrNtv_003CIDeviceSyncRulesProvider_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIDeviceSyncRulesProvider_003E);
					throw;
				}
				Module.CComPtrNtv_003CIDeviceSyncRulesProvider_003E_002ERelease(&cComPtrNtv_003CIDeviceSyncRulesProvider_003E);
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIMetadataManager_003E*, void>)(&Module.CComPtrNtv_003CIMetadataManager_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIMetadataManager_003E);
				throw;
			}
			Module.CComPtrNtv_003CIMetadataManager_003E_002ERelease(&cComPtrNtv_003CIMetadataManager_003E);
			return num;
		}

		public unsafe int SetCategorySyncMode(ESyncCategory cat, ESyncMode mode)
		{
			//IL_0055: Expected I, but got I8
			//IL_0055: Expected I, but got I8
			//IL_0076: Expected I, but got I8
			//IL_0076: Expected I, but got I8
			//IL_0099: Expected I, but got I8
			if (m_spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 504u);
				return -2147418113;
			}
			CComPtrNtv_003CIMetadataManager_003E cComPtrNtv_003CIMetadataManager_003E;
			*(long*)(&cComPtrNtv_003CIMetadataManager_003E) = 0L;
			int num;
			try
			{
				CComPtrNtv_003CIDeviceSyncRulesProvider_003E cComPtrNtv_003CIDeviceSyncRulesProvider_003E;
				*(long*)(&cComPtrNtv_003CIDeviceSyncRulesProvider_003E) = 0L;
				try
				{
					num = Module.GetSingleton(Module.GUID_IMetadataManager, (void**)(&cComPtrNtv_003CIMetadataManager_003E));
					if (num >= 0)
					{
						long num2 = *(long*)(&cComPtrNtv_003CIMetadataManager_003E);
						_GUID gUID_b12dc962_cc1b_46c5_a92a_68f1f2b9bff = Module.GUID_IDeviceSyncRulesProvider;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID, void**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIMetadataManager_003E)) + 24)))((nint)num2, gUID_b12dc962_cc1b_46c5_a92a_68f1f2b9bff, (void**)(&cComPtrNtv_003CIDeviceSyncRulesProvider_003E));
						if (num >= 0)
						{
							long num3 = *(long*)(&cComPtrNtv_003CIDeviceSyncRulesProvider_003E);
							int iDeviceID = m_iDeviceID;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, ESyncCategory, ESyncMode, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIDeviceSyncRulesProvider_003E)) + 136)))((nint)num3, iDeviceID, cat, mode);
							if (num >= 0)
							{
								IEndpointHost* p = m_spEndpointHost.p;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostAction, int>)(*(ulong*)(*(long*)p + 296)))((nint)p, EEndpointHostAction.eEndpointHostActionResetGasGauge);
							}
						}
					}
					if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 0x200u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
					{
						Module.WPP_SF_LLd(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 31, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x09bc4a52_002EWPP_SyncRulesAPI_cpp_Traceguids), (uint)cat, (uint)mode, num);
					}
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIDeviceSyncRulesProvider_003E*, void>)(&Module.CComPtrNtv_003CIDeviceSyncRulesProvider_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIDeviceSyncRulesProvider_003E);
					throw;
				}
				Module.CComPtrNtv_003CIDeviceSyncRulesProvider_003E_002ERelease(&cComPtrNtv_003CIDeviceSyncRulesProvider_003E);
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIMetadataManager_003E*, void>)(&Module.CComPtrNtv_003CIMetadataManager_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIMetadataManager_003E);
				throw;
			}
			Module.CComPtrNtv_003CIMetadataManager_003E_002ERelease(&cComPtrNtv_003CIMetadataManager_003E);
			return num;
		}

		public unsafe int GetDontSyncHatedContent(ref bool fDontSyncHatedContent)
		{
			//IL_0040: Expected I, but got I8
			CComPtrMgd_003CIEndpointHost_003E spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 554u);
				return -2147418113;
			}
			bool flag = false;
			IEndpointHost* p = spEndpointHost.p;
			int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, bool*, int>)(*(ulong*)(*(long*)p + 248)))((nint)p, EEndpointHostSetting.eEndpointHostSettingDontSyncHatedContent, &flag);
			if (num >= 0)
			{
				fDontSyncHatedContent = flag;
			}
			return num;
		}

		public unsafe int SetDontSyncHatedContent([MarshalAs(UnmanagedType.U1)] bool fDontSyncHatedContent)
		{
			//IL_0036: Expected I, but got I8
			IEndpointHost* p = m_spEndpointHost.p;
			if (p == null)
			{
				Module._ZuneShipAssert(1002u, 575u);
				return -2147418113;
			}
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EEndpointHostSetting, byte, int>)(*(ulong*)(*(long*)p + 280)))((nint)p, EEndpointHostSetting.eEndpointHostSettingDontSyncHatedContent, fDontSyncHatedContent ? ((byte)1) : ((byte)0));
		}

		public unsafe int GenerateSnapshot([MarshalAs(UnmanagedType.U1)] bool expandSyncAll, ref SyncRulesView syncRulesView)
		{
			//IL_0027: Expected I, but got I8
			//IL_0061: Expected I, but got I8
			CComPtrMgd_003CIEndpointHost_003E spEndpointHost = m_spEndpointHost;
			if (spEndpointHost.p == null)
			{
				Module._ZuneShipAssert(1002u, 592u);
				return -2147418113;
			}
			ISyncRulesView* ptr = null;
			EEndpointHostProperty propId = (expandSyncAll ? EEndpointHostProperty.eEndpointHostPropertyExpandedRulesView : EEndpointHostProperty.eEndpointHostPropertyCollapsedRulesView);
			int num = Module.GetInterfaceProperty_003Cstruct_0020IEndpointHost_002Cenum_0020EEndpointHostProperty_002Cstruct_0020ISyncRulesView_003E(spEndpointHost.p, propId, &ptr);
			if (num >= 0)
			{
				syncRulesView = new SyncRulesView(ptr);
			}
			if (0L != (nint)ptr)
			{
				ISyncRulesView* intPtr = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
			}
			return num;
		}

		private unsafe int AddInternal(int[] rgIds, EMediaTypes mediaType, EDeviceSyncRuleType ruleType, [MarshalAs(UnmanagedType.U1)] bool fAutoSelectRuleType)
		{
			int num = 0;
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 0x200u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 22, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x09bc4a52_002EWPP_SyncRulesAPI_cpp_Traceguids));
			}
			int num2 = 0;
			if (0 < (nint)rgIds.LongLength)
			{
				while (num >= 0)
				{
					num = Module.AddDeviceSyncRule(ruleType, fAutoSelectRuleType, m_iDeviceID, mediaType, rgIds[num2]);
					if (num < 0)
					{
						break;
					}
					num2++;
					if (num2 >= (nint)rgIds.LongLength)
					{
						break;
					}
				}
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 0x200u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 23, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x09bc4a52_002EWPP_SyncRulesAPI_cpp_Traceguids), num);
			}
			return num;
		}

		private unsafe int RemoveInternal(int[] rgIds, EMediaTypes mediaType, EDeviceSyncRuleType ruleType, [MarshalAs(UnmanagedType.U1)] bool fDeviceFolderIds)
		{
			int num = 0;
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 0x200u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 28, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x09bc4a52_002EWPP_SyncRulesAPI_cpp_Traceguids));
			}
			if (rgIds != null)
			{
				fixed (int* rgIdsPtr = &rgIds[0])
				{
					try
					{
						num = Module.DeleteDeviceSyncRules(ruleType, m_iDeviceID, mediaType, rgIdsPtr, rgIds.Length, fDeviceFolderIds);
					}
					catch
					{
						//try-fault
						rgIds = null;
						throw;
					}
				}
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 60uL)) & 0x200u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 57uL) >= 5u)
			{
				Module.WPP_SF_d(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 48uL), 29, (_GUID*)Unsafe.AsPointer(ref Module._003FA0x09bc4a52_002EWPP_SyncRulesAPI_cpp_Traceguids), num);
			}
			return num;
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				try
				{
					_007ESyncRules();
				}
				finally
				{
					((IDisposable)m_spEndpointHost).Dispose();
				}
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
