// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Equality
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Performs comparison operations. This class belongs to the abstract syntax tree category.</summary>
public class Equality : BinaryOp
{
  private object metaData;

  internal Equality(Context context, AST operand1, AST operand2, JSToken operatorTok)
    : base(context, operand1, operand2, operatorTok)
  {
  }

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.Equality" /> class.</summary>
  /// <param name="operatorTok">One of the <see cref="T:Microsoft.JScript.JSToken" /> enumeration values. Specifies the comparison to perform.</param>
  public Equality(int operatorTok)
    : base((Context) null, (AST) null, (AST) null, (JSToken) operatorTok)
  {
  }

  internal override object Evaluate()
  {
    bool equality = this.EvaluateEquality(this.operand1.Evaluate(), this.operand2.Evaluate(), VsaEngine.executeForJSEE);
    return this.operatorTok == JSToken.Equal ? (object) equality : (object) !equality;
  }

  /// <summary>Performs a comparison operation.</summary>
  /// <returns>The result of the operation.</returns>
  /// <param name="v1">The first operand.</param>
  /// <param name="v2">The second operand.</param>
  [DebuggerStepThrough]
  [DebuggerHidden]
  public bool EvaluateEquality(object v1, object v2) => this.EvaluateEquality(v1, v2, false);

  [DebuggerStepThrough]
  [DebuggerHidden]
  private bool EvaluateEquality(object v1, object v2, bool checkForDebuggerObjects)
  {
    switch (v1)
    {
      case string _ when v2 is string:
        return v1.Equals(v2);
      case int _ when v2 is int num1:
        return (int) v1 == num1;
      case double _ when v2 is double num2:
        return (double) v1 == num2;
      default:
        if ((v2 == null || v2 is DBNull || v2 is Missing) && !checkForDebuggerObjects)
        {
          switch (v1)
          {
            case null:
            case DBNull _:
              return true;
            default:
              return v1 is Missing;
          }
        }
        else
        {
          IConvertible iconvertible1 = Convert.GetIConvertible(v1);
          IConvertible iconvertible2 = Convert.GetIConvertible(v2);
          TypeCode typeCode1 = Convert.GetTypeCode(v1, iconvertible1);
          TypeCode typeCode2 = Convert.GetTypeCode(v2, iconvertible2);
          switch (typeCode1)
          {
            case TypeCode.Empty:
            case TypeCode.DBNull:
              return Equality.JScriptEquals(v1, v2, iconvertible1, iconvertible2, typeCode1, typeCode2, checkForDebuggerObjects);
            case TypeCode.Object:
              if (typeCode2 != TypeCode.Empty && typeCode2 != TypeCode.DBNull)
              {
                MethodInfo methodInfo = this.GetOperator((IReflect) v1.GetType(), (IReflect) v2.GetType());
                if (methodInfo != (MethodInfo) null)
                {
                  bool flag = (bool) methodInfo.Invoke((object) null, BindingFlags.Default, (Binder) JSBinder.ob, new object[2]
                  {
                    v1,
                    v2
                  }, (CultureInfo) null);
                  return this.operatorTok == JSToken.NotEqual ? !flag : flag;
                }
                goto case TypeCode.Empty;
              }
              goto case TypeCode.Empty;
            default:
              if (typeCode2 == TypeCode.Object)
              {
                MethodInfo methodInfo = this.GetOperator((IReflect) v1.GetType(), (IReflect) v2.GetType());
                if (methodInfo != (MethodInfo) null)
                {
                  bool flag = (bool) methodInfo.Invoke((object) null, BindingFlags.Default, (Binder) JSBinder.ob, new object[2]
                  {
                    v1,
                    v2
                  }, (CultureInfo) null);
                  return this.operatorTok == JSToken.NotEqual ? !flag : flag;
                }
                goto case TypeCode.Empty;
              }
              goto case TypeCode.Empty;
          }
        }
    }
  }

