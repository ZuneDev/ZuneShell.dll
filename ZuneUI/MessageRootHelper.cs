// Decompiled with JetBrains decompiler
// Type: ZuneUI.MessageRootHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using ZuneXml;

namespace ZuneUI
{
    public class MessageRootHelper
    {
        public static bool IsUnread(DataProviderObject message) => string.Compare(((MessageRoot)message).Status, "unread") == 0;

        public static void SetRead(DataProviderObject message, bool read) => ((MessageRoot)message).Status = read ? nameof(read) : "unread";

        public static string UiType(DataProviderObject message) => MessageRootHelper.GetTypeInfo(message).UIText;

        public static string DetailsTemplate(DataProviderObject message) => MessageRootHelper.GetTypeInfo(message).DetailsTemplate;

        private static MessageTypeInfo GetTypeInfo(DataProviderObject message) => MessageTypeInfo.GetMessageType(((MessageRoot)message).Type);
    }
}
