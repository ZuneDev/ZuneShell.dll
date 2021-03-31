// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.MarkupEncoder
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Debug;
using Microsoft.Iris.Markup.UIX;
using Microsoft.Iris.Markup.Validation;
using System.Collections;
using System.Diagnostics;

namespace Microsoft.Iris.Markup
{
    internal class MarkupEncoder
    {
        private ByteCodeWriter _writer;
        private MarkupConstantsTable _constantsTable;
        private MarkupLineNumberTable _lineNumberTable;
        private string _sourceFilePathBestGuess;

        public MarkupEncoder(
          MarkupImportTables importTables,
          MarkupConstantsTable constantsTable,
          MarkupLineNumberTable lineNumberTable)
        {
            this._constantsTable = constantsTable;
            this._lineNumberTable = lineNumberTable;
        }

        public ByteCodeReader EncodeOBJECTSection(
          ParseResult parseResult,
          string uri,
          string sourceFilePathBestGuess)
        {
            this._sourceFilePathBestGuess = sourceFilePathBestGuess;
            this._writer = new ByteCodeWriter();
            if (parseResult.ClassList.Count > 0)
            {
                for (int index = 0; index < parseResult.ClassList.Count; ++index)
                    this.EncodeClass(parseResult.ClassList[index]);
            }
            ByteCodeReader reader = this._writer.CreateReader();
            this._writer = (ByteCodeWriter)null;
            return reader;
        }

        private void EncodeClass(ValidateClass cls)
        {
            ValidateUI ui = (ValidateUI)null;
            if (cls.ObjectType == UISchema.Type)
                ui = (ValidateUI)cls;
            ValidateEffect validateEffect = (ValidateEffect)null;
            if (cls.ObjectType == EffectSchema.Type)
                validateEffect = (ValidateEffect)cls;
            if (cls.IndirectedObject == null)
            {
                int totalInitialEvaluates = 0;
                int totalFinalEvaluates = 0;
                if (cls.ActionList.Count > 0)
                    this.EncodeScript(cls.ActionList, out totalInitialEvaluates, out totalFinalEvaluates);
                if (cls.TriggerList.Count > 0)
                    this.EncodeListenerGroupRefresh(cls);
                if (cls.MethodList != null && cls.MethodList.Count > 0)
                    this.EncodeMethods(cls.TypeExport, cls.MethodList);
                if (cls.FoundPropertiesValidateProperty != null)
                {
                    uint offset = this.GetOffset();
                    cls.TypeExport.SetInitializePropertiesOffset(offset);
                    this.EncodeInitializeProperty(cls.FoundPropertiesValidateProperty);
                    cls.RemoveProperty(cls.FoundPropertiesValidateProperty);
                    this._writer.WriteByte(OpCode.ReturnVoid);
                }
                if (ui != null && ui.FoundContentValidateProperty != null)
                {
                    uint offset = this.GetOffset();
                    cls.TypeExport.SetInitializeContentOffset(offset);
                    this.EncodeInitializeProperty(ui.FoundContentValidateProperty);
                    cls.RemoveProperty(ui.FoundContentValidateProperty);
                    this._writer.WriteByte(OpCode.ReturnVoid);
                }
                if (validateEffect != null)
                    this.EncodeTechniquesProperty((EffectClassTypeSchema)cls.TypeExport, validateEffect);
                uint offset1 = this.GetOffset();
                cls.TypeExport.SetInitializeLocalsInputOffset(offset1);
                for (ValidateProperty property = cls.PropertyList; property != null; property = property.Next)
                    this.EncodeInitializeProperty(property);
                if (cls.TypeExport.ListenerCount > 0U)
                    this.EncodeListenerInitialize(cls);
                if (totalInitialEvaluates > 0)
                    this.EstablishInitialOrFinalEvaluateOffsets(cls, totalInitialEvaluates, true);
                if (totalFinalEvaluates > 0)
                    this.EstablishInitialOrFinalEvaluateOffsets(cls, totalFinalEvaluates, false);
                this._writer.WriteByte(OpCode.ReturnVoid);
                if (ui == null)
                    return;
                this.EncodeNamedContent(ui);
            }
            else
            {
                this.EncodeObjectBySource(cls.IndirectedObject);
                this._writer.WriteByte(OpCode.ReturnValue);
            }
        }

        private void EncodeConstructObject(ValidateObjectTag objectTag)
        {
            if (objectTag.IndirectedObject == null)
            {
                if (objectTag.DynamicConstructionType != null)
                {
                    this.EncodeObjectBySource(objectTag.DynamicConstructionType);
                    this._writer.WriteByte(OpCode.ConstructObjectIndirect);
                    this._writer.WriteUInt16(objectTag.FoundTypeIndex);
                    bool forceVerifyValues = objectTag.FoundType is MarkupTypeSchema;
                    this.EncodeInitializeProperties(objectTag, forceVerifyValues);
                    this.EncodeObjectBySource(objectTag.DynamicConstructionType);
                    if (!objectTag.FoundType.HasInitializer)
                        return;
                    this._writer.WriteByte(OpCode.InitializeInstanceIndirect);
                }
                else
                {
                    this._writer.WriteByte(OpCode.ConstructObject);
                    this._writer.WriteUInt16(objectTag.FoundTypeIndex);
                    this.EncodeInitializeProperties(objectTag);
                    this.EncodeInitializeInstance(objectTag.FoundType, objectTag.FoundTypeIndex);
                }
            }
            else
                this.EncodeObjectBySource(objectTag.IndirectedObject);
        }

