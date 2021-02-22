// Decompiled with JetBrains decompiler
// Type: ZuneUI.ThumbBarButtons
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;
using System;
using System.ComponentModel;
using System.Threading;

namespace ZuneUI
{
    public class ThumbBarButtons : SingletonModelItem<ThumbBarButtons>
    {
        private ThumbBar _thumbBar;
        private ThumbBarButton _playPause;
        private ThumbBarButton _previous;
        private ThumbBarButton _next;
        private ThumbBarButton _rating;
        private int _lastUpdateJobCookie;
        private object _updateJobLock = new object();
        private bool _thumbBarInitializationComplete;
        private static string s_playTooltip = Shell.LoadString(StringId.IDS_PLAY);
        private static string s_pauseTooltip = Shell.LoadString(StringId.IDS_PAUSE);
        private static string s_unratedTooltip = Shell.LoadString(StringId.IDS_NOWPLAYING_UNRATED_TOOLTIP);
        private static string s_loveItTooltip = Shell.LoadString(StringId.IDS_NOWPLAYING_LIKEIT_TOOLTIP);
        private static string s_hateItTooltip = Shell.LoadString(StringId.IDS_NOWPLAYING_DONTLIKEIT_TOOLTIP);

        public void Phase3Init()
        {
            TransportControls instance = SingletonModelItem<TransportControls>.Instance;
            instance.PropertyChanged += new PropertyChangedEventHandler(this.OnTransportControlsPropertyChanged);
            instance.Play.PropertyChanged += new PropertyChangedEventHandler(this.OnTransportControlsCommandPropertyChanged);
            instance.Pause.PropertyChanged += new PropertyChangedEventHandler(this.OnTransportControlsCommandPropertyChanged);
            instance.Back.PropertyChanged += new PropertyChangedEventHandler(this.OnTransportControlsCommandPropertyChanged);
            instance.Forward.PropertyChanged += new PropertyChangedEventHandler(this.OnTransportControlsCommandPropertyChanged);
            string previousText = Shell.LoadString(StringId.IDS_PREVIOUS);
            string nextText = Shell.LoadString(StringId.IDS_NEXT);
            IntPtr windowHandle = Application.Window.Handle;
            ThreadPool.QueueUserWorkItem((WaitCallback)delegate
           {
               Win7ShellManager.Instance.BeginThumbBarSession(windowHandle, out this._thumbBar);
               Win7ShellManager.Instance.OnThumbBarButtonPress += new ThumbBarButtonPressHandler(this.OnThumbButtonPressed);
               ThumbBarButton button;
               this._thumbBar.CreateButton(out button);
               button.UniqueID = 4U;
               button.IsHidden = true;
               this._thumbBar.CreateButton(out this._previous);
               this._previous.UniqueID = 1U;
               this._previous.Tooltip = previousText;
               this._thumbBar.CreateButton(out this._playPause);
               this._playPause.UniqueID = 0U;
               this._thumbBar.CreateButton(out this._next);
               this._next.UniqueID = 2U;
               this._next.Tooltip = nextText;
               this._thumbBar.CreateButton(out this._rating);
               this._rating.UniqueID = 3U;
               Application.DeferredInvoke((DeferredInvokeHandler)delegate
         {
                 this._thumbBarInitializationComplete = true;
                 this.UpdateButtonStatus();
             }, (object)null);
           }, (object)null);
        }

        private bool IsCurrentTrackRatable()
        {
            PlaybackTrack currentTrack = SingletonModelItem<TransportControls>.Instance.CurrentTrack;
            return currentTrack != null && currentTrack.CanRate;
        }

        private void OnTransportControlsPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (!(args.PropertyName == "CurrentTrackRating") && !(args.PropertyName == "Playing"))
                return;
            this.UpdateButtonStatus();
        }

        private void OnTransportControlsCommandPropertyChanged(
          object sender,
          PropertyChangedEventArgs args)
        {
            if (!(args.PropertyName == "Available"))
                return;
            this.UpdateButtonStatus();
        }

