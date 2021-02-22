// Decompiled with JetBrains decompiler
// Type: ZuneUI.PodcastTypeFilteringList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using ZuneXml;

namespace ZuneUI
{
    public class PodcastTypeFilteringList : FilterList
    {
        private const string PodcastTypeAudio = "audio";
        private const string PodcastTypeVideo = "video";
        private const string PodcastTypeAudioAndVideo = "both";
        private string _filterType;

        public string FilterType
        {
            get => this._filterType;
            set
            {
                if (!(this._filterType != value))
                    return;
                this._filterType = value;
                this.FirePropertyChanged(nameof(FilterType));
                this.ProduceFilteredList();
            }
        }

        protected override bool ShouldIncludeItem(int sourceIndex, int targetIndex, object item)
        {
            if (!(item is PodcastSeries podcastSeries))
                return false;
            string type = podcastSeries.Type;
            return this._filterType == null || type == this._filterType;
        }
    }
}
