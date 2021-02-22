// Decompiled with JetBrains decompiler
// Type: Microsoft.Zune.Shell.ZuneShellException
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Runtime.Serialization;

namespace Microsoft.Zune.Shell
{
    [Serializable]
    internal class ZuneShellException : InvalidOperationException
    {
        private string _context;

        public ZuneShellException(string message)
          : this(message, null)
        {
        }

        public ZuneShellException(string message, string context)
          : base(PrepareMessage(message, context))
          => this._context = context;

        protected ZuneShellException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }

        public string Context => this._context;

        private static string PrepareMessage(string message, string context) => message;
    }
}
