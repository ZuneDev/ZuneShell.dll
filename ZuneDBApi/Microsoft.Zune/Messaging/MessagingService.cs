using System;
using MicrosoftZuneLibrary;
using ZuneUI;

namespace Microsoft.Zune.Messaging;

// Original wraps a native IZuneNetMessaging* singleton (social inbox/comments/friends).
// Not reverse engineered — see logs/Microsoft.Zune/Messaging/PlaylistMessageData.md.
public class MessagingService : IDisposable
{
    private static MessagingService m_singletonInstance;

    public static bool HasInstance => m_singletonInstance != null;

    public static MessagingService Instance => m_singletonInstance ??= new MessagingService();

    private MessagingService()
    {
    }

    public bool MessageSetRead(string strMessageUrl) => false;

    public bool MessageDelete(string strMessageUrl) => false;

    public bool AcceptFriend(string strPostUrl) => false;

    public bool RejectFriend(string strPostUrl) => false;

    public bool ManageFriend(FriendAction eAction, string strPostUrl, string strZuneTag) => false;

    public bool AddComment(string strPostUrl, string strZuneTag, string strMessage, CommentCallback callback)
    {
        callback?.Invoke(HRESULT._E_FAIL, Guid.Empty);
        return false;
    }

    public bool DeleteComment(string strPostUrl, string strZuneTag, MessagingCallback callback)
    {
        callback?.Invoke(HRESULT._E_FAIL, null);
        return false;
    }

    public bool Compose(string strPostUrl, string strMessage, string strRecipientZuneTags, string strRequestType, IPropertySetMessageData messageData, MessagingCallback callback, object state)
    {
        callback?.Invoke(HRESULT._E_FAIL, state);
        return false;
    }

    public bool Compose(string strPostUrl, string strMessage, MessagingCallback callback, object state)
    {
        callback?.Invoke(HRESULT._E_FAIL, state);
        return false;
    }

    public bool ManageFavorites(FavoritesAction eAction, string strFavoritesUrl, string strInstructions, MessagingCallback callback, object state)
    {
        callback?.Invoke(HRESULT._E_FAIL, state);
        return false;
    }

    public bool ManageProfile(string strProfileUrl, string strFieldValue, MessagingCallback callback, object state)
    {
        callback?.Invoke(HRESULT._E_FAIL, state);
        return false;
    }

    public bool ManageProfileImage(string strProfileImageUrl, string strProfileImageResource, MessagingCallback callback, object state)
    {
        callback?.Invoke(HRESULT._E_FAIL, state);
        return false;
    }

    public bool ManageProfileImage(string strProfileImageUrl, SafeBitmap profileImage, MessagingCallback callback, object state)
    {
        callback?.Invoke(HRESULT._E_FAIL, state);
        return false;
    }

    public string GetInboxPhotoUrl(string title, string collectionName) => null;

    public bool AddInboxPhoto(string title, string collectionName, string localFilePath) => false;

    public int GetInboxDownloadFolderId(string collectionName) => -1;

    public void InitiateUploadDeviceCartItems()
    {
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
