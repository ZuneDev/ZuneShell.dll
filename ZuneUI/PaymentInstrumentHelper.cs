// Decompiled with JetBrains decompiler
// Type: ZuneUI.PaymentInstrumentHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using System;
using System.Collections;

namespace ZuneUI
{
    public class PaymentInstrumentHelper : ModelItem
    {
        private CreditCardCollection m_creditCards;
        private CreditCard m_newCreditCard;
        private PaymentInstrument m_defaultPaymentInstrument;
        private HRESULT m_errorCode;

        public void GetPaymentInstruments()
        {
            this.m_defaultPaymentInstrument = (PaymentInstrument)null;
            this.m_creditCards = (CreditCardCollection)null;
            this.ErrorCode = HRESULT._S_OK;
            ZuneApplication.Service.GetPaymentInstruments(new GetPaymentInstrumentsCompleteCallback(this.OnGetPaymentInstrumentsSuccess), new GetPaymentInstrumentsErrorCallback(this.OnGetPaymentInstrumnetsError));
        }

        public void AddPaymentInstrument(PaymentInstrument paymentInstrument)
        {
            this.ErrorCode = HRESULT._S_OK;
            ZuneApplication.Service.AddPaymentInstrument(paymentInstrument, new AddPaymentInstrumentCompleteCallback(this.OnAddPaymentInstrumentSuccess), new AddPaymentInstrumentErrorCallback(this.OnAddPaymentInstrumnetError));
        }

        public IList CreditCards => this.m_creditCards == null ? (IList)null : this.m_creditCards.Items;

        public PaymentInstrument Default
        {
            get => this.m_defaultPaymentInstrument;
            private set
            {
                if (this.m_defaultPaymentInstrument == value)
                    return;
                this.m_defaultPaymentInstrument = value;
                this.FirePropertyChanged(nameof(Default));
            }
        }

        public CreditCard NewCreditCard
        {
            get => this.m_newCreditCard;
            private set
            {
                if (this.m_newCreditCard == value)
                    return;
                this.m_newCreditCard = value;
                this.FirePropertyChanged(nameof(NewCreditCard));
            }
        }

        public HRESULT ErrorCode
        {
            get => this.m_errorCode;
            set
            {
                if (!(this.m_errorCode != value))
                    return;
                this.m_errorCode = value;
                this.FirePropertyChanged(nameof(ErrorCode));
            }
        }

        public event EventHandler GetPaymentInstrumentsCompleted;

        private void SetError(HRESULT hrError) => this.ErrorCode = hrError;

        private void SetCreditCards(CreditCardCollection creditCards)
        {
            if (this.IsDisposed)
            {
                creditCards?.Dispose();
            }
            else
            {
                if (this.m_creditCards != null)
                    this.m_creditCards.Dispose();
                this.m_creditCards = creditCards;
                this.FirePropertyChanged("CreditCards");
                this.CalculateDefault();
            }
        }

        private void CalculateDefault()
        {
            CreditCard creditCard1 = (CreditCard)null;
            if (this.NewCreditCard != null)
                creditCard1 = this.NewCreditCard;
            else if (this.CreditCards != null && this.CreditCards.Count > 0)
            {
                DateTime now = DateTime.Now;
                foreach (CreditCard creditCard2 in (IEnumerable)this.CreditCards)
                {
                    if (creditCard2.ExpirationDate.Year > now.Year || creditCard2.ExpirationDate.Year == now.Year && creditCard2.ExpirationDate.Month >= now.Month)
                        creditCard1 = creditCard2;
                }
            }
            this.Default = (PaymentInstrument)creditCard1;
        }

        private void OnGetPaymentInstrumentsSuccess(CreditCardCollection creditCards) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredGetPaymentInstrumentsSuccess), (object)creditCards);

        private void OnGetPaymentInstrumnetsError(HRESULT hrError) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredGetPaymentInstrumentsError), (object)hrError);

        private void OnAddPaymentInstrumentSuccess(PaymentInstrument paymentInstrument) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredAddPaymentInstrumentSuccess), (object)paymentInstrument);

        private void OnAddPaymentInstrumnetError(HRESULT hrError) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredAddPaymentInstrumentError), (object)hrError);

        private void DeferredGetPaymentInstrumentsSuccess(object args)
        {
            this.SetCreditCards((CreditCardCollection)args);
            this.OnGetPaymentInstrumentsCompleted();
        }

        private void DeferredGetPaymentInstrumentsError(object args)
        {
            this.SetError((HRESULT)args);
            this.OnGetPaymentInstrumentsCompleted();
        }

        private void DeferredAddPaymentInstrumentSuccess(object args)
        {
            this.NewCreditCard = (CreditCard)args;
            this.CalculateDefault();
        }

        private void DeferredAddPaymentInstrumentError(object args) => this.SetError((HRESULT)args);

        protected override void OnDispose(bool disposing)
        {
            base.OnDispose(disposing);
            if (!disposing || this.m_creditCards == null)
                return;
            this.m_creditCards.Dispose();
            this.m_creditCards = (CreditCardCollection)null;
        }

        private void OnGetPaymentInstrumentsCompleted()
        {
            if (this.GetPaymentInstrumentsCompleted != null)
                this.GetPaymentInstrumentsCompleted((object)this, (EventArgs)null);
            this.FirePropertyChanged("GetPaymentInstrumentsCompleted");
        }
    }
}
