// Decompiled with JetBrains decompiler
// Type: ZuneUI.VideoAttachment
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;
using System;

namespace ZuneUI
{
    public abstract class VideoAttachment : Attachment
    {
        public VideoAttachment(Guid id, string title, string subtitle, string imageUri)
          : base(id, title, subtitle, imageUri)
        {
        }

        public override MediaType MediaType => MediaType.Video;

        public override void LogSend() => SQMLog.Log(SQMDataId.InboxMessageSendVideo, 1);
    }
}
