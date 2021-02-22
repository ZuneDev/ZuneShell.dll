// Decompiled with JetBrains decompiler
// Type: ZuneUI.PlaylistAttachment
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;
using System;

namespace ZuneUI
{
    public class PlaylistAttachment : Attachment
    {
        public const string RequestTypeString = "playlist";

        public PlaylistAttachment(Guid id, string title, string imageUri)
          : base(id, title, (string)null, imageUri)
        {
            this.AttachmentUI = "res://ZuneShellResources!SocialComposer.uix#PlaylistAttachmentUI";
            this.Description = Shell.LoadString(StringId.IDS_COMPOSE_MESSAGE_PLAYLIST_ATTACHMENT);
        }

        public override MediaType MediaType => MediaType.Playlist;

        public override string RequestType => "playlist";

        public override void LogSend() => SQMLog.Log(SQMDataId.InboxMessageSendPlaylist, 1);
    }
}
