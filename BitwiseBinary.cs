// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.BitwiseBinary
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

/// <summary>Performs bitwise binary operations. This class belongs to the abstract syntax tree category.</summary>
public sealed class BitwiseBinary : BinaryOp
{
  private object metaData;

  internal BitwiseBinary(Context context, AST operand1, AST operand2, JSToken operatorTok)
    : base(context, operand1, operand2, operatorTok)
  {
  }

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.BitwiseBinary" /> class.</summary>
  /// <param name="operatorTok">One of the <see cref="T:Microsoft.JScript.JSToken" /> enumeration values. Specifies the operation to perform.</param>
  public BitwiseBinary(int operatorTok)
    : base((Context) null, (AST) null, (AST) null, (JSToken) operatorTok)
  {
  }

  internal override object Evaluate()
  {
    return this.EvaluateBitwiseBinary(this.operand1.Evaluate(), this.operand2.Evaluate());
  }

  /// <summary>Performs a bitwise binary operation.</summary>
  /// <returns>The result of the operation.</returns>
  /// <param name="v1">The first operand.</param>
  /// <param name="v2">The second operand.</param>
  [DebuggerStepThrough]
  [DebuggerHidden]
  public object EvaluateBitwiseBinary(object v1, object v2)
  {
    return v1 is int && v2 is int j ? BitwiseBinary.DoOp((int) v1, j, this.operatorTok) : this.EvaluateBitwiseBinary(v1, v2, this.operatorTok);
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  private object EvaluateBitwiseBinary(object v1, object v2, JSToken operatorTok)
  {
    IConvertible iconvertible1 = Convert.GetIConvertible(v1);
    IConvertible iconvertible2 = Convert.GetIConvertible(v2);
    TypeCode typeCode1 = Convert.GetTypeCode(v1, iconvertible1);
    TypeCode typeCode2 = Convert.GetTypeCode(v2, iconvertible2);
    switch (typeCode1)
    {
      case TypeCode.Empty:
      case TypeCode.DBNull:
        return this.EvaluateBitwiseBinary((object) 0, v2, operatorTok);
      case TypeCode.Boolean:
      case TypeCode.Char:
      case TypeCode.SByte:
      case TypeCode.Byte:
      case TypeCode.Int16:
      case TypeCode.UInt16:
      case TypeCode.Int32:
        int int32 = iconvertible1.ToInt32((IFormatProvider) null);
        switch (typeCode2)
        {
          case TypeCode.Empty:
          case TypeCode.DBNull:
            return BitwiseBinary.DoOp(int32, 0, operatorTok);
          case TypeCode.Boolean:
          case TypeCode.Char:
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.UInt16:
          case TypeCode.Int32:
            return BitwiseBinary.DoOp(int32, iconvertible2.ToInt32((IFormatProvider) null), operatorTok);
          case TypeCode.UInt32:
          case TypeCode.Int64:
          case TypeCode.UInt64:
          case TypeCode.Single:
          case TypeCode.Double:
            return BitwiseBinary.DoOp(int32, (int) Runtime.DoubleToInt64(iconvertible2.ToDouble((IFormatProvider) null)), operatorTok);
        }
        break;
      case TypeCode.UInt32:
      case TypeCode.Int64:
      case TypeCode.UInt64:
      case TypeCode.Single:
      case TypeCode.Double:
        int int64 = (int) Runtime.DoubleToInt64(iconvertible1.ToDouble((IFormatProvider) null));
        switch (typeCode2)
        {
          case TypeCode.Empty:
          case TypeCode.DBNull:
            return BitwiseBinary.DoOp(int64, 0, operatorTok);
          case TypeCode.Boolean:
          case TypeCode.Char:
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.UInt16:
          case TypeCode.Int32:
            return BitwiseBinary.DoOp(int64, iconvertible2.ToInt32((IFormatProvider) null), operatorTok);
          case TypeCode.UInt32:
          case TypeCode.Int64:
          case TypeCode.UInt64:
          case TypeCode.Single:
          case TypeCode.Double:
            return BitwiseBinary.DoOp(int64, (int) Runtime.DoubleToInt64(iconvertible2.ToDouble((IFormatProvider) null)), operatorTok);
        }
        break;
    }
    if (v2 == null)
      return BitwiseBinary.DoOp(Convert.ToInt32(v1), 0, this.operatorTok);
    MethodInfo methodInfo = this.GetOperator((IReflect) v1.GetType(), (IReflect) v2.GetType());
    if (!(methodInfo != (MethodInfo) null))
      return BitwiseBinary.DoOp(Convert.ToInt32(v1), Convert.ToInt32(v2), this.operatorTok);
    return methodInfo.Invoke((object) null, BindingFlags.Default, (Binder) JSBinder.ob, new object[2]
    {
      v1,
      v2
    }, (CultureInfo) null);
  }

