// Decompiled with JetBrains decompiler
// Type: ZuneUI.MetadataEditAlbum
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Shell;
using MicrosoftZuneLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UIXControls;

namespace ZuneUI
{
    public class MetadataEditAlbum : MetadataEditMedia
    {
        private static PropertyDescriptor[] s_properties = new PropertyDescriptor[9]
        {
       s_Title,
      s_AlbumTitleYomi,
       s_Artist,
      s_AlbumArtistYomi,
      s_Genre,
      s_Conductor,
      s_Composer,
       s_ReleaseYear,
      s_CoverUrl
        };
        private List<PropertyDescriptor> _linkedProperties = new List<PropertyDescriptor>(new PropertyDescriptor[3]
        {
      s_Genre,
      s_Conductor,
      s_Composer
        });
        private List<MetadataEditTrack> _trackList = new List<MetadataEditTrack>();

        public MetadataEditAlbum(AlbumMetadata albumMetadata) => this.InitializeFromMetadataList(new List<AlbumMetadata>(new AlbumMetadata[1]
        {
      albumMetadata
        }));

        public MetadataEditAlbum(IList albumList)
        {
            List<AlbumMetadata> albumMetadataList = new List<AlbumMetadata>(albumList.Count);
            try
            {
                foreach (DataProviderObject album in albumList)
                {
                    AlbumMetadata albumMetadata = ZuneApplication.ZuneLibrary.GetAlbumMetadata((int)album.GetProperty("LibraryId"));
                    albumMetadataList.Add(albumMetadata);
                }
            }
            catch (Exception ex)
            {
                this._creationFailed = true;
                albumMetadataList.Clear();
                MessageBox.Show(Shell.LoadString(StringId.IDS_EMI_UPDATEFAILED_TITLE), Shell.LoadString(StringId.IDS_EMI_UPDATEFAILED), null);
            }
            this.InitializeFromMetadataList(albumMetadataList);
        }

        private void InitializeFromMetadataList(List<AlbumMetadata> albumMetadataList)
        {
            this._source = AlbumMetadataPropertySource.Instance;
            this.Initialize(albumMetadataList, s_properties);
            foreach (AlbumMetadata albumMetadata in albumMetadataList)
            {
                for (uint index = 0; index < albumMetadata.TrackCount; ++index)
                    this._trackList.Add(new MetadataEditTrack(albumMetadata.GetTrack(index)));
                foreach (PropertyDescriptor linkedProperty in this._linkedProperties)
                {
                    this.GetProperty(linkedProperty).OriginalValue = this.GetPropertyStringFromTracks(linkedProperty);
                    this.GetProperty(linkedProperty).PropertyChanged += new PropertyChangedEventHandler(this.LinkedAlbumPropertyChanged);
                    foreach (MetadataEditMedia track in this._trackList)
                        track.GetProperty(linkedProperty).PropertyChanged += new PropertyChangedEventHandler(this.LinkedTrackPropertyChanged);
                }
            }
        }

        public override void Commit()
        {
            foreach (MetadataEditMedia track in this._trackList)
                track.Commit();
            base.Commit();
        }

        public override bool IsValid()
        {
            foreach (MetadataEditMedia track in this._trackList)
            {
                if (!track.IsValid())
                    return false;
            }
            return base.IsValid();
        }

        public override bool IsModified()
        {
            foreach (MetadataEditMedia track in this._trackList)
            {
                if (track.IsModified())
                    return true;
            }
            return base.IsModified();
        }

        private void LinkedTrackPropertyChanged(object sender, PropertyChangedEventArgs ebase)
        {
            MetadataPropertyChangedEventArgs changedEventArgs = ebase as MetadataPropertyChangedEventArgs;
            PropertyDescriptor descriptor = ((MetadataEditProperty)sender).Descriptor;
            if (!(changedEventArgs.PropertyName == "Value") || !this._linkedProperties.Contains(descriptor) || !changedEventArgs.Propagate)
                return;
            this.GetProperty(descriptor).SetValue(this.GetPropertyStringFromTracks(descriptor), false);
        }

        private void LinkedAlbumPropertyChanged(object sender, PropertyChangedEventArgs ebase)
        {
            MetadataPropertyChangedEventArgs changedEventArgs = ebase as MetadataPropertyChangedEventArgs;
            PropertyDescriptor descriptor = ((MetadataEditProperty)sender).Descriptor;
            if (!(changedEventArgs.PropertyName == "Value") || !this._linkedProperties.Contains(descriptor) || !changedEventArgs.Propagate)
                return;
            foreach (MetadataEditMedia track in this._trackList)
                track.GetProperty(descriptor).SetValue(this.GetProperty(descriptor).Value, false);
        }

        private string GetPropertyStringFromTracks(PropertyDescriptor descriptor)
        {
            string oldString = descriptor.UnknownString;
            foreach (MetadataEditMedia track in this._trackList)
            {
                string newString = track.GetProperty(descriptor).Value;
                oldString = this.AggregateString(oldString, newString, descriptor);
            }
            return oldString;
        }

        public IList GetTracks() => this.GetTracks(false);

        public IList GetTracks(bool filterAndSort)
        {
            if (!filterAndSort)
                return _trackList;
            List<MetadataEditTrack> metadataEditTrackList = new List<MetadataEditTrack>();
            foreach (MetadataEditTrack track in this._trackList)
            {
                if (track.GetProperty(s_MediaId).Value != "-1")
                    metadataEditTrackList.Add(track);
            }
            metadataEditTrackList.Sort(new TrackComparer());
            return metadataEditTrackList;
        }

        public int GetFirstInvalidTrackIndex()
        {
            int num = 0;
            foreach (MetadataEditMedia track in this._trackList)
            {
                if (!track.IsValid())
                    return num;
                ++num;
            }
            return -1;
        }
    }
}
