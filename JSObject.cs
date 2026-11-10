// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices.Expando;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents an object in JScript. This class belongs to the objects and functions category.</summary>
public class JSObject : ScriptObject, IEnumerable, IExpando, IReflect
{
  private bool isASubClass;
  private IReflect subClassIR;
  private SimpleHashtable memberCache;
  internal bool noExpando;
  internal SimpleHashtable name_table;
  /// <summary>Stores a list of fields that are dynamically added and removed from the object.</summary>
  protected ArrayList field_table;
  internal JSObject outer_class_instance;

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.JSObject" /> class.</summary>
  public JSObject()
    : this((ScriptObject) null, false)
  {
    this.noExpando = false;
  }

  internal JSObject(ScriptObject parent)
    : this(parent, true)
  {
  }

  internal JSObject(ScriptObject parent, bool checkSubType)
    : base(parent)
  {
    this.memberCache = (SimpleHashtable) null;
    this.isASubClass = false;
    this.subClassIR = (IReflect) null;
    if (checkSubType)
    {
      Type referenceContext = Globals.TypeRefs.ToReferenceContext(this.GetType());
      if (referenceContext != Typeob.JSObject)
      {
        this.isASubClass = true;
        this.subClassIR = (IReflect) TypeReflector.GetTypeReflectorFor(referenceContext);
      }
    }
    this.noExpando = this.isASubClass;
    this.name_table = (SimpleHashtable) null;
    this.field_table = (ArrayList) null;
    this.outer_class_instance = (JSObject) null;
  }

  internal JSObject(ScriptObject parent, Type subType)
    : base(parent)
  {
    this.memberCache = (SimpleHashtable) null;
    this.isASubClass = false;
    this.subClassIR = (IReflect) null;
    subType = Globals.TypeRefs.ToReferenceContext(subType);
    if (subType != Typeob.JSObject)
    {
      this.isASubClass = true;
      this.subClassIR = (IReflect) TypeReflector.GetTypeReflectorFor(subType);
    }
    this.noExpando = this.isASubClass;
    this.name_table = (SimpleHashtable) null;
    this.field_table = (ArrayList) null;
  }

  /// <summary>Adds the specified field to the object.</summary>
  /// <returns>The new field, if it is successfully added.</returns>
  /// <param name="name">The name of the field to add.</param>
  public FieldInfo AddField(string name)
  {
    if (this.noExpando)
      return (FieldInfo) null;
    FieldInfo fieldInfo = (FieldInfo) this.NameTable[(object) name];
    if (fieldInfo == (FieldInfo) null)
    {
      fieldInfo = (FieldInfo) new JSExpandoField(name);
      this.name_table[(object) name] = (object) fieldInfo;
      this.field_table.Add((object) fieldInfo);
    }
    return fieldInfo;
  }

  /// <summary>Adds the specified method to the object. <see cref="M:Microsoft.JScript.JSObject.System#Runtime#InteropServices#Expando#IExpando#AddMethod(System.String,System.Delegate)" /> is not implemented in JScript.</summary>
  /// <returns>null in all cases.</returns>
  /// <param name="name">The name of the method to add.</param>
  /// <param name="method">The delegate to the method.</param>
  MethodInfo IExpando.AddMethod(string name, Delegate method) => (MethodInfo) null;

  /// <summary>Adds the specified property to the object. This method is not implemented in JScript.</summary>
  /// <returns>null in all cases.</returns>
  /// <param name="name">The name of the property to add.</param>
  PropertyInfo IExpando.AddProperty(string name) => (PropertyInfo) null;

  internal override bool DeleteMember(string name)
  {
    FieldInfo fieldInfo = (FieldInfo) this.NameTable[(object) name];
    if (fieldInfo != (FieldInfo) null)
    {
      switch (fieldInfo)
      {
        case JSExpandoField _:
          fieldInfo.SetValue((object) this, (object) Missing.Value);
          this.name_table.Remove((object) name);
          this.field_table.Remove((object) fieldInfo);
          return true;
        case JSPrototypeField _:
          fieldInfo.SetValue((object) this, (object) Missing.Value);
          return true;
        default:
          return false;
      }
    }
    else
      return this.parent != null && LateBinding.DeleteMember((object) this.parent, name);
  }

