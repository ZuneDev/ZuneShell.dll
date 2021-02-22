// Decompiled with JetBrains decompiler
// Type: ZuneUI.SubscribeConfirmDialogHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using Microsoft.Zune.Subscription;
using System;
using UIXControls;
using ZuneXml;

namespace ZuneUI
{
    internal class SubscribeConfirmDialogHelper
    {
        private string m_feedTitle;
        private string m_feedUrl;

        private SubscribeConfirmDialogHelper(string feedTitle, string feedUrl)
        {
            this.m_feedUrl = feedUrl;
            if (!string.IsNullOrEmpty(feedTitle))
                this.m_feedTitle = feedTitle;
            else
                this.m_feedTitle = feedUrl;
        }

        private void ShowDialog() => MessageBox.Show(Shell.LoadString(StringId.IDS_PODCAST_CONFIRM_DIALOG_TITLE), string.Format(Shell.LoadString(StringId.IDS_PODCAST_CONFIRM_DIALOG_MESSAGE), (object)this.m_feedTitle), new EventHandler(this.OnConfirm));

        private void OnConfirm(object sender, EventArgs args)
        {
            SubscriptionManager instance = SubscriptionManager.Instance;
            int subscriptionMediaId = -1;
            if (!instance.FindByUrl(this.m_feedUrl, EMediaTypes.eMediaTypePodcastSeries, out subscriptionMediaId, out bool _))
            {
                HRESULT hresult = (HRESULT)instance.Subscribe(this.m_feedUrl, this.m_feedTitle, Guid.Empty, false, EMediaTypes.eMediaTypePodcastSeries, ESubscriptionSource.eSubscriptionSourceProtocolHandler, out subscriptionMediaId);
                if (hresult.IsSuccess)
                {
                    string endPointUri = Microsoft.Zune.Service.Service.GetEndPointUri(Microsoft.Zune.Service.EServiceEndpointId.SEID_RootCatalog);
                    if (!string.IsNullOrEmpty(endPointUri) && !string.IsNullOrEmpty(this.m_feedUrl) && this.m_feedUrl.Length < 1024)
                        WebRequestHelper.ConstructWebPostRequest(endPointUri + "/podcast", "URL=" + this.m_feedUrl, EPassportPolicyId.None, HttpRequestCachePolicy.Default, true, false).GetResponseAsync(new AsyncRequestComplete(this.OnRequestComplete), (object)null);
                }
                else
                    ErrorDialogInfo.Show(hresult.Int, Shell.LoadString(StringId.IDS_PODCAST_SUBSCRIPTION_ERROR));
            }
            PodcastLibraryPage.FindInCollection(subscriptionMediaId);
        }

        private void OnRequestComplete(HttpWebResponse response, object requestArgs)
        {
            int statusCode = (int)response.StatusCode;
        }

        public static void Show(string feedTitle, string feedUrl) => new SubscribeConfirmDialogHelper(feedTitle, feedUrl).ShowDialog();
    }
}
