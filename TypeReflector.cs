// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.TypeReflector
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Collections;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Provides an implementation of <see cref="M:Microsoft.JScript.TypeReflector.GetMember(System.String,System.Reflection.BindingFlags)" /> that is faster than <see cref="M:System.Type.GetMember(System.String,System.Reflection.BindingFlags)" />.</summary>
public sealed class TypeReflector : ScriptObject
{
  private MemberInfo[] defaultMembers;
  private SimpleHashtable staticMembers;
  private SimpleHashtable instanceMembers;
  private MemberInfo[][] memberInfos;
  private ArrayList memberLookupTable;
  internal Type type;
  private object implementsIReflect;
  private object is__ComObject;
  internal uint hashCode;
  internal TypeReflector next;
  private static MemberInfo[] EmptyMembers = new MemberInfo[0];
  private static TRHashtable Table = new TRHashtable();

  internal TypeReflector(Type type)
    : base((ScriptObject) null)
  {
    this.defaultMembers = (MemberInfo[]) null;
    ArrayList arrayList = new ArrayList(512 /*0x0200*/);
    int length = 0;
    SimpleHashtable simpleHashtable1 = new SimpleHashtable(256U /*0x0100*/);
    foreach (MemberInfo member in type.GetMembers(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy))
    {
      string name = member.Name;
      object obj1 = simpleHashtable1[(object) name];
      if (obj1 == null)
      {
        simpleHashtable1[(object) name] = (object) length++;
        arrayList.Add((object) member);
      }
      else
      {
        int index = (int) obj1;
        object obj2 = arrayList[index];
        MemberInfo elem = obj2 as MemberInfo;
        if (elem != (MemberInfo) null)
        {
          MemberInfoList memberInfoList = new MemberInfoList();
          memberInfoList.Add(elem);
          memberInfoList.Add(member);
          arrayList[index] = (object) memberInfoList;
        }
        else
          ((MemberInfoList) obj2).Add(member);
      }
    }
    this.staticMembers = simpleHashtable1;
    SimpleHashtable simpleHashtable2 = new SimpleHashtable(256U /*0x0100*/);
    foreach (MemberInfo member in type.GetMembers(BindingFlags.Instance | BindingFlags.Public))
    {
      string name = member.Name;
      object obj3 = simpleHashtable2[(object) name];
      if (obj3 == null)
      {
        simpleHashtable2[(object) name] = (object) length++;
        arrayList.Add((object) member);
      }
      else
      {
        int index = (int) obj3;
        object obj4 = arrayList[index];
        MemberInfo elem = obj4 as MemberInfo;
        if (elem != (MemberInfo) null)
        {
          MemberInfoList memberInfoList = new MemberInfoList();
          memberInfoList.Add(elem);
          memberInfoList.Add(member);
          arrayList[index] = (object) memberInfoList;
        }
        else
          ((MemberInfoList) obj4).Add(member);
      }
    }
    this.instanceMembers = simpleHashtable2;
    this.memberLookupTable = arrayList;
    this.memberInfos = new MemberInfo[length][];
    this.type = type;
    this.implementsIReflect = (object) null;
    this.is__ComObject = (object) null;
    this.hashCode = (uint) type.GetHashCode();
    this.next = (TypeReflector) null;
  }

  internal MemberInfo[] GetDefaultMembers()
  {
    MemberInfo[] defaultMembers = this.defaultMembers;
    if (defaultMembers == null)
    {
      defaultMembers = JSBinder.GetDefaultMembers(this.type) ?? new MemberInfo[0];
      TypeReflector.WrapMembers(this.defaultMembers = defaultMembers);
    }
    return defaultMembers;
  }

  /// <summary>Searches for the specified member, using the specified binding constraints.</summary>
  /// <returns>An array of <see cref="T:System.Reflection.MemberInfo" /> objects that represents the public members that have the specified name, if found; otherwise, an empty array.</returns>
  /// <param name="name">The name of the member to search for.</param>
  /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify the way in which the search for members and types is conducted by the reflection operation.</param>
  public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
  {
    bool flag = (bindingAttr & BindingFlags.Instance) != 0;
    SimpleHashtable simpleHashtable = flag ? this.instanceMembers : this.staticMembers;
    object obj = simpleHashtable[(object) name];
    if (obj == null)
    {
      if ((bindingAttr & BindingFlags.IgnoreCase) != BindingFlags.Default)
        obj = simpleHashtable.IgnoreCaseGet(name);
      if (obj == null)
        return flag && (bindingAttr & BindingFlags.Static) != BindingFlags.Default ? this.GetMember(name, bindingAttr & ~BindingFlags.Instance) : TypeReflector.EmptyMembers;
    }
    int index = (int) obj;
    return this.memberInfos[index] ?? this.GetNewMemberArray(name, index);
  }

  private MemberInfo[] GetNewMemberArray(string name, int index)
  {
    object obj = this.memberLookupTable[index];
    if (obj == null)
      return this.memberInfos[index];
    MemberInfo memberInfo = obj as MemberInfo;
    MemberInfo[] members;
    if (memberInfo != (MemberInfo) null)
      members = new MemberInfo[1]{ memberInfo };
    else
      members = ((MemberInfoList) obj).ToArray();
    this.memberInfos[index] = members;
    this.memberLookupTable[index] = (object) null;
    TypeReflector.WrapMembers(members);
    return members;
  }

  /// <summary>Throws a <see cref="T:Microsoft.JScript.JScriptException" /> exception in all cases.</summary>
  /// <returns>Throws a <see cref="T:Microsoft.JScript.JScriptException" /> exception in all cases.</returns>
  /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify the way in which the search for members and types is conducted by the reflection operation.</param>
  public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
  {
    throw new JScriptException(JSError.InternalError);
  }

  internal static TypeReflector GetTypeReflectorFor(Type type)
  {
    TypeReflector typeReflectorFor1 = TypeReflector.Table[type];
    if (typeReflectorFor1 != null)
      return typeReflectorFor1;
    TypeReflector typeReflectorFor2 = new TypeReflector(type);
    lock (TypeReflector.Table)
    {
      TypeReflector typeReflectorFor3 = TypeReflector.Table[type];
      if (typeReflectorFor3 != null)
        return typeReflectorFor3;
      TypeReflector.Table[type] = typeReflectorFor2;
    }
    return typeReflectorFor2;
  }

  internal bool ImplementsIReflect()
  {
    object implementsIreflect = this.implementsIReflect;
    if (implementsIreflect != null)
      return (bool) implementsIreflect;
    bool flag = typeof (IReflect).IsAssignableFrom(this.type);
    this.implementsIReflect = (object) flag;
    return flag;
  }

  internal bool Is__ComObject()
  {
    object isComObject = this.is__ComObject;
    if (isComObject != null)
      return (bool) isComObject;
    bool flag = this.type.ToString() == "System.__ComObject";
    this.is__ComObject = (object) flag;
    return flag;
  }

  private static void WrapMembers(MemberInfo[] members)
  {
    int index = 0;
    for (int length = members.Length; index < length; ++index)
    {
      MemberInfo member = members[index];
      switch (member.MemberType)
      {
        case MemberTypes.Field:
          members[index] = (MemberInfo) new JSFieldInfo((FieldInfo) member);
          break;
        case MemberTypes.Method:
          members[index] = (MemberInfo) new JSMethodInfo((MethodInfo) member);
          break;
        case MemberTypes.Property:
          members[index] = (MemberInfo) new JSPropertyInfo((PropertyInfo) member);
          break;
      }
    }
  }
}
