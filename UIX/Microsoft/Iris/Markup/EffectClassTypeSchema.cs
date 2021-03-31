// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.EffectClassTypeSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup.UIX;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using System;

namespace Microsoft.Iris.Markup
{
    internal class EffectClassTypeSchema : ClassTypeSchema
    {
        private uint[] _techniqueOffsets;
        private uint[] _instancePropertyAssignments;
        private string[] _dynamicElementAssignments;
        private int _defaultElementSymbolIndex = -1;
        private IEffectTemplate _effectTemplate;
        private int _templateIndexBuilt;
        private PropertySchema _defaultProperty;

        public EffectClassTypeSchema(MarkupLoadResult owner, string name)
          : base(owner, name)
        {
        }

        protected override void SealWorker()
        {
            base.SealWorker();
            this._defaultProperty = this.FindPropertyDeep("Default");
        }

        public override MarkupType MarkupType => MarkupType.Effect;

        protected override void OnDispose()
        {
            base.OnDispose();
            if (this._effectTemplate == null)
                return;
            this._effectTemplate.UnregisterUsage(this);
            this._effectTemplate = null;
        }

        protected override TypeSchema DefaultBase => EffectInstanceSchema.Type;

        public override Type RuntimeType => typeof(EffectClass);

        public string DefaultElementSymbol => this._defaultElementSymbolIndex >= 0 ? this.SymbolReferenceTable[this._defaultElementSymbolIndex].Symbol : null;

        protected override Class ConstructNewInstance()
        {
            this.EnsureEffectTemplate();
            return new EffectClass(this, this._effectTemplate);
        }

        protected override bool RunInitialEvaluates(IMarkupTypeBase scriptHost)
        {
            bool flag = true;
            if (this._instancePropertyAssignments != null && this._templateIndexBuilt >= 0)
            {
                ErrorManager.EnterContext(this);
                flag = this.RunInitializeScript(scriptHost, this._instancePropertyAssignments[this._templateIndexBuilt]);
                ErrorManager.ExitContext();
            }
            return flag && base.RunInitialEvaluates(scriptHost);
        }

        private void EnsureEffectTemplate()
        {
            if (this._effectTemplate != null)
                return;
            this._effectTemplate = UISession.Default.RenderSession.CreateEffectTemplate(this, this.Name);
            ErrorManager.EnterContext(this);
            Class @class = new Class(this);
            @class.DeclareOwner(this);
            this._templateIndexBuilt = -1;
            for (int index = 0; index < this._techniqueOffsets.Length; ++index)
            {
                object obj = this.RunAtOffset(@class, this._techniqueOffsets[index]);
                if (obj == Interpreter.ScriptError)
                {
                    if (!ErrorManager.IgnoringErrors)
                        ErrorManager.ReportWarning("Script runtime failure: Scripting errors have prevented '{0}' from properly initializing and will affect its operation", Name);
                }
                else
                {
                    EffectInput input = (EffectInput)obj;
                    if (this._dynamicElementAssignments != null)
                    {
                        foreach (string elementAssignment in this._dynamicElementAssignments)
                            this._effectTemplate.AddEffectProperty(elementAssignment);
                    }
                    if (this._effectTemplate.Build(input))
                    {
                        this._templateIndexBuilt = index;
                        break;
                    }
                }
            }
            @class.Dispose(this);
            ErrorManager.ExitContext();
        }

        public uint[] TechniqueOffsets => this._techniqueOffsets;

        public uint[] InstancePropertyAssignments => this._instancePropertyAssignments;

        public string[] DynamicElementAssignments => this._dynamicElementAssignments;

        public int DefaultElementSymbolIndex => this._defaultElementSymbolIndex;

        public void SetTechniqueOffsets(uint[] techniqueOffsets) => this._techniqueOffsets = techniqueOffsets;

        public void SetInstancePropertyAssignments(uint[] instancePropertyAssignments) => this._instancePropertyAssignments = instancePropertyAssignments;

        public void SetDynamicElementAssignments(string[] dynamicElementAssignments) => this._dynamicElementAssignments = dynamicElementAssignments;

        public void SetDefaultElementSymbolIndex(int symbolIndex) => this._defaultElementSymbolIndex = symbolIndex;
    }
}
