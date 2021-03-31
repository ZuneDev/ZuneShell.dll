// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Parser
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Markup.Validation;
using Microsoft.Iris.OS;
using Microsoft.Iris.Session;
using SSVParseLib;
using System.Collections;

namespace Microsoft.Iris.Markup
{
    internal class Parser
    {
        private static ParserLexClass s_lex;
        private static ParserYaccClass s_yacc;
        private static SSLexTable s_lexTable;
        private static SSYaccTable s_yaccTable;
        private static bool s_parserActive;

        public static ParseResult Invoke(SourceMarkupLoader owner, Resource resource)
        {
            ErrorWatermark watermark = ErrorManager.Watermark;
            ParseResult parseResult = new ParseResult();
            Stack parseStack = new Stack();
            bool flag = false;
            bool additionalMetadata = MarkupSystem.TrackAdditionalMetadata;
            using (NativeXmlReader xmlReader = new NativeXmlReader(resource))
            {
                try
                {
                    string str1 = string.Empty;
                    NativeXmlNodeType nodeType;
                    while (xmlReader.Read(out nodeType))
                    {
                        if (parseResult.Root == null || parseStack.Count == 0 && nodeType == NativeXmlNodeType.EndElement)
                        {
                            if (nodeType == NativeXmlNodeType.Element)
                            {
                                parseResult.Root = xmlReader.Name;
                                while (xmlReader.ReadAttribute())
                                {
                                    string name = xmlReader.Name;
                                    string prefix = xmlReader.Prefix;
                                    if (name == "xmlns")
                                        parseResult.Version = xmlReader.Value;
                                    else if (prefix == "xmlns")
                                    {
                                        ValidateNamespace validateNamespace = new ValidateNamespace(owner, xmlReader.LocalName, xmlReader.Value, xmlReader.LineNumber, xmlReader.LinePosition);
                                        if (parseResult.XmlnsList == null)
                                            parseResult.XmlnsList = validateNamespace;
                                        else
                                            parseResult.XmlnsList.AppendToEnd(validateNamespace);
                                    }
                                    else
                                        Parser.ReportError(xmlReader, "Unexpected attribute '{0}' on root tag", (object)name);
                                }
                            }
                        }
                        else
                        {
                            switch (nodeType)
                            {
                                case NativeXmlNodeType.Element:
                                    if (flag)
                                    {
                                        Parser.ReportError(xmlReader, "Script tag may not contain XML elements, found: '{0}'", (object)xmlReader.Name);
                                        continue;
                                    }
                                    bool isEmptyElement = xmlReader.IsEmptyElement;
                                    string prefix1 = xmlReader.Prefix;
                                    string localName = xmlReader.LocalName;
                                    if (parseStack.Count % 2 == 0)
                                    {
                                        if (prefix1 == "" && localName == "Script")
                                        {
                                            flag = true;
                                            parseStack.Push((object)new Parser.ScriptBlock(xmlReader.LineNumber, xmlReader.LinePosition));
                                            if (xmlReader.ReadAttribute())
                                                Parser.ReportError(xmlReader, "Script tag may not have XML attributes");
                                        }
                                        else
                                        {
                                            ValidateTypeIdentifier typeIdentifier = new ValidateTypeIdentifier(owner, prefix1, localName, xmlReader.LineNumber, xmlReader.LinePosition);
                                            bool isRootTag = parseStack.Count == 0;
                                            ValidateObjectTag objectTagValidator = owner.CreateObjectTagValidator(typeIdentifier, xmlReader.LineNumber, xmlReader.LinePosition, isRootTag);
                                            while (xmlReader.ReadAttribute())
                                            {
                                                if (xmlReader.Prefix == string.Empty)
                                                {
                                                    ValidateObject objectFromString = Parser.CreateValidateObjectFromString(owner, xmlReader);
                                                    ValidateProperty property = new ValidateProperty(owner, xmlReader.LocalName, objectFromString, xmlReader.LineNumber, xmlReader.LinePosition);
                                                    objectTagValidator.AddProperty(property);
                                                }
                                                else
                                                    Parser.ReportError(xmlReader, "Property or Object tag may not have prefixed attributes: '{0}'", (object)xmlReader.Name);
                                            }
                                            parseStack.Push((object)objectTagValidator);
                                            if (additionalMetadata && !string.IsNullOrEmpty(str1))
                                            {
                                                objectTagValidator.Metadata.Comments = str1;
                                                str1 = string.Empty;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (prefix1 != string.Empty)
                                            Parser.ReportError(xmlReader, "Property tag may not be prefixed: '{0}'", (object)xmlReader.Name);
                                        if (localName == "Methods")
                                        {
                                            flag = true;
                                            parseStack.Push((object)new Parser.ScriptBlock(xmlReader.LineNumber, xmlReader.LinePosition, Parser.CodeType.Methods));
                                            if (xmlReader.ReadAttribute())
                                                Parser.ReportError(xmlReader, "Script tag may not have XML attributes");
                                        }
                                        else
                                        {
                                            ValidateProperty validateProperty = new ValidateProperty(owner, localName, xmlReader.LineNumber, xmlReader.LinePosition);
                                            while (xmlReader.ReadAttribute())
                                            {
                                                if (xmlReader.Prefix == string.Empty)
                                                    validateProperty.AddAttribute(new PropertyAttribute()
                                                    {
                                                        Name = xmlReader.LocalName,
                                                        Value = xmlReader.Value
                                                    });
                                                else
                                                    Parser.ReportError(xmlReader, "Property or Object tag may not have prefixed attributes: '{0}'", (object)xmlReader.Name);
                                            }
                                            parseStack.Push((object)validateProperty);
                                        }
                                    }
                                    if (isEmptyElement)
                                    {
                                        flag = false;
                                        Parser.HandleEndElement(owner, parseResult, parseStack);
                                        continue;
                                    }
                                    continue;
                                case NativeXmlNodeType.Text:
                                case NativeXmlNodeType.CDATA:
                                    if (parseStack.Count == 0)
                                    {
                                        Parser.ReportError(xmlReader, "Text/CDATA is not allowed under root tag ('{0}')", (object)xmlReader.Value.Trim());
                                        continue;
                                    }
                                    switch (parseStack.Peek())
                                    {
                                        case ValidateProperty validateProperty:
                                            if (validateProperty.Value == null)
                                            {
                                                validateProperty.Value = Parser.CreateValidateObjectFromString(owner, xmlReader);
                                                continue;
                                            }
                                            if (validateProperty.Value is ValidateFromString)
                                            {
                                                Parser.ReportError(xmlReader, "Property tag may only contain one Text/CDATA, found more text: '{0}'", (object)xmlReader.Value.Trim());
                                                continue;
                                            }
                                            Parser.ReportError(xmlReader, "Property tag already has objects, may not also add Text/CDATA: '{0}'", (object)xmlReader.Value.Trim());
                                            continue;
                                        case Parser.ScriptBlock scriptBlock:
                                            if (scriptBlock.ValidateValue == null)
                                            {
                                                scriptBlock.ValidateValue = Parser.ParseCode(owner, xmlReader, scriptBlock.CodeType);
                                                continue;
                                            }
                                            Parser.ReportError(xmlReader, "Script tag may only contain one Text/CDATA, found more text: '{0}'", (object)xmlReader.Value.Trim());
                                            continue;
                                        default:
                                            Parser.ReportError(xmlReader, "Object tag may not contain Text/CDATA: '{0}'", (object)xmlReader.Value.Trim());
                                            continue;
                                    }
                                case NativeXmlNodeType.Comment:
                                    string str2 = (string)null;
                                    if (additionalMetadata)
                                    {
                                        str2 = xmlReader.Value;
                                        str1 = str1 + (object)'\n' + str2;
                                    }
                                    if (flag)
                                    {
                                        if (str2 == null)
                                            str2 = xmlReader.Value;
                                        Parser.ReportError(xmlReader, "Script tag may not contain XML comments, found: '{0}'", (object)str2.Trim());
                                        continue;
                                    }
                                    continue;
                                case NativeXmlNodeType.EndElement:
                                    flag = false;
                                    Parser.HandleEndElement(owner, parseResult, parseStack);
                                    continue;
                                default:
                                    continue;
                            }
                        }
                    }
                }
                catch (NativeXmlException ex)
                {
                    ErrorManager.ReportError(ex.LineNumber, ex.LinePosition, ex.Message);
                }
            }
            if (watermark.ErrorsDetected)
            {
                parseResult = new ParseResult();
                parseResult.HasErrors = true;
            }
            return parseResult;
        }

        private static void HandleEndElement(
          SourceMarkupLoader owner,
          ParseResult parseResult,
          Stack parseStack)
        {
            if (parseStack.Count % 2 == 1)
            {
                object obj = parseStack.Pop();
                bool flag = false;
                ValidateObject validateObject;
                // FIXME
                ScriptBlock scriptBlock = null;
                if (obj is Parser.ScriptBlock)
                {
                    scriptBlock = (Parser.ScriptBlock)obj;
                    validateObject = (ValidateObject)scriptBlock.ValidateValue;
                }
                else
                {
                    validateObject = (ValidateObject)obj;
                    flag = true;
                }
                if (parseStack.Count == 0)
                {
                    switch (validateObject)
                    {
                        case ValidateClass validateClass:
                            parseResult.ClassList.Add(validateClass);
                            break;
                        case ValidateAlias validateAlias:
                            if (parseResult.AliasList == ParseResult.s_EmptyAliasList)
                                parseResult.AliasList = new Vector<ValidateAlias>(4);
                            parseResult.AliasList.Add(validateAlias);
                            break;
                        case ValidateDataMapping validateDataMapping:
                            if (parseResult.DataMappingList == ParseResult.s_EmptyDataMappingList)
                                parseResult.DataMappingList = new Vector<ValidateDataMapping>(4);
                            parseResult.DataMappingList.Add(validateDataMapping);
                            break;
                        default:
                            if (flag)
                            {
                                ValidateObjectTag validateObjectTag = (ValidateObjectTag)validateObject;
                                ErrorManager.ReportError(validateObjectTag.Line, validateObjectTag.Column, "Unexpected root element '{0}', must be <Class>, <UI> or <Alias>", (object)validateObjectTag.TypeIdentifier.TypeName);
                                break;
                            }
                            ErrorManager.ReportError(scriptBlock.Line, scriptBlock.Column, "Unexpected root element '{0}', must be <Class>, <UI> or <Alias>", (object)"Script");
                            break;
                    }
                }
                else if (validateObject != null)
                    ((ValidateProperty)parseStack.Peek()).AddValue(validateObject);
                if (!flag)
                    return;
                ((ValidateObjectTag)validateObject).NotifyParseComplete();
            }
            else
            {
                object obj = parseStack.Pop();
                ValidateProperty property = obj as ValidateProperty;
                ValidateObjectTag validateObjectTag = (ValidateObjectTag)parseStack.Peek();
                if (property != null)
                {
                    validateObjectTag.AddProperty(property);
                }
                else
                {
                    Parser.ScriptBlock scriptBlock = (Parser.ScriptBlock)obj;
                    if (validateObjectTag is ValidateClass validateClass)
                        validateClass.NotifyFoundMethodList((ValidateMethodList)scriptBlock.ValidateValue);
                    else
                        ErrorManager.ReportError(scriptBlock.ValidateValue.Line, scriptBlock.ValidateValue.Column, "Methods allowed only on <Class>, <UI> and <Effect>");
                }
            }
        }

        private static ValidateObject CreateValidateObjectFromString(
          SourceMarkupLoader owner,
          NativeXmlReader xmlReader)
        {
            if (xmlReader.IsInlineExpression)
                return (ValidateObject)Parser.ParseCode(owner, xmlReader, Parser.CodeType.InlineExpression);
            string fromString = xmlReader.Value;
            bool expandEscapes = true;
            if (fromString.Length > 0 && fromString[0] == '@')
            {
                fromString = fromString.Substring(1);
                expandEscapes = false;
            }
            return (ValidateObject)new ValidateFromString(owner, fromString, expandEscapes, xmlReader.LineNumber, xmlReader.LinePosition);
        }

        private static Validate ParseCode(
          SourceMarkupLoader owner,
          NativeXmlReader xmlReader,
          Parser.CodeType codeType)
        {
            Validate validate = (Validate)null;
            if (Parser.s_lexTable == null)
            {
                Parser.s_lexTable = (SSLexTable)new ParserLexTable();
                Parser.s_yaccTable = (SSYaccTable)new ParserYaccTable();
                Parser.s_lex = new ParserLexClass(Parser.s_lexTable);
                Parser.s_yacc = new ParserYaccClass(Parser.s_yaccTable, (SSLex)Parser.s_lex);
            }
            string prefix;
            switch (codeType)
            {
                case Parser.CodeType.Methods:
                    prefix = "%%";
                    break;
                case Parser.CodeType.InlineExpression:
                    prefix = "$$";
                    break;
                default:
                    prefix = "@@";
                    break;
            }
            SSLexUnicodeBufferConsumer unicodeBufferConsumer = xmlReader.LexConsumerForValueWithPrefix(prefix);
            Parser.s_lex.Reset((SSLexConsumer)unicodeBufferConsumer);
            Parser.s_yacc.Reset(owner);
            Parser.s_parserActive = true;
            Parser.s_yacc.parse();
            if (!Parser.s_yacc.HasErrors)
            {
                validate = (Validate)Parser.s_yacc.treeRoot().Object;
                if (MarkupSystem.TrackAdditionalMetadata)
                    validate.Metadata.OriginalValue = (object)xmlReader.Value;
            }
            Parser.s_lex.Reset((SSLexConsumer)null);
            Parser.s_yacc.Reset((SourceMarkupLoader)null);
            Parser.s_parserActive = false;
            return validate;
        }

        private static void ReportError(NativeXmlReader xmlReader, string message) => ErrorManager.ReportError(xmlReader.LineNumber, xmlReader.LinePosition, message);

        private static void ReportError(NativeXmlReader xmlReader, string message, object param) => ErrorManager.ReportError(xmlReader.LineNumber, xmlReader.LinePosition, message, param);

        private enum CodeType
        {
            ScriptBlock,
            Methods,
            InlineExpression,
        }

        private class ScriptBlock
        {
            public Validate ValidateValue;
            public int Line;
            public int Column;
            public Parser.CodeType CodeType;

            public ScriptBlock(int line, int column)
            {
                this.Line = line;
                this.Column = column;
            }

            public ScriptBlock(int line, int column, Parser.CodeType codeType)
              : this(line, column)
              => this.CodeType = codeType;
        }
    }
}
