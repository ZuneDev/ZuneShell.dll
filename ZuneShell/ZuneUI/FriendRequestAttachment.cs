// Decompiled with JetBrains decompiler
// Type: ZuneUI.FriendRequestAttachment
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public class FriendRequestAttachment : Attachment
    {
        public const string RequestTypeString = "friendrequest";

        public FriendRequestAttachment()
          : base(Guid.Empty, null, null, null)
        {
            this.AttachmentUI = null;
            this.Description = Shell.LoadString(StringId.IDS_COMPOSE_MESSAGE_FRIENDREQUEST_ATTACHMENT);
        }

        public override MediaType MediaType => MediaType.Undefined;

        public override string RequestType => "friendrequest";
    }
}
