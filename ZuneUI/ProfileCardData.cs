// Decompiled with JetBrains decompiler
// Type: ZuneUI.ProfileCardData
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using MicrosoftZuneLibrary;
using ZuneXml;

namespace ZuneUI
{
    public class ProfileCardData : IDatabaseMedia
    {
        private LibraryDataProviderListItem _libraryData;
        private XmlDataProviderObject _serviceData;
        private ProfileInterests _profileInterests;
        private bool _isFriend;
        private string _zuneTag;
        private int _badgeCount;

        internal ProfileCardData(
          LibraryDataProviderListItem libraryData,
          XmlDataProviderObject serviceData)
        {
            this._libraryData = libraryData;
            this._serviceData = serviceData;
            this._isFriend = false;
            this._badgeCount = -1;
        }

        public static ProfileCardData Create(object item1, object item2)
        {
            LibraryDataProviderListItem libraryData = (LibraryDataProviderListItem)null;
            XmlDataProviderObject serviceData = (XmlDataProviderObject)null;
            if (item1 is LibraryDataProviderListItem)
                libraryData = (LibraryDataProviderListItem)item1;
            else if (item1 is XmlDataProviderObject)
                serviceData = (XmlDataProviderObject)item1;
            switch (item2)
            {
                case LibraryDataProviderListItem _:
                    libraryData = (LibraryDataProviderListItem)item2;
                    break;
                case XmlDataProviderObject _:
                    serviceData = (XmlDataProviderObject)item2;
                    break;
            }
            return new ProfileCardData(libraryData, serviceData);
        }

        public bool IsFriend
        {
            get => this._isFriend;
            set => this._isFriend = value;
        }

        public int BadgeCount
        {
            get => this._badgeCount;
            set => this._badgeCount = value;
        }

        public string ZuneTag
        {
            get
            {
                if (this._zuneTag == null)
                {
                    if (this.LibraryData != null)
                        this._zuneTag = this.LibraryData.GetProperty(nameof(ZuneTag)) as string;
                    else if (this.ServiceData != null)
                        this._zuneTag = this.ServiceData.GetProperty(nameof(ZuneTag)) as string;
                }
                return this._zuneTag;
            }
        }

        public DataProviderObject LibraryData => (DataProviderObject)this._libraryData;

        public DataProviderObject ServiceData => (DataProviderObject)this._serviceData;

        public ProfileInterests ProfileInterests
        {
            get
            {
                if (this._profileInterests == null)
                    this._profileInterests = new ProfileInterests();
                return this._profileInterests;
            }
        }

        public void GetMediaIdAndType(out int mediaId, out EMediaTypes mediaType)
        {
            if (this._libraryData != null)
            {
                this._libraryData.GetMediaIdAndType(out mediaId, out mediaType);
            }
            else
            {
                mediaId = -1;
                mediaType = EMediaTypes.eMediaTypeInvalid;
            }
        }

        public static object GetDataProviderObject(object data)
        {
            object obj = (object)null;
            switch (data)
            {
                case ProfileCardData _:
                    ProfileCardData profileCardData = (ProfileCardData)data;
                    obj = profileCardData.ServiceData == null ? (object)profileCardData.LibraryData : (object)profileCardData.ServiceData;
                    break;
                case DataProviderObject _:
                    obj = data;
                    break;
            }
            return obj;
        }

        public static object GetLibraryDataProviderListItem(object data)
        {
            object obj = (object)null;
            switch (data)
            {
                case ProfileCardData _:
                    obj = (object)((ProfileCardData)data).LibraryData;
                    break;
                case LibraryDataProviderListItem _:
                    obj = data;
                    break;
            }
            return obj;
        }

        public static object GetXmlDataProviderObject(object data)
        {
            object obj = (object)null;
            switch (data)
            {
                case ProfileCardData _:
                    obj = (object)((ProfileCardData)data).ServiceData;
                    break;
                case XmlDataProviderObject _:
                    obj = data;
                    break;
            }
            return obj;
        }
    }
}
