// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Lookup
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

internal sealed class Lookup : Binding
{
  private int lexLevel;
  private int evalLexLevel;
  private LocalBuilder fieldLoc;
  private LocalBuilder refLoc;
  private LateBinding lateBinding;
  private bool thereIsAnObjectOnTheStack;

  internal Lookup(Context context)
    : base(context, context.GetCode())
  {
    this.lexLevel = 0;
    this.evalLexLevel = 0;
    this.fieldLoc = (LocalBuilder) null;
    this.refLoc = (LocalBuilder) null;
    this.lateBinding = (LateBinding) null;
    this.thereIsAnObjectOnTheStack = false;
  }

  internal Lookup(string name, Context context)
    : this(context)
  {
    this.name = name;
  }

  internal string Name => this.name;

  private void BindName()
  {
    int num1 = 0;
    int num2 = 0;
    ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
    BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
    bool flag1 = false;
    bool flag2 = false;
    for (; scriptObject != null; scriptObject = scriptObject.GetParent())
    {
      WithObject withObject = scriptObject as WithObject;
      MemberInfo[] memberInfoArray = !(withObject != null & flag2) ? scriptObject.GetMember(this.name, bindingAttr) : withObject.GetMember(this.name, bindingAttr, false);
      this.members = memberInfoArray;
      if (memberInfoArray.Length == 0)
      {
        switch (scriptObject)
        {
          case WithObject _:
            this.isFullyResolved = this.isFullyResolved && ((WithObject) scriptObject).isKnownAtCompileTime;
            ++num1;
            break;
          case ActivationObject _:
            this.isFullyResolved = this.isFullyResolved && ((ActivationObject) scriptObject).isKnownAtCompileTime;
            if (scriptObject is BlockScope || scriptObject is FunctionScope && ((FunctionScope) scriptObject).mustSaveStackLocals)
              ++num1;
            if (scriptObject is ClassScope)
            {
              if (flag1)
                flag2 = true;
              if (((ClassScope) scriptObject).owner.isStatic)
              {
                bindingAttr &= ~BindingFlags.Instance;
                flag1 = true;
                break;
              }
              break;
            }
            break;
          case StackFrame _:
            ++num1;
            break;
        }
        ++num2;
      }
      else
        break;
    }
    if (this.members.Length == 0)
      return;
    this.lexLevel = num1;
    this.evalLexLevel = num2;
  }

  internal bool CanPlaceAppropriateObjectOnStack(object ob)
  {
    if (ob is LenientGlobalObject)
      return true;
    ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
    for (int lexLevel = this.lexLevel; lexLevel > 0 && (scriptObject is WithObject || scriptObject is BlockScope); scriptObject = scriptObject.GetParent())
    {
      if (scriptObject is WithObject)
        --lexLevel;
    }
    return scriptObject is WithObject || scriptObject is GlobalScope;
  }

  internal override void CheckIfOKToUseInSuperConstructorCall()
  {
    FieldInfo member1 = this.member as FieldInfo;
    if (member1 != (FieldInfo) null)
    {
      if (member1.IsStatic)
        return;
      this.context.HandleError(JSError.NotAllowedInSuperConstructorCall);
    }
    else
    {
      MethodInfo member2 = this.member as MethodInfo;
      if (member2 != (MethodInfo) null)
      {
        if (member2.IsStatic)
          return;
        this.context.HandleError(JSError.NotAllowedInSuperConstructorCall);
      }
      else
      {
        PropertyInfo member3 = this.member as PropertyInfo;
        if (!(member3 != (PropertyInfo) null))
          return;
        MethodInfo getMethod = JSProperty.GetGetMethod(member3, true);
        if (getMethod != (MethodInfo) null && !getMethod.IsStatic)
        {
          this.context.HandleError(JSError.NotAllowedInSuperConstructorCall);
        }
        else
        {
          MethodInfo setMethod = JSProperty.GetSetMethod(member3, true);
          if (!(setMethod != (MethodInfo) null) || setMethod.IsStatic)
            return;
          this.context.HandleError(JSError.NotAllowedInSuperConstructorCall);
        }
      }
    }
  }

