// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Vsa.JSVsaItemType
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript.Vsa;

/// <summary>Represents the type of the item.</summary>
[Guid("97453226-34DD-4379-B658-005DF8F84EA1")]
[Obsolete(
    "Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
[ComVisible(true)]
public enum JSVsaItemType
{
    /// <summary>Used to add a reference to an external .NET assembly, which can then be referenced from code. This type is used to create an IJSVsaReferenceItem object</summary>
    Reference,

    /// <summary>Used to add a global object to the .NET script engine. This type is used to create an IJSVsaGlobalItem object.</summary>
    AppGlobal,

    /// <summary>Used to create a code item for storing source code for the .NET script engine.</summary>
    Code,
}