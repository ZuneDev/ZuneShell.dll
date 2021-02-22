// Decompiled with JetBrains decompiler
// Type: ZuneUI.RecipientHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ZuneUI
{
    public class RecipientHelper : AutoCompleteHelper
    {
        private List<string> _validRecipients;
        private string _errorMessage;

        public RecipientHelper() => this._validRecipients = new List<string>();

        public void ClearState()
        {
            this._validRecipients = new List<string>();
            this.Entry = string.Empty;
            this.ErrorMessage = (string)null;
        }

        public static bool ValidZuneTag(string tag) => ZuneTagHelper.IsValid(tag);

        public static bool ValidEmail(string email) => EmailHelper.IsValid(email);

        public bool ValidateAll(bool allowEmailRecipients) => this.Validate(this.Entry, allowEmailRecipients);

        public string ErrorMessage
        {
            get => this._errorMessage;
            private set
            {
                if (!(this._errorMessage != value))
                    return;
                this._errorMessage = value;
                this.FirePropertyChanged(nameof(ErrorMessage));
            }
        }

        public IList ValidRecipients => (IList)this._validRecipients;

        private bool Validate(string entry, bool allowEmailRecipients)
        {
            bool flag = true;
            StringBuilder stringBuilder = new StringBuilder();
            this._validRecipients.Clear();
            if (entry != null)
            {
                string[] strArray = entry.Split(AutoCompleteHelper.s_entrySeparators);
                for (int index = 0; index < strArray.Length; ++index)
                {
                    string str = strArray[index].Trim();
                    if (str.Length != 0)
                    {
                        if (!RecipientHelper.ValidZuneTag(str) && (!allowEmailRecipients || !RecipientHelper.ValidEmail(str)))
                        {
                            flag = false;
                            if (index != 0)
                                stringBuilder.Append(' ');
                            stringBuilder.Append(str);
                        }
                        else
                            this._validRecipients.Add(str);
                    }
                }
            }
            this.ErrorMessage = flag ? (string)null : string.Format(Shell.LoadString(allowEmailRecipients ? StringId.IDS_COMPOSE_MESSAGE_ERROR_FRIENDS : StringId.IDS_COMPOSE_MESSAGE_ERROR_FRIENDS_NO_EMAIL), (object)stringBuilder.ToString());
            return flag;
        }

        public static IList MakeStringList() => (IList)new List<string>();
    }
}
