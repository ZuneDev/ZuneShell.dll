// Decompiled with JetBrains decompiler
// Type: ZuneUI.DelegateFuture`1
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class DelegateFuture<T> : FutureBase<T>
    {
        private CalculateValue<T> _delegate;

        public DelegateFuture(CalculateValue<T> calculateMethod) => this._delegate = calculateMethod;

        protected override T CalculateValue() => this._delegate();
    }
}
