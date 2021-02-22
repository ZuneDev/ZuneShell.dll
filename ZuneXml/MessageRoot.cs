// Decompiled with JetBrains decompiler
// Type: ZuneXml.MessageRoot
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Service;
using System;
using ZuneUI;

namespace ZuneXml
{
    internal class MessageRoot : XmlDataProviderObject
    {
        private MessageTypeEnum _messageType = MessageTypeEnum.Invalid;
        private EContentType _contentType = EContentType.Unknown;

        internal string UserTileUrl => ComposerHelper.GetUserTileUri(this.From);

        internal MessageTypeEnum MessageType
        {
            get => this._messageType;
            private set
            {
                if (this._messageType == value)
                    return;
                this._messageType = value;
                this.ContentType = MessageRoot.ToContentType(value);
                this.FirePropertyChanged(nameof(MessageType));
                this.FirePropertyChanged("IsSupported");
            }
        }

        internal EContentType ContentType
        {
            get => this._contentType;
            private set
            {
                if (this._contentType == value)
                    return;
                this._contentType = value;
                this.FirePropertyChanged(nameof(ContentType));
            }
        }

        internal bool IsSupported
        {
            get
            {
                if (!this.Wishlist)
                    return this.MessageType != MessageTypeEnum.Invalid;
                return this.MessageType == MessageTypeEnum.Song || this.MessageType == MessageTypeEnum.Album;
            }
        }

        private static EContentType ToContentType(MessageTypeEnum messageType)
        {
            EContentType econtentType;
            switch (messageType)
            {
                case MessageTypeEnum.Album:
                    econtentType = EContentType.MusicAlbum;
                    break;
                case MessageTypeEnum.MusicVideo:
                case MessageTypeEnum.Video:
                case MessageTypeEnum.Movie:
                case MessageTypeEnum.MovieTrailer:
                    econtentType = EContentType.Video;
                    break;
                case MessageTypeEnum.Playlist:
                    econtentType = EContentType.Playlist;
                    break;
                case MessageTypeEnum.Podcast:
                    econtentType = EContentType.PodcastSeries;
                    break;
                case MessageTypeEnum.Song:
                    econtentType = EContentType.MusicTrack;
                    break;
                default:
                    econtentType = EContentType.Unknown;
                    break;
            }
            return econtentType;
        }

        public override void SetProperty(string propertyName, object value)
        {
            base.SetProperty(propertyName, value);
            switch (propertyName)
            {
                case "Type":
                    this.MessageType = SchemaHelper.ToMessageType(value as string);
                    break;
            }
            base.SetProperty(propertyName, value);
        }

        internal static XmlDataProviderObject ConstructMessageRootObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new MessageRoot(owner, objectTypeCookie);
        }

        internal MessageRoot(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal string MessagingId => (string)base.GetProperty(nameof(MessagingId));

        internal string From => (string)base.GetProperty(nameof(From));

        internal string Type => (string)base.GetProperty(nameof(Type));

        internal string Subject => (string)base.GetProperty(nameof(Subject));

        internal DateTime Received => (DateTime)base.GetProperty(nameof(Received));

        internal string DetailsLink => (string)base.GetProperty(nameof(DetailsLink));

        internal string Status
        {
            get => (string)base.GetProperty(nameof(Status));
            set => this.SetProperty(nameof(Status), (object)value);
        }

        internal bool Wishlist => (bool)base.GetProperty(nameof(Wishlist));

        internal Guid MediaId => (Guid)base.GetProperty(nameof(MediaId));

        public override object GetProperty(string propertyName)
        {
            switch (propertyName)
            {
                case "UserTileUrl":
                    return (object)this.UserTileUrl;
                case "IsSupported":
                    return (object)this.IsSupported;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
