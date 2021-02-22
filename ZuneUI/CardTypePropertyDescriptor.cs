// Decompiled with JetBrains decompiler
// Type: ZuneUI.CardTypePropertyDescriptor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;

namespace ZuneUI
{
    public class CardTypePropertyDescriptor : PropertyDescriptor
    {
        public CardTypePropertyDescriptor(
          string name,
          string multiValueString,
          string unknownString,
          bool required)
          : base(name, multiValueString, unknownString, required)
        {
        }

        public override string ConvertToString(object value) => value == null ? CreditCardHelper.CardTypeToString(CreditCardType.Unknown) : CreditCardHelper.CardTypeToString((CreditCardType)value);

        public override object ConvertFromString(string value) => CreditCardHelper.CardTypeFromString(value);

        public override bool IsValidInternal(string value) => !StringParserHelper.IsNullOrEmptyOrBlank(value);
    }
}
