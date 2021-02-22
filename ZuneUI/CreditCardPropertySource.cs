// Decompiled with JetBrains decompiler
// Type: ZuneUI.CreditCardPropertySource
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using System;

namespace ZuneUI
{
    public class CreditCardPropertySource : PropertySource
    {
        private static PropertySource _instance;

        protected CreditCardPropertySource()
        {
        }

        public static PropertySource Instance
        {
            get
            {
                if (CreditCardPropertySource._instance == null)
                    CreditCardPropertySource._instance = (PropertySource)new CreditCardPropertySource();
                return CreditCardPropertySource._instance;
            }
        }

        public override object Get(object media, PropertyDescriptor property)
        {
            CreditCard creditCard = media as CreditCard;
            string descriptorName = property.DescriptorName;
            if (creditCard == null)
                return (object)null;
            if (descriptorName == PropertyEditCreditCard.s_Street1.DescriptorName)
                return (object)creditCard.Address.Street1;
            if (descriptorName == PropertyEditCreditCard.s_Street2.DescriptorName)
                return (object)creditCard.Address.Street2;
            if (descriptorName == PropertyEditCreditCard.s_City.DescriptorName)
                return (object)creditCard.Address.City;
            if (descriptorName == PropertyEditCreditCard.s_District.DescriptorName)
                return (object)creditCard.Address.District;
            if (descriptorName == PropertyEditCreditCard.s_State.DescriptorName)
                return (object)creditCard.Address.State;
            if (descriptorName == PropertyEditCreditCard.s_PostalCode.DescriptorName)
                return (object)creditCard.Address.PostalCode;
            if (descriptorName == PropertyEditCreditCard.s_CardType.DescriptorName)
                return (object)creditCard.CreditCardType;
            if (descriptorName == PropertyEditCreditCard.s_AccountHolderName.DescriptorName)
                return (object)creditCard.AccountHolderName;
            if (descriptorName == PropertyEditCreditCard.s_AccountNumber.DescriptorName)
                return (object)creditCard.AccountNumber;
            if (descriptorName == PropertyEditCreditCard.s_CcvNumber.DescriptorName)
                return (object)creditCard.CCVNumber;
            if (descriptorName == PropertyEditCreditCard.s_ExpirationDate.DescriptorName)
                return (object)creditCard.ExpirationDate;
            if (descriptorName == PropertyEditCreditCard.s_PhoneExtension.DescriptorName)
                return (object)creditCard.PhoneExtension;
            return descriptorName == PropertyEditCreditCard.s_PhoneNumber.DescriptorName ? (object)PropertyEditCreditCard.s_PhoneNumber.Combine(creditCard.PhonePrefix, creditCard.PhoneNumber) : (object)null;
        }

        public override void Set(object media, PropertyDescriptor property, object value)
        {
            CreditCard creditCard = media as CreditCard;
            string descriptorName = property.DescriptorName;
            if (creditCard == null)
                return;
            if (descriptorName == PropertyEditCreditCard.s_Street1.DescriptorName)
                creditCard.Address.Street1 = (string)value;
            else if (descriptorName == PropertyEditCreditCard.s_Street2.DescriptorName)
                creditCard.Address.Street2 = (string)value;
            else if (descriptorName == PropertyEditCreditCard.s_City.DescriptorName)
                creditCard.Address.City = (string)value;
            else if (descriptorName == PropertyEditCreditCard.s_District.DescriptorName)
                creditCard.Address.District = (string)value;
            else if (descriptorName == PropertyEditCreditCard.s_State.DescriptorName)
                creditCard.Address.State = (string)value;
            else if (descriptorName == PropertyEditCreditCard.s_PostalCode.DescriptorName)
                creditCard.Address.PostalCode = (string)value;
            else if (descriptorName == PropertyEditCreditCard.s_CardType.DescriptorName)
                creditCard.CreditCardType = (CreditCardType)value;
            else if (descriptorName == PropertyEditCreditCard.s_AccountHolderName.DescriptorName)
                creditCard.AccountHolderName = (string)value;
            else if (descriptorName == PropertyEditCreditCard.s_AccountNumber.DescriptorName)
                creditCard.AccountNumber = (string)value;
            else if (descriptorName == PropertyEditCreditCard.s_CcvNumber.DescriptorName)
                creditCard.CCVNumber = (string)value;
            else if (descriptorName == PropertyEditCreditCard.s_ExpirationDate.DescriptorName)
                creditCard.ExpirationDate = (DateTime)value;
            else if (descriptorName == PropertyEditCreditCard.s_PhoneExtension.DescriptorName)
            {
                creditCard.PhoneExtension = (string)value;
            }
            else
            {
                if (!(descriptorName == PropertyEditCreditCard.s_PhoneNumber.DescriptorName))
                    return;
                string areaCode;
                string number;
                PropertyEditCreditCard.s_PhoneNumber.Split(value as string, out areaCode, out number);
                creditCard.PhonePrefix = areaCode;
                creditCard.PhoneNumber = number;
            }
        }
    }
}