        private void EncodeInitializeProperties(ValidateObjectTag objectTag) => this.EncodeInitializeProperties(objectTag, false);

        private void EncodeInitializeProperties(ValidateObjectTag objectTag, bool forceVerifyValues)
        {
            if (objectTag.PropertyCount <= 0)
                return;
            for (ValidateProperty property = objectTag.PropertyList; property != null; property = property.Next)
                this.EncodeInitializeProperty(property, objectTag.DynamicConstructionType);
        }

        private void EncodeInitializeInstance(TypeSchema type, int typeIndex)
        {
            if (!type.HasInitializer)
                return;
            this._writer.WriteByte(OpCode.InitializeInstance);
            this._writer.WriteUInt16(typeIndex);
        }

        private void EncodeInitializeProperty(ValidateProperty property) => this.EncodeInitializeProperty(property, (ValidateObject)null);

        private void EncodeInitializeProperty(
          ValidateProperty property,
          ValidateObject dynamicConstructionType)
        {
            if (property.IsPseudo)
                return;
            if (property.ValueApplyMode == ValueApplyMode.SingleValueSet)
            {
                this.EncodeObjectBySource(property.Value);
                this.RecordLineNumber((Validate)property);
                if (dynamicConstructionType == null)
                {
                    this._writer.WriteByte(OpCode.PropertyInitialize);
                    this._writer.WriteUInt16(property.FoundPropertyIndex);
                }
                else
                {
                    this.EncodeObjectBySource(dynamicConstructionType);
                    this._writer.WriteByte(OpCode.PropertyInitializeIndirect);
                    this._writer.WriteUInt16(property.FoundPropertyIndex);
                }
            }
            else
            {
                if ((property.ValueApplyMode & ValueApplyMode.CollectionPopulateAndSet) != ValueApplyMode.SingleValueSet)
                {
                    this._writer.WriteByte(OpCode.ConstructObject);
                    this._writer.WriteUInt16(property.FoundPropertyTypeIndex);
                }
                for (ValidateObjectTag next = (ValidateObjectTag)property.Value; next != null; next = next.Next)
                {
                    if (!next.PropertyIsRequiredForCreation)
                    {
                        uint fixUpLocation = uint.MaxValue;
                        if (property.ShouldSkipDictionaryAddIfContains)
                        {
                            this._writer.WriteByte(OpCode.JumpIfDictionaryContains);
                            this._writer.WriteUInt16((property.ValueApplyMode & ValueApplyMode.CollectionAdd) != ValueApplyMode.SingleValueSet ? property.FoundPropertyIndex : -1);
                            this._writer.WriteUInt16(this._constantsTable.Add((TypeSchema)StringSchema.Type, (object)next.Name, MarkupConstantPersistMode.Binary));
                            fixUpLocation = this.GetOffset();
                            this._writer.WriteUInt32(uint.MaxValue);
                        }
                        this.EncodeConstructObject(next);
                        if ((property.ValueApplyMode & ValueApplyMode.MultiValueDictionary) != ValueApplyMode.SingleValueSet)
                        {
                            this._writer.WriteByte(OpCode.PropertyDictionaryAdd);
                            this._writer.WriteUInt16((property.ValueApplyMode & ValueApplyMode.CollectionAdd) != ValueApplyMode.SingleValueSet ? property.FoundPropertyIndex : -1);
                            this._writer.WriteUInt16(this._constantsTable.Add((TypeSchema)StringSchema.Type, (object)next.Name, MarkupConstantPersistMode.Binary));
                        }
                        else
                        {
                            this._writer.WriteByte(OpCode.PropertyListAdd);
                            this._writer.WriteUInt16((property.ValueApplyMode & ValueApplyMode.CollectionAdd) != ValueApplyMode.SingleValueSet ? property.FoundPropertyIndex : -1);
                        }
                        if (fixUpLocation != uint.MaxValue)
                            this.FixUpJumpOffset(fixUpLocation);
                    }
                }
                if ((property.ValueApplyMode & ValueApplyMode.CollectionPopulateAndSet) == ValueApplyMode.SingleValueSet)
                    return;
                if (dynamicConstructionType == null)
                {
                    this._writer.WriteByte(OpCode.PropertyInitialize);
                    this._writer.WriteUInt16(property.FoundPropertyIndex);
                }
                else
                {
                    this.EncodeObjectBySource(dynamicConstructionType);
                    this.RecordLineNumber((Validate)property);
                    this._writer.WriteByte(OpCode.PropertyInitializeIndirect);
                    this._writer.WriteUInt16(property.FoundPropertyIndex);
                }
            }
        }

        private void EncodeObjectBySource(ValidateObject obj)
        {
            switch (obj.ObjectSourceType)
            {
                case ObjectSourceType.ObjectTag:
                    this.EncodeConstructObject((ValidateObjectTag)obj);
                    break;
                case ObjectSourceType.FromString:
                    this.EncodeFromString((ValidateFromString)obj);
                    break;
                case ObjectSourceType.Code:
                    this.EncodeCode((ValidateCode)obj);
                    break;
                case ObjectSourceType.Expression:
                    this.EncodeExpression((ValidateExpression)obj, (ListenerEncodeMode)null);
                    break;
            }
        }

