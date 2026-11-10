// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Plus
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

/// <summary>Performs addition and concatenation operations. This class belongs to the abstract syntax tree category.</summary>
public sealed class Plus : BinaryOp
{
  private object metaData;

  internal Plus(Context context, AST operand1, AST operand2)
    : base(context, operand1, operand2, JSToken.FirstBinaryOp)
  {
  }

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.Plus" /> class.</summary>
  public Plus()
    : base((Context) null, (AST) null, (AST) null, JSToken.FirstBinaryOp)
  {
  }

  internal override object Evaluate()
  {
    return this.EvaluatePlus(this.operand1.Evaluate(), this.operand2.Evaluate());
  }

  /// <summary>Performs an addition or concatenation operation.</summary>
  /// <returns>The result of the operation.</returns>
  /// <param name="v1">The first operand.</param>
  /// <param name="v2">The second operand.</param>
  [DebuggerStepThrough]
  [DebuggerHidden]
  public object EvaluatePlus(object v1, object v2)
  {
    switch (v1)
    {
      case int _ when v2 is int y1:
        return Plus.DoOp((int) v1, y1);
      case double _ when v2 is double y2:
        return Plus.DoOp((double) v1, y2);
      default:
        return this.EvaluatePlus2(v1, v2);
    }
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  private object EvaluatePlus2(object v1, object v2)
  {
    IConvertible iconvertible1 = Convert.GetIConvertible(v1);
    IConvertible iconvertible2 = Convert.GetIConvertible(v2);
    TypeCode typeCode1 = Convert.GetTypeCode(v1, iconvertible1);
    TypeCode typeCode2 = Convert.GetTypeCode(v2, iconvertible2);
    switch (typeCode1)
    {
      case TypeCode.Empty:
        return Plus.DoOp(v1, v2);
      case TypeCode.DBNull:
        switch (typeCode2)
        {
          case TypeCode.Empty:
            return (object) double.NaN;
          case TypeCode.DBNull:
            return (object) 0;
          case TypeCode.Boolean:
          case TypeCode.Char:
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.UInt16:
          case TypeCode.Int32:
            return (object) iconvertible2.ToInt32((IFormatProvider) null);
          case TypeCode.UInt32:
            return (object) iconvertible2.ToUInt32((IFormatProvider) null);
          case TypeCode.Int64:
            return (object) iconvertible2.ToInt64((IFormatProvider) null);
          case TypeCode.UInt64:
            return (object) iconvertible2.ToUInt64((IFormatProvider) null);
          case TypeCode.Single:
          case TypeCode.Double:
            return (object) iconvertible2.ToDouble((IFormatProvider) null);
          case TypeCode.String:
            return (object) ("null" + iconvertible2.ToString((IFormatProvider) null));
        }
        break;
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
            return (object) int32_1;
          case TypeCode.Boolean:
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.UInt16:
          case TypeCode.Int32:
            return Plus.DoOp(int32_1, iconvertible2.ToInt32((IFormatProvider) null));
          case TypeCode.Char:
            return (object) ((IConvertible) Plus.DoOp(int32_1, iconvertible2.ToInt32((IFormatProvider) null))).ToChar((IFormatProvider) null);
          case TypeCode.UInt32:
          case TypeCode.Int64:
            return Plus.DoOp((long) int32_1, iconvertible2.ToInt64((IFormatProvider) null));
          case TypeCode.UInt64:
            return int32_1 >= 0 ? Plus.DoOp((ulong) int32_1, iconvertible2.ToUInt64((IFormatProvider) null)) : Plus.DoOp((double) int32_1, iconvertible2.ToDouble((IFormatProvider) null));
          case TypeCode.Single:
          case TypeCode.Double:
            return Plus.DoOp((double) int32_1, iconvertible2.ToDouble((IFormatProvider) null));
          case TypeCode.String:
            return (object) (Convert.ToString(v1) + iconvertible2.ToString((IFormatProvider) null));
        }
        break;
      case TypeCode.Char:
        int int32_2 = iconvertible1.ToInt32((IFormatProvider) null);
        switch (typeCode2)
        {
          case TypeCode.Empty:
            return (object) double.NaN;
          case TypeCode.Object:
          case TypeCode.Decimal:
          case TypeCode.DateTime:
            return Plus.DoOp(v1, v2);
          case TypeCode.DBNull:
            return (object) int32_2;
          case TypeCode.Boolean:
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.UInt16:
          case TypeCode.Int32:
            return (object) ((IConvertible) Plus.DoOp(int32_2, iconvertible2.ToInt32((IFormatProvider) null))).ToChar((IFormatProvider) null);
          case TypeCode.Char:
          case TypeCode.String:
            return (object) (iconvertible1.ToString((IFormatProvider) null) + iconvertible2.ToString((IFormatProvider) null));
          case TypeCode.UInt32:
          case TypeCode.Int64:
            return (object) ((IConvertible) Plus.DoOp((long) int32_2, iconvertible2.ToInt64((IFormatProvider) null))).ToChar((IFormatProvider) null);
          case TypeCode.UInt64:
            return (object) ((IConvertible) Plus.DoOp((ulong) int32_2, iconvertible2.ToUInt64((IFormatProvider) null))).ToChar((IFormatProvider) null);
          case TypeCode.Single:
          case TypeCode.Double:
            return (object) checked ((char) (int) Convert.CheckIfDoubleIsInteger((double) Plus.DoOp(unchecked ((double) int32_2), iconvertible2.ToDouble((IFormatProvider) null))));
        }
        break;
      case TypeCode.UInt32:
        uint uint32 = iconvertible1.ToUInt32((IFormatProvider) null);
        switch (typeCode2)
        {
          case TypeCode.Empty:
            return (object) double.NaN;
          case TypeCode.DBNull:
            return (object) uint32;
          case TypeCode.Boolean:
          case TypeCode.UInt16:
          case TypeCode.UInt32:
            return Plus.DoOp(uint32, iconvertible2.ToUInt32((IFormatProvider) null));
          case TypeCode.Char:
            return (object) ((IConvertible) Plus.DoOp(uint32, iconvertible2.ToUInt32((IFormatProvider) null))).ToChar((IFormatProvider) null);
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.Int32:
            int int32_3 = iconvertible2.ToInt32((IFormatProvider) null);
            return int32_3 >= 0 ? Plus.DoOp(uint32, (uint) int32_3) : Plus.DoOp((long) uint32, (long) int32_3);
          case TypeCode.Int64:
            return Plus.DoOp((long) uint32, iconvertible2.ToInt64((IFormatProvider) null));
          case TypeCode.UInt64:
            return Plus.DoOp((ulong) uint32, iconvertible2.ToUInt64((IFormatProvider) null));
          case TypeCode.Single:
          case TypeCode.Double:
            return Plus.DoOp((double) uint32, iconvertible2.ToDouble((IFormatProvider) null));
          case TypeCode.String:
            return (object) (Convert.ToString(v1) + iconvertible2.ToString((IFormatProvider) null));
        }
        break;
      case TypeCode.Int64:
        long int64_1 = iconvertible1.ToInt64((IFormatProvider) null);
        switch (typeCode2)
        {
          case TypeCode.Empty:
            return (object) double.NaN;
          case TypeCode.DBNull:
            return (object) int64_1;
          case TypeCode.Boolean:
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.UInt16:
          case TypeCode.Int32:
          case TypeCode.UInt32:
          case TypeCode.Int64:
            return Plus.DoOp(int64_1, iconvertible2.ToInt64((IFormatProvider) null));
          case TypeCode.Char:
            return (object) ((IConvertible) Plus.DoOp(int64_1, iconvertible2.ToInt64((IFormatProvider) null))).ToChar((IFormatProvider) null);
          case TypeCode.UInt64:
            return int64_1 >= 0L ? Plus.DoOp((ulong) int64_1, iconvertible2.ToUInt64((IFormatProvider) null)) : Plus.DoOp((double) int64_1, iconvertible2.ToDouble((IFormatProvider) null));
          case TypeCode.Single:
          case TypeCode.Double:
            return Plus.DoOp((double) int64_1, iconvertible2.ToDouble((IFormatProvider) null));
          case TypeCode.String:
            return (object) (Convert.ToString(v1) + iconvertible2.ToString((IFormatProvider) null));
        }
        break;
      case TypeCode.UInt64:
        ulong uint64 = iconvertible1.ToUInt64((IFormatProvider) null);
        switch (typeCode2)
        {
          case TypeCode.Empty:
            return (object) double.NaN;
          case TypeCode.DBNull:
            return (object) uint64;
          case TypeCode.Boolean:
          case TypeCode.UInt16:
          case TypeCode.UInt32:
          case TypeCode.UInt64:
            return Plus.DoOp(uint64, iconvertible2.ToUInt64((IFormatProvider) null));
          case TypeCode.Char:
            return (object) ((IConvertible) Plus.DoOp(uint64, iconvertible2.ToUInt64((IFormatProvider) null))).ToChar((IFormatProvider) null);
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.Int32:
          case TypeCode.Int64:
            long int64_2 = iconvertible2.ToInt64((IFormatProvider) null);
            return int64_2 >= 0L ? Plus.DoOp(uint64, (ulong) int64_2) : Plus.DoOp((double) uint64, (double) int64_2);
          case TypeCode.Single:
          case TypeCode.Double:
            return Plus.DoOp((double) uint64, iconvertible2.ToDouble((IFormatProvider) null));
          case TypeCode.String:
            return (object) (Convert.ToString(v1) + iconvertible2.ToString((IFormatProvider) null));
        }
        break;
      case TypeCode.Single:
      case TypeCode.Double:
        double d = iconvertible1.ToDouble((IFormatProvider) null);
        switch (typeCode2)
        {
          case TypeCode.Empty:
            return (object) double.NaN;
          case TypeCode.DBNull:
            return (object) iconvertible1.ToDouble((IFormatProvider) null);
          case TypeCode.Boolean:
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.UInt16:
          case TypeCode.Int32:
            return (object) (d + (double) iconvertible2.ToInt32((IFormatProvider) null));
          case TypeCode.Char:
            return (object) System.Convert.ToChar(System.Convert.ToInt32(d + (double) iconvertible2.ToInt32((IFormatProvider) null)));
          case TypeCode.UInt32:
          case TypeCode.Int64:
          case TypeCode.UInt64:
          case TypeCode.Single:
          case TypeCode.Double:
            return (object) (d + iconvertible2.ToDouble((IFormatProvider) null));
          case TypeCode.String:
            return (object) new ConcatString(Convert.ToString(d), iconvertible2.ToString((IFormatProvider) null));
        }
        break;
      case TypeCode.String:
        switch (typeCode2)
        {
          case TypeCode.Object:
            break;
          case TypeCode.String:
            return v1 is ConcatString ? (object) new ConcatString((ConcatString) v1, iconvertible2.ToString((IFormatProvider) null)) : (object) new ConcatString(iconvertible1.ToString((IFormatProvider) null), iconvertible2.ToString((IFormatProvider) null));
          default:
            return v1 is ConcatString ? (object) new ConcatString((ConcatString) v1, Convert.ToString(v2)) : (object) new ConcatString(iconvertible1.ToString((IFormatProvider) null), Convert.ToString(v2));
        }
        break;
    }
    MethodInfo methodInfo = this.GetOperator(v1 == null ? (IReflect) Typeob.Empty : (IReflect) v1.GetType(), v2 == null ? (IReflect) Typeob.Empty : (IReflect) v2.GetType());
    if (!(methodInfo != (MethodInfo) null))
      return Plus.DoOp(v1, v2);
    return methodInfo.Invoke((object) null, BindingFlags.Default, (Binder) JSBinder.ob, new object[2]
    {
      v1,
      v2
    }, (CultureInfo) null);
  }

