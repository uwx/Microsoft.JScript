// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.IDebugScriptScope
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines methods that set the this value that is associated with the global scope. Used by the <see cref="T:Microsoft.JScript.Vsa.VsaEngine" />.</summary>
[Guid("59447635-3E26-4873-BF26-05F173B80F5E")]
[ComVisible(true)]
public interface IDebugScriptScope
{
  /// <summary>When implemented in a class, sets the this value that is associated with the global scope.</summary>
  /// <param name="thisValue">The value that is used as the this object for the global scope.</param>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  void SetThisValue([MarshalAs(UnmanagedType.Interface)] object thisValue);
}
