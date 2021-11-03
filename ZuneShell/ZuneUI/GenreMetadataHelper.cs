// Decompiled with JetBrains decompiler
// Type: ZuneUI.GenreMetadataHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public static class GenreMetadataHelper
    {
        private static StringId[] s_movieGenreIds = new StringId[11]
        {
      StringId.IDS_VIDEO_GENRE_ACTION_ADVENTURE,
      StringId.IDS_VIDEO_GENRE_COMEDY,
      StringId.IDS_VIDEO_GENRE_DOCUMENTARY,
      StringId.IDS_VIDEO_GENRE_DRAMA,
      StringId.IDS_VIDEO_GENRE_FAMILY,
      StringId.IDS_VIDEO_GENRE_FOREIGN_INDEPENDENT,
      StringId.IDS_VIDEO_GENRE_HORROR,
      StringId.IDS_VIDEO_GENRE_OTHER,
      StringId.IDS_VIDEO_GENRE_ROMANCE,
      StringId.IDS_VIDEO_GENRE_SCIFI_FANTASY,
      StringId.IDS_VIDEO_GENRE_THRILLER_MYSTERY
        };
        private static StringId[] s_tvGenreIds = new StringId[15]
        {
      StringId.IDS_VIDEO_GENRE_ACTION_ADVENTURE,
      StringId.IDS_VIDEO_GENRE_ANIMATION,
      StringId.IDS_VIDEO_GENRE_COMEDY,
      StringId.IDS_VIDEO_GENRE_DOCUMENTARY_BIO,
      StringId.IDS_VIDEO_GENRE_DRAMA,
      StringId.IDS_VIDEO_GENRE_EDUCATIONAL,
      StringId.IDS_VIDEO_GENRE_FAMILY_CHILDREN,
      StringId.IDS_VIDEO_GENRE_MOVIES,
      StringId.IDS_VIDEO_GENRE_MUSIC,
      StringId.IDS_VIDEO_GENRE_NEWS,
      StringId.IDS_VIDEO_GENRE_OTHER,
      StringId.IDS_VIDEO_GENRE_REALITY_TV,
      StringId.IDS_VIDEO_GENRE_SCIFI_FANTASY,
      StringId.IDS_VIDEO_GENRE_SOAP,
      StringId.IDS_VIDEO_GENRE_SPORTS
        };
        private static StringId[] s_otherGenreIds = new StringId[3]
        {
      StringId.IDS_VIDEO_GENRE_CREATIONS,
      StringId.IDS_VIDEO_GENRE_FAMILY_FRIENDS,
      StringId.IDS_VIDEO_GENRE_OTHER
        };
        private static List<string> s_movieGenres;
        private static List<string> s_tvGenres;
        private static List<string> s_otherGenres;

        public static IList CannedTVGenres => EnsureGenres(ref s_tvGenres, s_tvGenreIds);

        public static IList CannedMovieGenres => EnsureGenres(ref s_movieGenres, s_movieGenreIds);

        public static IList CannedOtherGenres => EnsureGenres(ref s_otherGenres, s_otherGenreIds);

        private static List<string> EnsureGenres(ref List<string> genres, StringId[] genreIds)
        {
            if (genres == null)
            {
                genres = new List<string>(genreIds.Length);
                foreach (StringId genreId in genreIds)
                    genres.Add(Shell.LoadString(genreId));
            }
            return genres;
        }
    }
}
