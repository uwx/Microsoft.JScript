// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ConstructorCall
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal class ConstructorCall : AST
{
  internal bool isOK;
  internal bool isSuperConstructorCall;
  internal ASTList arguments;

  internal ConstructorCall(Context context, ASTList arguments, bool isSuperConstructorCall)
    : base(context)
  {
    this.isOK = false;
    this.isSuperConstructorCall = isSuperConstructorCall;
    if (arguments == null)
      this.arguments = new ASTList(context);
    else
      this.arguments = arguments;
  }

  internal override object Evaluate() => (object) new Completion();

  internal override AST PartiallyEvaluate()
  {
    if (!this.isOK)
    {
      this.context.HandleError(JSError.NotOKToCallSuper);
      return (AST) this;
    }
    int i = 0;
    for (int count = this.arguments.count; i < count; ++i)
    {
      this.arguments[i] = this.arguments[i].PartiallyEvaluate();
      this.arguments[i].CheckIfOKToUseInSuperConstructorCall();
    }
    ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
    if (!(scriptObject is FunctionScope))
    {
      this.context.HandleError(JSError.NotOKToCallSuper);
      return (AST) this;
    }
    if (!((FunctionScope) scriptObject).owner.isConstructor)
      this.context.HandleError(JSError.NotOKToCallSuper);
    ((FunctionScope) scriptObject).owner.superConstructorCall = this;
    return (AST) this;
  }

  internal override AST PartiallyEvaluateAsReference()
  {
    throw new JScriptException(JSError.InternalError);
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
  }
}
