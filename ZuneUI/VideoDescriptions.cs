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
        private static string _musicViewHeader = null;
        private static string _movieViewHeader = null;
        private static string _otherViewHeader = null;
        private static string _personalViewHeader = null;
        private static string _seriesDescription = Shell.LoadString(StringId.IDS_COLLECTION_VIDEO_TV_SERIES);
        private static string _shortsDescription = Shell.LoadString(StringId.IDS_COLLECTION_VIDEO_TV_SHORTS);
        private static string _newsDescription = Shell.LoadString(StringId.IDS_COLLECTION_VIDEO_TV_NEWS);
        private static string _musicDescription = Shell.LoadString(StringId.IDS_COLLECTION_VIDEO_MUSIC);
        private static string _moviesDescription = Shell.LoadString(StringId.IDS_COLLECTION_VIDEO_MOVIES);
        private static string _otherDescription = Shell.LoadString(StringId.IDS_COLLECTION_VIDEO_OTHER);
        private static string _personalDescription = Shell.LoadString(StringId.IDS_COLLECTION_VIDEO_PERSONAL);
        private static VideoViewCategory _tvCategory = new VideoViewCategory(_tvViewHeader, _seriesDescription);
        private static VideoViewCategory _shortsCategory = new VideoViewCategory(_tvViewHeader, _shortsDescription);
        private static VideoViewCategory _newsCategory = new VideoViewCategory(_tvViewHeader, _newsDescription);
        private static VideoViewCategory _musicCategory = new VideoViewCategory(_musicViewHeader, _musicDescription);
        private static VideoViewCategory _moviesCategory = new VideoViewCategory(_movieViewHeader, _moviesDescription);
        private static VideoViewCategory _otherCategory = new VideoViewCategory(_otherViewHeader, _otherDescription);
        private static VideoViewCategory _personalCategory = new VideoViewCategory(_personalViewHeader, _personalDescription);
        private static GroupedList _groupedCategories;

        public static int GetCategoryId(VideoCategory category) => (int)category;

        public static VideoCategory GetCategory(int id) => (VideoCategory)id;

        public static string GetDescription(int categoryId) => GetDescription((VideoCategory)categoryId);

        public static string GetDescription(VideoCategory category)
        {
            switch (category)
            {
                case VideoCategory.TV:
                    return _seriesDescription;
                case VideoCategory.News:
                    return _newsDescription;
                case VideoCategory.Music:
                    return _musicDescription;
                case VideoCategory.Movies:
                    return _moviesDescription;
                case VideoCategory.Personal:
                    return _personalDescription;
                case VideoCategory.Shorts:
                    return _shortsDescription;
                default:
                    return _otherDescription;
            }
        }

        public static VideoCategory GetCategory(string description)
        {
            if (description == _seriesDescription)
                return VideoCategory.TV;
            if (description == _shortsDescription)
                return VideoCategory.Shorts;
            if (description == _newsDescription)
                return VideoCategory.News;
            if (description == _musicDescription)
                return VideoCategory.Music;
            if (description == _moviesDescription)
                return VideoCategory.Movies;
            return description == _personalDescription ? VideoCategory.Personal : VideoCategory.Other;
        }

        public static GroupedList Categories
        {
            get
            {
                if (_groupedCategories == null)
                {
                    _groupedCategories = new GroupedList();
                    _groupedCategories.Comparer = new VideoViewCategoryComparer();
                    _groupedCategories.Source = (new VideoViewCategory[7]
                    {
            _tvCategory,
            _shortsCategory,
            _newsCategory,
            _musicCategory,
            _moviesCategory,
            _otherCategory,
            _personalCategory
                    });
                }
                return _groupedCategories;
            }
        }
    }
}
