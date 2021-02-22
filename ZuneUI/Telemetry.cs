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
                if (Telemetry.m_instance == null)
                    Telemetry.m_instance = new Telemetry();
                return Telemetry.m_instance;
            }
        }

        private Telemetry()
        {
            this.m_queue = new Queue<TelemetryInfo>();
            this.m_sqmToEventMap = new Hashtable();
            this.m_sqmToEventMap[(object)SQMDataId.AutoPlaylistCreations] = (object)new DatapointInfo(ETelemetryEvent.eTelemetryEventBuildPlaylist);
            this.m_sqmToEventMap[(object)SQMDataId.NowPlayingClicks] = (object)new DatapointInfo(ETelemetryEvent.eTelemetryEventNowPlaying);
            this.m_sqmToEventMap[(object)SQMDataId.QuickMixPlaylistCreates] = (object)new DatapointInfo(ETelemetryEvent.eTelemetryEventQuickMix);
            this.m_sqmToEventMap[(object)SQMDataId.MixViewTime] = (object)new DatapointInfo(ETelemetryEvent.eTelemetryEventMixView, "MixViewTime", true);
            this.m_sqmToEventMap[(object)SQMDataId.NowPlayingMusicViewTime] = (object)new DatapointInfo(ETelemetryEvent.eTelemetryEventPlayback, "PlaybackAudio", true);
            this.m_sqmToEventMap[(object)SQMDataId.NowPlayingVideoViewTime] = (object)new DatapointInfo(ETelemetryEvent.eTelemetryEventPlayback, "PlaybackVideo", true);
            this.m_sqmToEventMap[(object)SQMDataId.NowPlayingPhotoViewTime] = (object)new DatapointInfo(ETelemetryEvent.eTelemetryEventPlayback, "PlaybackPhoto", true);
        }

        internal void StartUpload()
        {
            lock (this.m_queue)
            {
                this.m_uploadAllowed = true;
                if (this.m_queue.Count <= 0)
                    return;
                Application.DeferredInvoke(new DeferredInvokeHandler(this.ProcessQueue), (object)null, DeferredInvokePriority.Low);
            }
        }

        private Hashtable FilterPageArgs(IDictionary args)
        {
            Hashtable hashtable = new Hashtable();
            if (args != null)
            {
                foreach (DictionaryEntry dictionaryEntry in args)
                {
                    if (dictionaryEntry.Key is string && Array.IndexOf<string>(Telemetry._supportedTags, (string)dictionaryEntry.Key) != -1 && (dictionaryEntry.Value is string || dictionaryEntry.Value is Guid || dictionaryEntry.Value is int))
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
            this.QueueTelemetry(new TelemetryInfo(ETelemetryEvent.eTelemetryEventUndefined, command, (IDictionary)hashtable, false));
        }

        public void ReportPlaybackTime(int timeData)
        {
            if (timeData <= 0)
                return;
            this.QueueTelemetry(new TelemetryInfo(ETelemetryEvent.eTelemetryEventPlayback, "", (IDictionary)new Hashtable()
      {
        {
          (object) "@Data",
          (object) timeData
        },
        {
          (object) "@EventType",
          (object) "CumulativePlaybackTime"
        }
      }, true));
        }

        public void ReportEvent(SQMDataPoint datapoint, int nData)
        {
            SQMDataId id = datapoint.id;
            bool flag = datapoint.action == SQMAction.Add || datapoint.action == SQMAction.Inc;
            if (id == SQMDataId.Invalid || !flag || !this.m_sqmToEventMap.ContainsKey((object)id))
                return;
            Hashtable hashtable = new Hashtable();
            DatapointInfo sqmToEvent = (DatapointInfo)this.m_sqmToEventMap[(object)id];
            if (sqmToEvent.IsSession)
            {
                hashtable.Add((object)"@Data", (object)nData);
                hashtable.Add((object)"@EventType", (object)sqmToEvent.TypeName);
            }
            this.QueueTelemetry(new TelemetryInfo(sqmToEvent.Event, "", (IDictionary)hashtable, sqmToEvent.IsSession));
        }

        public void ReportSearch(string search) => this.QueueTelemetry(new TelemetryInfo(ETelemetryEvent.eTelemetryEventUndefined, "Search", (IDictionary)new Hashtable()
    {
      {
        (object) "zune_query",
        (object) search
      }
    }, false));

        public void ReportPageLoad(string pageUri, int pageLoadTime, IDictionary args)
        {
            if (string.IsNullOrEmpty(pageUri))
                return;
            string uri = "PageLoadTime";
            Hashtable hashtable = this.FilterPageArgs(args);
            hashtable.Add((object)pageUri, (object)pageLoadTime);
            this.QueueTelemetry(new TelemetryInfo(ETelemetryEvent.eTelemetryEventUndefined, uri, (IDictionary)hashtable, false));
        }

        private void QueueTelemetry(TelemetryInfo info)
        {
            lock (this.m_queue)
            {
                this.m_queue.Enqueue(info);
                if (!this.m_uploadAllowed || this.m_queue.Count != 1)
                    return;
                Application.DeferredInvoke(new DeferredInvokeHandler(this.ProcessQueue), (object)null, DeferredInvokePriority.Low);
            }
        }

        private void ProcessQueue(object state)
        {
            TelemetryInfo telemetryInfo = (TelemetryInfo)null;
            bool flag;
            lock (this.m_queue)
            {
                if (this.m_queue.Count > 0)
                    telemetryInfo = this.m_queue.Dequeue();
                flag = this.m_queue.Count > 0;
            }
            if (telemetryInfo != null)
                this.SendTelemetry((object)telemetryInfo);
            if (!flag)
                return;
            Application.DeferredInvoke(new DeferredInvokeHandler(this.ProcessQueue), (object)null, DeferredInvokePriority.Low);
        }

        private void SendTelemetryInfo(TelemetryInfo info) => Application.DeferredInvoke(new DeferredInvokeHandler(this.SendTelemetry), (object)info, DeferredInvokePriority.Low);

        private void SendTelemetry(object state)
        {
            TelemetryInfo telemetryInfo = (TelemetryInfo)state;
            telemetryInfo.Args.Add((object)"@epochTime", (object)telemetryInfo.elapsedTime);
            if (telemetryInfo.fSessionDatapoint)
            {
                int int32 = Convert.ToInt32(telemetryInfo.Args[(object)"@Data"]);
                string key = telemetryInfo.Args[(object)"@EventType"].ToString();
                TelemetryAPI.AddToSessionEvent(telemetryInfo.eEvent, key, int32);
            }
            else if (telemetryInfo.eEvent == ETelemetryEvent.eTelemetryEventUndefined)
                TelemetryAPI.SendDatapoint(telemetryInfo.dcsUri, telemetryInfo.Args);
            else if (telemetryInfo.Args.Contains((object)"@EventType"))
                TelemetryAPI.SendEvent(telemetryInfo.eEvent, telemetryInfo.Args[(object)"@EventType"].ToString());
            else
                TelemetryAPI.SendEvent(telemetryInfo.eEvent, (string)null);
        }
    }
}
