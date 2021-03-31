// Decompiled with JetBrains decompiler
// Type: UIXControls.DialogHelper
// Assembly: UIXControls, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: 78800EA5-2757-404C-BA30-C33FCFC2852A
// Assembly location: C:\Program Files\Zune\UIXControls.dll

using Microsoft.Iris;

namespace UIXControls
{
    public class DialogHelper : ModelItem
    {
        private static string s_dialogCancel;
        private static string s_dialogYes;
        private static string s_dialogNo;
        private static string s_dialogOk;
        private string _contentUI;
        private Command _cancel;
        private bool _wouldLikeToBeHidden;

        public static string DialogCancel
        {
            get => s_dialogCancel;
            set => s_dialogCancel = value;
        }

        public static string DialogYes
        {
            get => s_dialogYes;
            set => s_dialogYes = value;
        }

        public static string DialogNo
        {
            get => s_dialogNo;
            set => s_dialogNo = value;
        }

        public static string DialogOk
        {
            get => s_dialogOk;
            set => s_dialogOk = value;
        }

        public DialogHelper()
          : this(null)
        {
        }

        public DialogHelper(string contentUI)
        {
            this._contentUI = contentUI;
            this._cancel = new Command();
            this._cancel.Description = DialogCancel;
        }

        public string ContentUI => this._contentUI;

        public Command Cancel => this._cancel;

        public bool WouldLikeToBeHidden
        {
            get => this._wouldLikeToBeHidden;
            private set
            {
                if (this._wouldLikeToBeHidden == value)
                    return;
                this._wouldLikeToBeHidden = value;
                this.FirePropertyChanged(nameof(WouldLikeToBeHidden));
            }
        }

        public void Hide() => this.WouldLikeToBeHidden = true;

        public void Show() => CodeDialogManager.Instance.ShowCodeDialog(this);
    }
}
