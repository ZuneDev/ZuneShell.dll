// Decompiled with JetBrains decompiler
// Type: ZuneUI.MetadataEditTrack
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using MicrosoftZuneLibrary;
using System.Collections;

namespace ZuneUI
{
    public class MetadataEditTrack : MetadataEditMedia
    {
        private static PropertyDescriptor[] s_dataProviderProperties = new PropertyDescriptor[14]
        {
       s_Title,
      s_TrackArtist,
      s_TrackArtistList,
      s_Genre,
      s_Conductor,
      s_Composer,
       s_TrackReleaseYear,
       s_AlbumTitle,
       s_AlbumArtist,
       s_TrackNumber,
       s_DiscNumber,
       s_MediaId,
      s_TitleYomi,
      s_ArtistYomi
        };
        private static PropertyDescriptor[] s_trackMetadataProperties = new PropertyDescriptor[14]
        {
       s_Title,
      s_TrackArtist,
      s_TrackArtistList,
      s_Genre,
      s_Conductor,
      s_Composer,
       s_ReleaseYear,
       s_AlbumTitle,
       s_AlbumArtist,
       s_TrackNumber,
       s_DiscNumber,
       s_MediaId,
      s_TitleYomi,
      s_ArtistYomi
        };

        public MetadataEditTrack(TrackMetadata trackMetadata)
        {
            this._source = TrackMetadataPropertySource.Instance;
            this.Initialize(new object[1]
            {
         trackMetadata
            }, s_trackMetadataProperties);
        }

        public MetadataEditTrack(IList trackList) => this.Initialize(trackList, s_dataProviderProperties);
    }
}