        private void OnThumbButtonPressed(uint uniqueID) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           TransportControls instance = SingletonModelItem<TransportControls>.Instance;
           switch (uniqueID)
           {
               case 0:
                   if (instance.Playing)
                   {
                       instance.Pause.Invoke();
                       break;
                   }
                   instance.Play.Invoke();
                   break;
               case 1:
                   instance.Back.Invoke();
                   break;
               case 2:
                   instance.Forward.Invoke();
                   break;
               case 3:
                   if (this.IsCurrentTrackRatable())
                   {
                       PlaybackTrack currentTrack = instance.CurrentTrack;
                       currentTrack.UserRating = currentTrack.UserRating != 0 ? (currentTrack.UserRating > 5 ? 2 : 0) : 8;
                   }
                   this.UpdateButtonStatus();
                   break;
           }
       }, (object)null);

        private void UpdateButtonStatus()
        {
            if (!this._thumbBarInitializationComplete)
                return;
            TransportControls instance = SingletonModelItem<TransportControls>.Instance;
            ThumbBarButtons.UpdateThumbBarJob updateThumbBarJob = new ThumbBarButtons.UpdateThumbBarJob();
            updateThumbBarJob.Cookie = Interlocked.Increment(ref this._lastUpdateJobCookie);
            updateThumbBarJob.IsPlaying = instance.Playing;
            updateThumbBarJob.CanPlay = instance.Play.Available;
            updateThumbBarJob.CanPause = instance.Pause.Available;
            updateThumbBarJob.CanGoBack = instance.Back.Available;
            updateThumbBarJob.CanGoForward = instance.Forward.Available;
            updateThumbBarJob.IsCurrentTrackRatable = this.IsCurrentTrackRatable();
            if (instance.CurrentTrack != null)
                updateThumbBarJob.CurrentTrackRating = instance.CurrentTrack.UserRating;
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.UpdateButtonStatusWorker), (object)updateThumbBarJob);
        }

        private void UpdateButtonStatusWorker(object data)
        {
            if (!(data is ThumbBarButtons.UpdateThumbBarJob updateThumbBarJob))
                return;
            lock (this._updateJobLock)
            {
                if (this._lastUpdateJobCookie != updateThumbBarJob.Cookie)
                    return;
                if (updateThumbBarJob.IsPlaying)
                {
                    this._playPause.IsEnabled = updateThumbBarJob.CanPause;
                    this._playPause.Tooltip = ThumbBarButtons.s_pauseTooltip;
                    this._playPause.Icon = ThumbBarButtonIcons.eThumbBarButtonIconPause;
                }
                else
                {
                    this._playPause.IsEnabled = updateThumbBarJob.CanPlay;
                    this._playPause.Tooltip = ThumbBarButtons.s_playTooltip;
                    this._playPause.Icon = updateThumbBarJob.CanPlay ? ThumbBarButtonIcons.eThumbBarButtonIconPlay : ThumbBarButtonIcons.eThumbBarButtonIconPlayDisabled;
                }
                this._previous.IsEnabled = updateThumbBarJob.CanGoBack;
                this._previous.Icon = updateThumbBarJob.CanGoBack ? ThumbBarButtonIcons.eThumbBarButtonIconPrevious : ThumbBarButtonIcons.eThumbBarButtonIconPreviousDisabled;
                this._next.IsEnabled = updateThumbBarJob.CanGoForward;
                this._next.Icon = updateThumbBarJob.CanGoForward ? ThumbBarButtonIcons.eThumbBarButtonIconNext : ThumbBarButtonIcons.eThumbBarButtonIconNextDisabled;
                if (!updateThumbBarJob.IsCurrentTrackRatable)
                {
                    this._rating.IsHidden = true;
                }
                else
                {
                    this._rating.IsHidden = false;
                    if (updateThumbBarJob.CurrentTrackRating == 0)
                    {
                        this._rating.Tooltip = ThumbBarButtons.s_unratedTooltip;
                        this._rating.Icon = ThumbBarButtonIcons.eThumbBarButtonIconUnrated;
                    }
                    else if (updateThumbBarJob.CurrentTrackRating <= 5)
                    {
                        this._rating.Tooltip = ThumbBarButtons.s_hateItTooltip;
                        this._rating.Icon = ThumbBarButtonIcons.eThumbBarButtonIconHateIt;
                    }
                    else
                    {
                        this._rating.Tooltip = ThumbBarButtons.s_loveItTooltip;
                        this._rating.Icon = ThumbBarButtonIcons.eThumbBarButtonIconLoveIt;
                    }
                }
                this._thumbBar.UpdateThumbBar();
            }
        }

        private enum ThumbButtonID : uint
        {
            PlayPause,
            Previous,
            Next,
            Rating,
            Spacer,
        }

        private class UpdateThumbBarJob
        {
            public int Cookie;
            public bool IsPlaying;
            public bool CanPlay;
            public bool CanPause;
            public bool CanGoForward;
            public bool CanGoBack;
            public bool IsCurrentTrackRatable;
            public int CurrentTrackRating;
        }
    }
}
