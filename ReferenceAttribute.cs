// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ReferenceAttribute
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a reference to an assembly.</summary>
[AttributeUsage(AttributeTargets.Assembly)]
public class ReferenceAttribute : Attribute
{
  /// <summary>Specifies the reference to the assembly.</summary>
  public string reference;

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.ReferenceAttribute" /> class.</summary>
  /// <param name="reference">The reference to the assembly.</param>
  public ReferenceAttribute(string reference) => this.reference = reference;
}
