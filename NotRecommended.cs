// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.NotRecommended
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Globalization;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Indicates that a method or field is not recommended.</summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field)]
public class NotRecommended : Attribute
{
  private string message;

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.NotRecommended" /> attribute class.</summary>
  /// <param name="message">The name of the method or field that is not recommended.</param>
  public NotRecommended(string message) => this.message = message;

  /// <summary>Gets a value that indicates whether using a method or field that is not recommended is an error.</summary>
  /// <returns>false in all cases.</returns>
  public bool IsError => false;

  /// <summary>Gets the name of the method or field that is not recommended.</summary>
  /// <returns>The name of the method or field that is not recommended.</returns>
  public string Message => JScriptException.Localize(this.message, (CultureInfo) null);
}