  internal virtual string GetClassName() => "Object";

  [DebuggerStepThrough]
  [DebuggerHidden]
  internal override object GetDefaultValue(PreferredType preferred_type)
  {
    switch (preferred_type)
    {
      case PreferredType.Either:
        if (this is DateObject)
          return this.GetDefaultValue(PreferredType.String);
        goto default;
      case PreferredType.String:
        if (this.GetMemberValue("toString") is ScriptFunction memberValue1)
        {
          object ob = memberValue1.Call(new object[0], (object) this);
          if (ob == null)
            return ob;
          IConvertible iconvertible = Convert.GetIConvertible(ob);
          if (iconvertible != null && iconvertible.GetTypeCode() != TypeCode.Object)
            return ob;
        }
        if (this.GetMemberValue("valueOf") is ScriptFunction memberValue2)
        {
          object ob = memberValue2.Call(new object[0], (object) this);
          if (ob == null)
            return ob;
          IConvertible iconvertible = Convert.GetIConvertible(ob);
          if (iconvertible != null && iconvertible.GetTypeCode() != TypeCode.Object)
            return ob;
          break;
        }
        break;
      case PreferredType.LocaleString:
        if (this.GetMemberValue("toLocaleString") is ScriptFunction memberValue3)
          return memberValue3.Call(new object[0], (object) this);
        break;
      default:
        if (this.GetMemberValue("valueOf") is ScriptFunction memberValue4)
        {
          object ob = memberValue4.Call(new object[0], (object) this);
          if (ob == null)
            return ob;
          IConvertible iconvertible = Convert.GetIConvertible(ob);
          if (iconvertible != null && iconvertible.GetTypeCode() != TypeCode.Object)
            return ob;
        }
        if (this.GetMemberValue("toString") is ScriptFunction memberValue5)
        {
          object ob = memberValue5.Call(new object[0], (object) this);
          if (ob == null)
            return ob;
          IConvertible iconvertible = Convert.GetIConvertible(ob);
          if (iconvertible != null && iconvertible.GetTypeCode() != TypeCode.Object)
            return ob;
          break;
        }
        break;
    }
    return (object) this;
  }

  /// <summary>Returns an enumerator that iterates through the members of the object.</summary>
  /// <returns>An enumerator object that is used to iterate through the members of the object.</returns>
  IEnumerator IEnumerable.GetEnumerator() => ForIn.JScriptGetEnumerator((object) this);

  private static bool IsHiddenMember(MemberInfo mem)
  {
    Type declaringType = mem.DeclaringType;
    return declaringType == Typeob.JSObject || declaringType == Typeob.ScriptObject || declaringType == Typeob.ArrayWrapper && mem.Name != "length";
  }

