// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ForIn
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

/// <summary>Represents the JScript for...in statement. This class belongs to the abstract syntax tree category.</summary>
public sealed class ForIn : AST
{
  private AST var;
  private AST initializer;
  private AST collection;
  private AST body;
  private Completion completion;
  private Context inExpressionContext;

  internal ForIn(Context context, AST var, AST initializer, AST collection, AST body)
    : base(context)
  {
    if (var != null)
    {
      this.var = var;
      this.inExpressionContext = this.var.context.Clone();
    }
    else
    {
      VariableDeclaration variableDeclaration = (VariableDeclaration) initializer;
      this.var = (AST) variableDeclaration.identifier;
      if (variableDeclaration.initializer == null)
        variableDeclaration.initializer = (AST) new ConstantWrapper((object) null, (Context) null);
      this.inExpressionContext = initializer.context.Clone();
    }
    this.initializer = initializer;
    this.collection = collection;
    this.inExpressionContext.UpdateWith(this.collection.context);
    this.body = body;
    this.completion = new Completion();
  }

  internal override object Evaluate()
  {
    AST var = this.var;
    if (this.initializer != null)
      this.initializer.Evaluate();
    this.completion.Continue = 0;
    this.completion.Exit = 0;
    this.completion.value = (object) null;
    object forInObject = Convert.ToForInObject(this.collection.Evaluate(), this.Engine);
    bool flag = forInObject is ScriptObject;
    IEnumerator enumerator;
    try
    {
      enumerator = ForIn.JScriptGetEnumerator(forInObject);
    }
    catch (JScriptException ex)
    {
      ex.context = this.collection.context;
      throw ex;
    }
    while (enumerator.MoveNext())
    {
      var.SetValue(enumerator.Current);
      Completion completion = (Completion) this.body.Evaluate();
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
    }
    return (object) this.completion;
  }

  /// <summary>Returns an enumerator that iterates through the specified collection.</summary>
  /// <returns>An enumerator that iterates through <paramref name="coll" />.</returns>
  /// <param name="coll">The collection for which to get an enumerator.</param>
  public static IEnumerator JScriptGetEnumerator(object coll)
  {
    switch (coll)
    {
      case IEnumerator _:
        return (IEnumerator) coll;
      case ScriptObject _:
        return (IEnumerator) new ScriptObjectPropertyEnumerator((ScriptObject) coll);
      case Array _:
        Array array = (Array) coll;
        return (IEnumerator) new RangeEnumerator(array.GetLowerBound(0), array.GetUpperBound(0));
      case IEnumerable _:
        return ((IEnumerable) coll).GetEnumerator() ?? (IEnumerator) new ScriptObjectPropertyEnumerator((ScriptObject) new JSObject());
      default:
        throw new JScriptException(JSError.NotCollection);
    }
  }

  internal override AST PartiallyEvaluate()
  {
    this.var = this.var.PartiallyEvaluateAsReference();
    this.var.SetPartialValue((AST) new ConstantWrapper((object) null, (Context) null));
    if (this.initializer != null)
      this.initializer = this.initializer.PartiallyEvaluate();
    this.collection = this.collection.PartiallyEvaluate();
    IReflect c = this.collection.InferType((JSField) null);
    if (c is ClassScope && ((ClassScope) c).noExpando && !((ClassScope) c).ImplementsInterface((IReflect) Typeob.IEnumerable) || (object) c != (object) Typeob.Object && (object) (c as Type) != null && !Typeob.ScriptObject.IsAssignableFrom((Type) c) && !Typeob.IEnumerable.IsAssignableFrom((Type) c) && !Typeob.IConvertible.IsAssignableFrom((Type) c) && !Typeob.IEnumerator.IsAssignableFrom((Type) c))
      this.collection.context.HandleError(JSError.NotCollection);
    ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
    while (true)
    {
      switch (scriptObject)
      {
        case WithObject _:
          scriptObject = scriptObject.GetParent();
          continue;
        case FunctionScope _:
          goto label_7;
        default:
          goto label_8;
      }
    }
label_7:
    FunctionScope functionScope = (FunctionScope) scriptObject;
    BitArray definedFlags = functionScope.DefinedFlags;
    this.body = this.body.PartiallyEvaluate();
    functionScope.DefinedFlags = definedFlags;
    goto label_9;
label_8:
    this.body = this.body.PartiallyEvaluate();
label_9:
    return (AST) this;
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    Label label1 = il.DefineLabel();
    Label label2 = il.DefineLabel();
    Label label3 = il.DefineLabel();
    this.compilerGlobals.BreakLabelStack.Push((object) label2);
    this.compilerGlobals.ContinueLabelStack.Push((object) label1);
    if (this.initializer != null)
      this.initializer.TranslateToIL(il, Typeob.Void);
    this.inExpressionContext.EmitLineInfo(il);
    this.collection.TranslateToIL(il, Typeob.Object);
    this.EmitILToLoadEngine(il);
    il.Emit(OpCodes.Call, CompilerGlobals.toForInObjectMethod);
    il.Emit(OpCodes.Call, CompilerGlobals.jScriptGetEnumeratorMethod);
    LocalBuilder local1 = il.DeclareLocal(Typeob.IEnumerator);
    il.Emit(OpCodes.Stloc, local1);
    il.Emit(OpCodes.Br, label1);
    il.MarkLabel(label3);
    this.body.TranslateToIL(il, Typeob.Void);
    il.MarkLabel(label1);
    this.context.EmitLineInfo(il);
    il.Emit(OpCodes.Ldloc, local1);
    il.Emit(OpCodes.Callvirt, CompilerGlobals.moveNextMethod);
    il.Emit(OpCodes.Brfalse, label2);
    il.Emit(OpCodes.Ldloc, local1);
    il.Emit(OpCodes.Callvirt, CompilerGlobals.getCurrentMethod);
    Type type = Convert.ToType(this.var.InferType((JSField) null));
    LocalBuilder local2 = il.DeclareLocal(type);
    Convert.Emit((AST) this, il, Typeob.Object, type);
    il.Emit(OpCodes.Stloc, local2);
    this.var.TranslateToILPreSet(il);
    il.Emit(OpCodes.Ldloc, local2);
    this.var.TranslateToILSet(il);
    il.Emit(OpCodes.Br, label3);
    il.MarkLabel(label2);
    this.compilerGlobals.BreakLabelStack.Pop();
    this.compilerGlobals.ContinueLabelStack.Pop();
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
    this.var.TranslateToILInitializer(il);
    if (this.initializer != null)
      this.initializer.TranslateToILInitializer(il);
    this.collection.TranslateToILInitializer(il);
    this.body.TranslateToILInitializer(il);
  }
}