        private void EncodeFromString(ValidateFromString fromString)
        {
            if (fromString.ObjectType.IsRuntimeImmutable)
            {
                MarkupConstantPersistMode persistMode;
                object persistData;
                if (fromString.ObjectType.SupportsBinaryEncoding)
                {
                    persistMode = MarkupConstantPersistMode.Binary;
                    persistData = fromString.FromStringInstance;
                }
                else
                {
                    persistMode = MarkupConstantPersistMode.FromString;
                    persistData = (object)fromString.FromString;
                }
                int rawValue = this._constantsTable.Add(fromString.ObjectType, fromString.FromStringInstance, persistMode, persistData);
                this._writer.WriteByte(OpCode.PushConstant);
                this._writer.WriteUInt16(rawValue);
            }
            else if (fromString.ObjectType.SupportsBinaryEncoding)
            {
                this._writer.WriteByte(OpCode.ConstructFromBinary);
                this._writer.WriteUInt16(fromString.TypeHintIndex);
                fromString.ObjectType.EncodeBinary(this._writer, fromString.FromStringInstance);
            }
            else
            {
                int rawValue = this._constantsTable.Add((TypeSchema)StringSchema.Type, (object)fromString.FromString, MarkupConstantPersistMode.FromString);
                this._writer.WriteByte(OpCode.ConstructFromString);
                this._writer.WriteUInt16(fromString.TypeHintIndex);
                this._writer.WriteUInt16(rawValue);
            }
        }

        private void EncodeCanonicalInstance(object instance, TypeSchema type, string memberName)
        {
            int rawValue = this._constantsTable.Add(type, instance, MarkupConstantPersistMode.Canonical, (object)memberName);
            this._writer.WriteByte(OpCode.PushConstant);
            this._writer.WriteUInt16(rawValue);
        }

        private void EncodeNamedContent(ValidateUI ui)
        {
            if (ui.FoundNamedContentProperties == null)
                return;
            NamedContentRecord[] namedContentTable = ((UIClassTypeSchema)ui.TypeExport).NamedContentTable;
            int index = 0;
            foreach (ValidateProperty namedContentProperty in ui.FoundNamedContentProperties)
            {
                uint offset = this.GetOffset();
                namedContentTable[index].SetOffset(offset);
                this.EncodeObjectBySource(namedContentProperty.Value);
                this._writer.WriteByte(OpCode.ReturnValue);
                ++index;
            }
        }

        private void EncodeCode(ValidateCode code)
        {
            uint offset = this.GetOffset();
            code.TrackEncodingOffset(offset);
            this.EncodeStatement((ValidateStatement)code.StatementCompound);
            if (code.ReturnStatements != null)
            {
                foreach (ValidateStatementReturn returnStatement in code.ReturnStatements)
                {
                    if (!returnStatement.IsTrailingReturn)
                        this.FixUpJumpOffset(returnStatement.JumpFixupOffset);
                }
            }
            if (code.Embedded)
                return;
            if (code.ObjectType != VoidSchema.Type)
                this._writer.WriteByte(OpCode.ReturnValue);
            else
                this._writer.WriteByte(OpCode.ReturnVoid);
        }

