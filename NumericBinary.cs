// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.NumericBinary
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

/// <summary>Performs binary operations on numeric operands. This class belongs to the abstract syntax tree category.</summary>
public sealed class NumericBinary : BinaryOp
{
  private object metaData;

  internal NumericBinary(Context context, AST operand1, AST operand2, JSToken operatorTok)
    : base(context, operand1, operand2, operatorTok)
  {
  }

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.NumericBinary" /> class.</summary>
  /// <param name="operatorTok">One of the <see cref="T:Microsoft.JScript.JSToken" /> enumeration values. Specifies the operation to perform.</param>
  public NumericBinary(int operatorTok)
    : base((Context) null, (AST) null, (AST) null, (JSToken) operatorTok)
  {
  }

  internal override object Evaluate()
  {
    return this.EvaluateNumericBinary(this.operand1.Evaluate(), this.operand2.Evaluate());
  }

  /// <summary>Performs a binary operation on the specified numeric operands.</summary>
  /// <returns>The result of the operation.</returns>
  /// <param name="v1">The first operand.</param>
  /// <param name="v2">The second operand.</param>
  [DebuggerStepThrough]
  [DebuggerHidden]
  public object EvaluateNumericBinary(object v1, object v2)
  {
    switch (v1)
    {
      case int _ when v2 is int y1:
        return NumericBinary.DoOp((int) v1, y1, this.operatorTok);
      case double _ when v2 is double y2:
        return NumericBinary.DoOp((double) v1, y2, this.operatorTok);
      default:
        return this.EvaluateNumericBinary(v1, v2, this.operatorTok);
    }
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  private object EvaluateNumericBinary(object v1, object v2, JSToken operatorTok)
  {
    IConvertible iconvertible1 = Convert.GetIConvertible(v1);
    IConvertible iconvertible2 = Convert.GetIConvertible(v2);
    TypeCode typeCode1 = Convert.GetTypeCode(v1, iconvertible1);
    TypeCode typeCode2 = Convert.GetTypeCode(v2, iconvertible2);
    switch (typeCode1)
    {
      case TypeCode.Empty:
        return (object) double.NaN;
      case TypeCode.DBNull:
        return this.EvaluateNumericBinary((object) 0, v2, operatorTok);
      case TypeCode.Boolean:
      case TypeCode.SByte:
      case TypeCode.Byte:
      case TypeCode.Int16:
      case TypeCode.UInt16:
      case TypeCode.Int32:
        int int32_1 = iconvertible1.ToInt32((IFormatProvider) null);
        switch (typeCode2)
        {
          case TypeCode.Empty:
            return (object) double.NaN;
          case TypeCode.DBNull:
            return NumericBinary.DoOp(int32_1, 0, operatorTok);
          case TypeCode.Boolean:
          case TypeCode.Char:
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.UInt16:
          case TypeCode.Int32:
            return NumericBinary.DoOp(int32_1, iconvertible2.ToInt32((IFormatProvider) null), operatorTok);
          case TypeCode.UInt32:
          case TypeCode.Int64:
            return NumericBinary.DoOp((long) int32_1, iconvertible2.ToInt64((IFormatProvider) null), operatorTok);
          case TypeCode.UInt64:
            return int32_1 >= 0 ? NumericBinary.DoOp((ulong) int32_1, iconvertible2.ToUInt64((IFormatProvider) null), operatorTok) : NumericBinary.DoOp((double) int32_1, iconvertible2.ToDouble((IFormatProvider) null), operatorTok);
          case TypeCode.Single:
          case TypeCode.Double:
            return NumericBinary.DoOp((double) int32_1, iconvertible2.ToDouble((IFormatProvider) null), operatorTok);
        }
        break;
      case TypeCode.Char:
        int int32_2 = iconvertible1.ToInt32((IFormatProvider) null);
        object numericBinary;
        switch (typeCode2)
        {
          case TypeCode.Empty:
            return (object) double.NaN;
          case TypeCode.DBNull:
            return NumericBinary.DoOp(int32_2, 0, operatorTok);
          case TypeCode.Boolean:
          case TypeCode.Char:
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.UInt16:
          case TypeCode.Int32:
            numericBinary = NumericBinary.DoOp(int32_2, iconvertible2.ToInt32((IFormatProvider) null), operatorTok);
            break;
          case TypeCode.UInt32:
          case TypeCode.Int64:
            numericBinary = NumericBinary.DoOp((long) int32_2, iconvertible2.ToInt64((IFormatProvider) null), operatorTok);
            break;
          case TypeCode.UInt64:
            numericBinary = NumericBinary.DoOp((double) int32_2, iconvertible2.ToDouble((IFormatProvider) null), operatorTok);
            break;
          case TypeCode.Single:
          case TypeCode.Double:
            numericBinary = NumericBinary.DoOp((double) iconvertible1.ToInt32((IFormatProvider) null), iconvertible2.ToDouble((IFormatProvider) null), operatorTok);
            break;
          case TypeCode.String:
            numericBinary = NumericBinary.DoOp((double) int32_2, Convert.ToNumber(v2, iconvertible2), operatorTok);
            break;
          default:
            numericBinary = (object) null;
            break;
        }
        if (this.operatorTok == JSToken.Minus && numericBinary != null && typeCode2 != TypeCode.Char)
          return Convert.Coerce2(numericBinary, TypeCode.Char, false);
        if (numericBinary != null)
          return numericBinary;
        break;
      case TypeCode.UInt32:
        uint uint32 = iconvertible1.ToUInt32((IFormatProvider) null);
        switch (typeCode2)
        {
          case TypeCode.Empty:
            return (object) double.NaN;
          case TypeCode.DBNull:
            return NumericBinary.DoOp(uint32, 0U, operatorTok);
          case TypeCode.Boolean:
          case TypeCode.Char:
          case TypeCode.UInt16:
          case TypeCode.UInt32:
            return NumericBinary.DoOp(uint32, iconvertible2.ToUInt32((IFormatProvider) null), operatorTok);
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.Int32:
            int int32_3 = iconvertible2.ToInt32((IFormatProvider) null);
            return int32_3 >= 0 ? NumericBinary.DoOp(uint32, (uint) int32_3, operatorTok) : NumericBinary.DoOp((long) uint32, (long) int32_3, operatorTok);
          case TypeCode.Int64:
            return NumericBinary.DoOp((long) uint32, iconvertible2.ToInt64((IFormatProvider) null), operatorTok);
          case TypeCode.UInt64:
            return NumericBinary.DoOp((ulong) uint32, iconvertible2.ToUInt64((IFormatProvider) null), operatorTok);
          case TypeCode.Single:
          case TypeCode.Double:
            return NumericBinary.DoOp((double) uint32, iconvertible2.ToDouble((IFormatProvider) null), operatorTok);
        }
        break;
      case TypeCode.Int64:
        long int64_1 = iconvertible1.ToInt64((IFormatProvider) null);
        switch (typeCode2)
        {
          case TypeCode.Empty:
            return (object) double.NaN;
          case TypeCode.DBNull:
            return NumericBinary.DoOp(int64_1, 0L, operatorTok);
          case TypeCode.Boolean:
          case TypeCode.Char:
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.UInt16:
          case TypeCode.Int32:
          case TypeCode.UInt32:
          case TypeCode.Int64:
            return NumericBinary.DoOp(int64_1, iconvertible2.ToInt64((IFormatProvider) null), operatorTok);
          case TypeCode.UInt64:
            return int64_1 >= 0L ? NumericBinary.DoOp((ulong) int64_1, iconvertible2.ToUInt64((IFormatProvider) null), operatorTok) : NumericBinary.DoOp((double) int64_1, iconvertible2.ToDouble((IFormatProvider) null), operatorTok);
          case TypeCode.Single:
          case TypeCode.Double:
            return NumericBinary.DoOp((double) int64_1, iconvertible2.ToDouble((IFormatProvider) null), operatorTok);
        }
        break;
      case TypeCode.UInt64:
        ulong uint64 = iconvertible1.ToUInt64((IFormatProvider) null);
        switch (typeCode2)
        {
          case TypeCode.Empty:
            return (object) double.NaN;
          case TypeCode.DBNull:
            return NumericBinary.DoOp(uint64, 0UL, operatorTok);
          case TypeCode.Boolean:
          case TypeCode.Char:
          case TypeCode.UInt16:
          case TypeCode.UInt32:
          case TypeCode.UInt64:
            return NumericBinary.DoOp(uint64, iconvertible2.ToUInt64((IFormatProvider) null), operatorTok);
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.Int32:
          case TypeCode.Int64:
            long int64_2 = iconvertible2.ToInt64((IFormatProvider) null);
            return int64_2 >= 0L ? NumericBinary.DoOp(uint64, (ulong) int64_2, operatorTok) : NumericBinary.DoOp((double) uint64, (double) int64_2, operatorTok);
          case TypeCode.Single:
          case TypeCode.Double:
            return NumericBinary.DoOp((double) uint64, iconvertible2.ToDouble((IFormatProvider) null), operatorTok);
        }
        break;
      case TypeCode.Single:
      case TypeCode.Double:
        double x = iconvertible1.ToDouble((IFormatProvider) null);
        switch (typeCode2)
        {
          case TypeCode.Empty:
            return (object) double.NaN;
          case TypeCode.DBNull:
            return NumericBinary.DoOp(x, 0.0, operatorTok);
          case TypeCode.Boolean:
          case TypeCode.Char:
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.UInt16:
          case TypeCode.Int32:
            return NumericBinary.DoOp(x, (double) iconvertible2.ToInt32((IFormatProvider) null), operatorTok);
          case TypeCode.UInt32:
          case TypeCode.Int64:
          case TypeCode.UInt64:
          case TypeCode.Single:
          case TypeCode.Double:
            return NumericBinary.DoOp(x, iconvertible2.ToDouble((IFormatProvider) null), operatorTok);
        }
        break;
    }
    if (v2 == null)
      return (object) double.NaN;
    MethodInfo methodInfo = this.GetOperator((IReflect) v1.GetType(), (IReflect) v2.GetType());
    if (!(methodInfo != (MethodInfo) null))
      return NumericBinary.DoOp(v1, v2, iconvertible1, iconvertible2, operatorTok);
    return methodInfo.Invoke((object) null, BindingFlags.Default, (Binder) JSBinder.ob, new object[2]
    {
      v1,
      v2
    }, (CultureInfo) null);
  }

