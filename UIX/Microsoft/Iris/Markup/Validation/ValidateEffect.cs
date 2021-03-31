// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateEffect
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup.UIX;
using Microsoft.Iris.UI;
using System.Collections.Generic;

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateEffect : ValidateClass
    {
        private ValidateProperty _foundTechniquesValidateProperty;
        private Dictionary<string, object> _foundDynamicElementAssignments;
        private Map<string, ValidateProperty> _foundInstancePropertyAssignments;
        private Map<string, int> _foundElementSymbolIndex;
        private static Map<string, int> s_methodNameMapping;

        public ValidateEffect(
          SourceMarkupLoader owner,
          ValidateTypeIdentifier typeIdentifier,
          int line,
          int offset)
          : base(owner, typeIdentifier, line, offset)
        {
        }

        public override void RearrangePropertiesForSymbols()
        {
            this.MovePropertyToFront("Techniques");
            base.RearrangePropertiesForSymbols();
        }

        protected override void FullValidation(ValidateContext context)
        {
            base.FullValidation(context);
            this._foundTechniquesValidateProperty = this.FindProperty("Techniques");
            if (this._foundTechniquesValidateProperty != null && this.FoundBaseType != null)
            {
                foreach (SymbolRecord symbolRecord in this.FoundBaseType.InheritableSymbolsTable)
                {
                    if (symbolRecord.SymbolOrigin == SymbolOrigin.Techniques)
                    {
                        this._foundTechniquesValidateProperty.ReportError("Overriding Techniques property is not allowed; base class has already defined techniques");
                        this.MarkHasErrors();
                        break;
                    }
                }
            }
            if (this._foundDynamicElementAssignments != null)
            {
                string[] dynamicElementAssignments = new string[this._foundDynamicElementAssignments.Count];
                int num = 0;
                foreach (string key in this._foundDynamicElementAssignments.Keys)
                    dynamicElementAssignments[num++] = key;
                ((EffectClassTypeSchema)this.TypeExport).SetDynamicElementAssignments(dynamicElementAssignments);
            }
            string str = "DefaultImage";
            SymbolOrigin origin;
            TypeSchema checkSchema = context.ResolveSymbol(str, out origin, out ExpressionRestriction _);
            if (checkSchema == null || !ImageElementInstanceSchema.Type.IsAssignableFrom(checkSchema) || origin != SymbolOrigin.Techniques)
                return;
            ((EffectClassTypeSchema)this.TypeExport).SetDefaultElementSymbolIndex(context.TrackSymbolUsage(str, origin));
        }

        public override void NotifyFoundPropertyAssignment(ValidateExpressionCall call)
        {
            string effectElementName = this.GetEffectElementName(call);
            if (effectElementName == null)
                return;
            this.TrackDynamicElementAssignment(effectElementName, call.MemberName);
        }

        public override void NotifyFoundMethodCall(ValidateExpressionCall call)
        {
            string effectElementName = this.GetEffectElementName(call);
            if (effectElementName == null)
                return;
            if (s_methodNameMapping == null)
            {
                s_methodNameMapping = new Map<string, int>();
                s_methodNameMapping["PlayAttenuationAnimation"] = 2;
                s_methodNameMapping["PlayBrightnessAnimation"] = 3;
                s_methodNameMapping["PlayColorAnimation"] = 4;
                s_methodNameMapping["PlayInnerConeAngleAnimation"] = 14;
                s_methodNameMapping["PlayOuterConeAngleAnimation"] = 18;
                s_methodNameMapping["PlayContrastAnimation"] = 5;
                s_methodNameMapping["PlayDarkColorAnimation"] = 6;
                s_methodNameMapping["PlayDecayAnimation"] = 7;
                s_methodNameMapping["PlayDensityAnimation"] = 8;
                s_methodNameMapping["PlayDesaturateAnimation"] = 9;
                s_methodNameMapping["PlayDirectionAngleAnimation"] = 10;
                s_methodNameMapping["PlayDownsampleAnimation"] = 25;
                s_methodNameMapping["PlayEdgeLimitAnimation"] = 11;
                s_methodNameMapping["PlayFallOffAnimation"] = 12;
                s_methodNameMapping["PlayHueAnimation"] = 13;
                s_methodNameMapping["PlayIntensityAnimation"] = 15;
                s_methodNameMapping["PlayLightColorAnimation"] = 16;
                s_methodNameMapping["PlayAmbientColorAnimation"] = 1;
                s_methodNameMapping["PlayLightnessAnimation"] = 17;
                s_methodNameMapping["PlayPositionAnimation"] = 19;
                s_methodNameMapping["PlayRadiusAnimation"] = 20;
                s_methodNameMapping["PlaySaturationAnimation"] = 21;
                s_methodNameMapping["PlayToneAnimation"] = 22;
                s_methodNameMapping["PlayWeightAnimation"] = 23;
                s_methodNameMapping["PlayValueAnimation"] = 24;
            }
            int num;
            if (!s_methodNameMapping.TryGetValue(call.MemberName, out num))
                return;
            this.TrackDynamicElementAssignment(effectElementName, (EffectProperty)num);
        }

        private string GetEffectElementName(ValidateExpressionCall call)
        {
            ValidateExpression target = call.Target;
            return EffectElementInstanceSchema.Type.IsAssignableFrom(target.ObjectType) && target.ExpressionType == ExpressionType.Symbol ? ((ValidateExpressionSymbol)target).Symbol : null;
        }

        private void TrackDynamicElementAssignment(string elementName, string propertyName) => this.TrackDynamicElementAssignment(EffectElementWrapper.MakeEffectPropertyName(elementName, propertyName));

        private void TrackDynamicElementAssignment(string elementName, EffectProperty property) => this.TrackDynamicElementAssignment(EffectElementWrapper.MakeEffectPropertyName(elementName, property));

        private void TrackDynamicElementAssignment(string dynamicPropertyName)
        {
            if (this._foundDynamicElementAssignments == null)
                this._foundDynamicElementAssignments = new Dictionary<string, object>();
            this._foundDynamicElementAssignments[dynamicPropertyName] = null;
        }

        public void TrackInstanceProperty(
          ValidateContext context,
          string name,
          ValidateProperty property)
        {
            if (this._foundInstancePropertyAssignments == null)
            {
                this._foundInstancePropertyAssignments = new Map<string, ValidateProperty>();
                this._foundElementSymbolIndex = new Map<string, int>();
            }
            ValidateProperty validateProperty;
            this._foundInstancePropertyAssignments.TryGetValue(name, out validateProperty);
            property.AppendToEnd(validateProperty);
            this._foundInstancePropertyAssignments[name] = property;
            SymbolOrigin origin;
            TypeSchema typeSchema = context.ResolveSymbol(name, out origin, out ExpressionRestriction _);
            if (typeSchema == null)
            {
                property.ReportError("Element '{0}' property '{1}' is being assigned a value which requires it to be set dynamically, but the property cannot be changed dynamically", property.FoundProperty.Owner.ToString(), property.FoundProperty.Name);
                this.MarkHasErrors();
            }
            else
            {
                this._foundElementSymbolIndex[name] = context.TrackSymbolUsage(name, origin);
                PropertySchema property1 = typeSchema.FindProperty(property.FoundProperty.Name);
                if (property1 == null)
                {
                    property.ReportError("Element '{0}' property '{1}' is being assigned a value which requires it to be set dynamically, but the property cannot be changed dynamically", property.FoundProperty.Owner.ToString(), property.FoundProperty.Name);
                    this.MarkHasErrors();
                }
                else
                {
                    property.UpdateFoundProperty(property1);
                    this.TrackDynamicElementAssignment(name, property.FoundProperty.Name);
                }
            }
        }

        public int GetElementSymbolIndex(string name) => this._foundElementSymbolIndex[name];

        public ValidateProperty FoundTechniquesValidateProperty => this._foundTechniquesValidateProperty;

        public Map<string, ValidateProperty> FoundInstancePropertyAssignments => this._foundInstancePropertyAssignments;
    }
}
