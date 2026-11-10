// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ClassScope
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

internal sealed class ClassScope : ActivationObject, IComparable
{
  internal string name;
  internal Type classwriter;
  internal Class owner;
  internal ConstructorInfo[] constructors;
  internal bool noExpando;
  internal PackageScope package;
  internal JSProperty itemProp;
  internal FieldInfo outerClassField;
  internal bool inStaticInitializerCode;
  internal bool staticInitializerUsesEval;
  internal bool instanceInitializerUsesEval;

  internal ClassScope(AST name, GlobalScope scope)
    : base((ScriptObject) scope)
  {
    this.name = name.ToString();
    this.engine = scope.engine;
    this.fast = scope.fast;
    this.noExpando = true;
    this.isKnownAtCompileTime = true;
    this.owner = (Class) null;
    this.constructors = (ConstructorInfo[]) new JSConstructor[0];
    ScriptObject scriptObject = this.engine.ScriptObjectStackTop();
    while (true)
    {
      switch (scriptObject)
      {
        case WithObject _:
          scriptObject = scriptObject.GetParent();
          continue;
        case ClassScope _:
          goto label_3;
        case PackageScope _:
          goto label_4;
        default:
          goto label_5;
      }
    }
label_3:
    this.package = ((ClassScope) scriptObject).GetPackage();
    goto label_6;
label_4:
    this.package = (PackageScope) scriptObject;
    goto label_6;
label_5:
    this.package = (PackageScope) null;
label_6:
    this.itemProp = (JSProperty) null;
    this.outerClassField = (FieldInfo) null;
    this.inStaticInitializerCode = false;
    this.staticInitializerUsesEval = false;
    this.instanceInitializerUsesEval = false;
  }

