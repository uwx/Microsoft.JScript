// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Eval
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

/// <summary>Evaluates JScript code. This class belongs to the abstract syntax tree category.</summary>
public sealed class Eval : AST
{
  private AST operand;
  private AST unsafeOption;
  private FunctionScope enclosingFunctionScope;

  internal Eval(Context context, AST operand, AST unsafeOption)
    : base(context)
  {
    this.operand = operand;
    this.unsafeOption = unsafeOption;
    ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
    ((IActivationObject) scriptObject).GetGlobalScope().evilScript = true;
    if (scriptObject is ActivationObject)
      ((ActivationObject) scriptObject).isKnownAtCompileTime = this.Engine.doFast;
    if (scriptObject is FunctionScope)
    {
      this.enclosingFunctionScope = (FunctionScope) scriptObject;
      this.enclosingFunctionScope.mustSaveStackLocals = true;
      for (ScriptObject parent = this.enclosingFunctionScope.GetParent(); parent != null; parent = parent.GetParent())
      {
        if (parent is FunctionScope functionScope)
        {
          functionScope.mustSaveStackLocals = true;
          functionScope.closuresMightEscape = true;
        }
      }
    }
    else
      this.enclosingFunctionScope = (FunctionScope) null;
  }

  internal override void CheckIfOKToUseInSuperConstructorCall()
  {
    this.context.HandleError(JSError.NotAllowedInSuperConstructorCall);
  }

  internal override object Evaluate()
  {
    if (VsaEngine.executeForJSEE)
      throw new JScriptException(JSError.NonSupportedInDebugger);
    object source = this.operand.Evaluate();
    object unsafeOption = (object) null;
    if (this.unsafeOption != null)
      unsafeOption = this.unsafeOption.Evaluate();
    this.Globals.CallContextStack.Push((object) new CallContext(this.context, (LateBinding) null, new object[2]
    {
      source,
      unsafeOption
    }));
    try
    {
      return Eval.JScriptEvaluate(source, unsafeOption, this.Engine);
    }
    catch (JScriptException ex)
    {
      if (ex.context == null)
        ex.context = this.context;
      throw ex;
    }
    catch (Exception ex)
    {
      throw new JScriptException(ex, this.context);
    }
    finally
    {
      this.Globals.CallContextStack.Pop();
    }
  }

  /// <summary>Evaluates the specified JScript code by using the specified engine.</summary>
  /// <returns>The result of the evaluation. If <paramref name="source" /> is not a string, return value is <paramref name="source" />.</returns>
  /// <param name="source">The code to evaluate.</param>
  /// <param name="engine">A reference to the scripting engine to use to evaluate <paramref name="source" />.</param>
  public static object JScriptEvaluate(object source, VsaEngine engine)
  {
    return Convert.GetTypeCode(source) != TypeCode.String ? source : Eval.DoEvaluate(source, engine, true);
  }

  /// <summary>Evaluates the specified JScript code by using the specified engine and safety option.</summary>
  /// <returns>The result of the evaluation. If <paramref name="source" /> is not a string, return value is <paramref name="source" />.</returns>
  /// <param name="source">The code to evaluate.</param>
  /// <param name="unsafeOption">The string literal unsafe to enable unsafe evaluation; otherwise, false.</param>
  /// <param name="engine">A reference to the scripting engine to use to evaluate <paramref name="source" />.</param>
  public static object JScriptEvaluate(object source, object unsafeOption, VsaEngine engine)
  {
    if (Convert.GetTypeCode(source) != TypeCode.String)
      return source;
    bool isUnsafe = false;
    if (Convert.GetTypeCode(unsafeOption) == TypeCode.String && ((IConvertible) unsafeOption).ToString() == "unsafe")
      isUnsafe = true;
    return Eval.DoEvaluate(source, engine, isUnsafe);
  }

  private static object DoEvaluate(object source, VsaEngine engine, bool isUnsafe)
  {
    if (engine.doFast)
      engine.PushScriptObject((ScriptObject) new BlockScope(engine.ScriptObjectStackTop()));
    try
    {
      return ((Completion) new JSParser(new Context(new DocumentContext("eval code", engine), ((IConvertible) source).ToString())).ParseEvalBody().PartiallyEvaluate().Evaluate()).value;
    }
    finally
    {
      if (engine.doFast)
        engine.PopScriptObject();
    }
  }

  internal override AST PartiallyEvaluate()
  {
    VsaEngine engine = this.Engine;
    ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
    ClassScope classScope = ClassScope.ScopeOfClassMemberInitializer(scriptObject);
    if (classScope != null)
    {
      if (classScope.inStaticInitializerCode)
        classScope.staticInitializerUsesEval = true;
      else
        classScope.instanceInitializerUsesEval = true;
    }
    if (engine.doFast)
    {
      engine.PushScriptObject((ScriptObject) new BlockScope(scriptObject));
    }
    else
    {
      for (; scriptObject is WithObject || scriptObject is BlockScope; scriptObject = scriptObject.GetParent())
      {
        if (scriptObject is BlockScope)
          ((ActivationObject) scriptObject).isKnownAtCompileTime = false;
      }
    }
    try
    {
      this.operand = this.operand.PartiallyEvaluate();
      if (this.unsafeOption != null)
        this.unsafeOption = this.unsafeOption.PartiallyEvaluate();
      if (this.enclosingFunctionScope != null && this.enclosingFunctionScope.owner == null)
        this.context.HandleError(JSError.NotYetImplemented);
      return (AST) this;
    }
    finally
    {
      if (engine.doFast)
        this.Engine.PopScriptObject();
    }
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    if (this.enclosingFunctionScope != null && this.enclosingFunctionScope.owner != null)
      this.enclosingFunctionScope.owner.TranslateToILToSaveLocals(il);
    this.operand.TranslateToIL(il, Typeob.Object);
    MethodInfo meth = (MethodInfo) null;
    if (this.unsafeOption is ConstantWrapper unsafeOption && unsafeOption.value is string str && str == "unsafe")
      meth = CompilerGlobals.jScriptEvaluateMethod1;
    if (meth == (MethodInfo) null)
    {
      meth = CompilerGlobals.jScriptEvaluateMethod2;
      if (this.unsafeOption == null)
        il.Emit(OpCodes.Ldnull);
      else
        this.unsafeOption.TranslateToIL(il, Typeob.Object);
    }
    this.EmitILToLoadEngine(il);
    il.Emit(OpCodes.Call, meth);
    Convert.Emit((AST) this, il, Typeob.Object, rtype);
    if (this.enclosingFunctionScope == null || this.enclosingFunctionScope.owner == null)
      return;
    this.enclosingFunctionScope.owner.TranslateToILToRestoreLocals(il);
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
    this.operand.TranslateToILInitializer(il);
    if (this.unsafeOption == null)
      return;
    this.unsafeOption.TranslateToILInitializer(il);
  }
}
