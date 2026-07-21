using Microsoft.Iris;

namespace Microsoft.Zune.Subscription;

// Original reads podcast episode metadata out of a native IMSMediaSchemaPropertySet*
// (via a PROPERTY_TO_PID_MAP lookup table) plus the local library database (via
// ZuneLibraryExports.GetFieldValues/SetFieldValues and a native ISubscriptionManager/
// IService pair for SaveToLibrary). None of that native layer has been reverse
// engineered — see logs/Microsoft.Zune/Subscription/SubscriptionManager.md.
public class SubscriptionDataProviderItem : DataProviderObject
{
    private string m_feedUrl;
    private int m_nSeriesId = -1;
    private int m_nEpisodeId = -1;
    private EItemDownloadState m_eLastDownloadState = EItemDownloadState.eDownloadStateNone;

    internal SubscriptionDataProviderItem(DataProviderQuery owner, object typeCookie, string feedUrl)
        : base(owner, typeCookie)
    {
        m_feedUrl = feedUrl;
    }

    public override object GetProperty(string propertyName)
    {
        return propertyName switch
        {
            "LibraryId" => m_nEpisodeId,
            "SeriesId" => m_nSeriesId,
            "DownloadState" => m_eLastDownloadState,
            "DownloadType" => EItemDownloadType.eDownloadTypeManual,
            _ => Mappings[propertyName].DefaultValue,
        };
    }

    public override void SetProperty(string propertyName, object value)
    {
        if (m_nEpisodeId > 0 && propertyName == "DownloadErrorCode")
        {
            FirePropertyChanged(propertyName);
        }
    }

    public virtual void SaveToLibrary()
    {
        FirePropertyChanged("DownloadState");
    }
}
