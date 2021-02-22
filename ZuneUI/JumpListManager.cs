// Decompiled with JetBrains decompiler
// Type: ZuneUI.JumpListManager
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace ZuneUI
{
    public class JumpListManager : SingletonModelItem<JumpListManager>
    {
        private const int _maxPins = 6;
        private const int _maxQuickMixes = 5;
        private const int _normalIconIndex = -12;
        private const int _quickMixIconIndex = -13;
        private Command _updatePins;
        private IList _pinList;
        private IList _quickMixList;
        private bool _pinListsHaveBeenSetAtLeastOnce;
        private bool _finalCommitHasBeenCompleted;
        private bool _showResumeNowPlaying;
        private bool _currentPlaylistWillBeSaved;
        private bool _transportControlsHavePlaylist;
        private string _resumeNowPlayingName;
        private string _shuffleAllName;
        private string _pinCategoryName;
        private string _quickMixCategoryName;
        private static int _lastUpdateJobCookie = 0;
        private static object _updateJobLock = new object();

        public JumpListManager()
          : base((IModelItemOwner)SingletonModelItem<TransportControls>.Instance)
        {
            this._updatePins = new Command((IModelItemOwner)this);
            this._pinListsHaveBeenSetAtLeastOnce = false;
            this._finalCommitHasBeenCompleted = false;
            this._showResumeNowPlaying = false;
            this._resumeNowPlayingName = ZuneUI.Shell.LoadString(StringId.IDS_JUMP_LIST_RESUME_NOWPLAYING);
            this._shuffleAllName = ZuneUI.Shell.LoadString(StringId.IDS_JUMP_LIST_SHUFFLE_ALL);
            this._pinCategoryName = ZuneUI.Shell.LoadString(StringId.IDS_JUMP_LIST_QUICKPLAY_CATEGORY);
            this._quickMixCategoryName = ZuneUI.Shell.LoadString(StringId.IDS_JUMP_LIST_QUICKMIX_CATEGORY);
            SingletonModelItem<TransportControls>.Instance.PropertyChanged += new PropertyChangedEventHandler(this.OnTransportControlsPropertyChanged);
            ZuneApplication.Closing += new EventHandler(this.OnApplicationClosing);
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                ZuneApplication.Closing -= new EventHandler(this.OnApplicationClosing);
                SingletonModelItem<TransportControls>.Instance.PropertyChanged -= new PropertyChangedEventHandler(this.OnTransportControlsPropertyChanged);
            }
            base.OnDispose(disposing);
        }

        public Command JumpListPinUpdateRequested => this._updatePins;

        public void SetPins(IList pinList, IList quickMixList)
        {
            this._pinList = pinList;
            this._quickMixList = quickMixList;
            this._pinListsHaveBeenSetAtLeastOnce = true;
            this.UpdateJumpList();
        }

        public static void PlayPin(JumpListPin pin)
        {
            if (pin == null)
                return;
            SingletonModelItem<TransportControls>.Instance.RequestedJumpListPin = pin;
        }

        private void OnTransportControlsPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "HasPlaylist" || args.PropertyName == "Playing")
            {
                TransportControls instance = SingletonModelItem<TransportControls>.Instance;
                bool flag = instance.HasPlaylist && !instance.Playing;
                if (this._showResumeNowPlaying != flag)
                {
                    this._showResumeNowPlaying = flag;
                    this.UpdateJumpList();
                }
                this._transportControlsHavePlaylist = instance.HasPlaylist;
            }
            else
            {
                if (!(args.PropertyName == "CurrentPlaylist"))
                    return;
                this._currentPlaylistWillBeSaved = SingletonModelItem<TransportControls>.Instance.WillSaveCurrentPlaylistOnShutdown();
            }
        }

        private void OnApplicationClosing(object sender, EventArgs args)
        {
            if (!this._currentPlaylistWillBeSaved)
                this._showResumeNowPlaying = false;
            else if (this._transportControlsHavePlaylist)
                this._showResumeNowPlaying = true;
            this.UpdateJumpList();
            this._finalCommitHasBeenCompleted = true;
        }

        private void UpdateJumpList()
        {
            if (!OSVersion.IsWin7() || !this._pinListsHaveBeenSetAtLeastOnce || this._finalCommitHasBeenCompleted)
                return;
            JumpListManager.UpdateJumpListJob updateJumpListJob = new JumpListManager.UpdateJumpListJob();
            updateJumpListJob.Cookie = Interlocked.Increment(ref JumpListManager._lastUpdateJobCookie);
            updateJumpListJob.ShowResumeNowPlaying = this._showResumeNowPlaying;
            updateJumpListJob.ResumeNowPlayingName = this._resumeNowPlayingName;
            updateJumpListJob.ShuffleAllName = this._shuffleAllName;
            updateJumpListJob.PinCategoryName = this._pinCategoryName;
            updateJumpListJob.QuickMixCategoryName = this._quickMixCategoryName;
            updateJumpListJob.PinList = new JumpListPin[this._pinList.Count];
            for (int index = 0; index < this._pinList.Count; ++index)
                updateJumpListJob.PinList[index] = new JumpListPin((JumpListPin)this._pinList[index]);
            updateJumpListJob.QuickMixList = new JumpListPin[this._quickMixList.Count];
            for (int index = 0; index < this._quickMixList.Count; ++index)
                updateJumpListJob.QuickMixList[index] = new JumpListPin((JumpListPin)this._quickMixList[index]);
            ThreadPool.QueueUserWorkItem(new WaitCallback(JumpListManager.UpdateJumpListWorker), (object)updateJumpListJob);
        }

        private static void UpdateJumpListWorker(object data)
        {
            if (!(data is JumpListManager.UpdateJumpListJob updateJumpListJob))
                return;
            lock (JumpListManager._updateJobLock)
            {
                if (JumpListManager._lastUpdateJobCookie != updateJumpListJob.Cookie)
                    return;
                List<JumpListEntry> disallowedDestinationList = (List<JumpListEntry>)null;
                JumpListSession session;
                HRESULT hresult = (HRESULT)Win7ShellManager.Instance.BeginJumpListSession(out session);
                if (hresult.IsSuccess)
                    hresult = (HRESULT)session.GetDisallowedDestinations(out disallowedDestinationList);
                if (hresult.IsSuccess)
                {
                    List<string> disallowedCommandLineArguments = disallowedDestinationList.ConvertAll<string>((Converter<JumpListEntry, string>)(rawEntry => rawEntry.CommandLineArguments));
                    JumpListEntry task;
                    if (updateJumpListJob.ShowResumeNowPlaying)
                    {
                        session.CreateTask(out task);
                        task.Name = updateJumpListJob.ResumeNowPlayingName;
                        task.CommandLineArguments = "/resumenowplaying";
                        task.IconIndex = -12;
                    }
                    session.CreateTask(out task);
                    task.Name = updateJumpListJob.ShuffleAllName;
                    task.CommandLineArguments = "/shuffleall";
                    task.IconIndex = -12;
                    JumpListCategory category;
                    if (updateJumpListJob.PinList != null && updateJumpListJob.PinList.Length > 0)
                    {
                        session.CreateCategory(out category);
                        category.Name = updateJumpListJob.PinCategoryName;
                        for (int index = 0; index < 6 && index < updateJumpListJob.PinList.Length; ++index)
                            JumpListManager.PopulateJumpListIfPossible(category, updateJumpListJob.PinList[index], -12, disallowedCommandLineArguments);
                    }
                    if (updateJumpListJob.QuickMixList != null && updateJumpListJob.QuickMixList.Length > 0)
                    {
                        session.CreateCategory(out category);
                        category.Name = updateJumpListJob.QuickMixCategoryName;
                        for (int index = 0; index < 5 && index < updateJumpListJob.QuickMixList.Length; ++index)
                            JumpListManager.PopulateJumpListIfPossible(category, updateJumpListJob.QuickMixList[index], -13, disallowedCommandLineArguments);
                    }
                    session.Commit();
                }
                else
                    session?.Cancel();
            }
        }

        private static void PopulateJumpListIfPossible(
          JumpListCategory category,
          JumpListPin pin,
          int iconIndex,
          List<string> disallowedCommandLineArguments)
        {
            string str = string.Format("/playpin:{0}", (object)pin.ToString());
            if (disallowedCommandLineArguments.Contains(str))
                return;
            JumpListEntry destination;
            category.CreateDestination(out destination);
            destination.Name = pin.Name;
            destination.CommandLineArguments = str;
            destination.IconIndex = iconIndex;
        }

        private class UpdateJumpListJob
        {
            public int Cookie;
            public bool ShowResumeNowPlaying;
            public string ResumeNowPlayingName;
            public string ShuffleAllName;
            public string PinCategoryName;
            public string QuickMixCategoryName;
            public JumpListPin[] PinList;
            public JumpListPin[] QuickMixList;
        }
    }
}
