// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Vsa.IJSVsaEngine
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Policy;

#nullable disable
namespace Microsoft.JScript.Vsa;

/// <summary>Defines the methods and properties that a script engine must support and provides programmatic access to the script engine.</summary>
[Guid("8FA2C97B-47E4-4A31-A7F5-FF39D1195CD9")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
[ComVisible(true)]
public interface IJSVsaEngine
{
  /// <summary>Sets or gets the host-implemented IJSVsaSite object that is used by the script engine to communicate with the host.</summary>
  /// <returns>The host-implemented object.</returns>
  IJSVsaSite Site { [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get; [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] set; }

  /// <summary>Sets or gets the display name of the script engine.</summary>
  /// <returns>The name of the engine.</returns>
  string Name { [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get; [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] set; }

  /// <summary>Sets or gets a script engine's root moniker.</summary>
  /// <returns>The root moniker of the engine.</returns>
  string RootMoniker { [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get; [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] set; }

  /// <summary>Sets or gets the root namespace used by the script engine.</summary>
  /// <returns>The namespace of the engine.</returns>
  string RootNamespace { [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get; [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] set; }

  /// <summary>Gets or sets the geographical locale and language in which to report exception messages.</summary>
  /// <returns>An integer value representing the locale in which exception messages are to be reported.</returns>
  int LCID { [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get; [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] set; }

  /// <summary>Sets or gets a Boolean value that signifies whether the script engine produces debug information when the Compile method is called.</summary>
  /// <returns>true to produce debug information; otherwise, false.</returns>
  bool GenerateDebugInfo { [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get; [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] set; }

  /// <summary>Provides evidence for the purpose of validating the current object's identity.</summary>
  /// <returns>Returns a reference to an Evidence object.</returns>
  Evidence Evidence { [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get; [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] set; }

  /// <summary>Gets the collection of IJSVsaItem objects, which represent all items added to the script engine.</summary>
  /// <returns>The collection of items.</returns>
  IJSVsaItems Items { [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get; }

  /// <summary>Gets or sets a value that indicates whether the current in-memory representation of the item differs from the persisted representation.</summary>
  /// <returns>true if the in-memory representation differs from the persisted representation; otherwise, false.</returns>
  bool IsDirty { [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get; }

  /// <summary>Gets the name of the programming language supported by the script engine.</summary>
  /// <returns>The name of the programming language.</returns>
  string Language { [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get; }

  /// <summary>Gets the current version of the language compiler supported by the script engine, in the form Major.Minor.Revision.Build.</summary>
  /// <returns>The current version.</returns>
  string Version { [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get; }

  /// <summary>Gets implementation-specific options for the engine.</summary>
  /// <returns>The value of the option.</returns>
  /// <param name="name">The name of the option to get.</param>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  object GetOption(string name);

  /// <summary>Sets implementation-specific options for a script engine.</summary>
  /// <param name="name">The name of the option to set.</param>
  /// <param name="value">The value for the option being set.</param>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  void SetOption(string name, object value);

  /// <summary>Causes the engine to compile the existing source state.</summary>
  /// <returns>true on successful compilation; otherwise, false.</returns>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  bool Compile();

  /// <summary>Initiates execution of compiled code in the script engine and binds all event handlers.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  void Run();

  /// <summary>Removes the script engine from the running state and disconnects automatically bound event handlers.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  void Reset();

  /// <summary>Closes the engine.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  void Close();

  /// <summary>Gets a Boolean value that reports whether the script engine is currently in run mode.</summary>
  /// <returns>true if the engine is in run mode; otherwise, false.</returns>
  bool IsRunning { [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get; }

  /// <summary>Gets or sets a value that indicates whether the source is compiled.</summary>
  /// <returns>true if the source is compiled; otherwise, false.</returns>
  bool IsCompiled { [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")] get; }

  /// <summary>Invalidates the cached assembly for the engine, as specified by its root moniker.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  void RevokeCache();

  /// <summary>Directs the script engine to persist its source state.</summary>
  /// <param name="site">The site to which source state is saved. </param>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  void SaveSourceState(IJSVsaPersistSite site);

  /// <summary>Directs the engine to load source items from their point of persistence, as specified by the host-provided object.</summary>
  /// <param name="site">The site from which the specified source items is loaded.</param>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  void LoadSourceState(IJSVsaPersistSite site);

  /// <summary>Saves the compiled state of the script engine; optionally, it also saves debugging information.</summary>
  /// <param name="pe">The compiled state of the script engine.</param>
  /// <param name="pdb">Specifies debugging information contained in the .PDB file corresponding to the PE (portable executable).</param>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  void SaveCompiledState(out byte[] pe, out byte[] pdb);

  /// <summary>Provides a notification that the initialization phase is complete.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  void InitNew();

  /// <summary>Checks whether the supplied identifier is valid for the engine.</summary>
  /// <returns>true if the identifier is valid; otherwise, it returns false.</returns>
  /// <param name="identifier">A string value provided as identifier.</param>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  bool IsValidIdentifier(string identifier);

  /// <summary>Gets a reference to the running assembly generated by Run method.</summary>
  /// <returns>Reference to the currently running assembly.</returns>
  Assembly Assembly { get; }
}
