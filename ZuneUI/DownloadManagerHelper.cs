// Decompiled with JetBrains decompiler
// Type: ZuneUI.DownloadManagerHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;
using System;
using System.Collections;

namespace ZuneUI
{
    public static class DownloadManagerHelper
    {
        public static void RetryDownloading(IList items)
        {
            if (items == null || items.Count == 0)
                return;
            ArrayList arrayList1 = new ArrayList(items.Count);
            ArrayList arrayList2 = new ArrayList(items.Count);
            Microsoft.Zune.Service.EDownloadFlags eDownloadFlags = Microsoft.Zune.Service.EDownloadFlags.None;
            string str = null;
            for (int index = 0; index < items.Count; ++index)
            {
                if (items[index] is DownloadTask task)
                {
                    switch (task.GetState())
                    {
                        case EDownloadTaskState.DLTaskCancelled:
                        case EDownloadTaskState.DLTaskFailed:
                            switch (Microsoft.Zune.Service.Service.Instance.GetContentType(task.GetProperty("Type")))
                            {
                                case Microsoft.Zune.Service.EContentType.MusicTrack:
                                case Microsoft.Zune.Service.EContentType.Video:
                                case Microsoft.Zune.Service.EContentType.App:
                                    Microsoft.Zune.Service.EDownloadFlags propertyInt = (Microsoft.Zune.Service.EDownloadFlags)task.GetPropertyInt("DownloadFlags");
                                    string property = task.GetProperty("DeviceEndpointId");
                                    bool flag1 = index == 0;
                                    bool flag2 = eDownloadFlags == propertyInt;
                                    bool flag3 = string.Equals(str, property, StringComparison.InvariantCultureIgnoreCase);
                                    if (flag1 || flag2 && flag3)
                                    {
                                        eDownloadFlags = propertyInt;
                                        str = property;
                                        arrayList2.Add(task);
                                        DownloadManager.Instance.RemoveFailed(task);
                                        continue;
                                    }
                                    arrayList1.Add(task);
                                    continue;
                                case Microsoft.Zune.Service.EContentType.PodcastEpisode:
                                    EpisodeDownloadCommand.DownloadEpisode(task.GetPropertyInt("SubscriptionId"), task.GetPropertyInt("MediaId"));
                                    DownloadManager.Instance.RemoveFailed(task);
                                    continue;
                                default:
                                    continue;
                            }
                        default:
                            continue;
                    }
                }
            }
            if (arrayList2.Count > 0)
                Download.Instance.DownloadContent(arrayList2, eDownloadFlags, str);
            RetryDownloading(arrayList1);
        }
    }
}
