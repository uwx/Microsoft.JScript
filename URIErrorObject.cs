// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.URIErrorObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a Uniform Resource Identifier (URI) error. For example, a URI error can occur when you try to encode or decode a URI.</summary>
public sealed class URIErrorObject : ErrorObject
{
  internal URIErrorObject(ErrorPrototype parent, object[] args)
    : base(parent, args)
  {
  }

  internal URIErrorObject(ErrorPrototype parent, object e)
    : base(parent, e)
  {
  }
}
