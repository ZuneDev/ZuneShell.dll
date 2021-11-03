// Decompiled with JetBrains decompiler
// Type: ZuneUI.Telemetry
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public class Telemetry
    {
        private const string sc_EpochTime = "@epochTime";
        private const string sc_SessionEventType = "@EventType";
        private const string sc_SessionData = "@Data";
        private static Telemetry m_instance;
        private Queue<TelemetryInfo> m_queue;
        private bool m_uploadAllowed;
        private static string[] _supportedTags = new string[25]
        {
      "AlbumId",
      "ArtistId",
      "CategoryId",
      "ChannelId",
      "ChannelUrl",
      "EpisodeId",
      "AppId",
      "GenreId",
      "MovieId",
      "MovieTrailerId",
      "NetworkId",
      "PlaylistId",
      "PodcastId",
      "SelectedPivot",
      "SeasonId",
      "SeriesId",
      "ShortId",
      "SubGenreId",
      "VideoId",
      "TrackId",
      "HubId",
      "PlaylistCategoryId",
      "Context",
      "SelectionId",
      "SelectionTitle"
        };
        private Hashtable m_sqmToEventMap;

        public static Telemetry Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new Telemetry();
                return m_instance;
            }
        }

        private Telemetry()
        {
            this.m_queue = new Queue<TelemetryInfo>();
            this.m_sqmToEventMap = new Hashtable();
            this.m_sqmToEventMap[SQMDataId.AutoPlaylistCreations] = new DatapointInfo(ETelemetryEvent.eTelemetryEventBuildPlaylist);
            this.m_sqmToEventMap[SQMDataId.NowPlayingClicks] = new DatapointInfo(ETelemetryEvent.eTelemetryEventNowPlaying);
            this.m_sqmToEventMap[SQMDataId.QuickMixPlaylistCreates] = new DatapointInfo(ETelemetryEvent.eTelemetryEventQuickMix);
            this.m_sqmToEventMap[SQMDataId.MixViewTime] = new DatapointInfo(ETelemetryEvent.eTelemetryEventMixView, "MixViewTime", true);
            this.m_sqmToEventMap[SQMDataId.NowPlayingMusicViewTime] = new DatapointInfo(ETelemetryEvent.eTelemetryEventPlayback, "PlaybackAudio", true);
            this.m_sqmToEventMap[SQMDataId.NowPlayingVideoViewTime] = new DatapointInfo(ETelemetryEvent.eTelemetryEventPlayback, "PlaybackVideo", true);
            this.m_sqmToEventMap[SQMDataId.NowPlayingPhotoViewTime] = new DatapointInfo(ETelemetryEvent.eTelemetryEventPlayback, "PlaybackPhoto", true);
        }

        internal void StartUpload()
        {
            lock (this.m_queue)
            {
                this.m_uploadAllowed = true;
                if (this.m_queue.Count <= 0)
                    return;
                Application.DeferredInvoke(new DeferredInvokeHandler(this.ProcessQueue), null, DeferredInvokePriority.Low);
            }
        }

        private Hashtable FilterPageArgs(IDictionary args)
        {
            Hashtable hashtable = new Hashtable();
            if (args != null)
            {
                foreach (DictionaryEntry dictionaryEntry in args)
                {
                    if (dictionaryEntry.Key is string && Array.IndexOf(_supportedTags, (string)dictionaryEntry.Key) != -1 && (dictionaryEntry.Value is string || dictionaryEntry.Value is Guid || dictionaryEntry.Value is int))
                        hashtable.Add(dictionaryEntry.Key, dictionaryEntry.Value);
                }
            }
            return hashtable;
        }

        public void ReportNavigation(string command, IDictionary args)
        {
            if (command == null)
                return;
            Hashtable hashtable = this.FilterPageArgs(args);
            this.QueueTelemetry(new TelemetryInfo(ETelemetryEvent.eTelemetryEventUndefined, command, hashtable, false));
        }

        public void ReportPlaybackTime(int timeData)
        {
            if (timeData <= 0)
                return;
            this.QueueTelemetry(new TelemetryInfo(ETelemetryEvent.eTelemetryEventPlayback, "", new Hashtable()
      {
        {
           "@Data",
           timeData
        },
        {
           "@EventType",
           "CumulativePlaybackTime"
        }
      }, true));
        }

        public void ReportEvent(SQMDataPoint datapoint, int nData)
        {
            SQMDataId id = datapoint.id;
            bool flag = datapoint.action == SQMAction.Add || datapoint.action == SQMAction.Inc;
            if (id == SQMDataId.Invalid || !flag || !this.m_sqmToEventMap.ContainsKey(id))
                return;
            Hashtable hashtable = new Hashtable();
            DatapointInfo sqmToEvent = (DatapointInfo)this.m_sqmToEventMap[id];
            if (sqmToEvent.IsSession)
            {
                hashtable.Add("@Data", nData);
                hashtable.Add("@EventType", sqmToEvent.TypeName);
            }
            this.QueueTelemetry(new TelemetryInfo(sqmToEvent.Event, "", hashtable, sqmToEvent.IsSession));
        }

        public void ReportSearch(string search) => this.QueueTelemetry(new TelemetryInfo(ETelemetryEvent.eTelemetryEventUndefined, "Search", new Hashtable()
    {
      {
         "zune_query",
         search
      }
    }, false));

        public void ReportPageLoad(string pageUri, int pageLoadTime, IDictionary args)
        {
            if (string.IsNullOrEmpty(pageUri))
                return;
            string uri = "PageLoadTime";
            Hashtable hashtable = this.FilterPageArgs(args);
            hashtable.Add(pageUri, pageLoadTime);
            this.QueueTelemetry(new TelemetryInfo(ETelemetryEvent.eTelemetryEventUndefined, uri, hashtable, false));
        }

        private void QueueTelemetry(TelemetryInfo info)
        {
            lock (this.m_queue)
            {
                this.m_queue.Enqueue(info);
                if (!this.m_uploadAllowed || this.m_queue.Count != 1)
                    return;
                Application.DeferredInvoke(new DeferredInvokeHandler(this.ProcessQueue), null, DeferredInvokePriority.Low);
            }
        }

        private void ProcessQueue(object state)
        {
            TelemetryInfo telemetryInfo = null;
            bool flag;
            lock (this.m_queue)
            {
                if (this.m_queue.Count > 0)
                    telemetryInfo = this.m_queue.Dequeue();
                flag = this.m_queue.Count > 0;
            }
            if (telemetryInfo != null)
                this.SendTelemetry(telemetryInfo);
            if (!flag)
                return;
            Application.DeferredInvoke(new DeferredInvokeHandler(this.ProcessQueue), null, DeferredInvokePriority.Low);
        }

        private void SendTelemetryInfo(TelemetryInfo info) => Application.DeferredInvoke(new DeferredInvokeHandler(this.SendTelemetry), info, DeferredInvokePriority.Low);

        private void SendTelemetry(object state)
        {
            TelemetryInfo telemetryInfo = (TelemetryInfo)state;
            telemetryInfo.Args.Add("@epochTime", telemetryInfo.elapsedTime);
            if (telemetryInfo.fSessionDatapoint)
            {
                int int32 = Convert.ToInt32(telemetryInfo.Args["@Data"]);
                string key = telemetryInfo.Args["@EventType"].ToString();
                TelemetryAPI.AddToSessionEvent(telemetryInfo.eEvent, key, int32);
            }
            else if (telemetryInfo.eEvent == ETelemetryEvent.eTelemetryEventUndefined)
                TelemetryAPI.SendDatapoint(telemetryInfo.dcsUri, telemetryInfo.Args);
            else if (telemetryInfo.Args.Contains("@EventType"))
                TelemetryAPI.SendEvent(telemetryInfo.eEvent, telemetryInfo.Args["@EventType"].ToString());
            else
                TelemetryAPI.SendEvent(telemetryInfo.eEvent, null);
        }
    }
}