  internal override object Evaluate()
  {
    ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
    if (!this.isFullyResolved)
    {
      object memberValue = ((IActivationObject) scriptObject).GetMemberValue(this.name, this.evalLexLevel);
      if (!(memberValue is Missing))
        return memberValue;
    }
    if (this.members == null && !VsaEngine.executeForJSEE)
    {
      this.BindName();
      this.ResolveRHValue();
    }
    object obj = base.Evaluate();
    return !(obj is Missing) ? obj : throw new JScriptException(JSError.UndefinedIdentifier, this.context);
  }

  internal override LateBinding EvaluateAsLateBinding()
  {
    if (!this.isFullyResolved)
    {
      this.BindName();
      this.isFullyResolved = false;
    }
    if (this.defaultMember == this.member)
      this.defaultMember = (MemberInfo) null;
    object obj = this.GetObject();
    LateBinding asLateBinding = this.lateBinding ?? (this.lateBinding = new LateBinding(this.name, obj, VsaEngine.executeForJSEE));
    asLateBinding.obj = obj;
    asLateBinding.last_object = obj;
    asLateBinding.last_members = this.members;
    asLateBinding.last_member = this.member;
    if (!this.isFullyResolved)
      this.members = (MemberInfo[]) null;
    return asLateBinding;
  }

  internal override WrappedNamespace EvaluateAsWrappedNamespace(bool giveErrorIfNameInUse)
  {
    Namespace @namespace = Namespace.GetNamespace(this.name, this.Engine);
    GlobalScope globalScope = ((IActivationObject) this.Globals.ScopeStack.Peek()).GetGlobalScope();
    FieldInfo fieldInfo1 = giveErrorIfNameInUse ? globalScope.GetLocalField(this.name) : globalScope.GetField(this.name, BindingFlags.Static | BindingFlags.Public);
    if (fieldInfo1 != (FieldInfo) null)
    {
      if (giveErrorIfNameInUse && (!fieldInfo1.IsLiteral || !(fieldInfo1.GetValue((object) null) is Namespace)))
        this.context.HandleError(JSError.DuplicateName, true);
    }
    else
    {
      FieldInfo fieldInfo2 = (FieldInfo) globalScope.AddNewField(this.name, (object) @namespace, FieldAttributes.Public | FieldAttributes.Literal);
      ((JSVariableField) fieldInfo2).type = new TypeExpression((AST) new ConstantWrapper((object) Typeob.Namespace, this.context));
      ((JSVariableField) fieldInfo2).originalContext = this.context;
    }
    return new WrappedNamespace(this.name, this.Engine);
  }

  protected override object GetObject()
  {
    ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
    object obj;
    if (this.member is JSMemberField)
    {
      for (; scriptObject != null; scriptObject = scriptObject.GetParent())
      {
        if (scriptObject is StackFrame stackFrame)
        {
          obj = stackFrame.closureInstance;
          goto label_10;
        }
      }
      return (object) null;
    }
    for (int evalLexLevel = this.evalLexLevel; evalLexLevel > 0; --evalLexLevel)
      scriptObject = scriptObject.GetParent();
    obj = (object) scriptObject;
label_10:
    if (this.defaultMember != (MemberInfo) null)
    {
      switch (this.defaultMember.MemberType)
      {
        case MemberTypes.Event:
          return (object) null;
        case MemberTypes.Field:
          return ((FieldInfo) this.defaultMember).GetValue(obj);
        case MemberTypes.Method:
          return ((MethodBase) this.defaultMember).Invoke(obj, new object[0]);
        case MemberTypes.Property:
          return ((PropertyInfo) this.defaultMember).GetValue(obj, (object[]) null);
        case MemberTypes.NestedType:
          return (object) this.member;
      }
    }
    return obj;
  }

  protected override void HandleNoSuchMemberError()
  {
    if (!this.isFullyResolved)
      return;
    this.context.HandleError(JSError.UndeclaredVariable, this.Engine.doFast);
  }

  internal override IReflect InferType(JSField inference_target)
  {
    return !this.isFullyResolved ? (IReflect) Typeob.Object : base.InferType(inference_target);
  }

