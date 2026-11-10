// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.StaticInitializer
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class StaticInitializer : AST
{
  private FunctionObject func;
  private Completion completion;

  internal StaticInitializer(Context context, Block body, FunctionScope own_scope)
    : base(context)
  {
    this.func = new FunctionObject((string) null, new ParameterDeclaration[0], (TypeExpression) null, body, own_scope, this.Globals.ScopeStack.Peek(), context, MethodAttributes.Private | MethodAttributes.Static);
    this.func.isMethod = true;
    this.func.hasArgumentsObject = false;
    this.completion = new Completion();
  }

  internal override object Evaluate()
  {
    this.func.Call(new object[0], (object) ((IActivationObject) this.Globals.ScopeStack.Peek()).GetGlobalScope());
    return (object) this.completion;
  }

  internal override AST PartiallyEvaluate()
  {
    this.func.PartiallyEvaluate();
    return (AST) this;
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    this.func.TranslateBodyToIL(il, this.compilerGlobals);
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
    throw new JScriptException(JSError.InternalError, this.context);
  }
}
