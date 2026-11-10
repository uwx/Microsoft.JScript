// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.LenientObjectPrototype
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Stores objects that represent the static methods of the <see cref="T:Microsoft.JScript.ObjectPrototype" /> class. The <see cref="T:Microsoft.JScript.LenientObjectPrototype" /> class belongs to the built-in object model category.</summary>
public class LenientObjectPrototype : ObjectPrototype
{
  /// <summary>Stores a reference to an <see cref="T:Microsoft.JScript.ObjectConstructor" /> object.</summary>
  public object constructor;
  /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.ObjectPrototype.hasOwnProperty(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.ObjectPrototype" /> class.</summary>
  public object hasOwnProperty;
  /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.ObjectPrototype.isPrototypeOf(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.ObjectPrototype" /> class.</summary>
  public object isPrototypeOf;
  /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.ObjectPrototype.propertyIsEnumerable(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.ObjectPrototype" /> class.</summary>
  public object propertyIsEnumerable;
  /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.ObjectPrototype.toLocaleString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.ObjectPrototype" /> class.</summary>
  public object toLocaleString;
  /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.ObjectPrototype.toString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.ObjectPrototype" /> class.</summary>
  public object toString;
  /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.ObjectPrototype.valueOf(System.Object)" /> method of the <see cref="T:Microsoft.JScript.ObjectPrototype" /> class.</summary>
  public object valueOf;

  internal LenientObjectPrototype(VsaEngine engine)
  {
    this.engine = engine;
    this.noExpando = false;
  }

  internal void Initialize(LenientFunctionPrototype funcprot)
  {
    Type type = typeof (ObjectPrototype);
    this.hasOwnProperty = (object) new BuiltinFunction("hasOwnProperty", (object) this, type.GetMethod("hasOwnProperty"), (ScriptFunction) funcprot);
    this.isPrototypeOf = (object) new BuiltinFunction("isPrototypeOf", (object) this, type.GetMethod("isPrototypeOf"), (ScriptFunction) funcprot);
    this.propertyIsEnumerable = (object) new BuiltinFunction("propertyIsEnumerable", (object) this, type.GetMethod("propertyIsEnumerable"), (ScriptFunction) funcprot);
    this.toLocaleString = (object) new BuiltinFunction("toLocaleString", (object) this, type.GetMethod("toLocaleString"), (ScriptFunction) funcprot);
    this.toString = (object) new BuiltinFunction("toString", (object) this, type.GetMethod("toString"), (ScriptFunction) funcprot);
    this.valueOf = (object) new BuiltinFunction("valueOf", (object) this, type.GetMethod("valueOf"), (ScriptFunction) funcprot);
  }
}