        private void EncodeStatement(ValidateStatement statement)
        {
            this.RecordLineNumber((Validate)statement);
            if (statement.StatementType != StatementType.Compound)
                this.DeclareDebugPoint(statement.Line, statement.Column);
            switch (statement.StatementType)
            {
                case StatementType.Assignment:
                    ValidateStatementAssignment statementAssignment = (ValidateStatementAssignment)statement;
                    if (statementAssignment.DeclaredScopedLocal != null)
                        this.EncodeStatement((ValidateStatement)statementAssignment.DeclaredScopedLocal);
                    this.EncodeExpression(statementAssignment.RValue);
                    this.EncodeExpression(statementAssignment.LValue);
                    this._writer.WriteByte(OpCode.DiscardValue);
                    break;
                case StatementType.Expression:
                    ValidateStatementExpression statementExpression = (ValidateStatementExpression)statement;
                    this.EncodeExpression(statementExpression.Expression);
                    if (statementExpression.Expression.ObjectType == VoidSchema.Type)
                        break;
                    this._writer.WriteByte(OpCode.DiscardValue);
                    break;
                case StatementType.ForEach:
                    ValidateStatementForEach statementForEach = (ValidateStatementForEach)statement;
                    this.EncodeScopedLocal(statementForEach.ScopedLocal);
                    this.EncodeExpression(statementForEach.Expression);
                    this._writer.WriteByte(OpCode.MethodInvoke);
                    this._writer.WriteUInt16(statementForEach.FoundGetEnumeratorIndex);
                    uint offset1 = this.GetOffset();
                    this._writer.WriteByte(OpCode.MethodInvokePeek);
                    this._writer.WriteUInt16(statementForEach.FoundMoveNextIndex);
                    this._writer.WriteByte(OpCode.JumpIfFalse);
                    uint offset2 = this.GetOffset();
                    this._writer.WriteUInt32(uint.MaxValue);
                    this._writer.WriteByte(OpCode.PropertyGetPeek);
                    this._writer.WriteUInt16(statementForEach.FoundCurrentIndex);
                    this._writer.WriteByte(OpCode.VerifyTypeCast);
                    this._writer.WriteUInt16(statementForEach.ScopedLocal.FoundTypeIndex);
                    this._writer.WriteByte(OpCode.WriteSymbol);
                    this._writer.WriteUInt16(statementForEach.ScopedLocal.FoundSymbolIndex);
                    this.EncodeStatement((ValidateStatement)statementForEach.StatementCompound);
                    this._writer.WriteByte(OpCode.Jump);
                    this._writer.WriteUInt32(offset1);
                    this.FixUpJumpOffset(offset2);
                    uint offset3 = this.GetOffset();
                    this._writer.WriteByte(OpCode.DiscardValue);
                    this.EncodeScopedLocalsWipe(statementForEach.ScopedLocalsToClear);
                    foreach (ValidateStatementBreak breakStatement in statementForEach.BreakStatements)
                    {
                        if (breakStatement.IsContinue)
                            this.FixUpJumpOffset(breakStatement.JumpFixupOffset, offset1);
                        else
                            this.FixUpJumpOffset(breakStatement.JumpFixupOffset, offset3);
                    }
                    break;
                case StatementType.While:
                    ValidateStatementWhile validateStatementWhile = (ValidateStatementWhile)statement;
                    uint offset4 = this.GetOffset();
                    if (validateStatementWhile.IsDoWhile)
                        this.EncodeStatement(validateStatementWhile.Body);
                    this.EncodeExpression(validateStatementWhile.Condition);
                    this._writer.WriteByte(OpCode.JumpIfFalse);
                    uint offset5 = this.GetOffset();
                    this._writer.WriteUInt32(uint.MaxValue);
                    if (!validateStatementWhile.IsDoWhile)
                        this.EncodeStatement(validateStatementWhile.Body);
                    this._writer.WriteByte(OpCode.Jump);
                    this._writer.WriteUInt32(offset4);
                    this.FixUpJumpOffset(offset5);
                    uint offset6 = this.GetOffset();
                    foreach (ValidateStatementBreak breakStatement in validateStatementWhile.BreakStatements)
                    {
                        if (breakStatement.IsContinue)
                            this.FixUpJumpOffset(breakStatement.JumpFixupOffset, offset4);
                        else
                            this.FixUpJumpOffset(breakStatement.JumpFixupOffset, offset6);
                    }
                    break;
                case StatementType.If:
                    ValidateStatementIf validateStatementIf = (ValidateStatementIf)statement;
                    this.EncodeExpression(validateStatementIf.Condition);
                    this._writer.WriteByte(OpCode.JumpIfFalse);
                    uint offset7 = this.GetOffset();
                    this._writer.WriteUInt32(uint.MaxValue);
                    this.EncodeStatement((ValidateStatement)validateStatementIf.StatementCompound);
                    this.FixUpJumpOffset(offset7);
                    break;
                case StatementType.IfElse:
                    ValidateStatementIfElse validateStatementIfElse = (ValidateStatementIfElse)statement;
                    this.EncodeExpression(validateStatementIfElse.Condition);
                    this._writer.WriteByte(OpCode.JumpIfFalse);
                    uint offset8 = this.GetOffset();
                    this._writer.WriteUInt32(uint.MaxValue);
                    this.EncodeStatement(validateStatementIfElse.StatementCompoundTrue);
                    this._writer.WriteByte(OpCode.Jump);
                    uint offset9 = this.GetOffset();
                    this._writer.WriteUInt32(uint.MaxValue);
                    this.FixUpJumpOffset(offset8);
                    this.EncodeStatement(validateStatementIfElse.StatementCompoundFalse);
                    this.FixUpJumpOffset(offset9);
                    break;
                case StatementType.Return:
                    ValidateStatementReturn validateStatementReturn = (ValidateStatementReturn)statement;
                    if (validateStatementReturn.Expression != null)
                        this.EncodeExpression(validateStatementReturn.Expression);
                    if (validateStatementReturn.IsTrailingReturn)
                        break;
                    this.EncodeScopedLocalsWipe(validateStatementReturn.ScopedLocalsToClear);
                    this._writer.WriteByte(OpCode.Jump);
                    uint offset10 = this.GetOffset();
                    this._writer.WriteUInt32(uint.MaxValue);
                    validateStatementReturn.TrackJumpFixupOffset(offset10);
                    break;
                case StatementType.ScopedLocal:
                    this.EncodeScopedLocal((ValidateStatementScopedLocal)statement);
                    break;
                case StatementType.Compound:
                    ValidateStatementCompound statementCompound = (ValidateStatementCompound)statement;
                    for (ValidateStatement statement1 = statementCompound.StatementList; statement1 != null; statement1 = statement1.Next)
                        this.EncodeStatement(statement1);
                    this.EncodeScopedLocalsWipe(statementCompound.ScopedLocalsToClear);
                    break;
                case StatementType.Break:
                    ValidateStatementBreak validateStatementBreak = (ValidateStatementBreak)statement;
                    this.EncodeScopedLocalsWipe(validateStatementBreak.ScopedLocalsToClear);
                    this._writer.WriteByte(OpCode.Jump);
                    uint offset11 = this.GetOffset();
                    this._writer.WriteUInt32(uint.MaxValue);
                    validateStatementBreak.TrackJumpFixupOffset(offset11);
                    break;
            }
        }

        private void EncodeExpression(ValidateExpression expression) => this.EncodeExpression(expression, (ListenerEncodeMode)null);

