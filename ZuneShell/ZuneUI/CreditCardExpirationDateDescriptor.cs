// Decompiled with JetBrains decompiler
// Type: ZuneUI.CreditCardExpirationDateDescriptor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public class CreditCardExpirationDateDescriptor : DatePropertyDescriptor
    {
        public CreditCardExpirationDateDescriptor(
          string name,
          string multiValueString,
          string unknownString,
          bool required)
          : base(name, multiValueString, unknownString, required)
        {
        }

        public override bool IsValidInternal(string value)
        {
            DateTime dateTime;
            if (StringParserHelper.IsNullOrEmptyOrBlank(value) || !StringParserHelper.TryParseDate(value, out dateTime))
                return false;
            DateTime now = DateTime.Now;
            if (dateTime.Year > now.Year)
                return true;
            return dateTime.Year == now.Year && dateTime.Month >= now.Month;
        }
    }
}
