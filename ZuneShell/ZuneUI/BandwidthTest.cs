// Decompiled with JetBrains decompiler
// Type: ZuneUI.BandwidthTest
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Util;
using MicrosoftZunePlayback;
using System.Collections;
using System.Threading;

namespace ZuneUI
{
    public class BandwidthTest : ModelItem
    {
        private const int c_minHDRequirement = 2800000;
        private const int c_minSDRequirement = 700000;
        private const int WIN32ERRCODE_TIMEOUT = -2147023436;
        private BandwidthTestInterop m_bandwidthTestInterop;
        private int m_bandwidthTestProgressPercent;
        private BandwidthCapacity m_bandwidthTestResult;
        private IList m_videoOffers;
        private string m_uri;
        private bool m_fBandwidthTestRunning;
        private bool m_fBandwidthTestCanceled;
        private bool m_fBandwidthTestCompleted;
        private bool m_fBandwidthTestErrFound;
        private BandwidthCapacity m_lastTestResult;
        private bool m_fIsRetried;
        private bool m_fIsContentSDOnly;
        private bool m_fIsContentHDOnly;

        protected override void OnDispose(bool disposing)
        {
            base.OnDispose(disposing);
            if (!disposing || this.m_bandwidthTestInterop == null)
                return;
            this.m_bandwidthTestInterop = null;
        }

        public int BandwidthTestProgressPercent
        {
            get => this.m_bandwidthTestProgressPercent;
            private set
            {
                this.m_bandwidthTestProgressPercent = value;
                this.FirePropertyChanged(nameof(BandwidthTestProgressPercent));
            }
        }

        public bool IsBandwidthTestErrFound
        {
            get => this.m_fBandwidthTestErrFound;
            private set
            {
                this.m_fBandwidthTestErrFound = value;
                this.FirePropertyChanged(nameof(IsBandwidthTestErrFound));
            }
        }

        public bool IsBandwidthTestRunning
        {
            get => this.m_fBandwidthTestRunning;
            private set
            {
                this.m_fBandwidthTestRunning = value;
                this.FirePropertyChanged(nameof(IsBandwidthTestRunning));
            }
        }

        public bool IsBandwidthTestCanceled
        {
            get => this.m_fBandwidthTestCanceled;
            private set
            {
                this.m_fBandwidthTestCanceled = value;
                this.FirePropertyChanged(nameof(IsBandwidthTestCanceled));
            }
        }

        public bool IsBandwidthTestCompleted
        {
            get => this.m_fBandwidthTestCompleted;
            private set
            {
                this.m_fBandwidthTestCompleted = value;
                this.FirePropertyChanged(nameof(IsBandwidthTestCompleted));
            }
        }

        public BandwidthCapacity BandwidthTestResult
        {
            get => this.m_bandwidthTestResult;
            private set
            {
                this.m_bandwidthTestResult = value;
                this.FirePropertyChanged(nameof(BandwidthTestResult));
            }
        }

        public bool IsContentSDOnly
        {
            get => this.m_fIsContentSDOnly;
            set
            {
                this.m_fIsContentSDOnly = value;
                this.FirePropertyChanged(nameof(IsContentSDOnly));
            }
        }

        public bool IsContentHDOnly
        {
            get => this.m_fIsContentHDOnly;
            set
            {
                this.m_fIsContentHDOnly = value;
                this.FirePropertyChanged(nameof(IsContentHDOnly));
            }
        }

        public void StartBandwidthTest(IList videoOffers)
        {
            if (this.IsBandwidthTestRunning)
                return;
            this.m_videoOffers = videoOffers;
            this.IsBandwidthTestRunning = true;
            this.IsBandwidthTestCanceled = false;
            this.IsBandwidthTestCompleted = false;
            this.IsBandwidthTestErrFound = false;
            this.StartGettingContentUri();
        }

        public void CancelBandwidthTest()
        {
            if (this.m_bandwidthTestInterop == null || !this.IsBandwidthTestRunning)
                return;
            this.m_bandwidthTestInterop.Cancel();
            this.m_bandwidthTestInterop = null;
            this.IsBandwidthTestCanceled = true;
            this.IsBandwidthTestRunning = false;
            if (!DownloadManager.Instance.IsQueuePaused)
                return;
            DownloadManager.Instance.ResumeQueue();
        }

        public void RetryBandwidthTest()
        {
            if (this.m_videoOffers == null)
                return;
            this.m_fIsRetried = true;
            this.StartBandwidthTest(this.m_videoOffers);
        }

        public void LogHDSDChoice(int chosenHDSDChoice)
        {
            if (this.BandwidthTestResult == BandwidthCapacity.None)
                return;
            SQMLog.LogToStream(SQMDataId.BandwidthTestResultAndDecision, (uint)this.BandwidthTestResult, (uint)chosenHDSDChoice);
        }

