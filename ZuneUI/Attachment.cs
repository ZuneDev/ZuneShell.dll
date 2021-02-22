// Decompiled with JetBrains decompiler
// Type: ZuneUI.Attachment
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Playlist;
using System;
using System.Collections;
using ZuneXml;

namespace ZuneUI
{
    public abstract class Attachment : NotifyPropertyChangedImpl
    {
        private string _attachmentUI;
        private string _description;
        private string _title;
        private string _subtitle;
        private string _imageUri;
        private Guid _imageId;
        private Guid _id;
        private bool _wishlist;
        private bool _allowEmailRecipients;

        protected Attachment(Guid id, string title, string subtitle, string imageUri)
        {
            this._attachmentUI = "res://ZuneShellResources!SocialComposer.uix#AttachmentUI";
            this._id = id;
            this._title = title;
            this._subtitle = subtitle;
            this._description = Shell.LoadString(StringId.IDS_COMPOSE_MESSAGE_DEFAULT_ATTACHMENT);
            this._imageUri = imageUri;
            this._allowEmailRecipients = true;
        }

        public virtual void LogSend()
        {
        }

        public static bool CanCreateAttachment(object obj)
        {
            DataProviderObject dataProviderObject = null;
            MediaType mediaType = MediaType.Undefined;
            Guid guid = Guid.Empty;
            int num = -1;
            bool flag = false;
            switch (obj)
            {
                case DataProviderObject _:
                    dataProviderObject = (DataProviderObject)obj;
                    object property1 = dataProviderObject.GetProperty("ZuneMediaId");
                    if (property1 != null)
                        guid = (Guid)property1;
                    object property2 = dataProviderObject.GetProperty("LibraryId");
                    if (property2 != null)
                        num = (int)property2;
                    if (dataProviderObject.GetProperty("Type") is string property3)
                    {
                        mediaType = ZuneShell.MapStringToMediaType(property3);
                        break;
                    }
                    break;
                case PlaybackTrack _:
                    PlaybackTrack playbackTrack = (PlaybackTrack)obj;
                    guid = playbackTrack.ZuneMediaId;
                    mediaType = playbackTrack.MediaType;
                    break;
            }
            switch (mediaType)
            {
                case MediaType.Track:
                case MediaType.Album:
                    flag = guid != Guid.Empty;
                    break;
                case MediaType.Video:
                    if (dataProviderObject != null && guid != Guid.Empty)
                    {
                        object property3 = dataProviderObject.GetProperty("CategoryId");
                        if (property3 != null)
                        {
                            VideoCategory videoCategory = (VideoCategory)property3;
                            flag = videoCategory == VideoCategory.TV || videoCategory == VideoCategory.Movies;
                            break;
                        }
                        break;
                    }
                    break;
                case MediaType.Playlist:
                    if (dataProviderObject != null && (guid != Guid.Empty || num != -1))
                    {
                        object property3 = dataProviderObject.GetProperty("PlaylistType");
                        if (property3 != null)
                        {
                            PlaylistType playlistType = (PlaylistType)property3;
                            flag = playlistType != PlaylistType.Channel && playlistType != PlaylistType.PersonalChannel;
                            break;
                        }
                        break;
                    }
                    break;
                case MediaType.PodcastEpisode:
                    if (dataProviderObject != null)
                    {
                        object property3 = dataProviderObject.GetProperty("SeriesId");
                        if (property3 != null)
                        {
                            flag = PodcastLibraryPage.GetZuneMediaId((int)property3) != Guid.Empty || !string.IsNullOrEmpty(dataProviderObject.GetProperty("SeriesFeedUrl") as string);
                            break;
                        }
                        break;
                    }
                    break;
                case MediaType.Podcast:
                    flag = guid != Guid.Empty;
                    if (!flag && dataProviderObject != null)
                    {
                        flag = !string.IsNullOrEmpty(dataProviderObject.GetProperty("FeedUrl") as string);
                        break;
                    }
                    break;
            }
            return flag;
        }