  internal static object DoOp(int i, int j, JSToken operatorTok)
  {
    switch (operatorTok)
    {
      case JSToken.BitwiseOr:
        return (object) (i | j);
      case JSToken.BitwiseXor:
        return (object) (i ^ j);
      case JSToken.BitwiseAnd:
        return (object) (i & j);
      case JSToken.LeftShift:
        return (object) (i << j);
      case JSToken.RightShift:
        return (object) (i >> j);
      case JSToken.UnsignedRightShift:
        return (object) (uint) (i >>> j);
      default:
        throw new JScriptException(JSError.InternalError);
    }
  }

  internal override IReflect InferType(JSField inference_target)
  {
    MethodInfo methodInfo = this.type1 == (Type) null || (FieldInfo) inference_target != (FieldInfo) null ? this.GetOperator(this.operand1.InferType(inference_target), this.operand2.InferType(inference_target)) : this.GetOperator((IReflect) this.type1, (IReflect) this.type2);
    if (!(methodInfo != (MethodInfo) null))
      return (IReflect) BitwiseBinary.ResultType(this.type1, this.type2, this.operatorTok);
    this.metaData = (object) methodInfo;
    return (IReflect) methodInfo.ReturnType;
  }

  internal static Type Operand2Type(JSToken operatorTok, Type bbrType)
  {
    switch (operatorTok)
    {
      case JSToken.LeftShift:
      case JSToken.RightShift:
      case JSToken.UnsignedRightShift:
        return Typeob.Int32;
      default:
        return bbrType;
    }
  }

