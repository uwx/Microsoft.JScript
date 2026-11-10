// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ThisLiteral
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class ThisLiteral : AST
{
  internal bool isSuper;
  private MethodInfo method;

  internal ThisLiteral(Context context, bool isSuper)
    : base(context)
  {
    this.isSuper = isSuper;
    this.method = (MethodInfo) null;
  }

  internal override void CheckIfOKToUseInSuperConstructorCall()
  {
    this.context.HandleError(JSError.NotAllowedInSuperConstructorCall);
  }

  internal override object Evaluate()
  {
    ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
    while (true)
    {
      switch (scriptObject)
      {
        case WithObject _:
        case BlockScope _:
          scriptObject = scriptObject.GetParent();
          continue;
        case StackFrame _:
          goto label_3;
        default:
          goto label_4;
      }
    }
label_3:
    return ((StackFrame) scriptObject).thisObject;
label_4:
    return ((GlobalScope) scriptObject).thisObject;
  }

  internal override IReflect InferType(JSField inference_target)
  {
    if (this.method != (MethodInfo) null)
    {
      ParameterInfo[] parameters = this.method.GetParameters();
      return parameters == null || parameters.Length == 0 ? (IReflect) this.method.ReturnType : (IReflect) parameters[0].ParameterType;
    }
    ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
    while (true)
    {
      switch (scriptObject)
      {
        case WithObject _:
          scriptObject = scriptObject.GetParent();
          continue;
        case GlobalScope _:
          goto label_7;
        case FunctionScope _:
          goto label_8;
        default:
          goto label_12;
      }
    }
label_7:
    return (IReflect) scriptObject;
label_8:
    if (((FunctionScope) scriptObject).isMethod)
    {
      ClassScope enclosingScope = (ClassScope) ((FunctionScope) scriptObject).owner.enclosing_scope;
      return this.isSuper ? enclosingScope.GetSuperType() : (IReflect) enclosingScope;
    }
label_12:
    return (IReflect) Typeob.Object;
  }

  internal override AST PartiallyEvaluate()
  {
    ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
    while (scriptObject is WithObject)
      scriptObject = scriptObject.GetParent();
    bool flag = false;
    if (scriptObject is FunctionScope)
      flag = ((FunctionScope) scriptObject).isStatic && ((FunctionScope) scriptObject).isMethod;
    else if (scriptObject is StackFrame)
      flag = ((StackFrame) scriptObject).thisObject is Type;
    if (!flag)
      return (AST) this;
    this.context.HandleError(JSError.NotAccessible);
    return new Lookup("this", this.context).PartiallyEvaluate();
  }

  internal override AST PartiallyEvaluateAsReference()
  {
    this.context.HandleError(JSError.CantAssignThis);
    return new Lookup("this", this.context).PartiallyEvaluateAsReference();
  }

  internal void ResolveAssignmentToDefaultIndexedProperty(
    ASTList args,
    IReflect[] argIRs,
    AST rhvalue)
  {
    IReflect reflect = this.InferType((JSField) null);
    Type t = (object) (reflect as Type) != null ? (Type) reflect : (Type) null;
    if (reflect is ClassScope)
      t = ((ClassScope) reflect).GetBakedSuperType();
    MemberInfo[] defaultMembers = JSBinder.GetDefaultMembers(t);
    if (defaultMembers != null)
    {
      if (defaultMembers.Length != 0)
      {
        try
        {
          PropertyInfo prop = JSBinder.SelectProperty(defaultMembers, argIRs);
          if (prop != (PropertyInfo) null)
          {
            this.method = JSProperty.GetSetMethod(prop, true);
            if (this.method == (MethodInfo) null)
              this.context.HandleError(JSError.AssignmentToReadOnly, true);
            if (Binding.CheckParameters(prop.GetIndexParameters(), argIRs, args, this.context, 0, false, true))
              return;
            this.method = (MethodInfo) null;
            return;
          }
        }
        catch (AmbiguousMatchException ex)
        {
          this.context.HandleError(JSError.AmbiguousMatch);
          return;
        }
      }
    }
    this.context.HandleError(JSError.NotIndexable, reflect is ClassScope ? ((ActivationObject) reflect).GetName() : ((Type) reflect).Name);
  }

  internal override void ResolveCall(
    ASTList args,
    IReflect[] argIRs,
    bool constructor,
    bool brackets)
  {
    if (constructor || !brackets)
    {
      if (this.isSuper)
        this.context.HandleError(JSError.IllegalUseOfSuper);
      else
        this.context.HandleError(JSError.IllegalUseOfThis);
    }
    else
    {
      IReflect reflect = this.InferType((JSField) null);
      Type t = (object) (reflect as Type) != null ? (Type) reflect : (Type) null;
      if (reflect is ClassScope)
        t = ((ClassScope) reflect).GetBakedSuperType();
      MemberInfo[] defaultMembers = JSBinder.GetDefaultMembers(t);
      if (defaultMembers != null)
      {
        if (defaultMembers.Length != 0)
        {
          try
          {
            this.method = JSBinder.SelectMethod(defaultMembers, argIRs);
            if (this.method != (MethodInfo) null)
            {
              if (Binding.CheckParameters(this.method.GetParameters(), argIRs, args, this.context, 0, false, true))
                return;
              this.method = (MethodInfo) null;
              return;
            }
          }
          catch (AmbiguousMatchException ex)
          {
            this.context.HandleError(JSError.AmbiguousMatch);
            return;
          }
        }
      }
      this.context.HandleError(JSError.NotIndexable, reflect is ClassScope ? ((ActivationObject) reflect).GetName() : ((Type) reflect).Name);
    }
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    if (rtype == Typeob.Void)
      return;
    if (this.InferType((JSField) null) is GlobalScope)
    {
      this.EmitILToLoadEngine(il);
      if (rtype == Typeob.LenientGlobalObject)
      {
        il.Emit(OpCodes.Call, CompilerGlobals.getLenientGlobalObjectMethod);
      }
      else
      {
        il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
        il.Emit(OpCodes.Castclass, Typeob.IActivationObject);
        il.Emit(OpCodes.Callvirt, CompilerGlobals.getDefaultThisObjectMethod);
      }
    }
    else
    {
      il.Emit(OpCodes.Ldarg_0);
      Convert.Emit((AST) this, il, Convert.ToType(this.InferType((JSField) null)), rtype);
    }
  }

  internal override void TranslateToILCall(
    ILGenerator il,
    Type rtype,
    ASTList argList,
    bool construct,
    bool brackets)
  {
    MethodInfo method = this.method;
    if (method != (MethodInfo) null)
    {
      Type reflectedType = method.ReflectedType;
      if (!method.IsStatic)
      {
        this.method = (MethodInfo) null;
        this.TranslateToIL(il, reflectedType);
        this.method = method;
      }
      ParameterInfo[] parameters = method.GetParameters();
      Binding.PlaceArgumentsOnStack(il, parameters, argList, 0, 0, (AST) Binding.ReflectionMissingCW);
      if (method.IsVirtual && !method.IsFinal && (!reflectedType.IsSealed || !reflectedType.IsValueType))
        il.Emit(OpCodes.Callvirt, method);
      else
        il.Emit(OpCodes.Call, method);
      Convert.Emit((AST) this, il, method.ReturnType, rtype);
    }
    else
      base.TranslateToILCall(il, rtype, argList, construct, brackets);
  }

  internal override void TranslateToILPreSet(ILGenerator il, ASTList argList)
  {
    MethodInfo method = this.method;
    if (method != (MethodInfo) null)
    {
      Type reflectedType = method.ReflectedType;
      if (!method.IsStatic)
        this.TranslateToIL(il, reflectedType);
      Binding.PlaceArgumentsOnStack(il, method.GetParameters(), argList, 0, 1, (AST) Binding.ReflectionMissingCW);
    }
    else
      base.TranslateToILPreSet(il, argList);
  }

  internal override void TranslateToILSet(ILGenerator il, AST rhvalue)
  {
    MethodInfo method = this.method;
    if (method != (MethodInfo) null)
    {
      rhvalue?.TranslateToIL(il, method.GetParameters()[0].ParameterType);
      Type reflectedType = method.ReflectedType;
      if (method.IsVirtual && !method.IsFinal && (!reflectedType.IsSealed || !reflectedType.IsValueType))
        il.Emit(OpCodes.Callvirt, method);
      else
        il.Emit(OpCodes.Call, method);
      if (!(method.ReturnType != Typeob.Void))
        return;
      il.Emit(OpCodes.Pop);
    }
    else
      base.TranslateToILSet(il, rhvalue);
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
  }
}
