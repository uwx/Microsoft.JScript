// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ActiveXObjectConstructor
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a prototype-based object that is defined by a constructor function. This class belongs to the built-in object model category.</summary>
public sealed class ActiveXObjectConstructor : ScriptFunction
{
  internal static readonly ActiveXObjectConstructor ob = new ActiveXObjectConstructor();

  internal ActiveXObjectConstructor()
    : base((ScriptObject) FunctionPrototype.ob, "ActiveXObject", 1)
  {
  }

  internal ActiveXObjectConstructor(LenientFunctionPrototype parent)
    : base((ScriptObject) parent, "ActiveXObject", 1)
  {
    this.noExpando = false;
  }

  internal override object Call(object[] args, object thisob) => (object) null;

  internal override object Construct(object[] args) => this.CreateInstance(args);

  /// <summary>Creates an instance of the prototype-based object, passing in the specified arguments.</summary>
  /// <returns>The instance of the object.</returns>
  /// <param name="args">The arguments to pass to the constructor.</param>
  [JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
  public new object CreateInstance(params object[] args)
  {
    string progID = args.Length != 0 && !(args[0].GetType() != typeof (string)) ? args[0].ToString() : throw new JScriptException(JSError.TypeMismatch);
    string server = (string) null;
    if (args.Length == 2)
      server = !(args[1].GetType() != typeof (string)) ? args[1].ToString() : throw new JScriptException(JSError.TypeMismatch);
    try
    {
      Type type = server != null ? Type.GetTypeFromProgID(progID, server) : Type.GetTypeFromProgID(progID);
      if (!type.IsPublic && type.Assembly == typeof (ActiveXObjectConstructor).Assembly)
        throw new JScriptException(JSError.CantCreateObject);
      return Activator.CreateInstance(type);
    }
    catch
    {
      throw new JScriptException(JSError.CantCreateObject);
    }
  }

  /// <summary>Returns null in all cases.</summary>
  /// <returns>null.</returns>
  public object Invoke() => (object) null;

  internal override bool HasInstance(object ob) => !(ob is JSObject);
}
