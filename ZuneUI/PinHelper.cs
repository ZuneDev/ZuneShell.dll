// Decompiled with JetBrains decompiler
// Type: ZuneUI.PinHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;

namespace ZuneUI
{
    public class PinHelper
    {
        public static int AddPin(int id, EMediaTypes mediaType, int userId) => AddPin(EPinType.ePinTypeGeneric, -1, id, mediaType, userId);

        public static int AddPin(
          EPinType pinType,
          int pinOrdinal,
          int id,
          EMediaTypes mediaType,
          int userId)
        {
            int pinId;
            PinManager.Instance.AddPin(pinType, id, mediaType, userId, pinOrdinal, out pinId);
            SingletonModelItem<JumpListManager>.Instance.JumpListPinUpdateRequested.Invoke();
            return pinId;
        }

        public static int AddPin(
          string moniker,
          EServiceMediaType mediaType,
          string description,
          int userId)
        {
            return AddPin(EPinType.ePinTypeGeneric, description, -1, moniker, mediaType, userId);
        }

        public static int AddPin(
          EPinType pinType,
          string description,
          int pinOrdinal,
          string moniker,
          EServiceMediaType mediaType,
          int userId)
        {
            int nPinId;
            PinManager.Instance.AddPin(pinType, moniker, description, mediaType, userId, pinOrdinal, out nPinId);
            SingletonModelItem<JumpListManager>.Instance.JumpListPinUpdateRequested.Invoke();
            return nPinId;
        }

        public static int FindPin(int id, EMediaTypes type, int userId, int maxAge)
        {
            int nPinId;
            PinManager.Instance.FindPin(EPinType.ePinTypeGeneric, id, type, userId, maxAge, out nPinId);
            return nPinId;
        }

        public static int FindPin(string moniker, EServiceMediaType type, int userId, int maxAge)
        {
            int nPinId;
            PinManager.Instance.FindPin(EPinType.ePinTypeGeneric, moniker, type, userId, maxAge, out nPinId);
            return nPinId;
        }

        public static void DeletePin(int id)
        {
            PinManager.Instance.DeletePin(id);
            SingletonModelItem<JumpListManager>.Instance.JumpListPinUpdateRequested.Invoke();
        }

        public static EServiceMediaType MapMarketplaceObjectToServiceMediaType(
          DataProviderObject dataProviderObject)
        {
            switch (dataProviderObject.TypeName)
            {
                case "Album":
                    return EServiceMediaType.eServiceMediaTypeAlbum;
                case "Artist":
                    return EServiceMediaType.eServiceMediaTypeArtist;
                default:
                    return EServiceMediaType.eServiceMediaTypeInvalid;
            }
        }
    }
}
