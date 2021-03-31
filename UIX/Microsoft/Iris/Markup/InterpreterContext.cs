﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.InterpreterContext
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Session;
using System.Collections;

namespace Microsoft.Iris.Markup
{
    internal class InterpreterContext : IErrorContextSource
    {
        private MarkupTypeSchema _type;
        private IMarkupTypeBase _instance;
        private MarkupLoadResult _loadResult;
        private uint _initialBytecodeOffset;
        private ParameterContext _parameterContext;
        private Map<object, object> _scopedLocals;
        private static Stack s_cache = new Stack();

        private InterpreterContext()
        {
        }

        string IErrorContextSource.GetErrorContextDescription() => this._type.Owner.ErrorContextUri;

        void IErrorContextSource.GetErrorPosition(ref int line, ref int column)
        {
            uint currentOffset = this._loadResult.ObjectSection.CurrentOffset;
            if (currentOffset > 0U)
                --currentOffset;
            this._loadResult.LineNumberTable.Lookup(currentOffset, out line, out column);
        }

        public IMarkupTypeBase Instance => this._instance;

        public MarkupTypeSchema MarkupType => this._type;

        public uint InitialBytecodeOffset => this._initialBytecodeOffset;

        public MarkupLoadResult LoadResult => this._loadResult;

        public object ReadSymbol(SymbolReference symbolRef)
        {
            object obj = (object)null;
            switch (symbolRef.Origin)
            {
                case SymbolOrigin.ScopedLocal:
                    this._scopedLocals.TryGetValue((object)symbolRef.Symbol, out obj);
                    break;
                case SymbolOrigin.Parameter:
                    obj = this._parameterContext.ReadParameter(symbolRef.Symbol);
                    break;
                default:
                    obj = this._instance.ReadSymbol(symbolRef);
                    break;
            }
            return obj;
        }

        public void WriteSymbol(SymbolReference symbolRef, object value)
        {
            switch (symbolRef.Origin)
            {
                case SymbolOrigin.ScopedLocal:
                    if (this._scopedLocals == null)
                        this._scopedLocals = new Map<object, object>();
                    this._scopedLocals[(object)symbolRef.Symbol] = value;
                    break;
                case SymbolOrigin.Parameter:
                    this._parameterContext.WriteParameter(symbolRef.Symbol, value);
                    break;
                default:
                    this._instance.WriteSymbol(symbolRef, value);
                    break;
            }
        }

        public void ClearSymbol(SymbolReference symbolRef)
        {
            if (symbolRef.Origin != SymbolOrigin.ScopedLocal)
                return;
            this._scopedLocals.Remove((object)symbolRef.Symbol);
        }

        public static InterpreterContext Acquire(
          IMarkupTypeBase instance,
          MarkupTypeSchema type,
          uint initialBytecodeOffset,
          ParameterContext parameterContext)
        {
            InterpreterContext interpreterContext = (InterpreterContext)null;
            if (InterpreterContext.s_cache.Count != 0)
                interpreterContext = (InterpreterContext)InterpreterContext.s_cache.Pop();
            if (interpreterContext == null)
                interpreterContext = new InterpreterContext();
            interpreterContext._instance = instance;
            interpreterContext._initialBytecodeOffset = initialBytecodeOffset;
            interpreterContext._type = type;
            interpreterContext._loadResult = (MarkupLoadResult)type.Owner;
            interpreterContext._parameterContext = parameterContext;
            return interpreterContext;
        }

        public static void Release(InterpreterContext context)
        {
            context._instance = (IMarkupTypeBase)null;
            context._type = (MarkupTypeSchema)null;
            context._loadResult = (MarkupLoadResult)null;
            context._initialBytecodeOffset = 0U;
            context._parameterContext = new ParameterContext((string[])null, (object[])null);
            if (context._scopedLocals != null)
                context._scopedLocals.Clear();
            InterpreterContext.s_cache.Push((object)context);
        }

        public override string ToString()
        {
            int line = 0;
            int column = 0;
            ((IErrorContextSource)this).GetErrorPosition(ref line, ref column);
            return string.Format("{0} ({1}, {2})", (object)((IErrorContextSource)this).GetErrorContextDescription(), (object)line, (object)column);
        }
    }
}