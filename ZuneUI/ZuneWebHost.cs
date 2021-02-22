// Decompiled with JetBrains decompiler
// Type: ZuneUI.ZuneWebHost
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;

namespace ZuneUI
{
    public class ZuneWebHost : ModelItem
    {
        private static int s_partnerServiceUnknownError = HRESULT._ZEST_E_PARTNER_SERVICE_UNKNOWN_ERROR.Int;
        private WebHostNavigationResult m_result;
        private string m_currentUrl;
        private string m_initialUrl;
        private string m_successUrl;
        private string m_failureUrl;
        private long m_childHwnd;
        private Microsoft.Zune.Util.ZuneWebHost m_host;
        private NavErrorData m_navErrorData;

        public ZuneWebHost() => this.m_host = Microsoft.Zune.Util.ZuneWebHost.Instance;

        public bool Initialize(long hWndHost, int width, int height)
        {
            this.m_host.SetNavigationCompleteHandler((NavigationCompleteHandler)(data => Application.DeferredInvoke((DeferredInvokeHandler)delegate
          {
              this.CurrentUrl = data;
              if (!string.IsNullOrEmpty(this.SuccessUrl) && this.SuccessUrl.Length <= this.m_currentUrl.Length && string.Compare(this.SuccessUrl, this.m_currentUrl.Substring(0, this.SuccessUrl.Length), false) == 0)
              {
                  this.ResponseToken = string.Empty;
                  this.NavResult = WebHostNavigationResult.SuccessUrl;
              }
              else
              {
                  if (string.IsNullOrEmpty(this.FailureUrl) || this.FailureUrl.Length > this.m_currentUrl.Length || string.Compare(this.FailureUrl, this.m_currentUrl.Substring(0, this.FailureUrl.Length), false) != 0)
                      return;
                  this.NavResult = WebHostNavigationResult.FailureUrl;
              }
          }, (object)data)));
            this.m_host.SetNavigationErrorHandler((NavigationErrorHandler)((errorUrl, errorCode) =>
           {
               NavErrorData data = new NavErrorData(errorUrl, errorCode);
               Application.DeferredInvoke((DeferredInvokeHandler)delegate
         {
                 this.NavigationError = data;
             }, (object)data);
           }));
            long num = this.m_host.Initialize(this.InitialUrl, hWndHost, width, height);
            if (num == 0L)
            {
                this.NavigationError = new NavErrorData(this.InitialUrl, HRESULT._ZEST_E_PARTNER_SERVICE_UNKNOWN_ERROR.Int);
                this.NavResult = WebHostNavigationResult.FailureUrl;
            }
            else
                this.ChildHwnd = num;
            return num != 0L;
        }

        public bool SetSize(long hWndHost, int width, int height) => this.m_host.SetSize(hWndHost, width, height);

        public long ChildHwnd
        {
            get => this.m_childHwnd;
            set
            {
                if (this.m_childHwnd == value)
                    return;
                this.m_childHwnd = value;
                this.FirePropertyChanged(nameof(ChildHwnd));
            }
        }

        public string CurrentUrl
        {
            get => this.m_currentUrl;
            set
            {
                if (string.Compare(value, this.m_currentUrl, false) == 0)
                    return;
                this.m_currentUrl = value;
                this.FirePropertyChanged(nameof(CurrentUrl));
            }
        }

        public string ResponseToken { get; private set; }

        public NavErrorData NavigationError
        {
            get => this.m_navErrorData;
            set
            {
                if (this.m_navErrorData != null && this.m_navErrorData.Equals(value))
                    return;
                this.m_navErrorData = value;
                this.FirePropertyChanged(nameof(NavigationError));
            }
        }

        public WebHostNavigationResult NavResult
        {
            get => this.m_result;
            set
            {
                if (this.m_result == value)
                    return;
                this.m_result = value;
                this.FirePropertyChanged(nameof(NavResult));
            }
        }

        public string InitialUrl
        {
            get => this.m_initialUrl;
            set
            {
                if (string.Compare(value, this.m_initialUrl, false) == 0)
                    return;
                this.m_initialUrl = value;
                this.FirePropertyChanged(nameof(InitialUrl));
            }
        }

        public string SuccessUrl
        {
            get => this.m_successUrl;
            set
            {
                if (string.Compare(value, this.m_successUrl, false) == 0)
                    return;
                this.m_successUrl = value;
                this.FirePropertyChanged(nameof(SuccessUrl));
            }
        }

        public string FailureUrl
        {
            get => this.m_failureUrl;
            set
            {
                if (string.Compare(value, this.m_failureUrl, false) == 0)
                    return;
                this.m_failureUrl = value;
                this.FirePropertyChanged(nameof(FailureUrl));
            }
        }

        public int PartnerServiceUnknownError => ZuneWebHost.s_partnerServiceUnknownError;
    }
}
