using System;
using System.Collections.Generic;
using Microsoft.Iris;

namespace MicrosoftZuneLibrary;

// GetProperty/SetProperty/GetMediaIdAndType are transcribed close to verbatim from the
// original decompiled bodies: they're pure C#/Iris DataProviderMapping logic with no
// native dependency (only the WPP/ETW tracing calls were dropped, same as
// VirtualDatabaseList — see logs/MicrosoftZuneLibrary/ZuneLibrary.md). Thumbnail
// extraction and GetFieldValue/SetFieldValue ultimately call into the native library
// database (ZuneLibraryExports), which is not reverse engineered, so those paths are
// stubbed.
public class LibraryDataProviderItemBase : DataProviderObject, IDatabaseMedia
{
    private Dictionary<string, object> m_setProperties;

    protected LibraryDataProviderItemBase(DataProviderQuery owner, object typeCookie)
        : base(owner, typeCookie)
    {
    }

    public override object GetProperty(string propertyName)
    {
        if (m_setProperties != null && m_setProperties.TryGetValue(propertyName, out object obj))
        {
            return obj;
        }
        if (Mappings.TryGetValue(propertyName, out DataProviderMapping mapping))
        {
            if (!string.IsNullOrEmpty(mapping.Source))
            {
                if (mapping.Source == "ThumbnailPath")
                {
                    throw new NotImplementedException();
                }
                if (ThumbnailSizeIndexFromSizeName(mapping.Source, -1) >= 0)
                {
                    return null;
                }
                return GetFieldValue(mapping.PropertyType, mapping.Source, mapping.DefaultValue);
            }
            return mapping.DefaultValue;
        }
        return null;
    }

    public override void SetProperty(string propertyName, object value)
    {
        if (!Mappings.TryGetValue(propertyName, out DataProviderMapping mapping) || string.IsNullOrEmpty(mapping.Source))
        {
            return;
        }
        if (mapping.Source == "ThumbnailPath")
        {
            SetNewThumbnail((string)value);
            return;
        }
        if (Equals(value, GetProperty(propertyName)))
        {
            return;
        }
        if (!(TypeName == "Playlist" && mapping.Source == "Title"))
        {
            int hr = SetFieldValue(mapping.Source, value);
            if (hr >= 0 && mapping.Source == "UserRating")
            {
                SetFieldValue("UserLastRatedDate", DateTime.UtcNow);
            }
        }
        m_setProperties ??= new Dictionary<string, object>();
        m_setProperties[propertyName] = value;
        FirePropertyChanged(propertyName);
    }

    public virtual void GetMediaIdAndType(out int mediaId, out EMediaTypes mediaType)
    {
        if (TypeName == "PlaylistContentItem")
        {
            mediaId = (int)GetProperty("MediaId");
            mediaType = (EMediaTypes)GetProperty("MediaType");
        }
        else if (TypeName == "SyncItem")
        {
            mediaId = (int)GetProperty("LibraryId");
            mediaType = (EMediaTypes)GetProperty("MediaType");
        }
        else
        {
            mediaId = (int)GetProperty("LibraryId");
            mediaType = LibraryDataProvider.NameToMediaType(TypeName);
        }
    }

    public static string GetArtUrl(int mediaId, string typeName, bool fCacheOnly) => null;

    public virtual void InvalidateAllProperties()
    {
        FirePropertyChanged(null);
    }

    public void SetSlowDataThumbnailExtraction(bool useSlowData)
    {
    }

    protected bool SetNewThumbnail(string strThumbnailPath) => false;

    public bool SetNewThumbnail(SafeBitmap safeBitmap) => false;

    protected internal virtual void OnRequestSlowData()
    {
    }

    protected bool BeginGetThumbnail(string thumbnailPropertyName, int thumbnailIndex, bool slowDataQuery) => false;

    protected virtual void UpdateThumbnail(object args)
    {
    }

    protected virtual object GetFieldValue(Type type, uint atom) => null;

    protected virtual object GetFieldValue(Type type, string atomName, object defaultValue) => defaultValue;

    protected virtual object GetFieldValue(Type type, uint atom, object defaultValue) => defaultValue;

    protected virtual int SetFieldValue(string atomName, object value) => unchecked((int)0x80004005);

    protected virtual int SetFieldValue(uint atom, object value) => unchecked((int)0x80004005);

    protected virtual void NotifySlowDataAcquireComplete()
    {
    }

    protected virtual bool AntialiasImageEdges() => false;

    protected virtual string ThumbnailFallbackImageUrl() => null;

    private static int ThumbnailSizeIndexFromSizeName(string thumbnailSizeName, int defaultSizeIndex) => defaultSizeIndex;
}
