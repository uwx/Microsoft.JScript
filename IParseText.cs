// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.IParseText
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines methods for parsing text when JScript code is being authored in an editor.</summary>
[Guid("C1468187-3DA1-49df-ADF8-5F8600E59EA8")]
[ComVisible(true)]
public interface IParseText
{
  /// <summary>When implemented in a class, parses the specified source code.</summary>
  /// <param name="code">The source code to parse.</param>
  /// <param name="error">An object that contains information about errors that occur when <paramref name="code" /> is being parsed.</param>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  void Parse(string code, IErrorHandler error);
}
