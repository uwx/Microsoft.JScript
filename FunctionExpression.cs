// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.FunctionExpression
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a function object. This class belongs to the abstract syntax tree category.</summary>
public sealed class FunctionExpression : AST
{
  private FunctionObject func;
  private string name;
  private JSVariableField field;
  private LocalBuilder func_local;
  private static int uniqueNumber;

  internal FunctionExpression(
    Context context,
    AST id,
    ParameterDeclaration[] formal_parameters,
    TypeExpression return_type,
    Block body,
    FunctionScope own_scope,
    FieldAttributes attributes)
    : base(context)
  {
    if (attributes != FieldAttributes.PrivateScope)
    {
      this.context.HandleError(JSError.SyntaxError);
      attributes = FieldAttributes.PrivateScope;
    }
    ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
    this.name = id.ToString();
    if (this.name.Length == 0)
      this.name = "anonymous " + FunctionExpression.uniqueNumber++.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    else
      this.AddNameTo(scriptObject);
    this.func = new FunctionObject(this.name, formal_parameters, return_type, body, own_scope, scriptObject, this.context, MethodAttributes.Public | MethodAttributes.Static);
  }

  private void AddNameTo(ScriptObject enclosingScope)
  {
    while (enclosingScope is WithObject)
      enclosingScope = enclosingScope.GetParent();
    if (((IActivationObject) enclosingScope).GetLocalField(this.name) != (FieldInfo) null)
      return;
    FieldInfo fieldInfo = !(enclosingScope is ActivationObject) ? (FieldInfo) ((StackFrame) enclosingScope).AddNewField(this.name, (object) null, FieldAttributes.Public) : (!(enclosingScope is FunctionScope) ? (FieldInfo) ((ActivationObject) enclosingScope).AddNewField(this.name, (object) null, FieldAttributes.Public | FieldAttributes.Static) : (FieldInfo) ((ActivationObject) enclosingScope).AddNewField(this.name, (object) null, FieldAttributes.Public));
    JSLocalField jsLocalField = fieldInfo as JSLocalField;
    if ((FieldInfo) jsLocalField != (FieldInfo) null)
    {
      jsLocalField.debugOn = this.context.document.debugOn;
      jsLocalField.isDefined = true;
    }
    this.field = (JSVariableField) fieldInfo;
  }

  internal override object Evaluate()
  {
    if (VsaEngine.executeForJSEE)
      throw new JScriptException(JSError.NonSupportedInDebugger);
    this.func.own_scope.SetParent(this.Globals.ScopeStack.Peek());
    Closure closure = new Closure(this.func);
    if ((FieldInfo) this.field != (FieldInfo) null)
      this.field.value = (object) closure;
    return (object) closure;
  }

  internal override IReflect InferType(JSField inference_target)
  {
    return (IReflect) Typeob.ScriptFunction;
  }

  /// <summary>Creates a new function object, using the specified information.</summary>
  /// <returns>A new function object.</returns>
  /// <param name="handle">A handle to the type of the function.</param>
  /// <param name="name">The name of the function.</param>
  /// <param name="method_name">The name of the function.</param>
  /// <param name="formal_params">The parameters of the function.</param>
  /// <param name="fields">The fields of the function.</param>
  /// <param name="must_save_stack_locals">true to save local stack variables. This is important for a nested function that accesses one of its local variables, or for an eval statement.</param>
  /// <param name="hasArgumentsObject">true if the function has a parameter array.</param>
  /// <param name="text">The text of the function declaration.</param>
  /// <param name="engine">A reference to the scripting engine.</param>
  public static FunctionObject JScriptFunctionExpression(
    RuntimeTypeHandle handle,
    string name,
    string method_name,
    string[] formal_params,
    JSLocalField[] fields,
    bool must_save_stack_locals,
    bool hasArgumentsObject,
    string text,
    VsaEngine engine)
  {
    return new FunctionObject(Type.GetTypeFromHandle(handle), name, method_name, formal_params, fields, must_save_stack_locals, hasArgumentsObject, text, engine);
  }