  internal static Type ResultType(Type type1, Type type2, JSToken operatorTok)
  {
    switch (operatorTok)
    {
      case JSToken.LeftShift:
      case JSToken.RightShift:
        if (Convert.IsPrimitiveIntegerType(type1))
          return type1;
        return Typeob.JSObject.IsAssignableFrom(type1) ? Typeob.Int32 : Typeob.Object;
      case JSToken.UnsignedRightShift:
        switch (Type.GetTypeCode(type1))
        {
          case TypeCode.SByte:
          case TypeCode.Byte:
            return Typeob.Byte;
          case TypeCode.Int16:
          case TypeCode.UInt16:
            return Typeob.UInt16;
          case TypeCode.Int32:
          case TypeCode.UInt32:
            return Typeob.UInt32;
          case TypeCode.Int64:
          case TypeCode.UInt64:
            return Typeob.UInt64;
          default:
            return Typeob.JSObject.IsAssignableFrom(type1) ? Typeob.Int32 : Typeob.Object;
        }
      default:
        TypeCode typeCode1 = Type.GetTypeCode(type1);
        TypeCode typeCode2 = Type.GetTypeCode(type2);
        switch (typeCode1)
        {
          case TypeCode.Empty:
          case TypeCode.DBNull:
          case TypeCode.Boolean:
            switch (typeCode2)
            {
              case TypeCode.Empty:
              case TypeCode.DBNull:
              case TypeCode.Boolean:
              case TypeCode.Int32:
              case TypeCode.Single:
              case TypeCode.Double:
                return Typeob.Int32;
              case TypeCode.Object:
                if (Typeob.JSObject.IsAssignableFrom(type2))
                  return Typeob.Int32;
                break;
              case TypeCode.Char:
              case TypeCode.UInt16:
                return Typeob.UInt16;
              case TypeCode.SByte:
                return Typeob.SByte;
              case TypeCode.Byte:
                return Typeob.Byte;
              case TypeCode.Int16:
                return Typeob.Int16;
              case TypeCode.UInt32:
                return Typeob.UInt32;
              case TypeCode.Int64:
                return Typeob.Int64;
              case TypeCode.UInt64:
                return Typeob.UInt64;
            }
            break;
          case TypeCode.Object:
            if (Typeob.JSObject.IsAssignableFrom(type1))
              return Typeob.Int32;
            break;
          case TypeCode.Char:
          case TypeCode.UInt16:
            switch (typeCode2)
            {
              case TypeCode.Empty:
              case TypeCode.DBNull:
              case TypeCode.Boolean:
              case TypeCode.Char:
              case TypeCode.SByte:
              case TypeCode.Byte:
              case TypeCode.Int16:
              case TypeCode.UInt16:
                return Typeob.UInt16;
              case TypeCode.Object:
                if (Typeob.JSObject.IsAssignableFrom(type2))
                  return Typeob.UInt32;
                break;
              case TypeCode.Int32:
              case TypeCode.UInt32:
              case TypeCode.Single:
              case TypeCode.Double:
                return Typeob.UInt32;
              case TypeCode.Int64:
              case TypeCode.UInt64:
                return Typeob.UInt64;
            }
            break;
          case TypeCode.SByte:
            switch (typeCode2)
            {
              case TypeCode.Empty:
              case TypeCode.DBNull:
              case TypeCode.Boolean:
              case TypeCode.SByte:
                return Typeob.SByte;
              case TypeCode.Object:
                if (Typeob.JSObject.IsAssignableFrom(type2))
                  return Typeob.Int32;
                break;
              case TypeCode.Char:
              case TypeCode.Int16:
                return Typeob.Int16;
              case TypeCode.Byte:
                return Typeob.Byte;
              case TypeCode.UInt16:
                return Typeob.UInt16;
              case TypeCode.Int32:
              case TypeCode.Single:
              case TypeCode.Double:
                return Typeob.Int32;
              case TypeCode.UInt32:
                return Typeob.UInt32;
              case TypeCode.Int64:
                return Typeob.Int64;
              case TypeCode.UInt64:
                return Typeob.UInt64;
            }
            break;
          case TypeCode.Byte:
            switch (typeCode2)
            {
              case TypeCode.Empty:
              case TypeCode.DBNull:
              case TypeCode.Boolean:
              case TypeCode.SByte:
              case TypeCode.Byte:
                return Typeob.Byte;
              case TypeCode.Object:
                if (Typeob.JSObject.IsAssignableFrom(type2))
                  return Typeob.UInt32;
                break;
              case TypeCode.Char:
              case TypeCode.Int16:
              case TypeCode.UInt16:
                return Typeob.UInt16;
              case TypeCode.Int32:
              case TypeCode.UInt32:
              case TypeCode.Single:
              case TypeCode.Double:
                return Typeob.UInt32;
              case TypeCode.Int64:
              case TypeCode.UInt64:
                return Typeob.UInt64;
            }
            break;
          case TypeCode.Int16:
            switch (typeCode2)
            {
              case TypeCode.Empty:
              case TypeCode.DBNull:
              case TypeCode.Boolean:
              case TypeCode.SByte:
              case TypeCode.Int16:
                return Typeob.Int16;
              case TypeCode.Object:
                if (Typeob.JSObject.IsAssignableFrom(type2))
                  return Typeob.Int32;
                break;
              case TypeCode.Char:
              case TypeCode.Byte:
              case TypeCode.UInt16:
                return Typeob.UInt16;
              case TypeCode.Int32:
              case TypeCode.Single:
              case TypeCode.Double:
                return Typeob.Int32;
              case TypeCode.UInt32:
                return Typeob.UInt32;
              case TypeCode.Int64:
                return Typeob.Int64;
              case TypeCode.UInt64:
                return Typeob.UInt64;
            }
            break;
          case TypeCode.Int32:
          case TypeCode.Single:
          case TypeCode.Double:
            switch (typeCode2)
            {
              case TypeCode.Empty:
              case TypeCode.DBNull:
              case TypeCode.Boolean:
              case TypeCode.SByte:
              case TypeCode.Int16:
              case TypeCode.Int32:
              case TypeCode.Single:
              case TypeCode.Double:
                return Typeob.Int32;
              case TypeCode.Object:
                if (Typeob.JSObject.IsAssignableFrom(type2))
                  return Typeob.Int32;
                break;
              case TypeCode.Char:
              case TypeCode.Byte:
              case TypeCode.UInt16:
              case TypeCode.UInt32:
                return Typeob.UInt32;
              case TypeCode.Int64:
                return Typeob.Int64;
              case TypeCode.UInt64:
                return Typeob.UInt64;
            }
            break;
          case TypeCode.UInt32:
            switch (typeCode2)
            {
              case TypeCode.Empty:
              case TypeCode.DBNull:
              case TypeCode.Boolean:
              case TypeCode.Char:
              case TypeCode.SByte:
              case TypeCode.Byte:
              case TypeCode.Int16:
              case TypeCode.UInt16:
              case TypeCode.Int32:
              case TypeCode.UInt32:
              case TypeCode.Single:
              case TypeCode.Double:
                return Typeob.UInt32;
              case TypeCode.Object:
                if (Typeob.JSObject.IsAssignableFrom(type2))
                  return Typeob.UInt32;
                break;
              case TypeCode.Int64:
              case TypeCode.UInt64:
                return Typeob.UInt64;
            }
            break;
          case TypeCode.Int64:
            switch (typeCode2)
            {
              case TypeCode.Empty:
              case TypeCode.DBNull:
              case TypeCode.Boolean:
              case TypeCode.SByte:
              case TypeCode.Int16:
              case TypeCode.Int32:
              case TypeCode.Int64:
              case TypeCode.Single:
              case TypeCode.Double:
                return Typeob.Int64;
              case TypeCode.Object:
                if (Typeob.JSObject.IsAssignableFrom(type2))
                  return Typeob.Int64;
                break;
              case TypeCode.Char:
              case TypeCode.Byte:
              case TypeCode.UInt16:
              case TypeCode.UInt32:
              case TypeCode.UInt64:
                return Typeob.UInt64;
            }
            break;
          case TypeCode.UInt64:
            switch (typeCode2)
            {
              case TypeCode.Empty:
              case TypeCode.DBNull:
              case TypeCode.Boolean:
              case TypeCode.Char:
              case TypeCode.SByte:
              case TypeCode.Byte:
              case TypeCode.Int16:
              case TypeCode.UInt16:
              case TypeCode.Int32:
              case TypeCode.UInt32:
              case TypeCode.Int64:
              case TypeCode.UInt64:
              case TypeCode.Single:
              case TypeCode.Double:
                return Typeob.UInt64;
              case TypeCode.Object:
                if (Typeob.JSObject.IsAssignableFrom(type2))
                  return Typeob.UInt64;
                break;
            }
            break;
        }
        return Typeob.Object;
    }
  }

