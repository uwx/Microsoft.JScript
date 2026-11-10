// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Call
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal class Call : AST
{
  internal AST func;
  private ASTList args;
  private object[] argValues;
  private int outParameterCount;
  internal bool isConstructor;
  internal bool inBrackets;
  private FunctionScope enclosingFunctionScope;
  private bool alreadyPartiallyEvaluated;
  private bool isAssignmentToDefaultIndexedProperty;

  internal Call(Context context, AST func, ASTList args, bool inBrackets)
    : base(context)
  {
    this.func = func;
    this.args = args == null ? new ASTList(context) : args;
    this.argValues = (object[]) null;
    this.outParameterCount = 0;
    int i = 0;
    for (int count = this.args.count; i < count; ++i)
    {
      if (this.args[i] is AddressOf)
        ++this.outParameterCount;
    }
    this.isConstructor = false;
    this.inBrackets = inBrackets;
    this.enclosingFunctionScope = (FunctionScope) null;
    this.alreadyPartiallyEvaluated = false;
    this.isAssignmentToDefaultIndexedProperty = false;
    ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
    while (!(scriptObject is FunctionScope))
    {
      scriptObject = scriptObject.GetParent();
      if (scriptObject == null)
        return;
    }
    this.enclosingFunctionScope = (FunctionScope) scriptObject;
  }

  private bool AllParamsAreMissing()
  {
    int i = 0;
    for (int count = this.args.count; i < count; ++i)
    {
      AST ast = this.args[i];
      if (!(ast is ConstantWrapper) || ((ConstantWrapper) ast).value != System.Reflection.Missing.Value)
        return false;
    }
    return true;
  }

  private IReflect[] ArgIRs()
  {
    int count = this.args.count;
    IReflect[] reflectArray = new IReflect[count];
    for (int i = 0; i < count; ++i)
    {
      AST ast = this.args[i];
      IReflect ir = reflectArray[i] = ast.InferType((JSField) null);
      if (ast is AddressOf)
      {
        if (ir is ClassScope)
          ir = (IReflect) ((ClassScope) ir).GetBakedSuperType();
        reflectArray[i] = (IReflect) Convert.ToType("&", Convert.ToType(ir));
      }
    }
    return reflectArray;
  }

  internal bool CanBeFunctionDeclaration()
  {
    bool flag = this.func is Lookup && this.outParameterCount == 0;
    if (flag)
    {
      int i = 0;
      for (int count = this.args.count; i < count; ++i)
      {
        flag = this.args[i] is Lookup;
        if (!flag)
          break;
      }
    }
    return flag;
  }

  internal override void CheckIfOKToUseInSuperConstructorCall()
  {
    this.func.CheckIfOKToUseInSuperConstructorCall();
  }

  internal override bool Delete()
  {
    object[] asArray = this.args == null ? (object[]) null : this.args.EvaluateAsArray();
    int length = asArray.Length;
    object obj = this.func.Evaluate();
    if (obj == null || length == 0)
      return true;
    Type type = obj.GetType();
    MethodInfo method = type.GetMethod("op_Delete", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, (Binder) null, new Type[2]
    {
      type,
      Typeob.ArrayOfObject
    }, (ParameterModifier[]) null);
    if (method == (MethodInfo) null || (method.Attributes & MethodAttributes.SpecialName) == MethodAttributes.PrivateScope || method.ReturnType != Typeob.Boolean)
      return LateBinding.DeleteMember(obj, Convert.ToString(asArray[length - 1]));
    return (bool) new JSMethodInfo(method).Invoke((object) null, new object[2]
    {
      obj,
      (object) asArray
    });
  }

  internal override object Evaluate()
  {
    if (this.outParameterCount > 0 && VsaEngine.executeForJSEE)
      throw new JScriptException(JSError.RefParamsNonSupportedInDebugger);
    LateBinding asLateBinding = this.func.EvaluateAsLateBinding();
    object[] asArray = this.args == null ? (object[]) null : this.args.EvaluateAsArray();
    this.Globals.CallContextStack.Push((object) new CallContext(this.context, asLateBinding, asArray));
    try
    {
      object obj = !(this.func is CallableExpression func) || !(func.expression is Call) ? asLateBinding.Call(asArray, this.isConstructor, this.inBrackets, this.Engine) : LateBinding.CallValue(asLateBinding.obj, asArray, this.isConstructor, this.inBrackets, this.Engine, func.GetObject2(), (Binder) JSBinder.ob, (CultureInfo) null, (string[]) null);
      if (this.outParameterCount > 0)
      {
        int i = 0;
        for (int count = this.args.count; i < count; ++i)
        {
          if (this.args[i] is AddressOf)
            this.args[i].SetValue(asArray[i]);
        }
      }
      return obj;
    }
    catch (TargetInvocationException ex)
    {
      JScriptException jscriptException;
      if (ex.InnerException is JScriptException)
      {
        jscriptException = (JScriptException) ex.InnerException;
        if (jscriptException.context == null)
          jscriptException.context = jscriptException.Number != -2146823281 ? this.context : this.func.context;
      }
      else
        jscriptException = new JScriptException(ex.InnerException, this.context);
      throw jscriptException;
    }
    catch (JScriptException ex)
    {
      if (ex.context == null)
        ex.context = ex.Number != -2146823281 ? this.context : this.func.context;
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

  internal void EvaluateIndices() => this.argValues = this.args.EvaluateAsArray();

  internal IdentifierLiteral GetName()
  {
    return new IdentifierLiteral(this.func.ToString(), this.func.context);
  }

  internal void GetParameters(ArrayList parameters)
  {
    int i = 0;
    for (int count = this.args.count; i < count; ++i)
    {
      AST ast = this.args[i];
      parameters.Add((object) new ParameterDeclaration(ast.context, ast.ToString(), (TypeExpression) null, (CustomAttributeList) null));
    }
  }

  internal override IReflect InferType(JSField inference_target)
  {
    if (this.func is Binding)
      return ((Binding) this.func).InferTypeOfCall(inference_target, this.isConstructor);
    if (this.func is ConstantWrapper)
    {
      object obj = ((ConstantWrapper) this.func).value;
      if ((object) (obj as Type) == null)
      {
        switch (obj)
        {
          case ClassScope _:
          case TypedArray _:
            break;
          default:
            goto label_6;
        }
      }
      return (IReflect) obj;
    }
label_6:
    return (IReflect) Typeob.Object;
  }

  private JSLocalField[] LocalsThatWereOutParameters()
  {
    int outParameterCount = this.outParameterCount;
    if (outParameterCount == 0)
      return (JSLocalField[]) null;
    JSLocalField[] jsLocalFieldArray = new JSLocalField[outParameterCount];
    int num = 0;
    for (int i = 0; i < outParameterCount; ++i)
    {
      AST ast = this.args[i];
      if (ast is AddressOf)
      {
        FieldInfo field = ((AddressOf) ast).GetField();
        if (field is JSLocalField)
          jsLocalFieldArray[num++] = (JSLocalField) field;
      }
    }
    return jsLocalFieldArray;
  }

  internal void MakeDeletable()
  {
    if (!(this.func is Binding))
      return;
    Binding func = (Binding) this.func;
    func.InvalidateBinding();
    func.PartiallyEvaluateAsCallable();
    func.ResolveLHValue();
  }

  internal override AST PartiallyEvaluate()
  {
    if (this.alreadyPartiallyEvaluated)
      return (AST) this;
    this.alreadyPartiallyEvaluated = true;
    if (this.inBrackets && this.AllParamsAreMissing())
    {
      if (this.isConstructor)
        this.args.context.HandleError(JSError.TypeMismatch);
      return (AST) new ConstantWrapper((object) new TypedArray(((TypeExpression) new TypeExpression(this.func).PartiallyEvaluate()).ToIReflect(), this.args.count + 1), this.context);
    }
    this.func = this.func.PartiallyEvaluateAsCallable();
    this.args = (ASTList) this.args.PartiallyEvaluate();
    IReflect[] argIRs = this.ArgIRs();
    this.func.ResolveCall(this.args, argIRs, this.isConstructor, this.inBrackets);
    if (!this.isConstructor && !this.inBrackets && this.func is Binding && this.args.count == 1)
    {
      Binding func = (Binding) this.func;
      if ((object) (func.member as Type) != null)
      {
        Type member = (Type) func.member;
        if (this.args[0] is ConstantWrapper constantWrapper)
        {
          try
          {
            if (constantWrapper.value == null || constantWrapper.value is DBNull)
              return (AST) this;
            return constantWrapper.isNumericLiteral && (member == Typeob.Decimal || member == Typeob.Int64 || member == Typeob.UInt64 || member == Typeob.Single) ? (AST) new ConstantWrapper(Convert.CoerceT((object) constantWrapper.context.GetCode(), member, true), this.context) : (AST) new ConstantWrapper(Convert.CoerceT(constantWrapper.Evaluate(), member, true), this.context);
          }
          catch
          {
            constantWrapper.context.HandleError(JSError.TypeMismatch);
          }
        }
        else if (!Binding.AssignmentCompatible((IReflect) member, this.args[0], argIRs[0], false))
          this.args[0].context.HandleError(JSError.ImpossibleConversion);
      }
      else if (func.member is JSVariableField)
      {
        JSVariableField member = (JSVariableField) func.member;
        if (member.IsLiteral)
        {
          if (member.value is ClassScope)
          {
            ClassScope classScope = (ClassScope) member.value;
            IReflect underlyingTypeIfEnum = classScope.GetUnderlyingTypeIfEnum();
            if (underlyingTypeIfEnum != null)
            {
              if (!Convert.IsPromotableTo(argIRs[0], underlyingTypeIfEnum) && !Convert.IsPromotableTo(underlyingTypeIfEnum, argIRs[0]) && ((object) argIRs[0] != (object) Typeob.String || underlyingTypeIfEnum == classScope))
                this.args[0].context.HandleError(JSError.ImpossibleConversion);
            }
            else if (!Convert.IsPromotableTo(argIRs[0], (IReflect) classScope) && !Convert.IsPromotableTo((IReflect) classScope, argIRs[0]))
              this.args[0].context.HandleError(JSError.ImpossibleConversion);
          }
          else if (member.value is TypedArray)
          {
            TypedArray typedArray = (TypedArray) member.value;
            if (!Convert.IsPromotableTo(argIRs[0], (IReflect) typedArray) && !Convert.IsPromotableTo((IReflect) typedArray, argIRs[0]))
              this.args[0].context.HandleError(JSError.ImpossibleConversion);
          }
        }
      }
    }
    return (AST) this;
  }

  internal override AST PartiallyEvaluateAsReference()
  {
    this.func = this.func.PartiallyEvaluateAsCallable();
    this.args = (ASTList) this.args.PartiallyEvaluate();
    return (AST) this;
  }

  internal override void SetPartialValue(AST partial_value)
  {
    if (this.isConstructor)
      this.context.HandleError(JSError.IllegalAssignment);
    else if (this.func is Binding)
    {
      ((Binding) this.func).SetPartialValue(this.args, this.ArgIRs(), partial_value, this.inBrackets);
    }
    else
    {
      if (!(this.func is ThisLiteral))
        return;
      ((ThisLiteral) this.func).ResolveAssignmentToDefaultIndexedProperty(this.args, this.ArgIRs(), partial_value);
    }
  }

  internal override void SetValue(object value)
  {
    LateBinding asLateBinding = this.func.EvaluateAsLateBinding();
    try
    {
      asLateBinding.SetIndexedPropertyValue(this.argValues != null ? this.argValues : this.args.EvaluateAsArray(), value);
    }
    catch (JScriptException ex)
    {
      if (ex.context == null)
        ex.context = this.func.context;
      throw ex;
    }
    catch (Exception ex)
    {
      throw new JScriptException(ex, this.func.context);
    }
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    if (this.context.document.debugOn)
      il.Emit(OpCodes.Nop);
    bool flag = true;
    if (this.enclosingFunctionScope != null && this.enclosingFunctionScope.owner != null)
    {
      if (this.func is Binding func && !this.enclosingFunctionScope.closuresMightEscape)
      {
        if (func.member is JSLocalField)
          this.enclosingFunctionScope.owner.TranslateToILToSaveLocals(il);
        else
          flag = false;
      }
      else
        this.enclosingFunctionScope.owner.TranslateToILToSaveLocals(il);
    }
    this.func.TranslateToILCall(il, rtype, this.args, this.isConstructor, this.inBrackets);
    if (flag && this.enclosingFunctionScope != null && this.enclosingFunctionScope.owner != null)
    {
      if (this.outParameterCount == 0)
        this.enclosingFunctionScope.owner.TranslateToILToRestoreLocals(il);
      else
        this.enclosingFunctionScope.owner.TranslateToILToRestoreLocals(il, this.LocalsThatWereOutParameters());
    }
    if (!this.context.document.debugOn)
      return;
    il.Emit(OpCodes.Nop);
  }

  internal CustomAttribute ToCustomAttribute()
  {
    return new CustomAttribute(this.context, this.func, this.args);
  }

  internal override void TranslateToILDelete(ILGenerator il, Type rtype)
  {
    IReflect ir = this.func.InferType((JSField) null);
    Type type = Convert.ToType(ir);
    this.func.TranslateToIL(il, type);
    this.args.TranslateToIL(il, Typeob.ArrayOfObject);
    if (this.func is Binding)
    {
      MethodInfo meth;
      if (ir is ClassScope)
        meth = (MethodInfo) ((ClassScope) ir).owner.deleteOpMethod;
      else
        meth = ir.GetMethod("op_Delete", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, (Binder) null, new Type[2]
        {
          type,
          Typeob.ArrayOfObject
        }, (ParameterModifier[]) null);
      if (meth != (MethodInfo) null && (meth.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope && meth.ReturnType == Typeob.Boolean)
      {
        il.Emit(OpCodes.Call, meth);
        Convert.Emit((AST) this, il, Typeob.Boolean, rtype);
        return;
      }
    }
    ConstantWrapper.TranslateToILInt(il, this.args.count - 1);
    il.Emit(OpCodes.Ldelem_Ref);
    Convert.Emit((AST) this, il, Typeob.Object, Typeob.String);
    il.Emit(OpCodes.Call, CompilerGlobals.deleteMemberMethod);
    Convert.Emit((AST) this, il, Typeob.Boolean, rtype);
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
    this.func.TranslateToILInitializer(il);
    this.args.TranslateToILInitializer(il);
  }

  internal override void TranslateToILPreSet(ILGenerator il)
  {
    this.func.TranslateToILPreSet(il, this.args);
  }

  internal override void TranslateToILPreSet(ILGenerator il, ASTList args)
  {
    this.isAssignmentToDefaultIndexedProperty = true;
    base.TranslateToILPreSet(il, args);
  }

  internal override void TranslateToILPreSetPlusGet(ILGenerator il)
  {
    this.func.TranslateToILPreSetPlusGet(il, this.args, this.inBrackets);
  }

  internal override void TranslateToILSet(ILGenerator il, AST rhvalue)
  {
    if (this.isAssignmentToDefaultIndexedProperty)
      base.TranslateToILSet(il, rhvalue);
    else
      this.func.TranslateToILSet(il, rhvalue);
  }
}
