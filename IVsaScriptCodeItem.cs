// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.IVsaScriptCodeItem
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines extra functionality that is needed by an IActiveScript host. </summary>
[Guid("E0C0FFE8-7eea-4ee5-b7e4-0080c7eb0b74")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComVisible(true)]
public interface IVsaScriptCodeItem : IJSVsaCodeItem, IJSVsaItem
{
  /// <summary>When implemented in a class, gets or sets the line number where this code starts.</summary>
  /// <returns>The line number where this code starts.</returns>
  int StartLine { get; set; }

  /// <summary>When implemented in a class, gets or sets the column number in the line where this code starts.</summary>
  /// <returns>The column number where this code starts.</returns>
  int StartColumn { get; set; }

  /// <summary>When implemented in a class, compiles and runs this code.</summary>
  /// <returns>The return value from running the code, if there is any; otherwise, null.</returns>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  object Execute();
}
