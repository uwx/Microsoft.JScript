// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.GlobalScope
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Expando;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents the global scope. </summary>
[ComVisible(true)]
public class GlobalScope : ActivationObject, IExpando, IReflect
{
  private ArrayList componentScopes;
  internal GlobalObject globalObject;
  private bool recursive;
  internal bool evilScript;
  internal object thisObject;
  internal bool isComponentScope;
  private TypeReflector globalObjectTR;
  private TypeReflector typeReflector;

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.GlobalScope" /> class.</summary>
  /// <param name="parent">A reference to the parent object.</param>
  /// <param name="engine">A reference to the scripting engine.</param>
  public GlobalScope(GlobalScope parent, VsaEngine engine)
    : this(parent, engine, parent != null)
  {
  }

  internal GlobalScope(GlobalScope parent, VsaEngine engine, bool isComponentScope)
    : base((ScriptObject) parent)
  {
    this.componentScopes = (ArrayList) null;
    this.recursive = false;
    this.isComponentScope = isComponentScope;
    if (parent == null)
    {
      this.globalObject = engine.Globals.globalObject;
      this.globalObjectTR = TypeReflector.GetTypeReflectorFor(Globals.TypeRefs.ToReferenceContext(this.globalObject.GetType()));
      this.fast = !(this.globalObject is LenientGlobalObject);
    }
    else
    {
      this.globalObject = (GlobalObject) null;
      this.globalObjectTR = (TypeReflector) null;
      this.fast = parent.fast;
      if (isComponentScope)
        ((GlobalScope) this.parent).AddComponentScope(this);
    }
    this.engine = engine;
    this.isKnownAtCompileTime = this.fast;
    this.evilScript = true;
    this.thisObject = (object) this;
    this.typeReflector = TypeReflector.GetTypeReflectorFor(Globals.TypeRefs.ToReferenceContext(this.GetType()));
    if (!isComponentScope)
      return;
    engine.Scopes.Add((object) this);
  }

  internal void AddComponentScope(GlobalScope component)
  {
    if (this.componentScopes == null)
      this.componentScopes = new ArrayList();
    this.componentScopes.Add((object) component);
    component.thisObject = this.thisObject;
  }

  /// <summary>Adds the specified field to the global scope.</summary>
  /// <returns>The new field, if it is successfully added.</returns>
  /// <param name="name">The name of the field to add.</param>
  public FieldInfo AddField(string name)
  {
    if (this.fast)
      return (FieldInfo) null;
    if (this.isComponentScope)
      return ((GlobalScope) this.parent).AddField(name);
    FieldInfo fieldInfo = (FieldInfo) this.name_table[(object) name];
    if (fieldInfo == (FieldInfo) null)
    {
      fieldInfo = (FieldInfo) new JSExpandoField(name);
      this.name_table[(object) name] = (object) fieldInfo;
      this.field_table.Add((object) fieldInfo);
    }
    return fieldInfo;
  }

  /// <summary>Adds the specified method to the global scope. This method is not implemented in JScript.</summary>
  /// <returns>null in all cases.</returns>
  /// <param name="name">The name of the method to add.</param>
  /// <param name="method">The delegate to the method.</param>
  MethodInfo IExpando.AddMethod(string name, Delegate method) => (MethodInfo) null;

  internal override JSVariableField AddNewField(
    string name,
    object value,
    FieldAttributes attributeFlags)
  {
    return !this.isComponentScope ? base.AddNewField(name, value, attributeFlags) : ((ActivationObject) this.parent).AddNewField(name, value, attributeFlags);
  }

  /// <summary>Adds the specified property to the global scope. This method is not implemented in JScript.</summary>
  /// <returns>null in all cases.</returns>
  /// <param name="name">The name of the property to add.</param>
  PropertyInfo IExpando.AddProperty(string name) => (PropertyInfo) null;

  internal override bool DeleteMember(string name)
  {
    if (this.isComponentScope)
      return this.parent.DeleteMember(name);
    FieldInfo fieldInfo = (FieldInfo) this.name_table[(object) name];
    if (!(fieldInfo != (FieldInfo) null) || !(fieldInfo is JSExpandoField))
      return false;
    fieldInfo.SetValue((object) this, (object) Missing.Value);
    this.name_table.Remove((object) name);
    this.field_table.Remove((object) fieldInfo);
    return true;
  }

  /// <summary>Gets the current default scope, which is either the contained object of a with statement scope, or the global scope.</summary>
  /// <returns>This object.</returns>
  public override object GetDefaultThisObject() => (object) this;

  internal override object GetDefaultValue(PreferredType preferred_type)
  {
    return preferred_type == PreferredType.String || preferred_type == PreferredType.LocaleString ? (object) "" : (object) double.NaN;
  }

