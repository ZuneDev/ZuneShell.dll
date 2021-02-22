// Decompiled with JetBrains decompiler
// Type: ZuneUI.ComposerHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.ErrorMapperApi;
using Microsoft.Zune.Messaging;
using Microsoft.Zune.Util;
using MicrosoftZuneLibrary;
using System;
using System.Collections;
using System.Text;
using UIXControls;

namespace ZuneUI
{
    public class ComposerHelper : DialogHelper
    {
        private const int c_maxMessageLength = 230;
        private RecipientHelper _recipientHelper;
        private Command _sendFailed;
        private Command _sendSucceeded;
        private string _message;
        private string _serviceContext;

        public ComposerHelper()
        {
            this._recipientHelper = new RecipientHelper();
            this._sendFailed = new Command();
            this._sendSucceeded = new Command();
        }

        public void ClearState()
        {
            this._recipientHelper.ClearState();
            this.Message = string.Empty;
        }

        public bool Send(Attachment attachment)
        {
            string endPointUri = Microsoft.Zune.Service.Service.GetEndPointUri(Microsoft.Zune.Service.EServiceEndpointId.SEID_Messaging);
            string zuneTag = SignIn.Instance.ZuneTag;
            Uri.EscapeDataString(",");
            string errorMessage = (string)null;
            HRESULT hresult = HRESULT._S_OK;
            bool flag = true;
            bool wishlist = false;
            bool allowEmailRecipients = true;
            if (attachment != null)
            {
                wishlist = attachment.Wishlist;
                allowEmailRecipients = attachment.AllowEmailRecipients;
                if (!attachment.IsValid(out errorMessage))
                {
                    flag = false;
                    hresult = HRESULT._NS_E_MESSAGING_CLIENT_ERROR;
                }
            }
            if (flag && !this.RecipientHelper.ValidateAll(allowEmailRecipients))
            {
                flag = false;
                hresult = HRESULT._NS_E_MESSAGING_RECIPIENT_ERROR;
                errorMessage = this.RecipientHelper.ErrorMessage;
            }
            if (!SignIn.Instance.SignedIn || string.IsNullOrEmpty(endPointUri) || string.IsNullOrEmpty(zuneTag))
                flag = false;
            if (flag)
                flag = this.Send(string.Format("{0}/messaging/{1}/send", (object)endPointUri, (object)Uri.EscapeDataString(zuneTag)), attachment, wishlist);
            if (!flag)
                this.OnSendCompleted(hresult.IsError ? hresult : HRESULT._NS_E_MESSAGING_CLIENT_ERROR, errorMessage, wishlist);
            return flag;
        }

        private bool Send(string requestUrl, Attachment attachment, bool wishlist)
        {
            bool flag = false;
            if (attachment is PropertySetAttachment)
            {
                PropertySetAttachment propertySetAttachment = (PropertySetAttachment)attachment;
                string recipientString = this.CreateRecipientString(false);
                flag = MessagingService.Instance.Compose(requestUrl, this.Message, recipientString, propertySetAttachment.RequestType, propertySetAttachment.PropertySet, new MessagingCallback(this.OnSendCompletedAsync), (object)wishlist);
            }
            else
            {
                string strMessage = (string)null;
                try
                {
                    strMessage = this.CreateUrlEncodedString(attachment);
                }
                catch (ArgumentNullException ex)
                {
                }
                if (strMessage != null)
                    flag = MessagingService.Instance.Compose(requestUrl, strMessage, new MessagingCallback(this.OnSendCompletedAsync), (object)wishlist);
            }
            if (flag && attachment != null)
                attachment.LogSend();
            return flag;
        }

        public static bool ManageFriend(FriendAction action, string zuneTag)
        {
            bool flag = !string.IsNullOrEmpty(zuneTag);
            string strPostUrl = (string)null;
            if (flag)
            {
                strPostUrl = ComposerHelper.CreateOperationUri("friends");
                flag = strPostUrl != null;
            }
            if (flag)
                flag = MessagingService.Instance.ManageFriend(action, strPostUrl, zuneTag);
            return flag;
        }

        public bool ManageFavorites(FavoritesAction action, MediaType mediaType, IList favorites)
        {
            string str1 = (string)null;
            string str2 = (string)null;
            string operationUri = ComposerHelper.CreateOperationUri("playlists/BuiltIn-FavoriteTracks");
            bool flag = operationUri != null;
            switch (action)
            {
                case FavoritesAction.Add:
                    str1 = "add";
                    break;
                case FavoritesAction.Remove:
                    str1 = "delete";
                    break;
                default:
                    flag = false;
                    break;
            }
            if (mediaType == MediaType.Track)
                str2 = "track";
            else
                flag = false;
            if (flag)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(str1);
                stringBuilder.Append(' ');
                stringBuilder.Append(str2);
                stringBuilder.Append(' ');
                for (int index = 0; index < favorites.Count; ++index)
                {
                    if (index > 0)
                        stringBuilder.Append(',');
                    stringBuilder.Append(favorites[index].ToString());
                }
                flag = MessagingService.Instance.ManageFavorites(action, operationUri, stringBuilder.ToString(), new MessagingCallback(this.OnFavoritesManagementCompletedAsync), (object)new ComposerHelper.FavoritesManagementState(action, favorites.Count));
            }
            return flag;
        }

