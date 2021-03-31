using System;
using System.Collections;
using Microsoft.Iris.Debug;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup.UIX;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup
{
    // Token: 0x0200018B RID: 395
    internal class Interpreter
    {
        // Token: 0x06000F21 RID: 3873 RVA: 0x00029E78 File Offset: 0x00028E78
        public static object Run(InterpreterContext context)
        {
            object result = null;
            ByteCodeReader byteCodeReader = null;
            long num = -1L;
            bool flag = true;
            ErrorManager.EnterContext(context);
            try
            {
                byteCodeReader = context.LoadResult.ObjectSection;
                num = (long)((ulong)byteCodeReader.CurrentOffset);
                byteCodeReader.CurrentOffset = context.InitialBytecodeOffset;
                result = Interpreter.Run(context, byteCodeReader);
                flag = false;
            }
            finally
            {
                if (flag)
                {
                    Interpreter.ExceptionContext = context.ToString();
                }
                ErrorManager.ExitContext();
                if (byteCodeReader != null && num != -1L)
                {
                    byteCodeReader.CurrentOffset = (uint)num;
                }
            }
            return result;
        }

        // Token: 0x06000F22 RID: 3874 RVA: 0x00029EF8 File Offset: 0x00028EF8
        private static object Run(InterpreterContext context, ByteCodeReader reader)
        {
            MarkupLoadResult loadResult = context.LoadResult;
            IMarkupTypeBase instance = context.Instance;
            MarkupImportTables importTables = loadResult.ImportTables;
            MarkupConstantsTable constantsTable = loadResult.ConstantsTable;
            SymbolReference[] symbolReferenceTable = context.MarkupType.SymbolReferenceTable;
            Trace.IsCategoryEnabled(TraceCategory.Markup);
            Stack stack = Interpreter._stack;
            int count = stack.Count;
            if (instance != null)
            {
                stack.Push(instance);
            }
            ErrorWatermark watermark = ErrorManager.Watermark;
            bool flag = false;
            object result = null;
            bool wasInDebugState = false;
            while (!flag)
            {
                OpCode opCode = (OpCode)reader.ReadByte();
                switch (opCode)
                {
                    case OpCode.ConstructObject:
                        {
                            int num = (int)reader.ReadUInt16();
                            TypeSchema typeSchema = importTables.TypeImports[num];
                            object obj = typeSchema.ConstructDefault();
                            Interpreter.ReportErrorOnNull(obj, "Construction", typeSchema.Name);
                            if (!Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                            {
                                Interpreter.RegisterDisposable(obj, typeSchema, instance);
                                stack.Push(obj);
                            }
                            break;
                        }
                    case OpCode.ConstructObjectIndirect:
                        {
                            int num2 = (int)reader.ReadUInt16();
                            TypeSchema typeSchema2 = importTables.TypeImports[num2];
                            TypeSchema typeSchema3 = (TypeSchema)stack.Pop();
                            if (!typeSchema2.IsAssignableFrom(typeSchema3))
                            {
                                ErrorManager.ReportError("Script runtime failure: Dynamic construction type override failed. Attempting to construct '{0}' in place of '{1}'", (typeSchema3 != null) ? typeSchema3.Name : "null", typeSchema2.Name);
                                if (Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                                {
                                    break;
                                }
                            }
                            IDynamicConstructionSchema dynamicConstructionSchema = typeSchema3 as IDynamicConstructionSchema;
                            object obj2;
                            if (dynamicConstructionSchema != null)
                            {
                                obj2 = dynamicConstructionSchema.ConstructDefault(typeSchema2);
                            }
                            else
                            {
                                obj2 = typeSchema3.ConstructDefault();
                            }
                            Interpreter.ReportErrorOnNull(obj2, "Construction", typeSchema3.Name);
                            if (!Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                            {
                                Interpreter.RegisterDisposable(obj2, typeSchema3, instance);
                                stack.Push(obj2);
                            }
                            break;
                        }
                    case OpCode.ConstructObjectParam:
                        {
                            int num3 = (int)reader.ReadUInt16();
                            TypeSchema typeSchema4 = importTables.TypeImports[num3];
                            int num4 = (int)reader.ReadUInt16();
                            ConstructorSchema constructorSchema = importTables.ConstructorImports[num4];
                            int i = constructorSchema.ParameterTypes.Length;
                            object[] array = Interpreter.ParameterListAllocator.Alloc(i);
                            for (i--; i >= 0; i--)
                            {
                                array[i] = stack.Pop();
                            }
                            object obj3 = constructorSchema.Construct(array);
                            Interpreter.ReportErrorOnNull(obj3, "Construction", typeSchema4.Name);
                            if (!Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                            {
                                Interpreter.RegisterDisposable(obj3, typeSchema4, instance);
                                stack.Push(obj3);
                                Interpreter.ParameterListAllocator.Free(array);
                            }
                            break;
                        }
                    case OpCode.ConstructFromString:
                        {
                            int num5 = (int)reader.ReadUInt16();
                            TypeSchema typeSchema5 = importTables.TypeImports[num5];
                            int index = (int)reader.ReadUInt16();
                            string from = (string)constantsTable.Get(index);
                            object obj4;
                            typeSchema5.TypeConverter(from, StringSchema.Type, out obj4);
                            Interpreter.ReportErrorOnNull(obj4, "Construction", typeSchema5.Name);
                            if (!Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                            {
                                Interpreter.RegisterDisposable(obj4, typeSchema5, instance);
                                stack.Push(obj4);
                            }
                            break;
                        }
                    case OpCode.ConstructFromBinary:
                        {
                            int num6 = (int)reader.ReadUInt16();
                            TypeSchema typeSchema6 = importTables.TypeImports[num6];
                            object obj5 = typeSchema6.DecodeBinary(reader);
                            Interpreter.ReportErrorOnNull(obj5, "Construction", typeSchema6.Name);
                            if (!Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                            {
                                Interpreter.RegisterDisposable(obj5, typeSchema6, instance);
                                stack.Push(obj5);
                            }
                            break;
                        }
                    case OpCode.InitializeInstance:
                        {
                            int num7 = (int)reader.ReadUInt16();
                            TypeSchema typeSchema7 = importTables.TypeImports[num7];
                            object obj6 = stack.Pop();
                            typeSchema7.InitializeInstance(ref obj6);
                            Interpreter.ReportErrorOnNull(obj6, "Initialize", typeSchema7.Name);
                            if (!Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                            {
                                stack.Push(obj6);
                            }
                            break;
                        }
                    case OpCode.InitializeInstanceIndirect:
                        {
                            TypeSchema typeSchema8 = (TypeSchema)stack.Pop();
                            object obj7 = stack.Pop();
                            typeSchema8.InitializeInstance(ref obj7);
                            Interpreter.ReportErrorOnNull(obj7, "Initialize", typeSchema8.Name);
                            if (!Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                            {
                                stack.Push(obj7);
                            }
                            break;
                        }
                    case OpCode.LookupSymbol:
                        {
                            int num8 = (int)reader.ReadUInt16();
                            SymbolReference symbolRef = symbolReferenceTable[num8];
                            object obj8 = context.ReadSymbol(symbolRef);
                            stack.Push(obj8);
                            if (Trace.IsCategoryEnabled(TraceCategory.Markup))
                            {
                            }
                            break;
                        }
                    case OpCode.WriteSymbol:
                    case OpCode.WriteSymbolPeek:
                        {
                            object value = (opCode == OpCode.WriteSymbolPeek) ? stack.Peek() : stack.Pop();
                            int num9 = (int)reader.ReadUInt16();
                            SymbolReference symbolRef2 = symbolReferenceTable[num9];
                            context.WriteSymbol(symbolRef2, value);
                            if (Trace.IsCategoryEnabled(TraceCategory.Markup))
                            {
                            }
                            break;
                        }
                    case OpCode.ClearSymbol:
                        {
                            int num10 = (int)reader.ReadUInt16();
                            SymbolReference symbolRef3 = symbolReferenceTable[num10];
                            context.ClearSymbol(symbolRef3);
                            if (Trace.IsCategoryEnabled(TraceCategory.Markup))
                            {
                            }
                            break;
                        }
                    case OpCode.PropertyInitialize:
                    case OpCode.PropertyInitializeIndirect:
                        {
                            bool flag2 = opCode == OpCode.PropertyInitializeIndirect;
                            TypeSchema typeSchema9 = null;
                            if (flag2)
                            {
                                typeSchema9 = (TypeSchema)stack.Pop();
                            }
                            int num11 = (int)reader.ReadUInt16();
                            PropertySchema propertySchema = importTables.PropertyImports[num11];
                            object obj9 = stack.Pop();
                            object obj10 = stack.Pop();
                            Interpreter.ReportErrorOnNull(obj10, "Property Set", propertySchema.Name);
                            if (!Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                            {
                                if (flag2)
                                {
                                    PropertySchema propertySchema2 = typeSchema9.FindPropertyDeep(propertySchema.Name);
                                    if (propertySchema2 != propertySchema)
                                    {
                                        TypeSchema propertyType = propertySchema2.PropertyType;
                                        if (!propertyType.IsAssignableFrom(obj9))
                                        {
                                            string param = TypeSchema.NameFromInstance(obj9);
                                            ErrorManager.ReportError("Script runtime failure: Incompatible value for property '{0}' supplied (expecting values of type '{1}' but got '{2}') while constructing runtime replacement type '{3}' (original type '{4}')", propertySchema.Name, propertyType.Name, param, typeSchema9.Name, propertySchema.Owner.Name);
                                            result = Interpreter.ScriptError;
                                        }
                                        if (Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                                        {
                                            break;
                                        }
                                    }
                                }
                                propertySchema.SetValue(ref obj10, obj9);
                                if (!Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                                {
                                    stack.Push(obj10);
                                }
                            }
                            break;
                        }
                    case OpCode.PropertyListAdd:
                        {
                            int propertyIndex = (int)reader.ReadUInt16();
                            object value2 = stack.Pop();
                            object collection = Interpreter.GetCollection(stack.Peek(), importTables, propertyIndex);
                            Interpreter.ReportErrorOnNull(collection, "List Add");
                            if (!Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                            {
                                ((IList)collection).Add(value2);
                                if (Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                                {
                                }
                            }
                            break;
                        }
                    case OpCode.PropertyDictionaryAdd:
                        {
                            int propertyIndex2 = (int)reader.ReadUInt16();
                            int index2 = (int)reader.ReadUInt16();
                            string key = (string)constantsTable.Get(index2);
                            object value3 = stack.Pop();
                            object collection2 = Interpreter.GetCollection(stack.Peek(), importTables, propertyIndex2);
                            Interpreter.ReportErrorOnNull(collection2, "Dictionary Add");
                            if (!Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                            {
                                ((IDictionary)collection2)[key] = value3;
                                if (Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                                {
                                }
                            }
                            break;
                        }
                    case OpCode.PropertyAssign:
                    case OpCode.PropertyAssignStatic:
                        {
                            int num12 = (int)reader.ReadUInt16();
                            PropertySchema propertySchema3 = importTables.PropertyImports[num12];
                            object instance2 = null;
                            if (opCode == OpCode.PropertyAssign)
                            {
                                instance2 = stack.Pop();
                                Interpreter.ReportErrorOnNullOrDisposed(instance2, "Property Set", propertySchema3.Name, propertySchema3.Owner);
                                if (Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                                {
                                    break;
                                }
                            }
                            object value4 = stack.Peek();
                            propertySchema3.SetValue(ref instance2, value4);
                            if (!Interpreter.ErrorsDetected(watermark, ref result, ref flag) && Trace.IsCategoryEnabled(TraceCategory.Markup))
                            {
                            }
                            break;
                        }
                    case OpCode.PropertyGet:
                    case OpCode.PropertyGetPeek:
                    case OpCode.PropertyGetStatic:
                        {
                            int num13 = (int)reader.ReadUInt16();
                            PropertySchema propertySchema4 = importTables.PropertyImports[num13];
                            object instance3 = null;
                            if (opCode != OpCode.PropertyGetStatic)
                            {
                                instance3 = ((opCode == OpCode.PropertyGet) ? stack.Pop() : stack.Peek());
                                Interpreter.ReportErrorOnNullOrDisposed(instance3, "Property Get", propertySchema4.Name, propertySchema4.Owner);
                                if (Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                                {
                                    break;
                                }
                            }
                            object value5 = propertySchema4.GetValue(instance3);
                            if (!Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                            {
                                stack.Push(value5);
                                if (Trace.IsCategoryEnabled(TraceCategory.Markup))
                                {
                                }
                            }
                            break;
                        }
                    case OpCode.MethodInvoke:
                    case OpCode.MethodInvokePeek:
                    case OpCode.MethodInvokeStatic:
                    case OpCode.MethodInvokePushLastParam:
                    case OpCode.MethodInvokeStaticPushLastParam:
                        {
                            int num14 = (int)reader.ReadUInt16();
                            MethodSchema methodSchema = importTables.MethodImports[num14];
                            int j = methodSchema.ParameterTypes.Length;
                            object[] array2 = Interpreter.ParameterListAllocator.Alloc(j);
                            for (j--; j >= 0; j--)
                            {
                                array2[j] = stack.Pop();
                            }
                            object instance4 = null;
                            bool flag3 = opCode != OpCode.MethodInvokeStatic && opCode != OpCode.MethodInvokeStaticPushLastParam;
                            bool flag4 = opCode == OpCode.MethodInvokePeek;
                            bool flag5 = opCode == OpCode.MethodInvokePushLastParam || opCode == OpCode.MethodInvokeStaticPushLastParam;
                            if (flag3)
                            {
                                if (!flag4)
                                {
                                    instance4 = stack.Pop();
                                }
                                else
                                {
                                    instance4 = stack.Peek();
                                }
                                Interpreter.ReportErrorOnNullOrDisposed(instance4, "Method Invoke", methodSchema.Name, methodSchema.Owner);
                                if (Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                                {
                                    break;
                                }
                            }
                            object obj11 = methodSchema.Invoke(instance4, array2);
                            if (!Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                            {
                                if (!flag5)
                                {
                                    if (methodSchema.ReturnType != VoidSchema.Type)
                                    {
                                        stack.Push(obj11);
                                    }
                                }
                                else
                                {
                                    stack.Push(array2[array2.Length - 1]);
                                }
                                Interpreter.ParameterListAllocator.Free(array2);
                            }
                            break;
                        }
                    case OpCode.VerifyTypeCast:
                        {
                            int num15 = (int)reader.ReadUInt16();
                            TypeSchema typeSchema10 = importTables.TypeImports[num15];
                            object obj12 = stack.Peek();
                            if (obj12 != null)
                            {
                                if (!typeSchema10.IsAssignableFrom(obj12))
                                {
                                    string param2 = TypeSchema.NameFromInstance(obj12);
                                    string name = typeSchema10.Name;
                                    ErrorManager.ReportError("Script runtime failure: Invalid type cast while attempting to cast an instance with a runtime type of '{0}' to '{1}'", param2, name);
                                    result = Interpreter.ScriptError;
                                }
                                if (Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                                {
                                }
                            }
                            else if (!typeSchema10.IsNullAssignable)
                            {
                                Interpreter.ReportErrorOnNull(obj12, "Verify Type Cast", typeSchema10.Name);
                                if (Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                                {
                                }
                            }
                            break;
                        }
                    case OpCode.ConvertType:
                        {
                            int num16 = (int)reader.ReadUInt16();
                            TypeSchema typeSchema11 = importTables.TypeImports[num16];
                            int num17 = (int)reader.ReadUInt16();
                            TypeSchema fromType = importTables.TypeImports[num17];
                            object obj13 = stack.Pop();
                            Interpreter.ReportErrorOnNull(obj13, "Type Conversion", typeSchema11.Name);
                            if (!Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                            {
                                object obj14;
                                Result result2 = typeSchema11.TypeConverter(obj13, fromType, out obj14);
                                if (result2.Failed)
                                {
                                    ErrorManager.ReportError("Script runtime failure: Type conversion failed while attempting to convert to '{0}' ({1})", typeSchema11.Name, result2.Error);
                                }
                                if (!Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                                {
                                    stack.Push(obj14);
                                }
                            }
                            break;
                        }
                    case OpCode.Operation:
                        {
                            int num18 = (int)reader.ReadUInt16();
                            TypeSchema typeSchema12 = importTables.TypeImports[num18];
                            OperationType op = (OperationType)reader.ReadByte();
                            object right = null;
                            if (!TypeSchema.IsUnaryOperation(op))
                            {
                                right = stack.Pop();
                            }
                            object left = stack.Pop();
                            object obj15 = typeSchema12.PerformOperationDeep(left, right, op);
                            if (!Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                            {
                                stack.Push(obj15);
                                if (Trace.IsCategoryEnabled(TraceCategory.Markup))
                                {
                                }
                            }
                            break;
                        }
                    case OpCode.IsCheck:
                        {
                            int num19 = (int)reader.ReadUInt16();
                            TypeSchema typeSchema13 = importTables.TypeImports[num19];
                            object obj16 = stack.Pop();
                            bool value6 = false;
                            if (obj16 != null)
                            {
                                value6 = typeSchema13.IsAssignableFrom(obj16);
                            }
                            stack.Push(BooleanBoxes.Box(value6));
                            if (Trace.IsCategoryEnabled(TraceCategory.Markup))
                            {
                            }
                            break;
                        }
                    case OpCode.As:
                        {
                            int num20 = (int)reader.ReadUInt16();
                            TypeSchema typeSchema14 = importTables.TypeImports[num20];
                            object obj17 = stack.Peek();
                            if (obj17 != null && !typeSchema14.IsAssignableFrom(obj17))
                            {
                                stack.Pop();
                                stack.Push(null);
                            }
                            if (Trace.IsCategoryEnabled(TraceCategory.Markup))
                            {
                            }
                            break;
                        }
                    case OpCode.TypeOf:
                        {
                            int num21 = (int)reader.ReadUInt16();
                            TypeSchema obj18 = importTables.TypeImports[num21];
                            stack.Push(obj18);
                            break;
                        }
                    case OpCode.PushNull:
                        stack.Push(null);
                        break;
                    case OpCode.PushConstant:
                        {
                            int index3 = (int)reader.ReadUInt16();
                            object obj19 = constantsTable.Get(index3);
                            stack.Push(obj19);
                            break;
                        }
                    case OpCode.PushThis:
                        stack.Push(instance);
                        break;
                    case OpCode.DiscardValue:
                        stack.Pop();
                        break;
                    case OpCode.ReturnValue:
                        {
                            object obj20 = stack.Pop();
                            result = obj20;
                            flag = true;
                            break;
                        }
                    case OpCode.ReturnVoid:
                        result = Interpreter.VoidReturnValue;
                        flag = true;
                        break;
                    case OpCode.JumpIfFalse:
                    case OpCode.JumpIfFalsePeek:
                    case OpCode.JumpIfTruePeek:
                        {
                            uint currentOffset = reader.ReadUInt32();
                            object obj21 = (opCode == OpCode.JumpIfFalse) ? stack.Pop() : stack.Peek();
                            bool flag6 = (bool)obj21;
                            bool flag7 = opCode == OpCode.JumpIfTruePeek;
                            Trace.IsCategoryEnabled(TraceCategory.Markup);
                            if (flag7 == flag6)
                            {
                                reader.CurrentOffset = currentOffset;
                                if (Trace.IsCategoryEnabled(TraceCategory.Markup))
                                {
                                }
                            }
                            break;
                        }
                    case OpCode.JumpIfDictionaryContains:
                        {
                            ushort propertyIndex3 = reader.ReadUInt16();
                            ushort index4 = reader.ReadUInt16();
                            uint currentOffset2 = reader.ReadUInt32();
                            string key2 = (string)constantsTable.Get((int)index4);
                            object collection3 = Interpreter.GetCollection(stack.Peek(), importTables, (int)propertyIndex3);
                            Interpreter.ReportErrorOnNull(collection3, "Dictionary Contains");
                            if (!Interpreter.ErrorsDetected(watermark, ref result, ref flag))
                            {
                                bool flag8 = ((IDictionary)collection3).Contains(key2);
                                Trace.IsCategoryEnabled(TraceCategory.Markup);
                                if (flag8)
                                {
                                    reader.CurrentOffset = currentOffset2;
                                    if (Trace.IsCategoryEnabled(TraceCategory.Markup))
                                    {
                                    }
                                }
                            }
                            break;
                        }
                    case OpCode.JumpIfNullPeek:
                        {
                            uint currentOffset3 = reader.ReadUInt32();
                            object obj22 = stack.Peek();
                            Trace.IsCategoryEnabled(TraceCategory.Markup);
                            if (obj22 == null)
                            {
                                reader.CurrentOffset = currentOffset3;
                                if (Trace.IsCategoryEnabled(TraceCategory.Markup))
                                {
                                }
                            }
                            break;
                        }
                    case OpCode.Jump:
                        {
                            uint currentOffset4 = reader.ReadUInt32();
                            reader.CurrentOffset = currentOffset4;
                            if (Trace.IsCategoryEnabled(TraceCategory.Markup))
                            {
                            }
                            break;
                        }
                    case OpCode.ConstructListenerStorage:
                        {
                            int listenerCount = (int)reader.ReadUInt16();
                            if (instance.Listeners == null)
                            {
                                MarkupListeners markupListeners = new MarkupListeners(listenerCount);
                                markupListeners.DeclareOwner(instance);
                                instance.Listeners = markupListeners;
                            }
                            else
                            {
                                instance.Listeners.AddEntries(listenerCount);
                            }
                            if (Trace.IsCategoryEnabled(TraceCategory.Markup))
                            {
                            }
                            break;
                        }
                    case OpCode.Listen:
                    case OpCode.DestructiveListen:
                        {
                            int index5 = (int)reader.ReadUInt16();
                            ListenerType listenerType = (ListenerType)reader.ReadByte();
                            int num22 = (int)reader.ReadUInt16();
                            uint scriptOffset = reader.ReadUInt32();
                            uint refreshOffset = uint.MaxValue;
                            if (opCode == OpCode.DestructiveListen)
                            {
                                refreshOffset = reader.ReadUInt32();
                            }
                            string watch = null;
                            switch (listenerType)
                            {
                                case ListenerType.Property:
                                    watch = importTables.PropertyImports[num22].Name;
                                    break;
                                case ListenerType.Event:
                                    watch = importTables.EventImports[num22].Name;
                                    break;
                                case ListenerType.Symbol:
                                    watch = symbolReferenceTable[num22].Symbol;
                                    break;
                            }
                            object obj23 = stack.Peek();
                            if (obj23 != null)
                            {
                                INotifyObject notifier = (INotifyObject)obj23;
                                Trace.IsCategoryEnabled(TraceCategory.Markup);
                                MarkupListeners listeners = instance.Listeners;
                                listeners.RefreshListener(index5, notifier, watch, instance, scriptOffset, refreshOffset);
                            }
                            else
                            {
                                Trace.IsCategoryEnabled(TraceCategory.Markup);
                            }
                            break;
                        }
                    case OpCode.EnterDebugState:
                        {
                            int breakpointIndex = reader.ReadInt32();
                            if (MarkupSystem.IsDebuggingEnabled(2))
                            {
                                wasInDebugState = MarkupDebugHelper.EnterDebugState(wasInDebugState, loadResult, breakpointIndex, instance.Storage);
                            }
                            break;
                        }
                }
            }
            while (stack.Count > count)
            {
                stack.Pop();
            }
            return result;
        }

        // Token: 0x06000F23 RID: 3875 RVA: 0x0002ADA7 File Offset: 0x00029DA7
        private static bool ErrorsDetected(ErrorWatermark watermark, ref object result, ref bool done)
        {
            if (watermark.ErrorsDetected)
            {
                result = Interpreter.ScriptError;
                done = true;
                return true;
            }
            return false;
        }

        // Token: 0x06000F24 RID: 3876 RVA: 0x0002ADC0 File Offset: 0x00029DC0
        private static void RegisterDisposable(object instance, TypeSchema type, IMarkupTypeBase root)
        {
            if (type.Disposable && root != null)
            {
                IDisposableObject disposableObject = (IDisposableObject)instance;
                root.RegisterDisposable(disposableObject);
                disposableObject.DeclareOwner(root);
            }
        }

        // Token: 0x06000F25 RID: 3877 RVA: 0x0002ADF0 File Offset: 0x00029DF0
        private static object GetCollection(object stackInstance, MarkupImportTables importTables, int propertyIndex)
        {
            object result = null;
            if (propertyIndex == 65535)
            {
                result = stackInstance;
            }
            else
            {
                PropertySchema propertySchema = importTables.PropertyImports[propertyIndex];
                Interpreter.ReportErrorOnNull(stackInstance, "Property Get", propertySchema.Name);
                if (stackInstance != null)
                {
                    result = propertySchema.GetValue(stackInstance);
                }
            }
            return result;
        }

        // Token: 0x06000F26 RID: 3878 RVA: 0x0002AE31 File Offset: 0x00029E31
        private static void ReportErrorOnNull(object instance, string operation, string member)
        {
            if (instance == null)
            {
                ErrorManager.ReportError("Script runtime failure: Null-reference while attempting a '{0}' of '{1}' on a null instance", operation, member);
            }
        }

        // Token: 0x06000F27 RID: 3879 RVA: 0x0002AE42 File Offset: 0x00029E42
        private static void ReportErrorOnNull(object instance, string operation)
        {
            if (instance == null)
            {
                ErrorManager.ReportError("Script runtime failure: Null-reference while attempting a '{0}'", operation);
            }
        }

        // Token: 0x06000F28 RID: 3880 RVA: 0x0002AE54 File Offset: 0x00029E54
        private static void ReportErrorOnNullOrDisposed(object instance, string operation, string member, TypeSchema typeSchema)
        {
            if (instance == null)
            {
                ErrorManager.ReportError("Script runtime failure: Null-reference while attempting a '{0}' of '{1}' on a null instance", operation, member);
                return;
            }
            if (typeSchema.Disposable)
            {
                IDisposableObject disposableObject = (IDisposableObject)instance;
                if (disposableObject.IsDisposed)
                {
                    ErrorManager.ReportError("Script runtime failure: Attempting a '{0}' of '{1}' on an object '{2}' that has already been disposed", operation, member, TypeSchema.NameFromInstance(instance));
                }
            }
        }

        // Token: 0x04000947 RID: 2375
        public const uint InvalidOffset = 4294967295U;

        // Token: 0x04000948 RID: 2376
        private static object VoidReturnValue = new object();

        // Token: 0x04000949 RID: 2377
        private static Stack _stack = new Stack();

        // Token: 0x0400094A RID: 2378
        public static object ScriptError = new Interpreter.ScriptErrorObject();

        // Token: 0x0400094B RID: 2379
        public static string ExceptionContext;

        // Token: 0x0200018C RID: 396
        private class ScriptErrorObject
        {
        }

        // Token: 0x0200018D RID: 397
        private struct ParameterListAllocator
        {
            // Token: 0x06000F2C RID: 3884 RVA: 0x0002AECC File Offset: 0x00029ECC
            public static object[] Alloc(int count)
            {
                object[] array;
                if (count == 0)
                {
                    array = Interpreter.ParameterListAllocator.s_params0;
                }
                else if (count < 20)
                {
                    array = Interpreter.ParameterListAllocator.s_cachedLists[count];
                    if (array != null)
                    {
                        Interpreter.ParameterListAllocator.s_cachedLists[count] = null;
                    }
                    else
                    {
                        array = new object[count];
                    }
                }
                else
                {
                    array = new object[count];
                }
                return array;
            }

            // Token: 0x06000F2D RID: 3885 RVA: 0x0002AF10 File Offset: 0x00029F10
            public static void Free(object[] paramList)
            {
                int num = paramList.Length;
                if (num != 0 && num < 20 && Interpreter.ParameterListAllocator.s_cachedLists[num] == null)
                {
                    Array.Clear(paramList, 0, paramList.Length);
                    Interpreter.ParameterListAllocator.s_cachedLists[num] = paramList;
                }
            }

            // Token: 0x0400094C RID: 2380
            private const int MAX_CACHED_LIST_SIZE = 20;

            // Token: 0x0400094D RID: 2381
            private static object[] s_params0 = new object[0];

            // Token: 0x0400094E RID: 2382
            private static object[][] s_cachedLists = new object[20][];
        }
    }
}
