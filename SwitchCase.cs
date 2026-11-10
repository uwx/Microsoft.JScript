// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.SwitchCase
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class SwitchCase : AST
{
  private AST case_value;
  private AST statements;
  private Completion completion;

  internal SwitchCase(Context context, AST statements)
    : this(context, (AST) null, statements)
  {
  }

  internal SwitchCase(Context context, AST case_value, AST statements)
    : base(context)
  {
    this.case_value = case_value;
    this.statements = statements;
    this.completion = new Completion();
  }

  internal override object Evaluate() => this.statements.Evaluate();

  internal Completion Evaluate(object expression)
  {
    return StrictEquality.JScriptStrictEquals(this.case_value.Evaluate(), expression) ? (Completion) this.statements.Evaluate() : (Completion) null;
  }

  internal bool IsDefault() => this.case_value == null;

  internal override AST PartiallyEvaluate()
  {
    if (this.case_value != null)
      this.case_value = this.case_value.PartiallyEvaluate();
    this.statements = this.statements.PartiallyEvaluate();
    return (AST) this;
  }

  internal void TranslateToConditionalBranch(
    ILGenerator il,
    Type etype,
    bool branchIfTrue,
    Label label,
    bool shortForm)
  {
    // Short-form branches corrupt the IL emitted by PersistedAssemblyBuilder on
    // .NET 10.0.x (ControlFlowBuilder desyncs on an operand at a blob boundary).
    shortForm = false;
    Type type1 = etype;
    Type type2 = Convert.ToType(this.case_value.InferType((JSField) null));
    if (type1 != type2 && type1.IsPrimitive && type2.IsPrimitive)
    {
      if (type1 == Typeob.Single && type2 == Typeob.Double)
        type2 = Typeob.Single;
      else if (Convert.IsPromotableTo((IReflect) type2, (IReflect) type1))
        type2 = type1;
      else if (Convert.IsPromotableTo((IReflect) type1, (IReflect) type2))
        type1 = type2;
    }
    bool flag = true;
    if (type1 == type2 && type1 != Typeob.Object)
    {
      Convert.Emit((AST) this, il, etype, type1);
      if (!type1.IsPrimitive && type1.IsValueType)
        il.Emit(OpCodes.Box, type1);
      this.case_value.context.EmitLineInfo(il);
      this.case_value.TranslateToIL(il, type1);
      if (type1 == Typeob.String)
        il.Emit(OpCodes.Call, CompilerGlobals.stringEqualsMethod);
      else if (!type1.IsPrimitive)
      {
        if (type1.IsValueType)
          il.Emit(OpCodes.Box, type1);
        il.Emit(OpCodes.Callvirt, CompilerGlobals.equalsMethod);
      }
      else
        flag = false;
    }
    else
    {
      Convert.Emit((AST) this, il, etype, Typeob.Object);
      this.case_value.context.EmitLineInfo(il);
      this.case_value.TranslateToIL(il, Typeob.Object);
      il.Emit(OpCodes.Call, CompilerGlobals.jScriptStrictEqualsMethod);
    }
    if (branchIfTrue)
    {
      if (flag)
        il.Emit(shortForm ? OpCodes.Brtrue_S : OpCodes.Brtrue, label);
      else
        il.Emit(shortForm ? OpCodes.Beq_S : OpCodes.Beq, label);
    }
    else if (flag)
      il.Emit(shortForm ? OpCodes.Brfalse_S : OpCodes.Brfalse, label);
    else
      il.Emit(shortForm ? OpCodes.Bne_Un_S : OpCodes.Bne_Un, label);
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    this.statements.TranslateToIL(il, Typeob.Void);
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
    if (this.case_value != null)
      this.case_value.TranslateToILInitializer(il);
    this.statements.TranslateToILInitializer(il);
  }
}