        public static bool ManageProfile(
          string fieldName,
          string fieldValue,
          MessagingCallback callback,
          object state)
        {
            bool flag = !string.IsNullOrEmpty(fieldName);
            string strProfileUrl = (string)null;
            if (flag)
            {
                strProfileUrl = ComposerHelper.CreateOperationUri(fieldName);
                flag = strProfileUrl != null;
            }
            if (flag)
                flag = MessagingService.Instance.ManageProfile(strProfileUrl, fieldValue, callback, state);
            return flag;
        }

        public static bool ManageProfileImage(
          ProfileImage image,
          MessagingCallback callback,
          object state)
        {
            bool flag = !string.IsNullOrEmpty(image.TypeName);
            string strProfileImageUrl = (string)null;
            if (flag)
            {
                strProfileImageUrl = ComposerHelper.CreateOperationUri(image.TypeName);
                flag = strProfileImageUrl != null;
            }
            if (flag)
                flag = image.Image == null ? image.ResourceId != null && MessagingService.Instance.ManageProfileImage(strProfileImageUrl, image.ResourceId, callback, state) : MessagingService.Instance.ManageProfileImage(strProfileImageUrl, (SafeBitmap)image.Image, callback, state);
            if (flag)
            {
                if (image.Type == ProfileImageType.Background)
                    SQMLog.Log(SQMDataId.SocialEditBackground, 1);
                else if (image.Type == ProfileImageType.Tile)
                    SQMLog.Log(SQMDataId.SocialEditTile, 1);
            }
            return flag;
        }

        public static string CreateOperationUri(string operation)
        {
            string zuneTagOrGuid = (string)null;
            Guid userGuid = SignIn.Instance.UserGuid;
            if (!GuidHelper.IsEmpty(userGuid))
                zuneTagOrGuid = userGuid.ToString();
            return ComposerHelper.CreateOperationUri(operation, zuneTagOrGuid);
        }

        public static string GetUserTileUri(string zuneTag) => ComposerHelper.CreateOperationUri("usertile", zuneTag);

        public static string CreateOperationUri(string operation, string zuneTagOrGuid)
        {
            if (string.IsNullOrEmpty(zuneTagOrGuid))
                return (string)null;
            string endPointUri = Microsoft.Zune.Service.Service.GetEndPointUri(operation == "comments" ? Microsoft.Zune.Service.EServiceEndpointId.SEID_Comments : Microsoft.Zune.Service.EServiceEndpointId.SEID_SocialApi);
            if (string.IsNullOrEmpty(endPointUri))
                return (string)null;
            zuneTagOrGuid = Uri.EscapeUriString(zuneTagOrGuid);
            StringBuilder stringBuilder = new StringBuilder(endPointUri);
            stringBuilder.Append("/members");
            if (operation == "search")
                return UrlHelper.MakeUrl(stringBuilder.ToString(), "q", zuneTagOrGuid);
            stringBuilder.Append("/");
            stringBuilder.Append(zuneTagOrGuid);
            if (!string.IsNullOrEmpty(operation))
            {
                stringBuilder.Append("/");
                stringBuilder.Append(operation);
            }
            return stringBuilder.ToString();
        }