  /// <summary>Performs a comparison operation.</summary>
  /// <returns>The result of the operation.</returns>
  /// <param name="v1">The first operand.</param>
  /// <param name="v2">The second operand.</param>
  public static bool JScriptEquals(object v1, object v2)
  {
    switch (v1)
    {
      case string _ when v2 is string:
        return v1.Equals(v2);
      case int _ when v2 is int num1:
        return (int) v1 == num1;
      case double _ when v2 is double num2:
        return (double) v1 == num2;
      default:
        if (v2 == null || v2 is DBNull || v2 is Missing)
        {
          switch (v1)
          {
            case null:
            case DBNull _:
              return true;
            default:
              return v1 is Missing;
          }
        }
        else
        {
          IConvertible iconvertible1 = Convert.GetIConvertible(v1);
          IConvertible iconvertible2 = Convert.GetIConvertible(v2);
          TypeCode typeCode1 = Convert.GetTypeCode(v1, iconvertible1);
          TypeCode typeCode2 = Convert.GetTypeCode(v2, iconvertible2);
          return Equality.JScriptEquals(v1, v2, iconvertible1, iconvertible2, typeCode1, typeCode2, false);
        }
    }
  }

  private static bool JScriptEquals(
    object v1,
    object v2,
    IConvertible ic1,
    IConvertible ic2,
    TypeCode t1,
    TypeCode t2,
    bool checkForDebuggerObjects)
  {
    if (StrictEquality.JScriptStrictEquals(v1, v2, ic1, ic2, t1, t2, checkForDebuggerObjects))
      return true;
    if (t2 == TypeCode.Boolean)
    {
      v2 = (object) (ic2.ToBoolean((IFormatProvider) null) ? 1 : 0);
      ic2 = Convert.GetIConvertible(v2);
      return Equality.JScriptEquals(v1, v2, ic1, ic2, t1, TypeCode.Int32, false);
    }
    switch (t1)
    {
      case TypeCode.Empty:
        switch (t2)
        {
          case TypeCode.Empty:
          case TypeCode.DBNull:
            return true;
          case TypeCode.Object:
            return v2 is Missing;
          default:
            return false;
        }
      case TypeCode.Object:
        switch (t2)
        {
          case TypeCode.Empty:
          case TypeCode.DBNull:
            return v1 is Missing;
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
          case TypeCode.Decimal:
          case TypeCode.String:
            IConvertible ic3 = ic1;
            object primitive1 = Convert.ToPrimitive(v1, PreferredType.Either, ref ic3);
            return ic3 != null && primitive1 != v1 && Equality.JScriptEquals(primitive1, v2, ic3, ic2, ic3.GetTypeCode(), t2, false);
          default:
            return false;
        }
      case TypeCode.DBNull:
        switch (t2)
        {
          case TypeCode.Empty:
          case TypeCode.DBNull:
            return true;
          case TypeCode.Object:
            return v2 is Missing;
          default:
            return false;
        }
      case TypeCode.Boolean:
        v1 = (object) (ic1.ToBoolean((IFormatProvider) null) ? 1 : 0);
        ic1 = Convert.GetIConvertible(v1);
        return Equality.JScriptEquals(v1, v2, ic1, ic2, TypeCode.Int32, t2, false);
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
      case TypeCode.Decimal:
        switch (t2)
        {
          case TypeCode.Object:
            IConvertible ic4 = ic2;
            object primitive2 = Convert.ToPrimitive(v2, PreferredType.Either, ref ic4);
            return ic4 != null && primitive2 != v2 && Equality.JScriptEquals(v1, primitive2, ic1, ic4, t1, ic4.GetTypeCode(), false);
          case TypeCode.String:
            if (v1 is Enum)
              return Convert.ToString(v1).Equals(ic2.ToString((IFormatProvider) null));
            v2 = (object) Convert.ToNumber(v2, ic2);
            ic2 = Convert.GetIConvertible(v2);
            return StrictEquality.JScriptStrictEquals(v1, v2, ic1, ic2, t1, TypeCode.Double, false);
          default:
            return false;
        }
      case TypeCode.DateTime:
        if (t2 == TypeCode.Object)
        {
          IConvertible ic5 = ic2;
          object primitive3 = Convert.ToPrimitive(v2, PreferredType.Either, ref ic5);
          if (primitive3 != null && primitive3 != v2)
            return StrictEquality.JScriptStrictEquals(v1, primitive3, ic1, ic5, t1, ic5.GetTypeCode(), false);
        }
        return false;
      case TypeCode.String:
        switch (t2)
        {
          case TypeCode.Object:
            IConvertible ic6 = ic2;
            object primitive4 = Convert.ToPrimitive(v2, PreferredType.Either, ref ic6);
            return ic6 != null && primitive4 != v2 && Equality.JScriptEquals(v1, primitive4, ic1, ic6, t1, ic6.GetTypeCode(), false);
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
          case TypeCode.Decimal:
            if (v2 is Enum)
              return Convert.ToString(v2).Equals(ic1.ToString((IFormatProvider) null));
            v1 = (object) Convert.ToNumber(v1, ic1);
            ic1 = Convert.GetIConvertible(v1);
            return StrictEquality.JScriptStrictEquals(v1, v2, ic1, ic2, TypeCode.Double, t2, false);
          default:
            return false;
        }
      default:
        return false;
    }
  }