        public static Attachment CreateAttachment(object obj)
        {
            DataProviderObject dataProviderObject = obj as DataProviderObject;
            MediaType mediaType = MediaType.Undefined;
            Guid zuneMediaId = Guid.Empty;
            VideoCategory videoCategory = VideoCategory.Other;
            string title = (string)null;
            string url = (string)null;
            int num = -1;
            if (dataProviderObject != null)
            {
                object property1 = dataProviderObject.GetProperty("ZuneMediaId");
                if (property1 != null)
                    zuneMediaId = (Guid)property1;
                if (dataProviderObject.GetProperty("Type") is string property)
                    mediaType = ZuneShell.MapStringToMediaType(property);
                object property3 = dataProviderObject.GetProperty("LibraryId");
                if (property3 != null)
                    num = (int)property3;
                object property4 = dataProviderObject.GetProperty("CategoryId");
                if (property4 != null)
                    videoCategory = (VideoCategory)property4;
                title = dataProviderObject.GetProperty("Title") as string;
                switch (mediaType)
                {
                    case MediaType.PodcastEpisode:
                        object property5 = dataProviderObject.GetProperty("SeriesId");
                        if (property5 != null)
                            num = (int)property5;
                        zuneMediaId = PodcastLibraryPage.GetZuneMediaId(num);
                        mediaType = MediaType.Podcast;
                        title = dataProviderObject.GetProperty("SeriesTitle") as string;
                        url = dataProviderObject.GetProperty("SeriesFeedUrl") as string;
                        break;
                    case MediaType.Podcast:
                        url = dataProviderObject.GetProperty("FeedUrl") as string;
                        break;
                }
            }
            return Attachment.CreateAttachment(zuneMediaId, mediaType, num, title, url, videoCategory, MovieType.Other);
        }

        public static Attachment CreateAttachment(Guid zuneMediaId, MediaType mediaType) => Attachment.CreateAttachment(zuneMediaId, mediaType, -1, (string)null);

        public static Attachment CreateAttachment(
          Guid zuneMediaId,
          MediaType mediaType,
          int libraryId,
          string title)
        {
            return Attachment.CreateAttachment(zuneMediaId, mediaType, libraryId, title, (string)null, VideoCategory.Other, MovieType.Other);
        }

        public static Attachment CreateAttachment(
          Guid zuneMediaId,
          MediaType mediaType,
          string title,
          VideoCategory videoCategory,
          MovieType movieType)
        {
            return Attachment.CreateAttachment(zuneMediaId, mediaType, -1, title, (string)null, videoCategory, movieType);
        }

        private static Attachment CreateAttachment(
          Guid zuneMediaId,
          MediaType mediaType,
          int libraryId,
          string title,
          string url,
          VideoCategory videoCategory,
          MovieType movieType)
        {
            Attachment attachment = (Attachment)null;
            if (libraryId != -1 && mediaType == MediaType.Playlist)
            {
                attachment = (Attachment)new CollectionPlaylistAttachment((string)null, title, (string)null, libraryId, (IList)null);
            }
            else
            {
                switch (mediaType)
                {
                    case MediaType.Track:
                        if (zuneMediaId != Guid.Empty)
                        {
                            attachment = (Attachment)new TrackAttachment(zuneMediaId, title, (string)null, (string)null);
                            break;
                        }
                        break;
                    case MediaType.Video:
                        if (zuneMediaId != Guid.Empty)
                        {
                            switch (videoCategory)
                            {
                                case VideoCategory.TV:
                                    attachment = (Attachment)new EpisodeAttachment(zuneMediaId, title, (string)null, (string)null);
                                    break;
                                case VideoCategory.Movies:
                                    attachment = movieType != MovieType.Trailer ? (Attachment)new MovieAttachment(zuneMediaId, title, (string)null) : (Attachment)new TrailerAttachment(zuneMediaId, title, (string)null);
                                    break;
                            }
                        }
                        else
                            break;
                        break;
                    case MediaType.Playlist:
                        if (zuneMediaId != Guid.Empty)
                        {
                            attachment = (Attachment)new PlaylistAttachment(zuneMediaId, title, (string)null);
                            break;
                        }
                        break;
                    case MediaType.Album:
                        if (zuneMediaId != Guid.Empty)
                        {
                            attachment = (Attachment)new AlbumAttachment(zuneMediaId, title, (string)null, (string)null, Guid.Empty);
                            break;
                        }
                        break;
                    case MediaType.Podcast:
                        if (zuneMediaId != Guid.Empty || !string.IsNullOrEmpty(url))
                        {
                            attachment = (Attachment)new PodcastAttachment(zuneMediaId, title, (string)null, url, (string)null);
                            break;
                        }
                        break;
                }
            }
            return attachment;
        }

        public static Attachment CreateAttachmentFromMessage(
          object message,
          object messageDetails)
        {
            return Attachment.CreateAttachmentFromMessage(message as MessageRoot, messageDetails as MessageDetails);
        }

