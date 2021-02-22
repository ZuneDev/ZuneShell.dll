// Decompiled with JetBrains decompiler
// Type: ZuneUI.ProfileCategories
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;

namespace ZuneUI
{
    public class ProfileCategories
    {
        private static Category _badges;
        private static Category _biography;
        private static Category _favorites;
        private static Category _lastPlayed;
        private static Category _topArtists;
        private static Category _comments;

        public static Category Badges
        {
            get
            {
                if (_badges == null)
                    _badges = new Category(StringId.IDS_PROFILE_BADGES_PIVOT, "res://ZuneShellResources!ProfileCategories.uix#ProfileCategoryBadges", false, SQMDataId.SocialViewBadges);
                return _badges;
            }
        }

        public static Category Biography
        {
            get
            {
                if (_biography == null)
                    _biography = new Category(StringId.IDS_PROFILE_BIOGRAPHY_PIVOT, "res://ZuneShellResources!ProfileCategories.uix#ProfileCategoryBiography", false, SQMDataId.SocialViewAbout);
                return _biography;
            }
        }

        public static Category Comments
        {
            get
            {
                if (_comments == null)
                    _comments = new Category(StringId.IDS_PROFILE_COMMENTS_PIVOT, "res://ZuneShellResources!ProfileCategories.uix#ProfileCategoryComments", false, SQMDataId.SocialViewComments);
                return _comments;
            }
        }

        public static Category Favorites
        {
            get
            {
                if (_favorites == null)
                    _favorites = new Category(StringId.IDS_PROFILE_FAVORITES_PIVOT, "res://ZuneShellResources!ProfileCategories.uix#ProfileCategoryFavorites", false, SQMDataId.SocialViewFavorites);
                return _favorites;
            }
        }

        public static Category RecentlyPlayed
        {
            get
            {
                if (_lastPlayed == null)
                    _lastPlayed = new Category(StringId.IDS_PROFILE_RECENTLY_PLAYED_PIVOT, "res://ZuneShellResources!ProfileCategories.uix#ProfileCategoryRecentlyPlayed", false, SQMDataId.SocialViewRecentPlays);
                return _lastPlayed;
            }
        }

        public static Category TopArtists
        {
            get
            {
                if (_topArtists == null)
                    _topArtists = new Category(StringId.IDS_PROFILE_MOST_PLAYED_ARTISTS_PIVOT, "res://ZuneShellResources!ProfileCategories.uix#ProfileCategoryTopArtists", false, SQMDataId.SocialViewTopArtists);
                return _topArtists;
            }
        }
    }
}
