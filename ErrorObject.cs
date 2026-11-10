// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ErrorObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents an error object. This class belongs to the built-in object model category.</summary>
public class ErrorObject : JSObject
{
  /// <summary>Specifies the message that explains the reason for the error.</summary>
  public object message;
  /// <summary>Specifies a number or code for the error.</summary>
  public object number;
  /// <summary>Specifies the message that explains the reason for the error.</summary>
  public object description;
  internal object exception;

  internal ErrorObject(ErrorPrototype parent, object[] args)
    : base((ScriptObject) parent)
  {
    this.exception = (object) null;
    this.description = (object) "";
    this.number = (object) 0;
    if (args.Length == 1)
    {
      if (args[0] == null || Convert.IsPrimitiveNumericType((IReflect) args[0].GetType()))
        this.number = (object) Convert.ToNumber(args[0]);
      else
        this.description = (object) Convert.ToString(args[0]);
    }
    else if (args.Length > 1)
    {
      this.number = (object) Convert.ToNumber(args[0]);
      this.description = (object) Convert.ToString(args[1]);
    }
    this.message = this.description;
    this.noExpando = false;
  }

  internal ErrorObject(ErrorPrototype parent, object e)
    : base((ScriptObject) parent)
  {
    this.exception = e;
    this.number = (object) -2146823266;
    if (e is Exception)
    {
      if (e is JScriptException)
        this.number = (object) ((JScriptException) e).Number;
      else if (e is ExternalException)
        this.number = (object) ((ExternalException) e).ErrorCode;
      this.description = (object) ((Exception) e).Message;
      if (((string) this.description).Length == 0)
        this.description = (object) e.GetType().FullName;
    }
    this.message = this.description;
    this.noExpando = false;
  }

  internal override string GetClassName() => "Error";

  internal string Message => Convert.ToString(this.message);

  /// <summary>Converts an <see cref="T:Microsoft.JScript.ErrorObject" /> to an <see cref="T:System.Exception" />, explicitly.</summary>
  /// <returns>The converted <see cref="T:Microsoft.JScript.ErrorObject" />.</returns>
  /// <param name="err">The object to convert.</param>
  public static explicit operator Exception(ErrorObject err) => err.exception as Exception;

  /// <summary>Converts an <see cref="T:Microsoft.JScript.ErrorObject" /> to an <see cref="T:System.Exception" />, implicitly.</summary>
  /// <returns>The converted <see cref="T:Microsoft.JScript.ErrorObject" />.</returns>
  /// <param name="err">The object to convert.</param>
  public static Exception ToException(ErrorObject err) => (Exception) err;
}