  private new MethodInfo GetOperator(IReflect ir1, IReflect ir2)
  {
    Type type1 = (object) (ir1 as Type) != null ? (Type) ir1 : Typeob.Object;
    Type type2 = (object) (ir2 as Type) != null ? (Type) ir2 : Typeob.Object;
    if (this.type1 == type1 && this.type2 == type2)
      return this.operatorMeth;
    if (!(type1 == Typeob.String) && !(type2 == Typeob.String) && (!Convert.IsPrimitiveNumericType((IReflect) type1) && !Typeob.JSObject.IsAssignableFrom(type1) || !Convert.IsPrimitiveNumericType((IReflect) type2) && !Typeob.JSObject.IsAssignableFrom(type2)))
      return base.GetOperator((IReflect) type1, (IReflect) type2);
    this.operatorMeth = (MethodInfo) null;
    this.type1 = type1;
    this.type2 = type2;
    return (MethodInfo) null;
  }

  private static object DoOp(double x, double y) => (object) (x + y);

  private static object DoOp(int x, int y)
  {
    int num = x + y;
    return num < x == y < 0 ? (object) num : (object) ((double) x + (double) y);
  }

  private static object DoOp(long x, long y)
  {
    long num = x + y;
    return num < x == y < 0L ? (object) num : (object) ((double) x + (double) y);
  }

