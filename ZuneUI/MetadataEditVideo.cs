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
       s_Title,
      s_TitleYomi,
       s_Artist,
      s_Genre,
      s_Conductor,
      s_Composer,
       s_Category,
      s_SeriesTitle,
       s_SeasonNumber,
       s_EpisodeNumber,
       s_ReleaseDate,
      s_Description
        };

        public MetadataEditVideo(IList videoList) => this.Initialize(videoList, s_properties);
    }
}
