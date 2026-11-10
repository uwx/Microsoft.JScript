// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.With
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents the with statement. This class belongs to the abstract syntax tree category.</summary>
public sealed class With : AST
{
  private AST obj;
  private AST block;
  private Completion completion;
  private FunctionScope enclosing_function;

  internal With(Context context, AST obj, AST block)
    : base(context)
  {
    this.obj = obj;
    this.block = block;
    this.completion = new Completion();
    ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
    if (scriptObject is FunctionScope)
      this.enclosing_function = (FunctionScope) scriptObject;
    else
      this.enclosing_function = (FunctionScope) null;
  }

  internal override object Evaluate()
  {
    try
    {
      With.JScriptWith(this.obj.Evaluate(), this.Engine);
    }
    catch (JScriptException ex)
    {
      ex.context = this.obj.context;
      throw ex;
    }
    Completion completion = (Completion) null;
    try
    {
      completion = (Completion) this.block.Evaluate();
    }
    finally
    {
      this.Globals.ScopeStack.Pop();
    }
    this.completion.Continue = completion.Continue <= 1 ? 0 : completion.Continue - 1;
    this.completion.Exit = completion.Exit <= 0 ? 0 : completion.Exit - 1;
    return completion.Return ? (object) completion : (object) this.completion;
  }

  /// <summary>Establishes the default object for a with statement block by adding it to the scope stack.</summary>
  /// <returns>
  /// <paramref name="withOb" /> converted to an object.</returns>
  /// <param name="withOb">The default object.</param>
  /// <param name="engine">A reference to the scripting engine.</param>
  public static object JScriptWith(object withOb, VsaEngine engine)
  {
    object contained_object = Convert.ToObject(withOb, engine);
    if (contained_object == null)
      throw new JScriptException(JSError.ObjectExpected);
    Globals globals = engine.Globals;
    globals.ScopeStack.GuardedPush((object) new WithObject(globals.ScopeStack.Peek(), contained_object));
    return contained_object;
  }

  internal override AST PartiallyEvaluate()
  {
    this.obj = this.obj.PartiallyEvaluate();
    WithObject withObject;
    if (this.obj is ConstantWrapper)
    {
      object contained_object = Convert.ToObject(this.obj.Evaluate(), this.Engine);
      withObject = new WithObject(this.Globals.ScopeStack.Peek(), contained_object);
      if (contained_object is JSObject && ((JSObject) contained_object).noExpando)
        withObject.isKnownAtCompileTime = true;
    }
    else
      withObject = new WithObject(this.Globals.ScopeStack.Peek(), (object) new JSObject((ScriptObject) null, false));
    this.Globals.ScopeStack.Push((object) withObject);
    try
    {
      this.block = this.block.PartiallyEvaluate();
    }
    finally
    {
      this.Globals.ScopeStack.Pop();
    }
    return (AST) this;
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    this.context.EmitLineInfo(il);
    this.Globals.ScopeStack.Push((object) new WithObject(this.Globals.ScopeStack.Peek(), (object) new JSObject((ScriptObject) null, false)));
    bool insideProtectedRegion = this.compilerGlobals.InsideProtectedRegion;
    this.compilerGlobals.InsideProtectedRegion = true;
    Label loc = il.DefineLabel();
    this.compilerGlobals.BreakLabelStack.Push((object) loc);
    this.compilerGlobals.ContinueLabelStack.Push((object) loc);
    this.obj.TranslateToIL(il, Typeob.Object);
    this.EmitILToLoadEngine(il);
    il.Emit(OpCodes.Call, CompilerGlobals.jScriptWithMethod);
    LocalBuilder local = (LocalBuilder) null;
    if (this.context.document.debugOn)
    {
      il.BeginScope();
      local = il.DeclareLocal(Typeob.Object);
      local.SetLocalSymInfo("with()");
      il.Emit(OpCodes.Stloc, local);
    }
    else
      il.Emit(OpCodes.Pop);
    il.BeginExceptionBlock();
    this.block.TranslateToILInitializer(il);
    this.block.TranslateToIL(il, Typeob.Void);
    il.BeginFinallyBlock();
    if (this.context.document.debugOn)
    {
      il.Emit(OpCodes.Ldnull);
      il.Emit(OpCodes.Stloc, local);
    }
    this.EmitILToLoadEngine(il);
    il.Emit(OpCodes.Call, CompilerGlobals.popScriptObjectMethod);
    il.Emit(OpCodes.Pop);
    il.EndExceptionBlock();
    if (this.context.document.debugOn)
      il.EndScope();
    il.MarkLabel(loc);
    this.compilerGlobals.BreakLabelStack.Pop();
    this.compilerGlobals.ContinueLabelStack.Pop();
    this.compilerGlobals.InsideProtectedRegion = insideProtectedRegion;
    this.Globals.ScopeStack.Pop();
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
    this.obj.TranslateToILInitializer(il);
  }
}
