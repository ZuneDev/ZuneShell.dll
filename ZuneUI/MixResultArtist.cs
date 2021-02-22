// Decompiled with JetBrains decompiler
// Type: ZuneUI.MixResultArtist
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using ZuneXml;

namespace ZuneUI
{
    public class MixResultArtist : MixResult
    {
        protected MixResultArtist()
        {
        }

        public static MixResultArtist CreateInstance(
          DataProviderObject dataProviderObject,
          string reason)
        {
            MixResultArtist mixResultArtist = new MixResultArtist();
            if (dataProviderObject.TypeName == "ArtistData")
                dataProviderObject = (DataProviderObject)dataProviderObject.GetProperty("Item");
            Artist artist = (Artist)dataProviderObject;
            mixResultArtist.Initialize(MixResultType.Artist, reason, artist.Title ?? string.Empty, string.Empty, artist.Id.ToString(), string.Empty, artist.ImageId, null);
            return mixResultArtist;
        }

        internal static int GetItemPriority(DataProviderObject item, int startPriority) => startPriority;
    }
}
