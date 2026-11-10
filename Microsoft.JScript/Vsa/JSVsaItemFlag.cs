// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Vsa.JSVsaItemFlag
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript.Vsa;

/// <summary>Identifies the type of code item as Class, Module, or None.</summary>
[Guid("55BB0B6C-7629-4824-97FF-78A6928E92A7")]
[Obsolete(
    "Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
[ComVisible(true)]
public enum JSVsaItemFlag
{
    /// <summary>Used when the code item is a class.</summary>
    None,

    /// <summary>Used when the code item is a module.</summary>
    Module,

    /// <summary>Used when the code item is generic or when the item type does not accept flags. In such cases, no special flags are required to create the item.</summary>
    Class,
}