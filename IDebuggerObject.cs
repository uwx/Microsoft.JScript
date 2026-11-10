// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.IDebuggerObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines methods that provide information about debugger objects. This interface is implemented by all <see cref="T:System.Reflection.IReflect" /> objects that are implemented by the debugger. Currently, this interface serves only to flag these objects as such to the scripting engine.</summary>
[Guid("8E93D770-6168-4b68-B896-A71B74C7076A")]
[ComVisible(true)]
public interface IDebuggerObject
{
  /// <summary>When implemented in a class, determines whether the current object is a true COM object or a proxy to a managed object.</summary>
  /// <returns>true if the current object is a COM object; false if the current object is a proxy to a managed object.</returns>
  bool IsCOMObject();

  /// <summary>When implemented in a class, determines whether the current object is equal to the specified object.</summary>
  /// <returns>true if the current object is the same as <paramref name="o" />; otherwise, false.</returns>
  /// <param name="o">The object to test for equality.</param>
  bool IsEqual(IDebuggerObject o);

  /// <summary>When implemented in a class, determines whether the current object has a member that has the specified name and that is enumerable.</summary>
  /// <returns>true if the current object has an enumerable member named <paramref name="name" />; otherwise, false.</returns>
  /// <param name="name">The name of the member to search for.</param>
  bool HasEnumerableMember(string name);

  /// <summary>When implemented in a class, determines whether the current object is a proxy to a managed JScript function.</summary>
  /// <returns>true if the current object is a proxy to a managed JScript function; otherwise, false.</returns>
  bool IsScriptFunction();

  /// <summary>When implemented in a class, determines whether the current object is a proxy to a managed JScript object.</summary>
  /// <returns>true if the current object is a proxy to a managed JScript object; otherwise, false.</returns>
  bool IsScriptObject();
}
