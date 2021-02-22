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
                if (ProfileCategories._badges == null)
                    ProfileCategories._badges = new Category(StringId.IDS_PROFILE_BADGES_PIVOT, "res://ZuneShellResources!ProfileCategories.uix#ProfileCategoryBadges", false, SQMDataId.SocialViewBadges);
                return ProfileCategories._badges;
            }
        }

        public static Category Biography
        {
            get
            {
                if (ProfileCategories._biography == null)
                    ProfileCategories._biography = new Category(StringId.IDS_PROFILE_BIOGRAPHY_PIVOT, "res://ZuneShellResources!ProfileCategories.uix#ProfileCategoryBiography", false, SQMDataId.SocialViewAbout);
                return ProfileCategories._biography;
            }
        }

        public static Category Comments
        {
            get
            {
                if (ProfileCategories._comments == null)
                    ProfileCategories._comments = new Category(StringId.IDS_PROFILE_COMMENTS_PIVOT, "res://ZuneShellResources!ProfileCategories.uix#ProfileCategoryComments", false, SQMDataId.SocialViewComments);
                return ProfileCategories._comments;
            }
        }

        public static Category Favorites
        {
            get
            {
                if (ProfileCategories._favorites == null)
                    ProfileCategories._favorites = new Category(StringId.IDS_PROFILE_FAVORITES_PIVOT, "res://ZuneShellResources!ProfileCategories.uix#ProfileCategoryFavorites", false, SQMDataId.SocialViewFavorites);
                return ProfileCategories._favorites;
            }
        }

        public static Category RecentlyPlayed
        {
            get
            {
                if (ProfileCategories._lastPlayed == null)
                    ProfileCategories._lastPlayed = new Category(StringId.IDS_PROFILE_RECENTLY_PLAYED_PIVOT, "res://ZuneShellResources!ProfileCategories.uix#ProfileCategoryRecentlyPlayed", false, SQMDataId.SocialViewRecentPlays);
                return ProfileCategories._lastPlayed;
            }
        }

        public static Category TopArtists
        {
            get
            {
                if (ProfileCategories._topArtists == null)
                    ProfileCategories._topArtists = new Category(StringId.IDS_PROFILE_MOST_PLAYED_ARTISTS_PIVOT, "res://ZuneShellResources!ProfileCategories.uix#ProfileCategoryTopArtists", false, SQMDataId.SocialViewTopArtists);
                return ProfileCategories._topArtists;
            }
        }
    }
}
