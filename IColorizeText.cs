// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.IColorizeText
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines services for colorizing text when JScript code is being authored in an editor.</summary>
[Guid("DB283E60-7ADB-4cf6-9758-2931893A12FC")]
[ComVisible(true)]
public interface IColorizeText
{
  /// <summary>When implemented in a class, scans the specified source code for individual units of code and identifies the color that each should have.</summary>
  /// <returns>An enumeration of <see cref="T:Microsoft.JScript.ITokenColorInfo" /> objects that represent units of code to colorize.</returns>
  /// <param name="sourceCode">The source code to colorize.</param>
  /// <param name="state">The current state (normal, comment, string) of the source code.</param>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  ITokenEnumerator Colorize(string sourceCode, SourceState state);

  /// <summary>When implemented in a class, scans the specified source code for individual units of code and identifies the state (normal, comment, string) of the specified code.</summary>
  /// <returns>The state of the source code.</returns>
  /// <param name="sourceCode">The source code to analyze.</param>
  /// <param name="currentState">The current state (normal, comment, string) of the source code.</param>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  SourceState GetStateForText(string sourceCode, SourceState currentState);
}
