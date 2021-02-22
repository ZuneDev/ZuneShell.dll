// Decompiled with JetBrains decompiler
// Type: ZuneUI.ProfileAttachment
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;
using System;

namespace ZuneUI
{
    public class ProfileAttachment : Attachment
    {
        public const string RequestTypeString = "card";

        public ProfileAttachment(string zuneTag, string status, string imageUri)
          : base(Guid.Empty, zuneTag, status, imageUri)
        {
            this.AttachmentUI = "res://ZuneShellResources!SocialComposer.uix#ProfileAttachmentUI";
            this.Description = Shell.LoadString(StringId.IDS_COMPOSE_MESSAGE_FRIEND_ATTACHMENT);
        }

        public override MediaType MediaType => MediaType.UserCard;

        public override string RequestType => "card";

        public override string[] Properties => new string[4]
        {
      "type",
      this.RequestType,
      "zunetag",
      this.Title
        };

        public override void LogSend() => SQMLog.Log(SQMDataId.InboxMessageSendProfileCard, 1);
    }
}
