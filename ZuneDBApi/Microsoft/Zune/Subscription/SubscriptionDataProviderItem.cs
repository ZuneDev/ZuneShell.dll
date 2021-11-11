using System;
using System.Runtime.CompilerServices;
using _003CCppImplementationDetails_003E;
using Microsoft.Iris;

namespace Microsoft.Zune.Subscription
{
	public class SubscriptionDataProviderItem : DataProviderObject
	{
		private string m_feedUrl;

		private int m_nSeriesId = -1;

		private int m_nEpisodeId = -1;

		private EItemDownloadState m_eLastDownloadState = EItemDownloadState.eDownloadStateNone;

		private unsafe IMSMediaSchemaPropertySet* m_pEpisodePropertySet;

		public unsafe SubscriptionDataProviderItem(DataProviderQuery owner, object typeCookie, string feedUrl, IMSMediaSchemaPropertySet* pEpisodePropertySet)
			: base(owner, typeCookie)
		{
			//IL_0039: Expected I, but got I8
			m_feedUrl = feedUrl;
			m_pEpisodePropertySet = pEpisodePropertySet;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pEpisodePropertySet + 8)))((nint)pEpisodePropertySet);
			BindToLocalEpisode();
		}

		public unsafe override object GetProperty(string propertyName)
		{
			//IL_00b6: Expected I4, but got I8
			//IL_024a: Expected I, but got I8
			//IL_0256: Expected I, but got I8
			//IL_0286: Expected I, but got I8
			_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040 _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040;
			*(long*)(&_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040) = (nint)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003F_003F_C_0040_1M_0040MNHBCACD_0040_003F_0024AAT_003F_0024AAi_003F_0024AAt_003F_0024AAl_003F_0024AAe_003F_0024AA_003F_0024AA_0040);
			System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 8)) = 16797697;
			System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 16)) = (nint)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003F_003F_C_0040_1BI_0040DLMANABL_0040_003F_0024AAD_003F_0024AAe_003F_0024AAs_003F_0024AAc_003F_0024AAr_003F_0024AAi_003F_0024AAp_003F_0024AAt_003F_0024AAi_003F_0024AAo_003F_0024AAn_003F_0024AA_003F_0024AA_0040);
			System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 24)) = 134238211;
			System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 32)) = (nint)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003F_003F_C_0040_1BC_0040CGFFANJJ_0040_003F_0024AAD_003F_0024AAu_003F_0024AAr_003F_0024AAa_003F_0024AAt_003F_0024AAi_003F_0024AAo_003F_0024AAn_003F_0024AA_003F_0024AA_0040);
			System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 40)) = 16797701;
			System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 48)) = (nint)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003F_003F_C_0040_1BI_0040IMGEIAFE_0040_003F_0024AAR_003F_0024AAe_003F_0024AAl_003F_0024AAe_003F_0024AAa_003F_0024AAs_003F_0024AAe_003F_0024AAD_003F_0024AAa_003F_0024AAt_003F_0024AAe_003F_0024AA_003F_0024AA_0040);
			System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 56)) = 150995204;
			System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 64)) = (nint)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003F_003F_C_0040_1BC_0040BFOBHOBE_0040_003F_0024AAE_003F_0024AAx_003F_0024AAp_003F_0024AAl_003F_0024AAi_003F_0024AAc_003F_0024AAi_003F_0024AAt_003F_0024AA_003F_0024AA_0040);
			System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 72)) = 83906566;
			System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 80)) = (nint)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003F_003F_C_0040_1O_0040IKCDCNCP_0040_003F_0024AAA_003F_0024AAu_003F_0024AAt_003F_0024AAh_003F_0024AAo_003F_0024AAr_003F_0024AA_003F_0024AA_0040);
			System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 88)) = 16797704;
			System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 96)) = (nint)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003F_003F_C_0040_1BK_0040DIFENCED_0040_003F_0024AAE_003F_0024AAn_003F_0024AAc_003F_0024AAl_003F_0024AAo_003F_0024AAs_003F_0024AAu_003F_0024AAr_003F_0024AAe_003F_0024AAU_003F_0024AAr_003F_0024AAl_003F_0024AA_003F_0024AA_0040);
			System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 104)) = 134238215;
			System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 112)) = (nint)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003F_003F_C_0040_1CC_0040PFACMMFM_0040_003F_0024AAE_003F_0024AAp_003F_0024AAi_003F_0024AAs_003F_0024AAo_003F_0024AAd_003F_0024AAe_003F_0024AAM_003F_0024AAe_003F_0024AAd_003F_0024AAi_003F_0024AAa_003F_0024AAT_003F_0024AAy_003F_0024AAp_003F_0024AAe_003F_0024AA_003F_0024AA_0040);
			System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 120)) = 100683786;
			int num = 0;
			CComPropVariant cComPropVariant;
			// IL initblk instruction
			System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant, 0, 24);
			object result;
			try
			{
				fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref _003CModule_003E.PtrToStringChars(propertyName)))
				{
					if (_003CModule_003E._wcsicmp(ptr, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003F_003F_C_0040_1BE_0040NCILDCLH_0040_003F_0024AAL_003F_0024AAi_003F_0024AAb_003F_0024AAr_003F_0024AAa_003F_0024AAr_003F_0024AAy_003F_0024AAI_003F_0024AAd_003F_0024AA_003F_0024AA_0040)) == 0)
					{
						System.Runtime.CompilerServices.Unsafe.As<CComPropVariant, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cComPropVariant, 8)) = m_nEpisodeId;
						*(short*)(&cComPropVariant) = 3;
					}
					else if (_003CModule_003E._wcsicmp(ptr, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003F_003F_C_0040_1BC_0040JCKMNCPP_0040_003F_0024AAS_003F_0024AAe_003F_0024AAr_003F_0024AAi_003F_0024AAe_003F_0024AAs_003F_0024AAI_003F_0024AAd_003F_0024AA_003F_0024AA_0040)) == 0)
					{
						System.Runtime.CompilerServices.Unsafe.As<CComPropVariant, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cComPropVariant, 8)) = m_nSeriesId;
						*(short*)(&cComPropVariant) = 3;
					}
					else
					{
						if (_003CModule_003E._wcsicmp(ptr, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003F_003F_C_0040_1BM_0040BGELMDBM_0040_003F_0024AAD_003F_0024AAo_003F_0024AAw_003F_0024AAn_003F_0024AAl_003F_0024AAo_003F_0024AAa_003F_0024AAd_003F_0024AAS_003F_0024AAt_003F_0024AAa_003F_0024AAt_003F_0024AAe_003F_0024AA_003F_0024AA_0040)) == 0)
						{
							int nEpisodeId = m_nEpisodeId;
							if (nEpisodeId >= 0)
							{
								num = GetDatabaseValue(nEpisodeId, 145u, (tagPROPVARIANT*)(&cComPropVariant));
								if (num < 0)
								{
									goto IL_02b0;
								}
								if (m_eLastDownloadState != (EItemDownloadState)System.Runtime.CompilerServices.Unsafe.As<CComPropVariant, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cComPropVariant, 8)))
								{
									InvalidateDownloadState();
								}
								m_eLastDownloadState = System.Runtime.CompilerServices.Unsafe.As<CComPropVariant, EItemDownloadState>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cComPropVariant, 8));
							}
							else
							{
								System.Runtime.CompilerServices.Unsafe.As<CComPropVariant, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cComPropVariant, 8)) = 0;
								*(short*)(&cComPropVariant) = 3;
							}
						}
						else if (_003CModule_003E._wcsicmp(ptr, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003F_003F_C_0040_1BK_0040EPGEAFPP_0040_003F_0024AAD_003F_0024AAo_003F_0024AAw_003F_0024AAn_003F_0024AAl_003F_0024AAo_003F_0024AAa_003F_0024AAd_003F_0024AAT_003F_0024AAy_003F_0024AAp_003F_0024AAe_003F_0024AA_003F_0024AA_0040)) == 0)
						{
							int nEpisodeId2 = m_nEpisodeId;
							if (nEpisodeId2 >= 0)
							{
								num = GetDatabaseValue(nEpisodeId2, 146u, (tagPROPVARIANT*)(&cComPropVariant));
							}
							else
							{
								System.Runtime.CompilerServices.Unsafe.As<CComPropVariant, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cComPropVariant, 8)) = 0;
								*(short*)(&cComPropVariant) = 3;
							}
						}
						else if (_003CModule_003E._wcsicmp(ptr, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003F_003F_C_0040_1BK_0040JJCAOHOO_0040_003F_0024AAP_003F_0024AAl_003F_0024AAa_003F_0024AAy_003F_0024AAe_003F_0024AAd_003F_0024AAS_003F_0024AAt_003F_0024AAa_003F_0024AAt_003F_0024AAu_003F_0024AAs_003F_0024AA_003F_0024AA_0040)) == 0)
						{
							*(short*)(&cComPropVariant) = 1;
							int nEpisodeId3 = m_nEpisodeId;
							if (nEpisodeId3 < 0)
							{
								goto IL_028b;
							}
							num = GetDatabaseValue(nEpisodeId3, 262u, (tagPROPVARIANT*)(&cComPropVariant));
						}
						else if (_003CModule_003E._wcsicmp(ptr, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003F_003F_C_0040_1BE_0040HHOJDPEL_0040_003F_0024AAS_003F_0024AAo_003F_0024AAu_003F_0024AAr_003F_0024AAc_003F_0024AAe_003F_0024AAU_003F_0024AAr_003F_0024AAl_003F_0024AA_003F_0024AA_0040)) == 0)
						{
							*(short*)(&cComPropVariant) = 1;
							int nEpisodeId4 = m_nEpisodeId;
							if (nEpisodeId4 < 0)
							{
								goto IL_028b;
							}
							num = GetDatabaseValue(nEpisodeId4, 317u, (tagPROPVARIANT*)(&cComPropVariant));
						}
						else if (_003CModule_003E._wcsicmp(ptr, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003F_003F_C_0040_1CE_0040ELHJHONN_0040_003F_0024AAD_003F_0024AAo_003F_0024AAw_003F_0024AAn_003F_0024AAl_003F_0024AAo_003F_0024AAa_003F_0024AAd_003F_0024AAE_003F_0024AAr_003F_0024AAr_003F_0024AAo_003F_0024AAr_003F_0024AAC_003F_0024AAo_003F_0024AAd_003F_0024AAe_003F_0024AA_003F_0024AA_0040)) == 0)
						{
							*(short*)(&cComPropVariant) = 1;
							int nEpisodeId5 = m_nEpisodeId;
							if (nEpisodeId5 < 0)
							{
								goto IL_028b;
							}
							num = GetDatabaseValue(nEpisodeId5, 144u, (tagPROPVARIANT*)(&cComPropVariant));
						}
						else
						{
							int num2 = 0;
							PROPERTY_TO_PID_MAP* ptr2 = (PROPERTY_TO_PID_MAP*)(&_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040);
							while (_003CModule_003E._wcsicmp(ptr, (ushort*)(*(ulong*)ptr2)) != 0)
							{
								num2++;
								ptr2 = (PROPERTY_TO_PID_MAP*)((ulong)(nint)ptr2 + 16uL);
								if ((uint)num2 < 8u)
								{
									continue;
								}
								goto IL_028b;
							}
							uint num3 = *(uint*)((long)num2 * 16L + (ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 8)));
							IMSMediaSchemaPropertySet* pEpisodePropertySet = m_pEpisodePropertySet;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, uint, tagPROPVARIANT*, int>)(*(ulong*)(*(long*)pEpisodePropertySet + 48)))((nint)pEpisodePropertySet, num3, 0u, (tagPROPVARIANT*)(&cComPropVariant));
						}
						if (num < 0)
						{
							goto IL_02b0;
						}
					}
					goto IL_028b;
					IL_02b0:
					result = base.Mappings[propertyName].DefaultValue;
					goto end_IL_00c8;
					IL_028b:
					if (_003CModule_003E.CComPropVariant_002EIsNullOrEmpty(&cComPropVariant))
					{
						goto IL_02b0;
					}
					result = SubscriptionDataProviderQueryResult.ConvertVariantToType(base.Mappings[propertyName].PropertyTypeName, &cComPropVariant);
					end_IL_00c8:;
				}
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

		public unsafe override void SetProperty(string propertyName, object value)
		{
			//IL_0046: Expected I, but got I8
			if (m_nEpisodeId > 0 && propertyName == "DownloadErrorCode")
			{
				tagVARIANT tagVARIANT;
				*(short*)(&tagVARIANT) = 3;
				System.Runtime.CompilerServices.Unsafe.As<tagVARIANT, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagVARIANT, 8)) = (int)value;
				DBPropertySubmitStruct dBPropertySubmitStruct;
				*(int*)(&dBPropertySubmitStruct) = 144;
				System.Runtime.CompilerServices.Unsafe.As<DBPropertySubmitStruct, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref dBPropertySubmitStruct, 8)) = (nint)(&tagVARIANT);
				int num = _003CModule_003E.ZuneLibraryExports_002ESetFieldValues(m_nEpisodeId, EListType.ePodcastEpisodeList, 1, &dBPropertySubmitStruct, null);
				if (0 == num)
				{
					FirePropertyChanged(propertyName);
				}
			}
		}

		public unsafe virtual void SaveToLibrary()
		{
			//IL_0003: Expected I, but got I8
			//IL_0006: Expected I, but got I8
			//IL_0033: Expected I, but got I8
			//IL_0033: Expected I, but got I8
			//IL_0067: Expected I, but got I8
			//IL_0098: Expected I, but got I8
			//IL_009c: Expected I, but got I8
			//IL_00ae: Expected I, but got I8
			IService* ptr = null;
			ISubscriptionManager* ptr2 = null;
			int num = 1;
			int nSeriesId = -1;
			int nEpisodeId = -1;
			int singleton = _003CModule_003E.GetSingleton((_GUID)_003CModule_003E._GUID_bb2d1edd_1bd5_4be1_8d38_36d4f0849911, (void**)(&ptr));
			if (singleton >= 0)
			{
				IService* intPtr = ptr;
				singleton = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, int*, int>)(*(ulong*)(*(long*)ptr + 384)))((nint)intPtr, null, &num);
				if (singleton >= 0)
				{
					singleton = _003CModule_003E.GetSingleton((_GUID)_003CModule_003E._GUID_9dc7c984_41d5_4130_a5ac_46d0825cd29d, (void**)(&ptr2));
					if (singleton >= 0)
					{
						ISubscriptionManager* intPtr2 = ptr2;
						int num2 = num;
						IMSMediaSchemaPropertySet* pEpisodePropertySet = m_pEpisodePropertySet;
						singleton = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, EMediaTypes, IMSMediaSchemaPropertySet*, int*, int*, int>)(*(ulong*)(*(long*)ptr2 + 136)))((nint)intPtr2, num2, EMediaTypes.eMediaTypePodcastSeries, pEpisodePropertySet, &nSeriesId, &nEpisodeId);
						if (singleton >= 0)
						{
							m_nSeriesId = nSeriesId;
							m_nEpisodeId = nEpisodeId;
							FirePropertyChanged("DownloadState");
						}
					}
				}
			}
			if (0L != (nint)ptr)
			{
				IService* intPtr3 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr3 + 16)))((nint)intPtr3);
				ptr = null;
			}
			if (0L != (nint)ptr2)
			{
				ISubscriptionManager* intPtr4 = ptr2;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr4 + 16)))((nint)intPtr4);
			}
		}

		internal unsafe void OnDispose()
		{
			//IL_0019: Expected I, but got I8
			//IL_0022: Expected I, but got I8
			IMSMediaSchemaPropertySet* pEpisodePropertySet = m_pEpisodePropertySet;
			if (0L != (nint)pEpisodePropertySet)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pEpisodePropertySet + 16)))((nint)pEpisodePropertySet);
				m_pEpisodePropertySet = null;
			}
		}

		private unsafe int BindToLocalEpisode()
		{
			//IL_0003: Expected I, but got I8
			//IL_000b: Expected I4, but got I8
			//IL_0013: Expected I4, but got I8
			//IL_0050: Expected I, but got I8
			//IL_0071: Expected I, but got I8
			//IL_009a: Expected I, but got I8
			//IL_009a: Expected I, but got I8
			//IL_009a: Expected I, but got I8
			//IL_00a7: Expected I4, but got I8
			//IL_00e2: Expected I, but got I8
			//IL_00e6: Expected I, but got I8
			ISubscriptionManager* ptr = null;
			CComPropVariant cComPropVariant;
			// IL initblk instruction
			System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant, 0, 24);
			int num2;
			try
			{
				CComPropVariant cComPropVariant2;
				// IL initblk instruction
				System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant2, 0, 24);
				try
				{
					int num = -1;
					fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref _003CModule_003E.PtrToStringChars(m_feedUrl)))
					{
						num2 = _003CModule_003E.GetSingleton((_GUID)_003CModule_003E._GUID_9dc7c984_41d5_4130_a5ac_46d0825cd29d, (void**)(&ptr));
						if (num2 >= 0)
						{
							IMSMediaSchemaPropertySet* pEpisodePropertySet = m_pEpisodePropertySet;
							num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, uint, tagPROPVARIANT*, int>)(*(ulong*)(*(long*)pEpisodePropertySet + 48)))((nint)pEpisodePropertySet, 16797697u, 0u, (tagPROPVARIANT*)(&cComPropVariant));
							if (num2 >= 0)
							{
								IMSMediaSchemaPropertySet* pEpisodePropertySet2 = m_pEpisodePropertySet;
								num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, uint, tagPROPVARIANT*, int>)(*(ulong*)(*(long*)pEpisodePropertySet2 + 48)))((nint)pEpisodePropertySet2, 134238215u, 0u, (tagPROPVARIANT*)(&cComPropVariant2));
								if (num2 >= 0)
								{
									long num3 = *(long*)ptr + 160;
									ISubscriptionManager* intPtr = ptr;
									_003F val = ptr2;
									long num4 = System.Runtime.CompilerServices.Unsafe.As<CComPropVariant, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cComPropVariant, 8));
									long num5 = System.Runtime.CompilerServices.Unsafe.As<CComPropVariant, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cComPropVariant2, 8));
									num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaTypes, ushort*, ushort*, ushort*, int*, int>)(*(ulong*)num3))((nint)intPtr, EMediaTypes.eMediaTypePodcastSeries, (ushort*)(nint)val, (ushort*)num4, (ushort*)num5, &num);
								}
							}
						}
						if (0 == num2)
						{
							tagPROPVARIANT tagPROPVARIANT;
							// IL initblk instruction
							System.Runtime.CompilerServices.Unsafe.InitBlock(ref tagPROPVARIANT, 0, 24);
							num2 = GetDatabaseValue(num, 311u, &tagPROPVARIANT);
							if (num2 >= 0)
							{
								m_nEpisodeId = num;
								m_nSeriesId = System.Runtime.CompilerServices.Unsafe.As<tagPROPVARIANT, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagPROPVARIANT, 8));
							}
						}
						if (0L != (nint)ptr)
						{
							ISubscriptionManager* intPtr2 = ptr;
							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
							ptr = null;
						}
					}
				}
				catch
				{
					//try-fault
					_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant2);
					throw;
				}
				_003CModule_003E.CComPropVariant_002EClear(&cComPropVariant2);
			}
			catch
			{
				//try-fault
				_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
				throw;
			}
			_003CModule_003E.CComPropVariant_002EClear(&cComPropVariant);
			return num2;
		}

		private int InvalidateDownloadState()
		{
			FirePropertyChanged("DownloadState");
			FirePropertyChanged("DownloadType");
			FirePropertyChanged("DownloadErrorCode");
			return 0;
		}

		private unsafe int GetDatabaseValue(int nMediaId, uint dwAtom, tagPROPVARIANT* pvarValue)
		{
			//IL_001b: Expected I, but got I8
			//IL_0042: Expected I, but got I8
			//IL_0042: Expected I, but got I8
			//IL_0046: Expected I, but got I8
			//IL_006e: Expected I, but got I8
			//IL_00b0: Expected I, but got I8
			//IL_00b4: Expected I, but got I8
			//IL_00c8: Expected I, but got I8
			//IL_00cc: Expected I, but got I8
			if (pvarValue == null)
			{
				_003CModule_003E._ZuneShipAssert(1001u, 633u);
				return -2147467261;
			}
			IService* ptr = null;
			int num = 1;
			int num2 = _003CModule_003E.GetSingleton((_GUID)_003CModule_003E._GUID_bb2d1edd_1bd5_4be1_8d38_36d4f0849911, (void**)(&ptr));
			if (num2 >= 0)
			{
				IService* intPtr = ptr;
				num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, int*, int>)(*(ulong*)(*(long*)ptr + 384)))((nint)intPtr, null, &num);
			}
			IQueryPropertyBag* ptr2 = null;
			DBPropertyRequestStruct dBPropertyRequestStruct;
			_003CModule_003E.DBPropertyRequestStruct_002E_007Bctor_007D(&dBPropertyRequestStruct, dwAtom);
			try
			{
				if (num2 >= 0)
				{
					num2 = _003CModule_003E.ZuneLibraryExports_002ECreatePropertyBag(&ptr2);
					if (num2 >= 0)
					{
						IQueryPropertyBag* intPtr2 = ptr2;
						int num3 = num;
						num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)ptr2 + 56)))((nint)intPtr2, (EQueryPropertyBagProp)0, num3);
						if (num2 >= 0)
						{
							num2 = _003CModule_003E.ZuneLibraryExports_002EGetFieldValues(nMediaId, EListType.ePodcastEpisodeList, 1, &dBPropertyRequestStruct, ptr2);
							if (num2 >= 0)
							{
								num2 = ((System.Runtime.CompilerServices.Unsafe.As<DBPropertyRequestStruct, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref dBPropertyRequestStruct, 4)) < 0) ? System.Runtime.CompilerServices.Unsafe.As<DBPropertyRequestStruct, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref dBPropertyRequestStruct, 4)) : _003CModule_003E.PropVariantCopy(pvarValue, (tagPROPVARIANT*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref dBPropertyRequestStruct, 8))));
							}
						}
					}
				}
				if (0L != (nint)ptr2)
				{
					IQueryPropertyBag* intPtr3 = ptr2;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr3 + 16)))((nint)intPtr3);
					ptr2 = null;
				}
				if (0L != (nint)ptr)
				{
					IService* intPtr4 = ptr;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr4 + 16)))((nint)intPtr4);
					ptr = null;
				}
			}
			catch
			{
				//try-fault
				_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<DBPropertyRequestStruct*, void>)(&_003CModule_003E.DBPropertyRequestStruct_002E_007Bdtor_007D), &dBPropertyRequestStruct);
				throw;
			}
			_003CModule_003E.DBPropertyRequestStruct_002E_007Bdtor_007D(&dBPropertyRequestStruct);
			return num2;
		}
	}
}
