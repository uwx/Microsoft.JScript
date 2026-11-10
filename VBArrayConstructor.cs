// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.VBArrayConstructor
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a prototype-based object that is defined by a constructor function. This class belongs to the built-in object model category.</summary>
public sealed class VBArrayConstructor : ScriptFunction
{
  internal static readonly VBArrayConstructor ob = new VBArrayConstructor();
  private VBArrayPrototype originalPrototype;

  internal VBArrayConstructor()
    : base((ScriptObject) FunctionPrototype.ob, "VBArray", 1)
  {
    this.originalPrototype = VBArrayPrototype.ob;
    VBArrayPrototype._constructor = this;
    this.proto = (object) VBArrayPrototype.ob;
  }

  internal VBArrayConstructor(
    LenientFunctionPrototype parent,
    LenientVBArrayPrototype prototypeProp)
    : base((ScriptObject) parent, "VBArray", 1)
  {
    this.originalPrototype = (VBArrayPrototype) prototypeProp;
    prototypeProp.constructor = (object) this;
    this.proto = (object) prototypeProp;
    this.noExpando = false;
  }

  internal override object Call(object[] args, object thisob) => (object) null;

  internal override object Construct(object[] args) => this.CreateInstance(args);

  internal VBArrayObject Construct() => new VBArrayObject(this.originalPrototype, (Array) null);

  /// <summary>Creates an instance of the prototype-based object, passing in the specified arguments.</summary>
  /// <returns>An instance of the object.</returns>
  /// <param name="args">The arguments to pass to the constructor.</param>
  [JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
  public new object CreateInstance(params object[] args)
  {
    return args.Length >= 1 && typeof (Array).IsAssignableFrom(args[0].GetType()) ? (object) new VBArrayObject(this.originalPrototype, (Array) args[0]) : throw new JScriptException(JSError.VBArrayExpected);
  }
}
