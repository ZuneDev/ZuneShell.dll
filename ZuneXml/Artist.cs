// Decompiled with JetBrains decompiler
// Type: ZuneXml.Artist
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;

namespace ZuneXml
{
    internal class Artist : XmlDataProviderObject
    {
        internal static XmlDataProviderObject ConstructArtistObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new Artist(owner, objectTypeCookie);
        }

        internal Artist(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal Guid Id => (Guid)this.GetProperty(nameof(Id));

        internal string Title => (string)this.GetProperty(nameof(Title));

        internal string SortTitle => (string)this.GetProperty(nameof(SortTitle));

        internal Guid ImageId => (Guid)this.GetProperty(nameof(ImageId));

        internal double Popularity => (double)this.GetProperty(nameof(Popularity));

        internal bool IsVariousArtist => (bool)this.GetProperty(nameof(IsVariousArtist));

        internal string BiographyLink => (string)this.GetProperty(nameof(BiographyLink));

        internal int PlayCount => (int)this.GetProperty(nameof(PlayCount));

        internal Genre PrimaryGenre => (Genre)this.GetProperty(nameof(PrimaryGenre));

        internal IList Genres => (IList)this.GetProperty(nameof(Genres));

        internal IList Moods => (IList)this.GetProperty(nameof(Moods));

        internal Guid AlbumImageId => (Guid)this.GetProperty(nameof(AlbumImageId));

        internal Guid BackgroundImageId => (Guid)this.GetProperty(nameof(BackgroundImageId));

        internal bool HasRadioChannel => (bool)this.GetProperty(nameof(HasRadioChannel));
    }
}
