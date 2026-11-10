// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Relational
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

/// <summary>Performs relational comparison operations. This class belongs to the abstract syntax tree category.</summary>
public class Relational : BinaryOp
{
  private object metaData;

  internal Relational(Context context, AST operand1, AST operand2, JSToken operatorTok)
    : base(context, operand1, operand2, operatorTok)
  {
  }

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.Relational" /> class.</summary>
  /// <param name="operatorTok">One of the <see cref="T:Microsoft.JScript.JSToken" /> enumeration values. Specifies the comparison to perform.</param>
  public Relational(int operatorTok)
    : base((Context) null, (AST) null, (AST) null, (JSToken) operatorTok)
  {
  }

  internal override object Evaluate()
  {
    double relational = this.EvaluateRelational(this.operand1.Evaluate(), this.operand2.Evaluate());
    switch (this.operatorTok)
    {
      case JSToken.GreaterThan:
        return (object) (relational > 0.0);
      case JSToken.LessThan:
        return (object) (relational < 0.0);
      case JSToken.LessThanEqual:
        return (object) (relational <= 0.0);
      case JSToken.GreaterThanEqual:
        return (object) (relational >= 0.0);
      default:
        throw new JScriptException(JSError.InternalError, this.context);
    }
  }

  /// <summary>Performs a relational comparison operation on the specified operands.</summary>
  /// <returns>The result of the operation.</returns>
  /// <param name="v1">The first operand.</param>
  /// <param name="v2">The second operand.</param>
  [DebuggerStepThrough]
  [DebuggerHidden]
  public double EvaluateRelational(object v1, object v2)
  {
    switch (v1)
    {
      case int _:
        switch (v2)
        {
          case int num1:
            return (double) (int) v1 - (double) num1;
          case double num2:
            return (double) (int) v1 - num2;
        }
        break;
      case double _:
        switch (v2)
        {
          case double num5:
            double num3 = (double) v1;
            double num4 = num5;
            return num3 == num4 ? 0.0 : num3 - num4;
          case int num6:
            return (double) v1 - (double) num6;
        }
        break;
    }
    IConvertible iconvertible1 = Convert.GetIConvertible(v1);
    IConvertible iconvertible2 = Convert.GetIConvertible(v2);
    TypeCode typeCode1 = Convert.GetTypeCode(v1, iconvertible1);
    TypeCode typeCode2 = Convert.GetTypeCode(v2, iconvertible2);
    if (typeCode1 == TypeCode.Object && typeCode2 == TypeCode.Object)
    {
      MethodInfo methodInfo = this.GetOperator((IReflect) v1.GetType(), (IReflect) v2.GetType());
      if (methodInfo != (MethodInfo) null)
      {
        bool boolean = Convert.ToBoolean(methodInfo.Invoke((object) null, BindingFlags.Default, (Binder) JSBinder.ob, new object[2]
        {
          v1,
          v2
        }, (CultureInfo) null));
        switch (this.operatorTok)
        {
          case JSToken.GreaterThan:
          case JSToken.GreaterThanEqual:
            return boolean ? 1.0 : -1.0;
          case JSToken.LessThan:
          case JSToken.LessThanEqual:
            return boolean ? -1.0 : 1.0;
          default:
            throw new JScriptException(JSError.InternalError, this.context);
        }
      }
    }
    return Relational.JScriptCompare2(v1, v2, iconvertible1, iconvertible2, typeCode1, typeCode2);
  }

  internal override IReflect InferType(JSField inference_target) => (IReflect) Typeob.Boolean;

  /// <summary>Performs a relational comparison operation on the specified operands.</summary>
  /// <returns>The result of the operation.</returns>
  /// <param name="v1">The first operand.</param>
  /// <param name="v2">The second operand.</param>
  public static double JScriptCompare(object v1, object v2)
  {
    switch (v1)
    {
      case int _:
        switch (v2)
        {
          case int num1:
            return (double) ((int) v1 - num1);
          case double num2:
            return (double) (int) v1 - num2;
        }
        break;
      case double _:
        switch (v2)
        {
          case double num5:
            double num3 = (double) v1;
            double num4 = num5;
            return num3 == num4 ? 0.0 : num3 - num4;
          case int num6:
            return (double) v1 - (double) num6;
        }
        break;
    }
    IConvertible iconvertible1 = Convert.GetIConvertible(v1);
    IConvertible iconvertible2 = Convert.GetIConvertible(v2);
    TypeCode typeCode1 = Convert.GetTypeCode(v1, iconvertible1);
    TypeCode typeCode2 = Convert.GetTypeCode(v2, iconvertible2);
    return Relational.JScriptCompare2(v1, v2, iconvertible1, iconvertible2, typeCode1, typeCode2);
  }

