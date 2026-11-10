// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSPrototypeObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

/// <summary>Adds a constructor function property to a <see cref="T:Microsoft.JScript.JSObject" /> for early binding purposes. This class belongs to the objects and functions category.</summary>
public class JSPrototypeObject : JSObject
{
  /// <summary>Stores a constructor function object that defines a prototype-based object.</summary>
  public object constructor;

  internal JSPrototypeObject(ScriptObject parent, ScriptFunction constructor)
    : base(parent, typeof (JSPrototypeObject))
  {
    this.constructor = (object) constructor;
    this.noExpando = false;
  }
}
