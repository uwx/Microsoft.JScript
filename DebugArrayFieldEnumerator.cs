// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.DebugArrayFieldEnumerator
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Collections;

#nullable disable
namespace Microsoft.JScript;

internal class DebugArrayFieldEnumerator
{
  private ScriptObjectPropertyEnumerator enumerator;
  private int count;
  private ArrayObject arrayObject;

  internal DebugArrayFieldEnumerator(
    ScriptObjectPropertyEnumerator enumerator,
    ArrayObject arrayObject)
  {
    this.enumerator = enumerator;
    this.arrayObject = arrayObject;
    this.EnsureCount();
  }

  internal DynamicFieldInfo[] Next(int count)
  {
    try
    {
      ArrayList arrayList = new ArrayList();
      for (; count > 0 && this.enumerator.MoveNext(); --count)
      {
        string current = (string) this.enumerator.Current;
        arrayList.Add((object) new DynamicFieldInfo(current, this.arrayObject.GetMemberValue(current)));
      }
      DynamicFieldInfo[] dynamicFieldInfoArray = new DynamicFieldInfo[arrayList.Count];
      arrayList.CopyTo((Array) dynamicFieldInfoArray);
      return dynamicFieldInfoArray;
    }
    catch
    {
      return new DynamicFieldInfo[0];
    }
  }

  internal int GetCount() => this.count;

  internal void Skip(int count)
  {
    while (count > 0 && this.enumerator.MoveNext())
      --count;
  }

  internal void Reset() => this.enumerator.Reset();

  internal void EnsureCount()
  {
    this.enumerator.Reset();
    this.count = 0;
    while (this.enumerator.MoveNext())
      ++this.count;
    this.enumerator.Reset();
  }
}
