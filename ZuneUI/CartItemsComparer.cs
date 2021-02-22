// Decompiled with JetBrains decompiler
// Type: ZuneUI.CartItemsComparer
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Collections;

namespace ZuneUI
{
    public class CartItemsComparer : IComparer
    {
        public readonly bool SortAscending;
        internal readonly GetCartItemPropertyDelegate PropertyDelegate;

        internal CartItemsComparer(bool sortAscending, GetCartItemPropertyDelegate propertyDelegate)
        {
            this.SortAscending = sortAscending;
            this.PropertyDelegate = propertyDelegate;
        }

        public static string GetCartItemSortTitle(CartItem item) => item.SortTitle;

        public static string GetCartItemArtistName(CartItem item) => item.ArtistName;

        public static string GetCartItemDisplayType(CartItem item) => item.DisplayType;

        public int Compare(object x, object y)
        {
            CartItem cartItem1 = x as CartItem;
            CartItem cartItem2 = y as CartItem;
            if (cartItem1 == null || cartItem2 == null)
                return 0;
            int num = ((IComparable)cartItem1.AvailableInMarketplace).CompareTo((object)cartItem2.AvailableInMarketplace);
            if (num != 0)
                return -num;
            if (this.PropertyDelegate != null)
                num = string.Compare(this.PropertyDelegate(cartItem1), this.PropertyDelegate(cartItem2));
            return !this.SortAscending ? -num : num;
        }
    }
}
