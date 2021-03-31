// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateContext
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Markup.UIX;
using System.Collections.Generic;

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateContext
    {
        private ValidateClass _owner;
        private MarkupTypeSchema _baseType;
        private LoadPass _currentPass;
        private SymbolOrigin _currentScope;
        private ValidateMethod _currentMethod;
        private SymbolResolutionDirective _resolutionDirective;
        private Vector<SymbolRecord> _symbolTable = new Vector<SymbolRecord>();
        private Vector<SymbolReference> _usedSymbols = new Vector<SymbolReference>();
        private Vector<Vector<SymbolRecord>> _scopedLocalFrameStack = new Vector<Vector<SymbolRecord>>();
        private Stack<int> _loopFramesStack = new Stack<int>();
        private Stack<ValidateStatementLoop> _loopStatementStack = new Stack<ValidateStatementLoop>();
        private Vector<SymbolRecord> _methodParameterRecords;
        private Stack<ValidateContext.PropertyScopeRecord> _propertyScopeStack = new Stack<ValidateContext.PropertyScopeRecord>();
        private Vector<ValidateExpression> _declaredTriggerTrackingList;
        private ValidateExpression _notifierTrackingRoot;
        private uint _notifierCount;
        private Vector<TriggerRecord> _triggerList = new Vector<TriggerRecord>();
        private Vector<ValidateCode> _actionList = new Vector<ValidateCode>();
        private Map<string, TypeSchema> _reservedSymbols;
        public static PropertySchema ClassPropertiesProperty;
        public static PropertySchema UIPropertiesProperty;
        private static PropertySchema ClassLocalsProperty;
        private static PropertySchema UILocalsProperty;
        private static PropertySchema UIInputProperty;
        private static PropertySchema UIContentProperty;
        private static PropertySchema EffectTechniquesProperty;
        private static string[] ReservedNameList = new string[4]
        {
      "Name",
      "RepeatedItem",
      "RepeatedItemIndex",
      "this"
        };

        public ValidateContext(ValidateClass owner, MarkupTypeSchema baseType, LoadPass currentPass)
        {
            this._owner = owner;
            this._currentScope = SymbolOrigin.None;
            this._baseType = baseType;
            this._currentPass = currentPass;
        }

        public static void InitializeStatics()
        {
            ValidateContext.ClassPropertiesProperty = ClassSchema.Type.FindProperty("Properties");
            ValidateContext.UIPropertiesProperty = UISchema.Type.FindProperty("Properties");
            ValidateContext.ClassLocalsProperty = ClassSchema.Type.FindProperty("Locals");
            ValidateContext.UILocalsProperty = UISchema.Type.FindProperty("Locals");
            ValidateContext.UIInputProperty = UISchema.Type.FindProperty("Input");
            ValidateContext.UIContentProperty = UISchema.Type.FindProperty("Content");
            ValidateContext.EffectTechniquesProperty = EffectSchema.Type.FindProperty("Techniques");
        }

        public TypeSchema ResolveSymbol(
          string name,
          out SymbolOrigin origin,
          out ExpressionRestriction expressionRestriction)
        {
            return this.ResolveSymbol(name, true, this._resolutionDirective, out origin, out expressionRestriction, out TypeSchema _);
        }

        public TypeSchema ResolveSymbolNoBaseTypes(string name, out SymbolOrigin origin) => this.ResolveSymbol(name, false, this._resolutionDirective, out origin, out ExpressionRestriction _, out TypeSchema _);

        private TypeSchema IsSymbolAlreadyDefined(
          string name,
          out SymbolOrigin origin,
          out ExpressionRestriction expressionRestriction,
          out TypeSchema baseTypeOrigin)
        {
            return this.ResolveSymbol(name, true, SymbolResolutionDirective.None, out origin, out expressionRestriction, out baseTypeOrigin);
        }

        private TypeSchema ResolveSymbol(
          string name,
          bool searchBaseTypes,
          SymbolResolutionDirective resolutionDirective,
          out SymbolOrigin origin,
          out ExpressionRestriction expressionRestriction,
          out TypeSchema baseTypeOrigin)
        {
            baseTypeOrigin = (TypeSchema)null;
            TypeSchema type = this.ResolveSymbol(this._symbolTable, name, out origin, out expressionRestriction);
            if (type == null && searchBaseTypes)
            {
                if (resolutionDirective != SymbolResolutionDirective.PropertyResolution)
                {
                    for (MarkupTypeSchema markupTypeSchema = this._baseType; markupTypeSchema != null; markupTypeSchema = markupTypeSchema.MarkupTypeBase)
                    {
                        if (markupTypeSchema.InheritableSymbolsTable != null)
                            type = this.ResolveSymbol(markupTypeSchema.InheritableSymbolsTable, name, out origin, out expressionRestriction);
                        if (type != null)
                        {
                            baseTypeOrigin = (TypeSchema)markupTypeSchema;
                            break;
                        }
                    }
                }
                else if (this._baseType != null && this._baseType.FindPropertyDeep(name) is MarkupPropertySchema propertyDeep && propertyDeep.RequiredForCreation)
                {
                    type = propertyDeep.PropertyType;
                    origin = SymbolOrigin.Properties;
                    expressionRestriction = propertyDeep.ExpressionRestriction;
                    baseTypeOrigin = propertyDeep.Owner;
                }
            }
            if (type == null && name == "this" && this._owner.ObjectType == ClassSchema.Type)
            {
                type = (TypeSchema)this._owner.TypeExport;
                origin = SymbolOrigin.Reserved;
                expressionRestriction = ExpressionRestriction.ReadOnly;
            }
            if (type != null)
                this._owner.Owner.TrackImportedType(type);
            return type;
        }

        private TypeSchema ResolveSymbol(
          Vector<SymbolRecord> symbolTable,
          string name,
          out SymbolOrigin origin,
          out ExpressionRestriction expressionRestriction)
        {
            foreach (SymbolRecord symbolRecord in symbolTable)
            {
                if (symbolRecord.Name == name)
                {
                    origin = symbolRecord.SymbolOrigin;
                    expressionRestriction = ExpressionRestriction.None;
                    if (symbolRecord.SymbolOrigin == SymbolOrigin.Content || symbolRecord.SymbolOrigin == SymbolOrigin.Input)
                        expressionRestriction = ExpressionRestriction.ReadOnly;
                    return symbolRecord.Type;
                }
            }
            TypeSchema type;
            if (this._reservedSymbols != null && this._reservedSymbols.TryGetValue(name, out type))
            {
                origin = SymbolOrigin.Reserved;
                expressionRestriction = ExpressionRestriction.ReadOnly;
                this._owner.Owner.TrackImportedType(type);
                return type;
            }
            origin = SymbolOrigin.None;
            expressionRestriction = ExpressionRestriction.None;
            return (TypeSchema)null;
        }

        private TypeSchema ResolveSymbol(
          SymbolRecord[] symbolTable,
          string name,
          out SymbolOrigin origin,
          out ExpressionRestriction expressionRestriction)
        {
            foreach (SymbolRecord symbolRecord in symbolTable)
            {
                if (symbolRecord.Name == name)
                {
                    origin = symbolRecord.SymbolOrigin;
                    expressionRestriction = ExpressionRestriction.None;
                    if (symbolRecord.SymbolOrigin == SymbolOrigin.Content || symbolRecord.SymbolOrigin == SymbolOrigin.Input)
                        expressionRestriction = ExpressionRestriction.ReadOnly;
                    return symbolRecord.Type;
                }
            }
            TypeSchema type;
            if (this._reservedSymbols != null && this._reservedSymbols.TryGetValue(name, out type))
            {
                origin = SymbolOrigin.Reserved;
                expressionRestriction = ExpressionRestriction.ReadOnly;
                this._owner.Owner.TrackImportedType(type);
                return type;
            }
            origin = SymbolOrigin.None;
            expressionRestriction = ExpressionRestriction.None;
            return (TypeSchema)null;
        }

        public void DeclareReservedSymbols(Map<string, TypeSchema> reservedSymbols)
        {
            if (this._reservedSymbols != null)
                return;
            this._reservedSymbols = reservedSymbols;
        }

        public void NotifyObjectTagScopeEnter(ValidateObjectTag objectTag)
        {
            if (this._owner == null)
                return;
            this._owner.NotifyDiscoveredObjectTag(objectTag);
        }

        public Result NotifyObjectTagScopeExit(ValidateObjectTag objectTag)
        {
            TypeSchema typeSchema1 = objectTag.ObjectType;
            NameUsage activeNameUsage = this.GetActiveNameUsage();
            string name1 = objectTag.Name;
            Result result = Result.Success;
            if (name1 != null && typeSchema1 != ClassSchema.Type && (typeSchema1 != UISchema.Type && typeSchema1 != EffectSchema.Type) && (typeSchema1 != DataTypeSchema.Type && typeSchema1 != DataQuerySchema.Type && (activeNameUsage != NameUsage.DictionaryKeys && activeNameUsage != NameUsage.NamedContent)))
            {
                if (this._currentScope == SymbolOrigin.Techniques && EffectElementSchema.Type.IsAssignableFrom(typeSchema1))
                {
                    string name2 = typeSchema1.Name;
                    string name3 = name2 + "Instance";
                    typeSchema1 = MarkupSystem.UIXGlobal.FindType(name3);
                    if (typeSchema1 == null)
                        result = Result.Fail(string.Format("Element '{0}' has no properties that can be changed dynamically and therefore cannot be named", (object)name2));
                    else
                        this._owner.Owner.TrackImportedType(typeSchema1);
                }
                SymbolOrigin origin;
                TypeSchema baseTypeOrigin;
                TypeSchema typeSchema2 = this.IsSymbolAlreadyDefined(name1, out origin, out ExpressionRestriction _, out baseTypeOrigin);
                if (!result.Failed && typeSchema2 != null)
                {
                    if (baseTypeOrigin != null)
                    {
                        if (this._currentScope == SymbolOrigin.Properties && origin == SymbolOrigin.Properties)
                        {
                            if (!typeSchema2.IsAssignableFrom(typeSchema1))
                                result = Result.Fail(string.Format("Property '{0}' exists in base class '{1}' with type '{2}' and type override '{3}' does not match", (object)name1, (object)baseTypeOrigin.Name, (object)typeSchema2.Name, (object)typeSchema1.Name));
                            else if (objectTag.PropertyOverrideCriteria != null)
                            {
                                MarkupPropertySchema propertyDeep = (MarkupPropertySchema)this._baseType.FindPropertyDeep(objectTag.Name);
                                result = objectTag.PropertyOverrideCriteria.Verify(propertyDeep.OverrideCriteria);
                            }
                        }
                        else if (this._currentScope != SymbolOrigin.Content || origin != SymbolOrigin.Content)
                        {
                            if (this._currentScope == origin)
                                result = Result.Fail("Name \"{0}\" (also defined in base class '{1}') cannot be overridden (tried to override {2}, but can only override Properties and Content)", (object)name1, (object)baseTypeOrigin.Name, (object)origin.ToString());
                            else
                                result = Result.Fail(string.Format("Name \"{0}\" (located in {1}) cannot override the same name within base class '{2}' (located in {3}) since overrides cannot cross section types", (object)name1, (object)this._currentScope.ToString(), (object)baseTypeOrigin.Name, (object)origin.ToString()));
                        }
                    }
                    else if (origin != SymbolOrigin.Reserved)
                        result = Result.Fail("Name \"{0}\" is already in use (type '{1}') located in '{2}'", (object)name1, (object)typeSchema2.Name, (object)origin.ToString());
                }
                if (!result.Failed)
                {
                    if (this.IsNameReserved(name1))
                        result = Result.Fail("Name \"{0}\" is reserved and cannot be used.", (object)name1);
                    else if (!ValidateContext.IsValidSymbolName(name1))
                        result = Result.Fail("Invalid name \"{0}\".  Valid names must begin with either an alphabetic character or an underscore and can otherwise contain only alphabetic, numeric, or underscore characters", (object)name1);
                }
                if (!result.Failed && typeSchema1 != null)
                    this._symbolTable.Add(new SymbolRecord()
                    {
                        Name = name1,
                        Type = typeSchema1,
                        SymbolOrigin = this._currentScope
                    });
            }
            return result;
        }

        public void NotifyPropertyScopeEnter(ValidateProperty property)
        {
            PropertySchema foundProperty = property.FoundProperty;
            NameUsage nameUsage;
            if (foundProperty == ValidateContext.ClassPropertiesProperty || foundProperty == ValidateContext.UIPropertiesProperty)
            {
                this._currentScope = SymbolOrigin.Properties;
                nameUsage = NameUsage.Symbols;
                this._resolutionDirective = SymbolResolutionDirective.PropertyResolution;
            }
            else if (foundProperty == ValidateContext.ClassLocalsProperty || foundProperty == ValidateContext.UILocalsProperty)
            {
                this._currentScope = SymbolOrigin.Locals;
                nameUsage = NameUsage.Symbols;
            }
            else if (foundProperty == ValidateContext.UIInputProperty)
            {
                this._currentScope = SymbolOrigin.Input;
                nameUsage = NameUsage.Symbols;
            }
            else if (foundProperty == ValidateContext.UIContentProperty)
            {
                if (property.PropertyAttributeList == null)
                {
                    this._currentScope = SymbolOrigin.Content;
                    nameUsage = NameUsage.Symbols;
                }
                else
                    nameUsage = NameUsage.NamedContent;
            }
            else if (foundProperty == ValidateContext.EffectTechniquesProperty)
            {
                this._currentScope = SymbolOrigin.Techniques;
                nameUsage = NameUsage.Symbols;
            }
            else
                nameUsage = !DictionarySchema.Type.IsAssignableFrom(foundProperty.PropertyType) ? NameUsage.Default : NameUsage.DictionaryKeys;
            this._propertyScopeStack.Push(new ValidateContext.PropertyScopeRecord()
            {
                nameUsage = nameUsage,
                property = foundProperty
            });
        }

        public void NotifyPropertyScopeExit(ValidateProperty property)
        {
            PropertySchema foundProperty = property.FoundProperty;
            if (foundProperty == ValidateContext.ClassPropertiesProperty || foundProperty == ValidateContext.UIPropertiesProperty)
            {
                this._resolutionDirective = SymbolResolutionDirective.None;
                this._currentScope = SymbolOrigin.None;
            }
            else if (foundProperty == ValidateContext.ClassLocalsProperty || foundProperty == ValidateContext.UILocalsProperty)
                this._currentScope = SymbolOrigin.None;
            else if (foundProperty == ValidateContext.UIInputProperty)
                this._currentScope = SymbolOrigin.None;
            else if (foundProperty == ValidateContext.UIContentProperty && property.PropertyAttributeList == null)
                this._currentScope = SymbolOrigin.None;
            else if (foundProperty == ValidateContext.EffectTechniquesProperty)
                this._currentScope = SymbolOrigin.None;
            this._propertyScopeStack.Pop();
        }

        public void NotifyMethodScopeEnter(ValidateMethod method)
        {
            this._currentMethod = method;
            if (this._methodParameterRecords == null)
                this._methodParameterRecords = new Vector<SymbolRecord>();
            MarkupMethodSchema methodExport = method.MethodExport;
            for (int index = 0; index < methodExport.ParameterNames.Length; ++index)
            {
                string parameterName = methodExport.ParameterNames[index];
                Result result = this.IsNameInUse(parameterName, false);
                if (result.Failed)
                {
                    method.ReportError(result.ToString());
                }
                else
                {
                    SymbolRecord symbolRecord = new SymbolRecord();
                    symbolRecord.Name = parameterName;
                    symbolRecord.Type = methodExport.ParameterTypes[index];
                    symbolRecord.SymbolOrigin = SymbolOrigin.Parameter;
                    this._symbolTable.Add(symbolRecord);
                    this._methodParameterRecords.Add(symbolRecord);
                }
            }
        }

        public void NotifyMethodScopeExit(ValidateMethod method)
        {
            foreach (SymbolRecord methodParameterRecord in this._methodParameterRecords)
                this._symbolTable.Remove(methodParameterRecord);
            this._methodParameterRecords.Clear();
            this._currentMethod = (ValidateMethod)null;
        }

        public Result NotifyMethodFound(string name)
        {
            Result result = this.IsNameInUse(name, false);
            if (result.Failed)
                return result;
            this._symbolTable.Add(new SymbolRecord()
            {
                Name = name,
                Type = (TypeSchema)null,
                SymbolOrigin = SymbolOrigin.Methods
            });
            return Result.Success;
        }

        public bool IsNameReserved(string name)
        {
            foreach (string reservedName in ValidateContext.ReservedNameList)
            {
                if (name == reservedName)
                    return true;
            }
            return this._reservedSymbols != null && this._reservedSymbols.ContainsKey(name);
        }

        public static bool IsValidSymbolName(string name)
        {
            bool flag = true;
            for (int index = 0; index < name.Length; ++index)
            {
                char ch = name[index];
                if (ch != '_' && (ch < 'a' || ch > 'z') && (ch < 'A' || ch > 'Z') && (ch < '0' || ch > '9' || index == 0))
                {
                    flag = false;
                    break;
                }
            }
            if (name.Length == 0)
                flag = false;
            return flag;
        }

        public Result IsNameInUse(string name, bool bypassReservedNameCheck)
        {
            SymbolOrigin origin;
            TypeSchema typeSchema = this.ResolveSymbol(name, out origin, out ExpressionRestriction _);
            if (typeSchema != null)
                return Result.Fail("Name \"{0}\" is already in use (type '{1}') located in '{2}'", (object)name, (object)typeSchema.Name, (object)origin.ToString());
            if (!bypassReservedNameCheck)
            {
                if (this.IsNameReserved(name))
                    return Result.Fail("Name \"{0}\" is reserved and cannot be used.", (object)name);
                if (!ValidateContext.IsValidSymbolName(name))
                    return Result.Fail("Invalid name \"{0}\".  Valid names must begin with either an alphabetic character or an underscore and can otherwise contain only alphabetic, numeric, or underscore characters", (object)name);
            }
            return Result.Success;
        }

        public void NotifyScopedLocalFrameEnter(ValidateStatementLoop statementLoop)
        {
            this._scopedLocalFrameStack.Add((Vector<SymbolRecord>)null);
            if (statementLoop == null)
                return;
            this._loopFramesStack.Push(this._scopedLocalFrameStack.Count);
            this._loopStatementStack.Push(statementLoop);
        }

        public void NotifyScopedLocalFrameEnter() => this.NotifyScopedLocalFrameEnter((ValidateStatementLoop)null);

        public Result NotifyScopedLocal(string name, TypeSchema type) => this.NotifyScopedLocal(name, type, false, SymbolOrigin.ScopedLocal);

        public Result NotifyScopedLocal(
          string name,
          TypeSchema type,
          bool bypassReservedNameCheck,
          SymbolOrigin symbolOrigin)
        {
            Result result = this.IsNameInUse(name, bypassReservedNameCheck);
            if (result.Failed)
                return result;
            SymbolRecord symbolRecord = new SymbolRecord();
            symbolRecord.Name = name;
            symbolRecord.Type = type;
            symbolRecord.SymbolOrigin = symbolOrigin;
            this._symbolTable.Add(symbolRecord);
            Vector<SymbolRecord> vector = this._scopedLocalFrameStack[this._scopedLocalFrameStack.Count - 1];
            if (vector == null)
            {
                vector = new Vector<SymbolRecord>();
                this._scopedLocalFrameStack.RemoveAt(this._scopedLocalFrameStack.Count - 1);
                this._scopedLocalFrameStack.Add(vector);
            }
            vector.Add(symbolRecord);
            return Result.Success;
        }

        public Vector<int> NotifyScopedLocalFrameExit()
        {
            if (this._loopFramesStack.Count > 0 && this._scopedLocalFrameStack.Count == this._loopFramesStack.Peek())
            {
                this._loopFramesStack.Pop();
                this._loopStatementStack.Pop();
            }
            Vector<SymbolRecord> scopedLocalFrame = this._scopedLocalFrameStack[this._scopedLocalFrameStack.Count - 1];
            this._scopedLocalFrameStack.RemoveAt(this._scopedLocalFrameStack.Count - 1);
            Vector<int> vector = (Vector<int>)null;
            if (scopedLocalFrame != null)
            {
                vector = new Vector<int>();
                foreach (SymbolRecord symbolRecord in scopedLocalFrame)
                {
                    this._symbolTable.Remove(symbolRecord);
                    int num = this.TrackSymbolUsage(symbolRecord.Name, symbolRecord.SymbolOrigin);
                    if (symbolRecord.SymbolOrigin == SymbolOrigin.ScopedLocal)
                        vector.Add(num);
                }
            }
            return vector == null || vector.Count <= 0 ? (Vector<int>)null : vector;
        }

        private Vector<int> GetImmediateFrameUnwindList(SourceMarkupLoader owner, bool stopAtLoop)
        {
            Vector<int> vector = (Vector<int>)null;
            int num1 = stopAtLoop ? this._loopFramesStack.Peek() : 0;
            for (int index = this._scopedLocalFrameStack.Count - 1; index >= num1; --index)
            {
                Vector<SymbolRecord> scopedLocalFrame = this._scopedLocalFrameStack[index];
                if (scopedLocalFrame != null)
                {
                    if (vector == null)
                        vector = new Vector<int>();
                    foreach (SymbolRecord symbolRecord in scopedLocalFrame)
                    {
                        int num2 = this.TrackSymbolUsage(symbolRecord.Name, symbolRecord.SymbolOrigin);
                        if (symbolRecord.SymbolOrigin == SymbolOrigin.ScopedLocal)
                            vector.Add(num2);
                    }
                }
            }
            return vector == null || vector.Count <= 0 ? (Vector<int>)null : vector;
        }

        public Vector<int> GetImmediateFrameUnwindList(SourceMarkupLoader owner) => this.GetImmediateFrameUnwindList(owner, false);

        public Vector<int> GetLoopUnwindList(SourceMarkupLoader owner) => this.GetImmediateFrameUnwindList(owner, true);

        public ValidateStatementLoop EnclosingLoop => this._loopStatementStack.Count <= 0 ? (ValidateStatementLoop)null : this._loopStatementStack.Peek();

        public int TrackSymbolUsage(string symbol, SymbolOrigin origin)
        {
            for (int index = 0; index < this._usedSymbols.Count; ++index)
            {
                SymbolReference usedSymbol = this._usedSymbols[index];
                if (usedSymbol.Symbol == symbol && usedSymbol.Origin == origin)
                    return index;
            }
            this._usedSymbols.Add(new SymbolReference(symbol, origin));
            return this._usedSymbols.Count - 1;
        }

        public SymbolReference[] SymbolReferenceTable
        {
            get
            {
                SymbolReference[] symbolReferenceArray = new SymbolReference[this._usedSymbols.Count];
                for (int index = 0; index < this._usedSymbols.Count; ++index)
                    symbolReferenceArray[index] = this._usedSymbols[index];
                return symbolReferenceArray;
            }
        }

        public SymbolRecord[] InheritableSymbolsTable
        {
            get
            {
                SymbolRecord[] symbolRecordArray = new SymbolRecord[this._symbolTable.Count];
                for (int index = 0; index < this._symbolTable.Count; ++index)
                    symbolRecordArray[index] = this._symbolTable[index];
                return symbolRecordArray;
            }
        }

        public void StartDeclaredTriggerTracking() => this._declaredTriggerTrackingList = new Vector<ValidateExpression>();

        public void TrackDeclaredTrigger(ValidateExpression expression) => this._declaredTriggerTrackingList.Add(expression);

        public bool IsTrackingDeclaredTriggers => this._declaredTriggerTrackingList != null;

        public Vector<ValidateExpression> StopDeclaredTriggerTracking()
        {
            Vector<ValidateExpression> triggerTrackingList = this._declaredTriggerTrackingList;
            this._declaredTriggerTrackingList = (Vector<ValidateExpression>)null;
            return triggerTrackingList;
        }

        public void StartNotifierTracking(ValidateExpression root) => this._notifierTrackingRoot = root;

        public bool IsNotifierTracking => this._notifierTrackingRoot != null;

        public int TrackDeclareNotifies(ValidateExpression expression) => this.IsNotifierTracking ? (int)this._notifierCount++ : -1;

        public bool StopNotifierTracking()
        {
            bool flag;
            if (this._notifierTrackingRoot.NotifyIndex != -1)
            {
                this._notifierTrackingRoot.MarkNotifierRoot();
                flag = true;
            }
            else
                flag = false;
            this._notifierTrackingRoot = (ValidateExpression)null;
            return flag;
        }

        public uint NotifierCount => this._notifierCount;

        public void RegisterAction(ValidateCode actionCode) => this._actionList.Add(actionCode);

        public void RegisterTrigger(ValidateExpression sourceExpression, ValidateCode actionCode) => this._triggerList.Add(new TriggerRecord()
        {
            SourceExpression = sourceExpression,
            ActionCode = actionCode
        });

        public Vector<TriggerRecord> TriggerList => this._triggerList;

        public PropertySchema GetActivePropertyScope() => this._propertyScopeStack.Count == 0 ? (PropertySchema)null : this._propertyScopeStack.Peek().property;

        public NameUsage GetActiveNameUsage()
        {
            NameUsage nameUsage = NameUsage.Default;
            if (this._propertyScopeStack.Count == 0)
            {
                nameUsage = NameUsage.Unknown;
            }
            else
            {
                foreach (ValidateContext.PropertyScopeRecord propertyScope in this._propertyScopeStack)
                {
                    nameUsage = propertyScope.nameUsage;
                    if (nameUsage != NameUsage.Default)
                        break;
                }
            }
            return nameUsage;
        }

        public SymbolOrigin ActiveSymbolScope => this._currentScope;

        public Vector<ValidateCode> ActionList => this._actionList;

        public LoadPass CurrentPass => this._currentPass;

        public ValidateClass Owner => this._owner;

        public ValidateMethod CurrentMethod => this._currentMethod;

        internal struct PropertyScopeRecord
        {
            public NameUsage nameUsage;
            public PropertySchema property;
        }
    }
}
