// Decompiled with JetBrains decompiler
// Type: ZuneUI.MediaIdAndType
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using MicrosoftZuneLibrary;

namespace ZuneUI
{
    public class MediaIdAndType : IDatabaseMedia
    {
        private int _id;
        private EMediaTypes _type;

        public MediaIdAndType(int mediaId, MediaType type)
          : this(mediaId, (EMediaTypes)type)
        {
        }

        public MediaIdAndType(int mediaId, EMediaTypes type)
        {
            this._id = mediaId;
            this._type = type;
        }

        public void GetMediaIdAndType(out int mediaId, out EMediaTypes mediaType)
        {
            mediaId = this._id;
            mediaType = this._type;
        }
    }
}
