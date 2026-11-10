// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ErrorType
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

/// <summary>Specifies the JScript error types.</summary>
public enum ErrorType
{
    /// <summary>Any error other than one of the other error types. This is the default value.</summary>
    OtherError,

    /// <summary>An eval Method (Visual Studio - JScript) error. Corresponds to the <see cref="T:Microsoft.JScript.EvalErrorObject" /> object.</summary>
    EvalError,

    /// <summary>A range error. Corresponds to the <see cref="T:Microsoft.JScript.RangeErrorObject" /> object.</summary>
    RangeError,

    /// <summary>A reference error. Corresponds to the <see cref="T:Microsoft.JScript.ReferenceErrorObject" /> object.</summary>
    ReferenceError,

    /// <summary>A syntax error. Corresponds to the <see cref="T:Microsoft.JScript.SyntaxErrorObject" /> object.</summary>
    SyntaxError,

    /// <summary>A type error. Corresponds to the <see cref="T:Microsoft.JScript.TypeErrorObject" /> object.</summary>
    TypeError,

    /// <summary>A Uniform Resource Identifier (URI) error. Corresponds to the <see cref="T:Microsoft.JScript.URIErrorObject" /> object.</summary>
    URIError,
}