// Decompiled with JetBrains decompiler
// Type: ZuneUI.MessageListFilter
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using ZuneXml;

namespace ZuneUI
{
    public class MessageListFilter : FilterList
    {
        private bool _wishlistItems;

        public MessageListFilter(bool wishlistItems) => this._wishlistItems = wishlistItems;

        protected override bool ShouldIncludeItem(int sourceIndex, int targetIndex, object item)
        {
            bool flag = false;
            if (item is MessageRoot messageRoot && messageRoot.Wishlist == this._wishlistItems)
                flag = messageRoot.IsSupported;
            return flag;
        }
    }
}
