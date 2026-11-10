// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.TRHashtable
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

internal sealed class TRHashtable
{
  private TypeReflector[] table;
  private int count;
  private int threshold;

  internal TRHashtable()
  {
    this.table = new TypeReflector[511 /*0x01FF*/];
    this.count = 0;
    this.threshold = 256 /*0x0100*/;
  }

  internal TypeReflector this[Type type]
  {
    get
    {
      for (TypeReflector next = this.table[(int) ((uint) type.GetHashCode() % (uint) this.table.Length)]; next != null; next = next.next)
      {
        if (next.type == type)
          return next;
      }
      return (TypeReflector) null;
    }
    set
    {
      if (++this.count >= this.threshold)
        this.Rehash();
      int index = (int) (value.hashCode % (uint) this.table.Length);
      value.next = this.table[index];
      this.table[index] = value;
    }
  }

  private void Rehash()
  {
    TypeReflector[] table = this.table;
    int num = this.threshold = table.Length + 1;
    int length = num * 2 - 1;
    TypeReflector[] typeReflectorArray = this.table = new TypeReflector[length];
    int index1 = num - 1;
    while (index1-- > 0)
    {
      TypeReflector next = table[index1];
      while (next != null)
      {
        TypeReflector typeReflector = next;
        next = next.next;
        int index2 = (int) (typeReflector.hashCode % (uint) length);
        typeReflector.next = typeReflectorArray[index2];
        typeReflectorArray[index2] = typeReflector;
      }
    }
  }
}
