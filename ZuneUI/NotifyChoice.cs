// Decompiled with JetBrains decompiler
// Type: ZuneUI.NotifyChoice
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;

namespace ZuneUI
{
    public class NotifyChoice : Choice
    {
        private InvokePolicy _invokePolicy;

        public NotifyChoice()
          : this((IModelItemOwner)null)
        {
        }

        public NotifyChoice(IModelItemOwner owner)
          : this(owner, InvokePolicy.Synchronous)
        {
        }

        public NotifyChoice(IModelItemOwner owner, InvokePolicy invokePolicy)
          : base(owner)
          => this._invokePolicy = invokePolicy;

        protected override void OnChosenChanged()
        {
            if (this.ChosenValue == null)
                return;
            ((Command)this.ChosenValue).Invoke(this._invokePolicy);
        }

        protected override void ValidateOptionsListWorker(IList potentialOptions)
        {
            base.ValidateOptionsListWorker(potentialOptions);
            if (potentialOptions == null)
                return;
            foreach (object potentialOption in (IEnumerable)potentialOptions)
            {
                if (!(potentialOption is Command))
                    throw new ArgumentException("Contents must be of type Command");
            }
        }
    }
}