  internal static void TranslateToBitCountMask(ILGenerator il, Type type, AST operand2)
  {
    int num = 0;
    switch (Type.GetTypeCode(type))
    {
      case TypeCode.SByte:
      case TypeCode.Byte:
        num = 7;
        break;
      case TypeCode.Int16:
      case TypeCode.UInt16:
        num = 15;
        break;
      case TypeCode.Int32:
      case TypeCode.UInt32:
        num = 31 /*0x1F*/;
        break;
      case TypeCode.Int64:
      case TypeCode.UInt64:
        num = 63 /*0x3F*/;
        break;
    }
    if (operand2 is ConstantWrapper constantWrapper && Convert.ToInt32(constantWrapper.value) <= num)
      return;
    il.Emit(OpCodes.Ldc_I4_S, num);
    il.Emit(OpCodes.And);
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    if (this.metaData == null)
    {
      Type type = BitwiseBinary.ResultType(this.type1, this.type2, this.operatorTok);
      if (Convert.IsPrimitiveNumericType((IReflect) this.type1))
      {
        this.operand1.TranslateToIL(il, this.type1);
        Convert.Emit((AST) this, il, this.type1, type, true);
      }
      else
      {
        this.operand1.TranslateToIL(il, Typeob.Double);
        Convert.Emit((AST) this, il, Typeob.Double, type, true);
      }
      Type target_type = BitwiseBinary.Operand2Type(this.operatorTok, type);
      if (Convert.IsPrimitiveNumericType((IReflect) this.type2))
      {
        this.operand2.TranslateToIL(il, this.type2);
        Convert.Emit((AST) this, il, this.type2, target_type, true);
      }
      else
      {
        this.operand2.TranslateToIL(il, Typeob.Double);
        Convert.Emit((AST) this, il, Typeob.Double, target_type, true);
      }
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
          BitwiseBinary.TranslateToBitCountMask(il, type, this.operand2);
          il.Emit(OpCodes.Shl);
          break;
        case JSToken.RightShift:
          BitwiseBinary.TranslateToBitCountMask(il, type, this.operand2);
          il.Emit(OpCodes.Shr);
          break;
        case JSToken.UnsignedRightShift:
          BitwiseBinary.TranslateToBitCountMask(il, type, this.operand2);
          il.Emit(OpCodes.Shr_Un);
          break;
        default:
          throw new JScriptException(JSError.InternalError, this.context);
      }
      Convert.Emit((AST) this, il, type, rtype);
    }
    else if ((object) (this.metaData as MethodInfo) != null)
    {
      MethodInfo metaData = (MethodInfo) this.metaData;
      ParameterInfo[] parameters = metaData.GetParameters();
      this.operand1.TranslateToIL(il, parameters[0].ParameterType);
      this.operand2.TranslateToIL(il, parameters[1].ParameterType);
      il.Emit(OpCodes.Call, metaData);
      Convert.Emit((AST) this, il, metaData.ReturnType, rtype);
    }
    else
    {
      il.Emit(OpCodes.Ldloc, (LocalBuilder) this.metaData);
      this.operand1.TranslateToIL(il, Typeob.Object);
      this.operand2.TranslateToIL(il, Typeob.Object);
      il.Emit(OpCodes.Call, CompilerGlobals.evaluateBitwiseBinaryMethod);
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