  private static object DoOp(uint x, uint y)
  {
    uint num = x + y;
    return num >= x ? (object) num : (object) ((double) x + (double) y);
  }

  private static object DoOp(ulong x, ulong y)
  {
    ulong num = x + y;
    return num >= x ? (object) num : (object) ((double) x + (double) y);
  }

  /// <summary>Performs an addition or concatenation operation.</summary>
  /// <returns>The result of the operation.</returns>
  /// <param name="v1">The first operand.</param>
  /// <param name="v2">The second operand.</param>
  public static object DoOp(object v1, object v2)
  {
    IConvertible iconvertible1 = Convert.GetIConvertible(v1);
    IConvertible iconvertible2 = Convert.GetIConvertible(v2);
    v1 = Convert.ToPrimitive(v1, PreferredType.Either, ref iconvertible1);
    v2 = Convert.ToPrimitive(v2, PreferredType.Either, ref iconvertible2);
    TypeCode typeCode1 = Convert.GetTypeCode(v1, iconvertible1);
    TypeCode typeCode2 = Convert.GetTypeCode(v2, iconvertible2);
    if (typeCode1 == TypeCode.String)
      return v1 is ConcatString ? (object) new ConcatString((ConcatString) v1, Convert.ToString(v2, iconvertible2)) : (object) new ConcatString(iconvertible1.ToString((IFormatProvider) null), Convert.ToString(v2, iconvertible2));
    if (typeCode2 == TypeCode.String)
      return (object) (Convert.ToString(v1, iconvertible1) + iconvertible2.ToString((IFormatProvider) null));
    if (typeCode1 == TypeCode.Char && typeCode2 == TypeCode.Char)
      return (object) (iconvertible1.ToString((IFormatProvider) null) + iconvertible2.ToString((IFormatProvider) null));
    return typeCode1 == TypeCode.Char && (Convert.IsPrimitiveNumericTypeCode(typeCode2) || typeCode2 == TypeCode.Boolean) || typeCode2 == TypeCode.Char && (Convert.IsPrimitiveNumericTypeCode(typeCode1) || typeCode1 == TypeCode.Boolean) ? (object) (char) Runtime.DoubleToInt64(Convert.ToNumber(v1, iconvertible1) + Convert.ToNumber(v2, iconvertible2)) : (object) (Convert.ToNumber(v1, iconvertible1) + Convert.ToNumber(v2, iconvertible2));
  }

