// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.NumericUnary
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

/// <summary>Performs unary operations on numeric operands. This class belongs to the abstract syntax tree category.</summary>
public sealed class NumericUnary : UnaryOp
{
    private object metaData;
    private JSToken operatorTok;
    private MethodInfo operatorMeth;
    private Type type;

    internal NumericUnary(Context context, AST operand, JSToken operatorTok)
        : base(context, operand)
    {
        this.operatorTok = operatorTok;
        this.operatorMeth = (MethodInfo)null;
        this.type = (Type)null;
    }

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.NumericUnary" /> class.</summary>
    /// <param name="operatorTok">One of the <see cref="T:Microsoft.JScript.JSToken" /> enumeration values. Specifies the operation to perform.</param>
    public NumericUnary(int operatorTok)
        : this((Context)null, (AST)null, (JSToken)operatorTok)
    {
    }

    internal override object Evaluate() => this.EvaluateUnary(this.operand.Evaluate());

    /// <summary>Performs a unary operation on a numeric operand.</summary>
    /// <returns>The result of the operation.</returns>
    /// <param name="v">The operand.</param>
    [DebuggerStepThrough]
    [DebuggerHidden]
    public object EvaluateUnary(object v)
    {
        IConvertible iconvertible = Convert.GetIConvertible(v);
        switch (Convert.GetTypeCode(v, iconvertible))
        {
            case TypeCode.Empty:
                return this.EvaluateUnary((object)double.NaN);
            case TypeCode.DBNull:
                return this.EvaluateUnary((object)0);
            case TypeCode.Boolean:
                return this.EvaluateUnary((object)(iconvertible.ToBoolean((IFormatProvider)null) ? 1 : 0));
            case TypeCode.Char:
                return this.EvaluateUnary((object)(int)iconvertible.ToChar((IFormatProvider)null));
            case TypeCode.SByte:
            case TypeCode.Byte:
            case TypeCode.Int16:
            case TypeCode.UInt16:
            case TypeCode.Int32:
                int int32 = iconvertible.ToInt32((IFormatProvider)null);
                switch (this.operatorTok)
                {
                    case JSToken.FirstOp:
                        return (object)(int32 == 0);
                    case JSToken.BitwiseNot:
                        return (object)~int32;
                    case JSToken.FirstBinaryOp:
                        return (object)int32;
                    case JSToken.Minus:
                        if (int32 == 0)
                            return (object)-(double)int32;
                        return int32 == int.MinValue ? (object)(ulong)-(double)int32 : (object)-int32;
                    default:
                        throw new JScriptException(JSError.InternalError, this.context);
                }
            case TypeCode.UInt32:
                uint uint32 = iconvertible.ToUInt32((IFormatProvider)null);
                switch (this.operatorTok)
                {
                    case JSToken.FirstOp:
                        return (object)(uint32 == 0U);
                    case JSToken.BitwiseNot:
                        return (object)(uint)~(int)uint32;
                    case JSToken.FirstBinaryOp:
                        return (object)uint32;
                    case JSToken.Minus:
                        return uint32 != 0U && uint32 <= (uint)int.MaxValue
                            ? (object)-(int)uint32
                            : (object)-(double)uint32;
                    default:
                        throw new JScriptException(JSError.InternalError, this.context);
                }
            case TypeCode.Int64:
                long int64 = iconvertible.ToInt64((IFormatProvider)null);
                switch (this.operatorTok)
                {
                    case JSToken.FirstOp:
                        return (object)(int64 == 0L);
                    case JSToken.BitwiseNot:
                        return (object)~int64;
                    case JSToken.FirstBinaryOp:
                        return (object)int64;
                    case JSToken.Minus:
                        return int64 == 0L || int64 == long.MinValue ? (object)-(double)int64 : (object)-int64;
                    default:
                        throw new JScriptException(JSError.InternalError, this.context);
                }
            case TypeCode.UInt64:
                ulong uint64 = iconvertible.ToUInt64((IFormatProvider)null);
                switch (this.operatorTok)
                {
                    case JSToken.FirstOp:
                        return (object)(uint64 == 0UL);
                    case JSToken.BitwiseNot:
                        return (object)(ulong)~(long)uint64;
                    case JSToken.FirstBinaryOp:
                        return (object)uint64;
                    case JSToken.Minus:
                        return uint64 != 0UL && uint64 <= (ulong)long.MaxValue
                            ? (object)-(long)uint64
                            : (object)-(double)uint64;
                    default:
                        throw new JScriptException(JSError.InternalError, this.context);
                }
            case TypeCode.Single:
            case TypeCode.Double:
                double unary = iconvertible.ToDouble((IFormatProvider)null);
                switch (this.operatorTok)
                {
                    case JSToken.FirstOp:
                        return (object)!Convert.ToBoolean(unary);
                    case JSToken.BitwiseNot:
                        return (object)~(int)Runtime.DoubleToInt64(unary);
                    case JSToken.FirstBinaryOp:
                        return (object)unary;
                    case JSToken.Minus:
                        return (object)-unary;
                    default:
                        throw new JScriptException(JSError.InternalError, this.context);
                }
            case TypeCode.String:
                switch (this.operatorTok)
                {
                    case JSToken.FirstOp:
                        return (object)!Convert.ToBoolean(v, iconvertible);
                    case JSToken.BitwiseNot:
                        return (object)~Convert.ToInt32(v, iconvertible);
                    case JSToken.FirstBinaryOp:
                        return (object)Convert.ToNumber(v, iconvertible);
                    case JSToken.Minus:
                        return (object)-Convert.ToNumber(v, iconvertible);
                    default:
                        throw new JScriptException(JSError.InternalError, this.context);
                }
            default:
                MethodInfo methodInfo = this.GetOperator((IReflect)v.GetType());
                if (methodInfo != (MethodInfo)null)
                    return methodInfo.Invoke((object)null, BindingFlags.Default, (Binder)JSBinder.ob, new object[1]
                    {
                        v
                    }, (CultureInfo)null);
                goto case TypeCode.String;
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
            case JSToken.FirstOp:
                this.operatorMeth = type.GetMethod("op_LogicalNot", BindingFlags.Static | BindingFlags.Public,
                    (Binder)JSBinder.ob, new Type[1]
                    {
                        type
                    }, (ParameterModifier[])null);
                break;
            case JSToken.BitwiseNot:
                this.operatorMeth = type.GetMethod("op_OnesComplement", BindingFlags.Static | BindingFlags.Public,
                    (Binder)JSBinder.ob, new Type[1]
                    {
                        type
                    }, (ParameterModifier[])null);
                break;
            case JSToken.FirstBinaryOp:
                this.operatorMeth = type.GetMethod("op_UnaryPlus", BindingFlags.Static | BindingFlags.Public,
                    (Binder)JSBinder.ob, new Type[1]
                    {
                        type
                    }, (ParameterModifier[])null);
                break;
            case JSToken.Minus:
                this.operatorMeth = type.GetMethod("op_UnaryNegation", BindingFlags.Static | BindingFlags.Public,
                    (Binder)JSBinder.ob, new Type[1]
                    {
                        type
                    }, (ParameterModifier[])null);
                break;
            default:
                throw new JScriptException(JSError.InternalError, this.context);
        }

