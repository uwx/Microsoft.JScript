// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ITokenEnumerator
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines methods for iterating over a collection of <see cref="T:Microsoft.JScript.ITokenColorInfo" /> objects.</summary>
[Guid("556BA9E0-BD6A-4837-89F0-C79B14759181")]
[ComVisible(true)]
public interface ITokenEnumerator
{
  /// <summary>When implemented in a class, returns the next item in a collection of <see cref="T:Microsoft.JScript.ITokenColorInfo" /> objects.</summary>
  /// <returns>The next item in the collection.</returns>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  ITokenColorInfo GetNext();

  /// <summary>When implemented in a class, sets the enumerator to its initial position before the first item in a collection of <see cref="T:Microsoft.JScript.ITokenColorInfo" /> objects.</summary>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  void Reset();
}
