using System;
using System.Runtime.CompilerServices;
using _003CCppImplementationDetails_003E;
using Microsoft.Iris;
using ZuneDBApi.Interop;

namespace Microsoft.Zune.Subscription;

public class SubscriptionDataProviderItem : DataProviderObject
{
    private string m_feedUrl;

    private int m_nSeriesId = -1;

    private int m_nEpisodeId = -1;

    private EItemDownloadState m_eLastDownloadState = EItemDownloadState.eDownloadStateNone;

    private unsafe IMSMediaSchemaPropertySet* m_pEpisodePropertySet;

    internal unsafe SubscriptionDataProviderItem(DataProviderQuery owner, object typeCookie, string feedUrl, IMSMediaSchemaPropertySet* pEpisodePropertySet)
        : base(owner, typeCookie)
    {
        m_feedUrl = feedUrl;
        m_pEpisodePropertySet = pEpisodePropertySet;
        pEpisodePropertySet->addRef((nint)pEpisodePropertySet);
        BindToLocalEpisode();
    }

    public unsafe override object GetProperty(string propertyName)
    {
        PROPERTY_TO_PID_MAP[] propertyToPidMap =
        [
            new()
            {
                propertyName = VCString.AsPtr(ref _003CModule_003E.PROPNAME_Title),
                propertyId = 0x1005001
            },
            new()
            {
                propertyName = VCString.AsPtr(ref _003CModule_003E.PROPNAME_Description),
                propertyId = 0x8005003
            },
            new()
            {
                propertyName = VCString.AsPtr(ref _003CModule_003E.PROPNAME_Duration),
                propertyId = 0x1005005
            },
            new()
            {
                propertyName = VCString.AsPtr(ref _003CModule_003E.PROPNAME_ReleaseDate),
                propertyId = 0x9000104
            },
            new()
            {
                propertyName = VCString.AsPtr(ref _003CModule_003E.PROPNAME_Explicit),
                propertyId = 0x5005006
            },
            new()
            {
                propertyName = VCString.AsPtr(ref _003CModule_003E.PROPNAME_Author),
                propertyId = 0x1005008
            },
            new()
            {
                propertyName = VCString.AsPtr(ref _003CModule_003E.PROPNAME_EnclosureUrl),
                propertyId = 0x8005007
            },
            new()
            {
                propertyName = VCString.AsPtr(ref _003CModule_003E.PROPNAME_EpisodeMediaType),
                propertyId = 0x600500A
            },
        ];

        Unsafe.SkipInit(out CComPropVariant cComPropVariant);
        Unsafe.InitBlock(&cComPropVariant, 0, 24);

        int hresult = 0;
        object result;

        try
        {
            fixed (ushort* ptr = VCString.ToWide(propertyName))
            {
                if (_003CModule_003E._wcsicmp(ptr, VCString.AsPtr(ref _003CModule_003E.PROPNAME_LibraryId)) == 0)
                {
                    cComPropVariant.vt = VARTYPE.VT_I4;
                    cComPropVariant.val1 = (uint)m_nEpisodeId;
                }
                else if (_003CModule_003E._wcsicmp(ptr, VCString.AsPtr(ref _003CModule_003E.PROPNAME_SeriesId)) == 0)
                {
                    cComPropVariant.vt = VARTYPE.VT_I4;
                    cComPropVariant.val1 = (uint)m_nSeriesId;
                }
                else
                {
                    if (_003CModule_003E._wcsicmp(ptr, VCString.AsPtr(ref _003CModule_003E.PROPNAME_DownloadState)) == 0)
                    {
                        if (m_nEpisodeId >= 0)
                        {
                            hresult = GetDatabaseValue(m_nEpisodeId, 145u, (tagPROPVARIANT*)&cComPropVariant);
                            if (hresult < 0)
                            {
                                goto IL_02b0;
                            }

                            // TODO: What size is EItemDownloadState
                            var newDownloadState = Unsafe.As<CComPropVariant, EItemDownloadState>(ref Unsafe.AddByteOffset(ref cComPropVariant, 8));
                            if (m_eLastDownloadState != newDownloadState)
                            {
                                InvalidateDownloadState();
                            }
                            m_eLastDownloadState = newDownloadState;
                        }
                        else
                        {
                            cComPropVariant.vt = VARTYPE.VT_I4;
                            cComPropVariant.val1 = 0;
                        }
                    }
                    else if (_003CModule_003E._wcsicmp(ptr, VCString.AsPtr(ref _003CModule_003E.PROPNAME_DownloadType)) == 0)
                    {
                        if (m_nEpisodeId >= 0)
                        {
                            hresult = GetDatabaseValue(m_nEpisodeId, 146u, (tagPROPVARIANT*)&cComPropVariant);
                        }
                        else
                        {
                            cComPropVariant.vt = VARTYPE.VT_I4;
                            cComPropVariant.val1 = 0;
                        }
                    }
                    else if (_003CModule_003E._wcsicmp(ptr, VCString.AsPtr(ref _003CModule_003E.PROPNAME_PlayedStatus)) == 0)
                    {
                        cComPropVariant.vt = VARTYPE.VT_NULL;
                        if (m_nEpisodeId < 0)
                        {
                            goto IL_028b;
                        }

                        hresult = GetDatabaseValue(m_nEpisodeId, 262u, (tagPROPVARIANT*)&cComPropVariant);
                    }
                    else if (_003CModule_003E._wcsicmp(ptr, VCString.AsPtr(ref _003CModule_003E.PROPNAME_SourceUrl)) == 0)
                    {
                        cComPropVariant.vt = VARTYPE.VT_NULL;
                        if (m_nEpisodeId < 0)
                        {
                            goto IL_028b;
                        }

                        hresult = GetDatabaseValue(m_nEpisodeId, 317u, (tagPROPVARIANT*)&cComPropVariant);
                    }
                    else if (_003CModule_003E._wcsicmp(ptr, VCString.AsPtr(ref _003CModule_003E.PROPNAME_DownloadErrorCode)) == 0)
                    {
                        cComPropVariant.vt = VARTYPE.VT_NULL;
                        if (m_nEpisodeId < 0)
                        {
                            goto IL_028b;
                        }
                        hresult = GetDatabaseValue(m_nEpisodeId, 144u, (tagPROPVARIANT*)&cComPropVariant);
                    }
                    else
                    {
                        int num2 = 0;
                        PROPERTY_TO_PID_MAP* ptr2 = (PROPERTY_TO_PID_MAP*)&propertyToPidMap;
                        while (_003CModule_003E._wcsicmp(ptr, (ushort*)*(ulong*)ptr2) != 0)
                        {
                            num2++;
                            ptr2 = (PROPERTY_TO_PID_MAP*)((ulong)(nint)ptr2 + 16uL);
                            if ((uint)num2 < 8u)
                            {
                                continue;
                            }
                            goto IL_028b;
                        }
                        uint num3 = *(uint*)((ref *(_003F*)((long)num2 * 16L)) + (ref Unsafe.As<_0024ArrayType_0024_0024_0024BY07UPROPERTY_TO_PID_MAP_0040Subscription_0040Zune_0040Microsoft_0040_0040, _003F>(ref Unsafe.AddByteOffset(ref propertyToPidMap, 8))));
                        IMSMediaSchemaPropertySet* pEpisodePropertySet = m_pEpisodePropertySet;
                        hresult = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, uint, tagPROPVARIANT*, int>)(*(ulong*)(*(long*)pEpisodePropertySet + 48)))((nint)pEpisodePropertySet, num3, 0u, (tagPROPVARIANT*)&cComPropVariant);
                    }
                    if (hresult < 0)
                    {
                        goto IL_02b0;
                    }
                }
                goto IL_028b;
                IL_02b0:
                result = Mappings[propertyName].DefaultValue;
                goto end_IL_00c8;
                IL_028b:
                if (_003CModule_003E.CComPropVariant_002EIsNullOrEmpty(&cComPropVariant))
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
            Unsafe.SkipInit(out tagVARIANT tagVARIANT);
            *(short*)&tagVARIANT = 3;
            Unsafe.As<tagVARIANT, int>(ref Unsafe.AddByteOffset(ref tagVARIANT, 8)) = (int)value;
            Unsafe.SkipInit(out DBPropertySubmitStruct dBPropertySubmitStruct);
            *(int*)&dBPropertySubmitStruct = 144;
            Unsafe.As<DBPropertySubmitStruct, long>(ref Unsafe.AddByteOffset(ref dBPropertySubmitStruct, 8)) = (nint)(&tagVARIANT);
            int num = ZuneLibraryExports.SetFieldValues(m_nEpisodeId, EListType.ePodcastEpisodeList, 1, &dBPropertySubmitStruct, null);
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
        //IL_0098: Expected I, but got I8
        //IL_009c: Expected I, but got I8
        //IL_00ae: Expected I, but got I8
        //IL_0067: Expected I, but got I8
        IService* ptr = null;
        ISubscriptionManager* ptr2 = null;
        int num = 1;
        int nSeriesId = -1;
        int nEpisodeId = -1;
        int singleton = _003CModule_003E.GetSingleton(_003CModule_003E.GuidToGUID(_003CModule_003E._GUID_bb2d1edd_1bd5_4be1_8d38_36d4f0849911), (void**)&ptr);
        if (singleton >= 0)
        {
            singleton = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, int*, int>)(*(ulong*)(*(long*)ptr + 384)))((nint)ptr, null, &num);
            if (singleton >= 0)
            {
                singleton = _003CModule_003E.GetSingleton((_GUID)_003CModule_003E._GUID_9dc7c984_41d5_4130_a5ac_46d0825cd29d, (void**)&ptr2);
                if (singleton >= 0)
                {
                    singleton = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, EMediaTypes, IMSMediaSchemaPropertySet*, int*, int*, int>)(*(ulong*)(*(long*)ptr2 + 136)))((nint)ptr2, num, EMediaTypes.eMediaTypePodcastSeries, m_pEpisodePropertySet, &nSeriesId, &nEpisodeId);
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
            IService* intPtr = ptr;
            ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
            ptr = null;
        }
        if (0L != (nint)ptr2)
        {
            ISubscriptionManager* intPtr2 = ptr2;
            ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
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
        //IL_00a7: Expected I4, but got I8
        //IL_0071: Expected I, but got I8
        //IL_00e2: Expected I, but got I8
        //IL_00e6: Expected I, but got I8
        //IL_009a: Expected I, but got I8
        //IL_009a: Expected I, but got I8
        //IL_009a: Expected I, but got I8
        ISubscriptionManager* ptr = null;
        Unsafe.SkipInit(out CComPropVariant cComPropVariant);
        // IL initblk instruction
        Unsafe.InitBlock(ref cComPropVariant, 0, 24);
        int num2;
        try
        {
            Unsafe.SkipInit(out CComPropVariant cComPropVariant2);
            // IL initblk instruction
            Unsafe.InitBlock(ref cComPropVariant2, 0, 24);
            try
            {
                int num = -1;
                fixed (ushort* ptr2 = &Unsafe.As<char, ushort>(ref _003CModule_003E.PtrToStringChars(m_feedUrl)))
                {
                    num2 = _003CModule_003E.GetSingleton((_GUID)_003CModule_003E._GUID_9dc7c984_41d5_4130_a5ac_46d0825cd29d, (void**)&ptr);
                    if (num2 >= 0)
                    {
                        IMSMediaSchemaPropertySet* pEpisodePropertySet = m_pEpisodePropertySet;
                        num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, uint, tagPROPVARIANT*, int>)(*(ulong*)(*(long*)pEpisodePropertySet + 48)))((nint)pEpisodePropertySet, 16797697u, 0u, (tagPROPVARIANT*)&cComPropVariant);
                        if (num2 >= 0)
                        {
                            IMSMediaSchemaPropertySet* pEpisodePropertySet2 = m_pEpisodePropertySet;
                            num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, uint, tagPROPVARIANT*, int>)(*(ulong*)(*(long*)pEpisodePropertySet2 + 48)))((nint)pEpisodePropertySet2, 134238215u, 0u, (tagPROPVARIANT*)&cComPropVariant2);
                            if (num2 >= 0)
                            {
                                long num3 = *(long*)ptr + 160;
                                num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaTypes, ushort*, ushort*, ushort*, int*, int>)(*(ulong*)num3))((nint)ptr, EMediaTypes.eMediaTypePodcastSeries, ptr2, (ushort*)Unsafe.As<CComPropVariant, ulong>(ref Unsafe.AddByteOffset(ref cComPropVariant, 8)), (ushort*)Unsafe.As<CComPropVariant, ulong>(ref Unsafe.AddByteOffset(ref cComPropVariant2, 8)), &num);
                            }
                        }
                    }
                    if (0 == num2)
                    {
                        Unsafe.SkipInit(out tagPROPVARIANT tagPROPVARIANT);
                        // IL initblk instruction
                        Unsafe.InitBlock(ref tagPROPVARIANT, 0, 24);
                        num2 = GetDatabaseValue(num, 311u, &tagPROPVARIANT);
                        if (num2 >= 0)
                        {
                            m_nEpisodeId = num;
                            m_nSeriesId = Unsafe.As<tagPROPVARIANT, int>(ref Unsafe.AddByteOffset(ref tagPROPVARIANT, 8));
                        }
                    }
                    if (0L != (nint)ptr)
                    {
                        ISubscriptionManager* intPtr = ptr;
                        ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
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
        //IL_0046: Expected I, but got I8
        //IL_0042: Expected I, but got I8
        //IL_0042: Expected I, but got I8
        //IL_00b0: Expected I, but got I8
        //IL_00b4: Expected I, but got I8
        //IL_006e: Expected I, but got I8
        //IL_00c8: Expected I, but got I8
        //IL_00cc: Expected I, but got I8
        if (pvarValue == null)
        {
            _003CModule_003E._ZuneShipAssert(1001u, 633u);
            return -2147467261;
        }
        IService* ptr = null;
        int num = 1;
        int num2 = _003CModule_003E.GetSingleton((_GUID)_003CModule_003E._GUID_bb2d1edd_1bd5_4be1_8d38_36d4f0849911, (void**)&ptr);
        if (num2 >= 0)
        {
            num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, int*, int>)(*(ulong*)(*(long*)ptr + 384)))((nint)ptr, null, &num);
        }
        IQueryPropertyBag* ptr2 = null;
        Unsafe.SkipInit(out DBPropertyRequestStruct dBPropertyRequestStruct);
        _003CModule_003E.DBPropertyRequestStruct_002E_007Bctor_007D(&dBPropertyRequestStruct, dwAtom);
        try
        {
            if (num2 >= 0)
            {
                num2 = ZuneLibraryExports.CreatePropertyBag(&ptr2);
                if (num2 >= 0)
                {
                    num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQueryPropertyBagProp, int, int>)(*(ulong*)(*(long*)ptr2 + 56)))((nint)ptr2, (EQueryPropertyBagProp)0, num);
                    if (num2 >= 0)
                    {
                        num2 = ZuneLibraryExports.GetFieldValues(nMediaId, EListType.ePodcastEpisodeList, 1, &dBPropertyRequestStruct, ptr2);
                        if (num2 >= 0)
                        {
                            num2 = (Unsafe.As<DBPropertyRequestStruct, int>(ref Unsafe.AddByteOffset(ref dBPropertyRequestStruct, 4)) < 0) ? Unsafe.As<DBPropertyRequestStruct, int>(ref Unsafe.AddByteOffset(ref dBPropertyRequestStruct, 4)) : _003CModule_003E.PropVariantCopy(pvarValue, (tagPROPVARIANT*)Unsafe.AsPointer(ref Unsafe.AddByteOffset(ref dBPropertyRequestStruct, 8)));
                        }
                    }
                }
            }
            if (0L != (nint)ptr2)
            {
                IQueryPropertyBag* intPtr = ptr2;
                ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
                ptr2 = null;
            }
            if (0L != (nint)ptr)
            {
                IService* intPtr2 = ptr;
                ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
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
