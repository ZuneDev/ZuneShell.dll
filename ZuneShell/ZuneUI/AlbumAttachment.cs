// Decompiled with JetBrains decompiler
// Type: ZuneUI.AlbumAttachment
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;
using System;

namespace ZuneUI
{
    public class AlbumAttachment : Attachment
    {
        public const string RequestTypeString = "album";

        public AlbumAttachment(Guid id, string title, string artist, string imageUri, Guid imageId)
          : base(id, title, artist, UrlHelper.MakeCatalogImageUri(imageId))
        {
            this.AttachmentUI = "res://ZuneShellResources!SocialComposer.uix#AlbumAttachmentUI";
            this.Description = Shell.LoadString(StringId.IDS_COMPOSE_MESSAGE_ALBUM_ATTACHMENT);
        }

        public override MediaType MediaType => MediaType.Album;

        public override string RequestType => "album";

        public override void LogSend() => SQMLog.Log(SQMDataId.InboxMessageSendAlbum, 1);
    }
}