        private void EncodeExpression(
          ValidateExpression expression,
          ListenerEncodeMode listenerEncodeMode)
        {
            this.RecordLineNumber((Validate)expression);
            expression.TrackEncodingOffset(this.GetOffset());
            switch (expression.ExpressionType)
            {
                case ExpressionType.Assignment:
                    ValidateExpressionAssignment expressionAssignment = (ValidateExpressionAssignment)expression;
                    if (!expressionAssignment.IsIndexAssignment)
                        this.EncodeExpression(expressionAssignment.RValue);
                    this.EncodeExpression(expressionAssignment.LValue);
                    break;
                case ExpressionType.Symbol:
                    bool flag1 = true;
                    ValidateExpressionSymbol expressionSymbol = (ValidateExpressionSymbol)expression;
                    if (expressionSymbol.NotifyIndex >= 0 && listenerEncodeMode != null)
                    {
                        this.EncodeListenerHookup(listenerEncodeMode, expressionSymbol.NotifyIndex, ListenerType.Symbol, expressionSymbol.FoundSymbolIndex, expressionSymbol.IsNotifierRoot);
                        flag1 = !expressionSymbol.IsNotifierRoot;
                    }
                    if (!flag1)
                        break;
                    this._writer.WriteByte(expressionSymbol.Usage != ExpressionUsage.RValue ? OpCode.WriteSymbolPeek : OpCode.LookupSymbol);
                    this._writer.WriteUInt16(expressionSymbol.FoundSymbolIndex);
                    break;
                case ExpressionType.Call:
                    ValidateExpressionCall validateExpressionCall = (ValidateExpressionCall)expression;
                    bool flag2 = false;
                    if (validateExpressionCall.Target != null)
                    {
                        this.EncodeExpression(validateExpressionCall.Target, listenerEncodeMode);
                        flag2 = true;
                    }
                    switch (validateExpressionCall.FoundMemberType)
                    {
                        case SchemaType.Property:
                            bool flag3 = true;
                            if (validateExpressionCall.NotifyIndex >= 0 && listenerEncodeMode != null)
                            {
                                this.EncodeListenerHookup(listenerEncodeMode, validateExpressionCall.NotifyIndex, ListenerType.Property, validateExpressionCall.FoundMemberIndex, validateExpressionCall.IsNotifierRoot);
                                flag3 = !validateExpressionCall.IsNotifierRoot;
                            }
                            if (!flag3)
                                return;
                            OpCode opCode = validateExpressionCall.Usage != ExpressionUsage.RValue ? (flag2 ? OpCode.PropertyAssign : OpCode.PropertyAssignStatic) : (flag2 ? OpCode.PropertyGet : OpCode.PropertyGetStatic);
                            this.RecordLineNumber((Validate)expression);
                            this._writer.WriteByte(opCode);
                            this._writer.WriteUInt16(validateExpressionCall.FoundMemberIndex);
                            return;
                        case SchemaType.Method:
                            if (validateExpressionCall.ParameterList != ValidateParameter.EmptyList)
                            {
                                for (ValidateParameter validateParameter = validateExpressionCall.ParameterList; validateParameter != null; validateParameter = validateParameter.Next)
                                    this.EncodeExpression(validateParameter.Expression);
                            }
                            this.RecordLineNumber((Validate)expression);
                            this._writer.WriteByte(validateExpressionCall.IsIndexAssignment ? (flag2 ? OpCode.MethodInvokePushLastParam : OpCode.MethodInvokeStaticPushLastParam) : (flag2 ? OpCode.MethodInvoke : OpCode.MethodInvokeStatic));
                            this._writer.WriteUInt16(validateExpressionCall.FoundMemberIndex);
                            return;
                        case SchemaType.Event:
                            if (validateExpressionCall.NotifyIndex < 0 || listenerEncodeMode == null)
                                return;
                            this.EncodeListenerHookup(listenerEncodeMode, validateExpressionCall.NotifyIndex, ListenerType.Event, validateExpressionCall.FoundMemberIndex, validateExpressionCall.IsNotifierRoot);
                            return;
                        case SchemaType.CanonicalInstance:
                            this.EncodeCanonicalInstance(validateExpressionCall.FoundCanonicalInstance, validateExpressionCall.ObjectType, validateExpressionCall.MemberName);
                            return;
                        default:
                            return;
                    }
                case ExpressionType.Cast:
                    ValidateExpressionCast validateExpressionCast = (ValidateExpressionCast)expression;
                    this.EncodeExpression(validateExpressionCast.Castee, listenerEncodeMode);
                    this.RecordLineNumber((Validate)expression);
                    if (validateExpressionCast.FoundCastMethod == CastMethod.Cast)
                    {
                        this._writer.WriteByte(OpCode.VerifyTypeCast);
                        this._writer.WriteUInt16(validateExpressionCast.FoundTypeCastIndex);
                        break;
                    }
                    this._writer.WriteByte(OpCode.ConvertType);
                    this._writer.WriteUInt16(validateExpressionCast.FoundTypeCastIndex);
                    this._writer.WriteUInt16(validateExpressionCast.FoundCasteeTypeIndex);
                    break;
                case ExpressionType.New:
                    ValidateExpressionNew validateExpressionNew = (ValidateExpressionNew)expression;
                    if (!validateExpressionNew.IsParameterizedConstruction)
                    {
                        this._writer.WriteByte(OpCode.ConstructObject);
                        this._writer.WriteUInt16(validateExpressionNew.FoundConstructTypeIndex);
                    }
                    else
                    {
                        for (ValidateParameter validateParameter = validateExpressionNew.ParameterList; validateParameter != null; validateParameter = validateParameter.Next)
                            this.EncodeExpression(validateParameter.Expression);
                        this.RecordLineNumber((Validate)expression);
                        this._writer.WriteByte(OpCode.ConstructObjectParam);
                        this._writer.WriteUInt16(validateExpressionNew.FoundConstructTypeIndex);
                        this._writer.WriteUInt16(validateExpressionNew.FoundParameterizedConstructorIndex);
                    }
                    this.EncodeInitializeInstance(validateExpressionNew.FoundConstructType, validateExpressionNew.FoundConstructTypeIndex);
                    break;
                case ExpressionType.Operation:
                    ValidateExpressionOperation expressionOperation = (ValidateExpressionOperation)expression;
                    this.EncodeExpression(expressionOperation.LeftSide);
                    uint fixUpLocation = uint.MaxValue;
                    if (expressionOperation.FoundOperationTargetType == BooleanSchema.Type && (expressionOperation.Op == OperationType.LogicalAnd || expressionOperation.Op == OperationType.LogicalOr))
                    {
                        this.RecordLineNumber((Validate)expression);
                        this._writer.WriteByte(expressionOperation.Op == OperationType.LogicalOr ? OpCode.JumpIfTruePeek : OpCode.JumpIfFalsePeek);
                        fixUpLocation = this.GetOffset();
                        this._writer.WriteUInt32(uint.MaxValue);
                    }
                    if (expressionOperation.RightSide != null)
                        this.EncodeExpression(expressionOperation.RightSide);
                    this.RecordLineNumber((Validate)expression);
                    this._writer.WriteByte(OpCode.Operation);
                    this._writer.WriteUInt16(expressionOperation.FoundOperationTargetTypeIndex);
                    this._writer.WriteByte((byte)expressionOperation.Op);
                    if (fixUpLocation == uint.MaxValue)
                        break;
                    this.FixUpJumpOffset(fixUpLocation);
                    break;
                case ExpressionType.IsCheck:
                    ValidateExpressionIsCheck expressionIsCheck = (ValidateExpressionIsCheck)expression;
                    this.EncodeExpression(expressionIsCheck.Expression);
                    this.RecordLineNumber((Validate)expression);
                    this._writer.WriteByte(OpCode.IsCheck);
                    this._writer.WriteUInt16(expressionIsCheck.TypeIdentifier.FoundTypeIndex);
                    break;
                case ExpressionType.As:
                    ValidateExpressionAs validateExpressionAs = (ValidateExpressionAs)expression;
                    this.EncodeExpression(validateExpressionAs.Expression);
                    this.RecordLineNumber((Validate)expression);
                    this._writer.WriteByte(OpCode.As);
                    this._writer.WriteUInt16(validateExpressionAs.TypeIdentifier.FoundTypeIndex);
                    break;
                case ExpressionType.Constant:
                    ValidateExpressionConstant expressionConstant = (ValidateExpressionConstant)expression;
                    if (expressionConstant.ConstantType != ConstantType.Null)
                    {
                        int rawValue = this._constantsTable.Add(expressionConstant.ObjectType, expressionConstant.FoundConstant, MarkupConstantPersistMode.Binary);
                        this._writer.WriteByte(OpCode.PushConstant);
                        this._writer.WriteUInt16(rawValue);
                        break;
                    }
                    this._writer.WriteByte(OpCode.PushNull);
                    break;
                case ExpressionType.DeclareTrigger:
                    this.EncodeExpression(((ValidateExpressionDeclareTrigger)expression).Expression);
                    break;
                case ExpressionType.TypeOf:
                    ValidateExpressionTypeOf expressionTypeOf = (ValidateExpressionTypeOf)expression;
                    this._writer.WriteByte(OpCode.TypeOf);
                    this._writer.WriteUInt16(expressionTypeOf.TypeIdentifier.FoundTypeIndex);
                    break;
                case ExpressionType.List:
                    ArrayList expressions = ((ValidateExpressionList)expression).Expressions;
                    for (int index = 0; index < expressions.Count; ++index)
                    {
                        ValidateExpression expression1 = (ValidateExpression)expressions[index];
                        this.EncodeExpression(expression1);
                        if (index < expressions.Count - 1 && expression1.ObjectType != VoidSchema.Type)
                            this._writer.WriteByte(OpCode.DiscardValue);
                    }
                    break;
                case ExpressionType.Index:
                    this.EncodeExpression(((ValidateExpressionIndex)expression).CallExpression);
                    break;
                case ExpressionType.Ternary:
                    ValidateExpressionTernary expressionTernary = (ValidateExpressionTernary)expression;
                    this.EncodeExpression(expressionTernary.Condition);
                    this._writer.WriteByte(OpCode.JumpIfFalse);
                    uint offset1 = this.GetOffset();
                    this._writer.WriteUInt32(uint.MaxValue);
                    this.EncodeExpression(expressionTernary.TrueClause);
                    this._writer.WriteByte(OpCode.Jump);
                    uint offset2 = this.GetOffset();
                    this._writer.WriteUInt32(uint.MaxValue);
                    this.FixUpJumpOffset(offset1);
                    this.EncodeExpression(expressionTernary.FalseClause);
                    this.FixUpJumpOffset(offset2);
                    break;
                case ExpressionType.NullCoalescing:
                    ValidateExpressionNullCoalescing expressionNullCoalescing = (ValidateExpressionNullCoalescing)expression;
                    this.EncodeExpression(expressionNullCoalescing.Condition);
                    this._writer.WriteByte(OpCode.JumpIfNullPeek);
                    uint offset3 = this.GetOffset();
                    this._writer.WriteUInt32(uint.MaxValue);
                    this._writer.WriteByte(OpCode.Jump);
                    uint offset4 = this.GetOffset();
                    this._writer.WriteUInt32(uint.MaxValue);
                    this.FixUpJumpOffset(offset3);
                    this._writer.WriteByte(OpCode.DiscardValue);
                    this.EncodeExpression(expressionNullCoalescing.NullClause);
                    this.FixUpJumpOffset(offset4);
                    break;
                case ExpressionType.This:
                case ExpressionType.BaseClass:
                    this._writer.WriteByte(OpCode.PushThis);
                    break;
            }
        }