        if (this.operatorMeth == (MethodInfo)null ||
            (this.operatorMeth.Attributes & MethodAttributes.SpecialName) == MethodAttributes.PrivateScope ||
            this.operatorMeth.GetParameters().Length != 1)
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

        if (this.operatorTok == JSToken.FirstOp)
            return (IReflect)Typeob.Boolean;
        switch (Type.GetTypeCode(this.type))
        {
            case TypeCode.Empty:
                return this.operatorTok != JSToken.BitwiseNot ? (IReflect)Typeob.Double : (IReflect)Typeob.Int32;
            case TypeCode.Object:
                return (IReflect)Typeob.Object;
            case TypeCode.DBNull:
                return (IReflect)Typeob.Int32;
            case TypeCode.Boolean:
                return (IReflect)Typeob.Int32;
            case TypeCode.Char:
                return (IReflect)Typeob.Int32;
            case TypeCode.SByte:
                return this.operatorTok != JSToken.BitwiseNot ? (IReflect)Typeob.Int32 : (IReflect)Typeob.SByte;
            case TypeCode.Byte:
                return this.operatorTok != JSToken.BitwiseNot ? (IReflect)Typeob.Int32 : (IReflect)Typeob.Byte;
            case TypeCode.Int16:
                return this.operatorTok != JSToken.BitwiseNot ? (IReflect)Typeob.Int32 : (IReflect)Typeob.Int16;
            case TypeCode.UInt16:
                return this.operatorTok != JSToken.BitwiseNot ? (IReflect)Typeob.Int32 : (IReflect)Typeob.UInt16;
            case TypeCode.Int32:
                return (IReflect)Typeob.Int32;
            case TypeCode.UInt32:
                return this.operatorTok != JSToken.Minus ? (IReflect)Typeob.UInt32 : (IReflect)Typeob.Double;
            case TypeCode.Int64:
                return (IReflect)Typeob.Int64;
            case TypeCode.UInt64:
                return this.operatorTok != JSToken.Minus ? (IReflect)Typeob.UInt64 : (IReflect)Typeob.Double;
            case TypeCode.Single:
            case TypeCode.Double:
            case TypeCode.String:
                return this.operatorTok != JSToken.BitwiseNot ? (IReflect)Typeob.Double : (IReflect)Typeob.Int32;
            default:
                return Typeob.JSObject.IsAssignableFrom(this.type) ? (IReflect)Typeob.Double : (IReflect)Typeob.Object;
        }
    }

