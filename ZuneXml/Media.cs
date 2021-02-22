// Decompiled with JetBrains decompiler
// Type: ZuneXml.Media
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;

namespace ZuneXml
{
    internal abstract class Media : MiniMedia
    {
        internal string Artist
        {
            get
            {
                string str = (string)null;
                MiniArtist primaryArtist = this.PrimaryArtist;
                if (primaryArtist != null)
                {
                    str = primaryArtist.Title;
                }
                else
                {
                    IList artists = this.Artists;
                    if (artists != null)
                    {
                        IEnumerator enumerator = artists.GetEnumerator();
                        try
                        {
                            if (enumerator.MoveNext())
                                str = ((MiniMedia)enumerator.Current).Title;
                        }
                        finally
                        {
                            if (enumerator is IDisposable disposable)
                                disposable.Dispose();
                        }
                    }
                }
                return str;
            }
        }

        protected Media(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal abstract string SortTitle { get; }

        internal abstract Guid ImageId { get; }

        internal abstract MediaRights Rights { get; }

        internal abstract MiniArtist PrimaryArtist { get; }

        internal abstract IList Artists { get; }

        internal abstract double Popularity { get; }
    }
}