  private MemberInfo[] GetLocalMember(string name, BindingFlags bindingAttr, bool wrapMembers)
  {
    MemberInfo[] members = (MemberInfo[]) null;
    FieldInfo fieldInfo = this.name_table == null ? (FieldInfo) null : (FieldInfo) this.name_table[(object) name];
    if (fieldInfo == (FieldInfo) null && this.isASubClass)
    {
      if (this.memberCache != null)
      {
        MemberInfo[] localMember = (MemberInfo[]) this.memberCache[(object) name];
        if (localMember != null)
          return localMember;
      }
      bindingAttr &= ~BindingFlags.NonPublic;
      members = this.subClassIR.GetMember(name, bindingAttr);
      if (members.Length == 0)
        members = this.subClassIR.GetMember(name, bindingAttr & ~BindingFlags.Instance | BindingFlags.Static);
      int length = members.Length;
      if (length > 0)
      {
        int num1 = 0;
        foreach (MemberInfo mem in members)
        {
          if (JSObject.IsHiddenMember(mem))
            ++num1;
        }
        if (num1 > 0 && (length != 1 || !(this is ObjectPrototype) || !(name == "ToString")))
        {
          MemberInfo[] memberInfoArray = new MemberInfo[length - num1];
          int num2 = 0;
          foreach (MemberInfo mem in members)
          {
            if (!JSObject.IsHiddenMember(mem))
              memberInfoArray[num2++] = mem;
          }
          members = memberInfoArray;
        }
      }
      if ((members == null || members.Length == 0) && (bindingAttr & BindingFlags.Public) != BindingFlags.Default && (bindingAttr & BindingFlags.Instance) != BindingFlags.Default)
      {
        BindingFlags bindingAttr1 = bindingAttr & BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
        switch (this)
        {
          case StringObject _:
            members = TypeReflector.GetTypeReflectorFor(Typeob.String).GetMember(name, bindingAttr1);
            break;
          case NumberObject _:
            members = TypeReflector.GetTypeReflectorFor(((NumberObject) this).baseType).GetMember(name, bindingAttr1);
            break;
          case BooleanObject _:
            members = TypeReflector.GetTypeReflectorFor(Typeob.Boolean).GetMember(name, bindingAttr1);
            break;
          case StringConstructor _:
            members = TypeReflector.GetTypeReflectorFor(Typeob.String).GetMember(name, (bindingAttr1 | BindingFlags.Static) & ~BindingFlags.Instance);
            break;
          case BooleanConstructor _:
            members = TypeReflector.GetTypeReflectorFor(Typeob.Boolean).GetMember(name, (bindingAttr1 | BindingFlags.Static) & ~BindingFlags.Instance);
            break;
          case ArrayWrapper _:
            members = TypeReflector.GetTypeReflectorFor(Typeob.Array).GetMember(name, bindingAttr1);
            break;
        }
      }
      if (members != null && members.Length != 0)
      {
        if (wrapMembers)
          members = ScriptObject.WrapMembers(members, (object) this);
        if (this.memberCache == null)
          this.memberCache = new SimpleHashtable(32U /*0x20*/);
        this.memberCache[(object) name] = (object) members;
        return members;
      }
    }
    if ((bindingAttr & BindingFlags.IgnoreCase) != BindingFlags.Default && (members == null || members.Length == 0))
    {
      members = (MemberInfo[]) null;
      IDictionaryEnumerator enumerator = this.name_table.GetEnumerator();
      while (enumerator.MoveNext())
      {
        if (string.Compare(enumerator.Key.ToString(), name, StringComparison.OrdinalIgnoreCase) == 0)
        {
          fieldInfo = (FieldInfo) enumerator.Value;
          break;
        }
      }
    }
    if (fieldInfo != (FieldInfo) null)
      return new MemberInfo[1]{ (MemberInfo) fieldInfo };
    if (members == null)
      members = new MemberInfo[0];
    return members;
  }

  /// <summary>Gets an array of <see cref="T:System.Reflection.MemberInfo" /> objects that correspond to all members that match the specified name.</summary>
  /// <returns>An array of members that match the <paramref name="name" /> argument.</returns>
  /// <param name="name">The name of the member to search for.</param>
  /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
  public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
  {
    return this.GetMember(name, bindingAttr, false);
  }

  private MemberInfo[] GetMember(string name, BindingFlags bindingAttr, bool wrapMembers)
  {
    MemberInfo[] localMember = this.GetLocalMember(name, bindingAttr, wrapMembers);
    if (localMember.Length != 0)
      return localMember;
    if (this.parent == null)
      return new MemberInfo[0];
    MemberInfo[] member;
    if (this.parent is JSObject)
    {
      member = ((JSObject) this.parent).GetMember(name, bindingAttr, true);
      wrapMembers = false;
    }
    else
      member = this.parent.GetMember(name, bindingAttr);
    foreach (MemberInfo prototypeField in member)
    {
      if (prototypeField.MemberType == MemberTypes.Field)
      {
        FieldInfo fieldInfo = (FieldInfo) prototypeField;
        JSMemberField jsMemberField = prototypeField as JSMemberField;
        if ((FieldInfo) jsMemberField != (FieldInfo) null)
        {
          if (!jsMemberField.IsStatic)
          {
            JSGlobalField jsGlobalField = new JSGlobalField((ScriptObject) this, name, jsMemberField.value, FieldAttributes.Public);
            this.NameTable[(object) name] = (object) jsGlobalField;
            this.field_table.Add((object) jsGlobalField);
            fieldInfo = (FieldInfo) jsMemberField;
          }
        }
        else
        {
          fieldInfo = (FieldInfo) new JSPrototypeField((object) this.parent, (FieldInfo) prototypeField);
          if (!this.noExpando)
          {
            this.NameTable[(object) name] = (object) fieldInfo;
            this.field_table.Add((object) fieldInfo);
          }
        }
        return new MemberInfo[1]{ (MemberInfo) fieldInfo };
      }
      if (!this.noExpando && prototypeField.MemberType == MemberTypes.Method)
      {
        FieldInfo fieldInfo = (FieldInfo) new JSPrototypeField((object) this.parent, (FieldInfo) new JSGlobalField((ScriptObject) this, name, LateBinding.GetMemberValue((object) this.parent, name, (MemberInfo) null, member), FieldAttributes.Public | FieldAttributes.InitOnly));
        this.NameTable[(object) name] = (object) fieldInfo;
        this.field_table.Add((object) fieldInfo);
        return new MemberInfo[1]{ (MemberInfo) fieldInfo };
      }
    }
    return wrapMembers ? ScriptObject.WrapMembers(member, (object) this.parent) : member;
  }

