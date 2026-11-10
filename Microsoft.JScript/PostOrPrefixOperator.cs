// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.PostOrPrefixOperator
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Performs increment and decrement operations that correspond to the prefix and postfix syntax. This class belongs to the abstract syntax tree category.</summary>
public class PostOrPrefixOperator : UnaryOp
{
    private MethodInfo operatorMeth;
    private PostOrPrefix operatorTok;
    private object metaData;
    private Type type;

    internal PostOrPrefixOperator(Context context, AST operand)
        : base(context, operand)
    {
    }

    internal PostOrPrefixOperator(Context context, AST operand, PostOrPrefix operatorTok)
        : base(context, operand)
    {
        this.operatorMeth = (MethodInfo)null;
        this.operatorTok = operatorTok;
        this.metaData = (object)null;
        this.type = (Type)null;
    }

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.PostOrPrefixOperator" /> class.</summary>
    /// <param name="operatorTok">One of the <see cref="T:Microsoft.JScript.JSToken" /> enumeration values. Specifies the operation to perform.</param>
    public PostOrPrefixOperator(int operatorTok)
        : this((Context)null, (AST)null, (PostOrPrefix)operatorTok)
    {
    }

    private object DoOp(int i)
    {
        switch (this.operatorTok)
        {
            case PostOrPrefix.PostfixIncrement:
            case PostOrPrefix.PrefixIncrement:
                return i == int.MaxValue ? (object)2147483648.0 : (object)(i + 1);
            default:
                return i == int.MinValue ? (object)-2147483649.0 : (object)(i - 1);
        }
    }

    private object DoOp(uint i)
    {
        switch (this.operatorTok)
        {
            case PostOrPrefix.PostfixIncrement:
            case PostOrPrefix.PrefixIncrement:
                return i == uint.MaxValue ? (object)4294967296.0 : (object)(uint)((int)i + 1);
            default:
                return i == 0U ? (object)-1.0 : (object)(uint)((int)i - 1);
        }
    }

    private object DoOp(long i)
    {
        switch (this.operatorTok)
        {
            case PostOrPrefix.PostfixIncrement:
            case PostOrPrefix.PrefixIncrement:
                return i == long.MaxValue ? (object)(double)long.MaxValue : (object)(i + 1L);
            default:
                return i == long.MinValue ? (object)(double)long.MinValue : (object)(i - 1L);
        }
    }

    private object DoOp(ulong i)
    {
        switch (this.operatorTok)
        {
            case PostOrPrefix.PostfixIncrement:
            case PostOrPrefix.PrefixIncrement:
                return i == ulong.MaxValue ? (object)1.8446744073709552E+19 : (object)(ulong)((long)i + 1L);
            default:
                return i == 0UL ? (object)-1.0 : (object)(ulong)((long)i - 1L);
        }
    }

    private object DoOp(double d)
    {
        switch (this.operatorTok)
        {
            case PostOrPrefix.PostfixIncrement:
            case PostOrPrefix.PrefixIncrement:
                return (object)(d + 1.0);
            default:
                return (object)(d - 1.0);
        }
    }

    internal override object Evaluate()
    {
        try
        {
            object v = this.operand.Evaluate();
            object postOrPrefix = this.EvaluatePostOrPrefix(ref v);
            this.operand.SetValue(postOrPrefix);
            switch (this.operatorTok)
            {
                case PostOrPrefix.PostfixDecrement:
                case PostOrPrefix.PostfixIncrement:
                    return v;
                case PostOrPrefix.PrefixDecrement:
                case PostOrPrefix.PrefixIncrement:
                    return postOrPrefix;
                default:
                    throw new JScriptException(JSError.InternalError, this.context);
            }
        }
        catch (JScriptException ex)
        {
            if (ex.context == null)
                ex.context = this.context;
            throw ex;
        }
        catch (Exception ex)
        {
            throw new JScriptException(ex, this.context);
        }
    }