        private void StartGettingContentUri() => ThreadPool.QueueUserWorkItem(args => Application.DeferredInvoke(new DeferredInvokeHandler(this.OnGotContentUri), this.GetContentUri()), null);

        private string GetContentUri()
        {
            string uriOut = null;
            if (this.m_videoOffers[0] is VideoOffer videoOffer)
            {
                if (!ZuneApplication.Service.InCompleteCollection(videoOffer.Id, EContentType.Video) && !string.IsNullOrEmpty(this.m_uri))
                    uriOut = this.m_uri;
                else
                    ZuneApplication.Service.GetContentUri(videoOffer.Id, EContentType.Video, EContentUriFlags.BypassLicense | EContentUriFlags.IgnoreCollection, videoOffer.IsHD, videoOffer.IsRental, out uriOut);
            }
            return uriOut;
        }

        private void OnGotContentUri(object obj)
        {
            string str = (string)obj;
            if (string.IsNullOrEmpty(str))
            {
                this.IsBandwidthTestErrFound = true;
            }
            else
            {
                this.m_uri = str;
                if (!DownloadManager.Instance.IsQueuePaused)
                    DownloadManager.Instance.PauseQueue();
                this.StartBandwidthTestCore(this.m_uri, ClientConfiguration.GeneralSettings.BandwidthTestTimeoutSec);
            }
        }

        private void StartBandwidthTestCore(string uri, int testTimeout)
        {
            if (string.IsNullOrEmpty(uri))
                return;
            this.IsBandwidthTestErrFound = false;
            this.m_bandwidthTestInterop = new BandwidthTestInterop();
            this.m_bandwidthTestInterop.BandwidthTestUpdate += new BandwidthTestUpdateEventHandler(this.OnBandwidthTestUpdate);
            this.m_bandwidthTestInterop.BandwidthTestError += new BandwidthTestErrorEventHandler(this.OnBandwidthTestError);
            this.m_bandwidthTestInterop.Start(uri, testTimeout);
        }

        private void OnBandwidthTestUpdate(object sender, BandwidthUpdateArgs args) => Application.DeferredInvoke(new DeferredInvokeHandler(this.OnBandwidthTestUpdateOnApp), args);

        private void OnBandwidthTestError(object sender, BandwidthTestErrorArgs args) => Application.DeferredInvoke(new DeferredInvokeHandler(this.OnBandwidthTestErrorOnApp), args);

        private void OnBandwidthTestUpdateOnApp(object obj)
        {
            if (obj == null)
                return;
            BandwidthUpdateArgs bandwidthUpdateArgs = (BandwidthUpdateArgs)obj;
            if (bandwidthUpdateArgs == null)
                return;
            switch (bandwidthUpdateArgs.currentState)
            {
                case MBRHeuristicState.Test_Start:
                    this.BandwidthTestProgressPercent = 5;
                    break;
                case MBRHeuristicState.Test_Inprogress:
                    if (bandwidthUpdateArgs.PercentComplete <= 5)
                        break;
                    this.BandwidthTestProgressPercent = bandwidthUpdateArgs.PercentComplete;
                    break;
                case MBRHeuristicState.Test_Completed:
                    if (DownloadManager.Instance.IsQueuePaused)
                        DownloadManager.Instance.ResumeQueue();
                    this.BandwidthTestResult = bandwidthUpdateArgs.TotalAverageBandwidth < 2800000 ? (bandwidthUpdateArgs.TotalAverageBandwidth < 700000 ? BandwidthCapacity.None : BandwidthCapacity.SDCapable) : BandwidthCapacity.HDCapable;
                    this.BandwidthTestProgressPercent = 100;
                    this.IsBandwidthTestCompleted = true;
                    this.IsBandwidthTestRunning = false;
                    this.m_bandwidthTestInterop = null;
                    if (this.BandwidthTestResult == BandwidthCapacity.None)
                        SQMLog.Log(SQMDataId.BandwidthTestDoesnotMeetMin, 1);
                    if (this.m_fIsRetried && this.BandwidthTestResult != this.m_lastTestResult)
                        SQMLog.Log(SQMDataId.BandwidthTestRetryDifferentResult, 1);
                    this.m_lastTestResult = this.BandwidthTestResult;
                    break;
            }
        }

        private void OnBandwidthTestErrorOnApp(object obj)
        {
            this.IsBandwidthTestErrFound = true;
            this.IsBandwidthTestRunning = false;
            if (DownloadManager.Instance.IsQueuePaused)
                DownloadManager.Instance.ResumeQueue();
            this.m_bandwidthTestInterop = null;
            if (((BandwidthTestErrorArgs)obj).ErrorCode != -2147023436)
                return;
            ShipAssert.Assert(false);
            SQMLog.Log(SQMDataId.BandwidthTestTimeout, 1);
        }
    }
}
