// Decompiled with JetBrains decompiler
// Type: ZuneUI.CartPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneUI
{
    public class CartPage : LibraryPage
    {
        private Command _refreshPageCommand;
        private CartPanel _cartPanel;
        private bool _isEmptyCart;
        public static readonly string CartPageTemplate = "res://ZuneMarketplaceResources!Cart.uix#CartLibrary";

        public CartPage()
        {
            this.PivotPreference = Shell.MainFrame.Marketplace.Cart;
            this.IsRootPage = true;
            this.UI = CartPageTemplate;
            this._refreshPageCommand = new Command(this);
            this._cartPanel = new CartPanel(this);
        }

        public override IPageState SaveAndRelease()
        {
            if (this.IsEmptyCart)
                return null;
            if (this.CartPanel.SelectedItem != null)
            {
                if (this.NavigationArguments == null)
                    this.NavigationArguments = new Hashtable(1);
                this.NavigationArguments["MessageId"] = CartPanel.SelectedItem.MessagingId;
                this.CartPanel.SelectedItem = null;
            }
            else
                this.NavigationArguments = null;
            this._cartPanel.Release();
            return base.SaveAndRelease();
        }

        public CartPanel CartPanel => this._cartPanel;

        public bool IsEmptyCart
        {
            get => this._isEmptyCart;
            set
            {
                if (this._isEmptyCart == value)
                    return;
                this._isEmptyCart = value;
                this.FirePropertyChanged(nameof(IsEmptyCart));
            }
        }

        public Command RefreshPageCommand => this._refreshPageCommand;
    }
}
