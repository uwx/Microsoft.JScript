// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.BitwiseBinaryAssign
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class BitwiseBinaryAssign : BinaryOp
{
  private BitwiseBinary binOp;
  private object metaData;

  internal BitwiseBinaryAssign(Context context, AST operand1, AST operand2, JSToken operatorTok)
    : base(context, operand1, operand2, operatorTok)
  {
    this.binOp = new BitwiseBinary(context, operand1, operand2, operatorTok);
    this.metaData = (object) null;
  }

  internal override object Evaluate()
  {
    object bitwiseBinary = this.binOp.EvaluateBitwiseBinary(this.operand1.Evaluate(), this.operand2.Evaluate());
    try
    {
      this.operand1.SetValue(bitwiseBinary);
      return bitwiseBinary;
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
    MethodInfo methodInfo = !(this.type1 == (Type) null) ? this.GetOperator((IReflect) this.type1, (IReflect) this.type2) : this.GetOperator(this.operand1.InferType(inference_target), this.operand2.InferType(inference_target));
    if (methodInfo != (MethodInfo) null)
    {
      this.metaData = (object) methodInfo;
      return (IReflect) methodInfo.ReturnType;
    }
    return (this.type1.IsPrimitive || Typeob.JSObject.IsAssignableFrom(this.type1)) && (this.type2.IsPrimitive || Typeob.JSObject.IsAssignableFrom(this.type2)) ? (IReflect) Typeob.Int32 : (IReflect) Typeob.Object;
  }

  internal override AST PartiallyEvaluate()
  {
    this.operand1 = this.operand1.PartiallyEvaluateAsReference();
    this.operand2 = this.operand2.PartiallyEvaluate();
    this.binOp = new BitwiseBinary(this.context, this.operand1, this.operand2, this.operatorTok);
    this.operand1.SetPartialValue((AST) this.binOp);
    return (AST) this;
  }

  private void TranslateToILForNoOverloadCase(ILGenerator il, Type rtype)
  {
    Type type1 = Convert.ToType(this.operand1.InferType((JSField) null));
    Type type2 = Convert.ToType(this.operand2.InferType((JSField) null));
    Type type3 = BitwiseBinary.ResultType(type1, type2, this.operatorTok);
    this.operand1.TranslateToILPreSetPlusGet(il);
    Convert.Emit((AST) this, il, type1, type3, true);
    this.operand2.TranslateToIL(il, type2);
    Convert.Emit((AST) this, il, type2, BitwiseBinary.Operand2Type(this.operatorTok, type3), true);
    switch (this.operatorTok)
    {
      case JSToken.BitwiseOr:
        il.Emit(OpCodes.Or);
        break;
      case JSToken.BitwiseXor:
        il.Emit(OpCodes.Xor);
        break;
      case JSToken.BitwiseAnd:
        il.Emit(OpCodes.And);
        break;
      case JSToken.LeftShift:
        BitwiseBinary.TranslateToBitCountMask(il, type3, this.operand2);
        il.Emit(OpCodes.Shl);
        break;
      case JSToken.RightShift:
        BitwiseBinary.TranslateToBitCountMask(il, type3, this.operand2);
        il.Emit(OpCodes.Shr);
        break;
      case JSToken.UnsignedRightShift:
        BitwiseBinary.TranslateToBitCountMask(il, type3, this.operand2);
        il.Emit(OpCodes.Shr_Un);
        break;
      default:
        throw new JScriptException(JSError.InternalError, this.context);
    }
    if (rtype != Typeob.Void)
    {
      LocalBuilder local = il.DeclareLocal(type3);
      il.Emit(OpCodes.Dup);
      il.Emit(OpCodes.Stloc, local);
      Convert.Emit((AST) this, il, type3, type1);
      this.operand1.TranslateToILSet(il);
      il.Emit(OpCodes.Ldloc, local);
      Convert.Emit((AST) this, il, type3, rtype);
    }
    else
    {
      Convert.Emit((AST) this, il, type3, type1);
      this.operand1.TranslateToILSet(il);
    }
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    if (this.metaData == null)
      this.TranslateToILForNoOverloadCase(il, rtype);
    else if ((object) (this.metaData as MethodInfo) != null)
    {
      object local = (object) null;
      MethodInfo metaData = (MethodInfo) this.metaData;
      Type type = Convert.ToType(this.operand1.InferType((JSField) null));
      ParameterInfo[] parameters = metaData.GetParameters();
      this.operand1.TranslateToILPreSetPlusGet(il);
      Convert.Emit((AST) this, il, type, parameters[0].ParameterType);
      this.operand2.TranslateToIL(il, parameters[1].ParameterType);
      il.Emit(OpCodes.Call, metaData);
      if (rtype != Typeob.Void)
      {
        local = (object) il.DeclareLocal(rtype);
        il.Emit(OpCodes.Dup);
        Convert.Emit((AST) this, il, type, rtype);
        il.Emit(OpCodes.Stloc, (LocalBuilder) local);
      }
      Convert.Emit((AST) this, il, metaData.ReturnType, type);
      this.operand1.TranslateToILSet(il);
      if (!(rtype != Typeob.Void))
        return;
      il.Emit(OpCodes.Ldloc, (LocalBuilder) local);
    }
    else
    {
      Type type = Convert.ToType(this.operand1.InferType((JSField) null));
      LocalBuilder local = il.DeclareLocal(Typeob.Object);
      this.operand1.TranslateToILPreSetPlusGet(il);
      Convert.Emit((AST) this, il, type, Typeob.Object);
      il.Emit(OpCodes.Stloc, local);
      il.Emit(OpCodes.Ldloc, (LocalBuilder) this.metaData);
      il.Emit(OpCodes.Ldloc, local);
      this.operand2.TranslateToIL(il, Typeob.Object);
      il.Emit(OpCodes.Call, CompilerGlobals.evaluateBitwiseBinaryMethod);
      if (rtype != Typeob.Void)
      {
        il.Emit(OpCodes.Dup);
        il.Emit(OpCodes.Stloc, local);
      }
      Convert.Emit((AST) this, il, Typeob.Object, type);
      this.operand1.TranslateToILSet(il);
      if (!(rtype != Typeob.Void))
        return;
      il.Emit(OpCodes.Ldloc, local);
      Convert.Emit((AST) this, il, Typeob.Object, rtype);
    }
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
    IReflect reflect = this.InferType((JSField) null);
    this.operand1.TranslateToILInitializer(il);
    this.operand2.TranslateToILInitializer(il);
    if ((object) reflect != (object) Typeob.Object)
      return;
    this.metaData = (object) il.DeclareLocal(Typeob.BitwiseBinary);
    ConstantWrapper.TranslateToILInt(il, (int) this.operatorTok);
    il.Emit(OpCodes.Newobj, CompilerGlobals.bitwiseBinaryConstructor);
    il.Emit(OpCodes.Stloc, (LocalBuilder) this.metaData);
  }
}
