// Decompiled with JetBrains decompiler
// Type: ZuneUI.CreditCardHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using System;
using System.Collections;

namespace ZuneUI
{
    public static class CreditCardHelper
    {
        private static string[] s_CardTypeStrs;
        private static string[] s_SupportedCardTypeStrs;
        private static string[] s_JpnSupportedCardTypeStrs;

        public static string CardTypeToString(CreditCardType cardType) => CreditCardHelper.CardTypeToString((int)cardType);

        public static string CardTypeToString(int cardType)
        {
            string[] creditCardMappings = CreditCardHelper.CreditCardMappings;
            return cardType >= 0 && cardType < creditCardMappings.Length ? creditCardMappings[cardType] : (string)null;
        }

        public static CreditCardType CardTypeFromString(string cardTypeStr)
        {
            string[] creditCardMappings = CreditCardHelper.CreditCardMappings;
            for (int index = 0; index < creditCardMappings.Length; ++index)
            {
                if (creditCardMappings[index] == cardTypeStr)
                    return (CreditCardType)index;
            }
            return CreditCardType.Unknown;
        }

        public static IList CardTypes
        {
            get
            {
                if (CreditCardHelper.s_SupportedCardTypeStrs == null || CreditCardHelper.s_JpnSupportedCardTypeStrs == null)
                {
                    string[] creditCardMappings = CreditCardHelper.CreditCardMappings;
                    CreditCardHelper.s_SupportedCardTypeStrs = new string[4]
                    {
            creditCardMappings[2],
            creditCardMappings[3],
            creditCardMappings[1],
            creditCardMappings[0]
                    };
                    CreditCardHelper.s_JpnSupportedCardTypeStrs = new string[4]
                    {
            creditCardMappings[2],
            creditCardMappings[1],
            creditCardMappings[0],
            creditCardMappings[4]
                    };
                    Array.Sort<string>(CreditCardHelper.s_SupportedCardTypeStrs);
                    Array.Sort<string>(CreditCardHelper.s_JpnSupportedCardTypeStrs);
                }
                return SignIn.Instance.SignedIn && string.Compare(SignIn.Instance.CountryCode, "JP", StringComparison.OrdinalIgnoreCase) == 0 ? (IList)CreditCardHelper.s_JpnSupportedCardTypeStrs : (IList)CreditCardHelper.s_SupportedCardTypeStrs;
            }
        }

        private static string[] CreditCardMappings
        {
            get
            {
                if (CreditCardHelper.s_CardTypeStrs == null)
                    CreditCardHelper.s_CardTypeStrs = new string[7]
                    {
            Shell.LoadString(StringId.IDS_BILLING_CC_VISA),
            Shell.LoadString(StringId.IDS_BILLING_CC_MASTERCARD),
            Shell.LoadString(StringId.IDS_BILLING_CC_AMEX),
            Shell.LoadString(StringId.IDS_BILLING_CC_DISCOVER),
            Shell.LoadString(StringId.IDS_BILLING_CC_JCB),
            Shell.LoadString(StringId.IDS_BILLING_CC_DINERS),
            Shell.LoadString(StringId.IDS_BILLING_CC_KLCC)
                    };
                return CreditCardHelper.s_CardTypeStrs;
            }
        }
    }
}