  internal override IReflect InferType(JSField inference_target)
  {
    MethodInfo methodInfo = this.type1 == (Type) null || (FieldInfo) inference_target != (FieldInfo) null ? this.GetOperator(this.operand1.InferType(inference_target), this.operand2.InferType(inference_target)) : this.GetOperator((IReflect) this.type1, (IReflect) this.type2);
    if (methodInfo != (MethodInfo) null)
    {
      this.metaData = (object) methodInfo;
      return (IReflect) methodInfo.ReturnType;
    }
    if (this.type1 == Typeob.String || this.type2 == Typeob.String)
      return (IReflect) Typeob.String;
    if (this.type1 == Typeob.Char && this.type2 == Typeob.Char)
      return (IReflect) Typeob.String;
    if (Convert.IsPrimitiveNumericTypeFitForDouble((IReflect) this.type1))
    {
      if (this.type2 == Typeob.Char)
        return (IReflect) Typeob.Char;
      return Convert.IsPrimitiveNumericTypeFitForDouble((IReflect) this.type2) ? (IReflect) Typeob.Double : (IReflect) Typeob.Object;
    }
    if (Convert.IsPrimitiveNumericTypeFitForDouble((IReflect) this.type2))
    {
      if (this.type1 == Typeob.Char)
        return (IReflect) Typeob.Char;
      return Convert.IsPrimitiveNumericTypeFitForDouble((IReflect) this.type1) ? (IReflect) Typeob.Double : (IReflect) Typeob.Object;
    }
    if (this.type1 == Typeob.Boolean && this.type2 == Typeob.Char)
      return (IReflect) Typeob.Char;
    return this.type1 == Typeob.Char && this.type2 == Typeob.Boolean ? (IReflect) Typeob.Char : (IReflect) Typeob.Object;
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    Type type1 = Convert.ToType(this.InferType((JSField) null));
    if (this.metaData == null)
    {
      Type type2 = Typeob.Object;
      Type type3 = !(rtype == Typeob.Double) ? (!(this.type1 == Typeob.Char) || !(this.type2 == Typeob.Char) ? (!Convert.IsPrimitiveNumericType((IReflect) rtype) || !Convert.IsPromotableTo((IReflect) this.type1, (IReflect) rtype) || !Convert.IsPromotableTo((IReflect) this.type2, (IReflect) rtype) ? (!(this.type1 != Typeob.String) || !(this.type2 != Typeob.String) ? Typeob.String : Typeob.Double) : rtype) : Typeob.String) : rtype;
      if (type3 == Typeob.SByte || type3 == Typeob.Int16)
        type3 = Typeob.Int32;
      else if (type3 == Typeob.Byte || type3 == Typeob.UInt16 || type3 == Typeob.Char)
        type3 = Typeob.UInt32;
      if (type3 == Typeob.String)
      {
        if (this.operand1 is Plus && this.type1 == type3)
        {
          Plus operand1_1 = (Plus) this.operand1;
          if (operand1_1.operand1 is Plus && operand1_1.type1 == type3)
          {
            Plus operand1_2 = (Plus) operand1_1.operand1;
            if (operand1_2.operand1 is Plus && operand1_2.type1 == type3)
            {
              int ilArrayOfStrings = operand1_1.TranslateToILArrayOfStrings(il, 1);
              il.Emit(OpCodes.Dup);
              ConstantWrapper.TranslateToILInt(il, ilArrayOfStrings - 1);
              this.operand2.TranslateToIL(il, type3);
              il.Emit(OpCodes.Stelem_Ref);
              il.Emit(OpCodes.Call, CompilerGlobals.stringConcatArrMethod);
              Convert.Emit((AST) this, il, type3, rtype);
            }
            else
            {
              Plus.TranslateToStringWithSpecialCaseForNull(il, operand1_2.operand1);
              Plus.TranslateToStringWithSpecialCaseForNull(il, operand1_2.operand2);
              Plus.TranslateToStringWithSpecialCaseForNull(il, operand1_1.operand2);
              Plus.TranslateToStringWithSpecialCaseForNull(il, this.operand2);
              il.Emit(OpCodes.Call, CompilerGlobals.stringConcat4Method);
              Convert.Emit((AST) this, il, type3, rtype);
            }
          }
          else
          {
            Plus.TranslateToStringWithSpecialCaseForNull(il, operand1_1.operand1);
            Plus.TranslateToStringWithSpecialCaseForNull(il, operand1_1.operand2);
            Plus.TranslateToStringWithSpecialCaseForNull(il, this.operand2);
            il.Emit(OpCodes.Call, CompilerGlobals.stringConcat3Method);
            Convert.Emit((AST) this, il, type3, rtype);
          }
        }
        else
        {
          Plus.TranslateToStringWithSpecialCaseForNull(il, this.operand1);
          Plus.TranslateToStringWithSpecialCaseForNull(il, this.operand2);
          il.Emit(OpCodes.Call, CompilerGlobals.stringConcat2Method);
          Convert.Emit((AST) this, il, type3, rtype);
        }
      }
      else
      {
        this.operand1.TranslateToIL(il, type3);
        this.operand2.TranslateToIL(il, type3);
        if (type3 == Typeob.Object)
          il.Emit(OpCodes.Call, CompilerGlobals.plusDoOpMethod);
        else if (type3 == Typeob.Double || type3 == Typeob.Single)
          il.Emit(OpCodes.Add);
        else if (type3 == Typeob.Int32 || type3 == Typeob.Int64)
          il.Emit(OpCodes.Add_Ovf);
        else
          il.Emit(OpCodes.Add_Ovf_Un);
        if (type1 == Typeob.Char)
        {
          Convert.Emit((AST) this, il, type3, Typeob.Char);
          Convert.Emit((AST) this, il, Typeob.Char, rtype);
        }
        else
          Convert.Emit((AST) this, il, type3, rtype);
      }
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
      il.Emit(OpCodes.Callvirt, CompilerGlobals.evaluatePlusMethod);
      Convert.Emit((AST) this, il, Typeob.Object, rtype);
    }
  }

