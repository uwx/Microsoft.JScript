// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSMemberField
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal sealed class JSMemberField : JSVariableField
{
  private object expandoValue;
  internal JSMemberField nextOverload;

  internal JSMemberField(
    ClassScope obj,
    string name,
    object value,
    FieldAttributes attributeFlags)
    : base(name, (ScriptObject) obj, attributeFlags)
  {
    this.value = value;
    this.nextOverload = (JSMemberField) null;
  }

  internal JSMemberField AddOverload(FunctionObject func, FieldAttributes attributeFlags)
  {
    JSMemberField jsMemberField1 = this;
    while ((FieldInfo) jsMemberField1.nextOverload != (FieldInfo) null)
      jsMemberField1 = jsMemberField1.nextOverload;
    JSMemberField jsMemberField2 = jsMemberField1.nextOverload = new JSMemberField((ClassScope) this.obj, this.Name, (object) func, attributeFlags);
    jsMemberField2.type = this.type;
    return jsMemberField2;
  }

  internal void AddOverloadedMembers(MemberInfoList mems, ClassScope scope, BindingFlags attrs)
  {
    for (JSMemberField jsMemberField = this; (FieldInfo) jsMemberField != (FieldInfo) null; jsMemberField = jsMemberField.nextOverload)
    {
      MethodInfo asMethod = jsMemberField.GetAsMethod((object) scope);
      if (asMethod.IsStatic)
      {
        if ((attrs & BindingFlags.Static) == BindingFlags.Default)
          continue;
      }
      else if ((attrs & BindingFlags.Instance) == BindingFlags.Default)
        continue;
      if (asMethod.IsPublic)
      {
        if ((attrs & BindingFlags.Public) == BindingFlags.Default)
          continue;
      }
      else if ((attrs & BindingFlags.NonPublic) == BindingFlags.Default)
        continue;
      mems.Add((MemberInfo) asMethod);
    }
    if ((attrs & BindingFlags.DeclaredOnly) != BindingFlags.Default && (attrs & BindingFlags.FlattenHierarchy) == BindingFlags.Default)
      return;
    foreach (MemberInfo elem in scope.GetSuperType().GetMember(this.Name, attrs & ~BindingFlags.DeclaredOnly))
    {
      if (elem.MemberType == MemberTypes.Method)
        mems.Add(elem);
    }
  }

  public override FieldAttributes Attributes
  {
    get
    {
      if ((this.attributeFlags & FieldAttributes.Literal) == FieldAttributes.PrivateScope)
        return this.attributeFlags;
      if (this.value is FunctionObject && !((FunctionObject) this.value).isStatic)
        return this.attributeFlags;
      if (!(this.value is JSProperty))
        return this.attributeFlags;
      JSProperty jsProperty = (JSProperty) this.value;
      if ((MethodInfo) jsProperty.getter != (MethodInfo) null && !jsProperty.getter.IsStatic)
        return this.attributeFlags;
      return (MethodInfo) jsProperty.setter != (MethodInfo) null && !jsProperty.setter.IsStatic ? this.attributeFlags : this.attributeFlags | FieldAttributes.Static;
    }
  }

  internal void CheckOverloadsForDuplicates()
  {
    for (JSMemberField jsMemberField = this; (FieldInfo) jsMemberField != (FieldInfo) null && jsMemberField.value is FunctionObject functionObject1; jsMemberField = jsMemberField.nextOverload)
    {
      for (JSMemberField nextOverload = jsMemberField.nextOverload; (FieldInfo) nextOverload != (FieldInfo) null; nextOverload = nextOverload.nextOverload)
      {
        FunctionObject functionObject = (FunctionObject) nextOverload.value;
        if (functionObject.implementedIface == functionObject1.implementedIface && Class.ParametersMatch((ParameterInfo[]) functionObject.parameter_declarations, (ParameterInfo[]) functionObject1.parameter_declarations))
        {
          functionObject1.funcContext.HandleError(JSError.DuplicateMethod);
          functionObject.funcContext.HandleError(JSError.DuplicateMethod);
          break;
        }
      }
    }
  }

  internal override object GetMetaData()
  {
    if (this.metaData == null)
      ((ClassScope) this.obj).GetTypeBuilderOrEnumBuilder();
    return this.metaData;
  }

  public override object GetValue(object obj)
  {
    switch (obj)
    {
      case StackFrame _:
        return this.GetValue(((StackFrame) obj).closureInstance, (ScriptObject) obj);
      case ScriptObject _:
        return this.GetValue(obj, (ScriptObject) obj);
      default:
        return this.GetValue(obj, (ScriptObject) null);
    }
  }

  private object GetValue(object obj, ScriptObject scope)
  {
    if (this.IsStatic || this.IsLiteral)
      return this.value;
    if (this.obj != obj)
    {
      if (obj is JSObject jsObject)
      {
        FieldInfo field = jsObject.GetField(this.Name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (field != (FieldInfo) null)
          return field.GetValue(obj);
        if (jsObject.outer_class_instance != null)
          return this.GetValue((object) jsObject.outer_class_instance, (ScriptObject) null);
      }
      throw new TargetException();
    }
    if (this.IsPublic || scope != null && this.IsAccessibleFrom(scope))
      return this.value;
    if (((JSObject) this.obj).noExpando)
      throw new JScriptException(JSError.NotAccessible, new Context(new DocumentContext("", (VsaEngine) null), this.Name));
    return this.expandoValue;
  }

  internal bool IsAccessibleFrom(ScriptObject scope)
  {
    while (true)
    {
      switch (scope)
      {
        case null:
        case ClassScope _:
          goto label_3;
        default:
          scope = scope.GetParent();
          continue;
      }
    }
label_3:
    ClassScope other = !(this.obj is ClassScope) ? (ClassScope) this.obj.GetParent() : (ClassScope) this.obj;
    if (this.IsPrivate)
    {
      if (scope == null)
        return false;
      return scope == other || ((ClassScope) scope).IsNestedIn(other, this.IsStatic);
    }
    if (this.IsFamily)
    {
      if (scope == null)
        return false;
      return ((ClassScope) scope).IsSameOrDerivedFrom(other) || ((ClassScope) scope).IsNestedIn(other, this.IsStatic);
    }
    if (this.IsFamilyOrAssembly && scope != null && (((ClassScope) scope).IsSameOrDerivedFrom(other) || ((ClassScope) scope).IsNestedIn(other, this.IsStatic)))
      return true;
    return scope == null ? other.GetPackage() == null : other.GetPackage() == ((ClassScope) scope).GetPackage();
  }

  internal ConstructorInfo[] GetAsConstructors(object proto)
  {
    JSMemberField jsMemberField1 = this;
    int length = 0;
    while ((FieldInfo) jsMemberField1 != (FieldInfo) null)
    {
      jsMemberField1 = jsMemberField1.nextOverload;
      ++length;
    }
    ConstructorInfo[] asConstructors = new ConstructorInfo[length];
    JSMemberField jsMemberField2 = this;
    int num = 0;
    for (; (FieldInfo) jsMemberField2 != (FieldInfo) null; jsMemberField2 = jsMemberField2.nextOverload)
    {
      FunctionObject cons = (FunctionObject) jsMemberField2.value;
      cons.isConstructor = true;
      cons.proto = proto;
      asConstructors[num++] = (ConstructorInfo) new JSConstructor(cons);
    }
    return asConstructors;
  }

  public override void SetValue(
    object obj,
    object value,
    BindingFlags invokeAttr,
    Binder binder,
    CultureInfo locale)
  {
    switch (obj)
    {
      case StackFrame _:
        this.SetValue(((StackFrame) obj).closureInstance, value, invokeAttr, binder, locale, (ScriptObject) obj);
        break;
      case ScriptObject _:
        this.SetValue(obj, value, invokeAttr, binder, locale, (ScriptObject) obj);
        break;
      default:
        this.SetValue(obj, value, invokeAttr, binder, locale, (ScriptObject) null);
        break;
    }
  }

  private void SetValue(
    object obj,
    object value,
    BindingFlags invokeAttr,
    Binder binder,
    CultureInfo locale,
    ScriptObject scope)
  {
    if (this.IsStatic || this.IsLiteral)
    {
      if ((this.IsLiteral || this.IsInitOnly) && !(this.value is Missing))
        throw new JScriptException(JSError.AssignmentToReadOnly);
    }
    else
    {
      if (this.obj != obj)
      {
        if (obj is JSObject)
        {
          FieldInfo field = ((ScriptObject) obj).GetField(this.Name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
          if (field != (FieldInfo) null)
          {
            field.SetValue(obj, value, invokeAttr, binder, locale);
            return;
          }
        }
        throw new TargetException();
      }
      if (!this.IsPublic && (scope == null || !this.IsAccessibleFrom(scope)))
      {
        if (((JSObject) this.obj).noExpando)
          throw new JScriptException(JSError.NotAccessible, new Context(new DocumentContext("", (VsaEngine) null), this.Name));
        this.expandoValue = value;
        return;
      }
    }
    if (this.type != null)
      this.value = Convert.Coerce(value, (object) this.type);
    else
      this.value = value;
  }
}
