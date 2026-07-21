using System;
using ZuneUI;

namespace Microsoft.Zune.Util;

// Original wraps a native photo-library management service. Not reverse engineered —
// see logs/Microsoft.Zune/Util/DownloadManager.md.
public class PhotoManager
{
    private static PhotoManager sm_PhotoManager;

    public static PhotoManager Instance => sm_PhotoManager ??= new PhotoManager();

    private PhotoManager()
    {
    }

    public HRESULT SetWindowHandle(IntPtr hWnd) => HRESULT._E_FAIL;

    public HRESULT FindFolder(string szFolderName, out int nFolderId)
    {
        nFolderId = -1;
        return HRESULT._E_FAIL;
    }

    public HRESULT FindPhotoContainer(int nMediaId, out int nFolderId)
    {
        nFolderId = -1;
        return HRESULT._E_FAIL;
    }

    public HRESULT CreateFolder(string szFolderName, int nParentFolderId, out int nCreatedFolderId)
    {
        nCreatedFolderId = -1;
        return HRESULT._E_FAIL;
    }

    public HRESULT RenameFolder(int nFolderId, string szNewFolderName) => HRESULT._E_FAIL;

    public HRESULT Move(int[] mediaIds, EMediaTypes mediaType, int nDestinationFolderId) => HRESULT._E_FAIL;

    public HRESULT Import(string szPath, EMediaTypes mediaType, int nDestinationFolderId) => HRESULT._E_FAIL;
}
