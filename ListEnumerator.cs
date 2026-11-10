// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ListEnumerator
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Collections;

#nullable disable
namespace Microsoft.JScript;

internal class ListEnumerator : IEnumerator
{
  private int curr;
  private ArrayList list;

  internal ListEnumerator(ArrayList list)
  {
    this.curr = -1;
    this.list = list;
  }

  public virtual bool MoveNext() => ++this.curr < this.list.Count;

  public virtual object Current => this.list[this.curr];

  public virtual void Reset() => this.curr = -1;
}