        private void EncodeScopedLocal(ValidateStatementScopedLocal statementScopedLocal)
        {
            if (statementScopedLocal.HasInitialAssignment)
                return;
            if (!statementScopedLocal.FoundType.IsNullAssignable)
            {
                this._writer.WriteByte(OpCode.ConstructObject);
                this._writer.WriteUInt16(statementScopedLocal.FoundTypeIndex);
            }
            else
                this._writer.WriteByte(OpCode.PushNull);
            this._writer.WriteByte(OpCode.WriteSymbol);
            this._writer.WriteUInt16(statementScopedLocal.FoundSymbolIndex);
        }

        private void EncodeScopedLocalsWipe(Vector<int> scopedLocalsToClear)
        {
            if (scopedLocalsToClear == null)
                return;
            foreach (int rawValue in scopedLocalsToClear)
            {
                this._writer.WriteByte(OpCode.ClearSymbol);
                this._writer.WriteUInt16(rawValue);
            }
        }

        private bool EncodeListenerHookup(
          ListenerEncodeMode listenerEncodeMode,
          int localListenerIndex,
          ListenerType listenerType,
          int targetIndex,
          bool isRoot)
        {
            uint rawValue = listenerEncodeMode.SequentialListenerIndex((uint)localListenerIndex);
            if (isRoot)
            {
                this._writer.WriteByte(OpCode.Listen);
                this._writer.WriteUInt16(rawValue);
                this._writer.WriteByte((byte)listenerType);
                this._writer.WriteUInt16(targetIndex);
                this._writer.WriteUInt32(listenerEncodeMode.ScriptId);
            }
            else
            {
                uint num = listenerEncodeMode.RunOnNonTailTrigger ? listenerEncodeMode.ScriptId : uint.MaxValue;
                this._writer.WriteByte(OpCode.DestructiveListen);
                this._writer.WriteUInt16(rawValue);
                this._writer.WriteByte((byte)listenerType);
                this._writer.WriteUInt16(targetIndex);
                this._writer.WriteUInt32(num);
                this._writer.WriteUInt32(listenerEncodeMode.RefreshId);
            }
            return isRoot;
        }

