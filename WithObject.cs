// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.WithObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Diagnostics;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

internal sealed class WithObject : ScriptObject, IActivationObject
{
  internal object contained_object;
  internal bool isKnownAtCompileTime;
  private bool isSuperType;

  internal WithObject(ScriptObject parent, object contained_object)
    : this(parent, contained_object, false)
  {
  }

  internal WithObject(ScriptObject parent, object contained_object, bool isSuperType)
    : base(parent)
  {
    this.contained_object = contained_object;
    this.isKnownAtCompileTime = (object) (contained_object as Type) != null || contained_object is ClassScope && ((ClassScope) contained_object).noExpando || contained_object is JSObject && ((JSObject) contained_object).noExpando;
    this.isSuperType = isSuperType;
  }

  public object GetDefaultThisObject() => this.contained_object;

  public FieldInfo GetField(string name, int lexLevel)
  {
    if (lexLevel <= 0)
      return (FieldInfo) null;
    IReflect reflect = !(this.contained_object is IReflect) ? (IReflect) Globals.TypeRefs.ToReferenceContext(this.contained_object.GetType()) : (IReflect) this.contained_object;
    FieldInfo field1 = reflect.GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
    if (field1 != (FieldInfo) null)
      return (FieldInfo) new JSWrappedField(field1, this.contained_object);
    PropertyInfo property = reflect.GetProperty(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
    if (property != (PropertyInfo) null)
      return (FieldInfo) new JSPropertyField(property, this.contained_object);
    if (this.parent != null && lexLevel > 1)
    {
      FieldInfo field2 = ((IActivationObject) this.parent).GetField(name, lexLevel - 1);
      if (field2 != (FieldInfo) null)
        return (FieldInfo) new JSWrappedField(field2, (object) this.parent);
    }
    return (FieldInfo) null;
  }

  public GlobalScope GetGlobalScope() => ((IActivationObject) this.GetParent()).GetGlobalScope();

  FieldInfo IActivationObject.GetLocalField(string name) => (FieldInfo) null;

  public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
  {
    return this.GetMember(name, bindingAttr, true);
  }

  internal MemberInfo[] GetMember(string name, BindingFlags bindingAttr, bool forceInstanceLookup)
  {
    Type type = (Type) null;
    BindingFlags bindingFlags = bindingAttr;
    if (forceInstanceLookup && this.isSuperType && (bindingAttr & BindingFlags.FlattenHierarchy) == BindingFlags.Default)
      bindingFlags |= BindingFlags.Instance;
    object containedObject = this.contained_object;
    MemberInfo[] member;
    while (true)
    {
      IReflect reflect;
      if (containedObject is IReflect)
      {
        reflect = (IReflect) containedObject;
        if ((object) (containedObject as Type) != null && !this.isSuperType)
          bindingFlags &= ~BindingFlags.Instance;
      }
      else
        reflect = (IReflect) (type = Globals.TypeRefs.ToReferenceContext(containedObject.GetType()));
      member = reflect.GetMember(name, bindingFlags & ~BindingFlags.DeclaredOnly);
      if (member.Length == 0)
      {
        if ((object) (containedObject as Type) != null && !this.isSuperType)
          member = Typeob.Type.GetMember(name, BindingFlags.Instance | BindingFlags.Public);
        if (member.Length == 0)
        {
          if (type != (Type) null)
          {
            if (type.IsNestedPublic)
            {
              try
              {
                new ReflectionPermission(ReflectionPermissionFlag.TypeInformation | ReflectionPermissionFlag.MemberAccess).Assert();
                FieldInfo field = type.GetField("outer class instance", BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != (FieldInfo) null)
                  containedObject = field.GetValue(containedObject);
                else
                  goto label_18;
              }
              finally
              {
                CodeAccessPermission.RevertAssert();
              }
            }
            else
              goto label_18;
          }
          else
            goto label_18;
        }
        else
          goto label_12;
      }
      else
        break;
    }
    return ScriptObject.WrapMembers(member, containedObject);
label_12:
    return ScriptObject.WrapMembers(member, containedObject);
label_18:
    return member.Length != 0 ? ScriptObject.WrapMembers(member, containedObject) : new MemberInfo[0];
  }

  public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
  {
    return ((IReflect) this.contained_object).GetMembers(bindingAttr);
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  internal override object GetMemberValue(string name)
  {
    object memberValue2 = LateBinding.GetMemberValue2(this.contained_object, name);
    if (!(memberValue2 is Missing))
      return memberValue2;
    return this.parent != null ? this.parent.GetMemberValue(name) : (object) Missing.Value;
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  public object GetMemberValue(string name, int lexlevel)
  {
    if (lexlevel <= 0)
      return (object) Missing.Value;
    object memberValue2 = LateBinding.GetMemberValue2(this.contained_object, name);
    return memberValue2 != Missing.Value ? memberValue2 : ((IActivationObject) this.parent).GetMemberValue(name, lexlevel - 1);
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  internal override void SetMemberValue(string name, object value)
  {
    if (LateBinding.GetMemberValue2(this.contained_object, name) is Missing)
      this.parent.SetMemberValue(name, value);
    else
      LateBinding.SetMemberValue(this.contained_object, name, value);
  }
}
