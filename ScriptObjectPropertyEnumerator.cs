// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ScriptObjectPropertyEnumerator
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Collections;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal class ScriptObjectPropertyEnumerator : IEnumerator
{
  private ArrayList enumerators;
  private ArrayList objects;
  private int index;
  private SimpleHashtable visited_names;

  internal ScriptObjectPropertyEnumerator(ScriptObject obj)
  {
    obj.GetPropertyEnumerator(this.enumerators = new ArrayList(), this.objects = new ArrayList());
    this.index = 0;
    this.visited_names = new SimpleHashtable(16U /*0x10*/);
  }

  public virtual bool MoveNext()
  {
    if (this.index >= this.enumerators.Count)
      return false;
    IEnumerator enumerator = (IEnumerator) this.enumerators[this.index];
    if (!enumerator.MoveNext())
    {
      ++this.index;
      return this.MoveNext();
    }
    object current = enumerator.Current;
    FieldInfo fieldInfo = current as FieldInfo;
    string key;
    if (fieldInfo != (FieldInfo) null)
    {
      JSPrototypeField jsPrototypeField = current as JSPrototypeField;
      if ((FieldInfo) jsPrototypeField != (FieldInfo) null && jsPrototypeField.value is Missing)
        return this.MoveNext();
      key = fieldInfo.Name;
      if (fieldInfo.GetValue(this.objects[this.index]) is Missing)
        return this.MoveNext();
    }
    else
      key = !(current is string) ? ((object) (current as MemberInfo) == null ? current.ToString() : ((MemberInfo) current).Name) : (string) current;
    if (this.visited_names[(object) key] != null)
      return this.MoveNext();
    this.visited_names[(object) key] = (object) key;
    return true;
  }

  public virtual object Current
  {
    get
    {
      object current = ((IEnumerator) this.enumerators[this.index]).Current;
      return (object) (current as MemberInfo) != null ? (object) ((MemberInfo) current).Name : (object) current.ToString();
    }
  }

  public virtual void Reset()
  {
    this.index = 0;
    foreach (IEnumerator enumerator in this.enumerators)
      enumerator.Reset();
    this.visited_names = new SimpleHashtable(16U /*0x10*/);
  }
}
