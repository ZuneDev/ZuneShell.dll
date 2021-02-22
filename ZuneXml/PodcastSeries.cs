// Decompiled with JetBrains decompiler
// Type: ZuneXml.PodcastSeries
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using System;
using System.Collections;

namespace ZuneXml
{
    internal class PodcastSeries : Media
    {
        private int _dbMediaId = -1;

        internal override MediaRights Rights => (MediaRights)null;

        internal override MiniArtist PrimaryArtist => (MiniArtist)null;

        internal override IList Artists => (IList)null;

        internal override double Popularity => 0.0;

        internal virtual int LibraryId
        {
            get => this.GetLibraryId();
            set
            {
                if (this._dbMediaId == value)
                    return;
                this._dbMediaId = value;
                this.FirePropertyChanged(nameof(LibraryId));
            }
        }

        protected int GetLibraryId()
        {
            int dbMediaId = -1;
            if (this.Id != Guid.Empty)
                ZuneApplication.Service.InVisibleCollection(this.Id, EContentType.PodcastSeries, out dbMediaId);
            return dbMediaId;
        }

        internal static XmlDataProviderObject ConstructPodcastSeriesObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new PodcastSeries(owner, objectTypeCookie);
        }

        internal PodcastSeries(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal string ShortDescription => (string)base.GetProperty(nameof(ShortDescription));

        internal string LongDescription => (string)base.GetProperty(nameof(LongDescription));

        internal bool Explicit => (bool)base.GetProperty(nameof(Explicit));

        internal string Author => (string)base.GetProperty(nameof(Author));

        internal string SourceUrl => (string)base.GetProperty(nameof(SourceUrl));

        internal DateTime ReleaseDate => (DateTime)base.GetProperty(nameof(ReleaseDate));

        internal IList Categories => (IList)base.GetProperty(nameof(Categories));

        internal string Type => (string)base.GetProperty(nameof(Type));

        internal string WebsiteUrl => (string)base.GetProperty(nameof(WebsiteUrl));

        internal override Guid Id => (Guid)base.GetProperty(nameof(Id));

        internal override string Title => (string)base.GetProperty(nameof(Title));

        internal override string SortTitle => (string)base.GetProperty(nameof(SortTitle));

        internal override Guid ImageId => (Guid)base.GetProperty(nameof(ImageId));

        public override object GetProperty(string propertyName)
        {
            switch (propertyName)
            {
                case "LibraryId":
                    return (object)this.LibraryId;
                case "Rights":
                    return (object)this.Rights;
                case "PrimaryArtist":
                    return (object)this.PrimaryArtist;
                case "Artists":
                    return (object)this.Artists;
                case "Popularity":
                    return (object)this.Popularity;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
