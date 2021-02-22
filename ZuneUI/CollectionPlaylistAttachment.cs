// Decompiled with JetBrains decompiler
// Type: ZuneUI.CollectionPlaylistAttachment
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Configuration;
using Microsoft.Zune.Messaging;
using Microsoft.Zune.Util;
using System;
using System.Collections;

namespace ZuneUI
{
    public class CollectionPlaylistAttachment : PropertySetAttachment
    {
        public const string RequestTypeString = "playlist";
        private PlaylistMessageData m_playlistData;
        private IList m_tracks;
        private int m_mediaId;

        public CollectionPlaylistAttachment(
          string author,
          string title,
          string imageUri,
          int mediaId,
          IList tracks)
          : base(Guid.Empty, title, author, imageUri)
        {
            this.AttachmentUI = "res://ZuneShellResources!SocialComposer.uix#CollectionPlaylistAttachmentUI";
            this.Description = Shell.LoadString(StringId.IDS_COMPOSE_MESSAGE_PLAYLIST_ATTACHMENT);
            this.AllowEmailRecipients = false;
            this.m_tracks = tracks;
            this.m_mediaId = mediaId;
        }

        protected override void FirePropertyChanged(string propertyName)
        {
            if (propertyName == "Subtitle")
                this.FirePropertyChanged("Author");
            if (propertyName == "Title" || propertyName == "Subtitle")
            {
                this.m_playlistData = null;
                this.FirePropertyChanged("PropertySet");
            }
            base.FirePropertyChanged(propertyName);
        }

        public override MediaType MediaType => MediaType.Playlist;

        public override string RequestType => "playlist";

        public override string[] Properties => new string[0];

        public override IPropertySetMessageData PropertySet
        {
            get
            {
                if (this.m_playlistData == null)
                    this.m_playlistData = new PlaylistMessageData(this.Title, this.Author, this.Tracks);
                return m_playlistData;
            }
        }

        public string Author
        {
            get => this.Subtitle;
            set => this.Subtitle = value;
        }

        public int MediaId => this.m_mediaId;

        public IList Tracks
        {
            get => this.m_tracks;
            set
            {
                if (this.m_tracks == value)
                    return;
                this.m_tracks = value;
                this.FirePropertyChanged(nameof(Tracks));
                this.FirePropertyChanged("IsReady");
            }
        }

        public override bool IsReady => this.Tracks != null;

        public override void LogSend() => SQMLog.Log(SQMDataId.InboxMessageSendUserPlaylist, 1);

        public override bool IsValid(out string errorMessage)
        {
            bool flag = true;
            errorMessage = null;
            int num = ClientConfiguration.Messaging.MaxSubMessagesPerMessage * ClientConfiguration.Messaging.MaxTracksPerMessage;
            if (this.m_tracks != null && this.m_tracks.Count > num)
            {
                flag = false;
                errorMessage = string.Format(Shell.LoadString(StringId.IDS_COMPOSE_MESSAGE_ERROR_TOO_MANY_TRACKS), num);
            }
            return flag;
        }
    }
}