    internal override void TranslateToConditionalBranch(
        ILGenerator il,
        bool branchIfTrue,
        Label label,
        bool shortForm)
    {
        // Short-form branches corrupt the IL emitted by PersistedAssemblyBuilder on
        // .NET 10.0.x (ControlFlowBuilder desyncs on an operand at a blob boundary).
        shortForm = false;
        if (this.operatorTok == JSToken.FirstOp)
            this.operand.TranslateToConditionalBranch(il, !branchIfTrue, label, shortForm);
        else
            base.TranslateToConditionalBranch(il, branchIfTrue, label, shortForm);
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        if (this.metaData == null)
        {
            Type type = this.operatorTok == JSToken.FirstOp ? Typeob.Boolean : Typeob.Double;
            if (Convert.IsPrimitiveNumericType((IReflect)rtype) &&
                Convert.IsPromotableTo((IReflect)this.type, (IReflect)rtype))
                type = rtype;
            if (this.operatorTok == JSToken.BitwiseNot && !Convert.IsPrimitiveIntegerType(type))
            {
                type = this.type;
                if (!Convert.IsPrimitiveIntegerType(type))
                    type = Typeob.Int32;
            }

            this.operand.TranslateToIL(il, this.type);
            Convert.Emit((AST)this, il, this.type, type, true);
            switch (this.operatorTok)
            {
                case JSToken.FirstOp:
                    Convert.Emit((AST)this, il, type, Typeob.Boolean, true);
                    type = Typeob.Boolean;
                    il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Ceq);
                    goto case JSToken.FirstBinaryOp;
                case JSToken.BitwiseNot:
                    il.Emit(OpCodes.Not);
                    goto case JSToken.FirstBinaryOp;
                case JSToken.FirstBinaryOp:
                    Convert.Emit((AST)this, il, type, rtype);
                    break;
                case JSToken.Minus:
                    il.Emit(OpCodes.Neg);
                    goto case JSToken.FirstBinaryOp;
                default:
                    throw new JScriptException(JSError.InternalError, this.context);
            }
        }
        else if ((object)(this.metaData as MethodInfo) != null)
        {
            MethodInfo metaData = (MethodInfo)this.metaData;
            ParameterInfo[] parameters = metaData.GetParameters();
            this.operand.TranslateToIL(il, parameters[0].ParameterType);
            il.Emit(OpCodes.Call, metaData);
            Convert.Emit((AST)this, il, metaData.ReturnType, rtype);
        }
        else
        {
            il.Emit(OpCodes.Ldloc, (LocalBuilder)this.metaData);
            this.operand.TranslateToIL(il, Typeob.Object);
            il.Emit(OpCodes.Call, CompilerGlobals.evaluateUnaryMethod);
            Convert.Emit((AST)this, il, Typeob.Object, rtype);
        }
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
        IReflect reflect = this.InferType((JSField)null);
        this.operand.TranslateToILInitializer(il);
        if ((object)reflect != (object)Typeob.Object)
            return;
        this.metaData = (object)il.DeclareLocal(Typeob.NumericUnary);
        ConstantWrapper.TranslateToILInt(il, (int)this.operatorTok);
        il.Emit(OpCodes.Newobj, CompilerGlobals.numericUnaryConstructor);
        il.Emit(OpCodes.Stloc, (LocalBuilder)this.metaData);
    }
}