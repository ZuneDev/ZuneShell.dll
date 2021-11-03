// Decompiled with JetBrains decompiler
// Type: ZuneUI.PodcastAttachment
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;
using System;

namespace ZuneUI
{
    public class PodcastAttachment : Attachment
    {
        public const string RequestTypeString = "podcast";
        private string _url;

        public PodcastAttachment(Guid id, string title, string author, string url, string imageUri)
          : base(id, title, author, imageUri)
        {
            this.AttachmentUI = "res://ZuneShellResources!SocialComposer.uix#PodcastAttachmentUI";
            this.Description = Shell.LoadString(StringId.IDS_COMPOSE_MESSAGE_PODCAST_ATTACHMENT);
            this._url = url;
        }

        public string Url
        {
            get => this._url;
            set
            {
                this._url = value;
                this.FirePropertyChanged(nameof(Url));
            }
        }

        public override MediaType MediaType => MediaType.Podcast;

        public override string RequestType => "podcast";

        public override string[] Properties => new string[8]
        {
      "type",
      this.RequestType,
      "podcasturl",
      this._url ?? string.Empty,
      "podcastmediaid",
      this.Id.ToString(),
      "podcastname",
      this.Title
        };

        public override void LogSend() => SQMLog.Log(SQMDataId.InboxMessageSendPodcast, 1);
    }
}
