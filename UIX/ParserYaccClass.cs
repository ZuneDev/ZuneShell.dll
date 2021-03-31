// Decompiled with JetBrains decompiler
// Type: ParserYaccClass
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup;
using Microsoft.Iris.Markup.Validation;
using SSVParseLib;

internal class ParserYaccClass : SSYacc
{
    public const int ParserYaccProdStartCodeBlock = 1;
    public const int ParserYaccProdStartInlineExpression = 2;
    public const int ParserYaccProdStartMethods = 3;
    public const int ParserYaccProdMethodsEmpty = 4;
    public const int ParserYaccProdMethods = 5;
    public const int ParserYaccProdMethodSpecifiersEmpty = 6;
    public const int ParserYaccProdMethodSpecifiers = 7;
    public const int ParserYaccProdMethodSpecifierVirtual = 8;
    public const int ParserYaccProdMethodSpecifierOverride = 9;
    public const int ParserYaccProdMethod = 10;
    public const int ParserYaccProdParameterDefinitionsEmpty = 11;
    public const int ParserYaccProdParameterDefinitions = 12;
    public const int ParserYaccProdParameterDefinitionListSingle = 13;
    public const int ParserYaccProdParameterDefinitionListMulti = 14;
    public const int ParserYaccProdParameterDefinition = 15;
    public const int ParserYaccProdExpressionListEmpty = 16;
    public const int ParserYaccProdExpressionList = 17;
    public const int ParserYaccProdExpressionsSingle = 18;
    public const int ParserYaccProdExpressionsMulti = 19;
    public const int ParserYaccProdStatementLocalDeclaration = 20;
    public const int ParserYaccProdStatementLocalAssignment = 21;
    public const int ParserYaccProdForInitializerDecl = 22;
    public const int ParserYaccProdForInitializerExprList = 23;
    public const int ParserYaccProdStatementIf = 24;
    public const int ParserYaccProdStatementIfElse = 25;
    public const int ParserYaccProdStatementForEach = 26;
    public const int ParserYaccProdStatementWhile = 27;
    public const int ParserYaccProdStatementDoWhile = 28;
    public const int ParserYaccProdStatementFor = 29;
    public const int ParserYaccProdStatementDecl = 30;
    public const int ParserYaccProdStatementExpr = 31;
    public const int ParserYaccProdStatementReturn = 32;
    public const int ParserYaccProdStatementReturnExpression = 33;
    public const int ParserYaccProdStatementBreak = 34;
    public const int ParserYaccProdStatementContinue = 35;
    public const int ParserYaccProdStatementAttribute = 36;
    public const int ParserYaccProdStatementCompound = 37;
    public const int ParserYaccProdStatementsMulti = 38;
    public const int ParserYaccProdStatementsEmpty = 39;
    public const int ParserYaccProdExpressionCallMethod = 40;
    public const int ParserYaccProdExpressionCallThisMethod = 41;
    public const int ParserYaccProdExpressionCallProperty = 42;
    public const int ParserYaccProdExpressionCallStaticProperty = 43;
    public const int ParserYaccProdExpressionCallStaticMethod = 44;
    public const int ParserYaccProdExpressionIndex = 45;
    public const int ParserYaccProdExpressionNew = 46;
    public const int ParserYaccProdExpressionSymbol = 47;
    public const int ParserYaccProdExpressionString = 48;
    public const int ParserYaccProdExpressionStringLiteral = 49;
    public const int ParserYaccProdExpressionInteger = 50;
    public const int ParserYaccProdExpressionLongInteger = 51;
    public const int ParserYaccProdExpressionFloat = 52;
    public const int ParserYaccProdExpressionTrue = 53;
    public const int ParserYaccProdExpressionFalse = 54;
    public const int ParserYaccProdExpressionNull = 55;
    public const int ParserYaccProdExpressionThis = 56;
    public const int ParserYaccProdExpressionBase = 57;
    public const int ParserYaccProdExpressionTypeOf = 58;
    public const int ParserYaccProdExpressionGroup = 59;
    public const int ParserYaccProdExpressionDeclareTrigger = 60;
    public const int ParserYaccProdExpressionUnaryOperationLogicalNot = 61;
    public const int ParserYaccProdExpressionUnaryMinus = 62;
    public const int ParserYaccProdExpressionPostIncrement = 63;
    public const int ParserYaccProdExpressionPostDecrement = 64;
    public const int ParserYaccProdExpressionCastPrefixed = 65;
    public const int ParserYaccProdExpressionCast = 66;
    public const int ParserYaccProdExpressionOperationMathMultiply = 67;
    public const int ParserYaccProdExpressionOperationMathDivide = 68;
    public const int ParserYaccProdExpressionOperationMathModulus = 69;
    public const int ParserYaccProdExpressionOperationMathAdd = 70;
    public const int ParserYaccProdExpressionOperationMathSubtract = 71;
    public const int ParserYaccProdExpressionOperationRelationalLessThan = 72;
    public const int ParserYaccProdExpressionOperationRelationalGreaterThan = 73;
    public const int ParserYaccProdExpressionOperationRelationalLessThanEquals = 74;
    public const int ParserYaccProdExpressionOperationRelationalGreaterThanEquals = 75;
    public const int ParserYaccProdExpressionOperationIs = 76;
    public const int ParserYaccProdExpressionOperationAs = 77;
    public const int ParserYaccProdExpressionOperationRelationalEquals = 78;
    public const int ParserYaccProdExpressionOperationRelationalNotEquals = 79;
    public const int ParserYaccProdExpressionOperationLogicalAnd = 80;
    public const int ParserYaccProdExpressionOperationLogicalOr = 81;
    public const int ParserYaccProdExpressionNullCoalescing = 82;
    public const int ParserYaccProdExpressionTernary = 83;
    public const int ParserYaccProdExpressionAssignment = 84;
    public const int ParserYaccProdParametersEmpty = 85;
    public const int ParserYaccProdParameters = 86;
    public const int ParserYaccProdParameterListSingle = 87;
    public const int ParserYaccProdParameterListMulti = 88;
    public const int ParserYaccProdTypeIdentifierNamespaced = 89;
    public const int ParserYaccProdTypeIdentifier = 90;

