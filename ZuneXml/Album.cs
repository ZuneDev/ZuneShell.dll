// Decompiled with JetBrains decompiler
// Type: ZuneXml.Album
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
    internal class Album : Media
    {
        private int _dbMediaId = -1;

        internal bool CanPurchaseMP3 => this.Rights.HasOfferRights(MediaRightsEnum.AlbumPurchase, AudioEncodingEnum.MP3, PriceTypeEnum.Points, out Right _);

        internal bool CanPurchase
        {
            get
            {
                Right offer;
                return this.Rights.HasOfferRights(MediaRightsEnum.AlbumPurchase, AudioEncodingEnum.WMA, PriceTypeEnum.Points, out offer) || this.Rights.HasOfferRights(MediaRightsEnum.AlbumPurchase, AudioEncodingEnum.MP3, PriceTypeEnum.Points, out offer);
            }
        }

        internal int PointsPrice
        {
            get
            {
                int num = -1;
                Right offer;
                if (this.Rights.HasOfferRights(MediaRightsEnum.AlbumPurchase, AudioEncodingEnum.MP3, PriceTypeEnum.Points, out offer))
                    num = offer.PointsPrice;
                else if (this.Rights.HasOfferRights(MediaRightsEnum.AlbumPurchase, AudioEncodingEnum.WMA, PriceTypeEnum.Points, out offer))
                    num = offer.PointsPrice;
                return num;
            }
        }

        internal bool InCollection
        {
            get
            {
                bool flag = false;
                if (this.Id != Guid.Empty)
                    flag = ZuneApplication.Service.InVisibleCollection(this.Id, EContentType.MusicAlbum, out int _);
                return flag;
            }
        }

        internal int LibraryId
        {
            get
            {
                int dbMediaId = -1;
                if (this.Id != Guid.Empty)
                {
                    ZuneApplication.Service.InVisibleCollection(this.Id, EContentType.MusicAlbum, out dbMediaId);
                    if (dbMediaId == -1)
                        dbMediaId = this._dbMediaId;
                }
                return dbMediaId;
            }
            set
            {
                if (this._dbMediaId == value)
                    return;
                this._dbMediaId = value;
                this.FirePropertyChanged(nameof(LibraryId));
            }
        }

        internal static XmlDataProviderObject ConstructAlbumObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return new Album(owner, objectTypeCookie);
        }

        internal Album(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal Genre PrimaryGenre => (Genre)base.GetProperty(nameof(PrimaryGenre));

        internal DateTime ReleaseDate => (DateTime)base.GetProperty(nameof(ReleaseDate));

        internal bool Explicit => (bool)base.GetProperty(nameof(Explicit));

        internal bool Actionable => (bool)base.GetProperty(nameof(Actionable));

        internal bool Premium => (bool)base.GetProperty(nameof(Premium));

        internal string Label => (string)base.GetProperty(nameof(Label));

        internal string ReviewLink => (string)base.GetProperty(nameof(ReviewLink));

        internal IList Genres => (IList)base.GetProperty(nameof(Genres));

        internal IList Tracks => (IList)base.GetProperty(nameof(Tracks));

        internal IList MusicVideos => (IList)base.GetProperty(nameof(MusicVideos));

        internal override string Title => (string)base.GetProperty(nameof(Title));

        internal override string SortTitle => (string)base.GetProperty(nameof(SortTitle));

        internal override Guid Id => (Guid)base.GetProperty(nameof(Id));

        internal override MiniArtist PrimaryArtist => (MiniArtist)base.GetProperty(nameof(PrimaryArtist));

        internal override Guid ImageId => (Guid)base.GetProperty(nameof(ImageId));

        internal override double Popularity => (double)base.GetProperty(nameof(Popularity));

        internal override MediaRights Rights => (MediaRights)base.GetProperty(nameof(Rights));

        internal override IList Artists => (IList)base.GetProperty(nameof(Artists));

        public override object GetProperty(string propertyName)
        {
            switch (propertyName)
            {
                case "PointsPrice":
                    return PointsPrice;
                case "CanPurchase":
                    return CanPurchase;
                case "CanPurchaseMP3":
                    return CanPurchaseMP3;
                case "InCollection":
                    return InCollection;
                case "LibraryId":
                    return LibraryId;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
