// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Vsa.IJSVsaReferenceItem
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript.Vsa;

/// <summary>Describes a reference added to the script engine.</summary>
[Guid("8EFD265B-677A-4B09-A471-E086787AA727")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
[ComVisible(true)]
public interface IJSVsaReferenceItem : IJSVsaItem
{
  /// <summary>Gets or sets the name of the referenced assembly.</summary>
  /// <returns>Returns the string literal name of the referenced assembly.</returns>
  string AssemblyName { [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get; [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] set; }
}