        private void EncodeScript(
          Vector<ValidateCode> actionList,
          out int totalInitialEvaluates,
          out int totalFinalEvaluates)
        {
            totalInitialEvaluates = 0;
            totalFinalEvaluates = 0;
            foreach (ValidateCode action in actionList)
            {
                if (action.InitialEvaluate)
                    ++totalInitialEvaluates;
                if (action.FinalEvaluate)
                    ++totalFinalEvaluates;
                this.EncodeCode(action);
            }
        }

        private void EncodeMethods(MarkupTypeSchema typeSchema, ArrayList methodList)
        {
            foreach (ValidateMethod method in methodList)
            {
                uint offset = this.GetOffset();
                this.EncodeCode(method.Body);
                uint codeOffset = typeSchema.EncodeScriptOffsetAsId(offset);
                method.MethodExport.SetCodeOffset(codeOffset);
            }
        }

        private void EncodeListenerGroupRefresh(ValidateClass cls)
        {
            Vector<TriggerRecord> triggerList = cls.TriggerList;
            ListenerEncodeMode listenerEncodeMode = new ListenerEncodeMode(cls.TypeExport);
            foreach (TriggerRecord triggerRecord in triggerList)
            {
                listenerEncodeMode.TriggerContainer = triggerRecord;
                this.EncodeExpression(triggerRecord.SourceExpression, listenerEncodeMode);
                this._writer.WriteByte((byte)37);
            }
        }

        private void EncodeListenerInitialize(ValidateClass cls)
        {
            this._writer.WriteByte(OpCode.ConstructListenerStorage);
            this._writer.WriteUInt16(cls.TypeExport.ListenerCount);
            Vector<TriggerRecord> triggerList = cls.TriggerList;
            uint[] refreshGroupOffsets = new uint[triggerList.Count];
            for (int index = 0; index < triggerList.Count; ++index)
            {
                TriggerRecord triggerRecord = triggerList[index];
                refreshGroupOffsets[index] = triggerRecord.SourceExpression.EncodeStartOffset;
            }
            cls.TypeExport.SetRefreshListenerGroupOffsets(refreshGroupOffsets);
        }

