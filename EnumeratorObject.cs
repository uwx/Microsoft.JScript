// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.EnumeratorObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Collections;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents an enumerator object. This class belongs to the built-in object model category.</summary>
public class EnumeratorObject : JSObject
{
  private IEnumerable collection;
  /// <summary>Contains the enumerator that is represented by the <see cref="T:Microsoft.JScript.EnumeratorObject" /> class.</summary>
  protected IEnumerator enumerator;
  private object obj;

  internal EnumeratorObject(EnumeratorPrototype parent)
    : base((ScriptObject) parent)
  {
    this.enumerator = (IEnumerator) null;
    this.collection = (IEnumerable) null;
    this.noExpando = false;
  }

  internal EnumeratorObject(EnumeratorPrototype parent, IEnumerable collection)
    : base((ScriptObject) parent)
  {
    this.collection = collection;
    if (collection != null)
      this.enumerator = collection.GetEnumerator();
    this.LoadObject();
    this.noExpando = false;
  }

  internal virtual bool atEnd() => this.enumerator == null || this.obj == null;

  internal virtual object item() => this.enumerator != null ? this.obj : (object) null;

  /// <summary>Advances the enumerator to the next element of the collection and stores a reference to the current element of the collection.</summary>
  protected void LoadObject()
  {
    if (this.enumerator != null && this.enumerator.MoveNext())
      this.obj = this.enumerator.Current;
    else
      this.obj = (object) null;
  }

  internal virtual void moveFirst()
  {
    if (this.collection != null)
      this.enumerator = this.collection.GetEnumerator();
    this.LoadObject();
  }

  internal virtual void moveNext()
  {
    if (this.enumerator == null)
      return;
    this.LoadObject();
  }
}
