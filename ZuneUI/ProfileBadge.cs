// Decompiled with JetBrains decompiler
// Type: ZuneUI.ProfileBadge
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;
using ZuneXml;

namespace ZuneUI
{
    public class ProfileBadge : NotifyPropertyChangedImpl
    {
        private BadgeData _rawData;
        private DataProviderObject _mediaData;

        public ProfileBadge(DataProviderObject rawData, DataProviderObject mediaData)
        {
            this._rawData = (BadgeData)rawData;
            this._mediaData = mediaData;
        }

        public DataProviderObject RawData => _rawData;

        public DataProviderObject MediaData
        {
            get => this._mediaData;
            set
            {
                if (this._mediaData == value)
                    return;
                this._mediaData = value;
                this.FirePropertyChanged(nameof(MediaData));
            }
        }

        public static GroupingComparer CreateGroupingComparer() => new GroupingComparer();

        public class GroupingComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                int num = -1;
                BadgeData badgeData1 = null;
                BadgeData badgeData2 = null;
                if (x is ProfileBadge)
                    badgeData1 = ((ProfileBadge)x)._rawData;
                else if (x is BadgeData)
                    badgeData1 = (BadgeData)x;
                if (y is ProfileBadge)
                    badgeData2 = ((ProfileBadge)y)._rawData;
                else if (y is BadgeData)
                    badgeData2 = (BadgeData)y;
                if (badgeData1 != null && badgeData2 != null)
                {
                    int typeId1 = badgeData1.TypeId;
                    int typeId2 = badgeData2.TypeId;
                    if (typeId1 < 0 && typeId2 < 0)
                    {
                        string type1 = badgeData1.Type;
                        string type2 = badgeData2.Type;
                        if (type1 != null && type2 != null)
                            num = string.Compare(type1, type2);
                    }
                    else if (typeId1 == typeId2)
                        num = 0;
                    else if (typeId1 > typeId2)
                        num = 1;
                }
                return num;
            }
        }
    }
}
