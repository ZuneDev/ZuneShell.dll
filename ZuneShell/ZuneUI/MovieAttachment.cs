// Decompiled with JetBrains decompiler
// Type: ZuneUI.MovieAttachment
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public class MovieAttachment : VideoAttachment
    {
        public const string RequestTypeString = "movie";

        public MovieAttachment(Guid id, string title, string imageUri)
          : base(id, title, string.Empty, imageUri)
        {
            this.AttachmentUI = "res://ZuneShellResources!SocialComposer.uix#MovieAttachmentUI";
            this.Description = Shell.LoadString(StringId.IDS_COMPOSE_MESSAGE_MOVIE_ATTACHMENT);
        }

        public override string RequestType => "movie";
    }
}
