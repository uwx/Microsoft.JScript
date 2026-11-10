// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.LenientBooleanPrototype
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Stores objects that represent the static methods of the <see cref="T:Microsoft.JScript.BooleanPrototype" /> class. The <see cref="T:Microsoft.JScript.LenientBooleanPrototype" /> class belongs to the built-in object model category.</summary>
public sealed class LenientBooleanPrototype : BooleanPrototype
{
  /// <summary>Stores a reference to a <see cref="T:Microsoft.JScript.BooleanConstructor" /> object.</summary>
  public object constructor;
  /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.BooleanPrototype.toString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.BooleanPrototype" /> class.</summary>
  public object toString;
  /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.BooleanPrototype.valueOf(System.Object)" /> method of the <see cref="T:Microsoft.JScript.BooleanPrototype" /> class.</summary>
  public object valueOf;

  internal LenientBooleanPrototype(LenientFunctionPrototype funcprot, LenientObjectPrototype parent)
    : base((ObjectPrototype) parent, typeof (LenientBooleanPrototype))
  {
    this.noExpando = false;
    Type type = typeof (BooleanPrototype);
    this.toString = (object) new BuiltinFunction(nameof (toString), (object) this, type.GetMethod(nameof (toString)), (ScriptFunction) funcprot);
    this.valueOf = (object) new BuiltinFunction(nameof (valueOf), (object) this, type.GetMethod(nameof (valueOf)), (ScriptFunction) funcprot);
  }
}
