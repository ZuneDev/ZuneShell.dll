// Decompiled with JetBrains decompiler
// Type: ZuneUI.TrackAttachment
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;
using System;

namespace ZuneUI
{
    public class TrackAttachment : Attachment
    {
        public const string RequestTypeString = "song";

        public TrackAttachment(Guid id, string title, string artist, string imageUri)
          : base(id, title, artist, imageUri)
        {
            this.AttachmentUI = "res://ZuneShellResources!SocialComposer.uix#TrackAttachmentUI";
            this.Description = Shell.LoadString(StringId.IDS_COMPOSE_MESSAGE_TRACK_ATTACHMENT);
        }

        public override MediaType MediaType => MediaType.Track;

        public override string RequestType => "song";

        public override void LogSend() => SQMLog.Log(SQMDataId.InboxMessageSendTrack, 1);
    }
}
