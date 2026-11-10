// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Vsa.IJSVsaPersistSite
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript.Vsa;

/// <summary>Manages project persistence and stores and retrieves code and other items using save and load operations implemented by the host.</summary>
[Guid("F901A1FF-8EBA-4C38-B6E0-E7E52606D325")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
[ComVisible(true)]
public interface IJSVsaPersistSite
{
  /// <summary>Saves an arbitrary source string with a given name parameter, which can then be used in a call to the <see cref="M:Microsoft.JScript.Vsa.IJSVsaPersistSite.LoadElement(System.String)" /> method to reload the string.</summary>
  /// <param name="name">A name to associate with the specified source item. This can be a null reference when saving the Project file.</param>
  /// <param name="source">The source string for the item.</param>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  void SaveElement(string name, string source);

  /// <summary>Gets the source string previously saved using the <see cref="M:Microsoft.JScript.Vsa.IJSVsaPersistSite.SaveElement(System.String,System.String)" /> method.</summary>
  /// <returns>Returns the contents of the source-code element associated with the name parameter.</returns>
  /// <param name="name">The name of the code item to be loaded. This can be a null reference when loading the Project file.</param>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  string LoadElement(string name);
}