  internal void AddClassesFromInheritanceChain(string name, ArrayList result)
  {
    IReflect reflect = (IReflect) this;
    bool flag = true;
    for (; reflect is ClassScope; reflect = !(reflect is ClassScope) ? (IReflect) ((Type) reflect).BaseType : ((ClassScope) reflect).GetSuperType())
    {
      if (reflect.GetMember(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length != 0)
      {
        result.Add((object) reflect);
        flag = false;
      }
    }
    if (!flag || (object) (reflect as Type) == null || reflect.GetMember(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length == 0)
      return;
    result.Add((object) reflect);
  }

  internal static ClassScope ScopeOfClassMemberInitializer(ScriptObject scope)
  {
    while (true)
    {
      switch (scope)
      {
        case null:
          return (ClassScope) null;
        case FunctionScope _:
          return (ClassScope) null;
        case ClassScope classScope:
          return classScope;
        default:
          scope = scope.GetParent();
          continue;
      }
    }
  }

  public int CompareTo(object ob)
  {
    if (ob == this)
      return 0;
    if (ob == null)
      return 1;
    if (!(ob is ClassScope other))
      return StringComparer.Ordinal.Compare(typeof (ClassScope).AssemblyQualifiedName, ob.GetType().AssemblyQualifiedName);
    bool flag1 = this.IsSameOrDerivedFrom(other);
    bool flag2 = other.IsSameOrDerivedFrom(this);
    if (flag1 == flag2)
      return StringComparer.Ordinal.Compare(this.name, other.name);
    return !flag1 ? 1 : -1;
  }

  protected override JSVariableField CreateField(
    string name,
    FieldAttributes attributeFlags,
    object value)
  {
    return (JSVariableField) new JSMemberField(this, name, value, attributeFlags);
  }

  internal object FakeCallToTypeMethod(MethodInfo method, object[] arguments, Exception e)
  {
    ParameterInfo[] parameters = method.GetParameters();
    int length = parameters.Length;
    Type[] types = new Type[length];
    for (int index = 0; index < length; ++index)
      types[index] = parameters[index].ParameterType;
    MethodInfo method1 = typeof (ClassScope).GetMethod(method.Name, types);
    return method1 != (MethodInfo) null ? method1.Invoke((object) this, arguments) : throw e;
  }

  public object[] GetCustomAttributes(bool inherit)
  {
    CustomAttributeList customAttributes = this.owner.customAttributes;
    return customAttributes == null ? new object[0] : (object[]) customAttributes.Evaluate();
  }

  public ConstructorInfo[] GetConstructors() => this.constructors;

  public FieldInfo GetField(string name)
  {
    return this.GetField(name, BindingFlags.Instance | BindingFlags.Public);
  }

  public MethodInfo GetMethod(string name)
  {
    return this.GetMethod(name, BindingFlags.Instance | BindingFlags.Public);
  }

  public PropertyInfo GetProperty(string name)
  {
    return this.GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
  }

  internal override object GetDefaultValue(PreferredType preferred_type)
  {
    return (object) this.GetFullName();
  }

  internal override void GetPropertyEnumerator(ArrayList enums, ArrayList objects)
  {
  }

  internal string GetFullName()
  {
    PackageScope package = this.GetPackage();
    if (package != null)
      return $"{package.GetName()}.{this.name}";
    return this.owner.enclosingScope is ClassScope ? $"{((ClassScope) this.owner.enclosingScope).GetFullName()}.{this.name}" : this.name;
  }

  public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
  {
    MemberInfoList mems = new MemberInfoList();
    FieldInfo elem1 = (FieldInfo) this.name_table[(object) name];
    if (elem1 != (FieldInfo) null)
    {
      if (elem1.IsPublic)
      {
        if ((bindingAttr & BindingFlags.Public) == BindingFlags.Default)
          goto label_22;
      }
      else if ((bindingAttr & BindingFlags.NonPublic) == BindingFlags.Default)
        goto label_22;
      if (elem1.IsLiteral)
      {
        object obj = ((JSVariableField) elem1).value;
        switch (obj)
        {
          case FunctionObject _:
            FunctionObject functionObject = (FunctionObject) obj;
            if (functionObject.isConstructor)
              return new MemberInfo[0];
            if (functionObject.isExpandoMethod)
            {
              if ((bindingAttr & BindingFlags.Instance) != BindingFlags.Default)
              {
                mems.Add((MemberInfo) elem1);
                goto label_22;
              }
              goto label_22;
            }
            ((JSMemberField) elem1).AddOverloadedMembers(mems, this, bindingAttr | BindingFlags.DeclaredOnly);
            goto label_22;
          case JSProperty _:
            JSProperty elem2 = (JSProperty) obj;
            if (((MethodInfo) elem2.getter != (MethodInfo) null ? (MethodBase) elem2.getter : (MethodBase) elem2.setter).IsStatic)
            {
              if ((bindingAttr & BindingFlags.Static) == BindingFlags.Default)
                goto label_22;
            }
            else if ((bindingAttr & BindingFlags.Instance) == BindingFlags.Default)
              goto label_22;
            mems.Add((MemberInfo) elem2);
            goto label_22;
          case ClassScope _:
            if ((bindingAttr & BindingFlags.Instance) != BindingFlags.Default && !((ClassScope) obj).owner.isStatic)
            {
              mems.Add((MemberInfo) elem1);
              goto label_22;
            }
            break;
        }
      }
      if (elem1.IsStatic)
      {
        if ((bindingAttr & BindingFlags.Static) == BindingFlags.Default)
          goto label_22;
      }
      else if ((bindingAttr & BindingFlags.Instance) == BindingFlags.Default)
        goto label_22;
      mems.Add((MemberInfo) elem1);
    }
label_22:
    if (this.owner != null && this.owner.isInterface && (bindingAttr & BindingFlags.DeclaredOnly) == BindingFlags.Default)
      return this.owner.GetInterfaceMember(name);
    if (this.parent != null && (bindingAttr & BindingFlags.DeclaredOnly) == BindingFlags.Default)
    {
      MemberInfo[] member1 = this.parent.GetMember(name, bindingAttr);
      if (member1 != null)
      {
        foreach (MemberInfo member2 in member1)
        {
          if (member2.MemberType == MemberTypes.Field)
          {
            FieldInfo fieldInfo = (FieldInfo) member2;
            if (!fieldInfo.IsStatic && !fieldInfo.IsLiteral && !(fieldInfo is JSWrappedField))
              fieldInfo = (FieldInfo) new JSWrappedField(fieldInfo, (object) this.parent);
            mems.Add((MemberInfo) fieldInfo);
          }
          else
            mems.Add(ScriptObject.WrapMember(member2, (object) this.parent));
        }
      }
    }
    return mems.ToArray();
  }

  internal bool HasInstance(object ob)
  {
    if (!(ob is JSObject))
      return false;
    for (ScriptObject parent = ((ScriptObject) ob).GetParent(); parent != null; parent = parent.GetParent())
    {
      if (parent == this || parent is WithObject && ((WithObject) parent).contained_object == this)
        return true;
    }
    return false;
  }

  internal JSMemberField[] GetMemberFields()
  {
    int count = this.field_table.Count;
    JSMemberField[] memberFields = new JSMemberField[count];
    for (int index = 0; index < count; ++index)
      memberFields[index] = (JSMemberField) this.field_table[index];
    return memberFields;
  }

  public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
  {
    MemberInfoList mems = new MemberInfoList();
    foreach (FieldInfo elem1 in this.field_table)
    {
      if (elem1.IsLiteral && elem1 is JSMemberField)
      {
        object elem2;
        if ((elem2 = ((JSVariableField) elem1).value) is FunctionObject)
        {
          if (!((FunctionObject) elem2).isConstructor)
            ((JSMemberField) elem1).AddOverloadedMembers(mems, this, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        }
        else if (elem2 is JSProperty)
          mems.Add((MemberInfo) elem2);
        else
          mems.Add((MemberInfo) elem1);
      }
      else
        mems.Add((MemberInfo) elem1);
    }
    if (this.parent != null)
      mems.AddRange(this.parent.GetMembers(bindingAttr));
    return mems.ToArray();
  }

  internal override string GetName() => this.name;

  internal Type GetBakedSuperType()
  {
    this.owner.PartiallyEvaluate();
    if (this.owner is EnumDeclaration)
      return ((EnumDeclaration) this.owner).baseType.ToType();
    object containedObject = ((WithObject) this.parent).contained_object;
    if (containedObject is ClassScope)
      return ((ClassScope) containedObject).GetBakedSuperType();
    return (object) (containedObject as Type) != null ? (Type) containedObject : Globals.TypeRefs.ToReferenceContext(containedObject.GetType());
  }

  internal PackageScope GetPackage() => this.package;

  internal IReflect GetSuperType()
  {
    this.owner.PartiallyEvaluate();
    return (IReflect) ((WithObject) this.parent).contained_object;
  }

  internal Type GetTypeBuilderOrEnumBuilder()
  {
    if (this.classwriter == (Type) null)
      this.classwriter = this.owner.GetTypeBuilderOrEnumBuilder();
    return this.classwriter;
  }

  internal TypeBuilder GetTypeBuilder() => (TypeBuilder) this.GetTypeBuilderOrEnumBuilder();

  internal IReflect GetUnderlyingTypeIfEnum()
  {
    return this.owner is EnumDeclaration ? ((EnumDeclaration) this.owner.PartiallyEvaluate()).baseType.ToIReflect() : (IReflect) this;
  }

  internal bool ImplementsInterface(IReflect iface)
  {
    this.owner.PartiallyEvaluate();
    object containedObject = ((WithObject) this.parent).contained_object;
    return containedObject is ClassScope ? ((ClassScope) containedObject).ImplementsInterface(iface) || this.owner.ImplementsInterface(iface) : (object) (containedObject as Type) != null && (object) (iface as Type) != null && ((Type) iface).IsAssignableFrom((Type) containedObject) || this.owner.ImplementsInterface(iface);
  }

  internal bool IsCLSCompliant()
  {
    this.owner.PartiallyEvaluate();
    switch (this.owner.attributes & TypeAttributes.VisibilityMask)
    {
      case TypeAttributes.Public:
      case TypeAttributes.NestedPublic:
        if (this.owner.clsCompliance == CLSComplianceSpec.NotAttributed)
          return this.owner.Engine.isCLSCompliant;
        return this.owner.clsCompliance == CLSComplianceSpec.CLSCompliant;
      default:
        return false;
    }
  }

  internal bool IsNestedIn(ClassScope other, bool isStatic)
  {
    if (this.parent == null)
      return false;
    this.owner.PartiallyEvaluate();
    return this.owner.enclosingScope == other ? isStatic || !this.owner.isStatic : this.owner.enclosingScope is ClassScope && ((ClassScope) this.owner.enclosingScope).IsNestedIn(other, isStatic);
  }

  internal bool IsSameOrDerivedFrom(ClassScope other)
  {
    if (this == other)
      return true;
    if (other.owner.isInterface)
      return this.ImplementsInterface((IReflect) other);
    if (this.parent == null)
      return false;
    this.owner.PartiallyEvaluate();
    object containedObject = ((WithObject) this.parent).contained_object;
    return containedObject is ClassScope && ((ClassScope) containedObject).IsSameOrDerivedFrom(other);
  }

  internal bool IsSameOrDerivedFrom(Type other)
  {
    if ((Type) this.owner.GetTypeBuilder() == other)
      return true;
    if (this.parent == null)
      return false;
    this.owner.PartiallyEvaluate();
    object containedObject = ((WithObject) this.parent).contained_object;
    return containedObject is ClassScope ? ((ClassScope) containedObject).IsSameOrDerivedFrom(other) : other.IsAssignableFrom((Type) containedObject);
  }

  internal bool IsPromotableTo(Type other)
  {
    Type bakedSuperType = this.GetBakedSuperType();
    return other.IsAssignableFrom(bakedSuperType) || other.IsInterface && this.ImplementsInterface((IReflect) other) || this.owner is EnumDeclaration owner && Convert.IsPromotableTo((IReflect) owner.baseType.ToType(), (IReflect) other);
  }

  internal bool ParentIsInSamePackage()
  {
    object containedObject = ((WithObject) this.parent).contained_object;
    return containedObject is ClassScope && ((ClassScope) containedObject).package == this.package;
  }
}
