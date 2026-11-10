// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.LenientErrorPrototype
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

/// <summary>Stores objects that represent the static methods of the <see cref="T:Microsoft.JScript.ErrorPrototype" /> class. The <see cref="T:Microsoft.JScript.LenientErrorPrototype" /> class belongs to the built-in object model category.</summary>
public sealed class LenientErrorPrototype : ErrorPrototype
{
  /// <summary>Stores a reference to an <see cref="T:Microsoft.JScript.ErrorConstructor" /> object.</summary>
  public object constructor;
  /// <summary>Stores a string that contains the name of the subtype of the error.</summary>
  public object name;
  /// <summary>Stores an object that represents the static <see cref="M:Microsoft.JScript.ErrorPrototype.toString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.ErrorPrototype" /> class.</summary>
  public object toString;

  internal LenientErrorPrototype(
    LenientFunctionPrototype funcprot,
    ScriptObject parent,
    string name)
    : base(parent, name)
  {
    this.noExpando = false;
    this.name = (object) name;
    this.toString = (object) new BuiltinFunction(nameof (toString), (object) this, typeof (ErrorPrototype).GetMethod(nameof (toString)), (ScriptFunction) funcprot);
  }
}
