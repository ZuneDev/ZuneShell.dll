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
      (PropertyDescriptor) MetadataEditMedia.s_Title,
      MetadataEditMedia.s_TrackArtist,
      MetadataEditMedia.s_TrackArtistList,
      MetadataEditMedia.s_Genre,
      MetadataEditMedia.s_Conductor,
      MetadataEditMedia.s_Composer,
      (PropertyDescriptor) MetadataEditMedia.s_TrackReleaseYear,
      (PropertyDescriptor) MetadataEditMedia.s_AlbumTitle,
      (PropertyDescriptor) MetadataEditMedia.s_AlbumArtist,
      (PropertyDescriptor) MetadataEditMedia.s_TrackNumber,
      (PropertyDescriptor) MetadataEditMedia.s_DiscNumber,
      (PropertyDescriptor) MetadataEditMedia.s_MediaId,
      MetadataEditMedia.s_TitleYomi,
      MetadataEditMedia.s_ArtistYomi
        };
        private static PropertyDescriptor[] s_trackMetadataProperties = new PropertyDescriptor[14]
        {
      (PropertyDescriptor) MetadataEditMedia.s_Title,
      MetadataEditMedia.s_TrackArtist,
      MetadataEditMedia.s_TrackArtistList,
      MetadataEditMedia.s_Genre,
      MetadataEditMedia.s_Conductor,
      MetadataEditMedia.s_Composer,
      (PropertyDescriptor) MetadataEditMedia.s_ReleaseYear,
      (PropertyDescriptor) MetadataEditMedia.s_AlbumTitle,
      (PropertyDescriptor) MetadataEditMedia.s_AlbumArtist,
      (PropertyDescriptor) MetadataEditMedia.s_TrackNumber,
      (PropertyDescriptor) MetadataEditMedia.s_DiscNumber,
      (PropertyDescriptor) MetadataEditMedia.s_MediaId,
      MetadataEditMedia.s_TitleYomi,
      MetadataEditMedia.s_ArtistYomi
        };

        public MetadataEditTrack(TrackMetadata trackMetadata)
        {
            this._source = TrackMetadataPropertySource.Instance;
            this.Initialize((IList)new object[1]
            {
        (object) trackMetadata
            }, MetadataEditTrack.s_trackMetadataProperties);
        }

        public MetadataEditTrack(IList trackList) => this.Initialize(trackList, MetadataEditTrack.s_dataProviderProperties);
    }
}
