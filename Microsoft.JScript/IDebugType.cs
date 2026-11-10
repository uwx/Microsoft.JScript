// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.IDebugType
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines methods that provide information about debugger objects. This interface is implemented by debugger objects that represent a type in the debugger target. </summary>
[Guid("613CC05D-05F4-4969-B369-5AEEF56E32D0")]
[ComVisible(true)]
public interface IDebugType
{
    /// <summary>When implemented in a class, determines whether the specified object is an instance of the current type.</summary>
    /// <returns>true if <paramref name="o" /> is an instance of the current type; otherwise, false.</returns>
    /// <param name="o">The object to test.</param>
    bool HasInstance([MarshalAs(UnmanagedType.Interface)] object o);
}