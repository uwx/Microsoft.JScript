// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.For
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class For : AST
{
  private AST initializer;
  private AST condition;
  private AST incrementer;
  private AST body;
  private Completion completion;

  internal For(Context context, AST initializer, AST condition, AST incrementer, AST body)
    : base(context)
  {
    this.initializer = initializer;
    this.condition = condition;
    this.incrementer = incrementer;
    this.body = body;
    this.completion = new Completion();
  }

  internal override object Evaluate()
  {
    this.completion.Continue = 0;
    this.completion.Exit = 0;
    this.completion.value = (object) null;
    this.initializer.Evaluate();
    while (Convert.ToBoolean(this.condition.Evaluate()))
    {
      Completion completion = (Completion) this.body.Evaluate();
      if (completion.value != null)
        this.completion.value = completion.value;
      if (completion.Continue > 1)
      {
        this.completion.Continue = completion.Continue - 1;
        break;
      }
      if (completion.Exit > 0)
      {
        this.completion.Exit = completion.Exit - 1;
        break;
      }
      if (completion.Return)
        return (object) completion;
      this.incrementer.Evaluate();
    }
    return (object) this.completion;
  }

  internal override AST PartiallyEvaluate()
  {
    this.initializer = this.initializer.PartiallyEvaluate();
    ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
    while (true)
    {
      switch (scriptObject)
      {
        case WithObject _:
          scriptObject = scriptObject.GetParent();
          continue;
        case FunctionScope _:
          goto label_3;
        default:
          goto label_4;
      }
    }
label_3:
    FunctionScope functionScope = (FunctionScope) scriptObject;
    BitArray definedFlags = functionScope.DefinedFlags;
    this.condition = this.condition.PartiallyEvaluate();
    this.body = this.body.PartiallyEvaluate();
    functionScope.DefinedFlags = definedFlags;
    this.incrementer = this.incrementer.PartiallyEvaluate();
    functionScope.DefinedFlags = definedFlags;
    goto label_5;
label_4:
    this.condition = this.condition.PartiallyEvaluate();
    this.body = this.body.PartiallyEvaluate();
    this.incrementer = this.incrementer.PartiallyEvaluate();
label_5:
    IReflect reflect = this.condition.InferType((JSField) null);
    if (reflect is FunctionPrototype || (object) reflect == (object) Typeob.ScriptFunction)
      this.context.HandleError(JSError.SuspectLoopCondition);
    return (AST) this;
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    Label label1 = il.DefineLabel();
    Label loc = il.DefineLabel();
    il.DefineLabel();
    Label label2 = il.DefineLabel();
    bool flag = false;
    this.compilerGlobals.BreakLabelStack.Push((object) label2);
    this.compilerGlobals.ContinueLabelStack.Push((object) loc);
    if (!(this.initializer is EmptyLiteral))
    {
      this.initializer.context.EmitLineInfo(il);
      this.initializer.TranslateToIL(il, Typeob.Void);
    }
    il.MarkLabel(label1);
    if (!(this.condition is ConstantWrapper) || !(this.condition.Evaluate() is bool) || !(bool) this.condition.Evaluate())
    {
      this.condition.context.EmitLineInfo(il);
      this.condition.TranslateToConditionalBranch(il, false, label2, false);
    }
    else if (this.condition.context.StartPosition + 1 == this.condition.context.EndPosition)
      flag = true;
    this.body.TranslateToIL(il, Typeob.Void);
    il.MarkLabel(loc);
    if (!(this.incrementer is EmptyLiteral))
    {
      this.incrementer.context.EmitLineInfo(il);
      this.incrementer.TranslateToIL(il, Typeob.Void);
    }
    else if (flag)
      this.context.EmitLineInfo(il);
    il.Emit(OpCodes.Br, label1);
    il.MarkLabel(label2);
    this.compilerGlobals.BreakLabelStack.Pop();
    this.compilerGlobals.ContinueLabelStack.Pop();
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
    this.initializer.TranslateToILInitializer(il);
    this.condition.TranslateToILInitializer(il);
    this.incrementer.TranslateToILInitializer(il);
    this.body.TranslateToILInitializer(il);
  }
}
