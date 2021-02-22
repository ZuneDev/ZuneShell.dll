// Decompiled with JetBrains decompiler
// Type: ZuneXml.Series
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;

namespace ZuneXml
{
    internal class Series : Media
    {
        internal override MediaRights Rights => (MediaRights)null;

        internal override MiniArtist PrimaryArtist => (MiniArtist)null;

        internal override IList Artists => (IList)null;

        internal static XmlDataProviderObject ConstructSeriesObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new Series(owner, objectTypeCookie);
        }

        internal Series(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal DateTime ReleaseDate => (DateTime)base.GetProperty(nameof(ReleaseDate));

        internal string Rating => (string)base.GetProperty(nameof(Rating));

        internal int SeasonCount => (int)base.GetProperty(nameof(SeasonCount));

        internal string ProductionCompany => (string)base.GetProperty(nameof(ProductionCompany));

        internal string Description => (string)base.GetProperty(nameof(Description));

        internal IList Categories => (IList)base.GetProperty(nameof(Categories));

        internal Network Network => (Network)base.GetProperty(nameof(Network));

        internal override Guid Id => (Guid)base.GetProperty(nameof(Id));

        internal override string Title => (string)base.GetProperty(nameof(Title));

        internal override Guid ImageId => (Guid)base.GetProperty(nameof(ImageId));

        internal override string SortTitle => (string)base.GetProperty(nameof(SortTitle));

        internal override double Popularity => (double)base.GetProperty(nameof(Popularity));

        public override object GetProperty(string propertyName)
        {
            switch (propertyName)
            {
                case "Rights":
                    return (object)this.Rights;
                case "PrimaryArtist":
                    return (object)this.PrimaryArtist;
                case "Artists":
                    return (object)this.Artists;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