        private void EstablishInitialOrFinalEvaluateOffsets(
          ValidateClass cls,
          int totalEvaluates,
          bool isInitialEvaluate)
        {
            Vector<ValidateCode> actionList = cls.ActionList;
            uint[] numArray = new uint[totalEvaluates];
            int num = 0;
            foreach (ValidateCode validateCode in actionList)
            {
                if (isInitialEvaluate && validateCode.InitialEvaluate || !isInitialEvaluate && validateCode.FinalEvaluate)
                    numArray[num++] = validateCode.EncodeStartOffset;
            }
            if (isInitialEvaluate)
                cls.TypeExport.SetInitialEvaluateOffsets(numArray);
            else
                cls.TypeExport.SetFinalEvaluateOffsets(numArray);
        }

        private void EncodeTechniquesProperty(
          EffectClassTypeSchema typeExport,
          ValidateEffect validateEffect)
        {
            ValidateProperty validateProperty = validateEffect.FoundTechniquesValidateProperty;
            if (validateProperty == null)
                return;
            int length = 0;
            ValidateObjectTag validateObjectTag1 = validateProperty.Value as ValidateObjectTag;
            for (ValidateObjectTag validateObjectTag2 = validateObjectTag1; validateObjectTag2 != null; validateObjectTag2 = validateObjectTag2.Next)
                ++length;
            uint[] techniqueOffsets = new uint[length];
            int num = 0;
            for (ValidateObjectTag objectTag = validateObjectTag1; objectTag != null; objectTag = objectTag.Next)
            {
                techniqueOffsets[num++] = this.GetOffset();
                this.EncodeConstructObject(objectTag);
                this._writer.WriteByte(OpCode.ReturnValue);
            }
            typeExport.SetTechniqueOffsets(techniqueOffsets);
            validateEffect.RemoveProperty(validateEffect.FoundTechniquesValidateProperty);
            if (validateEffect.FoundInstancePropertyAssignments == null)
                return;
            uint[] instancePropertyAssignments = new uint[length];
            for (int index = 0; index < length; ++index)
                instancePropertyAssignments[index] = this.GetOffset();
            foreach (KeyValueEntry<string, ValidateProperty> propertyAssignment in validateEffect.FoundInstancePropertyAssignments)
            {
                int elementSymbolIndex = validateEffect.GetElementSymbolIndex(propertyAssignment.Key);
                this._writer.WriteByte(OpCode.LookupSymbol);
                this._writer.WriteUInt16(elementSymbolIndex);
                for (ValidateProperty next = propertyAssignment.Value; next != null; next = next.Next)
                    this.EncodeInitializeProperty(next);
                this._writer.WriteByte(OpCode.DiscardValue);
            }
            this._writer.WriteByte(OpCode.ReturnVoid);
            typeExport.SetInstancePropertyAssignments(instancePropertyAssignments);
        }

        private uint GetOffset() => this._writer.DataSize;

        private void FixUpJumpOffset(uint fixUpLocation, uint fixedOffset) => this._writer.Overwrite(fixUpLocation, fixedOffset);

        private void FixUpJumpOffset(uint fixUpLocation) => this.FixUpJumpOffset(fixUpLocation, this.GetOffset());

        private void RecordLineNumber(Validate validate) => this._lineNumberTable.AddRecord(this.GetOffset(), validate.Line, validate.Column);

        [Conditional("DEBUG")]
        private void DEBUG_EmitStart()
        {
        }

        [Conditional("DEBUG")]
        private void DEBUG_EmitStop(OpCode opCode) => Microsoft.Iris.Debug.Trace.IsCategoryEnabled(TraceCategory.MarkupEncoding);

        [Conditional("DEBUG")]
        private void DEBUG_EmitStop(OpCode opCode, object param) => Microsoft.Iris.Debug.Trace.IsCategoryEnabled(TraceCategory.MarkupEncoding);

        [Conditional("DEBUG")]
        private void DEBUG_EmitStop(OpCode opCode, object param, object param2) => Microsoft.Iris.Debug.Trace.IsCategoryEnabled(TraceCategory.MarkupEncoding);

        [Conditional("DEBUG")]
        private void DEBUG_EmitStop(OpCode opCode, object param, object param2, object param3) => Microsoft.Iris.Debug.Trace.IsCategoryEnabled(TraceCategory.MarkupEncoding);

        [Conditional("DEBUG")]
        private void DEBUG_EmitStop(
          OpCode opCode,
          object param,
          object param2,
          object param3,
          object param4)
        {
            Microsoft.Iris.Debug.Trace.IsCategoryEnabled(TraceCategory.MarkupEncoding);
        }

        [Conditional("DEBUG")]
        private void DEBUG_EmitStop(
          OpCode opCode,
          object param,
          object param2,
          object param3,
          object param4,
          object param5)
        {
            Microsoft.Iris.Debug.Trace.IsCategoryEnabled(TraceCategory.MarkupEncoding);
        }

        [Conditional("DEBUG")]
        private void DEBUG_EmitStopWorker(
          OpCode opCode,
          object data0,
          object data1,
          object data2,
          object data3,
          object data4)
        {
        }

        [Conditional("DEBUG")]
        private void DEBUG_ReportFixup(uint fixupLocation, uint value)
        {
        }

        private void DeclareDebugPoint(int line, int column)
        {
        }
    }
}
