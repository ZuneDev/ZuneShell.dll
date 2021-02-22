// Decompiled with JetBrains decompiler
// Type: ZuneXml.ArtistEventList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneXml
{
    internal class ArtistEventList : ListResult
    {
        protected IList _concerts;

        internal IList Concerts
        {
            get
            {
                if (this._concerts == null)
                    this._concerts = this.FilterConcerts();
                return this._concerts;
            }
        }

        private IList FilterConcerts()
        {
            IList list = (IList)null;
            if (this.Items != null && this.Items.Count > 0)
            {
                list = (IList)new ArrayList(this.Items.Count);
                foreach (ArtistEvent artistEvent in (IEnumerable)this.Items)
                {
                    if (artistEvent.Type == "Concert")
                        list.Add((object)artistEvent);
                }
            }
            return list;
        }

        internal static XmlDataProviderObject ConstructArtistEventListObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new ArtistEventList(owner, objectTypeCookie);
        }

        internal ArtistEventList(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal override IList Items => (IList)base.GetProperty(nameof(Items));

        public override object GetProperty(string propertyName)
        {
            switch (propertyName)
            {
                case "Concerts":
                    return (object)this.Concerts;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
