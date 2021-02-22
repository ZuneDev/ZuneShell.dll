// Decompiled with JetBrains decompiler
// Type: ZuneUI.MetadataEditVideo
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;

namespace ZuneUI
{
    public class MetadataEditVideo : MetadataEditMedia
    {
        private static PropertyDescriptor[] s_properties = new PropertyDescriptor[12]
        {
      (PropertyDescriptor) MetadataEditMedia.s_Title,
      MetadataEditMedia.s_TitleYomi,
      (PropertyDescriptor) MetadataEditMedia.s_Artist,
      MetadataEditMedia.s_Genre,
      MetadataEditMedia.s_Conductor,
      MetadataEditMedia.s_Composer,
      (PropertyDescriptor) MetadataEditMedia.s_Category,
      MetadataEditMedia.s_SeriesTitle,
      (PropertyDescriptor) MetadataEditMedia.s_SeasonNumber,
      (PropertyDescriptor) MetadataEditMedia.s_EpisodeNumber,
      (PropertyDescriptor) MetadataEditMedia.s_ReleaseDate,
      MetadataEditMedia.s_Description
        };

        public MetadataEditVideo(IList videoList) => this.Initialize(videoList, MetadataEditVideo.s_properties);
    }
}
