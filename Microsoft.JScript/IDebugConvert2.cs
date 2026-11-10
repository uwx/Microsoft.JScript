// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.IDebugConvert2
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines utility methods that convert data from one type to another. Includes methods that convert values to <see cref="T:System.String" /> objects so that they can be displayed by the debugger.</summary>
[Guid("B370D709-72BD-4696-9825-C4EBADBF98CB")]
[ComVisible(true)]
public interface IDebugConvert2
{
    /// <summary>When implemented in a class, converts the specified <see cref="T:System.Decimal" /> value to a <see cref="T:System.String" />.</summary>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    /// <param name="value">The value to convert.</param>
    string DecimalToString(Decimal value);
}