  /// <summary>Performs the specified binary operation on the specified numeric operands.</summary>
  /// <returns>The result of the operation.</returns>
  /// <param name="v1">The first operand.</param>
  /// <param name="v2">The second operand.</param>
  /// <param name="operatorTok">One of the <see cref="T:Microsoft.JScript.JSToken" /> enumeration values. Specifies the operation to perform.</param>
  public static object DoOp(object v1, object v2, JSToken operatorTok)
  {
    return NumericBinary.DoOp(v1, v2, Convert.GetIConvertible(v1), Convert.GetIConvertible(v2), operatorTok);
  }

  private static object DoOp(
    object v1,
    object v2,
    IConvertible ic1,
    IConvertible ic2,
    JSToken operatorTok)
  {
    if (operatorTok == JSToken.Minus)
    {
      IConvertible ic3 = ic1;
      object primitive1 = Convert.ToPrimitive(v1, PreferredType.Either, ref ic3);
      if (Convert.GetTypeCode(primitive1, ic3) == TypeCode.Char)
      {
        IConvertible ic4 = ic2;
        object primitive2 = Convert.ToPrimitive(v2, PreferredType.Either, ref ic4);
        TypeCode typeCode = Convert.GetTypeCode(primitive2, ic4);
        if (typeCode == TypeCode.String)
        {
          string str = ic4.ToString((IFormatProvider) null);
          if (str.Length == 1)
          {
            typeCode = TypeCode.Char;
            primitive2 = (object) str[0];
            ic4 = Convert.GetIConvertible(primitive2);
          }
        }
        object obj = NumericBinary.DoOp(Convert.ToNumber(primitive1, ic3), Convert.ToNumber(primitive2, ic4), operatorTok);
        if (typeCode != TypeCode.Char)
          obj = Convert.Coerce2(obj, TypeCode.Char, false);
        return obj;
      }
    }
    return NumericBinary.DoOp(Convert.ToNumber(v1, ic1), Convert.ToNumber(v2, ic2), operatorTok);
  }