    /// <summary>Performs an increment or decrement operation on the specified operand.</summary>
    /// <returns>The result of the operation.</returns>
    /// <param name="v">The operand.</param>
    [DebuggerStepThrough]
    [DebuggerHidden]
    public object EvaluatePostOrPrefix(ref object v)
    {
        IConvertible iconvertible = Convert.GetIConvertible(v);
        switch (Convert.GetTypeCode(v, iconvertible))
        {
            case TypeCode.Empty:
                v = (object)double.NaN;
                return v;
            case TypeCode.DBNull:
                v = (object)0;
                return this.DoOp(0);
            case TypeCode.Boolean:
            case TypeCode.SByte:
            case TypeCode.Byte:
            case TypeCode.Int16:
            case TypeCode.UInt16:
            case TypeCode.Int32:
                return this.DoOp((int)(v = (object)iconvertible.ToInt32((IFormatProvider)null)));
            case TypeCode.Char:
                return (object)((IConvertible)this.DoOp(iconvertible.ToInt32((IFormatProvider)null))).ToChar(
                    (IFormatProvider)null);
            case TypeCode.UInt32:
                return this.DoOp((uint)(v = (object)iconvertible.ToUInt32((IFormatProvider)null)));
            case TypeCode.Int64:
                return this.DoOp((long)(v = (object)iconvertible.ToInt64((IFormatProvider)null)));
            case TypeCode.UInt64:
                return this.DoOp((ulong)(v = (object)iconvertible.ToUInt64((IFormatProvider)null)));
            case TypeCode.Single:
            case TypeCode.Double:
                return this.DoOp((double)(v = (object)iconvertible.ToDouble((IFormatProvider)null)));
            default:
                MethodInfo methodInfo = this.GetOperator((IReflect)v.GetType());
                if (methodInfo != (MethodInfo)null)
                    return methodInfo.Invoke((object)null, BindingFlags.Default, (Binder)JSBinder.ob, new object[1]
                    {
                        v
                    }, (CultureInfo)null);
                return this.DoOp((double)(v = (object)Convert.ToNumber(v, iconvertible)));
        }
    }

    private MethodInfo GetOperator(IReflect ir)
    {
        Type type = (object)(ir as Type) != null ? (Type)ir : Typeob.Object;
        if (this.type == type)
            return this.operatorMeth;
        this.type = type;
        if (Convert.IsPrimitiveNumericType((IReflect)type) || Typeob.JSObject.IsAssignableFrom(type))
        {
            this.operatorMeth = (MethodInfo)null;
            return (MethodInfo)null;
        }

        switch (this.operatorTok)
        {
            case PostOrPrefix.PostfixDecrement:
            case PostOrPrefix.PrefixDecrement:
                this.operatorMeth = type.GetMethod("op_Decrement", BindingFlags.Static | BindingFlags.Public,
                    (Binder)JSBinder.ob, new Type[1]
                    {
                        type
                    }, (ParameterModifier[])null);
                break;
            case PostOrPrefix.PostfixIncrement:
            case PostOrPrefix.PrefixIncrement:
                this.operatorMeth = type.GetMethod("op_Increment", BindingFlags.Static | BindingFlags.Public,
                    (Binder)JSBinder.ob, new Type[1]
                    {
                        type
                    }, (ParameterModifier[])null);
                break;
            default:
                throw new JScriptException(JSError.InternalError, this.context);
        }

        if (this.operatorMeth != (MethodInfo)null && (!this.operatorMeth.IsStatic ||
                                                      (this.operatorMeth.Attributes & MethodAttributes.SpecialName) ==
                                                      MethodAttributes.PrivateScope ||
                                                      this.operatorMeth.GetParameters().Length != 1))
            this.operatorMeth = (MethodInfo)null;
        if (this.operatorMeth != (MethodInfo)null)
            this.operatorMeth = (MethodInfo)new JSMethodInfo(this.operatorMeth);
        return this.operatorMeth;
    }

    internal override IReflect InferType(JSField inference_target)
    {
        MethodInfo methodInfo = this.type == (Type)null || (FieldInfo)inference_target != (FieldInfo)null
            ? this.GetOperator(this.operand.InferType(inference_target))
            : this.GetOperator((IReflect)this.type);
        if (methodInfo != (MethodInfo)null)
        {
            this.metaData = (object)methodInfo;
            return (IReflect)methodInfo.ReturnType;
        }

        if (Convert.IsPrimitiveNumericType((IReflect)this.type))
            return (IReflect)this.type;
        if (this.type == Typeob.Char)
            return (IReflect)this.type;
        return Typeob.JSObject.IsAssignableFrom(this.type) ? (IReflect)Typeob.Double : (IReflect)Typeob.Object;
    }

    internal override AST PartiallyEvaluate()
    {
        this.operand = this.operand.PartiallyEvaluateAsReference();
        this.operand.SetPartialValue((AST)this);
        return (AST)this;
    }

