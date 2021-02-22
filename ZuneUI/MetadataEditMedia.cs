// Decompiled with JetBrains decompiler
// Type: ZuneUI.MetadataEditMedia
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public class MetadataEditMedia
    {
        public static PropertyDescriptor s_TrackArtist = new PropertyDescriptor("ArtistName", Shell.LoadString(StringId.IDS_EMI_MULTIPLEARTISTS), "");
        public static PropertyDescriptor s_TrackArtistList = (PropertyDescriptor)new StringListPropertyDescriptor("ContributingArtistNames", Shell.LoadString(StringId.IDS_EMI_MULTIPLEARTISTS), "");
        public static NonBlankPropertyDescriptor s_Artist = new NonBlankPropertyDescriptor("ArtistName", Shell.LoadString(StringId.IDS_EMI_MULTIPLEARTISTS), "");
        public static PropertyDescriptor s_ArtistYomi = new PropertyDescriptor("ArtistNameYomi", "", "");
        public static NonBlankPropertyDescriptor s_Title = new NonBlankPropertyDescriptor("Title", Shell.LoadString(StringId.IDS_EMI_MULTIPLETITLES), "");
        public static PropertyDescriptor s_TitleYomi = new PropertyDescriptor("TitleYomi", "", "");
        public static IntYearPropertyDescriptor s_ReleaseYear = new IntYearPropertyDescriptor("ReleaseDate", Shell.LoadString(StringId.IDS_EMI_MULTIPLEYEARS), Shell.LoadString(StringId.IDS_UNKNOWN_YEAR), 4);
        public static YearPropertyDescriptor s_TrackReleaseYear = new YearPropertyDescriptor("ReleaseDate", Shell.LoadString(StringId.IDS_EMI_MULTIPLEYEARS), Shell.LoadString(StringId.IDS_UNKNOWN_YEAR), 4);
        public static DatePropertyDescriptor s_ReleaseDate = new DatePropertyDescriptor("ReleaseDate", Shell.LoadString(StringId.IDS_EMI_MULTIPLEDATES), Shell.LoadString(StringId.IDS_RELEASE_DATE_UNKNOWN), DateTimeKind.Utc);
        public static IntPropertyDescriptor s_DiscNumber = new IntPropertyDescriptor("DiscNumber", Shell.LoadString(StringId.IDS_EMI_MULTIPLEDISCS), "", 3);
        public static IntPropertyDescriptor s_TrackNumber = new IntPropertyDescriptor("TrackNumber", "0", "", 3);
        public static PropertyDescriptor s_SeriesTitle = new PropertyDescriptor("SeriesTitle", Shell.LoadString(StringId.IDS_EMI_MULTIPLETITLES), "");
        public static IntPropertyDescriptor s_SeasonNumber = new IntPropertyDescriptor("SeasonNumber", "", "", 6);
        public static IntPropertyDescriptor s_EpisodeNumber = new IntPropertyDescriptor("EpisodeNumber", "", "", 6);
        public static NonBlankPropertyDescriptor s_AlbumTitle = new NonBlankPropertyDescriptor("AlbumName", Shell.LoadString(StringId.IDS_EMI_MULTIPLETITLES), "");
        public static PropertyDescriptor s_AlbumTitleYomi = new PropertyDescriptor("AlbumTitleYomi", "", "");
        public static NonBlankPropertyDescriptor s_AlbumArtist = new NonBlankPropertyDescriptor("AlbumArtistName", Shell.LoadString(StringId.IDS_EMI_MULTIPLEARTISTS), "");
        public static PropertyDescriptor s_AlbumArtistYomi = new PropertyDescriptor("AlbumArtistYomi", "", "");
        public static PropertyDescriptor s_Composer = new PropertyDescriptor("ComposerName", Shell.LoadString(StringId.IDS_EMI_MULTIPLECOMPOSERS), "");
        public static PropertyDescriptor s_Conductor = new PropertyDescriptor("ConductorName", Shell.LoadString(StringId.IDS_EMI_MULTIPLECONDUCTORS), "");
        public static PropertyDescriptor s_Genre = new PropertyDescriptor("Genre", Shell.LoadString(StringId.IDS_EMI_MULTIPLEGENRES), Shell.LoadString(StringId.IDS_GENRE_UNKNOWN));
        public static TypePropertyDescriptor s_Category = new TypePropertyDescriptor("CategoryId", "", "");
        public static PropertyDescriptor s_Description = new PropertyDescriptor("Description", Shell.LoadString(StringId.IDS_EMI_MULTIPLEDESCRIPTIONS), "");
        public static PropertyDescriptor s_CoverUrl = new PropertyDescriptor("CoverUrl", "", "");
        public static IntPropertyDescriptor s_TrackCount = new IntPropertyDescriptor("TrackCount", "0", "");
        public static IntPropertyDescriptor s_MediaId = new IntPropertyDescriptor("MediaId", "-1", "-1");
        private IList _properties;
        private IList _mediaList;
        private Dictionary<PropertyDescriptor, MetadataEditProperty> _propertyMap = new Dictionary<PropertyDescriptor, MetadataEditProperty>();
        protected PropertySource _source = DataProviderObjectPropertySource.Instance;
        protected bool _creationFailed;

        protected void Initialize(IList mediaList, PropertyDescriptor[] properties)
        {
            this._mediaList = mediaList;
            this._properties = (IList)properties;
            foreach (PropertyDescriptor property in (IEnumerable)this._properties)
                this._propertyMap[property] = this.CreatePropertyFromMedia(this._mediaList, property);
        }

        public MetadataEditProperty GetProperty(PropertyDescriptor descriptor) => this._propertyMap[descriptor];

        public MetadataEditProperty GetProperty(string descriptorName)
        {
            MetadataEditProperty metadataEditProperty1 = (MetadataEditProperty)null;
            foreach (MetadataEditProperty metadataEditProperty2 in this._propertyMap.Values)
            {
                if (metadataEditProperty2.Descriptor.DescriptorName.Equals(descriptorName, StringComparison.InvariantCultureIgnoreCase))
                {
                    metadataEditProperty1 = metadataEditProperty2;
                    break;
                }
            }
            return metadataEditProperty1;
        }

        public object GetPropertyData(PropertyDescriptor descriptor)
        {
            object obj = (object)null;
            MetadataEditProperty property = this.GetProperty(descriptor);
            if (property != null)
                obj = property.ConvertToData();
            return obj;
        }

        public void SetPropertyData(PropertyDescriptor descriptor, object value) => this.GetProperty(descriptor)?.ConvertFromData(value);

        public void SetPropertyState(PropertyDescriptor descriptor, object state)
        {
            MetadataEditProperty property = this.GetProperty(descriptor);
            if (property == null)
                return;
            property.State = state;
        }

        public void ResetExternalErrors()
        {
            foreach (MetadataEditProperty metadataEditProperty in this._propertyMap.Values)
                metadataEditProperty.ExternalError = HRESULT._S_OK;
        }

        public virtual void Commit()
        {
            foreach (PropertyDescriptor property1 in (IEnumerable)this._properties)
            {
                MetadataEditProperty property2 = this._propertyMap[property1];
                if (property2.Modified)
                    this.SetPropertyToMedia(property2);
            }
            if (!this._source.NeedsCommit)
                return;
            foreach (object media in (IEnumerable)this._mediaList)
                this._source.Commit(media);
        }

        public virtual bool IsValid()
        {
            foreach (MetadataEditProperty metadataEditProperty in this._propertyMap.Values)
            {
                if ((metadataEditProperty.Modified || metadataEditProperty.Required) && !metadataEditProperty.Valid)
                    return false;
            }
            return true;
        }

        public virtual bool IsModified()
        {
            foreach (MetadataEditProperty metadataEditProperty in this._propertyMap.Values)
            {
                if (metadataEditProperty.Modified)
                    return true;
            }
            return false;
        }

        public bool CreationFailed => this._creationFailed;

        private MetadataEditProperty CreatePropertyFromMedia(
          IList mediaList,
          PropertyDescriptor descriptor)
        {
            string str = descriptor.UnknownString;
            foreach (object media in (IEnumerable)this._mediaList)
            {
                object obj = this._source.Get(media, descriptor);
                string newString = descriptor.ConvertToString(obj) ?? descriptor.UnknownString;
                str = this.AggregateString(str, newString, descriptor);
            }
            return new MetadataEditProperty(descriptor, str);
        }

        private void SetPropertyToMedia(MetadataEditProperty property)
        {
            foreach (object media in (IEnumerable)this._mediaList)
                this._source.Set(media, property.Descriptor, property.Descriptor.ConvertFromString(property.Value, property.State));
        }

        protected string AggregateString(
          string oldString,
          string newString,
          PropertyDescriptor descriptor)
        {
            string str = oldString;
            if (str != newString)
                str = !(str == descriptor.UnknownString) ? descriptor.MultiValueString : newString;
            return str;
        }

        public static PropertyDescriptor TrackArtistDescriptor => MetadataEditMedia.s_TrackArtist;

        public static PropertyDescriptor TrackArtistListDescriptor => MetadataEditMedia.s_TrackArtistList;

        public static PropertyDescriptor ArtistDescriptor => (PropertyDescriptor)MetadataEditMedia.s_Artist;

        public static PropertyDescriptor ArtistYomiDescriptor => MetadataEditMedia.s_ArtistYomi;

        public static PropertyDescriptor TitleDescriptor => (PropertyDescriptor)MetadataEditMedia.s_Title;

        public static PropertyDescriptor TitleYomiDescriptor => MetadataEditMedia.s_TitleYomi;

        public static PropertyDescriptor ReleaseYearDescriptor => (PropertyDescriptor)MetadataEditMedia.s_ReleaseYear;

        public static PropertyDescriptor TrackReleaseYearDescriptor => (PropertyDescriptor)MetadataEditMedia.s_TrackReleaseYear;

        public static PropertyDescriptor DiscNumberDescriptor => (PropertyDescriptor)MetadataEditMedia.s_DiscNumber;

        public static PropertyDescriptor TrackNumberDescriptor => (PropertyDescriptor)MetadataEditMedia.s_TrackNumber;

        public static PropertyDescriptor SeriesTitleDescriptor => MetadataEditMedia.s_SeriesTitle;

        public static PropertyDescriptor SeasonNumberDescriptor => (PropertyDescriptor)MetadataEditMedia.s_SeasonNumber;

        public static PropertyDescriptor EpisodeNumberDescriptor => (PropertyDescriptor)MetadataEditMedia.s_EpisodeNumber;

        public static PropertyDescriptor AlbumTitleDescriptor => (PropertyDescriptor)MetadataEditMedia.s_AlbumTitle;

        public static PropertyDescriptor AlbumTitleYomiDescriptor => MetadataEditMedia.s_AlbumTitleYomi;

        public static PropertyDescriptor AlbumArtistDescriptor => (PropertyDescriptor)MetadataEditMedia.s_AlbumArtist;

        public static PropertyDescriptor AlbumArtistYomiDescriptor => MetadataEditMedia.s_AlbumArtistYomi;

        public static PropertyDescriptor ComposerDescriptor => MetadataEditMedia.s_Composer;

        public static PropertyDescriptor ConductorDescriptor => MetadataEditMedia.s_Conductor;

        public static PropertyDescriptor GenreDescriptor => MetadataEditMedia.s_Genre;

        public static PropertyDescriptor CategoryDescriptor => (PropertyDescriptor)MetadataEditMedia.s_Category;

        public static PropertyDescriptor ReleaseDateDescriptor => (PropertyDescriptor)MetadataEditMedia.s_ReleaseDate;

        public static PropertyDescriptor DescriptionDescriptor => MetadataEditMedia.s_Description;

        public static PropertyDescriptor CoverUrlDescriptor => MetadataEditMedia.s_CoverUrl;

        public static PropertyDescriptor TrackCountDescriptor => (PropertyDescriptor)MetadataEditMedia.s_TrackCount;

        public static PropertyDescriptor MediaIdDescriptor => (PropertyDescriptor)MetadataEditMedia.s_MediaId;
    }
}
