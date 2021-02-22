// Decompiled with JetBrains decompiler
// Type: ZuneUI.MixResultAlbum
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using ZuneXml;

namespace ZuneUI
{
    public class MixResultAlbum : MixResult
    {
        protected MixResultAlbum()
        {
        }

        public static MixResultAlbum CreateInstance(
          DataProviderObject dataProviderObject,
          string reason)
        {
            MixResultAlbum mixResultAlbum = new MixResultAlbum();
            Album album = (Album)dataProviderObject;
            MiniArtist primaryArtist = album.PrimaryArtist;
            string secondaryText = primaryArtist != null ? primaryArtist.Title : string.Empty;
            mixResultAlbum.Initialize(MixResultType.Album, reason, album.Title ?? string.Empty, secondaryText, album.Id.ToString(), string.Empty, album.ImageId, (DataProviderObject)null);
            return mixResultAlbum;
        }

        public static MixResultAlbum CreateInstance(LibraryAlbumInfo libraryAlbumInfo)
        {
            MixResultAlbum mixResultAlbum = new MixResultAlbum();
            mixResultAlbum.Initialize(MixResultType.Album, string.Empty, libraryAlbumInfo.AlbumTitle, libraryAlbumInfo.ArtistName, libraryAlbumInfo.ZuneMediaId.ToString(), libraryAlbumInfo.AlbumArtUrl, Guid.Empty, (DataProviderObject)null);
            return mixResultAlbum;
        }

        internal static int GetItemPriority(DataProviderObject item, int startPriority)
        {
            int num = startPriority;
            Album album = (Album)item;
            if (!album.Actionable)
                num += MixResultAlbum.NonActionablePriorityBump;
            if ((album.Title ?? string.Empty).ToLowerInvariant().Contains("karaoke"))
                num += MixResultAlbum.KaraokePriorityBump;
            return num;
        }

        internal override bool IsDuplicate(MixResult compareTo) => base.IsDuplicate(compareTo) || string.Compare(this.SecondaryText, compareTo.SecondaryText, StringComparison.InvariantCultureIgnoreCase) == 0 && string.Compare(MixResultAlbum.GetComparableAlbumName(this.PrimaryText), MixResultAlbum.GetComparableAlbumName(compareTo.PrimaryText), StringComparison.InvariantCultureIgnoreCase) == 0;

        public static string GetComparableAlbumName(string albumName)
        {
            string str = albumName.Trim();
            int length = str.IndexOf('(');
            if (length > 3)
                str = str.Substring(0, length).Trim();
            str.ToLowerInvariant();
            return str;
        }

        public static int KaraokePriorityBump => 100;

        public static int NonActionablePriorityBump => 200;
    }
}