  /// <summary>Gets an array of <see cref="T:System.Reflection.MemberInfo" /> objects that correspond to all members of the current script object.</summary>
  /// <returns>An array of objects that contains all members of the current object.</returns>
  /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
  public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
  {
    MemberInfoList memberInfoList = new MemberInfoList();
    SimpleHashtable simpleHashtable = new SimpleHashtable(32U /*0x20*/);
    if (!this.noExpando && this.field_table != null)
    {
      foreach (FieldInfo elem in this.field_table)
      {
        memberInfoList.Add((MemberInfo) elem);
        simpleHashtable[(object) elem.Name] = (object) elem;
      }
    }
    if (this.isASubClass)
    {
      MemberInfo[] members = this.GetType().GetMembers(bindingAttr & ~BindingFlags.NonPublic);
      int index = 0;
      for (int length = members.Length; index < length; ++index)
      {
        MemberInfo elem = members[index];
        if (!elem.DeclaringType.IsAssignableFrom(Typeob.JSObject) && simpleHashtable[(object) elem.Name] == null)
        {
          MethodInfo methodInfo = elem as MethodInfo;
          if (methodInfo == (MethodInfo) null || !methodInfo.IsSpecialName)
          {
            memberInfoList.Add(elem);
            simpleHashtable[(object) elem.Name] = (object) elem;
          }
        }
      }
    }
    if (this.parent != null)
    {
      SimpleHashtable cache = this.parent.wrappedMemberCache ?? (this.parent.wrappedMemberCache = new SimpleHashtable(8U));
      MemberInfo[] memberInfoArray = ScriptObject.WrapMembers(this.parent.GetMembers(bindingAttr & ~BindingFlags.NonPublic), (object) this.parent, cache);
      int index = 0;
      for (int length = memberInfoArray.Length; index < length; ++index)
      {
        MemberInfo elem = memberInfoArray[index];
        if (simpleHashtable[(object) elem.Name] == null)
          memberInfoList.Add(elem);
      }
    }
    return memberInfoList.ToArray();
  }

  internal override void GetPropertyEnumerator(ArrayList enums, ArrayList objects)
  {
    if (this.field_table == null)
      this.field_table = new ArrayList();
    enums.Add((object) new ListEnumerator(this.field_table));
    objects.Add((object) this);
    if (this.parent == null)
      return;
    this.parent.GetPropertyEnumerator(enums, objects);
  }

