// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.SuperTypeMembersSorter
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Collections;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Stores members from base classes and interfaces.</summary>
public sealed class SuperTypeMembersSorter
{
  private SimpleHashtable members;
  private ArrayList names;
  private int count;

  internal SuperTypeMembersSorter()
  {
    this.members = new SimpleHashtable(64U /*0x40*/);
    this.names = new ArrayList();
    this.count = 0;
  }

  internal void Add(MemberInfo[] members)
  {
    foreach (MemberInfo member in members)
      this.Add(member);
  }

  internal void Add(MemberInfo member)
  {
    ++this.count;
    string name = member.Name;
    object member1 = this.members[(object) name];
    if (member1 == null)
    {
      this.members[(object) name] = (object) member;
      this.names.Add((object) name);
    }
    else if ((object) (member1 as MemberInfo) != null)
      this.members[(object) name] = (object) new ArrayList(8)
      {
        member1,
        (object) member
      };
    else
      ((ArrayList) member1).Add((object) member);
  }

  internal object[] GetMembers()
  {
    object[] members = new object[this.count];
    int num = 0;
    foreach (object name in this.names)
    {
      object member = this.members[name];
      if ((object) (member as MemberInfo) != null)
      {
        members[num++] = member;
      }
      else
      {
        foreach (object obj in (ArrayList) member)
          members[num++] = obj;
      }
    }
    return members;
  }
}
