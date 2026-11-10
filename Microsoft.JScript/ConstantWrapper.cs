// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ConstantWrapper
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal class ConstantWrapper : AST
{
    internal object value;
    internal bool isNumericLiteral;

    internal ConstantWrapper(object value, Context context)
        : base(context)
    {
        if (value is ConcatString)
            value = (object)value.ToString();
        this.value = value;
        this.isNumericLiteral = false;
    }

    internal override object Evaluate() => this.value;

    internal override IReflect InferType(JSField inference_target)
    {
        if (this.value == null || this.value is DBNull)
            return (IReflect)Typeob.Object;
        if (this.value is ClassScope || this.value is TypedArray)
            return (IReflect)Typeob.Type;
        return this.value is EnumWrapper
            ? ((EnumWrapper)this.value).classScopeOrType
            : (IReflect)Globals.TypeRefs.ToReferenceContext(this.value.GetType());
    }

    internal bool IsAssignableTo(Type rtype)
    {
        try
        {
            Convert.CoerceT(this.value, rtype, false);
            return true;
        }
        catch
        {
            return false;
        }
    }

    internal override AST PartiallyEvaluate() => (AST)this;

    public override string ToString() => this.value.ToString();

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        if (rtype == Typeob.Void)
            return;
        object val = this.value;
        if (val is EnumWrapper && rtype != Typeob.Object && rtype != Typeob.String)
            val = ((EnumWrapper)val).value;
        if (this.isNumericLiteral && (rtype == Typeob.Decimal || rtype == Typeob.Int64 || rtype == Typeob.UInt64 ||
                                      rtype == Typeob.Single))
            val = (object)this.context.GetCode();
        if (!(rtype is TypeBuilder))
        {
            try
            {
                val = Convert.CoerceT(val, rtype);
            }
            catch
            {
            }
        }

        this.TranslateToIL(il, val, rtype);
    }

    private void TranslateToIL(ILGenerator il, object val, Type rtype)
    {
        IConvertible iconvertible = Convert.GetIConvertible(val);
        switch (Convert.GetTypeCode(val, iconvertible))
        {
            case TypeCode.Empty:
                il.Emit(OpCodes.Ldnull);
                if (!rtype.IsValueType)
                    break;
                Convert.Emit((AST)this, il, Typeob.Object, rtype);
                break;
            case TypeCode.DBNull:
                il.Emit(OpCodes.Ldsfld, Typeob.Null.GetField("Value"));
                Convert.Emit((AST)this, il, Typeob.Null, rtype);
                break;
            case TypeCode.Boolean:
                ConstantWrapper.TranslateToILInt(il, iconvertible.ToInt32((IFormatProvider)null));
                Convert.Emit((AST)this, il, Typeob.Boolean, rtype);
                break;
            case TypeCode.Char:
            case TypeCode.SByte:
            case TypeCode.Byte:
            case TypeCode.Int16:
            case TypeCode.UInt16:
            case TypeCode.Int32:
                ConstantWrapper.TranslateToILInt(il, iconvertible.ToInt32((IFormatProvider)null));
                if (rtype.IsEnum)
                    break;
                if (val is EnumWrapper)
                {
                    Convert.Emit((AST)this, il, ((EnumWrapper)val).type, rtype);
                    break;
                }

                Convert.Emit((AST)this, il, Globals.TypeRefs.ToReferenceContext(val.GetType()), rtype);
                break;
            case TypeCode.UInt32:
                ConstantWrapper.TranslateToILInt(il, (int)iconvertible.ToUInt32((IFormatProvider)null));
                if (rtype.IsEnum)
                    break;
                if (val is EnumWrapper)
                {
                    Convert.Emit((AST)this, il, ((EnumWrapper)val).type, rtype);
                    break;
                }

                Convert.Emit((AST)this, il, Typeob.UInt32, rtype);
                break;
            case TypeCode.Int64:
                long int64_1 = iconvertible.ToInt64((IFormatProvider)null);
                if ((long)int.MinValue <= int64_1 && int64_1 <= (long)int.MaxValue)
                {
                    ConstantWrapper.TranslateToILInt(il, (int)int64_1);
                    il.Emit(OpCodes.Conv_I8);
                }
                else
                    il.Emit(OpCodes.Ldc_I8, int64_1);

                if (rtype.IsEnum)
                    break;
                if (val is EnumWrapper)
                {
                    Convert.Emit((AST)this, il, ((EnumWrapper)val).type, rtype);
                    break;
                }

                Convert.Emit((AST)this, il, Typeob.Int64, rtype);
                break;
            case TypeCode.UInt64:
                ulong uint64 = iconvertible.ToUInt64((IFormatProvider)null);
                if (uint64 <= (ulong)int.MaxValue)
                {
                    ConstantWrapper.TranslateToILInt(il, (int)uint64);
                    il.Emit(OpCodes.Conv_I8);
                }
                else
                    il.Emit(OpCodes.Ldc_I8, (long)uint64);

                if (rtype.IsEnum)
                    break;
                if (val is EnumWrapper)
                {
                    Convert.Emit((AST)this, il, ((EnumWrapper)val).type, rtype);
                    break;
                }

                Convert.Emit((AST)this, il, Typeob.UInt64, rtype);
                break;
            case TypeCode.Single:
                float single = iconvertible.ToSingle((IFormatProvider)null);
                if ((double)single == (double)single &&
                    ((double)single != 0.0 || !float.IsNegativeInfinity(1f / single)))
                {
                    int int64_2 = (int)Runtime.DoubleToInt64((double)single);
                    if ((int)sbyte.MinValue <= int64_2 && int64_2 <= (int)sbyte.MaxValue &&
                        (double)single == (double)int64_2)
                    {
                        ConstantWrapper.TranslateToILInt(il, int64_2);
                        il.Emit(OpCodes.Conv_R4);
                    }
                    else
                        il.Emit(OpCodes.Ldc_R4, single);
                }
                else
                    il.Emit(OpCodes.Ldc_R4, single);

                Convert.Emit((AST)this, il, Typeob.Single, rtype);
                break;
            case TypeCode.Double:
                double val1 = iconvertible.ToDouble((IFormatProvider)null);
                if (val1 == val1 && (val1 != 0.0 || !double.IsNegativeInfinity(1.0 / val1)))
                {
                    int int64_3 = (int)Runtime.DoubleToInt64(val1);
                    if ((int)sbyte.MinValue <= int64_3 && int64_3 <= (int)sbyte.MaxValue && val1 == (double)int64_3)
                    {
                        ConstantWrapper.TranslateToILInt(il, int64_3);
                        il.Emit(OpCodes.Conv_R8);
                    }
                    else
                        il.Emit(OpCodes.Ldc_R8, val1);
                }
                else
                    il.Emit(OpCodes.Ldc_R8, val1);

                Convert.Emit((AST)this, il, Typeob.Double, rtype);
                break;
            case TypeCode.Decimal:
                int[] bits = Decimal.GetBits(iconvertible.ToDecimal((IFormatProvider)null));
                ConstantWrapper.TranslateToILInt(il, bits[0]);
                ConstantWrapper.TranslateToILInt(il, bits[1]);
                ConstantWrapper.TranslateToILInt(il, bits[2]);
                il.Emit(bits[3] < 0 ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
                ConstantWrapper.TranslateToILInt(il, (bits[3] & int.MaxValue) >> 16 /*0x10*/);
                il.Emit(OpCodes.Newobj, CompilerGlobals.decimalConstructor);
                Convert.Emit((AST)this, il, Typeob.Decimal, rtype);
                break;
            case TypeCode.DateTime:
                long ticks = iconvertible.ToDateTime((IFormatProvider)null).Ticks;
                il.Emit(OpCodes.Ldc_I8, ticks);
                Convert.Emit((AST)this, il, Typeob.Int64, rtype);
                break;
            case TypeCode.String:
                string str = iconvertible.ToString((IFormatProvider)null);
                if (rtype == Typeob.Char && str.Length == 1)
                {
                    ConstantWrapper.TranslateToILInt(il, (int)str[0]);
                    break;
                }

                il.Emit(OpCodes.Ldstr, str);
                Convert.Emit((AST)this, il, Typeob.String, rtype);
                break;
            default:
                if (val is Enum)
                {
                    if (rtype == Typeob.String)
                    {
                        this.TranslateToIL(il, (object)val.ToString(), rtype);
                        break;
                    }

                    if (rtype.IsPrimitive)
                    {
                        this.TranslateToIL(il,
                            System.Convert.ChangeType(val,
                                Enum.GetUnderlyingType(Globals.TypeRefs.ToReferenceContext(val.GetType())),
                                (IFormatProvider)CultureInfo.InvariantCulture), rtype);
                        break;
                    }

                    Type referenceContext = Globals.TypeRefs.ToReferenceContext(val.GetType());
                    Type underlyingType = Enum.GetUnderlyingType(referenceContext);
                    this.TranslateToIL(il,
                        System.Convert.ChangeType(val, underlyingType, (IFormatProvider)CultureInfo.InvariantCulture),
                        underlyingType);
                    il.Emit(OpCodes.Box, referenceContext);
                    Convert.Emit((AST)this, il, Typeob.Object, rtype);
                    break;
                }

                if (val is EnumWrapper)
                {
                    if (rtype == Typeob.String)
                    {
                        this.TranslateToIL(il, (object)val.ToString(), rtype);
                        break;
                    }

                    if (rtype.IsPrimitive)
                    {
                        this.TranslateToIL(il, ((EnumWrapper)val).ToNumericValue(), rtype);
                        break;
                    }

                    Type type = ((EnumWrapper)val).type;
                    Type referenceContext = Globals.TypeRefs.ToReferenceContext(((EnumWrapper)val).value.GetType());
                    this.TranslateToIL(il, ((EnumWrapper)val).value, referenceContext);
                    il.Emit(OpCodes.Box, type);
                    Convert.Emit((AST)this, il, Typeob.Object, rtype);
                    break;
                }

                if ((object)(val as Type) != null)
                {
                    il.Emit(OpCodes.Ldtoken, (Type)val);
                    il.Emit(OpCodes.Call, CompilerGlobals.getTypeFromHandleMethod);
                    Convert.Emit((AST)this, il, Typeob.Type, rtype);
                    break;
                }

                switch (val)
                {
                    case Namespace _:
                        il.Emit(OpCodes.Ldstr, ((Namespace)val).Name);
                        this.EmitILToLoadEngine(il);
                        il.Emit(OpCodes.Call, CompilerGlobals.getNamespaceMethod);
                        Convert.Emit((AST)this, il, Typeob.Namespace, rtype);
                        return;
                    case ClassScope _:
                        il.Emit(OpCodes.Ldtoken, ((ClassScope)val).GetTypeBuilderOrEnumBuilder());
                        il.Emit(OpCodes.Call, CompilerGlobals.getTypeFromHandleMethod);
                        Convert.Emit((AST)this, il, Typeob.Type, rtype);
                        return;
                    case TypedArray _:
                        il.Emit(OpCodes.Ldtoken, Convert.ToType((IReflect)val));
                        il.Emit(OpCodes.Call, CompilerGlobals.getTypeFromHandleMethod);
                        Convert.Emit((AST)this, il, Typeob.Type, rtype);
                        return;
                    case NumberObject _:
                        this.TranslateToIL(il, ((NumberObject)val).value, Typeob.Object);
                        this.EmitILToLoadEngine(il);
                        il.Emit(OpCodes.Call, CompilerGlobals.toObjectMethod);
                        Convert.Emit((AST)this, il, Typeob.NumberObject, rtype);
                        return;
                    case StringObject _:
                        il.Emit(OpCodes.Ldstr, ((StringObject)val).value);
                        this.EmitILToLoadEngine(il);
                        il.Emit(OpCodes.Call, CompilerGlobals.toObjectMethod);
                        Convert.Emit((AST)this, il, Typeob.StringObject, rtype);
                        return;
                    case BooleanObject _:
                        il.Emit(((BooleanObject)val).value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
                        il.Emit(OpCodes.Box, Typeob.Boolean);
                        this.EmitILToLoadEngine(il);
                        il.Emit(OpCodes.Call, CompilerGlobals.toObjectMethod);
                        Convert.Emit((AST)this, il, Typeob.BooleanObject, rtype);
                        return;
                    case ActiveXObjectConstructor _:
                        il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("ActiveXObject").GetGetMethod());
                        Convert.Emit((AST)this, il, Typeob.ScriptFunction, rtype);
                        return;
                    case ArrayConstructor _:
                        il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("Array").GetGetMethod());
                        Convert.Emit((AST)this, il, Typeob.ScriptFunction, rtype);
                        return;
                    case BooleanConstructor _:
                        il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("Boolean").GetGetMethod());
                        Convert.Emit((AST)this, il, Typeob.ScriptFunction, rtype);
                        return;
                    case DateConstructor _:
                        il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("Date").GetGetMethod());
                        Convert.Emit((AST)this, il, Typeob.ScriptFunction, rtype);
                        return;
                    case EnumeratorConstructor _:
                        il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("Enumerator").GetGetMethod());
                        Convert.Emit((AST)this, il, Typeob.ScriptFunction, rtype);
                        return;
                    case ErrorConstructor _:
                        ErrorConstructor errorConstructor = (ErrorConstructor)val;
                        if (errorConstructor == ErrorConstructor.evalOb)
                            il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("EvalError").GetGetMethod());
                        else if (errorConstructor == ErrorConstructor.rangeOb)
                            il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("RangeError").GetGetMethod());
                        else if (errorConstructor == ErrorConstructor.referenceOb)
                            il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("ReferenceError").GetGetMethod());
                        else if (errorConstructor == ErrorConstructor.syntaxOb)
                            il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("SyntaxError").GetGetMethod());
                        else if (errorConstructor == ErrorConstructor.typeOb)
                            il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("TypeError").GetGetMethod());
                        else if (errorConstructor == ErrorConstructor.uriOb)
                            il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("URIError").GetGetMethod());
                        else
                            il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("Error").GetGetMethod());
                        Convert.Emit((AST)this, il, Typeob.ScriptFunction, rtype);
                        return;
                    case FunctionConstructor _:
                        il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("Function").GetGetMethod());
                        Convert.Emit((AST)this, il, Typeob.ScriptFunction, rtype);
                        return;
                    case MathObject _:
                        il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("Math").GetGetMethod());
                        Convert.Emit((AST)this, il, Typeob.JSObject, rtype);
                        return;
                    case NumberConstructor _:
                        il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("Number").GetGetMethod());
                        Convert.Emit((AST)this, il, Typeob.ScriptFunction, rtype);
                        return;
                    case ObjectConstructor _:
                        il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("Object").GetGetMethod());
                        Convert.Emit((AST)this, il, Typeob.ScriptFunction, rtype);
                        return;
                    case RegExpConstructor _:
                        il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("RegExp").GetGetMethod());
                        Convert.Emit((AST)this, il, Typeob.ScriptFunction, rtype);
                        return;
                    case StringConstructor _:
                        il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("String").GetGetMethod());
                        Convert.Emit((AST)this, il, Typeob.ScriptFunction, rtype);
                        return;
                    case VBArrayConstructor _:
                        il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("VBArray").GetGetMethod());
                        Convert.Emit((AST)this, il, Typeob.ScriptFunction, rtype);
                        return;
                    case IntPtr num1:
                        il.Emit(OpCodes.Ldc_I8, (long)num1);
                        il.Emit(OpCodes.Conv_I);
                        Convert.Emit((AST)this, il, Typeob.IntPtr, rtype);
                        return;
                    case UIntPtr num2:
                        il.Emit(OpCodes.Ldc_I8, (long)(ulong)num2);
                        il.Emit(OpCodes.Conv_U);
                        Convert.Emit((AST)this, il, Typeob.UIntPtr, rtype);
                        return;
                    case Missing _:
                        il.Emit(OpCodes.Ldsfld, CompilerGlobals.missingField);
                        Convert.Emit((AST)this, il, Typeob.Object, rtype);
                        return;
                    case System.Reflection.Missing _:
                        if (rtype.IsPrimitive)
                        {
                            this.TranslateToIL(il, (object)double.NaN, rtype);
                            return;
                        }

                        if (rtype != Typeob.Object && !rtype.IsValueType)
                        {
                            il.Emit(OpCodes.Ldnull);
                            return;
                        }

                        il.Emit(OpCodes.Ldsfld, CompilerGlobals.systemReflectionMissingField);
                        Convert.Emit((AST)this, il, Typeob.Object, rtype);
                        return;
                    default:
                        if (val == this.value)
                            throw new JScriptException(JSError.InternalError, this.context);
                        this.TranslateToIL(il, this.value, rtype);
                        return;
                }
        }
    }

    internal static void TranslateToILInt(ILGenerator il, int i)
    {
        switch (i)
        {
            case -1:
                il.Emit(OpCodes.Ldc_I4_M1);
                break;
            case 0:
                il.Emit(OpCodes.Ldc_I4_0);
                break;
            case 1:
                il.Emit(OpCodes.Ldc_I4_1);
                break;
            case 2:
                il.Emit(OpCodes.Ldc_I4_2);
                break;
            case 3:
                il.Emit(OpCodes.Ldc_I4_3);
                break;
            case 4:
                il.Emit(OpCodes.Ldc_I4_4);
                break;
            case 5:
                il.Emit(OpCodes.Ldc_I4_5);
                break;
            case 6:
                il.Emit(OpCodes.Ldc_I4_6);
                break;
            case 7:
                il.Emit(OpCodes.Ldc_I4_7);
                break;
            case 8:
                il.Emit(OpCodes.Ldc_I4_8);
                break;
            default:
                if ((int)sbyte.MinValue <= i && i <= (int)sbyte.MaxValue)
                {
                    il.Emit(OpCodes.Ldc_I4_S, (sbyte)i);
                    break;
                }

                il.Emit(OpCodes.Ldc_I4, i);
                break;
        }
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
    }
}