  private static object DoOp(int x, int y, JSToken operatorTok)
  {
    switch (operatorTok)
    {
      case JSToken.Minus:
        int num = x - y;
        return num < x == y > 0 ? (object) num : (object) ((double) x - (double) y);
      case JSToken.Multiply:
        if (x != 0)
        {
          if (y != 0)
          {
            try
            {
              return (object) checked (x * y);
            }
            catch (OverflowException ex)
            {
              return (object) ((double) x * (double) y);
            }
          }
        }
        return (object) ((double) x * (double) y);
      case JSToken.Divide:
        return (object) ((double) x / (double) y);
      case JSToken.Modulo:
        return x <= 0 || y <= 0 ? (object) ((double) x % (double) y) : (object) (x % y);
      default:
        throw new JScriptException(JSError.InternalError);
    }
  }

  private static object DoOp(uint x, uint y, JSToken operatorTok)
  {
    switch (operatorTok)
    {
      case JSToken.Minus:
        uint num = x - y;
        return num <= x ? (object) num : (object) ((double) x - (double) y);
      case JSToken.Multiply:
        try
        {
          return (object) checked (x * y);
        }
        catch (OverflowException ex)
        {
          return (object) ((double) x * (double) y);
        }
      case JSToken.Divide:
        return (object) ((double) x / (double) y);
      case JSToken.Modulo:
        return y == 0U ? (object) double.NaN : (object) (x % y);
      default:
        throw new JScriptException(JSError.InternalError);
    }
  }

