// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.CallableExpression
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class CallableExpression : Binding
{
  internal AST expression;
  private IReflect expressionInferredType;

  internal CallableExpression(AST expression)
    : base(expression.context, "")
  {
    this.expression = expression;
    JSLocalField inference_target = new JSLocalField("", (FunctionScope) null, 0, (object) Missing.Value);
    this.expressionInferredType = expression.InferType((JSField) inference_target);
    inference_target.inferred_type = this.expressionInferredType;
    this.member = (MemberInfo) inference_target;
    this.members = new MemberInfo[1]
    {
      (MemberInfo) inference_target
    };
  }

  internal override LateBinding EvaluateAsLateBinding()
  {
    return new LateBinding((string) null, this.expression.Evaluate(), VsaEngine.executeForJSEE);
  }

  protected override object GetObject() => this.GetObject2();

  internal object GetObject2()
  {
    return !(this.expression is Call expression) || !expression.inBrackets ? Convert.ToObject(this.expression.Evaluate(), this.Engine) : Convert.ToObject(expression.func.Evaluate(), this.Engine);
  }

  protected override void HandleNoSuchMemberError()
  {
    throw new JScriptException(JSError.InternalError, this.context);
  }

  internal override AST PartiallyEvaluate() => (AST) this;

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    this.expression.TranslateToIL(il, rtype);
  }

  internal override void TranslateToILCall(
    ILGenerator il,
    Type rtype,
    ASTList argList,
    bool construct,
    bool brackets)
  {
    if (this.defaultMember != (MemberInfo) null & construct & brackets)
    {
      base.TranslateToILCall(il, rtype, argList, construct, brackets);
    }
    else
    {
      JSGlobalField member = this.member as JSGlobalField;
      if ((FieldInfo) member != (FieldInfo) null && member.IsLiteral && argList.count == 1)
      {
        Type type = Convert.ToType((IReflect) member.value);
        argList[0].TranslateToIL(il, type);
        Convert.Emit((AST) this, il, type, rtype);
      }
      else
      {
        this.TranslateToILWithDupOfThisOb(il);
        argList.TranslateToIL(il, Typeob.ArrayOfObject);
        if (construct)
          il.Emit(OpCodes.Ldc_I4_1);
        else
          il.Emit(OpCodes.Ldc_I4_0);
        if (brackets)
          il.Emit(OpCodes.Ldc_I4_1);
        else
          il.Emit(OpCodes.Ldc_I4_0);
        this.EmitILToLoadEngine(il);
        il.Emit(OpCodes.Call, CompilerGlobals.callValueMethod);
        Convert.Emit((AST) this, il, Typeob.Object, rtype);
      }
    }
  }

  protected override void TranslateToILObject(ILGenerator il, Type obType, bool noValue)
  {
    this.EmitILToLoadEngine(il);
    il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
    il.Emit(OpCodes.Castclass, Typeob.IActivationObject);
    il.Emit(OpCodes.Callvirt, CompilerGlobals.getGlobalScopeMethod);
  }

  protected override void TranslateToILWithDupOfThisOb(ILGenerator il)
  {
    if (!(this.expression is Call expression) || !expression.inBrackets)
    {
      this.TranslateToILObject(il, (Type) null, false);
    }
    else
    {
      if (expression.isConstructor && expression.inBrackets)
      {
        expression.TranslateToIL(il, Typeob.Object);
        il.Emit(OpCodes.Dup);
        return;
      }
      expression.func.TranslateToIL(il, Typeob.Object);
    }
    this.expression.TranslateToIL(il, Typeob.Object);
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
    this.expression.TranslateToILInitializer(il);
    if (this.expressionInferredType.Equals((object) this.expression.InferType((JSField) null)))
      return;
    MemberInfo[] members = this.members;
    this.InvalidateBinding();
    this.members = members;
  }
}
