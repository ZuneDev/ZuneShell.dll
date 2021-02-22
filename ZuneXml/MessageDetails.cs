// Decompiled with JetBrains decompiler
// Type: ZuneXml.MessageDetails
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneXml
{
    internal class MessageDetails : XmlDataProviderObject
    {
        internal static XmlDataProviderObject ConstructMessageDetailsObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new MessageDetails(owner, objectTypeCookie);
        }

        internal MessageDetails(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal string TextContent => (string)this.GetProperty(nameof(TextContent));

        internal Guid MediaId => (Guid)this.GetProperty(nameof(MediaId));

        internal string ReplyLink => (string)this.GetProperty(nameof(ReplyLink));

        internal string AltLink => (string)this.GetProperty(nameof(AltLink));

        internal string AlbumTitle => (string)this.GetProperty(nameof(AlbumTitle));

        internal string ArtistName => (string)this.GetProperty(nameof(ArtistName));

        internal string SongTitle => (string)this.GetProperty(nameof(SongTitle));

        internal int TrackNumber => (int)this.GetProperty(nameof(TrackNumber));

        internal string PlaylistName => (string)this.GetProperty(nameof(PlaylistName));

        internal string PodcastName => (string)this.GetProperty(nameof(PodcastName));

        internal string PodcastUrl => (string)this.GetProperty(nameof(PodcastUrl));

        internal Guid PodcastMediaId => (Guid)this.GetProperty(nameof(PodcastMediaId));

        internal string UserTile => (string)this.GetProperty(nameof(UserTile));

        internal string ZuneTag => (string)this.GetProperty(nameof(ZuneTag));

        internal string ForumsMsgUrl => (string)this.GetProperty(nameof(ForumsMsgUrl));

        internal string NotifSubject => (string)this.GetProperty(nameof(NotifSubject));

        internal string NotifSource => (string)this.GetProperty(nameof(NotifSource));
    }
}