    private void TranslateToILForNoOverloadCase(ILGenerator il, Type rtype)
    {
        Type type1 = Convert.ToType(this.operand.InferType((JSField)null));
        this.operand.TranslateToILPreSetPlusGet(il);
        if (rtype == Typeob.Void)
        {
            Type type2 = Typeob.Double;
            if (Convert.IsPrimitiveNumericType((IReflect)type1))
                type2 = type1 == Typeob.SByte || type1 == Typeob.Int16
                    ? Typeob.Int32
                    : (type1 == Typeob.Byte || type1 == Typeob.UInt16 || type1 == Typeob.Char ? Typeob.UInt32 : type1);
            Convert.Emit((AST)this, il, type1, type2);
            il.Emit(OpCodes.Ldc_I4_1);
            Convert.Emit((AST)this, il, Typeob.Int32, type2);
            if (type2 == Typeob.Double || type2 == Typeob.Single)
            {
                if (this.operatorTok == PostOrPrefix.PostfixDecrement ||
                    this.operatorTok == PostOrPrefix.PrefixDecrement)
                    il.Emit(OpCodes.Sub);
                else
                    il.Emit(OpCodes.Add);
            }
            else if (type2 == Typeob.Int32 || type2 == Typeob.Int64)
            {
                if (this.operatorTok == PostOrPrefix.PostfixDecrement ||
                    this.operatorTok == PostOrPrefix.PrefixDecrement)
                    il.Emit(OpCodes.Sub_Ovf);
                else
                    il.Emit(OpCodes.Add_Ovf);
            }
            else if (this.operatorTok == PostOrPrefix.PostfixDecrement ||
                     this.operatorTok == PostOrPrefix.PrefixDecrement)
                il.Emit(OpCodes.Sub_Ovf_Un);
            else
                il.Emit(OpCodes.Add_Ovf_Un);

            Convert.Emit((AST)this, il, type2, type1);
            this.operand.TranslateToILSet(il);
        }
        else
        {
            Type type3 = Typeob.Double;
            if (Convert.IsPrimitiveNumericType((IReflect)rtype) &&
                Convert.IsPromotableTo((IReflect)type1, (IReflect)rtype))
                type3 = rtype;
            else if (Convert.IsPrimitiveNumericType((IReflect)type1) &&
                     Convert.IsPromotableTo((IReflect)rtype, (IReflect)type1))
                type3 = type1;
            if (type3 == Typeob.SByte || type3 == Typeob.Int16)
                type3 = Typeob.Int32;
            else if (type3 == Typeob.Byte || type3 == Typeob.UInt16 || type3 == Typeob.Char)
                type3 = Typeob.UInt32;
            LocalBuilder local = il.DeclareLocal(rtype);
            Convert.Emit((AST)this, il, type1, type3);
            if (this.operatorTok == PostOrPrefix.PostfixDecrement)
            {
                il.Emit(OpCodes.Dup);
                if (type1 == Typeob.Char)
                {
                    Convert.Emit((AST)this, il, type3, Typeob.Char);
                    Convert.Emit((AST)this, il, Typeob.Char, rtype);
                }
                else
                    Convert.Emit((AST)this, il, type3, rtype);

                il.Emit(OpCodes.Stloc, local);
                il.Emit(OpCodes.Ldc_I4_1);
                Convert.Emit((AST)this, il, Typeob.Int32, type3);
                if (type3 == Typeob.Double || type3 == Typeob.Single)
                    il.Emit(OpCodes.Sub);
                else if (type3 == Typeob.Int32 || type3 == Typeob.Int64)
                    il.Emit(OpCodes.Sub_Ovf);
                else
                    il.Emit(OpCodes.Sub_Ovf_Un);
            }
            else if (this.operatorTok == PostOrPrefix.PostfixIncrement)
            {
                il.Emit(OpCodes.Dup);
                if (type1 == Typeob.Char)
                {
                    Convert.Emit((AST)this, il, type3, Typeob.Char);
                    Convert.Emit((AST)this, il, Typeob.Char, rtype);
                }
                else
                    Convert.Emit((AST)this, il, type3, rtype);

                il.Emit(OpCodes.Stloc, local);
                il.Emit(OpCodes.Ldc_I4_1);
                Convert.Emit((AST)this, il, Typeob.Int32, type3);
                if (type3 == Typeob.Double || type3 == Typeob.Single)
                    il.Emit(OpCodes.Add);
                else if (type3 == Typeob.Int32 || type3 == Typeob.Int64)
                    il.Emit(OpCodes.Add_Ovf);
                else
                    il.Emit(OpCodes.Add_Ovf_Un);
            }
            else if (this.operatorTok == PostOrPrefix.PrefixDecrement)
            {
                il.Emit(OpCodes.Ldc_I4_1);
                Convert.Emit((AST)this, il, Typeob.Int32, type3);
                if (type3 == Typeob.Double || type3 == Typeob.Single)
                    il.Emit(OpCodes.Sub);
                else if (type3 == Typeob.Int32 || type3 == Typeob.Int64)
                    il.Emit(OpCodes.Sub_Ovf);
                else
                    il.Emit(OpCodes.Sub_Ovf_Un);
                il.Emit(OpCodes.Dup);
                if (type1 == Typeob.Char)
                {
                    Convert.Emit((AST)this, il, type3, Typeob.Char);
                    Convert.Emit((AST)this, il, Typeob.Char, rtype);
                }
                else
                    Convert.Emit((AST)this, il, type3, rtype);

                il.Emit(OpCodes.Stloc, local);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4_1);
                Convert.Emit((AST)this, il, Typeob.Int32, type3);
                if (type3 == Typeob.Double || type3 == Typeob.Single)
                    il.Emit(OpCodes.Add);
                else if (type3 == Typeob.Int32 || type3 == Typeob.Int64)
                    il.Emit(OpCodes.Add_Ovf);
                else
                    il.Emit(OpCodes.Add_Ovf_Un);
                il.Emit(OpCodes.Dup);
                if (type1 == Typeob.Char)
                {
                    Convert.Emit((AST)this, il, type3, Typeob.Char);
                    Convert.Emit((AST)this, il, Typeob.Char, rtype);
                }
                else
                    Convert.Emit((AST)this, il, type3, rtype);

                il.Emit(OpCodes.Stloc, local);
            }

            Convert.Emit((AST)this, il, type3, type1);
            this.operand.TranslateToILSet(il);
            il.Emit(OpCodes.Ldloc, local);
        }
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        if (this.metaData == null)
            this.TranslateToILForNoOverloadCase(il, rtype);
        else if ((object)(this.metaData as MethodInfo) != null)
        {
            object local = (object)null;
            Type type = Convert.ToType(this.operand.InferType((JSField)null));
            this.operand.TranslateToILPreSetPlusGet(il);
            if (rtype != Typeob.Void)
            {
                local = (object)il.DeclareLocal(rtype);
                if (this.operatorTok == PostOrPrefix.PostfixDecrement ||
                    this.operatorTok == PostOrPrefix.PostfixIncrement)
                {
                    il.Emit(OpCodes.Dup);
                    Convert.Emit((AST)this, il, type, rtype);
                    il.Emit(OpCodes.Stloc, (LocalBuilder)local);
                }
            }

            MethodInfo metaData = (MethodInfo)this.metaData;
            ParameterInfo[] parameters = metaData.GetParameters();
            Convert.Emit((AST)this, il, type, parameters[0].ParameterType);
            il.Emit(OpCodes.Call, metaData);
            if (rtype != Typeob.Void && (this.operatorTok == PostOrPrefix.PrefixDecrement ||
                                         this.operatorTok == PostOrPrefix.PrefixIncrement))
            {
                il.Emit(OpCodes.Dup);
                Convert.Emit((AST)this, il, type, rtype);
                il.Emit(OpCodes.Stloc, (LocalBuilder)local);
            }

            Convert.Emit((AST)this, il, metaData.ReturnType, type);
            this.operand.TranslateToILSet(il);
            if (!(rtype != Typeob.Void))
                return;
            il.Emit(OpCodes.Ldloc, (LocalBuilder)local);
        }
        else
        {
            Type type = Convert.ToType(this.operand.InferType((JSField)null));
            LocalBuilder local = il.DeclareLocal(Typeob.Object);
            this.operand.TranslateToILPreSetPlusGet(il);
            Convert.Emit((AST)this, il, type, Typeob.Object);
            il.Emit(OpCodes.Stloc, local);
            il.Emit(OpCodes.Ldloc, (LocalBuilder)this.metaData);
            il.Emit(OpCodes.Ldloca, local);
            il.Emit(OpCodes.Call, CompilerGlobals.evaluatePostOrPrefixOperatorMethod);
            if (rtype != Typeob.Void && (this.operatorTok == PostOrPrefix.PrefixDecrement ||
                                         this.operatorTok == PostOrPrefix.PrefixIncrement))
            {
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Stloc, local);
            }

            Convert.Emit((AST)this, il, Typeob.Object, type);
            this.operand.TranslateToILSet(il);
            if (!(rtype != Typeob.Void))
                return;
            il.Emit(OpCodes.Ldloc, local);
            Convert.Emit((AST)this, il, Typeob.Object, rtype);
        }
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
        IReflect reflect = this.InferType((JSField)null);
        this.operand.TranslateToILInitializer(il);
        if ((object)reflect != (object)Typeob.Object)
            return;
        this.metaData = (object)il.DeclareLocal(Typeob.PostOrPrefixOperator);
        ConstantWrapper.TranslateToILInt(il, (int)this.operatorTok);
        il.Emit(OpCodes.Newobj, CompilerGlobals.postOrPrefixConstructor);
        il.Emit(OpCodes.Stloc, (LocalBuilder)this.metaData);
    }
}