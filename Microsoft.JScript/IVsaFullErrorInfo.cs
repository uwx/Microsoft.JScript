// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.IVsaFullErrorInfo
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines properties to obtain information about errors. This interface provides extra functionality that is needed by an IActiveScript host.</summary>
[Guid("DC3691BC-F188-4b67-8338-326671E0F3F6")]
[ComVisible(true)]
public interface IVsaFullErrorInfo : IJSVsaError
{
    /// <summary>When implemented in a class, gets the end line of the code in which an error occurred.</summary>
    /// <returns>The end line of the code in which an error occurred.</returns>
    int EndLine { get; }
}