// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.IAuthorServices
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines services for authoring JScript code in an editor.</summary>
[Guid("9E2B453C-6EAA-4329-A619-62E4889C8C8A")]
[ComVisible(true)]
public interface IAuthorServices
{
  /// <summary>When implemented in a class, gets an object that is used to colorize text when JScript code is being authored in an editor.</summary>
  /// <returns>An object that is used to colorize text.</returns>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  IColorizeText GetColorizer();

  /// <summary>When implemented in a class, gets an object that is used to add IntelliSense to text when JScript code is being authored in an editor.</summary>
  /// <returns>An object that is used to add IntelliSense to text.</returns>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  IParseText GetCodeSense();
}
