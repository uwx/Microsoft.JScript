// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.AST
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System.CodeDom.Compiler;
using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents the abstract syntax tree that is used to parse JScript code.</summary>
public abstract class AST
{
  internal Context context;

  internal AST(Context context) => this.context = context;

  internal virtual void CheckIfOKToUseInSuperConstructorCall()
  {
  }

  internal CompilerGlobals compilerGlobals => this.context.document.compilerGlobals;

  internal virtual bool Delete() => true;

  internal void EmitILToLoadEngine(ILGenerator il)
  {
    ScriptObject scriptObject = this.Engine.ScriptObjectStackTop();
    while (scriptObject != null && (scriptObject is WithObject || scriptObject is BlockScope))
      scriptObject = scriptObject.GetParent();
    switch (scriptObject)
    {
      case FunctionScope _:
        ((FunctionScope) scriptObject).owner.TranslateToILToLoadEngine(il);
        break;
      case ClassScope _:
        if (this.Engine.doCRS)
        {
          il.Emit(OpCodes.Ldsfld, CompilerGlobals.contextEngineField);
          break;
        }
        if (this.context.document.engine.PEFileKind == PEFileKinds.Dll)
        {
          il.Emit(OpCodes.Ldtoken, (Type) ((ClassScope) scriptObject).GetTypeBuilder());
          il.Emit(OpCodes.Call, CompilerGlobals.createVsaEngineWithType);
          break;
        }
        il.Emit(OpCodes.Call, CompilerGlobals.createVsaEngine);
        break;
      default:
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldfld, CompilerGlobals.engineField);
        break;
    }
  }

  internal VsaEngine Engine => this.context.document.engine;

  internal abstract object Evaluate();

  internal virtual LateBinding EvaluateAsLateBinding()
  {
    return new LateBinding((string) null, this.Evaluate(), VsaEngine.executeForJSEE);
  }

  internal virtual WrappedNamespace EvaluateAsWrappedNamespace(bool giveErrorIfNameInUse)
  {
    throw new JScriptException(JSError.InternalError, this.context);
  }

  internal Globals Globals => this.context.document.engine.Globals;

  internal virtual bool HasReturn() => false;

  internal virtual IReflect InferType(JSField inference_target) => (IReflect) Typeob.Object;

  internal virtual void InvalidateInferredTypes()
  {
  }

  internal virtual bool OkToUseAsType() => false;

  internal abstract AST PartiallyEvaluate();

  internal virtual AST PartiallyEvaluateAsCallable()
  {
    return (AST) new CallableExpression(this.PartiallyEvaluate());
  }

  internal virtual AST PartiallyEvaluateAsReference() => this.PartiallyEvaluate();

  internal virtual void ResolveCall(
    ASTList args,
    IReflect[] argIRs,
    bool constructor,
    bool brackets)
  {
    throw new JScriptException(JSError.InternalError, this.context);
  }

  internal virtual object ResolveCustomAttribute(ASTList args, IReflect[] argIRs, AST target)
  {
    throw new JScriptException(JSError.InternalError, this.context);
  }

  internal virtual void SetPartialValue(AST partial_value)
  {
    this.context.HandleError(JSError.IllegalAssignment);
  }

  internal virtual void SetValue(object value)
  {
    this.context.HandleError(JSError.IllegalAssignment);
  }

  internal virtual void TranslateToConditionalBranch(
    ILGenerator il,
    bool branchIfTrue,
    Label label,
    bool shortForm)
  {
    // Short-form branches corrupt the IL emitted by PersistedAssemblyBuilder on
    // .NET 10.0.x (ControlFlowBuilder desyncs on an operand at a blob boundary).
    shortForm = false;
    IReflect reflect = this.InferType((JSField) null);
    if ((object) reflect != (object) Typeob.Object && (object) (reflect as Type) != null)
    {
      string name = branchIfTrue ? "op_True" : "op_False";
      MethodInfo method = reflect.GetMethod(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, (Binder) null, new Type[1]
      {
        (Type) reflect
      }, (ParameterModifier[]) null);
      if (method != (MethodInfo) null)
      {
        this.TranslateToIL(il, (Type) reflect);
        il.Emit(OpCodes.Call, method);
        il.Emit(OpCodes.Brtrue, label);
        return;
      }
    }
    Type type = Convert.ToType(reflect);
    this.TranslateToIL(il, type);
    Convert.Emit(this, il, type, Typeob.Boolean, true);
    if (branchIfTrue)
      il.Emit(shortForm ? OpCodes.Brtrue_S : OpCodes.Brtrue, label);
    else
      il.Emit(shortForm ? OpCodes.Brfalse_S : OpCodes.Brfalse, label);
  }

  internal abstract void TranslateToIL(ILGenerator il, Type rtype);

  internal virtual void TranslateToILCall(
    ILGenerator il,
    Type rtype,
    ASTList args,
    bool construct,
    bool brackets)
  {
    throw new JScriptException(JSError.InternalError, this.context);
  }

  internal virtual void TranslateToILDelete(ILGenerator il, Type rtype)
  {
    if (!(rtype != Typeob.Void))
      return;
    il.Emit(OpCodes.Ldc_I4_1);
    Convert.Emit(this, il, Typeob.Boolean, rtype);
  }

  internal virtual void TranslateToILInitializer(ILGenerator il)
  {
    throw new JScriptException(JSError.InternalError, this.context);
  }

  internal virtual void TranslateToILPreSet(ILGenerator il)
  {
    throw new JScriptException(JSError.InternalError, this.context);
  }

  internal virtual void TranslateToILPreSet(ILGenerator il, ASTList args)
  {
    this.TranslateToIL(il, Typeob.Object);
    args.TranslateToIL(il, Typeob.ArrayOfObject);
  }

  internal virtual void TranslateToILPreSetPlusGet(ILGenerator il)
  {
    throw new JScriptException(JSError.InternalError, this.context);
  }

  internal virtual void TranslateToILPreSetPlusGet(ILGenerator il, ASTList args, bool inBrackets)
  {
    il.Emit(OpCodes.Ldnull);
    this.TranslateToIL(il, Typeob.Object);
    il.Emit(OpCodes.Dup);
    LocalBuilder local1 = il.DeclareLocal(Typeob.Object);
    il.Emit(OpCodes.Stloc, local1);
    args.TranslateToIL(il, Typeob.ArrayOfObject);
    il.Emit(OpCodes.Dup);
    LocalBuilder local2 = il.DeclareLocal(Typeob.ArrayOfObject);
    il.Emit(OpCodes.Stloc, local2);
    il.Emit(OpCodes.Ldc_I4_0);
    if (inBrackets)
      il.Emit(OpCodes.Ldc_I4_1);
    else
      il.Emit(OpCodes.Ldc_I4_0);
    this.EmitILToLoadEngine(il);
    il.Emit(OpCodes.Call, CompilerGlobals.callValueMethod);
    LocalBuilder local3 = il.DeclareLocal(Typeob.Object);
    il.Emit(OpCodes.Stloc, local3);
    il.Emit(OpCodes.Ldloc, local1);
    il.Emit(OpCodes.Ldloc, local2);
    il.Emit(OpCodes.Ldloc, local3);
  }

  internal void TranslateToILSet(ILGenerator il) => this.TranslateToILSet(il, (AST) null);

  internal virtual void TranslateToILSet(ILGenerator il, AST rhvalue)
  {
    rhvalue?.TranslateToIL(il, Typeob.Object);
    il.Emit(OpCodes.Call, CompilerGlobals.setIndexedPropertyValueStaticMethod);
  }

  internal virtual object TranslateToILReference(ILGenerator il, Type rtype)
  {
    this.TranslateToIL(il, rtype);
    LocalBuilder local = il.DeclareLocal(rtype);
    il.Emit(OpCodes.Stloc, local);
    il.Emit(OpCodes.Ldloca, local);
    return (object) local;
  }

  internal virtual Context GetFirstExecutableContext() => this.context;
}