  /// <summary>Gets a field that has the specified name by searching the scope hierarchy. This method searches only the scope that is specified by the lexical level.</summary>
  /// <returns>A field that has the specified name.</returns>
  /// <param name="name">The name of the field to search for.</param>
  /// <param name="lexLevel">The lexical level to search.</param>
  public override FieldInfo GetField(string name, int lexLevel)
  {
    return this.GetField(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
  }

  internal JSField[] GetFields()
  {
    int count = this.field_table.Count;
    JSField[] fields = new JSField[count];
    for (int index = 0; index < count; ++index)
      fields[index] = (JSField) this.field_table[index];
    return fields;
  }

  /// <summary>Gets an array of <see cref="T:System.Reflection.FieldInfo" /> objects that correspond to all fields of the global scope.</summary>
  /// <returns>An array of objects that contains all fields of the global scope.</returns>
  /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
  public override FieldInfo[] GetFields(BindingFlags bindingAttr)
  {
    return base.GetFields(bindingAttr | BindingFlags.DeclaredOnly);
  }

  /// <summary>Gets the global scope by returning a reference to this object.</summary>
  /// <returns>This object.</returns>
  public override GlobalScope GetGlobalScope() => this;

  /// <summary>Gets the local declaration of a field that has the specified name. This is used at compile time for error checking.</summary>
  /// <returns>The local declaration of the field.</returns>
  /// <param name="name">The name of the field.</param>
  public override FieldInfo GetLocalField(string name)
  {
    return this.GetField(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
  }

  /// <summary>Gets an array of <see cref="T:System.Reflection.MemberInfo" /> objects that correspond to all members that match a specified name.</summary>
  /// <returns>An array of objects that match the <paramref name="name" /> argument.</returns>
  /// <param name="name">The name of the member to search for.</param>
  /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
  public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
  {
    return this.GetMember(name, bindingAttr, false);
  }

  private MemberInfo[] GetMember(string name, BindingFlags bindingAttr, bool calledFromParent)
  {
    if (this.recursive)
      return new MemberInfo[0];
    MemberInfo[] members = (MemberInfo[]) null;
    if (!this.isComponentScope)
    {
      MemberInfo[] member = base.GetMember(name, bindingAttr | BindingFlags.DeclaredOnly);
      if (member.Length != 0)
        return member;
      if (this.componentScopes != null)
      {
        int index = 0;
        for (int count = this.componentScopes.Count; index < count; ++index)
        {
          members = ((GlobalScope) this.componentScopes[index]).GetMember(name, bindingAttr | BindingFlags.DeclaredOnly, true);
          if (members.Length != 0)
            return members;
        }
      }
      if (this.globalObject != null)
        members = this.globalObjectTR.GetMember(name, bindingAttr & ~BindingFlags.NonPublic | BindingFlags.Static);
      if (members != null && members.Length != 0)
        return ScriptObject.WrapMembers(members, (object) this.globalObject);
    }
    else
    {
      members = this.typeReflector.GetMember(name, bindingAttr & ~BindingFlags.NonPublic | BindingFlags.Static);
      int length = members.Length;
      if (length > 0)
      {
        int num1 = 0;
        MemberInfo[] memberInfoArray = new MemberInfo[length];
        for (int index = 0; index < length; ++index)
        {
          MemberInfo memberInfo = memberInfoArray[index] = members[index];
          if (memberInfo.DeclaringType.IsAssignableFrom(Typeob.GlobalScope))
          {
            memberInfoArray[index] = (MemberInfo) null;
            ++num1;
          }
          else if ((object) (memberInfo as FieldInfo) != null)
          {
            FieldInfo fieldInfo = (FieldInfo) memberInfo;
            if (fieldInfo.IsStatic && fieldInfo.FieldType == Typeob.Type)
            {
              Type type = (Type) fieldInfo.GetValue((object) null);
              if (type != (Type) null)
                memberInfoArray[index] = (MemberInfo) type;
            }
          }
        }
        if (num1 == 0)
          return members;
        if (num1 == length)
          return new MemberInfo[0];
        MemberInfo[] member = new MemberInfo[length - num1];
        int num2 = 0;
        foreach (MemberInfo memberInfo in memberInfoArray)
        {
          if (memberInfo != (MemberInfo) null)
            member[num2++] = memberInfo;
        }
        return member;
      }
    }
    if (this.parent != null && !calledFromParent && ((bindingAttr & BindingFlags.DeclaredOnly) == BindingFlags.Default || this.isComponentScope))
    {
      this.recursive = true;
      try
      {
        members = this.parent.GetMember(name, bindingAttr);
      }
      finally
      {
        this.recursive = false;
      }
      if (members != null && members.Length != 0)
        return members;
    }
    return new MemberInfo[0];
  }

  /// <summary>Gets an array of <see cref="T:System.Reflection.MemberInfo" /> objects that correspond to all members of the global scope.</summary>
  /// <returns>An array of objects that contains all members of the global scope.</returns>
  /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
  public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
  {
    if (this.recursive)
      return new MemberInfo[0];
    MemberInfoList memberInfoList = new MemberInfoList();
    if (this.isComponentScope)
    {
      MemberInfo[] members = Globals.TypeRefs.ToReferenceContext(this.GetType()).GetMembers(bindingAttr | BindingFlags.DeclaredOnly);
      if (members != null)
      {
        foreach (MemberInfo elem in members)
          memberInfoList.Add(elem);
      }
    }
    else
    {
      if (this.componentScopes != null)
      {
        int index = 0;
        for (int count = this.componentScopes.Count; index < count; ++index)
        {
          GlobalScope componentScope = (GlobalScope) this.componentScopes[index];
          this.recursive = true;
          MemberInfo[] memberInfoArray = (MemberInfo[]) null;
          try
          {
            memberInfoArray = componentScope.GetMembers(bindingAttr);
          }
          finally
          {
            this.recursive = false;
          }
          if (memberInfoArray != null)
          {
            foreach (MemberInfo elem in memberInfoArray)
              memberInfoList.Add(elem);
          }
        }
      }
      foreach (FieldInfo elem in this.field_table)
        memberInfoList.Add((MemberInfo) elem);
    }
    if (this.parent != null && (this.isComponentScope || (bindingAttr & BindingFlags.DeclaredOnly) == BindingFlags.Default))
    {
      this.recursive = true;
      MemberInfo[] memberInfoArray = (MemberInfo[]) null;
      try
      {
        memberInfoArray = this.parent.GetMembers(bindingAttr);
      }
      finally
      {
        this.recursive = false;
      }
      if (memberInfoArray != null)
      {
        foreach (MemberInfo elem in memberInfoArray)
          memberInfoList.Add(elem);
      }
    }
    return memberInfoList.ToArray();
  }

  /// <summary>Gets an array of <see cref="T:System.Reflection.MethodInfo" /> objects that correspond to all methods of the global scope.</summary>
  /// <returns>An array of objects that contains all methods of the global scope.</returns>
  /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
  public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
  {
    return base.GetMethods(bindingAttr | BindingFlags.DeclaredOnly);
  }

  /// <summary>Gets an array of <see cref="T:System.Reflection.PropertyInfo" /> objects that correspond to all properties of the global scope.</summary>
  /// <returns>An array of objects that contains all properties of the global scope.</returns>
  /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
  public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
  {
    return base.GetProperties(bindingAttr | BindingFlags.DeclaredOnly);
  }

  internal override void GetPropertyEnumerator(ArrayList enums, ArrayList objects)
  {
    FieldInfo[] fields = this.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
    if (fields.Length != 0)
    {
      enums.Add((object) fields.GetEnumerator());
      objects.Add((object) this);
    }
    this.GetParent()?.GetPropertyEnumerator(enums, objects);
  }

  internal void SetFast()
  {
    this.fast = true;
    this.isKnownAtCompileTime = true;
    if (this.globalObject == null)
      return;
    this.globalObject = GlobalObject.commonInstance;
    this.globalObjectTR = TypeReflector.GetTypeReflectorFor(Globals.TypeRefs.ToReferenceContext(this.globalObject.GetType()));
  }

  /// <summary>Removes the specified member from the global scope.</summary>
  /// <param name="m">The name of the member to remove.</param>
  void IExpando.RemoveMember(MemberInfo m) => this.DeleteMember(m.Name);

  internal override void SetMemberValue(string name, object value)
  {
    MemberInfo[] member1 = this.GetMember(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
    if (member1.Length == 0)
    {
      if (VsaEngine.executeForJSEE)
        throw new JScriptException(JSError.UndefinedIdentifier, new Context(new DocumentContext("", (VsaEngine) null), name));
      FieldInfo fieldInfo = this.AddField(name);
      if (!(fieldInfo != (FieldInfo) null))
        return;
      fieldInfo.SetValue((object) this, value);
    }
    else
    {
      MemberInfo member2 = LateBinding.SelectMember(member1);
      if (member2 == (MemberInfo) null)
        throw new JScriptException(JSError.AssignmentToReadOnly);
      LateBinding.SetMemberValue((object) this, name, value, member2, member1);
    }
  }
}
