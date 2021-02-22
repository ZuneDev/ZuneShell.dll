// Decompiled with JetBrains decompiler
// Type: ZuneUI.JumpListPin
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class JumpListPin
    {
        private const string _formatString = "{0}~{1}~{2}~{3}~{4}";
        private const int _numberOfFormatStringParameters = 5;
        private string _name;
        private bool _isQuickMix;
        private bool _isMarketplace;
        private MediaType _type;
        private string _id;
        private int _userId;

        public JumpListPin()
        {
        }

        public JumpListPin(JumpListPin source)
        {
            this._name = source._name;
            this._isQuickMix = source._isQuickMix;
            this._isMarketplace = source._isMarketplace;
            this._type = source._type;
            this._id = source._id;
            this._userId = source._userId;
        }

        public string Name
        {
            get => this._name;
            set => this._name = value;
        }

        public bool IsQuickMix
        {
            get => this._isQuickMix;
            set => this._isQuickMix = value;
        }

        public bool IsMarketplace
        {
            get => this._isMarketplace;
            set => this._isMarketplace = value;
        }

        public MediaType Type
        {
            get => this._type;
            set => this._type = value;
        }

        public string ID
        {
            get => this._id;
            set => this._id = value;
        }

        public int UserID
        {
            get => this._userId;
            set => this._userId = value;
        }

        public override string ToString() => string.Format("{0}~{1}~{2}~{3}~{4}", (object)this.IsQuickMix, (object)this.IsMarketplace, (object)(int)this.Type, (object)this.UserID, (object)this.ID);

        public static JumpListPin Parse(string pinString)
        {
            JumpListPin jumpListPin = (JumpListPin)null;
            string[] strArray = pinString.Split('~');
            bool result1;
            bool result2;
            int result3;
            int result4;
            if (strArray.Length == 5 && bool.TryParse(strArray[0], out result1) && (bool.TryParse(strArray[1], out result2) && int.TryParse(strArray[2], out result3)) && int.TryParse(strArray[3], out result4))
            {
                jumpListPin = new JumpListPin();
                jumpListPin.IsQuickMix = result1;
                jumpListPin.IsMarketplace = result2;
                jumpListPin.Type = (MediaType)result3;
                jumpListPin.UserID = result4;
                jumpListPin.ID = strArray[4];
            }
            return jumpListPin;
        }
    }
}
