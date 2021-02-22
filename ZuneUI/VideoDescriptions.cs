// Decompiled with JetBrains decompiler
// Type: ZuneUI.VideoDescriptions
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneUI
{
    public static class VideoDescriptions
    {
        private static string _tvViewHeader = Shell.LoadString(StringId.IDS_COLLECTION_VIDEO_TV);
        private static string _musicViewHeader = (string)null;
        private static string _movieViewHeader = (string)null;
        private static string _otherViewHeader = (string)null;
        private static string _personalViewHeader = (string)null;
        private static string _seriesDescription = Shell.LoadString(StringId.IDS_COLLECTION_VIDEO_TV_SERIES);
        private static string _shortsDescription = Shell.LoadString(StringId.IDS_COLLECTION_VIDEO_TV_SHORTS);
        private static string _newsDescription = Shell.LoadString(StringId.IDS_COLLECTION_VIDEO_TV_NEWS);
        private static string _musicDescription = Shell.LoadString(StringId.IDS_COLLECTION_VIDEO_MUSIC);
        private static string _moviesDescription = Shell.LoadString(StringId.IDS_COLLECTION_VIDEO_MOVIES);
        private static string _otherDescription = Shell.LoadString(StringId.IDS_COLLECTION_VIDEO_OTHER);
        private static string _personalDescription = Shell.LoadString(StringId.IDS_COLLECTION_VIDEO_PERSONAL);
        private static VideoViewCategory _tvCategory = new VideoViewCategory(VideoDescriptions._tvViewHeader, VideoDescriptions._seriesDescription);
        private static VideoViewCategory _shortsCategory = new VideoViewCategory(VideoDescriptions._tvViewHeader, VideoDescriptions._shortsDescription);
        private static VideoViewCategory _newsCategory = new VideoViewCategory(VideoDescriptions._tvViewHeader, VideoDescriptions._newsDescription);
        private static VideoViewCategory _musicCategory = new VideoViewCategory(VideoDescriptions._musicViewHeader, VideoDescriptions._musicDescription);
        private static VideoViewCategory _moviesCategory = new VideoViewCategory(VideoDescriptions._movieViewHeader, VideoDescriptions._moviesDescription);
        private static VideoViewCategory _otherCategory = new VideoViewCategory(VideoDescriptions._otherViewHeader, VideoDescriptions._otherDescription);
        private static VideoViewCategory _personalCategory = new VideoViewCategory(VideoDescriptions._personalViewHeader, VideoDescriptions._personalDescription);
        private static GroupedList _groupedCategories;

        public static int GetCategoryId(VideoCategory category) => (int)category;

        public static VideoCategory GetCategory(int id) => (VideoCategory)id;

        public static string GetDescription(int categoryId) => VideoDescriptions.GetDescription((VideoCategory)categoryId);

        public static string GetDescription(VideoCategory category)
        {
            switch (category)
            {
                case VideoCategory.TV:
                    return VideoDescriptions._seriesDescription;
                case VideoCategory.News:
                    return VideoDescriptions._newsDescription;
                case VideoCategory.Music:
                    return VideoDescriptions._musicDescription;
                case VideoCategory.Movies:
                    return VideoDescriptions._moviesDescription;
                case VideoCategory.Personal:
                    return VideoDescriptions._personalDescription;
                case VideoCategory.Shorts:
                    return VideoDescriptions._shortsDescription;
                default:
                    return VideoDescriptions._otherDescription;
            }
        }

        public static VideoCategory GetCategory(string description)
        {
            if (description == VideoDescriptions._seriesDescription)
                return VideoCategory.TV;
            if (description == VideoDescriptions._shortsDescription)
                return VideoCategory.Shorts;
            if (description == VideoDescriptions._newsDescription)
                return VideoCategory.News;
            if (description == VideoDescriptions._musicDescription)
                return VideoCategory.Music;
            if (description == VideoDescriptions._moviesDescription)
                return VideoCategory.Movies;
            return description == VideoDescriptions._personalDescription ? VideoCategory.Personal : VideoCategory.Other;
        }

        public static GroupedList Categories
        {
            get
            {
                if (VideoDescriptions._groupedCategories == null)
                {
                    VideoDescriptions._groupedCategories = new GroupedList();
                    VideoDescriptions._groupedCategories.Comparer = (IComparer)new VideoViewCategoryComparer();
                    VideoDescriptions._groupedCategories.Source = (IList)new VideoViewCategory[7]
                    {
            VideoDescriptions._tvCategory,
            VideoDescriptions._shortsCategory,
            VideoDescriptions._newsCategory,
            VideoDescriptions._musicCategory,
            VideoDescriptions._moviesCategory,
            VideoDescriptions._otherCategory,
            VideoDescriptions._personalCategory
                    };
                }
                return VideoDescriptions._groupedCategories;
            }
        }
    }
}