    public ParserYaccClass(SSYaccTable q_table, SSLex q_lex)
      : base(q_table, q_lex)
    {
    }

    public override SSYaccStackElement reduce(int q_prod, int q_size)
    {
        switch (q_prod)
        {
            case 1:
                return this.ReturnObject((object)new ValidateCode(this.Owner, new ValidateStatementCompound(this.Owner, (ValidateStatement)this.FromProduction(1), this.CurrentLine, this.CurrentColumn), this.CurrentLine, this.CurrentColumn));
            case 2:
                return this.ReturnObject(this.FromProduction(2));
            case 3:
                return this.ReturnObject(this.FromProduction(1));
            case 4:
                return this.ReturnObject((object)new ValidateMethodList(this.Owner, this.CurrentLine, this.CurrentColumn));
            case 5:
                ValidateMethodList validateMethodList = (ValidateMethodList)this.FromProduction(0);
                ValidateMethod expression1 = (ValidateMethod)this.FromProduction(1);
                validateMethodList.AppendToEnd(expression1);
                return this.ReturnObject((object)validateMethodList);
            case 6:
                return this.ReturnObject((object)new Vector<MethodSpecifier>());
            case 7:
                Vector<MethodSpecifier> vector = (Vector<MethodSpecifier>)this.FromProduction(0);
                MethodSpecifier methodSpecifier = (MethodSpecifier)this.FromProduction(1);
                vector.Add(methodSpecifier);
                return this.ReturnObject((object)vector);
            case 8:
                return this.ReturnObject((object)MethodSpecifier.Virtual);
            case 9:
                return this.ReturnObject((object)MethodSpecifier.Override);
            case 10:
                return this.ReturnObject((object)this.ConstructValidateMethod(true));
            case 11:
                return this.ReturnObject((object)new ValidateParameterDefinitionList(this.Owner, this.CurrentLine, this.CurrentColumn));
            case 12:
                return this.ReturnObject((object)(ValidateParameterDefinitionList)this.FromProduction(0));
            case 13:
                ValidateParameterDefinition expression2 = (ValidateParameterDefinition)this.FromProduction(0);
                ValidateParameterDefinitionList parameterDefinitionList1 = new ValidateParameterDefinitionList(this.Owner, expression2.Line, expression2.Column);
                parameterDefinitionList1.AppendToEnd(expression2);
                return this.ReturnObject((object)parameterDefinitionList1);
            case 14:
                ValidateParameterDefinitionList parameterDefinitionList2 = (ValidateParameterDefinitionList)this.FromProduction(0);
                ValidateParameterDefinition expression3 = (ValidateParameterDefinition)this.FromProduction(2);
                parameterDefinitionList2.AppendToEnd(expression3);
                return this.ReturnObject((object)parameterDefinitionList2);
            case 15:
                ValidateTypeIdentifier typeIdentifier1 = (ValidateTypeIdentifier)this.FromProduction(0);
                string name = this.FromTerminal(1);
                return this.ReturnObject((object)new ValidateParameterDefinition(this.Owner, this.Line(1), this.Column(1), name, typeIdentifier1));
            case 16:
                return this.ReturnObject((object)new ValidateExpressionList(this.Owner, this.CurrentLine, this.CurrentColumn));
            case 17:
                return this.ReturnObject(this.FromProduction(0));
            case 18:
                ValidateExpression expression4 = (ValidateExpression)this.FromProduction(0);
                ValidateExpressionList validateExpressionList1 = new ValidateExpressionList(this.Owner, expression4.Line, expression4.Column);
                validateExpressionList1.AppendToEnd(expression4);
                return this.ReturnObject((object)validateExpressionList1);
            case 19:
                ValidateExpressionList validateExpressionList2 = (ValidateExpressionList)this.FromProduction(0);
                ValidateExpression expression5 = (ValidateExpression)this.FromProduction(2);
                validateExpressionList2.AppendToEnd(expression5);
                return this.ReturnObject((object)validateExpressionList2);
            case 20:
                ValidateTypeIdentifier typeIdentifier2 = (ValidateTypeIdentifier)this.FromProduction(0);
                return this.ReturnObject((object)new ValidateStatementScopedLocal(this.Owner, this.FromTerminal(1), typeIdentifier2, this.Line(1), this.Column(1)));
            case 21:
                ValidateTypeIdentifier typeIdentifier3 = (ValidateTypeIdentifier)this.FromProduction(0);
                string str = this.FromTerminal(1);
                return this.ReturnObject((object)new ValidateStatementAssignment(this.Owner, new ValidateStatementScopedLocal(this.Owner, str, typeIdentifier3, this.Line(1), this.Column(1)), (ValidateExpression)new ValidateExpressionSymbol(this.Owner, str, this.Line(1), this.Column(1)), (ValidateExpression)this.FromProduction(3), this.Line(1), this.Column(1)));
            case 22:
                return this.ReturnObject(this.FromProduction(0));
            case 23:
                ValidateExpressionList validateExpressionList3 = (ValidateExpressionList)this.FromProduction(0);
                return this.ReturnObject((object)new ValidateStatementExpression(this.Owner, (ValidateExpression)validateExpressionList3, validateExpressionList3.Line, validateExpressionList3.Column));
            case 24:
                return this.ReturnObject((object)new ValidateStatementIf(this.Owner, (ValidateExpression)this.FromProduction(2), ValidateStatementCompound.Encapsulate((ValidateStatement)this.FromProduction(4)), this.Line(0), this.Column(0)));
            case 25:
                ValidateExpression condition1 = (ValidateExpression)this.FromProduction(2);
                ValidateStatement statement1 = (ValidateStatement)this.FromProduction(4);
                ValidateStatement statement2 = (ValidateStatement)this.FromProduction(6);
                ValidateStatementCompound statementCompoundTrue = ValidateStatementCompound.Encapsulate(statement1);
                ValidateStatementCompound statementCompoundFalse = ValidateStatementCompound.Encapsulate(statement2);
                return this.ReturnObject((object)new ValidateStatementIfElse(this.Owner, condition1, statementCompoundTrue, statementCompoundFalse, this.Line(0), this.Column(0)));
            case 26:
                ValidateTypeIdentifier typeIdentifier4 = (ValidateTypeIdentifier)this.FromProduction(2);
                return this.ReturnObject((object)new ValidateStatementForEach(this.Owner, new ValidateStatementScopedLocal(this.Owner, this.FromTerminal(3), typeIdentifier4, this.Line(3), this.Column(3)), (ValidateExpression)this.FromProduction(5), ValidateStatementCompound.Encapsulate((ValidateStatement)this.FromProduction(7)), this.Line(0), this.Column(0)));
            case 27:
                ValidateExpression condition2 = (ValidateExpression)this.FromProduction(2);
                return this.ReturnObject((object)new ValidateStatementWhile(this.Owner, (ValidateStatement)this.FromProduction(4), condition2, false, this.Line(0), this.Column(0)));
            case 28:
                return this.ReturnObject((object)new ValidateStatementWhile(this.Owner, (ValidateStatement)this.FromProduction(1), (ValidateExpression)this.FromProduction(4), true, this.Line(0), this.Column(0)));
            case 29:
                ValidateStatement statementList1 = (ValidateStatement)this.FromProduction(2);
                ValidateExpression condition3 = (ValidateExpression)this.FromProduction(4);
                ValidateExpression expression6 = (ValidateExpression)this.FromProduction(6);
                ValidateStatement statementList2 = (ValidateStatement)this.FromProduction(8);
                ValidateStatementExpression statementExpression = new ValidateStatementExpression(this.Owner, expression6, expression6.Line, expression6.Column);
                statementList2.AppendToEnd((ValidateStatement)statementExpression);
                ValidateStatementWhile validateStatementWhile = new ValidateStatementWhile(this.Owner, (ValidateStatement)new ValidateStatementCompound(this.Owner, statementList2, statementList2.Line, statementList2.Column), condition3, false, this.Line(0), this.Column(0));
                statementList1.AppendToEnd((ValidateStatement)validateStatementWhile);
                return this.ReturnObject((object)new ValidateStatementCompound(this.Owner, statementList1, this.Line(0), this.Column(0)));
            case 30:
                return this.ReturnObject(this.FromProduction(0));
            case 31:
                ValidateExpression expression7 = (ValidateExpression)this.FromProduction(0);
                return this.ReturnObject((object)new ValidateStatementExpression(this.Owner, expression7, expression7.Line, expression7.Column));
            case 32:
                return this.ReturnObject((object)new ValidateStatementReturn(this.Owner, (ValidateExpression)null, this.Line(0), this.Column(0)));
            case 33:
                return this.ReturnObject((object)new ValidateStatementReturn(this.Owner, (ValidateExpression)this.FromProduction(1), this.Line(0), this.Column(0)));
            case 34:
                return this.ReturnObject((object)new ValidateStatementBreak(this.Owner, false, this.Line(0), this.Column(0)));
            case 35:
                return this.ReturnObject((object)new ValidateStatementBreak(this.Owner, true, this.Line(0), this.Column(0)));
            case 36:
                return this.ReturnObject((object)new ValidateStatementAttribute(this.Owner, this.FromTerminal(1), (ValidateParameter)this.FromProduction(3), this.Line(1), this.Column(1)));
            case 37:
                return this.ReturnObject((object)new ValidateStatementCompound(this.Owner, (ValidateStatement)this.FromProduction(1), this.Line(0), this.Column(0)));
            case 38:
                ValidateStatement validateStatement1 = (ValidateStatement)this.FromProduction(0);
                ValidateStatement validateStatement2 = (ValidateStatement)this.FromProduction(1);
                if (validateStatement1 != null)
                    validateStatement1.AppendToEnd(validateStatement2);
                else
                    validateStatement1 = validateStatement2;
                return this.ReturnObject((object)validateStatement1);
            case 39:
                return this.ReturnObject((object)null);
            case 40:
                return this.ReturnObject((object)new ValidateExpressionCall(this.Owner, (ValidateExpression)this.FromProduction(0), this.FromTerminal(2), (ValidateParameter)this.FromProduction(4), this.Line(2), this.Column(2)));
            case 41:
                string memberName = this.FromTerminal(0);
                ValidateParameter parameterList = (ValidateParameter)this.FromProduction(2);
                return this.ReturnObject((object)new ValidateExpressionCall(this.Owner, (ValidateExpression)new ValidateExpressionThis(this.Owner, this.Line(0), this.Column(0)), memberName, parameterList, this.Line(0), this.Column(0)));
            case 42:
                return this.ReturnObject((object)new ValidateExpressionCall(this.Owner, (ValidateExpression)this.FromProduction(0), this.FromTerminal(2), (ValidateParameter)null, this.Line(2), this.Column(2)));
            case 43:
                return this.ReturnObject((object)new ValidateExpressionCall(this.Owner, new ValidateTypeIdentifier(this.Owner, this.FromTerminal(0), this.FromTerminal(2), this.Line(0), this.Column(0)), this.FromTerminal(4), (ValidateParameter)null, this.Line(4), this.Column(4)));
            case 44:
                return this.ReturnObject((object)new ValidateExpressionCall(this.Owner, new ValidateTypeIdentifier(this.Owner, this.FromTerminal(0), this.FromTerminal(2), this.Line(0), this.Column(0)), this.FromTerminal(4), (ValidateParameter)this.FromProduction(6), this.Line(4), this.Column(4)));
            case 45:
                return this.ReturnObject((object)new ValidateExpressionIndex(this.Owner, (ValidateExpression)this.FromProduction(0), (ValidateParameter)this.FromProduction(2), this.Line(1), this.Column(1)));
            case 46:
                return this.ReturnObject((object)new ValidateExpressionNew(this.Owner, (ValidateTypeIdentifier)this.FromProduction(1), (ValidateParameter)this.FromProduction(3), this.Line(0), this.Column(0)));
            case 47:
                return this.ReturnObject((object)new ValidateExpressionSymbol(this.Owner, this.FromTerminal(0), this.Line(0), this.Column(0)));
            case 48:
                return this.ReturnObject((object)new ValidateExpressionConstant(this.Owner, this.FromTerminalTrim(0, 1, 1), ConstantType.String, this.Line(0), this.Column(0)));
            case 49:
                return this.ReturnObject((object)new ValidateExpressionConstant(this.Owner, this.FromTerminalTrim(0, 2, 1), ConstantType.StringLiteral, this.Line(0), this.Column(0)));
            case 50:
                return this.ReturnObject((object)new ValidateExpressionConstant(this.Owner, this.FromTerminal(0), ConstantType.Integer, this.Line(0), this.Column(0)));
            case 51:
                return this.ReturnObject((object)new ValidateExpressionConstant(this.Owner, this.FromTerminalTrim(0, 0, 1), ConstantType.LongInteger, this.Line(0), this.Column(0)));
            case 52:
                return this.ReturnObject((object)new ValidateExpressionConstant(this.Owner, this.FromTerminal(0), ConstantType.Float, this.Line(0), this.Column(0)));
            case 53:
                return this.ReturnObject((object)new ValidateExpressionConstant(this.Owner, true, this.Line(0), this.Column(0)));
            case 54:
                return this.ReturnObject((object)new ValidateExpressionConstant(this.Owner, false, this.Line(0), this.Column(0)));
            case 55:
                return this.ReturnObject((object)new ValidateExpressionConstant(this.Owner, (string)null, ConstantType.Null, this.Line(0), this.Column(0)));
            case 56:
                return this.ReturnObject((object)new ValidateExpressionThis(this.Owner, this.Line(0), this.Column(0)));
            case 57:
                return this.ReturnObject((object)new ValidateExpressionBaseClass(this.Owner, this.Line(0), this.Column(0)));
            case 58:
                return this.ReturnObject((object)new ValidateExpressionTypeOf(this.Owner, (ValidateTypeIdentifier)this.FromProduction(2), this.Line(0), this.Column(0)));
            case 59:
                return this.ReturnObject((object)(ValidateExpression)this.FromProduction(1));
            case 60:
                return this.ReturnObject((object)new ValidateExpressionDeclareTrigger(this.Owner, (ValidateExpression)this.FromProduction(1), this.Line(0), this.Column(0)));
            case 61:
                return this.ReturnObject((object)this.ConstructValidateExpressionUnaryOperation(OperationType.LogicalNot));
            case 62:
                return this.ReturnObject((object)this.ConstructValidateExpressionUnaryOperation(OperationType.MathNegate));
            case 63:
                return this.ReturnObject((object)this.ConstructValidateExpressionPostUnaryOperation(OperationType.PostIncrement));
            case 64:
                return this.ReturnObject((object)this.ConstructValidateExpressionPostUnaryOperation(OperationType.PostDecrement));
            case 65:
                string prefix = this.FromTerminal(1);
                string typeName = this.FromTerminal(3);
                ValidateExpression castee = (ValidateExpression)this.FromProduction(5);
                return this.ReturnObject((object)new ValidateExpressionCast(this.Owner, new ValidateTypeIdentifier(this.Owner, prefix, typeName, this.Line(1), this.Column(1)), castee, this.Line(0), this.Column(0)));
            case 66:
                return this.ReturnObject((object)new ValidateExpressionCast(this.Owner, (ValidateExpression)this.FromProduction(1), (ValidateExpression)this.FromProduction(3), this.Line(0), this.Column(0)));
            case 67:
                return this.ReturnObject((object)this.ConstructValidateExpressionOperation(OperationType.MathMultiply));
            case 68:
                return this.ReturnObject((object)this.ConstructValidateExpressionOperation(OperationType.MathDivide));
            case 69:
                return this.ReturnObject((object)this.ConstructValidateExpressionOperation(OperationType.MathModulus));
            case 70:
                return this.ReturnObject((object)this.ConstructValidateExpressionOperation(OperationType.MathAdd));
            case 71:
                return this.ReturnObject((object)this.ConstructValidateExpressionOperation(OperationType.MathSubtract));
            case 72:
                return this.ReturnObject((object)this.ConstructValidateExpressionOperation(OperationType.RelationalLessThan));
            case 73:
                return this.ReturnObject((object)this.ConstructValidateExpressionOperation(OperationType.RelationalGreaterThan));
            case 74:
                return this.ReturnObject((object)this.ConstructValidateExpressionOperation(OperationType.RelationalLessThanEquals));
            case 75:
                return this.ReturnObject((object)this.ConstructValidateExpressionOperation(OperationType.RelationalGreaterThanEquals));
            case 76:
                return this.ReturnObject((object)new ValidateExpressionIsCheck(this.Owner, (ValidateExpression)this.FromProduction(0), (ValidateTypeIdentifier)this.FromProduction(2), this.Line(1), this.Column(1)));
            case 77:
                return this.ReturnObject((object)new ValidateExpressionAs(this.Owner, (ValidateExpression)this.FromProduction(0), (ValidateTypeIdentifier)this.FromProduction(2), this.Line(1), this.Column(1)));
            case 78:
                return this.ReturnObject((object)this.ConstructValidateExpressionOperation(OperationType.RelationalEquals));
            case 79:
                return this.ReturnObject((object)this.ConstructValidateExpressionOperation(OperationType.RelationalNotEquals));
            case 80:
                return this.ReturnObject((object)this.ConstructValidateExpressionOperation(OperationType.LogicalAnd));
            case 81:
                return this.ReturnObject((object)this.ConstructValidateExpressionOperation(OperationType.LogicalOr));
            case 82:
                return this.ReturnObject((object)new ValidateExpressionNullCoalescing(this.Owner, (ValidateExpression)this.FromProduction(0), (ValidateExpression)this.FromProduction(2), this.Line(1), this.Column(1)));
            case 83:
                return this.ReturnObject((object)new ValidateExpressionTernary(this.Owner, (ValidateExpression)this.FromProduction(0), (ValidateExpression)this.FromProduction(2), (ValidateExpression)this.FromProduction(4), this.Line(1), this.Column(1)));
            case 84:
                return this.ReturnObject((object)new ValidateExpressionAssignment(this.Owner, (ValidateExpression)this.FromProduction(0), (ValidateExpression)this.FromProduction(2), this.Line(1), this.Column(1)));
            case 85:
                return this.ReturnObject((object)ValidateParameter.EmptyList);
            case 86:
                return this.ReturnObject((object)(ValidateParameter)this.FromProduction(0));
            case 87:
                ValidateExpression expression8 = (ValidateExpression)this.FromProduction(0);
                return this.ReturnObject((object)new ValidateParameter(this.Owner, expression8, expression8.Line, expression8.Column));
            case 88:
                ValidateParameter validateParameter1 = (ValidateParameter)this.FromProduction(0);
                ValidateExpression expression9 = (ValidateExpression)this.FromProduction(2);
                ValidateParameter validateParameter2 = new ValidateParameter(this.Owner, expression9, expression9.Line, expression9.Column);
                validateParameter1.AppendToEnd(validateParameter2);
                return this.ReturnObject((object)validateParameter1);
            case 89:
                return this.ReturnObject((object)new ValidateTypeIdentifier(this.Owner, this.FromTerminal(0), this.FromTerminal(2), this.Line(0), this.Column(0)));
            case 90:
                return this.ReturnObject((object)new ValidateTypeIdentifier(this.Owner, (string)null, this.FromTerminal(0), this.Line(0), this.Column(0)));
            default:
                return this.stackElement();
        }
    }