        private string CreateUrlEncodedString(Attachment attachment)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (attachment != null)
            {
                foreach (string property in attachment.Properties)
                {
                    if (property == null)
                        throw new ArgumentNullException();
                }
            }
            stringBuilder.Append("recipients=");
            stringBuilder.Append(this.CreateRecipientString(true));
            if (!string.IsNullOrEmpty(this._message) && this._message.Length <= 230)
            {
                stringBuilder.Append("&text=");
                stringBuilder.Append(Uri.EscapeDataString(this._message));
            }
            if (attachment != null)
            {
                string[] properties = attachment.Properties;
                int num = properties.Length - 1;
                for (int index = 0; index < num; index += 2)
                {
                    stringBuilder.Append("&");
                    stringBuilder.Append(Uri.EscapeDataString(properties[index]));
                    stringBuilder.Append("=");
                    stringBuilder.Append(Uri.EscapeDataString(properties[index + 1]));
                }
            }
            else
                stringBuilder.Append("&type=message");
            if (!string.IsNullOrEmpty(this._serviceContext))
            {
                stringBuilder.Append("&context=");
                stringBuilder.Append(Uri.EscapeDataString(this._serviceContext));
            }
            return stringBuilder.ToString();
        }

        private string CreateRecipientString(bool uriEscape)
        {
            StringBuilder stringBuilder = new StringBuilder();
            IList validRecipients = this.RecipientHelper.ValidRecipients;
            string str = ",";
            if (validRecipients != null)
            {
                for (int index = 0; index < validRecipients.Count; ++index)
                {
                    if (index != 0)
                        stringBuilder.Append(str);
                    if (uriEscape)
                        stringBuilder.Append(Uri.EscapeDataString((string)validRecipients[index]));
                    else
                        stringBuilder.Append((string)validRecipients[index]);
                }
            }
            return stringBuilder.ToString();
        }

        public RecipientHelper RecipientHelper => this._recipientHelper;

        public int MaxMessageLength => 230;

        public Command SendFailed => this._sendFailed;

        public Command SendSucceeded => this._sendSucceeded;

        public string Message
        {
            get => this._message;
            set
            {
                if (!(this._message != value))
                    return;
                this._message = value == null ? value : value.Replace("\r\n", "\n");
                this.FirePropertyChanged(nameof(Message));
            }
        }

        public string ServiceContext
        {
            get => this._serviceContext;
            set
            {
                if (!(this._serviceContext != value))
                    return;
                this._serviceContext = value;
                this.FirePropertyChanged(nameof(ServiceContext));
            }
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing && this._recipientHelper != null)
            {
                this._recipientHelper.Dispose();
                this._recipientHelper = (RecipientHelper)null;
            }
            base.OnDispose(disposing);
        }

        private void OnSendCompleted(HRESULT hr, string dialogMessage, bool wishlist)
        {
            if (hr.IsError)
            {
                if (wishlist)
                {
                    ErrorMapperResult descriptionAndUrl = Microsoft.Zune.ErrorMapperApi.ErrorMapperApi.GetMappedErrorDescriptionAndUrl(hr.Int, eErrorCondition.eEC_Cart);
                    ErrorDialogInfo.Show(descriptionAndUrl.Hr, Shell.LoadString(StringId.IDS_CART_CANT_ADD_ITEMS), descriptionAndUrl.Description);
                }
                else
                    ErrorDialogInfo.Show(hr.Int, this.Description, dialogMessage);
                this._sendFailed.Invoke();
            }
            else
            {
                this._sendSucceeded.Invoke();
                if (!wishlist)
                    NotificationArea.Instance.Add((Notification)new MessageNotification(Shell.LoadString(StringId.IDS_MESSAGE_SENT_NOTIFICATION), NotificationTask.Messaging, NotificationState.OneShot));
                else
                    ++Shell.MainFrame.Marketplace.CartItemsCount;
            }
        }

        private void OnSendCompleted(object obj)
        {
            ComposerHelper.SendCompletedParams sendCompletedParams = (ComposerHelper.SendCompletedParams)obj;
            this.OnSendCompleted(sendCompletedParams.HR, (string)null, sendCompletedParams.Wishlist);
        }

        private void OnSendCompletedAsync(HRESULT hr, object state)
        {
            bool wishlist = (bool)state;
            Application.DeferredInvoke(new DeferredInvokeHandler(this.OnSendCompleted), (object)new ComposerHelper.SendCompletedParams(hr, wishlist));
        }

        private void OnFavoritesManagementCompleted(object obj)
        {
            if (!(obj is ComposerHelper.FavoritesManagementState favoritesManagementState) || favoritesManagementState.Action != FavoritesAction.Add || !favoritesManagementState.MgmtOperationResult.IsSuccess)
                return;
            NotificationArea.Instance.Add((Notification)new MessageNotification(string.Format(Shell.LoadString(favoritesManagementState.FavoritesCount == 1 ? StringId.IDS_FAVORITE_MGMT_NOTIFICATION : StringId.IDS_FAVORITES_MGMT_NOTIFICATION), (object)favoritesManagementState.FavoritesCount), NotificationTask.Messaging, NotificationState.OneShot));
        }

        private void OnFavoritesManagementCompletedAsync(HRESULT hr, object state)
        {
            if (!(state is ComposerHelper.FavoritesManagementState favoritesManagementState))
                return;
            favoritesManagementState.MgmtOperationResult = hr;
            Application.DeferredInvoke(new DeferredInvokeHandler(this.OnFavoritesManagementCompleted), (object)favoritesManagementState);
        }

        private class SendCompletedParams
        {
            private readonly HRESULT _hr;
            private readonly bool _wishlist;

            public SendCompletedParams(HRESULT hr, bool wishlist)
            {
                this._hr = hr;
                this._wishlist = wishlist;
            }

            public HRESULT HR => this._hr;

            public bool Wishlist => this._wishlist;
        }

        private class FavoritesManagementState
        {
            public FavoritesAction Action;
            public int FavoritesCount;
            public HRESULT MgmtOperationResult;

            public FavoritesManagementState(FavoritesAction action, int favoritesCount)
            {
                this.Action = action;
                this.FavoritesCount = favoritesCount;
            }
        }
    }
}
