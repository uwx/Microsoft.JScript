// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ReferenceErrorObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a reference error. For example, a reference error can occur when you reference an undeclared variable.</summary>
public sealed class ReferenceErrorObject : ErrorObject
{
    internal ReferenceErrorObject(ErrorPrototype parent, object[] args)
        : base(parent, args)
    {
    }

    internal ReferenceErrorObject(ErrorPrototype parent, object e)
        : base(parent, e)
    {
    }
}