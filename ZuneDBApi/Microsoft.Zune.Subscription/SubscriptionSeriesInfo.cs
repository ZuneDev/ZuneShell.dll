using System;
using System.Runtime.CompilerServices;
using Microsoft.Iris;
using ZuneDBApi.Interop;

namespace Microsoft.Zune.Subscription;

public class SubscriptionSeriesInfo : DataProviderObject
{
    private const int PROPID_Title = 0x1006001;
    private const int PROPID_ServiceId = 0x400600F;

    private string m_serviceId;

    private ESubscriptionState m_eSubscriptionState;

    private unsafe IMSMediaSchemaPropertySet* m_pSeriesPropertySet;

    public unsafe SubscriptionSeriesInfo(DataProviderQuery owner, object typeCookie, string serviceId)
        : base(owner, typeCookie)
    {
        m_serviceId = serviceId;
        m_pSeriesPropertySet = null;
    }

    public unsafe override object GetProperty(string propertyName)
    {
        PROPERTY_TO_PID_MAP[] propertyToPidMaps =
        [
            new()
            {
                propertyName = (ushort*)Unsafe.AsPointer(ref _003CModule_003E.PROPNAME_FeedUrl),
                propertyId = 0x8000103
            },
            new()
            {
                propertyName = (ushort*)Unsafe.AsPointer(ref _003CModule_003E.PROPNAME_ErrorCode),
                propertyId = 0x600010A
            },
            new()
            {
                propertyName = (ushort*)Unsafe.AsPointer(ref _003CModule_003E.PROPNAME_Title),
                propertyId = PROPID_Title
            },
            new()
            {
                propertyName = (ushort*)Unsafe.AsPointer(ref _003CModule_003E.PROPNAME_HomeUrl),
                propertyId = 0x800600C
            },
            new()
            {
                propertyName = (ushort*)Unsafe.AsPointer(ref _003CModule_003E.PROPNAME_ArtUrl),
                propertyId = 0x800600D
            },
            new()
            {
                propertyName = (ushort*)Unsafe.AsPointer(ref _003CModule_003E.PROPNAME_Description),
                propertyId = 0x8006002
            },
            new()
            {
                propertyName = (ushort*)Unsafe.AsPointer(ref _003CModule_003E.PROPNAME_Explicit),
                propertyId = 0x5006006
            },
            new()
            {
                propertyName = (ushort*)Unsafe.AsPointer(ref _003CModule_003E.PROPNAME_Copyright),
                propertyId = 0x1006009
            },
            new()
            {
                propertyName = (ushort*)Unsafe.AsPointer(ref _003CModule_003E.PROPNAME_Author),
                propertyId = 0x1006004
            },
            new()
            {
                propertyName = (ushort*)Unsafe.AsPointer(ref _003CModule_003E.PROPNAME_OwnerName),
                propertyId = 0x1006007
            },
        ];

        Unsafe.SkipInit(out CComPropVariant cComPropVariant);
        Unsafe.InitBlock(&cComPropVariant, 0, 24);

        object propValue;
        try
        {
            fixed (ushort* pPropertyName = VCString.ToWide(propertyName))
            {
                if (_003CModule_003E._wcsicmp(pPropertyName, (ushort*)Unsafe.AsPointer(ref _003CModule_003E.PROPNAME_LibraryId)) != 0
                    && _003CModule_003E._wcsicmp(pPropertyName, (ushort*)Unsafe.AsPointer(ref _003CModule_003E.PROPNAME_SeriesState)) != 0
                    && _003CModule_003E._wcsicmp(pPropertyName, (ushort*)Unsafe.AsPointer(ref _003CModule_003E.PROPNAME_NumberOfEpisodes)) != 0
                    && m_pSeriesPropertySet != null)
                {
                    for (int currentMapIndex = 0; currentMapIndex < propertyToPidMaps.Length; currentMapIndex++)
                    {
                        var currentMap = propertyToPidMaps[currentMapIndex];
                        if (_003CModule_003E._wcsicmp(pPropertyName, currentMap.propertyName) != 0)
                        {
                            continue;
                        }

                        var currentPropertyId = currentMap.propertyId;
                        IMSMediaSchemaPropertySet* pSeriesPropertySet = m_pSeriesPropertySet;

                        var valueReadError = pSeriesPropertySet->readValue(new(pSeriesPropertySet), currentPropertyId, 0u, (tagPROPVARIANT*)&cComPropVariant);

                        if (valueReadError < 0 || *(ushort*)&cComPropVariant == 0)
                        {
                            break;
                        }

                        propValue = SubscriptionDataProviderQueryResult.ConvertVariantToType(Mappings[propertyName].PropertyTypeName, &cComPropVariant);

                        goto end;
                    }
                }
            }

            propValue = Mappings[propertyName].DefaultValue;

            end:;
        }
        catch
        {
            //try-fault
            _003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
            throw;
        }
        _003CModule_003E.CComPropVariant_002EClear(&cComPropVariant);
        return propValue;
    }

    public override void SetProperty(string propertyName, object value)
    {
        throw new NotSupportedException();
    }

    internal unsafe void OnDispose()
    {
        if (m_pSeriesPropertySet == null)
            return;

        m_pSeriesPropertySet->dispose((nint)m_pSeriesPropertySet);
        m_pSeriesPropertySet = null;
    }

    internal unsafe void SetPropertySet(IMSMediaSchemaPropertySet* pNewSeriesPropertySet)
    {
        Unsafe.SkipInit(out CComPropVariant cComPropVariant);
        Unsafe.InitBlock(&cComPropVariant, 0, 24);

        try
        {
            if (pNewSeriesPropertySet != null
                && pNewSeriesPropertySet->readValue((nint)pNewSeriesPropertySet, PROPID_Title, 0u, (tagPROPVARIANT*)&cComPropVariant) >= 0
                && *(ushort*)&cComPropVariant != 0)
            {
                IMSMediaSchemaPropertySet* pOldSeriesPropertySet = m_pSeriesPropertySet;
                if (pOldSeriesPropertySet != null)
                {
                    pOldSeriesPropertySet->dispose((nint)pOldSeriesPropertySet);
                    m_pSeriesPropertySet = null;
                }

                m_pSeriesPropertySet = pNewSeriesPropertySet;

                pNewSeriesPropertySet->addRef((nint)pNewSeriesPropertySet);

                if (!string.IsNullOrEmpty(m_serviceId))
                {
                    _GUID guid = _003CModule_003E.GuidToGUID(new(m_serviceId));

                    Unsafe.SkipInit(out tagPROPVARIANT tagPROPVARIANT);
                    tagPROPVARIANT.vt = VARTYPE.VT_CLSID;
                    tagPROPVARIANT.val1 = (nint)(&guid);

                    m_pSeriesPropertySet->setValue((nint)m_pSeriesPropertySet, PROPID_ServiceId, tagPROPVARIANT);
                }
            }
        }
        catch
        {
            //try-fault
            _003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
            throw;
        }
        _003CModule_003E.CComPropVariant_002EClear(&cComPropVariant);
    }
}