  private int TranslateToILArrayOfStrings(ILGenerator il, int n)
  {
    int i = n + 2;
    if (this.operand1 is Plus && this.type1 == Typeob.String)
    {
      i = ((Plus) this.operand1).TranslateToILArrayOfStrings(il, n + 1);
    }
    else
    {
      ConstantWrapper.TranslateToILInt(il, i);
      il.Emit(OpCodes.Newarr, Typeob.String);
      il.Emit(OpCodes.Dup);
      il.Emit(OpCodes.Ldc_I4_0);
      Plus.TranslateToStringWithSpecialCaseForNull(il, this.operand1);
      il.Emit(OpCodes.Stelem_Ref);
    }
    il.Emit(OpCodes.Dup);
    ConstantWrapper.TranslateToILInt(il, i - 1 - n);
    Plus.TranslateToStringWithSpecialCaseForNull(il, this.operand2);
    il.Emit(OpCodes.Stelem_Ref);
    return i;
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
    IReflect reflect = this.InferType((JSField) null);
    this.operand1.TranslateToILInitializer(il);
    this.operand2.TranslateToILInitializer(il);
    if ((object) reflect != (object) Typeob.Object)
      return;
    this.metaData = (object) il.DeclareLocal(Typeob.Plus);
    il.Emit(OpCodes.Newobj, CompilerGlobals.plusConstructor);
    il.Emit(OpCodes.Stloc, (LocalBuilder) this.metaData);
  }

  private static void TranslateToStringWithSpecialCaseForNull(ILGenerator il, AST operand)
  {
    if (operand is ConstantWrapper constantWrapper)
    {
      if (constantWrapper.value is DBNull)
        il.Emit(OpCodes.Ldstr, "null");
      else if (constantWrapper.value == Empty.Value)
        il.Emit(OpCodes.Ldstr, "undefined");
      else
        constantWrapper.TranslateToIL(il, Typeob.String);
    }
    else
      operand.TranslateToIL(il, Typeob.String);
  }
}
