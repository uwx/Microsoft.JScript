// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Member
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

internal sealed class Member : Binding
{
  private bool fast;
  private bool isImplicitWrapper;
  private LateBinding lateBinding;
  private Context memberNameContext;
  internal AST rootObject;
  private IReflect rootObjectInferredType;
  private LocalBuilder refLoc;
  private LocalBuilder temp;

  internal Member(Context context, AST rootObject, AST memberName)
    : base(context, memberName.context.GetCode())
  {
    this.fast = this.Engine.doFast;
    this.isImplicitWrapper = false;
    this.isNonVirtual = rootObject is ThisLiteral && ((ThisLiteral) rootObject).isSuper;
    this.lateBinding = (LateBinding) null;
    this.memberNameContext = memberName.context;
    this.rootObject = rootObject;
    this.rootObjectInferredType = (IReflect) null;
    this.refLoc = (LocalBuilder) null;
    this.temp = (LocalBuilder) null;
  }

  private void BindName(JSField inferenceTarget)
  {
    MemberInfo[] memberInfoArray = (MemberInfo[]) null;
    this.rootObject = this.rootObject.PartiallyEvaluate();
    IReflect obType = this.rootObjectInferredType = this.rootObject.InferType(inferenceTarget);
    if (this.rootObject is ConstantWrapper)
    {
      object object2 = Convert.ToObject2(this.rootObject.Evaluate(), this.Engine);
      if (object2 == null)
      {
        this.rootObject.context.HandleError(JSError.ObjectExpected);
        return;
      }
      ClassScope classScope1 = object2 as ClassScope;
      Type type1 = object2 as Type;
      if (classScope1 != null || type1 != (Type) null)
      {
        MemberInfo[] member;
        if (classScope1 != null)
          this.members = member = classScope1.GetMember(this.name, BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        else
          this.members = member = type1.GetMember(this.name, BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (member.Length != 0)
          return;
        this.members = memberInfoArray = Typeob.Type.GetMember(this.name, BindingFlags.Instance | BindingFlags.Public);
        return;
      }
      switch (object2)
      {
        case Namespace @namespace:
          string str = $"{@namespace.Name}.{this.name}";
          ClassScope classScope2 = this.Engine.GetClass(str);
          if (classScope2 != null)
          {
            FieldAttributes attributeFlags = FieldAttributes.Literal;
            if ((classScope2.owner.attributes & TypeAttributes.Public) == TypeAttributes.NotPublic)
              attributeFlags |= FieldAttributes.Private;
            this.members = new MemberInfo[1]
            {
              (MemberInfo) new JSGlobalField((ScriptObject) null, this.name, (object) classScope2, attributeFlags)
            };
            return;
          }
          Type type2 = this.Engine.GetType(str);
          if (type2 != (Type) null)
          {
            this.members = new MemberInfo[1]
            {
              (MemberInfo) type2
            };
            return;
          }
          break;
        case MathObject _:
label_18:
          obType = (IReflect) object2;
          break;
        case ScriptFunction _:
          if (object2 is FunctionObject)
            break;
          goto label_18;
      }
    }
    IReflect reflect = this.ProvideWrapperForPrototypeProperties(obType);
    if ((object) reflect == (object) Typeob.Object && !this.isNonVirtual)
    {
      this.members = new MemberInfo[0];
    }
    else
    {
      Type t = reflect as Type;
      if (t != (Type) null && t.IsInterface)
        this.members = JSBinder.GetInterfaceMembers(this.name, t);
      else if (reflect is ClassScope classScope4 && classScope4.owner.isInterface)
      {
        this.members = classScope4.owner.GetInterfaceMember(this.name);
      }
      else
      {
        while (reflect != null)
        {
          if (reflect is ClassScope classScope3)
          {
            if ((this.members = reflect.GetMember(this.name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)).Length != 0)
              break;
            reflect = classScope3.GetSuperType();
          }
          else
          {
            Type type = reflect as Type;
            if (type == (Type) null)
            {
              this.members = reflect.GetMember(this.name, BindingFlags.Instance | BindingFlags.Public);
              break;
            }
            MemberInfo[] mems = this.members = type.GetMember(this.name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (mems.Length != 0)
            {
              if (!(LateBinding.SelectMember(mems) == (MemberInfo) null) || (this.members = type.GetMember(this.name, MemberTypes.Method, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Length != 0)
                break;
              this.members = type.GetMember(this.name, MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
              break;
            }
            reflect = (IReflect) type.BaseType;
          }
        }
      }
    }
  }

  internal override object Evaluate()
  {
    object obj = base.Evaluate();
    if (obj is Missing)
      obj = (object) null;
    return obj;
  }

  internal override LateBinding EvaluateAsLateBinding()
  {
    LateBinding asLateBinding = this.lateBinding;
    if (asLateBinding == null)
    {
      if (this.member != (MemberInfo) null && !this.rootObjectInferredType.Equals((object) this.rootObject.InferType((JSField) null)))
        this.InvalidateBinding();
      this.lateBinding = asLateBinding = new LateBinding(this.name, (object) null, VsaEngine.executeForJSEE);
      asLateBinding.last_member = this.member;
    }
    object obj1 = this.rootObject.Evaluate();
    try
    {
      object obj2;
      asLateBinding.obj = obj2 = Convert.ToObject(obj1, this.Engine);
      if (this.defaultMember == (MemberInfo) null)
      {
        if (this.member != (MemberInfo) null)
          asLateBinding.last_object = obj2;
      }
    }
    catch (JScriptException ex)
    {
      if (ex.context == null)
        ex.context = this.rootObject.context;
      throw ex;
    }
    return asLateBinding;
  }

  internal object EvaluateAsType()
  {
    object memberValue = this.rootObject.EvaluateAsWrappedNamespace(false).GetMemberValue(this.name);
    switch (memberValue)
    {
      case null:
      case Missing _:
        object asType;
        if (!(this.rootObject is Member rootObject1))
        {
          if (!(this.rootObject is Lookup rootObject))
            return (object) null;
          if ((object) rootObject.PartiallyEvaluate() is ConstantWrapper constantWrapper)
          {
            asType = constantWrapper.value;
          }
          else
          {
            JSGlobalField member = rootObject.member as JSGlobalField;
            if (!((FieldInfo) member != (FieldInfo) null) || !member.IsLiteral)
              return (object) null;
            asType = member.value;
          }
        }
        else
          asType = rootObject1.EvaluateAsType();
        if (asType is ClassScope classScope)
        {
          MemberInfo[] member = classScope.GetMember(this.name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
          if (member.Length == 0)
            return (object) null;
          JSMemberField jsMemberField = member[0] as JSMemberField;
          return (FieldInfo) jsMemberField == (FieldInfo) null || !jsMemberField.IsLiteral || !(jsMemberField.value is ClassScope) || !jsMemberField.IsPublic && !jsMemberField.IsAccessibleFrom(this.Engine.ScriptObjectStackTop()) ? (object) null : jsMemberField.value;
        }
        Type type = asType as Type;
        return type != (Type) null ? (object) type.GetNestedType(this.name) : (object) null;
      default:
        return memberValue;
    }
  }

  internal override WrappedNamespace EvaluateAsWrappedNamespace(bool giveErrorIfNameInUse)
  {
    WrappedNamespace wrappedNamespace = this.rootObject.EvaluateAsWrappedNamespace(giveErrorIfNameInUse);
    string name = this.name;
    wrappedNamespace.AddFieldOrUseExistingField(name, (object) Namespace.GetNamespace($"{wrappedNamespace.ToString()}.{name}", this.Engine), FieldAttributes.Literal);
    return new WrappedNamespace($"{wrappedNamespace.ToString()}.{name}", this.Engine);
  }

  protected override object GetObject()
  {
    return Convert.ToObject(this.rootObject.Evaluate(), this.Engine);
  }

  protected override void HandleNoSuchMemberError()
  {
    IReflect ir1 = this.rootObject.InferType((JSField) null);
    object ir2 = (object) null;
    if (this.rootObject is ConstantWrapper)
      ir2 = this.rootObject.Evaluate();
    if ((object) ir1 == (object) Typeob.Object && !this.isNonVirtual || ir1 is JSObject && !((JSObject) ir1).noExpando || ir1 is GlobalScope && !((ActivationObject) ir1).isKnownAtCompileTime)
      return;
    if ((object) (ir1 as Type) != null)
    {
      Type type = (Type) ir1;
      if (Typeob.ScriptFunction.IsAssignableFrom(type) || type == Typeob.MathObject)
      {
        this.memberNameContext.HandleError(JSError.OLENoPropOrMethod);
        return;
      }
      if (Typeob.IExpando.IsAssignableFrom(type) || !this.fast && (type == Typeob.Boolean || type == Typeob.String || Convert.IsPrimitiveNumericType((IReflect) type)))
        return;
      if (ir2 is ClassScope && ((ScriptObject) ir2).GetMember(this.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length != 0)
      {
        this.memberNameContext.HandleError(JSError.NonStaticWithTypeName);
        return;
      }
    }
    if (ir2 is FunctionObject)
    {
      this.rootObject = (AST) new ConstantWrapper((object) ((ScriptFunction) ir2).name, this.rootObject.context);
      this.memberNameContext.HandleError(JSError.OLENoPropOrMethod);
    }
    else if (ir1 is ClassScope && ((ScriptObject) ir1).GetMember(this.name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length != 0)
      this.memberNameContext.HandleError(JSError.StaticRequiresTypeName);
    else if ((object) (ir2 as Type) != null)
      this.memberNameContext.HandleError(JSError.NoSuchStaticMember, Convert.ToTypeName((IReflect) (Type) ir2));
    else if (ir2 is ClassScope)
      this.memberNameContext.HandleError(JSError.NoSuchStaticMember, Convert.ToTypeName((IReflect) ir2));
    else if (ir2 is Namespace)
    {
      this.memberNameContext.HandleError(JSError.NoSuchType, $"{((Namespace) ir2).Name}.{this.name}");
    }
    else
    {
      if (ir1 == FunctionPrototype.ob && this.rootObject is Binding && ((Binding) this.rootObject).member is JSVariableField && ((JSVariableField) ((Binding) this.rootObject).member).value is FunctionObject)
        return;
      this.memberNameContext.HandleError(JSError.NoSuchMember, Convert.ToTypeName(ir1));
    }
  }

  internal override IReflect InferType(JSField inference_target)
  {
    if (this.members == null)
      this.BindName(inference_target);
    else if (!this.rootObjectInferredType.Equals((object) this.rootObject.InferType(inference_target)))
      this.InvalidateBinding();
    return base.InferType((JSField) null);
  }

  internal override IReflect InferTypeOfCall(JSField inference_target, bool isConstructor)
  {
    if (!this.rootObjectInferredType.Equals((object) this.rootObject.InferType(inference_target)))
      this.InvalidateBinding();
    return base.InferTypeOfCall((JSField) null, isConstructor);
  }

  internal override AST PartiallyEvaluate()
  {
    this.BindName((JSField) null);
    if (this.members == null || this.members.Length == 0)
    {
      if (this.rootObject is ConstantWrapper)
      {
        object obj = this.rootObject.Evaluate();
        if (obj is Namespace)
          return (AST) new ConstantWrapper((object) Namespace.GetNamespace($"{((Namespace) obj).Name}.{this.name}", this.Engine), this.context);
      }
      this.HandleNoSuchMemberError();
      return (AST) this;
    }
    this.ResolveRHValue();
    if ((object) (this.member as FieldInfo) != null && ((FieldInfo) this.member).IsLiteral)
    {
      object obj = this.member is JSVariableField ? ((JSVariableField) this.member).value : TypeReferences.GetConstantValue((FieldInfo) this.member);
      if (obj is AST)
      {
        AST ast = ((AST) obj).PartiallyEvaluate();
        if (ast is ConstantWrapper)
          return ast;
        obj = (object) null;
      }
      if (!(obj is FunctionObject) && (!(obj is ClassScope) || ((ClassScope) obj).owner.IsStatic))
        return (AST) new ConstantWrapper(obj, this.context);
    }
    else if ((object) (this.member as Type) != null)
      return (AST) new ConstantWrapper((object) this.member, this.context);
    return (AST) this;
  }

  internal override AST PartiallyEvaluateAsCallable()
  {
    this.BindName((JSField) null);
    return (AST) this;
  }

  internal override AST PartiallyEvaluateAsReference()
  {
    this.BindName((JSField) null);
    if (this.members == null || this.members.Length == 0)
    {
      if (this.isImplicitWrapper && !Convert.IsArray(this.rootObjectInferredType))
        this.context.HandleError(JSError.UselessAssignment);
      else
        this.HandleNoSuchMemberError();
      return (AST) this;
    }
    this.ResolveLHValue();
    if (this.isImplicitWrapper && (this.member == (MemberInfo) null || !(this.member is JSField) && Typeob.JSObject.IsAssignableFrom(this.member.DeclaringType)))
      this.context.HandleError(JSError.UselessAssignment);
    return (AST) this;
  }

  private IReflect ProvideWrapperForPrototypeProperties(IReflect obType)
  {
    if ((object) obType == (object) Typeob.String)
    {
      obType = (IReflect) this.Globals.globalObject.originalString.Construct();
      ((JSObject) obType).noExpando = this.fast;
      this.isImplicitWrapper = true;
    }
    else if ((object) (obType as Type) != null && Typeob.Array.IsAssignableFrom((Type) obType) || obType is TypedArray)
    {
      obType = (IReflect) this.Globals.globalObject.originalArray.ConstructWrapper();
      ((JSObject) obType).noExpando = this.fast;
      this.isImplicitWrapper = true;
    }
    else if ((object) obType == (object) Typeob.Boolean)
    {
      obType = (IReflect) this.Globals.globalObject.originalBoolean.Construct();
      ((JSObject) obType).noExpando = this.fast;
      this.isImplicitWrapper = true;
    }
    else if (Convert.IsPrimitiveNumericType(obType))
    {
      Type type = (Type) obType;
      obType = (IReflect) this.Globals.globalObject.originalNumber.Construct();
      ((JSObject) obType).noExpando = this.fast;
      ((NumberObject) obType).baseType = type;
      this.isImplicitWrapper = true;
    }
    else if ((object) (obType as Type) != null)
      obType = Convert.ToIReflect((Type) obType, this.Engine);
    return obType;
  }

  internal override object ResolveCustomAttribute(ASTList args, IReflect[] argIRs, AST target)
  {
    this.name += "Attribute";
    this.BindName((JSField) null);
    if (this.members == null || this.members.Length == 0)
    {
      this.name = this.name.Substring(0, this.name.Length - 9);
      this.BindName((JSField) null);
    }
    return base.ResolveCustomAttribute(args, argIRs, target);
  }

  public override string ToString() => $"{this.rootObject.ToString()}.{this.name}";

  internal override void TranslateToILInitializer(ILGenerator il)
  {
    this.rootObject.TranslateToILInitializer(il);
    if (!this.rootObjectInferredType.Equals((object) this.rootObject.InferType((JSField) null)))
      this.InvalidateBinding();
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
    il.Emit(OpCodes.Newobj, CompilerGlobals.lateBindingConstructor);
    il.Emit(OpCodes.Stloc, this.refLoc);
  }

  protected override void TranslateToILObject(ILGenerator il, Type obType, bool noValue)
  {
    if (noValue && obType.IsValueType && obType != Typeob.Enum)
    {
      if (this.temp == null)
      {
        this.rootObject.TranslateToILReference(il, obType);
      }
      else
      {
        Type type = Convert.ToType(this.rootObject.InferType((JSField) null));
        if (type == obType)
        {
          il.Emit(OpCodes.Ldloca, this.temp);
        }
        else
        {
          il.Emit(OpCodes.Ldloc, this.temp);
          Convert.Emit((AST) this, il, type, obType);
          Convert.EmitLdloca(il, obType);
        }
      }
    }
    else if (this.temp == null || this.rootObject is ThisLiteral)
    {
      this.rootObject.TranslateToIL(il, obType);
    }
    else
    {
      il.Emit(OpCodes.Ldloc, this.temp);
      Type type = Convert.ToType(this.rootObject.InferType((JSField) null));
      Convert.Emit((AST) this, il, type, obType);
    }
  }

  protected override void TranslateToILWithDupOfThisOb(ILGenerator il)
  {
    IReflect ir = this.rootObject.InferType((JSField) null);
    Type type = Convert.ToType(ir);
    this.rootObject.TranslateToIL(il, type);
    if ((object) ir == (object) Typeob.Object || (object) ir == (object) Typeob.String || ir is TypedArray || (object) (ir as Type) != null && (Type) ir == type && Typeob.Array.IsAssignableFrom(type))
    {
      type = Typeob.Object;
      this.EmitILToLoadEngine(il);
      il.Emit(OpCodes.Call, CompilerGlobals.toObjectMethod);
    }
    il.Emit(OpCodes.Dup);
    this.temp = il.DeclareLocal(type);
    il.Emit(OpCodes.Stloc, this.temp);
    Convert.Emit((AST) this, il, type, Typeob.Object);
    this.TranslateToIL(il, Typeob.Object);
  }

  internal void TranslateToLateBinding(ILGenerator il, bool speculativeEarlyBindingsExist)
  {
    if (speculativeEarlyBindingsExist)
    {
      LocalBuilder local = il.DeclareLocal(Typeob.Object);
      il.Emit(OpCodes.Stloc, local);
      il.Emit(OpCodes.Ldloc, this.refLoc);
      il.Emit(OpCodes.Dup);
      il.Emit(OpCodes.Ldloc, local);
    }
    else
    {
      il.Emit(OpCodes.Ldloc, this.refLoc);
      il.Emit(OpCodes.Dup);
      this.TranslateToILObject(il, Typeob.Object, false);
    }
    IReflect c = this.rootObject.InferType((JSField) null);
    if ((object) c == (object) Typeob.Object || (object) c == (object) Typeob.String || c is TypedArray || (object) (c as Type) != null && ((Type) c).IsPrimitive || (object) (c as Type) != null && Typeob.Array.IsAssignableFrom((Type) c))
    {
      this.EmitILToLoadEngine(il);
      il.Emit(OpCodes.Call, CompilerGlobals.toObjectMethod);
    }
    il.Emit(OpCodes.Stfld, CompilerGlobals.objectField);
  }
}