  internal override IReflect InferType(JSField inference_target) => (IReflect) Typeob.Boolean;

  internal override void TranslateToConditionalBranch(
    ILGenerator il,
    bool branchIfTrue,
    Label label,
    bool shortForm)
  {
    // Short-form branches corrupt the IL emitted by PersistedAssemblyBuilder on
    // .NET 10.0.x (ControlFlowBuilder desyncs on an operand at a blob boundary).
    shortForm = false;
    if (this.metaData == null)
    {
      Type type1 = this.type1;
      Type type2 = this.type2;
      Type rtype = Typeob.Object;
      bool flag = true;
      if (type1.IsPrimitive && type2.IsPrimitive)
      {
        rtype = Typeob.Double;
        if (type1 == Typeob.Single || type2 == Typeob.Single)
          rtype = Typeob.Single;
        else if (Convert.IsPromotableTo((IReflect) type1, (IReflect) type2))
          rtype = type2;
        else if (Convert.IsPromotableTo((IReflect) type2, (IReflect) type1))
          rtype = type1;
      }
      else if (type1 == Typeob.String && (type2 == Typeob.String || type2 == Typeob.Empty || type2 == Typeob.Null))
      {
        rtype = Typeob.String;
        if (type2 != Typeob.String)
        {
          flag = false;
          branchIfTrue = !branchIfTrue;
        }
      }
      else if ((type1 == Typeob.Empty || type1 == Typeob.Null) && type2 == Typeob.String)
      {
        rtype = Typeob.String;
        flag = false;
        branchIfTrue = !branchIfTrue;
      }
      if (rtype == Typeob.SByte || rtype == Typeob.Int16)
        rtype = Typeob.Int32;
      else if (rtype == Typeob.Byte || rtype == Typeob.UInt16)
        rtype = Typeob.UInt32;
      if (flag)
      {
        this.operand1.TranslateToIL(il, rtype);
        this.operand2.TranslateToIL(il, rtype);
        if (rtype == Typeob.Object)
          il.Emit(OpCodes.Call, CompilerGlobals.jScriptEqualsMethod);
        else if (rtype == Typeob.String)
          il.Emit(OpCodes.Call, CompilerGlobals.stringEqualsMethod);
      }
      else if (type1 == Typeob.String)
        this.operand1.TranslateToIL(il, rtype);
      else if (type2 == Typeob.String)
        this.operand2.TranslateToIL(il, rtype);
      if (branchIfTrue)
      {
        if (this.operatorTok == JSToken.Equal)
        {
          if (rtype == Typeob.String || rtype == Typeob.Object)
            il.Emit(shortForm ? OpCodes.Brtrue_S : OpCodes.Brtrue, label);
          else
            il.Emit(shortForm ? OpCodes.Beq_S : OpCodes.Beq, label);
        }
        else if (rtype == Typeob.String || rtype == Typeob.Object)
          il.Emit(shortForm ? OpCodes.Brfalse_S : OpCodes.Brfalse, label);
        else
          il.Emit(shortForm ? OpCodes.Bne_Un_S : OpCodes.Bne_Un, label);
      }
      else if (this.operatorTok == JSToken.Equal)
      {
        if (rtype == Typeob.String || rtype == Typeob.Object)
          il.Emit(shortForm ? OpCodes.Brfalse_S : OpCodes.Brfalse, label);
        else
          il.Emit(shortForm ? OpCodes.Bne_Un_S : OpCodes.Bne_Un, label);
      }
      else if (rtype == Typeob.String || rtype == Typeob.Object)
        il.Emit(shortForm ? OpCodes.Brtrue_S : OpCodes.Brtrue, label);
      else
        il.Emit(shortForm ? OpCodes.Beq_S : OpCodes.Beq, label);
    }
    else if ((object) (this.metaData as MethodInfo) != null)
    {
      MethodInfo metaData = (MethodInfo) this.metaData;
      ParameterInfo[] parameters = metaData.GetParameters();
      this.operand1.TranslateToIL(il, parameters[0].ParameterType);
      this.operand2.TranslateToIL(il, parameters[1].ParameterType);
      il.Emit(OpCodes.Call, metaData);
      if (branchIfTrue)
        il.Emit(shortForm ? OpCodes.Brtrue_S : OpCodes.Brtrue, label);
      else
        il.Emit(shortForm ? OpCodes.Brfalse_S : OpCodes.Brfalse, label);
    }
    else
    {
      il.Emit(OpCodes.Ldloc, (LocalBuilder) this.metaData);
      this.operand1.TranslateToIL(il, Typeob.Object);
      this.operand2.TranslateToIL(il, Typeob.Object);
      il.Emit(OpCodes.Call, CompilerGlobals.evaluateEqualityMethod);
      if (branchIfTrue)
      {
        if (this.operatorTok == JSToken.Equal)
          il.Emit(shortForm ? OpCodes.Brtrue_S : OpCodes.Brtrue, label);
        else
          il.Emit(shortForm ? OpCodes.Brfalse_S : OpCodes.Brfalse, label);
      }
      else if (this.operatorTok == JSToken.Equal)
        il.Emit(shortForm ? OpCodes.Brfalse_S : OpCodes.Brfalse, label);
      else
        il.Emit(shortForm ? OpCodes.Brtrue_S : OpCodes.Brtrue, label);
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
      if (this.operand1 is ConstantWrapper)
      {
        object obj = this.operand1.Evaluate();
        if (obj == null)
          this.type1 = Typeob.Empty;
        else if (obj is DBNull)
          this.type1 = Typeob.Null;
      }
      if (this.operand2 is ConstantWrapper)
      {
        object obj = this.operand2.Evaluate();
        if (obj == null)
          this.type2 = Typeob.Empty;
        else if (obj is DBNull)
          this.type2 = Typeob.Null;
      }
      if (this.type1 == Typeob.Empty || this.type1 == Typeob.Null || this.type2 == Typeob.Empty || this.type2 == Typeob.Null || (this.type1.IsPrimitive || this.type1 == Typeob.String || Typeob.JSObject.IsAssignableFrom(this.type1)) && (this.type2.IsPrimitive || this.type2 == Typeob.String || Typeob.JSObject.IsAssignableFrom(this.type2)))
        return;
      this.metaData = (object) il.DeclareLocal(Typeob.Equality);
      ConstantWrapper.TranslateToILInt(il, (int) this.operatorTok);
      il.Emit(OpCodes.Newobj, CompilerGlobals.equalityConstructor);
      il.Emit(OpCodes.Stloc, (LocalBuilder) this.metaData);
    }
  }
}
