// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.LenientEnumeratorPrototype
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Stores objects that represent the static methods of the <see cref="T:Microsoft.JScript.EnumeratorPrototype" /> class. The <see cref="T:Microsoft.JScript.LenientEnumeratorPrototype" /> class belongs to the built-in object model category.</summary>
public sealed class LenientEnumeratorPrototype : EnumeratorPrototype
{
  /// <summary>Stores a reference to an <see cref="T:Microsoft.JScript.EnumeratorConstructor" /> object.</summary>
  public object constructor;
  /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.EnumeratorPrototype.atEnd(System.Object)" /> method of the <see cref="T:Microsoft.JScript.EnumeratorPrototype" /> class.</summary>
  public object atEnd;
  /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.EnumeratorPrototype.item(System.Object)" /> method of the <see cref="T:Microsoft.JScript.EnumeratorPrototype" /> class.</summary>
  public object item;
  /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.EnumeratorPrototype.moveFirst(System.Object)" /> method of the <see cref="T:Microsoft.JScript.EnumeratorPrototype" /> class.</summary>
  public object moveFirst;
  /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.EnumeratorPrototype.moveNext(System.Object)" /> method of the <see cref="T:Microsoft.JScript.EnumeratorPrototype" /> class.</summary>
  public object moveNext;

  internal LenientEnumeratorPrototype(
    LenientFunctionPrototype funcprot,
    LenientObjectPrototype parent)
    : base((ObjectPrototype) parent)
  {
    this.noExpando = false;
    Type type = typeof (EnumeratorPrototype);
    this.atEnd = (object) new BuiltinFunction(nameof (atEnd), (object) this, type.GetMethod(nameof (atEnd)), (ScriptFunction) funcprot);
    this.item = (object) new BuiltinFunction(nameof (item), (object) this, type.GetMethod(nameof (item)), (ScriptFunction) funcprot);
    this.moveFirst = (object) new BuiltinFunction(nameof (moveFirst), (object) this, type.GetMethod(nameof (moveFirst)), (ScriptFunction) funcprot);
    this.moveNext = (object) new BuiltinFunction(nameof (moveNext), (object) this, type.GetMethod(nameof (moveNext)), (ScriptFunction) funcprot);
  }
}
