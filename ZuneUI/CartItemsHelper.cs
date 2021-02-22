// Decompiled with JetBrains decompiler
// Type: ZuneUI.CartItemsHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Service;
using System.Collections;
using ZuneXml;

namespace ZuneUI
{
    public class CartItemsHelper
    {
        public static EContentType ExtractContentType(object obj)
        {
            EContentType econtentType = EContentType.Unknown;
            if (obj is MessageRoot messageRoot)
                econtentType = messageRoot.ContentType;
            return econtentType;
        }

        public static void Sort(ListDataSet list, CartItemSortColumn column, bool sortAscending)
        {
            if (list == null || list.Count == 0)
                return;
            IComparer comparer = (IComparer)null;
            switch (column)
            {
                case CartItemSortColumn.AvailableInMarketplace:
                    comparer = (IComparer)new CartItemsAvailableInMarketplaceComparer(sortAscending);
                    break;
                case CartItemSortColumn.SortTitle:
                    comparer = (IComparer)new CartItemsComparer(sortAscending, new GetCartItemPropertyDelegate(CartItemsComparer.GetCartItemSortTitle));
                    break;
                case CartItemSortColumn.ArtistName:
                    comparer = (IComparer)new CartItemsComparer(sortAscending, new GetCartItemPropertyDelegate(CartItemsComparer.GetCartItemArtistName));
                    break;
                case CartItemSortColumn.DisplayType:
                    comparer = (IComparer)new CartItemsComparer(sortAscending, new GetCartItemPropertyDelegate(CartItemsComparer.GetCartItemDisplayType));
                    break;
            }
            if (comparer == null)
                return;
            ArrayList arrayList = new ArrayList(list.Count);
            arrayList.AddRange((ICollection)list);
            arrayList.Sort(comparer);
            list.Source = (IList)arrayList;
        }

        public static void ErrorMessageCartAlreadyFull() => ErrorDialogInfo.Show(HRESULT._NS_E_CART_FULL.Int, Shell.LoadString(StringId.IDS_CART_FULL));

        public static void ErrorMessageTooManyNewItems() => ErrorDialogInfo.Show(HRESULT._NS_E_CART_TOO_MANY_NEW_ITEMS.Int, Shell.LoadString(StringId.IDS_CART_FULL));

        public static void ErrorMessageMoreCartItemsAvailable(int extraCartItems)
        {
            StringId stringId = extraCartItems == 1 ? StringId.IDS_CART_FULL_MORE_DEVICE_ITEMS_SINGULAR : StringId.IDS_CART_FULL_MORE_DEVICE_ITEMS_PLURAL;
            ErrorDialogInfo.Show(HRESULT._NS_E_CART_MORE_ITEMS_AVAILABLE.Int, Shell.LoadString(StringId.IDS_CART_FULL), string.Format(Shell.LoadString(stringId), (object)extraCartItems));
        }
    }
}
