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
                if (TrackMetadataPropertySource._instance == null)
                    TrackMetadataPropertySource._instance = (PropertySource)new TrackMetadataPropertySource();
                return TrackMetadataPropertySource._instance;
            }
        }

        public override object Get(object media, PropertyDescriptor property)
        {
            TrackMetadata trackMetadata = media as TrackMetadata;
            string descriptorName = property.DescriptorName;
            if (descriptorName == MetadataEditMedia.s_Title.DescriptorName)
                return (object)trackMetadata.TrackTitle;
            if (descriptorName == MetadataEditMedia.s_Artist.DescriptorName)
                return (object)trackMetadata.TrackArtist;
            if (descriptorName == MetadataEditMedia.s_Genre.DescriptorName)
                return (object)trackMetadata.Genre;
            if (descriptorName == MetadataEditMedia.s_Composer.DescriptorName)
                return (object)trackMetadata.Composer;
            if (descriptorName == MetadataEditMedia.s_Conductor.DescriptorName)
                return (object)trackMetadata.Conductor;
            if (descriptorName == MetadataEditMedia.s_TrackNumber.DescriptorName)
                return (object)trackMetadata.TrackNumber;
            if (descriptorName == MetadataEditMedia.s_DiscNumber.DescriptorName)
                return (object)trackMetadata.DiscNumber;
            return descriptorName == MetadataEditMedia.s_MediaId.DescriptorName ? (object)trackMetadata.MediaId : (object)null;
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
