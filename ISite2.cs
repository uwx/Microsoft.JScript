// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ISite2
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines methods that are used by the <see cref="T:Microsoft.JScript.Vsa.VsaEngine" /> for name resolution. Provides extra functionality that is needed by an IActiveScript host. </summary>
[Guid("BFF6C980-0705-4394-88B8-A03A4B8B4CD7")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComVisible(true)]
public interface ISite2
{
  /// <summary>When implemented in a class, gets the parent object hierarchy for the specified object.</summary>
  /// <returns>An array that contains the parent objects of <paramref name="obj" />.</returns>
  /// <param name="obj">An <see cref="T:System.Reflection.IReflect" /> object to get the parent hierarchy of.</param>
  object[] GetParentChain(object obj);
}