    private ValidateExpressionOperation ConstructValidateExpressionOperation(
      OperationType op)
    {
        ValidateExpression leftSide = (ValidateExpression)this.FromProduction(0);
        ValidateExpression rightSide = (ValidateExpression)this.FromProduction(2);
        return new ValidateExpressionOperation(this.Owner, leftSide, op, rightSide, this.Line(1), this.Column(1));
    }

    private ValidateExpressionOperation ConstructValidateExpressionUnaryOperation(
      OperationType op)
    {
        return new ValidateExpressionOperation(this.Owner, (ValidateExpression)this.FromProduction(1), op, (ValidateExpression)null, this.Line(0), this.Column(0));
    }

    private ValidateExpressionOperation ConstructValidateExpressionPostUnaryOperation(
      OperationType op)
    {
        return new ValidateExpressionOperation(this.Owner, (ValidateExpression)this.FromProduction(0), op, (ValidateExpression)null, this.Line(1), this.Column(1));
    }

    private ValidateMethod ConstructValidateMethod(bool hasBody)
    {
        string methodName = this.FromTerminal(2);
        ValidateTypeIdentifier returnType = (ValidateTypeIdentifier)this.FromProduction(1);
        Vector<MethodSpecifier> specifiers = (Vector<MethodSpecifier>)this.FromProduction(0);
        ValidateParameterDefinitionList paramList = (ValidateParameterDefinitionList)this.FromProduction(4);
        ValidateCode body = new ValidateCode(this.Owner, new ValidateStatementCompound(this.Owner, hasBody ? (ValidateStatement)this.FromProduction(7) : (ValidateStatement)null, this.Line(6), this.Column(6)), this.Line(6), this.Column(6));
        return new ValidateMethod(this.Owner, this.Line(2), this.Column(2), methodName, returnType, specifiers, paramList, body);
    }

    private int CurrentLine => this.m_lex.consumer().line();

    private int CurrentColumn => this.m_lex.consumer().offset();
}
