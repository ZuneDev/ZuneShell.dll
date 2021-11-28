using System;
using System.Runtime.CompilerServices;
using Microsoft.Iris;

namespace Microsoft.Zune.Subscription
{
	public class SubscriptionSeriesInfo : DataProviderObject
	{
		private string m_serviceId;

		private ESubscriptionState m_eSubscriptionState;

		private unsafe IMSMediaSchemaPropertySet* m_pSeriesPropertySet;

		public unsafe SubscriptionSeriesInfo(DataProviderQuery owner, object typeCookie, string serviceId)
			: base(owner, typeCookie)
		{
			//IL_000f: Expected I, but got I8
			m_serviceId = serviceId;
			m_pSeriesPropertySet = null;
		}

		public unsafe override object GetProperty(string propertyName)
		{
			//IL_00ec: Expected I4, but got I8
			//IL_0138: Expected I, but got I8
			//IL_0144: Expected I, but got I8
			//IL_0175: Expected I, but got I8
			_0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040 _0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040;
			*(long*)(&_0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040) = (nint)Unsafe.AsPointer(ref Module.1BA_0040CEPEGMDB__003F_0024AAF_003F_0024AAe_003F_0024AAe_003F_0024AAd_003F_0024AAU_003F_0024AAr_003F_0024AAl);
            Unsafe.As<_0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 8)) = 134217987;
            Unsafe.As<_0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 16)) = (nint)Unsafe.AsPointer(ref Module.1BE_0040ILMBBNLH__003F_0024AAE_003F_0024AAr_003F_0024AAr_003F_0024AAo_003F_0024AAr_003F_0024AAC_003F_0024AAo_003F_0024AAd_003F_0024AAe);
            Unsafe.As<_0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 24)) = 100663562;
            Unsafe.As<_0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 32)) = (nint)Unsafe.AsPointer(ref Module.1M_0040MNHBCACD__003F_0024AAT_003F_0024AAi_003F_0024AAt_003F_0024AAl_003F_0024AAe);
            Unsafe.As<_0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 40)) = 16801793;
            Unsafe.As<_0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 48)) = (nint)Unsafe.AsPointer(ref Module.1BA_0040DPIPFNMB__003F_0024AAH_003F_0024AAo_003F_0024AAm_003F_0024AAe_003F_0024AAU_003F_0024AAr_003F_0024AAl);
            Unsafe.As<_0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 56)) = 134242316;
            Unsafe.As<_0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 64)) = (nint)Unsafe.AsPointer(ref Module.1O_0040OPPNLDOF__003F_0024AAA_003F_0024AAr_003F_0024AAt_003F_0024AAU_003F_0024AAr_003F_0024AAl);
            Unsafe.As<_0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 72)) = 134242317;
            Unsafe.As<_0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 80)) = (nint)Unsafe.AsPointer(ref Module.1BI_0040DLMANABL__003F_0024AAD_003F_0024AAe_003F_0024AAs_003F_0024AAc_003F_0024AAr_003F_0024AAi_003F_0024AAp_003F_0024AAt_003F_0024AAi_003F_0024AAo_003F_0024AAn);
            Unsafe.As<_0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 88)) = 134242306;
            Unsafe.As<_0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 96)) = (nint)Unsafe.AsPointer(ref Module.1BC_0040BFOBHOBE__003F_0024AAE_003F_0024AAx_003F_0024AAp_003F_0024AAl_003F_0024AAi_003F_0024AAc_003F_0024AAi_003F_0024AAt);
            Unsafe.As<_0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 104)) = 83910662;
            Unsafe.As<_0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 112)) = (nint)Unsafe.AsPointer(ref Module.1BE_0040NPMHDJAF__003F_0024AAC_003F_0024AAo_003F_0024AAp_003F_0024AAy_003F_0024AAr_003F_0024AAi_003F_0024AAg_003F_0024AAh_003F_0024AAt);
            Unsafe.As<_0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 120)) = 16801801;
            Unsafe.As<_0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 128)) = (nint)Unsafe.AsPointer(ref Module.1O_0040IKCDCNCP__003F_0024AAA_003F_0024AAu_003F_0024AAt_003F_0024AAh_003F_0024AAo_003F_0024AAr);
            Unsafe.As<_0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 136)) = 16801796;
            Unsafe.As<_0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 144)) = (nint)Unsafe.AsPointer(ref Module.1BE_0040GMGHCDOJ__003F_0024AAO_003F_0024AAw_003F_0024AAn_003F_0024AAe_003F_0024AAr_003F_0024AAN_003F_0024AAa_003F_0024AAm_003F_0024AAe);
            Unsafe.As<_0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 152)) = 16801799;
            PROPVARIANT cComPropVariant = new();
			object result;
			try
			{
				fixed (char* propertyNamePtr = propertyName.ToCharArray())
				{
					ushort* ptr = (ushort*)propertyNamePtr;
					if (Module._wcsicmp(ptr, (ushort*)Unsafe.AsPointer(ref Module.1BE_0040NCILDCLH_0040_003F_0024AAL_003F_0024AAi_003F_0024AAb_003F_0024AAr_003F_0024AAa_003F_0024AAr_003F_0024AAy_003F_0024AAI_003F_0024AAd_003F_0024AA_003F_0024AA_)) != 0 && Module._wcsicmp(ptr, (ushort*)Unsafe.AsPointer(ref Module._003F_003F_C_0040_1BI_0040KPFACBGA_0040_003F_0024AAS_003F_0024AAe_003F_0024AAr_003F_0024AAi_003F_0024AAe_003F_0024AAs_003F_0024AAS_003F_0024AAt_003F_0024AAa_003F_0024AAt_003F_0024AAe_003F_0024AA_003F_0024AA_0040)) != 0 && Module._wcsicmp(ptr, (ushort*)Unsafe.AsPointer(ref Module._003F_003F_C_0040_1CC_0040GKFOINAA_0040_003F_0024AAN_003F_0024AAu_003F_0024AAm_003F_0024AAb_003F_0024AAe_003F_0024AAr_003F_0024AAO_003F_0024AAf_003F_0024AAE_003F_0024AAp_003F_0024AAi_003F_0024AAs_003F_0024AAo_003F_0024AAd_003F_0024AAe_003F_0024AAs)) != 0 && m_pSeriesPropertySet != null)
					{
						int num = 0;
						PROPERTY_TO_PID_MAP* ptr2 = (PROPERTY_TO_PID_MAP*)(&_0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040);
						while (true)
						{
							if (Module._wcsicmp(ptr, (ushort*)(*(ulong*)ptr2)) != 0)
							{
								num++;
								ptr2 = (PROPERTY_TO_PID_MAP*)((ulong)(nint)ptr2 + 16uL);
								if ((uint)num >= 10u)
								{
									break;
								}
								continue;
							}
							uint num2 = *(uint*)((long)num * 16L + _0024ArrayType_0024_0024_0024BY09UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040 + 8);
							IMSMediaSchemaPropertySet* pSeriesPropertySet = m_pSeriesPropertySet;
							if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, uint, PROPVARIANT, int>)(*(ulong*)(*(long*)pSeriesPropertySet + 48)))((nint)pSeriesPropertySet, (uint)num2, (uint)0u, (PROPVARIANT)cComPropVariant) < 0 || *(ushort*)(&cComPropVariant) == 0)
							{
								break;
							}
							result = SubscriptionDataProviderQueryResult.ConvertVariantToType(Mappings[propertyName].PropertyTypeName, &cComPropVariant);
							goto end_IL_00fe;
						}
					}
					result = Mappings[propertyName].DefaultValue;
					end_IL_00fe:;
				}
			}
			catch
			{
                //try-fault
                Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<PROPVARIANT*, void>)(&Module.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
				throw;
			}
			cComPropVariant.Clear();
			return result;
		}

		public override void SetProperty(string propertyName, object value)
		{
			//Discarded unreachable code: IL_0006
			throw new NotSupportedException();
		}

		internal unsafe void OnDispose()
		{
			//IL_0019: Expected I, but got I8
			//IL_0022: Expected I, but got I8
			IMSMediaSchemaPropertySet* pSeriesPropertySet = m_pSeriesPropertySet;
			if (0L != (nint)pSeriesPropertySet)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pSeriesPropertySet + 16)))((nint)pSeriesPropertySet);
				m_pSeriesPropertySet = null;
			}
		}

		internal unsafe void SetPropertySet(IMSMediaSchemaPropertySet* pSeriesPropertySet)
		{
            //IL_0008: Expected I4, but got I8
            //IL_0025: Expected I, but got I8
            //IL_0050: Expected I, but got I8
            //IL_0059: Expected I, but got I8
            //IL_006c: Expected I, but got I8
            //IL_009a: Expected I4, but got I8
            //IL_00c8: Expected I, but got I8
            PROPVARIANT cComPropVariant = new();
			try
			{
				if (pSeriesPropertySet != null && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, uint, PROPVARIANT, int>)(*(ulong*)(*(long*)pSeriesPropertySet + 48)))((nint)pSeriesPropertySet, (uint)(uint)16801793u, (uint)(uint)0u, (PROPVARIANT)(PROPVARIANT)cComPropVariant) >= 0 && *(ushort*)(&cComPropVariant) != 0)
				{
                    IMSMediaSchemaPropertySet* pSeriesPropertySet2 = m_pSeriesPropertySet;
					if (0L != (nint)pSeriesPropertySet2)
					{
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pSeriesPropertySet2 + 16)))((nint)pSeriesPropertySet2);
                        m_pSeriesPropertySet = null;
					}
                    m_pSeriesPropertySet = pSeriesPropertySet;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pSeriesPropertySet + 8)))((nint)pSeriesPropertySet);
					if (!string.IsNullOrEmpty(m_serviceId))
					{
                        Guid guid = new(m_serviceId);
                        PROPVARIANT tagPROPVARIANT;
						*(short*)(&tagPROPVARIANT) = 72;
                        Unsafe.As<PROPVARIANT, long>(ref Unsafe.AddByteOffset(ref tagPROPVARIANT, 8)) = (nint)(&guid);
                        IMSMediaSchemaPropertySet* pSeriesPropertySet3 = m_pSeriesPropertySet;
                        PROPVARIANT tagPROPVARIANT2 = tagPROPVARIANT;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, PROPVARIANT, int>)(*(ulong*)(*(long*)pSeriesPropertySet3 + 56)))((nint)pSeriesPropertySet3, (uint)(uint)67133455u, (PROPVARIANT)(PROPVARIANT)tagPROPVARIANT2);
					}
				}
			}
			catch
			{
                //try-fault
                Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<PROPVARIANT*, void>)(&Module.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
				throw;
			}
			cComPropVariant.Clear();
		}
	}
}
