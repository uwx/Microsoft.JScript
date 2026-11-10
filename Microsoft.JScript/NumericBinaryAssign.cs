// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.NumericBinaryAssign
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal class NumericBinaryAssign : BinaryOp
{
    private NumericBinary binOp;
    private object metaData;

    internal NumericBinaryAssign(Context context, AST operand1, AST operand2, JSToken operatorTok)
        : base(context, operand1, operand2, operatorTok)
    {
        this.binOp = new NumericBinary(context, operand1, operand2, operatorTok);
        this.metaData = (object)null;
    }

    internal override object Evaluate()
    {
        object numericBinary = this.binOp.EvaluateNumericBinary(this.operand1.Evaluate(), this.operand2.Evaluate());
        try
        {
            this.operand1.SetValue(numericBinary);
            return numericBinary;
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

    internal override IReflect InferType(JSField inference_target)
    {
        MethodInfo methodInfo = this.type1 == (Type)null || (FieldInfo)inference_target != (FieldInfo)null
            ? this.GetOperator(this.operand1.InferType(inference_target), this.operand2.InferType(inference_target))
            : this.GetOperator((IReflect)this.type1, (IReflect)this.type2);
        if (methodInfo != (MethodInfo)null)
        {
            this.metaData = (object)methodInfo;
            return (IReflect)methodInfo.ReturnType;
        }

        if (this.type1 == Typeob.Char && this.operatorTok == JSToken.Minus)
        {
            TypeCode typeCode = Type.GetTypeCode(this.type2);
            if (Convert.IsPrimitiveNumericTypeCode(typeCode) || typeCode == TypeCode.Boolean)
                return (IReflect)Typeob.Char;
            if (typeCode == TypeCode.Char)
                return (IReflect)Typeob.Int32;
        }

        if (Convert.IsPrimitiveNumericType((IReflect)this.type1))
        {
            if (Convert.IsPromotableTo((IReflect)this.type2, (IReflect)this.type1) ||
                this.operand2 is ConstantWrapper && ((ConstantWrapper)this.operand2).IsAssignableTo(this.type1))
                return (IReflect)this.type1;
            if (Convert.IsPrimitiveNumericType((IReflect)this.type1) &&
                Convert.IsPrimitiveNumericTypeFitForDouble((IReflect)this.type2))
                return (IReflect)Typeob.Double;
        }

        return (IReflect)Typeob.Object;
    }

    internal override AST PartiallyEvaluate()
    {
        this.operand1 = this.operand1.PartiallyEvaluateAsReference();
        this.operand2 = this.operand2.PartiallyEvaluate();
        this.binOp = new NumericBinary(this.context, this.operand1, this.operand2, this.operatorTok);
        this.operand1.SetPartialValue((AST)this.binOp);
        return (AST)this;
    }

    private void TranslateToILForNoOverloadCase(ILGenerator il, Type rtype)
    {
        Type type1 = Convert.ToType(this.operand1.InferType((JSField)null));
        Type type2 = Convert.ToType(this.operand2.InferType((JSField)null));
        Type type3 = Typeob.Double;
        if (this.operatorTok != JSToken.Divide &&
            (rtype == Typeob.Void || rtype == type1 || Convert.IsPrimitiveNumericType((IReflect)type1)) &&
            (Convert.IsPromotableTo((IReflect)type2, (IReflect)type1) || this.operand2 is ConstantWrapper &&
                ((ConstantWrapper)this.operand2).IsAssignableTo(type1)))
            type3 = type1;
        if (type3 == Typeob.SByte || type3 == Typeob.Int16)
            type3 = Typeob.Int32;
        else if (type3 == Typeob.Byte || type3 == Typeob.UInt16 || type3 == Typeob.Char)
            type3 = Typeob.UInt32;
        if (this.operand2 is ConstantWrapper)
        {
            if (!((ConstantWrapper)this.operand2).IsAssignableTo(type3))
                type3 = Typeob.Object;
        }
        else if (Convert.IsPrimitiveSignedNumericType(type2) && Convert.IsPrimitiveUnsignedIntegerType(type1) ||
                 Convert.IsPrimitiveUnsignedIntegerType(type2) && Convert.IsPrimitiveSignedIntegerType(type1))
            type3 = Typeob.Object;

        this.operand1.TranslateToILPreSetPlusGet(il);
        Convert.Emit((AST)this, il, type1, type3);
        this.operand2.TranslateToIL(il, type3);
        if (type3 == Typeob.Object)
        {
            il.Emit(OpCodes.Ldc_I4, (int)this.operatorTok);
            il.Emit(OpCodes.Call, CompilerGlobals.numericbinaryDoOpMethod);
        }
        else if (type3 == Typeob.Double || type3 == Typeob.Single)
        {
            switch (this.operatorTok)
            {
                case JSToken.Minus:
                    il.Emit(OpCodes.Sub);
                    break;
                case JSToken.Multiply:
                    il.Emit(OpCodes.Mul);
                    break;
                case JSToken.Divide:
                    il.Emit(OpCodes.Div);
                    break;
                case JSToken.Modulo:
                    il.Emit(OpCodes.Rem);
                    break;
                default:
                    throw new JScriptException(JSError.InternalError, this.context);
            }
        }
        else if (type3 == Typeob.Int32 || type3 == Typeob.Int64 || type3 == Typeob.Int16 || type3 == Typeob.SByte)
        {
            switch (this.operatorTok)
            {
                case JSToken.Minus:
                    il.Emit(OpCodes.Sub_Ovf);
                    break;
                case JSToken.Multiply:
                    il.Emit(OpCodes.Mul_Ovf);
                    break;
                case JSToken.Divide:
                    il.Emit(OpCodes.Div);
                    break;
                case JSToken.Modulo:
                    il.Emit(OpCodes.Rem);
                    break;
                default:
                    throw new JScriptException(JSError.InternalError, this.context);
            }
        }
        else
        {
            switch (this.operatorTok)
            {
                case JSToken.Minus:
                    il.Emit(OpCodes.Sub_Ovf_Un);
                    break;
                case JSToken.Multiply:
                    il.Emit(OpCodes.Mul_Ovf_Un);
                    break;
                case JSToken.Divide:
                    il.Emit(OpCodes.Div);
                    break;
                case JSToken.Modulo:
                    il.Emit(OpCodes.Rem);
                    break;
                default:
                    throw new JScriptException(JSError.InternalError, this.context);
            }
        }

        if (rtype != Typeob.Void)
        {
            LocalBuilder local = il.DeclareLocal(type3);
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Stloc, local);
            Convert.Emit((AST)this, il, type3, type1);
            this.operand1.TranslateToILSet(il);
            il.Emit(OpCodes.Ldloc, local);
            Convert.Emit((AST)this, il, type3, rtype);
        }
        else
        {
            Convert.Emit((AST)this, il, type3, type1);
            this.operand1.TranslateToILSet(il);
        }
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        if (this.metaData == null)
            this.TranslateToILForNoOverloadCase(il, rtype);
        else if ((object)(this.metaData as MethodInfo) != null)
        {
            object local = (object)null;
            MethodInfo metaData = (MethodInfo)this.metaData;
            Type type = Convert.ToType(this.operand1.InferType((JSField)null));
            ParameterInfo[] parameters = metaData.GetParameters();
            this.operand1.TranslateToILPreSetPlusGet(il);
            Convert.Emit((AST)this, il, type, parameters[0].ParameterType);
            this.operand2.TranslateToIL(il, parameters[1].ParameterType);
            il.Emit(OpCodes.Call, metaData);
            if (rtype != Typeob.Void)
            {
                local = (object)il.DeclareLocal(rtype);
                il.Emit(OpCodes.Dup);
                Convert.Emit((AST)this, il, type, rtype);
                il.Emit(OpCodes.Stloc, (LocalBuilder)local);
            }

            Convert.Emit((AST)this, il, metaData.ReturnType, type);
            this.operand1.TranslateToILSet(il);
            if (!(rtype != Typeob.Void))
                return;
            il.Emit(OpCodes.Ldloc, (LocalBuilder)local);
        }
        else
        {
            Type type = Convert.ToType(this.operand1.InferType((JSField)null));
            LocalBuilder local = il.DeclareLocal(Typeob.Object);
            this.operand1.TranslateToILPreSetPlusGet(il);
            Convert.Emit((AST)this, il, type, Typeob.Object);
            il.Emit(OpCodes.Stloc, local);
            il.Emit(OpCodes.Ldloc, (LocalBuilder)this.metaData);
            il.Emit(OpCodes.Ldloc, local);
            this.operand2.TranslateToIL(il, Typeob.Object);
            il.Emit(OpCodes.Call, CompilerGlobals.evaluateNumericBinaryMethod);
            if (rtype != Typeob.Void)
            {
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Stloc, local);
            }

            Convert.Emit((AST)this, il, Typeob.Object, type);
            this.operand1.TranslateToILSet(il);
            if (!(rtype != Typeob.Void))
                return;
            il.Emit(OpCodes.Ldloc, local);
            Convert.Emit((AST)this, il, Typeob.Object, rtype);
        }
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
        IReflect reflect = this.InferType((JSField)null);
        this.operand1.TranslateToILInitializer(il);
        this.operand2.TranslateToILInitializer(il);
        if ((object)reflect != (object)Typeob.Object)
            return;
        this.metaData = (object)il.DeclareLocal(Typeob.NumericBinary);
        ConstantWrapper.TranslateToILInt(il, (int)this.operatorTok);
        il.Emit(OpCodes.Newobj, CompilerGlobals.numericBinaryConstructor);
        il.Emit(OpCodes.Stloc, (LocalBuilder)this.metaData);
    }
}