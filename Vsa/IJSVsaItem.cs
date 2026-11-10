// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Vsa.IJSVsaItem
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript.Vsa;

/// <summary>Defines an interface for all items added to the .NET script engine, including code items, reference items, and global items. It defines generic properties and methods that apply to all item types recognized by the engine.</summary>
[Guid("1F2377AC-8A09-417B-89DC-D146769F0B45")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
[ComVisible(true)]
public interface IJSVsaItem
{
  /// <summary>Sets or gets the name of the item.</summary>
  /// <returns>Returns the string literal name of the item.</returns>
  string Name { [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get; [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] set; }

  /// <summary>Gets the specified object's type, as determined by the <see cref="M:Microsoft.JScript.Vsa.IJSVsaItems.CreateItem(System.String,Microsoft.JScript.Vsa.JSVsaItemType,Microsoft.JScript.Vsa.JSVsaItemFlag)" /> method.</summary>
  /// <returns>A type as enumerated by the <see cref="T:Microsoft.JScript.Vsa.JSVsaItemType" /> enumeration.</returns>
  JSVsaItemType ItemType { [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get; }

  /// <summary>Returns a value indicating whether the current in-memory representation of the item differs from the persisted representation.</summary>
  /// <returns>Returns true if the item is dirty, and thus requires saving; returns false if the item is not dirty.</returns>
  bool IsDirty { [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get; }

  /// <summary>Gets implementation-specific options for a script engine.</summary>
  /// <returns>Returns the value of the specified option.</returns>
  /// <param name="name">The name of the option to retrieve.</param>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  object GetOption(string name);

  /// <summary>Sets implementation-specific options for a script engine.</summary>
  /// <param name="name">The name of the option to set.</param>
  /// <param name="value">A new value for the option.</param>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  void SetOption(string name, object value);
}
