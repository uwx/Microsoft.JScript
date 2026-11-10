// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.MemberInfoList
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Maintains a list of <see cref="T:System.Reflection.MemberInfo" /> elements.</summary>
public sealed class MemberInfoList
{
  internal int count;
  private MemberInfo[] list;

  internal MemberInfoList()
  {
    this.count = 0;
    this.list = new MemberInfo[16 /*0x10*/];
  }

  internal void Add(MemberInfo elem)
  {
    int index = this.count++;
    if (this.list.Length == index)
      this.Grow();
    this.list[index] = elem;
  }

  internal void AddRange(MemberInfo[] elems)
  {
    foreach (MemberInfo elem in elems)
      this.Add(elem);
  }

  private void Grow()
  {
    MemberInfo[] list = this.list;
    int length = list.Length;
    MemberInfo[] memberInfoArray = this.list = new MemberInfo[length + 16 /*0x10*/];
    for (int index = 0; index < length; ++index)
      memberInfoArray[index] = list[index];
  }

  internal MemberInfo this[int i]
  {
    get => this.list[i];
    set => this.list[i] = value;
  }

  internal MemberInfo[] ToArray()
  {
    int count = this.count;
    MemberInfo[] array = new MemberInfo[count];
    MemberInfo[] list = this.list;
    for (int index = 0; index < count; ++index)
      array[index] = list[index];
    return array;
  }
}
