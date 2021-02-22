// Decompiled with JetBrains decompiler
// Type: ZuneUI.StringListPropertyDescriptor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;

namespace ZuneUI
{
    public class StringListPropertyDescriptor : PropertyDescriptor
    {
        public StringListPropertyDescriptor(string name, string multiValueString, string unknownString)
          : base(name, multiValueString, unknownString)
        {
        }

        public override string ConvertToString(object value) => value != null ? TrackDetails.ContributingArtistListToString((IList)value) : (string)null;

        public override object ConvertFromString(string value) => value != null ? (object)TrackDetails.ContributingArtistStringToList(value) : (object)null;
    }
}
