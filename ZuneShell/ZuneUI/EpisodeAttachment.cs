// Decompiled with JetBrains decompiler
// Type: ZuneUI.EpisodeAttachment
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public class EpisodeAttachment : VideoAttachment
    {
        public const string RequestTypeString = "video";

        public EpisodeAttachment(Guid id, string title, string subtitle, string imageUri)
          : base(id, title, subtitle, imageUri)
        {
            this.AttachmentUI = "res://ZuneShellResources!SocialComposer.uix#EpisodeAttachmentUI";
            this.Description = Shell.LoadString(StringId.IDS_COMPOSE_MESSAGE_VIDEO_ATTACHMENT);
        }

        public override string RequestType => "video";
    }
}
