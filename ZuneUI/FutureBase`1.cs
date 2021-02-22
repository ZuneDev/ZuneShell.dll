// Decompiled with JetBrains decompiler
// Type: ZuneUI.FutureBase`1
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public abstract class FutureBase<T>
    {
        private T _value;
        private bool _calculationPerformed;

        public T Value
        {
            get
            {
                if (!this._calculationPerformed)
                {
                    this._value = this.CalculateValue();
                    this._calculationPerformed = true;
                }
                return this._value;
            }
        }

        protected abstract T CalculateValue();
    }
}
