// Decompiled with JetBrains decompiler
// Type: ZuneUI.TrackMetadataPropertySource
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using MicrosoftZuneLibrary;

namespace ZuneUI
{
    public class TrackMetadataPropertySource : PropertySource
    {
        private static PropertySource _instance;

        protected TrackMetadataPropertySource()
        {
        }

        public static PropertySource Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TrackMetadataPropertySource();
                return _instance;
            }
        }

        public override object Get(object media, PropertyDescriptor property)
        {
            TrackMetadata trackMetadata = media as TrackMetadata;
            string descriptorName = property.DescriptorName;
            if (descriptorName == MetadataEditMedia.s_Title.DescriptorName)
                return trackMetadata.TrackTitle;
            if (descriptorName == MetadataEditMedia.s_Artist.DescriptorName)
                return trackMetadata.TrackArtist;
            if (descriptorName == MetadataEditMedia.s_Genre.DescriptorName)
                return trackMetadata.Genre;
            if (descriptorName == MetadataEditMedia.s_Composer.DescriptorName)
                return trackMetadata.Composer;
            if (descriptorName == MetadataEditMedia.s_Conductor.DescriptorName)
                return trackMetadata.Conductor;
            if (descriptorName == MetadataEditMedia.s_TrackNumber.DescriptorName)
                return trackMetadata.TrackNumber;
            if (descriptorName == MetadataEditMedia.s_DiscNumber.DescriptorName)
                return trackMetadata.DiscNumber;
            return descriptorName == MetadataEditMedia.s_MediaId.DescriptorName ? trackMetadata.MediaId : (object)null;
        }

        public override void Set(object media, PropertyDescriptor property, object value)
        {
            TrackMetadata trackMetadata = media as TrackMetadata;
            string descriptorName = property.DescriptorName;
            if (descriptorName == MetadataEditMedia.s_Title.DescriptorName)
                trackMetadata.TrackTitle = (string)value;
            else if (descriptorName == MetadataEditMedia.s_Artist.DescriptorName)
                trackMetadata.TrackArtist = (string)value;
            else if (descriptorName == MetadataEditMedia.s_Genre.DescriptorName)
                trackMetadata.Genre = (string)value;
            else if (descriptorName == MetadataEditMedia.s_Composer.DescriptorName)
                trackMetadata.Composer = (string)value;
            else if (descriptorName == MetadataEditMedia.s_Conductor.DescriptorName)
                trackMetadata.Conductor = (string)value;
            else if (descriptorName == MetadataEditMedia.s_TrackNumber.DescriptorName)
                trackMetadata.TrackNumber = (int)value;
            else if (descriptorName == MetadataEditMedia.s_DiscNumber.DescriptorName)
            {
                trackMetadata.DiscNumber = (int)value;
            }
            else
            {
                if (!(descriptorName == MetadataEditMedia.s_MediaId.DescriptorName))
                    return;
                trackMetadata.MediaId = (int)value;
            }
        }
    }
}