  internal bool InFunctionNestedInsideInstanceMethod()
  {
    ScriptObject scriptObject1 = this.Globals.ScopeStack.Peek();
    while (true)
    {
      switch (scriptObject1)
      {
        case WithObject _:
        case BlockScope _:
          scriptObject1 = scriptObject1.GetParent();
          continue;
        default:
          goto label_3;
      }
    }
label_3:
    ScriptObject scriptObject2;
    for (FunctionScope functionScope = scriptObject1 as FunctionScope; functionScope != null; functionScope = scriptObject2 as FunctionScope)
    {
      if (functionScope.owner.isMethod)
        return !functionScope.owner.isStatic;
      scriptObject2 = functionScope.owner.enclosing_scope;
      while (true)
      {
        switch (scriptObject2)
        {
          case WithObject _:
          case BlockScope _:
            scriptObject2 = scriptObject2.GetParent();
            continue;
          default:
            goto label_10;
        }
      }
label_10:;
    }
    return false;
  }

  internal bool InStaticCode()
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
        case FunctionScope functionScope:
          return functionScope.isStatic;
        case StackFrame stackFrame:
          return stackFrame.thisObject is Type;
        case ClassScope classScope:
          return classScope.inStaticInitializerCode;
        default:
          return true;
      }
    }
  }

  internal override AST PartiallyEvaluate()
  {
    this.BindName();
    if (this.members == null || this.members.Length == 0)
    {
      ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
      while (true)
      {
        switch (scriptObject)
        {
          case FunctionScope _:
            scriptObject = scriptObject.GetParent();
            continue;
          case WithObject _:
            goto label_4;
          default:
            goto label_5;
        }
      }
label_4:
      if (!this.isFullyResolved)
        goto label_23;
label_5:
      this.context.HandleError(JSError.UndeclaredVariable, this.isFullyResolved && this.Engine.doFast);
    }
    else
    {
      this.ResolveRHValue();
      MemberInfo member = this.member;
      if ((object) (member as FieldInfo) != null)
      {
        FieldInfo field = (FieldInfo) member;
        if (field is JSLocalField && !((JSLocalField) field).isDefined)
        {
          ((JSLocalField) field).isUsedBeforeDefinition = true;
          this.context.HandleError(JSError.VariableMightBeUnitialized);
        }
        if (field.IsLiteral)
        {
          object obj = field is JSVariableField ? ((JSVariableField) field).value : TypeReferences.GetConstantValue(field);
          if (obj is AST)
          {
            AST ast = ((AST) obj).PartiallyEvaluate();
            if (ast is ConstantWrapper && this.isFullyResolved)
              return ast;
            obj = (object) null;
          }
          if (!(obj is FunctionObject) && this.isFullyResolved)
            return (AST) new ConstantWrapper(obj, this.context);
        }
        else if (field.IsInitOnly && field.IsStatic && field.DeclaringType == Typeob.GlobalObject && this.isFullyResolved)
          return (AST) new ConstantWrapper(field.GetValue((object) null), this.context);
      }
      else if ((object) (member as PropertyInfo) != null)
      {
        PropertyInfo propertyInfo = (PropertyInfo) member;
        if (!propertyInfo.CanWrite && !(propertyInfo is JSProperty) && propertyInfo.DeclaringType == Typeob.GlobalObject && this.isFullyResolved)
          return (AST) new ConstantWrapper(propertyInfo.GetValue((object) null, (object[]) null), this.context);
      }
      if ((object) (member as Type) != null && this.isFullyResolved)
        return (AST) new ConstantWrapper((object) member, this.context);
    }
label_23:
    return (AST) this;
  }

  internal override AST PartiallyEvaluateAsCallable()
  {
    this.BindName();
    return (AST) this;
  }

  internal override AST PartiallyEvaluateAsReference()
  {
    this.BindName();
    if (this.members == null || this.members.Length == 0)
    {
      if (!(this.Globals.ScopeStack.Peek() is WithObject) || this.isFullyResolved)
        this.context.HandleError(JSError.UndeclaredVariable, this.isFullyResolved && this.Engine.doFast);
    }
    else
      this.ResolveLHValue();
    return (AST) this;
  }

  internal override object ResolveCustomAttribute(ASTList args, IReflect[] argIRs, AST target)
  {
    if (this.name == "expando")
      this.members = (MemberInfo[]) Typeob.Expando.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
    else if (this.name == "override")
      this.members = (MemberInfo[]) Typeob.Override.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
    else if (this.name == "hide")
      this.members = (MemberInfo[]) Typeob.Hide.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
    else if (this.name == "...")
    {
      this.members = (MemberInfo[]) Typeob.ParamArrayAttribute.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
    }
    else
    {
      this.name += "Attribute";
      this.BindName();
      if (this.members == null || this.members.Length == 0)
      {
        this.name = this.name.Substring(0, this.name.Length - 9);
        this.BindName();
      }
    }
    return base.ResolveCustomAttribute(args, argIRs, target);
  }

  internal override void SetPartialValue(AST partial_value)
  {
    if (this.members == null || this.members.Length == 0)
      return;
    if (this.member is JSLocalField)
    {
      JSLocalField member = (JSLocalField) this.member;
      if (member.type == null)
      {
        IReflect ir = partial_value.InferType((JSField) member);
        if ((object) ir == (object) Typeob.String && partial_value is Plus)
        {
          member.SetInferredType((IReflect) Typeob.Object, partial_value);
          return;
        }
        member.SetInferredType(ir, partial_value);
        return;
      }
      member.isDefined = true;
    }
    Binding.AssignmentCompatible(this.InferType((JSField) null), partial_value, partial_value.InferType((JSField) null), this.isFullyResolved);
  }

  internal override void SetValue(object value)
  {
    if (!this.isFullyResolved)
      this.EvaluateAsLateBinding().SetValue(value);
    else
      base.SetValue(value);
  }

  internal void SetWithValue(WithObject scope, object value)
  {
    FieldInfo field = scope.GetField(this.name, this.lexLevel);
    if (!(field != (FieldInfo) null))
      return;
    field.SetValue((object) scope, value);
  }

  public override string ToString() => this.name;

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    if (this.isFullyResolved)
    {
      base.TranslateToIL(il, rtype);
    }
    else
    {
      Label label = il.DefineLabel();
      this.EmitILToLoadEngine(il);
      il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
      il.Emit(OpCodes.Castclass, Typeob.IActivationObject);
      il.Emit(OpCodes.Ldstr, this.name);
      ConstantWrapper.TranslateToILInt(il, this.lexLevel);
      il.Emit(OpCodes.Callvirt, CompilerGlobals.getMemberValueMethod);
      il.Emit(OpCodes.Dup);
      il.Emit(OpCodes.Call, CompilerGlobals.isMissingMethod);
      il.Emit(OpCodes.Brfalse, label);
      il.Emit(OpCodes.Pop);
      base.TranslateToIL(il, Typeob.Object);
      il.MarkLabel(label);
      Convert.Emit((AST) this, il, Typeob.Object, rtype);
    }
  }

  internal override void TranslateToILCall(
    ILGenerator il,
    Type rtype,
    ASTList argList,
    bool construct,
    bool brackets)
  {
    if (this.isFullyResolved)
    {
      base.TranslateToILCall(il, rtype, argList, construct, brackets);
    }
    else
    {
      Label label1 = il.DefineLabel();
      Label label2 = il.DefineLabel();
      this.EmitILToLoadEngine(il);
      il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
      il.Emit(OpCodes.Castclass, Typeob.IActivationObject);
      il.Emit(OpCodes.Ldstr, this.name);
      ConstantWrapper.TranslateToILInt(il, this.lexLevel);
      il.Emit(OpCodes.Callvirt, CompilerGlobals.getMemberValueMethod);
      il.Emit(OpCodes.Dup);
      il.Emit(OpCodes.Call, CompilerGlobals.isMissingMethod);
      il.Emit(OpCodes.Brfalse, label1);
      il.Emit(OpCodes.Pop);
      base.TranslateToILCall(il, Typeob.Object, argList, construct, brackets);
      il.Emit(OpCodes.Br, label2);
      il.MarkLabel(label1);
      this.TranslateToILDefaultThisObject(il);
      argList.TranslateToIL(il, Typeob.ArrayOfObject);
      if (construct)
        il.Emit(OpCodes.Ldc_I4_1);
      else
        il.Emit(OpCodes.Ldc_I4_0);
      if (brackets)
        il.Emit(OpCodes.Ldc_I4_1);
      else
        il.Emit(OpCodes.Ldc_I4_0);
      this.EmitILToLoadEngine(il);
      il.Emit(OpCodes.Call, CompilerGlobals.callValue2Method);
      il.MarkLabel(label2);
      Convert.Emit((AST) this, il, Typeob.Object, rtype);
    }
  }

  internal void TranslateToILDefaultThisObject(ILGenerator il)
  {
    this.TranslateToILDefaultThisObject(il, 0);
  }

  private void TranslateToILDefaultThisObject(ILGenerator il, int lexLevel)
  {
    this.EmitILToLoadEngine(il);
    il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
    while (lexLevel-- > 0)
      il.Emit(OpCodes.Call, CompilerGlobals.getParentMethod);
    il.Emit(OpCodes.Castclass, Typeob.IActivationObject);
    il.Emit(OpCodes.Callvirt, CompilerGlobals.getDefaultThisObjectMethod);
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
    if (this.defaultMember != (MemberInfo) null)
      return;
    if (this.member != (MemberInfo) null)
    {
      switch (this.member.MemberType)
      {
        case MemberTypes.Constructor:
          return;
        case MemberTypes.Field:
          if (!(this.member is JSExpandoField))
            return;
          this.member = (MemberInfo) null;
          break;
        case MemberTypes.Method:
          return;
        case MemberTypes.Property:
          return;
        case MemberTypes.TypeInfo:
          return;
        case MemberTypes.NestedType:
          return;
      }
    }
    this.refLoc = il.DeclareLocal(Typeob.LateBinding);
    il.Emit(OpCodes.Ldstr, this.name);
    if (this.isFullyResolved && this.member == (MemberInfo) null && this.IsBoundToMethodInfos())
    {
      MethodInfo member = this.members[0] as MethodInfo;
      if (member.IsStatic)
      {
        il.Emit(OpCodes.Ldtoken, member.DeclaringType);
        il.Emit(OpCodes.Call, CompilerGlobals.getTypeFromHandleMethod);
      }
      else
        this.TranslateToILObjectForMember(il, member.DeclaringType, false, (MemberInfo) member);
    }
    else
    {
      this.EmitILToLoadEngine(il);
      il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
    }
    il.Emit(OpCodes.Newobj, CompilerGlobals.lateBindingConstructor2);
    il.Emit(OpCodes.Stloc, this.refLoc);
  }

  private bool IsBoundToMethodInfos()
  {
    if (this.members == null || this.members.Length == 0)
      return false;
    for (int index = 0; index < this.members.Length; ++index)
    {
      if ((object) (this.members[index] as MethodInfo) == null)
        return false;
    }
    return true;
  }

  protected override void TranslateToILObject(ILGenerator il, Type obType, bool noValue)
  {
    this.TranslateToILObjectForMember(il, obType, noValue, this.member);
  }

  private void TranslateToILObjectForMember(
    ILGenerator il,
    Type obType,
    bool noValue,
    MemberInfo mem)
  {
    this.thereIsAnObjectOnTheStack = true;
    if (mem is IWrappedMember)
    {
      object wrappedObject = ((IWrappedMember) mem).GetWrappedObject();
      if (wrappedObject is LenientGlobalObject)
      {
        this.EmitILToLoadEngine(il);
        il.Emit(OpCodes.Call, CompilerGlobals.getLenientGlobalObjectMethod);
      }
      else if ((object) (wrappedObject as Type) != null || wrappedObject is ClassScope)
      {
        if (obType.IsAssignableFrom(Typeob.Type))
        {
          new ConstantWrapper(wrappedObject, (Context) null).TranslateToIL(il, Typeob.Type);
        }
        else
        {
          ScriptObject scriptObject1 = this.Globals.ScopeStack.Peek();
          while (true)
          {
            switch (scriptObject1)
            {
              case WithObject _:
              case BlockScope _:
                scriptObject1 = scriptObject1.GetParent();
                continue;
              case FunctionScope _:
                goto label_9;
              case ClassScope _:
                goto label_15;
              default:
                goto label_16;
            }
          }
label_9:
          if (((FunctionScope) scriptObject1).owner.isMethod)
          {
            il.Emit(OpCodes.Ldarg_0);
            goto label_16;
          }
          this.EmitILToLoadEngine(il);
          il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
          ScriptObject scriptObject2 = this.Globals.ScopeStack.Peek();
          while (true)
          {
            switch (scriptObject2)
            {
              case WithObject _:
              case BlockScope _:
                il.Emit(OpCodes.Call, CompilerGlobals.getParentMethod);
                scriptObject2 = scriptObject2.GetParent();
                continue;
              default:
                goto label_14;
            }
          }
label_14:
          il.Emit(OpCodes.Castclass, Typeob.StackFrame);
          il.Emit(OpCodes.Ldfld, CompilerGlobals.closureInstanceField);
          goto label_16;
label_15:
          il.Emit(OpCodes.Ldarg_0);
label_16:
          for (ScriptObject scriptObject3 = this.Globals.ScopeStack.Peek(); scriptObject3 != null; scriptObject3 = scriptObject3.GetParent())
          {
            if (scriptObject3 is ClassScope classScope)
            {
              if (classScope.IsSameOrDerivedFrom(obType))
                break;
              il.Emit(OpCodes.Ldfld, classScope.outerClassField);
            }
          }
        }
      }
      else
      {
        this.TranslateToILDefaultThisObject(il, this.lexLevel);
        Convert.Emit((AST) this, il, Typeob.Object, obType);
      }
    }
    else
    {
      ScriptObject scriptObject4 = this.Globals.ScopeStack.Peek();
      while (true)
      {
        switch (scriptObject4)
        {
          case WithObject _:
          case BlockScope _:
            scriptObject4 = scriptObject4.GetParent();
            continue;
          case FunctionScope _:
            goto label_27;
          default:
            goto label_38;
        }
      }
label_27:
      if (!((FunctionScope) scriptObject4).owner.isMethod)
      {
        this.EmitILToLoadEngine(il);
        il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
        ScriptObject scriptObject5 = this.Globals.ScopeStack.Peek();
        while (true)
        {
          switch (scriptObject5)
          {
            case WithObject _:
            case BlockScope _:
              il.Emit(OpCodes.Call, CompilerGlobals.getParentMethod);
              scriptObject5 = scriptObject5.GetParent();
              continue;
            default:
              goto label_31;
          }
        }
label_31:
        il.Emit(OpCodes.Castclass, Typeob.StackFrame);
        il.Emit(OpCodes.Ldfld, CompilerGlobals.closureInstanceField);
        for (; scriptObject5 != null; scriptObject5 = scriptObject5.GetParent())
        {
          if (scriptObject5 is ClassScope)
          {
            ClassScope classScope = (ClassScope) scriptObject5;
            if (!classScope.IsSameOrDerivedFrom(obType))
            {
              il.Emit(OpCodes.Castclass, (Type) classScope.GetTypeBuilder());
              il.Emit(OpCodes.Ldfld, classScope.outerClassField);
            }
            else
              break;
          }
        }
        il.Emit(OpCodes.Castclass, obType);
        return;
      }
label_38:
      il.Emit(OpCodes.Ldarg_0);
      for (; scriptObject4 != null; scriptObject4 = scriptObject4.GetParent())
      {
        if (scriptObject4 is ClassScope)
        {
          ClassScope classScope = (ClassScope) scriptObject4;
          if (classScope.IsSameOrDerivedFrom(obType))
            break;
          il.Emit(OpCodes.Ldfld, classScope.outerClassField);
        }
      }
    }
  }

  internal override void TranslateToILPreSet(ILGenerator il) => this.TranslateToILPreSet(il, false);

  internal void TranslateToILPreSet(ILGenerator il, bool doBoth)
  {
    if (this.isFullyResolved)
    {
      base.TranslateToILPreSet(il);
    }
    else
    {
      Label label1 = il.DefineLabel();
      LocalBuilder local = this.fieldLoc = il.DeclareLocal(Typeob.FieldInfo);
      this.EmitILToLoadEngine(il);
      il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
      il.Emit(OpCodes.Castclass, Typeob.IActivationObject);
      il.Emit(OpCodes.Ldstr, this.name);
      ConstantWrapper.TranslateToILInt(il, this.lexLevel);
      il.Emit(OpCodes.Callvirt, CompilerGlobals.getFieldMethod);
      il.Emit(OpCodes.Stloc, local);
      if (!doBoth)
      {
        il.Emit(OpCodes.Ldloc, local);
        il.Emit(OpCodes.Ldnull);
        il.Emit(OpCodes.Bne_Un, label1);
      }
      base.TranslateToILPreSet(il);
      if (this.thereIsAnObjectOnTheStack)
      {
        Label label2 = il.DefineLabel();
        il.Emit(OpCodes.Br, label2);
        il.MarkLabel(label1);
        il.Emit(OpCodes.Ldnull);
        il.MarkLabel(label2);
      }
      else
        il.MarkLabel(label1);
    }
  }

  internal override void TranslateToILPreSetPlusGet(ILGenerator il)
  {
    if (this.isFullyResolved)
    {
      base.TranslateToILPreSetPlusGet(il);
    }
    else
    {
      Label label1 = il.DefineLabel();
      Label label2 = il.DefineLabel();
      LocalBuilder local = this.fieldLoc = il.DeclareLocal(Typeob.FieldInfo);
      this.EmitILToLoadEngine(il);
      il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
      il.Emit(OpCodes.Castclass, Typeob.IActivationObject);
      il.Emit(OpCodes.Ldstr, this.name);
      ConstantWrapper.TranslateToILInt(il, this.lexLevel);
      il.Emit(OpCodes.Callvirt, CompilerGlobals.getFieldMethod);
      il.Emit(OpCodes.Stloc, local);
      il.Emit(OpCodes.Ldloc, local);
      il.Emit(OpCodes.Ldnull);
      il.Emit(OpCodes.Bne_Un, label2);
      base.TranslateToILPreSetPlusGet(il);
      il.Emit(OpCodes.Br, label1);
      il.MarkLabel(label2);
      if (this.thereIsAnObjectOnTheStack)
        il.Emit(OpCodes.Ldnull);
      il.Emit(OpCodes.Ldloc, this.fieldLoc);
      il.Emit(OpCodes.Ldnull);
      il.Emit(OpCodes.Callvirt, CompilerGlobals.getFieldValueMethod);
      il.MarkLabel(label1);
    }
  }

  internal override void TranslateToILSet(ILGenerator il, AST rhvalue)
  {
    this.TranslateToILSet(il, false, rhvalue);
  }

  internal void TranslateToILSet(ILGenerator il, bool doBoth, AST rhvalue)
  {
    if (this.isFullyResolved)
    {
      base.TranslateToILSet(il, rhvalue);
    }
    else
    {
      rhvalue?.TranslateToIL(il, Typeob.Object);
      if (this.fieldLoc == null)
      {
        il.Emit(OpCodes.Call, CompilerGlobals.setIndexedPropertyValueStaticMethod);
      }
      else
      {
        LocalBuilder local = il.DeclareLocal(Typeob.Object);
        if (doBoth)
        {
          il.Emit(OpCodes.Dup);
          il.Emit(OpCodes.Stloc, local);
          this.isFullyResolved = true;
          Convert.Emit((AST) this, il, Typeob.Object, Convert.ToType(this.InferType((JSField) null)));
          base.TranslateToILSet(il, (AST) null);
        }
        Label label1 = il.DefineLabel();
        il.Emit(OpCodes.Ldloc, this.fieldLoc);
        il.Emit(OpCodes.Ldnull);
        il.Emit(OpCodes.Beq, label1);
        Label label2 = il.DefineLabel();
        if (!doBoth)
        {
          il.Emit(OpCodes.Stloc, local);
          if (this.thereIsAnObjectOnTheStack)
            il.Emit(OpCodes.Pop);
        }
        il.Emit(OpCodes.Ldloc, this.fieldLoc);
        il.Emit(OpCodes.Ldnull);
        il.Emit(OpCodes.Ldloc, local);
        il.Emit(OpCodes.Callvirt, CompilerGlobals.setFieldValueMethod);
        il.Emit(OpCodes.Br, label2);
        il.MarkLabel(label1);
        if (!doBoth)
        {
          this.isFullyResolved = true;
          Convert.Emit((AST) this, il, Typeob.Object, Convert.ToType(this.InferType((JSField) null)));
          base.TranslateToILSet(il, (AST) null);
        }
        il.MarkLabel(label2);
      }
    }
  }

  protected override void TranslateToILWithDupOfThisOb(ILGenerator il)
  {
    this.TranslateToILDefaultThisObject(il);
    this.TranslateToIL(il, Typeob.Object);
  }

  internal void TranslateToLateBinding(ILGenerator il)
  {
    this.thereIsAnObjectOnTheStack = true;
    il.Emit(OpCodes.Ldloc, this.refLoc);
  }
}