  private static double JScriptCompare2(
    object v1,
    object v2,
    IConvertible ic1,
    IConvertible ic2,
    TypeCode t1,
    TypeCode t2)
  {
    if (t1 == TypeCode.Object)
    {
      v1 = Convert.ToPrimitive(v1, PreferredType.Number, ref ic1);
      t1 = Convert.GetTypeCode(v1, ic1);
    }
    if (t2 == TypeCode.Object)
    {
      v2 = Convert.ToPrimitive(v2, PreferredType.Number, ref ic2);
      t2 = Convert.GetTypeCode(v2, ic2);
    }
    switch (t1)
    {
      case TypeCode.Char:
        if (t2 == TypeCode.String)
          return (double) string.CompareOrdinal(Convert.ToString(v1, ic1), ic2.ToString((IFormatProvider) null));
        goto case TypeCode.SByte;
      case TypeCode.SByte:
      case TypeCode.Byte:
      case TypeCode.Int16:
      case TypeCode.UInt16:
      case TypeCode.Int32:
      case TypeCode.UInt32:
      case TypeCode.Int64:
        long int64_1 = ic1.ToInt64((IFormatProvider) null);
        switch (t2 - 5)
        {
          case TypeCode.Empty:
          case TypeCode.Object:
          case TypeCode.DBNull:
          case TypeCode.Boolean:
          case TypeCode.Char:
          case TypeCode.SByte:
          case TypeCode.Byte:
            return (double) (int64_1 - ic2.ToInt64((IFormatProvider) null));
          case TypeCode.Int16:
            if (int64_1 < 0L)
              return -1.0;
            ulong uint64_1 = ic2.ToUInt64((IFormatProvider) null);
            if ((ulong) int64_1 < uint64_1)
              return -1.0;
            return int64_1 == (long) uint64_1 ? 0.0 : 1.0;
          case TypeCode.UInt16:
          case TypeCode.Int32:
            return (double) int64_1 - ic2.ToDouble((IFormatProvider) null);
          case TypeCode.UInt32:
            return (double) (new Decimal(int64_1) - ic2.ToDecimal((IFormatProvider) null));
          default:
            object number1 = (object) Convert.ToNumber(v2, ic2);
            return Relational.JScriptCompare2(v1, number1, ic1, Convert.GetIConvertible(number1), t1, TypeCode.Double);
        }
      case TypeCode.UInt64:
        ulong uint64_2 = ic1.ToUInt64((IFormatProvider) null);
        switch (t2)
        {
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.UInt16:
          case TypeCode.Int32:
          case TypeCode.UInt32:
          case TypeCode.Int64:
            long int64_2 = ic2.ToInt64((IFormatProvider) null);
            if (int64_2 < 0L)
              return 1.0;
            return (long) uint64_2 == int64_2 ? 0.0 : -1.0;
          case TypeCode.UInt64:
            ulong uint64_3 = ic2.ToUInt64((IFormatProvider) null);
            if (uint64_2 < uint64_3)
              return -1.0;
            return (long) uint64_2 == (long) uint64_3 ? 0.0 : 1.0;
          case TypeCode.Single:
          case TypeCode.Double:
            return (double) uint64_2 - ic2.ToDouble((IFormatProvider) null);
          case TypeCode.Decimal:
            return (double) (new Decimal(uint64_2) - ic2.ToDecimal((IFormatProvider) null));
          default:
            object number2 = (object) Convert.ToNumber(v2, ic2);
            return Relational.JScriptCompare2(v1, number2, ic1, Convert.GetIConvertible(number2), t1, TypeCode.Double);
        }
      case TypeCode.Decimal:
        Decimal num = ic1.ToDecimal((IFormatProvider) null);
        switch (t2)
        {
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.UInt16:
          case TypeCode.Int32:
          case TypeCode.UInt32:
          case TypeCode.Int64:
            return (double) (num - new Decimal(ic2.ToInt64((IFormatProvider) null)));
          case TypeCode.UInt64:
            return (double) (num - new Decimal(ic2.ToUInt64((IFormatProvider) null)));
          case TypeCode.Single:
          case TypeCode.Double:
            return (double) (num - new Decimal(ic2.ToDouble((IFormatProvider) null)));
          case TypeCode.Decimal:
            return (double) (num - ic2.ToDecimal((IFormatProvider) null));
          default:
            return (double) (num - new Decimal(Convert.ToNumber(v2, ic2)));
        }
      case TypeCode.String:
        if (t2 == TypeCode.Char)
          return (double) string.CompareOrdinal(ic1.ToString((IFormatProvider) null), Convert.ToString(v2, ic2));
        if (t2 == TypeCode.String)
          return (double) string.CompareOrdinal(ic1.ToString((IFormatProvider) null), ic2.ToString((IFormatProvider) null));
        break;
    }
    double number3 = Convert.ToNumber(v1, ic1);
    double number4 = Convert.ToNumber(v2, ic2);
    return number3 == number4 ? 0.0 : number3 - number4;
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
    Type type1 = this.type1;
    Type type2 = this.type2;
    Type rtype = Typeob.Object;
    if (type1.IsPrimitive && type2.IsPrimitive)
    {
      rtype = Typeob.Double;
      if (Convert.IsPromotableTo((IReflect) type1, (IReflect) type2))
        rtype = type2;
      else if (Convert.IsPromotableTo((IReflect) type2, (IReflect) type1))
        rtype = type1;
      else if (type1 == Typeob.Int64 || type1 == Typeob.UInt64 || type2 == Typeob.Int64 || type2 == Typeob.UInt64)
        rtype = Typeob.Object;
    }
    if (rtype == Typeob.SByte || rtype == Typeob.Int16)
      rtype = Typeob.Int32;
    else if (rtype == Typeob.Byte || rtype == Typeob.UInt16)
      rtype = Typeob.UInt32;
    if (this.metaData == null)
    {
      this.operand1.TranslateToIL(il, rtype);
      this.operand2.TranslateToIL(il, rtype);
      if (rtype == Typeob.Object)
      {
        il.Emit(OpCodes.Call, CompilerGlobals.jScriptCompareMethod);
        il.Emit(OpCodes.Ldc_I4_0);
        il.Emit(OpCodes.Conv_R8);
        rtype = Typeob.Double;
      }
    }
    else
    {
      if ((object) (this.metaData as MethodInfo) != null)
      {
        MethodInfo metaData = (MethodInfo) this.metaData;
        ParameterInfo[] parameters = metaData.GetParameters();
        this.operand1.TranslateToIL(il, parameters[0].ParameterType);
        this.operand2.TranslateToIL(il, parameters[1].ParameterType);
        il.Emit(OpCodes.Call, metaData);
        if (branchIfTrue)
        {
          il.Emit(shortForm ? OpCodes.Brtrue_S : OpCodes.Brtrue, label);
          return;
        }
        il.Emit(shortForm ? OpCodes.Brfalse_S : OpCodes.Brfalse, label);
        return;
      }
      il.Emit(OpCodes.Ldloc, (LocalBuilder) this.metaData);
      this.operand1.TranslateToIL(il, Typeob.Object);
      this.operand2.TranslateToIL(il, Typeob.Object);
      il.Emit(OpCodes.Call, CompilerGlobals.evaluateRelationalMethod);
      il.Emit(OpCodes.Ldc_I4_0);
      il.Emit(OpCodes.Conv_R8);
      rtype = Typeob.Double;
    }
    if (branchIfTrue)
    {
      if (rtype == Typeob.UInt32 || rtype == Typeob.UInt64)
      {
        switch (this.operatorTok)
        {
          case JSToken.GreaterThan:
            il.Emit(shortForm ? OpCodes.Bgt_Un_S : OpCodes.Bgt_Un, label);
            break;
          case JSToken.LessThan:
            il.Emit(shortForm ? OpCodes.Blt_Un_S : OpCodes.Blt_Un, label);
            break;
          case JSToken.LessThanEqual:
            il.Emit(shortForm ? OpCodes.Ble_Un_S : OpCodes.Ble_Un, label);
            break;
          case JSToken.GreaterThanEqual:
            il.Emit(shortForm ? OpCodes.Bge_Un_S : OpCodes.Bge_Un, label);
            break;
          default:
            throw new JScriptException(JSError.InternalError, this.context);
        }
      }
      else
      {
        switch (this.operatorTok)
        {
          case JSToken.GreaterThan:
            il.Emit(shortForm ? OpCodes.Bgt_S : OpCodes.Bgt, label);
            break;
          case JSToken.LessThan:
            il.Emit(shortForm ? OpCodes.Blt_S : OpCodes.Blt, label);
            break;
          case JSToken.LessThanEqual:
            il.Emit(shortForm ? OpCodes.Ble_S : OpCodes.Ble, label);
            break;
          case JSToken.GreaterThanEqual:
            il.Emit(shortForm ? OpCodes.Bge_S : OpCodes.Bge, label);
            break;
          default:
            throw new JScriptException(JSError.InternalError, this.context);
        }
      }
    }
    else if (rtype == Typeob.Int32 || rtype == Typeob.Int64)
    {
      switch (this.operatorTok)
      {
        case JSToken.GreaterThan:
          il.Emit(shortForm ? OpCodes.Ble_S : OpCodes.Ble, label);
          break;
        case JSToken.LessThan:
          il.Emit(shortForm ? OpCodes.Bge_S : OpCodes.Bge, label);
          break;
        case JSToken.LessThanEqual:
          il.Emit(shortForm ? OpCodes.Bgt_S : OpCodes.Bgt, label);
          break;
        case JSToken.GreaterThanEqual:
          il.Emit(shortForm ? OpCodes.Blt_S : OpCodes.Blt, label);
          break;
        default:
          throw new JScriptException(JSError.InternalError, this.context);
      }
    }
    else
    {
      switch (this.operatorTok)
      {
        case JSToken.GreaterThan:
          il.Emit(shortForm ? OpCodes.Ble_Un_S : OpCodes.Ble_Un, label);
          break;
        case JSToken.LessThan:
          il.Emit(shortForm ? OpCodes.Bge_Un_S : OpCodes.Bge_Un, label);
          break;
        case JSToken.LessThanEqual:
          il.Emit(shortForm ? OpCodes.Bgt_Un_S : OpCodes.Bgt_Un, label);
          break;
        case JSToken.GreaterThanEqual:
          il.Emit(shortForm ? OpCodes.Blt_Un_S : OpCodes.Blt_Un, label);
          break;
        default:
          throw new JScriptException(JSError.InternalError, this.context);
      }
    }
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    Label label1 = il.DefineLabel();
    Label label2 = il.DefineLabel();
    this.TranslateToConditionalBranch(il, true, label1, true);
    il.Emit(OpCodes.Ldc_I4_0);
    il.Emit(OpCodes.Br, label2);
    il.MarkLabel(label1);
    il.Emit(OpCodes.Ldc_I4_1);
    il.MarkLabel(label2);
    Convert.Emit((AST) this, il, Typeob.Boolean, rtype);
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
    this.operand1.TranslateToILInitializer(il);
    this.operand2.TranslateToILInitializer(il);
    MethodInfo methodInfo = this.GetOperator(this.operand1.InferType((JSField) null), this.operand2.InferType((JSField) null));
    if (methodInfo != (MethodInfo) null)
    {
      this.metaData = (object) methodInfo;
    }
    else
    {
      if ((this.type1.IsPrimitive || Typeob.JSObject.IsAssignableFrom(this.type1)) && (this.type2.IsPrimitive || Typeob.JSObject.IsAssignableFrom(this.type2)))
        return;
      this.metaData = (object) il.DeclareLocal(Typeob.Relational);
      ConstantWrapper.TranslateToILInt(il, (int) this.operatorTok);
      il.Emit(OpCodes.Newobj, CompilerGlobals.relationalConstructor);
      il.Emit(OpCodes.Stloc, (LocalBuilder) this.metaData);
    }
  }
}
