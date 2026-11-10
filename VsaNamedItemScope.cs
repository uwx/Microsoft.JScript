// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.VsaNamedItemScope
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System.Diagnostics;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal sealed class VsaNamedItemScope : ScriptObject, IActivationObject
{
  internal object namedItem;
  private SimpleHashtable namedItemWrappedMemberCache;
  private IReflect reflectObj;
  private bool recursive;

  internal VsaNamedItemScope(object hostObject, ScriptObject parent, VsaEngine engine)
    : base(parent)
  {
    this.namedItem = hostObject;
    if ((this.reflectObj = hostObject as IReflect) == null)
      this.reflectObj = (IReflect) Globals.TypeRefs.ToReferenceContext(hostObject.GetType());
    this.recursive = false;
    this.engine = engine;
  }

  private static MemberInfo[] GetAndWrapMember(
    IReflect reflect,
    object namedItem,
    string name,
    BindingFlags bindingAttr)
  {
    PropertyInfo property = reflect.GetProperty(name, bindingAttr);
    if (property != (PropertyInfo) null)
    {
      MethodInfo getMethod = JSProperty.GetGetMethod(property, false);
      MethodInfo setMethod = JSProperty.GetSetMethod(property, false);
      if (getMethod != (MethodInfo) null && !getMethod.IsStatic || setMethod != (MethodInfo) null && !setMethod.IsStatic)
      {
        MethodInfo method = reflect.GetMethod(name, bindingAttr);
        if (method != (MethodInfo) null && !method.IsStatic)
          return new MemberInfo[1]
          {
            (MemberInfo) new JSWrappedPropertyAndMethod(property, method, namedItem)
          };
      }
    }
    MemberInfo[] member = reflect.GetMember(name, bindingAttr);
    return member != null && member.Length != 0 ? ScriptObject.WrapMembers(member, namedItem) : (MemberInfo[]) null;
  }

  public object GetDefaultThisObject()
  {
    return ((IActivationObject) this.GetParent()).GetDefaultThisObject();
  }

  public FieldInfo GetField(string name, int lexLevel)
  {
    throw new JScriptException(JSError.InternalError);
  }

  public GlobalScope GetGlobalScope() => ((IActivationObject) this.GetParent()).GetGlobalScope();

  FieldInfo IActivationObject.GetLocalField(string name) => (FieldInfo) null;

  [DebuggerStepThrough]
  [DebuggerHidden]
  public object GetMemberValue(string name, int lexlevel)
  {
    if (lexlevel <= 0)
      return (object) Missing.Value;
    object memberValue = LateBinding.GetMemberValue(this.namedItem, name);
    return !(memberValue is Missing) ? memberValue : ((IActivationObject) this.parent).GetMemberValue(name, lexlevel - 1);
  }

  public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
  {
    MemberInfo[] memberInfoArray = (MemberInfo[]) null;
    if (!this.recursive && this.reflectObj != null)
    {
      this.recursive = true;
      try
      {
        if (!this.reflectObj.GetType().IsCOMObject || !(this.engine.Site is ISite2 site))
          memberInfoArray = ScriptObject.WrapMembers(this.reflectObj.GetMember(name, bindingAttr), this.namedItem);
        else if ((memberInfoArray = VsaNamedItemScope.GetAndWrapMember(this.reflectObj, this.namedItem, name, bindingAttr)) == null)
        {
          object[] parentChain = site.GetParentChain((object) this.reflectObj);
          if (parentChain != null)
          {
            int length = parentChain.Length;
            for (int index = 0; index < length; ++index)
            {
              if (parentChain[index] is IReflect reflect)
              {
                if ((memberInfoArray = VsaNamedItemScope.GetAndWrapMember(reflect, (object) reflect, name, bindingAttr)) != null)
                  break;
              }
            }
          }
        }
      }
      finally
      {
        this.recursive = false;
      }
    }
    return memberInfoArray ?? new MemberInfo[0];
  }

  public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
  {
    MemberInfo[] members = (MemberInfo[]) null;
    if (!this.recursive)
    {
      this.recursive = true;
      try
      {
        members = this.reflectObj.GetMembers(bindingAttr);
        if (members != null)
        {
          if (members.Length != 0)
          {
            SimpleHashtable cache = this.namedItemWrappedMemberCache ?? (this.namedItemWrappedMemberCache = new SimpleHashtable(16U /*0x10*/));
            members = ScriptObject.WrapMembers(members, this.namedItem, cache);
          }
          else
            members = (MemberInfo[]) null;
        }
      }
      finally
      {
        this.recursive = false;
      }
    }
    return members;
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  internal override object GetMemberValue(string name)
  {
    object memberValue = (object) Missing.Value;
    if (!this.recursive)
    {
      this.recursive = true;
      try
      {
        FieldInfo field = this.reflectObj.GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
        if (field == (FieldInfo) null)
        {
          PropertyInfo property = this.reflectObj.GetProperty(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
          if (property != (PropertyInfo) null)
            memberValue = JSProperty.GetValue(property, this.namedItem, (object[]) null);
        }
        else
          memberValue = field.GetValue(this.namedItem);
        if (memberValue is Missing)
        {
          if (this.parent != null)
            memberValue = this.parent.GetMemberValue(name);
        }
      }
      finally
      {
        this.recursive = false;
      }
    }
    return memberValue;
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  internal override void SetMemberValue(string name, object value)
  {
    bool flag = false;
    if (this.recursive)
      return;
    this.recursive = true;
    try
    {
      FieldInfo field = this.reflectObj.GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
      if (field == (FieldInfo) null)
      {
        PropertyInfo property = this.reflectObj.GetProperty(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
        if (property != (PropertyInfo) null)
        {
          JSProperty.SetValue(property, this.namedItem, value, (object[]) null);
          flag = true;
        }
      }
      else
      {
        field.SetValue(this.namedItem, value);
        flag = true;
      }
      if (flag || this.parent == null)
        return;
      this.parent.SetMemberValue(name, value);
    }
    finally
    {
      this.recursive = false;
    }
  }
}