  internal override object GetValueAtIndex(uint index)
  {
    string str1 = System.Convert.ToString(index, (IFormatProvider) CultureInfo.InvariantCulture);
    FieldInfo fieldInfo = (FieldInfo) this.NameTable[(object) str1];
    if (fieldInfo != (FieldInfo) null)
      return fieldInfo.GetValue((object) this);
    object valueAtIndex = this.parent == null ? (object) Missing.Value : this.parent.GetMemberValue(str1);
    if (this is StringObject && valueAtIndex == Missing.Value)
    {
      string str2 = ((StringObject) this).value;
      if ((long) index < (long) str2.Length)
        return (object) str2[(int) index];
    }
    return valueAtIndex;
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  internal override object GetMemberValue(string name)
  {
    FieldInfo field = (FieldInfo) this.NameTable[(object) name];
    if (field == (FieldInfo) null && this.isASubClass)
    {
      field = this.subClassIR.GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
      if (field != (FieldInfo) null)
      {
        if (field.DeclaringType == Typeob.ScriptObject)
          return (object) Missing.Value;
      }
      else
      {
        PropertyInfo property = this.subClassIR.GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
        if (property != (PropertyInfo) null && !property.DeclaringType.IsAssignableFrom(Typeob.JSObject))
          return JSProperty.GetGetMethod(property, false).Invoke((object) this, BindingFlags.SuppressChangeType, (Binder) null, (object[]) null, (CultureInfo) null);
        try
        {
          MethodInfo method = this.subClassIR.GetMethod(name, BindingFlags.Static | BindingFlags.Public);
          if (method != (MethodInfo) null)
          {
            Type declaringType = method.DeclaringType;
            if (declaringType != Typeob.JSObject)
            {
              if (declaringType != Typeob.ScriptObject)
              {
                if (declaringType != Typeob.Object)
                  return (object) new BuiltinFunction((object) this, method);
              }
            }
          }
        }
        catch (AmbiguousMatchException ex)
        {
        }
      }
    }
    if (field != (FieldInfo) null)
      return field.GetValue((object) this);
    return this.parent != null ? this.parent.GetMemberValue(name) : (object) Missing.Value;
  }

  internal SimpleHashtable NameTable
  {
    get
    {
      SimpleHashtable nameTable = this.name_table;
      if (nameTable == null)
      {
        this.name_table = nameTable = new SimpleHashtable(16U /*0x10*/);
        this.field_table = new ArrayList();
      }
      return nameTable;
    }
  }

  /// <summary>Removes the specified member from the object.</summary>
  /// <param name="m">The name of the member to remove.</param>
  void IExpando.RemoveMember(MemberInfo m) => this.DeleteMember(m.Name);

  [DebuggerStepThrough]
  [DebuggerHidden]
  internal override void SetMemberValue(string name, object value)
  {
    this.SetMemberValue2(name, value);
  }

  /// <summary>Sets the value of a field that was dynamically added to the object and stored in <see cref="F:Microsoft.JScript.JSObject.field_table" />.</summary>
  /// <param name="name">The name of the field for which to set the value.</param>
  /// <param name="value">The new value of the field.</param>
  public void SetMemberValue2(string name, object value)
  {
    FieldInfo fieldInfo = (FieldInfo) this.NameTable[(object) name];
    if (fieldInfo == (FieldInfo) null && this.isASubClass)
      fieldInfo = this.GetType().GetField(name);
    if (fieldInfo == (FieldInfo) null)
    {
      if (this.noExpando)
        return;
      fieldInfo = (FieldInfo) new JSExpandoField(name);
      this.name_table[(object) name] = (object) fieldInfo;
      this.field_table.Add((object) fieldInfo);
    }
    if (fieldInfo.IsInitOnly || fieldInfo.IsLiteral)
      return;
    fieldInfo.SetValue((object) this, value);
  }

  internal override void SetValueAtIndex(uint index, object value)
  {
    this.SetMemberValue(System.Convert.ToString(index, (IFormatProvider) CultureInfo.InvariantCulture), value);
  }

  internal virtual void SwapValues(uint left, uint right)
  {
    string key1 = System.Convert.ToString(left, (IFormatProvider) CultureInfo.InvariantCulture);
    string key2 = System.Convert.ToString(right, (IFormatProvider) CultureInfo.InvariantCulture);
    FieldInfo fieldInfo1 = (FieldInfo) this.NameTable[(object) key1];
    FieldInfo fieldInfo2 = (FieldInfo) this.name_table[(object) key2];
    if (fieldInfo1 == (FieldInfo) null)
    {
      if (fieldInfo2 == (FieldInfo) null)
        return;
      this.name_table[(object) key1] = (object) fieldInfo2;
      this.name_table.Remove((object) key2);
    }
    else if (fieldInfo2 == (FieldInfo) null)
    {
      this.name_table[(object) key2] = (object) fieldInfo1;
      this.name_table.Remove((object) key1);
    }
    else
    {
      this.name_table[(object) key1] = (object) fieldInfo2;
      this.name_table[(object) key2] = (object) fieldInfo1;
    }
  }

  /// <summary>Returns a string that represents the current object.</summary>
  /// <returns>A string that represents the current object.</returns>
  public override string ToString() => Convert.ToString((object) this);
}