        private static Attachment CreateAttachmentFromMessage(
          MessageRoot message,
          MessageDetails messageDetails)
        {
            Attachment attachment = (Attachment)null;
            if (message != null)
            {
                string type = message.Type;
                if (message.MediaId != Guid.Empty)
                {
                    switch (type)
                    {
                        case "album":
                            attachment = (Attachment)new AlbumAttachment(message.MediaId, (string)null, (string)null, (string)null, Guid.Empty);
                            break;
                        case "playlist":
                            attachment = (Attachment)new PlaylistAttachment(message.MediaId, (string)null, (string)null);
                            break;
                        case "podcast":
                            attachment = (Attachment)new PodcastAttachment(message.MediaId, (string)null, (string)null, (string)null, (string)null);
                            break;
                        case "song":
                            attachment = (Attachment)new TrackAttachment(message.MediaId, (string)null, (string)null, (string)null);
                            break;
                        case "video":
                            attachment = (Attachment)new EpisodeAttachment(message.MediaId, (string)null, (string)null, (string)null);
                            break;
                        case "movie":
                            attachment = (Attachment)new MovieAttachment(message.MediaId, (string)null, (string)null);
                            break;
                        case "movietrailer":
                            attachment = (Attachment)new TrailerAttachment(message.MediaId, (string)null, (string)null);
                            break;
                    }
                }
                else if (messageDetails != null)
                {
                    if (messageDetails.PodcastMediaId != Guid.Empty)
                    {
                        if (type == "podcast")
                            attachment = (Attachment)new PodcastAttachment(messageDetails.PodcastMediaId, (string)null, (string)null, (string)null, (string)null);
                    }
                    else if (!string.IsNullOrEmpty(messageDetails.ZuneTag) && type == "card")
                        attachment = (Attachment)new ProfileAttachment(messageDetails.ZuneTag, (string)null, (string)null);
                }
            }
            return attachment;
        }

        public static Attachment CreateAttachment(
          Guid id,
          MediaType mediaType,
          bool wishlist)
        {
            Attachment attachment = Attachment.CreateAttachment(id, mediaType);
            if (attachment != null)
                attachment.Wishlist = wishlist;
            return attachment;
        }

        public string AttachmentUI
        {
            get => this._attachmentUI;
            set
            {
                if (!(this._attachmentUI != value))
                    return;
                this._attachmentUI = value;
                this.FirePropertyChanged(nameof(AttachmentUI));
            }
        }

        public string Description
        {
            get => this._description;
            set
            {
                if (this._description == null)
                    return;
                this._description = value;
                this.FirePropertyChanged(nameof(Description));
            }
        }

        public string ImageUri
        {
            get => this._imageUri;
            set
            {
                if (!(this._imageUri != value))
                    return;
                this._imageUri = value;
                this.FirePropertyChanged(nameof(ImageUri));
            }
        }

        public Guid ImageId
        {
            get => this._imageId;
            set
            {
                if (!(this._imageId != value))
                    return;
                this._imageId = value;
                this.FirePropertyChanged(nameof(ImageId));
            }
        }

        public string Title
        {
            get => this._title;
            set
            {
                if (!(this._title != value))
                    return;
                this._title = value;
                this.FirePropertyChanged(nameof(Title));
            }
        }

        public string Subtitle
        {
            get => this._subtitle;
            set
            {
                if (!(this._subtitle != value))
                    return;
                this._subtitle = value;
                this.FirePropertyChanged(nameof(Subtitle));
            }
        }

        public Guid Id => this._id;

        public bool Wishlist
        {
            get => this._wishlist;
            set
            {
                if (this._wishlist == value)
                    return;
                this._wishlist = value;
                this.FirePropertyChanged(nameof(Wishlist));
            }
        }

        public bool AllowEmailRecipients
        {
            get => this._allowEmailRecipients;
            set
            {
                if (this._allowEmailRecipients == value)
                    return;
                this._allowEmailRecipients = value;
                this.FirePropertyChanged(nameof(AllowEmailRecipients));
            }
        }

        public abstract MediaType MediaType { get; }

        public abstract string RequestType { get; }

        public virtual string[] Properties => new string[6]
        {
      "type",
      this.RequestType,
      "mediaid",
      this.Id.ToString(),
      "wishlist",
      this.Wishlist.ToString()
        };

        public virtual bool IsReady => true;

        public bool ContainsImage() => !string.IsNullOrEmpty(this._imageUri) || this._imageId != Guid.Empty;

        public virtual bool IsValid(out string errorMessage)
        {
            errorMessage = (string)null;
            return true;
        }
    }
}