  private static object DoOp(long x, long y, JSToken operatorTok)
  {
    switch (operatorTok)
    {
      case JSToken.Minus:
        long num1 = x - y;
        return num1 < x == y > 0L ? (object) num1 : (object) ((double) x - (double) y);
      case JSToken.Multiply:
        if (x != 0L)
        {
          if (y != 0L)
          {
            try
            {
              return (object) checked (x * y);
            }
            catch (OverflowException ex)
            {
              return (object) ((double) x * (double) y);
            }
          }
        }
        return (object) ((double) x * (double) y);
      case JSToken.Divide:
        return (object) ((double) x / (double) y);
      case JSToken.Modulo:
        if (y == 0L)
          return (object) double.NaN;
        long num2 = x % y;
        if (num2 != 0L)
          return (object) num2;
        return x < 0L ? (y < 0L ? (object) 0 : (object) -0.0) : (y < 0L ? (object) -0.0 : (object) 0);
      default:
        throw new JScriptException(JSError.InternalError);
    }
  }

  private static object DoOp(ulong x, ulong y, JSToken operatorTok)
  {
    switch (operatorTok)
    {
      case JSToken.Minus:
        ulong num = x - y;
        return num <= x ? (object) num : (object) ((double) x - (double) y);
      case JSToken.Multiply:
        try
        {
          return (object) checked (x * y);
        }
        catch (OverflowException ex)
        {
          return (object) ((double) x * (double) y);
        }
      case JSToken.Divide:
        return (object) ((double) x / (double) y);
      case JSToken.Modulo:
        return y == 0UL ? (object) double.NaN : (object) (x % y);
      default:
        throw new JScriptException(JSError.InternalError);
    }
  }

  private static object DoOp(double x, double y, JSToken operatorTok)
  {
    switch (operatorTok)
    {
      case JSToken.Minus:
        return (object) (x - y);
      case JSToken.Multiply:
        return (object) (x * y);
      case JSToken.Divide:
        return (object) (x / y);
      case JSToken.Modulo:
        return (object) (x % y);
      default:
        throw new JScriptException(JSError.InternalError);
    }
  }

  internal override IReflect InferType(JSField inference_target)
  {
    MethodInfo methodInfo = this.type1 == (Type) null || (FieldInfo) inference_target != (FieldInfo) null ? this.GetOperator(this.operand1.InferType(inference_target), this.operand2.InferType(inference_target)) : this.GetOperator((IReflect) this.type1, (IReflect) this.type2);
    if (methodInfo != (MethodInfo) null)
    {
      this.metaData = (object) methodInfo;
      return (IReflect) methodInfo.ReturnType;
    }
    if (this.type1 == Typeob.Char && this.operatorTok == JSToken.Minus)
    {
      TypeCode typeCode = Type.GetTypeCode(this.type2);
      if (Convert.IsPrimitiveNumericTypeCode(typeCode) || typeCode == TypeCode.Boolean)
        return (IReflect) Typeob.Char;
      if (typeCode == TypeCode.Char)
        return (IReflect) Typeob.Int32;
    }
    return (Convert.IsPrimitiveNumericTypeFitForDouble((IReflect) this.type1) || Typeob.JSObject.IsAssignableFrom(this.type1)) && (Convert.IsPrimitiveNumericTypeFitForDouble((IReflect) this.type2) || Typeob.JSObject.IsAssignableFrom(this.type2)) ? (IReflect) Typeob.Double : (IReflect) Typeob.Object;
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    if (this.metaData == null)
    {
      Type type = Typeob.Double;
      if (Convert.IsPrimitiveNumericType((IReflect) rtype) && Convert.IsPromotableTo((IReflect) this.type1, (IReflect) rtype) && Convert.IsPromotableTo((IReflect) this.type2, (IReflect) rtype))
        type = rtype;
      if (this.operatorTok == JSToken.Divide)
        type = Typeob.Double;
      else if (type == Typeob.SByte || type == Typeob.Int16)
        type = Typeob.Int32;
      else if (type == Typeob.Byte || type == Typeob.UInt16 || type == Typeob.Char)
        type = Typeob.UInt32;
      this.operand1.TranslateToIL(il, type);
      this.operand2.TranslateToIL(il, type);
      if (type == Typeob.Double || type == Typeob.Single)
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
      else if (type == Typeob.Int32 || type == Typeob.Int64)
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
      if (Convert.ToType(this.InferType((JSField) null)) == Typeob.Char)
      {
        Convert.Emit((AST) this, il, type, Typeob.Char);
        Convert.Emit((AST) this, il, Typeob.Char, rtype);
      }
      else
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
      il.Emit(OpCodes.Call, CompilerGlobals.evaluateNumericBinaryMethod);
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
    this.metaData = (object) il.DeclareLocal(Typeob.NumericBinary);
    ConstantWrapper.TranslateToILInt(il, (int) this.operatorTok);
    il.Emit(OpCodes.Newobj, CompilerGlobals.numericBinaryConstructor);
    il.Emit(OpCodes.Stloc, (LocalBuilder) this.metaData);
  }
}
