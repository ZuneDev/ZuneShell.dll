using System;
using Microsoft.Iris;

namespace MicrosoftZuneLibrary;

internal class AsyncGetThumbnailState : IDisposable
{
    public LibraryDataProviderItemBase listItem;
    public string thumbnailPropertyName;
    public int thumbnailIndex;
    public bool slowDataQuery;
    public Image thumbnail;
    public string strUrl;
    public int MediaId;
    public bool antialiasImageEdges;
    public bool isComplete;

    public AsyncGetThumbnailState(LibraryDataProviderItemBase item)
    {
        listItem = item;
        isComplete = false;
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
