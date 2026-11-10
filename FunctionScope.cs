// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.FunctionScope
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Collections;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal sealed class FunctionScope : ActivationObject
{
  internal bool isMethod;
  internal bool isStatic;
  internal bool mustSaveStackLocals;
  internal JSLocalField returnVar;
  internal FunctionObject owner;
  internal ArrayList nested_functions;
  private ArrayList fields_for_nested_functions;
  internal SimpleHashtable ProvidesOuterScopeLocals;
  internal bool closuresMightEscape;

  internal FunctionScope(ScriptObject parent)
    : this(parent, false)
  {
  }

  internal FunctionScope(ScriptObject parent, bool isMethod)
    : base(parent)
  {
    this.isKnownAtCompileTime = true;
    this.isMethod = isMethod;
    this.mustSaveStackLocals = false;
    if (parent != null && parent is ActivationObject)
      this.fast = ((ActivationObject) parent).fast;
    else
      this.fast = false;
    this.returnVar = (JSLocalField) null;
    this.owner = (FunctionObject) null;
    this.isStatic = false;
    this.nested_functions = (ArrayList) null;
    this.fields_for_nested_functions = (ArrayList) null;
    this.ProvidesOuterScopeLocals = !(parent is FunctionScope) ? (SimpleHashtable) null : new SimpleHashtable(16U /*0x10*/);
    this.closuresMightEscape = false;
  }

  internal JSVariableField AddNewField(
    string name,
    FieldAttributes attributeFlags,
    FunctionObject func)
  {
    if (this.nested_functions == null)
    {
      this.nested_functions = new ArrayList();
      this.fields_for_nested_functions = new ArrayList();
    }
    this.nested_functions.Add((object) func);
    JSVariableField jsVariableField = this.AddNewField(name, (object) func, attributeFlags);
    this.fields_for_nested_functions.Add((object) jsVariableField);
    return jsVariableField;
  }

  protected override JSVariableField CreateField(
    string name,
    FieldAttributes attributeFlags,
    object value)
  {
    return (attributeFlags & FieldAttributes.Static) != FieldAttributes.PrivateScope ? (JSVariableField) new JSGlobalField((ScriptObject) this, name, value, attributeFlags) : (JSVariableField) new JSLocalField(name, this, this.field_table.Count, value);
  }

  internal void AddOuterScopeField(string name, JSLocalField field)
  {
    this.name_table[(object) name] = (object) field;
    this.field_table.Add((object) field);
  }

  internal void AddReturnValueField()
  {
    if (this.name_table[(object) "return value"] != null)
      return;
    this.returnVar = new JSLocalField("return value", this, this.field_table.Count, (object) Missing.Value);
    this.name_table[(object) "return value"] = (object) this.returnVar;
    this.field_table.Add((object) this.returnVar);
  }

  internal void CloseNestedFunctions(StackFrame sf)
  {
    if (this.nested_functions == null)
      return;
    IEnumerator enumerator1 = this.nested_functions.GetEnumerator();
    IEnumerator enumerator2 = this.fields_for_nested_functions.GetEnumerator();
    while (enumerator1.MoveNext() && enumerator2.MoveNext())
    {
      FieldInfo current1 = (FieldInfo) enumerator2.Current;
      FunctionObject current2 = (FunctionObject) enumerator1.Current;
      current2.enclosing_scope = (ScriptObject) sf;
      current1.SetValue((object) sf, (object) new Closure(current2));
    }
  }

  internal BitArray DefinedFlags
  {
    get
    {
      int count = this.field_table.Count;
      BitArray definedFlags = new BitArray(count);
      for (int index = 0; index < count; ++index)
      {
        if (((JSLocalField) this.field_table[index]).isDefined)
          definedFlags[index] = true;
      }
      return definedFlags;
    }
    set
    {
      int count = value.Count;
      for (int index = 0; index < count; ++index)
        ((JSLocalField) this.field_table[index]).isDefined = value[index];
    }
  }

  internal JSLocalField[] GetLocalFields()
  {
    int count = this.field_table.Count;
    JSLocalField[] localFields = new JSLocalField[this.field_table.Count];
    for (int index = 0; index < count; ++index)
      localFields[index] = (JSLocalField) this.field_table[index];
    return localFields;
  }

  public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
  {
    FieldInfo fieldInfo = (FieldInfo) this.name_table[(object) name];
    if (fieldInfo != (FieldInfo) null)
      return new MemberInfo[1]{ (MemberInfo) fieldInfo };
    bool flag1 = false;
    ScriptObject parent;
    for (parent = this.parent; parent is FunctionScope; parent = parent.GetParent())
    {
      FunctionScope functionScope = (FunctionScope) parent;
      flag1 = functionScope.isMethod && !functionScope.isStatic;
      JSLocalField jsLocalField = (JSLocalField) functionScope.name_table[(object) name];
      if (!((FieldInfo) jsLocalField == (FieldInfo) null))
      {
        if (jsLocalField.IsLiteral && !(jsLocalField.value is FunctionObject))
          return new MemberInfo[1]
          {
            (MemberInfo) jsLocalField
          };
        JSLocalField field = new JSLocalField(jsLocalField.Name, this, this.field_table.Count, (object) Missing.Value);
        field.outerField = jsLocalField;
        field.debugOn = jsLocalField.debugOn;
        if (!field.debugOn && this.owner.funcContext.document.debugOn && functionScope.owner.funcContext.document.debugOn)
          field.debugOn = Array.IndexOf<string>(functionScope.owner.formal_parameters, jsLocalField.Name) >= 0;
        field.isDefined = jsLocalField.isDefined;
        field.debuggerName = "outer." + field.Name;
        if (jsLocalField.IsLiteral)
        {
          field.attributeFlags |= FieldAttributes.Literal;
          field.value = jsLocalField.value;
        }
        this.AddOuterScopeField(name, field);
        if (this.ProvidesOuterScopeLocals[(object) parent] == null)
          this.ProvidesOuterScopeLocals[(object) parent] = (object) parent;
        ((FunctionScope) parent).mustSaveStackLocals = true;
        return new MemberInfo[1]{ (MemberInfo) field };
      }
    }
    if (parent is ClassScope & flag1)
    {
      MemberInfo[] member = parent.GetMember(name, bindingAttr & ~BindingFlags.DeclaredOnly);
      int length = member.Length;
      bool flag2 = false;
      for (int index = 0; index < length; ++index)
      {
        MemberInfo memberInfo = member[index];
        switch (memberInfo.MemberType)
        {
          case MemberTypes.Field:
            FieldInfo field = (FieldInfo) memberInfo;
            if (field.IsLiteral)
            {
              JSMemberField jsMemberField = field as JSMemberField;
              if ((FieldInfo) jsMemberField != (FieldInfo) null && jsMemberField.value is ClassScope && !((ClassScope) jsMemberField.value).owner.IsStatic)
                flag2 = true;
            }
            if (!field.IsStatic && !field.IsLiteral)
            {
              member[index] = (MemberInfo) new JSClosureField(field);
              flag2 = true;
              break;
            }
            break;
          case MemberTypes.Method:
            MethodInfo method = (MethodInfo) memberInfo;
            if (!method.IsStatic)
            {
              member[index] = (MemberInfo) new JSClosureMethod(method);
              flag2 = true;
              break;
            }
            break;
          case MemberTypes.Property:
            PropertyInfo propertyInfo = (PropertyInfo) memberInfo;
            MethodInfo methodInfo1 = JSProperty.GetGetMethod(propertyInfo, (bindingAttr & BindingFlags.NonPublic) != 0);
            MethodInfo methodInfo2 = JSProperty.GetSetMethod(propertyInfo, (bindingAttr & BindingFlags.NonPublic) != 0);
            bool flag3 = false;
            if (methodInfo1 != (MethodInfo) null && !methodInfo1.IsStatic)
            {
              flag3 = true;
              methodInfo1 = (MethodInfo) new JSClosureMethod(methodInfo1);
            }
            if (methodInfo2 != (MethodInfo) null && !methodInfo2.IsStatic)
            {
              flag3 = true;
              methodInfo2 = (MethodInfo) new JSClosureMethod(methodInfo2);
            }
            if (flag3)
            {
              member[index] = (MemberInfo) new JSClosureProperty(propertyInfo, methodInfo1, methodInfo2);
              flag2 = true;
              break;
            }
            break;
        }
      }
      if (flag2)
        this.GiveOuterFunctionsTheBadNews();
      if (length > 0)
        return member;
    }
    return (bindingAttr & BindingFlags.DeclaredOnly) != BindingFlags.Default ? new MemberInfo[0] : parent.GetMember(name, bindingAttr);
  }

  internal override string GetName()
  {
    string str = (string) null;
    if (this.parent != null)
      str = ((ActivationObject) this.parent).GetName();
    return str != null ? $"{str}.{this.owner.name}" : this.owner.name;
  }

  internal int GetNextSlotNumber() => this.field_table.Count;

  internal JSLocalField GetOuterLocalField(string name)
  {
    FieldInfo fieldInfo = (FieldInfo) this.name_table[(object) name];
    return fieldInfo != (FieldInfo) null && fieldInfo is JSLocalField && (FieldInfo) ((JSLocalField) fieldInfo).outerField != (FieldInfo) null ? (JSLocalField) fieldInfo : (JSLocalField) null;
  }

  private void GiveOuterFunctionsTheBadNews()
  {
    FunctionScope parent = (FunctionScope) this.parent;
    parent.mustSaveStackLocals = true;
    while (!parent.isMethod)
    {
      parent = (FunctionScope) parent.GetParent();
      parent.mustSaveStackLocals = true;
    }
  }

  internal void HandleUnitializedVariables()
  {
    int index = 0;
    for (int count = this.field_table.Count; index < count; ++index)
    {
      JSLocalField jsLocalField = (JSLocalField) this.field_table[index];
      if (jsLocalField.isUsedBeforeDefinition)
        jsLocalField.SetInferredType((IReflect) Typeob.Object, (AST) null);
    }
  }

  internal override void SetMemberValue(string name, object value)
  {
    FieldInfo fieldInfo = (FieldInfo) this.name_table[(object) name];
    if (fieldInfo != (FieldInfo) null)
      fieldInfo.SetValue((object) this, value);
    else
      this.parent.SetMemberValue(name, value);
  }

  internal void SetMemberValue(string name, object value, StackFrame sf)
  {
    FieldInfo fieldInfo = (FieldInfo) this.name_table[(object) name];
    if (fieldInfo != (FieldInfo) null)
      fieldInfo.SetValue((object) sf, value);
    else
      this.parent.SetMemberValue(name, value);
  }
}
