// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.UI.Class
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.Session;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Iris.UI
{
    internal class Class :
      DisposableObject,
      INotifyObject,
      IMarkupTypeBase,
      IDisposableOwner,
      ISchemaInfo
    {
        public const string ClassStateReservedSymbolName = "Class";
        public const string ThisReservedSymbolName = "this";
        private MarkupTypeSchema _typeSchema;
        protected Dictionary<object, object> _storage;
        private Vector<IDisposableObject> _disposables;
        private MarkupListeners _listeners;
        protected NotifyService _notifier = new NotifyService();
        private ScriptRunScheduler _scriptRunScheduler = new ScriptRunScheduler();
        private bool _scriptEnabled;
        private static DeferredHandler s_executePendingScriptsHandler = new DeferredHandler(ExecutePendingScripts);

        public Class(MarkupTypeSchema type)
        {
            this._typeSchema = type;
            this._storage = new Dictionary<object, object>(type.TotalPropertiesAndLocalsCount);
            this._scriptEnabled = true;
        }

        protected override void OnDispose()
        {
            this._typeSchema.RunFinalEvaluates(this);
            base.OnDispose();
            this._scriptEnabled = false;
            if (this._listeners != null)
            {
                this._listeners.Dispose(this);
                this._listeners = null;
            }
            this._notifier.ClearListeners();
            this._storage.Clear();
            if (this._disposables == null)
                return;
            for (int index = 0; index < this._disposables.Count; ++index)
                this._disposables[index].Dispose(this);
        }

        public void RegisterDisposable(IDisposableObject disposable)
        {
            if (this._disposables == null)
                this._disposables = new Vector<IDisposableObject>();
            this._disposables.Add(disposable);
        }

        public bool UnregisterDisposable(ref IDisposableObject disposable)
        {
            if (this._disposables != null)
            {
                int index = this._disposables.IndexOf(disposable);
                if (index != -1)
                {
                    disposable = this._disposables[index];
                    this._disposables.RemoveAt(index);
                    return true;
                }
            }
            return false;
        }

        public TypeSchema TypeSchema => _typeSchema;

        public virtual void NotifyInitialized()
        {
        }

        void INotifyObject.AddListener(Listener listener) => this._notifier.AddListener(listener);

        public virtual object ReadSymbol(SymbolReference symbolRef)
        {
            object obj = null;
            switch (symbolRef.Origin)
            {
                case SymbolOrigin.Properties:
                case SymbolOrigin.Locals:
                    obj = this._storage[symbolRef.Symbol];
                    break;
                case SymbolOrigin.Reserved:
                    if (symbolRef.Symbol == nameof(Class) || symbolRef.Symbol == "this")
                    {
                        obj = this;
                        break;
                    }
                    break;
            }
            return obj;
        }

        public virtual void WriteSymbol(SymbolReference symbolRef, object value) => this.SetProperty(symbolRef.Symbol, value);

        public virtual object GetProperty(string name) => this._storage[name];

        public virtual void SetProperty(string name, object value)
        {
            if (this._storage.ContainsKey(name) && Utility.IsEqual(this._storage[name], value))
                return;
            this._storage[name] = value;
            this._notifier.Fire(name);
        }

        public MarkupListeners Listeners
        {
            get => this._listeners;
            set => this._listeners = value;
        }

        public Dictionary<object, object> Storage => this._storage;

        public void ScheduleScriptRun(uint scriptId, bool ignoreErrors)
        {
            if (!this._scriptRunScheduler.Pending)
                DeferredCall.Post(DispatchPriority.Script, s_executePendingScriptsHandler, this);
            this._scriptRunScheduler.ScheduleRun(scriptId, ignoreErrors);
        }

        private static void ExecutePendingScripts(object args)
        {
            Class @class = (Class)args;
            @class._scriptRunScheduler.Execute(@class);
        }

        public object RunScript(uint scriptId, bool ignoreErrors, ParameterContext parameterContext) => this._typeSchema.Run(this, scriptId, ignoreErrors, parameterContext);

        public void NotifyScriptErrors()
        {
            this._scriptEnabled = false;
            ErrorManager.ReportWarning("Script runtime failure: Scripting has been disabled for '{0}' due to runtime scripting errors", _typeSchema.Name);
        }

        public bool ScriptEnabled => this._scriptEnabled;

        public override string ToString() => this._typeSchema.ToString();

        [Conditional("DEBUG")]
        public void DEBUG_MarkInitialized()
        {
        }
    }
}