  internal override AST PartiallyEvaluate()
  {
    ScriptObject scope = this.Globals.ScopeStack.Peek();
    if (ClassScope.ScopeOfClassMemberInitializer(scope) != null)
    {
      this.context.HandleError(JSError.MemberInitializerCannotContainFuncExpr);
      return (AST) this;
    }
    ScriptObject scriptObject = scope;
    while (scriptObject is WithObject || scriptObject is BlockScope)
      scriptObject = scriptObject.GetParent();
    if (scriptObject is FunctionScope functionScope)
      functionScope.closuresMightEscape = true;
    if (scriptObject != scope)
      this.func.own_scope.SetParent((ScriptObject) new WithObject((ScriptObject) new JSObject(), (object) this.func.own_scope.GetGlobalScope()));
    this.func.PartiallyEvaluate();
    return (AST) this;
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    if (rtype == Typeob.Void)
      return;
    il.Emit(OpCodes.Ldloc, this.func_local);
    il.Emit(OpCodes.Newobj, CompilerGlobals.closureConstructor);
    Convert.Emit((AST) this, il, Typeob.Closure, rtype);
    if (!((FieldInfo) this.field != (FieldInfo) null))
      return;
    il.Emit(OpCodes.Dup);
    object metaData = this.field.GetMetaData();
    if (metaData is LocalBuilder)
      il.Emit(OpCodes.Stloc, (LocalBuilder) metaData);
    else
      il.Emit(OpCodes.Stsfld, (FieldInfo) metaData);
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
    this.func.TranslateToIL(this.compilerGlobals);
    this.func_local = il.DeclareLocal(Typeob.FunctionObject);
    il.Emit(OpCodes.Ldtoken, (Type) this.func.classwriter);
    il.Emit(OpCodes.Ldstr, this.name);
    il.Emit(OpCodes.Ldstr, this.func.GetName());
    int length1 = this.func.formal_parameters.Length;
    ConstantWrapper.TranslateToILInt(il, length1);
    il.Emit(OpCodes.Newarr, Typeob.String);
    for (int i = 0; i < length1; ++i)
    {
      il.Emit(OpCodes.Dup);
      ConstantWrapper.TranslateToILInt(il, i);
      il.Emit(OpCodes.Ldstr, this.func.formal_parameters[i]);
      il.Emit(OpCodes.Stelem_Ref);
    }
    int length2 = this.func.fields.Length;
    ConstantWrapper.TranslateToILInt(il, length2);
    il.Emit(OpCodes.Newarr, Typeob.JSLocalField);
    for (int i = 0; i < length2; ++i)
    {
      JSLocalField field = this.func.fields[i];
      il.Emit(OpCodes.Dup);
      ConstantWrapper.TranslateToILInt(il, i);
      il.Emit(OpCodes.Ldstr, field.Name);
      il.Emit(OpCodes.Ldtoken, field.FieldType);
      ConstantWrapper.TranslateToILInt(il, field.slotNumber);
      il.Emit(OpCodes.Newobj, CompilerGlobals.jsLocalFieldConstructor);
      il.Emit(OpCodes.Stelem_Ref);
    }
    if (this.func.must_save_stack_locals)
      il.Emit(OpCodes.Ldc_I4_1);
    else
      il.Emit(OpCodes.Ldc_I4_0);
    if (this.func.hasArgumentsObject)
      il.Emit(OpCodes.Ldc_I4_1);
    else
      il.Emit(OpCodes.Ldc_I4_0);
    il.Emit(OpCodes.Ldstr, this.func.ToString());
    this.EmitILToLoadEngine(il);
    il.Emit(OpCodes.Call, CompilerGlobals.jScriptFunctionExpressionMethod);
    il.Emit(OpCodes.Stloc, this.func_local);
  }
}
