using System;
using System.Runtime.CompilerServices;
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
            *(long*)(&_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040) = (nint)Unsafe.AsPointer(ref Module.1M_0040MNHBCACD_Title);
            Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 8)) = 16797697;
            Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 16)) = (nint)Unsafe.AsPointer(ref Module.1BI_0040DLMANABL_Description);
            Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 24)) = 134238211;
            Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 32)) = (nint)Unsafe.AsPointer(ref Module.1BC_0040CGFFANJJ_Duration);
            Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 40)) = 16797701;
            Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 48)) = (nint)Unsafe.AsPointer(ref Module.1BI_0040IMGEIAFE_ReleaseDate);
            Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 56)) = 150995204;
            Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 64)) = (nint)Unsafe.AsPointer(ref Module.1BC_0040BFOBHOBE_Explicit);
            Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 72)) = 83906566;
            Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 80)) = (nint)Unsafe.AsPointer(ref Module.1O_0040IKCDCNCP_Author);
            Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 88)) = 16797704;
            Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 96)) = (nint)Unsafe.AsPointer(ref Module.1BK_0040DIFENCED_EnclosureUrl);
            Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 104)) = 134238215;
            Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, long>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 112)) = (nint)Unsafe.AsPointer(ref Module.1CC_0040PFACMMFM_EpisodeMediaType);
            Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, 120)) = 100683786;
            int num = 0;
            PROPVARIANT cComPropVariant = new();
            object result;
            try
            {
                fixed (char* propertyNamePtr = propertyName.ToCharArray())
                {
                    ushort* ptr = (ushort*)propertyNamePtr;
                    if (Module._wcsicmp(ptr, (ushort*)Unsafe.AsPointer(ref Module.1BE_0040NCILDCLH_LibraryId)) == 0)
                    {
                        Unsafe.As<PROPVARIANT, int>(ref Unsafe.AddByteOffset(ref cComPropVariant, 8)) = m_nEpisodeId;
                        *(short*)(&cComPropVariant) = 3;
                    }
                    else if (Module._wcsicmp(ptr, (ushort*)Unsafe.AsPointer(ref Module.1BC_0040JCKMNCPP_SeriesId)) == 0)
                    {
                        Unsafe.As<PROPVARIANT, int>(ref Unsafe.AddByteOffset(ref cComPropVariant, 8)) = m_nSeriesId;
                        *(short*)(&cComPropVariant) = 3;
                    }
                    else
                    {
                        if (Module._wcsicmp(ptr, (ushort*)Unsafe.AsPointer(ref Module.1BM_0040BGELMDBM_DownloadState)) == 0)
                        {
                            int nEpisodeId = m_nEpisodeId;
                            if (nEpisodeId >= 0)
                            {
                                num = GetDatabaseValue(nEpisodeId, 145u, cComPropVariant);
                                if (num < 0)
                                {
                                    goto IL_02b0;
                                }
                                if (m_eLastDownloadState != (EItemDownloadState)Unsafe.As<PROPVARIANT, int>(ref Unsafe.AddByteOffset(ref cComPropVariant, 8)))
                                {
                                    InvalidateDownloadState();
                                }
                                m_eLastDownloadState = Unsafe.As<PROPVARIANT, EItemDownloadState>(ref Unsafe.AddByteOffset(ref cComPropVariant, 8));
                            }
                            else
                            {
                                Unsafe.As<PROPVARIANT, int>(ref Unsafe.AddByteOffset(ref cComPropVariant, 8)) = 0;
                                *(short*)(&cComPropVariant) = 3;
                            }
                        }
                        else if (Module._wcsicmp(ptr, (ushort*)Unsafe.AsPointer(ref Module.1BK_0040EPGEAFPP_DownloadType)) == 0)
                        {
                            int nEpisodeId2 = m_nEpisodeId;
                            if (nEpisodeId2 >= 0)
                            {
                                num = GetDatabaseValue(nEpisodeId2, 146u, cComPropVariant);
                            }
                            else
                            {
                                Unsafe.As<PROPVARIANT, int>(ref Unsafe.AddByteOffset(ref cComPropVariant, 8)) = 0;
                                *(short*)(&cComPropVariant) = 3;
                            }
                        }
                        else if (Module._wcsicmp(ptr, (ushort*)Unsafe.AsPointer(ref Module.1BK_0040JJCAOHOO_PlayedStatus)) == 0)
                        {
                            *(short*)(&cComPropVariant) = 1;
                            int nEpisodeId3 = m_nEpisodeId;
                            if (nEpisodeId3 < 0)
                            {
                                goto IL_028b;
                            }
                            num = GetDatabaseValue(nEpisodeId3, 262u, cComPropVariant);
                        }
                        else if (Module._wcsicmp(ptr, (ushort*)Unsafe.AsPointer(ref Module.1BE_0040HHOJDPEL_SourceUrl)) == 0)
                        {
                            *(short*)(&cComPropVariant) = 1;
                            int nEpisodeId4 = m_nEpisodeId;
                            if (nEpisodeId4 < 0)
                            {
                                goto IL_028b;
                            }
                            num = GetDatabaseValue(nEpisodeId4, 317u, cComPropVariant);
                        }
                        else if (Module._wcsicmp(ptr, (ushort*)Unsafe.AsPointer(ref Module.1CE_0040ELHJHONN_DownloadErrorCode)) == 0)
                        {
                            *(short*)(&cComPropVariant) = 1;
                            int nEpisodeId5 = m_nEpisodeId;
                            if (nEpisodeId5 < 0)
                            {
                                goto IL_028b;
                            }
                            num = GetDatabaseValue(nEpisodeId5, 144u, cComPropVariant);
                        }
                        else
                        {
                            int num2 = 0;
                            PROPERTY_TO_PID_MAP* ptr2 = (PROPERTY_TO_PID_MAP*)(&_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040);
                            while (Module._wcsicmp(ptr, (ushort*)(*(ulong*)ptr2)) != 0)
                            {
                                num2++;
                                ptr2 = (PROPERTY_TO_PID_MAP*)((ulong)(nint)ptr2 + 16uL);
                                if ((uint)num2 < 8u)
                                {
                                    continue;
                                }
                                goto IL_028b;
                            }
                            uint num3 = *(uint*)((long)num2 * 16L + _0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040 + 8);
                            IMSMediaSchemaPropertySet* pEpisodePropertySet = m_pEpisodePropertySet;
                            num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, uint, PROPVARIANT, int>)(*(ulong*)(*(long*)pEpisodePropertySet + 48)))((nint)pEpisodePropertySet, (uint)(uint)num3, (uint)(uint)0u, (PROPVARIANT)(PROPVARIANT)cComPropVariant);
                        }
                        if (num < 0)
                        {
                            goto IL_02b0;
                        }
                    }
                    goto IL_028b;
                IL_02b0:
                    result = Mappings[propertyName].DefaultValue;
                    goto end_IL_00c8;
                IL_028b:
                    if (Module.CComPropVariant_002EIsNullOrEmpty(&cComPropVariant))
                    {
                        goto IL_02b0;
                    }
                    result = SubscriptionDataProviderQueryResult.ConvertVariantToType(Mappings[propertyName].PropertyTypeName, &cComPropVariant);
                end_IL_00c8:;
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

        public unsafe override void SetProperty(string propertyName, object value)
        {
            //IL_0046: Expected I, but got I8
            if (m_nEpisodeId > 0 && propertyName == "DownloadErrorCode")
            {
                VARIANT tagVARIANT;
                *(short*)(&tagVARIANT) = 3;
                Unsafe.As<VARIANT, int>(ref Unsafe.AddByteOffset(ref tagVARIANT, 8)) = (int)value;
                DBPropertySubmitStruct dBPropertySubmitStruct;
                *(int*)(&dBPropertySubmitStruct) = 144;
                Unsafe.As<DBPropertySubmitStruct, long>(ref Unsafe.AddByteOffset(ref dBPropertySubmitStruct, 8)) = (nint)(&tagVARIANT);
                int num = Module.SetFieldValues(m_nEpisodeId, EListType.ePodcastEpisodeList, 1, &dBPropertySubmitStruct, null);
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
            int singleton = Module.GetSingleton(Module.GUID_IService, (void**)(&ptr));
            if (singleton >= 0)
            {
                IService* intPtr = ptr;
                singleton = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, int*, int>)(*(ulong*)(*(long*)ptr + 384)))((nint)intPtr, null, &num);
                if (singleton >= 0)
                {
                    singleton = Module.GetSingleton(Module.GUID_ISubscriptionManager, (void**)(&ptr2));
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
            PROPVARIANT cComPropVariant = new();
            int num2;
            try
            {
                PROPVARIANT cComPropVariant2 = new();
                try
                {
                    int num = -1;
                    fixed (char* m_feedUrlPtr = m_feedUrl.ToCharArray())
                    {
                        ushort* ptr2 = (ushort*)m_feedUrlPtr;
                        num2 = Module.GetSingleton(Module.GUID_ISubscriptionManager, (void**)(&ptr));
                        if (num2 >= 0)
                        {
                            IMSMediaSchemaPropertySet* pEpisodePropertySet = m_pEpisodePropertySet;
                            num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, uint, PROPVARIANT, int>)(*(ulong*)(*(long*)pEpisodePropertySet + 48)))((nint)pEpisodePropertySet, (uint)(uint)16797697u, (uint)(uint)0u, (PROPVARIANT)(PROPVARIANT)cComPropVariant);
                            if (num2 >= 0)
                            {
                                IMSMediaSchemaPropertySet* pEpisodePropertySet2 = m_pEpisodePropertySet;
                                num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, uint, PROPVARIANT, int>)(*(ulong*)(*(long*)pEpisodePropertySet2 + 48)))((nint)pEpisodePropertySet2, (uint)(uint)134238215u, (uint)(uint)0u, (PROPVARIANT)(PROPVARIANT)cComPropVariant2);
                                if (num2 >= 0)
                                {
                                    long num3 = *(long*)ptr + 160;
                                    ISubscriptionManager* intPtr = ptr;
                                    _003F val = ptr2;
                                    long num4 = Unsafe.As<PROPVARIANT, long>(ref Unsafe.AddByteOffset(ref cComPropVariant, 8));
                                    long num5 = Unsafe.As<PROPVARIANT, long>(ref Unsafe.AddByteOffset(ref cComPropVariant2, 8));
                                    num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaTypes, ushort*, ushort*, ushort*, int*, int>)(*(ulong*)num3))((nint)intPtr, EMediaTypes.eMediaTypePodcastSeries, (ushort*)(nint)val, (ushort*)num4, (ushort*)num5, &num);
                                }
                            }
                        }
                        if (0 == num2)
                        {
                            PROPVARIANT tagPROPVARIANT = new();
                            num2 = GetDatabaseValue(num, 311u, &tagPROPVARIANT);
                            if (num2 >= 0)
                            {
                                m_nEpisodeId = num;
                                m_nSeriesId = Unsafe.As<PROPVARIANT, int>(ref Unsafe.AddByteOffset(ref tagPROPVARIANT, 8));
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
            return num2;
        }

        private int InvalidateDownloadState()
        {
            FirePropertyChanged("DownloadState");
            FirePropertyChanged("DownloadType");
            FirePropertyChanged("DownloadErrorCode");
            return 0;
        }

        private unsafe int GetDatabaseValue(int nMediaId, uint dwAtom, PROPVARIANT pvarValue)
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
                Module._ZuneShipAssert(1001u, 633u);
                return -2147467261;
            }
            IService* ptr = null;
            int num = 1;
            int num2 = Module.GetSingleton(Module.GUID_IService, (void**)(&ptr));
            if (num2 >= 0)
            {
                IService* intPtr = ptr;
                num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, int*, int>)(*(ulong*)(*(long*)ptr + 384)))((nint)intPtr, null, &num);
            }
            IQueryPropertyBag* ptr2 = null;
            DBPropertyRequestStruct dBPropertyRequestStruct;
            Module.DBPropertyRequestStruct_002E_007Bctor_007D(&dBPropertyRequestStruct, dwAtom);
            try
            {
                if (num2 >= 0)
                {
                    num2 = Module.CreatePropertyBag(&ptr2);
                    if (num2 >= 0)
                    {
                        IQueryPropertyBag* intPtr2 = ptr2;
                        int num3 = num;
                        num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)ptr2 + 56)))((nint)intPtr2, 0, num3);
                        if (num2 >= 0)
                        {
                            num2 = Module.GetFieldValues(nMediaId, EListType.ePodcastEpisodeList, 1, &dBPropertyRequestStruct, ptr2);
                            if (num2 >= 0)
                            {
                                num2 = ((Unsafe.As<DBPropertyRequestStruct, int>(ref Unsafe.AddByteOffset(ref dBPropertyRequestStruct, 4)) < 0) ? Unsafe.As<DBPropertyRequestStruct, int>(ref Unsafe.AddByteOffset(ref dBPropertyRequestStruct, 4)) : Module.PropVariantCopy(pvarValue, (PROPVARIANT)Unsafe.AsPointer(ref Unsafe.AddByteOffset(ref dBPropertyRequestStruct, 8))));
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
                Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<DBPropertyRequestStruct*, void>)(&Module.DBPropertyRequestStruct_002E_007Bdtor_007D), &dBPropertyRequestStruct);
                throw;
            }
            Module.DBPropertyRequestStruct_002E_007Bdtor_007D(&dBPropertyRequestStruct);
            return num2;
        }
    }
}
