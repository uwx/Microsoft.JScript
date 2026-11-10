// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.LenientRegExpPrototype
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Stores objects that represent the static methods of the <see cref="T:Microsoft.JScript.RegExpPrototype" /> class. The <see cref="T:Microsoft.JScript.LenientRegExpPrototype" /> class belongs to the built-in object model category.</summary>
public sealed class LenientRegExpPrototype : RegExpPrototype
{
  /// <summary>Stores a reference to a <see cref="T:Microsoft.JScript.RegExpConstructor" /> object.</summary>
  public object constructor;
  /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.RegExpPrototype.compile(System.Object,System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.RegExpPrototype" /> class.</summary>
  public object compile;
  /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.RegExpPrototype.exec(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.RegExpPrototype" /> class.</summary>
  public object exec;
  /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.RegExpPrototype.test(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.RegExpPrototype" /> class.</summary>
  public object test;
  /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.RegExpPrototype.toString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.RegExpPrototype" /> class.</summary>
  public object toString;

  internal LenientRegExpPrototype(LenientFunctionPrototype funcprot, LenientObjectPrototype parent)
    : base((ObjectPrototype) parent)
  {
    this.noExpando = false;
    Type type = typeof (RegExpPrototype);
    this.compile = (object) new BuiltinFunction(nameof (compile), (object) this, type.GetMethod(nameof (compile)), (ScriptFunction) funcprot);
    this.exec = (object) new BuiltinFunction(nameof (exec), (object) this, type.GetMethod(nameof (exec)), (ScriptFunction) funcprot);
    this.test = (object) new BuiltinFunction(nameof (test), (object) this, type.GetMethod(nameof (test)), (ScriptFunction) funcprot);
    this.toString = (object) new BuiltinFunction(nameof (toString), (object) this, type.GetMethod(nameof (toString)), (ScriptFunction) funcprot);
  }
}
